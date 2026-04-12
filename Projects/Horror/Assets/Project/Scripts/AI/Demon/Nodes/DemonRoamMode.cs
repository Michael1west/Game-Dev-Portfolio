using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class DemonRoamNode : ActionNode
{
    private float searchRadius = 20f;
    private float maxPathLength = 30f;
    private List<Vector3> searchPoints = new List<Vector3>();
    private int currentPointIndex = 0;
    private bool isInitialised = false;

    private NavMeshAgent agent;
    private DemonDetection detection;
    private DemonBrain brain;

    public DemonRoamNode(NavMeshAgent agent, DemonDetection detection, DemonBrain brain)
    {
        this.agent = agent;
        this.detection = detection;
        this.brain = brain;
    }

    public override NodeState Evaluate()
    {
        brain.SetCurrentState("Roam");
        agent.speed = 2f;

        if (!isInitialised)
        {
            GenerateSearchPoints();

            if (searchPoints.Count == 0)
            {
                detection.SetHasRoamed();
                detection.ResetSearchMemory();
                State = NodeState.Failure;
                return State;
            }

            agent.SetDestination(searchPoints[0]);
            isInitialised = true;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentPointIndex++;

            if (currentPointIndex >= searchPoints.Count)
            {
                detection.SetHasRoamed();
                detection.ResetSearchMemory();
                State = NodeState.Failure;
                isInitialised = false;
                return State;
            }

            agent.SetDestination(searchPoints[currentPointIndex]);
        }

        State = NodeState.Running;
        return State;
    }

    private void GenerateSearchPoints()
    {
        searchPoints.Clear();
        currentPointIndex = 0;
        float minPointDistance = 3f;

        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * searchRadius;
            randomDirection += detection.LastKnownPosition;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, searchRadius, 1) && IsPathAcceptable(hit.position))
            {
                bool tooClose = false;

                foreach (Vector3 existingPoint in searchPoints)
                {
                    if (Vector3.Distance(hit.position, existingPoint) < minPointDistance)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    searchPoints.Add(hit.position);
                }

                if (searchPoints.Count >= 3)
                {
                    break;
                }
            }
        }
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
}