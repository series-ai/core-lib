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
        public void Init_WhenProviderNotNullAndCanvasNotNull_ShouldNotThrowException()
        {
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            Assert.DoesNotThrow(() => screenManager.Init(provider, parentCanvas));
        }
        
        [Test]
        public void Init_WhenProviderNotNullAndCanvasNull_ShouldThrowException()
        {
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            Assert.Throws<Exception>(() => screenManager.Init(provider, null));
        }
        
        [Test]
        public void Init_WhenProviderNullAndCanvasNotNull_ShouldThrowException()
        {
            var screenManager = new ScreenManager<TestScreenId>();
            
            Assert.Throws<Exception>(() => screenManager.Init(null, parentCanvas));
        }

        [Test]
        public async void ShowScreen_WhenProviderNotNullAndScreenNotNullAndScreenNotShowed_ShouldNotThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);

            Assert.DoesNotThrow(async () => await screenManager.ShowScreen(screenId));
        }

        [Test]
        public async void ShowScreen_WhenProviderNotNullAndScreenNull_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestNullScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);

            Assert.Throws<Exception>(async () => await screenManager.ShowScreen(screenId));
        }

        [Test]
        public async void ShowScreen_WhenNotInitialized_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var screenManager = new ScreenManager<TestScreenId>();

            Assert.Throws<Exception>(async () => await screenManager.ShowScreen(screenId));
        }
        
        [Test]
        public async void ShowScreen_WhenProviderNotNullAndScreenNotNullAndIsShowed_ShouldLogWarning()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId);
            await screenManager.ShowScreen(screenId);
            
            LogAssert.Expect(LogType.Warning, new Regex(string.Empty));
        }
        
        [Test]
        public async void ShowScreen_WhenOpenedAndCleared_ShouldNotThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId);
            screenManager.Clear();
            
            Assert.DoesNotThrow(async () => await screenManager.ShowScreen(screenId));
        }

        [Test]
        public async void CloseScreen_WhenShowed_ShouldNotThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId);
            
            Assert.DoesNotThrow(async () => await screenManager.CloseScreen(screenId));
        }

        [Test]
        public async void CloseScreen_WhenNotShowed_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            
            Assert.Throws<Exception>(async () => await screenManager.CloseScreen(screenId));
        }

        [Test]
        public async void CloseScreen_WhenCleared_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId);
            
            screenManager.Clear();
            
            Assert.Throws<Exception>(async () => await screenManager.CloseScreen(screenId));
        }

        [Test]
        public async void CloseAndShowScreen_WhenProviderNotNullAndScreenNotNullAndScreenShowed_ShouldNotThrowException()
        {
            var screenId = TestScreenId.Test;
            var screenId2 = TestScreenId.Test2;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId);

            Assert.DoesNotThrow(async () => await screenManager.CloseAndShowScreen(screenId2)); 
        }

        [Test]
        public async void CloseAndShowScreen_WhenProviderNotNullAndScreenNotNullAndScreenNotShowed_ShouldNotThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);

            Assert.DoesNotThrow(async () => await screenManager.CloseAndShowScreen(screenId)); 
        }
        
        [Test]
        public async void CloseAndShowScreen_WhenProviderNullAndScreenNotNullAndScreenShowed_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var screenId2 = TestScreenId.Test2;
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(null, parentCanvas);
            await screenManager.ShowScreen(screenId);

            Assert.Throws<Exception>(async () => await screenManager.CloseAndShowScreen(screenId2)); 
        }
        
        [Test]
        public async void CloseAndShowScreen_WhenProviderNotNullAndScreenNullAndScreenShowed_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var screenId2 = TestScreenId.Test2;
            var provider = new TestNullScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId);

            Assert.Throws<Exception>(async () => await screenManager.CloseAndShowScreen(screenId2)); 
        }
        
        [Test]
        public async void CloseAndShowScreen_WhenCleared_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var screenId2 = TestScreenId.Test2;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId);
            screenManager.Clear();

            Assert.Throws<Exception>(async () => await screenManager.CloseAndShowScreen(screenId2)); 
        }
    }
}