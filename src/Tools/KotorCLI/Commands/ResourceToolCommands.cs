using System;
using System.CommandLine;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.TPC;
using BioWare.Tools;
using KotorCLI.Logging;

namespace KotorCLI.Commands
{
    /// <summary>
    /// Resource tool commands (texture-convert, sound-convert, model-convert).
    /// </summary>
    public static class ResourceToolCommands
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var textureCmd = new Command("texture-convert", "Convert texture files (TPC↔TGA)");
            var textureInput = new Argument<string>("input");
            textureInput.Description = "Input texture file (TPC or TGA)";
            textureCmd.Add(textureInput);
            var textureOutput = Cli.Opt<string>("--output", "Output texture file");
            textureCmd.Options.Add(textureOutput);
            var txiOption = Cli.Opt<string>("--txi", "TXI file path (for TPC↔TGA conversion)");
            textureCmd.Options.Add(txiOption);
            textureCmd.SetAction(parseResult =>
            {
                var input = parseResult.GetValue(textureInput);
                var output = parseResult.GetValue(textureOutput);
                var txi = parseResult.GetValue(txiOption);
                var logger = new StandardLogger();
                int exitCode = ExecuteTextureConvert(input, output, txi, logger);
                Environment.Exit(exitCode);
            });
            rootCommand.Add(textureCmd);

            var soundCmd = new Command("sound-convert", "Convert sound files (WAV↔clean WAV)");
            var soundInput = new Argument<string>("input");
            soundInput.Description = "Input WAV file";
            soundCmd.Add(soundInput);
            var soundOutput = Cli.Opt<string>("--output", "Output WAV file");
            soundCmd.Options.Add(soundOutput);
            var forceOverwrite = Cli.Opt<bool>("--force", "Force overwrite output file if it exists");
            soundCmd.Options.Add(forceOverwrite);
            soundCmd.SetAction(parseResult =>
            {
                var input = parseResult.GetValue(soundInput);
                var output = parseResult.GetValue(soundOutput);
                var force = parseResult.GetValue(forceOverwrite);
                var logger = new StandardLogger();

                try
                {
                    // Validate input file
                    if (string.IsNullOrEmpty(input))
                    {
                        logger.Error("Input file path is required");
                        Environment.Exit(1);
                    }

                    if (!File.Exists(input))
                    {
                        logger.Error($"Input file does not exist: {input}");
                        Environment.Exit(1);
                    }

                    // Determine output file path
                    string outputPath = output;
                    if (string.IsNullOrEmpty(outputPath))
                    {
                        // Generate output filename by adding "_clean" suffix before extension
                        string inputDir = Path.GetDirectoryName(input) ?? "";
                        string inputName = Path.GetFileNameWithoutExtension(input);
                        string inputExt = Path.GetExtension(input);
                        outputPath = Path.Combine(inputDir, $"{inputName}_clean{inputExt}");
                    }

                    // Check if output file exists
                    if (File.Exists(outputPath) && !force)
                    {
                        logger.Error($"Output file already exists: {outputPath}. Use --force to overwrite.");
                        Environment.Exit(1);
                    }

                    logger.Info($"Converting sound: {input} -> {outputPath}");
                    ResourceConversions.ConvertWavToClean(input, outputPath);
                    logger.Info("Sound conversion completed successfully");
                }
                catch (Exception ex)
                {
                    logger.Error($"Sound conversion failed: {ex.Message}");
                    logger.Error($"Stack trace: {ex.StackTrace}");
                    Environment.Exit(1);
                }
            });
            rootCommand.Add(soundCmd);

