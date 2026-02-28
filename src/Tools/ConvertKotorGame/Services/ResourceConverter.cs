using System;
using System.IO;
using System.Collections.Generic;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource.Formats.GFF.Generics.ARE;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using BioWare.Resource.Formats.GFF.Generics.UTI;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Resource.Formats.MDL;
using BioWare.Resource.Formats.NCS;
using ConvertKotorGame.Models;

namespace ConvertKotorGame.Services
{
    public sealed class ResourceConverter
    {
        private static readonly HashSet<string> GameSpecificConvertedTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "ncs",
            "mdl",
            "mdx",
            "are",
            "dlg",
            "git",
            "utc",
            "utd",
            "ute",
            "uti",
            "utp",
        };

        private static readonly Dictionary<string, string> CopyAsIsReasons = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "2da", "shared writer format (no game-specific writer branch)" },
            { "tlk", "shared writer format (no game-specific writer branch)" },
            { "ssf", "shared writer format (no game-specific writer branch)" },
            { "lip", "shared writer format (no game-specific writer branch)" },
            { "lyt", "shared writer format (no game-specific writer branch)" },
            { "vis", "shared writer format (no game-specific writer branch)" },
            { "txi", "shared writer format (no game-specific writer branch)" },
            { "ltr", "shared writer format (no game-specific writer branch)" },
            { "wav", "shared writer format (no game-specific writer branch)" },
            { "wok", "shared writer format (no game-specific writer branch)" },
            { "dwk", "shared writer format (no game-specific writer branch)" },
            { "pwk", "shared writer format (no game-specific writer branch)" },
            { "bwm", "shared writer format (no game-specific writer branch)" },
            { "tga", "shared writer format (no game-specific writer branch)" },
            { "dds", "shared writer format (no game-specific writer branch)" },
            { "tpc", "shared writer format (no game-specific writer branch)" },
            { "bmp", "shared writer format (no game-specific writer branch)" },
            { "jpg", "shared writer format (no game-specific writer branch)" },
            { "png", "shared writer format (no game-specific writer branch)" },
            { "ico", "shared writer format (no game-specific writer branch)" },
            { "nss", "source script format; copied as-is" },
            { "itp", "palette format has no game-specific conversion path" },
            { "ifo", "GFF helper has no K1/TSL-specific write branches" },
            { "utm", "GFF helper has no K1/TSL-specific write branches" },
            { "pth", "GFF helper has no K1/TSL-specific write branches" },
            { "utt", "GFF helper has no K1/TSL-specific write branches" },
            { "uts", "GFF helper has no K1/TSL-specific write branches" },
            { "utw", "GFF helper has no K1/TSL-specific write branches" },
            { "jrl", "GFF helper has no K1/TSL-specific write branches" },
            { "fac", "GFF helper has no K1/TSL-specific write branches" },
            { "gam", "Odyssey save compatibility handled via NFO remap; GAM is not used here" },
            { "cnv", "non-Odyssey conversation format; no K1/TSL conversion path" },
            { "gff", "generic GFF blob with no type-specific game rules" },
            { "res", "generic GFF blob with no type-specific game rules" },
            { "bic", "no K1/TSL-specific writer branch in converter" },
            { "btc", "no K1/TSL-specific writer branch in converter" },
            { "btd", "no K1/TSL-specific writer branch in converter" },
            { "bte", "no K1/TSL-specific writer branch in converter" },
            { "bti", "no K1/TSL-specific writer branch in converter" },
            { "btm", "no K1/TSL-specific writer branch in converter" },
            { "btp", "no K1/TSL-specific writer branch in converter" },
            { "btt", "no K1/TSL-specific writer branch in converter" },
            { "cut", "no K1/TSL-specific writer branch in converter" },
            { "gui", "no K1/TSL-specific writer branch in converter" },
            { "qdb", "no K1/TSL-specific writer branch in converter" },
            { "qst", "no K1/TSL-specific writer branch in converter" },
            { "gic", "no K1/TSL-specific writer branch in converter" },
        };

        private readonly HashSet<string> _copyAsIsWarned = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public byte[] ConvertResourceData(
            string resref,
            ResourceType type,
            byte[] data,
            BioWareGame sourceGame,
            BioWareGame targetGame,
            Action<string, LogLevelKind> log,
            out bool converted)
        {
            converted = false;

            if (type == null || type.IsInvalid || data == null)
            {
                throw new InvalidOperationException("Cannot convert invalid resource input (null/invalid type or null data).");
            }

            ResourceType normalizedType = type.TargetType();
            string ext = normalizedType.Extension.ToLowerInvariant();

            try
            {
                switch (ext)
                {
                    case "ncs":
                        return ConvertNcs(resref, data, sourceGame, targetGame, log, out converted);
                }

                if (ext == "mdl" || ext == "mdx")
                {
                    // Pair conversion is handled by the archive/file traversal pass.
                    LogCopyAsIs(ext, log, "paired conversion handled at archive/file layer");
                    return data;
                }

                if (normalizedType.IsGff())
                {
                    return ConvertGffType(ext, data, targetGame, log, out converted);
                }
            }
            catch (Exception ex)
            {
                string message = "Failed converting " + resref + "." + type.Extension + ": " + ex.Message;
                throw new InvalidOperationException(message, ex);
            }

            // Known copy-as-is types must be explicitly documented. Unknown types are a hard error.

            if (CopyAsIsReasons.TryGetValue(ext, out string reason))
            {
                LogCopyAsIs(ext, log, reason);
                return data;
            }

            throw new NotSupportedException(
                "No conversion policy is defined for resource type ." + ext +
                ". Add a conversion branch or explicitly classify this type as copy-as-is with a documented reason.");
        }

        public bool TryConvertMdlPair(byte[] mdlBytes, byte[] mdxBytes, BioWareGame targetGame, out byte[] outMdl, out byte[] outMdx, out string error)
        {
            outMdl = mdlBytes;
            outMdx = mdxBytes;
            error = string.Empty;

            if (mdlBytes == null || mdlBytes.Length == 0)
            {
                error = "MDL data is empty or null";
                return false;
            }

            if (mdxBytes == null || mdxBytes.Length == 0)
            {
                error = "MDX data is empty or null";
                return false;
            }

            try
            {
                var mdl = MDLAuto.ReadMdl(mdlBytes, 0, null, mdxBytes);
                using (var mdlStream = new MemoryStream())
                using (var mdxStream = new MemoryStream())
                {
                    var writer = new MDLBinaryWriter(mdl, mdlStream, mdxStream, targetGame);
                    writer.Write();
                    outMdl = mdlStream.ToArray();
                    outMdx = mdxStream.ToArray();
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        private static byte[] ConvertNcs(string resref, byte[] data, BioWareGame sourceGame, BioWareGame targetGame, Action<string, LogLevelKind> log, out bool converted)
        {
            converted = false;

            var result = NCSActionPatcher.Patch(data, sourceGame, targetGame);

            if (result.ActionsPatched > 0)
            {
                converted = true;
                log?.Invoke($"NCS patched {result.ActionsPatched}/{result.ActionsTotal} action IDs for target game.", LogLevelKind.Trace);
            }

            if (result.UnmappableActionIds.Count > 0)
            {
                // This can only happen for TSL -> K1.
                string message =
                    $"NCS '{resref}.ncs' calls {result.UnmappableActionIds.Count} TSL-only engine functions with no K1 equivalent: " +
                    $"[{string.Join(", ", result.UnmappableActionIds)}]. " +
                    "This script cannot be converted losslessly. It will be replaced with a safe no-op script to avoid crashing the target engine.";
                throw new ConversionBlockedException(message, fallbackData: NCSActionPatcher.CreateNoOpNcs());
            }

            if (result.ParamCountMismatches.Count > 0)
            {
                // K1↔TSL param-count mismatches are always safe at the VM level.
                //
                // The NWScript ACTION instruction embeds a param_count byte that was written by the
                // original compiler. The VM unconditionally pops exactly param_count items from the
                // script stack before calling the engine function — it does NOT consult the engine
                // function's own parameter table to decide how many items to pop.
                //
                // Two safe cases arise when function signatures differ between games:
                //
                //   script > target (e.g. K1 SoundObjectFadeAndStop: 2 args → TSL: 1 arg)
                //     The VM pops both args per bytecode. The engine function reads only the args
                //     it declares; the extra value was already removed from the stack and is silently
                //     discarded. Stack balance is preserved.
                //
                //   script < target (e.g. K1 PlayMovie: 1 arg → TSL: 2 args)
                //     The VM pops the single arg per bytecode. The engine function receives fewer
                //     args than its full signature and uses built-in defaults for the missing ones.
                //     Stack balance is preserved.
                //
                // In both cases the script stack is balanced by the ACTION instruction itself, so
                // no corruption occurs. All 18 known K1↔TSL signature differences were verified
                // against KotOR.js NWScriptDefK1.ts and NWScriptDefK2.ts.
                if (log != null)
                {
                    var examples = new List<string>();
                    foreach (var mismatch in result.ParamCountMismatches)
                    {
                        if (examples.Count >= 3) break;
                        examples.Add(
                            "ACTION " + mismatch.ActionId + ": script=" + mismatch.ScriptParamCount +
                            " target=" + mismatch.ExpectedTargetParamCount +
                            (mismatch.SourceActionId != mismatch.ActionId
                                ? " (remapped from " + mismatch.SourceActionId + ")"
                                : string.Empty));
                    }
                    log(
                        "NCS '" + resref + ".ncs' " + result.ParamCountMismatches.Count +
                        " param-count delta(s) (safe, see NCSActionPatcher): " +
                        string.Join("; ", examples),
                        LogLevelKind.Trace);
                }
            }

            return result.Data;
        }

        private byte[] ConvertGffType(string ext, byte[] data, BioWareGame targetGame, Action<string, LogLevelKind> log, out bool converted)
        {
            switch (ext)
            {
                case "are":
                    converted = true;
                    return AREHelpers.BytesAre(AREHelpers.ReadAre(data), targetGame, ResourceType.ARE);
                case "dlg":
                    converted = true;
                    return DLGHelper.BytesDlg(DLGHelper.ReadDlg(data), targetGame, ResourceType.DLG);
                case "git":
                {
                    converted = true;
                    GFF gitGff = GFFAuto.ReadGff(data, fileFormat: ResourceType.GIT);
                    GIT git = GITHelpers.ConstructGit(gitGff);
                    GFF rebuilt = GITHelpers.DismantleGit(git, targetGame);
                    return GFFAuto.BytesGff(rebuilt, ResourceType.GIT);
                }
                case "utc":
                    converted = true;
                    return UTCHelpers.BytesUtc(UTCHelpers.ReadUtc(data), targetGame, ResourceType.UTC);
                case "utd":
                    converted = true;
                    return UTDHelpers.BytesUtd(UTDHelpers.ReadUtd(data), targetGame, ResourceType.UTD);
                case "ute":
                {
                    converted = true;
                    GFF gff = GFFAuto.ReadGff(data, fileFormat: ResourceType.UTE);
                    UTE ute = UTEHelpers.ConstructUte(gff);
                    return GFFAuto.BytesGff(UTEHelpers.DismantleUte(ute, targetGame), ResourceType.UTE);
                }
                case "uti":
                    converted = true;
                    return UTIHelpers.BytesUti(UTIHelpers.ReadUti(data), targetGame, ResourceType.UTI);
                case "utp":
                {
                    converted = true;
                    GFF gff = GFFAuto.ReadGff(data, fileFormat: ResourceType.UTP);
                    UTP utp = UTPHelpers.ConstructUtp(gff);
                    return GFFAuto.BytesGff(UTPHelpers.DismantleUtp(utp, targetGame), ResourceType.UTP);
                }
                default:
                {
                    converted = false;
                    if (CopyAsIsReasons.TryGetValue(ext, out string reason))
                    {
                        LogCopyAsIs(ext, log, reason);
                        return data;
                    }

                    throw new NotSupportedException(
                        "No GFF conversion policy is defined for type ." + ext +
                        ". Add a conversion branch or explicitly classify it as copy-as-is with rationale.");
                }
            }
        }

        private void LogCopyAsIs(string ext, Action<string, LogLevelKind> log, string reason)
        {
            if (log != null && _copyAsIsWarned.Add(ext))
            {
                log("Copy-as-is for type ." + ext + " (" + reason + ").", LogLevelKind.Trace);
            }
        }
    }
}
