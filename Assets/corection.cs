// RestrictMovementXR.cs (Atașat la XR Origin)
using UnityEngine;

public class RestrictMovementXR : MonoBehaviour
{
    [Header("Poziția Scaunului (față de ROOT)")]
    public float targetLocalY = 0.5f; 
    public float targetLocalZ = 0f;
    private Vector3 desiredLocalPosition;

    void Awake()
    {
        // Poziția locală dorită față de părinte (Motorcycle_ROOT)
        desiredLocalPosition = new Vector3(0f, targetLocalY, targetLocalZ);
    }
    
    // LATEUPDATE: Se execută după ce VR-ul a aplicat tracking-ul.
    void LateUpdate()
    {
        // Forțează poziția locală. Anulează mișcarea fizică a corpului.
        transform.localPosition = desiredLocalPosition;
    }
}
/*
// Script: RestrictMovementXR.cs
using UnityEngine;

public class RestrictMovementXR : MonoBehaviour
{
    // Aceste valori vor stabili înălțimea scaunului față de 'moto'
    public float targetLocalY = 0.5f;
    public float targetLocalZ = 0f;
    private Vector3 desiredLocalPosition;

    void Awake()
    {
        desiredLocalPosition = new Vector3(0f, targetLocalY, targetLocalZ);
    }
    
    void LateUpdate()
    {
        // Forțează poziția LOCALĂ a XR Rig-ului, anulând mișcarea fizică
        transform.localPosition = desiredLocalPosition;
    }
}*/
/*using UnityEngine;

public class RestrictMovement : MonoBehaviour
{
    // Stocăm poziția locală dorită (de obicei, Vector3.zero dacă e copil direct al motos)
    private readonly Vector3 desiredLocalPosition = Vector3.zero;

    void LateUpdate()
    {
        // 1. Forțează poziția locală la zero
        // Acest lucru anulează mișcările de translație (x, y, z) preluate de Rig.
        transform.localPosition = desiredLocalPosition;

        // 2. Opțional: Ajustează Y-ul (înălțimea)
        // Dacă motocicleta nu are un scaun la (0,0,0) în raport cu Rig-ul
        // poți folosi: transform.localPosition = new Vector3(0, inaltimea_scaunului, 0); 
    }
}*/
/*using UnityEngine;

public class corection : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/