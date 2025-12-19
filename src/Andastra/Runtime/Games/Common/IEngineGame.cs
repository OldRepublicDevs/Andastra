using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;

namespace Andastra.Runtime.Engines.Common
{
    /// <summary>
    /// Base interface for game session management across all engines.
    /// </summary>
    /// <remarks>
    /// Engine Game Interface:
    /// - Common contract shared across all BioWare engines (Odyssey, Aurora, Eclipse, Infinity)
    /// - Defines the interface for game session management that all engines must provide
    /// - Engine-specific implementations must be in concrete classes (OdysseyGameSession, AuroraGameSession, EclipseGameSession)
    /// - TODO: Complete cross-engine reverse engineering to identify common game session patterns
    /// </remarks>
    public interface IEngineGame
    {
        /// <summary>
        /// Gets the current module name.
        /// </summary>
        [CanBeNull]
        string CurrentModuleName { get; }

        /// <summary>
        /// Gets the current player entity.
        /// </summary>
        [CanBeNull]
        IEntity PlayerEntity { get; }

        /// <summary>
        /// Gets the world instance.
        /// </summary>
        IWorld World { get; }

        /// <summary>
        /// Loads a module by name.
        /// </summary>
        Task LoadModuleAsync(string moduleName, [CanBeNull] Action<float> progressCallback = null);

        /// <summary>
        /// Unloads the current module.
        /// </summary>
        void UnloadModule();

        /// <summary>
        /// Updates the game session (called every frame).
        /// </summary>
        void Update(float deltaTime);

        /// <summary>
        /// Shuts down the game session.
        /// </summary>
        void Shutdown();
    }
}


