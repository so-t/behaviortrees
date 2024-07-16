using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTrees.Nodes.ControlFlow
{
    public class Fallback : Node, IParent 
    {
        public GameObject gameObject { get; set; }

        public List<INode> Children{ get; } = new List<INode>();

        public override Status Tick()
        {
            foreach (var child in Children)
            {
                var status = child.Tick();
                if (status != Status.Failure)
                {
                    return status;
                }
            }

            return Status.Failure;
        }

        public void AddChild(INode child)
        {
            Children.Add(child);
        }
    }
}