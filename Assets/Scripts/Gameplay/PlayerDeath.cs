using Platformer.Core;
using Platformer.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Platformer.Core.Simulation;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    public class PlayerDeath : Event<PlayerDeath>
    {
        public override void Execute()
        {
            Debug.Log("Player died.");

            
            GamesceneUIController.instance.Death();
            var reload =Simulation.Schedule<LoadScene>(1.5f);
            reload.SceneName = SceneManager.GetActiveScene().name;
            GamesceneUIController.instance.Respawn();
        }
    }
}