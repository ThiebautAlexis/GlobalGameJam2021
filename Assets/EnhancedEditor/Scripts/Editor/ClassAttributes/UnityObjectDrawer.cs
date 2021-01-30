// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using System;
using UnityEditor;

namespace EnhancedEditor.Editor
{
    /// <summary>
    /// Derive from this base class to create a custom drawer for your custom class attribute
    /// (which must inherit from <see cref="ClassAttribute"/>).
    /// </summary>
    public abstract class UnityObjectDrawer
    {
        #region Content
        /// <summary>
        /// Creates a new instance of a <see cref="UnityObjectDrawer"/>
        /// with a specified <see cref="SerializedObject"/> target.
        /// </summary>
        /// <param name="_type">Class type to create. Must inherit from <see cref="UnityObjectDrawer"/>!</param>.
        /// <param name="_serializedObject">Target editing <see cref="SerializedObject"/>.</param>
        /// <returns>Returns newly created <see cref="UnityObjectDrawer"/> instance.</returns>
        public static UnityObjectDrawer CreateInstance(Type _type, SerializedObject _serializedObject, ClassAttribute _attribute)
        {
            UnityObjectDrawer _drawer = (UnityObjectDrawer)Activator.CreateInstance(_type);
            _drawer.serializedObject = _serializedObject;
            _drawer.attribute = _attribute;
            return _drawer;
        }

        // -------------------------------------------
        // Drawer Callbacks & Drawer
        // -------------------------------------------

        /// <summary>
        /// A <see cref="SerializedObject"/> representing the object or objects being inspected.
        /// </summary>
        protected SerializedObject serializedObject = null;

        /// <summary>
        /// <see cref="ClassAttribute"/> associated with this drawer.
        /// </summary>
        protected ClassAttribute attribute = null;

        // -----------------------

        /// <summary>
        /// Use this function to alter editing object(s) inspector.
        /// Return value determines if inspector should continue being drawn or not.
        /// </summary>
        /// <returns>Returns true to continue drawing inspector, false otherwise.</returns>
        public virtual bool OnInspectorGUI() => true;

        /// <summary>
        /// This function is called when the editor for target object(s) is loaded.
        /// </summary>
        public virtual void OnEnable() { }

        /// <summary>
        /// This function is called when the editor for target object(s) goes out of scope.
        /// </summary>
        public virtual void OnDisable() { }
        #endregion
    }
}
