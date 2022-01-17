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
            ConfigLog();
            SetupProjectContext();
        }

        private static void ConfigLog()
        {
            var logSettings = new LogSettings()
            {
                LogType = Padoru.Diagnostics.LogType.Info,
                StacktraceLogType = Padoru.Diagnostics.LogType.Info,
            };

            Debug.Configure(logSettings, new UnityDefaultLogFormatter(), new UnityDefaultStackTraceFormatter());
            Debug.AddOutput(new UnityConsoleOutput());
        }

        private static void SetupProjectContext()
        {
            var projectContextPrefab = Resources.Load<Context>(Constants.PROJECT_CONTEXT_PREFAB_NAME);

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