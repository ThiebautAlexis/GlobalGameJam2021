// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using UnityEngine;

namespace EnhancedEditor
{
    /// <summary>
    /// Creates an PinAsset (<see cref="ScriptableObject"/>) linked to this object
    /// when a component with this attribute is added to a GameObject.
    /// 
    /// The created asset allow the users to ping the associated GameObject
    /// in its scene from a simple button, easing important scene objects management in project.
    /// </summary>
    public class PinnedObjectAttribute : ClassAttribute
    {
        /// <summary>
        /// Associated assets will be created in this folder from
        /// default PinAsset path.
        /// </summary>
        public readonly string AssetsFolder = string.Empty;

        /// <summary>
        /// Associates a new PinAsset to every instance of this component in a scene.
        /// Created PinAsset can be used to instantly ping link object.
        /// </summary>
        /// <param name="_assetsFolder">Folder where to create associated PinAsset.</param>
        public PinnedObjectAttribute(string _assetsFolder) => AssetsFolder = _assetsFolder;
    }
}
