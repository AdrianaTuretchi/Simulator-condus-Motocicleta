using UnityEngine;
using UnityEngine.InputSystem; // Obligatoriu pentru noul sistem

public class KeyboardSteering : MonoBehaviour
{
    public MotorcycleController bikeController;

    void Update()
    {
        if (bikeController == null) return;

        // Verificăm tastatura folosind noul sistem
        var keyboard = Keyboard.current;
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