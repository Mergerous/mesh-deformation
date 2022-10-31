using System;
using UnityEngine;

namespace Scripts.Behaviours
{
    public class ActorBehaviour : MonoBehaviour
    {
        private LayerMask _layerMask;
        private Transform _currentTarget;
        private float _speed;
        private event Func<Transform> _targetRequest;

        #region Unity

        private void Start()
        {
            _currentTarget = _targetRequest.Invoke();
        }

        private void Update()
        {
            FindTarget();
            RaycastSurface();
            MoveToTarget();
        }

        #endregion

        #region Public
        public void SetLayerMask(LayerMask mask) => _layerMask = mask;
        public void SetSpeed(float speed) => _speed = speed;
        public void SetTargetRequest(Func<Transform> request) => _targetRequest = request;

        #endregion

        #region Private

        private void RaycastSurface()
        {
            if( Physics.Raycast(transform.position + Vector3.up * 100, Vector3.down, out var hit, 200, _layerMask))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + transform.localScale.y, transform.position.z);
            }
        }

        private void MoveToTarget()
        {
            transform.position = Vector3.MoveTowards(transform.position, _currentTarget.position, Time.deltaTime * _speed);
        }

        private void FindTarget()
        {
            if (Math.Abs(transform.position.x - _currentTarget.position.x) < 0.5f && 
                Math.Abs(transform.position.z - _currentTarget.position.z) < 0.5f)
            {
                _currentTarget = _targetRequest?.Invoke();
            }
        }

        #endregion
    }
}
