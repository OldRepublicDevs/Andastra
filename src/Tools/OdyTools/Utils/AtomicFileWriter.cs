using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace OdyTools.Utils
{
    public sealed class AtomicWriteOptions
    {
        public int RetryCount { get; set; } = 3;
        public int RetryDelayMs { get; set; } = 300;
        public bool CreateBackup { get; set; } = true;
        public bool VerifyLength { get; set; } = true;
        public int MaxBackups { get; set; } = 5;
    }

    public static class AtomicFileWriter
    {
        public static void WriteAtomic(string targetPath, byte[] data, AtomicWriteOptions options = null)
        {
            if (string.IsNullOrWhiteSpace(targetPath))
            {
                throw new ArgumentException("Target path cannot be null or whitespace.", nameof(targetPath));
            }
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (options == null)
            {
                options = new AtomicWriteOptions();
            }

            string directory = Path.GetDirectoryName(targetPath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            CleanupStaleTempFiles(targetPath);

            int retryCount = Math.Max(0, options.RetryCount);
            for (int attempt = 0; attempt <= retryCount; attempt++)
            {
                string tempPath = BuildTempPath(targetPath);
                try
                {
                    WriteTempFile(tempPath, data, options.VerifyLength);

                    if (File.Exists(targetPath))
                    {
                        if (options.CreateBackup)
                        {
                            BackupRotator.RotateBackups(targetPath, options.MaxBackups);
                        }

                        string backupPath = options.CreateBackup ? BackupRotator.GetPrimaryBackupPath(targetPath) : null;
                        ReplaceWithFallback(tempPath, targetPath, backupPath);
                    }
                    else
                    {
                        File.Move(tempPath, targetPath);
                    }

                    return;
                }
                catch (UnauthorizedAccessException)
                {
                    TryDelete(tempPath);
                    throw;
                }
                catch (IOException) when (attempt < retryCount)
                {
                    TryDelete(tempPath);
                    if (options.RetryDelayMs > 0)
                    {
                        Thread.Sleep(options.RetryDelayMs);
                    }
                }
                catch
                {
                    // Preserve temp for manual recovery on unexpected failures.
                    throw;
                }
            }

            throw new IOException("Atomic write failed after retry attempts were exhausted.");
        }

        public static string GetAutosavePathForFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath)) return string.Empty;
            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "OdyToolsV3", "Autosave");
            Directory.CreateDirectory(dir);
            string name = Sanitize(Path.GetFileName(filePath));
            int hash = StringComparer.OrdinalIgnoreCase.GetHashCode(Path.GetFullPath(filePath));
            return Path.Combine(dir, $"{name}.{hash:X8}.autosave");
        }

        public static void DeleteAutosaveFor(string filePath)
        {
            string autosavePath = GetAutosavePathForFile(filePath);
            TryDelete(autosavePath);
        }

        public static bool TryReadAutosaveIfNewer(string filePath, out byte[] data, out DateTime autosaveWriteUtc, out DateTime targetWriteUtc)
        {
            data = null;
            autosaveWriteUtc = DateTime.MinValue;
            targetWriteUtc = DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(filePath)) return false;
            string autosavePath = GetAutosavePathForFile(filePath);
            if (!File.Exists(autosavePath)) return false;

            autosaveWriteUtc = File.GetLastWriteTimeUtc(autosavePath);
            targetWriteUtc = File.Exists(filePath) ? File.GetLastWriteTimeUtc(filePath) : DateTime.MinValue;
            if (autosaveWriteUtc <= targetWriteUtc)
            {
                return false;
            }

            data = File.ReadAllBytes(autosavePath);
            return data.Length > 0;
        }

        private static void ReplaceWithFallback(string tempPath, string targetPath, string backupPath)
        {
            try
            {
                File.Replace(tempPath, targetPath, backupPath, true);
            }
            catch
            {
                if (!string.IsNullOrWhiteSpace(backupPath) && File.Exists(targetPath))
                {
                    File.Copy(targetPath, backupPath, overwrite: true);
                }
                File.Copy(tempPath, targetPath, overwrite: true);
                TryDelete(tempPath);
            }
        }

        private static void WriteTempFile(string tempPath, byte[] data, bool verifyLength)
        {
            using (var fs = new FileStream(tempPath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 81920, FileOptions.WriteThrough))
            {
                fs.Write(data, 0, data.Length);
                fs.Flush(flushToDisk: true);
            }

            if (!verifyLength)
            {
                return;
            }

            long written = new FileInfo(tempPath).Length;
            if (written != data.Length)
            {
                throw new IOException($"Temp file verification failed. Expected {data.Length} bytes, found {written} bytes.");
            }
        }

        private static string BuildTempPath(string targetPath)
        {
            string directory = Path.GetDirectoryName(targetPath);
            string fileName = Path.GetFileName(targetPath);
            return Path.Combine(directory ?? string.Empty, $"{fileName}.{Guid.NewGuid():N}.tmp");
        }

        private static void CleanupStaleTempFiles(string targetPath)
        {
            string directory = Path.GetDirectoryName(targetPath);
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                return;
            }

            string fileName = Path.GetFileName(targetPath);
            var staleFiles = Directory
                .EnumerateFiles(directory, fileName + ".*.tmp")
                .Where(path =>
                {
                    try
                    {
                        return File.GetLastWriteTimeUtc(path) < DateTime.UtcNow.AddDays(-1);
                    }
                    catch
                    {
                        return false;
                    }
                })
                .ToList();

            foreach (string stale in staleFiles)
            {
                TryDelete(stale);
            }
        }

        private static string Sanitize(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return "untitled";
            }

            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }

        private static void TryDelete(string path)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch
            {
                // Intentionally ignored.
            }
        }
    }
}
