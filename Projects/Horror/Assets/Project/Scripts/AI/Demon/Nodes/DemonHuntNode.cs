using UnityEngine;
using UnityEngine.AI;

public class DemonHuntNode : ActionNode
{
    private IThreatTarget target;
    private NavMeshAgent agent;
    private DemonBrain brain;


    public DemonHuntNode(NavMeshAgent agent, IThreatTarget target, DemonBrain brain)
    {
        this.agent = agent;
        this.target = target;
        this.brain = brain;
    }

    public override NodeState Evaluate()
    {
        this.agent.speed = 5f;
        agent.SetDestination(target.targetPosition);
        State = NodeState.Running;
        brain.SetCurrentState("Hunt");
        return State;
    }
}
