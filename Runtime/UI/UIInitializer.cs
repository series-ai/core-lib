using System;
using UnityEngine;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public class UIInitializer : MonoBehaviour, IInitializable, IShutdowneable
    {
        [SerializeField] private InstantiatorScreenProvider initialScreenProvider;
        [SerializeField] private Canvas canvas;

        private IScreenManager screenManager;

        public void Init()
        {
            if(initialScreenProvider == null || canvas == null)
            {
                Debug.LogError($"Could not initialize {nameof(UIInitializer)} due to missing references");
                return;
            }

            screenManager = Locator.GetService<IScreenManager>();
            screenManager.ParentCanvas = canvas.transform;
            screenManager.ShowScreen(initialScreenProvider).OnFail(LogException);
        }

        public void Shutdown()
        {
            if(screenManager == null)
            {
                return;
            }

            screenManager.Clear();
        }

        private void LogException(Exception e)
        {
            Debug.LogException(e);
        }
    }
}