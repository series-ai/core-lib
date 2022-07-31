using System;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public class ReadCommand<T> : ICommand
    {
        public event Action<OpResult> OnFinish;

        private FileSystemProtocol protocol;
        private string uri;
        private Action<File<T>> finishCallback;

        public ReadCommand(string uri, FileSystemProtocol protocol, Action<File<T>> finishCallback)
        {
            this.protocol = protocol;
            this.uri = uri;
            this.finishCallback = finishCallback;
        }

        public void Execute()
        {
            try
            {
                protocol.FileSystem.Get(uri, (file) =>
                {
                    var bytes = file.Data;

                    protocol.Serializer.Deserialize(typeof(T), ref bytes, out var result);

                    finishCallback?.Invoke(new File<T>(uri, (T)result));
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