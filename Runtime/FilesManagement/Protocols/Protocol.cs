using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Padoru.Core.Files
{
    public class Protocol : IProtocol
    {
        private readonly ISerializer serializer;
        private readonly IFileSystem fileSystem;
        
        public Protocol(ISerializer serializer, IFileSystem fileSystem)
        {
            this.serializer = serializer;
            this.fileSystem = fileSystem;
        }
        
        public async Task<bool> Exists(string uri, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException("The provided uri is null or empty");
            }

            return await fileSystem.Exists(uri, cancellationToken);
        }

        [ItemCanBeNull]
        public async Task<File<T>> Read<T>(string uri, CancellationToken cancellationToken, string version = null)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException("The provided uri is null or empty");
            }
                
            var bytes = await fileSystem.Read(uri, cancellationToken, version);
            var data = await serializer.Deserialize(typeof(T), bytes, uri, cancellationToken);
            var file = new File<T>(uri, (T) data, bytes);
            return file;
        }

        public async Task<File<T>> Write<T>(string uri, T value, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException("The provided uri is null or empty");
            }
                
            var bytes = await serializer.Serialize(value, cancellationToken);

            await fileSystem.Write(uri, bytes, cancellationToken);

            return new File<T>(uri, value, bytes);
        }

        public async Task Delete(string uri, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException("The provided uri is null or empty");
            }
                
            if (!await fileSystem.Exists(uri, cancellationToken))
            {
                throw new FileNotFoundException($"Cannot delete file because it does not exists: {uri}");
            }
            
            await fileSystem.Delete(uri, cancellationToken);
        }
    }
}
