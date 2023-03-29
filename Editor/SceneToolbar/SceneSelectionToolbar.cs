using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private static Dictionary<string, ToolbarSceneInfo> scenes;
        private static ToolbarSceneInfo currentScene;
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
            GUILayout.Space(20);
            GUILayout.FlexibleSpace();

            selectedIndex = EditorGUILayout.Popup(selectedIndex, displayedOptions);

            GUI.enabled = selectedIndex == 0;
            if (GUILayout.Button("+"))
            {
                AddScene(currentScene);
            }

            GUI.enabled = selectedIndex > 0;
            if (GUILayout.Button("-"))
            {
                RemoveScene(currentScene);
            }

            GUI.enabled = true;
            if (GUI.changed && selectedIndex > 0 && scenes.Count > selectedIndex - 1)
            {
                OpenScene(scenes.Values.ElementAt(selectedIndex - 1));
            }
        }

        private static void OpenScene(ToolbarSceneInfo scene)
        {
            try
            {
                if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(scene.GUID));
                }
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

            var index = 1;
            foreach (var scene in scenes.Values)
            {
                displayedOptions[index] = scene.Name;
                index++;
            }
        }

        private static void HandleSceneOpened(Scene scene, OpenSceneMode mode)
        {
            SetOpenedScene(GetSceneInfo(scene));
        }

        private static void SetOpenedScene(ToolbarSceneInfo scene)
        {
            if(scene == null)
            {
                return;
            }

            currentScene = scene;

            selectedIndex = scenes.ContainsKey(scene.GUID) ?
                            scenes.Keys.ToList().IndexOf(scene.GUID) + 1 : 
                            0;

            SaveToPlayerPrefs(true);
        }

        private static void AddScene(ToolbarSceneInfo scene)
        {
            if (scenes.ContainsKey(scene.GUID))
            {
                RemoveScene(scene);
            }

            scenes.Add(scene.GUID, scene);
            selectedIndex = scenes.Count;
            SetOpenedScene(scene);
            RefreshDisplayedOptions();
            SaveToPlayerPrefs();
        }

        private static void RemoveScene(ToolbarSceneInfo scene)
        {
            if (!scenes.ContainsKey(scene.GUID))
            {
                return;
            }

            scenes.Remove(scene.GUID);
            selectedIndex = 0;
            RefreshDisplayedOptions();
            SaveToPlayerPrefs();
        }

        private static void SaveToPlayerPrefs(bool onlyLatestOpenedScene = false)
        {
            if (!onlyLatestOpenedScene)
            {
                var scenesGUIDs = string.Join(";", scenes.Values.Where(s => !string.IsNullOrEmpty(s.GUID)).Select(s => s.GUID));
                SetPref(TOOLBAR_SCENES_KEY, scenesGUIDs);
            }

            if (currentScene != null)
            {
                SetPref(TOOLBAR_LAST_OPENED_SCENE_KEY, currentScene.GUID);
            }
        }

        private static void LoadFromPlayerPrefs()
        {
            scenes = GetSavedScenes();

            var lastOpenedScene = GetLastOpenedScene();

            SetOpenedScene(lastOpenedScene);

            RefreshDisplayedOptions();
        }

        private static Dictionary<string, ToolbarSceneInfo> GetSavedScenes()
        {
            var scenes = new Dictionary<string, ToolbarSceneInfo>();

            var scencesString = GetPref(TOOLBAR_SCENES_KEY);
            var scenesList = scencesString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(s => new ToolbarSceneInfo(s)).ToList();

            if (scenesList != null)
            {
                foreach (var sceneInfo in scenesList)
                {
                    scenes.Add(sceneInfo.GUID, sceneInfo);
                }
            }

            return scenes;
        }

        private static ToolbarSceneInfo GetLastOpenedScene()
        {
            var lastOpenedSceneGIUD = GetPref(TOOLBAR_LAST_OPENED_SCENE_KEY);

            if (string.IsNullOrEmpty(lastOpenedSceneGIUD))
            {
                return null;
            }

            return GetSceneInfo(lastOpenedSceneGIUD);
        }

        private static void SetPref(string name, string value)
        {
            EditorPrefs.SetString($"{Application.productName}_{name}", value);
        }

        private static string GetPref(string name)
        {
            return EditorPrefs.GetString($"{Application.productName}_{name}");
        }

        private static ToolbarSceneInfo GetSceneInfo(Scene scene)
        {
            var sceneGUID = AssetDatabase.AssetPathToGUID(scene.path);
            return GetSceneInfo(sceneGUID);
        }

        private static ToolbarSceneInfo GetSceneInfo(string sceneGUID)
        {
            if (scenes.ContainsKey(sceneGUID))
            {
                return scenes[sceneGUID];
            }
            else
            {
                return new ToolbarSceneInfo(sceneGUID);
            }
        }
    }
}