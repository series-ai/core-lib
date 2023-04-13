using System;

namespace Padoru.Core.Files
{
    public interface ISerializer
    {
        void Serialize(object value, out string text);

        void Deserialize(Type type, ref string text, out object value);
    }
}