using System;
using System.CommandLine;
using System.Collections.Generic;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Tools;
using KotorCLI.Logging;

namespace KotorCLI.Commands
{
    /// <summary>
    /// Validation and investigation commands.
    /// </summary>
    public static class ValidationCommands
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var checkTxiCmd = new Command("check-txi", "Check if TXI files exist for specific textures");
            var checkTxiInstall = Cli.Opt<string>("--installation", "Path to KOTOR installation");
            checkTxiInstall.Required = true;
            checkTxiCmd.Options.Add(checkTxiInstall);
            var texturesOption = Cli.Opt<string[]>("--textures", "Texture names to check (without extension)");
            texturesOption.Required = true;
            checkTxiCmd.Options.Add(texturesOption);
            checkTxiCmd.SetAction(parseResult =>
            {
                var install = parseResult.GetValue(checkTxiInstall);
                var textures = parseResult.GetValue(texturesOption);
                var logger = new StandardLogger();
                int exitCode = ExecuteCheckTxi(install, textures, logger);
                Environment.Exit(exitCode);
            });
            rootCommand.Add(checkTxiCmd);

            var check2DaCmd = new Command("check-2da", "Check if a 2DA file exists in installation");
            var check2DaName = Cli.Opt<string>("--2da", "2DA file name (without extension)");
            check2DaName.Required = true;
            check2DaCmd.Options.Add(check2DaName);
            var check2DaInstall = Cli.Opt<string>("--installation", "Path to KOTOR installation");
            check2DaInstall.Required = true;
            check2DaCmd.Options.Add(check2DaInstall);
            check2DaCmd.SetAction(parseResult =>
            {
                var name = parseResult.GetValue(check2DaName);
                var install = parseResult.GetValue(check2DaInstall);
                var logger = new StandardLogger();
                var exitCode = CheckTwoDAFile(name, install, logger);
                Environment.Exit(exitCode);
            });
            rootCommand.Add(check2DaCmd);
        }

        public static int ExecuteCheckTxi(string installPath, string[] textures, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(installPath))
            {
                logger.Error("Installation path cannot be empty");
                return 1;
            }

            if (textures == null || textures.Length == 0)
            {
                logger.Error("At least one texture name is required");
                return 1;
            }

            try
            {
                var installation = new Installation(installPath);
                var textureNames = new List<string>(textures);
                Dictionary<string, List<string>> results = Validation.CheckTxiFiles(installation, textureNames);

                int missingCount = 0;
                foreach (string textureName in textureNames)
                {
                    if (!results.TryGetValue(textureName, out List<string> paths) || paths == null || paths.Count == 0)
                    {
                        logger.Error("Missing TXI for texture: " + textureName);
                        missingCount++;
                        continue;
                    }

                    logger.Info("Found TXI for " + textureName + ":");
                    foreach (string path in paths)
                    {
                        logger.Info("  " + path);
                    }
                }

                return missingCount > 0 ? 1 : 0;
            }
            catch (Exception ex)
            {
                logger.Error("check-txi failed: " + ex.Message);
                return 1;
            }
        }

        private static int CheckTwoDAFile(string name, string installPath, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                logger.Error("2DA name cannot be empty");
                return 1;
            }

            if (string.IsNullOrWhiteSpace(installPath))
            {
                logger.Error("Installation path cannot be empty");
                return 1;
            }

            try
            {
                // Create installation instance to access resources
                var installation = new Installation(installPath);

                // Check if the 2DA file exists in the installation
                // Search in default order: Override, Modules, Chitin
                var result = installation.Resource(name, ResourceType.TwoDA);

                if (result != null && result.Data != null)
                {
                    logger.Info($"✓ 2DA file '{name}' exists in installation");
                    logger.Info($"  Location: {result.FilePath ?? "Embedded in archive"}");
                    logger.Info($"  Size: {result.Data.Length} bytes");

                    // Try to load and validate the 2DA structure
                    try
                    {
                        var twoDA = BioWare.Resource.Formats.TwoDA.TwoDA.FromBytes(result.Data);
                        logger.Info($"  Valid 2DA structure: {twoDA.GetWidth()} columns x {twoDA.GetHeight()} rows");

                        // Show first few headers if available
                        var headers = twoDA.GetHeaders();
                        if (headers.Count > 0)
                        {
                            var headerPreview = string.Join(", ", headers.GetRange(0, Math.Min(5, headers.Count)));
                            if (headers.Count > 5)
                            {
                                headerPreview += ", ...";
                            }
                            logger.Info($"  Headers: {headerPreview}");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Warning($"  Could not parse 2DA structure: {ex.Message}");
                        // Still consider it valid if we found the file, just warn about parsing issues
                    }

                    return 0;
                }

                logger.Error($"✗ 2DA file '{name}' not found in installation");
                logger.Info("  Searched locations: Override, Modules, Chitin");
                return 1;
            }
            catch (Exception ex)
            {
                logger.Error($"Failed to check 2DA file: {ex.Message}");
                return 1;
            }
        }
    }
}
