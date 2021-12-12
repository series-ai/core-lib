using System;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Padoru.Core.Editor
{
    [Serializable]
    public class ToolbarSceneInfo
    {
        public string Name;
        public string Path;
        public string GUID;

        public ToolbarSceneInfo(Scene scene)
        {
            Name = scene.name;
            Path = scene.path;
            GUID = AssetDatabase.AssetPathToGUID(scene.path);
        }

        public ToolbarSceneInfo(string path)
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            Path = path;
            GUID = AssetDatabase.AssetPathToGUID(path);
        }
    }
}