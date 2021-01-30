// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using System;
using System.Collections;
using System.IO;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace EnhancedEditor.Editor
{
	public class PinAsset : ScriptableObject
    {
        #region Fields
        private static readonly string PinAssetsFolder = "/CORE/Editor/Pin Assets/";

        [SerializeField] private int objectID = 999;
        [SerializeField] private string objectType = null;
        [SerializeField] private string sceneGUID = string.Empty;
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="PinAsset"/> associated with a given <see cref="GameObject"/> in scene.
        /// </summary>
        /// <param name="_assetFolder">Folder where to save asset. Value is added to <see cref="PinAssetsFolder"/> from Assets path.</param>
        /// <param name="_sceneGUID">GUID of the scene the object is in.</param>
        /// <param name="_objectID">ID of the associated object. Simply use its name has hash code.</param>
        /// <param name="_objectType">Type of the associated object.</param>
        public static void CreatePinAsset(string _assetFolder, string _sceneGUID, int _objectID, Type _objectType)
        {
            var _asset = CreateInstance<PinAsset>();
            _asset.sceneGUID = _sceneGUID;
            _asset.objectID = _objectID;
            _asset.objectType = _objectType.ToString() + ", " + _objectType.Assembly;

            string _directory = Application.dataPath + PinAssetsFolder + Path.GetDirectoryName(_assetFolder);
            if (!Directory.Exists(_directory))
                Directory.CreateDirectory(_directory);

            _directory = "Assets" + PinAssetsFolder + _assetFolder + ".asset";
            AssetDatabase.CreateAsset(_asset, _directory);
            AssetDatabase.SaveAssets();
        }

        // -------------------------------------------
        // Cleaner
        // -------------------------------------------

        private const float ProgressBarSaveRatio = .1f;
        private const float ProgressBarAssetRatio = .8f;

        private static readonly string progressBarTitle = "Cleaning project PinAssets, please wait until process ends...";
        private static readonly string progressBarSaveInfos = "Saving open scenes...";
        private static readonly string progressBarAssetInfos1 = "Checking asset '";
        private static readonly string progressBarAssetInfos2 = "'...";
        private static readonly string progressBarFolderInfos = "Deleting empty folders...";

        // -----------------------

        [MenuItem("Enhanced Editor/Clean Pin Assets")]
        public static void CleanPinAssets() => EditorCoroutineUtility.StartCoroutineOwnerless(DoCleanPinAssets());

        private static IEnumerator DoCleanPinAssets()
        {
            // Store editor open scenes to reopen them on cleaning end.
            string[] openScenes = new string[EditorSceneManager.sceneCount];
            for (int _i = 0; _i < openScenes.Length; _i++)
                openScenes[_i] = EditorSceneManager.GetSceneAt(_i).path;

            // Initialize progress bar.
            EditorUtility.DisplayProgressBar(progressBarTitle, progressBarSaveInfos, ProgressBarSaveRatio / 2f);
            yield return null;

            // Save open scenes before messing it up.
            EditorSceneManager.SaveOpenScenes();
            string[] _pinAssetPaths = Array.ConvertAll(AssetDatabase.FindAssets("t:PinAsset"), AssetDatabase.GUIDToAssetPath);
            yield return null;

            float _progress = ProgressBarSaveRatio;
            for (int _i = 0; _i < _pinAssetPaths.Length; _i++)
            {
                // Update progress bar.
                yield return null;

                string _path = _pinAssetPaths[_i];
                _progress += (ProgressBarAssetRatio - ProgressBarSaveRatio) / _pinAssetPaths.Length;
                EditorUtility.DisplayProgressBar(progressBarTitle, progressBarAssetInfos1 + _path + progressBarAssetInfos2, _progress);

                PinAsset _pinAsset = AssetDatabase.LoadAssetAtPath<PinAsset>(_path);
                if (_pinAsset.PinAssetInScene())
                {
                    // Associated object exist, that's cool.
                    //
                    // Now, check if object type and names do match,
                    // and move asset in correct folder if not.
                    string _newPath = Path.Combine(EditorSceneManager.GetActiveScene().name, Selection.activeObject.name + ".asset");
                    var _attributes = Type.GetType(_pinAsset.objectType).GetCustomAttributes(typeof(PinnedObjectAttribute), true) as PinnedObjectAttribute[];
                    if (_attributes.Length > 0)
                    {
                        _newPath = "Assets" + PinAssetsFolder + Path.Combine(_attributes[0].AssetsFolder, _newPath);
                        if (_newPath != _path)
                        {
                            // Create destination directory and move asset.
                            string _fullPath = Application.dataPath + Path.GetDirectoryName(_newPath.Remove(0, 6));
                            if (!Directory.Exists(_fullPath))
                            {
                                yield return null;
                                Directory.CreateDirectory(_fullPath);
                                AssetDatabase.Refresh();
                            }

                            yield return null;
                            AssetDatabase.MoveAsset(_path, _newPath);
                        }
                        continue;
                    }

                    // If something went wrong,
                    // delete asset as it should not exist.
                    yield return null;
                    AssetDatabase.DeleteAsset(_path);
                }
            }

            // Refresh database and delete empty folders
            AssetDatabase.Refresh();
            yield return null;
            EditorUtility.DisplayProgressBar(progressBarTitle, progressBarFolderInfos, ProgressBarAssetRatio);

            string _directory = Application.dataPath + PinAssetsFolder;
            DeleteEmptyDirectories(_directory);

            yield return null;
            EditorUtility.DisplayProgressBar(progressBarTitle, progressBarFolderInfos, 1);
            yield return null;

            // Refresh and clear progress bar.
            AssetDatabase.Refresh();
            EditorUtility.ClearProgressBar();

            // Reopen loaded scenes before clean up.
            EditorSceneManager.OpenScene(openScenes[0], OpenSceneMode.Single);
            for (int _i = 1; _i < openScenes.Length; _i++)
                EditorSceneManager.OpenScene(openScenes[_i], OpenSceneMode.Additive);

            // ---------- Local Method ---------- //

            void DeleteEmptyDirectories(string _checkDirectory)
            {
                // Delete each directories recursively.
                foreach (var directory in Directory.GetDirectories(_checkDirectory))
                {
                    DeleteEmptyDirectories(directory);
                    if (Directory.GetDirectories(directory).Length == 0)
                    {
                        // If find another file type than meta, do not delete directory.
                        bool _doDestroy = true;
                        foreach (string _file in Directory.GetFiles(directory))
                        {
                            if (_file.EndsWith("meta"))
                                File.Delete(_file);
                            else
                                _doDestroy = false;
                        }

                        if (_doDestroy)
                            Directory.Delete(directory, false);
                    }
                }
            }
        }

        // -----------------------

        /// <summary>
        /// Get if this <see cref="PinAsset"/> matches given ids.
        /// </summary>
        /// <param name="_sceneGUID">GUID of associated scene.</param>
        /// <param name="_objectID">ID of associated object.</param>
        /// <returns>Returns true if ids match, false otherwise.</returns>
        public bool DoMatch(string _sceneGUID, int _objectID) => (_sceneGUID == sceneGUID) && (_objectID == objectID);

        /// <summary>
        /// Pin associated object in scene.
        /// If not found, automatically destroy self.
        /// </summary>
        /// <returns>Returns true if successfully found object, false otherwise.</returns>
        public bool PinAssetInScene()
        {
            Type _type = Type.GetType(objectType);
            string _scenePath = AssetDatabase.GUIDToAssetPath(sceneGUID);
            if (!string.IsNullOrEmpty(_scenePath) && (_type != null))
            {
                EditorSceneManager.OpenScene(_scenePath);
                MonoBehaviour[] _monobehaviours = FindObjectsOfType(_type) as MonoBehaviour[];
                foreach (MonoBehaviour _monobehaviour in _monobehaviours)
                {
                    if (_monobehaviour.gameObject.name.GetHashCode() == objectID)
                    {
                        EditorGUIUtility.PingObject(_monobehaviour);
                        Selection.activeObject = _monobehaviour;
                        return true;
                    }
                }
            }

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
            return false;
        }
        #endregion
    }
}
