using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TSLPatcher.Core.Common;
using TSLPatcher.Core.Config;
using TSLPatcher.Core.Installation;
using TSLPatcher.Core.Logger;
using TSLPatcher.Core.Namespaces;
using TSLPatcher.Core.Patcher;
using TSLPatcher.Core.Reader;
using TSLPatcher.Core.Uninstall;

namespace HoloPatcher
{

    /// <summary>
    /// Core functionality for HoloPatcher.
    /// Equivalent to holopatcher/core.py
    /// </summary>
    public static class Core
    {
        public const string VersionLabel = "v1.80";

        /// <summary>
        /// Exit codes for the application.
        /// </summary>
        public enum ExitCode
        {
            Success = 0,
            UnknownStartupError = 1,
            NumberOfArgs = 2,
            NamespacesIniNotFound = 3,
            NamespaceIndexOutOfRange = 4,
            ChangesIniNotFound = 5,
            AbortInstallUnsafe = 6,
            ExceptionDuringInstall = 7,
            InstallCompletedWithErrors = 8,
            Crash = 9,
            CloseForUpdateProcess = 10
        }

        /// <summary>
        /// Information about a loaded mod.
        /// </summary>
        public class ModInfo
        {
            public string ModPath { get; set; } = string.Empty;
            public List<PatcherNamespace> Namespaces { get; set; } = new List<PatcherNamespace>();
            public ConfigReader ConfigReader { get; set; }
        }

        /// <summary>
        /// Information about a selected namespace.
        /// </summary>
        public class NamespaceInfo
        {
            public ConfigReader ConfigReader { get; set; } = null!;
            public LogLevel LogLevel { get; set; }
            public int? GameNumber { get; set; }
            public List<string> GamePaths { get; set; } = new();
            public string? InfoContent { get; set; }
        }

        /// <summary>
        /// Result of a mod installation.
        /// </summary>
        public class InstallResult
        {
            public TimeSpan InstallTime { get; set; }
            public int NumErrors { get; set; }
            public int NumWarnings { get; set; }
            public int NumPatches { get; set; }
        }

        /// <summary>
        /// Loads a mod from a directory.
        /// </summary>
        public static ModInfo LoadMod(string directoryPath)
        {
            var tslPatchDataPath = new CaseAwarePath(directoryPath, "tslpatchdata");
            if (!tslPatchDataPath.IsDirectory() &&
                Path.GetFileName(Path.GetDirectoryName(directoryPath) ?? "")?.ToLowerInvariant() == "tslpatchdata")
            {
                tslPatchDataPath = new CaseAwarePath(Path.GetDirectoryName(directoryPath) ?? "");
            }

            string modPath = tslPatchDataPath.DirectoryName;
            CaseAwarePath namespacePath = tslPatchDataPath.Combine("namespaces.ini");
            CaseAwarePath changesPath = tslPatchDataPath.Combine("changes.ini");

            List<PatcherNamespace> namespaces;
            ConfigReader? configReader = null;

            if (namespacePath.IsFile())
            {
                namespaces = NamespaceReader.FromFilePath(namespacePath.GetResolvedPath());
            }
            else if (changesPath.IsFile())
            {
                configReader = ConfigReader.FromFilePath(changesPath.GetResolvedPath(), tslPatchDataPath: tslPatchDataPath.GetResolvedPath());
                namespaces = new List<PatcherNamespace>
            {
                new PatcherNamespace("changes.ini", "info.rtf")
                {
                    Name = "Default",
                    Description = "Default installation"
                }
            };
            }
            else
            {
                throw new FileNotFoundException($"No namespaces.ini or changes.ini found in {tslPatchDataPath}");
            }

            return new ModInfo
            {
                ModPath = modPath,
                Namespaces = namespaces,
                ConfigReader = configReader
            };
        }

