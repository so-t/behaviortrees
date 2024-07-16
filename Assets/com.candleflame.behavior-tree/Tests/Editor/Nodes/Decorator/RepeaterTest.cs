using NUnit.Framework;
using Moq;
using BehaviorTrees;
using BehaviorTrees.Nodes;
using BehaviorTrees.Nodes.Execution;
using BehaviorTrees.Nodes.Decorator;

public class RepeaterTest 
{
    public class UpdateMethod 
    {
        [Test]
        public void Ticking_with_no_child_throws_exception() 
        {
            var repeater = new Repeater(2);
            
            Assert.Throws<System.ApplicationException>(() => repeater.Tick());
        }

        [Test]
        public void Only_accepts_a_single_child() 
        {
            var repeater = new Repeater();

            var child1 = new Action(() => Status.Success);
            var child2 = new Action(() => Status.Success);

            repeater.AddChild(child1);
            repeater.AddChild(child2);

            Assert.AreEqual(1, repeater.Children.Count);
            Assert.AreEqual(child1, repeater.Children[0]);
        }

        [Test]
        public void Ticks_child_set_number_of_times()
        {
            var repeater = new Repeater(3);

            var mockChild = new Mock<INode>();
            mockChild
                .Setup(m => m.Tick())
                .Returns(Status.Success);

            repeater.AddChild(mockChild.Object);

            Assert.AreEqual(Status.Success, repeater.Tick());

            mockChild.Verify(m => m.Tick(), Times.Exactly(3));
        }

        [Test]
        public void Returns_failure_when_child_returns_failure() 
        {
            var repeater = new Repeater(2);
            var child = new Action(() => Status.Failure);

            repeater.AddChild(child);

            Assert.AreEqual(Status.Failure, repeater.Tick());
        }

        [Test]
        public void Returns_success_when_child_returns_success() 
        {
            var repeater = new Repeater(2);
            var child = new Action(() => Status.Success);

            repeater.AddChild(child);

            Assert.AreEqual(Status.Success, repeater.Tick());
        }

        [Test]
        public void Returns_running_when_child_returns_running() 
        {
            var repeater = new Repeater(2);
            var child = new Action(() => Status.Running);

            repeater.AddChild(child);

            Assert.AreEqual(Status.Running, repeater.Tick());
        }
    }
}
