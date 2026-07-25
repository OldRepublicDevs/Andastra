using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using OdyTools.Editors;

namespace OdyTools.Editors.Standalone.EditorStandaloneHost
{
    /// <summary>
    /// Entry point for the unified editor standalone (OdyTools.Standalone).
    /// Usage: OdyTools.Standalone.exe [editor] [--editor editor] [--open path] [--game-path path] [--list]
    ///   No args or "2da"/"twoda" = TwoDA editor (default)
    ///   Editor keys: 2da/twoda, tlk, gff, txt, ssf, ltr, lip, jrl, erf, ifo, git, utc, utp, utd, uts, utt, utm, utw, ute, uti, are, pth, dlg, wav, sav/savegame, lyt, mdl, tpc, bwm, nss
    ///   App keys: module-designer, indoor-builder
    /// </summary>
    public static class EditorStandaloneProgram
    {
        public static string[] StartupArgs;
        public static string StartupThemeVariantName = "dark";
        public static string StartupEditorKey;
        public static string StartupOpenPath;
        public static string StartupGamePath;
        public static bool? StartupGameIsTsl;
        public static bool StartupListRequested;
        public static bool StartupHelpRequested;

        /// <summary>Set when startup fails; used by StandaloneErrorApp to show the error in a window.</summary>
        public static Exception LastStartupException;

        /// <summary>Entry point when StartupObject is this class (e.g. most editor standalones).</summary>
        [STAThread]
        public static void Main(string[] args) => Run(args);