        /// <summary>
        /// Loads configuration for a specific namespace.
        /// </summary>
        public static NamespaceInfo LoadNamespaceConfig(
            string modPath,
            List<PatcherNamespace> namespaces,
            string selectedNamespaceName,
            ConfigReader? configReader = null)
        {
            PatcherNamespace? namespaceOption = namespaces.FirstOrDefault(x => x.Name == selectedNamespaceName);
            if (namespaceOption == null)
            {
                throw new ArgumentException($"Namespace '{selectedNamespaceName}' not found in namespaces list");
            }

            var changesIniPath = new CaseAwarePath(modPath, "tslpatchdata", namespaceOption.ChangesFilePath());
            string tslPatchDataPath = new CaseAwarePath(modPath, "tslpatchdata").GetResolvedPath();

            ConfigReader reader = configReader ?? ConfigReader.FromFilePath(changesIniPath.GetResolvedPath(), tslPatchDataPath: tslPatchDataPath);
            if (configReader == null)
            {
                reader.Load(reader.Config); // Load() populates the Config
            }

            int? gameNumber = reader.Config.GameNumber;
            Game? game = gameNumber.HasValue ? (Game)gameNumber.Value : null;

            List<string> gamePaths = new();
            if (game.HasValue)
            {
                // Find KOTOR paths - simplified version, should use proper path detection
                Dictionary<Game, List<string>> detectedPaths = FindKotorPathsFromDefault();
                if (detectedPaths.TryGetValue(game.Value, out List<string>? paths))
                {
                    gamePaths.AddRange(paths);
                }
                // If TSL, also include K1 paths
                if (game.Value == Game.TSL && detectedPaths.TryGetValue(Game.K1, out List<string>? k1Paths))
                {
                    gamePaths.AddRange(k1Paths);
                }
            }

            // Load info.rtf or info.rte
            var infoRtfPath = new CaseAwarePath(modPath, "tslpatchdata", namespaceOption.RtfFilePath());
            var infoRtePath = new CaseAwarePath(infoRtfPath.GetResolvedPath().Replace(".rtf", ".rte"));

            string? infoContent = null;
            if (infoRtePath.IsFile())
            {
                byte[] data = File.ReadAllBytes(infoRtePath.GetResolvedPath());
                infoContent = DecodeBytesWithFallbacks(data);
            }
            else if (infoRtfPath.IsFile())
            {
                byte[] data = File.ReadAllBytes(infoRtfPath.GetResolvedPath());
                string rtfText = DecodeBytesWithFallbacks(data);
                infoContent = StripRtf(rtfText);
            }

            return new NamespaceInfo
            {
                ConfigReader = reader,
                LogLevel = reader.Config.LogLevel,
                GameNumber = gameNumber,
                GamePaths = gamePaths,
                InfoContent = infoContent
            };
        }

        /// <summary>
        /// Validates a KOTOR game directory.
        /// </summary>
        public static string ValidateGameDirectory(string directoryPath)
        {
            var directory = new CaseAwarePath(directoryPath);
            if (!directory.IsDirectory())
            {
                throw new ArgumentException($"Invalid KOTOR directory: {directoryPath}");
            }
            return directory.GetResolvedPath();
        }

        /// <summary>
        /// Validates that mod and game paths are ready for installation.
        /// </summary>
        public static bool ValidateInstallPaths(string modPath, string gamePath)
        {
            return !string.IsNullOrEmpty(modPath) &&
                   new CaseAwarePath(modPath).IsDirectory() &&
                   !string.IsNullOrEmpty(gamePath) &&
                   new CaseAwarePath(gamePath).IsDirectory();
        }

        /// <summary>
        /// Gets the description for a namespace by name.
        /// </summary>
        public static string GetNamespaceDescription(List<PatcherNamespace> namespaces, string selectedNamespaceName)
        {
            PatcherNamespace? namespaceOption = namespaces.FirstOrDefault(x => x.Name == selectedNamespaceName);
            return namespaceOption?.Description ?? "";
        }

        /// <summary>
        /// Calculates total number of patches for progress calculation.
        /// </summary>
        public static int CalculateTotalPatches(ModInstaller installer)
        {
            PatcherConfig config = installer.Config();
            // Count TLK patches manually since GetTlkPatches is private
            int tlkPatches = config.PatchesTLK.Modifiers.Count;
            return config.InstallList.Count +
                   tlkPatches +
                   config.Patches2DA.Count +
                   config.PatchesGFF.Count +
                   config.PatchesNSS.Count +
                   config.PatchesNCS.Count +
                   config.PatchesSSF.Count;
        }

