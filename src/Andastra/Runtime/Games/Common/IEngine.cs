using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Content.Interfaces;
using Andastra.Runtime.Scripting.Interfaces;

namespace Andastra.Runtime.Engines.Common
{
    /// <summary>
    /// Base interface for all BioWare engine implementations.
    /// </summary>
    /// <remarks>
    /// Engine Interface:
    /// - Common contract shared across all BioWare engines (Odyssey, Aurora, Eclipse, Infinity)
    /// - Defines the interface that all engine implementations must provide
    /// - Engine-specific implementations must be in concrete classes (OdysseyEngine, AuroraEngine, EclipseEngine)
    /// - TODO: Complete cross-engine reverse engineering to identify common interface patterns
    /// </remarks>
    public interface IEngine
    {
        /// <summary>
        /// Gets the engine family (Odyssey, Aurora, Eclipse).
        /// </summary>
        EngineFamily EngineFamily { get; }

        /// <summary>
        /// Gets the game profile for this engine instance.
        /// </summary>
        IEngineProfile Profile { get; }

        /// <summary>
        /// Gets the resource provider for loading game resources.
        /// </summary>
        IGameResourceProvider ResourceProvider { get; }

        /// <summary>
        /// Gets the world instance.
        /// </summary>
        IWorld World { get; }

        /// <summary>
        /// Gets the engine API instance.
        /// </summary>
        IEngineApi EngineApi { get; }

        /// <summary>
        /// Creates a new game session for this engine.
        /// </summary>
        IEngineGame CreateGameSession();

        /// <summary>
        /// Initializes the engine with the specified installation path.
        /// </summary>
        void Initialize(string installationPath);

        /// <summary>
        /// Shuts down the engine and cleans up resources.
        /// </summary>
        void Shutdown();
    }

    /// <summary>
    /// Engine family enumeration for grouping related engines.
    /// </summary>
    public enum EngineFamily
    {
        /// <summary>
        /// Aurora Engine (NWN, NWN2)
        /// </summary>
        Aurora,

        /// <summary>
        /// Odyssey Engine (KOTOR, KOTOR2, Jade Empire)
        /// </summary>
        Odyssey,

        /// <summary>
        /// Eclipse/Unreal Engine (Mass Effect series, Dragon Age)
        /// </summary>
        Eclipse,

        /// <summary>
        /// Unknown or unsupported engine
        /// </summary>
        Unknown
    }
}


