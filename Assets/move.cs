using UnityEngine;

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