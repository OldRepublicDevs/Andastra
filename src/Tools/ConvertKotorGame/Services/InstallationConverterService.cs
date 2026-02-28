using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.BIF;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.KEY;
using BioWare.Resource.Formats.RIM;
using BioWare.Resource.Formats.TLK;
using ConvertKotorGame.Models;

namespace ConvertKotorGame.Services
{
    public sealed class InstallationConverterService
    {
        private readonly ResourceConverter _resourceConverter = new ResourceConverter();

        private static readonly Dictionary<string, string> K1ToTslModuleAliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "end_m01aa", "001ebo" },
        };

        /// <summary>
        /// Stream directory mapping: K1→TSL uses target names; TSL→K1 uses inverse.
        /// Key = source dir name, Value = output dir name.
        /// </summary>
        private static readonly Dictionary<string, string> StreamDirMappingK1ToTsl = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "StreamWaves", "StreamVoice" },
            { "StreamVoice", "StreamVoice" },
            { "StreamMusic", "StreamMusic" },
            { "StreamSounds", "StreamSounds" },
        };

        private static readonly Dictionary<string, string> StreamDirMappingTslToK1 = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "StreamVoice", "StreamWaves" },
            { "StreamWaves", "StreamWaves" },
            { "StreamMusic", "StreamMusic" },
            { "StreamSounds", "StreamSounds" },
        };

        public string BuildOutputDirectory(string sourcePath, BioWareGame targetGame)
        {
            string parent = Directory.GetParent(sourcePath)?.FullName ?? sourcePath;
            string leaf = Path.GetFileName(sourcePath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
            string suffix = targetGame.IsK1() ? "k1" : "tsl";
            return Path.Combine(parent, leaf + "_converted_to_" + suffix);
        }

        public async Task<ConversionSummary> ConvertInstallationAsync(
            string sourcePath,
            string targetBasePath,
            BioWareGame sourceGame,
            BioWareGame targetGame,
            Action<string, LogLevelKind> log,
            Action<int, int> progress,
            CancellationToken cancellationToken,
            string requestedOutputPath = null)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(sourcePath) || !Directory.Exists(sourcePath))
                {
                    throw new DirectoryNotFoundException("Source installation path does not exist: " + (sourcePath ?? "(null)"));
                }

                if (string.IsNullOrWhiteSpace(targetBasePath) || !Directory.Exists(targetBasePath))
                {
                    throw new DirectoryNotFoundException("Target base installation path does not exist: " + (targetBasePath ?? "(null)"));
                }

                string outputPath = !string.IsNullOrWhiteSpace(requestedOutputPath)
                    ? requestedOutputPath
                    : BuildOutputDirectory(sourcePath, targetGame);
                var summary = new ConversionSummary { OutputPath = outputPath };

                Directory.CreateDirectory(outputPath);

                int totalSteps = EstimateTotalSteps(sourcePath, targetBasePath);
                int currentStep = 0;

                // Phase 1: Copy all target game files into output (exe, DLLs, chitin.key, data/, etc.). Overwrites; skips StreamWaves, StreamMusic, StreamVoice.
                log?.Invoke("Phase 1/7: Copying target game files (overwrite, skip stream dirs)...", LogLevelKind.Info);
                currentStep = CopyDirectoryRecursive(targetBasePath, outputPath, log, progress, currentStep, totalSteps);
                cancellationToken.ThrowIfCancellationRequested();

                // Phase 2: Patch dialog.tlk (target base, overlay source entries) and copy streams/lips as-is.
                log?.Invoke("Phase 2/7: Patching dialog.tlk and copying streams/lips...", LogLevelKind.Info);
                currentStep = PatchDialogTlkAndCopyStreams(sourcePath, targetBasePath, outputPath, sourceGame, targetGame, log, progress, currentStep, totalSteps);
                cancellationToken.ThrowIfCancellationRequested();

                // Phase 3: Extract and convert source BIF resources → Override/.
                log?.Invoke("Phase 3/7: Extracting and converting BIF resources to Override/...", LogLevelKind.Info);
                string overridePath = Path.Combine(outputPath, "Override");
                Directory.CreateDirectory(overridePath);
                currentStep = ExtractAndConvertBifs(sourcePath, overridePath, sourceGame, targetGame, summary, log, progress, currentStep, totalSteps, cancellationToken);

                // Phase 4: Extract patch.erf (source root-level override archive) → Override/.
                string patchErfPath = FindFile(sourcePath, "patch.erf");
                if (patchErfPath != null)
                {
                    log?.Invoke("Phase 4/7: Extracting patch.erf to Override/...", LogLevelKind.Info);
                    currentStep = ExtractErfToOverride(patchErfPath, overridePath, sourceGame, targetGame, summary, log, progress, currentStep, totalSteps, cancellationToken);
                }
                else
                {
                    log?.Invoke("Phase 4/7: No patch.erf found, skipping.", LogLevelKind.Trace);
                }

                // Phase 5: Extract source TexturePacks/*.erf → Override/ (same as BIFs).
                string sourceTexturePacks = FindSubdirectory(sourcePath, "TexturePacks");
                if (sourceTexturePacks != null)
                {
                    log?.Invoke("Phase 5/7: Extracting TexturePacks to Override/...", LogLevelKind.Info);
                    currentStep = ExtractTexturePacksToOverride(sourceTexturePacks, overridePath, sourceGame, targetGame, summary, log, progress, currentStep, totalSteps, cancellationToken);
                }
                else
                {
                    log?.Invoke("Phase 5/7: No TexturePacks found, skipping.", LogLevelKind.Trace);
                }

                // Phase 6: Convert source modules/ → output modules/.
                log?.Invoke("Phase 6/7: Converting module archives...", LogLevelKind.Info);
                string outputModulesPath = Path.Combine(outputPath, "modules");
                Directory.CreateDirectory(outputModulesPath);
                currentStep = ConvertModules(sourcePath, outputModulesPath, sourceGame, targetGame, summary, log, progress, currentStep, totalSteps, cancellationToken);

                // Also convert source Override/ loose files → output Override/.
                log?.Invoke("Phase 6/7 (cont): Converting source Override/ files...", LogLevelKind.Info);
                currentStep = ConvertSourceOverride(sourcePath, overridePath, sourceGame, targetGame, summary, log, progress, currentStep, totalSteps, cancellationToken);

                // Phase 7: Post-processing (INI merge, module aliases, reports).
                log?.Invoke("Phase 7/7: Post-processing...", LogLevelKind.Info);
                PostProcess(sourcePath, targetBasePath, outputPath, sourceGame, targetGame, summary, log);

                if (summary.BlockedFiles.Count > 0)
                {
                    WriteBlockedConversionReport(outputPath, summary, sourceGame, targetGame, log);
                }

                progress?.Invoke(totalSteps, totalSteps);
                log?.Invoke("Done. Output: " + outputPath, LogLevelKind.Info);
                return summary;
            }, cancellationToken);
        }

        private static int EstimateTotalSteps(string sourcePath, string targetBasePath)
        {
            int steps = 0;

            // Phase 1 (CopyDirectoryRecursive): all target files recursively (overwrite; skips StreamWaves, StreamMusic, StreamVoice).
            steps += Directory.GetFiles(targetBasePath, "*", SearchOption.AllDirectories).Length;

            // Source BIF files.
            string sourceData = FindSubdirectory(sourcePath, "data");
            if (sourceData != null)
            {
                steps += Directory.GetFiles(sourceData, "*.bif", SearchOption.AllDirectories).Length;
                steps += Directory.GetFiles(sourceData, "*.bzf", SearchOption.AllDirectories).Length;
            }

            // Source modules.
            string sourceModules = FindSubdirectory(sourcePath, "modules");
            if (sourceModules != null)
            {
                steps += Directory.GetFiles(sourceModules, "*", SearchOption.TopDirectoryOnly).Length;
            }

            // Source Override.
            string sourceOverride = FindSubdirectory(sourcePath, "Override");
            if (sourceOverride != null)
            {
                steps += Directory.GetFiles(sourceOverride, "*", SearchOption.AllDirectories).Length;
            }

            // Source stream/lips directories.
            foreach (string dirName in new[] { "lips", "StreamMusic", "StreamSounds", "StreamVoice", "StreamWaves" })
            {
                string dir = FindSubdirectory(sourcePath, dirName);
                if (dir != null)
                {
                    steps += Directory.GetFiles(dir, "*", SearchOption.AllDirectories).Length;
                }
            }

            // Source TexturePacks .erf files.
            string sourceTexturePacks = FindSubdirectory(sourcePath, "TexturePacks");
            if (sourceTexturePacks != null)
            {
                steps += Directory.GetFiles(sourceTexturePacks, "*.erf", SearchOption.TopDirectoryOnly).Length;
            }

            return steps > 0 ? steps : 1;
        }

        private static readonly string[] CopyDirectoryRecursiveSkipFolders = { "StreamWaves", "StreamMusic", "StreamVoice" };

        private static int CopyDirectoryRecursive(
            string sourceDir,
            string destDir,
            Action<string, LogLevelKind> log,
            Action<int, int> progress,
            int currentStep,
            int totalSteps)
        {
            foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                currentStep++;
                progress?.Invoke(currentStep, totalSteps);

                string relative = file.Substring(sourceDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                string relativeNorm = relative.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                bool skip = false;
                foreach (string folder in CopyDirectoryRecursiveSkipFolders)
                {
                    if (relativeNorm.StartsWith(folder + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                        || relativeNorm.Equals(folder, StringComparison.OrdinalIgnoreCase))
                    {
                        skip = true;
                        break;
                    }
                }
                if (skip) continue;
                string dest = Path.Combine(destDir, relative);

                string destParent = Path.GetDirectoryName(dest);
                if (!string.IsNullOrEmpty(destParent))
                {
                    Directory.CreateDirectory(destParent);
                }
                File.Copy(file, dest, overwrite: true);
                log?.Invoke("Copied " + relative, LogLevelKind.Trace);
            }

            return currentStep;
        }

        private static int PatchDialogTlkAndCopyStreams(
            string sourcePath,
            string targetBasePath,
            string outputPath,
            BioWareGame sourceGame,
            BioWareGame targetGame,
            Action<string, LogLevelKind> log,
            Action<int, int> progress,
            int currentStep,
            int totalSteps)
        {
            // Patch dialog.tlk: target as base, overlay source entries (replace target entries with source at same indices).
            string targetTlk = FindFile(targetBasePath, "dialog.tlk");
            string sourceTlk = FindFile(sourcePath, "dialog.tlk");
            if (targetTlk != null && sourceTlk != null)
            {
                try
                {
                    TLK baseTlk = TLKAuto.ReadTlk(targetTlk);
                    TLK sourceTlkData = TLKAuto.ReadTlk(sourceTlk);
                    int count = Math.Min(baseTlk.Count, sourceTlkData.Count);
                    for (int i = 0; i < count; i++)
                    {
                        var srcEntry = sourceTlkData.Get(i);
                        if (srcEntry != null)
                        {
                            baseTlk.Replace(i, srcEntry.Text, srcEntry.Voiceover?.ToString() ?? "");
                        }
                    }
                    if (sourceTlkData.Count > baseTlk.Count)
                    {
                        baseTlk.Resize(sourceTlkData.Count);
                        for (int i = count; i < sourceTlkData.Count; i++)
                        {
                            var srcEntry = sourceTlkData.Get(i);
                            if (srcEntry != null)
                            {
                                baseTlk.Replace(i, srcEntry.Text, srcEntry.Voiceover?.ToString() ?? "");
                            }
                        }
                    }
                    TLKAuto.WriteTlk(baseTlk, Path.Combine(outputPath, "dialog.tlk"), ResourceType.TLK);
                    log?.Invoke("Patched dialog.tlk (target base, source entries overlaid).", LogLevelKind.Info);
                }
                catch (Exception ex)
                {
                    log?.Invoke("Failed to patch dialog.tlk, copying source: " + ex.Message, LogLevelKind.Warning);
                    File.Copy(sourceTlk, Path.Combine(outputPath, "dialog.tlk"), true);
                }
            }
            else if (sourceTlk != null)
            {
                File.Copy(sourceTlk, Path.Combine(outputPath, "dialog.tlk"), true);
                log?.Invoke("Copied source dialog.tlk (no target to patch).", LogLevelKind.Trace);
            }

            // Stream directory mapping: K1→TSL uses StreamDirMappingK1ToTsl; TSL→K1 uses inverse.
            var mapping = targetGame.IsK1() ? StreamDirMappingTslToK1 : StreamDirMappingK1ToTsl;

            foreach (string dirName in new[] { "lips", "StreamMusic", "StreamSounds", "StreamVoice", "StreamWaves" })
            {
                string sourceDir = FindSubdirectory(sourcePath, dirName);
                if (sourceDir == null)
                {
                    continue;
                }

                string outDirName;
                if (!mapping.TryGetValue(dirName, out outDirName))
                {
                    outDirName = dirName;
                }

                string outputDir = Path.Combine(outputPath, outDirName);
                string[] files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    currentStep++;
                    progress?.Invoke(currentStep, totalSteps);

                    string relative = GetRelativePath(sourceDir, file);
                    string dest = Path.Combine(outputDir, relative);
                    EnsureParentExists(dest);
                    File.Copy(file, dest, true);
                }

                if (files.Length > 0)
                {
                    log?.Invoke("Copied " + dirName + "/ → " + outDirName + "/ (" + files.Length + " files).", LogLevelKind.Trace);
                }
            }

            return currentStep;
        }

        private int ExtractAndConvertBifs(
            string sourcePath,
            string overridePath,
            BioWareGame sourceGame,
            BioWareGame targetGame,
            ConversionSummary summary,
            Action<string, LogLevelKind> log,
            Action<int, int> progress,
            int currentStep,
            int totalSteps,
            CancellationToken token)
        {
            Dictionary<int, KeyEntry> keyEntryById = LoadKeyEntryMap(sourcePath, log);

            string dataDir = FindSubdirectory(sourcePath, "data");
            if (dataDir == null)
            {
                log?.Invoke("No data/ directory found in source installation; BIF extraction skipped.", LogLevelKind.Warning);
                return currentStep;
            }

            string[] bifFiles = Directory.GetFiles(dataDir, "*.bif", SearchOption.AllDirectories);
            string[] bzfFiles = Directory.GetFiles(dataDir, "*.bzf", SearchOption.AllDirectories);
            string[] allBifs = bifFiles.Concat(bzfFiles).ToArray();

            foreach (string bifPath in allBifs)
            {
                currentStep++;
                progress?.Invoke(currentStep, totalSteps);
                token.ThrowIfCancellationRequested();

                string bifName = Path.GetFileName(bifPath);
                try
                {
                    var reader = new BIFBinaryReader(bifPath);
                    BIF bif = reader.Load();
                    var resources = bif.Resources;

                    var byKey = new Dictionary<string, BIFResource>(StringComparer.OrdinalIgnoreCase);
                    foreach (BIFResource res in resources)
                    {
                        string rr = ResolveBifResRef(res, keyEntryById);
                        string k = rr.ToLowerInvariant() + "|" + res.ResType.Extension;
                        byKey[k] = res;
                    }

                    var handled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    foreach (BIFResource res in resources)
                    {
                        token.ThrowIfCancellationRequested();
                        string rr = ResolveBifResRef(res, keyEntryById);
                        string k = rr.ToLowerInvariant() + "|" + res.ResType.Extension;
                        if (handled.Contains(k))
                        {
                            continue;
                        }
                        handled.Add(k);

                        if (res.ResType == ResourceType.MDL)
                        {
                            string mdxK = rr.ToLowerInvariant() + "|mdx";
                            BIFResource mdxRes;
                            if (byKey.TryGetValue(mdxK, out mdxRes))
                            {
                                handled.Add(mdxK);
                                WriteMdlMdxPairToOverride(rr, res.Data, mdxRes.Data, overridePath, targetGame, summary, log, bifName);
                                continue;
                            }
                        }

                        if (res.ResType == ResourceType.MDX)
                        {
                            string mdlK = rr.ToLowerInvariant() + "|mdl";
                            if (byKey.ContainsKey(mdlK))
                            {
                                continue;
                            }
                        }

                        WriteResourceToOverride(rr, res.ResType, res.Data, overridePath, sourceGame, targetGame, summary, log, bifName);
                    }

                    log?.Invoke("Extracted BIF " + bifName + " (" + resources.Count + " resources).", LogLevelKind.Trace);
                }
                catch (Exception ex)
                {
                    log?.Invoke("Failed to process BIF " + bifName + ": " + ex.Message, LogLevelKind.Error);
                }
            }

            return currentStep;
        }

        private int ExtractErfToOverride(
            string erfPath,
            string overridePath,
            BioWareGame sourceGame,
            BioWareGame targetGame,
            ConversionSummary summary,
            Action<string, LogLevelKind> log,
            Action<int, int> progress,
            int currentStep,
            int totalSteps,
            CancellationToken token)
        {
            currentStep++;
            progress?.Invoke(currentStep, totalSteps);

            try
            {
                ERF erf = ERFAuto.ReadErf(erfPath);
                var byResRef = erf.ToDictionary(
                    r => r.ResRef.ToString().ToLowerInvariant() + "|" + r.ResType.Extension,
                    r => r,
                    StringComparer.OrdinalIgnoreCase);
                var handled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (ERFResource res in erf)
                {
                    token.ThrowIfCancellationRequested();
                    string k = res.ResRef.ToString().ToLowerInvariant() + "|" + res.ResType.Extension;
                    if (handled.Contains(k))
                    {
                        continue;
                    }
                    handled.Add(k);

                    string rr = res.ResRef.ToString();

                    if (res.ResType == ResourceType.MDL)
                    {
                        string mdxK = rr.ToLowerInvariant() + "|mdx";
                        ERFResource mdxRes;
                        if (byResRef.TryGetValue(mdxK, out mdxRes))
                        {
                            handled.Add(mdxK);
                            WriteMdlMdxPairToOverride(rr, res.Data, mdxRes.Data, overridePath, targetGame, summary, log, "patch.erf");
                            continue;
                        }
                    }

                    if (res.ResType == ResourceType.MDX)
                    {
                        string mdlK = rr.ToLowerInvariant() + "|mdl";
                        if (byResRef.ContainsKey(mdlK))
                        {
                            continue;
                        }
                    }

                    WriteResourceToOverride(rr, res.ResType, res.Data, overridePath, sourceGame, targetGame, summary, log, "patch.erf");
                }

                log?.Invoke("Extracted patch.erf (" + erf.Count() + " resources) to Override/.", LogLevelKind.Trace);
            }
            catch (Exception ex)
            {
                log?.Invoke("Failed to extract patch.erf: " + ex.Message, LogLevelKind.Error);
            }

            return currentStep;
        }

        private int ExtractTexturePacksToOverride(
            string texturePacksDir,
            string overridePath,
            BioWareGame sourceGame,
            BioWareGame targetGame,
            ConversionSummary summary,
            Action<string, LogLevelKind> log,
            Action<int, int> progress,
            int currentStep,
            int totalSteps,
            CancellationToken token)
        {
            string[] erfFiles = Directory.GetFiles(texturePacksDir, "*.erf", SearchOption.TopDirectoryOnly);

            foreach (string erfPath in erfFiles)
            {
                currentStep++;
                progress?.Invoke(currentStep, totalSteps);
                token.ThrowIfCancellationRequested();

                string erfName = Path.GetFileName(erfPath);
                try
                {
                    ERF erf = ERFAuto.ReadErf(erfPath);
                    var byResRef = erf.ToDictionary(
                        r => r.ResRef.ToString().ToLowerInvariant() + "|" + r.ResType.Extension,
                        r => r,
                        StringComparer.OrdinalIgnoreCase);
                    var handled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                    foreach (ERFResource res in erf)
                    {
                        token.ThrowIfCancellationRequested();
                        string k = res.ResRef.ToString().ToLowerInvariant() + "|" + res.ResType.Extension;
                        if (handled.Contains(k))
                        {
                            continue;
                        }
                        handled.Add(k);

                        string rr = res.ResRef.ToString();

                        if (res.ResType == ResourceType.MDL)
                        {
                            string mdxK = rr.ToLowerInvariant() + "|mdx";
                            ERFResource mdxRes;
                            if (byResRef.TryGetValue(mdxK, out mdxRes))
                            {
                                handled.Add(mdxK);
                                WriteMdlMdxPairToOverride(rr, res.Data, mdxRes.Data, overridePath, targetGame, summary, log, erfName);
                                continue;
                            }
                        }

                        if (res.ResType == ResourceType.MDX)
                        {
                            string mdlK = rr.ToLowerInvariant() + "|mdl";
                            if (byResRef.ContainsKey(mdlK))
                            {
                                continue;
                            }
                        }

                        WriteResourceToOverride(rr, res.ResType, res.Data, overridePath, sourceGame, targetGame, summary, log, erfName);
                    }

                    log?.Invoke("Extracted TexturePack " + erfName + " (" + erf.Count() + " resources) to Override/.", LogLevelKind.Trace);
                }
                catch (Exception ex)
                {
                    log?.Invoke("Failed to extract TexturePack " + erfName + ": " + ex.Message, LogLevelKind.Error);
                }
            }

            return currentStep;
        }

        private int ConvertModules(
            string sourcePath,
            string outputModulesPath,
            BioWareGame sourceGame,
            BioWareGame targetGame,
            ConversionSummary summary,
            Action<string, LogLevelKind> log,
            Action<int, int> progress,
            int currentStep,
            int totalSteps,
            CancellationToken token)
        {
            string sourceModules = FindSubdirectory(sourcePath, "modules");
            if (sourceModules == null)
            {
                log?.Invoke("No modules/ directory found in source installation.", LogLevelKind.Warning);
                return currentStep;
            }

            string[] moduleFiles = Directory.GetFiles(sourceModules, "*", SearchOption.TopDirectoryOnly);

            foreach (string moduleFile in moduleFiles)
            {
                currentStep++;
                progress?.Invoke(currentStep, totalSteps);
                token.ThrowIfCancellationRequested();

                string fileName = Path.GetFileName(moduleFile);
                string ext = Path.GetExtension(moduleFile).TrimStart('.').ToLowerInvariant();
                string targetFile = Path.Combine(outputModulesPath, fileName);

                try
                {
                    if (IsErfLikeExtension(ext))
                    {
                        summary.ContainersProcessed++;
                        CountContainer(summary, ext);
                        ConvertErfArchive(moduleFile, fileName, targetFile, sourceGame, targetGame, summary, log, token);
                    }
                    else if (ext == "rim")
                    {
                        summary.ContainersProcessed++;
                        CountContainer(summary, ext);
                        ConvertRimArchive(moduleFile, fileName, targetFile, sourceGame, targetGame, summary, log, token);
                    }
                    else
                    {
                        // Copy other module files as-is.
                        File.Copy(moduleFile, targetFile, true);
                    }
                }
                catch (Exception ex)
                {
                    log?.Invoke("Failed module " + fileName + ": " + ex.Message, LogLevelKind.Error);
                    File.Copy(moduleFile, targetFile, true);
                }
            }

            log?.Invoke("Module conversion done (" + moduleFiles.Length + " files).", LogLevelKind.Trace);
            return currentStep;
        }

        private int ConvertSourceOverride(
            string sourcePath,
            string outputOverridePath,
            BioWareGame sourceGame,
            BioWareGame targetGame,
            ConversionSummary summary,
            Action<string, LogLevelKind> log,
            Action<int, int> progress,
            int currentStep,
            int totalSteps,
            CancellationToken token)
        {
            string sourceOverride = FindSubdirectory(sourcePath, "Override");
            if (sourceOverride == null)
            {
                log?.Invoke("No Override/ directory found in source installation.", LogLevelKind.Trace);
                return currentStep;
            }

            string[] files = Directory.GetFiles(sourceOverride, "*", SearchOption.AllDirectories);
            var handled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string file in files)
            {
                currentStep++;
                progress?.Invoke(currentStep, totalSteps);
                token.ThrowIfCancellationRequested();

                ResourceIdentifier identifier = ResourceIdentifier.FromPath(file);

                // MDL/MDX pair handling.
                if (identifier.ResType == ResourceType.MDL)
                {
                    string mdxPath = Path.Combine(Path.GetDirectoryName(file) ?? "", identifier.ResName + ".mdx");
                    if (File.Exists(mdxPath) && !handled.Contains(mdxPath.ToLowerInvariant()))
                    {
                        handled.Add(file.ToLowerInvariant());
                        handled.Add(mdxPath.ToLowerInvariant());
                        byte[] mdlData = File.ReadAllBytes(file);
                        byte[] mdxData = File.ReadAllBytes(mdxPath);
                        WriteMdlMdxPairToOverride(identifier.ResName, mdlData, mdxData, outputOverridePath, targetGame, summary, log, "Override");
                        continue;
                    }
                }

                if (identifier.ResType == ResourceType.MDX)
                {
                    string mdlPath = Path.Combine(Path.GetDirectoryName(file) ?? "", identifier.ResName + ".mdl");
                    if (File.Exists(mdlPath))
                    {
                        continue;
                    }
                }

                if (handled.Contains(file.ToLowerInvariant()))
                {
                    continue;
                }
                handled.Add(file.ToLowerInvariant());

                if (identifier.ResType == null || identifier.ResType.IsInvalid)
                {
                    string dest = Path.Combine(outputOverridePath, Path.GetFileName(file));
                    File.Copy(file, dest, true);
                    summary.CopiedCount++;
                    continue;
                }

                byte[] data = File.ReadAllBytes(file);
                WriteResourceToOverride(identifier.ResName, identifier.ResType, data, outputOverridePath, sourceGame, targetGame, summary, log, "Override");
            }

            if (files.Length > 0)
            {
                log?.Invoke("Converted source Override/ (" + files.Length + " files).", LogLevelKind.Trace);
            }

            return currentStep;
        }

        private void WriteResourceToOverride(
            string resref,
            ResourceType resType,
            byte[] data,
            string overridePath,
            BioWareGame sourceGame,
            BioWareGame targetGame,
            ConversionSummary summary,
            Action<string, LogLevelKind> log,
            string containerName)
        {
            CountSeen(summary, resType.Extension);

            if (data == null || data.Length == 0)
            {
                log?.Invoke("Skipping empty resource " + resref + "." + resType.Extension + " from " + containerName, LogLevelKind.Trace);
                return;
            }

            try
            {
                bool converted;
                byte[] output = _resourceConverter.ConvertResourceData(resref, resType, data, sourceGame, targetGame, log, out converted);

                File.WriteAllBytes(Path.Combine(overridePath, resref + "." + resType.Extension), output);

                if (converted)
                {
                    summary.ConvertedCount++;
                    CountConverted(summary, resType.Extension);
                }
                else
                {
                    summary.CopiedCount++;
                }
            }
            catch (ConversionBlockedException ex)
            {
                summary.FailedCount++;
                CountFailed(summary, resType.Extension);
                summary.BlockedFiles.Add((containerName + "::" + resref + "." + resType.Extension, ex.Message));
                log?.Invoke("Blocked " + resref + "." + resType.Extension + " (" + containerName + "): " + ex.Message, LogLevelKind.Error);

                byte[] fallback = ex.FallbackData ?? data;
                File.WriteAllBytes(Path.Combine(overridePath, resref + "." + resType.Extension), fallback);
                summary.CopiedCount++;
            }
            catch (Exception ex)
            {
                summary.FailedCount++;
                CountFailed(summary, resType.Extension);
                log?.Invoke("Failed " + resref + "." + resType.Extension + " (" + containerName + "): " + ex.Message, LogLevelKind.Warning);

                File.WriteAllBytes(Path.Combine(overridePath, resref + "." + resType.Extension), data);
                summary.CopiedCount++;
            }
        }

        private void WriteMdlMdxPairToOverride(
            string resref,
            byte[] mdlData,
            byte[] mdxData,
            string overridePath,
            BioWareGame targetGame,
            ConversionSummary summary,
            Action<string, LogLevelKind> log,
            string containerName)
        {
            CountSeen(summary, "mdl");
            CountSeen(summary, "mdx");

            byte[] mdl = mdlData ?? Array.Empty<byte>();
            byte[] mdx = mdxData ?? Array.Empty<byte>();

            string error;
            byte[] outMdl;
            byte[] outMdx;
            bool ok = _resourceConverter.TryConvertMdlPair(mdl, mdx, targetGame, out outMdl, out outMdx, out error);

            File.WriteAllBytes(Path.Combine(overridePath, resref + ".mdl"), ok ? outMdl : mdl);
            File.WriteAllBytes(Path.Combine(overridePath, resref + ".mdx"), ok ? outMdx : mdx);

            if (ok)
            {
                summary.ConvertedCount += 2;
                CountConverted(summary, "mdl");
                CountConverted(summary, "mdx");
            }
            else
            {
                summary.CopiedCount += 2;
                bool emptyOrNull = error != null && error.IndexOf("empty or null", StringComparison.OrdinalIgnoreCase) >= 0;
                log?.Invoke("MDL/MDX fallback copy " + resref + " (" + containerName + "): " + error, emptyOrNull ? LogLevelKind.Trace : LogLevelKind.Warning);
            }
        }

        private void ConvertErfArchive(
            string sourceFile,
            string containerName,
            string targetFile,
            BioWareGame sourceGame,
            BioWareGame targetGame,
            ConversionSummary summary,
            Action<string, LogLevelKind> log,
            CancellationToken token)
        {
            ERF source = ERFAuto.ReadErf(sourceFile);
            var output = new ERF(source.ErfType, source.IsSaveErf);

            var byResRef = source.ToDictionary(
                r => r.ResRef.ToString().ToLowerInvariant() + "|" + r.ResType.Extension,
                r => r,
                StringComparer.OrdinalIgnoreCase);
            var handled = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (ERFResource resource in source)
            {
                token.ThrowIfCancellationRequested();
                string key = resource.ResRef.ToString().ToLowerInvariant() + "|" + resource.ResType.Extension;
                if (handled.Contains(key))
                {
                    continue;
                }
                handled.Add(key);

                if (resource.ResType == ResourceType.MDL)
                {
                    string mdxKey = resource.ResRef.ToString().ToLowerInvariant() + "|mdx";
                    ERFResource mdxResource;
                    if (byResRef.TryGetValue(mdxKey, out mdxResource))
                    {
                        handled.Add(mdxKey);
                        CountSeen(summary, "mdl");
                        CountSeen(summary, "mdx");

                        byte[] outMdl;
                        byte[] outMdx;
                        string error;
                        bool ok = _resourceConverter.TryConvertMdlPair(resource.Data, mdxResource.Data, targetGame, out outMdl, out outMdx, out error);

                        output.SetData(resource.ResRef.ToString(), ResourceType.MDL, ok ? outMdl : resource.Data);
                        output.SetData(mdxResource.ResRef.ToString(), ResourceType.MDX, ok ? outMdx : mdxResource.Data);

                        if (ok)
                        {
                            summary.ConvertedCount += 2;
                            CountConverted(summary, "mdl");
                            CountConverted(summary, "mdx");
                        }
                        else
                        {
                            summary.CopiedCount += 2;
                            bool emptyOrNull = error != null && error.IndexOf("empty or null", StringComparison.OrdinalIgnoreCase) >= 0;
                            log?.Invoke("ERF MDL/MDX fallback copy " + resource.ResRef + " (" + containerName + "): " + error, emptyOrNull ? LogLevelKind.Trace : LogLevelKind.Warning);
                        }
                        continue;
                    }
                }

                if (resource.ResType == ResourceType.MDX)
                {
                    string mdlKey = resource.ResRef.ToString().ToLowerInvariant() + "|mdl";
                    if (byResRef.ContainsKey(mdlKey))
                    {
                        continue;
                    }
                }

                CountSeen(summary, resource.ResType.Extension);
                try
                {
                    bool wasConverted;
                    byte[] converted = _resourceConverter.ConvertResourceData(
                        resource.ResRef.ToString(), resource.ResType, resource.Data,
                        sourceGame, targetGame, log, out wasConverted);
                    output.SetData(resource.ResRef.ToString(), resource.ResType, converted);

                    if (wasConverted)
                    {
                        summary.ConvertedCount++;
                        CountConverted(summary, resource.ResType.Extension);
                    }
                    else
                    {
                        summary.CopiedCount++;
                    }
                }
                catch (ConversionBlockedException ex)
                {
                    summary.FailedCount++;
                    CountFailed(summary, resource.ResType.Extension);
                    summary.BlockedFiles.Add((containerName + "::" + resource.ResRef + "." + resource.ResType.Extension, ex.Message));
                    log?.Invoke("Blocked " + resource.ResRef + "." + resource.ResType.Extension + " (" + containerName + "): " + ex.Message, LogLevelKind.Error);
                    output.SetData(resource.ResRef.ToString(), resource.ResType, ex.FallbackData ?? resource.Data);
                    summary.CopiedCount++;
                }
                catch (Exception ex)
                {
                    summary.FailedCount++;
                    CountFailed(summary, resource.ResType.Extension);
                    log?.Invoke("Failed " + resource.ResRef + "." + resource.ResType.Extension + " (" + containerName + "): " + ex.Message, LogLevelKind.Warning);
                    output.SetData(resource.ResRef.ToString(), resource.ResType, resource.Data);
                    summary.CopiedCount++;
                }
            }

            EnsureParentExists(targetFile);
            ERFAuto.WriteErf(output, targetFile, ResourceType.FromExtension(Path.GetExtension(sourceFile)));
        }

        private void ConvertRimArchive(
            string sourceFile,
            string containerName,
            string targetFile,
            BioWareGame sourceGame,
            BioWareGame targetGame,
            ConversionSummary summary,
            Action<string, LogLevelKind> log,
            CancellationToken token)
        {
            RIM source = RIMAuto.ReadRim(sourceFile);
            var output = new RIM();
            var all = source.GetResources();

            var byResRef = all.ToDictionary(
                r => r.ResRef.ToString().ToLowerInvariant() + "|" + r.ResType.Extension,
                r => r,
                StringComparer.OrdinalIgnoreCase);

            foreach (RIMResource resource in all)
            {
                token.ThrowIfCancellationRequested();

                if (resource.ResType == ResourceType.MDX)
                {
                    string mdlKey = resource.ResRef.ToString().ToLowerInvariant() + "|mdl";
                    if (byResRef.ContainsKey(mdlKey))
                    {
                        continue;
                    }
                }

                if (resource.ResType == ResourceType.MDL)
                {
                    string mdxKey = resource.ResRef.ToString().ToLowerInvariant() + "|mdx";
                    RIMResource mdxResource;
                    if (byResRef.TryGetValue(mdxKey, out mdxResource))
                    {
                        CountSeen(summary, "mdl");
                        CountSeen(summary, "mdx");

                        byte[] outMdl;
                        byte[] outMdx;
                        string error;
                        bool ok = _resourceConverter.TryConvertMdlPair(resource.Data, mdxResource.Data, targetGame, out outMdl, out outMdx, out error);

                        output.SetData(resource.ResRef.ToString(), ResourceType.MDL, ok ? outMdl : resource.Data);
                        output.SetData(mdxResource.ResRef.ToString(), ResourceType.MDX, ok ? outMdx : mdxResource.Data);

                        if (ok)
                        {
                            summary.ConvertedCount += 2;
                            CountConverted(summary, "mdl");
                            CountConverted(summary, "mdx");
                        }
                        else
                        {
                            summary.CopiedCount += 2;
                            bool emptyOrNull = error != null && error.IndexOf("empty or null", StringComparison.OrdinalIgnoreCase) >= 0;
                            log?.Invoke("RIM MDL/MDX fallback copy " + resource.ResRef + " (" + containerName + "): " + error, emptyOrNull ? LogLevelKind.Trace : LogLevelKind.Warning);
                        }
                        continue;
                    }
                }

                CountSeen(summary, resource.ResType.Extension);
                try
                {
                    bool wasConverted;
                    byte[] converted = _resourceConverter.ConvertResourceData(
                        resource.ResRef.ToString(), resource.ResType, resource.Data,
                        sourceGame, targetGame, log, out wasConverted);
                    output.SetData(resource.ResRef.ToString(), resource.ResType, converted);

                    if (wasConverted)
                    {
                        summary.ConvertedCount++;
                        CountConverted(summary, resource.ResType.Extension);
                    }
                    else
                    {
                        summary.CopiedCount++;
                    }
                }
                catch (ConversionBlockedException ex)
                {
                    summary.FailedCount++;
                    CountFailed(summary, resource.ResType.Extension);
                    summary.BlockedFiles.Add((containerName + "::" + resource.ResRef + "." + resource.ResType.Extension, ex.Message));
                    log?.Invoke("Blocked " + resource.ResRef + "." + resource.ResType.Extension + " (" + containerName + "): " + ex.Message, LogLevelKind.Error);
                    output.SetData(resource.ResRef.ToString(), resource.ResType, ex.FallbackData ?? resource.Data);
                    summary.CopiedCount++;
                }
                catch (Exception ex)
                {
                    summary.FailedCount++;
                    CountFailed(summary, resource.ResType.Extension);
                    log?.Invoke("Failed " + resource.ResRef + "." + resource.ResType.Extension + " (" + containerName + "): " + ex.Message, LogLevelKind.Warning);
                    output.SetData(resource.ResRef.ToString(), resource.ResType, resource.Data);
                    summary.CopiedCount++;
                }
            }

            EnsureParentExists(targetFile);
            RIMAuto.WriteRim(output, targetFile, ResourceType.RIM);
        }

        private static void PostProcess(
            string sourcePath,
            string targetBasePath,
            string outputPath,
            BioWareGame sourceGame,
            BioWareGame targetGame,
            ConversionSummary summary,
            Action<string, LogLevelKind> log)
        {
            // INI merge: target game's INI prioritized (swkotor2.ini for K1→TSL, swkotor.ini for TSL→K1).
            string outputIniName = targetGame.IsK1() ? "swkotor.ini" : "swkotor2.ini";
            string sourceIniPath = FindFile(sourcePath, sourceGame.IsK1() ? "swkotor.ini" : "swkotor2.ini");
            string targetIniPath = FindFile(targetBasePath, outputIniName);
            if (sourceIniPath != null || targetIniPath != null)
            {
                string mergedPath = Path.Combine(outputPath, outputIniName);
                MergeIni(sourceIniPath, targetIniPath, mergedPath, targetGame.IsK1() ? "swkotor.ini" : "swkotor2.ini", log);
            }

            // Module startup aliases for K1→TSL so swkotor2.exe can resolve the starting module.
            if (sourceGame.IsK1() && targetGame.IsTSL())
            {
                string modulesDir = Path.Combine(outputPath, "modules");
                if (Directory.Exists(modulesDir))
                {
                    foreach (var pair in K1ToTslModuleAliases)
                    {
                        CopyModuleAliasSet(modulesDir, pair.Key, pair.Value, log);
                    }
                }
            }
        }

        private static void MergeIni(
            string sourceIniPath,
            string targetIniPath,
            string outputPath,
            string outputIniName,
            Action<string, LogLevelKind> log)
        {
            // Merge: start with source as base, overlay target (target prioritized on conflict).
            var sections = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

            void LoadIni(string path)
            {
                if (path == null || !File.Exists(path))
                {
                    return;
                }
                string[] lines = File.ReadAllLines(path);
                string currentSection = "";
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    string trimmed = line.TrimStart();
                    if (trimmed.Length == 0 || trimmed[0] == ';' || trimmed[0] == '#')
                    {
                        continue;
                    }
                    if (trimmed.StartsWith("[") && trimmed.IndexOf(']') > 0)
                    {
                        int end = trimmed.IndexOf(']');
                        currentSection = trimmed.Substring(1, end - 1).Trim();
                        if (!sections.ContainsKey(currentSection))
                        {
                            sections[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        }
                        continue;
                    }
                    int eq = line.IndexOf('=');
                    if (eq > 0 && !string.IsNullOrEmpty(currentSection))
                    {
                        string key = line.Substring(0, eq).Trim();
                        string value = line.Substring(eq + 1).Trim();
                        sections[currentSection][key] = value;
                    }
                }
            }

            LoadIni(sourceIniPath);
            LoadIni(targetIniPath);

            var sb = new StringBuilder();
            foreach (var kv in sections.OrderBy(s => s.Key))
            {
                sb.AppendLine("[" + kv.Key + "]");
                foreach (var keyVal in kv.Value.OrderBy(k => k.Key))
                {
                    sb.AppendLine(keyVal.Key + "=" + keyVal.Value);
                }
                sb.AppendLine();
            }

            EnsureParentExists(outputPath);
            File.WriteAllText(outputPath, sb.ToString(), Encoding.UTF8);
            log?.Invoke("Merged INI → " + outputIniName + " (target prioritized).", LogLevelKind.Info);
        }

        private static void CopyModuleAliasSet(
            string modulesDir,
            string sourceBase,
            string targetBase,
            Action<string, LogLevelKind> log)
        {
            string[] suffixes = { ".mod", ".rim", "_s.rim", "_dlg.erf" };
            bool copiedAny = false;

            foreach (string suffix in suffixes)
            {
                string source = Path.Combine(modulesDir, sourceBase + suffix);
                string target = Path.Combine(modulesDir, targetBase + suffix);
                if (!File.Exists(source) || File.Exists(target))
                {
                    continue;
                }

                File.Copy(source, target, false);
                copiedAny = true;
            }

            if (copiedAny)
            {
                log?.Invoke("Created module aliases: " + sourceBase + " → " + targetBase, LogLevelKind.Info);
            }
        }

        private static void WriteBlockedConversionReport(
            string outputPath,
            ConversionSummary summary,
            BioWareGame sourceGame,
            BioWareGame targetGame,
            Action<string, LogLevelKind> log)
        {
            string txtPath = Path.Combine(outputPath, "conversion_blocked_report.txt");
            string jsonPath = Path.Combine(outputPath, "conversion_blocked_report.json");
            try
            {
                using (var writer = new StreamWriter(txtPath, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine("# ConvertKotorGame – Blocked Conversion Report");
                    writer.WriteLine();
                    writer.WriteLine("The following resources had blocked conversions. Fallback stubs were written where possible.");
                    writer.WriteLine();
                    writer.WriteLine("Source game: " + (sourceGame.IsK1() ? "K1" : "TSL"));
                    writer.WriteLine("Target game: " + (targetGame.IsK1() ? "K1" : "TSL"));
                    writer.WriteLine("Blocked count: " + summary.BlockedFiles.Count);
                    writer.WriteLine();
                    writer.WriteLine("---");
                    writer.WriteLine();
                    foreach (var entry in summary.BlockedFiles)
                    {
                        writer.WriteLine("## " + entry.RelativePath);
                        writer.WriteLine(entry.Reason);
                        writer.WriteLine();
                    }
                }

                using (var writer = new StreamWriter(jsonPath, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine("{");
                    writer.WriteLine("  \"sourceGame\": \"" + (sourceGame.IsK1() ? "K1" : "TSL") + "\",");
                    writer.WriteLine("  \"targetGame\": \"" + (targetGame.IsK1() ? "K1" : "TSL") + "\",");
                    writer.WriteLine("  \"blockedCount\": " + summary.BlockedFiles.Count + ",");
                    writer.WriteLine("  \"blockedFiles\": [");
                    for (int i = 0; i < summary.BlockedFiles.Count; i++)
                    {
                        var entry = summary.BlockedFiles[i];
                        string escapedPath = System.Text.Json.JsonSerializer.Serialize(entry.RelativePath);
                        string escapedReason = System.Text.Json.JsonSerializer.Serialize(entry.Reason);
                        string comma = i < summary.BlockedFiles.Count - 1 ? "," : "";
                        writer.WriteLine("    { \"path\": " + escapedPath + ", \"reason\": " + escapedReason + " }" + comma);
                    }
                    writer.WriteLine("  ]");
                    writer.WriteLine("}");
                }

                log?.Invoke("Wrote blocked conversion report to " + outputPath, LogLevelKind.Info);
            }
            catch (Exception ex)
            {
                log?.Invoke("Failed to write blocked conversion report: " + ex.Message, LogLevelKind.Warning);
            }
        }

        private static string ResolveBifResRef(BIFResource resource, Dictionary<int, KeyEntry> keyEntryById)
        {
            if (resource.ResRef != null && !string.IsNullOrWhiteSpace(resource.ResRef.ToString()))
            {
                return resource.ResRef.ToString();
            }

            KeyEntry entry;
            if (keyEntryById.TryGetValue(resource.ResnameKeyIndex, out entry))
            {
                return entry.ResRef.ToString();
            }

            return "resource_" + resource.ResnameKeyIndex.ToString("D6");
        }

        private static bool IsErfLikeExtension(string ext)
        {
            return ext == "erf" || ext == "mod" || ext == "sav" || ext == "hak" || ext == "nwm";
        }

        private static Dictionary<int, KeyEntry> LoadKeyEntryMap(string sourcePath, Action<string, LogLevelKind> log)
        {
            var map = new Dictionary<int, KeyEntry>();
            string keyPath = FindFile(sourcePath, "chitin.key");
            if (keyPath == null)
            {
                log?.Invoke("chitin.key not found in source installation; BIF resref recovery will be limited.", LogLevelKind.Warning);
                return map;
            }

            try
            {
                KEY key = KEYAuto.ReadKey(keyPath);
                foreach (KeyEntry entry in key.KeyEntries)
                {
                    map[(int)entry.ResourceId] = entry;
                }
            }
            catch (Exception ex)
            {
                log?.Invoke("Unable to parse chitin.key; BIF name recovery disabled: " + ex.Message, LogLevelKind.Warning);
            }

            return map;
        }

        private static string FindSubdirectory(string parent, string name)
        {
            if (!Directory.Exists(parent))
            {
                return null;
            }

            foreach (string dir in Directory.GetDirectories(parent))
            {
                if (Path.GetFileName(dir).Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return dir;
                }
            }

            return null;
        }

        private static string FindFile(string directory, string fileName)
        {
            if (!Directory.Exists(directory))
            {
                return null;
            }

            foreach (string file in Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly))
            {
                if (Path.GetFileName(file).Equals(fileName, StringComparison.OrdinalIgnoreCase))
                {
                    return file;
                }
            }

            return null;
        }

        private static int CountRootFiles(string directory)
        {
            if (!Directory.Exists(directory))
            {
                return 0;
            }
            return Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly).Length;
        }

        private static void EnsureParentExists(string path)
        {
            string parent = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(parent))
            {
                Directory.CreateDirectory(parent);
            }
        }

        private static string GetRelativePath(string root, string file)
        {
            string normalizedRoot = root.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (file.StartsWith(normalizedRoot, StringComparison.OrdinalIgnoreCase))
            {
                return file.Substring(normalizedRoot.Length);
            }
            return Path.GetFileName(file);
        }

        private static void CountSeen(ConversionSummary summary, string extension)
        {
            string key = (extension ?? "unknown").ToLowerInvariant();
            if (!summary.SeenByType.ContainsKey(key))
            {
                summary.SeenByType[key] = 0;
            }
            summary.SeenByType[key]++;
        }

        private static void CountConverted(ConversionSummary summary, string extension)
        {
            string key = (extension ?? "unknown").ToLowerInvariant();
            if (!summary.ConvertedByType.ContainsKey(key))
            {
                summary.ConvertedByType[key] = 0;
            }
            summary.ConvertedByType[key]++;
        }

        private static void CountContainer(ConversionSummary summary, string extension)
        {
            string key = (extension ?? "unknown").ToLowerInvariant();
            if (!summary.ContainersByType.ContainsKey(key))
            {
                summary.ContainersByType[key] = 0;
            }
            summary.ContainersByType[key]++;
        }

        private static void CountFailed(ConversionSummary summary, string extension)
        {
            string key = (extension ?? "unknown").ToLowerInvariant();
            if (!summary.FailedByType.ContainsKey(key))
            {
                summary.FailedByType[key] = 0;
            }
            summary.FailedByType[key]++;
        }
    }
}
