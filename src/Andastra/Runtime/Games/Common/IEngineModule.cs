using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Navigation;

namespace Andastra.Runtime.Engines.Common
{
    /// <summary>
    /// Base interface for module management across all engines.
    /// </summary>
    /// <remarks>
    /// Engine Module Interface:
    /// - Common contract shared across all BioWare engines (Odyssey, Aurora, Eclipse, Infinity)
    /// - Defines the interface for module management that all engines must provide
    /// - Engine-specific implementations must be in concrete classes (OdysseyModuleLoader, AuroraModuleLoader, EclipseModuleLoader)
    /// - TODO: Complete cross-engine reverse engineering to identify common module management patterns
    /// </remarks>
    public interface IEngineModule
    {
        /// <summary>
        /// Gets the current module name.
        /// </summary>
        [CanBeNull]
        string CurrentModuleName { get; }

        /// <summary>
        /// Gets the current area.
        /// </summary>
        [CanBeNull]
        IArea CurrentArea { get; }

        /// <summary>
        /// Gets the current navigation mesh.
        /// </summary>
        [CanBeNull]
        NavigationMesh CurrentNavigationMesh { get; }

        /// <summary>
        /// Loads a module by name.
        /// </summary>
        Task LoadModuleAsync(string moduleName, [CanBeNull] Action<float> progressCallback = null);

        /// <summary>
        /// Unloads the current module.
        /// </summary>
        void UnloadModule();

        /// <summary>
        /// Checks if a module exists.
        /// </summary>
        bool HasModule(string moduleName);
    }
}


