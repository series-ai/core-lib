﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public class ScreenManager<TScreenId> : IScreenManager<TScreenId>
    {
        private readonly Dictionary<TScreenId, IScreen> screens = new();
        private readonly List<TScreenId> activeScreens = new();
        private IScreenHandler<TScreenId> handler;
        private Canvas parentCanvas;

        private TScreenId CurrentActiveScreen => activeScreens.LastOrDefault();
        
        public event Action<TScreenId> OnScreenShown;
        public event Action<TScreenId> OnScreenClosed;

        public void Init(IScreenHandler<TScreenId> handler, Canvas parentCanvas)
        {
            if (handler == null)
            {
                throw new Exception("The provider reference is null");
            }
            
            if (parentCanvas == null)
            {
                throw new Exception("The parent canvas reference is null");
            }
            
            this.handler = handler;
            this.parentCanvas = parentCanvas;
        }

        public async Task ShowScreen(TScreenId id, CancellationToken cancellationToken)
        {
            await ShowScreen(id, parentCanvas.transform, cancellationToken);
        }

        public async Task ShowScreen(TScreenId id, Transform parent, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                throw new Exception("Cannot Show screen. Provided screen id is null");
            }
            
            if (parent == null)
            {
                throw new Exception("Parent is null. Cannot show screen");
            }
            
            if (handler == null)
            {
                throw new Exception("ScreenProvider is null. Cannot show screen");
            }

            if (screens.ContainsKey(id))
            {
                return;
            }
            
            var screen = handler.GetScreen(id, parent);
            
            if (screen == null)
            {
                throw new Exception($"Unable to show screen of id '{id}' because the provider returned null");
            }
            
            activeScreens.Add(id);
            screens.Add(id, screen);
            
            await screen.Show(cancellationToken);
            OnScreenShown?.Invoke(id);
        }

        public async Task CloseScreen(TScreenId id, CancellationToken cancellationToken)
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
            
            await screen.Close(cancellationToken);
            
            handler.DisposeScreen(id);
            activeScreens.Remove(id);
            screens.Remove(id);
            OnScreenClosed?.Invoke(id);
        }
        
        public void CloseScreenImmediately(TScreenId id)
        {
            if (id == null)
            {
                throw new Exception("Cannot close screen. Provided screen id is null");
            }
            
            if (!screens.ContainsKey(id))
            {
                throw new Exception("Trying to close a closed screen");
            }

            handler.DisposeScreen(id);
            activeScreens.Remove(id);
            screens.Remove(id);
            OnScreenClosed?.Invoke(id);
        }

        /// <summary>
        /// Closes the current active screen, if there is any, then shows the requested screen
        /// </summary>
        /// <param name="id">The id of the screen to show</param>
        /// <returns></returns>
        public async Task CloseAndShowScreen(TScreenId id, CancellationToken cancellationToken)
        {
            if (CurrentActiveScreen != null)
            {
                await CloseScreen(CurrentActiveScreen, cancellationToken);
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            
            await ShowScreen(id, cancellationToken);
        }

        public bool IsScreenOpened(TScreenId id)
        {
            return activeScreens.Contains(id);
        }

        public async Task Clear(CancellationToken cancellationToken)
        {
            var screensList = screens.Keys.ToList();

            for (var i = screensList.Count - 1; i >= 0; i--)
            {
                await CloseScreen(screensList[i], cancellationToken);
                
                if (cancellationToken.IsCancellationRequested)
                {
                    return;
                }
            }
        }
    }
}
