namespace Odyssey.Game.Core
{
    /// <summary>
    /// Represents the current state of the game.
    /// </summary>
    /// <remarks>
    /// Game State Enum:
    /// - Based on swkotor2.exe game state management system
    /// - Located via string references: "GameState" @ 0x007c15d0 (game state field), "GameMode" @ 0x007c15e0 (game mode field)
    /// - "GAMEINPROGRESS" @ 0x007c15c8 (game in progress flag), "ModuleLoaded" @ 0x007bdd70 (module loaded flag)
    /// - "ModuleRunning" @ 0x007bdd58 (module running flag)
    /// - Menu states: "RIMS:MAINMENU" @ 0x007b6044 (main menu RIM), "MAINMENU" @ 0x007cc030 (main menu constant)
    /// - "mainmenu_p" @ 0x007cc000 (main menu panel), "mainmenu01" @ 0x007cc108, "mainmenu02" @ 0x007cc138 (main menu variants)
    /// - "Action Menu" @ 0x007c8480 (action menu), "CB_ACTIONMENU" @ 0x007d29d4 (action menu checkbox)
    /// - Original implementation: Game state tracks current UI/mode (main menu, loading, in-game, paused, save/load menus)
    /// - State transitions: MainMenu -> Loading -> InGame, InGame -> Paused/SaveMenu/LoadMenu
    /// - Based on swkotor2.exe: FUN_005226d0 @ 0x005226d0 manages game state transitions
    /// </remarks>
    public enum GameState
    {
        /// <summary>
        /// Main menu - player selects install path and starting module.
        /// </summary>
        MainMenu,

        /// <summary>
        /// Loading screen - game is loading module and initializing world.
        /// </summary>
        Loading,

        /// <summary>
        /// In game - player is actively playing.
        /// </summary>
        InGame,

        /// <summary>
        /// Paused - game is paused (in-game menu).
        /// </summary>
        Paused,

        /// <summary>
        /// Save menu - player is selecting a save slot.
        /// </summary>
        SaveMenu,

        /// <summary>
        /// Load menu - player is selecting a save to load.
        /// </summary>
        LoadMenu
    }
}
