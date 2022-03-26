using UnityEngine;

namespace Padoru.Core
{
    public interface IScreenManager
    {
        Transform ParentCanvas { get; set; }
        IPromise<IScreen> ShowScreen(IScreenProvider provider); 
        IPromise CloseScreen(IScreen screen);
        IPromise<IScreen> CloseAndShowScreen(IScreenProvider provider);
        void Clear();
    }
}
