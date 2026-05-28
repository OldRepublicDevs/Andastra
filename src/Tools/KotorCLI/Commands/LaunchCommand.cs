using System;
using System.CommandLine;
using System.Diagnostics;
using System.IO;
using KotorCLI.Logging;

namespace KotorCLI.Commands
{
    /// <summary>
    /// Launch command - Convert, compile, pack, install, and launch target in-game.
    /// Aliases: serve, play, test
    /// </summary>
    public static class LaunchCommand
    {
        public static void AddToRootCommand(RootCommand rootCommand)
        {
            // launch, serve, play, test are all aliases for the same command
            foreach (string alias in new[] { "launch", "serve", "play", "test" })
            {
                var launchCommand = new Command(alias, "Convert, compile, pack, install, and launch target in-game (--install-only, --dry-run, --wait)");
                var targetsArgument = new Argument<string[]>("targets");
                targetsArgument.Description = "Target to launch";
                launchCommand.Add(targetsArgument);
                var gameBinOption = Cli.Opt<string>("--gameBin", "Path to the swkotor binary file");
                launchCommand.Options.Add(gameBinOption);
                var installDirOption = Cli.Opt<string>("--installDir", "The location of the KOTOR installation directory");
                launchCommand.Options.Add(installDirOption);
                var dryRunOption = Cli.Opt<bool>("--dry-run", "Resolve and print the game executable path without launching");
                launchCommand.Options.Add(dryRunOption);
                var installOnlyOption = Cli.Opt<bool>("--install-only", "Run install (convert, compile, pack, copy to game) without launching the game");
                launchCommand.Options.Add(installOnlyOption);
                var waitOption = Cli.Opt<bool>("--wait", "Wait for the game process to exit and return its exit code");
                launchCommand.Options.Add(waitOption);

                launchCommand.SetAction(parseResult =>
                {
                    var targets = parseResult.GetValue(targetsArgument) ?? Array.Empty<string>();
                    var gameBin = parseResult.GetValue(gameBinOption);
                    var installDir = parseResult.GetValue(installDirOption);
                    var dryRun = parseResult.GetValue(dryRunOption);
                    var installOnly = parseResult.GetValue(installOnlyOption);
                    var waitForExit = parseResult.GetValue(waitOption);

                    var logger = new StandardLogger();
                    var exitCode = Execute(targets, gameBin, installDir, dryRun, installOnly, waitForExit, logger);
                    Environment.Exit(exitCode);
                });

                rootCommand.Add(launchCommand);
            }
        }

        public static int Execute(string[] targetNames, string gameBin, string installDir, ILogger logger)
        {
            return Execute(targetNames, gameBin, installDir, false, logger);
        }

        public static int Execute(string[] targetNames, string gameBin, string installDir, bool dryRun, ILogger logger)
        {
            return Execute(targetNames, gameBin, installDir, dryRun, false, logger);
        }

        public static int Execute(string[] targetNames, string gameBin, string installDir, bool dryRun, bool installOnly, ILogger logger)
        {
            return Execute(targetNames, gameBin, installDir, dryRun, installOnly, false, logger);
        }

