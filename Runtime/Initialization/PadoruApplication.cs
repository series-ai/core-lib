using System.Threading.Tasks;
using Padoru.Diagnostics;
using UnityEngine;
using Debug = Padoru.Diagnostics.Debug;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Padoru.Core
{
	public static class PadoruApplication
	{
		public static void Quit()
		{
			Application.Quit();
			
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#endif
		}
		
		internal static void ConfigLog(Settings settings)
		{
			Debug.Configure(settings.logSettings, new UnityDefaultLogFormatter(), new UnityDefaultStackTraceFormatter());
			Debug.AddOutput(new UnityConsoleOutput());
		}

		internal static async Task Start(Settings settings)
		{
			await SetupProjectContext(settings);

			InitializeGameFsm(settings);
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
			Locator.Register(projectContext, Constants.PROJECT_CONTEXT_TAG);

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
	}   
}