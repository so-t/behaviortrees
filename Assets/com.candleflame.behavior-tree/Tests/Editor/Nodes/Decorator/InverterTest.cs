using NUnit.Framework;
using BehaviorTree;
using BehaviorTree.Nodes.Execution;
using BehaviorTree.Nodes.Decorator;

public class InverterTest 
{
    public class UpdateMethod 
    {
        [Test]
        public void Ticking_With_No_Child_Throws_Exception() 
        {
            var inverter = new Inverter();
            
            Assert.Throws<System.ApplicationException>(() => inverter.Tick());
        }
        
        [Test]
        public void Only_accepts_a_single_child() 
        {
            var inverter = new Inverter();

            var child1 = new Action(() => Status.Success);
            var child2 = new Action(() => Status.Success);

            inverter.AddChild(child1);
            inverter.AddChild(child2);

            Assert.AreEqual(1, inverter.Children.Count);
            Assert.AreEqual(child1, inverter.Children[0]);
        }

        [Test]
        public void Returns_failure_when_child_returns_success() 
        {
            var inverter = new Inverter();
            var child = new Action(() => Status.Success);

            inverter.AddChild(child);

            Assert.AreEqual(Status.Failure, inverter.Tick());
        }

        [Test]
        public void Returns_success_when_child_returns_failure() 
        {
            var inverter = new Inverter();
            var child = new Action(() => Status.Failure);

            inverter.AddChild(child);

            Assert.AreEqual(Status.Success, inverter.Tick());
        }

        [Test]
        public void Returns_running_when_child_returns_running()
        {
            var inverter = new Inverter();
            var child = new Action(() => Status.Running);

            inverter.AddChild(child);

            Assert.AreEqual(Status.Running, inverter.Tick());

        }
    }
}
