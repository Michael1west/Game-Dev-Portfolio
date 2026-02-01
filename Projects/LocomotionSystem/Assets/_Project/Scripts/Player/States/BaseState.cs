using UnityEngine;

public abstract class BaseState
{
    protected PlayerStateMachine stateMachine;

    public BaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();

}
