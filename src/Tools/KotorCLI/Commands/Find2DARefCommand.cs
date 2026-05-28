using System;
using System.Collections.Generic;
using System.CommandLine;
using System.IO;
using BioWare.Extract;
using BioWare.Tools;
using KotorCLI.Logging;

namespace KotorCLI.Commands
{
    /// <summary>
    /// Installation-wide 2DA memory reference search (GFF fields indexing a 2DA row).
    /// </summary>
    public static class Find2DARefCommand
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var find2DaRefCommand = new Command("find-2da-ref", "Search a KOTOR installation for GFF references to a 2DA row");
            var twodaArgument = new Argument<string>("twoda");
            twodaArgument.Description = "2DA filename or resname (e.g. appearance or appearance.2da)";
            find2DaRefCommand.Add(twodaArgument);

            var rowArgument = new Argument<int>("row");
            rowArgument.Description = "Row index in the 2DA table";
            find2DaRefCommand.Add(rowArgument);

            var installDirOption = Cli.Opt<string>("--install-dir", "KOTOR installation directory (or set KOTOR_PATH / K1_PATH)");
            find2DaRefCommand.Options.Add(installDirOption);

            var installationOption = Cli.Opt<string>("--installation", "Alias for --install-dir");
            find2DaRefCommand.Options.Add(installationOption);

            var overrideOnlyOption = Cli.Opt<bool>("--override-only", "Search override folder only");
            find2DaRefCommand.Options.Add(overrideOnlyOption);

            var noOverrideOption = Cli.Opt<bool>("--no-override", "Skip override folder");
            find2DaRefCommand.Options.Add(noOverrideOption);

            var noChitinOption = Cli.Opt<bool>("--no-chitin", "Skip chitin/BIF archives");
            find2DaRefCommand.Options.Add(noChitinOption);

            var noModulesOption = Cli.Opt<bool>("--no-modules", "Skip module capsules");
            find2DaRefCommand.Options.Add(noModulesOption);

            var jsonOption = Cli.Opt<bool>("--json", "Emit results as a single JSON object");
            find2DaRefCommand.Options.Add(jsonOption);

            var countOnlyOption = Cli.Opt<bool>("--count-only", "Print only the number of matches");
            find2DaRefCommand.Options.Add(countOnlyOption);

            var moduleGlobOption = Cli.Opt<string[]>("--module-glob", "Module filename glob (repeatable; e.g. tar_m02*)");
            find2DaRefCommand.Options.Add(moduleGlobOption);

            var cacheFileOption = Cli.Opt<string>("--cache-file", "Load or save 2DA memory reference cache JSON at this path");
            find2DaRefCommand.Options.Add(cacheFileOption);

            var rebuildCacheOption = Cli.Opt<bool>("--rebuild-cache", "Rescan installation and overwrite --cache-file");
            find2DaRefCommand.Options.Add(rebuildCacheOption);

            var fullRowOption = Cli.Opt<bool>("--full-row", "Include row label field-value refs and row StrRef column refs (loads 2DA from installation when available)");
            find2DaRefCommand.Options.Add(fullRowOption);

            find2DaRefCommand.SetAction(parseResult =>
            {
                var twoda = parseResult.GetValue(twodaArgument);
                var row = parseResult.GetValue(rowArgument);
                var installDir = parseResult.GetValue(installDirOption) ?? parseResult.GetValue(installationOption);
                var overrideOnly = parseResult.GetValue(overrideOnlyOption);
                var noOverride = parseResult.GetValue(noOverrideOption);
                var noChitin = parseResult.GetValue(noChitinOption);
                var noModules = parseResult.GetValue(noModulesOption);
                var json = parseResult.GetValue(jsonOption);
                var countOnly = parseResult.GetValue(countOnlyOption);
                var moduleGlob = parseResult.GetValue(moduleGlobOption);
                var cacheFile = parseResult.GetValue(cacheFileOption);
                var rebuildCache = parseResult.GetValue(rebuildCacheOption);
                var fullRow = parseResult.GetValue(fullRowOption);

                var logger = new StandardLogger();
                var exitCode = Execute(
                    twoda,
                    row,
                    installDir,
                    overrideOnly,
                    noOverride,
                    noChitin,
                    noModules,
                    json,
                    countOnly,
                    moduleGlob,
                    cacheFile,
                    rebuildCache,
                    fullRow,
                    logger);
                Environment.Exit(exitCode);
            });

