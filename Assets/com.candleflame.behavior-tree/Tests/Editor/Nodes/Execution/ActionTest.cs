using NUnit.Framework;
using BehaviorTree;
using BehaviorTree.Nodes.Execution;

public class ActionTest 
{
    public class UpdateMethod 
    {
        [Test]
        public void Ticking_calls_supplied_function() 
        {
            var flag = false;

            var action = new Action(() => 
            {
                flag = true; 
                return Status.Success;
                });

            Assert.AreEqual(Status.Success, action.Tick());
            Assert.AreEqual(true, flag);
        }

        [Test]
        public void Initialization_flag_should_be_cleared_when_tick_returns_success() 
        {
            var action = new Action(() => Status.Success);

            Assert.AreEqual(Status.Success, action.Tick());
            Assert.AreEqual(action.Initialized, false);

        }

        [Test]
        public void Initialization_flag_should_be_cleared_when_tick_returns_failure() 
        {
            var action = new Action(() => Status.Failure);

            Assert.AreEqual(Status.Failure , action.Tick());
            Assert.AreEqual(action.Initialized, false);

        }

        [Test]
        public void Initialization_flag_should_not_be_cleared_when_tick_returns_running() 
        {
            var action = new Action(() => Status.Running);

            Assert.AreEqual(Status.Running, action.Tick());
            Assert.AreEqual(action.Initialized, true);

        }
    }
}
