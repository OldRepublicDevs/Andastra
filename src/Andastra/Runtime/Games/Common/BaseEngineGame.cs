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
    /// - Common game session management pattern across all BioWare engines
    /// - Cross-engine analysis shows similar module state management patterns:
    ///   - Odyssey: Server command parser, manages module state flags
    ///   - Aurora: Similar module state management (module loading/unloading, state flags)
    ///   - Eclipse: Similar module state management
    ///   - Infinity: Similar module state management (ARE/GAM file-based)
    /// - Inheritance: Base class BaseEngineGame (Runtime.Games.Common) implements common module state management
    ///   - Odyssey: OdysseyGameSession : BaseEngineGame (Runtime.Games.Odyssey)
    ///   - Aurora: AuroraGameSession : BaseEngineGame (Runtime.Games.Aurora)
    ///   - Eclipse: EclipseGameSession : BaseEngineGame (Runtime.Games.Eclipse)
    ///   - Infinity: InfinityGameSession : BaseEngineGame (Runtime.Games.Infinity)
    /// - Common module state management:
    ///   - State 0 = Idle (no module loaded)
    ///   - State 1 = ModuleLoaded (module loaded but not running)
    ///   - State 2 = ModuleRunning (module loaded and running)
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


