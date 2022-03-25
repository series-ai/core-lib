using NUnit.Framework;
using System;

namespace Padoru.Core.Tests
{
    [TestFixture]
    public class GenericPromiseTests
    {
        private class TestClass { }

        private bool completed = false;
        private bool failed = false;
        private TestClass result;

        [Test]
        public void Complete_WhenNotCompletedAndNotFailed_ShouldNotThrowAndShouldRunCallbackAndShouldOutputTheGivenResult()
        {
            completed = false;
            var promise = new Promise<TestClass>();
            promise.OnComplete(OnCompleteCallback);

            var testClass = new TestClass();

            Assert.DoesNotThrow(() => promise.Complete(testClass));
            Assert.IsTrue(completed);
            Assert.AreEqual(testClass, result);
        }

        [Test]
        public void Complete_WhenCompleted_ShouldThrow()
        {
            var promise = new Promise<TestClass>();
            promise.Complete(new TestClass());

            Assert.Throws<Exception>(() => promise.Complete(new TestClass()));
        }

        [Test]
        public void Complete_WhenFailed_ShouldThrow()
        {
            var promise = new Promise<TestClass>();
            promise.Fail(null);

            Assert.Throws<Exception>(() => promise.Complete(new TestClass()));
        }

        [Test]
        public void Complete_WhenReseted_ShouldNotThrowAndShouldRunCallbackAndShouldOutputTheGivenResult()
        {
            completed = false;
            var promise = new Promise<TestClass>();
            promise.Complete(new TestClass());
            promise.Reset();
            promise.OnComplete(OnCompleteCallback);

            var testClass = new TestClass();

            Assert.DoesNotThrow(() => promise.Complete(testClass));
            Assert.IsTrue(completed);
            Assert.AreEqual(testClass, result);
        }

        [Test]
        public void Fail_WhenNotCompletedAndNotFailed_ShouldNotThrowAndShouldRunCallback()
        {
            failed = false;
            var promise = new Promise<TestClass>();
            promise.OnFail(OnFailCallback);

            Assert.DoesNotThrow(() => promise.Fail(null));
            Assert.IsTrue(failed);
        }

        [Test]
        public void Fail_WhenCompleted_ShouldThrow()
        {
            var promise = new Promise<TestClass>();
            promise.Complete(new TestClass());

            Assert.Throws<Exception>(() => promise.Fail(null));
        }

        [Test]
        public void Fail_WhenFailed_ShouldThrow()
        {
            var promise = new Promise<TestClass>();
            promise.Fail(null);

            Assert.Throws<Exception>(() => promise.Fail(null));
        }

        [Test]
        public void Fail_WhenReseted_ShouldNotThrowAndShouldRunCallback()
        {
            failed = false;
            var promise = new Promise<TestClass>();
            promise.Fail(null);
            promise.Reset();
            promise.OnFail(OnFailCallback);

            Assert.DoesNotThrow(() => promise.Fail(null));
            Assert.IsTrue(failed);
        }

        private void OnCompleteCallback(TestClass testClass)
        {
            result = testClass;
            completed = true;
        }

        private void OnFailCallback(Exception e)
        {
            failed = true;
        }
    }
}