        /// <summary>
        /// Gets confirmation message if mod requires it.
        /// </summary>
        public static string? GetConfirmMessage(ModInstaller installer)
        {
            string msg = installer.Config().ConfirmMessage?.Trim() ?? "";
            return !string.IsNullOrEmpty(msg) && msg != "N/A" ? msg : null;
        }

        /// <summary>
        /// Installs a mod.
        /// </summary>
        public static InstallResult InstallMod(
            string modPath,
            string gamePath,
            List<PatcherNamespace> namespaces,
            string selectedNamespaceName,
            PatchLogger logger,
            CancellationToken cancellationToken,
            Action<int>? progressCallback = null)
        {
            PatcherNamespace? namespaceOption = namespaces.FirstOrDefault(x => x.Name == selectedNamespaceName);
            if (namespaceOption == null)
            {
                throw new ArgumentException($"Namespace '{selectedNamespaceName}' not found in namespaces list");
            }

            string tslPatchDataPath = new CaseAwarePath(modPath, "tslpatchdata").GetResolvedPath();
            string iniFilePath = new CaseAwarePath(tslPatchDataPath, namespaceOption.ChangesFilePath()).GetResolvedPath();
            string namespaceModPath = Path.GetDirectoryName(iniFilePath) ?? tslPatchDataPath;

            var installer = new ModInstaller(namespaceModPath, gamePath, iniFilePath, logger)
            {
                TslPatchDataPath = tslPatchDataPath
            };

            DateTime installStartTime = DateTime.UtcNow;
            installer.Install(cancellationToken, progressCallback);
            TimeSpan totalInstallTime = DateTime.UtcNow - installStartTime;

            int numErrors = logger.Errors.Count();
            int numWarnings = logger.Warnings.Count();
            int numPatches = installer.Config().PatchCount();

            string timeStr = FormatInstallTime(totalInstallTime);
            logger.AddNote(
                $"The installation is complete with {numErrors} errors and {numWarnings} warnings.{Environment.NewLine}" +
                $"Total install time: {timeStr}{Environment.NewLine}" +
                $"Total patches: {numPatches}");

            return new InstallResult
            {
                InstallTime = totalInstallTime,
                NumErrors = numErrors,
                NumWarnings = numWarnings,
                NumPatches = numPatches
            };
        }

        /// <summary>
        /// Validates a mod's configuration.
        /// </summary>
        public static void ValidateConfig(
            string modPath,
            List<PatcherNamespace> namespaces,
            string selectedNamespaceName,
            PatchLogger logger)
        {
            PatcherNamespace? namespaceOption = namespaces.FirstOrDefault(x => x.Name == selectedNamespaceName);
            if (namespaceOption == null)
            {
                throw new ArgumentException($"Namespace '{selectedNamespaceName}' not found in namespaces list");
            }

            string iniFilePath = new CaseAwarePath(modPath, "tslpatchdata", namespaceOption.ChangesFilePath()).GetResolvedPath();
            string tslPatchDataPath = new CaseAwarePath(modPath, "tslpatchdata").GetResolvedPath();

            var reader = ConfigReader.FromFilePath(iniFilePath, logger, tslPatchDataPath: tslPatchDataPath);
            reader.Load(reader.Config);
        }

        /// <summary>
        /// Uninstalls a mod using its backup.
        /// </summary>
        public static bool UninstallMod(
            string modPath,
            string gamePath,
            PatchLogger logger,
            Func<string, string, bool>? showYesNoDialog = null,
            Func<string, string, bool?>? showYesNoCancelDialog = null,
            Action<string, string>? showErrorDialog = null)
        {
            string backupParentFolder = Path.Combine(modPath, "backup");
            if (!Directory.Exists(backupParentFolder))
            {
                throw new DirectoryNotFoundException($"Backup folder not found: {backupParentFolder}");
            }

            var uninstaller = new ModUninstaller(
                new CaseAwarePath(backupParentFolder),
                new CaseAwarePath(gamePath),
                logger);

            return uninstaller.UninstallSelectedMod(
                showErrorDialog: showErrorDialog ?? ((title, msg) => { }),
                showYesNoDialog: showYesNoDialog ?? ((title, msg) => true),
                showYesNoCancelDialog: showYesNoCancelDialog ?? ((title, msg) => true));
        }

