using System;
using Eto.Forms;
using Andastra.Parsing.Common;
using Andastra.Runtime.Game.Core;
using Andastra.Runtime.Graphics;

namespace Andastra.Runtime.Game
{
    /// <summary>
    /// Entry point for the Odyssey Engine game launcher.
    /// </summary>
    /// <remarks>
    /// Program Entry Point:
    /// - Based on swkotor2.exe: entry @ 0x0076e2dd (PE entry point)
    /// - Main initialization: FUN_00404250 @ 0x00404250 (WinMain equivalent, initializes game)
    /// - Located via string references: "swkotor2" @ 0x007b575c (executable name), "KotOR2" @ 0x0080c210 (game title)
    /// - Original implementation: Entry point calls GetVersionExA, initializes heap, calls FUN_00404250
    /// - FUN_00404250 @ 0x00404250: Creates mutex "swkotor2" via CreateMutexA, initializes COM via CoInitialize, loads config.txt (FUN_00460ff0), loads swKotor2.ini (FUN_00630a90), creates engine objects, runs game loop
    /// - Mutex creation: CreateMutexA with name "swkotor2" prevents multiple instances, WaitForSingleObject checks if already running
    /// - Config loading: FUN_00460ff0 @ 0x00460ff0 loads and executes text files (config.txt, startup.txt)
    /// - INI loading: FUN_00630a90 @ 0x00630a90 loads INI file values, FUN_00631ea0 @ 0x00631ea0 parses INI sections, FUN_00630c20 cleans up INI structures
    /// - Sound initialization: Checks "Disable Sound" setting from INI, sets DAT_008b73c0 flag
    /// - Window creation: FUN_00403f70 creates main window, FUN_004015b0/FUN_00401610 initialize graphics
    /// - Game loop: PeekMessageA/GetMessageA for Windows message processing, TranslateMessage/DispatchMessageA for input
    /// - Game initialization: Detects KOTOR installation path, loads configuration, creates game instance
    /// - Command line: DAT_008ba024 = GetCommandLineA() stores command-line arguments
    /// - Exit: Returns 0 on success, 0xffffffff if mutex already exists, 1 on error
    /// </remarks>
    public static class Program
    {
    [STAThread]
    public static int Main(string[] args)
    {
        try
        {
            // Check for --no-launcher flag to skip launcher UI
            bool skipLauncher = false;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--no-launcher" || args[i] == "-n")
                {
                    skipLauncher = true;
                    break;
                }
            }

            GameSettings settings = null;
            string gamePath = null;
            Game selectedGame = Game.K1;

            if (!skipLauncher)
            {
                // Initialize Eto.Forms application (cross-platform)
                var app = new Application(Eto.Platform.Detect);
                
                // Show launcher UI
                using (var launcher = new Andastra.Game.GUI.GameLauncher())
                {
                    if (launcher.ShowModal() != DialogResult.Ok || !launcher.StartClicked)
                    {
                        app.Dispose();
                        return 0; // User cancelled
                    }

                    selectedGame = launcher.SelectedGame;
                    gamePath = launcher.SelectedPath;
                }
                
                app.Dispose();

                // Create game settings based on selected game type
                // Handle different game engines (Odyssey, Aurora, Eclipse, Infinity)
                if (selectedGame.IsOdyssey())
                {
                    // Odyssey Engine games (KotOR 1 and 2)
                    KotorGame kotorGame = KotorGame.K1;
                    if (selectedGame == Game.K2 || selectedGame == Game.TSL || selectedGame.IsK2())
                    {
                        kotorGame = KotorGame.K2;
                    }

                    settings = new GameSettings
                    {
                        Game = kotorGame,
                        GamePath = gamePath
                    };
                }
                else if (selectedGame.IsAurora())
                {
                    // Aurora Engine games (Neverwinter Nights)
                    // TODO: IMPLEMENT - Aurora game support
                    // Based on nwmain.exe: Neverwinter Nights game initialization
                    // Based on nwn2main.exe: Neverwinter Nights 2 game initialization
                    // When implemented, create AuroraGameSettings and AuroraGame classes
                    var errorApp = new Application(Eto.Platform.Detect);
                    string gameName = selectedGame.IsNWN1() ? "Neverwinter Nights" : "Neverwinter Nights 2";
                    MessageBox.Show(
                        $"{gameName} support is not yet implemented.\n\n" +
                        "Aurora Engine games require:\n" +
                        "- AuroraGameSettings class\n" +
                        "- AuroraGame class (similar to OdysseyGame)\n" +
                        "- Aurora-specific module loading and world initialization\n\n" +
                        "This feature is planned for future implementation.",
                        "Not Yet Implemented",
                        MessageBoxType.Information);
                    errorApp.Dispose();
                    return 1;
                }
                else if (selectedGame.IsEclipse())
                {
                    // Eclipse Engine games (Dragon Age)
                    // TODO: IMPLEMENT - Eclipse game support
                    // Based on daorigins.exe: Dragon Age: Origins game initialization
                    // Based on DragonAge2.exe: Dragon Age II game initialization
                    // When implemented, create EclipseGameSettings and EclipseGame classes
                    var errorApp = new Application(Eto.Platform.Detect);
                    string gameName = selectedGame.IsDragonAgeOrigins() ? "Dragon Age: Origins" : "Dragon Age II";
                    MessageBox.Show(
                        $"{gameName} support is not yet implemented.\n\n" +
                        "Eclipse Engine games require:\n" +
                        "- EclipseGameSettings class\n" +
                        "- EclipseGame class (similar to OdysseyGame)\n" +
                        "- Eclipse-specific module loading and world initialization\n" +
                        "- UnrealScript-based scripting system\n\n" +
                        "This feature is planned for future implementation.",
                        "Not Yet Implemented",
                        MessageBoxType.Information);
                    errorApp.Dispose();
                    return 1;
                }
                else if (selectedGame.IsInfinity())
                {
                    // Infinity Engine games (Baldur's Gate, Icewind Dale, Planescape: Torment)
                    // TODO: IMPLEMENT - Infinity game support
                    // Based on bgmain.exe: Baldur's Gate game initialization
                    // Based on iwdmain.exe: Icewind Dale game initialization
                    // Based on pstmain.exe: Planescape: Torment game initialization
                    // When implemented, create InfinityGameSettings and InfinityGame classes
                    var errorApp = new Application(Eto.Platform.Detect);
                    string gameName = "Infinity Engine Game";
                    if (selectedGame.IsBaldursGate())
                    {
                        gameName = "Baldur's Gate";
                    }
                    else if (selectedGame.IsIcewindDale())
                    {
                        gameName = "Icewind Dale";
                    }
                    else if (selectedGame.IsPlanescapeTorment())
                    {
                        gameName = "Planescape: Torment";
                    }
                    MessageBox.Show(
                        $"{gameName} support is not yet implemented.\n\n" +
                        "Infinity Engine games require:\n" +
                        "- InfinityGameSettings class\n" +
                        "- InfinityGame class (similar to OdysseyGame)\n" +
                        "- Infinity-specific module loading and world initialization\n" +
                        "- Infinity Engine scripting system\n\n" +
                        "This feature is planned for future implementation.",
                        "Not Yet Implemented",
                        MessageBoxType.Information);
                    errorApp.Dispose();
                    return 1;
                }
                else
                {
                    // Unknown or unsupported game type
                    var errorApp = new Application(Eto.Platform.Detect);
                    MessageBox.Show(
                        $"Game type {selectedGame} is not recognized or supported.\n\n" +
                        "Supported game engines:\n" +
                        "- Odyssey Engine (KotOR 1, KotOR 2)\n" +
                        "- Aurora Engine (Neverwinter Nights, Neverwinter Nights 2) - Planned\n" +
                        "- Eclipse Engine (Dragon Age: Origins, Dragon Age II) - Planned\n" +
                        "- Infinity Engine (Baldur's Gate, Icewind Dale, Planescape: Torment) - Planned",
                        "Unsupported Game",
                        MessageBoxType.Warning);
                    errorApp.Dispose();
                    return 1;
                }
            }
            else
            {
                // Parse command line arguments (legacy mode)
                settings = GameSettingsExtensions.FromCommandLine(args);

                // Detect KOTOR installation if not specified
                if (string.IsNullOrEmpty(settings.GamePath))
                {
                    settings.GamePath = GamePathDetector.DetectKotorPath(settings.Game);
                    if (string.IsNullOrEmpty(settings.GamePath))
                    {
                        Console.Error.WriteLine("ERROR: Could not detect KOTOR installation.");
                        Console.Error.WriteLine("Please specify the game path with --path <path>");
                        return 1;
                    }
                }
            }

            // Determine graphics backend (default to MonoGame, can be overridden via command line)
            GraphicsBackendType backendType = GraphicsBackendType.MonoGame;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--backend" && i + 1 < args.Length)
                {
                    if (args[i + 1].Equals("stride", StringComparison.OrdinalIgnoreCase))
                    {
                        backendType = GraphicsBackendType.Stride;
                    }
                    else if (args[i + 1].Equals("monogame", StringComparison.OrdinalIgnoreCase))
                    {
                        backendType = GraphicsBackendType.MonoGame;
                    }
                    break;
                }
            }

