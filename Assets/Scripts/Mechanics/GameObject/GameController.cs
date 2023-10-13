using Platformer.Core;
using UnityEngine;

namespace Platformer.Mechanics {
    /// <summary>
    /// Tick the simulation.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        public static GameObject player { get; private set; }
        public static GameController Instance { get; private set; }
        void OnEnable()
        {
            Instance = this;
            player = GameObject.FindGameObjectWithTag("Player");
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
        }
    }
}

