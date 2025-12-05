/*using UnityEngine;

public class moving : MonoBehaviour
{[Header("Movement Settings")]
    public float acceleration = 2f;
    public float brakeForce = 3f;
    public float maxSpeed = 5f;

    [Header("Input Action")]
    public InputActionProperty moveAction; // legat la thumbstick

    private float speed = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { Vector2 input = moveAction.action.ReadValue<Vector2>();

        // dacă împingi stick-ul în față → accelerează
        if (input.y > 0.1f)
        {
            speed += acceleration * Time.deltaTime;
        }

        // dacă tragi stick-ul înapoi → frânează
        if (input.y < -0.1f)
        {
            speed -= brakeForce * Time.deltaTime;
        }

        // limitează viteza
        speed = Mathf.Clamp(speed, 0f, maxSpeed);

        // deplasează XR Origin înainte (pe direcția camerei)
        Vector3 forward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        transform.Translate(forward * speed * Time.deltaTime, Space.World);
    }
}
*/