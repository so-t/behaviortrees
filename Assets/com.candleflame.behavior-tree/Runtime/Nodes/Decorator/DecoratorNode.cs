using UnityEngine;

namespace BehaviorTree.Nodes.Decorator
{
    public abstract class DecoratorNode : ParentNode, IParent 
    {
        public GameObject gameObject { get; set; }

        protected override int MaxChildren { get; } = 1;

        public virtual void AddChild(INode child)
        {
            if (Children.Count < MaxChildren || MaxChildren < 0)
            {
                Children.Add(child);
            }
        }
    }
}