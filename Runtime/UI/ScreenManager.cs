using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public class ScreenManager : IScreenManager
    {
        private List<IScreen> screens = new List<IScreen>();

        private IScreen currentScreen => screens.LastOrDefault();

        public Canvas ParentCanvas { get; set; }

        public IScreen ShowScreen(IScreenProvider provider)
        {
            if(ParentCanvas == null)
            {
                throw new Exception("ParentCanvas is not set. Cannot show screen");
            }
            
            if(provider == null)
            {
                throw new Exception("ScreenProvider is null. Cannot show screen");
            }

            var screen = provider.GetScreen(ParentCanvas.transform);
            
            if (screen == null)
            {
                throw new Exception("Screen is null. Cannot show screen");
            }
            
            screens.Add(screen);
            screen.Show();

            return screen;
        }

        public void CloseScreen(IScreen screen)
        {
            if (screen == null)
            {
                throw new Exception("Trying to close a null screen");
            }

            if (!screens.Contains(screen))
            {
                throw new Exception("Trying to close a closed screen");
            }
            
            screens.Remove(screen);
            screen.Close();
        }

        public void CloseAndShowScreen(IScreenProvider provider)
        {
            CloseScreen(currentScreen);
            
            ShowScreen(provider);
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
    }
}
