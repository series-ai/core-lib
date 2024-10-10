namespace Padoru.Core.Files
{
    public class File<T>
    {
        public readonly string Uri;
        public readonly T Data;
        public readonly byte[] Bytes;

        public File(string uri, T data, byte[] bytes)
        {
            Uri = uri;
            Data = data;
            Bytes = bytes;
        }
    }
}