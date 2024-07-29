using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public interface IFileManager
    {
        void RegisterProtocol(string protocolHeader, ISerializer serializer, IFileSystem fileSystem);
        
        void RegisterProtocol(string protocolHeader, IProtocol protocol);

        bool UnregisterProtocol(string protocolHeader);

        Task<bool> Exists(string uri, CancellationToken cancellationToken);

        Task<File<T>> Read<T>(string uri, CancellationToken cancellationToken, string version = null);

        Task<File<T>> Write<T>(string uri, T value, CancellationToken cancellationToken);

        Task Delete(string uri, CancellationToken cancellationToken);
    }
}
