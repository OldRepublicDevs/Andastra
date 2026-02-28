using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Tools;
using ConvertKotorGame.Models;

namespace ConvertKotorGame.Services
{
    public sealed class InstallationDetectionService
    {
        public const string AutoDetectOption = "auto-detect";

        public List<InstallationDetectionInfo> DetectInstallations()
        {
            var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string path in GetDefaultCandidates())
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    continue;
                }

                string fullPath;
                try
                {
                    fullPath = System.IO.Path.GetFullPath(path);
                }
                catch
                {
                    continue;
                }

                if (!Directory.Exists(fullPath))
                {
                    continue;
                }

                BioWareGame? game = DetermineGame(fullPath);
                if (!game.HasValue)
                {
                    continue;
                }

                paths.Add(fullPath);
            }

            var detected = new List<InstallationDetectionInfo>();
            foreach (string path in paths.OrderBy(p => p, StringComparer.OrdinalIgnoreCase))
            {
                detected.Add(DetectSingle(path));
            }

            return detected;
        }

        public InstallationDetectionInfo DetectSingle(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return new InstallationDetectionInfo
                {
                    Path = string.Empty,
                    Game = null,
                    Distribution = "Unknown",
                    PlatformSummary = GetPlatformSummary(),
                };
            }

            string fullPath;
            try
            {
                fullPath = System.IO.Path.GetFullPath(path);
            }
            catch
            {
                fullPath = path;
            }

            return new InstallationDetectionInfo
            {
                Path = fullPath,
                Game = DetermineGame(fullPath),
                Distribution = DetectDistribution(fullPath),
                PlatformSummary = GetPlatformSummary(),
            };
        }

        private static BioWareGame? DetermineGame(string installPath)
        {
            BioWareGame? game = Installation.DetermineGame(installPath);
            if (game.HasValue)
            {
                return NormalizeKotorGame(game.Value);
            }

            BioWareGame? heuristicsGame = Heuristics.DetermineGame(installPath);
            return heuristicsGame.HasValue ? NormalizeKotorGame(heuristicsGame.Value) : (BioWareGame?)null;
        }

        private static BioWareGame? NormalizeKotorGame(BioWareGame game)
        {
            if (game.IsK1())
            {
                return BioWareGame.K1;
            }

            if (game.IsK2() || game.IsTSL())
            {
                return BioWareGame.TSL;
            }

            return null;
        }

        private static string DetectDistribution(string path)
        {
            string lower = path.ToLowerInvariant();
            if (lower.Contains("steamapps"))
            {
                return "Steam";
            }

            if (lower.Contains("gog") || File.Exists(System.IO.Path.Combine(path, "goggame.dll")))
            {
                return "GOG";
            }

            if (lower.Contains("amazon") || File.Exists(System.IO.Path.Combine(path, "AmazonGames.ini")))
            {
                return "Amazon";
            }

            if (File.Exists(System.IO.Path.Combine(path, "swkotor.exe")) ||
                File.Exists(System.IO.Path.Combine(path, "swkotor2.exe")))
            {
                return "Retail/Legacy";
            }

            return "Unknown";
        }

        private static string GetPlatformSummary()
        {
            string os;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                os = "Windows";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                os = "macOS";
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                os = "Linux";
            }
            else
            {
                os = "Unknown OS";
            }

            string arch = RuntimeInformation.ProcessArchitecture.ToString();
            return os + " / " + arch;
        }

        private static IEnumerable<string> GetDefaultCandidates()
        {
            var candidates = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var discovered = PathTools.FindKotorPathsFromDefault();
                foreach (var gamePaths in discovered.Values)
                {
                    foreach (CaseAwarePath path in gamePaths)
                    {
                        string resolved = path.GetResolvedPath();
                        if (!string.IsNullOrWhiteSpace(resolved))
                        {
                            candidates.Add(resolved);
                        }
                    }
                }
            }
            catch
            {
                // Fall through to static defaults below.
            }

            // Fallback defaults for portability when discovery fails.
            candidates.Add(@"C:\Program Files\Steam\steamapps\common\swkotor");
            candidates.Add(@"C:\Program Files (x86)\Steam\steamapps\common\swkotor");
            candidates.Add(@"C:\Program Files\Steam\steamapps\common\Knights of the Old Republic II");
            candidates.Add(@"C:\Program Files (x86)\Steam\steamapps\common\Knights of the Old Republic II");
            candidates.Add(@"C:\GOG Games\Star Wars - KotOR");
            candidates.Add(@"C:\GOG Games\Star Wars - KotOR2");
            candidates.Add(@"C:\Amazon Games\Library\Star Wars - Knights of the Old");
            candidates.Add(@"C:\Program Files\LucasArts\SWKotOR");
            candidates.Add(@"C:\Program Files (x86)\LucasArts\SWKotOR");
            candidates.Add(@"C:\Program Files\LucasArts\SWKotOR2");
            candidates.Add(@"C:\Program Files (x86)\LucasArts\SWKotOR2");

            string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (!string.IsNullOrWhiteSpace(home))
            {
                candidates.Add(System.IO.Path.Combine(home, ".local/share/Steam/steamapps/common/swkotor"));
                candidates.Add(System.IO.Path.Combine(home, ".steam/debian-installation/steamapps/common/swkotor"));
                candidates.Add(System.IO.Path.Combine(home, ".local/share/Steam/steamapps/common/Knights of the Old Republic II"));
                candidates.Add(System.IO.Path.Combine(home, ".steam/debian-installation/steamapps/common/Knights of the Old Republic II"));
                candidates.Add(System.IO.Path.Combine(home, ".local/share/aspyr-media/kotor2"));
            }

            return candidates;
        }
    }
}
