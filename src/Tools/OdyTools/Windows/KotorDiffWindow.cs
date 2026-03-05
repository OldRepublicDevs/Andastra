using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OdyTools.Data;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using OdyTools.Utils;
using IconType = MsBox.Avalonia.Enums.Icon;

namespace OdyTools.Windows
{
    public class KotorDiffWindow : Window
    {
        private Dictionary<string, OdyInstallation> _installations;
        private OdyInstallation _activeInstallation;

        public KotorDiffWindowUi Ui { get; private set; }

        public KotorDiffWindow(
            Window parent = null,
            Dictionary<string, OdyInstallation> installations = null,
            OdyInstallation activeInstallation = null)
        {
            InitializeComponent();
            _installations = installations ?? new Dictionary<string, OdyInstallation>();
            _activeInstallation = activeInstallation;
            SetupUI();
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
            Title = "KotorDiff - OdyTools";
            Width = 900;
            Height = 700;

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = "KotorDiff",
                FontSize = 18,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            panel.Children.Add(titleLabel);
            Content = panel;
        }

        private void SetupUI()
        {
            // Create UI wrapper for testing
            Ui = new KotorDiffWindowUi();
        }

        public async void Compare()
        {
            await DialogHelper.ShowWindowAsync(this, "KotorDiff", "KotorDiff comparison is not yet fully implemented. Use an external diff tool or wait for a future update.", ButtonEnum.Ok, IconType.Info);
        }
    }

    public class KotorDiffWindowUi
    {
    }
}
