using UnityEngine;
using UnityEngine.AI;

public class DemonInvestigateNode : ActionNode
{
    private DemonDetection detection;
    private NavMeshAgent agent;
    private DemonBrain brain;

    private bool hasInitialised = false;
    private bool hasArrived = false;
    private float searchTimer = 0f;
    private const float SearchDuration = 3f;

    public DemonInvestigateNode(NavMeshAgent agent, DemonDetection detection, DemonBrain brain)
    {
        this.agent = agent;
        this.detection = detection;
        this.brain = brain;
    }

    public override NodeState Evaluate()
    {
        Debug.Log($"Investigate evaluating. LOS={detection.HasLOSToPlayer}, hasArrived={hasArrived}, searchTimer={searchTimer:F2}");

        // Transition: spotted player - Hunting
        if (detection.HasLOSToPlayer)
        {
            brain.SetState(DemonState.Hunting);
            ResetState();
            State = NodeState.Failure;
            return State;
        }

        // First frame of this investigation — set up
        if (!hasInitialised)
        {
            agent.speed = 2f;
            agent.SetDestination(brain.InvestigationTarget);
            hasInitialised = true;
            hasArrived = false;
            searchTimer = 0f;
        }

        // Arrival check
        if (!hasArrived && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            hasArrived = true;
        }

        // Search timer once arrived
        if (hasArrived)
        {
            searchTimer += Time.deltaTime;
            if (searchTimer >= SearchDuration)
            {
                brain.SetState(DemonState.Patrol);
                ResetState();
                State = NodeState.Failure;
                return State;
            }
        }

        State = NodeState.Running;
        return State;
    }

    private void ResetState()
    {
        hasInitialised = false;
        hasArrived = false;
        searchTimer = 0f;
    }
}