            var modelCmd = new Command("model-convert", "Convert model files (MDL↔ASCII)");
            var modelInput = new Argument<string>("input");
            modelInput.Description = "Input MDL file";
            modelCmd.Add(modelInput);
            var modelOutput = Cli.Opt<string>("--output", "Output MDL file");
            modelCmd.Options.Add(modelOutput);
            var toAsciiOption = Cli.Opt<bool>("--to-ascii", "Convert to ASCII format");
            modelCmd.Options.Add(toAsciiOption);
            var mdxOption = Cli.Opt<string>("--mdx", "MDX file path (for binary MDL conversion)");
            modelCmd.Options.Add(mdxOption);
            modelCmd.SetAction(parseResult =>
            {
                var input = parseResult.GetValue(modelInput);
                var output = parseResult.GetValue(modelOutput);
                var toAscii = parseResult.GetValue(toAsciiOption);
                var mdx = parseResult.GetValue(mdxOption);
                var logger = new StandardLogger();
                int exitCode = ExecuteModelConvert(input, output, toAscii, mdx, logger);
                Environment.Exit(exitCode);
            });
            rootCommand.Add(modelCmd);
        }

        public static int ExecuteTextureConvert(string input, string output, string txi, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                logger.Error("Input file path is required");
                return 1;
            }

            if (!File.Exists(input))
            {
                logger.Error("Input file does not exist: " + input);
                return 1;
            }

            try
            {
                string extension = Path.GetExtension(input).ToLowerInvariant();
                string outputPath = output;

                if (string.IsNullOrEmpty(outputPath))
                {
                    string inputDir = Path.GetDirectoryName(input) ?? string.Empty;
                    string inputName = Path.GetFileNameWithoutExtension(input);
                    if (extension == ".tpc")
                    {
                        outputPath = Path.Combine(inputDir, inputName + ".tga");
                    }
                    else if (extension == ".tga")
                    {
                        outputPath = Path.Combine(inputDir, inputName + ".tpc");
                    }
                    else
                    {
                        logger.Error("Unsupported texture input extension: " + extension);
                        return 1;
                    }
                }

                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                logger.Info("Converting texture: " + input + " -> " + outputPath);

                if (extension == ".tpc" || ResourceIdentifier.FromPath(input).ResType == ResourceType.TPC)
                {
                    string txiOutput = txi;
                    if (string.IsNullOrEmpty(txiOutput) && string.Equals(Path.GetExtension(outputPath), ".tga", StringComparison.OrdinalIgnoreCase))
                    {
                        txiOutput = Path.ChangeExtension(outputPath, ".txi");
                    }

                    ResourceConversions.ConvertTpcToTga(input, outputPath, txiOutput);
                }
                else if (extension == ".tga" || ResourceIdentifier.FromPath(input).ResType == ResourceType.TGA)
                {
                    ResourceConversions.ConvertTgaToTpc(input, outputPath, txi);
                }
                else
                {
                    logger.Error("Unsupported texture input extension: " + extension);
                    return 1;
                }

                logger.Info("Texture conversion completed successfully");
                return 0;
            }
            catch (Exception ex)
            {
                logger.Error("Texture conversion failed: " + ex.Message);
                return 1;
            }
        }

        public static int ExecuteModelConvert(
            string input,
            string output,
            bool toAscii,
            string mdxPath,
            ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                logger.Error("Input file path is required");
                return 1;
            }

            if (!File.Exists(input))
            {
                logger.Error("Input file does not exist: " + input);
                return 1;
            }

            try
            {
                string outputPath = output;

                if (string.IsNullOrEmpty(outputPath))
                {
                    string inputDir = Path.GetDirectoryName(input) ?? string.Empty;
                    string inputName = Path.GetFileNameWithoutExtension(input);
                    if (toAscii)
                    {
                        outputPath = Path.Combine(inputDir, inputName + ".mdl.ascii");
                    }
                    else
                    {
                        outputPath = Path.Combine(inputDir, inputName + ".mdl");
                    }
                }

                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                logger.Info("Converting model: " + input + " -> " + outputPath);

                if (toAscii)
                {
                    ResourceConversions.ConvertMdlToAscii(input, outputPath, mdxPath);
                }
                else
                {
                    string mdxOutput = mdxPath;
                    if (string.IsNullOrEmpty(mdxOutput))
                    {
                        mdxOutput = Path.ChangeExtension(outputPath, ".mdx");
                    }

                    ResourceConversions.ConvertAsciiToMdl(input, outputPath, mdxOutput);
                }

                logger.Info("Model conversion completed successfully");
                return 0;
            }
            catch (Exception ex)
            {
                logger.Error("Model conversion failed: " + ex.Message);
                return 1;
            }
        }
    }
}
