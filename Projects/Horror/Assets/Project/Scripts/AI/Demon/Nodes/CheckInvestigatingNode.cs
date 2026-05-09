public class CheckInvestigatingNode : Node
{
    private DemonBrain brain;

    public CheckInvestigatingNode(DemonBrain brain)
    {
        this.brain = brain;
    }

    public override NodeState Evaluate()
    {
        if (brain.CurrentState == DemonState.Investigating)
        {
            State = NodeState.Success;
            return State;
        }

        State = NodeState.Failure;
        return State;
    }
}