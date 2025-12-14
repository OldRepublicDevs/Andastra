using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using HolocronToolset.NET.Windows;

namespace HolocronToolset.NET
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/main_app.py:208
    // Original: def main(): app = QApplication(sys.argv)
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/__main__.py:44
            // Original: main_init()
            MainInit.Initialize();

            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/main_app.py:217
            // Original: setup_pre_init_settings()
            MainSettings.SetupPreInitSettings();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/main_app.py:266
            // Original: setup_post_init_settings()
            MainSettings.SetupPostInitSettings();

            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/main_app.py:267
            // Original: setup_toolset_default_env()
            MainSettings.SetupToolsetDefaultEnv();

            base.OnFrameworkInitializationCompleted();
        }
    }
}
