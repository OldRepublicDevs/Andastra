// Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1951-1980
// Original: class InstallationLogger: ...
using System;
using System.Collections.Generic;
using System.Linq;

namespace KotorDiff.Diff
{
    // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1951-1980
    // Original: class InstallationLogger: ...
    /// <summary>
    /// Logger that captures installation search output to a string.
    /// </summary>
    public class InstallationLogger
    {
        private readonly List<string> _logBuffer = new List<string>();
        private string _currentResource = null;
        private readonly Dictionary<string, List<string>> _resourceLogs = new Dictionary<string, List<string>>();

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1954-1956
        // Original: def __init__(self): ...
        public InstallationLogger()
        {
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1959-1968
        // Original: def __call__(self, message: str) -> None: ...
        /// <summary>
        /// Log a message and store it in the buffer.
        /// </summary>
        public void Log(string message)
        {
            _logBuffer.Add(message);

            // If this is a new resource being processed, start a new log entry
            if (message.ToLowerInvariant().StartsWith("processing resource: "))
            {
                string[] parts = message.Split(new[] { ": " }, 2, StringSplitOptions.None);
                if (parts.Length > 1)
                {
                    _currentResource = parts[1].Trim();
                    _resourceLogs[_currentResource] = new List<string>();
                }
            }
            else if (!string.IsNullOrEmpty(_currentResource))
            {
                if (!_resourceLogs.ContainsKey(_currentResource))
                {
                    _resourceLogs[_currentResource] = new List<string>();
                }
                _resourceLogs[_currentResource].Add(message);
            }
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1970-1974
        // Original: def get_resource_log(self, resource_name: str) -> str: ...
        /// <summary>
        /// Get the log output for a specific resource.
        /// </summary>
        public string GetResourceLog(string resourceName)
        {
            if (_resourceLogs.ContainsKey(resourceName))
            {
                return string.Join("\n", _resourceLogs[resourceName]);
            }
            return "";
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1976-1980
        // Original: def clear(self) -> None: ...
        /// <summary>
        /// Clear the log buffer.
        /// </summary>
        public void Clear()
        {
            _logBuffer.Clear();
            _resourceLogs.Clear();
            _currentResource = null;
        }

        /// <summary>
        /// Get all log messages.
        /// </summary>
        public List<string> GetAllLogs()
        {
            return new List<string>(_logBuffer);
        }
    }
}

