using System;
using System.IO;

namespace OdyTools.Shell
{
    internal sealed class ShellCommandResult
    {
        public bool Handled { get; set; }
        public bool ShouldLaunchUi { get; set; }
        public string OpenTslPatchDataPath { get; set; }
    }

    internal static class ShellCommandRouter
    {
        public static ShellCommandResult Handle(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                return new ShellCommandResult { Handled = false };
            }

            string command = (args[0] ?? string.Empty).Trim().ToLowerInvariant();

            if (command == "--open-tslpatchdata")
            {
                return new ShellCommandResult
                {
                    Handled = true,
                    ShouldLaunchUi = true,
                    OpenTslPatchDataPath = args.Length > 1 ? args[1] : null
                };
            }

            if (command == "--shell-open")
            {
                string path = args.Length > 1 ? args[1] : null;
                bool opened = ShellFileActions.TryOpenWithSystemDefault(path);
                if (!opened)
                {
                    Console.Error.WriteLine($"Unable to open path '{path}'.");
                }
                return new ShellCommandResult { Handled = true };
            }

            if (command == "--shell-convert")
            {
                if (args.Length < 3)
                {
                    Console.Error.WriteLine("Usage: --shell-convert <targetFormat> <path>");
                    return new ShellCommandResult { Handled = true };
                }

                string outputPath = ShellFileActions.ConvertFile(args[2], args[1]);
                Console.WriteLine($"Converted file generated at: {outputPath}");
                return new ShellCommandResult { Handled = true };
            }

            if (command == "--shell-diff-pick1")
            {
                string path = args.Length > 1 ? args[1] : null;
                if (string.IsNullOrWhiteSpace(path) || (!File.Exists(path) && !Directory.Exists(path)))
                {
                    Console.Error.WriteLine("Please provide an existing file or directory for diff target 1.");
                    return new ShellCommandResult { Handled = true };
                }

                string normalized = Directory.Exists(path) ? path : Path.GetDirectoryName(path) ?? path;
                ShellDiffStateStore.SaveTarget1(normalized);
                Console.WriteLine($"Diff target 1 saved: {normalized}");
                return new ShellCommandResult { Handled = true };
            }

            if (command == "--shell-diff-run")
            {
                string path = args.Length > 1 ? args[1] : null;
                bool guiRequested = HasArg(args, "--gui");
                if (string.IsNullOrWhiteSpace(path) || (!File.Exists(path) && !Directory.Exists(path)))
                {
                    Console.Error.WriteLine("Please provide an existing file or directory for diff target 2.");
                    return new ShellCommandResult { Handled = true };
                }

                string target1 = ShellDiffStateStore.LoadTarget1();
                if (string.IsNullOrWhiteSpace(target1))
                {
                    Console.Error.WriteLine("No diff target 1 is currently selected. Run --shell-diff-pick1 first.");
                    return new ShellCommandResult { Handled = true };
                }

                string target2 = Directory.Exists(path) ? path : Path.GetDirectoryName(path) ?? path;
                string tslPatchDataPath = ShellDiffService.BuildDiffPatch(target1, target2);
                ShellDiffStateStore.Clear();
                Console.WriteLine($"Generated tslpatchdata at: {tslPatchDataPath}");

                return new ShellCommandResult
                {
                    Handled = true,
                    ShouldLaunchUi = guiRequested,
                    OpenTslPatchDataPath = guiRequested ? tslPatchDataPath : null
                };
            }

            return new ShellCommandResult { Handled = false };
        }

        private static bool HasArg(string[] args, string token)
        {
            foreach (string arg in args)
            {
                if (string.Equals(arg, token, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
