using UnityEngine;
using Unity.XR.CoreUtils; // Asigurați-vă că acest using este prezent pentru VR Rig

public class HandlebarControl : MonoBehaviour
{
    [Header("Motorcycle Setup")]
    public MotorcycleController bikeController; // Referință la scriptul de control al motocicletei
    
    [Header("Steering Limits")]
    // Unghiul maxim pe care jucătorul îl poate roti ghidonul (e.g., 45 de grade în fiecare direcție)
    public float maxHandlebarAngle = 45f; 

    void Update()
    {
        // 1. Citirea Rotației pe Axa Y (Yaw) a ghidonului
        // Rotim ghidonul virtual în funcție de mișcarea controlerelor apucate.
        // Citim Euler Angles (rotația) pe axa Y, pe sistemul de coordonate locale.
        float currentYaw = transform.localEulerAngles.y;

        // 2. Ajustare Unghi (De la 0-360 la -180 la 180)
        // Rotația Y din Unity este de la 0 la 360. Trebuie să o mapăm la un interval negativ/pozitiv.
        if (currentYaw > 180)
        {
            currentYaw -= 360; 
        }

        // 3. Maparea Rotației la Input (-1 la +1)
        // Folosim Mathf.Clamp pentru a ne asigura că unghiul nu depășește limita
        float clampedYaw = Mathf.Clamp(currentYaw, -maxHandlebarAngle, maxHandlebarAngle);
        
        // Maparea valorii (ex: -45 la +45) la valoarea de input (-1 la +1)
        float steerInput = clampedYaw / maxHandlebarAngle;

        // 4. Trimiterea Input-ului la Controllerul Motocicletei
       //if (bikeController != null)
       // {
//bikeController.SetSteerInput(steerInput);
      //  }
       // else
       // {
       //     Debug.LogError("Referința la MotorcycleController lipsește!");
      //  }
    }
}