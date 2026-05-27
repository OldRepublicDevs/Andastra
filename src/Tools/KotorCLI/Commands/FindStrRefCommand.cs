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
    /// Installation-wide StrRef reference search (2DA, SSF, GFF, NCS).
    /// </summary>
    public static class FindStrRefCommand
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var findStrRefCommand = new Command("find-strref", "Search a KOTOR installation for references to a TLK StrRef");
            var strrefArgument = new Argument<int>("strref");
            strrefArgument.Description = "TLK string reference number to search for";
            findStrRefCommand.Add(strrefArgument);

            var installDirOption = Cli.Opt<string>("--install-dir", "KOTOR installation directory (or set KOTOR_PATH / K1_PATH)");
            findStrRefCommand.Options.Add(installDirOption);

            var installationOption = Cli.Opt<string>("--installation", "Alias for --install-dir");
            findStrRefCommand.Options.Add(installationOption);

            var overrideOnlyOption = Cli.Opt<bool>("--override-only", "Search override folder only");
            findStrRefCommand.Options.Add(overrideOnlyOption);

            var noOverrideOption = Cli.Opt<bool>("--no-override", "Skip override folder");
            findStrRefCommand.Options.Add(noOverrideOption);

            var noChitinOption = Cli.Opt<bool>("--no-chitin", "Skip chitin/BIF archives");
            findStrRefCommand.Options.Add(noChitinOption);

            var noModulesOption = Cli.Opt<bool>("--no-modules", "Skip module capsules");
            findStrRefCommand.Options.Add(noModulesOption);

            var noNcsOption = Cli.Opt<bool>("--no-ncs", "Skip NCS bytecode (CONSTI) scanning");
            findStrRefCommand.Options.Add(noNcsOption);

            var ncsStrRefMinOption = new Option<int?>("--ncs-strref-min");
            ncsStrRefMinOption.Description = "Minimum CONSTI value indexed as plausible StrRef in cache scans (default 100); explicit queries still match any value";
            findStrRefCommand.Options.Add(ncsStrRefMinOption);

            var jsonOption = Cli.Opt<bool>("--json", "Emit results as a single JSON object");
            findStrRefCommand.Options.Add(jsonOption);

            var countOnlyOption = Cli.Opt<bool>("--count-only", "Print only the number of matches");
            findStrRefCommand.Options.Add(countOnlyOption);

            var moduleGlobOption = Cli.Opt<string[]>("--module-glob", "Module filename glob (repeatable; e.g. tar_m02*)");
            findStrRefCommand.Options.Add(moduleGlobOption);

            findStrRefCommand.SetAction(parseResult =>
            {
                var strref = parseResult.GetValue(strrefArgument);
                var installDir = parseResult.GetValue(installDirOption) ?? parseResult.GetValue(installationOption);
                var overrideOnly = parseResult.GetValue(overrideOnlyOption);
                var noOverride = parseResult.GetValue(noOverrideOption);
                var noChitin = parseResult.GetValue(noChitinOption);
                var noModules = parseResult.GetValue(noModulesOption);
                var noNcs = parseResult.GetValue(noNcsOption);
                var ncsStrRefMin = parseResult.GetValue(ncsStrRefMinOption);
                var json = parseResult.GetValue(jsonOption);
                var countOnly = parseResult.GetValue(countOnlyOption);
                var moduleGlob = parseResult.GetValue(moduleGlobOption);

                var logger = new StandardLogger();
                var exitCode = Execute(
                    strref,
                    installDir,
                    overrideOnly,
                    noOverride,
                    noChitin,
                    noModules,
                    noNcs,
                    ncsStrRefMin,
                    json,
                    countOnly,
                    moduleGlob,
                    logger);
                Environment.Exit(exitCode);
            });

            rootCommand.Add(findStrRefCommand);
        }

        public static int Execute(int strref, string installDir, ILogger logger)
        {
            return Execute(strref, installDir, false, false, false, false, false, null, false, false, null, logger);
        }

        public static int Execute(
            int strref,
            string installDir,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            ILogger logger)
        {
            return Execute(strref, installDir, overrideOnly, noOverride, noChitin, noModules, false, null, false, false, null, logger);
        }

        public static int Execute(
            int strref,
            string installDir,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool noNcs,
            ILogger logger)
        {
            return Execute(strref, installDir, overrideOnly, noOverride, noChitin, noModules, noNcs, null, false, false, null, logger);
        }

        public static int Execute(
            int strref,
            string installDir,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool noNcs,
            int? ncsStrRefMin,
            ILogger logger)
        {
            return Execute(strref, installDir, overrideOnly, noOverride, noChitin, noModules, noNcs, ncsStrRefMin, false, false, null, logger);
        }

        public static int Execute(
            int strref,
            string installDir,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool noNcs,
            int? ncsStrRefMin,
            bool jsonOutput,
            bool countOnly,
            ILogger logger)
        {
            return Execute(strref, installDir, overrideOnly, noOverride, noChitin, noModules, noNcs, ncsStrRefMin, jsonOutput, countOnly, null, logger);
        }

        public static int Execute(
            int strref,
            string installDir,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool noNcs,
            int? ncsStrRefMin,
            bool jsonOutput,
            bool countOnly,
            string[] moduleGlobFilters,
            ILogger logger)
        {
            if (strref < 0)
            {
                logger.Error("StrRef must be zero or greater.");
                return 1;
            }

            if (ncsStrRefMin.HasValue && ncsStrRefMin.Value < 0)
            {
                logger.Error("--ncs-strref-min must be zero or greater.");
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
            options.IncludeNcsStrRefScan = !noNcs;
            options.NcsStrRefCandidateMinimum = ncsStrRefMin;

            List<StrRefSearchResult> strrefResults = ReferenceCacheHelpers.FindStrRefReferences(
                installation,
                strref,
                null,
                null,
                options);

            List<ReferenceSearchResult> results = ReferenceCacheHelpers.ConvertToReferenceSearchResults(
                strrefResults,
                strref);

            return ReferenceSearchOutputFormatter.EmitReferenceResults(
                logger,
                strref.ToString(),
                "strref",
                results,
                jsonOutput,
                countOnly);
        }
    }
}
