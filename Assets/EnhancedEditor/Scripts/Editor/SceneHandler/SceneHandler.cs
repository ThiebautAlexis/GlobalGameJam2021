// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EnhancedEditor.Editor
{
    public class SceneHandler : EditorWindow
    {
        #region Content
        /// <summary>
        /// Get currently opened Scene Handler or create a new one.
        /// </summary>
        [MenuItem("Enhanced Editor/Scene Handler")]
        public static void Get() => GetWindow<SceneHandler>("Scene Handler", true).Show();

        // -------------------------------------------
        // Scene Handler
        // -------------------------------------------

        private const float OrderIconWidth = 25;
        private const float LoadedIconWidth = 20;
        private const float DropdownIconWidth = 15;

        private const float RectHeight = 25;
        private const float RectHeightMargin = 5;
        private const float Spacing = 10;

        private const float RefreshButtonWidth = 55;
        private const float SceneLabelWidth = 150;
        private const float SceneButtonWidth = 70;
        private const float SceneButtonMargin = 7;

        private readonly GUIContent refreshGUI = new GUIContent("Refresh", "Refresh and load new project scenes");
        private readonly GUIContent createTagGUI = new GUIContent("Create Tag", "Create a new tag and assign it to this scene");
        private readonly GUIContent openSceneGUI = new GUIContent("OPEN", "Load this scene");
        private readonly GUIContent closeSceneGUI = new GUIContent("CLOSE", "Close this scene");

        private readonly Color oddColor = new Color(.195f, .195f, .195f);
        private readonly Color sceneLabelColor = new Color(.7f, .7f, .7f);
        private readonly Color openSceneColor = new Color(.4f, .9f, .7f);
        private readonly Color removeSceneColor = new Color(.9f, .4f, .6f);
        private readonly Color loadedColor = new Color(.0f, 1f, .0f);

        private readonly string settingsPath = "Assets/CORE/Editor/SceneHandlerSettings.asset";
        private readonly float toolbarHeight = EditorGUIUtility.singleLineHeight + 2;

        private GenericMenu openSceneMenu = null;

        private GUIContent oderIcon = null;
        private GUIContent tagIcon = null;
        private GUIContent dropdownIcon = null;
        private GUIContent loadedIcon = null;

        private GUIStyle sceneLabelStyle = null;
        private bool areStylesInitialized = false;

        private SceneHandlerSettings settings = null;
        private SceneData[] sceneDatas = new SceneData[] { };

        // Clic on tag => Generic menu with all tags from tags.
        // Select tag => tagContent[tagIndex].Add(Scene).
        // Create tag => Window adding tag and Scene in tag.
        // Tag changed => Remove from list and add to new.

        // -----------------------

        private void OnEnable()
        {
            // Load SceneHandler settings, and create one if none is found.
            // Delete also all settings that should not exist.
            bool _isLoaded = false;
            string[] _settingsPaths = Array.ConvertAll(AssetDatabase.FindAssets("t:SceneHandlerSettings"), AssetDatabase.GUIDToAssetPath);
            foreach (string _setting in _settingsPaths)
            {
                if (_setting == settingsPath)
                {
                    _isLoaded = true;
                    settings = AssetDatabase.LoadAssetAtPath<SceneHandlerSettings>(_setting);
                }
                else
                    AssetDatabase.DeleteAsset(_setting);
            }

            if (!_isLoaded)
            {
                string _directory = Application.dataPath + Path.GetDirectoryName(settingsPath.Remove(0, 6));
                if (!Directory.Exists(_directory))
                    Directory.CreateDirectory(_directory);

                settings = CreateInstance<SceneHandlerSettings>();
                AssetDatabase.CreateAsset(settings, settingsPath);
                AssetDatabase.SaveAssets();
            }

            // Refresh scenes.
            sceneDatas = settings.RefreshScenes();

            // Initialize icons & menu.
            oderIcon = EditorGUIUtility.IconContent("d_FilterByType", "Type of the object");
            tagIcon = EditorGUIUtility.IconContent("d_FilterByLabel@2x", "Scene Tag");
            dropdownIcon = EditorGUIUtility.IconContent("d_icon dropdown", "Scene Tag");
            loadedIcon = EditorGUIUtility.IconContent("d_FilterSelectedOnly", "Loaded scene");

            openSceneMenu = new GenericMenu();
            openSceneMenu.AddItem(new GUIContent("Single"), false, () => OpenScene(OpenSceneMode.Single));
            openSceneMenu.AddItem(new GUIContent("Additive"), false, () => OpenScene(OpenSceneMode.Additive));
        }

        // Add tags to this thing.
        private void OnGUI()
        {
            // Initialize GUIStyles if not loaded yet.
            if (!areStylesInitialized)
            {
                areStylesInitialized = true;

                sceneLabelStyle = new GUIStyle(GUI.skin.button);
                sceneLabelStyle.alignment = TextAnchor.MiddleLeft;
            }

            #region Toolbar
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            EditorGUILayout.GetControlRect();
            EditorGUILayout.EndHorizontal();

            // Icon.
            Rect _rect = new Rect(0, 0, OrderIconWidth, toolbarHeight);
            if (GUI.Button(_rect, oderIcon, EditorStyles.toolbarButton))
            {
                // Show order generic menu
                // • Name
                // • Token
                // • Loaded
            }

            // Refresh button.
            _rect.width = RefreshButtonWidth;
            _rect.x = position.width - _rect.width;
            if (GUI.Button(_rect, refreshGUI, EditorStyles.toolbarButton))
            {
                sceneDatas = settings.RefreshScenes();
            }
            #endregion

            #region Core Scene
            // Do the core scene thing.

            #endregion

            #region Loader
            _rect.y += _rect.height + Spacing;
            _rect.height = RectHeight;

            // Initialize rect and draw each scene informations.
            string _actualTag = SceneData.DefaultTag;
            for (int _i = 0; _i < sceneDatas.Length; _i++)
            {
                SceneData _scene = sceneDatas[_i];

                _rect.x = 5;
                _rect.width = position.width;

                // Draw tag name if not default one.
                if (_scene.Tag != _actualTag)
                {
                    _actualTag = _scene.Tag;
                    _rect.y += 10;
                   
                    EditorGUI.LabelField(_rect, _actualTag, EditorStyles.boldLabel);
                    _rect.y += 25;
                }

                // Odd color background.
                if ((_i % 2) == 1)
                    EditorGUI.DrawRect(new Rect(0, _rect.y - RectHeightMargin, position.width, _rect.height + (RectHeightMargin * 2)), oddColor);

                // Tag button.
                _rect.x = 0;
                _rect.width = OrderIconWidth;

                EditorGUIUtilityEnhanced.PushColor(_rect.Contains(Event.current.mousePosition) ? SuperColor.Silver.GetColor() : GUI.color);
                if (GUI.Button(_rect, tagIcon, EditorStyles.label))
                {
                    // Open menu to set tag.
                    GenericMenu _tagMenu = new GenericMenu();
                    for (int _j = 0; _j < settings.TagsCount; _j++)
                    {
                        string _tag = settings.GetTag(_j);
                        if (_tag != _scene.Tag)
                        {
                            int _index = _j;
                            _tagMenu.AddItem(new GUIContent(_tag), false, () => settings.SetTag(settings.GetTag(_index), _scene));
                        }
                    }

                    _tagMenu.AddSeparator(string.Empty);
                    _tagMenu.AddItem(createTagGUI, false, () => CreateSceneTagWindow.Get(settings, _scene));
                    _tagMenu.ShowAsContext();
                }
                EditorGUIUtilityEnhanced.PopColor();

                // Scene label.
                _rect.x += _rect.width + 5;
                _rect.width = SceneLabelWidth;

                EditorGUIUtilityEnhanced.PushColor(sceneLabelColor);
                EditorGUI.LabelField(_rect, _scene.Name, sceneLabelStyle);
                EditorGUIUtilityEnhanced.PopColor();

                _rect.x = position.width - SceneButtonWidth - Spacing;
                _rect.width = SceneButtonWidth;

                // Draw token here.
                // Just do it.

                // Loaded scene informations:
                //   • Draw a checkmark as visual feedback
                //   • Button to close the scene.
                if (_scene.IsLoaded)
                {
                    EditorGUIUtilityEnhanced.PushColor(removeSceneColor);
                    if (GUI.Button(_rect, closeSceneGUI))
                    {
                        EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByPath(AssetDatabase.GUIDToAssetPath(_scene.GUID)), true);
                        _scene.IsLoaded = false;
                    }
                    EditorGUIUtilityEnhanced.PopColor();

                    _rect.x -= LoadedIconWidth + (Spacing * .5f);
                    _rect.width = LoadedIconWidth;

                    EditorGUIUtilityEnhanced.PushColor(loadedColor);
                    EditorGUI.LabelField(_rect, loadedIcon);
                    EditorGUIUtilityEnhanced.PopColor();
                }
                else
                {
                    // Button to load (additively or destructively) the scene,
                    // with a dropdown icon next to it.
                    EditorGUIUtilityEnhanced.PushColor(openSceneColor);
                    if (GUI.Button(_rect, GUIContent.none))
                    {
                        openSceneIndex = _i;
                        openSceneMenu.DropDown(_rect);
                    }

                    _rect.x += SceneButtonMargin;
                    _rect.width -= SceneButtonMargin;
                    EditorGUI.LabelField(_rect, openSceneGUI);

                    _rect.x = position.width - DropdownIconWidth - (Spacing * 1.5f);
                    _rect.width = DropdownIconWidth;

                    EditorGUI.LabelField(_rect, dropdownIcon);
                    EditorGUIUtilityEnhanced.PopColor();
                }

                _rect.y += _rect.height + (RectHeightMargin * 2);
            }
            #endregion

            Repaint();
        }

        // -------------------------------------------
        // Open Scene
        // -------------------------------------------

        private int openSceneIndex = 0;

        // -----------------------

        private void OpenScene(OpenSceneMode _mode)
        {
            EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(sceneDatas[openSceneIndex].GUID), _mode);
            sceneDatas[openSceneIndex].IsLoaded = true;

            if (_mode == OpenSceneMode.Single)
            {
                for (int _i = 0; _i < sceneDatas.Length; _i++)
                {
                    if (_i != openSceneIndex)
                        sceneDatas[_i].IsLoaded = false;
                }
            }
        }
        #endregion

        #region Create Tag Window
        private class CreateSceneTagWindow : EditorWindow
        {
            SceneHandlerSettings settings = null;
            SceneData scene = null;
            string tagValue = "New Tag";

            // -----------------------

            public static void Get(SceneHandlerSettings _settings, SceneData _scene)
            {
                CreateSceneTagWindow _window = GetWindow<CreateSceneTagWindow>(true, "New Scene Tag", true);
                _window.settings = _settings;
                _window.scene = _scene;

                _window.minSize = new Vector2(250, 70);
                _window.maxSize = new Vector2(250, 70);

                _window.ShowUtility();
            }

            private void OnGUI()
            {
                Rect _rect = new Rect(5, 5, 40, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(_rect, "Tag:");
                _rect.x += 50;

                _rect.width = position.width - _rect.x - 5;
                tagValue = EditorGUI.TextField(_rect, tagValue);

                string _value = tagValue.Trim();
                if (string.IsNullOrEmpty(_value))
                {
                    _rect.x = 5;
                    _rect.y += _rect.height + 5;
                    _rect.height = 35;
                    _rect.width = position.width - 10;

                    EditorGUI.HelpBox(_rect, "Tag cannot be null or empty!", UnityEditor.MessageType.Error);
                }
                else if (settings.DoesTagExist(_value))
                {
                    _rect.x = 5;
                    _rect.y += _rect.height + 5;
                    _rect.height = 35;
                    _rect.width = position.width - 10;

                    EditorGUI.HelpBox(_rect, "Similar Tag already exist.", UnityEditor.MessageType.Error);
                }
                else
                {
                    _rect.x = position.width - 55;
                    _rect.y += _rect.height + 10;
                    _rect.width = 50;
                    _rect.height = 25;

                    if (GUI.Button(_rect, "OK"))
                    {
                        settings.AddTag(_value, scene);
                        Close();
                    }
                }
            }
        }
        #endregion
    }
}
