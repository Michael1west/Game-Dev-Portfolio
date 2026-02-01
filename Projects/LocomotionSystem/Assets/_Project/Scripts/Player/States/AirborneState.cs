using UnityEngine;

public class AirborneState : BaseState
{
    private float jumpBufferCounter;

    public AirborneState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.SetAnimatorBool("IsAirborne", true);
    }

    public override void Exit()
    {
        stateMachine.SetAnimatorBool("IsAirborne", false);
    }

    public override void Update()
    {
        jumpBufferCounter -= Time.deltaTime;

        if (stateMachine.Rb.linearVelocity.y < -2f)
        {
            if (stateMachine.DetectLedge(out Vector3 grabPos, out Vector3 wallNormal))
            {
                stateMachine.ChangeState(new HangingState(stateMachine, grabPos, wallNormal));
                return;
            }
        }

        if (stateMachine.PlayerInput.JumpPressed)
        {
            jumpBufferCounter = stateMachine.JumpBufferTime;
        }

        if (stateMachine.GroundDetector.IsGrounded && jumpBufferCounter > 0)
        {
            Vector3 velocity = stateMachine.Rb.linearVelocity;
            stateMachine.Rb.linearVelocity = new Vector3(velocity.x, stateMachine.JumpForce, velocity.z);
            stateMachine.ChangeState(new AirborneState(stateMachine));
            return;
        }

        if (stateMachine.GroundDetector.IsGrounded && jumpBufferCounter <= 0)
        {
            stateMachine.SetAnimatorTrigger("Land");
            stateMachine.ChangeState(new GroundedState(stateMachine));
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
        stateMachine.Animator.SetFloat("Speed", horizontalVelocity.magnitude, 0.15f, Time.fixedDeltaTime);
    }
}