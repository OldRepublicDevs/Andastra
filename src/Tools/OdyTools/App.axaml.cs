using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OdyTools.Editors;
using OdyTools.Windows;

namespace OdyTools.NET
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
                // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/main_app.py:269
                // Original: if is_running_from_temp():
                if (MainInit.IsRunningFromTemp())
                {
                    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/main_app.py:270-275
                    // Original: QMessageBox.critical(...); sys.exit(...)
                    throw new InvalidOperationException(
                        "This application cannot be run from within a zip or temporary directory. " +
                        "Please extract it to a permanent location before running.");
                }

                // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/main_app.py:278
                // Original: tool_window = ToolWindow()
                desktop.MainWindow = new MainWindow();

                // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/main_app.py:281
                // Original: tool_window.show()
                desktop.MainWindow.Show();

                // Notepad++-style crash recovery: periodic backup of open editors
                EditorCrashRecoveryService.Start();

                // Check for recovery after crash (post so UI is ready)
                Dispatcher.UIThread.Post(async () =>
                {
                    if (await EditorCrashRecoveryService.ShowRecoveryDialogIfNeededAsync())
                    {
                        EditorCrashRecoveryService.Restore();
                    }
                });

                // Clean recovery data on normal app exit
                desktop.ShutdownRequested += (s, _) => EditorCrashRecoveryService.OnCleanExit();
                if (desktop.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.UpdateManager?.CheckForUpdates(silent: true);
                    if (!string.IsNullOrWhiteSpace(Program.PendingOpenTslPatchDataPath))
                    {
                        string tslpatchdataPath = Program.PendingOpenTslPatchDataPath;
                        Dispatcher.UIThread.Post(() => mainWindow.OpenTslPatchDataEditor(tslpatchdataPath));
                    }
                }
            }

            MainSettings.SetupPostInitSettings();
            MainSettings.SetupToolsetDefaultEnv();

            base.OnFrameworkInitializationCompleted();
        }
    }
}
