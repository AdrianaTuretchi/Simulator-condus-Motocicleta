
using UnityEngine;
using TMPro; // <--- OBLIGATORIU pentru TextMeshPr
using UnityEngine.InputSystem; // Adaugă această linie sus!

public class KeyboardMotorcycle : MonoBehaviour
{
    public float[] lanes = { -3f, 0f, 3f };
    public TextMeshProUGUI textViteza;
    public TextMeshProUGUI textGear;
    private int currentLane = 1;
    public float laneChangeSpeed = 15f;
    public float moveSpeed = 15f;
    private bool canChange = true;
    public int currentGear = 1;
    public int maxGear = 5;
    public float currentViteza = 0f;
    public int currentSpeed = 0;
    public float maxViteza = 22f;
    private int[] maxGearSpeeds = { 22, 42, 62, 82, 120 };
    private int[] minGearSpeeds = { 0, 22, 42, 62, 82 };
    private int[] idle = { 10, 30, 52, 72, 90 };
    public int gear = 1;
    void Update()
    {
        if (canChange)
        {
            if (Keyboard.current.dKey.wasPressedThisFrame && currentLane < 2) 
            {
                currentLane++;
                StartCoroutine(Cooldown());
            }
            else if (Keyboard.current.aKey.wasPressedThisFrame && currentLane > 0)
            {
                currentLane--;
                StartCoroutine(Cooldown());
            }
        }
        
        if(Keyboard.current.shiftKey.wasPressedThisFrame && gear < maxGear && currentViteza >= idle[gear - 1])
        {
            gear++;
            maxViteza = maxGearSpeeds[gear - 1];
        }
        else if (Keyboard.current.ctrlKey.wasPressedThisFrame && gear > 1 && currentViteza <= idle[gear - 1])
        {
            gear--;
            maxViteza = maxGearSpeeds[gear - 1];
        }
        textGear.text = "Gear: " + gear.ToString();    
        if (Keyboard.current.wKey.isPressed && currentViteza < maxGearSpeeds[gear - 1]) 
            currentViteza += 0.3f;
        else if (currentViteza > idle[gear - 1])
                    currentViteza -= 0.05f;
        
        if (Keyboard.current.sKey.isPressed && currentViteza > 0 && currentViteza > minGearSpeeds[gear - 1]) currentViteza -= 0.3f;

        currentSpeed = (int)currentViteza;
        textViteza.text = currentSpeed.ToString("F0") + " km/h";

        transform.Translate(Vector3.forward * currentViteza * Time.deltaTime);
        Vector3 targetPos = new Vector3(lanes[currentLane], transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * laneChangeSpeed);
    }

    System.Collections.IEnumerator Cooldown()
    {
        canChange = false;
        yield return new WaitForSeconds(0.2f);
        canChange = true;
    }
}