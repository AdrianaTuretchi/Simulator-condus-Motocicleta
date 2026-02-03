using UnityEngine;
using TMPro;
using UnityEngine.XR;

public class OculusMotorcycle : MonoBehaviour
{
    [Header("UI & Display")]
    public TextMeshProUGUI textViteza;
    public TextMeshProUGUI textGear;

    [Header("Setari Benzi")]
    public float[] lanes = { -3f, 0f, 3f };
    private int currentLane = 1;
    public float laneChangeSpeed = 15f;
    private bool canChange = true;

    [Header("Sistem Cutie Viteze")]
    public int gear = 1;
    public int maxGear = 5;
    public float currentViteza = 0f;
    private int[] maxGearSpeeds = { 22, 42, 62, 82, 120 };
    private int[] minGearSpeeds = { 0, 22, 42, 62, 82 };
    private int[] idleThresholds = { 10, 30, 52, 72, 90 };

    [Header("Input VR")]
    public float tiltThreshold = 0.15f;
    private bool canShift = true;

    void Update()
    {
        HandleSteering();
        HandleGears();
        HandleSpeed();
        ApplyMovement();
        UpdateUI();
    }

    void HandleSteering()
    {
        if (!canChange) return;

        float vrTilt = GetVRTilt();
        // Mana dreapta sus -> Mergi la STANGA
        if (vrTilt > tiltThreshold && currentLane > 0)
        {
            currentLane--;
            StartCoroutine(LaneCooldown());
        }
        // Mana stanga sus -> Mergi la DREAPTA
        else if (vrTilt < -tiltThreshold && currentLane < 2)
        {
            currentLane++;
            StartCoroutine(LaneCooldown());
        }
    }

    void HandleGears()
    {
        if (!canShift) return;

        bool gearUpPressed = GetButtonDown(XRNode.RightHand, CommonUsages.primaryButton); // Butonul A
        bool gearDownPressed = GetButtonDown(XRNode.LeftHand, CommonUsages.primaryButton); // Butonul X

        // Gear UP
        if (gearUpPressed && gear < maxGear && currentViteza >= idleThresholds[gear - 1])
        {
            gear++;
            StartCoroutine(ShiftCooldown());
        }
        // Gear DOWN
        else if (gearDownPressed && gear > 1 && currentViteza <= idleThresholds[gear - 1])
        {
            gear--;
            StartCoroutine(ShiftCooldown());
        }
    }

    void HandleSpeed()
    {
        float accelInput = GetTriggerInput();
        float brakeInput = GetGripInput();

        // Acceleratie (W echivalent)
        if (accelInput > 0.1f && currentViteza < maxGearSpeeds[gear - 1])
        {
            currentViteza += 0.3f * accelInput;
        }
        // Decelerare naturala (Idle)
        else if (currentViteza > idleThresholds[gear - 1])
        {
            currentViteza -= 0.05f;
        }

        // Frana (S echivalent)
        if (brakeInput > 0.1f && currentViteza > minGearSpeeds[gear - 1])
        {
            currentViteza -= 0.3f * brakeInput;
        }

        // Siguranta sa nu scada sub 0
        if (currentViteza < 0) currentViteza = 0;
    }

