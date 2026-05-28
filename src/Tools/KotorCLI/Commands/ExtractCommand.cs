using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BioWare.Common;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.RIM;
using BioWare.Resource.Formats.KEY;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.BIF;
using BioWare.Tools;
using KotorCLI.Logging;

namespace KotorCLI.Commands
{
    /// <summary>
    /// Extract command - Extract resources from archive files (KEY/BIF, RIM, ERF, etc.).
    /// </summary>
    /// <remarks>
    /// Based on vendor/PyKotor/Tools/KotorCLI/src/kotorcli/commands/extract.py
    /// Supports:
    /// - KEY/BIF archives
    /// - RIM archives
    /// - ERF/MOD/SAV/HAK archives
    /// - BIF files (with optional KEY file for resource names)
    /// </remarks>
    public static class ExtractCommand
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var extractCommand = new Command("extract", "Extract resources from archive files (KEY/BIF, RIM, ERF, etc.)");
            var fileOption = Cli.Opt<string>("--file", "Archive file to extract");
            fileOption.Required = true;
            extractCommand.Options.Add(fileOption);
            var outputOption = Cli.Opt<string>("--output", "Output directory (default: archive_name)");
            extractCommand.Options.Add(outputOption);
            var filterOption = Cli.Opt<string>("--filter", "Filter resources by name pattern (supports wildcards)");
            extractCommand.Options.Add(filterOption);
            var keyFileOption = Cli.Opt<string>("--key-file", "KEY file for BIF extraction (default: chitin.key)");
            extractCommand.Options.Add(keyFileOption);

            extractCommand.SetAction(parseResult =>
            {
                var file = parseResult.GetValue(fileOption);
                var output = parseResult.GetValue(outputOption);
                var filter = parseResult.GetValue(filterOption);
                var keyFile = parseResult.GetValue(keyFileOption);

                var logger = new StandardLogger();
                var exitCode = Execute(file, output, filter, keyFile, logger);
                Environment.Exit(exitCode);
            });

