using System.Collections.Generic;
using UnityEngine;

public abstract class Node 
{
    protected List<Node> children = new List<Node>();
    public NodeState State { get; protected set; }

    public abstract NodeState Evaluate();
}

