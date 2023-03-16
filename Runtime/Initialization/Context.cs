using System;
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
        // TODO: Get the modules automatically when detecting changes in the hierarchy and only allow to change order
        [SerializeField] private GameObject[] modules;

        public bool IsInitialized { get; private set; }

        private async void Awake()
        {
            if (registerOnLocator)
            {
                Debug.Log($"Context registered with tag '{gameObject.scene.name}'");
                
                Locator.RegisterService<Context>(this, gameObject.scene.name);
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
                Locator.UnregisterService<Context>(gameObject.scene.name);
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

            if (modules != null)
            {
                for (int i = 0; i < modules.Length; i++)
                {
                    var module = modules[i];
                    var initializable = module.GetComponent<IInitializable>();
                    if (initializable != null)
                    {
                        initializable.Init();
                    }
                    else
                    {
                        var initializableAsync = module.GetComponent<IInitializableAsync>();
                        if (initializableAsync != null)
                        {
                            await initializableAsync.Init();
                        }
                    }
                }
            }

            IsInitialized = true;
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

            IsInitialized = false;

            Debug.Log($"Context shutdown: {gameObject.name}", gameObject);
        }
    }
}
