using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private HUDController _hudController;
        public HUDController HUDController => _hudController;
    }
}