using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemonRetreatNode : ActionNode
{
    private float restDuration = 5f;
    private float restTimer = 0f;
    private Transform[] restPoints;
    private bool isInitialised = false;

    private NavMeshAgent agent;
    private DemonDetection detection;
    private DemonBrain brain;

    public DemonRetreatNode(NavMeshAgent agent, DemonDetection detection, Transform[] restPoints, DemonBrain brain)
    {
        this.agent = agent;
        this.detection = detection;
        this.restPoints = restPoints;
        this.brain = brain;
    }
    public override NodeState Evaluate()
    {
        Transform nearest = null;
        float shortestDistance = float.MaxValue;
        brain.SetCurrentState("Retreat");

        if (!isInitialised)
        {
            foreach (Transform point in restPoints)
            {
                float distance = Vector3.Distance(agent.transform.position, point.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearest = point;
                }
            }

            agent.SetDestination(nearest.position);
            isInitialised = true;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            restTimer += Time.deltaTime;

            if (restTimer >= restDuration)
            {
                detection.ResetRoamMemory();
                isInitialised = false;
                restTimer = 0f;
                State = NodeState.Failure;
                return State;
            }
        }

        State = NodeState.Running;
        return State;
    }
}
