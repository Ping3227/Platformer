using UnityEngine;

namespace Platformer.Mechanics
{
    /// <summary>
    /// player Controller than controll all physic and animation of player
    /// </summary>
    [RequireComponent(typeof(Stamina), typeof(Health), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
     
        private Stamina stamina;
        private Health health;
        private Animator animator;
        void OnEnable()
        {
            stamina = GetComponent<Stamina>();
            health = GetComponent<Health>();
            animator =GetComponent<Animator>();
        }
        /// <summary>
        /// Receive input and calculate all physics and control animation 
        /// </summary>
        void Update()
        {


        }
    }

}