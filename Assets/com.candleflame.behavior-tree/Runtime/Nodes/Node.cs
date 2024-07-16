using System.Collections.Generic;
using BehaviorTree;

namespace BehaviorTree.Nodes
{
    public abstract class Node
    {
        public bool Initialized { get; private set; }

        public virtual Status Tick()
        {
            if (!Initialized) 
            {
                Init();
                Initialized = true;
            }

            Status status = Process();

            if (status != Status.Running)
            {
                Initialized = false;
                End();
            }

            return status;
        }

        protected virtual void Init()
        {
            
        }

        protected virtual Status Process()
        {
            return Status.Success;
        }

        protected virtual void End()
        {

        }
    }
}