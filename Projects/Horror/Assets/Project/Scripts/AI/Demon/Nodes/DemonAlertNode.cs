using UnityEngine;
using UnityEngine.AI;

public class DemonAlertNode : ActionNode
{
    private DemonDetection detection;
    private NavMeshAgent agent;
    private DemonBrain brain;


    public DemonAlertNode(NavMeshAgent agent, DemonDetection detection, DemonBrain brain)
    {
        this.agent = agent;
        this.detection = detection;
        this.brain = brain;
    }

    public override NodeState Evaluate()
    {
        this.agent.speed = 2f;
        agent.SetDestination(detection.LastKnownPosition);
        State = NodeState.Running;
        brain.SetCurrentState("Alert");
        return State;
    }
}
