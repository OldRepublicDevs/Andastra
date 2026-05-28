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
                int exitCode = ExecuteCheck2da(install, name, logger);
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

        // Matching PyKotor: validation.check_2da_file via BioWare.Tools.Validation.Check2daFile
        public static int ExecuteCheck2da(string installPath, string twodaName, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(twodaName))
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
                var installation = new Installation(installPath);
                (bool found, List<string> paths) = Validation.Check2daFile(installation, twodaName);

                if (!found || paths == null || paths.Count == 0)
                {
                    logger.Error("Missing 2DA: " + twodaName);
                    return 1;
                }

                logger.Info("Found 2DA for " + twodaName + ":");
                foreach (string path in paths)
                {
                    logger.Info("  " + path);
                }

                LogTwoDAStructureIfPossible(paths, logger);
                return 0;
            }
            catch (Exception ex)
            {
                logger.Error("check-2da failed: " + ex.Message);
                return 1;
            }
        }

        private static void LogTwoDAStructureIfPossible(List<string> paths, ILogger logger)
        {
            if (paths == null || paths.Count == 0)
            {
                return;
            }

            string firstPath = paths[0];
            if (string.IsNullOrEmpty(firstPath) || !System.IO.File.Exists(firstPath))
            {
                return;
            }

            try
            {
                byte[] data = System.IO.File.ReadAllBytes(firstPath);
                var twoDA = BioWare.Resource.Formats.TwoDA.TwoDA.FromBytes(data);
                logger.Info("  Valid 2DA structure: " + twoDA.GetWidth() + " columns x " + twoDA.GetHeight() + " rows");

                var headers = twoDA.GetHeaders();
                if (headers.Count > 0)
                {
                    var headerPreview = string.Join(", ", headers.GetRange(0, Math.Min(5, headers.Count)));
                    if (headers.Count > 5)
                    {
                        headerPreview += ", ...";
                    }

                    logger.Info("  Headers: " + headerPreview);
                }
            }
            catch (Exception ex)
            {
                logger.Warning("  Could not parse 2DA structure: " + ex.Message);
            }
        }
    }
}
