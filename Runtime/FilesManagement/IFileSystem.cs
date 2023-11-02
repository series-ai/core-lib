using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public interface IFileSystem
    {
        Task<bool> Exists(string uri, CancellationToken token = default);

        Task<File<byte[]>> Read(string uri, CancellationToken token = default);

        Task Write(File<byte[]> file, CancellationToken token = default);

        Task Delete(string uri, CancellationToken token = default);
    }
}