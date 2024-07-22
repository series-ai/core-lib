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
                Debug.LogError("Failed to initialize application. Could not find settings object.", DebugChannels.APP_LIFE_CYCLE);
                return;
            }

            ConfigLog(settings);

            if (!ShouldInitializeFramework(settings))
            {
                Debug.Log("Will not initialize Padoru Framework because the current scene is not added to the settings.");
                return;
            }
            
            await SetupProjectContext(settings);

            InitializeGameFsm(settings);
        }

        private static void ConfigLog(Settings settings)
        {
            Debug.Configure(settings.logSettings, new UnityDefaultLogFormatter(), new UnityDefaultStackTraceFormatter());
            Debug.AddOutput(new UnityConsoleOutput());
        }

        private static async Task SetupProjectContext(Settings settings)
        {
            var projectContextPrefab = Resources.Load<Context>(settings.projectContextPrefabName);

            if (projectContextPrefab == null)
            {
                Debug.LogError("Could not find ProjectContext.", DebugChannels.APP_LIFE_CYCLE);
                return;
            }

            Debug.Log("Instantiating ProjectContext", DebugChannels.APP_LIFE_CYCLE);
            var projectContext = Object.Instantiate(projectContextPrefab);
            Object.DontDestroyOnLoad(projectContext);

            Debug.Log($"ProjectContext registered to the Locator under the tag: {settings.projectContextPrefabName}", DebugChannels.APP_LIFE_CYCLE);
            Locator.Register(projectContext, settings.projectContextPrefabName);

            Debug.Log("Initializing ProjectContext", DebugChannels.APP_LIFE_CYCLE);
            await projectContext.Init();
        }

        private static void InitializeGameFsm(Settings settings)
        {
            Debug.Log("Initializing GameFSM", DebugChannels.APP_LIFE_CYCLE);
            
            var fsmInitializer = new GameFsmInitializer(settings);
            
            var fsm = fsmInitializer.Init();
            
            Locator.Register(fsm, Constants.GAME_FSM_TAG);
            
            Debug.Log($"GameFSM registered under tag '{Constants.GAME_FSM_TAG}' and type '{fsm.GetType()}'", DebugChannels.APP_LIFE_CYCLE);
        }

        private static bool ShouldInitializeFramework(Settings settings)
        {
            var activeSceneName = SceneManager.GetActiveScene().name;

            return settings.scenes.Contains(activeSceneName);
        }
    }
}