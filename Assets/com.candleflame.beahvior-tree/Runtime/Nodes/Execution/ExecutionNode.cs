using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes.Execution
{
    public abstract class ExecutionNode : Node, INode 
    {
        public GameObject gameObject { get; set; }

        public List<INode> Children { get; } = null;
    }
}