using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees.Nodes;
using BehaviorTrees.Nodes.ControlFlow;
using BehaviorTrees.Nodes.Decorator;
using BehaviorTrees.Nodes.Execution;

namespace BehaviorTrees 
{
    public class BehaviorTreeBuilder 
    {
        private readonly BehaviorTree _tree;
        private readonly Stack<IParent> _parents = new Stack<IParent>();


        private IParent LastParent
        {
            get 
            {
                return _parents.Count == 0 ? 
                    null 
                    : _parents.Peek();
            }
        }
        
        public BehaviorTreeBuilder(GameObject gameObject)
        {
            _tree = new BehaviorTree(gameObject);
            _parents.Push(_tree.Root);
        }

        public BehaviorTreeBuilder AddNode (INode node)
        {
            _tree.AddNode(LastParent, node);

            return this;
        }

        public BehaviorTreeBuilder AddParent(IParent parent)
        {
            AddNode(parent);
            _parents.Push(parent);

            return this;
        }

        public BehaviorTreeBuilder ParentNode<P>() where P : IParent, new() 
        {
            var parent = new P {};

            return AddParent(parent);
        }

        public BehaviorTreeBuilder Fallback()
        {
            return ParentNode<Fallback>();
        }

        public BehaviorTreeBuilder Sequence()
        {
            return ParentNode<Sequence>();
        }

        public BehaviorTreeBuilder Parallel(int threshold)
        {
            var parallel = new Parallel(threshold);
            return AddParent(parallel);
        }

        public BehaviorTreeBuilder Invert()
        {
            return ParentNode<Inverter>();
        }

        public BehaviorTreeBuilder Repeat(int repititions)
        {
            var repeater = new Repeater(repititions);
            return AddParent(repeater);
        }

        public BehaviorTreeBuilder Do(System.Func<Status> fn)
        {
            return AddNode(new Action(fn));
        }

        public BehaviorTreeBuilder End()
        {
            _parents.Pop();

            return this;
        }

        public BehaviorTree Build()
        {
            return _tree;
        }
    }
}