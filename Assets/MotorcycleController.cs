
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class MotorcycleController : MonoBehaviour
{
    [Header("Game State")]
    public bool isEngineRunning = false;
    private bool isGameOver = false;
    [Header("Scoring")]
    public float distanceTraveled = 0f;
    private float initialZPosition; 
    public UIManager uiManager;
    [Header("Motor Setup")]
    public float forwardSpeed = 15f;   
    [Header("Arcade Steer Setup")]
    public float laneWidth = 3.5f;     
    public float laneSwitchSpeed = 8f; 
    public float maxRollAngle = 20f;  
    
    private Rigidbody rb;
    private int targetLane = 0;        
    private float currentRollInput = 0f; 
    


    [Header("Advanced Speed Setup")]
public float currentSpeed = 0f;      // Viteza actuală
public float maxSpeed = 30f;         // Viteza maximă pe care o poate atinge
public float acceleration = 5f;      // Cât de repede crește viteza pe secundă
public float deceleration = 3f;      // Cât de repede scade când nu accelerezi
public float brakingForce = 10f;     // Cât de repede scade când pui frână

void FixedUpdate()
{
    if (isGameOver || !isEngineRunning) 
    {
        currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.fixedDeltaTime * deceleration);
        return;
    }

    // Aplicăm mișcarea bazată pe currentSpeed
    Vector3 velocity = transform.forward * currentSpeed;
    velocity.y = rb.linearVelocity.y; // Păstrăm gravitația
    rb.linearVelocity = velocity;

    // Logica de translație laterală (X) rămâne neschimbată...
}

// Metodă pentru a controla accelerația din exterior
public void ApplyThrottle(float input) // input între -1 (frână) și 1 (accelerație)
{
    if (input > 0) 
    {
        currentSpeed += acceleration * input * Time.deltaTime;
    }
    else if (input < 0) 
    {
        currentSpeed += brakingForce * input * Time.deltaTime;
    }
    else 
    {
        // Decelerare naturală (fricțiune cu aerul/solul)
        currentSpeed = Mathf.MoveTowards(currentSpeed, 5f, deceleration * Time.deltaTime);
    }

    currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
}



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody missing on MotorcycleController!");
            enabled = false;
        }

        initialZPosition = transform.position.z;
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
        }
        // rb.freezeRotation = FreezeRotationFlags.FreezeRotationX | FreezeRotationFlags.FreezeRotationZ;
    }
    public void StartEngine()
    {
        if (!isGameOver)
        {
            rb.isKinematic = false; 
            isEngineRunning = true;
            Debug.Log("Motocicleta a pornit! Schimbă banda cu butoanele X/A.");
        }
    }
   /* void FixedUpdate()
{
    if (isGameOver)
    {
        return;
    }
    if (isEngineRunning && !isGameOver)
        {
            distanceTraveled = transform.position.z - initialZPosition;
            
        }
    if (!isEngineRunning) 
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f) 
        {
            rb.linearVelocity *= 0.98f; 
        }
        return;
    }

    Vector3 forwardForce = transform.forward * forwardSpeed * rb.mass;
    rb.AddForce(forwardForce, ForceMode.Acceleration);
    float targetX = targetLane * laneWidth;
    
    Vector3 newPosition = rb.position;
    newPosition.x = Mathf.Lerp(rb.position.x, targetX, Time.fixedDeltaTime * laneSwitchSpeed);
    rb.MovePosition(newPosition);
    float rollAngle = -currentRollInput * maxRollAngle; 
    Quaternion rollRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, rollAngle);
    transform.localRotation = Quaternion.Lerp(transform.localRotation, rollRotation, Time.fixedDeltaTime * laneSwitchSpeed);

    // 4. Limitează viteza (opțional)
    if (rb.linearVelocity.magnitude > forwardSpeed * 1.5f) // Folosim proprietatea corectă: rb.velocity
    {
        rb.linearVelocity = rb.linearVelocity.normalized * (forwardSpeed * 1.5f); // Folosim proprietatea corectă: rb.velocity
    }
}*/
    /*void FixedUpdate()
    {
        if (isGameOver)
        {
            return;
        }
        
        if (!isEngineRunning) 
        {
            // Oprește motocicleta dacă motorul nu este pornit
            if (rb.linearVelocity.sqrMagnitude > 0.01f)
            {
                rb.linearVelocity *= 0.98f; 
            }
            return;
        }

        // 1. Accelerație Constantă (Mișcare înainte pe axa Z)
        Vector3 forwardForce = transform.forward * forwardSpeed * rb.mass;
        rb.AddForce(forwardForce, ForceMode.Acceleration);

        // 2. Translație Laterală (Mutarea între benzi)
        float targetX = targetLane * laneWidth;
        
        Vector3 newPosition = rb.position;
        // Mută poziția X a motocicletei către banda țintă (Smooth Transition)
        newPosition.x = Mathf.Lerp(rb.position.x, targetX, Time.fixedDeltaTime * laneSwitchSpeed);
        
        // Aplică noua poziție, păstrând rotația existentă
        rb.MovePosition(newPosition);
        
        // 3. Simulează Înclinarea (Roll) Vizual
        // Înclină motocicleta vizual pe axa Z în funcție de banda țintă
        float rollAngle = -currentRollInput * maxRollAngle; 
        
        // Calculează rotația vizuală (păstrează rotația Yaw de bază)
        Quaternion rollRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, rollAngle);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rollRotation, Time.fixedDeltaTime * laneSwitchSpeed);

        // 4. Limitează viteza (opțional)
        if (rb.linearVelocity.magnitude > forwardSpeed * 1.5f)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * (forwardSpeed * 1.5f);
        }
    }*/
    
    // ====================================================================
    // 3. LOGICĂ DE INPUT (Apelată de LaneSwitcher.cs)
    // ====================================================================
    
    // Metodă publică apelată de LaneSwitcher la apăsarea butonului X sau A
    public void ChangeLane(int direction) // direction este -1 (stânga) sau 1 (dreapta)
    {
        if (!isEngineRunning || isGameOver) return;
        
        // Schimbă banda țintă și o limitează la intervalul [-1, 1]
        targetLane = Mathf.Clamp(targetLane + direction, -1, 1);
        
        // Setează input-ul de Roll pentru înclinarea vizuală
        currentRollInput = direction; 
        
        // Resetăm roll-ul vizual după o scurtă perioadă pentru a preveni
        // ca motocicleta să rămână înclinată (poate fi apelată și la 'canceled')
        Invoke("ResetRoll", 0.3f); 
    }
    
    private void ResetRoll()
    {
        currentRollInput = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            HandleGameOver();
        }
    }

    void HandleGameOver()
    {
        if (isGameOver) return; 

        isGameOver = true;
        isEngineRunning = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; 

        Debug.Log("GAME OVER! Ai lovit un obstacol!");
        if (uiManager != null)
        {
            uiManager.ShowGameOver(distanceTraveled);
        }
    }
}
