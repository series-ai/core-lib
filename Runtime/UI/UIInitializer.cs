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
            screenManager = new ScreenManager();
            Locator.RegisterService(screenManager);

            if (canvas == null)
            {
                Debug.LogError($"Could not initialize {nameof(UIInitializer)} due to null canvas parent", gameObject);
                return;
            }

            screenManager.ParentCanvas = canvas;

            if (initialScreenProvider != null)
            {
                screenManager.ShowScreen(initialScreenProvider);
            }
        }

        public void Shutdown()
        {
            if(screenManager == null)
            {
                return;
            }

            screenManager.Clear();

            Locator.UnregisterService<IScreenManager>();
        }
    }
}