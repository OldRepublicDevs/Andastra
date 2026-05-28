using System;
using System.CommandLine;
using System.IO;
using BioWare.Common;
using BioWare.Extract.Capsule;
using BioWare.Resource;
using KotorCLI.Logging;

namespace KotorCLI.Commands
{
    /// <summary>
    /// Cat command - Display resource contents to stdout.
    /// </summary>
    public static class CatCommand
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var catCommand = new Command("cat", "Display resource contents to stdout");
            var archiveArgument = new Argument<string>("archive");
            archiveArgument.Description = "Archive file (ERF, RIM)";
            catCommand.Add(archiveArgument);
            var resourceArgument = new Argument<string>("resource");
            resourceArgument.Description = "Resource reference name";
            catCommand.Add(resourceArgument);
            var typeOption = Cli.Opt<string>("--type", "Resource type extension (optional, will try to detect)");
            catCommand.Options.Add(typeOption);

            catCommand.SetAction(parseResult =>
            {
                var archive = parseResult.GetValue(archiveArgument);
                var resource = parseResult.GetValue(resourceArgument);
                var type = parseResult.GetValue(typeOption);

                var logger = new StandardLogger();
                int exitCode = Execute(archive, resource, type, logger);
                Environment.Exit(exitCode);
            });

            rootCommand.Add(catCommand);
        }

        public static int Execute(string archive, string resource, string type, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(archive))
            {
                logger.Error("Archive path is required");
                return 1;
            }

            if (string.IsNullOrWhiteSpace(resource))
            {
                logger.Error("Resource name is required");
                return 1;
            }

            if (!File.Exists(archive))
            {
                logger.Error("Archive file does not exist: " + archive);
                return 1;
            }

            try
            {
                ResourceType resourceType = ResourceType.INVALID;
                if (!string.IsNullOrWhiteSpace(type))
                {
                    resourceType = ResourceType.FromExtension(type.TrimStart('.'));
                }

                var capsule = new LazyCapsule(archive);
                string resName = resource.Trim();
                foreach (BioWare.Extract.FileResource fileResource in capsule.GetResources())
                {
                    if (fileResource == null)
                    {
                        continue;
                    }

                    if (!string.Equals(fileResource.ResName, resName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    if (resourceType != ResourceType.INVALID && !resourceType.IsInvalid &&
                        fileResource.ResType != resourceType)
                    {
                        continue;
                    }

                    byte[] data = fileResource.GetData();
                    if (data == null)
                    {
                        logger.Error("Resource data is empty: " + resName);
                        return 1;
                    }

                    using (Stream stdout = Console.OpenStandardOutput())
                    {
                        stdout.Write(data, 0, data.Length);
                    }

                    return 0;
                }

                logger.Error("Resource not found in archive: " + resName);
                return 1;
            }
            catch (Exception ex)
            {
                logger.Error("Cat failed: " + ex.Message);
                return 1;
            }
        }
    }
}
