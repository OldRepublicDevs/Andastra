using System;
using System.CommandLine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.RIM;
using BioWare.Common;
using BioWare.Resource;
using KotorCLI.Configuration;
using KotorCLI.Logging;
using Tomlyn.Model;

namespace KotorCLI.Commands
{
    /// <summary>
    /// Unpack command - Unpack a file into the project source tree.
    /// </summary>
    public static class UnpackCommand
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var unpackCommand = new Command("unpack", "Unpack a file into the project source tree");
            var targetArgument = new Argument<string>("target");
            unpackCommand.Add(targetArgument);
            var fileArgument = new Argument<string>("file");
            unpackCommand.Add(fileArgument);
            var fileOption = Cli.Opt<string>("--file", "File or directory to unpack into the target's source tree");
            unpackCommand.Options.Add(fileOption);
            var removeDeletedOption = Cli.Opt<bool>("--removeDeleted", "Remove source files not present in the file being unpacked");
            unpackCommand.Options.Add(removeDeletedOption);

            unpackCommand.SetAction(parseResult =>
            {
                var target = parseResult.GetValue(targetArgument);
                var file = parseResult.GetValue(fileArgument);
                var fileOpt = parseResult.GetValue(fileOption);
                var removeDeleted = parseResult.GetValue(removeDeletedOption);

                var logger = new StandardLogger();
                var exitCode = Execute(target, fileOpt ?? file, removeDeleted, logger);
                Environment.Exit(exitCode);
            });

