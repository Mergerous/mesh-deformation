using UnityEngine;

namespace Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/Brush Configuration SO")]

    public class BrushConfigurationSO : ScriptableObject
    {
        [SerializeField] private float _brushSize;
        [SerializeField] private float _brushStrenght;
        [SerializeField] private Vector2 _smoothness = new Vector2(0.1f, 0.04f);

        public float BrushSize => _brushSize;
        public float BrushStrength => _brushStrenght;
        public Vector2 Smoothness => _smoothness;
    }
}