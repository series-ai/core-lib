using UnityEngine;

namespace Padoru.Core
{
    public interface IScreenManager
    {
        Transform ParentCanvas { get; set; }
        IPromise ShowScreen(IScreenProvider provider); 
        IPromise CloseScreen(IScreen screen);
        IPromise CloseAndShowScreen(IScreenProvider provider);
        void Clear();
    }
}
