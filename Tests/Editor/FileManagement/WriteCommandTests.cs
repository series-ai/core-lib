using NUnit.Framework;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TestTools;

namespace Padoru.Core.Files.Tests
{
    public class WriteCommandTests
    {
        private class TestClass
        {
            public string value;
        }

        private const string TEST_PROTOCOL_HEADER = "testProtocol://";

        private string testUri;
        private TestClass value;
        private ISerializer serializer;
        private IFileSystem fileSystem;

        [OneTimeSetUp]
        public void Setup()
        {
            testUri = Path.Combine(TEST_PROTOCOL_HEADER, Application.dataPath);
            value = new TestClass()
            {
                value = "TestValue"
            };

            serializer = new JsonSerializer();
            fileSystem = new MemoryFileSystem();
        }

        [Test]
        public void Write_WhenFileSystemNull_ShouldReturnNullAndLog()
        {
            var protocol = new FileSystemProtocol(TEST_PROTOCOL_HEADER, serializer, null);

            File<TestClass> file = null;

            var command = new WriteCommand<TestClass>(testUri, value, protocol, (resultFile) =>
            {
                file = resultFile;
            });

            LogAssert.Expect(LogType.Error, new Regex(""));

            command.Execute();

            Assert.IsNull(file);
        }

        [Test]
        public void Write_WhenSerializerNull_ShouldReturnNullAndLog()
        {
            var protocol = new FileSystemProtocol(TEST_PROTOCOL_HEADER, null, fileSystem);

            File<TestClass> file = null;

            var command = new WriteCommand<TestClass>(testUri, value, protocol, (resultFile) =>
            {
                file = resultFile;
            });

            LogAssert.Expect(LogType.Error, new Regex(""));

            command.Execute();

            Assert.IsNull(file);
        }

        [Test]
        public void Write_WhenValueNull_ShouldReturnValue()
        {
            var protocol = new FileSystemProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            File<TestClass> file = null;

            var command = new WriteCommand<TestClass>(testUri, null, protocol, (resultFile) =>
            {
                file = resultFile;
            });

            command.Execute();

            Assert.IsNotNull(file);
        }

        [Test]
        public void Write_WhenValueNotNull_ShouldReturnValue()
        {
            var protocol = new FileSystemProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            File<TestClass> file = null;

            var command = new WriteCommand<TestClass>(testUri, value, protocol, (resultFile) =>
            {
                file = resultFile;
            });

            command.Execute();

            Assert.IsNotNull(file);
        }

        [Test]
        public void Write_WhenFileAlreadyExist_ShouldReturnValue()
        {
            var protocol = new FileSystemProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            File<TestClass> file = null;

            var command = new WriteCommand<TestClass>(testUri, value, protocol, (resultFile) =>
            {
                file = resultFile;
            });

            command.Execute();

            command.Execute();

            Assert.IsNotNull(file);
        }
    }
}