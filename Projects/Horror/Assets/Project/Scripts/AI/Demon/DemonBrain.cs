using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class DemonBrain : BehaviourTree
{
    [Header("Debug")]
    [SerializeField] private DemonState currentState = DemonState.Patrol;

    public NavMeshAgent agent;
    public FirstPersonController player;

    private Vector3 investigationTarget;

    public DemonState CurrentState => currentState;
    public Vector3 InvestigationTarget => investigationTarget;

    protected override Node SetupTree()
    {
        DemonDetection detection = GetComponent<DemonDetection>();
        DemonAnimator demonAnimator = GetComponent<DemonAnimator>();
        IThreatTarget target = player as IThreatTarget;

        Node root = new SelectorNode(new List<Node>
        {
            // Kill — top priority, distance-based interrupt
            new SequenceNode(new List<Node>
            {
                new CheckKillNode(transform, player.transform),
                new DemonKillNode(agent, transform, player.transform, demonAnimator)
            }),

            // Hunting — sprint to player, escape if LOS lost too long
            new SequenceNode(new List<Node>
            {
                new CheckHuntingNode(this),
                new DemonHuntNode(agent, target, this, detection)
            }),

             // Investigating — go to target, search, give up
            new SequenceNode(new List<Node>
            {
                new CheckInvestigatingNode(this),
                new DemonInvestigateNode(agent, detection, this)
            }),

            // Wander — fallback, watches suspicion to promote to Investigating
            new DemonWanderNode(agent, this, detection)

        });

        return root;
    }

    public void SetState(DemonState newState, Vector3? target = null)
    {
        currentState = newState;
        if (target.HasValue)
        {
            investigationTarget = target.Value;
        }
        Debug.Log($"Demon state -> {newState}");
    }
}
