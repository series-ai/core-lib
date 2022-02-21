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
        private string tag1 = "Pepe1";
        private string tag2 = "Pepe2";

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
        public void RegisterServiceWithoutTag_WhenServiceNotRegistered_ShouldNotThrow()
        {
            Assert.DoesNotThrow(locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>);
        }

        [Test]
        public void RegisterServiceWithoutTag_WhenServiceAlreadyRegistered_ShouldThrow()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>();

            Assert.Throws<Exception>(locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>);
        }

        [Test]
        public void RegisterServiceWithTag_WhenServiceNotRegistered_ShouldNotThrow()
        {
            Assert.DoesNotThrow(() => locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>(tag1));
        }

        [Test]
        public void RegisterServiceWithTag_WhenServiceAlreadyRegistered_ShouldThrow()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>(tag1);

            Assert.Throws<Exception>(() => locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>(tag1));
        }

        [Test]
        public void RegisterServiceWithTag_WhenServiceTypeAlreadyRegistered_ShouldNotThrow()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>(tag1);

            Assert.DoesNotThrow(() => locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>(tag2));
        }

        [Test]
        public void UnregisterServiceWithoutTag_WhenServiceNotRegistered_ShouldThrow()
        {
            Assert.Throws<Exception>(locator.UnregisterService<IEnumerable<GameObject>>);
        }

        [Test]
        public void UnregisterServiceWithoutTag_WhenServiceRegistered_ShouldNotThrow()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>();

            Assert.DoesNotThrow(locator.UnregisterService<IEnumerable<GameObject>>);
        }

        [Test]
        public void UnregisterServiceWithTag_WhenServiceNotRegistered_ShouldThrow()
        {
            Assert.Throws<Exception>(() => locator.UnregisterService<IEnumerable<GameObject>>(tag1));
        }

        [Test]
        public void UnregisterServiceWithTag_WhenServiceRegistered_ShouldNotThrow()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>(tag1);

            Assert.DoesNotThrow(() => locator.UnregisterService<IEnumerable<GameObject>>(tag1));
        }

        [Test]
        public void GetServiceWithoutTag_WhenServiceNotRegistered_ShouldThrow()
        {
            Assert.Throws<Exception>(() => locator.GetService<IEnumerable<GameObject>>());
        }

        [Test]
        public void GetServiceWithoutTag_WhenServiceRegistered_ShouldNotBeNull()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>();

            var service = locator.GetService<IEnumerable<GameObject>>();

            Assert.NotNull(service);
        }

        [Test]
        public void GetServiceWithTag_WhenServiceNotRegistered_ShouldThrow()
        {
            Assert.Throws<Exception>(() => locator.GetService<IEnumerable<GameObject>>(tag1));
        }

        [Test]
        public void GetServiceWithTag_WhenServiceRegistered_ShouldNotBeNull()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>(tag1);

            var service = locator.GetService<IEnumerable<GameObject>>(tag1);

            Assert.NotNull(service);
        }

        [Test]
        public void GetServicesWithSameTag_WhenServiceRegisteredWithDifferentType_NeitherShouldNotBeNull()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>(tag1);
            locator.RegisterService<IList<GameObject>, List<GameObject>>(tag1);

            var service1 = locator.GetService<IEnumerable<GameObject>>(tag1);
            var service2 = locator.GetService<IList<GameObject>>(tag1);

            Assert.NotNull(service1);
            Assert.NotNull(service2);
        }

        [Test]
        public void GetServicesWithSameType_WhenServiceRegisteredWithDifferentTag_NeitherShouldNotBeNull()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>(tag1);
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>(tag2);

            var service1 = locator.GetService<IEnumerable<GameObject>>(tag1);
            var service2 = locator.GetService<IEnumerable<GameObject>>(tag2);

            Assert.NotNull(service1);
            Assert.NotNull(service2);
        }

        [Test]
        public void GetAllServices_WhenRegisteredWithAndWithoutTag_ShouldGetBoth()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>();
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>(tag1);

            var services = locator.GetAllServices<IEnumerable<GameObject>>();

            Assert.AreEqual(2, services.Length);
        }

        [Test]
        public void ClearServices_WhenNormalAndTaggedServicesRegistered_ShouldThrow()
        {
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>();
            locator.RegisterService<IEnumerable<GameObject>, List<GameObject>>(tag1);

            locator.Clear();

            Assert.Throws<Exception>(() => locator.GetService<IEnumerable<GameObject>>());
            Assert.Throws<Exception>(() => locator.GetService<IEnumerable<GameObject>>(tag1));
        }
    }
}
