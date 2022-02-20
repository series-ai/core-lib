using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace Padoru.Core.Tests
{
    [TestFixture]
    public class BasicLocatorTests
    {
        private BasicLocator locator = new BasicLocator();

        [SetUp]
        public void Setup()
        {
            locator.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            locator.Clear();
        }

        [Test]
        public void RegisterService_WhenServiceNotRegistered_ShouldNotThrow()
        {
            Assert.DoesNotThrow(locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>);
        }

        [Test]
        public void RegisterService_WhenServiceAlreadyRegistered_ShouldThrow()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>();

            Assert.Throws<Exception>(locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>);
        }

        [Test]
        public void UnregisterService_WhenServiceNotRegistered_ShouldThrow()
        {
            Assert.Throws<Exception>(locator.UnregisterService<IEnumerable<GameObject>>);
        }

        [Test]
        public void UnregisterService_WhenServiceRegistered_ShouldNotThrow()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>();

            Assert.DoesNotThrow(locator.UnregisterService<IEnumerable<GameObject>>);
        }

        [Test]
        public void GetService_WhenServiceNotRegistered_ShouldThrow()
        {
            Assert.Throws<Exception>(() => locator.GetService<IEnumerable<GameObject>>());
        }

        [Test]
        public void GetService_WhenServiceRegistered_ShouldNotBeNull()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>();

            var service = locator.GetService<IEnumerable<GameObject>>();

            Assert.NotNull(service);
        }
    }
}
