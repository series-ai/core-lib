using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Padoru.Core.Files
{
    public class BinarySerializer : ISerializer
    {
        private readonly BinaryFormatter binaryFormatter = new();
        
        public void Serialize(object value, out byte[] bytes)
        {
            var ms = new MemoryStream();
            
            binaryFormatter.Serialize(ms, value);
            
            bytes = ms.ToArray();
        }

        public void Deserialize(Type type, ref byte[] bytes, out object value)
        {
            var ms = new MemoryStream();

            ms.Write(bytes, 0, bytes.Length);
            
            ms.Seek(0, SeekOrigin.Begin);
            
            value = binaryFormatter.Deserialize(ms);
        }
    }
}