// Nume: RestrictMovement.cs (Atașat la obiectul 'xrRig' - Copilul lui 'moto')

using UnityEngine;

public class Restrict : MonoBehaviour
{
    [Header("Setări Poziție Scaun")]
    public float targetLocalY = 0.5f; 
    public float targetLocalZ = 0f;
    private Vector3 desiredLocalPosition;

    void Awake()
    {
        // Poziția locală dorită față de părinte (moto)
        desiredLocalPosition = new Vector3(0f, targetLocalY, targetLocalZ);
    }

    void LateUpdate()
    {
        // Forțează poziția locală. Aceasta anulează mișcarea ta fizică,
        // dar nu afectează mișcarea moștenită de la părinte (moto).
        transform.localPosition = desiredLocalPosition;
    }
}
/*using UnityEngine;

public class playerSpawner : MonoBehaviour
{   public Transform startPoint; 
    public Transform sa;  // referința la obiectul StartPoint
    public GameObject xrRig;
    public GameObject moto;
    public float targetLocalY = 0.5f; // Înălțimea la care să stai pe scaun
    public float targetLocalZ = 0f;
    private Vector3 desiredLocalPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xrRig.transform.position = sa.position;
        xrRig.transform.rotation = startPoint.rotation;
        moto.transform.position = startPoint.position;
        moto.transform.rotation = startPoint.rotation;
    }
    void Awake()
    {
        // Setăm poziția locală dorită inițial
        desiredLocalPosition = new Vector3(0f, targetLocalY, targetLocalZ);
    }
    void LateUpdate()
    {
        // Forțează poziția locală (translația) să fie cea dorită,
        // anulând orice mișcare fizică laterală sau înainte/înapoi a capului.
        transform.localPosition = desiredLocalPosition;
        
        // Rotația (privirea) este lăsată liberă și funcționează normal.
    }
    // Update is called once per frame
    //void Update()
    //{
        // transform.localPosition = desiredLocalPosition;
   // }
}
*/