using System.Collections.Generic;

namespace BehaviorTrees.Nodes
{
    public abstract class ParentNode
    {        
        public List<INode> Children { get; } = new List<INode>();

        protected virtual int MaxChildren { get; } = -1;

        public virtual Status Tick()
        {
            return Status.Success;
        }
    }
}