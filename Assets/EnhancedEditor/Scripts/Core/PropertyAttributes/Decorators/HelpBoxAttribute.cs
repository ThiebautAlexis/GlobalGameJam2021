// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using System;
using UnityEngine;

namespace EnhancedEditor
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class HelpBoxAttribute : PropertyAttribute
    {
        #region Fields
        /// <summary>
        /// Help box type do display.
        /// </summary>
        public readonly MessageType Type = MessageType.Info;

        /// <summary>
        /// Label displayed in the box.
        /// </summary>
        public readonly string Label = string.Empty;
        #endregion

        #region Constructor
        /// <summary>
        /// Display a help box with a message on top of the field in the inspector.
        /// </summary>
        /// <param name="_label">Label to display.</param>
        /// <param name="_type">Help box type.</param>
        public HelpBoxAttribute(string _label, MessageType _type)
        {
            Type = _type;
            Label = _label;
        }
        #endregion
    }
}
