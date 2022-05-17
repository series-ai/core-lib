using System;
using UnityEngine;

namespace Padoru.Core
{
    [CreateAssetMenu(menuName = "Padoru/UI/ScreenProviders/InstantiatorScreenProvider")]
    public class InstantiatorScreenProvider : ScriptableScreenProvider
    {
        [SerializeField] private GameObject screenPrefab;

        public override IPromise<IScreen> GetScreen(Transform parent = null)
        {
            var promise = new Promise<IScreen>();

            var go = Instantiate(screenPrefab, parent);
            var screen = go.GetComponent<IScreen>();

            if(screen == null)
            {
                promise.Fail(new Exception($"Screen provider screen does not contain an IScreen component"));
            }

            promise.Complete(screen);

            return promise;
        }
    }
}