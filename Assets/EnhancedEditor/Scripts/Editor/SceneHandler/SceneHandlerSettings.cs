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
    public enum UserToken
    {
        Free,
        Alexis,
        Lucas,
        William,
        Matthieu
    }

    [Serializable]
    public class SceneData
    {
        public const string DefaultTag = "<None>";

        public string GUID = string.Empty;
        public string Tag = DefaultTag;
        public UserToken Token = UserToken.Free;

        [NonSerialized] public string Name = string.Empty;
        [NonSerialized] public bool IsLoaded = false;
    }

    [NonEditable("These datas are sensitives.\n\nIf you want to edit the Scene settings, please use the SceneHandler window instead.")]
    public class SceneHandlerSettings : ScriptableObject
    {
        #region Fields / Properties
        [SerializeField] private SceneData[] sceneDatas = new SceneData[] { };
        [SerializeField] private string[] tags = new string[] { SceneData.DefaultTag };

        public int TagsCount => tags.Length;

        // Core scene system.
        private string coreSceneGUID;
        private bool isCoreSceneEnable;
        private bool isCoreSceneAdditive;
        #endregion

        #region Methods

        #region Scene Datas
        /// <summary>
        /// Refresh project scenes as datas.
        /// </summary>
        public SceneData[] RefreshScenes()
        {
            // Store all open scenes path.
            string[] _activeScenePaths = new string[EditorSceneManager.sceneCount];
            for (int _i = 0; _i < _activeScenePaths.Length; _i++)
                _activeScenePaths[_i] = EditorSceneManager.GetSceneAt(_i).path;

            // Get all project scenes and store them as datas.
            string[] _allSceneGUIDs = AssetDatabase.FindAssets("t:Scene");
            SceneData[] _sceneDatas = new SceneData[_allSceneGUIDs.Length];

            for (int _i = 0; _i < _allSceneGUIDs.Length; _i++)
            {
                // Update existing datas and create new ones.
                string _guid = _allSceneGUIDs[_i];
                string _path = AssetDatabase.GUIDToAssetPath(_guid);

                if (sceneDatas.Find((_scene) => _scene.GUID == _guid, out SceneData _match))
                {
                    _match.Name = Path.GetFileNameWithoutExtension(_path);
                    _match.IsLoaded = Array.Exists(_activeScenePaths, (_scenePath) => _scenePath == _path);

                    _sceneDatas[_i] = _match;
                }
                else
                {
                    _sceneDatas[_i] = new SceneData()
                    {
                        GUID = _guid,
                        Tag = tags[0],
                        Name = Path.GetFileNameWithoutExtension(_path),
                        IsLoaded = Array.Exists(_activeScenePaths, (_scenePath) => _scenePath == _path)
                    };
                }
            }

            // Update and set tags.
            List<string> _newTags = new List<string>() { tags[0] };
            for (int _i = 1; _i < tags.Length; _i++)
            {
                if (Array.Exists(_sceneDatas, (_scene) => _scene.Tag == tags[_i]))
                    _newTags.Add(tags[_i]);
            }
            tags = _newTags.ToArray();

            // Set new scene datas.
            SortScenes(_sceneDatas);
            sceneDatas = _sceneDatas;
            return _sceneDatas;
        }

        private void SortScenes(SceneData[] _sceneDatas)
        {
            Array.Sort(_sceneDatas, (a, b) =>
            {
                int _aIndex = Array.IndexOf(tags, a.Tag);
                int _bIndex = Array.IndexOf(tags, b.Tag);

                if (_aIndex != _bIndex)
                    return _aIndex.CompareTo(_bIndex);

                return a.Name.CompareTo(b.Name); ;
            });
        }
        #endregion

        #region Tag Management
        public bool DoesTagExist(string _tag) => Array.Exists(tags, (_t) => _t == _tag);

        public string GetTag(int _index) => tags[_index];

        /// <summary>
        /// Create and assign a new tag to a scene.
        /// </summary>
        public void AddTag(string _tag, SceneData _scene)
        {
            if ((_scene.Tag != tags[0]) && !Array.Exists(sceneDatas, (_s) => (_s != _scene) && (_s.Tag == _scene.Tag)))
            {
                tags[Array.IndexOf(tags, _scene.Tag)] = _tag;
                _scene.Tag = _tag;
            }
            else
            {
                tags = tags.Add(_tag);

                _scene.Tag = _tag;
                SortScenes(sceneDatas);
            }
        }

        /// <summary>
        /// Assign an existing tag to a scene.
        /// </summary>
        public void SetTag(string _tag, SceneData _scene)
        {
            if ((_scene.Tag != tags[0]) && !Array.Exists(sceneDatas, (_s) => (_s != _scene) && (_s.Tag == _scene.Tag)))
                tags = tags.RemoveAt(Array.IndexOf(tags, _scene.Tag));

            _scene.Tag = _tag;
            SortScenes(sceneDatas);
        }
        #endregion

        #endregion
    }
}
