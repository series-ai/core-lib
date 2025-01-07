using NUnit.Framework;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace Padoru.Core.Tests
{
	public class CotextTests
	{
		private GameObject go;

        [SetUp]
        public void Setup()
        {
            go = new GameObject("Context");
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(go);
        }

        [Test]
        public async Task InitContext_WhenContextNotInitialized_ShouldBeInitialized()
        {
            var context = go.AddComponent<Context>();

            await context.Init();
            
            Assert.IsTrue(context.IsInitialized);
        }

        [Test]
        public async Task InitContext_WhenContextInitialized_ShouldThrowException()
        {
            var context = go.AddComponent<Context>();

            Assert.ThrowsAsync<AggregateException>(async () => await context.Init());
        }

        [UnityTest]
        public IEnumerator ShutdownContext_WhenContextInitialized_ShouldNotBeInitialized()
        {
            yield return null;

            var context = go.AddComponent<Context>();
            context.Init();

            Assert.DoesNotThrow(context.Shutdown);
            Assert.IsFalse(context.IsInitialized);
        }

        [UnityTest]
        public IEnumerator ShutdownContext_WhenContextNotInitialized_ShouldThrowException()
        {
            yield return null;

            var context = go.AddComponent<Context>();

            Assert.Throws<Exception>(context.Shutdown);
        }
    }
}
