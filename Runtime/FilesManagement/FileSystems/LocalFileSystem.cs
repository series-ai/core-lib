using System.IO;
using System.Threading.Tasks;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Files
{
    public class LocalFileSystem : IFileSystem
    {
        private readonly string basePath;

        public LocalFileSystem(string basePath)
        {
            this.basePath = basePath;
        }

        public async Task<bool> Exists(string uri)
        {
            var path = GetFullPath(uri);

            return await Task.FromResult(File.Exists(path));
        }

        public async Task<File<string>> Read(string uri)
        {
            var path = GetFullPath(uri);

            var text = await File.ReadAllTextAsync(path);

            Debug.Log($"Read file from path '{path}'");

            return new File<string>(uri, text);
        }

        public async Task Write(File<string> file)
        {
            var path = GetFullPath(file.Uri);

            var directory = Path.GetDirectoryName(path) ?? ".";
            
            Directory.CreateDirectory(directory);

            await File.WriteAllTextAsync(path, file.Data);

            Debug.Log($"Wrote file to path '{path}'");
        }

        public async Task Delete(string uri)
        {
            var path = GetFullPath(uri);

            File.Delete(path);
            
            await Task.CompletedTask;
        }

        private string GetFullPath(string uri)
        {
            return Path.Combine(basePath, FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)));
        }
    }
}