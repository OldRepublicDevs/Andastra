using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Content.Interfaces;

namespace Andastra.Runtime.Engines.Eclipse.MassEffect
{
    /// <summary>
    /// Base module/package loader for Mass Effect series (ME1 and ME2).
    /// </summary>
    /// <remarks>
    /// Mass Effect Package Loading (Common):
    /// - Mass Effect uses packages instead of modules
    /// - ME1: intABioSPGameexecPreloadPackage @ 0x117fede8, Engine.StartupPackages @ 0x11849d54, Package @ 0x11849d84
    /// - ME2: Similar package system to ME1
    /// - Loads packages from Packages directory
    /// </remarks>
    public abstract class MassEffectModuleLoaderBase : EclipseModuleLoader
    {
        protected MassEffectModuleLoaderBase(IWorld world, IGameResourceProvider resourceProvider)
            : base(world, resourceProvider)
        {
        }

        /// <summary>
        /// Checks if a Mass Effect package exists and can be loaded.
        /// </summary>
        /// <param name="packageName">The resource reference name of the package to check.</param>
        /// <returns>True if the package exists and can be loaded, false otherwise.</returns>
        /// <remarks>
        /// Mass Effect Package Existence Check:
        /// - Validates package name (non-null, non-empty)
        /// - Checks for package file in Packages directory: Packages\{packageName}.upk
        /// - Also checks for package file without extension (package name matches directory or file)
        /// - Returns false for invalid input or missing packages
        /// 
        /// Based on MassEffect.exe: Package existence checking pattern (Package @ 0x11849d84)
        /// Mass Effect uses Unreal Engine package format (.upk files) stored in Packages directory.
        /// Package names are case-insensitive and may or may not include the .upk extension.
        /// </remarks>
        public override bool HasModule(string packageName)
        {
            if (string.IsNullOrEmpty(packageName))
            {
                return false;
            }

            try
            {
                // Mass Effect uses packages stored in Packages directory
                // Based on MassEffect.exe: Package @ 0x11849d84
                string packagesPath = _installation.PackagePath();
                
                if (!Directory.Exists(packagesPath))
                {
                    return false;
                }

                // Remove .upk extension if present (package names may or may not include extension)
                string packageNameWithoutExt = packageName;
                if (packageName.EndsWith(".upk", StringComparison.OrdinalIgnoreCase))
                {
                    packageNameWithoutExt = packageName.Substring(0, packageName.Length - 4);
                }

                // Check for package file with .upk extension
                string packageFilePath = Path.Combine(packagesPath, packageNameWithoutExt + ".upk");
                if (File.Exists(packageFilePath))
                {
                    return true;
                }

                // Also check if package name matches a file without extension
                // (some Mass Effect packages may be referenced without extension)
                string packageFileWithoutExt = Path.Combine(packagesPath, packageNameWithoutExt);
                if (File.Exists(packageFileWithoutExt))
                {
                    return true;
                }

                // Check case-insensitive match (Windows file system is case-insensitive, but we check explicitly)
                // Search for matching package files in Packages directory
                if (Directory.Exists(packagesPath))
                {
                    string[] packageFiles = Directory.GetFiles(packagesPath, "*.upk", SearchOption.TopDirectoryOnly);
                    foreach (string packageFile in packageFiles)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(packageFile);
                        if (string.Equals(fileName, packageNameWithoutExt, StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        protected override async Task LoadModuleInternalAsync(string packageName, [CanBeNull] Action<float> progressCallback)
        {
            progressCallback?.Invoke(0.1f);

            // Mass Effect uses packages instead of modules
            await LoadMassEffectPackageAsync(packageName, progressCallback);

            progressCallback?.Invoke(0.5f);

            // Set package name
            _currentModuleId = packageName;

            progressCallback?.Invoke(0.9f);
        }

        /// <summary>
        /// Load Mass Effect-specific package resources.
        /// Override in subclasses for game-specific differences.
        /// </summary>
        protected virtual async Task LoadMassEffectPackageAsync(string packageName, [CanBeNull] Action<float> progressCallback)
        {
            // TODO: Load package from Packages directory
            // This requires understanding Mass Effect package format structure
            await Task.CompletedTask;
        }
    }
}

