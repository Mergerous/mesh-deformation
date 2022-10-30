using System;
using Scripts.Behaviours;
using UnityEngine;

namespace Scripts.Controllers
{
    public class ActorController
    {
        private readonly ActorBehaviour _behaviour;
        private readonly ActorsConfigurationSO _configuration;
        public ActorController(ActorBehaviour behaviour, ActorsConfigurationSO configuration)
        {
            _behaviour = behaviour;
            _configuration = configuration;
            
            _behaviour.SetSpeed(_configuration.Speed);
        }

        public void SetTargetRequest(Func<Transform> request) => _behaviour.SetTargetRequest(request);
        
        public void SetEnabled(bool isEnabled) => _behaviour.gameObject.SetActive(isEnabled);
    }
}