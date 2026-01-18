using UnityEngine;
using UnityEngine.InputSystem; // Obligatoriu pentru noul sistem

public class KeyboardSteering : MonoBehaviour
{
    public MotorcycleController bikeController;


    void Update()
    {
        var keyboard = Keyboard.current;
    if (keyboard == null) return;

    float throttleInput = 0;
    if (keyboard.wKey.isPressed) throttleInput = 1;
    if (keyboard.sKey.isPressed) throttleInput = -1;

    bikeController.ApplyThrottle(throttleInput);
        if (bikeController == null) return;

        // Verificăm tastatura folosind noul sistem
        if (keyboard == null) return;

        // Detectăm apăsarea tastei A (Stânga)
        if (keyboard.aKey.wasPressedThisFrame)
        {
            bikeController.ChangeLane(-1);
        }
        
        // Detectăm apăsarea tastei D (Dreapta)
        if (keyboard.dKey.wasPressedThisFrame)
        {
            bikeController.ChangeLane(1);
        }

        // Detectăm Space pentru pornirea motorului
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            bikeController.StartEngine();
        }
    }
}