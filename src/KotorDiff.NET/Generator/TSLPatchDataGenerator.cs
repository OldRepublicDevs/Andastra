// Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/generator.py:68-129
// Original: class TSLPatchDataGenerator: ...
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSharpKOTOR.Mods;
using CSharpKOTOR.Mods.GFF;
using CSharpKOTOR.Mods.SSF;
using CSharpKOTOR.Mods.TLK;
using CSharpKOTOR.Mods.TwoDA;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Formats.TwoDA;
using CSharpKOTOR.Formats.TLK;
using CSharpKOTOR.Formats.SSF;
using CSharpKOTOR.Resources;

namespace KotorDiff.NET.Generator
{
    // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/generator.py:68-129
    // Original: class TSLPatchDataGenerator: ...
    public class TSLPatchDataGenerator
    {
        private readonly DirectoryInfo _tslpatchdataPath;

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/generator.py:71-79
        // Original: def __init__(self, tslpatchdata_path: Path): ...
        public TSLPatchDataGenerator(DirectoryInfo tslpatchdataPath)
        {
            _tslpatchdataPath = tslpatchdataPath;
            if (!_tslpatchdataPath.Exists)
            {
                _tslpatchdataPath.Create();
            }
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/generator.py:81-129
        // Original: def generate_all_files(...): ...
        public Dictionary<string, FileInfo> GenerateAllFiles(
            ModificationsByType modifications,
            DirectoryInfo baseDataPath = null)
        {
            var generatedFiles = new Dictionary<string, FileInfo>();

            // Generate TLK files
            if (modifications.Tlk != null && modifications.Tlk.Count > 0)
            {
                var tlkFiles = GenerateTlkFiles(modifications.Tlk, baseDataPath);
                foreach (var kvp in tlkFiles)
                {
                    generatedFiles[kvp.Key] = kvp.Value;
                }
            }

            // Generate 2DA files
            if (modifications.Twoda != null && modifications.Twoda.Count > 0)
            {
                var twodaFiles = Generate2DaFiles(modifications.Twoda, baseDataPath);
                foreach (var kvp in twodaFiles)
                {
                    generatedFiles[kvp.Key] = kvp.Value;
                }
            }

            // Generate GFF files
            if (modifications.Gff != null && modifications.Gff.Count > 0)
            {
                var gffFiles = GenerateGffFiles(modifications.Gff, baseDataPath);
                foreach (var kvp in gffFiles)
                {
                    generatedFiles[kvp.Key] = kvp.Value;
                }
            }

            // Generate SSF files
            if (modifications.Ssf != null && modifications.Ssf.Count > 0)
            {
                var ssfFiles = GenerateSsfFiles(modifications.Ssf, baseDataPath);
                foreach (var kvp in ssfFiles)
                {
                    generatedFiles[kvp.Key] = kvp.Value;
                }
            }

            // Copy missing files from install folders if base_data_path provided
            if (modifications.Install != null && modifications.Install.Count > 0 && baseDataPath != null)
            {
                var installFiles = CopyInstallFiles(modifications.Install, baseDataPath);
                foreach (var kvp in installFiles)
                {
                    generatedFiles[kvp.Key] = kvp.Value;
                }
            }

            return generatedFiles;
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/generator.py:289-338
        // Original: def _generate_tlk_files(...): ...
        private Dictionary<string, FileInfo> GenerateTlkFiles(
            List<ModificationsTLK> tlkModifications,
            DirectoryInfo baseDataPath)
        {
            var generated = new Dictionary<string, FileInfo>();

            foreach (var modTlk in tlkModifications)
            {
                // TODO: Implement TLK file generation
                // This should create append.tlk with all modifiers
            }

            return generated;
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/generator.py:340-391
        // Original: def _generate_2da_files(...): ...
        private Dictionary<string, FileInfo> Generate2DaFiles(
            List<Modifications2DA> twodaModifications,
            DirectoryInfo baseDataPath)
        {
            var generated = new Dictionary<string, FileInfo>();

            foreach (var mod2da in twodaModifications)
            {
                string filename = mod2da.SourceFile;
                var outputPath = new FileInfo(Path.Combine(_tslpatchdataPath.FullName, filename));

                // TODO: Load base 2DA file from baseDataPath and write to output
                // This should use CSharpKOTOR.Formats.TwoDA readers/writers
            }

            return generated;
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/generator.py:393-450
        // Original: def _generate_gff_files(...): ...
        private Dictionary<string, FileInfo> GenerateGffFiles(
            List<ModificationsGFF> gffModifications,
            DirectoryInfo baseDataPath)
        {
            var generated = new Dictionary<string, FileInfo>();

            foreach (var modGff in gffModifications)
            {
                // TODO: Implement GFF file generation
                // This should use CSharpKOTOR.Formats.GFF readers/writers
            }

            return generated;
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/generator.py:911-972
        // Original: def _generate_ssf_files(...): ...
        private Dictionary<string, FileInfo> GenerateSsfFiles(
            List<ModificationsSSF> ssfModifications,
            DirectoryInfo baseDataPath)
        {
            var generated = new Dictionary<string, FileInfo>();

            foreach (var modSsf in ssfModifications)
            {
                // TODO: Implement SSF file generation
                // This should use CSharpKOTOR.Formats.SSF readers/writers
            }

            return generated;
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/generator.py:131-231
        // Original: def _copy_install_files(...): ...
        private Dictionary<string, FileInfo> CopyInstallFiles(
            List<InstallFile> installFiles,
            DirectoryInfo baseDataPath)
        {
            var copiedFiles = new Dictionary<string, FileInfo>();

            // Group files by folder for efficient processing
            var filesByFolder = new Dictionary<string, List<string>>();
            foreach (var installFile in installFiles)
            {
                string folder = !string.IsNullOrEmpty(installFile.Destination) && installFile.Destination != "." 
                    ? installFile.Destination 
                    : "Override";
                string filename = !string.IsNullOrEmpty(installFile.SaveAs) 
                    ? installFile.SaveAs 
                    : installFile.SourceFile;
                
                if (!filesByFolder.ContainsKey(folder))
                {
                    filesByFolder[folder] = new List<string>();
                }
                filesByFolder[folder].Add(filename);
            }

            foreach (var kvp in filesByFolder)
            {
                string folder = kvp.Key;
                var filenames = kvp.Value;
                var sourceFolder = new DirectoryInfo(Path.Combine(baseDataPath.FullName, folder));

                foreach (string filename in filenames)
                {
                    var sourceFile = new FileInfo(Path.Combine(sourceFolder.FullName, filename));
                    if (sourceFile.Exists)
                    {
                        var destFile = new FileInfo(Path.Combine(_tslpatchdataPath.FullName, filename));
                        File.Copy(sourceFile.FullName, destFile.FullName, true);
                        copiedFiles[filename] = destFile;
                    }
                }
            }

            return copiedFiles;
        }
    }
}

