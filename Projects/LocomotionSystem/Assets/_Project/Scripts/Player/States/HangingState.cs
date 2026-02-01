using UnityEngine;

public class HangingState : BaseState
{
    private Vector3 grabPosition;
    private Vector3 wallNormal;

    public HangingState(PlayerStateMachine stateMachine, Vector3 grabPosition, Vector3 wallNormal) : base(stateMachine)
    {
        this.grabPosition = grabPosition;
        this.wallNormal = wallNormal;
    }

    public override void Enter()
    {
        stateMachine.Rb.isKinematic = true;
        stateMachine.transform.position = grabPosition + wallNormal * 0.5f - Vector3.up * 1.5f;
        stateMachine.transform.rotation = Quaternion.LookRotation(-wallNormal);
        stateMachine.SetAnimatorBool("IsHanging", true);
        stateMachine.SetModelOffset(stateMachine.HangModelOffsetY, stateMachine.HangModelOffsetZ);
    }

    public override void Exit()
    {
        stateMachine.Rb.isKinematic = false;
        stateMachine.SetAnimatorBool("IsHanging", false);
        stateMachine.SetModelOffset(0f, 0f);
    }

    public override void Update()
    {
        if (stateMachine.PlayerInput.JumpPressed)
        {
            stateMachine.ChangeState(new ClimbingState(stateMachine, grabPosition));
            return;
        }
        if (stateMachine.PlayerInput.CrouchHeld)
        {
            stateMachine.transform.position += wallNormal * 0.5f;
            stateMachine.Rb.isKinematic = false;
            stateMachine.Rb.linearVelocity = wallNormal * 2f;
            stateMachine.ChangeState(new AirborneState(stateMachine));
            return;
        }
    }

    public override void FixedUpdate()
    {
        float x = stateMachine.PlayerInput.MoveInput.x;

        Vector3 ledgeCheckOrigin = stateMachine.transform.position
            + stateMachine.transform.right * x * 0.5f
            + Vector3.up * 2f
            + stateMachine.transform.forward * 0.5f;

        Vector3 cornerCheckOrigin = stateMachine.transform.position
            + stateMachine.transform.right * x * 1f
            + Vector3.up * 1f
            + stateMachine.transform.forward * 0.5f;

        if (Physics.Raycast(ledgeCheckOrigin, Vector3.down, 2f))
        {
            if (Physics.Raycast(stateMachine.transform.position + Vector3.up * 0.5f, stateMachine.transform.right * x, out RaycastHit insideCornerHit, 0.3f, stateMachine.GrabbableLayer))
            {
                Vector3 newGrabPosition = new Vector3(
                    insideCornerHit.point.x,
                    stateMachine.transform.position.y + 1.5f,
                    insideCornerHit.point.z);
                stateMachine.ChangeState(new CornerState(stateMachine, newGrabPosition, insideCornerHit.normal));
                return;
            }
            stateMachine.transform.position += stateMachine.transform.right * x * stateMachine.ShimmySpeed * Time.fixedDeltaTime;
            grabPosition = stateMachine.transform.position + stateMachine.transform.forward * 0.5f + Vector3.up * 1.5f;
        }
        else
        {
            if (Physics.Raycast(cornerCheckOrigin, -stateMachine.transform.right * x, out RaycastHit hitInfo, 1.5f, stateMachine.GrabbableLayer))
            {
                Vector3 newGrabPosition = new Vector3(
                    hitInfo.point.x,
                    stateMachine.transform.position.y + 1.5f,
                    hitInfo.point.z);
                stateMachine.ChangeState(new CornerState(stateMachine, newGrabPosition, hitInfo.normal));
                return;
            }
        }

        // Update shimmy animation
        stateMachine.Animator.SetFloat("ShimmyDirection", x, 0.1f, Time.fixedDeltaTime);
    }
}