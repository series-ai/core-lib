using UnityEngine;
using System.Threading.Tasks;

namespace Padoru.Core
{
    public interface IScreenManager<TScreenId>
    {
        void Init(IScreenProvider<TScreenId> provider, Canvas parentCanvas);
        Task ShowScreen(TScreenId id); 
        Task CloseScreen(TScreenId id);
        Task CloseAndShowScreen(TScreenId id);
        bool IsScreenOpened(TScreenId id);
        void Clear();
    }
}
