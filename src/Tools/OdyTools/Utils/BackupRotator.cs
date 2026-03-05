using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OdyTools.Utils
{
    public sealed class BackupFileInfo
    {
        public int Generation { get; set; }
        public string Path { get; set; }
        public DateTime LastWriteUtc { get; set; }
        public long Length { get; set; }
    }

    public static class BackupRotator
    {
        public static string GetPrimaryBackupPath(string filePath)
        {
            return filePath + ".bak";
        }

        public static string GetBackupPath(string filePath, int generation)
        {
            return generation <= 0
                ? GetPrimaryBackupPath(filePath)
                : filePath + ".bak" + generation;
        }

        public static void RotateBackups(string filePath, int maxCount)
        {
            if (string.IsNullOrWhiteSpace(filePath) || maxCount <= 0)
            {
                return;
            }

            int highestGeneration = Math.Max(0, maxCount - 1);
            for (int generation = highestGeneration; generation >= 1; generation--)
            {
                string source = GetBackupPath(filePath, generation - 1);
                string destination = GetBackupPath(filePath, generation);
                if (!File.Exists(source))
                {
                    continue;
                }

                if (File.Exists(destination))
                {
                    File.Delete(destination);
                }

                File.Move(source, destination);
            }
        }

        public static void RestoreFromBackup(string filePath, int generation)
        {
            string backupPath = GetBackupPath(filePath, generation);
            if (!File.Exists(backupPath))
            {
                throw new FileNotFoundException("Backup file not found.", backupPath);
            }

            File.Copy(backupPath, filePath, overwrite: true);
        }

        public static List<BackupFileInfo> GetAvailableBackups(string filePath)
        {
            var backups = new List<BackupFileInfo>();

            string primaryPath = GetPrimaryBackupPath(filePath);
            if (File.Exists(primaryPath))
            {
                var fi = new FileInfo(primaryPath);
                backups.Add(new BackupFileInfo
                {
                    Generation = 0,
                    Path = primaryPath,
                    LastWriteUtc = fi.LastWriteTimeUtc,
                    Length = fi.Length
                });
            }

            for (int generation = 1; generation <= 99; generation++)
            {
                string path = GetBackupPath(filePath, generation);
                if (!File.Exists(path))
                {
                    continue;
                }

                var fi = new FileInfo(path);
                backups.Add(new BackupFileInfo
                {
                    Generation = generation,
                    Path = path,
                    LastWriteUtc = fi.LastWriteTimeUtc,
                    Length = fi.Length
                });
            }

            return backups
                .OrderBy(b => b.Generation)
                .ToList();
        }
    }
}
