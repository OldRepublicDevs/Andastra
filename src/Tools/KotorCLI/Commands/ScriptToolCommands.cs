using System;
using System.CommandLine;
using System.IO;
using KotorCLI.Logging;
using BioWare.Common;
using BioWare.Resource.Formats.NCS;
using BioWare.Tools;

namespace KotorCLI.Commands
{
    /// <summary>
    /// Script tool commands (decompile, disassemble, assemble).
    /// </summary>
    public static class ScriptToolCommands
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            var decompileCmd = new Command("decompile", "Decompile NCS bytecode to NSS source");
            var decompileInput = new Argument<string>("input");
            decompileInput.Description = "Input NCS file";
            decompileCmd.Add(decompileInput);
            var decompileOutput = Cli.Opt<string>("--output", "Output NSS file");
            decompileCmd.Options.Add(decompileOutput);
            var gameOption = Cli.Opt<string>("--game", "Target game (k1 or k2). Defaults to k2.");
            decompileCmd.Options.Add(gameOption);
            decompileCmd.SetAction(parseResult =>
            {
                var input = parseResult.GetValue(decompileInput);
                var output = parseResult.GetValue(decompileOutput);
                var game = parseResult.GetValue(gameOption);
                var logger = new StandardLogger();

                try
                {
                    // Validate input file
                    if (!File.Exists(input))
                    {
                        logger.Error($"Input file does not exist: {input}");
                        Environment.Exit(1);
                    }

                    // Determine game type
                    BioWareGame gameType = BioWareGame.K1; // Default to K1
                    if (string.Equals(game, "k1", StringComparison.OrdinalIgnoreCase))
                    {
                        gameType = BioWareGame.K1;
                    }
                    else if (string.Equals(game, "k2", StringComparison.OrdinalIgnoreCase) ||
                             string.Equals(game, "tsl", StringComparison.OrdinalIgnoreCase))
                    {
                        gameType = BioWareGame.TSL;
                    }
                    else
                    {
                        logger.Error($"Invalid game type: {game}. Must be 'k1' or 'k2'.");
                        Environment.Exit(1);
                        return; // Unreachable but helps compiler
                    }

                    // Determine output file path
                    string outputFile = output;
                    if (string.IsNullOrEmpty(outputFile))
                    {
                        // Default: same directory as input, change extension to .nss
                        string inputDir = Path.GetDirectoryName(input);
                        string inputName = Path.GetFileNameWithoutExtension(input);
                        outputFile = Path.Combine(inputDir, inputName + ".nss");
                    }

                    // Ensure output directory exists
                    string outputDir = Path.GetDirectoryName(outputFile);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    logger.Info($"Decompiling: {input}");
                    logger.Info($"Game: {gameType}");
                    logger.Info($"Output: {outputFile}");

                    // Read NCS file
                    NCS ncs = NCSAuto.ReadNcs(input);

                    // Decompile to NSS source
                    string nssCode = NCSAuto.DecompileNcs(ncs, gameType);

                    // Write output
                    File.WriteAllText(outputFile, nssCode, System.Text.Encoding.UTF8);

                    logger.Info($"Successfully decompiled {nssCode.Length} characters");
                }
                catch (Exception ex)
                {
                    logger.Error($"Decompilation failed: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        logger.Error($"Inner exception: {ex.InnerException.Message}");
                    }
                    Environment.Exit(1);
                }
            });
            rootCommand.Add(decompileCmd);

            var disassembleCmd = new Command("disassemble", "Disassemble NCS bytecode to text");
            var disassembleInput = new Argument<string>("input");
            disassembleInput.Description = "Input NCS file";
            disassembleCmd.Add(disassembleInput);
            var disassembleOutput = Cli.Opt<string>("--output", "Output text file");
            disassembleCmd.Options.Add(disassembleOutput);
            disassembleCmd.SetAction(parseResult =>
            {
                var input = parseResult.GetValue(disassembleInput);
                var output = parseResult.GetValue(disassembleOutput);
                var logger = new StandardLogger();
                int exitCode = ExecuteDisassemble(input, output, logger);
                Environment.Exit(exitCode);
            });
            rootCommand.Add(disassembleCmd);

