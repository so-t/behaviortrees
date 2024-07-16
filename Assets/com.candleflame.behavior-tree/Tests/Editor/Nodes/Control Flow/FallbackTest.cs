using NUnit.Framework;
using Moq;
using BehaviorTrees;
using BehaviorTrees.Nodes;
using BehaviorTrees.Nodes.Execution;
using BehaviorTrees.Nodes.ControlFlow;

public class FallbackTest 
{
    public class UpdateMethod 
    {
        private Fallback _fallback;

        [SetUp]
        public void Init()
        {
            _fallback = new Fallback();
        }

        public class SingleChild : UpdateMethod
        {
            [Test]
            public void Returns_success_if_child_returns_success()
            {
                var child = new Action(() => Status.Success);
                _fallback.AddChild(child);

                Assert.AreEqual(Status.Success, _fallback.Tick());
            }

            [Test]
            public void Returns_failure_if_child_returns_failure()
            {
                var child = new Action(() => Status.Failure);
                _fallback.AddChild(child);

                Assert.AreEqual(Status.Failure, _fallback.Tick());
            }

            [Test]
            public void Returns_running_if_child_returns_running()
            {
                var child = new Action(() => Status.Running);
                _fallback.AddChild(child);

                Assert.AreEqual(Status.Running, _fallback.Tick());
            }
        }

        public class MultipleChildren : UpdateMethod 
        {
            [Test]
            public void Continues_running_after_a_child_returns_failure()
            {
                var mockChild1 = new Mock<INode>();
                mockChild1
                    .Setup(m => m.Tick())
                    .Returns(Status.Failure);

                var mockChild2 = new Mock<INode>();
                mockChild2
                    .Setup(m => m.Tick())
                    .Returns(Status.Success);

                _fallback.AddChild(mockChild1.Object);
                _fallback.AddChild(mockChild2.Object);

                Assert.AreEqual(Status.Success, _fallback.Tick());

                mockChild1.Verify(m => m.Tick(), Times.Once());
                mockChild2.Verify(m => m.Tick(), Times.Once());
            }

            [Test]
            public void Returns_after_the_first_child_succeeds() 
            {
                var mockChild1 = new Mock<INode>();
                mockChild1
                    .Setup(m => m.Tick())
                    .Returns(Status.Success);

                var mockChild2 = new Mock<INode>();

                _fallback.AddChild(mockChild1.Object);
                _fallback.AddChild(mockChild2.Object);

                Assert.AreEqual(Status.Success, _fallback.Tick());

                mockChild1.Verify(m => m.Tick(), Times.Once());
                mockChild2.Verify(m => m.Tick(), Times.Never());

            }

            [Test]
            public void Returns_after_the_first_child_returns_running() 
            {
                var mockChild1 = new Mock<INode>();
                mockChild1
                    .Setup(m => m.Tick())
                    .Returns(Status.Running);

                var mockChild2 = new Mock<INode>();

                _fallback.AddChild(mockChild1.Object);
                _fallback.AddChild(mockChild2.Object);

                Assert.AreEqual(Status.Running, _fallback.Tick());

                mockChild1.Verify(m => m.Tick(), Times.Once());
                mockChild2.Verify(m => m.Tick(), Times.Never());
            }

            [Test]
            public void Returns_failure_if_all_children_return_failure()
            {
                var child1 = new Action(() => Status.Failure);
                var child2 = new Action(() => Status.Failure);

                _fallback.AddChild(child1);
                _fallback.AddChild(child2);

                Assert.AreEqual(Status.Failure, _fallback.Tick());
            }
        }
    }
}
