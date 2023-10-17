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
            currentHP += value;
            if (currentHP > MaxHP)
            {
                currentHP = MaxHP;
            }
            GamesceneUIController.instance.SetHealth(healthPercentage);
        }
        public void Hurt(float  value)
        {
            currentHP -= value;
            if (currentHP <= 0)
            {
                currentHP = 0;
            }
            GamesceneUIController.instance.SetHealth(healthPercentage);

        }
        void Start()
        {
            currentHP = MaxHP;
            GamesceneUIController.instance.SetMaxHealth(1);
        }

    }
}

