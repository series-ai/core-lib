using NUnit.Framework;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TestTools;

namespace Padoru.Core.Files.Tests
{
    public class ReadCommandTests
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
        public void Read_WhenFileSystemNull_ShouldReturnNullAndLog()
        {
            var protocol = new FileSystemProtocol(TEST_PROTOCOL_HEADER, serializer, null);

            File<TestClass> file = null;

            var command = new ReadCommand<TestClass>(testUri, protocol, (resultFile) =>
            {
                file = resultFile;
            });

            LogAssert.Expect(LogType.Error, new Regex(""));

            command.Execute();

            Assert.IsNull(file);
        }

        [Test]
        public void Read_WhenSerializerNull_ShouldReturnNullAndLog()
        {
            var protocol = new FileSystemProtocol(TEST_PROTOCOL_HEADER, null, fileSystem);

            File<TestClass> file = null;

            var command = new ReadCommand<TestClass>(testUri, protocol, (resultFile) =>
            {
                file = resultFile;
            });

            LogAssert.Expect(LogType.Error, new Regex(""));

            command.Execute();

            Assert.IsNull(file);
        }

        [Test]
        public void Read_WhenFileDoesNotExist_ShouldReturnNullAndLog()
        {
            var protocol = new FileSystemProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            File<TestClass> file = null;

            var command = new ReadCommand<TestClass>(testUri, protocol, (resultFile) =>
            {
                file = resultFile;
            });

            LogAssert.Expect(LogType.Error, new Regex(""));

            command.Execute();

            Assert.IsNull(file);
        }

        [Test]
        public void Read_WhenFileDoesExist_ShouldReturnValue()
        {
            var protocol = new FileSystemProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            var value = new TestClass()
            {
                value = "TestValue",
            };

            protocol.Serializer.Serialize(value, out var bytes);
            var newFile = new File<byte[]>(testUri, bytes);
            protocol.FileSystem.Set(newFile, null);

            File<TestClass> file = null;

            var command = new ReadCommand<TestClass>(testUri, protocol, (resultFile) =>
            {
                file = resultFile;
            });

            command.Execute();

            Assert.IsNotNull(file);

            protocol.FileSystem.Delete(testUri, null);
        }
    }
}