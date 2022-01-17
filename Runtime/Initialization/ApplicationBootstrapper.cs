using Padoru.Diagnostics;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public static class ApplicationBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void StartApplication()
        {
            var settings = Resources.Load<Settings>(Constants.SETTINGS_OBJECT_NAME);

            if(settings == null)
            {
                Debug.LogError($"Failed to initialize application. Could not find settings object.");
                return;
            }

            ConfigLog(settings);
            SetupProjectContext(settings);
        }

        private static void ConfigLog(Settings settings)
        {
            var logSettings = new LogSettings()
            {
                LogType = settings.LogType,
                StacktraceLogType = settings.StacktraceLogType,
            };

            Debug.Configure(logSettings, new UnityDefaultLogFormatter(), new UnityDefaultStackTraceFormatter());
            Debug.AddOutput(new UnityConsoleOutput());
        }

        private static void SetupProjectContext(Settings settings)
        {
            var projectContextPrefab = Resources.Load<Context>(settings.ProjectContextPrefabName);

            if (projectContextPrefab == null)
            {
                Debug.LogError("Could not find ProjectContext.");
                return;
            }

            Debug.Log($"Instantiating ProjectContext");
            var projectContext = Object.Instantiate(projectContextPrefab);

            Debug.Log($"Initializing ProjectContext");
            projectContext.Init();
        }
    }
}