using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;

namespace Andastra.Runtime.Engines.Common
{
    /// <summary>
    /// Abstract base class for game session management across all engines.
    /// </summary>
    /// <remarks>
    /// Base Engine Game:
    /// - Based on swkotor2.exe: FUN_006caab0 @ 0x006caab0 (server command parser, handles module state management)
    /// - Located via string references: "ModuleLoaded" @ 0x007bdd70, "ModuleRunning" @ 0x007bdd58, "ServerStatus" @ 0x007bdd8c
    /// - Cross-engine analysis:
    ///   - Odyssey (swkotor.exe, swkotor2.exe): FUN_006caab0 @ 0x006caab0 (swkotor2.exe) - server command parser, manages module state flags
    ///   - Aurora (nwmain.exe): Similar module state management (module loading/unloading, state flags)
    ///   - Eclipse (daorigins.exe, DragonAge2.exe, MassEffect.exe, MassEffect2.exe): Similar module state management
    ///   - Infinity (BaldurGate.exe, IcewindDale.exe, PlanescapeTorment.exe): Similar module state management (ARE/GAM file-based)
    /// - Inheritance: Base class BaseEngineGame (Runtime.Games.Common) implements common module state management
    ///   - Odyssey: OdysseyGameSession : BaseEngineGame (Runtime.Games.Odyssey)
    ///   - Aurora: AuroraGameSession : BaseEngineGame (Runtime.Games.Aurora)
    ///   - Eclipse: EclipseGameSession : BaseEngineGame (Runtime.Games.Eclipse)
    ///   - Infinity: InfinityGameSession : BaseEngineGame (Runtime.Games.Infinity)
    /// - Original implementation: FUN_006caab0 parses server commands starting with 'S', manages module state flags:
    ///   - State 0 = Idle (no module loaded)
    ///   - State 1 = ModuleLoaded (module loaded but not running)
    ///   - State 2 = ModuleRunning (module loaded and running)
    /// - Module state stored in DAT_008283d4 structure, accessed via FUN_00638850
    /// - Module commands: "Module" command loads module, "SavegameList" command lists save games
    /// - Game session: Manages current module, player entity, world state, module transitions
    /// </remarks>
    public abstract class BaseEngineGame : IEngineGame
    {
        protected readonly IEngine _engine;
        protected readonly IWorld _world;
        protected string _currentModuleName;
        protected IEntity _playerEntity;

        protected BaseEngineGame(IEngine engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            _engine = engine;
            _world = engine.World;
        }

        [CanBeNull]
        public string CurrentModuleName
        {
            get { return _currentModuleName; }
            protected set { _currentModuleName = value; }
        }

        [CanBeNull]
        public IEntity PlayerEntity
        {
            get { return _playerEntity; }
            protected set { _playerEntity = value; }
        }

        public IWorld World
        {
            get { return _world; }
        }

        public abstract Task LoadModuleAsync(string moduleName, [CanBeNull] Action<float> progressCallback = null);

        public virtual void UnloadModule()
        {
            if (_currentModuleName != null)
            {
                OnUnloadModule();
                _currentModuleName = null;
                _playerEntity = null;
            }
        }

        public virtual void Update(float deltaTime)
        {
            if (_world != null)
            {
                _world.Update(deltaTime);
            }
        }

        public virtual void Shutdown()
        {
            UnloadModule();
        }

        protected abstract void OnUnloadModule();
    }
}