            // Launch the game based on game type
            // Currently only Odyssey Engine (KotOR 1/2) is fully implemented
            try
            {
                // Create graphics backend
                IGraphicsBackend graphicsBackend = Core.GraphicsBackendFactory.CreateBackend(backendType);

                // Route to appropriate game implementation based on settings
                // Odyssey Engine games use OdysseyGame
                if (settings != null && (settings.Game == KotorGame.K1 || settings.Game == KotorGame.K2))
                {
                    // Create and run Odyssey Engine game
                    using (var game = new OdysseyGame(settings, graphicsBackend))
                    {
                        game.Run();
                    }
                }
                else
                {
                    // This should not happen if game selection logic above is correct
                    // But handle it gracefully just in case
                    throw new InvalidOperationException(
                        $"Game settings are invalid or game type is not supported. " +
                        $"Only Odyssey Engine games (KotOR 1/2) are currently implemented.");
                }

                return 0;
            }
            catch (Exception ex)
            {
                // Show error dialog (cross-platform)
                string errorMessage = $"Failed to start the game:\n\n{ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMessage += $"\n\nInner Exception: {ex.InnerException.Message}";
                }
                errorMessage += $"\n\nStack Trace:\n{ex.StackTrace}";

                // Use Eto.Forms for cross-platform message box
                var app = new Application(Eto.Platform.Detect);
                MessageBox.Show(
                    errorMessage,
                    "Game Launch Error",
                    MessageBoxType.Error);
                app.Dispose();

                return 1;
            }
        }
        catch (Exception ex)
        {
            // Fatal error in launcher itself
            var app = new Application(Eto.Platform.Detect);
            MessageBox.Show(
                $"Fatal error in launcher:\n\n{ex.Message}\n\n{ex.StackTrace}",
                "Launcher Error",
                MessageBoxType.Error);
            app.Dispose();
            return 1;
        }
    }
    }
}

