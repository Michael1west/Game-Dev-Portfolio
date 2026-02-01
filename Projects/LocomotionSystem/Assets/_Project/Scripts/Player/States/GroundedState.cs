using UnityEngine;

public class GroundedState : BaseState
{
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    public GroundedState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        coyoteTimeCounter = stateMachine.CoyoteTime;
        stateMachine.SetModelOffset(0f, 0f);

    }

    public override void Exit()
    {

    }

    public override void Update()
    {
        jumpBufferCounter -= Time.deltaTime;

        if (!stateMachine.GroundDetector.IsGrounded)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        else
        {
            coyoteTimeCounter = stateMachine.CoyoteTime;
        }

        if (stateMachine.PlayerInput.JumpPressed)
        {
            if (stateMachine.DetectLedge(out Vector3 grabPos, out Vector3 wallNormal))
            {
                stateMachine.ChangeState(new HangingState(stateMachine, grabPos, wallNormal));
                return;
            }

            jumpBufferCounter = stateMachine.JumpBufferTime;
        }

        if (coyoteTimeCounter <= 0)
        {
            stateMachine.ChangeState(new AirborneState(stateMachine));
            return;
        }

        if (stateMachine.PlayerInput.SlidePressed &&
            stateMachine.PlayerInput.SprintHeld &&
            stateMachine.Rb.linearVelocity.magnitude >= stateMachine.SlideSpeedThreshold)
        {
            stateMachine.ChangeState(new SlidingState(stateMachine));
            return;
        }

        if (stateMachine.PlayerInput.CrouchHeld)
        {
            stateMachine.ChangeState(new CrouchingState(stateMachine));
            return;
        }

        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            Vector3 velocity = stateMachine.Rb.linearVelocity;
            stateMachine.Rb.linearVelocity = new Vector3(velocity.x, stateMachine.JumpForce, velocity.z);
            stateMachine.SetAnimatorTrigger("Jump");
            stateMachine.ChangeState(new AirborneState(stateMachine));
            return;
        }
    }

    public override void FixedUpdate()
    {
        float x = stateMachine.PlayerInput.MoveInput.x;
        float z = stateMachine.PlayerInput.MoveInput.y;
        float currentSpeed = stateMachine.MoveSpeed;

        if (stateMachine.PlayerInput.SprintHeld)
        {
            currentSpeed = stateMachine.MoveSpeed * stateMachine.SprintMultiplier;
        }

        Vector3 forward = stateMachine.CameraTarget.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 right = stateMachine.CameraTarget.right;
        right.y = 0f;
        right.Normalize();

        Vector3 moveDirection = (forward * z + right * x).normalized;

        stateMachine.Rb.linearVelocity = new Vector3(
            moveDirection.x * currentSpeed,
            stateMachine.Rb.linearVelocity.y,
            moveDirection.z * currentSpeed
        );

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