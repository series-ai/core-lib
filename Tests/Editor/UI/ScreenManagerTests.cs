using System;
using NUnit.Framework;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

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
        public void ShowScreen_WhenProviderNotNullAndScreenNotNull_ShouldNotThrowException()
        {
            var screenManager = new ScreenManager();
            var provider = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            Assert.DoesNotThrow(() => screenManager.ShowScreen(provider));
        }

        [Test]
        public void ShowScreen_WhenProviderNotNullAndScreenNull_ShouldThrowException()
        {
            var screenManager = new ScreenManager();
            var provider = new TestNullScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            Assert.Throws<Exception>(() => screenManager.ShowScreen(provider));
        }

        [Test]
        public void ShowScreen_WhenProviderNull_ShouldThrowException()
        {
            var screenManager = new ScreenManager();

            screenManager.ParentCanvas = parentCanvas;
            
            Assert.Throws<Exception>(() => screenManager.ShowScreen(null));
        }

        [Test]
        public void CloseScreen_WhenShowed_ShouldNotThrowException()
        {
            var screenManager = new ScreenManager();
            var provider = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            var screen = screenManager.ShowScreen(provider);
            
            Assert.DoesNotThrow(() => screenManager.CloseScreen(screen));
        }

        [Test]
        public void CloseScreen_WhenNotShowed_ShouldThrowException()
        {
            var screenManager = new ScreenManager();
            var provider = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            var screen = provider.GetScreen();

            Assert.Throws<Exception>(() => screenManager.CloseScreen(screen));
        }

        [Test]
        public void CloseScreen_WhenNull_ShouldThrowException()
        {
            var screenManager = new ScreenManager();

            screenManager.ParentCanvas = parentCanvas;
            
            Assert.Throws<Exception>(() => screenManager.CloseScreen(null));
        }

        [Test]
        public void CloseScreen_WhenCleared_ShouldThrowException()
        {
            var screenManager = new ScreenManager();
            var provider = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;

            var screen = screenManager.ShowScreen(provider);

            screenManager.Clear();
            Assert.Throws<Exception>(() => screenManager.CloseScreen(screen));
        }

        [Test]
        public void CloseAndShowScreen_WhenScreenOpened_ShouldNotThrowException()
        {
            var screenManager = new ScreenManager();
            var provider1 = new TestScreenProvider();
            var provider2 = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;
            screenManager.ShowScreen(provider1);
            
            Assert.DoesNotThrow(() => screenManager.CloseAndShowScreen(provider2));
        }

        [Test]
        public void CloseAndShowScreen_WhenNoScreenOpened_ShouldThrowException()
        {
            var screenManager = new ScreenManager();
            var provider = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;
            
            Assert.Throws<Exception>(() => screenManager.CloseAndShowScreen(provider));
        }

        [Test]
        public void CloseAndShowScreen_WhenProviderNull_ShouldThrowException()
        {
            var screenManager = new ScreenManager();
            var provider = new TestScreenProvider();

            screenManager.ParentCanvas = parentCanvas;
            screenManager.ShowScreen(provider);

            Assert.Throws<Exception>(() => screenManager.CloseAndShowScreen(null));
        }

        [Test]
        public void CloseAndShowScreen_WhenProviderNotNullAndScreenNull_ShouldThrowException()
        {
            var screenManager = new ScreenManager();
            var provider1 = new TestScreenProvider();
            var provider2 = new TestNullScreenProvider();

            screenManager.ParentCanvas = parentCanvas;
            screenManager.ShowScreen(provider1);
            
            Assert.Throws<Exception>(() => screenManager.CloseAndShowScreen(provider2));
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