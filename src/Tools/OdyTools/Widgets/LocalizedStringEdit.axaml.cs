using BioWare.Common;
using System;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using BioWare;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Editors;

namespace OdyTools.Widgets
{
    public partial class LocalizedStringEdit : UserControl
    {
        private TextBox _locstringText;
        private Button _editButton;
        private OdyInstallation _installation;
        private LocalizedString _locstring;

        public LocalizedStringEdit()
        {
            InitializeComponent();
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

            if (xamlLoaded)
            {
                _locstringText = EditorHelpers.FindControlSafe<TextBox>(this, "locstringText");
                _editButton = EditorHelpers.FindControlSafe<Button>(this, "editButton");
            }
            else
            {
                SetupProgrammaticUI();
                return;
            }

            if (_locstringText == null || _editButton == null)
            {
                SetupProgrammaticUI();
                return;
            }

            if (_editButton != null)
            {
                _editButton.Click += (s, e) => EditLocString();
            }
        }

        private void SetupProgrammaticUI()
        {
            // Create UI controls programmatically for test scenarios
            _locstringText = new TextBox { Name = "locstringText", IsReadOnly = true, Watermark = "Localized String" };
            _editButton = new Button { Name = "editButton", Content = "Edit" };
            _editButton.Click += (s, e) => EditLocString();

            var panel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5 };
            panel.Children.Add(_locstringText);
            panel.Children.Add(_editButton);
            Content = panel;
        }

        public void SetInstallation(OdyInstallation installation)
        {
            _installation = installation;
        }

        public void SetLocString(LocalizedString locstring)
        {
            _locstring = locstring ?? LocalizedString.FromInvalid();
            UpdateText();
        }

        public LocalizedString GetLocString()
        {
            return _locstring ?? LocalizedString.FromInvalid();
        }

        private void UpdateText()
        {
            if (_locstringText == null || _locstring == null)
            {
                return;
            }

            if (_locstring.StringRef == -1)
            {
                _locstringText.Text = _locstring.ToString();
            }
            else if (_installation != null)
            {
                _locstringText.Text = _installation.String(_locstring);
            }
            else
            {
                _locstringText.Text = $"StringRef: {_locstring.StringRef}";
            }
        }

        private async void EditLocString()
        {
            var parentWindow = TopLevel.GetTopLevel(this) as Window;
            var dialog = new LocalizedStringDialog(parentWindow, _installation, _locstring);
            var result = await dialog.ShowDialog<bool>(parentWindow);
            if (result)
            {
                _locstring = dialog.LocString;
                UpdateText();
            }
        }

        internal bool CanOpenEditorWithoutInstallationForTest()
        {
            return _installation == null;
        }
    }
}
