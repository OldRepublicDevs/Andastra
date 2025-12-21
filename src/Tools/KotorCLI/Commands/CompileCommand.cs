using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.CommandLine;
using Andastra.Parsing;
using Andastra.Parsing.Common;
using Andastra.Parsing.Formats.NCS;
using KotorCLI.Configuration;
using KotorCLI.Logging;
using Tomlyn.Model;

namespace KotorCLI.Commands
{
    public static class CompileCommand
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var compileCommand = new Command("compile", "Compile all nss sources for target");
            var targetsArgument = new Argument<string[]>("targets", () => new string[0], "Targets to compile (use 'all' for all targets)");
            compileCommand.AddArgument(targetsArgument);
            var cleanOption = new Option<bool>("--clean", "Clear the cache before compiling");
            compileCommand.AddOption(cleanOption);
            var fileOption = new Option<string[]>(new[] { "-f", "--file" }, "Compile specific file(s)");
            compileCommand.AddOption(fileOption);
            compileCommand.SetHandler((string[] targets, bool clean, string[] files) =>
            {
                var logger = new StandardLogger();
                ExecuteCompile(targets, clean, files, logger);
            }, targetsArgument, cleanOption, fileOption);
            rootCommand.AddCommand(compileCommand);
        }

        private static void ExecuteCompile(string[] targets, bool clean, string[] files, ILogger logger)
        {
            // Load configuration
            var config = CommandBase.LoadConfig(logger);
            if (config == null)
            {
                return;
            }

            // Determine game version (default to K2 for compatibility)
            var game = Game.K2;

            // Determine targets
            List<TomlTable> targetList;
            if (targets != null && targets.Length > 0 && targets.Contains("all", StringComparer.OrdinalIgnoreCase))
            {
                targetList = config.GetTargets();
            }
            else
            {
                targetList = new List<TomlTable>();
                var targetNames = targets != null && targets.Length > 0 ? targets : new[] { (string)null };
                foreach (var name in targetNames)
                {
                    var target = config.GetTarget(name);
                    if (target == null)
                    {
                        if (name != null)
                        {
                            logger.Error($"Target not found: {name}");
                        }
                        else
                        {
                            logger.Error("No default target found");
                        }
                        return;
                    }
                    targetList.Add(target);
                }
            }

            if (targetList.Count == 0)
            {
                logger.Error("No targets found to compile");
                return;
            }

            // Process each target
            foreach (var target in targetList)
            {
                var targetName = target.TryGetValue("name", out object nameObj) && nameObj is string name ? name : "unnamed";
                logger.Info($"Compiling target: {targetName}");

                // Get cache directory
                var cacheDir = Path.Combine(config.RootDir, ".kotorcli", "cache", targetName);
                if (clean && Directory.Exists(cacheDir))
                {
                    logger.Info($"Cleaning cache: {cacheDir}");
                    Directory.Delete(cacheDir, true);
                }
                Directory.CreateDirectory(cacheDir);

                // Get source patterns
                var sources = config.GetTargetSources(target);
                var includePatterns = sources.ContainsKey("include") ? sources["include"] : new List<string>();
                var excludePatterns = sources.ContainsKey("exclude") ? sources["exclude"] : new List<string>();
                var skipCompilePatterns = sources.ContainsKey("skipCompile") ? sources["skipCompile"] : new List<string>();

                // Find NSS files to compile
                var nssFiles = new List<string>();
                if (files != null && files.Length > 0)
                {
                    // Specific files specified
                    foreach (var fileSpec in files)
                    {
                        var filePath = Path.IsPathRooted(fileSpec) ? fileSpec : Path.Combine(config.RootDir, fileSpec);
                        if (File.Exists(filePath) && Path.GetExtension(filePath).Equals(".nss", StringComparison.OrdinalIgnoreCase))
                        {
                            nssFiles.Add(filePath);
                        }
                        else
                        {
                            // Try to find by name in include patterns
                            foreach (var pattern in includePatterns)
                            {
                                var patternPath = Path.Combine(config.RootDir, pattern);
                                var matches = GlobFiles(patternPath);
                                foreach (var match in matches)
                                {
                                    var matchName = Path.GetFileName(match);
                                    if (matchName.Equals(fileSpec, StringComparison.OrdinalIgnoreCase) ||
                                        matchName.Equals(fileSpec + ".nss", StringComparison.OrdinalIgnoreCase))
                                    {
                                        nssFiles.Add(match);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    // Find all NSS files matching patterns
                    foreach (var pattern in includePatterns)
                    {
                        var patternPath = Path.Combine(config.RootDir, pattern);
                        var matches = GlobFiles(patternPath);
                        foreach (var match in matches)
                        {
                            if (!Path.GetExtension(match).Equals(".nss", StringComparison.OrdinalIgnoreCase))
                            {
                                continue;
                            }

                            // Check against exclude patterns
                            var excluded = false;
                            var relativePath = Path.GetRelativePath(config.RootDir, match);
                            foreach (var excludePattern in excludePatterns)
                            {
                                var excludePath = Path.Combine(config.RootDir, excludePattern);
                                if (MatchesPattern(match, excludePath) || MatchesPattern(relativePath, excludePattern))
                                {
                                    excluded = true;
                                    break;
                                }
                            }

                            if (excluded)
                            {
                                continue;
                            }

                            // Check against skipCompile patterns
                            var fileName = Path.GetFileName(match);
                            foreach (var skipPattern in skipCompilePatterns)
                            {
                                if (MatchesPattern(fileName, skipPattern))
                                {
                                    excluded = true;
                                    logger.Debug($"Skipping compilation: {fileName}");
                                    break;
                                }
                            }

                            if (!excluded)
                            {
                                nssFiles.Add(match);
                            }
                        }
                    }
                }

                logger.Info($"Found {nssFiles.Count} scripts to compile");

                if (nssFiles.Count == 0)
                {
                    logger.Warning("No scripts found to compile");
                    continue;
                }

                // Compile scripts using built-in compiler
                int compiledCount = 0;
                int errorCount = 0;

                foreach (var nssPath in nssFiles)
                {
                    try
                    {
                        var fileName = Path.GetFileName(nssPath);
                        var outputFile = Path.Combine(cacheDir, Path.ChangeExtension(fileName, ".ncs"));

                        // Read NSS source
                        var nssSource = File.ReadAllText(nssPath, Encoding.GetEncoding("windows-1252"));

                        // Determine library lookup paths (parent directory and common include locations)
                        var libraryLookup = new List<string>();
                        var parentDir = Path.GetDirectoryName(nssPath);
                        if (!string.IsNullOrEmpty(parentDir))
                        {
                            libraryLookup.Add(parentDir);
                        }
                        libraryLookup.Add(Path.Combine(config.RootDir, "include"));
                        libraryLookup.Add(Path.Combine(config.RootDir, "src", "include"));

                        // Compile NSS to NCS
                        var ncs = NCSAuto.CompileNss(nssSource, game, null, null, libraryLookup, null, false);

                        // Write NCS to output file
                        NCSAuto.WriteNcs(ncs, outputFile);

                        logger.Debug($"Compiled: {fileName} -> {Path.GetFileName(outputFile)}");
                        compiledCount++;
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"Compilation failed for {Path.GetFileName(nssPath)}: {ex.Message}");
                        if (logger.IsDebug)
                        {
                            logger.Debug($"Stack trace: {ex.StackTrace}");
                        }
                        errorCount++;
                    }
                }

                logger.Info($"Compiled {compiledCount} scripts, {errorCount} errors");

                if (errorCount > 0)
                {
                    logger.Warning($"Some scripts failed to compile. Check errors above.");
                }
            }
        }

        /// <summary>
        /// Glob files matching a pattern (supports * and ** wildcards).
        /// </summary>
        private static List<string> GlobFiles(string pattern)
        {
            var results = new List<string>();
            if (string.IsNullOrEmpty(pattern))
            {
                return results;
            }

            // Normalize path separators
            pattern = pattern.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);

            // Handle absolute and relative paths
            string baseDir;
            string searchPattern;
            bool recursive = false;

            if (Path.IsPathRooted(pattern))
            {
                // Absolute path
                var dir = Path.GetDirectoryName(pattern);
                searchPattern = Path.GetFileName(pattern);
                baseDir = dir ?? Path.GetPathRoot(pattern) ?? "";
            }
            else
            {
                // Relative path
                baseDir = Directory.GetCurrentDirectory();
                searchPattern = pattern;
            }

            // Check for recursive pattern (**)
            if (pattern.Contains("**"))
            {
                recursive = true;
                searchPattern = searchPattern.Replace("**", "*");
            }

            // Expand wildcards in directory path
            if (searchPattern.Contains(Path.DirectorySeparatorChar))
            {
                var parts = searchPattern.Split(Path.DirectorySeparatorChar);
                var dirPattern = string.Join(Path.DirectorySeparatorChar.ToString(), parts.Take(parts.Length - 1));
                searchPattern = parts[parts.Length - 1];

                // Recursively search directories
                if (recursive || dirPattern.Contains("*"))
                {
                    var dirs = GlobDirectories(baseDir, dirPattern, recursive);
                    foreach (var dir in dirs)
                    {
                        if (Directory.Exists(dir))
                        {
                            var files = Directory.GetFiles(dir, searchPattern, recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                            results.AddRange(files);
                        }
                    }
                    return results;
                }
                else
                {
                    baseDir = Path.Combine(baseDir, dirPattern);
                }
            }

            if (!Directory.Exists(baseDir))
            {
                return results;
            }

            // Get files matching pattern
            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var matchedFiles = Directory.GetFiles(baseDir, searchPattern, searchOption);
            results.AddRange(matchedFiles);

            return results;
        }

        /// <summary>
        /// Glob directories matching a pattern.
        /// </summary>
        private static List<string> GlobDirectories(string baseDir, string pattern, bool recursive)
        {
            var results = new List<string>();
            if (!Directory.Exists(baseDir))
            {
                return results;
            }

            var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var dirs = Directory.GetDirectories(baseDir, pattern, searchOption);
            results.AddRange(dirs);
            results.Add(baseDir); // Include base directory

            return results;
        }

        /// <summary>
        /// Check if a path matches a pattern (supports * and ? wildcards, similar to fnmatch).
        /// </summary>
        private static bool MatchesPattern(string path, string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return false;
            }

            // Convert pattern to regex
            var regexPattern = "^" + Regex.Escape(pattern)
                .Replace("\\*", ".*")
                .Replace("\\?", ".")
                .Replace("\\[", "[")
                .Replace("\\]", "]") + "$";

            try
            {
                return Regex.IsMatch(path, regexPattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}

