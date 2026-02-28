using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.Input;
using BioWare.Common;
using ConvertKotorGame.Models;
using ConvertKotorGame.Services;

namespace ConvertKotorGame.ViewModels
{
    public sealed class MainWindowViewModel : ViewModelBase
    {
        private readonly InstallationDetectionService _detectionService = new InstallationDetectionService();
        private readonly InstallationConverterService _converterService = new InstallationConverterService();

        private string _k1Path = "";
        private string _tslPath = "";
        private string _outputPath = "";
        private bool _outputPathUserModified;
        private string _detectionDetailsText = "No installations detected.";
        private int _progressValue;
        private int _progressMaximum = 1;
        private bool _isBusy;
        private bool _isRefreshing;
        private string _lastOutputPath;
        private BioWareGame? _lastTargetGame;
        private StreamWriter _logFileWriter;
        private readonly object _logFileLock = new object();
        private LogLevelKind _logVerbosity = LogLevelKind.Info;

        public MainWindowViewModel()
        {
            K1Options = new ObservableCollection<string>();
            TslOptions = new ObservableCollection<string>();
            LogVerbosityOptions = new ObservableCollection<LogLevelKind> { LogLevelKind.Trace, LogLevelKind.Info, LogLevelKind.Warning, LogLevelKind.Error };
            AllLogs = new ObservableCollection<LogEntry>();
            DisplayedLogs = new ObservableCollection<LogEntry>();
            BrowseK1Command = new AsyncRelayCommand(BrowseK1Async);
            BrowseTslCommand = new AsyncRelayCommand(BrowseTslAsync);
            BrowseOutputCommand = new AsyncRelayCommand(BrowseOutputAsync);
            ConvertK1ToTslCommand = new AsyncRelayCommand(ConvertK1ToTslAsync);
            ConvertTslToK1Command = new AsyncRelayCommand(ConvertTslToK1Async);
            OpenConvertDirectoryCommand = new RelayCommand(OpenConvertDirectory, () => CanOpenConvertDirectory);
            StartGameCommand = new RelayCommand(StartGame, () => CanStartGame);
            _logFileWriter = OpenOrCreateLogFile();
            RefreshAutoDetectedInstallations();
        }

        public ObservableCollection<string> K1Options { get; }
        public ObservableCollection<string> TslOptions { get; }
        public ObservableCollection<LogLevelKind> LogVerbosityOptions { get; }
        public ObservableCollection<LogEntry> AllLogs { get; }
        public ObservableCollection<LogEntry> DisplayedLogs { get; }

        public LogLevelKind LogVerbosity
        {
            get => _logVerbosity;
            set
            {
                if (SetProperty(ref _logVerbosity, value))
                {
                    RefreshDisplayedLogs();
                }
            }
        }

        public IAsyncRelayCommand BrowseK1Command { get; }
        public IAsyncRelayCommand BrowseTslCommand { get; }
        public IAsyncRelayCommand BrowseOutputCommand { get; }
        public IAsyncRelayCommand ConvertK1ToTslCommand { get; }
        public IAsyncRelayCommand ConvertTslToK1Command { get; }
        public IRelayCommand OpenConvertDirectoryCommand { get; }
        public IRelayCommand StartGameCommand { get; }

        public string K1Path
        {
            get => _k1Path;
            set
            {
                if (SetProperty(ref _k1Path, value))
                {
                    if (!_isRefreshing)
                    {
                        if (string.Equals(value, InstallationDetectionService.AutoDetectOption, StringComparison.OrdinalIgnoreCase))
                        {
                            RefreshAutoDetectedInstallations();
                            return;
                        }
                        UpdateDetectionDetails();
                    }
                    if (!_outputPathUserModified)
                    {
                        _outputPath = GenerateDefaultOutputPath(value, "tsl");
                        OnPropertyChanged(nameof(OutputPath));
                    }
                    OnPropertyChanged(nameof(CanConvert));
                }
            }
        }

        public string OutputPath
        {
            get => _outputPath;
            set
            {
                if (SetProperty(ref _outputPath, value))
                {
                    _outputPathUserModified = true;
                    OnPropertyChanged(nameof(CanConvert));
                }
            }
        }

