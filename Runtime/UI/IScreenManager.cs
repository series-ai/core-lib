using System.Threading;
using UnityEngine;
using System.Threading.Tasks;
using System;

namespace Padoru.Core
{
    public interface IScreenManager<TScreenId>
    {
        void Init(IScreenHandler<TScreenId> handler, Canvas parentCanvas);
        Task ShowScreen(TScreenId id, CancellationToken cancellationToken);
        Task ShowScreen(TScreenId id, Transform parent, CancellationToken cancellationToken);
        Task CloseScreen(TScreenId id, CancellationToken cancellationToken);
        void CloseScreenImmediately(TScreenId id);
        Task CloseAndShowScreen(TScreenId id, CancellationToken cancellationToken);
        bool IsScreenOpened(TScreenId id);
        Task Clear(CancellationToken cancellationToken);
        event Action<TScreenId> OnScreenShown;
    }
}
