using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public interface IFileSystem
    {
        Task<bool> Exists(string uri);

        Task<File<byte[]>> Read(string uri);

        Task<File<byte[]>>  Write(File<byte[]> file);

        Task Delete(string uri);
    }
}