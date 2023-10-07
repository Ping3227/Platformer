using Cinemachine;
using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics
{
    public class CameraSwitchZone : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera target;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var camera = Schedule<SwitchCamera>();
                camera.source = CamearaController.Instance.Current_camera;
                camera.target = target;
            }
        }
    }
}

