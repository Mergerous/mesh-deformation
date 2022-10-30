using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class SurfaceRenderer : MonoBehaviour
{
    public struct VertexData
    {
        public Vector4 pos;
        public Vector4 uv;
    }

    [SerializeField] private ComputeShader _computeShader;
    [SerializeField] private MeshFilter _meshFilter;
    private int _textureResolution;
    
    private ComputeBuffer _vertexBuffer;
    private RenderTexture tex;
    private RenderTexture tex2;
    private int _kernelHeightMap;
    private int _kernelHeightMapSave;
    private Vector2Int _dispatchCountHeightMap;
    private Material _heightMapDebug;
    private VertexData[] meshVertData;
    private int _kernel; 
    private int dispatchCount = 0;

    private void Start()
    {
        //Compute Shader kernel
        _kernel = _computeShader.FindKernel ("CSMain");
        _kernelHeightMap = _computeShader.FindKernel ("CSMainHeightMap");
        _kernelHeightMapSave = _computeShader.FindKernel ("CSMainHeightMapSave");

        var mesh = _meshFilter.mesh;
        meshVertData = new VertexData[mesh.vertexCount];
        
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            meshVertData[i].pos = mesh.vertices[i];
            meshVertData[i].uv = mesh.uv[i];
        }
        
        tex = new RenderTexture (_textureResolution, _textureResolution, 0, GraphicsFormat.R8_UNorm);
        tex.enableRandomWrite = true;
        tex.wrapMode = TextureWrapMode.Clamp;
        tex.Create ();
        _heightMapDebug.mainTexture = tex;
        
        tex2 = new RenderTexture (_textureResolution, _textureResolution, 0, GraphicsFormat.R8_UNorm);
        tex2.enableRandomWrite = true;
        tex2.wrapMode = TextureWrapMode.Clamp;
        tex2.Create ();
        
        //Dispatch counts
        uint threadX = 0;
        uint threadY = 0;
        uint threadZ = 0;
        //kernel
        _computeShader.GetKernelThreadGroupSizes(_kernel, out threadX, out threadY, out threadZ);
        dispatchCount = Mathf.CeilToInt(meshVertData.Length / threadX)+1;
        
        _dispatchCountHeightMap = Vector2Int.one;
        _dispatchCountHeightMap.x = Mathf.CeilToInt(_textureResolution / threadX) + 1;
        _dispatchCountHeightMap.y = Mathf.CeilToInt(_textureResolution / threadY) + 1;
        
        _vertexBuffer = new ComputeBuffer(mesh.vertexCount, 8*4);
        _vertexBuffer.SetData(meshVertData);
        
        _computeShader.SetInt("_texResolution",_textureResolution);
        _computeShader.SetTexture(_kernelHeightMap, "heightMap", tex); // read & write
        _computeShader.SetTexture(_kernelHeightMapSave, "heightMap2", tex2); // read & write
        _computeShader.SetTexture(_kernel, "heightMapTex", tex); // readonly
        _computeShader.SetTexture(_kernel, "heightMapTex2", tex2); // readonly
        _computeShader.SetBuffer(_kernel, "vertexBuffer", _vertexBuffer); // read & w
    }

    public void Dispatch()
    {

        _computeShader.Dispatch (_kernelHeightMap, _dispatchCountHeightMap.x , _dispatchCountHeightMap.y, 1);
        _computeShader.Dispatch (_kernel, dispatchCount , 1, 1);
        
        _vertexBuffer.GetData(meshVertData);
        var mesh = _meshFilter.mesh;
        mesh.MarkDynamic();
        mesh.SetVertices(meshVertData.Select(v => (Vector3)v.pos).ToArray());
        mesh.RecalculateNormals();

    }

    public void SetTextureResolution(int resolution) => _textureResolution = resolution;

    public void SetDebugMaterial(Material material) => _heightMapDebug = material;
    public void SetPosition(Vector3 position) => _computeShader.SetVector("_MousePos", position);
    public void SetSize(float size) => _computeShader.SetFloat("_Radius", size);
    public void SetStrength(float strength) => _computeShader.SetFloat("_Power", strength);
    public void SetSmoothness(Vector2 smooth) => _computeShader.SetVector("_Smooth", smooth);
    
    public void Clear()
    {
        tex.Release();
        tex2.Release();
        tex.Create();
        tex2.Create();
        Rebuild();
        Dispatch();
    }
    public void Rebuild()
    {
        _computeShader.Dispatch (_kernelHeightMapSave, _dispatchCountHeightMap.x , _dispatchCountHeightMap.y, 1);
        _computeShader.SetVector("_MousePos", Vector4.positiveInfinity);
        _computeShader.Dispatch (_kernelHeightMap, _dispatchCountHeightMap.x , _dispatchCountHeightMap.y, 1);
    }
}
