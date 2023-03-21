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
            
            var www = UnityWebRequest.Get(path);
            var request = www.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield();
            }

            return www.result == UnityWebRequest.Result.Success;
        }

        public async Task<File<byte[]>> Read(string uri)
        {
            var path = GetFullPath(uri);
            
            var www = UnityWebRequest.Get(path);
            var request = www.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield();
            }
            
            if (www.result == UnityWebRequest.Result.Success) 
            {
                var manifestData = www.downloadHandler.data;
                return new File<byte[]>(uri, manifestData);
            }
            
            Debug.LogError($"Could not read file at path '{path}'. Error: {www.error}");
            return new File<byte[]>(uri, default);
        }

        public async Task<File<byte[]>> Write(File<byte[]> file)
        {
            var path = GetFullPath(file.Uri);
            
            var www = UnityWebRequest.Post(path, file.Data.ToString());
            var request = www.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield();
            }
            
            if (www.result == UnityWebRequest.Result.Success) 
            {
                return file;
            }
            
            Debug.LogError($"Could not write file at path '{path}'. Error: {www.error}");
            return file;
        }

        public async Task Delete(string uri)
        {
            var path = GetFullPath(uri);
            
            var www = UnityWebRequest.Delete(path);
            var request = www.SendWebRequest();

            while (!request.isDone)
            {
                await Task.Yield();
            }
            
            if (www.result != UnityWebRequest.Result.Success) 
            {
                Debug.LogError($"Deleted file at path '{path}'.");
            }
            else
            {
                Debug.LogError($"Could not delete file at path '{path}'. Error: {www.error}");
            }
        }
        
        private string GetFullPath(string uri)
        {
            return Path.Combine(basePath, FileUtils.ValidatedFileName(FileUtils.PathFromUri(uri)));
        }
    }
}