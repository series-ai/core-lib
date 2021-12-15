using UnityEngine;

namespace Padoru.Core
{
    [DefaultExecutionOrder(-2000)]
    public class Context : MonoBehaviour
    {
        [SerializeField] private bool initializeOnAwake = true;

        private void Awake()
        {
            if (initializeOnAwake)
            {
                Init();
            }
        }

        public void Init()
        {
            var initializables = GetComponentsInChildren<IInitializable>();
            foreach (var item in initializables)
            {
                item.Init();
            }
        }

        public void Shutdown()
        {
            var shutdownables = GetComponentsInChildren<IShutdowneable>();
            foreach (var item in shutdownables)
            {
                item.Shutdown();
            }
        }
    }
}
