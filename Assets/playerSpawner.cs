using UnityEngine;

public class playerSpawner : MonoBehaviour
{public Transform startPoint; 
public Transform sa;  // referința la obiectul StartPoint
    public GameObject xrRig;
    public GameObject moto;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        xrRig.transform.position = sa.position;
        xrRig.transform.rotation = startPoint.rotation;
        moto.transform.position = startPoint.position;
        moto.transform.rotation = startPoint.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
