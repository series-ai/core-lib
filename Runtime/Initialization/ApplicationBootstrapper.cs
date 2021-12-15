using UnityEngine;

namespace Padoru.Core
{
    public static class ApplicationBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void StartApplication()
        {
            var fsm = Resources.Load<GameObject>(Constants.FSM_PREFAB_NAME);

            if(fsm == null)
            {
                Debug.LogError("Could not find GameFSM.");
                return;
            }

            Debug.Log($"Instantiating GameFSM");
            Object.Instantiate(fsm);
        }
    }
}