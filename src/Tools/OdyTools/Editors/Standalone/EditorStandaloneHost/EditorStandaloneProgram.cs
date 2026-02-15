using System;
using Avalonia;
using Avalonia.ReactiveUI;

namespace OdyTools.Editors.Standalone.EditorStandaloneHost
{
    /// <summary>
    /// Entry point for the unified editor standalone (OdyTools.Standalone).
    /// Usage: OdyTools.Standalone.exe [editor]
    ///   No args or "2da" = TwoDA editor (default)
    ///   Editor keys: 2da, tlk, gff, txt, ssf, ltr, lip, jrl, erf, ifo, git, utc, utp, utd, uts, utt, utm, utw, ute, uti, are, pth, dlg, wav, sav, lyt, mdl, tpc, bwm, nss
    /// </summary>
    internal static class EditorStandaloneProgram
    {
        internal static string[] StartupArgs;

        [STAThread]
        public static void Main(string[] args)
        {
            StartupArgs = args ?? new string[0];
            try
            {
                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Console.Error.WriteLine("Editor standalone failed to start: " + ex);
                throw;
            }
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<EditorStandaloneApp>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
    }
}
