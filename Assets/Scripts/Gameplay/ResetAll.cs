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
    public class ResetAll : Event<ResetAll>
    {
        public override void Execute()
        {
            GamesceneUIController.instance.RecoverNum = 2;
        }
    }
}