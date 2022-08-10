using NUnit.Framework;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TestTools;

namespace Padoru.Core.Tests
{
    [TestFixture]
    public class ScreenManagerTests
    {
        private Canvas parentCanvas;

        [OneTimeSetUp]
        public void Setup()
        {
            var canvasGO = new GameObject("ParentCanvas");
            parentCanvas = canvasGO.AddComponent<Canvas>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(parentCanvas.gameObject);
        }

        [Test]
        public void ShowScreen_WhenProviderNotNullAndScreenNotNull_ShouldNotReturnFailedPromise()
        {
            var promiseFailed = false;

            var screenManager = new ScreenManager();
            var provider = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            screenManager.ShowScreen(provider).OnFail((e) =>
            {
                promiseFailed = true;
            });

            Assert.IsFalse(promiseFailed);
        }

        [Test]
        public void ShowScreen_WhenProviderNotNullAndScreenNull_ShouldReturnFailedPromise()
        {
            var promiseFailed = false;

            var screenManager = new ScreenManager();
            var provider = new TestNullScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            LogAssert.Expect(LogType.Error, new Regex(""));

            screenManager.ShowScreen(provider).OnFail((e) => promiseFailed = true);

            Assert.IsTrue(promiseFailed);
        }

        [Test]
        public void ShowScreen_WhenProviderNull_ShouldReturnFailedPromise()
        {
            var promiseFailed = false;

            var screenManager = new ScreenManager();

            screenManager.ParentCanvas = parentCanvas;

            LogAssert.Expect(LogType.Error, new Regex(""));

            screenManager.ShowScreen(null).OnFail((e) => promiseFailed = true);

            Assert.IsTrue(promiseFailed);
        }

        [Test]
        public void CloseScreen_WhenShowed_ShouldNotReturnFailedPromise()
        {
            var promiseFailed = false;

            var screenManager = new ScreenManager();
            var provider = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            screenManager.ShowScreen(provider).OnComplete((screen) =>
            {
                screenManager.CloseScreen(screen).OnFail((e) => promiseFailed = true);
            });

            Assert.IsFalse(promiseFailed);
        }

        [Test]
        public void CloseScreen_WhenNotShowed_ShouldReturnFailedPromise()
        {
            var promiseFailed = false;

            var screenManager = new ScreenManager();
            var provider = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            provider.GetScreen().OnComplete((screen) =>
            {
                screenManager.CloseScreen(screen).OnFail((e) => promiseFailed = true);
            });

            Assert.IsTrue(promiseFailed);
        }

        [Test]
        public void CloseScreen_WhenNull_ShouldReturnFailedPromise()
        {
            var promiseFailed = false;

            var screenManager = new ScreenManager();

            screenManager.ParentCanvas = parentCanvas;

            screenManager.CloseScreen(null).OnFail((e) => promiseFailed = true);

            Assert.IsTrue(promiseFailed);
        }

        [Test]
        public void CloseScreen_WhenCleared_ShouldReturnFailedPromise()
        {
            var promiseFailed = false;

            var screenManager = new ScreenManager();
            var provider = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            screenManager.ShowScreen(provider).OnFail((e) => promiseFailed = true).OnComplete((screen) =>
            {
                screenManager.Clear();
                screenManager.CloseScreen(screen).OnFail((e) => promiseFailed = true);
            });

            Assert.IsTrue(promiseFailed);
        }

        [Test]
        public void CloseAndShowScreen_WhenNoScreenOpened_ShouldReturnFailedPromise()
        {
            var promiseFailed = false;

            var screenManager = new ScreenManager();
            var provider = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            screenManager.CloseAndShowScreen(provider).OnFail((e) => promiseFailed = true);

            Assert.IsTrue(promiseFailed);
        }

        [Test]
        public void CloseAndShowScreen_WhenScreenOpened_ShouldNotReturnFailedPromise()
        {
            var promiseFailed = false;

            var screenManager = new ScreenManager();
            var provider1 = new TestScreenProvider();
            var provider2 = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            screenManager.ShowScreen(provider1).OnFail((e) => promiseFailed = true).OnComplete((screen) =>
            {
                screenManager.CloseAndShowScreen(provider2).OnFail((e) => promiseFailed = true);
            });

            Assert.IsFalse(promiseFailed);
        }

        [Test]
        public void CloseAndShowScreen_WhenProviderNull_ShouldReturnFailedPromise()
        {
            var promiseFailed = false;

            var screenManager = new ScreenManager();
            var provider = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            LogAssert.Expect(LogType.Error, new Regex(""));

            screenManager.ShowScreen(provider).OnFail((e) => promiseFailed = true).OnComplete((screen) =>
            {
                screenManager.CloseAndShowScreen(null).OnFail((e) => promiseFailed = true);
            });

            Assert.IsTrue(promiseFailed);
        }

        [Test]
        public void CloseAndShowScreen_WhenProviderNotNullAndScreenNull_ShouldReturnFailedPromise()
        {
            var promiseFailed = false;

            var screenManager = new ScreenManager();
            var provider1 = new TestScreenProvider();
            var provider2 = new TestNullScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            LogAssert.Expect(LogType.Error, new Regex(""));

            screenManager.ShowScreen(provider1).OnFail((e) => promiseFailed = true).OnComplete((screen) =>
            {
                screenManager.CloseAndShowScreen(provider2).OnFail((e) => promiseFailed = true);
            });

            Assert.IsTrue(promiseFailed);
        }
        
        [Test]
        public void GetParentCanvas_WhenCleared_ShouldBeNull()
        {
            var screenManager = new ScreenManager();

            screenManager.ParentCanvas = parentCanvas;

            screenManager.Clear();

            Assert.IsNull(screenManager.ParentCanvas);
        }
    }
}