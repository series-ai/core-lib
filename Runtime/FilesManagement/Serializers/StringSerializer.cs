using System;
using System.Text;

namespace Padoru.Core.Files
{
    public class StringSerializer : ISerializer
    {
        public void Serialize(object value, out byte[] bytes)
        {
            bytes = Encoding.UTF8.GetBytes(value.ToString());
        }

        public void Deserialize(Type type, ref byte[] bytes, out object value)
        {
            value = Encoding.UTF8.GetString(bytes);
        }
    }
}