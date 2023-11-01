using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Padoru.Diagnostics;
using UnityEngine.Networking;

namespace Padoru.Core.Files
{
    public class WebGLFileSystem : IFileSystem
    {
        private readonly string basePath;
        private readonly string webRequestProtocol;
        private readonly Regex protocolRegex;
        
        private readonly Dictionary<string, File<byte[]>> files = new();

        public WebGLFileSystem(string basePath, string webRequestProtocol)
        {
            this.basePath = basePath;
            this.protocolRegex = new Regex(@"^[a-zA-Z]+://");;
            this.webRequestProtocol = webRequestProtocol;
        }
        
        public async Task<bool> Exists(string uri)
        {
            return await Task.FromResult(files.ContainsKey(uri));
        }

        public async Task<File<byte[]>> Read(string uri)
        {
            if (files.ContainsKey(uri))
            {
                return files[uri];
            }
            
            var requestUri = GetRequestUri(uri);
			
			Debug.Log($"Sending Get Web Request. Uri: {requestUri}");
			
            var uwr = UnityWebRequest.Get(requestUri);
            var request = uwr.SendWebRequest();

            while (!request.isDone)
            {
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

        public async Task Write(File<byte[]> file)
        {
            files[file.Uri] = file;

            await Task.CompletedTask;
        }

        public async Task Delete(string uri)
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
                Debug.LogWarning($"Skipped adding web protocol to URI '{path}' because it already has a protocol.");
                return path;
            }
            
            return webRequestProtocol + path;
        }
    }
}