using Platformer.Gameplay;
using UnityEngine;
using static Platformer.Core.Simulation;

namespace Platformer.Mechanics {
    public class HurtZone : MonoBehaviour
    {
        [SerializeField] float damage;
        void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<Player>().Hurt(damage);
            }
        }
        
    }
}

