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
        public int Damage;
        public override void Execute()
        {
            var health= GameObject.Find("Player").GetComponent<Health>();
            health.Hurt(Damage);
            Debug.Log($"Damage: {Damage},IsAlive:{health.IsAlive}");
            if (true) {
                PlayerDeath ev = Schedule<PlayerDeath>();
            }
        }
    }
}