        public string TslPath
        {
            get => _tslPath;
            set
            {
                if (SetProperty(ref _tslPath, value))
                {
                    if (!_isRefreshing)
                    {
                        if (string.Equals(value, InstallationDetectionService.AutoDetectOption, StringComparison.OrdinalIgnoreCase))
                        {
                            RefreshAutoDetectedInstallations();
                            return;
                        }
                        UpdateDetectionDetails();
                    }
                    OnPropertyChanged(nameof(CanConvert));
                }
            }
        }

        public string DetectionDetailsText
        {
            get => _detectionDetailsText;
            private set => SetProperty(ref _detectionDetailsText, value);
        }

        public int ProgressValue
        {
            get => _progressValue;
            private set => SetProperty(ref _progressValue, value);
        }

        public int ProgressMaximum
        {
            get => _progressMaximum;
            private set => SetProperty(ref _progressMaximum, value);
        }

        public bool CanConvert => !_isBusy &&
            !string.IsNullOrWhiteSpace(_k1Path) &&
            !string.IsNullOrWhiteSpace(_tslPath) &&
            !string.IsNullOrWhiteSpace(_outputPath) &&
            !string.Equals(_k1Path, InstallationDetectionService.AutoDetectOption, StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(_tslPath, InstallationDetectionService.AutoDetectOption, StringComparison.OrdinalIgnoreCase);

        public bool CanOpenConvertDirectory => !string.IsNullOrWhiteSpace(_lastOutputPath) && Directory.Exists(_lastOutputPath);

        public bool CanStartGame => CanOpenConvertDirectory && _lastTargetGame.HasValue;

        private void RefreshAutoDetectedInstallations()
        {
            _isRefreshing = true;
            try
            {
                K1Options.Clear();
                TslOptions.Clear();
                K1Options.Add(InstallationDetectionService.AutoDetectOption);
                TslOptions.Add(InstallationDetectionService.AutoDetectOption);

                var installs = _detectionService.DetectInstallations();
                string firstK1 = null;
                string firstTsl = null;

                foreach (InstallationDetectionInfo info in installs)
                {
                    if (!info.Game.HasValue)
                    {
                        continue;
                    }

                    if (info.Game.Value.IsK1())
                    {
                        K1Options.Add(info.Path);
                        if (firstK1 == null)
                        {
                            firstK1 = info.Path;
                        }
                    }
                    else if (info.Game.Value.IsK2() || info.Game.Value.IsTSL())
                    {
                        TslOptions.Add(info.Path);
                        if (firstTsl == null)
                        {
                            firstTsl = info.Path;
                        }
                    }
                }

                _k1Path = firstK1 ?? "";
                OnPropertyChanged(nameof(K1Path));

                _tslPath = firstTsl ?? "";
                OnPropertyChanged(nameof(TslPath));

                if (!_outputPathUserModified)
                {
                    _outputPath = GenerateDefaultOutputPath(_k1Path, "tsl");
                    OnPropertyChanged(nameof(OutputPath));
                }
            }
            finally
            {
                _isRefreshing = false;
            }

            UpdateDetectionDetails();
            OnPropertyChanged(nameof(CanConvert));
        }

        private void UpdateDetectionDetails()
        {
            string k1Info = "";
            string tslInfo = "";

            if (!string.IsNullOrWhiteSpace(_k1Path) &&
                !string.Equals(_k1Path, InstallationDetectionService.AutoDetectOption, StringComparison.OrdinalIgnoreCase))
            {
                InstallationDetectionInfo info = _detectionService.DetectSingle(_k1Path);
                k1Info = "K1: " + _k1Path + " (" + info.Distribution + " | " + info.PlatformSummary + ")";
            }

            if (!string.IsNullOrWhiteSpace(_tslPath) &&
                !string.Equals(_tslPath, InstallationDetectionService.AutoDetectOption, StringComparison.OrdinalIgnoreCase))
            {
                InstallationDetectionInfo info = _detectionService.DetectSingle(_tslPath);
                tslInfo = "TSL: " + _tslPath + " (" + info.Distribution + " | " + info.PlatformSummary + ")";
            }

            if (string.IsNullOrEmpty(k1Info) && string.IsNullOrEmpty(tslInfo))
            {
                DetectionDetailsText = "No installations detected. Enter paths or browse.";
            }
            else
            {
                string[] parts = new[] { k1Info, tslInfo }.Where(s => !string.IsNullOrEmpty(s)).ToArray();
                DetectionDetailsText = string.Join(Environment.NewLine, parts);
            }
        }

        private async Task BrowseK1Async()
        {
            string path = await BrowseFolderAsync("Select KotOR 1 installation folder");
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (!K1Options.Contains(path))
                {
                    K1Options.Add(path);
                }
                K1Path = path;
            }
        }

