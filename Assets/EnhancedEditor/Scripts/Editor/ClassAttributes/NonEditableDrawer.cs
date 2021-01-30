// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using UnityEngine;
using UnityEditor;

namespace EnhancedEditor.Editor
{
    [CustomDrawer(typeof(NonEditableAttribute))]
    public class NonEditableDrawer : UnityObjectDrawer
    {
        private bool isInitialized = false;
        private GUIContent messageGUI = GUIContent.none;
        private GUIStyle messageStyle = null;

        // -----------------------

        /// <summary>
        /// Use this function to alter editing object(s) inspector.
        /// Return value determines if inspector should continue being drawn or not.
        /// </summary>
        /// <returns>Returns true to continue drawing inspector, false otherwise.</returns>
        public override bool OnInspectorGUI()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                messageGUI = new GUIContent(((NonEditableAttribute)attribute).Message);

                messageStyle = new GUIStyle(EditorStyles.boldLabel);
                messageStyle.alignment = TextAnchor.MiddleCenter;
                messageStyle.fontSize = 24;
                messageStyle.wordWrap = true;
            }

            Rect _rect = EditorGUILayout.GetControlRect(true, Screen.height * .5f);
            EditorGUI.LabelField(_rect, messageGUI, messageStyle);
            return false;
        }
    }
}
