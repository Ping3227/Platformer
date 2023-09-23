using UnityEngine;
namespace Platformer.Mechanics {
    /// <summary>
    /// Manager player health related attributes
    /// </summary>
    public class Health : MonoBehaviour
    {
        private int currentHP;

        [SerializeField]
        int MaxHP;

        bool IsAlive => currentHP > 0;
        public void Heal(int value)
        {
            currentHP += value;
            if (currentHP > MaxHP)
            {
                currentHP = MaxHP;
            }
        }
        public void Hurt(int value)
        {
            currentHP -= value;
            if (currentHP <= 0)
            {
                currentHP = 0;
            }
        }
        void OnEnable()
        {
            currentHP = MaxHP;
        }

    }
}

