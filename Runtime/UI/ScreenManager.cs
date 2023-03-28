using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public class ScreenManager<TScreenId> : IScreenManager<TScreenId>
    {
        private readonly Dictionary<TScreenId, IScreen> screens = new();
        private readonly List<TScreenId> activeScreens = new List<TScreenId>();
        private IScreenProvider<TScreenId> provider;
        private Canvas parentCanvas;

        private TScreenId CurrentActiveScreen => activeScreens.LastOrDefault();

        public void Init(IScreenProvider<TScreenId> providerReference, Canvas parentCanvasReference)
        {
            if (providerReference == null)
            {
                throw new Exception("The provider reference is null");
            }
            
            if (parentCanvasReference == null)
            {
                throw new Exception("The parent canvas reference is null");
            }
            
            provider = providerReference;
            parentCanvas = parentCanvasReference;
        }

        public async Task<IScreen> ShowScreen(TScreenId id)
        {
            if (id == null)
            {
                throw new Exception("Cannot Show screen. Provided screen id is null");
            }
            
            if (parentCanvas == null)
            {
                throw new Exception("ParentCanvas is not set. Cannot show screen");
            }
            
            if (provider == null)
            {
                throw new Exception("ScreenProvider is null. Cannot show screen");
            }

            if (screens.ContainsKey(id))
            {
                Debug.LogWarning($"Unable to show screen {id} because is already active");
                return screens[id];
            }
            
            var screen = provider.GetScreen(id, parentCanvas.transform);
            
            if (screen == null)
            {
                throw new Exception("Screen is null. Cannot show screen");
            }
            
            activeScreens.Add(id);
            screens.Add(id, screen);
            
            await screen.Show();
            
            return screen;
        }

        public async Task CloseScreen(TScreenId id)
        {
            if (id == null)
            {
                throw new Exception("Cannot close screen. Provided screen id is null");
            }
            
            if (!screens.ContainsKey(id))
            {
                throw new Exception("Trying to close a closed screen");
            }

            var screen = screens[id];
            activeScreens.Remove(id);
            screens.Remove(id);
            await screen.Close();
        }

        /// <summary>
        /// Closes the current active screen, if there is any, then shows the requested screen
        /// </summary>
        /// <param name="id">The id of the screen to show</param>
        /// <returns></returns>
        public async Task<IScreen> CloseAndShowScreen(TScreenId id)
        {
            if (CurrentActiveScreen != null)
            {
                await CloseScreen(CurrentActiveScreen);
            }
            
            return await ShowScreen(id);
        }
        
        public async void Clear()
        {
            var screensList = screens.Keys.ToList();

            for (var i = screensList.Count - 1; i >= 0; i--)
            {
                await CloseScreen(screensList[i]);
            }
        }
    }
}
