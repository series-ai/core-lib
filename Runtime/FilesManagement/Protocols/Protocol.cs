using System;
using System.Threading;
using System.Threading.Tasks;

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

        public async Task<object> Read<T>(string uri, CancellationToken cancellationToken, string version = null)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException("The provided uri is null or empty");
            }
                
            var file = await fileSystem.Read(uri, cancellationToken, version);
                    
            var bytes = file.Data;

            var result = await serializer.Deserialize(typeof(T), bytes, uri, cancellationToken);

            return result;
        }

        public async Task<File<T>> Write<T>(string uri, T value, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException("The provided uri is null or empty");
            }
                
            var bytes = await serializer.Serialize(value, cancellationToken);

            var newFile = new File<byte[]>(uri, bytes);

            await fileSystem.Write(newFile, cancellationToken);

            return new File<T>(uri, value);
        }

        public async Task Delete(string uri, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException("The provided uri is null or empty");
            }
                
            if (!await fileSystem.Exists(uri, cancellationToken))
            {
                throw new Exception($"Cannot delete file because it does not exists: {uri}");
            }
            
            await fileSystem.Delete(uri, cancellationToken);
        }
    }
}
