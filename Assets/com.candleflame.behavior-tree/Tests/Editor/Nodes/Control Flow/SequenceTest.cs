using NUnit.Framework;
using Moq;
using BehaviorTrees;
using BehaviorTrees.Nodes;
using BehaviorTrees.Nodes.Execution;
using BehaviorTrees.Nodes.ControlFlow;

public class SequenceTest 
{
    public class UpdateMethod 
    {
        private Sequence _sequence;

        [SetUp]
        public void Init()
        {
            _sequence = new Sequence();
        }

        [Test]
        public void Ticks_all_children_in_order()
        {
            var callCount = 0;

            var mockChild1 = new Mock<INode>();
            mockChild1
                .Setup(m => m.Tick())
                .Returns(Status.Success)
                .Callback(() => 
                { 
                    Assert.AreEqual(1, ++callCount);
                });


            var mockChild2 = new Mock<INode>();
            mockChild2
                .Setup(m => m.Tick())
                .Returns(Status.Success)
                .Callback(() => 
                { 
                    Assert.AreEqual(2, ++callCount);
                });

            _sequence.AddChild(mockChild1.Object);
            _sequence.AddChild(mockChild2.Object);

            Assert.AreEqual(Status.Success, _sequence.Tick());
            Assert.AreEqual(2, callCount);

            mockChild1.Verify(m => m.Tick(), Times.Once());
            mockChild2.Verify(m => m.Tick(), Times.Once());
        }

        [Test]
        public void Continues_running_after_a_child_returns_success()
        {
            var mockChild1 = new Mock<INode>();
            mockChild1
                .Setup(m => m.Tick())
                .Returns(Status.Success);


            var mockChild2 = new Mock<INode>();
            mockChild2
                .Setup(m => m.Tick())
                .Returns(Status.Failure);

            _sequence.AddChild(mockChild1.Object);
            _sequence.AddChild(mockChild2.Object);

            Assert.AreEqual(Status.Failure, _sequence.Tick());

            mockChild1.Verify(m => m.Tick(), Times.Once());
            mockChild2.Verify(m => m.Tick(), Times.Once());
        }

        [Test]
        public void Returns_failure_if_a_child_returns_failure()
        {
            var mockChild1 = new Mock<INode>();
            mockChild1
                .Setup(m => m.Tick())
                .Returns(Status.Failure);


            var mockChild2 = new Mock<INode>();
            mockChild2
                .Setup(m => m.Tick())
                .Returns(Status.Success);

            _sequence.AddChild(mockChild1.Object);
            _sequence.AddChild(mockChild2.Object);

            Assert.AreEqual(Status.Failure, _sequence.Tick());

            mockChild1.Verify(m => m.Tick(), Times.Once());
            mockChild2.Verify(m => m.Tick(), Times.Never());
        }

        [Test]
        public void Returns_running_if_a_child_returns_running()
        {
            var mockChild1 = new Mock<INode>();
            mockChild1
                .Setup(m => m.Tick())
                .Returns(Status.Running);


            var mockChild2 = new Mock<INode>();
            mockChild2
                .Setup(m => m.Tick())
                .Returns(Status.Success);

            _sequence.AddChild(mockChild1.Object);
            _sequence.AddChild(mockChild2.Object);

            Assert.AreEqual(Status.Running, _sequence.Tick());

            mockChild1.Verify(m => m.Tick(), Times.Once());
            mockChild2.Verify(m => m.Tick(), Times.Never());
        }

        [Test]
        public void Returns_success_if_all_children_return_success()
        {
            var child1 = new Action(() => Status.Success);
            var child2 = new Action(() => Status.Success);

            _sequence.AddChild(child1);
            _sequence.AddChild(child2);

            Assert.AreEqual(Status.Success, _sequence.Tick());
        }
    }
}
