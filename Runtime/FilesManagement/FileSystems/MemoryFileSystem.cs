using System;
using System.Collections.Generic;

namespace Padoru.Core.Files
{
    public class MemoryFileSystem : IFileSystem
    {
        private readonly Dictionary<string, File<byte[]>> files = new Dictionary<string, File<byte[]>>();

        public bool Exists(string uri)
        {
            return files.ContainsKey(uri);
        }

        public void Get(string uri, Action<File<byte[]>> OnFinish)
        {
            File<byte[]> file;
            if (files.TryGetValue(uri, out file))
            {
                OnFinish?.Invoke(file);
                return;
            }

            throw new Exception("Could not find file.");
        }

        public void Set(File<byte[]> file, Action<File<byte[]>> OnFinish)
        {
            var newFile = new File<byte[]>(file.Uri, file.Data);
            files[file.Uri] = newFile;

            OnFinish?.Invoke(newFile);
        }

        public void Delete(string uri, Action OnFinish)
        {
            files.Remove(uri);
        }
    }
}