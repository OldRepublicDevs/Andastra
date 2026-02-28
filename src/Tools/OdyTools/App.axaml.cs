using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OdyTools.Editors;
using OdyTools.Windows;

namespace OdyTools.NET
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            MainInit.Initialize();

            MainSettings.SetupPreInitSettings();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (MainInit.IsRunningFromTemp())
                {
                    throw new InvalidOperationException(
                        "This application cannot be run from within a zip or temporary directory. " +
                        "Please extract it to a permanent location before running.");
                }

                desktop.MainWindow = new MainWindow();

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
#if !NET48
                    mainWindow.UpdateManager?.CheckForUpdates(silent: true);
#endif
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
