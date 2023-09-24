using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics {
    public class HurtZone : MonoBehaviour
    {
        void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.gameObject.GetComponent<PlatfromerPlayer>())
            {
                var ev = Schedule<PlayerHurt>();
                ev.Damage = 1;
            }
        }
        
    }
}

