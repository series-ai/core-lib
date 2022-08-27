using System;
using System.IO;
using UnityEngine;

using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Files
{
    public class LocalFileSystem : IFileSystem
    {
        private readonly string basePath;

        public LocalFileSystem(string basePath = null)
        {
            this.basePath = basePath ?? string.Empty;
        }

        public bool Exists(string uri)
        {
            var path = Path.Combine(basePath, ValidatedFileName(RelativeUri(uri)));

            return File.Exists(path);
        }

        public void Get(string uri, Action<File<byte[]>> OnFinish)
        {
            var path = Path.Combine(basePath, ValidatedFileName(RelativeUri(uri)));

            Debug.Log($"Get({path})");

            var bytes = File.ReadAllBytes(path);

            OnFinish?.Invoke(new File<byte[]>(uri, bytes));
        }

        public void Set(File<byte[]> file, Action<File<byte[]>> OnFinish)
        {
            var path = Path.Combine(basePath, ValidatedFileName(RelativeUri(file.Uri)));

            Debug.Log($"Set({path})");

            var directory = Path.GetDirectoryName(path) ?? ".";
            Directory.CreateDirectory(directory);

            File.WriteAllBytes(path, file.Data);

            OnFinish?.Invoke(file);
        }

        public void Delete(string uri, Action OnFinish)
        {
            var path = Path.Combine(basePath, ValidatedFileName(RelativeUri(uri)));

            File.Delete(path);

            OnFinish?.Invoke();
        }

        private string RelativeUri(string uri)
        {
            var index = uri.IndexOf("://", StringComparison.Ordinal);
            if (index == -1)
            {
                return uri;
            }
            
            return uri.Substring(index + 3);
        }
        
        private string ValidatedFileName(string filePath)
        {
            filePath = filePath.Replace('\\', '/');
            var parts = filePath.Split('/');
            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var invalidChars = Path.GetInvalidFileNameChars();
                for (var index = 0; index < invalidChars.Length; index++)
                {
                    var c = invalidChars[index];

                    if (c == ':')
                    {
                        continue;
                    }

                    part = part.Replace(c, '_');
                }

                parts[i] = part;
            }

            return string.Join("/", parts);
        }
    }
}