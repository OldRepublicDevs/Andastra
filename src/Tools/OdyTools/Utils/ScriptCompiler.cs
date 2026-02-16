using System;
using System.Collections.Generic;
using BioWare.Resource.Formats.NCS;
using OdyTools.Data;
using Game = BioWare.Common.BioWareGame;

namespace OdyTools.Utils
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/script_compiler.py:28
    // Original: def ht_compile_script(...):
    public static class ScriptCompiler
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/script_compiler.py:28-73
        // Original: def ht_compile_script(source: str, installation_path: Path, *, tsl: bool) -> bytes | None:
        /// <param name="logMessage">Optional callback to receive compile progress/errors (e.g. for NSS editor Output panel).</param>
        public static byte[] HtCompileScript(string source, string installationPath, bool tsl = false, Action<string> logMessage = null)
        {
            if (string.IsNullOrEmpty(source))
            {
                logMessage?.Invoke("Compile skipped: source is empty.");
                return null;
            }

            string extractPath = ScriptUtils.SetupExtractPath();

            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/utils/script_compiler.py:62-64
            // Original: if os.name == "posix" or return_value == QMessageBox.StandardButton.Yes:
            // Original: log.debug("user chose Yes, compiling with builtin")
            // Original: return bytes(bytes_ncs(compile_nss(source, Game.K2 if tsl else Game.K1, library_lookup=[extract_path])))
            // Use built-in compiler (matching Python behavior on posix or when user chooses built-in)
            try
            {
                Game game = tsl ? Game.TSL : Game.K1;
                List<string> libraryLookup = new List<string>();
                if (!string.IsNullOrEmpty(extractPath))
                {
                    libraryLookup.Add(extractPath);
                }

                NCS ncs = NCSAuto.CompileNss(source, game, null, libraryLookup);
                if (ncs == null)
                {
                    logMessage?.Invoke("Compile returned no NCS output.");
                    return null;
                }

                logMessage?.Invoke("Compile succeeded.");
                return NCSAuto.BytesNcs(ncs);
            }
            catch (Exception ex)
            {
                string msg = $"Error compiling script: {ex.Message}";
                logMessage?.Invoke(msg);
                if (logMessage == null)
                {
                    System.Console.WriteLine($"Error compiling script: {ex}");
                }
                return null;
            }
        }
    }
}
