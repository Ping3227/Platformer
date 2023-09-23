using UnityEngine;
/// <summary>
/// Manager player Stamina related attributes
/// </summary>
public class Stamina : MonoBehaviour
{
    private int CurrentStamina;

    [SerializeField]
    int MaxStamina;

    [SerializeField]
    int RechargingRate;
    bool IsEmpty => CurrentStamina == 0;
    bool IsCharging;
    void Update()
    {
        if (IsCharging) { 
            CurrentStamina += RechargingRate;
            if (CurrentStamina > MaxStamina)
            {
                CurrentStamina = MaxStamina;
                IsCharging = false;
            }
        }
    }
    void OnEnable()
    {
        CurrentStamina = MaxStamina;
    }

}
