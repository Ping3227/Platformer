using UnityEngine;
namespace Platformer.Mechanics
{
    /// <summary>
    /// Manager player Stamina related attributes
    /// </summary>
    public class Stamina : MonoBehaviour
    {
        private int CurrentStamina;

        [SerializeField] int MaxStamina;

        [SerializeField] int RechargingRate;
        bool IsEmpty => CurrentStamina == 0;
        /// <summary>
        /// can only be charging while player is not doing speical actions
        /// </summary>
        bool IsCharging;
        /// <summary>
        /// charge stamina while player is not doing speical actions
        /// </summary>
        void Update()
        {
            if (IsCharging)
            {
                CurrentStamina += RechargingRate;
                // Schedule<UpdateStamina>
                if (CurrentStamina > MaxStamina)
                {
                    CurrentStamina = MaxStamina;
                    IsCharging = false;
                }
            }
        }
        public bool ConsumeStamina(int Value) { 
            if (CurrentStamina >= Value)
            {
                CurrentStamina -= Value;
                // Scedule<UpdateStamina>
                return true;
            }
            return false;
        }
        void OnEnable()
        {
            CurrentStamina = MaxStamina;
        }

    }

}
