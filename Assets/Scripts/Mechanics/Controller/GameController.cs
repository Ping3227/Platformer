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
        private Vector3 CheckPoint;
        private bool IsSaved = false;
        void OnEnable()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if(IsSaved) player.transform.position = CheckPoint;
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

