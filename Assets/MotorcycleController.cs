using UnityEngine;
using UnityEngine.InputSystem;

public class MotorcycleController : MonoBehaviour
{
    // --- STAREA JOCULUI ȘI ACCESIBILITATE ---
    [Header("Game State")]
    public bool isEngineRunning = false;
    private bool isGameOver = false;
    
    // --- SETUP FIZICĂ ȘI VITEZĂ ---
    [Header("Motor Setup")]
    public float forwardSpeed = 15f;    // Viteza constantă de bază (în unități/secundă)
    
    // --- CONFIGURARE VIRAJ (TRANSLAȚIE LATERALĂ) ---
    [Header("Arcade Steer Setup")]
    public float laneWidth = 3.5f;     // Distanța pe axa X între benzi
    public float laneSwitchSpeed = 8f; // Cât de repede se mută motocicleta lateral
    public float maxRollAngle = 30f;   // Unghiul maxim de înclinare vizuală (Roll)
    
    // --- REFERINȚE INTERNE ---
    private Rigidbody rb;
    private int targetLane = 0;        // Banda țintă: -1 (stânga), 0 (centru), 1 (dreapta)
    private float currentRollInput = 0f; // Input-ul pentru înclinarea vizuală (-1 sau 1)

    // ====================================================================
    // 1. INIȚIALIZARE ȘI PORNIRE
    // ====================================================================

    void Start()
    {
        // Obține Rigidbody-ul
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody missing on MotorcycleController!");
            enabled = false;
        }

