using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement { get; private set; }
    public static bool IsFire { get; private set; } = false;

    private InputAction moveAction;
    private InputAction fireAction;


    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        fireAction = InputSystem.actions.FindAction("Attack");
    }
    void Update()
    {
        Movement = moveAction.ReadValue<Vector2>();
        IsFire = fireAction.WasPressedThisFrame();
    }
}
