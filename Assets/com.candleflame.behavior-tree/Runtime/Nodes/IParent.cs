namespace BehaviorTree.Nodes
{
    public interface IParent : INode
    {
        void AddChild(INode child);
    }
}