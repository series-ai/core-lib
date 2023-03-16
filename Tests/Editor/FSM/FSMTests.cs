using NUnit.Framework;
using System;

namespace Padoru.Core.Tests
{
	public class FSMTests
    {
        private FSM<TestStates, TestTriggers> fsm;
        private TestStates startState = TestStates.State1;

        [OneTimeSetUp]
        public void Setup()
        {
            var tickManager = new MockTickManager();
            Locator.Register<ITickManager>(tickManager);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Locator.Unregister<ITickManager>();
        }

        [Test]
        public void StartFSM_WhenNotStarted_InitialStateShouldBeTheOneSet()
        {
            fsm = new FSM<TestStates, TestTriggers>(startState);
            fsm.Start();

            Assert.AreEqual(fsm.CurrentState, fsm.GetState(startState));
        }

        [Test]
        public void StartFSM_WhenNotStarted_ShouldBeActive()
        {
            fsm = new FSM<TestStates, TestTriggers>(startState);
            fsm.Start();

            Assert.IsTrue(fsm.IsActive);
        }

        [Test]
        public void StartFSM_WhenStarted_ShouldThrowException()
        {
            fsm = new FSM<TestStates, TestTriggers>(startState);
            fsm.Start();

            Assert.Throws<Exception>(fsm.Start);
        }

        [Test]
        public void StopFSM_WhenStarted_ShouldNotBeActive()
        {
            fsm = new FSM<TestStates, TestTriggers>(startState);
            fsm.Start();
            fsm.Stop();

            Assert.IsFalse(fsm.IsActive);
        }

        [Test]
        public void StopFSM_WhenNotStarted_ShouldThrowException()
        {
            fsm = new FSM<TestStates, TestTriggers>(startState);

            Assert.Throws<Exception>(fsm.Stop);
        }

        [Test]
        public void AddTransition_WhenNotAdded_ShouldNotThrow()
        {
            fsm = new FSM<TestStates, TestTriggers>(startState);

            Assert.DoesNotThrow(() => fsm.AddTransition(TestStates.State1, TestStates.State2, TestTriggers.Trigger1));
        }

        [Test]
        public void AddTransition_WhenAdded_ShouldThrow()
        {
            fsm = new FSM<TestStates, TestTriggers>(startState);
            fsm.AddTransition(TestStates.State1, TestStates.State2, TestTriggers.Trigger1);

            Assert.Throws<Exception>(() => fsm.AddTransition(TestStates.State1, TestStates.State2, TestTriggers.Trigger1));
        }

        [Test]
        public void GetState_WhenFSMCreated_ShouldNotThrowOrBeNull()
        {
            fsm = new FSM<TestStates, TestTriggers>(startState);

            var stateIds = Enum.GetValues(typeof(TestStates));
            foreach (var stateId in stateIds)
            {
                Assert.DoesNotThrow(() => fsm.GetState((TestStates)stateId));
                Assert.IsNotNull(fsm.GetState((TestStates)stateId));
            }
        }

        [Test]
        public void SetTrigger_WhenRegistered_ShouldTransition()
        {
            var targetState = TestStates.State2;

            fsm = new FSM<TestStates, TestTriggers>(startState);
            fsm.AddTransition(TestStates.State1, targetState, TestTriggers.Trigger1);
            fsm.Start();

            Assert.DoesNotThrow(() => fsm.SetTrigger(TestTriggers.Trigger1));
            Assert.AreEqual(fsm.GetState(targetState), fsm.CurrentState);
        }

        [Test]
        public void SetTrigger_WhenUnregistered_ShouldNotTransition()
        {
            fsm = new FSM<TestStates, TestTriggers>(startState);
            fsm.Start();

            Assert.DoesNotThrow(() => fsm.SetTrigger(TestTriggers.Trigger1));
            Assert.AreEqual(fsm.GetState(startState), fsm.CurrentState);
        }
    }
}
