using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using BioWare;
using BioWare.Resource.Formats.NCS;
using BioWare.Resource.Formats.NCS.Decomp;
using OdyTools.Data;
using OdyTools.Utils;
using NcsFile = BioWare.Resource.Formats.NCS.Decomp.NcsFile;

namespace OdyTools.Utils
{
    public static class ScriptDecompiler
    {
        public static string HtDecompileScript(byte[] compiledBytes, string installationPath, bool tsl = false)
        {
            if (compiledBytes == null || compiledBytes.Length == 0)
            {
                return "";
            }

            var settings = new GlobalSettings();
            string extractPath = ScriptUtils.SetupExtractPath();

            string ncsDecompilerPath = settings.GetValue("NcsDecompilerPath", "");
            if (string.IsNullOrEmpty(ncsDecompilerPath) || !File.Exists(ncsDecompilerPath))
            {
                return DecompileUsingBuiltIn(compiledBytes, installationPath, tsl);
            }

            try
            {
                string externalResult = DecompileUsingExternal(ncsDecompilerPath, compiledBytes, extractPath);
                if (!string.IsNullOrWhiteSpace(externalResult))
                {
                    return externalResult;
                }
            }
            catch
            {
                // Fall through to built-in
            }

            return DecompileUsingBuiltIn(compiledBytes, installationPath, tsl);
        }

        private static string DecompileUsingExternal(string decompilerPath, byte[] ncsBytes, string workingDir)
        {
            string tempNcs = Path.Combine(workingDir, "temp_decompile_" + Guid.NewGuid().ToString("N").Substring(0, 8) + ".ncs");
            string tempNss = Path.ChangeExtension(tempNcs, ".nss");
            try
            {
                File.WriteAllBytes(tempNcs, ncsBytes);
                var startInfo = new ProcessStartInfo
                {
                    FileName = decompilerPath,
                    Arguments = "-d \"" + tempNcs + "\"",
                    WorkingDirectory = workingDir,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };
                using (var process = Process.Start(startInfo))
                {
                    if (process == null) return null;
                    process.WaitForExit(15000);
                    if (File.Exists(tempNss))
                    {
                        return File.ReadAllText(tempNss, Encoding.UTF8);
                    }
                    string stdout = process.StandardOutput?.ReadToEnd();
                    if (!string.IsNullOrWhiteSpace(stdout)) return stdout;
                }
            }
            finally
            {
                try { if (File.Exists(tempNcs)) File.Delete(tempNcs); } catch { }
                try { if (File.Exists(tempNss)) File.Delete(tempNss); } catch { }
            }
            return null;
        }

        private static string DecompileUsingBuiltIn(byte[] ncsData, string installationPath, bool tsl)
        {
            // Read NCS from bytes
            NCS ncs = NCSAuto.ReadNcs(ncsData);
            if (ncs == null)
            {
                throw new InvalidOperationException("Failed to read NCS data.");
            }

            // Create FileDecompiler
            FileDecompiler decompiler = null;

            // Try to load nwscript.nss from override folder for actions data
            if (!string.IsNullOrEmpty(installationPath))
            {
                string overridePath = Path.Combine(installationPath, "override");
                string nwscriptPath = Path.Combine(overridePath, "nwscript.nss");

                if (File.Exists(nwscriptPath))
                {
                    try
                    {
                        decompiler = new FileDecompiler(new NcsFile(nwscriptPath));
                    }
                    catch
                    {
                        // Failed to load nwscript.nss, will use empty actions
                    }
                }
            }

            // If nwscript.nss wasn't found, create decompiler without actions
            if (decompiler == null)
            {
                decompiler = new FileDecompiler();
            }

            // Decompile NCS object
            try
            {
                var scriptData = decompiler.DecompileNcsObject(ncs);
                if (scriptData == null)
                {
                    throw new InvalidOperationException("Decompilation failed: DecompileNcsObject returned null");
                }

                scriptData.GenerateCode();
                string result = scriptData.GetCode();

                if (string.IsNullOrEmpty(result))
                {
                    throw new InvalidOperationException("Decompilation failed: result is empty");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Decompilation failed: {ex.Message}");
            }
        }
    }
}