        /// <summary>Runs the standalone editor app. Call this from each standalone's Program.Main.</summary>
        public static void Run(string[] args)
        {
            // Run from app base dir so assets and native deps resolve when launched via "dotnet run" from repo root.
            try { System.IO.Directory.SetCurrentDirectory(AppContext.BaseDirectory); } catch { }

            StartupArgs = args ?? new string[0];
            ParseStartupArguments(StartupArgs);
            if (StartupHelpRequested)
            {
                PrintUsage();
                return;
            }
            if (StartupListRequested)
            {
                PrintEditorList();
                return;
            }
#if DEBUG
            try { Console.WriteLine("Standalone editor starting (base: {0})", AppContext.BaseDirectory); } catch { }
#endif
            try
            {
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                LastStartupException = ex;
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Console.Error.WriteLine("Editor standalone failed to start: " + ex);
                try
                {
                    AppBuilder.Configure<StandaloneErrorApp>()
                        .UsePlatformDetect()
                        .StartWithClassicDesktopLifetime(Array.Empty<string>());
                }
                catch
                {
                    Console.Error.WriteLine(ex.ToString());
                    Environment.Exit(1);
                }
            }
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<EditorStandaloneApp>()
                .UsePlatformDetect()
                .LogToTrace();

        private static void ParseStartupArguments(string[] args)
        {
            StartupThemeVariantName = "dark";
            StartupEditorKey = null;
            StartupOpenPath = null;
            StartupGamePath = null;
            StartupGameIsTsl = null;
            StartupListRequested = false;
            StartupHelpRequested = false;

            if (args == null || args.Length == 0)
            {
                return;
            }

            for (int i = 0; i < args.Length; i++)
            {
                var rawArg = (args[i] ?? string.Empty).Trim();
                var arg = rawArg.ToLowerInvariant();
                if (arg == "--theme" && i + 1 < args.Length)
                {
                    var value = (args[i + 1] ?? string.Empty).Trim().ToLowerInvariant();
                    if (value == "light" || value == "dark")
                    {
                        StartupThemeVariantName = value;
                    }
                    i++;
                }
                else if ((arg == "--editor" || arg == "-e") && i + 1 < args.Length)
                {
                    StartupEditorKey = StandaloneEditorRouting.NormalizeEditorKey(args[i + 1]);
                    i++;
                }
                else if ((arg == "--open" || arg == "--file") && i + 1 < args.Length)
                {
                    StartupOpenPath = args[i + 1];
                    i++;
                }
                else if (arg == "--game-path" && i + 1 < args.Length)
                {
                    StartupGamePath = args[i + 1];
                    i++;
                }
                else if (arg == "--tsl" || arg == "--k2")
                {
                    StartupGameIsTsl = true;
                }
                else if (arg == "--k1")
                {
                    StartupGameIsTsl = false;
                }
                else if (arg == "--list" || arg == "list")
                {
                    StartupListRequested = true;
                }
                else if (arg == "--help" || arg == "-h" || arg == "/?")
                {
                    StartupHelpRequested = true;
                }
                else if (!rawArg.StartsWith("--", StringComparison.Ordinal))
                {
                    if (StartupOpenPath == null && (System.IO.File.Exists(rawArg) || System.IO.Directory.Exists(rawArg)))
                    {
                        StartupOpenPath = rawArg;
                    }
                    else if (StartupEditorKey == null)
                    {
                        StartupEditorKey = StandaloneEditorRouting.NormalizeEditorKey(rawArg);
                    }
                }
            }
        }

        private static void PrintUsage()
        {
            Console.WriteLine("Usage: OdyTools.Standalone [editor] [--editor editor] [--open path] [--game-path path] [--k1|--tsl] [--theme light|dark] [--list]");
            Console.WriteLine();
            PrintEditorList();
        }

        private static void PrintEditorList()
        {
            Console.WriteLine("Editors:");
            Console.WriteLine("  2da, twoda       2DA Table Editor        .2da");
            Console.WriteLine("  are              Area Editor             .are");
            Console.WriteLine("  bwm              Walkmesh Editor         .wok .dwk .pwk .bwm");
            Console.WriteLine("  dlg              Dialog Editor           .dlg");
            Console.WriteLine("  erf              ERF/MOD/RIM/SAV/BIF/HAK .erf .mod .rim .sav .bif .hak");
            Console.WriteLine("  fac              Faction Editor          .fac");
            Console.WriteLine("  gff              GFF Editor              .gff");
            Console.WriteLine("  git              GIT Editor              .git");
            Console.WriteLine("  gui              GUI Editor              .gui");
            Console.WriteLine("  ifo              Module Info Editor      .ifo");
            Console.WriteLine("  jrl              Journal Editor          .jrl");
            Console.WriteLine("  lip              LIP Sync Editor         .lip");
            Console.WriteLine("  ltr              LTR Editor              .ltr");
            Console.WriteLine("  lyt              Layout Editor           .lyt");
            Console.WriteLine("  mdl              Model Viewer            .mdl .mdx");
            Console.WriteLine("  nss              Script Editor           .nss .ncs");
            Console.WriteLine("  pth              Path Editor             .pth");
            Console.WriteLine("  sav, savegame    Save Game Editor");
            Console.WriteLine("  ssf              Sound Set Editor        .ssf");
            Console.WriteLine("  tlk              Talk Table Editor       .tlk");
            Console.WriteLine("  tpc              Texture Editor          .tpc .tga .dds .png .jpg .bmp");
            Console.WriteLine("  txt              Text Editor             .txt .ini .cfg .log .2da_bak .txi .vis");
            Console.WriteLine("  utc              Creature Editor         .utc .btc .bic");
            Console.WriteLine("  utd              Door Editor             .utd .btd");
            Console.WriteLine("  ute              Encounter Editor        .ute .bte");
            Console.WriteLine("  uti              Item Editor             .uti .bti");
            Console.WriteLine("  utm              Store Editor            .utm .btm");
            Console.WriteLine("  utp              Placeable Editor        .utp .btp");
            Console.WriteLine("  uts              Sound Editor            .uts");
            Console.WriteLine("  utt              Trigger Editor          .utt .btt");
            Console.WriteLine("  utw              Waypoint Editor         .utw");
            Console.WriteLine("  wav              Audio Player            .wav .mp3 .ogg .wma .wmv .xmv .flac .bmu");
            Console.WriteLine();
            Console.WriteLine("Apps:");
            Console.WriteLine("  module-designer  Module Designer");
            Console.WriteLine("  indoor-builder   Indoor Builder");
        }
    }

    /// <summary>Minimal Avalonia app used when main editor fails to start; shows error in a window so the process does not exit silently.</summary>
    internal sealed class StandaloneErrorApp : Application
    {
        public override void Initialize() { }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var ex = EditorStandaloneProgram.LastStartupException;
                desktop.MainWindow = new Window
                {
                    Title = "Editor failed to start",
                    Width = 700,
                    Height = 400,
                    Content = new ScrollViewer
                    {
                        Content = new TextBlock
                        {
                            Text = ex?.ToString() ?? "Unknown error",
                            TextWrapping = TextWrapping.Wrap,
                            Margin = new Thickness(12)
                        }
                    }
                };
                desktop.MainWindow.Show();
            }
            base.OnFrameworkInitializationCompleted();
        }
    }
}
