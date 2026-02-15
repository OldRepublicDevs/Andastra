using System;
using System.Threading;
using BioWare.Common;
using Andastra.Runtime.Core;
using Andastra.Game.Core;
using Andastra.Game.GUI;
using Andastra.Runtime.Graphics;
using Andastra.Runtime.Graphics.Common.Enums;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;

namespace Andastra.Game
{
    /// <summary>
    /// Entry point for the Odyssey Engine game launcher.
    /// </summary>
    /// <remarks>
    /// Program Entry Point (addresses from Reva/Ghidra; K1 = k1_win_gog_swkotor.exe, TSL = k2_win_gog_legacypc_swkotor2.exe):
    /// - entry (PE entry point): K1 @ 0x006fb38d, TSL @ 0x0076e2dd
    /// - WinMain (main initialization): K1 @ 0x004041f0, TSL @ 0x00404250
    /// - Located via string references: "swkotor2" @ 0x007b575c (TSL executable name), "KotOR2" @ 0x0080c210 (BioWareGame title)
    /// - Original implementation: entry calls GetVersionExA, initializes heap, then calls WinMain
    /// - WinMain (TSL 0x00404250): Creates mutex "swkotor2" via CreateMutexA, initializes COM via CoInitialize, loads config.txt (0x00460ff0), loads swKotor2.ini (0x00630a90), creates engine objects, runs game loop
    /// - Mutex creation: CreateMutexA with name "swkotor2" prevents multiple instances, WaitForSingleObject checks if already running
    /// - Config loading: 0x00460ff0 @ 0x00460ff0 loads and executes text files (config.txt, startup.txt)
    /// - INI loading: 0x00630a90 @ 0x00630a90 loads INI file values, 0x00631ea0 @ 0x00631ea0 parses INI sections, 0x00630c20 cleans up INI structures
    /// - Sound initialization: Checks "Disable Sound" setting from INI, sets DAT_008b73c0 flag
    /// - Window creation: 0x00403f70 creates main window, 0x004015b0/0x00401610 initialize graphics
    /// - Game loop: PeekMessageA/GetMessageA for Windows message processing, TranslateMessage/DispatchMessageA for input
    /// - Game initialization: Detects KOTOR installation path, loads configuration, creates game instance
    /// - Command line: DAT_008ba024 = GetCommandLineA() stores command-line arguments
    /// - Exit: Returns 0 on success, 0xffffffff if mutex already exists, 1 on error
    /// </remarks>
    public static class Program
    {
        public static GUI.GameLauncher _staticLauncher;

        [STAThread]
        public static int Main(string[] args)
        {
            try
            {
                // Handle --help immediately (before launching GUI)
                for (int i = 0; i < args.Length; i++)
                {
                    string a = args[i];
                    if (a == "--help" || a == "-?")
                    {
                        GameSettingsExtensions.PrintHelp();
                        return 0;
                    }
                }

                // Parse CLI args for --game and --path (supports autodetect)
                CliParseResult cliResult = GameSettingsExtensions.ParseCliArgs(args);

                // When --game or --no-launcher is specified, skip the dialog and use CLI mode
                bool skipLauncher = false;
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "--no-launcher" || args[i] == "-n")
                    {
                        skipLauncher = true;
                        break;
                    }
                }
                // --game specified: skip launcher and use autodetect or --path (will error if path not found)
                if (!skipLauncher && cliResult.HasGameSpecified && cliResult.Game.HasValue)
                {
                    skipLauncher = true;
                }

                GameSettings settings = null;
                string gamePath = null;
                BioWareGame selectedGame = BioWareGame.K1;

