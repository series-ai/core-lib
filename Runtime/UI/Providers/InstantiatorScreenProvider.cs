using System;
using UnityEngine;

namespace Padoru.Core
{
    [CreateAssetMenu(menuName = "Padoru/UI/ScreenProviders/InstantiatorScreenProvider")]
    public class InstantiatorScreenProvider : ScriptableScreenProvider
    {
        [SerializeField] private GameObject screenPrefab;

        public override IScreen GetScreen(Transform parent)
        {
            var go = Instantiate(screenPrefab, parent);
            var screen = go.GetComponent<IScreen>();
            
            return screen;
        }
    }
}