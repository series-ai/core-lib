using System;
using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        public async Task ShowScreen_WhenProviderNotNullAndScreenNotNullAndScreenNotShowed_ShouldNotThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);

            Assert.DoesNotThrowAsync(async () => await screenManager.ShowScreen(screenId, default));
        }

        [Test]
        public async Task ShowScreen_WhenProviderNotNullAndScreenNull_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestNullScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);

            Assert.ThrowsAsync<Exception>(async () => await screenManager.ShowScreen(screenId, default));
        }

        [Test]
        public async Task ShowScreen_WhenNotInitialized_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var screenManager = new ScreenManager<TestScreenId>();
            var provider = new TestNullScreenProvider();
            
            screenManager.Init(provider, parentCanvas);
            
            Assert.ThrowsAsync<Exception>(async () => await screenManager.ShowScreen(screenId, default));
        }
        
        [Test]
        public async Task ShowScreen_WhenProviderNotNullAndScreenNotNullAndIsShowed_ShouldLogWarning()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId, default);
            await screenManager.ShowScreen(screenId, default);
            
            LogAssert.Expect(LogType.Warning, new Regex(string.Empty));
        }
        
        [Test]
        public async Task ShowScreen_WhenOpenedAndCleared_ShouldNotThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId, default);
            await screenManager.Clear(default);
            
            Assert.DoesNotThrowAsync(async () => await screenManager.ShowScreen(screenId, default));
        }

        [Test]
        public async Task CloseScreen_WhenShowed_ShouldNotThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId, default);
            
            Assert.DoesNotThrowAsync(async () => await screenManager.CloseScreen(screenId, default));
        }

        [Test]
        public async Task CloseScreen_WhenNotShowed_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            
            Assert.ThrowsAsync<Exception>(async () => await screenManager.CloseScreen(screenId, default));
        }

        [Test]
        public async Task CloseScreen_WhenCleared_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId, default);
            
            await screenManager.Clear(default);
            
            Assert.ThrowsAsync<Exception>(async () => await screenManager.CloseScreen(screenId, default));
        }

        [Test]
        public async Task CloseAndShowScreen_WhenProviderNotNullAndScreenNotNullAndScreenShowed_ShouldNotThrowException()
        {
            var screenId = TestScreenId.Test;
            var screenId2 = TestScreenId.Test2;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId, default);

            Assert.DoesNotThrowAsync(async () => await screenManager.CloseAndShowScreen(screenId2, default)); 
        }

        [Test]
        public async Task CloseAndShowScreen_WhenProviderNotNullAndScreenNotNullAndScreenNotShowed_ShouldNotThrowException()
        {
            var screenId = TestScreenId.Test;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);

            Assert.DoesNotThrowAsync(async () => await screenManager.CloseAndShowScreen(screenId, default)); 
        }
        
        [Test]
        public async Task CloseAndShowScreen_WhenProviderNullAndScreenNotNullAndScreenShowed_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var screenId2 = TestScreenId.Test2;
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(null, parentCanvas);
            await screenManager.ShowScreen(screenId, default);

            Assert.ThrowsAsync<Exception>(async () => await screenManager.CloseAndShowScreen(screenId2, default)); 
        }
        
        [Test]
        public async Task CloseAndShowScreen_WhenProviderNotNullAndScreenNullAndScreenShowed_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var screenId2 = TestScreenId.Test2;
            var provider = new TestNullScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId, default);

            Assert.ThrowsAsync<Exception>(async () => await screenManager.CloseAndShowScreen(screenId2, default)); 
        }
        
        [Test]
        public async Task CloseAndShowScreen_WhenCleared_ShouldThrowException()
        {
            var screenId = TestScreenId.Test;
            var screenId2 = TestScreenId.Test2;
            var provider = new TestScreenProvider();
            var screenManager = new ScreenManager<TestScreenId>();
            
            screenManager.Init(provider, parentCanvas);
            await screenManager.ShowScreen(screenId, default);
            await screenManager.Clear(default);

            Assert.ThrowsAsync<Exception>(async () => await screenManager.CloseAndShowScreen(screenId2, default)); 
        }
    }
}