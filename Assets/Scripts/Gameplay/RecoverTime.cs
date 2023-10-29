using UnityEngine;
using Platformer.Mechanics;
using static Platformer.Core.Simulation;
using Platformer.Core;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player been Hurt.
    /// </summary>
    /// <typeparam name="PlayerHurt"></typeparam>
    public class RecoverTime : Event<RecoverTime>
    {
        public float RecoverRate;
        public override void Execute()
        {
            Time.timeScale += RecoverRate;
            if(Time.timeScale > 1) {
                Time.timeScale = 1;
            }
            else
            {
                var ev= Simulation.Schedule<RecoverTime>(0.1f);
                ev.RecoverRate = RecoverRate;
            }


            //TODO: Camera shake and time slower 
        }
    }
}