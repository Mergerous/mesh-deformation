using Scripts.Configs;
using UnityEngine;

namespace Scripts.Managers
{
    public class WorldManager : MonoBehaviour
    {
        [Header("Options")]
        [SerializeField] private SurfaceConfigurationSO _surfaceConfigurationSo;

        [Header("Components")] [SerializeField]
        private SurfaceRenderer _surfaceRenderer;

        private void Awake()
        {
            _surfaceRenderer.SetDebugMaterial(_surfaceConfigurationSo.DebugMaterial);
            _surfaceRenderer.SetTextureResolution(_surfaceConfigurationSo.TextureResolution);
        }

        public void ClearSurfaces()
        {
            _surfaceRenderer.Clear();
        }
    }
}