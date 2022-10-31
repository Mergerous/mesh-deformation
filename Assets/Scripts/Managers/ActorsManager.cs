using System.Collections.Generic;
using Scripts.Behaviours;
using Scripts.Configs;
using Scripts.Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Managers
{
    public class ActorsManager : MonoBehaviour
    {
        [Header("Options")] [SerializeField] private ActorsConfigurationSO _actorsConfigurationSo;

        [Header("Components")] [SerializeField]
        private ActorBehaviour[] _actors;

        [SerializeField] private Transform[] _points;

        private readonly List<ActorController> _controllers = new List<ActorController>();
        private int _unitCount = 0;

        #region Unity

        private void Awake()
        {
            for (int i = 0; i < _actors.Length; i++)
            {
                var controller = new ActorController(_actors[i], _actorsConfigurationSo);
                controller.SetTargetRequest(OnTargetRequested);
                _controllers.Add(controller);
            }
            for (int i = 0; i < _actorsConfigurationSo.Count; i++)
            {
                if (i < _controllers.Count)
                {
                    _controllers[i].SetEnabled(true);
                }
            }
            _unitCount = _actorsConfigurationSo.Count;
        }

        #endregion

        #region Public

        public void Add()
        {

            if (_unitCount < _controllers.Count)
            {
                _controllers[_unitCount++].SetEnabled(true);
            }
        }

        public void Remove()
        {
            if (_unitCount > 0)
            {
                _controllers[--_unitCount].SetEnabled(false);
            }
        }

        #endregion

        #region Private

        private Transform OnTargetRequested()
        {
            return _points[Random.Range(0, _points.Length)];
        }

        #endregion
    }

}