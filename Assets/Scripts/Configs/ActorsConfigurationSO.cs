using UnityEngine;

namespace Scripts.Configs
{
    [CreateAssetMenu(menuName = "Configs/Actors Configuration SO")]
    public class ActorsConfigurationSO : ScriptableObject
    {
        [Range(0, 4)] [SerializeField] private int _count = 2;

        [SerializeField] private float _speed = 5;

        public int Count => _count;
        public float Speed => _speed;
    }
}