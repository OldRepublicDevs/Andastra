using System;
using System.Collections.Generic;
using System.IO;

namespace BioWare.Extract.SaveData
{
    /// <summary>
    /// Centralized filesystem I/O for save folders.
    /// </summary>
    /// <remarks>
    /// This is intentionally format-agnostic: it only reads/writes known file names and returns raw bytes.
    /// Parsing/serialization is handled by the appropriate format layers (e.g., GFF/ERF/NFO).
    /// </remarks>
    public static class SaveFolderIO
    {
        public const string SaveNfoFileName = "savenfo.res";
        public const string SaveArchiveFileName = "savegame.sav";
        public const string ScreenshotFileName = "screen.tga";

        public static void EnsureDirectoryExists(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath)) throw new ArgumentException("Directory path cannot be null or empty.", nameof(directoryPath));
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }

        public static bool DirectoryExists(string directoryPath)
        {
            return !string.IsNullOrEmpty(directoryPath) && Directory.Exists(directoryPath);
        }

        public static IEnumerable<string> GetDirectories(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath) || !Directory.Exists(directoryPath))
            {
                yield break;
            }

            foreach (string dir in Directory.GetDirectories(directoryPath))
            {
                yield return dir;
            }
        }

        public static DateTime GetDirectoryLastWriteTime(string directoryPath)
        {
            return Directory.GetLastWriteTime(directoryPath);
        }

        public static void DeleteDirectoryRecursive(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath)) throw new ArgumentException("Directory path cannot be null or empty.", nameof(directoryPath));
            if (!Directory.Exists(directoryPath)) return;
            Directory.Delete(directoryPath, true);
        }

        public static void WriteSaveNfo(string saveDirectoryPath, byte[] nfoBytes)
        {
            WriteSaveFile(saveDirectoryPath, SaveNfoFileName, nfoBytes, nameof(nfoBytes));
        }

        public static byte[] ReadSaveNfo(string saveDirectoryPath)
        {
            return ReadSaveFile(saveDirectoryPath, SaveNfoFileName);
        }

        public static void WriteSaveArchive(string saveDirectoryPath, byte[] archiveBytes)
        {
            WriteSaveFile(saveDirectoryPath, SaveArchiveFileName, archiveBytes, nameof(archiveBytes));
        }

        public static byte[] ReadSaveArchive(string saveDirectoryPath)
        {
            return ReadSaveFile(saveDirectoryPath, SaveArchiveFileName);
        }

        public static void WriteScreenshot(string saveDirectoryPath, byte[] screenshotBytes)
        {
            WriteSaveFile(saveDirectoryPath, ScreenshotFileName, screenshotBytes, nameof(screenshotBytes));
        }

        public static void WriteBytesAtomic(string path, byte[] data)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("Path cannot be null or empty.", nameof(path));
            if (data == null) throw new ArgumentNullException(nameof(data));

            string directoryPath = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directoryPath))
            {
                EnsureDirectoryExists(directoryPath);
            }

            string fileName = Path.GetFileName(path);
            string tempDirectory = string.IsNullOrEmpty(directoryPath) ? Directory.GetCurrentDirectory() : directoryPath;
            string tempPath = Path.Combine(tempDirectory, fileName + ".tmp." + Guid.NewGuid().ToString("N"));

            try
            {
                using (var stream = new FileStream(tempPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    stream.Write(data, 0, data.Length);
                    stream.Flush(true);
                }

                if (File.Exists(path))
                {
                    string backupPath = path + ".bak";
                    if (File.Exists(backupPath))
                    {
                        File.Delete(backupPath);
                    }

                    File.Replace(tempPath, path, backupPath, true);

                    if (File.Exists(backupPath))
                    {
                        File.Delete(backupPath);
                    }
                }
                else
                {
                    File.Move(tempPath, path);
                }
            }
            catch
            {
                if (File.Exists(tempPath))
                {
                    try
                    {
                        File.Delete(tempPath);
                    }
                    catch
                    {
                    }
                }

                throw;
            }
        }

        public static byte[] ReadScreenshot(string saveDirectoryPath)
        {
            return ReadSaveFile(saveDirectoryPath, ScreenshotFileName);
        }

        private static void WriteSaveFile(string saveDirectoryPath, string fileName, byte[] fileBytes, string bytesParamName)
        {
            if (fileBytes == null) throw new ArgumentNullException(bytesParamName);
            EnsureDirectoryExists(saveDirectoryPath);

            string path = Path.Combine(saveDirectoryPath, fileName);
            WriteBytesAtomic(path, fileBytes);
        }

        private static byte[] ReadSaveFile(string saveDirectoryPath, string fileName)
        {
            if (string.IsNullOrEmpty(saveDirectoryPath)) return null;

            string path = Path.Combine(saveDirectoryPath, fileName);
            return File.Exists(path) ? File.ReadAllBytes(path) : null;
        }
    }
}


