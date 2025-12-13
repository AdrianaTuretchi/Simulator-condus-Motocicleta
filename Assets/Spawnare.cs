// MotorController.cs (Atașat la Motorcycle_ROOT)
using UnityEngine;

public class MotorController : MonoBehaviour
{
    [Header("Referințe")]
    public Transform startPoint;         // Punctul unde spawnează

    [Header("Setări Mișcare")]
    public float constantSpeed = 5f;
    public float rotationSpeed = 2f;
    public Transform headTransform;      // Camera principala din XR Origin

    // AWAKE: Poziționarea inițială (cea mai sigură)
    void Awake()
    {
        if (startPoint != null)
        {
            // Mută întregul părinte la poziția de start.
            transform.position = startPoint.position;
            transform.rotation = startPoint.rotation;
            
            //Debug.Log("Motocicleta a fost spawnată la: " + startPoint.position);
        }
    }

    // UPDATE: Mișcarea continuă
    void Update()
    {
        // 1. Calcul Rotație (pentru a vira)
        Vector3 targetForwardDir = new Vector3(headTransform.forward.x, 0, headTransform.forward.z).normalized;
        
        if (targetForwardDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetForwardDir);

            // Rotește MOTOCICLETA (ROOT-ul)
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                rotationSpeed * Time.deltaTime
            );
        }

        // 2. Mișcare Înainte
        transform.Translate(Vector3.forward * constantSpeed * Time.deltaTime);
    }
}
/*// Script: SpawnerMotor.cs
using UnityEngine;

public class SpawnerMotor : MonoBehaviour
{ 
    public Transform startPoint; 
    // NU MAI ESTE NEVOIE DE SA

    // Awake este cel mai bun pentru operațiunile de poziționare inițială
    void Awake()
    {
        if (startPoint != null)
        {
            // Mută PĂRINTELE (acest obiect) la poziția dorită
            transform.position = startPoint.position;
            transform.rotation = startPoint.rotation;
            
            Debug.Log("Motocicleta Rădăcină (moto) a fost spawnată.");
        }
    }
}*/
/*using UnityEngine;

public class Spawnare : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/