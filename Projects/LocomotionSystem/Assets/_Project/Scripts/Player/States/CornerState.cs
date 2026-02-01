using UnityEngine;

public class CornerState : BaseState
{
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Vector3 wallNormal;
    private float timer;
    private Vector3 grabPosition;
    private Vector3 startPosition;
    private Quaternion startRotation;

    public CornerState(PlayerStateMachine stateMachine, Vector3 grabPosition, Vector3 wallNormal) : base(stateMachine)
    {
        this.grabPosition = grabPosition;
        this.wallNormal = wallNormal;
    }

    public override void Enter()
    {
        stateMachine.Rb.isKinematic = true;
        startPosition = stateMachine.transform.position;
        startRotation = stateMachine.transform.rotation;

        targetPosition = grabPosition + wallNormal * 0.5f - Vector3.up * 1.5f;
        targetRotation = Quaternion.LookRotation(-wallNormal);
        timer = 0f;
        stateMachine.SetAnimatorBool("IsHanging", true);
        stateMachine.SetModelOffset(stateMachine.HangModelOffsetY, stateMachine.HangModelOffsetZ);
    }

    public override void Exit()
    {
        stateMachine.Rb.isKinematic = false;

    }

    public override void Update()
    {
        timer += Time.deltaTime / stateMachine.CornerSpeed;

        if (timer >= 1)
        {
            stateMachine.transform.position = targetPosition;
            stateMachine.transform.rotation = targetRotation;
            stateMachine.ChangeState(new HangingState(stateMachine, grabPosition, wallNormal));
            return;
        }

        stateMachine.transform.position = Vector3.Lerp(startPosition, targetPosition, timer);
        stateMachine.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timer);
    }

    public override void FixedUpdate()
    {

    }
}