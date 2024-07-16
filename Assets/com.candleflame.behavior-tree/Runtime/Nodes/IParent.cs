namespace BehaviorTrees.Nodes
{
    public interface IParent : INode
    {
        void AddChild(INode child);
    }
}