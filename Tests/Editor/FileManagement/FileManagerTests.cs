using NUnit.Framework;
using System;
using System.IO;
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
        public void Setup()
        {
            fileManager = new FileManager(new JsonSerializer(), new MemoryFileSystem());

            fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem);
            if (fileManager.Exists(testUri))
            {
                fileManager.Delete(testUri, null);
            }
            fileManager.Unregister(TEST_PROTOCOL_HEADER);
        }


        [Test]
        public void RegisterProtocol_WhenSerializerNull_ShouldThrow()
        {
            Assert.Throws<Exception>(() => fileManager.Register(TEST_PROTOCOL_HEADER, null, fileSystem));
        }

        [Test]
        public void RegisterProtocol_WhenFileSystemNull_ShouldThrow()
        {
            Assert.Throws<Exception>(() => fileManager.Register(TEST_PROTOCOL_HEADER, serializer, null));
        }

        [Test]
        public void RegisterProtocol_WhenProtocolFormatIncorrect_ShouldThrow()
        {
            Assert.Throws<Exception>(() => fileManager.Register("TestString", serializer, fileSystem));
        }

        [Test]
        public void RegisterProtocol_WhenProtocolIsAlreadyRegistered_ShouldThrow()
        {
            fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            Assert.Throws<Exception>(() => fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem));
        }

        [Test]
        public void RegisterProtocol_WhenProtocolIsNotRegistered_ShouldNotThrow()
        {
            Assert.DoesNotThrow(() => fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem));
        }

        [Test]
        public void UnregisterProtocol_WhenProtocolIsNotRegistered_ShouldFail()
        {
            var succeeded = fileManager.Unregister(TEST_PROTOCOL_HEADER);

            Assert.IsFalse(succeeded);
        }

        [Test]
        public void UnregisterProtocol_WhenProtocolIsRegistered_ShouldSucceed()
        {
            fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            var succeeded = fileManager.Unregister(TEST_PROTOCOL_HEADER);

            Assert.IsTrue(succeeded);
        }

        [Test]
        public void CheckFileIfExists_WhenFileDoesExist_ShouldReturnTrue()
        {
            fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            fileManager.Set(testUri, value, null);

            var exists = fileManager.Exists(testUri);

            Assert.IsTrue(exists);
        }

        [Test]
        public void CheckFileIfExists_WhenFileDoesNotExist_ShouldReturnFalse()
        {
            fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            var exists = fileManager.Exists(testUri);

            Assert.IsFalse(exists);
        }

        [Test]
        public void CheckFileIfExists_WhenProtocolNotRegistered_ShouldLogWarning()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));

            fileManager.Exists(testUri);
        }
        
        [Test]
        public void DeleteFile_WhenFileDoesExist_ShouldNotLog()
        {
            fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            fileManager.Set(testUri, value, null);

            fileManager.Delete(testUri, null);
        }

        [Test]
        public void DeleteFile_WhenFileDoesNotExist_ShouldLogError()
        {
            fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            LogAssert.Expect(LogType.Error, new Regex(""));

            fileManager.Delete(testUri, null);
        }

        [Test]
        public void DeleteFile_WhenProtocolNotRegistered_ShouldLogErrorAndWarning()
        {
            LogAssert.Expect(LogType.Warning, new Regex(""));
            LogAssert.Expect(LogType.Error, new Regex(""));

            fileManager.Delete(testUri, null);
        }

        [Test]
        public void Write_WhenValueNull_ShouldReturnValue()
        {
            fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            File<TestClass> file = null;

            fileManager.Set<TestClass>(testUri, null, (resultFile) =>
            {
                file = resultFile;
            });

            Assert.IsNotNull(file);
        }

        [Test]
        public void Write_WhenValueNotNull_ShouldReturnValue()
        {
            fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            File<TestClass> file = null;

            fileManager.Set(testUri, value, (resultFile) =>
            {
                file = resultFile;
            });

            Assert.IsNotNull(file);
        }

        [Test]
        public void Write_WhenFileAlreadyExist_ShouldReturnValue()
        {
            fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            File<TestClass> file = null;

            fileManager.Set(testUri, value, (resultFile) =>
            {
                file = resultFile;
            });

            fileManager.Set(testUri, value, (resultFile) =>
            {
                file = resultFile;
            });

            Assert.IsNotNull(file);
        }

        [Test]
        public void Read_WhenFileDoesNotExist_ShouldReturnNullAndLog()
        {
            fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            File<TestClass> file = null;

            LogAssert.Expect(LogType.Error, new Regex(""));

            fileManager.Get<TestClass>(testUri, (resultFile) =>
            {
                file = resultFile;
            });

            Assert.IsNull(file);
        }

        [Test]
        public void Read_WhenFileDoesExist_ShouldReturnValue()
        {
            fileManager.Register(TEST_PROTOCOL_HEADER, serializer, fileSystem);

            File<TestClass> file = null;

            fileManager.Set(testUri, value, (resultFile) =>
            {
                file = resultFile;
            });

            fileManager.Get<TestClass>(testUri, (resultFile) =>
            {
                file = resultFile;
            });

            Assert.IsNotNull(file);
        }
    }
}