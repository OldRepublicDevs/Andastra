using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using OdyTools.Data;
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

                ConfigureCrashRecoveryStartup(
                    GlobalSettings.Instance.CrashRecoveryEnabled,
                    () => EditorCrashRecoveryService.Start(),
                    () => EditorCrashRecoveryService.ShowRecoveryDialogIfNeededAsync(),
                    () => EditorCrashRecoveryService.Restore(),
                    () => EditorCrashRecoveryService.OnCleanExit(),
                    work => Dispatcher.UIThread.Post(async () => await work()),
                    handler => desktop.ShutdownRequested += handler);
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

        private static void ConfigureCrashRecoveryStartup(
            bool enabled,
            Action startRecovery,
            Func<Task<bool>> showRecoveryDialogIfNeededAsync,
            Action restoreRecovery,
            Action onCleanExit,
            Action<Func<Task>> scheduleRecoveryPrompt,
            Action<EventHandler<ShutdownRequestedEventArgs>> registerShutdownHandler)
        {
            if (!enabled)
            {
                return;
            }

            startRecovery?.Invoke();

            scheduleRecoveryPrompt?.Invoke(async () =>
            {
                if (showRecoveryDialogIfNeededAsync != null && await showRecoveryDialogIfNeededAsync())
                {
                    restoreRecovery?.Invoke();
                }
            });

            registerShutdownHandler?.Invoke((s, _) => onCleanExit?.Invoke());
        }
    }
}
