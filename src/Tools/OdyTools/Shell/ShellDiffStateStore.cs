using System;
using System.IO;
using Newtonsoft.Json;

namespace OdyTools.Shell
{
    internal sealed class ShellDiffState
    {
        public string Target1Path { get; set; }
        public DateTimeOffset UpdatedUtc { get; set; }
    }

    internal static class ShellDiffStateStore
    {
        private static string StateDirectory =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Andastra", "OdyTools");

        private static string StatePath => Path.Combine(StateDirectory, "shell_diff_state.json");

        public static void SaveTarget1(string target1Path)
        {
            Directory.CreateDirectory(StateDirectory);
            var state = new ShellDiffState
            {
                Target1Path = target1Path,
                UpdatedUtc = DateTimeOffset.UtcNow
            };
            File.WriteAllText(StatePath, JsonConvert.SerializeObject(state, Formatting.Indented));
        }

        public static string LoadTarget1()
        {
            if (!File.Exists(StatePath))
            {
                return null;
            }

            try
            {
                var state = JsonConvert.DeserializeObject<ShellDiffState>(File.ReadAllText(StatePath));
                return state?.Target1Path;
            }
            catch
            {
                return null;
            }
        }

        public static void Clear()
        {
            if (File.Exists(StatePath))
            {
                File.Delete(StatePath);
            }
        }
    }
}
