using System.IO;
using System.Threading.Tasks;
using Padoru.Diagnostics;
using UnityEngine.Networking;

namespace Padoru.Core.Files
{
    //On Android you need to read with WebRequest but write with File.WriteAllBytes,
    //while on WebGL you can't write no matter what so it doesn't really matter
    public class WebFileSystem : IFileSystem
    {
        private readonly string basePath;
        private readonly string protocol;

        public WebFileSystem(string basePath, string protocol)
        {
            this.basePath = basePath;
            this.protocol = protocol;
        }
        
        public async Task<bool> Exists(string uri)
        {
            var path = GetAppropiateUri(uri);
            
            var uwr = UnityWebRequest.Get(path);
            var request = uwr.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield();
            }

            return uwr.result == UnityWebRequest.Result.Success;
        }

        public async Task<File<byte[]>> Read(string uri)
        {
            var path = GetAppropiateUri(uri);
            
            var uwr = UnityWebRequest.Get(path);
            var request = uwr.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield();
            }
            
            if (uwr.result == UnityWebRequest.Result.Success) 
            {
                Debug.Log($"Read file at path '{path}'.");
                
                var manifestData = uwr.downloadHandler.data;
                return new File<byte[]>(uri, manifestData);
            }
            
            throw new FileNotFoundException($"Could not read file at path '{path}'. Error: {uwr.error}");
        }

        public async Task Write(File<byte[]> file)
        {
            var path = GetFullPath(file.Uri);

            var directory = Path.GetDirectoryName(path) ?? ".";
            
            Directory.CreateDirectory(directory);

            await File.WriteAllBytesAsync(path, file.Data);

            Debug.Log($"Wrote file to path '{path}'");
        }

        public Task Delete(string uri)
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

        private string GetAppropiateUri(string uri)
        {
            var path = GetFullPath(uri);
            return protocol + path;
        }
    }
}