using System.IO;
using UnityEngine;

namespace Padoru.Core.Files
{
    public class FileManagerInitializer : MonoBehaviour, IInitializable, IShutdowneable
    {
        public void Init()
        {
            var basePath = Path.Combine(Application.persistentDataPath, "Files");
            var serializer = new JsonSerializer();
            var fileSystem = new LocalFileSystem(basePath);
            var fileManager = new FileManager(serializer, fileSystem);

            fileManager.Register(Protocols.LOCAL_JSON_PPROTOCOL, serializer, fileSystem);

            Locator.RegisterService<IFileManager>(fileManager);
        }

        public void Shutdown()
        {
            Locator.UnregisterService<IFileManager>();
        }
    }
}