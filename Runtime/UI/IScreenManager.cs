using UnityEngine;

namespace Padoru.Core
{
    public interface IScreenManager
    {
        Canvas ParentCanvas { get; set; }
        IScreen ShowScreen(IScreenProvider provider); 
        void CloseScreen(IScreen screen);
        void CloseAndShowScreen(IScreenProvider provider);
        void Clear();
    }
}
