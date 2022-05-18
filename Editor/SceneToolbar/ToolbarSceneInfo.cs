using System;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace Padoru.Core.Editor
{
    [Serializable]
    public class ToolbarSceneInfo
    {
        public string Name;
        public string GUID;

        public ToolbarSceneInfo(Scene scene)
        {
            Name = scene.name;
            GUID = AssetDatabase.AssetPathToGUID(scene.path);
        }

        public ToolbarSceneInfo(string GUID)
        {
            var path = AssetDatabase.GUIDToAssetPath(GUID);
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            this.GUID = GUID;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ToolbarSceneInfo;
            return other.GUID == GUID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}