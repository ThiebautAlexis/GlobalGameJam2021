// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using System;

namespace EnhancedEditor.Editor
{
    /// <summary>
    /// Tells an Enhanced Drawer class which run-time type it's an drawer for.
    /// When you make a custom drawer for a class or an attribute, you need put this attribute on the editor class.
    /// </summary>
    public class CustomDrawerAttribute : Attribute
    {
        internal Type targetType = null;

        /// <summary>
        /// Tells an Enhanced Drawer class which run-time type it's an drawer for.
        /// </summary>
        /// <param name="_targetType">Defines which object type the custom drawer class can draw.</param>
        public CustomDrawerAttribute(Type _targetType) => targetType = _targetType;
    }
}
