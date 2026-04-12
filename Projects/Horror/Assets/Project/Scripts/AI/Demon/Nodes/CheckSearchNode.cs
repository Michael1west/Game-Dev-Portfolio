using UnityEngine;

public class CheckSearchNode : ConditionNode
{
    
    private DemonDetection detection;

    public CheckSearchNode(DemonDetection detection)
    {
        this.detection = detection;
    }

    public override NodeState Evaluate()
    {
        if (!detection.IsAlert() && detection.SeenPlayer && detection.HasBeenAlerted)
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