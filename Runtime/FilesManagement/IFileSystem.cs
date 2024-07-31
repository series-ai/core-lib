using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public interface IFileSystem
    {
        Task<bool> Exists(string uri, CancellationToken cancellationToken);

        Task<File<byte[]>> Read(string uri, CancellationToken cancellationToken, string version = null);

        Task Write(File<byte[]> file, CancellationToken cancellationToken);

        Task Delete(string uri, CancellationToken cancellationToken);
    }
}