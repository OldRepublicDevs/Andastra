using Andastra.Runtime.Engines.Eclipse;

namespace Andastra.Runtime.Engines.Eclipse.MassEffect2
{
    /// <summary>
    /// Mass Effect 2 game session implementation (MassEffect2.exe).
    /// </summary>
    /// <remarks>
    /// Mass Effect 2 Game Session:
    /// - Based on MassEffect2.exe: Similar to ME1 but with ME2-specific differences
    /// - Uses MassEffect2ModuleLoader for package loading
    /// - Uses MassEffect2-specific save/load system
    /// </remarks>
    public class MassEffect2GameSession : EclipseGameSession
    {
        public MassEffect2GameSession(MassEffect2Engine engine)
            : base(engine)
        {
        }

        protected override EclipseModuleLoader CreateModuleLoader()
        {
            return new MassEffect2ModuleLoader(_engine.World, _engine.ResourceProvider);
        }
    }
}

