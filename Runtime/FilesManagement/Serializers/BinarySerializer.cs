using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Padoru.Core.Files
{
    public class BinarySerializer : ISerializer
    {
        private readonly BinaryFormatter binaryFormatter = new();
        
        public Task<byte[]> Serialize(object value)
        {
            var ms = new MemoryStream();
            
            binaryFormatter.Serialize(ms, value);
            
            var bytes = ms.ToArray();
            
            return Task.FromResult(bytes);
        }

        public Task<object> Deserialize(Type type, byte[] bytes, string uri)
        {
            var ms = new MemoryStream();

            ms.Write(bytes, 0, bytes.Length);
            
            ms.Seek(0, SeekOrigin.Begin);
            
            var value = binaryFormatter.Deserialize(ms);
            
            return Task.FromResult(value);
        }
    }
}