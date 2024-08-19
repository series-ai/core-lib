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
        public async void InitContext_WhenContextNotInitialized_ShouldBeInitialized()
        {
            var context = go.AddComponent<Context>();

            await Task.Run(context.Init);
            
            Assert.IsTrue(context.IsInitialized);
        }

        [Test]
        public async void InitContext_WhenContextInitialized_ShouldThrowException()
        {
            var context = go.AddComponent<Context>();
            
            await Task.Run(context.Init);

            Assert.Throws<AggregateException>(() => Task.Run(context.Init).Wait());
        }

        [Test]
        public async void ShutdownContext_WhenContextInitialized_ShouldNotBeInitialized()
        {
            var context = go.AddComponent<Context>();
            await Task.Run(context.Init);

            Assert.DoesNotThrow(() => Task.Run(context.Shutdown).Wait());
            Assert.IsFalse(context.IsInitialized);
        }

        [Test]
        public async void ShutdownContext_WhenContextNotInitialized_ShouldThrowException()
        {
            var context = go.AddComponent<Context>();

            Assert.Throws<AggregateException>(() => Task.Run(context.Shutdown).Wait());
        }
    }
}
