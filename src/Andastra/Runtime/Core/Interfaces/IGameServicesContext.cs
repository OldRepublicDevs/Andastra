using Andastra.Runtime.Core.Entities;
using Andastra.Runtime.Core.Interfaces.Components;
using Audio = Andastra.Runtime.Core.Audio;

namespace Andastra.Runtime.Core.Interfaces
{
    /// <summary>
    /// Interface for game services context accessible from script execution context.
    /// This interface allows script execution to access game services without depending on engine-specific implementations.
    /// </summary>
    /// <remarks>
    /// Game Services Context Interface:
    /// - Common interface for game services accessible from script execution context across all BioWare engines
    /// - Script execution context provides access to game systems for engine API functions
    /// - Services accessible from scripts: DialogueManager, PlayerEntity, CombatManager, PartyManager, ModuleLoader
    /// - FactionManager: Manages faction relationships and hostility (repute.2da lookup)
    /// - PerceptionManager: Handles creature perception (sight/hearing, OnPerception events)
    /// - CameraController: Controls camera positioning and movement (dialogue cameras, cutscenes)
    /// - SoundPlayer: Plays sound effects and ambient audio (positional audio, volume control)
    /// - GameSession: Manages game state (current module, save/load, player progression)
    /// - IsLoadingFromSave: Flag indicating if game is loading from save file (prevents script execution during load)
    /// - UISystem: Manages UI screens and overlays
    /// - JournalSystem: Quest and journal management
    ///
    /// Base implementation: BaseGameServicesContext (Runtime.Games.Common) provides common functionality
    /// Engine-specific implementations:
    /// - Odyssey: OdysseyGameServicesContext (swkotor.exe, swkotor2.exe)
    /// - Aurora: AuroraGameServicesContext (nwmain.exe)
    /// - Eclipse: EclipseGameServicesContext (daorigins.exe, DragonAge2.exe)
    /// </remarks>
    public interface IGameServicesContext
    {
        /// <summary>
        /// Gets the dialogue manager (KOTOR-specific, accessed as object to avoid dependency).
        /// </summary>
        object DialogueManager { get; }

        /// <summary>
        /// Gets the player entity.
        /// </summary>
        IEntity PlayerEntity { get; }

        /// <summary>
        /// Gets the combat manager (KOTOR-specific, accessed as object to avoid dependency).
        /// </summary>
        object CombatManager { get; }

        /// <summary>
        /// Gets the party manager (KOTOR-specific, accessed as object to avoid dependency).
        /// </summary>
        object PartyManager { get; }

        /// <summary>
        /// Gets the module loader (KOTOR-specific, accessed as object to avoid dependency).
        /// </summary>
        object ModuleLoader { get; }

        /// <summary>
        /// Gets the faction manager (KOTOR-specific, accessed as object to avoid dependency).
        /// </summary>
        object FactionManager { get; }

        /// <summary>
        /// Gets the perception manager (KOTOR-specific, accessed as object to avoid dependency).
        /// </summary>
        object PerceptionManager { get; }

        /// <summary>
        /// Gets or sets whether the game is loading from a save.
        /// </summary>
        bool IsLoadingFromSave { get; set; }

        /// <summary>
        /// Gets the game session (KOTOR-specific, accessed as object to avoid dependency).
        /// </summary>
        object GameSession { get; }

        /// <summary>
        /// Gets the camera controller (KOTOR-specific, accessed as object to avoid dependency).
        /// </summary>
        object CameraController { get; }

        /// <summary>
        /// Gets the sound player.
        /// </summary>
        Audio.ISoundPlayer SoundPlayer { get; }

        /// <summary>
        /// Gets the journal system (KOTOR-specific, accessed as object to avoid dependency).
        /// </summary>
        object JournalSystem { get; }

        /// <summary>
        /// Gets the UI system for managing UI screens and overlays.
        /// </summary>
        IUISystem UISystem { get; }
    }
}

