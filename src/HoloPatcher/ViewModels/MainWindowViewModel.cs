using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using TSLPatcher.Core.Common;
using TSLPatcher.Core.Config;
using TSLPatcher.Core.Installation;
using TSLPatcher.Core.Logger;
using TSLPatcher.Core.Namespaces;
using TSLPatcher.Core.Patcher;
using TSLPatcher.Core.Reader;
using TSLPatcher.Core.Uninstall;

namespace HoloPatcher.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly PatchLogger _logger = new();
    private readonly StringBuilder _logTextBuilder = new();

    [ObservableProperty]
    private string _logText = string.Empty;

    [ObservableProperty]
    private bool _isTaskRunning;

    [ObservableProperty]
    private int _progressValue;

    [ObservableProperty]
    private int _progressMaximum = 100;

    [ObservableProperty]
    private string? _selectedNamespace;

    [ObservableProperty]
    private string? _selectedGamePath;

    [ObservableProperty]
    private string _modPath = string.Empty;

    private List<PatcherNamespace> _loadedNamespaces = new();
    private CancellationTokenSource? _cancellationTokenSource;
    private ConfigReader? _currentConfigReader;
    private LogLevel _logLevel = LogLevel.Warnings;
    private bool _oneShot = false;

    public ObservableCollection<string> Namespaces { get; } = new();
    public ObservableCollection<string> GamePaths { get; } = new();

    public bool CanInstall => !IsTaskRunning &&
                              !string.IsNullOrEmpty(SelectedNamespace) &&
                              !string.IsNullOrEmpty(SelectedGamePath);

    public MainWindowViewModel()
    {
        // Subscribe to logger events
        _logger.VerboseLogged += OnLogEntry;
        _logger.NoteLogged += OnLogEntry;
        _logger.WarningLogged += OnLogEntry;
        _logger.ErrorLogged += OnLogEntry;

        // Initialize with welcome message
        AddLogEntry("Welcome to HoloPatcher!");
        AddLogEntry("Select a mod and your KOTOR directory to begin.");

        // Try to detect KOTOR installations
        DetectGamePaths();
    }

    private void OnLogEntry(object? sender, PatchLog log)
    {
        WriteLogEntry(log);
    }

    private void WriteLogEntry(PatchLog log)
    {
        // Write to log file
        try
        {
            string logFilePath = Core.GetLogFilePath(ModPath);
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath) ?? "");
            File.AppendAllText(logFilePath, log.FormattedMessage + Environment.NewLine, Encoding.UTF8);
        }
        catch (Exception ex)
        {
            // Log error but don't fail
            Debug.WriteLine($"Failed to write log file: {ex.Message}");
        }

        // Filter by log level
        LogType minLevel = GetLogTypeForLevel(_logLevel);
        if ((int)log.LogType < (int)minLevel)
        {
            return;
        }

        // Add to UI
        AddLogEntry(log.FormattedMessage);
    }

    private LogType GetLogTypeForLevel(LogLevel level)
    {
        return level switch
        {
            LogLevel.Errors => LogType.Warning,
            LogLevel.General => LogType.Warning,
            LogLevel.Full => LogType.Verbose,
            LogLevel.Warnings => LogType.Note,
            LogLevel.Nothing => LogType.Warning,
            _ => LogType.Warning
        };
    }

    private void AddLogEntry(string message)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            _logTextBuilder.AppendLine(message);
            LogText = _logTextBuilder.ToString();
        });
    }

    [RelayCommand]
    private async Task BrowseMod()
    {
        Window? window = GetMainWindow();
        if (window == null)
        {
            return;
        }

        IReadOnlyList<IStorageFolder> folders = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select Mod Directory",
            AllowMultiple = false
        });

        if (folders.Count > 0)
        {
            string path = folders[0].Path.LocalPath;
            await LoadModFromPath(path);
        }
    }

    [RelayCommand]
    private async Task BrowseGamePath()
    {
        Window? window = GetMainWindow();
        if (window == null)
        {
            return;
        }

        IReadOnlyList<IStorageFolder> folders = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select KOTOR Directory",
            AllowMultiple = false
        });

        if (folders.Count > 0)
        {
            string path = folders[0].Path.LocalPath;
            if (!GamePaths.Contains(path))
            {
                GamePaths.Add(path);
            }
            SelectedGamePath = path;
        }
    }

    [RelayCommand]
    private async Task Install()
    {
        if (!PreInstallValidate())
        {
            return;
        }

        IsTaskRunning = true;
        ProgressValue = 0;
        _cancellationTokenSource = new CancellationTokenSource();
        ClearLogText();

        try
        {
            PatcherNamespace? selectedNs = _loadedNamespaces.FirstOrDefault(ns => ns.Name == SelectedNamespace);
            if (selectedNs == null)
            {
                throw new InvalidOperationException("Selected namespace not found.");
            }

            string tslPatchDataPath = Path.Combine(ModPath, "tslpatchdata");
            string iniFilePath = Path.Combine(tslPatchDataPath, selectedNs.ChangesFilePath());

            if (!File.Exists(iniFilePath))
            {
                throw new FileNotFoundException($"Changes INI file not found: {iniFilePath}");
            }

            var installer = new ModInstaller(ModPath, SelectedGamePath!, iniFilePath, _logger)
            {
                TslPatchDataPath = tslPatchDataPath
            };

            // Check for confirmation message
            string? confirmMsg = Core.GetConfirmMessage(installer);
            if (!string.IsNullOrEmpty(confirmMsg) && !_oneShot)
            {
                MsBox.Avalonia.Base.IMsBox<ButtonResult> confirmBox = MessageBoxManager.GetMessageBoxStandard(
                    "This mod requires confirmation",
                    confirmMsg,
                    ButtonEnum.OkCancel,
                    Icon.Question);
                ButtonResult result = await confirmBox.ShowAsync();
                if (result != ButtonResult.Ok)
                {
                    IsTaskRunning = false;
                    return;
                }
            }

            AddLogEntry("Starting installation...");

            // Calculate total patches for progress
            int totalPatches = Core.CalculateTotalPatches(installer);
            ProgressMaximum = totalPatches;

            DateTime installStartTime = DateTime.UtcNow;

            await Task.Run(() =>
            {
                installer.Install(
                    _cancellationTokenSource.Token,
                    progress =>
                    {
                        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                        {
                            ProgressValue = progress;
                        });
                    });
            }, _cancellationTokenSource.Token);

            TimeSpan installTime = DateTime.UtcNow - installStartTime;
            int numErrors = _logger.Errors.Count();
            int numWarnings = _logger.Warnings.Count();
            int numPatches = installer.Config().PatchCount();

            string timeStr = Core.FormatInstallTime(installTime);
            _logger.AddNote(
                $"The installation is complete with {numErrors} errors and {numWarnings} warnings.{Environment.NewLine}" +
                $"Total install time: {timeStr}{Environment.NewLine}" +
                $"Total patches: {numPatches}");

            ProgressValue = ProgressMaximum;

            // Show completion message
            if (numErrors > 0)
            {
                MsBox.Avalonia.Base.IMsBox<ButtonResult> errorBox = MessageBoxManager.GetMessageBoxStandard(
                    "Install completed with errors!",
                    $"The install completed with {numErrors} errors and {numWarnings} warnings! The installation may not have been successful, check the logs for more details.{Environment.NewLine}{Environment.NewLine}Total install time: {timeStr}{Environment.NewLine}Total patches: {numPatches}",
                    ButtonEnum.Ok,
                    Icon.Error);
                await errorBox.ShowAsync();
            }
            else if (numWarnings > 0)
            {
                MsBox.Avalonia.Base.IMsBox<ButtonResult> warningBox = MessageBoxManager.GetMessageBoxStandard(
                    "Install completed with warnings",
                    $"The install completed with {numWarnings} warnings! Review the logs for details. The script in the 'uninstall' folder of the mod directory will revert these changes.{Environment.NewLine}{Environment.NewLine}Total install time: {timeStr}{Environment.NewLine}Total patches: {numPatches}",
                    ButtonEnum.Ok,
                    Icon.Warning);
                await warningBox.ShowAsync();
            }
            else
            {
                MsBox.Avalonia.Base.IMsBox<ButtonResult> infoBox = MessageBoxManager.GetMessageBoxStandard(
                    "Install complete!",
                    $"Check the logs for details on what has been done. Utilize the script in the 'uninstall' folder of the mod directory to revert these changes.{Environment.NewLine}{Environment.NewLine}Total install time: {timeStr}{Environment.NewLine}Total patches: {numPatches}",
                    ButtonEnum.Ok,
                    Icon.Success);
                await infoBox.ShowAsync();
            }
        }
        catch (OperationCanceledException)
        {
            AddLogEntry("[WARNING] Installation was cancelled.");
        }
        catch (Exception ex)
        {
            AddLogEntry($"[ERROR] Installation failed: {ex.Message}");
            MsBox.Avalonia.Base.IMsBox<ButtonResult> errorBox = MessageBoxManager.GetMessageBoxStandard(
                ex.GetType().Name,
                $"An unexpected error occurred during the installation and the installation was forced to terminate.{Environment.NewLine}{Environment.NewLine}{ex.Message}",
                ButtonEnum.Ok,
                Icon.Error);
            await errorBox.ShowAsync();
        }
        finally
        {
            IsTaskRunning = false;
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }

    private void ClearLogText()
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            _logTextBuilder.Clear();
            LogText = string.Empty;
        });
    }

    [RelayCommand]
    private void Exit()
    {
        Window? window = GetMainWindow();
        window?.Close();
    }

    [RelayCommand]
    private async Task ValidateIni()
    {
        if (!PreInstallValidate())
        {
            return;
        }

        IsTaskRunning = true;
        ClearLogText();

        await Task.Run(() =>
        {
            try
            {
                PatcherNamespace? selectedNs = _loadedNamespaces.FirstOrDefault(ns => ns.Name == SelectedNamespace);
                if (selectedNs == null)
                {
                    throw new InvalidOperationException("Selected namespace not found.");
                }

                Core.ValidateConfig(ModPath, _loadedNamespaces, SelectedNamespace!, _logger);
            }
            catch (Exception ex)
            {
                AddLogEntry($"[ERROR] Validation failed: {ex.Message}");
            }
            finally
            {
                IsTaskRunning = false;
                _logger.AddNote("Config reader test is complete.");
            }
        });
    }

    [RelayCommand]
    private async Task UninstallMod()
    {
        if (!ValidateModPathAndGamePath())
        {
            return;
        }

        IsTaskRunning = true;

        try
        {
            string backupRoot = ModPath;
            while (!Directory.Exists(Path.Combine(backupRoot, "tslpatchdata")) && !string.IsNullOrEmpty(Path.GetDirectoryName(backupRoot)))
            {
                backupRoot = Path.GetDirectoryName(backupRoot) ?? backupRoot;
            }

            string backupsLocation = Path.Combine(backupRoot, "backup");

            if (!Directory.Exists(backupsLocation))
            {
                AddLogEntry($"[ERROR] No backup directory found at {backupsLocation}");
                return;
            }

            var uninstaller = new ModUninstaller(new CaseAwarePath(backupsLocation), new CaseAwarePath(SelectedGamePath), _logger);

            await Task.Run(() =>
            {
                uninstaller.UninstallSelectedMod(
                    showErrorDialog: (title, msg) =>
                    {
                        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
                        {
                            MsBox.Avalonia.Base.IMsBox<ButtonResult> box = MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.Ok, Icon.Error);
                            await box.ShowAsync();
                        }).Wait();
                    },
                    showYesNoDialog: (title, msg) =>
                    {
                        bool result = false;
                        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
                        {
                            MsBox.Avalonia.Base.IMsBox<ButtonResult> box = MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.YesNo, Icon.Question);
                            ButtonResult res = await box.ShowAsync();
                            result = res == ButtonResult.Yes;
                        }).Wait();
                        return result;
                    },
                    showYesNoCancelDialog: (title, msg) =>
                    {
                        bool? result = null;
                        Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
                        {
                            MsBox.Avalonia.Base.IMsBox<ButtonResult> box = MessageBoxManager.GetMessageBoxStandard(title, msg, ButtonEnum.YesNoCancel, Icon.Question);
                            ButtonResult res = await box.ShowAsync();
                            if (res == ButtonResult.Yes)
                            {
                                result = true;
                            }
                            else if (res == ButtonResult.No)
                            {
                                result = false;
                            }
                            else
                            {
                                result = null;
                            }
                        }).Wait();
                        return result;
                    }
                );
            });

            AddLogEntry("Uninstall process finished.");
        }
        catch (Exception ex)
        {
            AddLogEntry($"[ERROR] Uninstall failed: {ex.Message}");
        }
        finally
        {
            IsTaskRunning = false;
        }
    }

    [RelayCommand]
    private async Task FixPermissions()
    {
        Window? window = GetMainWindow();
        if (window == null)
        {
            return;
        }

        IReadOnlyList<IStorageFolder> folders = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select Directory to Fix Permissions",
            AllowMultiple = false
        });

        if (folders.Count == 0)
        {
            return;
        }

        string directory = folders[0].Path.LocalPath;

        MsBox.Avalonia.Base.IMsBox<ButtonResult> confirmBox = MessageBoxManager.GetMessageBoxStandard(
            "Warning!",
            "This is not a toy. Really continue?",
            ButtonEnum.YesNo,
            Icon.Warning);
        ButtonResult result = await confirmBox.ShowAsync();
        if (result != ButtonResult.Yes)
        {
            return;
        }

        IsTaskRunning = true;
        ClearLogText();
        AddLogEntry("Please wait, this may take awhile...");

        await Task.Run(() =>
        {
            try
            {
                SystemHelpers.FixPermissions(directory, msg => AddLogEntry(msg));

                int numFiles = 0;
                int numFolders = 0;
                if (Directory.Exists(directory))
                {
                    numFiles = Directory.GetFiles(directory, "*", SearchOption.AllDirectories).Length;
                    numFolders = Directory.GetDirectories(directory, "*", SearchOption.AllDirectories).Length;
                }

                string extraMsg = $"{numFiles} files and {numFolders} folders finished processing.";
                AddLogEntry(extraMsg);

                Avalonia.Threading.Dispatcher.UIThread.Post(async () =>
                {
                    MsBox.Avalonia.Base.IMsBox<ButtonResult> successBox = MessageBoxManager.GetMessageBoxStandard(
                        "Successfully acquired permission",
                        $"The operation was successful. {extraMsg}",
                        ButtonEnum.Ok,
                        Icon.Success);
                    await successBox.ShowAsync();
                });
            }
            catch (Exception ex)
            {
                AddLogEntry($"[ERROR] Failed to fix permissions: {ex.Message}");
                Avalonia.Threading.Dispatcher.UIThread.Post(async () =>
                {
                    MsBox.Avalonia.Base.IMsBox<ButtonResult> errorBox = MessageBoxManager.GetMessageBoxStandard(
                        "Could not acquire permission!",
                        $"Permissions denied! Check the logs for more details.{Environment.NewLine}{ex.Message}",
                        ButtonEnum.Ok,
                        Icon.Error);
                    await errorBox.ShowAsync();
                });
            }
            finally
            {
                IsTaskRunning = false;
                _logger.AddNote("File/Folder permissions fixer task completed.");
            }
        });
    }

    [RelayCommand]
    private async Task FixCaseSensitivity()
    {
        Window? window = GetMainWindow();
        if (window == null)
        {
            return;
        }

        IReadOnlyList<IStorageFolder> folders = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select Directory to Fix Case Sensitivity",
            AllowMultiple = false
        });

        if (folders.Count == 0)
        {
            return;
        }

        string directory = folders[0].Path.LocalPath;

        IsTaskRunning = true;
        ClearLogText();
        AddLogEntry("Please wait, this may take awhile...");

        await Task.Run(() =>
        {
            try
            {
                bool madeChange = false;
                SystemHelpers.FixCaseSensitivity(directory, msg =>
                {
                    AddLogEntry(msg);
                    madeChange = true;
                });

                if (!madeChange)
                {
                    AddLogEntry("Nothing to change - all files/folders already correct case.");
                }
                AddLogEntry("iOS case rename task completed.");
            }
            catch (Exception ex)
            {
                AddLogEntry($"[ERROR] Failed to fix case sensitivity: {ex.Message}");
            }
            finally
            {
                IsTaskRunning = false;
            }
        });
    }

    [RelayCommand]
    private void OpenUrl(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            AddLogEntry($"[ERROR] Failed to open URL: {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task CheckUpdates()
    {
        try
        {
            Dictionary<string, object>? updateInfo = await Config.GetRemoteHolopatcherUpdateInfoAsync();
            if (updateInfo == null)
            {
                MsBox.Avalonia.Base.IMsBox<ButtonResult> errorBox = MessageBoxManager.GetMessageBoxStandard(
                    "Error occurred fetching update information.",
                    "An error occurred while fetching the latest toolset information. Would you like to check against the local database instead?",
                    ButtonEnum.YesNo,
                    Icon.Error);
                ButtonResult result = await errorBox.ShowAsync();
                if (result == ButtonResult.No)
                {
                    return;
                }
                updateInfo = new Dictionary<string, object> { ["holopatcherLatestVersion"] = Config.CurrentVersion };
            }

            string latestVersion = updateInfo.ContainsKey("holopatcherLatestVersion")
                ? updateInfo["holopatcherLatestVersion"].ToString() ?? Config.CurrentVersion
                : Config.CurrentVersion;

            if (Config.RemoteVersionNewer(Config.CurrentVersion, latestVersion))
            {
                MsBox.Avalonia.Base.IMsBox<ButtonResult> updateBox = MessageBoxManager.GetMessageBoxStandard(
                    "Update Available",
                    "A newer version of HoloPatcher is available, would you like to download it now?",
                    ButtonEnum.YesNo,
                    Icon.Question);
                ButtonResult updateResult = await updateBox.ShowAsync();
                if (updateResult == ButtonResult.Yes)
                {
                    // TODO: Implement auto-update
                    string downloadLink = updateInfo.ContainsKey("holopatcherDownloadLink")
                        ? updateInfo["holopatcherDownloadLink"].ToString() ?? ""
                        : "";
                    if (!string.IsNullOrEmpty(downloadLink))
                    {
                        OpenUrl(downloadLink);
                    }
                }
            }
            else
            {
                MsBox.Avalonia.Base.IMsBox<ButtonResult> infoBox = MessageBoxManager.GetMessageBoxStandard(
                    "No updates available.",
                    $"You are already running the latest version of HoloPatcher ({Core.VersionLabel})",
                    ButtonEnum.Ok,
                    Icon.Info);
                await infoBox.ShowAsync();
            }
        }
        catch (Exception ex)
        {
            MsBox.Avalonia.Base.IMsBox<ButtonResult> errorBox = MessageBoxManager.GetMessageBoxStandard(
                "Unable to fetch latest version",
                $"An error occurred: {ex.Message}",
                ButtonEnum.Ok,
                Icon.Error);
            await errorBox.ShowAsync();
        }
    }

    [RelayCommand]
    private async Task ShowNamespaceInfo()
    {
        if (string.IsNullOrEmpty(SelectedNamespace))
        {
            MsBox.Avalonia.Base.IMsBox<ButtonResult> infoBox = MessageBoxManager.GetMessageBoxStandard(
                "No namespace selected",
                "Please select a namespace first.",
                ButtonEnum.Ok,
                Icon.Info);
            await infoBox.ShowAsync();
            return;
        }

        string description = Core.GetNamespaceDescription(_loadedNamespaces, SelectedNamespace);
        MsBox.Avalonia.Base.IMsBox<ButtonResult> descBox = MessageBoxManager.GetMessageBoxStandard(
            SelectedNamespace,
            string.IsNullOrEmpty(description) ? "No description available." : description,
            ButtonEnum.Ok,
            Icon.Info);
        await descBox.ShowAsync();
    }

    private async Task LoadModFromPath(string path)
    {
        try
        {
            Core.ModInfo modInfo = Core.LoadMod(path);
            ModPath = modInfo.ModPath;
            _loadedNamespaces = modInfo.Namespaces;
            _currentConfigReader = modInfo.ConfigReader;

            AddLogEntry($"Loaded {_loadedNamespaces.Count} namespace(s)");

            // Update UI on main thread
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                Namespaces.Clear();
                foreach (PatcherNamespace ns in _loadedNamespaces)
                {
                    string displayName = !string.IsNullOrWhiteSpace(ns.Name) ? ns.Name : ns.ChangesFilePath();
                    Namespaces.Add(displayName);
                }

                if (Namespaces.Count > 0)
                {
                    SelectedNamespace = Namespaces[0];
                    OnNamespaceSelected();
                }
            });
        }
        catch (Exception ex)
        {
            AddLogEntry($"[ERROR] Failed to load mod: {ex.Message}");
            MsBox.Avalonia.Base.IMsBox<ButtonResult> errorBox = MessageBoxManager.GetMessageBoxStandard(
                "Error",
                $"Could not find a mod located at the given folder.{Environment.NewLine}{ex.Message}",
                ButtonEnum.Ok,
                Icon.Error);
            await errorBox.ShowAsync();
        }
    }

    private void OnNamespaceSelected()
    {
        if (string.IsNullOrEmpty(SelectedNamespace) || string.IsNullOrEmpty(ModPath))
        {
            return;
        }

        try
        {
            Core.NamespaceInfo namespaceInfo = Core.LoadNamespaceConfig(ModPath, _loadedNamespaces, SelectedNamespace, _currentConfigReader);
            _logLevel = namespaceInfo.LogLevel;

            // Update game paths based on namespace
            if (namespaceInfo.GamePaths.Count > 0)
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    GamePaths.Clear();
                    foreach (string gamePath in namespaceInfo.GamePaths)
                    {
                        if (!GamePaths.Contains(gamePath))
                        {
                            GamePaths.Add(gamePath);
                        }
                    }
                    if (GamePaths.Count > 0 && string.IsNullOrEmpty(SelectedGamePath))
                    {
                        SelectedGamePath = GamePaths[0];
                    }
                });
            }

            // Load and display info.rtf/rte content
            if (!string.IsNullOrEmpty(namespaceInfo.InfoContent))
            {
                ClearLogText();
                AddLogEntry(namespaceInfo.InfoContent);
            }
        }
        catch (Exception ex)
        {
            AddLogEntry($"[ERROR] Failed to load namespace config: {ex.Message}");
        }
    }

    partial void OnSelectedNamespaceChanged(string? value)
    {
        OnPropertyChanged(nameof(CanInstall));
        if (!string.IsNullOrEmpty(value))
        {
            OnNamespaceSelected();
        }
    }

    private void DetectGamePaths()
    {
        // Common installation paths for KOTOR
        string[] commonPaths = new[]
        {
            @"C:\Program Files (x86)\Steam\steamapps\common\Knights of the Old Republic II",
            @"C:\Program Files (x86)\Steam\steamapps\common\swkotor",
            @"C:\Program Files\Steam\steamapps\common\Knights of the Old Republic II",
            @"C:\Program Files\Steam\steamapps\common\swkotor",
            @"C:\GOG Games\Star Wars - KotOR",
            @"C:\GOG Games\Star Wars - KotOR2",
        };

        foreach (string? path in commonPaths)
        {
            if (Directory.Exists(path) && Installation.DetermineGame(path) != null)
            {
                if (!GamePaths.Contains(path))
                {
                    GamePaths.Add(path);
                }
            }
        }

        if (GamePaths.Count > 0)
        {
            SelectedGamePath = GamePaths[0];
            AddLogEntry($"Detected {GamePaths.Count} KOTOR installation(s).");
        }
    }

    private static Window? GetMainWindow()
    {
        return App.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
    }

    partial void OnIsTaskRunningChanged(bool value)
    {
        OnPropertyChanged(nameof(CanInstall));
    }


    partial void OnSelectedGamePathChanged(string? value)
    {
        OnPropertyChanged(nameof(CanInstall));
    }

    private bool PreInstallValidate()
    {
        if (IsTaskRunning)
        {
            MsBox.Avalonia.Base.IMsBox<ButtonResult> infoBox = MessageBoxManager.GetMessageBoxStandard(
                "Task already running",
                "Wait for the previous task to finish.",
                ButtonEnum.Ok,
                Icon.Info);
            Avalonia.Threading.Dispatcher.UIThread.Post(async () => await infoBox.ShowAsync());
            return false;
        }

        if (string.IsNullOrEmpty(ModPath) || !Directory.Exists(ModPath))
        {
            MsBox.Avalonia.Base.IMsBox<ButtonResult> infoBox = MessageBoxManager.GetMessageBoxStandard(
                "No mod chosen",
                "Select your mod directory first.",
                ButtonEnum.Ok,
                Icon.Info);
            Avalonia.Threading.Dispatcher.UIThread.Post(async () => await infoBox.ShowAsync());
            return false;
        }

        if (string.IsNullOrEmpty(SelectedGamePath))
        {
            MsBox.Avalonia.Base.IMsBox<ButtonResult> infoBox = MessageBoxManager.GetMessageBoxStandard(
                "No KOTOR directory chosen",
                "Select your KOTOR directory first.",
                ButtonEnum.Ok,
                Icon.Info);
            Avalonia.Threading.Dispatcher.UIThread.Post(async () => await infoBox.ShowAsync());
            return false;
        }

        var gamePath = new CaseAwarePath(SelectedGamePath);
        if (!gamePath.IsDirectory())
        {
            MsBox.Avalonia.Base.IMsBox<ButtonResult> infoBox = MessageBoxManager.GetMessageBoxStandard(
                "Invalid KOTOR directory chosen",
                "Select a valid path to your KOTOR install.",
                ButtonEnum.Ok,
                Icon.Info);
            Avalonia.Threading.Dispatcher.UIThread.Post(async () => await infoBox.ShowAsync());
            return false;
        }

        return true;
    }

    private bool ValidateGamePathAndNamespace()
    {
        return PreInstallValidate() && !string.IsNullOrEmpty(SelectedNamespace);
    }

    private bool ValidateModPathAndGamePath()
    {
        if (string.IsNullOrEmpty(ModPath) || string.IsNullOrEmpty(SelectedGamePath))
        {
            AddLogEntry("[ERROR] Please select both a mod and game directory.");
            return false;
        }
        return true;
    }
}