        private async Task BrowseTslAsync()
        {
            string path = await BrowseFolderAsync("Select KotOR 2 (TSL) installation folder");
            if (!string.IsNullOrWhiteSpace(path))
            {
                if (!TslOptions.Contains(path))
                {
                    TslOptions.Add(path);
                }
                TslPath = path;
            }
        }

        private async Task BrowseOutputAsync()
        {
            string path = await BrowseFolderAsync("Select output folder (will be created if needed)");
            if (!string.IsNullOrWhiteSpace(path))
            {
                OutputPath = path;
            }
        }

        private static string GenerateDefaultOutputPath(string k1Path, string targetGameSuffix)
        {
            if (string.IsNullOrWhiteSpace(k1Path) ||
                string.Equals(k1Path, InstallationDetectionService.AutoDetectOption, StringComparison.OrdinalIgnoreCase))
            {
                return "";
            }

            string parent = System.IO.Directory.GetParent(
                k1Path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar))?.FullName ?? k1Path;
            string randomSuffix = Guid.NewGuid().ToString("N").Substring(0, 5);
            return Path.Combine(parent, "converted_" + targetGameSuffix + "_" + randomSuffix);
        }

        private async Task ConvertK1ToTslAsync()
        {
            await RunConversionAsync(_k1Path, _tslPath, BioWareGame.K1, BioWareGame.TSL, _outputPath);
        }

        private async Task ConvertTslToK1Async()
        {
            await RunConversionAsync(_tslPath, _k1Path, BioWareGame.TSL, BioWareGame.K1, _outputPath);
        }