            rootCommand.Add(unpackCommand);
        }

        public static int Execute(string targetName, string unpackFile, bool removeDeleted, ILogger logger)
        {
            var configPath = ConfigFileFinder.FindConfigFile();
            if (configPath == null)
            {
                logger.Error("This is not a kotorcli repository. Please run 'kotorcli init'");
                return 1;
            }

            try
            {
                var config = new KotorCLIConfig(configPath);
                var target = config.GetTarget(targetName);
                if (target == null)
                {
                    logger.Error(targetName != null ? $"Target not found: {targetName}" : "No default target found");
                    return 1;
                }

                var actualTargetName = GetTomlValue<string>(target, "name") ?? "unnamed";
                logger.Info($"Unpacking target: {actualTargetName}");

                // Determine file to unpack
                string filePathStr;
                if (!string.IsNullOrEmpty(unpackFile))
                {
                    filePathStr = unpackFile;
                }
                else
                {
                    var targetFile = config.ResolveTargetValue(target, "file")?.ToString();
                    if (string.IsNullOrEmpty(targetFile))
                    {
                        logger.Error("No file specified and target has no file defined");
                        return 1;
                    }

                    filePathStr = targetFile;
                    if (!Path.IsPathRooted(filePathStr))
                    {
                        filePathStr = Path.Combine(Path.GetDirectoryName(configPath), filePathStr);
                    }
                }

                var filePath = new FileInfo(filePathStr);
                if (!filePath.Exists)
                {
                    logger.Error($"File not found: {filePathStr}");
                    return 1;
                }

                logger.Info($"Unpacking from: {filePath.FullName}");

                // Determine file type and unpack
                var suffix = filePath.Extension.ToLowerInvariant();
                ERF erf = null;
                RIM rim = null;

                if (suffix == ".mod" || suffix == ".erf" || suffix == ".sav")
                {
                    erf = ERFAuto.ReadErf(filePath.FullName);
                    if (erf == null)
                    {
                        logger.Error("Failed to read ERF archive");
                        return 1;
                    }
                    logger.Info($"Archive contains {erf.Count()} resources");
                }
                else if (suffix == ".rim")
                {
                    rim = RIMAuto.ReadRim(filePath.FullName);
                    if (rim == null)
                    {
                        logger.Error("Failed to read RIM archive");
                        return 1;
                    }
                    logger.Info($"Archive contains {rim.Count()} resources");
                }
                else
                {
                    logger.Error($"Unsupported file type: {suffix}");
                    return 1;
                }

                // Get target rules
                var rules = config.GetTargetRules(target);

                // Create cache directory for tracking
                var cacheDir = Path.Combine(Path.GetDirectoryName(configPath), ".kotorcli", "cache", actualTargetName);
                Directory.CreateDirectory(cacheDir);

                var projectRoot = Path.GetDirectoryName(configPath);
                var writtenPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var unpackedCount = 0;

                // Unpack ERF
                if (erf != null)
                {
                    foreach (var resource in erf)
                    {
                        if (TryUnpackResource(resource.ResRef.ToString(), resource.ResType, resource.Data, rules, projectRoot, logger, writtenPaths))
                        {
                            unpackedCount++;
                        }
                    }
                }

                // Unpack RIM
                if (rim != null)
                {
                    foreach (var resource in rim)
                    {
                        if (TryUnpackResource(resource.ResRef.ToString(), resource.ResType, resource.Data, rules, projectRoot, logger, writtenPaths))
                        {
                            unpackedCount++;
                        }
                    }
                }

                logger.Info($"Successfully unpacked {unpackedCount} files");

                if (removeDeleted)
                {
                    RemoveDeletedSources(writtenPaths, CollectRuleRoots(rules, projectRoot), projectRoot, logger);
                }

                return 0;
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to unpack: {ex.Message}");
                if (logger.IsDebug)
                {
                    logger.Debug($"Stack trace: {ex.StackTrace}");
                }
                return 1;
            }
        }

        private static bool TryUnpackResource(
            string resref,
            ResourceType resType,
            byte[] resourceData,
            Dictionary<string, string> rules,
            string rootDir,
            ILogger logger,
            HashSet<string> writtenPaths)
        {
            var filename = $"{resref}.{resType.Extension}";
            var destination = DetermineDestination(filename, resType, rules, rootDir);
            if (destination == null)
            {
                logger.Debug($"No rule found for {filename}, skipping");
                return false;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(destination));

            if (resType.IsGff())
            {
                try
                {
                    var reader = new GFFBinaryReader(resourceData);
                    var gff = reader.Load();
                    var jsonDest = destination + ".json";
                    GFFAuto.WriteGff(gff, jsonDest, ResourceType.GFF_JSON);
                    destination = jsonDest;
                    logger.Debug($"Converted {filename} to JSON: {Path.GetFileName(destination)}");
                }
                catch (Exception ex)
                {
                    logger.Warning($"Failed to convert {filename} to JSON: {ex.Message}, writing as binary");
                    File.WriteAllBytes(destination, resourceData);
                }
            }
            else
            {
                File.WriteAllBytes(destination, resourceData);
                logger.Debug($"Extracted {filename}: {Path.GetFileName(destination)}");
            }

            writtenPaths.Add(Path.GetFullPath(destination));
            return true;
        }

        internal static HashSet<string> CollectRuleRoots(Dictionary<string, string> rules, string rootDir)
        {
            var roots = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                Path.GetFullPath(Path.Combine(rootDir, "src"))
            };

            if (rules != null)
            {
                foreach (string destination in rules.Values)
                {
                    roots.Add(Path.GetFullPath(Path.Combine(rootDir, destination)));
                }
            }

            return roots;
        }

        internal static void RemoveDeletedSources(
            HashSet<string> writtenPaths,
            HashSet<string> scanRoots,
            string projectRoot,
            ILogger logger)
        {
            string kotorcliDir = Path.GetFullPath(Path.Combine(projectRoot, ".kotorcli"));
            int removed = 0;

            foreach (string root in scanRoots)
            {
                if (!Directory.Exists(root))
                {
                    continue;
                }

                foreach (string file in Directory.GetFiles(root, "*", SearchOption.AllDirectories))
                {
                    string fullPath = Path.GetFullPath(file);
                    if (fullPath.StartsWith(kotorcliDir, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (writtenPaths.Contains(fullPath))
                    {
                        continue;
                    }

                    File.Delete(fullPath);
                    removed++;
                    logger.Debug($"Removed stale source: {fullPath}");
                }
            }

            if (removed > 0)
            {
                logger.Info($"Removed {removed} stale source file(s)");
            }
        }

        private static string DetermineDestination(string filename, ResourceType resType, System.Collections.Generic.Dictionary<string, string> rules, string rootDir)
        {
            // Check rules for pattern match
            foreach (var kvp in rules)
            {
                var pattern = kvp.Key;
                var destination = kvp.Value;

                // Simple wildcard matching
                if (pattern == "*" || MatchesPattern(filename, pattern))
                {
                    var destPath = Path.Combine(rootDir, destination);
                    return Path.Combine(destPath, filename);
                }
            }

            // Default to src directory
            return Path.Combine(rootDir, "src", filename);
        }

        private static bool MatchesPattern(string filename, string pattern)
        {
            if (pattern == "*")
            {
                return true;
            }

            if (pattern.StartsWith("*."))
            {
                var ext = pattern.Substring(1);
                return filename.EndsWith(ext, StringComparison.OrdinalIgnoreCase);
            }

            return filename.Equals(pattern, StringComparison.OrdinalIgnoreCase);
        }

        private static T GetTomlValue<T>(Tomlyn.Model.TomlTable table, string key)
        {
            if (table.TryGetValue(key, out object value))
            {
                if (value is T direct)
                {
                    return direct;
                }
                return (T)Convert.ChangeType(value, typeof(T));
            }
            return default(T);
        }
    }
}
