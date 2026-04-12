using UnityEngine;
using System.Collections.Generic;

public class SelectorNode : Node
{
    public SelectorNode(List<Node> children)
    {
        this.children = children;
    }

    public override NodeState Evaluate()
    {
        foreach (Node child in children)
        {
            switch (child.Evaluate())
            {
                case NodeState.Success:
                    State = NodeState.Success;
                    return State;
                case NodeState.Running:
                    State = NodeState.Running;
                    return State;
                case NodeState.Failure:
                    continue;
            }
        }
        State = NodeState.Failure;
        return State;
    }
}
