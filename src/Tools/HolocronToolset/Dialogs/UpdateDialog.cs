using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HolocronToolset.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/update_dialog.py:83
    // Original: class UpdateDialog(QDialog):
    public partial class UpdateDialog : Window
    {
        private Dictionary<string, object> _remoteInfo;
        private List<object> _releases;
        private Dictionary<string, List<object>> _forksCache;
        private CheckBox _preReleaseCheckbox;
        private ComboBox _forkComboBox;
        private ComboBox _releaseComboBox;
        private TextBox _changelogEdit;
        private Button _fetchReleasesButton;
        private Button _installSelectedButton;
        private Button _updateLatestButton;

        // Public parameterless constructor for XAML
        public UpdateDialog() : this(null)
        {
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/update_dialog.py:84-100
        // Original: def __init__(self, parent=None):
        public UpdateDialog(Window parent)
        {
            InitializeComponent();
            Title = "Update Application";
            Width = 800;
            Height = 600;
            _remoteInfo = new Dictionary<string, object>();
            _releases = new List<object>();
            _forksCache = new Dictionary<string, List<object>>();
            SetupUI();
            InitConfig();
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
            var mainPanel = new StackPanel { Margin = new Avalonia.Thickness(10), Spacing = 10 };

            _preReleaseCheckbox = new CheckBox { Content = "Include Pre-releases" };
            mainPanel.Children.Add(_preReleaseCheckbox);

            _fetchReleasesButton = new Button { Content = "Fetch Releases", Height = 50 };
            _fetchReleasesButton.Click += (s, e) => InitConfig();
            mainPanel.Children.Add(_fetchReleasesButton);

            _forkComboBox = new ComboBox { MinWidth = 300 };
            mainPanel.Children.Add(new TextBlock { Text = "Select Fork:" });
            mainPanel.Children.Add(_forkComboBox);

            _releaseComboBox = new ComboBox { MinWidth = 300 };
            mainPanel.Children.Add(new TextBlock { Text = "Select Release:" });
            mainPanel.Children.Add(_releaseComboBox);

            _installSelectedButton = new Button { Content = "Install Selected", Width = 150, Height = 30 };
            _installSelectedButton.Click += (s, e) => OnInstallSelected();
            mainPanel.Children.Add(_installSelectedButton);

            _changelogEdit = new TextBox { IsReadOnly = true, AcceptsReturn = true, MinHeight = 200 };
            mainPanel.Children.Add(_changelogEdit);

            _updateLatestButton = new Button { Content = "Update to Latest", Height = 50 };
            _updateLatestButton.Click += (s, e) => OnUpdateLatestClicked();
            mainPanel.Children.Add(_updateLatestButton);

            Content = mainPanel;
        }

        private void SetupUI()
        {
            // Find controls from XAML
            _preReleaseCheckbox = this.FindControl<CheckBox>("preReleaseCheckbox");
            _forkComboBox = this.FindControl<ComboBox>("forkComboBox");
            _releaseComboBox = this.FindControl<ComboBox>("releaseComboBox");
            _changelogEdit = this.FindControl<TextBox>("changelogEdit");
            _fetchReleasesButton = this.FindControl<Button>("fetchReleasesButton");
            _installSelectedButton = this.FindControl<Button>("installSelectedButton");
            _updateLatestButton = this.FindControl<Button>("updateLatestButton");

            if (_fetchReleasesButton != null)
            {
                _fetchReleasesButton.Click += (s, e) => InitConfig();
            }
            if (_installSelectedButton != null)
            {
                _installSelectedButton.Click += (s, e) => OnInstallSelected();
            }
            if (_updateLatestButton != null)
            {
                _updateLatestButton.Click += (s, e) => OnUpdateLatestClicked();
            }
            if (_preReleaseCheckbox != null)
            {
                _preReleaseCheckbox.IsCheckedChanged += (s, e) => OnPreReleaseChanged();
            }
            if (_forkComboBox != null)
            {
                _forkComboBox.SelectionChanged += (s, e) => OnForkChanged();
            }
            if (_releaseComboBox != null)
            {
                _releaseComboBox.SelectionChanged += (s, e) => OnReleaseChanged();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/update_dialog.py:102-106
        // Original: def include_prerelease(self) -> bool:
        private bool IncludePrerelease()
        {
            return _preReleaseCheckbox?.IsChecked ?? false;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/update_dialog.py:180-185
        // Original: def init_config(self):
        private void InitConfig()
        {
            // TODO: Fetch and cache forks with releases when GitHub API integration is available
            System.Console.WriteLine("Init config - fetching releases");
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/update_dialog.py:225-229
        // Original: def on_pre_release_changed(self, state: bool):
        private void OnPreReleaseChanged()
        {
            FilterReleasesBasedOnPrerelease();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/update_dialog.py:231-249
        // Original: def filter_releases_based_on_prerelease(self):
        private void FilterReleasesBasedOnPrerelease()
        {
            // TODO: Filter releases based on prerelease checkbox when available
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/update_dialog.py:251-257
        // Original: def on_fork_changed(self, index: int):
        private void OnForkChanged()
        {
            FilterReleasesBasedOnPrerelease();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/update_dialog.py:263-273
        // Original: def on_release_changed(self, index: int):
        private void OnReleaseChanged()
        {
            // TODO: Update changelog when release selection changes
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/update_dialog.py:281-286
        // Original: def on_update_latest_clicked(self):
        private void OnUpdateLatestClicked()
        {
            // TODO: Get latest release and start update when available
            System.Console.WriteLine("Update to latest clicked");
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/update_dialog.py:288-294
        // Original: def on_install_selected(self):
        private void OnInstallSelected()
        {
            // TODO: Start update for selected release when available
            System.Console.WriteLine("Install selected clicked");
        }
    }
}
