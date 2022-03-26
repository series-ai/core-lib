using UnityEngine;

namespace Padoru.Core
{
    public class ScreenManagerInitializer : MonoBehaviour, IInitializable, IShutdowneable
    {
        public void Init()
        {
            var screenManager = new ScreenManager();

            Locator.RegisterService<IScreenManager>(screenManager);
        }

        public void Shutdown()
        {
            Locator.UnregisterService<IScreenManager>();
        }
    }
}