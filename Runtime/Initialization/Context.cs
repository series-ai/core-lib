using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
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

        public event Action<long> OnInitializationFinish;

        private async void Awake()
        {
            if (registerOnLocator)
            {
                Debug.Log($"Context registered with tag '{gameObject.scene.name}'", DebugChannels.APP_LIFE_CYCLE);
                
                Locator.Register(this, gameObject.scene.name);
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

            var watch = new Stopwatch();
            watch.Start();
            
            var sb = new StringBuilder();
            sb.Append($"Context {name} initialization finished. Report:");
            sb.Append(Environment.NewLine);

            foreach (var stage in initializationStages)
            {
                await stage.Init(sb);
            }

            watch.Stop();
            sb.Append($"Total initialization time: {watch.ElapsedMilliseconds}. " +
                      "Keep in mind some modules might be initialized in parallel");
            
            Debug.Log(sb, DebugChannels.APP_LIFE_CYCLE, gameObject);

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
                Debug.Log($"{item.GetType()}.Shutdown()", DebugChannels.APP_LIFE_CYCLE);
                
                item.Shutdown();
            }

            IsInitialized = false;

            Debug.Log($"Context shutdown: {gameObject.name}", DebugChannels.APP_LIFE_CYCLE, gameObject);
        }
    }
}
