using System;

namespace BehaviorTrees.Nodes.Decorator
{
    public class Inverter : DecoratorNode, IParent 
    {
        public override Status Tick()
        {
            if (Children.Count == 0)
            {
                throw new ApplicationException("Decorator node has no child.");
            }

            var result = Children[0].Tick();
            if (result == Status.Failure) 
            {
                return Status.Success;
            } 
            else if (result == Status.Success) 
            { 
                return Status.Failure;
            }
            else 
            {
                return result;
            }
        }
    }
}