            rootCommand.Add(extractCommand);
        }

        public static int Execute(string file, string output, string filter, string keyFile, ILogger logger)
        {
            if (string.IsNullOrEmpty(file))
            {
                logger.Error("No input file specified. Use --file <archive>");
                return 1;
            }

            string inputPath = Path.GetFullPath(file);
            if (!File.Exists(inputPath))
            {
                logger.Error($"Input file not found: {inputPath}");
                return 1;
            }

            // Determine output directory
            string outputDir;
            if (!string.IsNullOrEmpty(output))
            {
                outputDir = Path.GetFullPath(output);
            }
            else
            {
                string archiveName = Path.GetFileNameWithoutExtension(inputPath);
                outputDir = Path.Combine(Directory.GetCurrentDirectory(), archiveName);
            }

            Directory.CreateDirectory(outputDir);

            // Detect archive type by extension
            string extension = Path.GetExtension(inputPath).ToLowerInvariant();
            logger.Info($"Extracting from {extension} archive: {Path.GetFileName(inputPath)}");

            try
            {
                // Dispatch to appropriate extractor
                if (extension == ".key")
                {
                    return ExtractKey(inputPath, outputDir, filter, logger);
                }
                else if (extension == ".bif")
                {
                    return ExtractBif(inputPath, outputDir, filter, keyFile, logger);
                }
                else if (extension == ".rim")
                {
                    return ExtractRim(inputPath, outputDir, filter, logger);
                }
                else if (extension == ".erf" || extension == ".mod" || extension == ".sav" || extension == ".hak")
                {
                    return ExtractErf(inputPath, outputDir, filter, logger);
                }
                else
                {
                    logger.Error($"Unsupported archive type: {extension}");
                    logger.Info("Supported types: .key, .bif, .rim, .erf, .mod, .sav, .hak");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to extract archive: {ex.Message}");
                logger.Error(ex.StackTrace);
                return 1;
            }
        }

        /// <summary>
        /// Checks if text matches filter pattern (supports wildcards).
        /// </summary>
        private static bool MatchesFilter(string text, string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return true;
            }

            if (pattern.Contains("*") || pattern.Contains("?"))
            {
                // Convert wildcard pattern to regex
                string regexPattern = "^" + Regex.Escape(pattern)
                    .Replace("\\*", ".*")
                    .Replace("\\?", ".") + "$";
                return Regex.IsMatch(text, regexPattern, RegexOptions.IgnoreCase);
            }

            return text.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Extracts resources from an ERF/MOD/SAV/HAK archive.
        /// </summary>
        private static int ExtractErf(string erfPath, string outputDir, string filter, ILogger logger)
        {
            try
            {
                ERF erf = new ERFBinaryReader(erfPath).Load();
                if (erf == null)
                {
                    logger.Error("Failed to load ERF file");
                    return 1;
                }

                int extractedCount = 0;
                foreach (ERFResource resource in erf)
                {
                    string resref = resource.ResRef?.ToString() ?? "unknown";

                    // Apply filter
                    if (!MatchesFilter(resref, filter))
                    {
                        continue;
                    }

                    string ext = resource.ResType?.Extension ?? "bin";
                    string outputFile = Path.Combine(outputDir, $"{resref}.{ext}");

                    Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                    File.WriteAllBytes(outputFile, resource.Data);
                    extractedCount++;
                }

                logger.Info($"Extracted {extractedCount} resources from ERF archive");
                return 0;
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to extract ERF: {ex.Message}");
                return 1;
            }
        }

        /// <summary>
        /// Extracts resources from a RIM archive.
        /// </summary>
        private static int ExtractRim(string rimPath, string outputDir, string filter, ILogger logger)
        {
            try
            {
                RIM rim = new RIMBinaryReader(rimPath).Load();
                if (rim == null)
                {
                    logger.Error("Failed to load RIM file");
                    return 1;
                }

                int extractedCount = 0;
                foreach (RIMResource resource in rim)
                {
                    string resref = resource.ResRef?.ToString() ?? "unknown";

                    // Apply filter
                    if (!MatchesFilter(resref, filter))
                    {
                        continue;
                    }

                    string ext = resource.ResType?.Extension ?? "bin";
                    string outputFile = Path.Combine(outputDir, $"{resref}.{ext}");

                    Directory.CreateDirectory(Path.GetDirectoryName(outputFile));
                    File.WriteAllBytes(outputFile, resource.Data);
                    extractedCount++;
                }

                logger.Info($"Extracted {extractedCount} resources from RIM archive");
                return 0;
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to extract RIM: {ex.Message}");
                return 1;
            }
        }

        /// <summary>
        /// Extracts resources from a BIF file (requires KEY for resource names).
        /// </summary>
        private static int ExtractBif(string bifPath, string outputDir, string filter, string keyFile, ILogger logger)
        {
            try
            {
                string keyPath = ResolveKeyPath(bifPath, keyFile, logger);

                int extractedCount = 0;
                foreach ((ArchiveResource resource, string outputFile) in ArchiveHelpers.ExtractBif(
                             bifPath,
                             outputDir,
                             keyPath,
                             filter))
                {
                    string outputDirectory = Path.GetDirectoryName(outputFile);
                    if (!string.IsNullOrEmpty(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    File.WriteAllBytes(outputFile, resource.Data);
                    extractedCount++;
                }

                logger.Info($"Extracted {extractedCount} resources");
                return 0;
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to extract BIF: {ex.Message}");
                return 1;
            }
        }

        /// <summary>
        /// Extracts resources from KEY/BIF archives.
        /// </summary>
        private static int ExtractKey(string keyPath, string outputDir, string filter, ILogger logger)
        {
            try
            {
                string searchDir = Path.GetDirectoryName(keyPath);
                int extractedCount = 0;
                var seenBifs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach ((ArchiveResource resource, string outputFile, string bifPath) in ArchiveHelpers.ExtractKeyBif(
                             keyPath,
                             outputDir,
                             searchDir,
                             filter))
                {
                    if (!seenBifs.Contains(bifPath))
                    {
                        logger.Info($"Extracting from BIF: {Path.GetFileName(bifPath)}");
                        seenBifs.Add(bifPath);
                    }

                    string outputDirectory = Path.GetDirectoryName(outputFile);
                    if (!string.IsNullOrEmpty(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    File.WriteAllBytes(outputFile, resource.Data);
                    extractedCount++;
                }

                logger.Info($"Extracted {extractedCount} resources");
                return 0;
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to extract KEY/BIF: {ex.Message}");
                return 1;
            }
        }

        private static string ResolveKeyPath(string bifPath, string keyFile, ILogger logger)
        {
            string keyPath;
            if (!string.IsNullOrEmpty(keyFile))
            {
                keyPath = Path.GetFullPath(keyFile);
                if (!File.Exists(keyPath))
                {
                    logger.Warning($"KEY file not found: {keyPath}. Resources will have numeric names.");
                    return null;
                }

                return keyPath;
            }

            keyPath = Path.Combine(Path.GetDirectoryName(bifPath) ?? string.Empty, "chitin.key");
            if (File.Exists(keyPath))
            {
                return keyPath;
            }

            logger.Warning($"KEY file not found: {keyPath}. Resources will have numeric names.");
            return null;
        }
    }
}
