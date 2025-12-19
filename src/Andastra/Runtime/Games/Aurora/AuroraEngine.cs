using System;
using Andastra.Runtime.Content.Interfaces;
using Andastra.Runtime.Engines.Common;

namespace Andastra.Runtime.Engines.Aurora
{
    /// <summary>
    /// Aurora Engine implementation for Neverwinter Nights and Neverwinter Nights 2.
    /// </summary>
    /// <remarks>
    /// Aurora Engine:
    /// - Based on Aurora Engine architecture (Neverwinter Nights, Neverwinter Nights 2)
    /// - Note: This engine is for different games (NWN/NWN2), not KOTOR, so no swkotor2.exe references needed
    /// - Engine initialization: Similar pattern to Odyssey engine but for Aurora-based games
    /// - Resource provider: Uses Aurora-specific resource system (hak files, module files, etc.)
    /// - Game session: Coordinates module loading, entity management, script execution for Aurora games
    /// </remarks>
    public class AuroraEngine : BaseEngine
    {
        public AuroraEngine(IEngineProfile profile)
            : base(profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            if (profile.EngineFamily != EngineFamily.Aurora)
            {
                throw new ArgumentException("Profile must be for Aurora engine family", nameof(profile));
            }
        }

        public override IEngineGame CreateGameSession()
        {
            if (!_initialized)
            {
                throw new InvalidOperationException("Engine must be initialized before creating game session");
            }

            // TODO: STUB - Implement AuroraGameSession
            throw new NotImplementedException("Aurora game session not yet implemented");
        }

        protected override IGameResourceProvider CreateResourceProvider(string installationPath)
        {
            // TODO: STUB - Implement Aurora-specific resource provider
            throw new NotImplementedException("Aurora resource provider not yet implemented");
        }
    }
}


