using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Andastra.Parsing.Resource.Generics.DLG;

namespace HolocronToolset.Editors.DLG
{
    /// <summary>
    /// Debug utilities for DLG editor.
    /// Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/debug_utils.py
    /// </summary>
    public static class DLGDebugUtils
    {
        /// <summary>
        /// Generate a string representation of the object with additional details.
        /// Matching PyKotor: def custom_extra_info(obj) -> str
        /// </summary>
        public static string CustomExtraInfo(object obj)
        {
            if (obj == null)
            {
                return "null";
            }
            return $"{obj.GetType().Name} id={RuntimeHelpers.GetHashCode(obj)}";
        }

        /// <summary>
        /// Custom function to provide additional details about objects in the graph.
        /// Matching PyKotor: def detailed_extra_info(obj) -> str
        /// </summary>
        public static string DetailedExtraInfo(object obj)
        {
            if (obj == null)
            {
                return "null";
            }
            try
            {
                return obj.ToString();
            }
            catch (Exception)
            {
                return obj.GetType().Name;
            }
        }

        /// <summary>
        /// Filter to decide if the object should be included in the graph.
        /// Matching PyKotor: def is_interesting(obj) -> bool
        /// </summary>
        public static bool IsInteresting(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            // Check if object has fields/properties (has __dict__ equivalent)
            Type type = obj.GetType();
            return type.GetFields(BindingFlags.Public | BindingFlags.Instance).Length > 0 ||
                   type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Length > 0 ||
                   obj is System.Collections.ICollection;
        }

        /// <summary>
        /// Display a graph of back references for the given target_object.
        /// Matching PyKotor: def identify_reference_path(obj, max_depth=10)
        /// Note: Full implementation would require objgraph library equivalent
        /// </summary>
        public static void IdentifyReferencePath(object obj, int maxDepth = 10)
        {
            if (obj == null)
            {
                return;
            }
            // TODO: SIMPLIFIED - Full implementation would require reference tracking
            // For now, just output basic information
            Debug.WriteLine($"Reference Path for {obj.GetType().Name}:");
            Debug.WriteLine($"  Type={obj.GetType().Name}, ID={RuntimeHelpers.GetHashCode(obj)}");
        }

        /// <summary>
        /// Debug references for an object.
        /// Matching PyKotor: def debug_references(obj: Any)
        /// </summary>
        public static void DebugReferences(object obj)
        {
            IdentifyReferencePath(obj);
        }
    }
}

