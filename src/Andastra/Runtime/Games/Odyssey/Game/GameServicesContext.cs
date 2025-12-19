using System;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Audio;
using Andastra.Runtime.Engines.Odyssey.UI;
using Andastra.Parsing.Installation;

namespace Andastra.Runtime.Engines.Odyssey.Game
{
    /// <summary>
    /// Game services context implementation for Odyssey engine.
    /// Provides access to game systems from script execution context.
    /// </summary>
    /// <remarks>
    /// Game Services Context Implementation:
    /// - Based on swkotor2.exe script execution context system
    /// - Located via string references: Script execution context provides access to game systems
    /// - Original implementation: NWScript execution context (IExecutionContext) provides access to game services
    /// - Services accessible from scripts: DialogueManager, PlayerEntity, CombatManager, PartyManager, ModuleLoader, UISystem
    /// - Based on swkotor2.exe: FUN_005226d0 @ 0x005226d0 (script execution context setup)
    /// </remarks>
    internal class GameServicesContext : IGameServicesContext
    {
        private readonly GameSession _gameSession;
        private readonly IUISystem _uiSystem;
        private readonly object _combatManager;
        private readonly object _partyManager;
        private readonly object _moduleLoader;
        private readonly object _factionManager;
        private readonly object _perceptionManager;
        private readonly object _cameraController;
        private readonly ISoundPlayer _soundPlayer;
        private readonly object _journalSystem;
        private bool _isLoadingFromSave;

        public GameServicesContext(
            GameSession gameSession,
            Installation installation,
            IWorld world,
            object combatManager = null,
            object partyManager = null,
            object moduleLoader = null,
            object factionManager = null,
            object perceptionManager = null,
            object cameraController = null,
            ISoundPlayer soundPlayer = null,
            object journalSystem = null)
        {
            if (gameSession == null)
            {
                throw new ArgumentNullException("gameSession");
            }
            if (installation == null)
            {
                throw new ArgumentNullException("installation");
            }
            if (world == null)
            {
                throw new ArgumentNullException("world");
            }

            _gameSession = gameSession;
            _uiSystem = new OdysseyUISystem(installation, world);
            _combatManager = combatManager;
            _partyManager = partyManager;
            _moduleLoader = moduleLoader;
            _factionManager = factionManager;
            _perceptionManager = perceptionManager;
            _cameraController = cameraController;
            _soundPlayer = soundPlayer;
            _journalSystem = journalSystem;
        }

        public object DialogueManager
        {
            get { return _gameSession.DialogueManager; }
        }

        public IEntity PlayerEntity
        {
            get { return _gameSession.PlayerEntity; }
        }

        public object CombatManager
        {
            get { return _combatManager; }
        }

        public object PartyManager
        {
            get { return _partyManager; }
        }

        public object ModuleLoader
        {
            get { return _moduleLoader; }
        }

        public object FactionManager
        {
            get { return _factionManager; }
        }

        public object PerceptionManager
        {
            get { return _perceptionManager; }
        }

        public bool IsLoadingFromSave
        {
            get { return _isLoadingFromSave; }
            set { _isLoadingFromSave = value; }
        }

        public object GameSession
        {
            get { return _gameSession; }
        }

        public object CameraController
        {
            get { return _cameraController; }
        }

        public ISoundPlayer SoundPlayer
        {
            get { return _soundPlayer; }
        }

        public object JournalSystem
        {
            get { return _journalSystem; }
        }

        public IUISystem UISystem
        {
            get { return _uiSystem; }
        }
    }
}

