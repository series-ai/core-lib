using NUnit.Framework;
using System;
using System.Collections;
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

        [UnityTest]
        public IEnumerator InitContext_WhenContextNotInitialized_ShouldBeInitialized()
        {
            yield return null;

            var context = go.AddComponent<Context>();

            Assert.DoesNotThrow(context.Init);
            Assert.IsTrue(context.IsInitialized);
        }

        [UnityTest]
        public IEnumerator InitContext_WhenContextInitialized_ShouldThrowException()
        {
            yield return null;

            var context = go.AddComponent<Context>();
            context.Init();

            Assert.Throws<Exception>(context.Init);
        }

        [UnityTest]
        public IEnumerator ShudownContext_WhenContextInitialized_ShouldNotBeInitialized()
        {
            yield return null;

            var context = go.AddComponent<Context>();
            context.Init();

            Assert.DoesNotThrow(context.Shutdown);
            Assert.IsFalse(context.IsInitialized);
        }

        [UnityTest]
        public IEnumerator ShudownContext_WhenContextNotInitialized_ShouldThrowException()
        {
            yield return null;

            var context = go.AddComponent<Context>();

            Assert.Throws<Exception>(context.Shutdown);
        }
    }
}
