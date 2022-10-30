using Scripts.Managers;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private ActorsManager _actorsManager;
    [SerializeField] private WorldManager _worldManager;
    [SerializeField] private InputManager _inputManager;

    private void Awake()
    {
        
    }

    private void Start()
    {
        _uiManager.HUDController.SetClearCallback(_worldManager.ClearSurfaces);
        _uiManager.HUDController.SetUnitAddCallback(_actorsManager.Add);
        _uiManager.HUDController.SetUnitRemoveCallback(_actorsManager.Remove);
    }
}
