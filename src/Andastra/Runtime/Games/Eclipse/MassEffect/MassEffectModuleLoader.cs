using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Content.Interfaces;

namespace Andastra.Runtime.Engines.Eclipse.MassEffect
{
    /// <summary>
    /// Mass Effect module/package loader implementation (MassEffect.exe).
    /// </summary>
    /// <remarks>
    /// Mass Effect Package Loading:
    /// - Based on MassEffect.exe: intABioSPGameexecPreloadPackage @ 0x117fede8, Engine.StartupPackages @ 0x11849d54
    /// - Package @ 0x11849d84, intUBioMorphFaceFrontEndexecPreload2DAPackage @ 0x1180ecc0
    /// - Inherits common Mass Effect package loading from MassEffectModuleLoaderBase
    /// </remarks>
    public class MassEffectModuleLoader : MassEffectModuleLoaderBase
    {
        public MassEffectModuleLoader(IWorld world, IGameResourceProvider resourceProvider)
            : base(world, resourceProvider)
        {
        }

        // All common Mass Effect package loading logic is in MassEffectModuleLoaderBase
        // Override LoadMassEffectPackageAsync if ME1 has specific differences
    }
}

