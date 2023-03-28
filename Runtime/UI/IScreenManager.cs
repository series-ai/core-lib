using UnityEngine;
using System.Threading.Tasks;

namespace Padoru.Core
{
    public interface IScreenManager<TScreenId>
    {
        void Init(IScreenProvider<TScreenId> provider, Canvas parentCanvas);
        Task<IScreen> ShowScreen(TScreenId id); 
        Task CloseScreen(TScreenId id);
        Task<IScreen> CloseAndShowScreen(TScreenId id);
        void Clear();
    }
}
