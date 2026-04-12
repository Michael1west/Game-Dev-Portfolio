using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class DemonBrain : BehaviourTree
{
    [Header("Debug")]
    [SerializeField] private string currentState;

    public NavMeshAgent agent;
    public FirstPersonController player;
    public Transform[] restPoints;

    protected override Node SetupTree()
    {
        DemonDetection detection = GetComponent<DemonDetection>();
        IThreatTarget target = player as IThreatTarget;

        Node root = new SelectorNode(new List<Node>
        {
            //Hunt
            new SequenceNode(new List<Node>
            {
                new CheckHuntingNode(detection),
                new DemonHuntNode(agent, target, this)
            }),

            //Alert
            new SequenceNode(new List<Node>
            {
                new CheckAlertNode(detection),
                new DemonAlertNode(agent, detection, this)
            }),

            //Search
            new SequenceNode(new List<Node>
            {
                new CheckSearchNode(detection),
                new DemonSearchNode(agent, detection, this)
            }),

            //Roam
            new SequenceNode(new List<Node>
            {
                new CheckRoamNode(detection),
                new DemonRoamNode(agent, detection, this)
            }),

            //Retreat
             new SequenceNode(new List<Node>
            {
                new CheckRetreatNode(detection),
                new DemonRetreatNode(agent, detection, restPoints, this)
            }),

            //Wander
            new DemonWanderNode(agent, this)

        });

        return root;
    }

    public void SetCurrentState(string state)
    {
        currentState = state;
    }
}
