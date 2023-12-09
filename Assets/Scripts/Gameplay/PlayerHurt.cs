using UnityEngine;
using Platformer.Mechanics;
using static Platformer.Core.Simulation;
namespace Platformer.Gameplay
{
    /// <summary>
    /// Fired when the player been Hurt.
    /// </summary>
    /// <typeparam name="PlayerHurt"></typeparam>
    public class PlayerHurt : Event<PlayerHurt>
    {
        public float TimeSlower;
        public float RecoverRate;
        public float ShakeAmp;
        public float ShakeFrequency;
        public float ShakeDuration;
        public override void Execute()
        {
            Time.timeScale= TimeSlower;
           
            var ev =Schedule<RecoverTime>(0.1f);
            ev.RecoverRate = RecoverRate;
            CamearaController.Instance.ShakeCamera(ShakeAmp, ShakeFrequency, ShakeDuration);

            //TODO: Camera shake and time slower 
        }
    }
}