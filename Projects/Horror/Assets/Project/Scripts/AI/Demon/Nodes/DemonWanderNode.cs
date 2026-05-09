using UnityEngine;
using UnityEngine.AI;

public class DemonWanderNode : ActionNode
{
    private NavMeshAgent agent;
    private DemonBrain brain;
    private float wanderRadius = 12f;
    private float maxPathLength = 10f;
    private float minWaitTime = 3f;
    private float maxWaitTime = 8f;
    private float waitCounter = 0f;
    private bool isWaiting = false;

    private DemonDetection detection;

    public DemonWanderNode(NavMeshAgent agent, DemonBrain brain, DemonDetection detection)
    {
        this.agent = agent;
        this.brain = brain;
        this.detection = detection;
    }

    public override NodeState Evaluate()
    {
        // Only promote from Patrol — if the brain has already moved to a higher state this frame, bail
        if (brain.CurrentState != DemonState.Patrol)
        {
            State = NodeState.Failure;
            return State;
        }

        if (detection.Suspicion >= detection.SuspicionThreshold)
        {
            brain.SetState(DemonState.Investigating, detection.LastKnownPosition);
            State = NodeState.Failure;
            return State;
        }

        this.agent.speed = 1f;

        if (isWaiting)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0f)
            {
                isWaiting = false;
                agent.SetDestination(GetValidPoint());
            }
            State = NodeState.Running;
            return State;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitCounter = Random.Range(minWaitTime, maxWaitTime);
            isWaiting = true;
        }

        State = NodeState.Running;
        return State;
    }

    private Vector3 GetValidPoint()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 candidate = GetRandomPoint();
            if (IsPathAcceptable(candidate))
            {
                return candidate;
            }
        }
        return agent.transform.position;
    }

    private bool IsPathAcceptable(Vector3 destination)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(destination, path);

        if (path.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }

        float length = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }

        return length <= maxPathLength;
    }

    private Vector3 GetRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += agent.transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        return hit.position;
    }
}
