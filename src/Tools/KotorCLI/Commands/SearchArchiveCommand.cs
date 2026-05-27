using System;
using System.CommandLine;
using System.Collections.Generic;
using System.IO;
using KotorCLI.Logging;

namespace KotorCLI.Commands
{
    /// <summary>
    /// Search-archive command - Search for resources in archive files.
    /// </summary>
    public static class SearchArchiveCommand
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var searchArchiveCommand = new Command("search-archive", "Search for resources in archive files");
            var fileOption = Cli.Opt<string>("--file", "Archive file to search");
            fileOption.Required = true;
            searchArchiveCommand.Options.Add(fileOption);
            var patternArgument = new Argument<string>("pattern");
            patternArgument.Description = "Search pattern (supports wildcards)";
            searchArchiveCommand.Add(patternArgument);
            var caseSensitiveOption = Cli.Opt<bool>("--case-sensitive", "Case-sensitive search");
            searchArchiveCommand.Options.Add(caseSensitiveOption);
            var searchContentOption = Cli.Opt<bool>("--content", "Search in resource content (not just names)");
            searchArchiveCommand.Options.Add(searchContentOption);

            searchArchiveCommand.SetAction(parseResult =>
            {
                var file = parseResult.GetValue(fileOption);
                var pattern = parseResult.GetValue(patternArgument);
                var caseSensitive = parseResult.GetValue(caseSensitiveOption);
                var searchContent = parseResult.GetValue(searchContentOption);

                var logger = new StandardLogger();
                int exitCode = Execute(file, pattern, caseSensitive, searchContent, logger);
                Environment.Exit(exitCode);
            });

            rootCommand.Add(searchArchiveCommand);
        }

        public static int Execute(
            string file,
            string pattern,
            bool caseSensitive,
            bool searchContent,
            ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                logger.Error("Archive file path is required");
                return 1;
            }

            if (string.IsNullOrWhiteSpace(pattern))
            {
                logger.Error("Search pattern is required");
                return 1;
            }

            if (!File.Exists(file))
            {
                logger.Error("Archive file does not exist: " + file);
                return 1;
            }

            try
            {
                List<ArchiveCommandHelpers.ArchiveResourceEntry> resources =
                    ArchiveCommandHelpers.ReadArchiveResources(file, logger);

                int matches = 0;
                foreach (ArchiveCommandHelpers.ArchiveResourceEntry entry in resources)
                {
                    string resName = entry.ResRef ?? string.Empty;
                    string ext = entry.ResType != null && !entry.ResType.IsInvalid
                        ? entry.ResType.Extension
                        : "bin";
                    string fullName = string.IsNullOrEmpty(ext) ? resName : resName + "." + ext;

                    bool nameMatch = ArchiveCommandHelpers.MatchesFilter(resName, pattern, caseSensitive) ||
                                     ArchiveCommandHelpers.MatchesFilter(fullName, pattern, caseSensitive);
                    bool contentMatch = searchContent &&
                                          ArchiveCommandHelpers.ContentMatches(entry.Data, pattern, caseSensitive);

                    if (!nameMatch && !contentMatch)
                    {
                        continue;
                    }

                    logger.Info(fullName);
                    matches++;
                }

                if (matches == 0)
                {
                    logger.Error("No resources matched pattern in archive: " + file);
                    return 1;
                }

                return 0;
            }
            catch (Exception ex)
            {
                logger.Error("search-archive failed: " + ex.Message);
                return 1;
            }
        }
    }
}
