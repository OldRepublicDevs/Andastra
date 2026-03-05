using System;
using System.IO;
using OdyTools.Data;
using OdyTools.Utils;
using IconType = MsBox.Avalonia.Enums.Icon;

namespace OdyTools.Utils
{
    public class NoOpRegistrySpoofer : IDisposable
    {
        public NoOpRegistrySpoofer()
        {
            Console.WriteLine("Enter NoOpRegistrySpoofer");
        }

        public void Dispose()
        {
            Console.WriteLine("Exit NoOpRegistrySpoofer");
        }
    }

    public static class ScriptUtils
    {
        public static string SetupExtractPath()
        {
            var settings = new GlobalSettings();
            string extractPath = settings.GetValue("ExtractPath", "");

            if (string.IsNullOrEmpty(extractPath) || !Directory.Exists(extractPath))
            {
                // Use LocalApplicationData so the extract path is persistent and user-specific
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                extractPath = string.IsNullOrEmpty(appData)
                    ? Path.Combine(Path.GetTempPath(), "OdyTools")
                    : Path.Combine(appData, "OdyTools", "Extract");
                try
                {
                    Directory.CreateDirectory(extractPath);
                }
                catch
                {
                    extractPath = Path.Combine(Path.GetTempPath(), "OdyTools");
                    Directory.CreateDirectory(extractPath);
                }
            }

            settings.SetValue("ExtractPath", extractPath);
            return extractPath;
        }

        /// <summary>Returns path to nwscript.nss (or k1/tsl variant) for function list loading, or null if not found.</summary>
        public static string GetNwscriptPath(string installationPath, bool tsl)
        {
            if (string.IsNullOrEmpty(installationPath)) return null;
            string fileName = tsl ? "tsl_nwscript.nss" : "k1_nwscript.nss";
            string overridePath = Path.Combine(installationPath, "override", fileName);
            if (File.Exists(overridePath)) return overridePath;
            string fallback = Path.Combine(installationPath, "override", "nwscript.nss");
            return File.Exists(fallback) ? fallback : null;
        }

        public static void HandlePermissionError(NoOpRegistrySpoofer regSpoofer, string installationPath, Exception e)
        {
            Console.WriteLine($"Permission error: {e}");
        }

        /// <summary>
        /// Shows a message box for permission errors when parent window is provided; otherwise logs to console.
        /// </summary>
        public static void HandlePermissionError(NoOpRegistrySpoofer regSpoofer, string installationPath, Exception e, Avalonia.Controls.Window parent)
        {
            if (parent != null)
            {
                DialogHelper.ShowWindow(parent, "Permission error", "A permission error occurred: " + (e?.Message ?? "Unknown"), IconType.Warning);
            }
            else
            {
                Console.WriteLine($"Permission error: {e}");
            }
        }
    }
}
