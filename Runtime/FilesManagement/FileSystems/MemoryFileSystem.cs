using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public class MemoryFileSystem : IFileSystem
    {
        private readonly Dictionary<string, File<byte[]>> files = new();

        public async Task<bool> Exists(string uri)
        {
            return await Task.FromResult(files.ContainsKey(uri));
        }

        public async Task<File<byte[]>> Read(string uri)
        {
            File<byte[]> file;
            if (files.TryGetValue(uri, out file))
            {
                return await Task.FromResult(file);
            }

            throw new Exception("Could not find file.");
        }

        public async Task<File<byte[]>> Write(File<byte[]> file)
        {
            var newFile = new File<byte[]>(file.Uri, file.Data);
            files[file.Uri] = newFile;

            return await Task.FromResult(newFile);
        }

        public async Task Delete(string uri)
        {
            files.Remove(uri);
            
            await Task.CompletedTask;
        }
    }
}