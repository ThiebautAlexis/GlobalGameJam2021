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
    /// Base class to derive custom class attributes from.
    /// Use this to create custom attributes for your <see cref="UnityEngine.Object"/> script classes.
    /// 
    /// A custom attribute can be hooked up with a custom Drawer class
    /// to get callbacks when the script editor is being drawn in the inspector.
    /// </summary>
    [Conditional("UNITY_EDITOR")]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public abstract class ClassAttribute : Attribute { }
}
