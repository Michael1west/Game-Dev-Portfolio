using UnityEngine;

public class ClimbingState : BaseState
{
    private Vector3 grabPosition;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private float climbTimer;
    public ClimbingState(PlayerStateMachine stateMachine, Vector3 grabPosition) : base(stateMachine)
    {
        this.grabPosition = grabPosition;
    }

    public override void Enter()
    {
        stateMachine.Rb.isKinematic = true;
        startPosition = stateMachine.transform.position;
        targetPosition = grabPosition + Vector3.up * 0.5f + (stateMachine.transform.forward * 0.5f);
        climbTimer = 0f;
        stateMachine.SetAnimatorBool("IsClimbing", true);
    }

    public override void Exit()
    {
        stateMachine.Rb.isKinematic = false;
        stateMachine.SetAnimatorBool("IsClimbing", false);
    }

    public override void FixedUpdate()
    {

    }

    public override void Update()
    {
        climbTimer += Time.deltaTime / 0.5f;


        if (climbTimer >= 1)
        {
            stateMachine.transform.position = targetPosition;
            stateMachine.ChangeState(new GroundedState(stateMachine));
            return;
        }

        stateMachine.transform.position = Vector3.Lerp(startPosition, targetPosition, climbTimer);
    }
}