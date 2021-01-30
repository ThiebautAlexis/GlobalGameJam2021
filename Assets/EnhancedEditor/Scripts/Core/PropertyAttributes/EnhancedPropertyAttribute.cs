// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using System;
using System.Diagnostics;
using UnityEngine;

namespace EnhancedEditor
{
    /// <summary>
    /// Base class to derive custom property attributes from.
    /// Use this to create and assign multiple custom attributes to your script variables.
    /// 
    /// A custom attribute can be hooked up with a custom Drawer class
    /// to customize the way the variable is drawn in the inspector.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnhancedPropertyAttribute : PropertyAttribute { }
}
