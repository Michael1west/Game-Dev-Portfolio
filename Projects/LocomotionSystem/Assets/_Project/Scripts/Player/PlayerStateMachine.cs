using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    // Components
    public Rigidbody Rb { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public GroundDetector GroundDetector { get; private set; }
    public Transform CameraTarget;
    public CapsuleCollider PlayerCollider;

    [SerializeField] private Animator animator;
    public Animator Animator => animator;

    // Settings Exposed in editor
    [Header("Movement")]
    public float MoveSpeed = 5f;
    public float SprintMultiplier = 2f;
    public float RotationSpeed = 10f;

    [Header("Jumping")]
    public float JumpForce = 12f;
    public float CoyoteTime = 0.1f;
    public float JumpBufferTime = 0.1f;

    [Header("Sliding")]
    public float SlideSpeed = 14f;
    public float SlideEndSpeed = 10f;
    public float SlideDeceleration = 15f;
    public float SlideDuration = 0.8f;
    public float SlideSpeedThreshold = 9f;
    public float SlideHeight = 0.5f;
    public float SlideCenter = 0.25f;

    [Header("Crouching")]
    public float CrouchSpeedMultiplier = 0.6f;
    public float CrouchHeight = 0.5f;
    public float CrouchCenter = 0.25f;

    [Header("Hanging")]
    public float ShimmySpeed = 2f;
    public float CornerSpeed = 0.3f;

    [Header("Model")]
    public Transform ModelTransform;
    public float CrouchModelOffset = 0.3f;
    public float SlideModelOffset = 0.2f;
    public float HangModelOffsetY = 0f;
    public float HangModelOffsetZ = 0f;

    [Header("Detection")]
    public LayerMask GrabbableLayer;

    // State Management
    public BaseState CurrentState { get; private set; }

    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        PlayerInput = GetComponent<PlayerInput>();
        GroundDetector = GetComponent<GroundDetector>();

        ChangeState(new GroundedState(this));
    }

    void Update()
    {
        CurrentState?.Update();
    }

    void FixedUpdate()
    {
        CurrentState?.FixedUpdate();
    }

    public void ChangeState(BaseState newState)
    {
        CurrentState?.Exit();
        Debug.Log(CurrentState?.GetType().Name + " -> " + newState.GetType().Name);
        CurrentState = newState;
        CurrentState.Enter();
    }

    //Ledge detection
    public bool DetectLedge(out Vector3 grabPosition, out Vector3 wallNormal)
    {
        grabPosition = Vector3.zero;
        wallNormal = Vector3.zero;

        Vector3 rayOrigin = transform.position + (transform.forward * 0.75f) + Vector3.up * 2f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hitInfo, 3f, GrabbableLayer))
        {
            Debug.Log("Ledge detect hit: " + hitInfo.collider.name + " at height: " + (hitInfo.point.y - transform.position.y));
            float ledgeHeight = hitInfo.point.y - transform.position.y;

            if (ledgeHeight >= -1f && ledgeHeight <= 2.5f)
            {
                Vector3 secondOrigin = transform.position + Vector3.up * ledgeHeight;

                if (Physics.Raycast(secondOrigin, transform.forward, out RaycastHit wallHit, 1.5f, GrabbableLayer))
                {
                    grabPosition = wallHit.point;
                    wallNormal = wallHit.normal;
                    return true;
                }
            }
        }

        return false;
    }

    public void SetModelOffset(float yOffset, float zOffset = 0f)
    {
        if (ModelTransform != null)
        {
            ModelTransform.localPosition = new Vector3(0f, yOffset, zOffset);
        }
    }

    public void SetAnimatorBool(string parameter, bool value)
    {
        if (animator != null)
        {
            animator.SetBool(parameter, value);
        }
    }

    public void SetAnimatorFloat(string parameter, float value)
    {
        if (animator != null)
        {
            animator.SetFloat(parameter, value);
        }
    }

    public void SetAnimatorTrigger(string parameter)
    {
        if (animator != null)
        {
            animator.SetTrigger(parameter);
        }
    }
}