// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using System;
using System.Collections.Generic;
using UnityEditor;

using Object = UnityEngine.Object;

namespace EnhancedEditor.Editor
{
    /// <summary>
    /// Base class to derive custom class editors from
    /// to keep using drawers for <see cref="ClassAttribute"/>.
    /// </summary>
    [CustomEditor(typeof(Object), true)]
	public class UnityObjectEditor : UnityEditor.Editor
    {
        private List<UnityObjectDrawer> monoDrawers = new List<UnityObjectDrawer>();

        // -----------------------

        public override void OnInspectorGUI()
        {
            // Draw inspector while authorized.
            for (int _i = 0; _i < monoDrawers.Count; _i++)
            {
                if (!monoDrawers[_i].OnInspectorGUI())
                    return;
            }

            base.OnInspectorGUI();
        }

        protected virtual void OnEnable()
        {
            // Get all ClassAttribute from editing objects.
            Type _type = serializedObject.targetObject.GetType();
            var _attributes = _type.GetCustomAttributes(typeof(ClassAttribute), true) as ClassAttribute[];

            // Create and enable associated drawer for each attribute.
            monoDrawers.Clear();
            foreach (KeyValuePair<Type, Type> _pair in EnhancedDrawerUtility.GetCustomDrawers())
            {
                foreach (ClassAttribute _attribute in _attributes)
                {
                    if (_pair.Value == _attribute.GetType())
                    {
                        UnityObjectDrawer _customDrawer = UnityObjectDrawer.CreateInstance(_pair.Key, serializedObject, _attribute);
                        _customDrawer.OnEnable();

                        monoDrawers.Add(_customDrawer);
                    }
                }
            }
        }

        protected virtual void OnDisable()
        {
            for (int _i = 0; _i < monoDrawers.Count; _i++)
                monoDrawers[_i].OnDisable();
        }
    }
}
