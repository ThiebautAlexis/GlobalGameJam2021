﻿using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EnhancedEditor.Editor
{
    public class CoreSceneSystem : EditorWindow 
    {
        /// <summary>
        /// Serializable class used to store settings relative to the Core Scene system,
        /// saved with <see cref="EditorPrefs"/> using Json.
        /// </summary>
        [Serializable]
        private struct CoreSceneSettings
        {
            public bool IsEnable;
            public bool DoLoadActiveScenes;

            public string CoreScenePath;

            // ----------------------------------------

            public CoreSceneSettings(bool _isEnable, bool _doLoadActiveScenes, string _coreScenePath)
            {
                IsEnable = _isEnable;
                DoLoadActiveScenes = _doLoadActiveScenes;
                CoreScenePath = _coreScenePath;
            }
        }

        #region Fields
        private const string SettingsPrefs =         "CoreSceneSettings";
        private const string StartingSessionState =  "IsStarting";

        private readonly GUIContent coreScenePathGUI =      new GUIContent("Core scene path", "Path of the scene to load when entering play mode");
        private readonly GUIContent isEnableGUI =           new GUIContent("Enable", "Is the core scene loading enable");
        private readonly GUIContent doLoadActiveScenesGUI = new GUIContent("Load active scene(s)",
                                                                           "When enabled, active scenes in the hierarchy will be loaded after the core scene");
        /// <summary>
        /// ID used do display a help box at the bottom of the window :
        /// 
        /// • -1  :  Registration Error
        /// • 0   :  Copy path info
        /// • 1   :  Autoload scene registered
        /// • 2   :  Scene autoload disabled
        /// </summary>
        private int messageID = 0;

        [SerializeField] private CoreSceneSettings settings = new CoreSceneSettings();
        #endregion

        #region Methods

        #region Editor Window
        // -------------------------------------------
        // Editor Window
        // -------------------------------------------

        [MenuItem("Enhanced Editor/Core Scene System")]
        public static void GetWindow() => GetWindow(typeof(CoreSceneSystem)).Show();

        private static CoreSceneSettings LoadSettings()
        {
            string _settings = EditorPrefs.GetString(SettingsPrefs, string.Empty);

            if (!string.IsNullOrEmpty(_settings))
                return JsonUtility.FromJson<CoreSceneSettings>(_settings);
            else
                return new CoreSceneSettings(false, true, "Assets/Core.unity");
        }

        private void Save()
        {
            EditorPrefs.SetString(SettingsPrefs, JsonUtility.ToJson(settings));
        }

        // ----------------------------------------

        private void SetCoreScene()
        {
            SceneAsset _mainScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(settings.CoreScenePath);
            if (_mainScene)
            {
                EditorSceneManager.playModeStartScene = _mainScene;
                messageID = 1;

                // Save changes.
                Save();
            }
            else
                messageID = -1;
        }

        // ----------------------------------------

        private void OnEnable()
        {
            settings = LoadSettings();
        }

        private void OnGUI()
        {
            // Header draw.
            EditorGUILayout.LabelField("Core Scene System", EditorStyles.boldLabel);

            // Top right window - Enable toggle.
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            EditorGUILayout.LabelField(isEnableGUI, GUILayout.MaxWidth(50));
            bool _enable = EditorGUILayout.Toggle(settings.IsEnable, GUILayout.MaxWidth(15));
            if (_enable != settings.IsEnable)
            {
                if (!_enable)
                {
                    EditorSceneManager.playModeStartScene = null;
                    messageID = 2;

                    // Save changes.
                    Save();
                }  
                else
                    SetCoreScene();

                settings.IsEnable = _enable;
            }

            EditorGUILayout.EndHorizontal();
            
            // Active scene(s) load parameter.
            bool _doLoadActiveScenes = EditorGUILayout.Toggle(doLoadActiveScenesGUI, settings.DoLoadActiveScenes);
            if (_doLoadActiveScenes != settings.DoLoadActiveScenes)
            {
                settings.DoLoadActiveScenes = _doLoadActiveScenes;

                // Save changes.
                Save();
            }

            EditorGUILayoutEnhanced.HorizontalLine(1, SuperColor.Grey.GetColor());

            // Scene registration.
            settings.CoreScenePath = EditorGUILayout.TextField(coreScenePathGUI, settings.CoreScenePath);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Load Core Scene", GUILayout.MaxWidth(125)))
            {
                SetCoreScene();
            }

            EditorGUILayout.EndHorizontal();

            // Display scene load informative message.
            switch (messageID)
            {
                case -1:
                    EditorGUILayout.HelpBox("Missing scene error ! Couldn't find scene at requested path.",
                                             UnityEditor.MessageType.Error);
                    break;

                case 0:
                    EditorGUILayout.HelpBox("You can copy an asset path with a context click and selecting \" Copy Path\".",
                                             UnityEditor.MessageType.Warning);
                    break;

                case 1:
                    EditorGUILayout.HelpBox("Core scene successfully loaded !",
                                             UnityEditor.MessageType.Info);
                    break;

                case 2:
                    EditorGUILayout.HelpBox("Core scene load disabled.",
                                             UnityEditor.MessageType.Info);
                    break;

                default:
                    // Display nothing.
                    break;
            }
        }
        #endregion

        #region Play Mode Loads
        // -------------------------------------------
        // Play Mode Loads
        // -------------------------------------------

        private static string[]    activeScenes;

        /// <summary>
        /// This method has two purpose :
        /// 
        /// • First, when Unity starts, set registered core scene if one,
        /// as Unity do not keep it in cache.
        /// 
        /// • Second, if the associated option is enabled, when entering play mode,
        /// register active scenes path to load them soon after ; scene loads cannot
        /// be performed before entering play mode, and once in, opened scene informations
        /// will not be available anymore.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void Initialization()
        {
            // When Unity starts, set core scene if one has been saved
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
            {
                if (!EditorSceneManager.playModeStartScene && SessionState.GetBool(StartingSessionState, true))
                {
                    SessionState.SetBool(StartingSessionState, false);
                    SceneAsset _mainScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(LoadSettings().CoreScenePath);

                    if (_mainScene)
                        EditorSceneManager.playModeStartScene = _mainScene;
                }

                return;
            }

            // When entering play mode, register active scenes path to load them
            // if the option is enabled
            if (!EditorSceneManager.playModeStartScene)
                return;

            if (LoadSettings().DoLoadActiveScenes)
            {
                activeScenes = new string[EditorSceneManager.sceneCount];

                for (int _i = 0; _i < activeScenes.Length; _i++)
                {
                    activeScenes[_i] = EditorSceneManager.GetSceneAt(_i).path;
                }
            }
            else
                activeScenes = null;
        }

        /// <summary>
        /// When entering play mode, load registered active scenes.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadActiveScenes()
        {
            if (activeScenes == null)
                return;

            LoadSceneParameters _parameter = new LoadSceneParameters();
            _parameter.loadSceneMode = LoadSceneMode.Additive;
            string _coreSceneName = LoadSettings().CoreScenePath;

            for (int _i = 0; _i < activeScenes.Length; _i++)
            {
                if (activeScenes[_i] != _coreSceneName)
                    EditorSceneManager.LoadSceneInPlayMode(activeScenes[_i], _parameter);
            }
        }
        #endregion

        #endregion
    }
}
