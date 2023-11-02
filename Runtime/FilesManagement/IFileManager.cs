using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public interface IFileManager
    {
        void RegisterProtocol(string protocolHeader, ISerializer serializer, IFileSystem fileSystem);
        
        void RegisterProtocol(string protocolHeader, IProtocol protocol);

        bool UnregisterProtocol(string protocolHeader);

        Task<bool> Exists(string uri, CancellationToken token = default);

        Task<File<T>> Read<T>(string uri, CancellationToken token = default);

        Task<File<T>> Write<T>(string uri, T value, CancellationToken token = default);

        Task Delete(string uri, CancellationToken token = default);
    }
}
