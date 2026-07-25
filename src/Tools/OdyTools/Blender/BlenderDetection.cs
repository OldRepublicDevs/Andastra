using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace OdyTools.Blender
{
    /// <summary>
    /// Blender installation detection for Windows, macOS, and Linux.
    /// </summary>
    public static class BlenderDetection
    {
        private const int MinVersionMajor = 3;
        private const int MinVersionMinor = 6;
        private const int MinVersionPatch = 0;

        /// <summary>
        /// Find all valid Blender installations on the system.
        /// Similar to find_kotor_paths_from_default pattern.
        /// </summary>
        public static List<BlenderInfo> FindAllBlenderInstallations()
        {
            var candidates = new List<string>();

            // Check PATH
            string pathBlender = GetBlenderFromPath();
            if (!string.IsNullOrEmpty(pathBlender))
            {
                candidates.Add(pathBlender);
            }

            // Windows registry
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                candidates.AddRange(GetWindowsRegistryBlenderPaths());
            }

            // Common paths
            candidates.AddRange(GetCommonBlenderPaths());

            // Remove duplicates while preserving order
            var uniqueCandidates = new List<string>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var candidate in candidates)
            {
                try
                {
                    string resolved = Path.GetFullPath(candidate);
                    if (!seen.Contains(resolved))
                    {
                        seen.Add(resolved);
                        uniqueCandidates.Add(candidate);
                    }
                }
                catch
                {
                    continue;
                }
            }

            // Build info for each valid installation
            var installations = new List<BlenderInfo>();

            foreach (var candidate in uniqueCandidates)
            {
                var version = GetBlenderVersion(candidate);
                if (version == null)
                {
                    continue;
                }

                if (!IsSupportedVersion(version.Value))
                {
                    continue;
                }

                var (addonsPath, extensionsPath) = GetBlenderConfigPaths(version.Value);

                var info = new BlenderInfo
                {
                    Executable = candidate,
                    Version = version,
                    AddonsPath = addonsPath,
                    ExtensionsPath = extensionsPath,
                    IsValid = true
                };
                info.UpdateVersionString();

                info.HasKotorblender = CheckKotorblenderInstalled(info);
                installations.Add(info);
            }

            // Sort by version (newest first), then by kotorblender status
            installations.Sort((a, b) =>
            {
                int kotorCompare = b.HasKotorblender.CompareTo(a.HasKotorblender);
                if (kotorCompare != 0) return kotorCompare;

                if (!a.Version.HasValue && !b.Version.HasValue) return 0;
                if (!a.Version.HasValue) return 1;
                if (!b.Version.HasValue) return -1;

                var aV = a.Version.Value;
                var bV = b.Version.Value;

                int majorCompare = bV.Major.CompareTo(aV.Major);
                if (majorCompare != 0) return majorCompare;

                int minorCompare = bV.Minor.CompareTo(aV.Minor);
                if (minorCompare != 0) return minorCompare;

                return bV.Patch.CompareTo(aV.Patch);
            });

            return installations;
        }

        /// <summary>
        /// Find a valid Blender installation.
        /// </summary>
        public static BlenderInfo FindBlenderExecutable(string customPath = null)
        {
            // Custom path takes priority
            if (!string.IsNullOrEmpty(customPath))
            {
                string executable = null;
                var custom = new DirectoryInfo(customPath);
                if (custom.Exists)
                {
                    // Check for blender executable in directory
                    foreach (var name in new[] { "blender", "blender.exe", "Blender" })
                    {
                        var exe = new FileInfo(Path.Combine(custom.FullName, name));
                        if (exe.Exists)
                        {
                            executable = exe.FullName;
                            break;
                        }
                    }

                    // macOS .app bundle
                    if (executable == null && customPath.EndsWith(".app", StringComparison.OrdinalIgnoreCase))
                    {
                        var exe = new FileInfo(Path.Combine(custom.FullName, "Contents", "MacOS", "Blender"));
                        if (exe.Exists)
                        {
                            executable = exe.FullName;
                        }
                    }
                }
                else
                {
                    var customFile = new FileInfo(customPath);
                    if (customFile.Exists)
                    {
                        executable = customFile.FullName;
                    }
                }

                if (!string.IsNullOrEmpty(executable))
                {
                    var version = GetBlenderVersion(executable);
                    if (version.HasValue)
                    {
                        var v = version.Value;
                        if (IsSupportedVersion(v))
                        {
                            var (addonsPath, extensionsPath) = GetBlenderConfigPaths(v);
                            var info = new BlenderInfo
                            {
                                Executable = executable,
                                Version = version,
                                AddonsPath = addonsPath,
                                ExtensionsPath = extensionsPath,
                                IsValid = true
                            };
                            info.UpdateVersionString();
                            info.HasKotorblender = CheckKotorblenderInstalled(info);
                            return info;
                        }
                    }
                }
            }

            // Find all installations and return the best one
            var installations = FindAllBlenderInstallations();
            return installations.FirstOrDefault();
        }

        /// <summary>
        /// Detect Blender installation with full status information.
        /// </summary>
        public static BlenderInfo DetectBlender(string customPath = null)
        {
            return DetectBlender(FindBlenderExecutable, customPath);
        }

        public static bool IsBlenderAvailable(string customPath = null)
        {
            return IsBlenderAvailable(DetectBlender, customPath);
        }

        internal static bool IsBlenderAvailable(Func<string, BlenderInfo> detectBlender, string customPath = null)
        {
            var info = detectBlender(customPath);
            return info != null && info.IsValid && info.HasKotorblender;
        }

        public static Process LaunchBlenderWithIpc(
            BlenderInfo blenderInfo,
            int ipcPort = 7531,
            string installationPath = null,
            string modulePath = null,
            string blendFile = null,
            bool background = false)
        {
            if (blenderInfo == null || !blenderInfo.IsValid || string.IsNullOrEmpty(blenderInfo.Executable))
            {
                return null;
            }

            try
            {
                return Process.Start(CreateBlenderIpcStartInfo(blenderInfo, ipcPort, installationPath, modulePath, blendFile, background));
            }
            catch
            {
                return null;
            }
        }

        internal static BlenderInfo DetectBlender(Func<string, BlenderInfo> findBlenderExecutable, string customPath = null)
        {
            var info = findBlenderExecutable(customPath);

            if (info == null)
            {
                return new BlenderInfo
                {
                    Executable = "",
                    IsValid = false,
                    Error = "No valid Blender installation found. Please install Blender 3.6 or later."
                };
            }

            if (!info.HasKotorblender)
            {
                info.Error = $"Blender {info.VersionString} found but kotorblender add-on is not installed. " +
                            "Click 'Install kotorblender' to install it automatically.";
            }

            return info;
        }

        internal static ProcessStartInfo CreateBlenderIpcStartInfo(
            BlenderInfo blenderInfo,
            int ipcPort,
            string installationPath = null,
            string modulePath = null,
            string blendFile = null,
            bool background = false)
        {
            if (blenderInfo == null)
            {
                throw new ArgumentNullException(nameof(blenderInfo));
            }

            var script = GenerateIpcStartupScript(ipcPort, installationPath, modulePath, background);
            var args = new List<string>();

            if (background)
            {
                args.Add("--background");
            }

            if (!string.IsNullOrEmpty(blendFile))
            {
                args.Add(blendFile);
            }

            args.Add("--python-expr");
            args.Add(script);

            return new ProcessStartInfo
            {
                FileName = blenderInfo.Executable,
                Arguments = string.Join(" ", args.Select(QuoteCommandLineArgument)),
                UseShellExecute = false,
                CreateNoWindow = false
            };
        }

        internal static string GenerateIpcStartupScript(
            int ipcPort,
            string installationPath = null,
            string modulePath = null,
            bool background = false)
        {
            return $@"
import sys
import traceback

def _odytools_enable_kotor_addon():
    module_names = ['bl_ext.user_default.io_scene_kotor', 'io_scene_kotor']
    try:
        import bpy
        import addon_utils
        for module_name in module_names:
            try:
                bpy.ops.preferences.addon_enable(module=module_name)
                print('[OdyTools.NET] Enabled Blender add-on ' + module_name + ' via bpy.ops.preferences.addon_enable')
                return module_name
            except Exception:
                pass
            try:
                addon_utils.enable(module_name, default_set=True, persistent=True)
                print('[OdyTools.NET] Enabled Blender add-on ' + module_name + ' via addon_utils.enable')
                return module_name
            except Exception:
                pass
    except Exception as exc:
        print('[OdyTools.NET] Failed while enabling kotorblender add-on: ' + str(exc))
        traceback.print_exc()
    print('[OdyTools.NET] Failed to enable io_scene_kotor add-on using known module names')
    return None

try:
    module_path = {PythonStringOrNone(modulePath)}
    enabled_module = _odytools_enable_kotor_addon()
    if enabled_module:
        import importlib
        bridge_module = importlib.import_module(enabled_module + '.ipc')
        server = bridge_module.start_ipc_server(port={ipcPort}, installation_path={PythonStringOrNone(installationPath)})
        try:
            sync_module = importlib.import_module(enabled_module + '.ipc.sync')
            sync_module.start_scene_monitor(server)
        except Exception as monitor_exc:
            print('[OdyTools.NET] Failed to start Blender scene monitor: ' + str(monitor_exc))
            traceback.print_exc()
        print('[OdyTools.NET] IPC server started on port {ipcPort}')
        if {PythonBool(background)}:
            import time
            while getattr(server, '_running', False):
                try:
                    server._process_requests()
                    if getattr(server, '_monitor_running', False):
                        server._monitor_scene()
                except Exception as loop_exc:
                    print('[OdyTools.NET] Background IPC loop error: ' + str(loop_exc))
                    traceback.print_exc()
                    break
                time.sleep(0.05)
    else:
        print('[OdyTools.NET] IPC server was not started because the add-on could not be enabled.')
except ImportError as exc:
    print('[OdyTools.NET] Warning: Could not start IPC server: ' + str(exc))
    print('[OdyTools.NET] kotorblender IPC module not found. Make sure kotorblender is properly installed.')
except Exception as exc:
    print('[OdyTools.NET] Error starting IPC server: ' + str(exc))
    traceback.print_exc()
";
        }

        private static string PythonBool(bool value)
        {
            return value ? "True" : "False";
        }

        private static string PythonStringOrNone(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "None";
            }

            return "'" + value.Replace("\\", "\\\\").Replace("'", "\\'") + "'";
        }

        private static string QuoteCommandLineArgument(string argument)
        {
            if (string.IsNullOrEmpty(argument))
            {
                return "\"\"";
            }

            return "\"" + argument.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
        }

        /// <summary>
        /// Get Blender paths from Windows registry.
        /// </summary>
        private static List<string> GetWindowsRegistryBlenderPaths()
        {
            var paths = new List<string>();

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return paths;
            }

            try
            {
                var registryKeys = new[]
                {
                    (RegistryHive.LocalMachine, @"SOFTWARE\BlenderFoundation\Blender"),
                    (RegistryHive.CurrentUser, @"SOFTWARE\BlenderFoundation\Blender"),
                    (RegistryHive.LocalMachine, @"SOFTWARE\WOW6432Node\BlenderFoundation\Blender")
                };

                foreach (var (hive, keyPath) in registryKeys)
                {
                    try
                    {
                        using (var baseKey = RegistryKey.OpenBaseKey(hive, RegistryView.Default))
                        using (var key = baseKey.OpenSubKey(keyPath))
                        {
                            if (key != null)
                            {
                                // Enumerate subkeys (version numbers like "4.2", "3.6")
                                foreach (var versionKeyName in key.GetSubKeyNames())
                                {
                                    try
                                    {
                                        using (var versionKey = key.OpenSubKey(versionKeyName))
                                        {
                                            if (versionKey != null)
                                            {
                                                var installPath = versionKey.GetValue("") as string;
                                                if (!string.IsNullOrEmpty(installPath))
                                                {
                                                    var exePath = Path.Combine(installPath, "blender.exe");
                                                    if (File.Exists(exePath))
                                                    {
                                                        paths.Add(exePath);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            catch
            {
                // Registry access may fail
            }

            return paths;
        }

        /// <summary>
        /// Get common Blender installation paths based on OS.
        /// </summary>
        private static List<string> GetCommonBlenderPaths()
        {
            var paths = new List<string>();
            var system = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" :
                        RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "Darwin" : "Linux";

            if (system == "Windows")
            {
                var programFiles = Environment.GetEnvironmentVariable("ProgramFiles");
                var programFilesX86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
                var localAppData = Environment.GetEnvironmentVariable("LOCALAPPDATA");

                var windowsPaths = new[]
                {
                    Path.Combine(programFiles ?? "", "Blender Foundation"),
                    Path.Combine(programFilesX86 ?? "", "Blender Foundation"),
                    Path.Combine(localAppData ?? "", "Blender Foundation"),
                    Path.Combine(programFiles ?? "", "Steam", "steamapps", "common", "Blender"),
                    Path.Combine(programFilesX86 ?? "", "Steam", "steamapps", "common", "Blender"),
                    @"C:\ProgramData\chocolatey\lib\blender\tools",
                    @"C:\Blender",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Blender")
                };

                foreach (var basePath in windowsPaths)
                {
                    if (string.IsNullOrEmpty(basePath) || !Directory.Exists(basePath))
                    {
                        continue;
                    }

                    // Check for versioned directories
                    try
                    {
                        var dir = new DirectoryInfo(basePath);
                        foreach (var item in dir.GetDirectories())
                        {
                            if (item.Name.ToLowerInvariant().StartsWith("blender"))
                            {
                                var exe = new FileInfo(Path.Combine(item.FullName, "blender.exe"));
                                if (exe.Exists)
                                {
                                    paths.Add(exe.FullName);
                                }
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
            else if (system == "Darwin")
            {
                var appsDirs = new[]
                {
                    "/Applications",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Applications")
                };

                foreach (var appsDir in appsDirs)
                {
                    if (!Directory.Exists(appsDir))
                    {
                        continue;
                    }

                    try
                    {
                        var dir = new DirectoryInfo(appsDir);
                        foreach (var item in dir.GetDirectories())
                        {
                            if (item.Name.StartsWith("Blender", StringComparison.OrdinalIgnoreCase) && item.Extension == ".app")
                            {
                                var exe = new FileInfo(Path.Combine(item.FullName, "Contents", "MacOS", "Blender"));
                                if (exe.Exists)
                                {
                                    paths.Add(exe.FullName);
                                }
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                // Homebrew paths
                var homebrewPaths = new[] { "/opt/homebrew/bin/blender", "/usr/local/bin/blender" };
                foreach (var path in homebrewPaths)
                {
                    if (File.Exists(path))
                    {
                        paths.Add(path);
                    }
                }
            }
            else // Linux
            {
                var linuxPaths = new[]
                {
                    "/usr/bin/blender",
                    "/usr/local/bin/blender",
                    "/snap/bin/blender",
                    "/var/lib/snapd/snap/bin/blender",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local", "share", "flatpak", "exports", "bin", "org.blender.Blender"),
                    "/var/lib/flatpak/exports/bin/org.blender.Blender",
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local", "bin", "blender"),
                    "/opt/blender/blender"
                };

                foreach (var path in linuxPaths)
                {
                    if (File.Exists(path))
                    {
                        paths.Add(path);
                    }
                }

                // Check for versioned installations in /opt and home
                var optDirs = new[] { "/opt", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) };
                foreach (var optDir in optDirs)
                {
                    if (!Directory.Exists(optDir))
                    {
                        continue;
                    }

                    try
                    {
                        var dir = new DirectoryInfo(optDir);
                        foreach (var item in dir.GetDirectories())
                        {
                            if (item.Name.ToLowerInvariant().StartsWith("blender"))
                            {
                                var exe = new FileInfo(Path.Combine(item.FullName, "blender"));
                                if (exe.Exists)
                                {
                                    paths.Add(exe.FullName);
                                }
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return paths;
        }

        /// <summary>
        /// Get Blender from system PATH.
        /// </summary>
        private static string GetBlenderFromPath()
        {
            var pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (string.IsNullOrEmpty(pathEnv))
            {
                return null;
            }

            var pathDirs = pathEnv.Split(Path.PathSeparator);
            foreach (var dir in pathDirs)
            {
                if (string.IsNullOrEmpty(dir))
                {
                    continue;
                }

                foreach (var name in new[] { "blender", "blender.exe", "Blender" })
                {
                    var exePath = Path.Combine(dir, name);
                    if (File.Exists(exePath))
                    {
                        return exePath;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get Blender version from executable.
        /// </summary>
        private static (int Major, int Minor, int Patch)? GetBlenderVersion(string executable)
        {
            if (string.IsNullOrEmpty(executable) || !File.Exists(executable))
            {
                return null;
            }

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = executable,
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    if (process == null)
                    {
                        return null;
                    }

                    if (!process.WaitForExit(10000))
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch
                        {
                            // Best effort only; failed version probes should not block editor startup.
                        }

                        return null;
                    }

                    string output = process.StandardOutput.ReadToEnd();
                    if (string.IsNullOrEmpty(output))
                    {
                        output = process.StandardError.ReadToEnd();
                    }

                    return ParseBlenderVersion(output);
                }
            }
            catch
            {
                // Failed to get version
            }

            return null;
        }

        internal static (int Major, int Minor, int Patch)? ParseBlenderVersion(string output)
        {
            if (string.IsNullOrEmpty(output))
            {
                return null;
            }

            var match = Regex.Match(output, @"Blender\s+(\d+)\.(\d+)\.(\d+)");
            if (!match.Success)
            {
                return null;
            }

            int major = int.Parse(match.Groups[1].Value);
            int minor = int.Parse(match.Groups[2].Value);
            int patch = int.Parse(match.Groups[3].Value);
            return (major, minor, patch);
        }

        internal static bool IsSupportedVersion((int Major, int Minor, int Patch) version)
        {
            return version.Major > MinVersionMajor ||
                   (version.Major == MinVersionMajor && version.Minor > MinVersionMinor) ||
                   (version.Major == MinVersionMajor &&
                    version.Minor == MinVersionMinor &&
                    version.Patch >= MinVersionPatch);
        }

        /// <summary>
        /// Get Blender addons and extensions paths for a given version.
        /// </summary>
        private static (string AddonsPath, string ExtensionsPath) GetBlenderConfigPaths((int Major, int Minor, int Patch) version)
        {
            string versionStr = $"{version.Major}.{version.Minor}";
            string addonsPath = null;
            string extensionsPath = null;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var appData = Environment.GetEnvironmentVariable("APPDATA");
                if (!string.IsNullOrEmpty(appData))
                {
                    var basePath = Path.Combine(appData, "Blender Foundation", "Blender", versionStr);
                    addonsPath = Path.Combine(basePath, "scripts", "addons");
                    extensionsPath = Path.Combine(basePath, "extensions");
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var basePath = Path.Combine(home, "Library", "Application Support", "Blender", versionStr);
                addonsPath = Path.Combine(basePath, "scripts", "addons");
                extensionsPath = Path.Combine(basePath, "extensions");
            }
            else // Linux
            {
                var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                var basePath = Path.Combine(home, ".config", "blender", versionStr);
                addonsPath = Path.Combine(basePath, "scripts", "addons");
                extensionsPath = Path.Combine(basePath, "extensions");
            }

            return (addonsPath, extensionsPath);
        }

        /// <summary>
        /// Check if kotorblender is installed and get its version.
        /// </summary>
        internal static bool CheckKotorblenderInstalled(BlenderInfo info)
        {
            string kotorblenderPath = info.KotorblenderPath;
            if (string.IsNullOrEmpty(kotorblenderPath))
            {
                return false;
            }

            var initFile = new FileInfo(Path.Combine(kotorblenderPath, "__init__.py"));
            if (!initFile.Exists)
            {
                return false;
            }

            // Try to extract version from __init__.py
            try
            {
                string content = File.ReadAllText(initFile.FullName);
                var match = Regex.Match(content, @"\""version\""\s*:\s*\((\d+),\s*(\d+),\s*(\d+)\)");
                if (match.Success)
                {
                    info.KotorblenderVersion = $"{match.Groups[1].Value}.{match.Groups[2].Value}.{match.Groups[3].Value}";
                    return true;
                }
            }
            catch
            {
                // Failed to read or parse
            }

            // File exists but couldn't parse version - assume installed
            return true;
        }
    }
}