                const DisplayModePreference currentMode = DisplayModePreference.BorderlessFullscreen;
                bool hasValidSelection = false;
                while (true)
                {
                    if (!hasValidSelection)
                    {
                        if (!skipLauncher)
                        {
                            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, Avalonia.Controls.ShutdownMode.OnMainWindowClose);

                            if (_staticLauncher == null || !_staticLauncher.StartClicked)
                            {
                                return 0; // User cancelled
                            }

                            selectedGame = _staticLauncher.SelectedGame;
                            gamePath = _staticLauncher.SelectedPath;

                            if (selectedGame.IsOdyssey())
                            {
                                KotorGame kotorGame = selectedGame.IsK2() ? KotorGame.K2 : KotorGame.K1;
                                settings = new GameSettings
                                {
                                    Game = kotorGame,
                                    GamePath = gamePath
                                };
                                GameSettingsExtensions.LoadFromConfigFile(settings);
                            }
                            else
                            {
                                settings = null;
                            }
                        }
                        else
                        {
                            if (!cliResult.HasGameSpecified || !cliResult.Game.HasValue)
                            {
                                settings = GameSettingsExtensions.FromCommandLine(args);
                                selectedGame = settings.Game == KotorGame.K2 ? BioWareGame.K2 : BioWareGame.K1;
                                gamePath = settings.GamePath ?? GameSettingsExtensions.DetectGamePath(selectedGame);
                            }
                            else
                            {
                                selectedGame = cliResult.Game.Value;
                                gamePath = cliResult.Path ?? GameSettingsExtensions.DetectGamePath(selectedGame);
                            }

                            if (string.IsNullOrEmpty(gamePath))
                            {
                                Console.Error.WriteLine($"ERROR: Could not detect {selectedGame} installation.");
                                Console.Error.WriteLine("Please specify the game path with --path <path>");
                                return 1;
                            }

                            if (!System.IO.Directory.Exists(gamePath))
                            {
                                Console.Error.WriteLine($"ERROR: Path does not exist: {gamePath}");
                                return 1;
                            }

                            if (selectedGame.IsOdyssey())
                            {
                                var kotorGame = selectedGame == BioWareGame.K2 ? KotorGame.K2 : KotorGame.K1;
                                if (!GamePathDetector.IsValidInstallation(gamePath, kotorGame))
                                {
                                    Console.Error.WriteLine($"ERROR: Invalid KOTOR installation at: {gamePath}");
                                    return 1;
                                }
                                settings = new GameSettings { Game = kotorGame, GamePath = gamePath };
                                GameSettingsExtensions.LoadFromConfigFile(settings);
                            }
                            else
                            {
                                if (!GamePathDetector.IsValidGameInstallation(gamePath, selectedGame))
                                {
                                    Console.Error.WriteLine($"ERROR: Invalid {selectedGame} installation at: {gamePath}");
                                    return 1;
                                }
                                settings = null;
                            }
                        }

                        hasValidSelection = true;
                    }

                    if (settings != null)
                    {
                        DisplayModeContext.CurrentMode = currentMode;
                        ApplyDisplayMode(settings, currentMode);
                        if (!skipLauncher && _staticLauncher != null)
                        {
                            ApplyLauncherGraphicsSettings(settings, _staticLauncher.GraphicsSettings);
                        }
                    }

                    GraphicsBackendType backendType = GraphicsBackendType.MonoGame;
                    if (!skipLauncher && _staticLauncher != null)
                    {
                        backendType = _staticLauncher.SelectedGraphicsBackend;
                    }
                    else
                    {
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
                    }

                    try
                    {
                        KotorGame? kotorGameType = null;
                        if (backendType == GraphicsBackendType.OdysseyEngine)
                        {
                            if (settings != null)
                            {
                                kotorGameType = settings.Game;
                            }
                            else if (selectedGame.IsOdyssey())
                            {
                                kotorGameType = selectedGame.IsK2() ? KotorGame.K2 : KotorGame.K1;
                            }

                            if (!kotorGameType.HasValue)
                            {
                                throw new InvalidOperationException("Game type (K1 or K2) is required when using OdysseyEngine backend");
                            }
                        }

                        IGraphicsBackend graphicsBackend = Core.GraphicsBackendFactory.CreateBackend(backendType, kotorGameType);

                        if ((selectedGame.IsOdyssey() || settings != null) && settings != null)
                        {
                            SynchronizationContext.SetSynchronizationContext(null);

                            using (var game = new OdysseyGame(settings, graphicsBackend))
                            {
                                game.Run();
                            }
                        }
                        else
                        {
                            string gamePathForLauncher = settings != null ? settings.GamePath : gamePath;

                            if (string.IsNullOrEmpty(gamePathForLauncher))
                            {
                                throw new InvalidOperationException($"Game path is required for {selectedGame}");
                            }

                            using (var launcher = new UnifiedGameLauncher(selectedGame, gamePathForLauncher, graphicsBackend, settings))
                            {
                                launcher.Initialize();
                                launcher.Run();
                            }
                        }

                        if (skipLauncher)
                        {
                            return 0;
                        }

                        hasValidSelection = false;
                        continue;
                    }
                    catch (Exception ex)
                    {
                        string errorMessage = $"Failed to start the game:\n\n{ex.Message}";
                        if (ex.InnerException != null)
                        {
                            errorMessage += $"\n\nInner Exception: {ex.InnerException.Message}";
                        }
                        errorMessage += $"\n\nStack Trace:\n{ex.StackTrace}";
                        ShowErrorMessage(errorMessage);
                        if (skipLauncher)
                        {
                            return 1;
                        }
                        hasValidSelection = false;
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                // Fatal error in launcher itself
                ShowErrorMessage($"Fatal error in launcher:\n\n{ex.Message}\n\n{ex.StackTrace}");
                return 1;
            }
        }

        /// <summary>
        /// Builds the Avalonia application instance.
        /// </summary>
        /// <returns>The configured Avalonia application builder.</returns>
        private static AppBuilder BuildAvaloniaApp()
        {
            return AppBuilder.Configure<AvaloniaApp>()
                .UsePlatformDetect()
                .LogToTrace();
        }

