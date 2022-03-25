using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Padoru.Core
{
    public class ScreenManager : MonoBehaviour, IScreenManager, IInitializable, IShutdowneable
    {
        private List<IScreen> screens = new List<IScreen>();

        private IScreen currentScreen => screens.LastOrDefault();

        public Transform ParentCanvas { get; set; }

        public void Init()
        {
            Locator.RegisterService<IScreenManager>(this);
        }

        public void Shutdown()
        {
            Locator.UnregisterService<IScreenManager>();
        }

        public IPromise ShowScreen(IScreenProvider provider)
        {
            if(provider == null)
            {
                throw new Exception($"Cannot show a null screen");
            }

            var promise = new Promise();
            LoadScreen(provider).OnComplete((screen) =>
            {
                PresentScreen(screen).OnComplete(promise.Complete);
            });
            return promise;
        }

        public IPromise CloseScreen(IScreen screen)
        {
            return DismissScreen(screen).OnComplete(() => 
            {
                DisposeScreen(screen);
            });
        }

        public IPromise CloseAndShowScreen(IScreenProvider provider)
        {
            var promise = new Promise();
            CloseScreen(currentScreen).OnComplete(() =>
            {
                ShowScreen(provider).OnComplete(promise.Complete);
            });
            return promise;
        }

        public void Clear()
        {
            ParentCanvas = null;

            foreach (var screen in screens)
            {
                CloseScreen(screen);
            }
        }

        private IPromise<IScreen> LoadScreen(IScreenProvider provider)
        {
            if (ParentCanvas == null)
            {
                return PromiseFactory.CreateFailed<IScreen>(new Exception("ParentCanvas is not set"));   
            }

            return provider.GetScreen(ParentCanvas).OnComplete(screen => screen.Initialize());
        }

        private IPromise PresentScreen(IScreen screen)
        {
            screens.Add(screen);

            screen.Focus();
            return screen.PlayIntroAnimation();
        }

        private IPromise DismissScreen(IScreen screen)
        {
            screen.Unfocus();
            return screen.PlayOutroAnimation();
        }

        private void DisposeScreen(IScreen screen)
        {
            screens.Remove(screen);
            screen.Dispose();
        }
    }
}
