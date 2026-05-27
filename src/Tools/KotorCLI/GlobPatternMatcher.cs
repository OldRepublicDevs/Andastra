using System;
using System.Collections.Generic;
using System.IO;

namespace KotorCLI
{
    /// <summary>
    /// Glob-style file discovery for kotorcli source include patterns.
    /// Matching PyKotor: glob.glob() behavior
    /// </summary>
    internal static class GlobPatternMatcher
    {
        internal static List<string> FindFilesMatchingPattern(string rootDir, string pattern)
        {
            var results = new List<string>();

            try
            {
                if (pattern.Contains("**/"))
                {
                    int splitIndex = pattern.IndexOf("**/", StringComparison.Ordinal);
                    string prefix = pattern.Substring(0, splitIndex);
                    string remainder = pattern.Substring(splitIndex + 3);
                    string searchRoot = string.IsNullOrEmpty(prefix)
                        ? rootDir
                        : Path.Combine(rootDir, prefix);

                    if (Directory.Exists(searchRoot))
                    {
                        results.AddRange(Directory.GetFiles(searchRoot, remainder, SearchOption.AllDirectories));
                    }

                    return results;
                }

                if (pattern.Contains("**"))
                {
                    var basePattern = pattern;
                    if (basePattern.StartsWith("**/"))
                    {
                        basePattern = basePattern.Substring(3);
                    }

                    var filePattern = Path.GetFileName(basePattern);
                    var dirPattern = Path.GetDirectoryName(basePattern);

                    if (string.IsNullOrEmpty(dirPattern) || dirPattern == ".")
                    {
                        results.AddRange(Directory.GetFiles(rootDir, filePattern, SearchOption.AllDirectories));
                    }
                    else if (Directory.Exists(rootDir))
                    {
                        var searchDirs = Directory.GetDirectories(rootDir, dirPattern, SearchOption.AllDirectories);
                        foreach (var dir in searchDirs)
                        {
                            results.AddRange(Directory.GetFiles(dir, filePattern, SearchOption.TopDirectoryOnly));
                        }
                    }
                }
                else if (pattern.Contains("*") || pattern.Contains("?"))
                {
                    var directory = Path.GetDirectoryName(Path.Combine(rootDir, pattern));
                    var filePattern = Path.GetFileName(pattern);

                    if (string.IsNullOrEmpty(directory) || directory == ".")
                    {
                        directory = rootDir;
                    }

                    if (Directory.Exists(directory))
                    {
                        results.AddRange(Directory.GetFiles(directory, filePattern, SearchOption.TopDirectoryOnly));
                    }
                }
                else
                {
                    var fullPath = Path.Combine(rootDir, pattern);
                    if (File.Exists(fullPath))
                    {
                        results.Add(fullPath);
                    }
                }
            }
            catch (Exception)
            {
                // Ignore errors and continue.
            }

            return results;
        }
    }
}
