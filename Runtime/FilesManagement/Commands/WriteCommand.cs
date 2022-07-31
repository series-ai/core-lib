using System;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public class WriteCommand<T> : ICommand
    {
        public event Action<OpResult> OnFinish;

        private FileSystemProtocol protocol;
        private string uri;
        private T value;
        private Action<File<T>> finishCallback;

        public WriteCommand(string uri, T value, FileSystemProtocol protocol, Action<File<T>> finishCallback)
        {
            this.protocol = protocol;
            this.uri = uri;
            this.value = value;
            this.finishCallback = finishCallback;
        }

        public void Execute()
        {
            try
            {
                protocol.Serializer.Serialize(value, out var bytes);

                var newFile = new File<byte[]>(uri, bytes);

                protocol.FileSystem.Set(newFile, (file) =>
                {
                    finishCallback?.Invoke(new File<T>(uri, value));
                    OnFinish?.Invoke(OpResult.Succeeded);
                });
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                OnFinish?.Invoke(OpResult.Failed);
            }
        }
    }
}