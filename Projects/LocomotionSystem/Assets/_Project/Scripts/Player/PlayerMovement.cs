using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    private Rigidbody rb;
    private GroundDetector groundDetector;
    private PlayerSlide playerSlide;
    private PlayerInput playerInput;
    private bool jumpRequested;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundDetector = GetComponent<GroundDetector>();
        playerSlide = GetComponent<PlayerSlide>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // Jump buffer counts down every frame
        jumpBufferCounter -= Time.deltaTime;

        // Coyote Time
        if (!groundDetector.IsGrounded)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        else
        {
            coyoteTimeCounter = coyoteTime;
        }

       
        if (playerInput.JumpPressed)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        

        // Jump check
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            jumpRequested = true;
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
    }

    void FixedUpdate()
    {
        float x = playerInput.MoveInput.x;
        float z = playerInput.MoveInput.y;

        float currentSpeed = moveSpeed;

       

        if (playerInput.SprintHeld)
        {
            currentSpeed = moveSpeed * sprintMultiplier;
        }
  
        

        if (!playerSlide.isSliding)
        {
            // Get camera directions, flattened to ground
            Vector3 forward = cameraTarget.forward;
            forward.y = 0f;
            forward.Normalize();

            Vector3 right = cameraTarget.right;
            right.y = 0f;
            right.Normalize();

            // Build movement from camera's perspective
            Vector3 moveDirection = (forward * z + right * x).normalized;

            // Apply velocity
            rb.linearVelocity = new Vector3(
                moveDirection.x * currentSpeed,
                rb.linearVelocity.y,
                moveDirection.z * currentSpeed
            );

            // Rotate player to face movement direction
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }

            // Jumping
            if (jumpRequested)
            {
                rb.linearVelocity = new Vector3(moveDirection.x * currentSpeed, jumpForce, moveDirection.z * currentSpeed);
                jumpRequested = false;
            }
        }
    }
}