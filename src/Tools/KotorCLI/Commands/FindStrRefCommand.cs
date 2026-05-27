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
    /// Installation-wide StrRef reference search (2DA, SSF, GFF).
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

            findStrRefCommand.SetAction(parseResult =>
            {
                var strref = parseResult.GetValue(strrefArgument);
                var installDir = parseResult.GetValue(installDirOption) ?? parseResult.GetValue(installationOption);
                var overrideOnly = parseResult.GetValue(overrideOnlyOption);
                var noOverride = parseResult.GetValue(noOverrideOption);
                var noChitin = parseResult.GetValue(noChitinOption);
                var noModules = parseResult.GetValue(noModulesOption);

                var logger = new StandardLogger();
                var exitCode = Execute(
                    strref,
                    installDir,
                    overrideOnly,
                    noOverride,
                    noChitin,
                    noModules,
                    logger);
                Environment.Exit(exitCode);
            });

            rootCommand.Add(findStrRefCommand);
        }

        public static int Execute(
            int strref,
            string installDir,
            ILogger logger,
            bool overrideOnly = false,
            bool noOverride = false,
            bool noChitin = false,
            bool noModules = false)
        {
            return Execute(strref, installDir, overrideOnly, noOverride, noChitin, noModules, logger);
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
            if (strref < 0)
            {
                logger.Error("StrRef must be zero or greater.");
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
                partialMatch: false);

            List<StrRefSearchResult> strrefResults = ReferenceCacheHelpers.FindStrRefReferences(
                installation,
                strref,
                null,
                null,
                options);

            List<ReferenceSearchResult> results = ReferenceCacheHelpers.ConvertToReferenceSearchResults(
                strrefResults,
                strref);

            if (results == null || results.Count == 0)
            {
                logger.Info("No references found.");
                return 1;
            }

            foreach (ReferenceSearchResult result in results)
            {
                logger.Info(result.DisplayLabel);
            }

            logger.Info("Found " + results.Count + " reference(s).");
            return 0;
        }
    }
}
