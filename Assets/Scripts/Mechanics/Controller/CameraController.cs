using Platformer.Core;
using UnityEngine;
using Cinemachine;
namespace Platformer.Mechanics
{
    /// <summary>
    /// Tick the simulation.
    /// </summary>
    public class CamearaController : MonoBehaviour
    {
        public CinemachineVirtualCamera Current_camera;
        public static CamearaController Instance { get; private set; }
        void OnEnable()
        {
            Instance = this;
            Current_camera.Priority = 10;
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

    }
}

