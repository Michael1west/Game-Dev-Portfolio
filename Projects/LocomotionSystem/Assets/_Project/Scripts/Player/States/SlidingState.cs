using UnityEngine;

public class SlidingState : BaseState
{
    private Vector3 slideDirection;
    private float slideCounter;
    private float originalHeight;
    private float originalCenter;
    private float currentSlideSpeed;

    public SlidingState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter()
    {
        // Store originals FIRST
        originalHeight = stateMachine.PlayerCollider.height;
        originalCenter = stateMachine.PlayerCollider.center.y;

        // Then modify
        stateMachine.PlayerCollider.height = stateMachine.SlideHeight;
        stateMachine.PlayerCollider.center = new Vector3(0f, stateMachine.SlideCenter, 0f);

        // Set initial values
        currentSlideSpeed = stateMachine.SlideSpeed;
        slideCounter = stateMachine.SlideDuration;

        // Capture direction
        Vector3 horizontalVelocity = stateMachine.Rb.linearVelocity;
        horizontalVelocity.y = 0f;
        slideDirection = horizontalVelocity.normalized;
        stateMachine.SetAnimatorBool("IsSliding", true);
        stateMachine.SetAnimatorTrigger("Slide");
        stateMachine.SetModelOffset(stateMachine.SlideModelOffset, 0f);
    }

    public override void Exit()
    {
        stateMachine.PlayerCollider.height = originalHeight;
        stateMachine.PlayerCollider.center = new Vector3(0f, originalCenter, 0f);
        stateMachine.SetAnimatorBool("IsSliding", false);
        stateMachine.SetModelOffset(0f, 0f);
    }

    public override void Update()
    {
        // Just count down and check for exit
        slideCounter -= Time.deltaTime;

        if (slideCounter <= 0)
        {
            stateMachine.ChangeState(new GroundedState(stateMachine));
            return;
        }
    }

    public override void FixedUpdate()
    {
        // Decelerate and apply velocity - no conditions needed
        currentSlideSpeed = Mathf.MoveTowards(currentSlideSpeed, stateMachine.SlideEndSpeed, stateMachine.SlideDeceleration * Time.fixedDeltaTime);
        Vector3 slideVelocity = slideDirection * currentSlideSpeed;
        stateMachine.Rb.linearVelocity = new Vector3(slideVelocity.x, stateMachine.Rb.linearVelocity.y, slideVelocity.z);
    }
}
