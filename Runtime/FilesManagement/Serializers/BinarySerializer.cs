using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Padoru.Core.Files
{
    public class BinarySerializer : ISerializer
    {
        private readonly BinaryFormatter binaryFormatter = new();
        
        public void Serialize(object value, out string text)
        {
            var ms = new MemoryStream();
            
            binaryFormatter.Serialize(ms, value);
            
            text = Encoding.UTF8.GetString(ms.ToArray());
        }

        public void Deserialize(Type type, ref string text, out object value)
        {
            var ms = new MemoryStream();
            
            var bytes = Encoding.UTF8.GetBytes(text);
            
            ms.Write(bytes, 0, bytes.Length);
            
            ms.Seek(0, SeekOrigin.Begin);
            
            var obj = binaryFormatter.Deserialize(ms);
            
            value = obj;
        }
    }
}