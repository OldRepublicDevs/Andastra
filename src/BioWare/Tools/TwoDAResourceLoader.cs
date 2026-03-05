using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Common.Logger;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.TwoDA;

namespace BioWare.Tools
{
    /// <summary>
    /// Shared 2DA loading helper for installation-backed resource resolution.
    /// Uses locations first, then falls back to direct resource lookup.
    /// </summary>
    internal static class TwoDAResourceLoader
    {
        internal static TwoDA LoadFromInstallation(
            Installation installation,
            string resourceName,
            RobustLogger logger = null)
        {
            if (installation == null)
            {
                throw new ArgumentNullException(nameof(installation));
            }

            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentException("Resource name cannot be null or whitespace.", nameof(resourceName));
            }

            if (logger == null)
            {
                logger = new RobustLogger();
            }

            TwoDA result = TryLoadFromLocations(installation, resourceName, logger)
                ?? TryLoadFromLookup(installation, resourceName, logger);

            return result;
        }

        private static TwoDA TryLoadFromLocations(
            Installation installation,
            string resourceName,
            RobustLogger logger)
        {
            try
            {
                var locationResults = installation.Locations(
                    new List<ResourceIdentifier> { new ResourceIdentifier(resourceName, ResourceType.TwoDA) },
                    new[] { SearchLocation.OVERRIDE, SearchLocation.CHITIN });

                foreach (var kvp in locationResults)
                {
                    if (kvp.Value == null || kvp.Value.Count == 0)
                    {
                        continue;
                    }

                    var loc = kvp.Value[0];
                    if (string.IsNullOrEmpty(loc.FilePath) || !File.Exists(loc.FilePath))
                    {
                        continue;
                    }

                    using (var file = File.OpenRead(loc.FilePath))
                    {
                        file.Seek(loc.Offset, SeekOrigin.Begin);
                        var data = new byte[loc.Size];
                        file.Read(data, 0, loc.Size);

                        return new TwoDABinaryReader(data).Load();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Debug($"locations() failed for {resourceName}.2da: {ex}");
            }

            return null;
        }

        private static TwoDA TryLoadFromLookup(
            Installation installation,
            string resourceName,
            RobustLogger logger)
        {
            try
            {
                var resource = installation.Resources.LookupResource(resourceName, ResourceType.TwoDA);
                if (resource?.Data != null)
                {
                    return new TwoDABinaryReader(resource.Data).Load();
                }
            }
            catch (Exception ex)
            {
                logger.Debug($"resource() also failed for {resourceName}.2da: {ex}");
            }

            return null;
        }
    }
}
