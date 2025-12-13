using UnityEngine;
using UnityEngine.InputSystem;

public class LaneSwitcher : MonoBehaviour
{
    public InputActionProperty steerLeftAction;
    public InputActionProperty steerRightAction;
    public MotorcycleController bikeController;

    void Start()
    {
        if (bikeController == null) bikeController = GetComponent<MotorcycleController>();

        steerLeftAction.action.Enable();
        steerRightAction.action.Enable();

        // Abonare la evenimente (se declanșează o singură dată la apăsare)
        steerLeftAction.action.performed += ctx => bikeController.ChangeLane(-1);
        steerRightAction.action.performed += ctx => bikeController.ChangeLane(1);
    }
}