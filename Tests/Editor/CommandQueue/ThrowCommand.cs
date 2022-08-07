using System;

namespace Padoru.Core.Tests
{
    public class ThrowCommand : ICommand
    {
        public event Action<OpResult> OnFinish;

        private string exceptionMessage;

        public ThrowCommand(string exceptionMessage)
        {
            this.exceptionMessage = exceptionMessage;
        }

        public void Execute()
        {
            throw new Exception(exceptionMessage);
        }
    }
}