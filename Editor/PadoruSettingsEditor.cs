using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Padoru.Core.Editor
{
    [CustomEditor(typeof(Settings))]
    public class PadoruSettingsEditor : UnityEditor.Editor
    {
        private const string PROJECT_CONTEXT_PREFAB_NAME_PROPERTY_NAME = "projectContextPrefabName";
        private const string SCENES_PROPERTY_NAME = "scenes";
        private const string INITIAL_SCENE_PROPERTY_NAME = "initialScene";
        private const string LOG_SETTINGS_PROPERTY_NAME = "logSettings";

        private SerializedProperty projectContextPrefabNameProperty;
        private SerializedProperty scenesProperty;
        private SerializedProperty initialSceneProperty;
        private SerializedProperty logSettingsProperty;
        private Settings settings;

        private void OnEnable()
        {
            projectContextPrefabNameProperty = serializedObject.FindProperty(PROJECT_CONTEXT_PREFAB_NAME_PROPERTY_NAME);
            scenesProperty = serializedObject.FindProperty(SCENES_PROPERTY_NAME);
            initialSceneProperty = serializedObject.FindProperty(INITIAL_SCENE_PROPERTY_NAME);
            logSettingsProperty = serializedObject.FindProperty(LOG_SETTINGS_PROPERTY_NAME);

            settings = (Settings)target;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("Initialization");
            GUILayout.Space(5);
            
            EditorGUILayout.PropertyField(projectContextPrefabNameProperty);
            EditorGUILayout.PropertyField(scenesProperty);
            
            DrawInitialScenePopup();
            
            GUILayout.Space(5);
            GUILayout.Label("Logging");
            GUILayout.Space(5);
            
            EditorGUILayout.PropertyField(logSettingsProperty);
            
            serializedObject.ApplyModifiedProperties();
        }

        private void OnValidate()
        {
            settings = (Settings)target;
            
            if (settings.scenes is { Count: > 0 } && !settings.scenes.Contains(settings.initialScene))
            {
                Debug.LogError($"Settings scenes does not contain the scene '{settings.initialScene}'. Picking the first scene in the list as a default");

                SetInitialScene(settings.scenes[0]);
            }
        }

        private void DrawInitialScenePopup()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Initial Scene", GUILayout.ExpandWidth(false), GUILayout.Width(250));

            if (GUILayout.Button($"{initialSceneProperty.stringValue}", EditorStyles.popup))
            {
                SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), GetSearchProvider());
            }

            EditorGUILayout.EndHorizontal();
        }

        private void SetInitialScene(string initialScene)
        {
            initialSceneProperty.stringValue = initialScene;
            serializedObject.ApplyModifiedProperties();
        }

        private StringListSearchProvider GetSearchProvider()
        {
            if (settings == null)
            {
                var invalidProvider = CreateInstance<StringListSearchProvider>();
                invalidProvider.Init($"No {nameof(Settings)} found", new List<string>(), null);
                return invalidProvider;
            }
            
            var provider = CreateInstance<StringListSearchProvider>();
            provider.Init("List", settings.scenes, SetInitialScene);
            return provider;
        }

        private Settings LoadSettings()
        {
            var padoruSettingsFiles = AssetDatabase.FindAssets($"t:{typeof(Settings)}");

            if (padoruSettingsFiles == null || padoruSettingsFiles.Length <= 0)
            {
                return null;
            }

            if(padoruSettingsFiles.Length > 1)
            {
                Debug.LogWarning($"There are more than one {nameof(Settings)} in the project, window will only display values on the first one found");
            }

            var padoruSettingsFilePath = AssetDatabase.GUIDToAssetPath(padoruSettingsFiles[0]);
            var padoruSettings = (Settings)AssetDatabase.LoadAssetAtPath(padoruSettingsFilePath, typeof(Settings));

            return padoruSettings;
        }
    }
}
