using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Files
{
    public class HttpsFileSystem : IFileSystem
    {
        private readonly string basePath;
        private readonly HttpClient client;

        public HttpsFileSystem(string basePath, int requestTimeoutInSeconds)
        {
            this.basePath = basePath;
            
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(requestTimeoutInSeconds);
            // TODO: Use client base address instead of appending it to every request
        }
        
        public async Task<bool> Exists(string uri, CancellationToken token = default)
        {
            var path = GetFullPath(uri);
            var response = await client.GetAsync(path, token);
            return response.IsSuccessStatusCode;
        }

        public async Task<File<byte[]>> Read(string uri, string version = null, CancellationToken token = default)
        {
            var path = GetFullPath(uri);

            path += $"?version={version}";
            
            var response = await client.GetAsync(path, token);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsByteArrayAsync();
                Debug.Log($"Read file at path '{path}'.");
                return new File<byte[]>(uri, data);
            }
            
            throw new FileNotFoundException($"Could not read file at path '{path}'. Error code: {response.StatusCode}");
        }

        public async Task Write(File<byte[]> file, CancellationToken token = default)
        {
            var path = GetFullPath(file.Uri);
            var content = new ByteArrayContent(file.Data);
            var response = await client.PostAsync(path, content, token);
            
            if (response.IsSuccessStatusCode)
            {
                Debug.Log($"Written file at path '{path}'.");
            }
            else
            {
                Debug.LogError($"Could not write file at path '{path}'. Error code: {response.StatusCode}");
            }
        }

        public async Task Delete(string uri, CancellationToken token = default)
        {
            var path = GetFullPath(uri);
            var response = await client.DeleteAsync(path, token);

            if (response.IsSuccessStatusCode)
            {
                Debug.Log($"Deleted file at path '{path}'.");
                return;
            }

            throw new FileNotFoundException($"Could not find file. Uri {uri}. Error code: {response.StatusCode}");
        }
        
        private string GetFullPath(string uri)
        {
            return Path.Combine(basePath, FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)));
        }
    }
}