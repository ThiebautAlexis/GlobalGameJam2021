// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using UnityEngine;
using UnityEditor;

namespace EnhancedEditor.Editor
{
	[CustomEditor(typeof(PinAsset))]
	public class PinAssetEditor : UnityEditor.Editor
    {
        #region Content
        private readonly GUIContent sectionGUI = new GUIContent("PIN ASSET");
        private readonly GUIContent buttonGUI = new GUIContent("PING OBJECT", "Ping associated object in its scene");

        // -----------------------

        public override void OnInspectorGUI()
        {
            EditorGUILayoutEnhanced.Section(sectionGUI, SectionAttribute.DefaultLineWidth);
            if (GUILayout.Button(buttonGUI))
                ((PinAsset)serializedObject.targetObject).PinAssetInScene();
        }
        #endregion
    }
}
