namespace Padoru.Core
{
    public class FileSystemProtocol
    {
        public string ProtocolHeader;
        public ISerializer Serializer;
        public IFileSystem FileSystem;
    }
}
