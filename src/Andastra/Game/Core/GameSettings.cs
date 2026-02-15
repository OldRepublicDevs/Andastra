using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Andastra.Runtime.Core;
using Andastra.Runtime.Graphics;
using BioWare.Common;
using BioWare.Tools;

namespace Andastra.Game.Core
{
    /// <summary>
    /// Result of parsing CLI arguments for game launch.
    /// </summary>
    public sealed class CliParseResult
    {
        public BioWareGame? Game { get; set; }
        public string Path { get; set; }
        public bool HasGameSpecified { get; set; }
        public bool HasPathSpecified { get; set; }
    }

    /// <summary>
    /// Game settings and configuration with command-line parsing.
    /// </summary>
    /// <remarks>
    /// Game Settings Extensions:
    /// - Directory aliases and configuration (Reva; K1 = k1_win_gog_swkotor.exe, TSL = k2_win_gog_legacypc_swkotor2.exe): K1 LoadOptions @ 0x0061dbe0, TSL FUN_00633270 @ 0x00633270 (loads INI and sets up HD0, CD0, OVERRIDE, etc.).
    /// - Located via string references: "swkotor2.ini" @ 0x007b5740, ".\swkotor2.ini" @ 0x007b5644, "config.txt" @ 0x007b5750
    /// - "DiffSettings" @ 0x007c2cdc (display settings, referenced by 0x005d7ce0 @ 0x005d7ce0)
    /// - INI loading: 0x00630a90 @ 0x00630a90 (string constructor for INI values), 0x00631ea0 @ 0x00631ea0 (calls 0x00633270)
    /// - INI reading: 0x00631fe0 @ 0x00631fe0 (reads INI values via 0x00635fb0), 0x00631ff0 @ 0x00631ff0 (writes INI values)
    /// - Directory aliases: 0x00633270 sets up HD0, CD0, OVERRIDE, MODULES, SAVES, MUSIC, MOVIES, etc. (maps to .\ paths)
    /// - Command-line: DAT_008ba024 = GetCommandLineA() stores command-line arguments (set in entry @ 0x0076e2dd)
    /// - Original implementation: Command-line arguments parsed to override INI file settings
    /// - Settings include: BioWareGame path window size, fullscreen mode, graphics options, audio options
    /// </remarks>
    public static class GameSettingsExtensions
    {
        /// <summary>
        /// Parses CLI arguments for game and path. Supports --game (k1, k2, tsl, nwn, nwn2, dao, da2) and --path.
        /// When path is not provided, autodetects using same logic as the GUI (PathTools for K1/K2, GamePathDetector for others).
        /// </summary>
        public static CliParseResult ParseCliArgs(string[] args)
        {
            var result = new CliParseResult();

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];
                string argLower = arg.ToLowerInvariant();

                switch (argLower)
                {
                    case "--game":
                    case "-g":
                        if (i + 1 < args.Length)
                        {
                            string gameArg = args[++i].ToLowerInvariant();
                            result.Game = ParseGameArgument(gameArg);
                            result.HasGameSpecified = result.Game.HasValue;
                        }
                        break;

                    case "--path":
                    case "-p":
                        if (i + 1 < args.Length)
                        {
                            result.Path = args[++i];
                            result.HasPathSpecified = !string.IsNullOrWhiteSpace(result.Path);
                        }
                        break;
                }
            }

