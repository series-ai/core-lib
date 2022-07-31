using System;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public class DeleteCommand : ICommand
    {
        public event Action<OpResult> OnFinish;

        private FileSystemProtocol protocol;
        private string uri;
        private Action finishCallback;

        public DeleteCommand(string uri, FileSystemProtocol protocol, Action finishCallback)
        {
            this.protocol = protocol;
            this.uri = uri;
            this.finishCallback = finishCallback;
        }

        public void Execute()
        {
            try
            {
                protocol.FileSystem.Delete(uri, finishCallback);
                OnFinish?.Invoke(OpResult.Succeeded);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                OnFinish?.Invoke(OpResult.Failed);
            }
        }
    }
}