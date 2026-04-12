using UnityEngine;

public class CheckHuntingNode : ConditionNode
{
    private DemonDetection detection;

    public CheckHuntingNode(DemonDetection detection)
    {
        this.detection = detection;
    }

    public override NodeState Evaluate()
    {
        if(detection.IsHunting())
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
