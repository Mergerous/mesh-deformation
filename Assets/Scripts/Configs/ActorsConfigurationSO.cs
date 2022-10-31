using UnityEngine;

namespace Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/Actors Configuration SO")]
    public class ActorsConfigurationSO : ScriptableObject
    {
        [Range(0, 4)] [SerializeField] private int _count = 2;

        [SerializeField] private float _speed = 5;
        [SerializeField] private LayerMask _layerMask;

        public LayerMask LayerMask => _layerMask;
        public int Count => _count;
        public float Speed => _speed;
    }
}