        public static int Execute(string[] targetNames, string gameBin, string installDir, bool dryRun, bool installOnly, bool waitForExit, ILogger logger)
        {
            if (installOnly)
            {
                if (dryRun)
                {
                    logger.Info("--install-only takes precedence over --dry-run; running install.");
                }

                logger.Info("Install-only mode: running install without launching the game.");
                return InstallCommand.Execute(targetNames ?? Array.Empty<string>(), installDir, false, false, logger);
            }

            string resolvedBinary = ResolveGameBinary(gameBin, installDir, logger);
            if (string.IsNullOrEmpty(resolvedBinary))
            {
                logger.Error("Could not resolve KOTOR game executable. Specify --gameBin or --installDir (with chitin.key).");
                return 1;
            }

            if (dryRun)
            {
                logger.Info($"Resolved game executable: {resolvedBinary}");
                if (targetNames != null && targetNames.Length > 0)
                {
                    logger.Info("Targets: " + string.Join(", ", targetNames));
                }

                logger.Info("Dry-run only; skipping install and game launch.");
                return 0;
            }

            logger.Info("Running install before launch.");
            int installExitCode = InstallCommand.Execute(targetNames ?? Array.Empty<string>(), installDir, false, false, logger);
            if (installExitCode != 0)
            {
                logger.Error("Install failed; game will not be launched.");
                return installExitCode;
            }

            string workingDirectory = DetermineInstallationDirectory(installDir, logger);
            if (string.IsNullOrEmpty(workingDirectory))
            {
                workingDirectory = Path.GetDirectoryName(resolvedBinary);
            }

            logger.Info("Launching game: " + resolvedBinary);
            int processExitCode;
            if (!TryStartGameProcess(resolvedBinary, workingDirectory, waitForExit, logger, out processExitCode))
            {
                return 1;
            }

            if (waitForExit)
            {
                logger.Info("Game process exited with code: " + processExitCode);
                return processExitCode;
            }

            logger.Info("Game process started.");
            return 0;
        }

        internal static bool TryStartGameProcess(string gameBinaryPath, string workingDirectory, bool waitForExit, ILogger logger, out int processExitCode)
        {
            processExitCode = 0;
            if (string.IsNullOrEmpty(gameBinaryPath) || !File.Exists(gameBinaryPath))
            {
                logger.Error("Game executable does not exist: " + gameBinaryPath);
                return false;
            }

            string workDir = workingDirectory;
            if (string.IsNullOrEmpty(workDir))
            {
                workDir = Path.GetDirectoryName(gameBinaryPath);
            }

            if (!string.IsNullOrEmpty(workDir) && !Directory.Exists(workDir))
            {
                logger.Error("Working directory does not exist: " + workDir);
                return false;
            }

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = gameBinaryPath,
                    WorkingDirectory = workDir,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(startInfo))
                {
                    if (process == null)
                    {
                        logger.Error("Process.Start returned null.");
                        return false;
                    }

                    if (waitForExit)
                    {
                        process.WaitForExit();
                        processExitCode = process.ExitCode;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Failed to start game process: " + ex.Message);
                return false;
            }
        }

        internal static string ResolveGameBinary(string gameBin, string installDir, ILogger logger)
        {
            if (!string.IsNullOrEmpty(gameBin))
            {
                string fullPath = Path.GetFullPath(gameBin);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }

                logger.Warning($"Specified game binary does not exist: {fullPath}");
            }

            string installRoot = DetermineInstallationDirectory(installDir, logger);
            if (string.IsNullOrEmpty(installRoot))
            {
                return null;
            }

            string k1Exe = Path.Combine(installRoot, "swkotor.exe");
            if (File.Exists(k1Exe))
            {
                return k1Exe;
            }

            string tslExe = Path.Combine(installRoot, "swkotor2.exe");
            if (File.Exists(tslExe))
            {
                return tslExe;
            }

            logger.Warning($"No swkotor.exe or swkotor2.exe found under installation directory: {installRoot}");
            return null;
        }

        private static string DetermineInstallationDirectory(string userSpecifiedDir, ILogger logger)
        {
            if (!string.IsNullOrEmpty(userSpecifiedDir))
            {
                if (Directory.Exists(userSpecifiedDir))
                {
                    return Path.GetFullPath(userSpecifiedDir);
                }

                logger.Warning($"Specified installation directory does not exist: {userSpecifiedDir}");
            }

            string kotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            if (string.IsNullOrEmpty(kotorPath))
            {
                kotorPath = Environment.GetEnvironmentVariable("K1_PATH");
            }

            if (string.IsNullOrEmpty(kotorPath))
            {
                kotorPath = Environment.GetEnvironmentVariable("K2_PATH");
            }

            if (!string.IsNullOrEmpty(kotorPath) && Directory.Exists(kotorPath))
            {
                string chitinPath = Path.Combine(kotorPath, "chitin.key");
                if (File.Exists(chitinPath))
                {
                    return Path.GetFullPath(kotorPath);
                }
            }

            return null;
        }
    }
}
