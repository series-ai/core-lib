using System;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    [DefaultExecutionOrder(-2000)]
    public class Context : MonoBehaviour
    {
        [SerializeField] private bool initializeOnAwake = true;

        public bool IsInitialized { get; private set; }

        private void Awake()
        {
            if (initializeOnAwake)
            {
                Init();
            }
        }

        private void OnDestroy()
        {
            Shutdown();
        }

        public void Init()
        {
            if (IsInitialized)
            {
                throw new Exception($"Trying to initialize context more than once {gameObject.name}");
            }

            var initializables = GetComponentsInChildren<IInitializable>();
            foreach (var item in initializables)
            {
                item.Init();
            }

            IsInitialized = true;

            Debug.Log($"Context initialized: {gameObject.name}", gameObject);
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

            Debug.Log($"Context shutdowned: {gameObject.name}", gameObject);
        }
    }
}
