using UnityEngine;

public class CheckRetreatNode : ConditionNode
{

    private DemonDetection detection;

    public CheckRetreatNode(DemonDetection detection)
    {
        this.detection = detection;
    }

    public override NodeState Evaluate()
    {
        if (detection.HasRoamed)
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