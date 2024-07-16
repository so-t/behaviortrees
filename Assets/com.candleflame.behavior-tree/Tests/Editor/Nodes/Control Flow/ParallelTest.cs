using NUnit.Framework;
using Moq;
using BehaviorTree;
using BehaviorTree.Nodes;
using BehaviorTree.Nodes.ControlFlow;

public class ParallelTest 
{
    public class UpdateMethod 
    {
        private Parallel _parallel;

        [SetUp]
        public void Init()
        {
            _parallel = new Parallel(2);
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

            _parallel.AddChild(mockChild1.Object);
            _parallel.AddChild(mockChild2.Object);

            Assert.AreEqual(Status.Success, _parallel.Tick());
            Assert.AreEqual(2, callCount);

            mockChild1.Verify(m => m.Tick(), Times.Once());
            mockChild2.Verify(m => m.Tick(), Times.Once());
        }

        [Test]
        public void Returns_failure_when_child_failure_count_is_over_threshold()
        {
            var mockChild1 = new Mock<INode>();
            mockChild1
                .Setup(m => m.Tick())
                .Returns(Status.Failure);

            var mockChild2 = new Mock<INode>();
            mockChild2
                .Setup(m => m.Tick())
                .Returns(Status.Failure);

            var mockChild3 = new Mock<INode>();
            mockChild3
                .Setup(m => m.Tick())
                .Returns(Status.Running);

            _parallel.AddChild(mockChild1.Object);
            _parallel.AddChild(mockChild2.Object);
            _parallel.AddChild(mockChild3.Object);

            Assert.AreEqual(Status.Failure, _parallel.Tick());

            mockChild1.Verify(m => m.Tick(), Times.Once());
            mockChild2.Verify(m => m.Tick(), Times.Once());
            mockChild3.Verify(m => m.Tick(), Times.Once());
        }

        [Test]
        public void Returns_success_when_child_failure_count_is_below_threshold()
        {
            var mockChild1 = new Mock<INode>();
            mockChild1
                .Setup(m => m.Tick())
                .Returns(Status.Success);

            var mockChild2 = new Mock<INode>();
            mockChild2
                .Setup(m => m.Tick())
                .Returns(Status.Success);

            var mockChild3 = new Mock<INode>();
            mockChild3
                .Setup(m => m.Tick())
                .Returns(Status.Failure);

            _parallel.AddChild(mockChild1.Object);
            _parallel.AddChild(mockChild2.Object);
            _parallel.AddChild(mockChild3.Object);

            Assert.AreEqual(Status.Success, _parallel.Tick());

            mockChild1.Verify(m => m.Tick(), Times.Once());
            mockChild2.Verify(m => m.Tick(), Times.Once());
            mockChild3.Verify(m => m.Tick(), Times.Once());
        }
    }
}
