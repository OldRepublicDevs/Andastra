using Andastra.Runtime.Engines.Eclipse;

namespace Andastra.Runtime.Engines.Eclipse.MassEffect
{
    /// <summary>
    /// Mass Effect game session implementation (MassEffect.exe).
    /// </summary>
    /// <remarks>
    /// Mass Effect Game Session:
    /// - Based on MassEffect.exe: intABioWorldInfoexecBioSaveGame @ 0x11800ca0
    /// - Uses MassEffectModuleLoader for package loading
    /// - Uses MassEffect-specific save/load system
    /// </remarks>
    public class MassEffectGameSession : EclipseGameSession
    {
        public MassEffectGameSession(MassEffectEngine engine)
            : base(engine)
        {
        }

        protected override EclipseModuleLoader CreateModuleLoader()
        {
            return new MassEffectModuleLoader(_engine.World, _engine.ResourceProvider);
        }
    }
}

