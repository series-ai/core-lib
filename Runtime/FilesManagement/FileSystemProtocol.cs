namespace Padoru.Core
{
    public class FileSystemProtocol
    {
        public readonly string ProtocolHeader;
        public readonly ISerializer Serializer;
        public readonly IFileSystem FileSystem;
        
        public FileSystemProtocol(string protocolHeader, ISerializer serializer, IFileSystem fileSystem)
        {
            ProtocolHeader = protocolHeader;
            Serializer = serializer;
            FileSystem = fileSystem;
        }
    }
}
