using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Threading;
using BioWare.Common;
using Newtonsoft.Json;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using OdyTools.Data;
using OdyTools.Utils;
using IconType = MsBox.Avalonia.Enums.Icon;

namespace OdyTools.Editors
{
    /// <summary>
    /// Crash recovery service modeled after Notepad++ session backup.
    /// Periodically saves editor state to disk so it can be recovered after a crash.
    /// On normal exit, backup files are removed. On crash, they remain for recovery.
    /// </summary>
    public static class EditorCrashRecoveryService
    {
        private static readonly string BackupDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "OdyToolsV3", "Backup");
        private static readonly string SessionFilePath = Path.Combine(BackupDirectory, "session-recovery.json");
        private static readonly object Lock = new object();
        private static DispatcherTimer _timer;
        private static bool _cleanExitRequested;
        private const int MaxRecoveryAgeDays = 7;

        /// <summary>
        /// Starts the periodic backup timer. Call from app startup after MainWindow is created.
        /// </summary>
        public static void Start()
        {
            lock (Lock)
            {
                if (_timer != null) return;
                EnsureBackupDirectory();
                CleanupStaleBackups();
                _timer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(GetBackupIntervalSeconds())
                };
                _timer.Tick += OnBackupTick;
                _timer.Start();
            }
        }

        private static int GetBackupIntervalSeconds()
        {
            try
            {
                return Math.Max(5, GlobalSettings.Instance.CrashRecoveryIntervalSeconds);
            }
            catch
            {
                return 30;
            }
        }

        /// <summary>
        /// Stops the timer and deletes recovery data. Call on normal app exit.
        /// </summary>
        public static void OnCleanExit()
        {
            _cleanExitRequested = true;
            lock (Lock)
            {
                _timer?.Stop();
                _timer = null;
            }
            DeleteRecoveryData();
        }

        /// <summary>
        /// Returns true if recovery data exists (app crashed without clean exit).
        /// </summary>
        public static bool HasRecoveryData()
        {
            return File.Exists(SessionFilePath);
        }

        /// <summary>
        /// Gets recovery entries for display in recovery dialog.
        /// </summary>
        public static List<RecoveryEntry> GetRecoveryEntries()
        {
            if (!File.Exists(SessionFilePath)) return new List<RecoveryEntry>();
            try
            {
                string json = File.ReadAllText(SessionFilePath);
                var entries = JsonConvert.DeserializeObject<List<RecoveryEntry>>(json);
                return entries ?? new List<RecoveryEntry>();
            }
            catch
            {
                return new List<RecoveryEntry>();
            }
        }

        /// <summary>
        /// Restores editors from recovery data. Call after user confirms recovery.
        /// </summary>
        /// <param name="openEditor">Optional action to open each recovered editor</param>
        /// <returns>Number of editors restored</returns>
        public static int Restore(Func<string, string, ResourceType, byte[], object> openEditor = null)
        {
            var entries = GetRecoveryEntries();
            int restored = 0;
            foreach (var entry in entries)
            {
                try
                {
                    if (!File.Exists(entry.BackupPath)) continue;
                    byte[] data = File.ReadAllBytes(entry.BackupPath);
                    var restype = ResourceType.FromExtension(entry.RestypeExtension);
                    if (restype == null || restype.IsInvalid) continue;

                    string filepath = string.IsNullOrWhiteSpace(entry.Filepath) ? null : entry.Filepath.Trim();
                    string resname = string.IsNullOrWhiteSpace(entry.Resname)
                        ? (string.IsNullOrEmpty(filepath) ? "recovered" : Path.GetFileNameWithoutExtension(filepath))
                        : entry.Resname.Trim();

                    if (openEditor != null)
                    {
                        var result = openEditor(filepath, resname, restype, data);
                        if (result != null) restored++;
                    }
                    else
                    {
                        var result = WindowUtils.OpenResourceEditor(filepath, resname, restype, data, null, null, null);
                        if (result?.Item2 != null) restored++;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Recovery failed for {entry.DisplayName}: {ex.Message}");
                }
            }
            DeleteRecoveryData();
            return restored;
        }

        /// <summary>
        /// Discards recovery data without restoring.
        /// </summary>
        public static void DiscardRecovery()
        {
            DeleteRecoveryData();
        }

        /// <summary>
        /// Shows recovery dialog if recovery data exists. Call on app startup before MainWindow.
        /// Returns true if user chose to restore, false if discard or no recovery data.
        /// </summary>
        public static async System.Threading.Tasks.Task<bool> ShowRecoveryDialogIfNeededAsync()
        {
            if (!HasRecoveryData()) return false;
            var entries = GetRecoveryEntries();
            if (entries.Count == 0) { DiscardRecovery(); return false; }
            var fileList = string.Join("\n", entries.ConvertAll(e => "• " + e.DisplayName));
            var result = await DialogHelper.ShowAsync("Recovery Available", $"The application may have closed unexpectedly. Recover {entries.Count} unsaved editor(s)?\n\n{fileList}", ButtonEnum.YesNo, IconType.Question);
            if (result != ButtonResult.Yes)
            {
                DiscardRecovery();
                return false;
            }
            return true;
        }

        private static void EnsureBackupDirectory()
        {
            try
            {
                if (!Directory.Exists(BackupDirectory))
                    Directory.CreateDirectory(BackupDirectory);
            }
            catch
            {
                // Ignore
            }
        }

        private static void OnBackupTick(object sender, EventArgs e)
        {
            if (_cleanExitRequested) return;

            var windows = WindowUtils.GetTrackedWindows();
            var editors = windows.OfType<Editor>().ToList();
            if (editors.Count == 0) return;

            var entries = new List<RecoveryEntry>();
            try
            {
                EnsureBackupDirectory();
                foreach (var editor in editors)
                {
                    try
                    {
                        var entry = BackupEditor(editor);
                        if (entry != null) entries.Add(entry);
                    }
                    catch
                    {
                        // Skip failed editors
                    }
                }
                if (entries.Count > 0)
                {
                    string json = JsonConvert.SerializeObject(entries, Formatting.Indented);
                    AtomicFileWriter.WriteAtomic(SessionFilePath, System.Text.Encoding.UTF8.GetBytes(json), new AtomicWriteOptions
                    {
                        CreateBackup = false,
                        MaxBackups = 1,
                        RetryCount = 2,
                        RetryDelayMs = 150
                    });
                }
                else if (File.Exists(SessionFilePath))
                {
                    File.Delete(SessionFilePath);
                }
            }
            catch
            {
                // Ignore backup failures
            }
        }

        private static RecoveryEntry BackupEditor(Editor editor)
        {
            var (data, _) = editor.Build();
            if (data == null || data.Length == 0) return null;

            var (filepath, resname, restype) = editor.GetRecoveryInfo();
            if (string.IsNullOrEmpty(resname)) resname = "new";
            string ext = restype != null ? restype.Extension : "2da";

            string safeName = SanitizeFileName(string.IsNullOrEmpty(filepath) ? resname : Path.GetFileName(filepath));
            string backupFileName = $"{safeName}_{DateTime.UtcNow:yyyyMMddHHmmss}.backup";
            string backupPath = Path.Combine(BackupDirectory, backupFileName);

            AtomicFileWriter.WriteAtomic(backupPath, data, new AtomicWriteOptions
            {
                CreateBackup = false,
                MaxBackups = 1,
                RetryCount = 2,
                RetryDelayMs = 150
            });

            var fi = new FileInfo(backupPath);
            if (!fi.Exists || fi.Length == 0)
            {
                return null;
            }

            return new RecoveryEntry
            {
                Filepath = filepath ?? "",
                Resname = string.IsNullOrEmpty(resname) ? Path.GetFileNameWithoutExtension(safeName) : resname.Trim(),
                RestypeExtension = ext,
                BackupPath = backupPath,
                DisplayName = string.IsNullOrEmpty(filepath) ? $"{resname} (new)" : Path.GetFileName(filepath)
            };
        }

        private static string SanitizeFileName(string name)
        {
            var invalid = Path.GetInvalidFileNameChars();
            foreach (char c in invalid)
                name = name.Replace(c, '_');
            if (name.Length > 80) name = name.Substring(0, 80);
            return name;
        }

        private static void DeleteRecoveryData()
        {
            try
            {
                if (File.Exists(SessionFilePath))
                {
                    var entries = GetRecoveryEntries();
                    foreach (var e in entries)
                    {
                        try
                        {
                            if (File.Exists(e.BackupPath))
                                File.Delete(e.BackupPath);
                        }
                        catch { }
                    }
                    File.Delete(SessionFilePath);
                }
            }
            catch
            {
                // Ignore
            }
        }

        private static void CleanupStaleBackups()
        {
            try
            {
                if (!Directory.Exists(BackupDirectory))
                {
                    return;
                }

                DateTime threshold = DateTime.UtcNow.AddDays(-MaxRecoveryAgeDays);
                foreach (string file in Directory.EnumerateFiles(BackupDirectory, "*.backup"))
                {
                    try
                    {
                        if (File.GetLastWriteTimeUtc(file) < threshold)
                        {
                            File.Delete(file);
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }

                if (File.Exists(SessionFilePath) && File.GetLastWriteTimeUtc(SessionFilePath) < threshold)
                {
                    File.Delete(SessionFilePath);
                }
            }
            catch
            {
                // ignored
            }
        }

        public class RecoveryEntry
        {
            public string Filepath { get; set; }
            public string Resname { get; set; }
            public string RestypeExtension { get; set; }
            public string BackupPath { get; set; }
            public string DisplayName { get; set; }
        }
    }
}
