using UnityEngine;
using UnityEngine.InputSystem;

public class VRHandSteering : MonoBehaviour
{
    public MotorcycleController bikeController;
    
    [Header("VR Controllers")]
    public Transform leftHand;  
    public Transform rightHand; 
    
    [Header("Input Actions")]
    public InputActionReference throttleAction;

    [Header("Settings")]
    public float tiltThreshold = 0.15f; 
    private bool canChangeLane = true;  

    // CRITIC PENTRU DEVICE: Activarea acțiunii de input
    private void OnEnable()
    {
        if (throttleAction != null)
            throttleAction.action.Enable();
    }

    private void OnDisable()
    {
        if (throttleAction != null)
            throttleAction.action.Disable();
    }

void Update()
{
    // Verificăm dacă obiectele există în scenă
    if (leftHand == null || rightHand == null || bikeController == null) 
    {
        return; 
    }

    // DEBUG VIZUAL: Dacă în consolă valoarea e mereu 0, tracking-ul e mort
    float hDiff = rightHand.position.y - leftHand.position.y;

    if (canChangeLane)
    {
        if (hDiff > tiltThreshold)
        {
            bikeController.ChangeLane(1);
            StartCoroutine(InputCooldown());
        }
        else if (hDiff < -tiltThreshold)
        {
            bikeController.ChangeLane(-1);
            StartCoroutine(InputCooldown());
        }
    }
    
    // Accelerația
    if (throttleAction != null)
    {
        float tVal = throttleAction.action.ReadValue<float>();
        bikeController.ApplyThrottle(tVal);
    }
}/*
    void Update()
    {
        if (bikeController == null) return;

        // 1. ACCELERAȚIA
        if (throttleAction != null && throttleAction.action.enabled)
        {
            float throttleValue = throttleAction.action.ReadValue<float>();
            bikeController.ApplyThrottle(throttleValue);
        }

        // 2. VIRAREA
        // Verificăm dacă mâinile sunt asignate
        if (leftHand != null && rightHand != null)
        {
            // Folosim position (world) sau localPosition? 
            // Pe device, localPosition este mai stabilă în interiorul XR Origin
            float heightDifference = rightHand.localPosition.y - leftHand.localPosition.y;

            if (canChangeLane)
            {
                if (heightDifference > tiltThreshold)
                {
                    bikeController.ChangeLane(1); 
                    StartCoroutine(InputCooldown());
                }
                else if (heightDifference < -tiltThreshold)
                {
                    bikeController.ChangeLane(-1); 
                    StartCoroutine(InputCooldown());
                }
            }
        }
        else
        {
            // DEBUG: Dacă nu găsește mâinile pe device, apare în consolă
            // Debug.LogWarning("Mâinile VR nu sunt conectate în Inspector!");
        }
    }
*/
    System.Collections.IEnumerator InputCooldown()
    {
        canChangeLane = false;
        yield return new WaitForSeconds(0.4f);
        canChangeLane = true;
    }
}



/*
using UnityEngine;
using UnityEngine.InputSystem;
public class VRHandSteering : MonoBehaviour
{
    public MotorcycleController bikeController;
    
    [Header("VR Controllers")]
    public Transform leftHand;  // Trage aici Left Hand Controller din VR Rig
    public Transform rightHand; // Trage aici Right Hand Controller din VR Rig
    
    [Header("Input Actions")]
    // Aceasta este variabila care lipsea!
    public InputActionReference throttleAction;
    [Header("Settings")]
    public float tiltThreshold = 0.15f; // Cât de mult trebuie să înclini mâinile (metri)
    private bool canChangeLane = true;  // Previne schimbarea multiplă a benzilor la o singură înclinare

    void Update()
    {
        float throttleValue = throttleAction.action.ReadValue<float>();
    
            bikeController.ApplyThrottle(throttleValue);
        if (leftHand == null || rightHand == null || bikeController == null) return;

        // Calculăm diferența de înălțime pe verticală (Y) între mâna dreaptă și cea stângă
        // Imaginează-ți că ții un ghidon: dacă mâna dreaptă coboară, înclini spre dreapta.
        float heightDifference = rightHand.localPosition.y - leftHand.localPosition.y;

        // Logica de schimbare a benzii (Discrete Steering)
        if (canChangeLane)
        {
            if (heightDifference > tiltThreshold)
            {
                bikeController.ChangeLane(1); // Înclinare spre dreapta
                StartCoroutine(InputCooldown());
            }
            else if (heightDifference < -tiltThreshold)
            {
                bikeController.ChangeLane(-1); // Înclinare spre stânga
                StartCoroutine(InputCooldown());
            }
        }
    }

    // Cooldown mic pentru a nu sări peste 2 benzi deodată dintr-o mișcare bruscă
    System.Collections.IEnumerator InputCooldown()
    {
        canChangeLane = false;
        yield return new WaitForSeconds(0.4f);
        canChangeLane = true;
    }
}
*/