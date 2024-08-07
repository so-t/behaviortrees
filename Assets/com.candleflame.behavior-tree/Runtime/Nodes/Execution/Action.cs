using System;

namespace BehaviorTrees.Nodes.Execution
{
    public class Action : ExecutionNode, INode 
    {
        private Func<Status> _process;

        public Action(Func<Status> process)
        {
            this._process = process;
        }

        protected override Status Process() => _process();
    }
}