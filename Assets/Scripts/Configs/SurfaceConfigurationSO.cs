using UnityEngine;

namespace Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/Surface Configuration SO")]
    public class SurfaceConfigurationSO : ScriptableObject
    {
        [SerializeField] private Material _debugMaterial;
        [SerializeField] private int _textureResolution = 512;

        public Material DebugMaterial => _debugMaterial;
        public int TextureResolution => _textureResolution;
    }
}