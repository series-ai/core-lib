using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core.Files
{
    public class FileManager : IFileManager
    {
        private const string PROTOCOL_HEADER_REGEX = @"^\w+://$";
        private const string DEFAULT_PROTOCOL_HEADER_REGEX = "default://";

        private readonly Dictionary<string, FileSystemProtocol> protocols = new();
        private readonly Regex protocolRegex;
        private readonly FileSystemProtocol defaultProtocol;

        public FileManager(ISerializer defaultSerializer, IFileSystem defaultFileSystem)
        {
            protocolRegex = new Regex(PROTOCOL_HEADER_REGEX);

            defaultProtocol = new FileSystemProtocol(DEFAULT_PROTOCOL_HEADER_REGEX, defaultSerializer, defaultFileSystem);
        }

        public void RegisterProtocol(string protocolHeader, ISerializer serializer, IFileSystem fileSystem)
        {
            if (!protocolRegex.IsMatch(protocolHeader ?? string.Empty))
            {
                throw new Exception($"Invalid protocol '{protocolHeader}'. Only word characters allowed.");
            }

            if (serializer == null)
            {
                throw new Exception($"Cannot register protocol with a null {nameof(ISerializer)}");
            }

            if (fileSystem == null)
            {
                throw new Exception($"Cannot register protocol with a null {nameof(IFileSystem)}");
            }

            if (protocols.ContainsKey(protocolHeader))
            {
                throw new Exception($"Cannot register protocol {protocolHeader}, it is already registered");
            }

            var protocol = new FileSystemProtocol(protocolHeader, serializer, fileSystem);

            protocols.Add(protocolHeader, protocol);
        }

        public bool UnregisterProtocol(string protocolHeader)
        {
            if (protocols.ContainsKey(protocolHeader))
            {
                protocols.Remove(protocolHeader);
                return true;
            }

            return false;
        }

        public async Task<bool> Exists(string uri)
        {
            return await GetProtocol(uri).FileSystem.Exists(uri);
        }

        public async Task<File<T>> Read<T>(string uri)
        {
            var protocol = GetProtocol(uri);
                
            var file = await protocol.FileSystem.Read(uri);
                    
            var bytes = file.Data;

            protocol.Serializer.Deserialize(typeof(T), ref bytes, out var result);

            return new File<T>(uri, (T)result);
        }

        public async Task<File<T>> Write<T>(string uri, T value)
        {
            var protocol = GetProtocol(uri);
                
            protocol.Serializer.Serialize(value, out var text);

            var newFile = new File<string>(uri, text);

            await protocol.FileSystem.Write(newFile);

            return new File<T>(uri, value);
        }

        public async Task Delete(string uri)
        {
            var protocol = GetProtocol(uri);
                
            if (!await protocol.FileSystem.Exists(uri))
            {
                throw new Exception($"Cannot delete file because it does not exists: {uri}");
            }
            
            await protocol.FileSystem.Delete(uri);
        }

        private FileSystemProtocol GetProtocol(string uri)
        {
            foreach (var protocol in protocols)
            {
                if (uri.StartsWith(protocol.Value.ProtocolHeader))
                {
                    return protocol.Value;
                }
            }

            Debug.LogWarning($"There is not protocol registered for the uri : {uri}. Returning default protocol");

            return defaultProtocol;
        }
    }
}