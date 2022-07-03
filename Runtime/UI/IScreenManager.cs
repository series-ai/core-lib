using UnityEngine;

namespace Padoru.Core
{
    public interface IScreenManager
    {
        Canvas ParentCanvas { get; set; }
        IPromise<IScreen> ShowScreen(IScreenProvider provider); 
        IPromise CloseScreen(IScreen screen);
        IPromise<IScreen> CloseAndShowScreen(IScreenProvider provider);
        void Clear();
    }
}
