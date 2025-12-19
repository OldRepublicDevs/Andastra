using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Content.Interfaces;
using Andastra.Runtime.Engines.Eclipse.MassEffect;

namespace Andastra.Runtime.Engines.Eclipse.MassEffect2
{
    /// <summary>
    /// Mass Effect 2 module/package loader implementation (MassEffect2.exe).
    /// </summary>
    /// <remarks>
    /// Mass Effect 2 Package Loading:
    /// - Based on MassEffect2.exe: Similar to ME1 but with ME2-specific differences
    /// - Inherits common Mass Effect package loading from MassEffectModuleLoaderBase
    /// </remarks>
    public class MassEffect2ModuleLoader : MassEffectModuleLoaderBase
    {
        public MassEffect2ModuleLoader(IWorld world, IGameResourceProvider resourceProvider)
            : base(world, resourceProvider)
        {
        }

        // All common Mass Effect package loading logic is in MassEffectModuleLoaderBase
        // Override LoadMassEffectPackageAsync if ME2 has specific differences
    }
}

