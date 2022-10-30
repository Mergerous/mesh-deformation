using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Scripts.Behaviours
{
    public class SurfaceRenderer : MonoBehaviour
    {
        public struct VertexData
        {
            public Vector4 pos;
            public Vector4 uv;
        }

        [SerializeField] private ComputeShader _computeShader;
        [SerializeField] private MeshFilter _meshFilter;


        private ComputeBuffer _vertexBuffer;
        private RenderTexture tex;
        private RenderTexture tex2;

        private Vector2Int _dispatchCountHeightMap;
        private Material _heightMapDebug;
        private VertexData[] meshVertData;

        private int dispatchCount = 0;
        private int _textureResolution = 512;

        private int _kernel;
        private int _kernelHeightMapTemp;
        private int _kernelHeightMapPersistent;
        private int _kernelHeightMapClear;

        private const int BufferStride = 8 * 4;

        #region Unity

        private void Start()
        {
            _kernel = _computeShader.FindKernel("CSMain");
            _kernelHeightMapTemp = _computeShader.FindKernel("CSMainHeightMapTemp");
            _kernelHeightMapPersistent = _computeShader.FindKernel("CSMainHeightMapPersistent");
            _kernelHeightMapClear = _computeShader.FindKernel("CSMainHeightMapClear");

            var mesh = _meshFilter.mesh;
            meshVertData = new VertexData[mesh.vertexCount];

            for (int i = 0; i < mesh.vertexCount; i++)
            {
                meshVertData[i].pos = mesh.vertices[i];
                meshVertData[i].uv = mesh.uv[i];
            }

            tex = new RenderTexture(_textureResolution, _textureResolution, 0, GraphicsFormat.R8_UNorm);
            tex.enableRandomWrite = true;
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.Create();
            _heightMapDebug.mainTexture = tex;

            tex2 = new RenderTexture(_textureResolution, _textureResolution, 0, GraphicsFormat.R8_UNorm);
            tex2.enableRandomWrite = true;
            tex2.wrapMode = TextureWrapMode.Clamp;
            tex2.Create();
            
            uint threadX = 0;
            uint threadY = 0;
            uint threadZ = 0;
            
            _computeShader.GetKernelThreadGroupSizes(_kernel, out threadX, out threadY, out threadZ);
            dispatchCount = Mathf.CeilToInt(meshVertData.Length / threadX) + 1;

            _dispatchCountHeightMap = Vector2Int.one;
            _dispatchCountHeightMap.x = Mathf.CeilToInt(_textureResolution / threadX) + 1;
            _dispatchCountHeightMap.y = Mathf.CeilToInt(_textureResolution / threadY) + 1;

            _vertexBuffer = new ComputeBuffer(mesh.vertexCount, BufferStride);
            _vertexBuffer.SetData(meshVertData);

            _computeShader.SetInt("_texResolution", _textureResolution);
            _computeShader.SetTexture(_kernelHeightMapTemp, "heightMapTemp", tex); // read & write
            _computeShader.SetTexture(_kernelHeightMapClear, "heightMapTemp", tex); // read & write
            _computeShader.SetTexture(_kernelHeightMapClear, "heightMapPersistent", tex2); // read & write
            _computeShader.SetTexture(_kernelHeightMapPersistent, "heightMapPersistent", tex2); // read & write
            _computeShader.SetTexture(_kernel, "heightMapTexTemp", tex); // readonly
            _computeShader.SetTexture(_kernel, "heightMapTexPersistent", tex2); // readonly
            _computeShader.SetBuffer(_kernel, "vertexBuffer", _vertexBuffer); // read & w

            DispatchClear();
            RedrawMesh();
        }

        #endregion

        #region Public

        private void OnDestroy()
        {
            _vertexBuffer.Dispose();
        }
        
        public void DispatchTemp()
        {
            _computeShader.Dispatch(_kernelHeightMapTemp, _dispatchCountHeightMap.x, _dispatchCountHeightMap.y, 1);
        }

        public void DispatchPersistent()
        {
            _computeShader.Dispatch(_kernelHeightMapPersistent, _dispatchCountHeightMap.x, _dispatchCountHeightMap.y, 1);
        }
        
        public void DispatchClear()
        {
            _computeShader.Dispatch(_kernelHeightMapClear, _dispatchCountHeightMap.x, _dispatchCountHeightMap.y, 1);
        }


        public void SetTextureResolution(int resolution) => _textureResolution = resolution;
        public void SetDebugMaterial(Material material) => _heightMapDebug = material;
        public void SetPosition(Vector3 position) => _computeShader.SetVector("_MousePos", position);
        public void SetSize(float size) => _computeShader.SetFloat("_Radius", size);
        public void SetStrength(float strength) => _computeShader.SetFloat("_Power", strength);
        public void SetSmoothness(float smooth) => _computeShader.SetFloat("_Smooth", smooth);

        
        public void RedrawMesh()
        {
            _computeShader.Dispatch(_kernel, dispatchCount, 1, 1);
            _vertexBuffer.GetData(meshVertData);
            var mesh = _meshFilter.mesh;
            mesh.MarkDynamic();
            mesh.SetVertices(meshVertData.Select(v => (Vector3)v.pos).ToArray());
            mesh.RecalculateNormals();
        }

        #endregion
    }
}
