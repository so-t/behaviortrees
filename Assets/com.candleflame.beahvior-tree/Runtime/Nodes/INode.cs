using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree.Nodes
{
    public interface INode
    {
        public GameObject gameObject { get; set; }

        List<INode> Children { get; }

        Status Tick();
    }
}