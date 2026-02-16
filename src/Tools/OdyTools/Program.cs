using System;
using Avalonia;
using ReactiveUI.Avalonia;
using OdyTools.Shell;

namespace OdyTools.NET
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/__main__.py:43
    // Original: if __name__ == "__main__": main_init(); main()
    internal class Program
    {
        internal static string PendingOpenTslPatchDataPath { get; private set; }

        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                var shellResult = ShellCommandRouter.Handle(args);
                if (shellResult.Handled && !shellResult.ShouldLaunchUi)
                {
                    return;
                }
                if (shellResult.ShouldLaunchUi)
                {
                    PendingOpenTslPatchDataPath = shellResult.OpenTslPatchDataPath;
                }

                BuildAvaloniaApp()
                    .StartWithClassicDesktopLifetime(args);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Console.Error.WriteLine("OdyTools failed to start: " + ex);
                throw;
            }
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI()
                .RegisterReactiveUIViewsFromEntryAssembly();
    }
}
