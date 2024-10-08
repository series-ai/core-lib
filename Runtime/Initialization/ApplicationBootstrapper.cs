using System.Threading.Tasks;
using Padoru.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public static class ApplicationBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static async void StartApplication()
        {
            var settings = Resources.Load<Settings>(Constants.SETTINGS_OBJECT_NAME);

            if(settings == null)
            {
                Debug.LogError($"Failed to initialize application. Could not find settings object.", DebugChannels.FSM);
                return;
            }

            ConfigLog(settings);

            if (ShouldInitializeFramework(settings))
            {
                await SetupProjectContext(settings);
            }
        }

        private static void ConfigLog(Settings settings)
        {
            Debug.Configure(settings.logSettings, new UnityDefaultLogFormatter(), new UnityDefaultStackTraceFormatter());
            Debug.AddOutput(new UnityConsoleOutput());
        }

        private static async Task SetupProjectContext(Settings settings)
        {
            var projectContextPrefab = Resources.Load<Context>(settings.ProjectContextPrefabName);

            if (projectContextPrefab == null)
            {
                Debug.LogError("Could not find ProjectContext.", DebugChannels.INIT);
                return;
            }

            Debug.Log($"Instantiating ProjectContext", DebugChannels.INIT);
            var projectContext = Object.Instantiate(projectContextPrefab);
            Object.DontDestroyOnLoad(projectContext);

            Debug.Log($"ProjectContext registered to the Locator under the tag: {settings.ProjectContextPrefabName}", DebugChannels.INIT);
            Locator.Register(projectContext, settings.ProjectContextPrefabName);

            Debug.Log($"Initializing ProjectContext", DebugChannels.INIT);
            await projectContext.Init();
        }

        private static bool ShouldInitializeFramework(Settings settings)
        {
            var activeSceneName = SceneManager.GetActiveScene().name;

            return settings.scenes.Contains(activeSceneName);
        }
    }
}