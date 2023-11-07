using Platformer.Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Platformer.Mechanics {
    /// <summary>
    /// Tick the simulation.
    /// </summary>
    public class GameController : MonoBehaviour
    {
        public static Player player { get; private set; }
        public static GameController Instance { get; private set; }
        public  Vector3 CheckPoint{ get; private set; }
        public bool IsSaved { get; private set; } 
        void OnEnable()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            IsSaved = false;
            
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else { 
                Destroy(gameObject);
            }
            
        }

        void OnDisable()
        {
            if (Instance == this) Instance = null;
        }

        void Update()
        {
            if (Instance == this) Simulation.Tick();
            
        }
        public void SetCheckPoint(Vector3 position) {
            CheckPoint = position;
            IsSaved = true;
        }

    }
}

