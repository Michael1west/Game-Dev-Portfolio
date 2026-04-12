using UnityEngine;
using System.Collections.Generic;

public class SequenceNode : Node
{
    public SequenceNode(List<Node> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        foreach (Node child in children)
        {
            switch (child.Evaluate())
            {
                case NodeState.Failure:
                    State = NodeState.Failure;
                    return State;
                case NodeState.Running:
                    State = NodeState.Running;
                    return State;
                case NodeState.Success:
                    continue;
            }
        }
        State = NodeState.Success;
        return State;
    }
}
