using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions inputActions;

    public Vector2 LookInput { get; private set; }
    public Vector2 MoveInput { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool SlidePressed { get; private set; }
    public bool CrouchHeld { get; private set; }

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        MoveInput = inputActions.Player.Move.ReadValue<Vector2>();
        SprintHeld = inputActions.Player.Sprint.IsPressed();
        JumpPressed = inputActions.Player.Jump.WasPressedThisFrame();
        SlidePressed = inputActions.Player.Slide.WasPressedThisFrame();
        LookInput = inputActions.Player.Look.ReadValue<Vector2>();
        CrouchHeld = inputActions.Player.Crouch.IsPressed();
    }
}
