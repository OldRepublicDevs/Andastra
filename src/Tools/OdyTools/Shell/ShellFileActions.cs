using System;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace OdyTools.Shell
{
    internal static class ShellFileActions
    {
        public static bool TryOpenWithSystemDefault(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            if (!File.Exists(path) && !Directory.Exists(path))
            {
                return false;
            }

            Process.Start(new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            });
            return true;
        }

        public static string ConvertFile(string path, string targetFormat)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                throw new FileNotFoundException("Input file was not found.", path);
            }

            if (string.IsNullOrWhiteSpace(targetFormat))
            {
                throw new ArgumentException("Target format must be provided.", nameof(targetFormat));
            }

            string normalized = targetFormat.Trim().TrimStart('.').ToLowerInvariant();
            string outputPath;

            if (normalized == "json")
            {
                outputPath = Path.ChangeExtension(path, ".json");
                byte[] data = File.ReadAllBytes(path);
                var jsonObject = new
                {
                    sourcePath = path,
                    sourceExtension = Path.GetExtension(path),
                    generatedUtc = DateTimeOffset.UtcNow,
                    payloadBase64 = Convert.ToBase64String(data)
                };
                File.WriteAllText(outputPath, JsonConvert.SerializeObject(jsonObject, Formatting.Indented));
                return outputPath;
            }

            outputPath = Path.ChangeExtension(path, "." + normalized);
            File.Copy(path, outputPath, overwrite: true);
            return outputPath;
        }
    }
}