        private static void ApplyDisplayMode(GameSettings settings, DisplayModePreference mode)
        {
            if (settings == null)
            {
                return;
            }

            if (settings.Graphics == null)
            {
                settings.Graphics = new GameSettings.GraphicsSettings();
            }

            settings.Graphics.DisplayMode = mode;

            switch (mode)
            {
                case DisplayModePreference.BorderlessFullscreen:
                    settings.Fullscreen = true;
                    GameSettingsExtensions.ApplyBorderlessFullscreen(settings);
                    settings.Graphics.Fullscreen = true;
                    settings.Graphics.ResolutionWidth = settings.Width;
                    settings.Graphics.ResolutionHeight = settings.Height;
                    break;

                case DisplayModePreference.Windowed:
                    settings.Fullscreen = false;
                    settings.Width = 800;
                    settings.Height = 600;
                    settings.Graphics.Fullscreen = false;
                    settings.Graphics.ResolutionWidth = 800;
                    settings.Graphics.ResolutionHeight = 600;
                    break;

                case DisplayModePreference.ExclusiveFullscreen:
                    settings.Fullscreen = true;
                    GameSettingsExtensions.ApplyBorderlessFullscreen(settings);
                    settings.Graphics.Fullscreen = true;
                    settings.Graphics.ResolutionWidth = settings.Width;
                    settings.Graphics.ResolutionHeight = settings.Height;
                    break;
            }
        }

        private static void ApplyLauncherGraphicsSettings(GameSettings settings, GraphicsSettingsData graphicsSettings)
        {
            if (settings == null || graphicsSettings == null)
            {
                return;
            }

            if (settings.Graphics == null)
            {
                settings.Graphics = new GameSettings.GraphicsSettings();
            }

            if (graphicsSettings.WindowWidth.HasValue)
            {
                settings.Width = graphicsSettings.WindowWidth.Value;
                settings.Graphics.ResolutionWidth = graphicsSettings.WindowWidth.Value;
            }

            if (graphicsSettings.WindowHeight.HasValue)
            {
                settings.Height = graphicsSettings.WindowHeight.Value;
                settings.Graphics.ResolutionHeight = graphicsSettings.WindowHeight.Value;
            }

            if (graphicsSettings.WindowFullscreen.HasValue)
            {
                settings.Fullscreen = graphicsSettings.WindowFullscreen.Value;
                settings.Graphics.Fullscreen = graphicsSettings.WindowFullscreen.Value;
            }

            if (graphicsSettings.WindowVSync.HasValue)
            {
                settings.Graphics.VSync = graphicsSettings.WindowVSync.Value;
            }
        }

        /// <summary>
        /// Shows an error message to the user. Uses Avalonia when the app has a window (cross-platform);
        /// otherwise falls back to console only. No platform-specific APIs.
        /// </summary>
        /// <param name="message">The error message to display.</param>
        private static void ShowErrorMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            try
            {
                Console.Error.WriteLine(message);
            }
            catch
            {
                // Console not available
            }

            // Show Avalonia error dialog when we have a main window (same UI stack on all platforms)
            Window owner = null;
            try
            {
                var lifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
                owner = lifetime?.MainWindow ?? _staticLauncher;
            }
            catch
            {
                // Avalonia not initialized
            }

            if (owner == null)
                return;

            try
            {
                if (Dispatcher.UIThread.CheckAccess())
                {
                    Dispatcher.UIThread.Post(() => ShowErrorDialogAsync(owner, message));
                }
                else
                {
                    Dispatcher.UIThread.InvokeAsync(() => ShowErrorDialogAsync(owner, message)).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                try { Console.Error.WriteLine("Error dialog failed: " + ex.Message); }
                catch { }
            }
        }

        /// <summary>
        /// Shows a modal error window using Avalonia (cross-platform). Must be called from the UI thread.
        /// </summary>
        private static async void ShowErrorDialogAsync(Window owner, string message)
        {
            var msgWindow = new Window
            {
                Title = "Error",
                Width = 450,
                MinWidth = 300,
                Height = 200,
                MinHeight = 120,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                CanResize = true
            };
            var panel = new StackPanel
            {
                Margin = new Avalonia.Thickness(20),
                Spacing = 10
            };
            panel.Children.Add(new TextBlock
            {
                Text = message ?? "",
                TextWrapping = TextWrapping.Wrap
            });
            var okButton = new Button
            {
                Content = "OK",
                Width = 100,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            okButton.Click += (s, ev) => msgWindow.Close();
            panel.Children.Add(okButton);
            msgWindow.Content = panel;
            await msgWindow.ShowDialog(owner);
        }
    }

    /// <summary>
    /// Avalonia application class for the game launcher.
    /// </summary>
    public class AvaloniaApp : Application
    {
        public override void Initialize()
        {
            // Initialize Avalonia theme
            Styles.Add(new Avalonia.Themes.Fluent.FluentTheme());
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                Program._staticLauncher = new GUI.GameLauncher();
                desktop.MainWindow = Program._staticLauncher;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
