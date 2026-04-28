using UnityEngine;

public class CheckKillNode : ConditionNode
{
    private Transform demon;
    private Transform player;
    private const float KillRange = 1.5f;

    public CheckKillNode(Transform demon, Transform player)
    {
        this.demon = demon;
        this.player = player;

    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(demon.position, player.position);

        if (distance <= KillRange)
        {
            State = NodeState.Success;
            return State;
        }
        else
        {
            State = NodeState.Failure;
            return State;
        }
    }
}
