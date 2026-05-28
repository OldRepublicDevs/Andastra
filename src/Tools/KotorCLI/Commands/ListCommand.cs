using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using System.Linq;
using KotorCLI.Configuration;
using KotorCLI.Logging;
using Tomlyn.Model;

namespace KotorCLI.Commands
{
    /// <summary>
    /// List command - List all targets defined in kotorcli.cfg.
    /// </summary>
    public static class ListCommand
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var listCommand = new Command("list", "List all targets defined in kotorcli.cfg");
            var targetsArgument = new Argument<string[]>("targets");
            listCommand.Add(targetsArgument);
            var quietOption = Cli.Opt<bool>("--quiet", "List only target names");
            listCommand.Options.Add(quietOption);
            var verboseOption = Cli.Opt<bool>("--verbose", "List source files as well");
            listCommand.Options.Add(verboseOption);

            listCommand.SetAction(parseResult =>
            {
                var targets = parseResult.GetValue(targetsArgument) ?? Array.Empty<string>();
                var quiet = parseResult.GetValue(quietOption);
                var verbose = parseResult.GetValue(verboseOption);

                var logger = new StandardLogger();
                var exitCode = Execute(targets, quiet, verbose, logger);
                Environment.Exit(exitCode);
            });

            rootCommand.Add(listCommand);
        }

        public static int Execute(string[] targets, bool quiet, bool verbose, ILogger logger)
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
                var rootDir = Path.GetDirectoryName(configPath);
                var allTargets = config.GetTargets();

                if (allTargets.Count == 0)
                {
                    logger.Info("No targets defined in kotorcli.cfg");
                    return 0;
                }

                var targetsToShow = targets.Length > 0
                    ? allTargets.Where(t => targets.Contains(GetTomlValue<string>(t, "name") ?? "")).ToList()
                    : allTargets;

                if (targetsToShow.Count == 0)
                {
                    logger.Warning($"No targets found matching: {string.Join(", ", targets)}");
                    return 1;
                }

                foreach (var target in targetsToShow)
                {
                    var targetName = GetTomlValue<string>(target, "name") ?? "unnamed";
                    var targetFile = GetTomlValue<string>(target, "file") ?? "";
                    var targetDescription = GetTomlValue<string>(target, "description") ?? "";

                    if (quiet)
                    {
                        logger.Info(targetName);
                    }
                    else
                    {
                        logger.Info($"Target: {targetName}");
                        if (!string.IsNullOrEmpty(targetDescription))
                        {
                            logger.Info($"  Description: {targetDescription}");
                        }
                        logger.Info($"  File: {targetFile}");

                        if (verbose)
                        {
                            var sourceFiles = GetTargetSourceFiles(config, target, rootDir);
                            if (sourceFiles.Count > 0)
                            {
                                logger.Info("  Source files:");
                                foreach (var sourceFile in sourceFiles.OrderBy(f => f))
                                {
                                    logger.Info($"    {sourceFile}");
                                }
                            }
                            else
                            {
                                logger.Info("  Source files: (none found)");
                            }
                        }
                    }
                }

                return 0;
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to list targets: {ex.Message}");
                return 1;
            }
        }

        private static List<string> GetTargetSourceFiles(KotorCLIConfig config, TomlTable target, string rootDir)
        {
            var sourceFiles = new List<string>();

            // Get source patterns from target configuration
            var sources = config.GetTargetSources(target);
            var includePatterns = sources.ContainsKey("include") ? sources["include"] : new List<string>();
            var excludePatterns = sources.ContainsKey("exclude") ? sources["exclude"] : new List<string>();

            // If no include patterns defined, use default patterns
            if (includePatterns.Count == 0)
            {
                includePatterns.Add("**/*.nss");  // Default NWScript source files
                includePatterns.Add("**/*.ncs");  // Default compiled scripts
            }

            // Find all files matching include patterns
            foreach (var pattern in includePatterns)
            {
                var matches = GlobPatternMatcher.FindFilesMatchingPattern(rootDir, pattern);
                foreach (var match in matches)
                {
                    bool excluded = false;
                    foreach (var excludePattern in excludePatterns)
                    {
                        var excludePath = Path.Combine(rootDir, excludePattern);
                        if (GlobPatternMatcher.MatchPattern(match, excludePath))
                        {
                            excluded = true;
                            break;
                        }
                    }

                    if (!excluded)
                    {
                        var relativePath = GetRelativePath(rootDir, match);
                        if (!sourceFiles.Contains(relativePath))
                        {
                            sourceFiles.Add(relativePath);
                        }
                    }
                }
            }

            return sourceFiles;
        }

        private static string GetRelativePath(string rootDir, string fullPath)
        {
            if (fullPath.StartsWith(rootDir, StringComparison.OrdinalIgnoreCase))
            {
                var relativePath = fullPath.Substring(rootDir.Length);
                if (relativePath.StartsWith(Path.DirectorySeparatorChar.ToString()) || relativePath.StartsWith(Path.AltDirectorySeparatorChar.ToString()))
                {
                    relativePath = relativePath.Substring(1);
                }
                return relativePath;
            }
            return fullPath;
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
