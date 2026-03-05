using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using OdyTools.Data;

namespace OdyTools.Widgets.Settings
{
    /// <summary>
    /// Centralized utility for environment variable management across multiple settings widgets.
    /// Encapsulates grid/list operations (binding, selection), dictionary conversion, and settings mutations.
    /// Eliminates duplicated CRUD logic in ApplicationSettingsWidget and EnvVarsWidget.
    /// </summary>
    internal static class EnvironmentVariableGridHelper
    {
        /// <summary>
        /// Convert a dictionary of environment variables to a list of EnvironmentVariable objects.
        /// Used when loading settings from GlobalSettings into UI grids.
        /// </summary>
        /// <param name="envVars">Source dictionary mapping keys to values</param>
        /// <returns>List of EnvironmentVariable objects; empty list if input is null</returns>
        internal static List<EnvironmentVariable> FromDictionary(IReadOnlyDictionary<string, string> envVars)
        {
            var environmentVariables = new List<EnvironmentVariable>();
            if (envVars == null)
            {
                return environmentVariables;
            }

            foreach (var kvp in envVars)
            {
                environmentVariables.Add(new EnvironmentVariable(kvp.Key, kvp.Value));
            }

            return environmentVariables;
        }

        /// <summary>
        /// Rebind a DataGrid to an updated list of environment variables.
        /// Clears current ItemsSource and reassigns to trigger UI refresh.
        /// </summary>
        /// <param name="tableWidget">DataGrid to refresh</param>
        /// <param name="environmentVariables">Updated list of EnvironmentVariable objects</param>
        internal static void RefreshGrid(DataGrid tableWidget, IReadOnlyList<EnvironmentVariable> environmentVariables)
        {
            if (tableWidget == null)
            {
                return;
            }

            tableWidget.ItemsSource = null;
            tableWidget.ItemsSource = environmentVariables;
        }

        /// <summary>
        /// Get the currently selected row from a DataGrid as an EnvironmentVariable.
        /// </summary>
        /// <param name="tableWidget">DataGrid to query</param>
        /// <param name="selected">Output: selected EnvironmentVariable, or null if none selected</param>
        /// <returns>True if an EnvironmentVariable is selected; false otherwise</returns>
        internal static bool TryGetSelected(DataGrid tableWidget, out EnvironmentVariable selected)
        {
            selected = null;
            if (tableWidget == null || !(tableWidget.SelectedItem is EnvironmentVariable))
            {
                return false;
            }

            selected = (EnvironmentVariable)tableWidget.SelectedItem;
            return true;
        }

        /// <summary>
        /// Check if a key already exists in the environment variable list (case-insensitive).
        /// Used for duplicate key validation in edit/add dialogs.
        /// </summary>
        /// <param name="environmentVariables">List to search</param>
        /// <param name="key">Key to search for (case-insensitive)</param>
        /// <param name="except">Optional: exclude specific EnvironmentVariable from check (for edit scenario)</param>
        /// <returns>True if key exists elsewhere in list; false if not found or list is null</returns>
        internal static bool HasKey(IEnumerable<EnvironmentVariable> environmentVariables, string key, EnvironmentVariable except = null)
        {
            if (environmentVariables == null || string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return environmentVariables.Any(ev =>
                !ReferenceEquals(ev, except)
                && ev.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Convert a list of EnvironmentVariable objects back to a dictionary.
        /// Used when saving UI state back to GlobalSettings.
        /// </summary>
        /// <param name="environmentVariables">List of EnvironmentVariable objects</param>
        /// <returns>Dictionary mapping keys to values; empty dict if input is null</returns>
        internal static Dictionary<string, string> ToDictionary(IEnumerable<EnvironmentVariable> environmentVariables)
        {
            var envVars = new Dictionary<string, string>();
            if (environmentVariables == null)
            {
                return envVars;
            }

            foreach (var envVar in environmentVariables)
            {
                if (!string.IsNullOrWhiteSpace(envVar.Key))
                {
                    envVars[envVar.Key] = envVar.Value ?? "";
                }
            }

            return envVars;
        }

        /// <summary>
        /// Insert or update an environment variable in GlobalSettings.
        /// If key exists, overwrites value; if new, adds entry.
        /// </summary>
        /// <param name="settings">GlobalSettings instance to modify</param>
        /// <param name="key">Environment variable key</param>
        /// <param name="value">Environment variable value (null treated as empty string)</param>
        internal static void UpsertSetting(GlobalSettings settings, string key, string value)
        {
            if (settings == null || string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            var envVars = settings.AppEnvVariables;
            envVars[key] = value ?? "";
            settings.AppEnvVariables = envVars;
        }

        /// <summary>
        /// Remove an environment variable from GlobalSettings.
        /// Safe to call if key does not exist (no-op).
        /// </summary>
        /// <param name="settings">GlobalSettings instance to modify</param>
        /// <param name="key">Environment variable key to remove</param>
        internal static void RemoveSetting(GlobalSettings settings, string key)
        {
            if (settings == null || string.IsNullOrWhiteSpace(key))
            {
                return;
            }

            var envVars = settings.AppEnvVariables;
            if (!envVars.ContainsKey(key))
            {
                return;
            }

            envVars.Remove(key);
            settings.AppEnvVariables = envVars;
        }
    }
}
