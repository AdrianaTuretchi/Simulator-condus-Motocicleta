// Nume: MotorController.cs (Atașat la obiectul 'moto')

using UnityEngine;

public class MotorController2 : MonoBehaviour
{
    [Header("Referințe Spawn")]
    // Trageți obiectul StartPoint aici
    public Transform startPoint; 

    // Awake este cel mai sigur loc pentru poziționarea inițială în VR
    void Awake()
    {
        if (startPoint != null)
        {
            // MUTĂ DOAR RĂDĂCINA (MOTO)
            transform.position = startPoint.position;
            transform.rotation = startPoint.rotation;
        }
    }

    // Aici vine logica ta de mișcare din Update()
}
/*using UnityEngine;

public class MotorController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float constantSpeed = 5f;        // Viteza de înaintare (forward)
    public float rotationSpeed = 2f;        // Viteză de rotație a motocicletei

    [Header("References")]
    public Transform headTransform;         // Camera din XR Origin (Main Camera)

    void Update()
    {
        // 1. CALCULEAZĂ DIRECȚIA DORITĂ
        // Extragem doar rotația pe axa Y (Yaw) din privirea camerei, ignorând sus/jos (pitch).
        Vector3 targetForwardDir = new Vector3(headTransform.forward.x, 0, headTransform.forward.z).normalized;
        
        // Calculează rotația necesară pentru a privi în acea direcție
        if (targetForwardDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetForwardDir);

            // 2. APLICĂ ROTAȚIA pe obiectul RĂDĂCINĂ (motos)
            // Rotim lin obiectul 'motos' către direcția privirii jucătorului.
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                rotationSpeed * Time.deltaTime
            );
        }

        // 3. APLICĂ MIȘCAREA ÎNAINTE (FORWARD)
        // Obiectul 'motos' se mișcă acum în direcția sa LOCALĂ (transform.forward)
        // Folosim Space.Self sau Vector3.forward (implicit Space.Self)
        transform.Translate(Vector3.forward * constantSpeed * Time.deltaTime);
    }
}
*/

/*using UnityEngine;

public class MotorController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float constantSpeed = 5f;   // viteza standard

    [Header("References")]
    public Transform headTransform;    // Camera din XR Origin (Main Camera)

    void Update()
    {
        // Folosim doar rotația pe axa Y (yaw) a camerei
        Vector3 forwardDir = new Vector3(headTransform.forward.x, 0, headTransform.forward.z);
        forwardDir.Normalize();

        // Motorul se deplasează în direcția privirii
        transform.Translate(forwardDir * constantSpeed * Time.deltaTime, Space.World);
    }
}

*/
/*using UnityEngine;

public class MotorController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float constantSpeed = 5f;   // viteza standard

    [Header("References")]
    public Transform bodyTransform;    // XR Rig sau Camera (pentru direcția corpului)

    void Update()
    {
        // direcția de deplasare = orientarea corpului (yaw)
        Vector3 forwardDir = bodyTransform.forward;
        forwardDir.y = 0; // ignoră înclinarea pe verticală
        forwardDir.Normalize();

        // deplasează Motorul în direcția corpului cu viteză constantă
        transform.Translate(forwardDir * constantSpeed * Time.deltaTime, Space.World);
    }
}
*/
/*
using UnityEngine;
using UnityEngine.InputSystem;

public class MotorController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 10f;
    public float brakeForce = 5f;
    public float maxSpeed = 10f;

    [Header("Input Actions")]
    public InputActionProperty triggerAction; // legat la XRI RightHand/Trigger
    public InputActionProperty grabAction;    // legat la XRI RightHand/Grip (Grab)

    [Header("References")]
    public Transform bodyTransform; // XR Rig sau Camera (pentru direcția corpului)

    private float speed = 0f;

    void Update()
    {
        // citește valoarea triggerului (0–1)
        float triggerValue = triggerAction.action.ReadValue<float>();

        // accelerează proporțional cu triggerul
        if (triggerValue > 0.1f)
        {
            speed += acceleration * triggerValue * Time.deltaTime;
        }

        // frânează dacă grab e apăsat
        if (grabAction.action.IsPressed())
        {
            speed -= brakeForce * Time.deltaTime;
        }

        // limitează viteza
        speed = Mathf.Clamp(speed, 0f, maxSpeed);

        // direcția de deplasare = orientarea corpului (yaw)
        Vector3 forwardDir = bodyTransform.forward;
        forwardDir.y = 0; // ignoră înclinarea pe verticală
        forwardDir.Normalize();

        // deplasează Motorul în direcția corpului
        transform.Translate(forwardDir * speed * Time.deltaTime, Space.World);
    }
}
*/

/*using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class MotorController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 10f;
    public float brakeForce = 5f;
    public float maxSpeed = 10f;

    [Header("Input Actions")]
    public InputActionProperty triggerAction; // legat la XRI RightHand/Trigger
    public InputActionProperty grabAction;    // legat la XRI RightHand/Grip (Grab)

    private float speed = 0f;

    void Update()
    {
        // citește valoarea triggerului (0–1)
        float triggerValue = triggerAction.action.ReadValue<float>();
        //Debug.Log("Trigger value: " + triggerValue);
        speed += acceleration *( triggerValue +1)* Time.deltaTime;
        // accelerează proporțional cu triggerul
        if (triggerValue > 0.1f)
        {
            speed += acceleration *( triggerValue +100)* Time.deltaTime;
        }
        Debug.Log(grabAction.action.IsPressed());
        // frânează dacă grab e apasat
        if (grabAction.action.IsPressed())
        {
            speed -= brakeForce * Time.deltaTime;
        }

        // limitează viteza
        speed = Mathf.Clamp(speed, 0f, maxSpeed);

        // deplasează Motorul înainte
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
*/