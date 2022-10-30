using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Controllers
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private Button _addUnitButton;
        [SerializeField] private Button _removeUnitButton;
        [SerializeField] private Button _clearSurfaceButton;

        private event Action _unitAddCallback;
        private event Action _unitRemoveCallback;
        private event Action _clearCallback;

        #region Unity

        private void Awake()
        {
            _addUnitButton.onClick.AddListener(OnAddButtonClicked);
            _removeUnitButton.onClick.AddListener(OnRemoveButtonClicked);
            _clearSurfaceButton.onClick.AddListener(OnClearButtonClicked);
        }

        private void OnDestroy()
        {
            _addUnitButton.onClick.RemoveListener(OnAddButtonClicked);
            _removeUnitButton.onClick.RemoveListener(OnRemoveButtonClicked);
            _clearSurfaceButton.onClick.RemoveListener(OnClearButtonClicked);
        }

        #endregion

        #region Public

        public void SetUnitAddCallback(Action callback) => _unitAddCallback = callback;

        public void SetUnitRemoveCallback(Action callback) => _unitRemoveCallback = callback;

        public void SetClearCallback(Action callback) => _clearCallback = callback;

        #endregion

        #region Private

        private void OnAddButtonClicked() => _unitAddCallback?.Invoke();

        private void OnRemoveButtonClicked() => _unitRemoveCallback?.Invoke();

        private void OnClearButtonClicked() => _clearCallback?.Invoke();

        #endregion
    }
}