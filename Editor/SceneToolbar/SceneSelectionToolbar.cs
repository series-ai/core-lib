using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

namespace Padoru.Core.Editor
{
    [InitializeOnLoad]
    public static class SceneSelectionToolbar
    {
        private const string TOOLBAR_SCENES_KEY = "SceneSelectionToolbar.Scenes";
        private const string TOOLBAR_LAST_OPENED_SCENE_KEY = "SceneSelectionToolbar.LatestOpenedScene";

        private static List<ToolbarSceneInfo> scenes;
        private static ToolbarSceneInfo sceneOpened;
        private static int selectedIndex;
        private static string[] displayedOptions;

        static SceneSelectionToolbar()
        {
            LoadFromPlayerPrefs();

            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);

            EditorSceneManager.sceneOpened += HandleSceneOpened;
        }

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            selectedIndex = EditorGUILayout.Popup(selectedIndex, displayedOptions); ;

            GUI.enabled = selectedIndex == 0;
            if (GUILayout.Button("+"))
            {
                AddScene(sceneOpened);
            }

            GUI.enabled = selectedIndex > 0;
            if (GUILayout.Button("-"))
            {
                RemoveScene(sceneOpened);
            }

            GUI.enabled = true;
            if (GUI.changed && selectedIndex > 0 && scenes.Count > selectedIndex - 1)
            {                
                OpenScene(scenes[selectedIndex - 1]);
            }
        }

        private static void OpenScene(ToolbarSceneInfo scene)
        {
            try
            {
                EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(scene.GUID));
            }
            catch (Exception e)
            {
                Debug.LogError($"Could not load scene {scene.Name}. Removing from list.\n{e.Message}");
                RemoveScene(scene);
            }
        }

        private static void RefreshDisplayedOptions()
        {
            displayedOptions = new string[scenes.Count + 1];
            displayedOptions[0] = "Click on '+' to add current scene";

            for (int i = 0; i < scenes.Count; i++)
            {
                displayedOptions[i + 1] = scenes[i].Name;
            }
        }

        private static void HandleSceneOpened(Scene scene, OpenSceneMode mode)
        {
            SetOpenedScene(new ToolbarSceneInfo(scene));
        }

        private static void SetOpenedScene(ToolbarSceneInfo scene)
        {
            if (scene == null || string.IsNullOrEmpty(scene.Path)) return;

            for (int i = 0; i < scenes.Count; i++)
            {
                if (scenes[i].Path == scene.Path)
                {
                    sceneOpened = scenes[i];
                    selectedIndex = i + 1;
                    SaveToPlayerPrefs(true);
                    return;
                }
            }

            sceneOpened = scene;
            selectedIndex = 0;
            SaveToPlayerPrefs(true);
        }

        private static void AddScene(ToolbarSceneInfo scene)
        {
            if (scene == null) return;

            if (scenes.Any(s => s.Path == scene.Path))
            {
                RemoveScene(scene);
            }

            scenes.Add(scene);
            selectedIndex = scenes.Count;
            SetOpenedScene(scene);
            RefreshDisplayedOptions();
            SaveToPlayerPrefs();
        }

        private static void RemoveScene(ToolbarSceneInfo scene)
        {
            scenes.Remove(scene);
            selectedIndex = 0;
            RefreshDisplayedOptions();
            SaveToPlayerPrefs();
        }

        private static void SaveToPlayerPrefs(bool onlyLatestOpenedScene = false)
        {
            if (!onlyLatestOpenedScene)
            {
                var serialized = string.Join(";", scenes.Where(s => !string.IsNullOrEmpty(s.Path)).Select(s => s.Path));
                SetPref(TOOLBAR_SCENES_KEY, serialized);
            }

            if (sceneOpened != null)
            {
                SetPref(TOOLBAR_LAST_OPENED_SCENE_KEY, sceneOpened.Path);
            }
        }

        private static void LoadFromPlayerPrefs()
        {
            var serialized = GetPref(TOOLBAR_SCENES_KEY);

            scenes = serialized.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => new ToolbarSceneInfo(s)).ToList();

            if (scenes == null)
            {
                scenes = new List<ToolbarSceneInfo>();
            }

            serialized = GetPref(TOOLBAR_LAST_OPENED_SCENE_KEY);

            if (!string.IsNullOrEmpty(serialized))
            {
                SetOpenedScene(new ToolbarSceneInfo(serialized));
            }

            RefreshDisplayedOptions();
        }

        private static void SetPref(string name, string value)
        {
            EditorPrefs.SetString($"{Application.productName}_{name}", value);
        }

        private static string GetPref(string name)
        {
            return EditorPrefs.GetString($"{Application.productName}_{name}");
        }
    }
}