using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTrees.Nodes.ControlFlow
{
    public class Parallel : Node, IParent 
    {
        public GameObject gameObject { get; set; }

        public List<INode> Children{ get; } = new List<INode>();

        private int threshold;

        public Parallel(int threshold = -1)
        {
            this.threshold = threshold;
        }

        public override Status Tick()
        {
            int failures = 0, successes = 0;

            foreach (var child in Children)
            {
                var status = child.Tick();
                switch (status)
                {
                    case Status.Success: successes++; break;
                    case Status.Failure: failures++; break;
                    default: break;
                }
            }

            if (threshold > 0)
            {
                if (successes >= this.threshold) 
                {
                    return Status.Success;
                } 
                else if (failures > Children.Count - threshold)
                {
                    return Status.Failure;
                }
                else return Status.Running;
            }
            else return Status.Success;
        }

        public void AddChild(INode child)
        {
            Children.Add(child);
        }
    }
}