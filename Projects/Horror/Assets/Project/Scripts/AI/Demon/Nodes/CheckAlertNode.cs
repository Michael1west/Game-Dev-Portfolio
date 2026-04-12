using UnityEngine;

public class CheckAlertNode : ConditionNode
{
    private DemonDetection detection;

    public CheckAlertNode(DemonDetection detection)
    {
        this.detection = detection;
    }

    public override NodeState Evaluate()
    {
        if (detection.IsAlert())
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
