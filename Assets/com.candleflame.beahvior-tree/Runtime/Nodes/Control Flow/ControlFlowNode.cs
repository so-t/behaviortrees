using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

namespace BehaviorTree.Nodes.ControlFlow 
{
    public abstract class ControlFlowNode : ParentNode, IParent 
    {
        public GameObject gameObject { get; set; }

        public virtual void AddChild(INode child)
        {
            if (Children.Count < MaxChildren || MaxChildren < 0)
            {
                Children.Add(child);
            }
        }
    }
}