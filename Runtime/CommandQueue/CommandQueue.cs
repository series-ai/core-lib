using System;
using System.Collections.Generic;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public class CommandQueue : ICommandQueue
    {
        private Queue<ICommand> commands;
        private ICommand currentCommand;

        private bool IsQueueEmpty => commands.Count <= 0;

        public CommandQueue()
        {
            commands = new Queue<ICommand>();
        }

        public void QueueCommand(ICommand command)
        {
            if(command == null)
            {
                throw new Exception($"Cannot register a null command");
            }

            if (commands.Contains(command))
            {
                throw new Exception($"Command already registered {command}");
            }

            commands.Enqueue(command);
        }

        public void Execute()
        {
            if (IsQueueEmpty)
            {
                return;
            }

            ExecuteNextCommand();
        }

        private void OnCommandFinished(OpResult result)
        {
            currentCommand.OnFinish -= OnCommandFinished;

            if(result == OpResult.Failed)
            {
                Debug.LogWarning($"Command execution failed: {currentCommand}.", Constants.DEBUG_CHANNEL_NAME);
            }

            if(IsQueueEmpty)
            {
                OnFinishExecuting();
            }
            else
            {
                ExecuteNextCommand();
            }
        }

        private void OnFinishExecuting()
        {
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

            try
            {
                currentCommand.Execute();
            }
            catch (Exception e)
            {
                OnCommandFinished(OpResult.Failed);
                Debug.LogException(e, Constants.DEBUG_CHANNEL_NAME);
            }
        }
    }
}