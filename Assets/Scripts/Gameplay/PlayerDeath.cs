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
        }
    }
}