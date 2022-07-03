using UnityEngine;

namespace Padoru.Core
{
    public abstract class ScriptableScreenProvider : ScriptableObject, IScreenProvider
    {
        public abstract IPromise<IScreen> GetScreen(Transform parent);
    }
}