public class CheckKillNode : Node
{
    private DemonBrain brain;

    public CheckKillNode(DemonBrain brain)
    {
        this.brain = brain;
    }

    public override NodeState Evaluate()
    {
        if (brain.CurrentState == DemonState.Killing)
        {
            State = NodeState.Success;
            return State;
        }

        State = NodeState.Failure;
        return State;
    }
}