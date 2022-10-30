using Scripts.Controllers;
using UnityEngine;

namespace Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private HUDController _hudController;
        public HUDController HUDController => _hudController;
    }
}