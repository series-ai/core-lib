using System;

namespace Padoru.Core.Tests
{
    public class TestCommand : ICommand
    {
        private OpResult result;
        
        public event Action<OpResult> OnFinish;

        public TestCommand(OpResult result)
        {
            this.result = result;
        }

        public void Execute()
        {
            OnFinish?.Invoke(result);
        }
    }
}