            // Backward compat: --k1, --k2, --tsl also set game
            if (!result.HasGameSpecified)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    string argLower = args[i].ToLowerInvariant();
                    if (argLower == "--k1" || argLower == "-k1")
                    {
                        result.Game = BioWareGame.K1;
                        result.HasGameSpecified = true;
                        break;
                    }
                    if (argLower == "--k2" || argLower == "-k2" || argLower == "--tsl")
                    {
                        result.Game = BioWareGame.K2;
                        result.HasGameSpecified = true;
                        break;
                    }
                }
            }

            // Autodetect path when game is specified but path is not
            if (result.HasGameSpecified && result.Game.HasValue && !result.HasPathSpecified)
            {
                string detectedPath = DetectGamePath(result.Game.Value);
                if (!string.IsNullOrEmpty(detectedPath))
                {
                    result.Path = detectedPath;
                }
            }

            return result;
        }

        /// <summary>
        /// Parses a game identifier string (k1, k2, tsl, nwn, nwn2, dao, da, da2) to BioWareGame.
        /// </summary>
        public static BioWareGame? ParseGameArgument(string gameArg)
        {
            if (string.IsNullOrWhiteSpace(gameArg)) return null;
            switch (gameArg.ToLowerInvariant())
            {
                case "k1": return BioWareGame.K1;
                case "k2":
                case "tsl": return BioWareGame.K2;
                case "nwn": return BioWareGame.NWN;
                case "nwn2": return BioWareGame.NWN2;
                case "dao":
                case "da": return BioWareGame.DA;
                case "da2": return BioWareGame.DA2;
                default: return null;
            }
        }

        /// <summary>
        /// Detects game installation path using same logic as the GUI (PathTools for K1/K2, GamePathDetector for others).
        /// </summary>
        public static string DetectGamePath(BioWareGame game)
        {
            if (game == BioWareGame.K1 || game == BioWareGame.K2)
            {
                var foundPaths = PathTools.FindKotorPathsFromDefault();
                if (foundPaths.TryGetValue(game, out List<CaseAwarePath> paths) && paths != null && paths.Count > 0)
                {
                    string resolved = paths[0].GetResolvedPath();
                    if (Directory.Exists(resolved)) return resolved;
                }
                var kotorGame = game == BioWareGame.K1 ? KotorGame.K1 : KotorGame.K2;
                List<string> detectorPaths = GamePathDetector.FindKotorPathsFromDefault(kotorGame);
                return detectorPaths != null && detectorPaths.Count > 0 ? detectorPaths[0] : null;
            }
            else
            {
                List<string> paths = GamePathDetector.FindGamePathsFromDefault(game);
                return paths != null && paths.Count > 0 ? paths[0] : null;
            }
        }

        /// <summary>
        /// Parse command line arguments into settings.
        /// </summary>
        public static GameSettings FromCommandLine(string[] args)
        {
            var settings = new GameSettings();
            var cliResult = ParseCliArgs(args);

            if (cliResult.Game.HasValue)
            {
                if (cliResult.Game.Value == BioWareGame.K1)
                    settings.Game = KotorGame.K1;
                else if (cliResult.Game.Value == BioWareGame.K2)
                    settings.Game = KotorGame.K2;
            }

            if (!string.IsNullOrEmpty(cliResult.Path))
            {
                settings.GamePath = cliResult.Path;
            }

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i].ToLowerInvariant();

                switch (arg)
                {
                    case "--k1":
                    case "-k1":
                        settings.Game = KotorGame.K1;
                        break;

                    case "--k2":
                    case "-k2":
                    case "--tsl":
                        settings.Game = KotorGame.K2;
                        break;

                    case "--path":
                    case "-p":
                        if (i + 1 < args.Length)
                        {
                            settings.GamePath = args[++i];
                        }
                        break;

                    case "--module":
                    case "-m":
                        if (i + 1 < args.Length)
                        {
                            settings.StartModule = args[++i];
                        }
                        break;

                    case "--load":
                    case "-l":
                        if (i + 1 < args.Length)
                        {
                            settings.LoadSave = args[++i];
                        }
                        break;

                    case "--width":
                    case "-w":
                        if (i + 1 < args.Length)
                        {
                            int.TryParse(args[++i], out int width);
                            if (width > 0) settings.Width = width;
                        }
                        break;

                    case "--height":
                    case "-h":
                        if (i + 1 < args.Length)
                        {
                            int.TryParse(args[++i], out int height);
                            if (height > 0) settings.Height = height;
                        }
                        break;

                    case "--fullscreen":
                    case "-f":
                        settings.Fullscreen = true;
                        break;

                    case "--debug":
                    case "-d":
                        settings.DebugRender = true;
                        break;

                    case "--no-intro":
                        settings.SkipIntro = true;
                        break;

                    case "--help":
                    case "-?":
                        PrintHelp();
                        Environment.Exit(0);
                        break;
                }
            }

            return settings;
        }

        /// <summary>
        /// Loads settings from andastra.ini in the game path. Reva: swkotor.ini/swkotor2.ini config loading.
        /// Call after GamePath is set. OptionsScreen saves here; this applies on next launch.
        /// </summary>
        public static void LoadFromConfigFile(GameSettings settings)
        {
            if (settings == null || string.IsNullOrEmpty(settings.GamePath)) return;
            string path = Path.Combine(settings.GamePath, "andastra.ini");
            if (!File.Exists(path)) return;
            try
            {
                foreach (string line in File.ReadAllLines(path))
                {
                    string trimmed = line.Trim();
                    if (trimmed.StartsWith(";") || !trimmed.Contains("=")) continue;
                    int eq = trimmed.IndexOf('=');
                    string key = trimmed.Substring(0, eq).Trim().ToLowerInvariant();
                    string val = trimmed.Substring(eq + 1).Trim();
                    switch (key)
                    {
                        case "width":
                            if (int.TryParse(val, out int w) && w >= 320)
                            {
                                settings.Width = w;
                                if (settings.Graphics != null) settings.Graphics.ResolutionWidth = w;
                            }
                            break;
                        case "height":
                            if (int.TryParse(val, out int h) && h >= 240)
                            {
                                settings.Height = h;
                                if (settings.Graphics != null) settings.Graphics.ResolutionHeight = h;
                            }
                            break;
                        case "fullscreen":
                            settings.Fullscreen = val == "1" || val.Equals("true", StringComparison.OrdinalIgnoreCase);
                            if (settings.Graphics != null) settings.Graphics.Fullscreen = settings.Fullscreen;
                            break;
                        case "vsync":
                            bool vsync = val == "1" || val.Equals("true", StringComparison.OrdinalIgnoreCase);
                            if (settings.Graphics != null) settings.Graphics.VSync = vsync;
                            break;
                        case "musicvolume":
                            if (float.TryParse(val, out float mv) && settings.Audio != null)
                                settings.Audio.MusicVolume = Math.Max(0, Math.Min(1, mv));
                            break;
                        case "soundvolume":
                            if (float.TryParse(val, out float sv) && settings.Audio != null)
                                settings.Audio.SfxVolume = Math.Max(0, Math.Min(1, sv));
                            break;
                        case "disablesound":
                            bool disable = val == "1" || val.Equals("true", StringComparison.OrdinalIgnoreCase);
                            if (settings.Audio != null) settings.Audio.MusicEnabled = !disable;
                            break;
                    }
                }
            }
            catch { /* ignore parse errors */ }
        }

        /// <summary>
        /// Applies borderless fullscreen dimensions to settings (primary screen size).
        /// Call when launching from launcher to ensure game window fills the screen without borders.
        /// </summary>
        public static void ApplyBorderlessFullscreen(GameSettings settings)
        {
            if (settings == null) return;
            var (width, height) = GetPrimaryScreenSize();
            if (width > 0 && height > 0)
            {
                settings.Width = width;
                settings.Height = height;
                if (settings.Graphics != null)
                {
                    settings.Graphics.ResolutionWidth = width;
                    settings.Graphics.ResolutionHeight = height;
                    settings.Graphics.DisplayMode = DisplayModePreference.BorderlessFullscreen;
                }
            }
        }

        /// <summary>
        /// Gets the primary screen dimensions. Uses GetSystemMetrics on Windows, fallback 1920x1080 otherwise.
        /// </summary>
        private static (int width, int height) GetPrimaryScreenSize()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                try
                {
                    int w = GetSystemMetrics(0); // SM_CXSCREEN
                    int h = GetSystemMetrics(1); // SM_CYSCREEN
                    if (w > 0 && h > 0) return (w, h);
                }
                catch { }
            }
            return (1920, 1080);
        }

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int nIndex);

        public static void PrintHelp()
        {
            Console.WriteLine("Andastra - BioWare Game Launcher");
            Console.WriteLine();
            Console.WriteLine("Usage: Andastra [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --game, -g <game>   Game to launch: k1, k2, tsl, nwn, nwn2, dao, da, da2");
            Console.WriteLine("  --path, -p <path>   Path to game installation (autodetect if omitted)");
            Console.WriteLine("  --no-launcher, -n   Skip GUI launcher (use with --game for CLI launch)");
            Console.WriteLine("  --k1, -k1           Run KOTOR 1 (default for Odyssey)");
            Console.WriteLine("  --k2, -k2, --tsl    Run KOTOR 2 (TSL)");
            Console.WriteLine("  --module, -m <name> Start at specific module (KOTOR)");
            Console.WriteLine("  --load, -l <save>   Load save game");
            Console.WriteLine("  --width, -w <n>     Window width (default: 1280)");
            Console.WriteLine("  --height, -h <n>    Window height (default: 720)");
            Console.WriteLine("  --fullscreen, -f    Run in fullscreen");
            Console.WriteLine("  --debug, -d         Enable debug rendering");
            Console.WriteLine("  --no-intro          Skip intro videos");
            Console.WriteLine("  --help, -?          Show this help");
        }
    }
}
