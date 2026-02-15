using System;
using Avalonia;
using ReactiveUI.Avalonia;

namespace OdyTools.NET
{
    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/__main__.py:43
    // Original: if __name__ == "__main__": main_init(); main()
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
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
