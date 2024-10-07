using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Files
{
    public class LocalFileSystem : IFileSystem
    {
        private readonly string basePath;

        public LocalFileSystem()
        {
            this.basePath = string.Empty;
        }
        
        public LocalFileSystem(string basePath)
        {
            this.basePath = basePath;
        }

        public async Task<bool> Exists(string uri, CancellationToken cancellationToken)
        {
            var path = GetFullPath(uri);

            return await Task.FromResult(File.Exists(path));
        }

        public async Task<File<byte[]>> Read(string uri, CancellationToken cancellationToken, string version = null)
        {
            var path = GetFullPath(uri);

            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var bytes = new byte[fileStream.Length];
                var remaining = (int) fileStream.Length;
                var offset = 0;
                var bytesRead = 0;

                while (remaining > 0)
                {
                    bytesRead = await fileStream.ReadAsync(bytes, offset, remaining, cancellationToken);

                    if (bytesRead == 0)
                    {
                        throw new EndOfStreamException($"End of stream reached with {remaining} bytes remaining to read");
                    }
                    
                    remaining -= bytesRead;
                    offset += bytesRead;
                }

                return new File<byte[]>(uri, bytes);
            }
        }

        public async Task Write(File<byte[]> file, CancellationToken cancellationToken)
        {
            var path = GetFullPath(file.Uri);

            var directory = Path.GetDirectoryName(path) ?? ".";
            
            Directory.CreateDirectory(directory);
            
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                await fs.WriteAsync(file.Data, 0, file.Data.Length, cancellationToken);
            }
        }

        public Task Delete(string uri, CancellationToken cancellationToken)
        {
            var path = GetFullPath(uri);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Could not find file. Uri {uri}");
            }
            
            File.Delete(path);
            
            return Task.CompletedTask;
        }

        private string GetFullPath(string uri)
        {
            return string.IsNullOrEmpty(basePath) 
                ? FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)) 
                : Path.Combine(basePath, FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)));
        }
    }
}