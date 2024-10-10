using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Padoru.Core.Files
{
    // WebGL uses a hybrid file system with a dictionary for in-memory files and web requests for files that 
    // don't exist in the dictionary because it is unable to store files locally.
    public class WebGLFileSystem : IFileSystem
    {
        private readonly string basePath;
        private readonly string webRequestProtocol;
        private readonly Regex protocolRegex;
        
        private readonly Dictionary<string, byte[]> files = new();

        public WebGLFileSystem(string basePath, string webRequestProtocol)
        {
            this.basePath = basePath;
            this.webRequestProtocol = webRequestProtocol;
            
            protocolRegex = new Regex(Constants.PROTOCOL_REGEX_PATTERN);
        }
        
        public async Task<bool> Exists(string uri, CancellationToken cancellationToken)
        {
            if (files.ContainsKey(uri))
            {
                return await Task.FromResult(true);
            }
            
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

        public async Task<byte[]> Read(string uri, CancellationToken cancellationToken, string version = null)
        {
            if (files.ContainsKey(uri))
            {
                return files[uri];
            }

            uri += $"?version={version}";
            
            var requestUri = GetRequestUri(uri);
			
            var uwr = UnityWebRequest.Get(requestUri);
            var request = uwr.SendWebRequest();

            while (!request.isDone)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Yield();
            }
            
            cancellationToken.ThrowIfCancellationRequested();
            
            if (uwr.result == UnityWebRequest.Result.Success) 
            {
                var manifestData = uwr.downloadHandler.data;
                return manifestData;
            }
            
            throw new FileNotFoundException($"Could not read file at path '{requestUri}'. Error: {uwr.error}");
        }

        public async Task Write(string uri, byte[] bytes, CancellationToken cancellationToken)
        {
            files[uri] = bytes;

            await Task.CompletedTask;
        }

        public async Task Delete(string uri, CancellationToken cancellationToken)
        {
            if (!files.ContainsKey(uri))
            {
                throw new FileNotFoundException($"Could not find file. Uri {uri}");
            }
            
            files.Remove(uri);
            
            await Task.CompletedTask;
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
                return path;
            }
            
            return webRequestProtocol + path;
        }
    }
}