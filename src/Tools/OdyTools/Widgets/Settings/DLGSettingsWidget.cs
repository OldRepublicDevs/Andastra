using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OdyTools.Data;
using OdyTools.Editors.DLG;

namespace OdyTools.Widgets.Settings
{
    public partial class DLGSettingsWidget : UserControl
    {
        private DLGSettings _dlgSettings;
        private TextBox _tlkPathEdit;
        private TextBox _femaleTlkPathEdit;
        private TextBox _override2DADirEdit;

        public DLGSettingsWidget()
        {
            _dlgSettings = new DLGSettings();
            InitializeComponent();
            SetupUI();
            SetupValues();
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

            if (!xamlLoaded)
            {
                SetupProgrammaticUI();
            }
        }

        private void SetupProgrammaticUI()
        {
            var panel = new StackPanel { Spacing = 10, Margin = new Avalonia.Thickness(10) };

            panel.Children.Add(new TextBlock
            {
                Text = "When no game installation is selected, the DLG editor uses these paths for TLK and 2DA files. To choose which installation the DLG editor uses (and to set manual paths in one place), open a DLG and go to File → DLG Settings.",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                MaxWidth = 500
            });

            panel.Children.Add(new TextBlock { Text = "TLK path (dialog.tlk):", FontWeight = Avalonia.Media.FontWeight.Bold });
            _tlkPathEdit = new TextBox { Watermark = "Full path to dialog.tlk", Name = "tlkPathEdit" };
            panel.Children.Add(_tlkPathEdit);

            panel.Children.Add(new TextBlock { Text = "Female TLK path (dialogf.tlk, optional):", FontWeight = Avalonia.Media.FontWeight.Bold });
            _femaleTlkPathEdit = new TextBox { Watermark = "Full path to dialogf.tlk", Name = "femaleTlkPathEdit" };
            panel.Children.Add(_femaleTlkPathEdit);

            panel.Children.Add(new TextBlock { Text = "2DA directory (folder containing dialoganimations.2da, emotions.2da, etc.):", FontWeight = Avalonia.Media.FontWeight.Bold });
            _override2DADirEdit = new TextBox { Watermark = "Folder path", Name = "override2DADirEdit" };
            panel.Children.Add(_override2DADirEdit);

            Content = panel;
        }

        private void SetupUI()
        {
            _tlkPathEdit = this.FindControl<TextBox>("tlkPathEdit");
            _femaleTlkPathEdit = this.FindControl<TextBox>("femaleTlkPathEdit");
            _override2DADirEdit = this.FindControl<TextBox>("override2DADirEdit");
            if (_tlkPathEdit == null) _tlkPathEdit = new TextBox();
            if (_femaleTlkPathEdit == null) _femaleTlkPathEdit = new TextBox();
            if (_override2DADirEdit == null) _override2DADirEdit = new TextBox();
        }

        private void SetupValues()
        {
            if (_tlkPathEdit != null)
                _tlkPathEdit.Text = _dlgSettings.TlkPath("");
            if (_femaleTlkPathEdit != null)
                _femaleTlkPathEdit.Text = _dlgSettings.FemaleTlkPath("");
            if (_override2DADirEdit != null)
                _override2DADirEdit.Text = _dlgSettings.Override2DADirectory("");
        }

        public void Save()
        {
            if (_tlkPathEdit != null)
                _dlgSettings.SetTlkPath(_tlkPathEdit.Text?.Trim() ?? "");
            if (_femaleTlkPathEdit != null)
                _dlgSettings.SetFemaleTlkPath(_femaleTlkPathEdit.Text?.Trim() ?? "");
            if (_override2DADirEdit != null)
                _dlgSettings.SetOverride2DADirectory(_override2DADirEdit.Text?.Trim() ?? "");
        }
    }
}
