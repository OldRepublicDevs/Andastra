using System;
using System.IO;
using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.ERF;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Formats.RIM;
using CSharpKOTOR.Formats.SSF;
using CSharpKOTOR.Formats.TLK;
using CSharpKOTOR.Formats.TwoDA;
using CSharpKOTOR.Resource.Formats.LYT;
using CSharpKOTOR.Resource.Formats.VIS;
using CSharpKOTOR.Resource.Generics;
using JetBrains.Annotations;

namespace CSharpKOTOR.Resources
{
    /// <summary>
    /// Automatic resource loading and saving utilities.
    /// Provides automatic detection and loading of different resource types.
    /// </summary>
    [PublicAPI]
    public static class ResourceAuto
    {
        /// <summary>
        /// Automatically loads a resource from file data based on its type.
        /// </summary>
        /// <param name="data">The resource data.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <returns>The loaded resource object, or null if loading failed.</returns>
        [CanBeNull]
        public static object LoadResource(byte[] data, ResourceType resourceType)
        {
            if (data == null || resourceType == null)
            {
                return null;
            }

            try
            {
                switch (resourceType.TypeId)
                {
                    case 2002: // ERF
                        var erfReader = new ERFBinaryReader(data);
                        return erfReader.Load();

                    case 2005: // GFF
                        var gffReader = new GFFBinaryReader(data);
                        return gffReader.Load();

                    case 2008: // RIM
                        var rimReader = new RIMBinaryReader(data);
                        return rimReader.Load();

                    case 2017: // SSF
                        var ssfReader = new SSFBinaryReader(data);
                        return ssfReader.Load();

                    case 2018: // TLK
                        var tlkReader = new TLKBinaryReader(data);
                        return tlkReader.Load();

                    case 2019: // TwoDA
                        var twodaReader = new TwoDABinaryReader(data);
                        return twodaReader.Load();

                    // Add more resource types as they are implemented
                    default:
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Automatically loads a resource from a file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The loaded resource object, or null if loading failed.</returns>
        [CanBeNull]
        public static object LoadResourceFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }

            try
            {
                byte[] data = File.ReadAllBytes(filePath);
                string extension = Path.GetExtension(filePath).ToLowerInvariant();

                ResourceType resourceType = ResourceType.FromExtension(extension);
                if (resourceType == null)
                {
                    return null;
                }

                return LoadResource(data, resourceType);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Automatically saves a resource object to bytes based on its type.
        /// </summary>
        /// <param name="resource">The resource object.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <returns>The resource data as bytes, or null if saving failed.</returns>
        [CanBeNull]
        public static byte[] SaveResource(object resource, ResourceType resourceType)
        {
            if (resource == null || resourceType == null)
            {
                return null;
            }

            try
            {
                // Placeholder - full implementation would have save logic for each type
                // This would require implementing write methods for each format
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Automatically saves a resource object to a file.
        /// </summary>
        /// <param name="resource">The resource object.</param>
        /// <param name="resourceType">The resource type.</param>
        /// <param name="filePath">The file path to save to.</param>
        /// <returns>True if saving succeeded, false otherwise.</returns>
        public static bool SaveResourceToFile(object resource, ResourceType resourceType, string filePath)
        {
            if (resource == null || resourceType == null || string.IsNullOrEmpty(filePath))
            {
                return false;
            }

            try
            {
                byte[] data = SaveResource(resource, resourceType);
                if (data == null)
                {
                    return false;
                }

                File.WriteAllBytes(filePath, data);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
