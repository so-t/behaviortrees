using System;

namespace BehaviorTrees.Nodes.Decorator
{
    public class Repeater : DecoratorNode, IParent
    {
        private int repititions;

        public Repeater(int repititions = 0)
        {
            this.repititions = repititions;
        }

        public override Status Tick()
        {
            if (Children.Count == 0)
            {
                throw new ApplicationException("Decorator node has no child.");
            }

            Status status = Status.Success;
            for (int i = 0; i < this.repititions; i++)
            {
                status = Children[0].Tick();
            }

            return status;
        }
    }
}