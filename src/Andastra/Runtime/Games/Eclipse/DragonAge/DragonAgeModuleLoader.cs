using System;
using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Content.Interfaces;
using Andastra.Parsing.Installation;

namespace Andastra.Runtime.Engines.Eclipse.DragonAge
{
    /// <summary>
    /// Base module loader for Dragon Age series (DA:O and DA2).
    /// </summary>
    /// <remarks>
    /// Dragon Age Module Loading (Common):
    /// - Both DA:O and DA2 use MODULES directory structure
    /// - DA:O: LoadModule @ 0x00b17da4, MODULES @ 0x00ad9810, WRITE_MODULES @ 0x00ad98d8
    /// - DA2: LoadModuleMessage @ 0x00bf5df8, MODULES: @ 0x00bf5d10, WRITE_MODULES: @ 0x00bf5d24
    /// - Module format: .rim files, area files, etc.
    /// </remarks>
    public abstract class DragonAgeModuleLoader : EclipseModuleLoader
    {
        protected DragonAgeModuleLoader(IWorld world, IGameResourceProvider resourceProvider)
            : base(world, resourceProvider)
        {
        }

        public override bool HasModule(string moduleName)
        {
            // Dragon Age modules are in MODULES directory (both DA:O and DA2)
            return HasModuleInModulesDirectory(moduleName);
        }

        protected override async Task LoadModuleInternalAsync(string moduleName, [CanBeNull] Action<float> progressCallback)
        {
            progressCallback?.Invoke(0.1f);

            // Load module from MODULES directory (common for both DA:O and DA2)
            string fullModulePath = GetModulePath(moduleName);

            if (!System.IO.Directory.Exists(fullModulePath))
            {
                throw new System.IO.DirectoryNotFoundException($"Module directory not found: {fullModulePath}");
            }

            progressCallback?.Invoke(0.3f);

            // Load module resources
            // Dragon Age modules contain: .rim files, area files, etc.
            await LoadDragonAgeModuleResourcesAsync(fullModulePath, progressCallback);

            progressCallback?.Invoke(0.7f);

            // Set module ID
            _currentModuleId = moduleName;

            progressCallback?.Invoke(0.9f);
        }

        /// <summary>
        /// Load Dragon Age-specific module resources.
        /// Override in subclasses for game-specific differences.
        /// </summary>
        protected virtual async Task LoadDragonAgeModuleResourcesAsync(string modulePath, [CanBeNull] Action<float> progressCallback)
        {
            // TODO: Load module.rim, area files, etc.
            // This requires understanding Dragon Age module format structure
            await Task.CompletedTask;
        }
    }
}

