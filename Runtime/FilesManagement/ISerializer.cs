using System;

namespace Padoru.Core.Files
{
    public interface ISerializer
    {
        void Serialize(object value, out byte[] bytes);

        void Deserialize(Type type, ref byte[] bytes, out object value);
    }
}