using System;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree.Nodes;

namespace BehaviorTree
{
    public class BehaviorTree
    {
        private readonly GameObject _gameObject;
        private readonly List<INode> _nodes = new List<INode>();

        public RootNode Root { get; } = new RootNode();

        public BehaviorTree(GameObject gameObject)
        {
            this._gameObject = gameObject;
        }

        public Status Tick() => Root.Tick();

        public void AddNode(IParent parent, INode child)
        {
            parent.AddChild(child);
            child.gameObject = _gameObject;
        }

        public void AddSubtree(IParent parent, BehaviorTree subtree)
        {
            parent.AddChild(subtree.Root);
            Sync(subtree.Root);
        }

        public void Sync(IParent parent)
        {
            parent.gameObject = _gameObject;

            foreach (var child in parent.Children)
            {
                child.gameObject = _gameObject;

                var newParent = child as IParent;
                if (newParent != null)
                {
                    Sync(newParent);
                }
            }
        }


    }
}