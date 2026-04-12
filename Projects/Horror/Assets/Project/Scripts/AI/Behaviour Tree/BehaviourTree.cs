using UnityEngine;

public class BehaviourTree : MonoBehaviour
{
    private Node root;

    protected void Start()
    {
        root = SetupTree();
    }

    private void Update()
    {
        root?.Evaluate();
    }

    protected virtual Node SetupTree()
    {
        return null;
    }
}
