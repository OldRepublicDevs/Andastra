using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Navigation;
using Andastra.Runtime.Content.Interfaces;

namespace Andastra.Runtime.Engines.Common
{
    /// <summary>
    /// Abstract base class for module management across all engines.
    /// </summary>
    /// <remarks>
    /// Base Engine Module:
    /// - Based on swkotor2.exe: FUN_006caab0 @ 0x006caab0 (server command parser, handles module state management)
    /// - Located via string references: "ModuleLoaded" @ 0x007bdd70, "ModuleRunning" @ 0x007bdd58, "MODULE" @ module loading
    /// - Cross-engine analysis:
    ///   - Odyssey (swkotor.exe, swkotor2.exe): FUN_006caab0 @ 0x006caab0 (swkotor2.exe) - server command parser, manages module state flags
    ///   - Aurora (nwmain.exe): Similar module loading system (module.ifo, area files, entity spawning)
    ///   - Eclipse (daorigins.exe, DragonAge2.exe, MassEffect.exe, MassEffect2.exe): Similar module loading system
    ///   - Infinity (BaldurGate.exe, IcewindDale.exe, PlanescapeTorment.exe): Similar module loading system (ARE/WED/GAM files)
    /// - Inheritance: Base class BaseEngineModule (Runtime.Games.Common) implements common module loading/unloading
    ///   - Odyssey: OdysseyModuleLoader : BaseEngineModule (Runtime.Games.Odyssey)
    ///   - Aurora: AuroraModuleLoader : BaseEngineModule (Runtime.Games.Aurora)
    ///   - Eclipse: EclipseModuleLoader : BaseEngineModule (Runtime.Games.Eclipse)
    ///   - Infinity: InfinityModuleLoader : BaseEngineModule (Runtime.Games.Infinity)
    /// - Original implementation: FUN_006caab0 parses server commands, manages module state flags (Idle=0, ModuleLoaded=1, ModuleRunning=2)
    /// - Module loading order: IFO (module info) -> LYT (layout) -> VIS (visibility) -> GIT (instances) -> ARE (area properties)
    /// - Module management: Loads module areas, navigation meshes, entities from GIT files
    /// - Module state: Tracks current module name, current area, navigation mesh
    /// </remarks>
    public abstract class BaseEngineModule : IEngineModule
    {
        protected readonly IWorld _world;
        protected readonly IGameResourceProvider _resourceProvider;
        protected string _currentModuleName;
        protected IArea _currentArea;
        protected NavigationMesh _currentNavigationMesh;

        protected BaseEngineModule(IWorld world, IGameResourceProvider resourceProvider)
        {
            if (world == null)
            {
                throw new ArgumentNullException(nameof(world));
            }

            if (resourceProvider == null)
            {
                throw new ArgumentNullException(nameof(resourceProvider));
            }

            _world = world;
            _resourceProvider = resourceProvider;
        }

        [CanBeNull]
        public string CurrentModuleName
        {
            get { return _currentModuleName; }
            protected set { _currentModuleName = value; }
        }

        [CanBeNull]
        public IArea CurrentArea
        {
            get { return _currentArea; }
            protected set { _currentArea = value; }
        }

        [CanBeNull]
        public NavigationMesh CurrentNavigationMesh
        {
            get { return _currentNavigationMesh; }
            protected set { _currentNavigationMesh = value; }
        }

        public abstract Task LoadModuleAsync(string moduleName, [CanBeNull] Action<float> progressCallback = null);

        public virtual void UnloadModule()
        {
            if (_currentModuleName != null)
            {
                OnUnloadModule();
                _currentModuleName = null;
                _currentArea = null;
                _currentNavigationMesh = null;
            }
        }

        public abstract bool HasModule(string moduleName);

        protected abstract void OnUnloadModule();
    }
}

