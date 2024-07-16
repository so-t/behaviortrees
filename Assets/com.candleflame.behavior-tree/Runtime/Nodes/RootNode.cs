using UnityEngine;

namespace BehaviorTree.Nodes
{
    public class RootNode : ParentNode, IParent
    {
        public GameObject gameObject { get; set; }

        protected override int MaxChildren { get; } = 1;

        public override Status Tick()
        {
            if (Children.Count == 0)
            {
                return Status.Success;
            }

            var child = Children[0];
            return child.Tick();
        }

        public virtual void AddChild(INode child)
        {
            if (Children.Count < MaxChildren || MaxChildren < 0)
            {
                Children.Add(child);
            }
        }
    }
}