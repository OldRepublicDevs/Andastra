using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OdyTools.Widgets
{
    public partial class LongSpinBox : NumericUpDown
    {
        private long _min = 0;
        private long _max = 0xFFFFFFFF;

        // Public parameterless constructor for XAML
        public LongSpinBox()
        {
            InitializeComponent();
            Minimum = 0;
            Maximum = 0xFFFFFFFF;
        }

        private void InitializeComponent()
        {
            bool xamlLoaded = false;
            try
            {
                AvaloniaXamlLoader.Load(this);
                xamlLoaded = true;
            }
            catch
            {
                // XAML not available - will use programmatic UI
            }
        }

        public void StepUp()
        {
            long currentValue = GetValue();
            SetValue(currentValue + 1);
        }

        public void StepDown()
        {
            long currentValue = GetValue();
            SetValue(currentValue - 1);
        }

        public void StepBy(int steps)
        {
            long currentValue = GetValue();
            SetValue(currentValue + steps);
        }

        public void SetRange(long minValue, long maxValue)
        {
            _min = minValue;
            _max = maxValue;
            Minimum = minValue;
            Maximum = maxValue;
        }

        private bool WithinRange(long value)
        {
            return _min <= value && value <= _max;
        }

        private void ClampLineEdit()
        {
            if (Value.HasValue)
            {
                long value = (long)Value.Value;
                value = Math.Max(_min, Math.Min(_max, value));
                Value = value;
            }
            else
            {
                Value = 0;
            }
        }

        public void SetValue(long value)
        {
            value = Math.Max(_min, Math.Min(_max, value));
            Value = value;
            // Note: Setting Value property automatically raises ValueChanged event in Avalonia
        }

        public long GetValue()
        {
            if (Value.HasValue)
            {
                return (long)Value.Value;
            }
            return 0;
        }
    }
}
