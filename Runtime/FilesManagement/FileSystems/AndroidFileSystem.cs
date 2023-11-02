using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Padoru.Diagnostics;
using UnityEngine.Networking;

namespace Padoru.Core.Files
{
    //On Android you need to read with WebRequest but write with File.WriteAllBytes
    public class AndroidFileSystem : IFileSystem
    {
        private readonly string basePath;
        private readonly string webRequestProtocol;
        private readonly Regex protocolRegex;

        public AndroidFileSystem(string basePath, string webRequestProtocol)
        {
            this.basePath = basePath;
            this.webRequestProtocol = webRequestProtocol;
            
            protocolRegex = new Regex(@"^[a-zA-Z]+://");;
        }
        
        public async Task<bool> Exists(string uri, CancellationToken token = default)
        {
            var requestUri = GetRequestUri(uri);
            var uwr = UnityWebRequest.Get(requestUri);
            var request = uwr.SendWebRequest();

            while (!request.isDone)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                
                await Task.Yield();
            }

            return uwr.result == UnityWebRequest.Result.Success;
        }

        public async Task<File<byte[]>> Read(string uri, CancellationToken token = default)
        {
            var requestUri = GetRequestUri(uri);
			
			Debug.Log($"Sending Get Web Request. Uri: {requestUri}");
			
            var uwr = UnityWebRequest.Get(requestUri);
            var request = uwr.SendWebRequest();

            while (!request.isDone)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }
                
                await Task.Yield();
            }
            
            if (uwr.result == UnityWebRequest.Result.Success) 
            {
                Debug.Log($"Read file at path '{requestUri}'.");
                
                var manifestData = uwr.downloadHandler.data;
                return new File<byte[]>(uri, manifestData);
            }
            
            throw new FileNotFoundException($"Could not read file at path '{requestUri}'. Error: {uwr.error}");
        }

        public async Task Write(File<byte[]> file, CancellationToken token = default)
        {
            var path = GetFullPath(file.Uri);

            var directory = Path.GetDirectoryName(path) ?? ".";
            
            Directory.CreateDirectory(directory);

            await File.WriteAllBytesAsync(path, file.Data, token);

            Debug.Log($"Wrote file to path '{path}'");
        }

        public Task Delete(string uri, CancellationToken token = default)
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
            return Path.Combine(basePath, FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)));
        }

        private string GetRequestUri(string uri)
        {
            var path = GetFullPath(uri);

            if (protocolRegex.IsMatch(path))
            {
                Debug.LogWarning($"Skipped adding web protocol to URI '{path}' because it already has a protocol.");
                return path;
            }
            
            return webRequestProtocol + path;
        }
    }
}