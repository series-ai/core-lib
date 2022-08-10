using NUnit.Framework;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TestTools;

namespace Padoru.Core.Tests
{
    public class DeleteCommandTests
    {
        private class TestClass
        {
            public string value;
        }

        private const string TEST_PROTOCOL_HEADER = "testProtocol://";

        private string testUri;
        private ISerializer serializer;
        private IFileSystem fileSystem;

        [OneTimeSetUp]
        public void Setup()
        {
            testUri = Path.Combine(TEST_PROTOCOL_HEADER, Application.dataPath);

            serializer = new JsonSerializer();
            fileSystem = new MemoryFileSystem();
        }

        [Test]
        public void Delete_WhenFileDoesExist_ShouldNotLog()
        {
            var protocol = new FileSystemProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            var value = new TestClass()
            {
                value = "TestValue",
            };

            protocol.Serializer.Serialize(value, out var bytes);
            var newFile = new File<byte[]>(testUri, bytes);
            protocol.FileSystem.Set(newFile, null);

            var command = new DeleteCommand(testUri, protocol, null);

            command.Execute();

            protocol.FileSystem.Delete(testUri, null);
        }

        [Test]
        public void Delete_WhenFileDoesNotExist_ShouldLog()
        {
            var protocol = new FileSystemProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            var command = new DeleteCommand(testUri, protocol, null);

            LogAssert.Expect(LogType.Error, new Regex(""));

            command.Execute();
        }

    }
}