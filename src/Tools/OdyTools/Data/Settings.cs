using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OdyTools.Data
{
    public class Settings
    {
        private readonly string _scope;
        private readonly Dictionary<string, object> _values = new Dictionary<string, object>();
        private static readonly string SettingsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "OdyToolsV3");

        public Settings(string scope)
        {
            _scope = scope;
            Load();
        }

        private string GetSettingsFilePath()
        {
            if (!Directory.Exists(SettingsDirectory))
            {
                Directory.CreateDirectory(SettingsDirectory);
            }
            return Path.Combine(SettingsDirectory, $"{_scope}.json");
        }

        private void Load()
        {
            string filePath = GetSettingsFilePath();
            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    var loaded = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                    if (loaded != null)
                    {
                        foreach (var kvp in loaded)
                        {
                            _values[kvp.Key] = kvp.Value;
                        }
                    }
                }
                catch
                {
                    // If loading fails, use defaults
                }
            }
        }

        public void Save()
        {
            try
            {
                string filePath = GetSettingsFilePath();
                string json = JsonConvert.SerializeObject(_values, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch
            {
                // Ignore save errors
            }
        }

        public T GetValue<T>(string name, T defaultValue)
        {
            if (_values.TryGetValue(name, out object value))
            {
                try
                {
                    if (value is JToken token)
                    {
                        return token.ToObject<T>();
                    }
                    if (value is T)
                    {
                        return (T)value;
                    }
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }

        public void SetValue<T>(string name, T value)
        {
            _values[name] = value;
            Save();
        }

        public SettingsProperty<T> GetProperty<T>(string name)
        {
            var prop = GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null && prop.GetValue(this) is SettingsProperty<T> settingsProp)
            {
                return settingsProp;
            }
            throw new ArgumentException($"'{GetType().Name}' object has no property '{name}'");
        }

        // Uses IResettableSettingsProperty so any SettingsProperty<T> works (not just SettingsProperty<object>).
        public object GetDefault(string name)
        {
            var prop = GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null && prop.GetValue(this) is IResettableSettingsProperty resettable)
            {
                return resettable.GetDefaultValue();
            }
            throw new ArgumentException($"'{GetType().Name}' object has no property '{name}'");
        }

        // Uses IResettableSettingsProperty so any SettingsProperty<T> works (not just SettingsProperty<object>).
        public void ResetSetting(string name)
        {
            var prop = GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null && prop.GetValue(this) is IResettableSettingsProperty resettable)
            {
                resettable.ResetToDefault(this);
                return;
            }
            throw new ArgumentException($"'{GetType().Name}' object has no property '{name}'");
        }

        public void Clear()
        {
            _values.Clear();
            Save();
        }
    }

    /// <summary>
    /// Allows GetDefault/ResetSetting to work with any SettingsProperty&lt;T&gt; without requiring T to be object.
    /// </summary>
    internal interface IResettableSettingsProperty
    {
        void ResetToDefault(Settings settings);
        object GetDefaultValue();
    }

    public class SettingsProperty<T> : IResettableSettingsProperty
    {
        public string Name { get; }
        public T Default { get; }

        public SettingsProperty(string name, T defaultValue)
        {
            Name = name;
            Default = defaultValue;
        }

        public T GetValue(Settings settings)
        {
            return settings.GetValue(Name, Default);
        }

        public void SetValue(Settings settings, T value)
        {
            settings.SetValue(Name, value);
        }

        public void ResetToDefault(Settings settings)
        {
            settings.SetValue(Name, Default);
        }

        void IResettableSettingsProperty.ResetToDefault(Settings settings) => ResetToDefault(settings);
        object IResettableSettingsProperty.GetDefaultValue() => Default;
    }
}
