using UnityEngine;

namespace Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/Brush Configuration SO")]

    public class BrushConfigurationSO : ScriptableObject
    {
        [SerializeField] private float _brushSize;
        [SerializeField] private float _brushStrength;
        [SerializeField] private float _smoothness = 4f;

        public float BrushSize => _brushSize;
        public float BrushStrength => _brushStrength;
        public float Smoothness => _smoothness;
    }
}