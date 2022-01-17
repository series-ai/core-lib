using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    [DefaultExecutionOrder(-2000)]
    public class Context : MonoBehaviour
    {
        [SerializeField] private bool initializeOnAwake = true;

        private bool initialized;

        private void Awake()
        {
            if (initializeOnAwake)
            {
                Init();
            }
        }

        public void Init()
        {
            if (initialized)
            {
                Debug.LogWarning($"Trying to initialize context more than once", gameObject);
                return;
            }

            var initializables = GetComponentsInChildren<IInitializable>();
            foreach (var item in initializables)
            {
                item.Init();
            }

            initialized = true;
        }

        public void Shutdown()
        {
            if (!initialized)
            {
                Debug.LogWarning($"Trying to shutdown context more than once", gameObject);
                return;
            }

            var shutdownables = GetComponentsInChildren<IShutdowneable>();
            foreach (var item in shutdownables)
            {
                item.Shutdown();
            }

            initialized = false;
        }
    }
}
