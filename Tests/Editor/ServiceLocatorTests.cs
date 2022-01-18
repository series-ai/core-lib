using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using System;

namespace Padoru.Core.Tests
{
    [TestFixture]
    public class ServiceLocatorTests
    {
        [SetUp]
        public void Setup()
        {
            Locator.Clear();
        }

        [TearDown]
        public void TearDown()
        {
            Locator.Clear();
        }

        [Test]
        public void RegisterService_WhenServiceNotRegistered_ShouldNotThrow()
        {
            Assert.DoesNotThrow(Locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>);
        }

        [Test]
        public void RegisterService_WhenServiceAlreadyRegistered_ShouldThrow()
        {
            Locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>();

            Assert.Throws<Exception>(Locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>);
        }

        [Test]
        public void UnregisterService_WhenServiceNotRegistered_ShouldThrow()
        {
            Assert.Throws<Exception>(Locator.UnregisterService<IEnumerable<GameObject>>);
        }

        [Test]
        public void UnregisterService_WhenServiceRegistered_ShouldNotThrow()
        {
            Locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>();

            Assert.DoesNotThrow(Locator.UnregisterService<IEnumerable<GameObject>>);
        }

        [Test]
        public void GetService_WhenServiceNotRegistered_ShouldThrow()
        {
            Assert.Throws<Exception>(() => Locator.GetService<IEnumerable<GameObject>>());
        }

        [Test]
        public void GetService_WhenServiceRegistered_ShouldNotBeNull()
        {
            Locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>();

            var service = Locator.GetService<IEnumerable<GameObject>>();

            Assert.NotNull(service);
        }
    }
}
