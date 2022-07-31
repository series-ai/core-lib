using System;

namespace Padoru.Core
{
    public interface ICommand
    {
        event Action<OpResult> OnFinish;

        void Execute();
    }
}