    void ApplyMovement()
    {
        // Miscare inainte
        transform.Translate(Vector3.forward * currentViteza * Time.deltaTime);

        // Miscare laterala (Lerp intre benzi)
        Vector3 targetPos = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * laneChangeSpeed);
    }

    void UpdateUI()
    {
        if (textGear != null) textGear.text = "Gear: " + gear.ToString();
        if (textViteza != null) textViteza.text = ((int)currentViteza).ToString() + " km/h";
    }

    // --- HELPER METHODS PENTRU VR ---

    float GetVRTilt()
    {
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 lPos);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 rPos);
        return rPos.y - lPos.y;
    }

    float GetTriggerInput()
    {
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.trigger, out float val);
        return val;
    }

    float GetGripInput()
    {
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.grip, out float val);
        return val;
    }

    bool GetButtonDown(XRNode node, InputFeatureUsage<bool> button)
    {
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(button, out bool pressed);
        return pressed;
    }

    System.Collections.IEnumerator LaneCooldown()
    {
        canChange = false;
        yield return new WaitForSeconds(0.3f);
        canChange = true;
    }

    System.Collections.IEnumerator ShiftCooldown()
    {
        canShift = false;
        yield return new WaitForSeconds(0.5f); // Previne schimbarea prea rapida a vitezelor
        canShift = true;
    }
}/*using UnityEngine;
using UnityEngine.XR;

public class OculusMotorcycle : MonoBehaviour
{
    [Header("Setari Benzi")]
    public float[] lanes = { -3f, 0f, 3f };
    private int currentLane = 1;
    public float laneChangeSpeed = 12f;

    [Header("Setari Viteza")]
    public float currentSpeed = 0f;
    public float maxSpeed = 40f;
    public float acceleration = 10f;

    [Header("Setari VR")]
    public float tiltThreshold = 0.15f; // Diferenta de inaltime intre maini
    private bool canChange = true;

    void Update()
    {
        // --- VIRARE (Inclinare: Mana Dreapta sus = Stanga, Mana Stanga sus = Dreapta) ---
        float vrTilt = GetVRTilt();

        if (canChange)
        {
            // Mana dreapta mai sus ca stanga -> Mergi la STANGA
            if (vrTilt > tiltThreshold && currentLane > 0)
            {
                currentLane--;
                StartCoroutine(Cooldown());
            }
            // Mana stanga mai sus ca dreapta -> Mergi la DREAPTA
            else if (vrTilt < -tiltThreshold && currentLane < 2)
            {
                currentLane++;
                StartCoroutine(Cooldown());
            }
        }

        // --- VITEZA (Triggere pentru acceleratie, Grip pentru frana) ---
        float triggerInput = GetVRTrigger(); // Acceleratie
        float gripInput = GetVRGrip();       // Frana

        if (triggerInput > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * triggerInput * Time.deltaTime);
        }
        else if (gripInput > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, 20f * gripInput * Time.deltaTime);
        }

        // --- APLICARE MISCARE ---
        transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

        Vector3 targetPos = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * laneChangeSpeed);
    }

    float GetVRTilt()
    {
        Vector3 lPos, rPos;
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.devicePosition, out lPos);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.devicePosition, out rPos);
        return rPos.y - lPos.y;
    }

    float GetVRTrigger()
    {
        float lTrig, rTrig;
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.trigger, out lTrig);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.trigger, out rTrig);
        return (lTrig + rTrig); // Suma ambelor triggere
    }

    float GetVRGrip()
    {
        float lGrip, rGrip;
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.grip, out lGrip);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.grip, out rGrip);
        return (lGrip + rGrip); // Suma ambelor butoane Grip pentru frana
    }

    System.Collections.IEnumerator Cooldown()
    {
        canChange = false;
        yield return new WaitForSeconds(0.4f);
        canChange = true;
    }
}*/
/*using UnityEngine;
using UnityEngine.XR;

public class OculusMotorcycle : MonoBehaviour
{
    public float[] lanes = { -3f, 0f, 3f };
    private int currentLane = 1;
    public float laneChangeSpeed = 12f;
    public float moveSpeed = 15f;
    
    public float tiltThreshold = 0.15f; // Diferenta de inaltime intre maini
    private bool canChange = true;

    void Update()
    {
        // 1. Logica Viraj (Mana dreapta mai sus ca stanga = STÂNGA, conform cerintei tale)
        float vrTilt = GetVRTilt();

        if (canChange)
        {
            // Daca mana dreapta e mai sus ca stanga -> STÂNGA
            if (vrTilt > tiltThreshold && currentLane > 0)
            {
                currentLane--;
                StartCoroutine(Cooldown());
            }
            // Daca mana stanga e mai sus ca dreapta -> DREAPTA
            else if (vrTilt < -tiltThreshold && currentLane < 2)
            {
                currentLane++;
                StartCoroutine(Cooldown());
            }
        }

        // 2. Logica Acceleratie (Trigger suma/media)
        float accel = GetVRTrigger();
        float currentViteza = moveSpeed + (accel * 20f);

        // 3. Aplicare Miscare
        transform.Translate(Vector3.forward * currentViteza * Time.deltaTime);
        Vector3 targetPos = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * laneChangeSpeed);
    }

    float GetVRTilt()
    {
        Vector3 lPos, rPos;
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.devicePosition, out lPos);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.devicePosition, out rPos);
        return rPos.y - lPos.y; // Pozitie relativa Y
    }

    float GetVRTrigger()
    {
        float lTrig, rTrig;
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.trigger, out lTrig);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.trigger, out rTrig);
        return (lTrig + rTrig); // Suma triggere pentru acceleratie
    }

    System.Collections.IEnumerator Cooldown()
    {
        canChange = false;
        yield return new WaitForSeconds(0.4f);
        canChange = true;
    }
}*/