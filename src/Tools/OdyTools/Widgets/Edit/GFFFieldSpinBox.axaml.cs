using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OdyTools.Widgets.Edit
{
    public partial class GFFFieldSpinBox : NumericUpDown
    {
        private Dictionary<int, string> _specialValueTextMapping;
        private int _minValue;
        private int? _cachedValue;

        // Public parameterless constructor for XAML
        public GFFFieldSpinBox()
        {
            InitializeComponent();
            _specialValueTextMapping = new Dictionary<int, string> { { 0, "0" }, { -1, "-1" } };
            _minValue = (int)Minimum;
            Minimum = -2147483646M;
            Maximum = 2147483647M;
            _cachedValue = null;
        }

        private void InitializeComponent()
        {
            try
            {
                AvaloniaXamlLoader.Load(this);
            }
            catch
            {
                // XAML not available - will use programmatic UI
            }
        }

        private int TrueMinimum()
        {
            int minValue = (int)Minimum;
            int specialMin = _specialValueTextMapping.Keys.Any() ? _specialValueTextMapping.Keys.Min() : minValue;
            return Math.Min(Math.Min(minValue, specialMin), _minValue);
        }

        private int TrueMaximum()
        {
            int maxValue = (int)Maximum;
            int specialMax = _specialValueTextMapping.Keys.Any() ? _specialValueTextMapping.Keys.Max() : maxValue;
            return Math.Max(maxValue, specialMax);
        }

        public void StepBy(int steps)
        {
            int currentValue = (int)Value;
            _cachedValue = NextValue(currentValue, steps);
            ApplyFinalValue(_cachedValue.HasValue ? _cachedValue.Value : currentValue);
        }

        private int NextValue(int currentValue, int steps)
        {
            int tentativeNextValue = currentValue + steps;
            int trueMin = TrueMinimum();
            if (tentativeNextValue < trueMin)
            {
                return trueMin;
            }
            int maxVal = (int)Maximum;
            if (tentativeNextValue > maxVal)
            {
                return maxVal;
            }

            var specialValues = _specialValueTextMapping.Keys.OrderBy(x => x).ToList();
            if (steps > 0)
            {
                foreach (int sv in specialValues)
                {
                    if (sv > currentValue && sv <= tentativeNextValue)
                    {
                        return sv;
                    }
                }
                if (_minValue > tentativeNextValue)
                {
                    return _minValue;
                }
                return Math.Min(tentativeNextValue, maxVal);
            }
            if (_minValue <= tentativeNextValue)
            {
                return tentativeNextValue;
            }
            int specialVal = -1;
            foreach (int sv in specialValues.OrderByDescending(x => x))
            {
                if (sv <= tentativeNextValue)
                {
                    return sv;
                }
            }
            return specialVal;
        }

        private void OnTextChanged(string text)
        {
            if (int.TryParse(text, out int parsedValue))
            {
                _cachedValue = parsedValue;
            }
            else
            {
                _cachedValue = (int)Value;
            }
        }

        private void ApplyFinalValue(int value)
        {
            if (value < TrueMinimum())
            {
                value = TrueMinimum();
            }
            else if (value > TrueMaximum())
            {
                value = TrueMaximum();
            }
            Value = (decimal)value;
            ValueChanged?.Invoke(value);
        }

        public void SetMinimum(int value)
        {
            _minValue = value;
            base.Minimum = Math.Min(-2, value);
        }

        // Intentionally hides base ValueChanged event (EventHandler<NumericUpDownValueChangedEventArgs>)
        // to provide Action<int> signature for GFF field value changes
        public new event Action<int> ValueChanged;
    }
}