            rootCommand.Add(find2DaRefCommand);
        }

        public static int Execute(
            string twodaFilename,
            int rowIndex,
            string installDir,
            ILogger logger,
            bool overrideOnly = false,
            bool noOverride = false,
            bool noChitin = false,
            bool noModules = false)
        {
            return Execute(twodaFilename, rowIndex, installDir, overrideOnly, noOverride, noChitin, noModules, false, false, null, null, false, false, logger);
        }

        public static int Execute(
            string twodaFilename,
            int rowIndex,
            string installDir,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            ILogger logger)
        {
            return Execute(twodaFilename, rowIndex, installDir, overrideOnly, noOverride, noChitin, noModules, false, false, null, null, false, false, logger);
        }

        public static int Execute(
            string twodaFilename,
            int rowIndex,
            string installDir,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool jsonOutput,
            bool countOnly,
            ILogger logger)
        {
            return Execute(twodaFilename, rowIndex, installDir, overrideOnly, noOverride, noChitin, noModules, jsonOutput, countOnly, null, null, false, false, logger);
        }

        public static int Execute(
            string twodaFilename,
            int rowIndex,
            string installDir,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool jsonOutput,
            bool countOnly,
            string[] moduleGlobFilters,
            ILogger logger)
        {
            return Execute(twodaFilename, rowIndex, installDir, overrideOnly, noOverride, noChitin, noModules, jsonOutput, countOnly, moduleGlobFilters, null, false, false, logger);
        }

        public static int Execute(
            string twodaFilename,
            int rowIndex,
            string installDir,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool jsonOutput,
            bool countOnly,
            string[] moduleGlobFilters,
            string cacheFilePath,
            bool rebuildCache,
            ILogger logger)
        {
            return Execute(twodaFilename, rowIndex, installDir, overrideOnly, noOverride, noChitin, noModules, jsonOutput, countOnly, moduleGlobFilters, cacheFilePath, rebuildCache, false, logger);
        }

        public static int Execute(
            string twodaFilename,
            int rowIndex,
            string installDir,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool jsonOutput,
            bool countOnly,
            string[] moduleGlobFilters,
            string cacheFilePath,
            bool rebuildCache,
            bool fullRow,
            ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(twodaFilename))
            {
                logger.Error("2DA filename is required.");
                return 1;
            }

            if (rowIndex < 0)
            {
                logger.Error("Row index must be zero or greater.");
                return 1;
            }

            string installRoot = FindRefsCommand.ResolveInstallDirectory(installDir, logger);
            if (string.IsNullOrEmpty(installRoot))
            {
                logger.Error("Could not resolve installation directory. Pass --install-dir or set KOTOR_PATH.");
                return 1;
            }

            if (!Directory.Exists(installRoot))
            {
                logger.Error("Installation directory does not exist: " + installRoot);
                return 1;
            }

            Installation installation;
            try
            {
                installation = new Installation(installRoot);
            }
            catch (Exception ex)
            {
                logger.Error("Failed to open installation: " + ex.Message);
                return 1;
            }

            ReferenceSearchOptions options = FindRefsCommand.BuildSearchOptions(
                overrideOnly,
                noOverride,
                noChitin,
                noModules,
                caseSensitive: false,
                partialMatch: false,
                moduleGlobFilters);

            TwoDAMemoryReferenceCache twodaCache = null;
            if (!string.IsNullOrWhiteSpace(cacheFilePath))
            {
                if (File.Exists(cacheFilePath) && !rebuildCache)
                {
                    try
                    {
                        twodaCache = TwoDAMemoryReferenceCacheIO.Load(cacheFilePath, installation.Game, validateGame: true);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Failed to load 2DA memory cache: " + ex.Message);
                        return 1;
                    }
                }
                else
                {
                    try
                    {
                        twodaCache = ReferenceCacheHelpers.BuildTwoDAMemoryReferenceCache(
                            installation,
                            logger.Info,
                            options);
                        TwoDAMemoryReferenceCacheIO.Save(cacheFilePath, twodaCache);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("Failed to build or save 2DA memory cache: " + ex.Message);
                        return 1;
                    }
                }
            }

            List<ReferenceSearchResult> results;
            if (fullRow)
            {
                results = ReferenceCacheHelpers.CollectTwoDARowReferences(
                    installation,
                    twodaFilename,
                    rowIndex,
                    null,
                    twodaCache,
                    null,
                    options);
            }
            else
            {
                results = ReferenceCacheHelpers.Find2DAMemoryReferences(
                    installation,
                    twodaFilename,
                    rowIndex,
                    twodaCache,
                    null,
                    options);
            }

            string needle = twodaFilename.Trim() + ":" + rowIndex;
            return ReferenceSearchOutputFormatter.EmitReferenceResults(
                logger,
                needle,
                "2da-ref",
                results,
                jsonOutput,
                countOnly);
        }
    }
}
