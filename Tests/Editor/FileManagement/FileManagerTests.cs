using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TestTools;

namespace Padoru.Core.Files.Tests
{
    public class FileManagerTests
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
        private FileManager fileManager;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            testUri = TEST_PROTOCOL_HEADER + Application.dataPath;
            value = new TestClass()
            {
                value = "TestValue"
            };

            serializer = new JsonSerializer();
            fileSystem = new MemoryFileSystem();
        }

        [SetUp]
        public async void Setup()
        {
            fileManager = new FileManager(new JsonSerializer(), new MemoryFileSystem());

            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);
            if (await fileManager.Exists(testUri))
            {
                await fileManager.Delete(testUri);
            }
            fileManager.UnregisterProtocol(TEST_PROTOCOL_HEADER);
        }


        [Test]
        public void RegisterProtocol_WhenSerializerNull_ShouldThrow()
        {
            Assert.Throws<Exception>(() => fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, null, fileSystem));
        }

        [Test]
        public void RegisterProtocol_WhenFileSystemNull_ShouldThrow()
        {
            Assert.Throws<Exception>(() => fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, null));
        }

        [Test]
        public void RegisterProtocol_WhenProtocolFormatIncorrect_ShouldThrow()
        {
            Assert.Throws<Exception>(() => fileManager.RegisterProtocol("TestString", serializer, fileSystem));
        }

        [Test]
        public void RegisterProtocol_WhenProtocolIsAlreadyRegistered_ShouldThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            Assert.Throws<Exception>(() => fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem));
        }

        [Test]
        public void RegisterProtocol_WhenProtocolIsNotRegistered_ShouldNotThrow()
        {
            Assert.DoesNotThrow(() => fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem));
        }

        [Test]
        public void UnregisterProtocol_WhenProtocolIsNotRegistered_ShouldFail()
        {
            var succeeded = fileManager.UnregisterProtocol(TEST_PROTOCOL_HEADER);

            Assert.IsFalse(succeeded);
        }

        [Test]
        public void UnregisterProtocol_WhenProtocolIsRegistered_ShouldSucceed()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            var succeeded = fileManager.UnregisterProtocol(TEST_PROTOCOL_HEADER);

            Assert.IsTrue(succeeded);
        }

        [Test]
        public async void CheckFileIfExists_WhenFileDoesExist_ShouldReturnTrue()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            await fileManager.Write(testUri, value);

            var exists = await fileManager.Exists(testUri);

            Assert.IsTrue(exists);
        }

        [Test]
        public async void CheckFileIfExists_WhenFileDoesNotExist_ShouldReturnFalse()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            var exists = await fileManager.Exists(testUri);

            Assert.IsFalse(exists);
        }

        [Test]
        public async void CheckFileIfExists_WhenProtocolNotRegistered_ShouldLogWarning()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));

            await fileManager.Exists(testUri);
        }
        
        [Test]
        public async void DeleteFile_WhenFileDoesExist_ShouldNotLog()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            await fileManager.Write(testUri, value);

            await fileManager.Delete(testUri);
        }

        [Test]
        public async void DeleteFile_WhenFileDoesNotExist_ShouldLogError()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            LogAssert.Expect(LogType.Error, new Regex(""));

            await fileManager.Delete(testUri);
        }

        [Test]
        public async void DeleteFile_WhenProtocolNotRegistered_ShouldLogErrorAndWarning()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));
            LogAssert.Expect(LogType.Error, new Regex(""));

            await fileManager.Delete(testUri);
        }

        [Test]
        public async void Write_WhenValueNull_ShouldReturnValue()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            var file = await fileManager.Write<TestClass>(testUri, null);

            Assert.IsNotNull(file);
        }

        [Test]
        public async void Write_WhenValueNotNull_ShouldReturnValue()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            var file = await fileManager.Write(testUri, value);

            Assert.IsNotNull(file);
        }

        [Test]
        public async void Write_WhenFileAlreadyExist_ShouldReturnValue()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            var file = await fileManager.Write(testUri, value);

            file = await fileManager.Write(testUri, value);

            Assert.IsNotNull(file);
        }

        [Test]
        public async void Read_WhenFileDoesNotExist_ShouldLogAndReturnNull()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            LogAssert.Expect(LogType.Error, new Regex(""));

            var file = await fileManager.Read<TestClass>(testUri);
            
            Assert.IsNull(file);
        }

        [Test]
        public async void Read_WhenFileDoesExist_ShouldReturnValue()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            await fileManager.Write(testUri, value);

            var file = await fileManager.Read<TestClass>(testUri);

            Assert.IsNotNull(file);
        }
    }
}