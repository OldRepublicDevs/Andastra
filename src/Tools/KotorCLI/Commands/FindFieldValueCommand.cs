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
    /// Installation-wide GFF field value reference search.
    /// </summary>
    public static class FindFieldValueCommand
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var findFieldValueCommand = new Command("find-field-value", "Search a KOTOR installation for GFF string/ResRef field values");
            var valueArgument = new Argument<string>("value");
            valueArgument.Description = "Field value to search for";
            findFieldValueCommand.Add(valueArgument);

            var installDirOption = Cli.Opt<string>("--install-dir", "KOTOR installation directory (or set KOTOR_PATH / K1_PATH)");
            findFieldValueCommand.Options.Add(installDirOption);

            var installationOption = Cli.Opt<string>("--installation", "Alias for --install-dir");
            findFieldValueCommand.Options.Add(installationOption);

            var overrideOnlyOption = Cli.Opt<bool>("--override-only", "Search override folder only");
            findFieldValueCommand.Options.Add(overrideOnlyOption);

            var noOverrideOption = Cli.Opt<bool>("--no-override", "Skip override folder");
            findFieldValueCommand.Options.Add(noOverrideOption);

            var noChitinOption = Cli.Opt<bool>("--no-chitin", "Skip chitin/BIF archives");
            findFieldValueCommand.Options.Add(noChitinOption);

            var noModulesOption = Cli.Opt<bool>("--no-modules", "Skip module capsules");
            findFieldValueCommand.Options.Add(noModulesOption);

            var partialOption = Cli.Opt<bool>("--partial", "Allow partial substring match");
            findFieldValueCommand.Options.Add(partialOption);

            var caseSensitiveOption = Cli.Opt<bool>("--case-sensitive", "Case-sensitive match");
            findFieldValueCommand.Options.Add(caseSensitiveOption);

            var jsonOption = Cli.Opt<bool>("--json", "Emit results as a single JSON object");
            findFieldValueCommand.Options.Add(jsonOption);

            var countOnlyOption = Cli.Opt<bool>("--count-only", "Print only the number of matches");
            findFieldValueCommand.Options.Add(countOnlyOption);

            var moduleGlobOption = Cli.Opt<string[]>("--module-glob", "Module filename glob (repeatable; e.g. tar_m02*)");
            findFieldValueCommand.Options.Add(moduleGlobOption);

            findFieldValueCommand.SetAction(parseResult =>
            {
                var value = parseResult.GetValue(valueArgument);
                var installDir = parseResult.GetValue(installDirOption) ?? parseResult.GetValue(installationOption);
                var overrideOnly = parseResult.GetValue(overrideOnlyOption);
                var noOverride = parseResult.GetValue(noOverrideOption);
                var noChitin = parseResult.GetValue(noChitinOption);
                var noModules = parseResult.GetValue(noModulesOption);
                var partial = parseResult.GetValue(partialOption);
                var caseSensitive = parseResult.GetValue(caseSensitiveOption);
                var json = parseResult.GetValue(jsonOption);
                var countOnly = parseResult.GetValue(countOnlyOption);
                var moduleGlob = parseResult.GetValue(moduleGlobOption);

                var logger = new StandardLogger();
                var exitCode = Execute(
                    value,
                    installDir,
                    overrideOnly,
                    noOverride,
                    noChitin,
                    noModules,
                    partial,
                    caseSensitive,
                    json,
                    countOnly,
                    moduleGlob,
                    logger);
                Environment.Exit(exitCode);
            });

            rootCommand.Add(findFieldValueCommand);
        }

        public static int Execute(
            string value,
            string installDir,
            bool partial,
            bool caseSensitive,
            ILogger logger)
        {
            return Execute(
                value,
                installDir,
                false,
                false,
                false,
                false,
                partial,
                caseSensitive,
                false,
                false,
                null,
                logger);
        }

        public static int Execute(
            string value,
            string installDir,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool partial,
            bool caseSensitive,
            ILogger logger)
        {
            return Execute(
                value,
                installDir,
                overrideOnly,
                noOverride,
                noChitin,
                noModules,
                partial,
                caseSensitive,
                false,
                false,
                null,
                logger);
        }

        public static int Execute(
            string value,
            string installDir,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool partial,
            bool caseSensitive,
            bool jsonOutput,
            bool countOnly,
            ILogger logger)
        {
            return Execute(
                value,
                installDir,
                overrideOnly,
                noOverride,
                noChitin,
                noModules,
                partial,
                caseSensitive,
                jsonOutput,
                countOnly,
                null,
                logger);
        }

        public static int Execute(
            string value,
            string installDir,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool partial,
            bool caseSensitive,
            bool jsonOutput,
            bool countOnly,
            string[] moduleGlobFilters,
            ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                logger.Error("Value is required.");
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
                caseSensitive,
                partial,
                moduleGlobFilters);

            List<ReferenceSearchResult> results = ReferenceFinder.FindFieldValueReferences(
                installation,
                value,
                null,
                options);

            return ReferenceSearchOutputFormatter.EmitReferenceResults(
                logger,
                value.Trim(),
                "field-value",
                results,
                jsonOutput,
                countOnly);
        }
    }
}
