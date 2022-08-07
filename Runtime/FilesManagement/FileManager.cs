using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Padoru.Core
{
    public class FileManager : IFileManager
    {
        private const string PROTOCOL_HEADER_REGEX = @"^\w+://$";

        private readonly IFileSystem defaultFileSystem = new MemoryFileSystem();
        private readonly ISerializer defaultSerializer = new JsonSerializer();
        private readonly Dictionary<string, FileSystemProtocol> protocols = new Dictionary<string, FileSystemProtocol>();
        private readonly Regex protocolRegex;
        private readonly CommandQueue commandsQueue = new CommandQueue();

        public FileManager()
        {
            protocolRegex = new Regex(PROTOCOL_HEADER_REGEX);
        }

        public void Register(string protocolHeader, ISerializer serializer, IFileSystem fileSystem)
        {
            if (!protocolRegex.IsMatch(protocolHeader ?? string.Empty))
            {
                throw new ArgumentException($"Invalid protocol '{protocolHeader}'. Only word characters allowed.");
            }

            var protocol = new FileSystemProtocol
            {
                ProtocolHeader = protocolHeader,
                Serializer = serializer,
                FileSystem = fileSystem
            };

            protocols.Add(protocolHeader, protocol);
        }

        public bool Unregister(string protocolHeader)
        {
            if (protocols.ContainsKey(protocolHeader))
            {
                protocols.Remove(protocolHeader);
                return true;
            }

            return false;
        }

        public bool Exists(string uri)
        {
            return GetProtocol(uri).FileSystem.Exists(uri);
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

            return new FileSystemProtocol
            {
                FileSystem = defaultFileSystem,
                ProtocolHeader = "",
                Serializer = defaultSerializer
            };
        }

        public void Get<T>(string uri, Action<File<T>> OnFinish)
        {
            var protocol = GetProtocol(uri);

            var command = new ReadCommand<T>(uri, protocol, OnFinish);

            ExecuteCommand(command);
        }

        public void Set<T>(string uri, T value, Action<File<T>> OnFinish)
        {
            var protocol = GetProtocol(uri);

            var command = new WriteCommand<T>(uri, value, protocol, OnFinish);

            ExecuteCommand(command);
        }

        public void Delete(string uri, Action OnFinish)
        {
            var protocol = GetProtocol(uri);

            var command = new DeleteCommand(uri, protocol, OnFinish);

            ExecuteCommand(command);
        }

        private void ExecuteCommand(ICommand command)
        {
            commandsQueue.QueueCommand(command);

            commandsQueue.Execute();
        }
    }
}