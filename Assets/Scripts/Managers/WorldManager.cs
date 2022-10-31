using Scripts.Behaviours;
using Scripts.Configs;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Managers
{
    public class WorldManager : MonoBehaviour
    {
        [Header("Options")]
        [SerializeField] private SurfaceConfigurationSO _surfaceConfigurationSo;

        [Header("Components")] 
        [SerializeField] private SurfaceRenderer _surfaceRenderer;
        #region Unity

        private void Awake()
        {
            _surfaceRenderer.SetDebugMaterial(_surfaceConfigurationSo.DebugMaterial);
            _surfaceRenderer.SetTextureResolution(_surfaceConfigurationSo.TextureResolution);
        }

        #endregion

        #region Public

        public void ClearSurfaces()
        {
            _surfaceRenderer.DispatchClear();
            _surfaceRenderer.RedrawMesh();
        }

        #endregion
    }
}