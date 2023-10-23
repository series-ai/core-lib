using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Files
{
    public class HttpsFileSystem : IFileSystem
    {
        private readonly string basePath;
        private readonly int requestTimeout;
        private readonly HttpClient client;

        public HttpsFileSystem(string basePath, int requestTimeout)
        {
            this.basePath = basePath;
            this.requestTimeout = requestTimeout;
            client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, requestTimeout);
        }
        
        public async Task<bool> Exists(string uri)
        {
            var path = GetFullPath(uri);
            var response = await client.GetAsync(path);
            return response.IsSuccessStatusCode;
        }

        public async Task<File<byte[]>> Read(string uri)
        {
            var path = GetFullPath(uri);
            var response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsByteArrayAsync();
                Debug.Log($"Read file at path '{path}'.");
                return new File<byte[]>(uri, data);
            }
            
            throw new FileNotFoundException($"Could not read file at path '{path}'. Error code: {response.StatusCode}");
        }

        public async Task Write(File<byte[]> file)
        {
            var path = GetFullPath(file.Uri);
            var content = new ByteArrayContent(file.Data);
            var response = await client.PostAsync(path, content);
            
            if (response.IsSuccessStatusCode)
            {
                Debug.Log($"Written file at path '{path}'.");
            }
            else
            {
                Debug.LogError($"Could not write file at path '{path}'. Error code: {response.StatusCode}");
            }
        }

        public async Task Delete(string uri)
        {
            var path = GetFullPath(uri);
            var response = await client.DeleteAsync(path);

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