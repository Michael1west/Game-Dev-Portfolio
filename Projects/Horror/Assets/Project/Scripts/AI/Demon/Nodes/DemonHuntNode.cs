using UnityEngine;
using UnityEngine.AI;

public class DemonHuntNode : ActionNode
{
    private const float LOSLostThreshold = 4f;
    private const float KillRange = 1.5f;

    private IThreatTarget target;
    private NavMeshAgent agent;
    private DemonBrain brain;
    private DemonDetection detection;


    public DemonHuntNode(NavMeshAgent agent, IThreatTarget target, DemonBrain brain, DemonDetection detection)
    {
        this.agent = agent;
        this.target = target;
        this.brain = brain;
        this.detection = detection;
    }

    public override NodeState Evaluate()
    {
        // Kill transition — player within bite range
        if (Vector3.Distance(agent.transform.position, target.targetPosition) <= KillRange)
        {
            brain.SetState(DemonState.Killing);
            State = NodeState.Failure;
            return State;
        }

        // Escape transition — lost LOS for too long
        if (detection.TimeSinceLOSLost >= LOSLostThreshold)
        {
            brain.SetState(DemonState.Investigating, detection.LastKnownPosition);
            State = NodeState.Failure;
            return State;
        }

        // Hunt behaviour
        agent.speed = 5f;
        agent.SetDestination(target.targetPosition);
        State = NodeState.Running;
        return State;
    }
}
