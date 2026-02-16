using System;
using System.IO;
using OdyTools.Data;

namespace OdyTools.Utils
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/script_utils.py:34
    // Original: class NoOpRegistrySpoofer:
    public class NoOpRegistrySpoofer : IDisposable
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/script_utils.py:35-45
        // Original: def __enter__(self) -> Self: / def __exit__(...):
        public NoOpRegistrySpoofer()
        {
            Console.WriteLine("Enter NoOpRegistrySpoofer");
        }

        public void Dispose()
        {
            Console.WriteLine("Exit NoOpRegistrySpoofer");
        }
    }

    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/script_utils.py:48-68
    // Original: def setup_extract_path() -> Path:
    public static class ScriptUtils
    {
        public static string SetupExtractPath()
        {
            var settings = new GlobalSettings();
            string extractPath = settings.GetValue("ExtractPath", "");

            if (string.IsNullOrEmpty(extractPath) || !Directory.Exists(extractPath))
            {
                // Prompt user for directory - will be implemented when file dialogs are available
                // TODO: STUB - For now, use temp directory
                extractPath = Path.Combine(Path.GetTempPath(), "OdyTools");
                Directory.CreateDirectory(extractPath);
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

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/script_utils.py:71-109
        // Original: def handle_permission_error(...):
        public static void HandlePermissionError(NoOpRegistrySpoofer regSpoofer, string installationPath, Exception e)
        {
            // Handle permission errors - will be implemented when MessageBox is available
            Console.WriteLine($"Permission error: {e}");
        }
    }
}
