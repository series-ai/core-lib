using NUnit.Framework;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        public async Task Setup()
        {
            fileManager = new FileManager(new JsonSerializer(), new MemoryFileSystem());

            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);
            if (await fileManager.Exists(testUri, default))
            {
                await fileManager.Delete(testUri, default);
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
        public async Task CheckFileIfExists_WhenFileDoesExist_ShouldReturnTrue()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            await fileManager.Write(testUri, value, default);

            var exists = await fileManager.Exists(testUri, default);

            Assert.IsTrue(exists);
        }

        [Test]
        public async Task CheckFileIfExists_WhenFileDoesNotExist_ShouldReturnFalse()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            var exists = await fileManager.Exists(testUri, default);

            Assert.IsFalse(exists);
        }

        [Test]
        public async Task CheckFileIfExists_WhenProtocolNotRegistered_ShouldLogWarning()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));

            await fileManager.Exists(testUri, default);
        }
        
        [Test]
        public async Task CheckFileIfExists_WhenUriIsNull_ShouldThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            var exception = Assert.ThrowsAsync<ArgumentException>(async() => await fileManager.Exists(null, default));
        }
    
        [Test]
        public async Task CheckFileIfExists_WhenUriIsEmpty_ShouldThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            Assert.ThrowsAsync<ArgumentException>(async () => await fileManager.Exists("", default));
        }
        
        [Test]
        public async Task DeleteFile_WhenFileDoesExist_ShouldNotThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            await fileManager.Write(testUri, value, default);

            Assert.DoesNotThrowAsync(async () => await fileManager.Delete(testUri, default));
        }

        [Test]
        public async Task DeleteFile_WhenFileDoesNotExist_ShouldThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            Assert.ThrowsAsync<FileNotFoundException>(async () => await fileManager.Delete(testUri, default));
        }

        [Test]
        public async Task DeleteFile_WhenUriIsNull_ShouldThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            Assert.ThrowsAsync<ArgumentException>(async () => await fileManager.Delete(null, default));
        }
        
        [Test]
        public async Task DeleteFile_WhenUriIsEmpty_ShouldThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            Assert.ThrowsAsync<ArgumentException>(async () => await fileManager.Delete(string.Empty, default));
        }
        
        [Test]
        public async Task DeleteFile_WhenProtocolNotRegistered_ShouldLogWarningAndNotThrow()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));

            await fileManager.Write(testUri, value, default);

            Assert.DoesNotThrowAsync(async () => await fileManager.Delete(testUri, default));
        }

        [Test]
        public async Task Write_WhenValueNull_ShouldNotThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            Assert.DoesNotThrowAsync(async () => await fileManager.Write<TestClass>(testUri, null, default));
        }

        [Test]
        public async Task Write_WhenValueNotNull_ShouldNotThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            Assert.DoesNotThrowAsync(async () => await fileManager.Write(testUri, value, default));
        }

        [Test]
        public async Task Write_WhenFileAlreadyExist_ShouldNotThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            await fileManager.Write(testUri, value, default);

            Assert.DoesNotThrowAsync(async () => await fileManager.Write(testUri, value, default));
        }

        [Test]
        public async Task Write_WhenUriIsNull_ShouldThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            Assert.ThrowsAsync<ArgumentException>(async () => await fileManager.Write(null, value, default));
        }
    
        [Test]
        public async Task Write_WhenUriIsEmpty_ShouldThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            Assert.ThrowsAsync<ArgumentException>(async () => await fileManager.Write(string.Empty, value, default));
        }

        [Test]
        public async Task Read_WhenFileDoesNotExist_ShouldThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            Assert.ThrowsAsync<FileNotFoundException>(async () => await fileManager.Read<TestClass>(testUri, default));
        }

        [Test]
        public async Task Read_WhenFileDoesExist_ShouldNotThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            await fileManager.Write(testUri, value, default);

            Assert.DoesNotThrowAsync(async () => await fileManager.Read<TestClass>(testUri, default));
        }
        
        [Test]
        public async Task Read_WhenUriIsNull_ShouldThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            Assert.ThrowsAsync<ArgumentException>(async () => await fileManager.Read<TestClass>(null, default));
        }
    
        [Test]
        public async Task Read_WhenUriIsEmpty_ShouldThrow()
        {
            fileManager.RegisterProtocol(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            Assert.ThrowsAsync<ArgumentException>(async () => await fileManager.Read<TestClass>(string.Empty, default));
        }
    }
}