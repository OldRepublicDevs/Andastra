using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;

namespace OdyTools.Editors.Standalone.EditorStandaloneHost
{
    /// <summary>
    /// Entry point for the unified editor standalone (OdyTools.Standalone).
    /// Usage: OdyTools.Standalone.exe [editor]
    ///   No args or "2da" = TwoDA editor (default)
    ///   Editor keys: 2da, tlk, gff, txt, ssf, ltr, lip, jrl, erf, ifo, git, utc, utp, utd, uts, utt, utm, utw, ute, uti, are, pth, dlg, wav, sav, lyt, mdl, tpc, bwm, nss
    /// </summary>
    public static class EditorStandaloneProgram
    {
        public static string[] StartupArgs;
        public static string StartupThemeVariantName = "dark";

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

#if DEBUG
            try { Console.WriteLine("Standalone editor starting (base: {0})", AppContext.BaseDirectory); } catch { }
#endif
            StartupArgs = args ?? new string[0];
            ParseStartupArguments(StartupArgs);
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
            if (args == null || args.Length == 0)
            {
                return;
            }

            for (int i = 0; i < args.Length; i++)
            {
                var arg = (args[i] ?? string.Empty).Trim().ToLowerInvariant();
                if (arg == "--theme" && i + 1 < args.Length)
                {
                    var value = (args[i + 1] ?? string.Empty).Trim().ToLowerInvariant();
                    if (value == "light" || value == "dark")
                    {
                        StartupThemeVariantName = value;
                    }
                    i++;
                }
            }
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
