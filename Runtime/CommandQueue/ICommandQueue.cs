namespace Padoru.Core
{
    public interface ICommandQueue
    {
        void QueueCommand(ICommand command);

        void Execute();
    }
}