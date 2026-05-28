using System;
using System.CommandLine;
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
                var launchCommand = new Command(alias, "Convert, compile, pack, install, and launch target in-game (--install-only for install; --dry-run for path check; spawn stub otherwise)");
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

                launchCommand.SetAction(parseResult =>
                {
                    var targets = parseResult.GetValue(targetsArgument) ?? Array.Empty<string>();
                    var gameBin = parseResult.GetValue(gameBinOption);
                    var installDir = parseResult.GetValue(installDirOption);
                    var dryRun = parseResult.GetValue(dryRunOption);
                    var installOnly = parseResult.GetValue(installOnlyOption);

                    var logger = new StandardLogger();
                    var exitCode = Execute(targets, gameBin, installDir, dryRun, installOnly, logger);
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

                logger.Info("Launch is not yet implemented; dry-run only reports resolved paths.");
                return 0;
            }

            logger.Error("Launch command is not yet implemented. Use install + run the game executable manually.");
            logger.Info("Planned workflow:");
            logger.Info("  1. Call install command");
            logger.Info("  2. Launch KOTOR game executable");
            logger.Info("  3. Pass module load arguments");
            logger.Info($"Resolved game executable (for manual launch): {resolvedBinary}");
            logger.Info("Tip: pass --dry-run to verify path resolution without this error.");
            return 1;
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
