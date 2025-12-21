using System;
using System.IO;
using System.Linq;
using System.CommandLine;
using KotorCLI.Logging;
using Andastra.Parsing.Formats.KEY;
using Andastra.Parsing.Resource.Formats.BIF;
using Andastra.Parsing;
using Andastra.Parsing.Common;
using Andastra.Parsing.Resource;

namespace KotorCLI.Commands
{
    public static class KeyPackCommand
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var keyPackCommand = new Command("key-pack", "Create KEY file from directory containing BIF files");
            keyPackCommand.AddAlias("create-key");
            var directoryOption = new Option<string>(new[] { "-d", "--directory" }, "Directory containing BIF files") { IsRequired = true };
            keyPackCommand.AddOption(directoryOption);
            var outputOption = new Option<string>(new[] { "-o", "--output" }, "Output KEY file") { IsRequired = true };
            keyPackCommand.AddOption(outputOption);
            keyPackCommand.SetHandler((string directory, string output) =>
            {
                var logger = new StandardLogger();
                try
                {
                    ExecuteKeyPack(directory, output, logger);
                }
                catch (Exception ex)
                {
                    logger.Error($"Error creating KEY file: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        logger.Error($"Inner exception: {ex.InnerException.Message}");
                    }
                    Environment.Exit(1);
                }
            }, directoryOption, outputOption);
            rootCommand.AddCommand(keyPackCommand);
        }

        private static void ExecuteKeyPack(string directory, string outputPath, StandardLogger logger)
        {
            // Validate directory exists
            if (!Directory.Exists(directory))
            {
                throw new DirectoryNotFoundException($"Directory not found: {directory}");
            }

            logger.Info($"Scanning directory for BIF files: {directory}");

            // Find all BIF files in the directory
            string[] bifFiles = Directory.GetFiles(directory, "*.bif", SearchOption.AllDirectories)
                .Concat(Directory.GetFiles(directory, "*.bzf", SearchOption.AllDirectories))
                .OrderBy(f => f)
                .ToArray();

            if (bifFiles.Length == 0)
            {
                throw new InvalidOperationException($"No BIF files found in directory: {directory}");
            }

            logger.Info($"Found {bifFiles.Length} BIF file(s)");

            // Create new KEY file
            KEY key = new KEY();
            
            // Set build date to current date
            DateTime now = DateTime.Now;
            key.BuildYear = now.Year - 1900;
            key.BuildDay = now.DayOfYear;

            // Process each BIF file
            for (int bifIndex = 0; bifIndex < bifFiles.Length; bifIndex++)
            {
                string bifPath = bifFiles[bifIndex];
                logger.Info($"Processing BIF file {bifIndex + 1}/{bifFiles.Length}: {Path.GetFileName(bifPath)}");

                // Get file size
                FileInfo fileInfo = new FileInfo(bifPath);
                long fileSize = fileInfo.Length;

                // Get relative path from directory (for KEY file entry)
                string relativePath = Path.GetRelativePath(directory, bifPath);
                // Normalize path separators to forward slashes (KEY format uses forward slashes)
                relativePath = relativePath.Replace('\\', '/');

                // Load BIF file
                BIF bif;
                using (var bifReader = new BIFBinaryReader(bifPath))
                {
                    bif = bifReader.Load();
                }

                // Add BIF entry to KEY
                BifEntry bifEntry = key.AddBif(relativePath, (int)fileSize, 0x0001); // 0x0001 = HD drive flag

                logger.Info($"  BIF contains {bif.Resources.Count} resource(s)");

                // Add KEY entries for each resource in the BIF
                for (int resIndex = 0; resIndex < bif.Resources.Count; resIndex++)
                {
                    BIFResource bifResource = bif.Resources[resIndex];

                    // Determine ResRef: use resource's ResRef if available and not blank, otherwise generate one
                    string resref;
                    if (bifResource.ResRef != null && !bifResource.ResRef.IsBlank())
                    {
                        resref = bifResource.ResRef.ToString();
                    }
                    else
                    {
                        // Generate ResRef based on resource index
                        // Format: resource_00001, resource_00002, etc.
                        resref = $"resource_{resIndex + 1:D5}";
                    }

                    // Get resource type
                    ResourceType resType = bifResource.ResType ?? ResourceType.INVALID;

                    // Add KEY entry
                    key.AddKeyEntry(resref, resType, bifIndex, resIndex);
                }
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Write KEY file
            logger.Info($"Writing KEY file: {outputPath}");
            using (var keyWriter = new KEYBinaryWriter(key, outputPath))
            {
                keyWriter.Write();
            }

            logger.Info($"Successfully created KEY file with {key.BifEntries.Count} BIF entry(ies) and {key.KeyEntries.Count} resource entry(ies)");
        }
    }
}

