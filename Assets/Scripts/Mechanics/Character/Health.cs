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

       
        public void Heal(int value)
        {
            currentHP += value;
            if (currentHP > MaxHP)
            {
                currentHP = MaxHP;
            }
            GamesceneUIController.instance.SetHealth(currentHP);
        }
        public void Hurt(int value)
        {
            currentHP -= value;
            if (currentHP <= 0)
            {
                currentHP = 0;
            }
            GamesceneUIController.instance.SetHealth(currentHP);

        }
        void Start()
        {
            currentHP = MaxHP;
            GamesceneUIController.instance.SetMaxHealth(MaxHP);
        }

    }
}

