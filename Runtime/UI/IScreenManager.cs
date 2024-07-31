﻿using System.Threading;
using UnityEngine;
using System.Threading.Tasks;

namespace Padoru.Core
{
    public interface IScreenManager<TScreenId>
    {
        void Init(IScreenProvider<TScreenId> provider, Canvas parentCanvas);
        Task ShowScreen(TScreenId id, CancellationToken cancellationToken);
        Task ShowScreen(TScreenId id, Transform parent, CancellationToken cancellationToken);
        Task CloseScreen(TScreenId id, CancellationToken cancellationToken);
        Task CloseAndShowScreen(TScreenId id, CancellationToken cancellationToken);
        bool IsScreenOpened(TScreenId id);
        Task Clear(CancellationToken cancellationToken);
    }
}
