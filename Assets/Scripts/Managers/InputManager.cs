using Scripts.Behaviours;
using Scripts.Configs;
using Scripts.Controllers;
using UnityEngine;

namespace Scripts.Managers
{
    public class InputManager : MonoBehaviour
    {
        [Header("Options")]
        [SerializeField] private BrushConfigurationSO _brushConfigurationSo;

        [Header("Components")]
        [SerializeField] private BrushBehaviour _brushBehaviour;

        #region Unity

        private void Awake()
        {
            var controller = new BrushController(_brushBehaviour, _brushConfigurationSo);
        }

        #endregion
    }
}