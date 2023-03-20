using UnityEngine;

namespace Padoru.Core
{
    public abstract class ScriptableScreenProvider : ScriptableObject, IScreenProvider
    {
        public abstract IScreen GetScreen(Transform parent);
    }
}