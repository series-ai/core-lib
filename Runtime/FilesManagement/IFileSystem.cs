using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public interface IFileSystem
    {
        Task<bool> Exists(string uri, CancellationToken cancellationToken);

        Task<byte[]> Read(string uri, CancellationToken cancellationToken, string version = null);

        Task Write(string uri, byte[] bytes, CancellationToken cancellationToken);

        Task Delete(string uri, CancellationToken cancellationToken);
    }
}