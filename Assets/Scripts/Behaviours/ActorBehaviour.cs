using System;
using UnityEngine;
using UnityEngine.AI;

namespace Scripts.Behaviours
{
    public class ActorBehaviour : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;

        private Transform _currentTarget;
        private event Func<Transform> _targetRequest;

        private void Start()
        {
            _currentTarget = _targetRequest.Invoke();
            _agent.SetDestination(_currentTarget.position);
        }

        private void Update()
        {

            if (_agent.remainingDistance <= 0.5f)
            {
                _currentTarget = _targetRequest.Invoke();
                _agent.SetDestination(_currentTarget.position);

            }
        }

        public void SetSpeed(float speed)
        {
            _agent.speed = speed;
        }

        public void SetTargetRequest(Func<Transform> request)
        {
            _targetRequest = request;
        }
    }
}
