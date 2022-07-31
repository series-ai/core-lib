using System.Collections.Generic;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public class CommandQueue : ICommandQueue
    {
        private Queue<ICommand> commands;
        private ICommand currentCommand;

        public bool IsExecuting { get; private set; }

        private bool IsQueueEmpty => commands.Count <= 0;

        public CommandQueue()
        {
            commands = new Queue<ICommand>();
        }

        public void QueueCommand(ICommand command)
        {
            commands.Enqueue(command);
        }

        public void Execute()
        {
            if (IsExecuting || IsQueueEmpty)
            {
                return;
            }

            IsExecuting = true;

            ExecuteNextCommand();
        }

        private void OnCommandFinished(OpResult result)
        {
            currentCommand.OnFinish -= OnCommandFinished;

            if(result == OpResult.Failed)
            {
                Debug.LogError($"Command execution failed: {currentCommand}.");
            }

            if(IsQueueEmpty)
            {
                ResetQueue();
            }
            else
            {
                ExecuteNextCommand();
            }
        }

        private void ResetQueue()
        {
            IsExecuting = false;
            currentCommand = null;
        }

        private void ExecuteNextCommand()
        {
            if (IsQueueEmpty)
            {
                return;
            }

            currentCommand = commands.Dequeue();
            currentCommand.OnFinish += OnCommandFinished;
            currentCommand.Execute();
        }
    }
}