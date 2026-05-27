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
    /// Installation-wide reference search (script, tag, template, conversation ResRef).
    /// </summary>
    public static class FindRefsCommand
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var findRefsCommand = new Command("find-refs", "Search a KOTOR installation for references to a script, tag, template, or conversation");
            var needleArgument = new Argument<string>("needle");
            needleArgument.Description = "Value to search for (ResRef or tag string)";
            findRefsCommand.Add(needleArgument);

            var installDirOption = Cli.Opt<string>("--install-dir", "KOTOR installation directory (or set KOTOR_PATH / K1_PATH)");
            findRefsCommand.Options.Add(installDirOption);

            var installationOption = Cli.Opt<string>("--installation", "Alias for --install-dir");
            findRefsCommand.Options.Add(installationOption);

            var typeOption = Cli.Opt<string>("--type", "Reference kind: script, tag, template, or conversation (default: script)");
            findRefsCommand.Options.Add(typeOption);

            var overrideOnlyOption = Cli.Opt<bool>("--override-only", "Search override folder only");
            findRefsCommand.Options.Add(overrideOnlyOption);

            var noOverrideOption = Cli.Opt<bool>("--no-override", "Skip override folder");
            findRefsCommand.Options.Add(noOverrideOption);

            var noChitinOption = Cli.Opt<bool>("--no-chitin", "Skip chitin/BIF archives");
            findRefsCommand.Options.Add(noChitinOption);

            var noModulesOption = Cli.Opt<bool>("--no-modules", "Skip module capsules");
            findRefsCommand.Options.Add(noModulesOption);

            var caseSensitiveOption = Cli.Opt<bool>("--case-sensitive", "Case-sensitive match");
            findRefsCommand.Options.Add(caseSensitiveOption);

            var partialOption = Cli.Opt<bool>("--partial", "Allow partial substring match");
            findRefsCommand.Options.Add(partialOption);

            var jsonOption = Cli.Opt<bool>("--json", "Emit results as a single JSON object");
            findRefsCommand.Options.Add(jsonOption);

            var countOnlyOption = Cli.Opt<bool>("--count-only", "Print only the number of matches");
            findRefsCommand.Options.Add(countOnlyOption);

            findRefsCommand.SetAction(parseResult =>
            {
                var needle = parseResult.GetValue(needleArgument);
                var installDir = parseResult.GetValue(installDirOption) ?? parseResult.GetValue(installationOption);
                var type = parseResult.GetValue(typeOption);
                var overrideOnly = parseResult.GetValue(overrideOnlyOption);
                var noOverride = parseResult.GetValue(noOverrideOption);
                var noChitin = parseResult.GetValue(noChitinOption);
                var noModules = parseResult.GetValue(noModulesOption);
                var caseSensitive = parseResult.GetValue(caseSensitiveOption);
                var partial = parseResult.GetValue(partialOption);
                var json = parseResult.GetValue(jsonOption);
                var countOnly = parseResult.GetValue(countOnlyOption);

                var logger = new StandardLogger();
                var exitCode = Execute(
                    needle,
                    installDir,
                    type,
                    overrideOnly,
                    noOverride,
                    noChitin,
                    noModules,
                    caseSensitive,
                    partial,
                    json,
                    countOnly,
                    logger);
                Environment.Exit(exitCode);
            });

            rootCommand.Add(findRefsCommand);
        }

        public static int Execute(
            string needle,
            string installDir,
            string referenceType,
            bool overrideOnly,
            bool noChitin,
            bool noModules,
            bool caseSensitive,
            bool partialMatch,
            ILogger logger)
        {
            return Execute(
                needle,
                installDir,
                referenceType,
                overrideOnly,
                noOverride: false,
                noChitin,
                noModules,
                caseSensitive,
                partialMatch,
                jsonOutput: false,
                countOnly: false,
                logger);
        }

        public static int Execute(
            string needle,
            string installDir,
            string referenceType,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool caseSensitive,
            bool partialMatch,
            ILogger logger)
        {
            return Execute(
                needle,
                installDir,
                referenceType,
                overrideOnly,
                noOverride,
                noChitin,
                noModules,
                caseSensitive,
                partialMatch,
                jsonOutput: false,
                countOnly: false,
                logger);
        }

        public static int Execute(
            string needle,
            string installDir,
            string referenceType,
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool caseSensitive,
            bool partialMatch,
            bool jsonOutput,
            bool countOnly,
            ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(needle))
            {
                logger.Error("Needle argument is required.");
                return 1;
            }

            string installRoot = ResolveInstallDirectory(installDir, logger);
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

            string normalizedType = (referenceType ?? "script").Trim().ToLowerInvariant();
            if (!IsSupportedType(normalizedType))
            {
                logger.Error("Unsupported --type value. Use script, tag, template, or conversation.");
                return 1;
            }

            var options = BuildSearchOptions(overrideOnly, noOverride, noChitin, noModules, caseSensitive, partialMatch);

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

            List<ReferenceSearchResult> results = FindReferences(installation, normalizedType, needle.Trim(), options);
            if (results == null || results.Count == 0)
            {
                if (jsonOutput)
                {
                    logger.Info(ReferenceSearchOutputFormatter.FormatJson(needle.Trim(), normalizedType, results));
                }
                else if (countOnly)
                {
                    logger.Info(ReferenceSearchOutputFormatter.FormatCount(0));
                }
                else
                {
                    logger.Info("No references found.");
                }

                return 1;
            }

            if (jsonOutput)
            {
                logger.Info(ReferenceSearchOutputFormatter.FormatJson(needle.Trim(), normalizedType, results));
                return 0;
            }

            if (countOnly)
            {
                logger.Info(ReferenceSearchOutputFormatter.FormatCount(results.Count));
                return 0;
            }

            foreach (ReferenceSearchResult result in results)
            {
                logger.Info(result.DisplayLabel);
            }

            logger.Info("Found " + results.Count + " reference(s).");
            return 0;
        }

        internal static ReferenceSearchOptions BuildSearchOptions(
            bool overrideOnly,
            bool noOverride,
            bool noChitin,
            bool noModules,
            bool caseSensitive,
            bool partialMatch)
        {
            var options = new ReferenceSearchOptions
            {
                SearchOverride = !noOverride,
                SearchModules = !noModules,
                SearchChitin = !noChitin,
                CaseSensitive = caseSensitive,
                PartialMatch = partialMatch
            };

            if (overrideOnly)
            {
                options.SearchModules = false;
                options.SearchChitin = false;
                options.SearchOverride = true;
            }

            return options;
        }

        internal static List<ReferenceSearchResult> FindReferences(
            Installation installation,
            string referenceType,
            string needle,
            ReferenceSearchOptions options)
        {
            switch (referenceType)
            {
                case "script":
                    return ReferenceFinder.FindScriptReferences(installation, needle, options);
                case "tag":
                    return ReferenceFinder.FindTagReferences(installation, needle, options);
                case "template":
                    return ReferenceFinder.FindTemplateResRefReferences(installation, needle, options);
                case "conversation":
                    return ReferenceFinder.FindConversationResRefReferences(installation, needle, options);
                default:
                    return new List<ReferenceSearchResult>();
            }
        }

        internal static bool IsSupportedType(string referenceType)
        {
            return referenceType == "script"
                || referenceType == "tag"
                || referenceType == "template"
                || referenceType == "conversation";
        }

        internal static string ResolveInstallDirectory(string installDir, ILogger logger)
        {
            if (!string.IsNullOrWhiteSpace(installDir))
            {
                return Path.GetFullPath(installDir);
            }

            string kotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            if (string.IsNullOrEmpty(kotorPath))
            {
                kotorPath = Environment.GetEnvironmentVariable("K1_PATH");
            }

            if (!string.IsNullOrEmpty(kotorPath) && Directory.Exists(kotorPath))
            {
                return Path.GetFullPath(kotorPath);
            }

            if (logger != null)
            {
                logger.Warning("No --install-dir specified and KOTOR_PATH is unset.");
            }

            return null;
        }
    }
}
