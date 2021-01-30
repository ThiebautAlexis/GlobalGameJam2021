// ===== Enhanced Editor - https://github.com/LucasJoestar/EnhancedEditor ===== //
// 
// Notes:
//
// ============================================================================ //

using System;
using System.Collections.Generic;
using System.Reflection;

namespace EnhancedEditor.Editor
{
	internal static class EnhancedDrawerUtility
    {
        /// <summary>
        /// <see cref="UnityObjectDrawer"/> as key, target class as value.
        /// </summary>
        private static Dictionary<Type, Type> customDrawers = new Dictionary<Type, Type>();

        private static bool isInitialized = false;

        // -----------------------

        public static Dictionary<Type, Type> GetCustomDrawers()
        {
            // When not initialized, search for all custom drawers among project.
            if (!isInitialized)
            {
                isInitialized = true;
                foreach (Assembly _assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type _type in _assembly.GetTypes())
                    {
                        // Register custom drawer type if having a target class.
                        if (_type.IsSubclassOf(typeof(UnityObjectDrawer)) && !_type.IsAbstract)
                        {
                            CustomDrawerAttribute _drawer = _type.GetCustomAttribute<CustomDrawerAttribute>(true);
                            if (_drawer != null)
                                customDrawers[_type] = _drawer.targetType;
                        }
                    }
                }
            }

            return customDrawers;
        }
    }
}
