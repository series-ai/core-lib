using System;
using Padoru.Core.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public class GameFsmInitializer
    {
        public const string TRIGGER_PREFIX = "GoTo";
        
        private readonly Settings settings;
        
        private FSM<string, string> fsm;
        private string startingState;

        public GameFsmInitializer(Settings settings)
        {
            this.settings = settings;
        }
        
        public IFSM<string, string> Init()
        {
            startingState = GetInitialGameState();

            SetupFsm(startingState);

            HookStateEvents();

            fsm.Start();

            return fsm;
        }
        
        private string GetInitialGameState()
        {
            if (Application.isEditor)
            {
                return SceneManager.GetActiveScene().name;
            }

            if (string.IsNullOrWhiteSpace(settings.initialScene))
            {
                throw new Exception("Initial scene in padoru settings is empty.");
            }

            if (!settings.scenes.Contains(settings.initialScene))
            {
                throw new Exception($"Initial scene '{settings.initialScene}' in padoru settings is not in the defined scenes list.");
            }

            return settings.initialScene;
        }

        private void SetupFsm(string initialState)
        {
            fsm = new FSM<string, string>(initialState, settings.scenes, debugChannel: DebugChannels.GAME_FSM);

            foreach (var currentScene in settings.scenes)
            {
                foreach (var scene in settings.scenes)
                {
                    fsm.AddTransition(currentScene, scene, TRIGGER_PREFIX + scene);
                }
            }
        }

        private void HookStateEvents()
        {
            foreach (var scene in settings.scenes)
            {
                var state = fsm.GetState(scene);
                
                state.OnStateEnterEvent += () => LoadScene(scene);
                state.OnStateExitEvent += () => ShutdownScene(scene);
            }
        }

        private void LoadScene(string nextSceneName)
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            var comesFromDifferentScene = currentSceneName != nextSceneName;
            
            if (!comesFromDifferentScene)
            {
                var scene = SceneManager.GetSceneByName(nextSceneName);
                
                OnSceneLoaded(scene, LoadSceneMode.Single);
            }
            else
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                
                SceneManager.LoadScene(nextSceneName);
            }
        }

        private async void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            try
            {
                // TODO: This is not pretty, see how to improve the flow in the future.
                await TaskUtils.WaitUntil(() => Locator.Has<Context>(scene.name));
                
                var context = Locator.Get<Context>(scene.name);
                
                await context.Init();
            }
            catch (Exception e)
            {
                Debug.LogException($"Failed to initialize context for scene {scene.name}", e);
            }
        }
        
        private void ShutdownScene(string sceneName)
        {
            try
            {
                var sceneContext = Locator.Get<Context>(sceneName);
                
                sceneContext.Shutdown();
            }
            catch (Exception e)
            {
                Debug.LogException($"Failed to shutdown context for scene {sceneName}", e);
            }
        }
    }
}