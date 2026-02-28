using BioWare.Common;
using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using BioWare;

namespace OdyTools.Widgets.Edit
{
    public partial class PlainTextEdit : TextBox
    {
        private LocalizedString _locstring;

        // Public parameterless constructor for XAML
        public PlainTextEdit()
        {
            InitializeComponent();
            AcceptsReturn = true;
            AcceptsTab = false;
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

        public PlainTextEdit(LocalizedString locstring = null)
        {
            InitializeComponent();
            _locstring = locstring;
            AcceptsReturn = true;
            AcceptsTab = false;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            KeyReleased?.Invoke();
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (e.ClickCount == 2)
            {
                DoubleClicked?.Invoke();
            }
        }

        public LocalizedString Locstring
        {
            get => _locstring;
            set => _locstring = value;
        }

        public event Action KeyReleased;
        public event Action DoubleClicked;
    }
}
