using System;
using Andastra.Runtime.Content.Interfaces;
using Andastra.Runtime.Engines.Common;

namespace Andastra.Runtime.Engines.Infinity
{
    /// <summary>
    /// Infinity Engine implementation for ___________________.
    /// </summary>
    /// <remarks>
    /// Infinity Engine:
    /// - Based on Infinity Engine architecture (___________________)
    /// - Resource provider: Uses Infinity-specific resource system (hak files, module files, etc.)
    /// - Game session: Coordinates module loading, entity management, script execution for Infinity games
    /// </remarks>
    public class InfinityEngine : BaseEngine
    {
        public InfinityEngine(IEngineProfile profile)
            : base(profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            if (profile.EngineFamily != EngineFamily.Infinity)
            {
                throw new ArgumentException("Profile must be for Infinity engine family", nameof(profile));
            }
        }

        public override IEngineGame CreateGameSession()
        {
            if (!_initialized)
            {
                throw new InvalidOperationException("Engine must be initialized before creating game session");
            }

            return new InfinityGameSession(this);
        }

        protected override IGameResourceProvider CreateResourceProvider(string installationPath)
        {
            // TODO: STUB - Implement Infinity-specific resource provider
            throw new NotImplementedException("Infinity resource provider not yet implemented");
        }
    }
}


