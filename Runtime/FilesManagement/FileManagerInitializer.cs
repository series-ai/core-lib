using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Padoru.Core.Files
{
    public class FileManagerInitializer : MonoBehaviour, IInitializable, IShutdowneable
    {
        public void Init()
        {
            var basePath = Path.Combine(Application.persistentDataPath, "Files");
            
            var serializer = new JsonSerializer(new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            
            var fileSystem = new LocalFileSystem(basePath);
            var fileManager = new FileManager(serializer, fileSystem);

            fileManager.RegisterProtocol(Protocols.LOCAL_JSON_PPROTOCOL, serializer, fileSystem);

            Locator.Register<IFileManager>(fileManager);
        }

        public void Shutdown()
        {
            Locator.Unregister<IFileManager>();
        }
    }
}