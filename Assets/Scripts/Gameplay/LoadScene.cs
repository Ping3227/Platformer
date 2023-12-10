using Platformer.UI;

using UnityEngine;
using static Platformer.Core.Simulation;

using UnityEngine.SceneManagement;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    public class LoadScene : Event<LoadScene>
    {
        public string SceneName;
        public override void Execute()
        {
            
            SceneManager.LoadScene(SceneName);
            Schedule<ResetAll>();
        }
    }
}

