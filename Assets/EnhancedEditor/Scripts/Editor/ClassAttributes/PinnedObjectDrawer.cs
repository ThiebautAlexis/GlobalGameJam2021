// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EnhancedEditor.Editor
{
    [CustomDrawer(typeof(PinnedObjectAttribute))]
    public class PinnedObjectDrawer : UnityObjectDrawer
    {
        /// <summary>
        /// This function is called when the editor for target object(s) is loaded.
        /// </summary>
        public override void OnEnable()
        {
            // Get object ID based on its name.
            MonoBehaviour _mono = (MonoBehaviour)serializedObject.targetObject;
            string _name = _mono.gameObject.name;
            int _id = _name.GetHashCode();

            Scene _scene = SceneManager.GetActiveScene();
            string _sceneID = _scene.path;
            if (!string.IsNullOrEmpty(_sceneID))
            {
                _sceneID = AssetDatabase.AssetPathToGUID(_sceneID);
                string[] _pinAssetPaths = Array.ConvertAll(AssetDatabase.FindAssets("t:PinAsset"), AssetDatabase.GUIDToAssetPath);
                foreach (string _path in _pinAssetPaths)
                {
                    PinAsset _pinAsset = AssetDatabase.LoadAssetAtPath<PinAsset>(_path);
                    if (_pinAsset.DoMatch(_sceneID, _id))
                    {
                        // Refresh pin assets database if path has changed.
                        string _oldSceneName = Path.GetFileName(Path.GetDirectoryName(_path));
                        if (_oldSceneName != _scene.name)
                            PinAsset.CleanPinAssets();

                        // If a pin assets associated with this object already exist,
                        // we've got nothing more the do here.
                        return;
                    }
                }

                // Create pin asset for target object.
                PinAsset.CreatePinAsset(Path.Combine(((PinnedObjectAttribute)attribute).AssetsFolder, _scene.name, _name), _sceneID, _id, _mono.GetType());
            }
        }
    }
}
