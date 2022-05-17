using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Padoru.Core
{
    public class ScreenManager : IScreenManager
    {
        private List<IScreen> screens = new List<IScreen>();

        private IScreen currentScreen => screens.LastOrDefault();

        public Transform ParentCanvas { get; set; }

        public IPromise<IScreen> ShowScreen(IScreenProvider provider)
        {
            var promise = new Promise<IScreen>();
            LoadScreen(provider).OnFail((e) => promise.Fail(e)).OnComplete((screen) =>
            {
                PresentScreen(screen).OnFail((e) => promise.Fail(e)).OnComplete(() => promise.Complete(screen));
            });
            return promise;
        }

        public IPromise CloseScreen(IScreen screen)
        {
            if (screen == null)
            {
                return PromiseFactory.CreateFailed(new Exception("Screen is null"));
            }

            if (!screens.Contains(screen))
            {
                return PromiseFactory.CreateFailed(new Exception("Screen is not opened"));
            }

            return DismissScreen(screen).OnComplete(() => 
            {
                DisposeScreen(screen);
            });
        }

        public IPromise<IScreen> CloseAndShowScreen(IScreenProvider provider)
        {
            var promise = new Promise<IScreen>();
            CloseScreen(currentScreen).OnFail((e) => promise.Fail(e)).OnComplete(() =>
            {
                ShowScreen(provider).OnFail((e) => promise.Fail(e)).OnComplete((screen) => promise.Complete(screen));
            });
            return promise;
        }

        public void Clear()
        {
            ParentCanvas = null;

            var screensClone = new List<IScreen>(screens);
            foreach (var screen in screensClone)
            {
                CloseScreen(screen);
            }
            screens.Clear();
        }

        private IPromise<IScreen> LoadScreen(IScreenProvider provider)
        {
            if(provider == null)
            {
                return PromiseFactory.CreateFailed<IScreen>(new Exception("ScreenProvider is null"));
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

        private IPromise DisposeScreen(IScreen screen)
        {
            screens.Remove(screen);
            screen.Dispose();

            return PromiseFactory.CreateCompleted();
        }
    }
}
