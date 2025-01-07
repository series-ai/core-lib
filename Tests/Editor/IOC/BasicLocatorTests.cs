using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

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
        public async Task RegisterServiceWithoutTag_WhenServiceNotRegistered_ShouldNotThrow()
        {
            await ThenRegisterServiceDoesNotThrow_WhenServiceNotRegisteredWithoutTag<IEnumerable<GameObject>, List<GameObject>>();
        }

        [Test]
        public async Task RegisterServiceWithoutTag_WhenServiceAlreadyRegistered_ShouldThrow()
        {
            GivenAnAlreadyRegisteredServiceWithoutTag<IEnumerable<GameObject>, List<GameObject>>();

            await ThenRegisterServiceThrows_WhenRegisteringAnAlreadyRegisteredServiceWithoutTag<IEnumerable<GameObject>, List<GameObject>>();
        }

        [Test]
        public void RegisterServiceWithTag_WhenServiceNotRegistered_ShouldNotThrow()
        {
            ThenRegisterServiceDoesNotThrow_WhenServiceNotRegisteredWithTag<IEnumerable<GameObject>, List<GameObject>>(tag1);
        }

        [Test]
        public void RegisterServiceWithTag_WhenServiceAlreadyRegistered_ShouldThrow()
        {
            GivenAnAlreadyRegisteredServiceWithTag<IEnumerable<GameObject>, List<GameObject>>(tag1);

            ThenRegisterServiceThrows_WhenRegisteringAnAlreadyRegisteredServiceWithTag<IEnumerable<GameObject>, List<GameObject>>(tag1);
        }

        [Test]
        public void RegisterServiceWithTag_WhenServiceTypeAlreadyRegistered_ShouldNotThrow()
        {
            locator.Register<IEnumerable<GameObject>, List<GameObject>>(tag1);

            Assert.DoesNotThrow(() => locator.Register<IEnumerable<GameObject>, List<GameObject>>(tag2));
        }

        [Test]
        public void RegisterServiceWithoutTag_WhenServiceNull_ShouldThrow()
        {
            Assert.Throws<Exception>(() => locator.Register<IEnumerable<GameObject>>(null));
        }

        [Test]
        public void RegisterServiceWithTag_WhenServiceNull_ShouldThrow()
        {
            Assert.Throws<Exception>(() => locator.Register<IEnumerable<GameObject>>(null, tag1));
        }

        [Test]
        public void UnregisterServiceWithoutTag_WhenServiceNotRegistered_ShouldThrow()
        {
            Assert.Throws<Exception>(locator.Unregister<IEnumerable<GameObject>>);
        }

        [Test]
        public void UnregisterServiceWithoutTag_WhenServiceRegistered_ShouldNotThrow()
        {
            locator.Register<IEnumerable<GameObject>, List<GameObject>>();

            Assert.DoesNotThrow(locator.Unregister<IEnumerable<GameObject>>);
        }

        [Test]
        public void UnregisterServiceWithTag_WhenServiceNotRegistered_ShouldThrow()
        {
            Assert.Throws<Exception>(() => locator.Unregister<IEnumerable<GameObject>>(tag1));
        }

        [Test]
        public void UnregisterServiceWithTag_WhenServiceRegistered_ShouldNotThrow()
        {
            locator.Register<IEnumerable<GameObject>, List<GameObject>>(tag1);

            Assert.DoesNotThrow(() => locator.Unregister<IEnumerable<GameObject>>(tag1));
        }

        [Test]
        public void GetServiceWithoutTag_WhenServiceNotRegistered_ShouldThrow()
        {
            Assert.Throws<Exception>(() => locator.Get<IEnumerable<GameObject>>());
        }

        [Test]
        public void GetServiceWithoutTag_WhenServiceRegistered_ShouldNotBeNull()
        {
            locator.Register<IEnumerable<GameObject>, List<GameObject>>();

            var service = locator.Get<IEnumerable<GameObject>>();

            Assert.NotNull(service);
        }

        [Test]
        public void GetServiceWithTag_WhenServiceNotRegistered_ShouldThrow()
        {
            Assert.Throws<Exception>(() => locator.Get<IEnumerable<GameObject>>(tag1));
        }

        [Test]
        public void GetServiceWithTag_WhenServiceRegistered_ShouldNotBeNull()
        {
            locator.Register<IEnumerable<GameObject>, List<GameObject>>(tag1);

            var service = locator.Get<IEnumerable<GameObject>>(tag1);

            Assert.NotNull(service);
        }

        [Test]
        public void GetServicesWithSameTag_WhenServiceRegisteredWithDifferentType_NeitherShouldNotBeNull()
        {
            locator.Register<IEnumerable<GameObject>, List<GameObject>>(tag1);
            locator.Register<IList<GameObject>, List<GameObject>>(tag1);

            var service1 = locator.Get<IEnumerable<GameObject>>(tag1);
            var service2 = locator.Get<IList<GameObject>>(tag1);

            Assert.NotNull(service1);
            Assert.NotNull(service2);
        }

        [Test]
        public void GetServicesWithSameType_WhenServiceRegisteredWithDifferentTag_NeitherShouldNotBeNull()
        {
            locator.Register<IEnumerable<GameObject>, List<GameObject>>(tag1);
            locator.Register<IEnumerable<GameObject>, List<GameObject>>(tag2);

            var service1 = locator.Get<IEnumerable<GameObject>>(tag1);
            var service2 = locator.Get<IEnumerable<GameObject>>(tag2);

            Assert.NotNull(service1);
            Assert.NotNull(service2);
        }

        [Test]
        public void GetAllServices_WhenRegisteredWithAndWithoutTag_ShouldGetBoth()
        {
            locator.Register<IEnumerable<GameObject>, List<GameObject>>();
            locator.Register<IEnumerable<GameObject>, List<GameObject>>(tag1);

            var services = locator.GetAll<IEnumerable<GameObject>>();

            Assert.AreEqual(2, services.Count);
        }

        [Test]
        public void GetServices_WhenServicesCleared_ShouldThrow()
        {
            locator.Register<IEnumerable<GameObject>, List<GameObject>>();
            locator.Register<IEnumerable<GameObject>, List<GameObject>>(tag1);

            locator.Clear();

            Assert.Throws<Exception>(() => locator.Get<IEnumerable<GameObject>>());
            Assert.Throws<Exception>(() => locator.Get<IEnumerable<GameObject>>(tag1));
        }

        private void GivenAnAlreadyRegisteredServiceWithoutTag<T, S>() where S : T, new()
        {
            locator.Register<T, S>();
        }

        private void GivenAnAlreadyRegisteredServiceWithTag<T, S>(string tag) where S : T, new()
        {
            locator.Register<T, S>(tag);
        }

        private async Task ThenRegisterServiceThrows_WhenRegisteringAnAlreadyRegisteredServiceWithoutTag<T, S>() where S : T, new()
        {
            Assert.ThrowsAsync<Exception>(async () => locator.Register<T, S>());
        }

        private async Task ThenRegisterServiceDoesNotThrow_WhenServiceNotRegisteredWithoutTag<T, S>() where S : T, new()
        {
            Assert.DoesNotThrowAsync(async () => locator.Register<T, S>());
        }

        private void ThenRegisterServiceDoesNotThrow_WhenServiceNotRegisteredWithTag<T, S>(string tag) where S : T, new()
        {
            Assert.DoesNotThrow(() => locator.Register<T, S>(tag));
        }

        private void ThenRegisterServiceThrows_WhenRegisteringAnAlreadyRegisteredServiceWithTag<T, S>(string tag) where S : T, new()
        {
            Assert.Throws<Exception>(() => locator.Register<T, S>(tag));
        }
    }
}
