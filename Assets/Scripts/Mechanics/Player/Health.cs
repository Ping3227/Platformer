using Platformer.Core;
using Platformer.Gameplay;
using Platformer.UI;
using UnityEngine;
namespace Platformer.Mechanics {
    /// <summary>
    /// Manager player health related attributes
    /// </summary>
    public class Health : MonoBehaviour
    {
        private float currentHP;

        [SerializeField] float MaxHP;
        public bool IsAlive => currentHP > 0;
        private float healthPercentage => currentHP / MaxHP;
       
        public void Heal(float value)
        {
            if (!IsAlive) return;
           
            currentHP += value;
            if (currentHP > MaxHP)
            {
                currentHP = MaxHP;
            }
            GamesceneUIController.instance.SetHealth(healthPercentage);
        }
        public void Hurt(float  value)
        {
            if(IsAlive) currentHP -= value;
            if (currentHP <= 0)
            {
                currentHP = 0;
                GetComponent<Player>().Dead();
                Simulation.Schedule<PlayerDeath>();
            }
            GamesceneUIController.instance.SetHealth(healthPercentage);

        }
        void Start()
        {
            currentHP = MaxHP;
            GamesceneUIController.instance.SetMaxHealth(1);
        }
        public void SetParameter(int index) { 
            MaxHP = ModeManager.instance.modes[ModeManager.instance.index].health;
            currentHP = MaxHP;
            GamesceneUIController.instance.SetMaxHealth(1);
        }
    }
}

