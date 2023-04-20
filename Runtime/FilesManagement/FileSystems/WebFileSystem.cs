using System.IO;
using System.Threading.Tasks;
using Padoru.Diagnostics;
using UnityEngine.Networking;

namespace Padoru.Core.Files
{
    public class WebFileSystem : IFileSystem
    {
        private readonly string basePath;

        public WebFileSystem(string basePath)
        {
            this.basePath = basePath;
        }
        
        public async Task<bool> Exists(string uri)
        {
            var path = GetFullPath(uri);
            
            var uwr = UnityWebRequest.Get(path);
            var request = uwr.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield();
            }

            return uwr.result == UnityWebRequest.Result.Success;
        }

        public async Task<File<string>> Read(string uri)
        {
            var path = GetFullPath(uri);
            
            var uwr = UnityWebRequest.Get(path);
            var request = uwr.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield();
            }
            
            if (uwr.result == UnityWebRequest.Result.Success) 
            {
                Debug.Log($"Read file at path '{path}'.");
                
                var manifestData = uwr.downloadHandler.text;
                return new File<string>(uri, manifestData);
            }
            
            throw new Exception($"Could not read file at path '{path}'. Error: {uwr.error}");
        }

        public async Task Write(File<string> file)
        {
            var path = GetFullPath(file.Uri);
            
            var uwr = UnityWebRequest.Post(path, file.Data);
            var request = uwr.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield();
            }
            
            if (uwr.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Written file at path '{path}'.");
            }
            else
            {
                Debug.LogError($"Could not write file at path '{path}'. Error: {uwr.error}");
            }
        }

        public async Task Delete(string uri)
        {
            var path = GetFullPath(uri);
            
            var uwr = UnityWebRequest.Delete(path);
            var request = uwr.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield();
            }
            
            if (uwr.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Deleted file at path '{path}'.");
            }
            else
            {
                Debug.LogError($"Could not delete file at path '{path}'. Error: {uwr.error}");
            }
        }
        
        private string GetFullPath(string uri)
        {
            return Path.Combine(basePath, FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)));
        }
    }
}