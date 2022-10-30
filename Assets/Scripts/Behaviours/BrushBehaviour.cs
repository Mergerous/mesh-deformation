using System;
using UnityEngine;

namespace Scripts.Behaviours
{
    public class BrushBehaviour : MonoBehaviour
    { 
        private Camera _camera;
        private event Action<RaycastHit> _hitCallback;
        private event Action<RaycastHit> _mouseUpCallback;

        #region Unity

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    _hitCallback?.Invoke(hit);

                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                RaycastHit hit;
                if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    _mouseUpCallback?.Invoke(hit);
                }
            }
        }

        #endregion

        #region Public

        public void SetMouseUpCallback(Action<RaycastHit> callback)
        {
            _mouseUpCallback = callback;
        }

        public void SetHitCallback(Action<RaycastHit> callback)
        {
            _hitCallback = callback;
        }

        #endregion
    }
}