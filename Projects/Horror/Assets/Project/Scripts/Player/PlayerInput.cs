using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions inputActions;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool SprintHeld { get; private set; }
    public bool CrouchHeld { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool UseItemPressed { get; private set; }
    public bool EquipSlot1Pressed { get; private set; }
    public bool EquipSlot2Pressed { get; private set; }
    public bool EquipSlot3Pressed { get; private set; }
    public bool EquipSlot4Pressed { get; private set; }
    public bool DropItemPressed { get; private set; }

    void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        inputActions.Player.Enable();
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Update()
    {
        MoveInput = inputActions.Player.Move.ReadValue<Vector2>();
        LookInput = inputActions.Player.Look.ReadValue<Vector2>();
        SprintHeld = inputActions.Player.Sprint.IsPressed();
        CrouchHeld = inputActions.Player.Crouch.IsPressed();
        InteractPressed = inputActions.Player.Interact.WasPressedThisFrame();
        UseItemPressed = inputActions.Player.UseItem.WasPressedThisFrame();

        EquipSlot1Pressed = inputActions.Player.EquipSlot1.WasPressedThisFrame();
        EquipSlot2Pressed = inputActions.Player.EquipSlot2.WasPressedThisFrame();
        EquipSlot3Pressed = inputActions.Player.EquipSlot3.WasPressedThisFrame();
        EquipSlot4Pressed = inputActions.Player.EquipSlot4.WasPressedThisFrame();
        DropItemPressed = inputActions.Player.DropItem.WasPressedThisFrame();
    }
}
