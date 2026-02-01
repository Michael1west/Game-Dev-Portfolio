using UnityEngine;

public class CrouchingState : BaseState
{
    private float OriginalHeight;
    private float OriginalCenter;
    public CrouchingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        OriginalHeight = stateMachine.PlayerCollider.height;
        OriginalCenter = stateMachine.PlayerCollider.center.y;
        stateMachine.PlayerCollider.height = stateMachine.CrouchHeight;
        stateMachine.PlayerCollider.center = new Vector3(0f, stateMachine.CrouchCenter, 0f);
        stateMachine.SetAnimatorBool("IsCrouching", true);
        stateMachine.SetModelOffset(stateMachine.CrouchModelOffset, 0f);
    }

    public override void Exit()
    {
        stateMachine.PlayerCollider.height = OriginalHeight;
        stateMachine.PlayerCollider.center = new Vector3(0f, OriginalCenter, 0f);
        stateMachine.SetAnimatorBool("IsCrouching", false);
        stateMachine.SetModelOffset(0f, 0f);
    }

    public override void Update()
    {
        if (!stateMachine.PlayerInput.CrouchHeld)
        {
            int layerMask = ~LayerMask.GetMask("Player");

            if (!Physics.SphereCast(stateMachine.transform.position,
                                    stateMachine.PlayerCollider.radius,
                                    Vector3.up,
                                    out _,
                                    OriginalHeight,
                                    layerMask))
            {
                stateMachine.ChangeState(new GroundedState(stateMachine));
                return;
            }
        }
    }

    public override void FixedUpdate()
    {
        // Get input
        float x = stateMachine.PlayerInput.MoveInput.x;
        float z = stateMachine.PlayerInput.MoveInput.y;
        float currentSpeed = stateMachine.MoveSpeed * stateMachine.CrouchSpeedMultiplier;
        
        // Get camera directions, flattened to ground
        Vector3 forward = stateMachine.CameraTarget.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = stateMachine.CameraTarget.right;
        right.y = 0f;
        right.Normalize();

        // Build movement from camera's perspective
        Vector3 moveDirection = (forward * z + right * x).normalized;

        // Apply velocity
        stateMachine.Rb.linearVelocity = new Vector3(
            moveDirection.x * currentSpeed,
            stateMachine.Rb.linearVelocity.y,
            moveDirection.z * currentSpeed
        );

        // Rotate player to face movement direction
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            stateMachine.transform.rotation = Quaternion.Slerp(
                stateMachine.transform.rotation,
                targetRotation,
                stateMachine.RotationSpeed * Time.deltaTime
            );
        }

        Vector3 horizontalVelocity = stateMachine.Rb.linearVelocity;
        horizontalVelocity.y = 0f;
        float targetSpeed = horizontalVelocity.magnitude;
        stateMachine.Animator.SetFloat("Speed", targetSpeed, 0.15f, Time.fixedDeltaTime);
    }
}
