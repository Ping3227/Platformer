using Platformer.UI;
using UnityEngine;
namespace Platformer.Mechanics {
    /// <summary>
    /// Manager player health related attributes
    /// </summary>
    public class Health : MonoBehaviour
    {
        private int currentHP;
        public GamesceneUIController healthbar;

        [SerializeField] int MaxHP;
        public bool IsAlive => currentHP > 0;

       
        public void Heal(int value)
        {
            currentHP += value;
            if (currentHP > MaxHP)
            {
                currentHP = MaxHP;
            }

            healthbar.SetHealth(currentHP);
        }
        public void Hurt(int value)
        {
            currentHP -= value;
            if (currentHP <= 0)
            {
                currentHP = 0;
            }

            healthbar.SetHealth(currentHP);
        }
        void OnEnable()
        {
            currentHP = MaxHP;
            healthbar.SetMaxHealth(MaxHP);
        }

    }
}

