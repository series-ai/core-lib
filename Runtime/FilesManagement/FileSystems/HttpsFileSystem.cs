using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public class HttpsFileSystem : IFileSystem
    {
        private readonly string basePath;
        private readonly HttpClient client;
        private readonly int maxDownloadRetries;
        
        public HttpsFileSystem(string basePath, int requestTimeoutInSeconds, int maxDownloadRetries, HttpMessageHandler messageHandler = null)
        {
            this.basePath = basePath;
            
            client = messageHandler == null ? new HttpClient() : new HttpClient(messageHandler);
            client.Timeout = TimeSpan.FromSeconds(requestTimeoutInSeconds);
            this.maxDownloadRetries = maxDownloadRetries;
            // TODO: Use client base address instead of appending it to every request
        }
        
        public async Task<bool> Exists(string uri, CancellationToken cancellationToken)
        {
            var path = GetFullPath(uri);
            var response = await client.GetAsync(path, cancellationToken);
            return response.IsSuccessStatusCode;
        }

        public async Task<File<byte[]>> Read(string uri, CancellationToken cancellationToken, string version = null)
        {
            var path = GetFullPath(uri);

            path += $"?version={version}";

            HttpResponseMessage response = null;
            for (var i = 0; i < maxDownloadRetries; i++)
            {
                try
                {
                    response = await client.GetAsync(path, cancellationToken);
                }
                catch (Exception e) when (e is TaskCanceledException or HttpRequestException)
                {
                    if (cancellationToken.IsCancellationRequested || i == maxDownloadRetries - 1)
                    {
                        throw;
                    }
                    continue;
                }

                if (response.IsSuccessStatusCode)
                {
                    break;
                }
            }
            
            if (!response.IsSuccessStatusCode)
            {
                throw new FileNotFoundException($"Could not read file at path '{path}'. Error code: {response.StatusCode}");
            }
            
            var data = await response.Content.ReadAsByteArrayAsync();
                
            return new File<byte[]>(uri, data);
        }

        public async Task Write(File<byte[]> file, CancellationToken cancellationToken)
        {
            var path = GetFullPath(file.Uri);
            var content = new ByteArrayContent(file.Data);
            var response = await client.PostAsync(path, content, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Could not write file at path '{path}'. Error code: {response.StatusCode}");
            }
        }

        public async Task Delete(string uri, CancellationToken cancellationToken)
        {
            var path = GetFullPath(uri);
            var response = await client.DeleteAsync(path, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new FileNotFoundException($"Could not find file. Uri {uri}. Error code: {response.StatusCode}");
            }
        }
        
        private string GetFullPath(string uri)
        {
            return Path.Combine(basePath, FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)));
        }
    }
}