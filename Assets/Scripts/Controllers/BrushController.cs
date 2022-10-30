using Scripts.Behaviours;
using Scripts.Configs;
using Unity.AI.Navigation;
using UnityEngine;

namespace Scripts.Controllers
{
    public class BrushController
    {
        private readonly BrushBehaviour _behaviour;
        private readonly BrushConfigurationSO _configuration;

        public BrushController(BrushBehaviour behaviour, BrushConfigurationSO configuration)
        {
            _behaviour = behaviour;
            _configuration = configuration;

            _behaviour.SetHitCallback(OnHit);
            _behaviour.SetMouseUpCallback(OnMouseUp);
        }

        private void OnHit(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent(out SurfaceRenderer renderer))
            {
                renderer.SetSize(_configuration.BrushSize);
                renderer.SetStrength(_configuration.BrushStrength);
                renderer.SetSmoothness(_configuration.Smoothness);
                renderer.SetPosition(hit.textureCoord);
                renderer.Dispatch();
            }
        }

        private void OnMouseUp(RaycastHit hit)
        {
            if (hit.transform.TryGetComponent(out SurfaceRenderer renderer))
            {
                renderer.Rebuild();
            }

            if (hit.transform.TryGetComponent(out NavMeshSurface surface))
            {
                surface.BuildNavMesh();
            }
        }
    }
}