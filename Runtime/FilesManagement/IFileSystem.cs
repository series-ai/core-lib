using System;

namespace Padoru.Core.Files
{
    public interface IFileSystem
    {
        bool Exists(string uri);

        void Get(string uri, Action<File<byte[]>> OnFinish);

        void Set(File<byte[]> file, Action<File<byte[]>> OnFinish);

        void Delete(string uri, Action OnFinish);
    }
}