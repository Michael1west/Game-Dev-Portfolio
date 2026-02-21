using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Preferences")]
    [SerializeField] private Transform cameraTransform;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 7f;

    [Header("Look")]
    [SerializeField] private float lookSensitivity = 0.1f;
    [SerializeField] private float verticalLookLimit = 85f;

    [Header("Crouching")]
    [SerializeField] private float crouchHeight = 1f;
    [SerializeField] private float cameraCrouchHeight = 0.8f;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private CapsuleCollider playerCapsule;
    private LayerMask crouchCheckLayer;
    private float verticalRotation = 0f;
    private float playerHeight = 2f;
    private float cameraHeight = 1.6f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerCapsule = GetComponent<CapsuleCollider>();
        crouchCheckLayer = ~LayerMask.GetMask("Player");

        // Lock cursor to center screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
    }

    void FixedUpdate()
    {
        HandleMovement();

        if (playerInput.CrouchHeld)
        {
            playerCapsule.height = crouchHeight;
            playerCapsule.center = new Vector3(0f, 0.5f, 0f);
            cameraTransform.localPosition = new Vector3(0f, cameraCrouchHeight, 0f);
        }
        else
        {
            if (!Physics.SphereCast(transform.position, playerCapsule.radius, Vector3.up, out _, 1f, crouchCheckLayer))
            {
                playerCapsule.height = playerHeight;
                playerCapsule.center = new Vector3(0f, 1f, 0f);
                cameraTransform.localPosition = new Vector3(0f, cameraHeight, 0f);
            }
        }
    }

    private void HandleLook()
    {
        Vector2 lookInput = playerInput.LookInput;

        // Horizontal rotation - rotate the whole player
        transform.Rotate(Vector3.up, lookInput.x * lookSensitivity);

        // Vertical rotation - rotate the camera
        verticalRotation -= lookInput.y * lookSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

    }

    private void HandleMovement()
    {
        Vector2 moveInput = playerInput.MoveInput;
        float currentSpeed = playerInput.SprintHeld ? sprintSpeed : walkSpeed;

        // Movement relative to where the player is facing
        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;

        rb.linearVelocity = new Vector3(
            moveDirection.x * currentSpeed,
            rb.linearVelocity.y,
            moveDirection.z * currentSpeed
            );
    }
}