            var assembleCmd = new Command("assemble", "Assemble/compile NSS source to NCS bytecode");
            var assembleInput = new Argument<string>("input");
            assembleInput.Description = "Input NSS file";
            assembleCmd.Add(assembleInput);
            var assembleOutput = Cli.Opt<string>("--output", "Output NCS file");
            assembleCmd.Options.Add(assembleOutput);
            var includeOption = Cli.Opt<string[]>("--include", "Include directory for #include files");
            assembleCmd.Options.Add(includeOption);
            var debugOption = Cli.Opt<bool>("--debug", "Enable debug output");
            assembleCmd.Options.Add(debugOption);
            var assembleGameOption = Cli.Opt<string>("--game", "Target game (k1 or k2). Defaults to k2.");
            assembleCmd.Options.Add(assembleGameOption);
            assembleCmd.SetAction(parseResult =>
            {
                var input = parseResult.GetValue(assembleInput);
                var output = parseResult.GetValue(assembleOutput);
                var includes = parseResult.GetValue(includeOption);
                var debug = parseResult.GetValue(debugOption);
                var game = parseResult.GetValue(assembleGameOption);
                var logger = new StandardLogger();
                int exitCode = ExecuteAssemble(input, output, includes, debug, game, logger);
                Environment.Exit(exitCode);
            });
            rootCommand.Add(assembleCmd);
        }

        public static int ExecuteDisassemble(string input, string output, ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                logger.Error("Error: No input file specified");
                return 1;
            }

            if (!File.Exists(input))
            {
                logger.Error("Input file does not exist: " + input);
                return 1;
            }

            try
            {
                string outputFile = output;
                if (string.IsNullOrEmpty(outputFile))
                {
                    string inputDir = Path.GetDirectoryName(input);
                    string inputName = Path.GetFileNameWithoutExtension(input);
                    outputFile = Path.Combine(inputDir ?? string.Empty, inputName + ".ncsdis");
                }

                string outputDir = Path.GetDirectoryName(outputFile);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                logger.Info("Disassembling: " + input);
                logger.Info("Output: " + outputFile);

                string disassembly = Scripts.DisassembleNcs(input, outputFile);
                logger.Info("Successfully disassembled " + disassembly.Length + " characters");
                return 0;
            }
            catch (Exception ex)
            {
                logger.Error("Disassembly failed: " + ex.Message);
                return 1;
            }
        }

        public static int ExecuteAssemble(
            string input,
            string output,
            string[] includes,
            bool debug,
            string game,
            ILogger logger)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                logger.Error("Error: No input file specified");
                return 1;
            }

            if (!File.Exists(input))
            {
                logger.Error("Input file does not exist: " + input);
                return 1;
            }

            BioWareGame gameType = BioWareGame.TSL;
            if (string.Equals(game, "k1", StringComparison.OrdinalIgnoreCase))
            {
                gameType = BioWareGame.K1;
            }
            else if (!string.IsNullOrEmpty(game) &&
                     !string.Equals(game, "k2", StringComparison.OrdinalIgnoreCase) &&
                     !string.Equals(game, "tsl", StringComparison.OrdinalIgnoreCase))
            {
                logger.Error("Invalid game type: " + game + ". Must be 'k1' or 'k2'.");
                return 1;
            }

            try
            {
                string outputFile = output;
                if (string.IsNullOrEmpty(outputFile))
                {
                    string inputDir = Path.GetDirectoryName(input);
                    string inputName = Path.GetFileNameWithoutExtension(input);
                    outputFile = Path.Combine(inputDir ?? string.Empty, inputName + ".ncs");
                }

                string outputDir = Path.GetDirectoryName(outputFile);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                logger.Info("Assembling: " + input);
                logger.Info("Game: " + gameType);
                logger.Info("Output: " + outputFile);

                string nssSource = File.ReadAllText(input);
                object libraryLookup = includes != null && includes.Length > 0 ? includes : null;
                NCS ncs = NCSAuto.CompileNss(nssSource, gameType, null, libraryLookup, debug);
                if (ncs == null)
                {
                    logger.Error("Assembly failed: compiler returned null");
                    return 1;
                }

                NCSAuto.WriteNcs(ncs, outputFile);
                logger.Info("Successfully assembled NCS to " + outputFile);
                return 0;
            }
            catch (Exception ex)
            {
                logger.Error("Assembly failed: " + ex.Message);
                if (ex.InnerException != null)
                {
                    logger.Error("Inner exception: " + ex.InnerException.Message);
                }
                return 1;
            }
        }
    }
}
