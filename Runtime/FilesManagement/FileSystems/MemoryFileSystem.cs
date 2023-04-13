using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public class MemoryFileSystem : IFileSystem
    {
        private readonly Dictionary<string, File<string>> files = new();

        public async Task<bool> Exists(string uri)
        {
            return await Task.FromResult(files.ContainsKey(uri));
        }

        public async Task<File<string>> Read(string uri)
        {
            if (files.TryGetValue(uri, out var file))
            {
                return await Task.FromResult(file);
            }

            throw new Exception($"Could not find file. Uri {uri}");
        }

        public async Task Write(File<string> file)
        {
            files[file.Uri] = file;

            await Task.CompletedTask;
        }

        public async Task Delete(string uri)
        {
            files.Remove(uri);
            
            await Task.CompletedTask;
        }
    }
}