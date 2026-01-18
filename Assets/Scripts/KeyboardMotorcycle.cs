

using UnityEngine;

using UnityEngine.InputSystem; // Adaugă această linie sus!

public class KeyboardMotorcycle : MonoBehaviour
{
    public float[] lanes = { -3f, 0f, 3f };
    private int currentLane = 1;
    public float laneChangeSpeed = 15f;
    public float moveSpeed = 15f;
    private bool canChange = true;

    void Update()
    {
        // --- LOGICA NOUA PENTRU TASTE ---
        if (canChange)
        {
            // Verificăm dacă tasta D a fost apăsată în acest cadru
            if (Keyboard.current.dKey.wasPressedThisFrame && currentLane < 2) 
            {
                currentLane++;
                StartCoroutine(Cooldown());
            }
            // Verificăm dacă tasta A a fost apăsată
            else if (Keyboard.current.aKey.wasPressedThisFrame && currentLane > 0)
            {
                currentLane--;
                StartCoroutine(Cooldown());
            }
        }

        float currentViteza = moveSpeed;
        // Verificăm dacă tastele sunt ținute apăsate
        if (Keyboard.current.wKey.isPressed) currentViteza *= 2;
        if (Keyboard.current.sKey.isPressed) currentViteza /= 2;

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
/*
using UnityEngine;
public class KeyboardMotorcycle : MonoBehaviour
{
    public float[] lanes = { -3f, 0f, 3f };
    private int currentLane = 1;
    public float laneChangeSpeed = 15f;
    public float moveSpeed = 15f;
    private bool canChange = true;

    void Update()
    {
        // Directie
        if (canChange)
        {
            if (Input.GetKeyDown(KeyCode.D) && currentLane < 2) 
            {
                currentLane++;
                StartCoroutine(Cooldown());
            }
            else if (Input.GetKeyDown(KeyCode.A) && currentLane > 0)
            {
                currentLane--;
                StartCoroutine(Cooldown());
            }
        }

        // Viteza (W accelereaza, S franeaza)
        float currentViteza = moveSpeed;
        if (Input.GetKey(KeyCode.W)) currentViteza *= 2;
        if (Input.GetKey(KeyCode.S)) currentViteza /= 2;

        // Miscare inainte si lateral
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
*/