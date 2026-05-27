using System;
using System.CommandLine;
using System.Collections.Generic;
using System.IO;
using KotorCLI.Logging;

namespace KotorCLI.Commands
{
    /// <summary>
    /// List-archive command - List contents of archive files (KEY/BIF, RIM, ERF, etc.).
    /// </summary>
    public static class ListArchiveCommand
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var listArchiveCommand = new Command("list-archive", "List contents of archive files (KEY/BIF, RIM, ERF, etc.)");
            var fileOption = Cli.Opt<string>("--file", "Archive file to list");
            fileOption.Required = true;
            listArchiveCommand.Options.Add(fileOption);
            var verboseOption = Cli.Opt<bool>("--verbose", "Show detailed resource information");
            listArchiveCommand.Options.Add(verboseOption);
            var filterOption = Cli.Opt<string>("--filter", "Filter resources by name pattern (supports wildcards)");
            listArchiveCommand.Options.Add(filterOption);

            listArchiveCommand.SetAction(parseResult =>
            {
                var file = parseResult.GetValue(fileOption);
                var verbose = parseResult.GetValue(verboseOption);
                var filter = parseResult.GetValue(filterOption);

                var logger = new StandardLogger();
                int exitCode = Execute(file, verbose, filter, logger);
                Environment.Exit(exitCode);
            });

            rootCommand.Add(listArchiveCommand);
        }

        public static int Execute(string file, bool verbose, string filter, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                logger.Error("Archive file path is required");
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

                if (resources.Count == 0)
                {
                    logger.Error("No resources found in archive: " + file);
                    return 1;
                }

                int listed = 0;
                foreach (ArchiveCommandHelpers.ArchiveResourceEntry entry in resources)
                {
                    string resName = entry.ResRef ?? string.Empty;
                    string ext = entry.ResType != null && !entry.ResType.IsInvalid
                        ? entry.ResType.Extension
                        : "bin";
                    string fullName = string.IsNullOrEmpty(ext) ? resName : resName + "." + ext;

                    if (!ArchiveCommandHelpers.MatchesFilter(resName, filter) &&
                        !ArchiveCommandHelpers.MatchesFilter(fullName, filter))
                    {
                        continue;
                    }

                    if (verbose)
                    {
                        logger.Info(fullName + " (" + entry.Size + " bytes)");
                    }
                    else
                    {
                        logger.Info(fullName);
                    }

                    listed++;
                }

                if (listed == 0)
                {
                    logger.Error("No resources matched filter in archive: " + file);
                    return 1;
                }

                return 0;
            }
            catch (Exception ex)
            {
                logger.Error("list-archive failed: " + ex.Message);
                return 1;
            }
        }
    }
}
