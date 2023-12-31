using Platformer.UI;
using UnityEngine;
namespace Platformer.Mechanics
{
    /// <summary>
    /// Manager player Stamina related attributes
    /// </summary>
    public class Stamina : MonoBehaviour
    {
        private float CurrentStamina;
        private float CoolDownTime;
        [SerializeField] float MaxStamina = 1f;
        [Tooltip("Time need to wait for recharging after using special movement")]
        [SerializeField] float RechargeCoolDown = 0.5f;
        [SerializeField] float RechargingRate =0.1f;
        /// <summary>
        /// can only be charging while player is finished speical actions
        /// </summary>
        bool IsCharging =false;
        /// <summary>
        /// charge stamina while player is not doing speical actions
        /// </summary>
        void Update()
        {
            
            if (!IsCharging && CoolDownTime>0)
            {
                
                CoolDownTime -= Time.deltaTime;
                
                if (CoolDownTime <= 0) { 
                    CoolDownTime = 0;
                    IsCharging = true;
                }
                
            }
            if(IsCharging)
            {
                CurrentStamina += RechargingRate*Time.deltaTime;

                if (CurrentStamina > MaxStamina)
                {
                    CurrentStamina = MaxStamina;
                    IsCharging = false;
                }
                else { 
                    GamesceneUIController.instance.SetStamina(CurrentStamina);
                }

            }
        }
        /// <summary>
        /// use this function in if condtion 
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public bool ConsumeStamina(float Value) {
            if (CurrentStamina != 0)
            {

                CurrentStamina -= Value;
                if (CurrentStamina < 0){
                    CurrentStamina = 0;
                }
                IsCharging = false;
                CoolDownTime = RechargeCoolDown;
                GamesceneUIController.instance.SetStamina(CurrentStamina);
                return true;
            }

            return false;
        }
        
        void OnEnable()
        {
            CurrentStamina = MaxStamina;
            if (GamesceneUIController.instance != null)
            GamesceneUIController.instance.SetStamina(1);
        }
        public void setParameter(int index) { 
            RechargingRate = ModeManager.instance.modes[ModeManager.instance.index].RechargeingRate;
            RechargeCoolDown = ModeManager.instance.modes[ModeManager.instance.index].RechargeingDelay;
        }
        
    }

}
