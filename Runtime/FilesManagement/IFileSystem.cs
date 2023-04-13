using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public interface IFileSystem
    {
        Task<bool> Exists(string uri);

        Task<File<string>> Read(string uri);

        Task Write(File<string> file);

        Task Delete(string uri);
    }
}