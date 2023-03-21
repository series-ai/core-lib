using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public interface IFileManager
    {
        void RegisterProtocol(string protocol, ISerializer serializer, IFileSystem fileSystem);

        bool UnregisterProtocol(string protocol);

        Task<bool> Exists(string uri);

        Task<File<T>> Read<T>(string uri);

        Task<File<T>> Write<T>(string uri, T value);

        Task Delete(string uri);
    }
}