        // Setări de bază (ar trebui setate și în Inspector)
        // rb.freezeRotation = FreezeRotationFlags.FreezeRotationX | FreezeRotationFlags.FreezeRotationZ;
    }
    
    // Metodă publică apelată de scriptul LaneSwitcher/StartGameTrigger
    public void StartEngine()
    {
        if (!isGameOver)
        {
            rb.isKinematic = false; // Asigură-te că fizica este activă
            isEngineRunning = true;
            Debug.Log("Motocicleta a pornit! Schimbă banda cu butoanele X/A.");
        }
    }

    // ====================================================================
    // 2. LOGICĂ DE FIZICĂ ȘI MIȘCARE (FixedUpdate)
    // ====================================================================
    void FixedUpdate()
{
    if (isGameOver)
    {
        return;
    }
    
    if (!isEngineRunning) 
    {
        // Oprește motocicleta dacă motorul nu este pornit
        if (rb.linearVelocity.sqrMagnitude > 0.01f) // Folosim .velocity.sqrMagnitude pentru performanță
        {
            rb.linearVelocity *= 0.98f; // Folosim proprietatea corectă: rb.velocity
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
    if (rb.linearVelocity.magnitude > forwardSpeed * 1.5f) // Folosim proprietatea corectă: rb.velocity
    {
        rb.linearVelocity = rb.linearVelocity.normalized * (forwardSpeed * 1.5f); // Folosim proprietatea corectă: rb.velocity
    }
}
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
    
    // Resetează înclinarea vizuală la 0
    private void ResetRoll()
    {
        currentRollInput = 0f;
    }

    // ====================================================================
    // 4. COLIZIUNE ȘI GAME OVER
    // ====================================================================

    private void OnCollisionEnter(Collision collision)
    {
        // Motocicleta trebuie să aibă Rigidbody și Collider
        // Obstacolul trebuie să aibă Collider și Tag-ul "Obstacle"
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

        // Oprește complet mișcarea fizică
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; 

        Debug.Log("GAME OVER! Ai lovit un obstacol!");
        // Aici se va adăuga logica de UI/restart.
    }
}
/*using UnityEngine;
using UnityEngine.InputSystem;

public class MotorcycleController : MonoBehaviour
{
    // --- SETUP INPUT (Legat in Inspector) ---
    [Header("Input Setup")]
    public InputActionProperty steerAction; // Nu mai e folosit direct pentru viraj, dar poate fi legat de X/A pentru simplitate
    
    // --- STARE JOC ȘI FIZICĂ ---
    [Header("Game State")]
    public bool isEngineRunning = false;
    private bool isGameOver = false;

    [Header("Motor Setup")]
    public float forwardSpeed = 15f;    // Viteza constantă de mers înainte
    
    [Header("Arcade Steer Setup")]
    public float laneWidth = 3.5f;     // Distanța dintre benzi (X)
    public float laneSwitchSpeed = 8f; // Cât de repede se mută lateral
    public float maxRollAngle = 30f;   // Unghiul maxim de înclinare vizuală (Roll)
    
    [Header("References")]
    private Rigidbody rb;

    // --- VARIABILE INTERNE ---
    private int targetLane = 0;        // Banda țintă: -1 (stânga), 0 (centru), 1 (dreapta)
    private float currentRollInput = 0f; // Valoare -1 la 1 pentru înclinarea vizuală

    // ====================================================================
    // 1. START ȘI PORNIRE
    // ====================================================================

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody missing on Motorcycle!");
            enabled = false;
            return;
        }

        // Activează citirea acțiunii de input (dacă o folosiți pentru viraj/pornire)
        if (steerAction.action != null)
        {
            steerAction.action.Enable();
        }
    }
    
    // Metodă apelată de scriptul 'StartGameTrigger' la apăsarea butonului A/X
    public void StartEngine()
    {
        if (!isGameOver)
        {
            isEngineRunning = true;
            Debug.Log("Motocicleta a pornit! Schimbă banda cu butoanele A/X.");
        }
    }

    // ====================================================================
    // 2. LOGICĂ DE FIZICĂ ȘI MIȘCARE
    // ====================================================================
    
    void FixedUpdate()
    {
        if (isGameOver)
        {
            return;
        }
        
        if (!isEngineRunning) 
        {
            // Frânare ușoară cât timp motorul e oprit
            if (rb.linearVelocity.sqrMagnitude > 0.01f)
            {
                rb.linearVelocity *= 0.98f; 
            }
            return;
        }

        // 1. Accelerație Constantă (Aplică forță înainte)
        Vector3 forwardForce = transform.forward * forwardSpeed * rb.mass;
        rb.AddForce(forwardForce, ForceMode.Acceleration);

        // 2. Translație Laterală (Mutarea între benzi)
        float targetX = targetLane * laneWidth;
        
        Vector3 newPosition = rb.position;
        // Mută poziția X a motocicletei către banda țintă
        newPosition.x = Mathf.Lerp(rb.position.x, targetX, Time.fixedDeltaTime * laneSwitchSpeed);
        
        // Aplică noua poziție (MovePosition este folosit pentru Rigidbody)
        rb.MovePosition(newPosition);
        
        // 3. Simulează Înclinarea (Roll) Vizual
        // Înclină motocicleta vizual în direcția benzii
        float rollAngle = -currentRollInput * maxRollAngle; 
        
        // Menține rotația pe Y (Yaw), dar aplică rotația vizuală pe Z (Roll)
        Quaternion rollRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, rollAngle);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rollRotation, Time.fixedDeltaTime * laneSwitchSpeed);

        // 4. Limitează viteza
        if (rb.linearVelocity.magnitude > forwardSpeed * 1.5f)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * (forwardSpeed * 1.5f);
        }
    }
    
    // ====================================================================
    // 3. LOGICĂ DE INPUT (Butoane X/A)
    // ====================================================================
    
    // Metodă publică apelată de scriptul LaneSwitcher la apăsarea X sau A
    public void ChangeLane(int direction) // direction este -1 (stânga) sau 1 (dreapta)
    {
        if (!isEngineRunning || isGameOver) return;
        
        // Schimbă banda țintă și o limitează între -1 (stânga) și 1 (dreapta)
        targetLane = Mathf.Clamp(targetLane + direction, -1, 1);
        
        // Setează input-ul de Roll pentru înclinarea vizuală
        currentRollInput = direction; 
    }
    
    // Resetează înclinarea vizuală (pentru a evita ca motocicleta să rămână înclinată)
    public void ResetRoll()
    {
        currentRollInput = 0f;
    }

    // ====================================================================
    // 4. COLIZIUNE ȘI GAME OVER
    // ====================================================================

    private void OnCollisionEnter(Collision collision)
    {
        // Asigură-te că obstacolul are Tag-ul "Obstacle"
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

        // Oprește mișcarea și fizica
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; 

        Debug.Log("GAME OVER! Ai lovit un obstacol!");
        // Aici se adaugă logica de UI/restart.
    }
}*/
/*using UnityEngine;
using UnityEngine.InputSystem;
public class MotorcycleController : MonoBehaviour
{
    [Header("Input Setup")]
    // Legăm acțiunea de input de viraj (e.g., A/D sau controlerul VR)
    public InputActionProperty steerAction;
    public bool isEngineRunning = false;
    [Header("Motor Setup")]
    public float forwardSpeed = 2f;  
    public float maxSteerAngle = 30f;   
    public float steeringSpeed = 1f;  
    [Header("References")]
    private Rigidbody rb;
    private float currentSteerAngle = 0f;
    private float currentSteerInput = 0f; // Valoare între -1 și 1 (Stânga/Dreapta)

    void Start()
    {
        // Obține referința la Rigidbody la Start
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody missing on Motorcycle!");
            enabled = false; // Dezactivează scriptul dacă Rigidbody nu există
        }
        if (steerAction.action != null)
    {
        steerAction.action.Enable(); 
    }
    }
void Update()
    {
        // Citirea Input-ului
        // Valoarea (float) va fi între -1 (stânga) și 1 (dreapta)
        float steerInput = steerAction.action.ReadValue<float>();
        // Trimite valoarea către logica fizică din FixedUpdate
        SetSteerInput(steerInput);
    }
    void FixedUpdate()
    {
        if (!isEngineRunning) 
        {
            // Dacă motorul nu rulează, putem adăuga o forță de frânare ușoară
            rb.linearVelocity *= 0.98f; 
            return; // Ieși din funcție
        }
        // Fizica se execută în FixedUpdate pentru precizie

        // 1. Accelerație Constantă (Endless Runner)
        // Aplică o forță constantă înainte (pe axa Z locală a motocicletei)
        Vector3 forwardForce = transform.forward * forwardSpeed * rb.mass;
        rb.AddForce(forwardForce, ForceMode.Acceleration);

        // 2. Aplică Virajul
        // Calculează unghiul țintă pe baza input-ului
        currentSteerAngle = currentSteerInput * maxSteerAngle;

        // Roata direcțională (Yaw/Rotirea pe axa Y)
        Quaternion turnRotation = Quaternion.Euler(0, currentSteerAngle * Time.deltaTime * steeringSpeed, 0);
        rb.MoveRotation(rb.rotation * turnRotation);

        // 3. Aplica înclinarea vizuală (Roll)
        // Deși avem Freeze Rotation, putem simula vizual înclinarea:
        float rollAngle = -currentSteerInput * maxSteerAngle * 0.5f; // Înclinare în direcția virajului
        // Aplicăm rotația pe axa Z (Roll) local. (Putem folosi Lerp pentru smooth)
        Quaternion rollRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, rollAngle);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, rollRotation, Time.deltaTime * steeringSpeed);

        // 4. Limitează viteza (opțional, pentru a nu accelera la infinit)
        if (rb.linearVelocity.magnitude > forwardSpeed * 1.5f)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * (forwardSpeed * 1.5f);
        }
    }
    public void StartEngine()
    {
        isEngineRunning = true;
        Debug.Log("Motocicleta a pornit!");
    }
    public void SetSteerInput(float input)
    {
        currentSteerInput = Mathf.Clamp(input, -1f, 1f);
    }
}*/