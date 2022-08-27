using System;

namespace Padoru.Core.Files
{
    public interface IFileManager
    {
        void Register(string protocol, ISerializer serializer, IFileSystem fileSystem);

        bool Unregister(string protocol);

        bool Exists(string uri);

        void Get<T>(string uri, Action<File<T>> OnFinish);

        void Set<T>(string uri, T value, Action<File<T>> OnFinish);

        void Delete(string uri, Action OnFinish);
    }
}
