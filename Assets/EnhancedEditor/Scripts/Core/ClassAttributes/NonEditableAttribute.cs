// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

namespace EnhancedEditor
{
    /// <summary>
    /// Avoid an object inspector from being drawn.
    /// A customizable message will be displayed instead.
    /// </summary>
	public class NonEditableAttribute : ClassAttribute
    {
        /// <summary>
        /// Message displayed instead of the usual inspector.
        /// </summary>
        public readonly string Message = "Editing this object is not allowed.\nThese datas may be sensitives.";

        public NonEditableAttribute() { }

        /// <summary>
        /// Associates a new PinAsset to every instance of this component in a scene.
        /// Created PinAsset can be used to instantly ping link object.
        /// </summary>
        /// <param name="_assetsFolder">Folder where to create associated PinAsset.</param>
        public NonEditableAttribute(string _message) => Message = _message;
    }
}
