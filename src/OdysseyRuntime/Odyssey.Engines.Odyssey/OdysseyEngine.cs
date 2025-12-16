using System;
using JetBrains.Annotations;
using Odyssey.Content.Interfaces;
using Odyssey.Content.ResourceProviders;
using Odyssey.Engines.Common;
using AuroraEngine.Common.Installation;

namespace Odyssey.Engines.Odyssey
{
    /// <summary>
    /// Odyssey Engine implementation for KOTOR 1/2.
    /// </summary>
    /// <remarks>
    /// Engine Initialization:
    /// - Based on swkotor2.exe engine initialization system
    /// - Located via string references: "ModuleLoaded" @ 0x007bdd70, "ModuleRunning" @ 0x007bdd58
    /// - Original implementation: Engine initializes resource provider, game session, and module loader
    /// - Resource provider: Accesses game installation directory for file loading (GFF, 2DA, MDL, etc.)
    /// - Game session: Coordinates module loading, entity management, script execution, combat, AI
    /// - Module loader: Handles loading module files (MOD, ARE, GIT, etc.) and spawning entities
    /// </remarks>
    public class OdysseyEngine : BaseEngine
    {
        private string _installationPath;

        public OdysseyEngine(IEngineProfile profile)
            : base(profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            if (profile.EngineFamily != EngineFamily.Odyssey)
            {
                throw new ArgumentException("Profile must be for Odyssey engine family", nameof(profile));
            }
        }

        public override IEngineGame CreateGameSession()
        {
            if (!_initialized)
            {
                throw new InvalidOperationException("Engine must be initialized before creating game session");
            }

            return new OdysseyGameSession(this);
        }

        protected override IGameResourceProvider CreateResourceProvider(string installationPath)
        {
            if (string.IsNullOrEmpty(installationPath))
            {
                throw new ArgumentException("Installation path cannot be null or empty", nameof(installationPath));
            }

            _installationPath = installationPath;
            Installation installation = new Installation(installationPath);
            return new GameResourceProvider(installation);
        }
    }
}

