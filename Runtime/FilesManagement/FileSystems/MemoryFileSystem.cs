﻿using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public class MemoryFileSystem : IFileSystem
    {
        private readonly Dictionary<string, File<byte[]>> files = new();

        public async Task<bool> Exists(string uri, CancellationToken cancellationToken)
        {
            return await Task.FromResult(files.ContainsKey(uri));
        }

        public async Task<File<byte[]>> Read(string uri, CancellationToken cancellationToken, string version = null)
        {
            if (files.TryGetValue(uri, out var file))
            {
                return await Task.FromResult(file);
            }

            throw new FileNotFoundException($"Could not find file. Uri {uri}");
        }

        public async Task Write(File<byte[]> file, CancellationToken cancellationToken)
        {
            files[file.Uri] = file;

            await Task.CompletedTask;
        }

        public async Task Delete(string uri, CancellationToken cancellationToken)
        {
            if (!files.ContainsKey(uri))
            {
                throw new FileNotFoundException($"Could not find file. Uri {uri}");
            }
            
            files.Remove(uri);
            
            await Task.CompletedTask;
        }
    }
}