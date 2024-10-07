using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Padoru.Core.Files
{
    //On Android you need to read with WebRequest but write with File.WriteAllBytes
    public class AndroidFileSystem : IFileSystem
    {
        private readonly string basePath;
        private readonly string webRequestProtocol;
        private readonly Regex protocolRegex;

        public AndroidFileSystem(string webRequestProtocol)
        {
            this.basePath = string.Empty;
            this.webRequestProtocol = webRequestProtocol;
            
            protocolRegex = new Regex(Constants.PROTOCOL_REGEX_PATTERN);
        }
        
        public AndroidFileSystem(string basePath, string webRequestProtocol)
        {
            this.basePath = basePath;
            this.webRequestProtocol = webRequestProtocol;
            
            protocolRegex = new Regex(Constants.PROTOCOL_REGEX_PATTERN);
        }
        
        public async Task<bool> Exists(string uri, CancellationToken cancellationToken)
        {
            var requestUri = GetRequestUri(uri);
            var uwr = UnityWebRequest.Get(requestUri);
            var request = uwr.SendWebRequest();

            while (!request.isDone)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Yield();
            }

            cancellationToken.ThrowIfCancellationRequested();
            
            return uwr.result == UnityWebRequest.Result.Success;
        }

        public async Task<File<byte[]>> Read(string uri, CancellationToken cancellationToken, string version = null)
        {
            var requestUri = GetRequestUri(uri);
			
            var uwr = UnityWebRequest.Get(requestUri);
            var request = uwr.SendWebRequest();

            while (!request.isDone)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Yield();
            }
            
            cancellationToken.ThrowIfCancellationRequested();
            
            if (uwr.result != UnityWebRequest.Result.Success) 
            {
                throw new FileNotFoundException($"Could not read file at path '{requestUri}'. Error: {uwr.error}");
            }
            
            var manifestData = uwr.downloadHandler.data;
            return new File<byte[]>(uri, manifestData);
        }

        public async Task Write(File<byte[]> file, CancellationToken cancellationToken)
        {
            var path = GetFullPath(file.Uri);

            var directory = Path.GetDirectoryName(path) ?? ".";
            
            Directory.CreateDirectory(directory);

            await File.WriteAllBytesAsync(path, file.Data, cancellationToken);
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

        private string GetRequestUri(string uri)
        {
            var path = GetFullPath(uri);

            if (protocolRegex.IsMatch(path))
            {
                return path;
            }
            
            return webRequestProtocol + path;
        }
    }
}