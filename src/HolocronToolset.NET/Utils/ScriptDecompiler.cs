using System;
using System.IO;
using System.Text;
using CSharpKOTOR.Common;
using HolocronToolset.NET.Data;
using HolocronToolset.NET.Utils;

namespace HolocronToolset.NET.Utils
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/script_decompiler.py:19
    // Original: def ht_decompile_script(...):
    public static class ScriptDecompiler
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/script_decompiler.py:19-95
        // Original: def ht_decompile_script(compiled_bytes: bytes, installation_path: Path, *, tsl: bool) -> str:
        public static string HtDecompileScript(byte[] compiledBytes, string installationPath, bool tsl = false)
        {
            if (compiledBytes == null || compiledBytes.Length == 0)
            {
                return "";
            }

            var settings = new GlobalSettings();
            string extractPath = ScriptUtils.SetupExtractPath();

            // Use external decompiler if configured
            string ncsDecompilerPath = settings.GetValue<string>("NcsDecompilerPath", "");
            if (string.IsNullOrEmpty(ncsDecompilerPath) || !File.Exists(ncsDecompilerPath))
            {
                // Prompt user for decompiler - will be implemented when file dialogs are available
                throw new InvalidOperationException("NCS Decompiler has not been set or is invalid.");
            }

            // Decompile script - will be implemented when decompiler integration is available
            // For now, return empty string
            return "";
        }
    }
}
