using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Padoru.Core.ActionRouter;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    [DefaultExecutionOrder(-2000)]
    public class Context : MonoBehaviour
    {
        [SerializeField] private bool initializeOnAwake = true;
        [SerializeField] private bool registerOnLocator = true;
        [SerializeField] private InitializationStage[] initializationStages;

        public bool IsInitialized { get; private set; }
        
        public float InitializationPercentage { get; private set; }

        public event Action<long> OnInitializationFinish;
        public event Action<float> OnInitializationStageFinished; 
        private async void Awake()
        {
            if (registerOnLocator)
            {
                Debug.Log($"Context registered with tag '{gameObject.scene.name}'");
                
                Locator.Register<Context>(this, gameObject.scene.name);
            }
            
            if (initializeOnAwake)
            {
                await Init();
            }
        }

        private void OnDestroy()
        {
            if (registerOnLocator)
            {
                Locator.Unregister<Context>(gameObject.scene.name);
            }

            if (IsInitialized)
            {
                Shutdown();
            }
        }

        public async Task Init()
        {
            if (IsInitialized)
            {
                throw new Exception($"Trying to initialize context more than once");
            }
            
            try
            {
                var actionRouter = Locator.Get<IActionRouter>();
                actionRouter.Invoke(ActionRouterEvents.ON_CONTEXT_INIT, this);
            }
            catch (Exception e)
            {
                // ignored
            }

            var watch = new Stopwatch();
            watch.Start();
            
            var sb = new StringBuilder();
            sb.Append($"Context {name} initialization finished. Report:");
            sb.Append(Environment.NewLine);

            for (int i = 0; i < initializationStages.Length; i++)
            {
                var stage = initializationStages[i];
                
                await stage.Init(sb);

                InitializationPercentage = (i + 1f) / initializationStages.Length;
                OnInitializationStageFinished?.Invoke(InitializationPercentage);
            }

            watch.Stop();
            sb.Append($"Total initialization time: {watch.ElapsedMilliseconds}. " +
                      $"Keep in mind some modules might be initialized in parallel");
            
            Debug.Log(sb, gameObject);

            IsInitialized = true;
            OnInitializationFinish?.Invoke(watch.ElapsedMilliseconds);
        }

        public void Shutdown()
        {
            if (!IsInitialized)
            {
                throw new Exception($"Trying to shutdown context when not initialized: {gameObject.name}");
            }

            var shutdownables = GetComponentsInChildren<IShutdowneable>();
            foreach (var item in shutdownables)
            {
                item.Shutdown();
            }
            
            try
            {
                var actionRouter = Locator.Get<IActionRouter>();
                actionRouter.Invoke(ActionRouterEvents.ON_CONTEXT_SHUTDOWN, this);
            }
            catch (Exception e)
            {
                // ignored
            }

            IsInitialized = false;

            Debug.Log($"Context shutdown: {gameObject.name}", gameObject);
        }
    }
}
