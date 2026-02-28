using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using BioWare.Common.Logger;
using OdyTools.Utils;

namespace OdyTools.NET
{
    public static class MainInit
    {
        public static bool IsFrozen()
        {
            string entryAssembly = Assembly.GetEntryAssembly()?.Location;
            if (string.IsNullOrEmpty(entryAssembly))
            {
                return false;
            }
            // Check if running from a single-file executable
            return !File.Exists(entryAssembly);
        }

        /// <summary>
        /// Determines if the application is running in debug mode.
        /// Checks for debugger attachment, environment variables, and frozen state.
        /// </summary>
        /// <returns>True if debug mode is enabled, false otherwise.</returns>
        public static bool IsDebugMode()
        {
            bool ret = false;

            // Check for DEBUG_MODE environment variable (equivalent to Python's DEBUG_MODE)
            string debugMode = Environment.GetEnvironmentVariable("DEBUG_MODE");
            if (debugMode == "1")
            {
                ret = true;
            }

            // Check if debugger is attached (equivalent to Python's sys.gettrace)
            if (Debugger.IsAttached)
            {
                ret = true;
            }

            // If frozen, disable debug mode (equivalent to Python's sys.frozen or sys._MEIPASS check)
            if (IsFrozen())
            {
                ret = false;
            }

            return ret;
        }

        /// <summary>
        /// Handles uncaught exceptions.
        /// This function should be called when an uncaught exception occurs, set to AppDomain.CurrentDomain.UnhandledException.
        /// </summary>
        /// <param name="exception">The uncaught exception</param>
        public static void OnAppCrash(Exception exception)
        {
            if (exception is System.Threading.ThreadAbortException)
            {
                return;
            }

            // Get log directory and create logger with log file path
            RobustLogger logger;
            try
            {
                string logDirectory = LogDirectoryHelper.GetLogDirectory();
                string logFilePath = Path.Combine(logDirectory, "andastra_toolset.log");
                logger = new RobustLogger(logFilePath);
            }
            catch (Exception ex)
            {
                // If log directory setup fails, use logger without file path (console only)
                // This ensures we can still log the exception even if file logging fails
                logger = new RobustLogger(null);
                // Log the setup failure, but don't let it prevent the actual exception from being logged
                System.Diagnostics.Debug.WriteLine($"Failed to setup log file path: {ex.Message}");
            }

            // Use Critical() method as in PyKotor, with excInfo=true to include full exception details
            logger.Critical("Uncaught exception", excInfo: true, exception: exception);
        }

        public static bool IsRunningFromTemp()
        {
            string entryAssembly = Assembly.GetEntryAssembly()?.Location;
            if (string.IsNullOrEmpty(entryAssembly))
            {
                return false;
            }
            string tempPath = Path.GetTempPath();
            return entryAssembly.StartsWith(tempPath, StringComparison.OrdinalIgnoreCase);
        }

        public static void Initialize()
        {
            // Set up exception handling
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                {
                    OnAppCrash(ex);
                }
            };

            // Check if running from temp directory
            if (IsRunningFromTemp())
            {
                throw new InvalidOperationException(
                    "This application cannot be run from within a zip or temporary directory. " +
                    "Please extract it to a permanent location before running.");
            }
        }
    }
}
