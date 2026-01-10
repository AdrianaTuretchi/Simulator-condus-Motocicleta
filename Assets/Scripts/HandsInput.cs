using UnityEngine;
using UnityEngine.InputSystem;

public class HandsInput : MonoBehaviour
{
    public InputAction grabInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grabInput.Enable();
       // grabInput.performed += OnGrabPerformed;
       // grabInput.canceled += OnGrabCanceled;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
