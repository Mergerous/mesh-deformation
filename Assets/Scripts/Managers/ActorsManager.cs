using System.Collections.Generic;
using Scripts.Behaviours;
using Scripts.Controllers;
using UnityEngine;
using Random = UnityEngine.Random;

public class ActorsManager : MonoBehaviour
{
    [Header("Options")] 
    [SerializeField] private ActorsConfigurationSO _actorsConfigurationSo;
    [Header("Components")]
    [SerializeField] private ActorBehaviour[] _actors;
    [SerializeField] private Transform[] _points;

    private readonly List<ActorController> _controllers = new List<ActorController>();
    private int _unitCount = 0;

    private void Awake()
    {
        foreach (var actor in _actors)
        {
            var controller = new ActorController(actor, _actorsConfigurationSo);
            controller.SetTargetRequest(OnTargetRequested);
            _controllers.Add(controller);
        }
    }

    private Transform OnTargetRequested()
    {
        return _points[Random.Range(0, _points.Length)];
    }

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
}
