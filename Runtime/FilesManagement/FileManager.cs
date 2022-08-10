using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Debug = Padoru.Diagnostics.Debug;

namespace Padoru.Core
{
    public class FileManager : IFileManager
    {
        private const string PROTOCOL_HEADER_REGEX = @"^\w+://$";
        private const string DEFAULT_PROTOCOL_HEADER_REGEX = "default://";

        private readonly Dictionary<string, FileSystemProtocol> protocols = new Dictionary<string, FileSystemProtocol>();
        private readonly Regex protocolRegex;
        private readonly CommandQueue commandsQueue = new CommandQueue();
        private readonly FileSystemProtocol defaultProtocol;

        public FileManager(ISerializer defaultSerializer, IFileSystem defaultFileSystem)
        {
            protocolRegex = new Regex(PROTOCOL_HEADER_REGEX);

            defaultProtocol = new FileSystemProtocol(DEFAULT_PROTOCOL_HEADER_REGEX, defaultSerializer, defaultFileSystem);
        }

        public void Register(string protocolHeader, ISerializer serializer, IFileSystem fileSystem)
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

            Debug.LogWarning($"There is not protocol registered for the uri : {uri}. Returning default protocol");

            return defaultProtocol;
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