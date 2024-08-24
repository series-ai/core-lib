using UnityEngine;
using UnityEngine.SceneManagement;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public static class ApplicationBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static async void BootstrapApplication()
        {
            var settings = Resources.Load<Settings>(Constants.SETTINGS_OBJECT_NAME);

            if(settings == null)
            {
                Debug.LogError("Failed to initialize application. Could not find settings object.", DebugChannels.APP_LIFE_CYCLE);
                return;
            }

            PadoruApplication.ConfigLog(settings);

            if (!ShouldInitializeFramework(settings))
            {
                Debug.Log("Will not initialize Padoru Framework because the current scene is not added to the settings.");
                return;
            }

            await PadoruApplication.Start(settings);
        }

        private static bool ShouldInitializeFramework(Settings settings)
        {
            var activeSceneName = SceneManager.GetActiveScene().name;

            return settings.scenes.Contains(activeSceneName);
        }
    }
}