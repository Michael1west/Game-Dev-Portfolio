using UnityEngine;

public class CheckRoamNode : ConditionNode
{

    private DemonDetection detection;

    public CheckRoamNode(DemonDetection detection)
    {
        this.detection = detection;
    }

    public override NodeState Evaluate()
    {
        if (detection.HasSearched && !detection.HasRoamed && !detection.IsAlert())
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