        private async Task RunConversionAsync(string sourcePath, string targetBasePath, BioWareGame sourceGame, BioWareGame targetGame, string outputPath)
        {
            if (_isBusy)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(targetBasePath))
            {
                AddLog("Both K1 and TSL installation paths must be set.", LogLevelKind.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                AddLog("Output path must be set.", LogLevelKind.Warning);
                return;
            }

            if (!Directory.Exists(sourcePath))
            {
                AddLog("Source path does not exist: " + sourcePath, LogLevelKind.Error);
                return;
            }

            if (!Directory.Exists(targetBasePath))
            {
                AddLog("Target base path does not exist: " + targetBasePath, LogLevelKind.Error);
                return;
            }

            _isBusy = true;
            OnPropertyChanged(nameof(CanConvert));
            ProgressValue = 0;
            ProgressMaximum = 1;

            string direction = sourceGame.IsK1() ? "K1 → TSL" : "TSL → K1";
            AddLog("Starting conversion: " + direction, LogLevelKind.Info);
            AddLog("Source: " + sourcePath, LogLevelKind.Info);
            AddLog("Target base: " + targetBasePath, LogLevelKind.Info);
            AddLog("Output: " + outputPath, LogLevelKind.Info);

            try
            {
                ConversionSummary summary = await _converterService.ConvertInstallationAsync(
                    sourcePath,
                    targetBasePath,
                    sourceGame,
                    targetGame,
                    (msg, level) => AddLog(msg, level),
                    (value, max) =>
                    {
                        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                        {
                            ProgressMaximum = max <= 0 ? 1 : max;
                            ProgressValue = value;
                        });
                    },
                    CancellationToken.None,
                    outputPath);

                _lastOutputPath = summary.OutputPath ?? outputPath;
                _lastTargetGame = targetGame;
                OnPropertyChanged(nameof(CanOpenConvertDirectory));
                OnPropertyChanged(nameof(CanStartGame));
                OpenConvertDirectoryCommand.NotifyCanExecuteChanged();
                StartGameCommand.NotifyCanExecuteChanged();

                AddLog("Conversion completed. Output: " + summary.OutputPath, LogLevelKind.Info);
                AddLog("Converted=" + summary.ConvertedCount +
                       ", Copied=" + summary.CopiedCount +
                       ", Failed=" + summary.FailedCount, LogLevelKind.Info);

                foreach (var pair in summary.SeenByType.OrderBy(p => p.Key))
                {
                    int converted = summary.ConvertedByType.ContainsKey(pair.Key) ? summary.ConvertedByType[pair.Key] : 0;
                    int failed = summary.FailedByType.ContainsKey(pair.Key) ? summary.FailedByType[pair.Key] : 0;
                    string line = "  ." + pair.Key + ": seen=" + pair.Value + " converted=" + converted;
                    if (failed > 0)
                    {
                        line += " failed=" + failed;
                    }
                    AddLog(line, LogLevelKind.Trace);
                }
            }
            catch (Exception ex)
            {
                AddLog("Conversion failed: " + ex.Message, LogLevelKind.Error);
            }
            finally
            {
                _isBusy = false;
                OnPropertyChanged(nameof(CanConvert));
            }
        }

        private void AddLog(string message, LogLevelKind level)
        {
            string levelStr = level.ToString().ToUpperInvariant();
            string line = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] [" + levelStr + "] " + message;

            lock (_logFileLock)
            {
                try
                {
                    _logFileWriter?.WriteLine(line);
                    _logFileWriter?.Flush();
                }
                catch
                {
                    // Ignore log file write failures
                }
            }

            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                LogEntry entry = new LogEntry(message, level);
                AllLogs.Add(entry);
                if (PassesVerbosityFilter(level))
                {
                    DisplayedLogs.Add(entry);
                }
            });
        }

        private bool PassesVerbosityFilter(LogLevelKind level)
        {
            return (int)level >= (int)_logVerbosity;
        }

        private void RefreshDisplayedLogs()
        {
            DisplayedLogs.Clear();
            foreach (LogEntry entry in AllLogs)
            {
                if (PassesVerbosityFilter(entry.Level))
                {
                    DisplayedLogs.Add(entry);
                }
            }
        }

        private static StreamWriter OpenOrCreateLogFile()
        {
            string fileName = "convert_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

            // Primary: exe directory (where the running executable lives).
            string exeDir = AppContext.BaseDirectory;
            if (!string.IsNullOrWhiteSpace(exeDir) && TryOpenLogInDir(exeDir, fileName, out StreamWriter writer1))
            {
                return writer1;
            }

            // Fallback: platform-specific app data directory.
            string fallbackDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ConvertKotorGame");
            if (TryOpenLogInDir(fallbackDir, fileName, out StreamWriter writer2))
            {
                return writer2;
            }

            return null;
        }

        private static bool TryOpenLogInDir(string baseDir, string fileName, out StreamWriter writer)
        {
            writer = null;
            try
            {
                Directory.CreateDirectory(baseDir);
                string path = Path.Combine(baseDir, fileName);
                var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read);
                writer = new StreamWriter(fs) { AutoFlush = false };
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void OpenConvertDirectory()
        {
            if (string.IsNullOrWhiteSpace(_lastOutputPath) || !Directory.Exists(_lastOutputPath))
            {
                return;
            }

            try
            {
                string fileName;
                string arguments;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    fileName = "explorer.exe";
                    arguments = _lastOutputPath;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    fileName = "open";
                    arguments = _lastOutputPath;
                }
                else
                {
                    fileName = "xdg-open";
                    arguments = _lastOutputPath;
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = fileName,
                    Arguments = arguments,
                    UseShellExecute = !RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                });
            }
            catch (Exception ex)
            {
                AddLog("Failed to open directory: " + ex.Message, LogLevelKind.Error);
            }
        }

        private void StartGame()
        {
            if (string.IsNullOrWhiteSpace(_lastOutputPath) || !_lastTargetGame.HasValue)
            {
                return;
            }

            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            string exeName = _lastTargetGame.Value.IsK1()
                ? (isWindows ? "swkotor.exe" : "swkotor")
                : (isWindows ? "swkotor2.exe" : "swkotor2");
            string exePath = Path.Combine(_lastOutputPath, exeName);

            if (!File.Exists(exePath))
            {
                AddLog("Game executable not found: " + exePath, LogLevelKind.Error);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = exePath,
                    WorkingDirectory = _lastOutputPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                AddLog("Failed to start game: " + ex.Message, LogLevelKind.Error);
            }
        }

        private static async Task<string> BrowseFolderAsync(string title)
        {
            TopLevel topLevel = GetMainWindowTopLevel();
            if (topLevel == null)
            {
                return null;
            }

            var results = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = title,
                AllowMultiple = false,
            });

            IStorageFolder folder = results.FirstOrDefault();
            if (folder == null || folder.Path == null)
            {
                return null;
            }

            return folder.Path.LocalPath;
        }

        private static TopLevel GetMainWindowTopLevel()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                return TopLevel.GetTopLevel(desktop.MainWindow);
            }
            return null;
        }
    }
}
