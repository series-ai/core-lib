using UnityEngine;

namespace Padoru.Core
{
    public interface IScreenManager<TScreenId>
    {
        void Init(IScreenProvider<TScreenId> provider, Canvas parentCanvas);
        IScreen ShowScreen(TScreenId id); 
        void CloseScreen(TScreenId id);
        void Clear();
    }
}
