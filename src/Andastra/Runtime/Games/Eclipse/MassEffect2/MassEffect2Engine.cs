using Andastra.Runtime.Engines.Common;
using Andastra.Parsing.Common;

namespace Andastra.Runtime.Engines.Eclipse.MassEffect2
{
    /// <summary>
    /// Mass Effect 2 engine implementation (MassEffect2.exe).
    /// </summary>
    /// <remarks>
    /// Mass Effect 2 Engine:
    /// - Based on MassEffect2.exe: Eclipse/Unreal Engine architecture
    /// - Similar to MassEffect.exe but with ME2-specific differences
    /// - Save/Load: Similar structure to ME1 but with ME2-specific save format
    /// - Dialogue: Similar conversation system to ME1
    /// - Combat: Similar combat system to ME1
    /// - Module/Package: Similar package system to ME1
    /// </remarks>
    public class MassEffect2Engine : EclipseEngine
    {
        public MassEffect2Engine(IEngineProfile profile)
            : base(profile, Game.ME2)
        {
        }

        protected override IEngineGame CreateGameSessionInternal()
        {
            return new MassEffect2GameSession(this);
        }
    }
}

