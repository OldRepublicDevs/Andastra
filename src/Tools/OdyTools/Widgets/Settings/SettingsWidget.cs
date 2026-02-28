using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using OdyTools.Data;
using OdyTools.Widgets.Edit;
using OdyTools.Common;
using BioWare.Common;
using BioWare.Utility;
using SettingsBase = OdyTools.Data.Settings;

namespace OdyTools.Widgets.Settings
{
    public abstract class SettingsWidget : UserControl
    {
        protected Dictionary<string, SetBindWidget> _binds;
        protected Dictionary<string, ColorEdit> _colours;
        protected SettingsBase _settings;
        protected NoScrollEventFilter _noScrollEventFilter;
        protected HoverEventFilter _hoverEventFilter;

        protected SettingsWidget()
        {
            _binds = new Dictionary<string, SetBindWidget>();
            _colours = new Dictionary<string, ColorEdit>();

            // Initialize event filters (set up when widget is loaded)
            _noScrollEventFilter = new NoScrollEventFilter();
            _hoverEventFilter = new HoverEventFilter();
        }

        // Override OnLoaded to automatically install event filters when widget is loaded
        // This ensures the widget tree is fully constructed before installing filters
        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);

            // Install event filters automatically when widget is loaded
            InstallEventFilters(this);
        }

        /// <summary>
        /// Recursively installs event filters on all child widgets matching the specified types.
        /// This method sets up NoScrollEventFilter and optionally HoverEventFilter on child controls
        /// to prevent scrollbar interaction with controls like ComboBox, Slider, etc.
        /// </summary>
        /// <param name="parentWidget">The parent widget to start installation from (typically 'this')</param>
        /// <param name="includeTypes">Optional array of control types to include. If null, uses default types: ComboBox, Slider, NumericUpDown, CheckBox</param>
        protected void InstallEventFilters(Control parentWidget, Type[] includeTypes = null)
        {
            if (parentWidget == null)
            {
                return;
            }

            // Set default include types if not provided (matching PyKotor default types)
            if (includeTypes == null)
            {
                includeTypes = new[]
                {
                    typeof(ComboBox),
                    typeof(Slider),
                    typeof(NumericUpDown),
                    typeof(CheckBox),
                    // Additional types from PyKotor: QGroupBox, QAbstractSpinBox, QDoubleSpinBox
                    // Note: Avalonia equivalents may differ, but these are the core types
                };
            }

            // Install NoScrollEventFilter (primary filter for preventing scrollbar interaction)
            // The NoScrollEventFilter.SetupFilter method handles recursive installation
            if (_noScrollEventFilter != null)
            {
                _noScrollEventFilter.SetupFilter(parentWidget, includeTypes);
            }

            // Note: HoverEventFilter installation is commented out in PyKotor (line 44)
            // So we don't install it here, but the instance is available if needed
        }

        protected Tuple<HashSet<Key>, HashSet<PointerUpdateKind>> ValidateBind(string bindName, Tuple<HashSet<Key>, HashSet<PointerUpdateKind>> bind)
        {
            if (bind == null || bind.Item1 == null || bind.Item2 == null)
            {
                System.Console.WriteLine($"Invalid setting bind: '{bindName}', expected a Bind type");
                bind = ResetAndGetDefaultBind(bindName);
            }
            return bind;
        }

        protected int ValidateColour(string colourName, object colorValue)
        {
            if (!UtilityMisc.IsInt(colorValue))
            {
                System.Console.WriteLine($"Invalid color setting: '{colourName}', expected a RGBA color integer, but got {colorValue} (type {colorValue?.GetType().Name ?? "null"})");
                return ResetAndGetDefaultColour(colourName);
            }
            // Convert to int if it's a valid integer
            return Convert.ToInt32(colorValue);
        }

        public virtual void Save()
        {
            foreach (var kvp in _binds)
            {
                var bind = ValidateBind(kvp.Key, kvp.Value.GetMouseAndKeyBinds());
                _settings.SetValue(kvp.Key, bind);
            }
            foreach (var kvp in _colours)
            {
                int colorValue = kvp.Value.GetColor().ToRgbaInteger();
                _settings.SetValue(kvp.Key, colorValue);
            }
        }

        protected void RegisterBind(SetBindWidget widget, string bindName)
        {
            var bind = ValidateBind(bindName, _settings.GetValue<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>>(bindName, null));
            widget.SetMouseAndKeyBinds(bind);
            _binds[bindName] = widget;
        }

        protected void RegisterColour(ColorEdit widget, string colourName)
        {
            // Get raw value from settings (may be any type) and validate it
            object rawValue = _settings.GetValue<object>(colourName, 0);
            int colorValue = ValidateColour(colourName, rawValue);
            widget.SetColor(BioWare.Common.Color.FromRgbaInteger(colorValue));
            _colours[colourName] = widget;
        }

        /// <summary>
        /// Resets a bind setting to its default value and returns the default.
        ///
        /// This method uses the SettingsProperty system to reset the setting and retrieve
        /// its default value. If the SettingsProperty system is not available for this setting,
        /// it falls back to returning an empty bind tuple.
        ///
        /// </summary>
        /// <param name="settingName">The name of the bind setting to reset.</param>
        /// <returns>The default bind value (tuple of Key set and PointerUpdateKind set).</returns>
        private Tuple<HashSet<Key>, HashSet<PointerUpdateKind>> ResetAndGetDefaultBind(string settingName)
        {
            try
            {
                // Reset the setting to its default value
                _settings.ResetSetting(settingName);

                // Get the default value from the SettingsProperty system
                object defaultValue = _settings.GetDefault(settingName);
                System.Console.WriteLine($"Due to last error, will use default value '{defaultValue}'");

                // Convert default value to bind tuple
                if (defaultValue is Tuple<HashSet<Key>, HashSet<PointerUpdateKind>> bindValue)
                {
                    return bindValue;
                }

                // Try to deserialize if it's stored as a different format
                // This handles cases where the value might be stored as JSON or another format
                if (defaultValue != null)
                {
                    try
                    {
                        // Try to convert using GetValue with the expected type
                        var convertedValue = _settings.GetValue<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>>(settingName, null);
                        if (convertedValue != null)
                        {
                            return convertedValue;
                        }
                    }
                    catch
                    {
                        // Conversion failed, fall through to default
                    }
                }

                // If default is not a bind tuple, return empty bind as fallback
                System.Console.WriteLine($"Warning: Default value for '{settingName}' is not a bind tuple, using empty bind");
                return Tuple.Create(new HashSet<Key>(), new HashSet<PointerUpdateKind>());
            }
            catch (Exception ex)
            {
                // If ResetSetting or GetDefault fails (e.g., property doesn't use SettingsProperty system),
                // return empty bind as fallback
                System.Console.WriteLine($"Error resetting bind setting '{settingName}': {ex.Message}");
                System.Console.WriteLine($"Warning: SettingsProperty system not available for '{settingName}', using empty bind as fallback");
                return Tuple.Create(new HashSet<Key>(), new HashSet<PointerUpdateKind>());
            }
        }

        private int ResetAndGetDefaultColour(string settingName)
        {
            try
            {
                _settings.ResetSetting(settingName);
                object defaultValue = _settings.GetDefault(settingName);
                System.Console.WriteLine($"Due to last error, will use default value '{defaultValue}'");

                // Convert default value to int
                if (defaultValue is int intValue)
                {
                    return intValue;
                }
                if (UtilityMisc.IsInt(defaultValue))
                {
                    return Convert.ToInt32(defaultValue);
                }
                // If default is not an int, return 0 (transparent black)
                System.Console.WriteLine($"Warning: Default value for '{settingName}' is not an integer, using 0");
                return 0;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error resetting color setting '{settingName}': {ex.Message}");
                // Return 0 (transparent black) as fallback
                return 0;
            }
        }
    }
}