        /// <summary>
        /// Formats an installation time as a human-readable string.
        /// </summary>
        public static string FormatInstallTime(TimeSpan installTime)
        {
            int days = (int)installTime.TotalDays;
            int hours = installTime.Hours;
            int minutes = installTime.Minutes;
            int seconds = installTime.Seconds;

            var parts = new List<string>();
            if (days > 0) { parts.Add($"{days} days"); }
            if (hours > 0) { parts.Add($"{hours} hours"); }
            if (minutes > 0 || (days == 0 && hours == 0)) { parts.Add($"{minutes} minutes"); }
            parts.Add($"{seconds} seconds");

            return string.Join(", ", parts);
        }

        /// <summary>
        /// Gets the log file path for a mod.
        /// </summary>
        public static string GetLogFilePath(string modPath)
        {
            return Path.Combine(modPath, "installlog.txt");
        }

        /// <summary>
        /// Finds KOTOR installation paths from default locations.
        /// </summary>
        private static Dictionary<Game, List<string>> FindKotorPathsFromDefault()
        {
            var paths = new Dictionary<Game, List<string>>();

            // Common installation paths
            string[] commonPaths = new[]
            {
            @"C:\Program Files (x86)\Steam\steamapps\common\Knights of the Old Republic II",
            @"C:\Program Files (x86)\Steam\steamapps\common\swkotor",
            @"C:\Program Files\Steam\steamapps\common\Knights of the Old Republic II",
            @"C:\Program Files\Steam\steamapps\common\swkotor",
            @"C:\GOG Games\Star Wars - KotOR",
            @"C:\GOG Games\Star Wars - KotOR2",
        };

            foreach (string path in commonPaths)
            {
                if (Directory.Exists(path))
                {
                    Game? game = Installation.DetermineGame(path);
                    if (game.HasValue)
                    {
                        if (!paths.ContainsKey(game.Value))
                        {
                            paths[game.Value] = new List<string>();
                        }
                        if (!paths[game.Value].Contains(path))
                        {
                            paths[game.Value].Add(path);
                        }
                    }
                }
            }

            return paths;
        }

        /// <summary>
        /// Decodes bytes with fallback encodings.
        /// </summary>
        private static string DecodeBytesWithFallbacks(byte[] data)
        {
            // Try UTF-8 first
            try
            {
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                // Fallback to Windows-1252
                try
                {
                    Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    return Encoding.GetEncoding("windows-1252").GetString(data);
                }
                catch
                {
                    // Final fallback to ASCII
                    return Encoding.ASCII.GetString(data);
                }
            }
        }

        /// <summary>
        /// Strips RTF formatting from text.
        /// </summary>
        private static string StripRtf(string rtfText)
        {
            if (string.IsNullOrEmpty(rtfText))
            {
                return "";
            }

            // Simple RTF stripping - remove control words and groups
            var sb = new StringBuilder();
            bool inControl = false;
            bool inGroup = false;
            int braceDepth = 0;

            for (int i = 0; i < rtfText.Length; i++)
            {
                char c = rtfText[i];

                if (c == '{')
                {
                    braceDepth++;
                    inGroup = true;
                    continue;
                }

                if (c == '}')
                {
                    braceDepth--;
                    if (braceDepth == 0)
                    {
                        inGroup = false;
                    }
                    continue;
                }

                if (inGroup && c == '\\')
                {
                    inControl = true;
                    continue;
                }

                if (inControl)
                {
                    if (char.IsLetter(c))
                    {
                        // Skip control word
                        continue;
                    }
                    if (c == ' ')
                    {
                        inControl = false;
                        continue;
                    }
                    if (char.IsDigit(c) || c == '-')
                    {
                        // Skip numeric parameter
                        continue;
                    }
                    inControl = false;
                }

                if (!inGroup && !inControl)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Trim();
        }
    }
}

