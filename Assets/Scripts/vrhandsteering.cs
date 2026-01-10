using UnityEngine;

public class VRHandSteering : MonoBehaviour
{
    public MotorcycleController bikeController;
    
    [Header("VR Controllers")]
    public Transform leftHand;  // Trage aici Left Hand Controller din VR Rig
    public Transform rightHand; // Trage aici Right Hand Controller din VR Rig

    [Header("Settings")]
    public float tiltThreshold = 0.15f; // Cât de mult trebuie să înclini mâinile (metri)
    private bool canChangeLane = true;  // Previne schimbarea multiplă a benzilor la o singură înclinare

    void Update()
    {
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