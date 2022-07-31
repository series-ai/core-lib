namespace Padoru.Core
{
    public interface ICommandQueue
    {
        bool IsExecuting { get; }

        void QueueCommand(ICommand command);

        void Execute();
    }
}