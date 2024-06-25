using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes.ControlFlow
{
    public class Sequence : Node, IParent 
    {
        public GameObject gameObject { get; set; }

        public List<INode> Children{ get; } = new List<INode>();

        public override Status Tick()
        {
            foreach (var child in Children)
            {
                var status = child.Tick();
                if (status != Status.Success)
                {
                    return status;
                }
            }

            return Status.Success;
        }

        public void AddChild(INode child)
        {
            Children.Add(child);
        }
    }
}