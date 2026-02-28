using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BioWare.Resource.Formats.TwoDA;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using OdyTools.Data;
using OdyTools.Editors;
using OdyTools.Editors.DLG;
using OdyTools.Widgets.Edit;

namespace OdyTools.Dialogs.Edit
{
    public partial class DialogAnimationDialog : Window
    {
        private OdyInstallation _installation;
        private DLGAnimation _animation;
        private TwoDA _anim2DA;
        private string _anim2DASource;

        // 2DA table
        private TextBlock _anim2daSourceText;
        private TextBlock _anim2daStatsText;
        private DataGrid _anim2daGrid;

        // Primary: animation selection
        private ComboBox2DA _animationSelect;
        private NumericUpDown _animationIndexSpin;
        private TextBox _animationFilterBox;
        private Button _applyNameButton;
        private Button _clearFilterButton;

        // Participant
        private TextBox _participantEdit;

        // Stats / details
        private Panel _statsPanel;
        private TextBlock _statsSummary;
        private Panel _statsDetailsPanel;

        // Buttons
        private Button _okButton;
        private Button _cancelButton;

        private bool _suppressSync;

        public DialogAnimationDialog() : this(null, null, null)
        {
        }

        public DialogAnimationDialog(Window parent, OdyInstallation installation, DLGAnimation animationArg = null)
        {
            InitializeComponent();
            _installation = installation;
            _animation = animationArg ?? new DLGAnimation();
            _anim2DA = null;
            _suppressSync = false;
            SetupUI();
            LoadAnimationData();
            RefreshStats();
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
            Title = "Edit Animation";
            Width = 480;
            Height = 280;

            var panel = new StackPanel { Margin = new Avalonia.Thickness(10), Spacing = 10 };
            var animationLabel = new TextBlock { Text = "Animation:" };
            _animationSelect = new ComboBox2DA();
            var participantLabel = new TextBlock { Text = "Participant:" };
            _participantEdit = new TextBox();
            var okButton = new Button { Content = "OK" };
            okButton.Click += (s, e) => { ApplyToAnimation(); Close(true); };
            var cancelButton = new Button { Content = "Cancel" };
            cancelButton.Click += (s, e) => Close(false);

            panel.Children.Add(animationLabel);
            panel.Children.Add(_animationSelect);
            panel.Children.Add(participantLabel);
            panel.Children.Add(_participantEdit);
            panel.Children.Add(okButton);
            panel.Children.Add(cancelButton);
            Content = panel;
        }

        private void SetupUI()
        {
            try
            {
                _anim2daSourceText = EditorHelpers.FindControlSafe<TextBlock>(this, "anim2daSourceText");
                _anim2daStatsText = EditorHelpers.FindControlSafe<TextBlock>(this, "anim2daStatsText");
                _anim2daGrid = EditorHelpers.FindControlSafe<DataGrid>(this, "anim2daGrid");
                _animationSelect = EditorHelpers.FindControlSafe<ComboBox2DA>(this, "animationSelect");
                _animationIndexSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "animationIndexSpin");
                _animationFilterBox = EditorHelpers.FindControlSafe<TextBox>(this, "animationFilterBox");
                _applyNameButton = EditorHelpers.FindControlSafe<Button>(this, "applyNameButton");
                _clearFilterButton = EditorHelpers.FindControlSafe<Button>(this, "clearFilterButton");
                _participantEdit = EditorHelpers.FindControlSafe<TextBox>(this, "participantEdit");
                _statsPanel = EditorHelpers.FindControlSafe<Panel>(this, "statsPanel");
                _statsSummary = EditorHelpers.FindControlSafe<TextBlock>(this, "statsSummary");
                _statsDetailsPanel = EditorHelpers.FindControlSafe<Panel>(this, "statsDetailsPanel");
                _okButton = EditorHelpers.FindControlSafe<Button>(this, "okButton");
                _cancelButton = EditorHelpers.FindControlSafe<Button>(this, "cancelButton");

                if (_animationSelect == null && _participantEdit == null)
                {
                    SetupProgrammaticUI();
                    return;
                }

                if (_okButton != null)
                    _okButton.Click += (s, e) => { ApplyToAnimation(); Close(true); };
                if (_cancelButton != null)
                    _cancelButton.Click += (s, e) => Close(false);

                if (_animationSelect != null)
                    _animationSelect.SelectionChanged += OnAnimationSelectionChanged;
                if (_animationIndexSpin != null)
                    _animationIndexSpin.ValueChanged += OnAnimationIndexSpinChanged;
                if (_applyNameButton != null)
                    _applyNameButton.Click += OnApplyNameClick;
                if (_clearFilterButton != null)
                    _clearFilterButton.Click += (s, e) =>
                    {
                        if (_animationFilterBox != null)
                            _animationFilterBox.Text = "";
                    };
                if (_anim2daGrid != null)
                    _anim2daGrid.SelectionChanged += OnGridSelectionChanged;
            }
            catch
            {
                SetupProgrammaticUI();
            }
        }

        private void OnAnimationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_suppressSync || _animationIndexSpin == null || _animationSelect == null)
                return;
            int row = _animationSelect.SelectedIndex;
            _suppressSync = true;
            try
            {
                _animationIndexSpin.Value = Math.Max(0, Math.Min(row, 65535));
            }
            finally
            {
                _suppressSync = false;
            }
            RefreshStats();
        }

        private void OnAnimationIndexSpinChanged(object sender, NumericUpDownValueChangedEventArgs e)
        {
            if (_suppressSync || _animationIndexSpin == null || _animationSelect == null)
                return;
            int row = (int)(_animationIndexSpin.Value ?? 0);
            _suppressSync = true;
            try
            {
                _animationSelect.SetSelectedIndex(row);
            }
            finally
            {
                _suppressSync = false;
            }
            RefreshStats();
        }

        private void OnApplyNameClick(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_animationSelect == null || _animationFilterBox == null)
                return;
            string name = (_animationFilterBox.Text ?? "").Trim();
            if (string.IsNullOrEmpty(name))
                return;

            // Resolve by name using 2DA if available
            if (_anim2DA != null)
            {
                try
                {
                    List<string> nameCol = _anim2DA.GetColumn("name");
                    for (int row = 0; row < nameCol.Count; row++)
                    {
                        string rowName = nameCol[row] ?? "";
                        if (rowName.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            _animationSelect.SetSelectedIndex(row);
                            if (_animationIndexSpin != null)
                                _animationIndexSpin.Value = row;
                            RefreshStats();
                            return;
                        }
                    }
                }
                catch (KeyNotFoundException)
                {
                    // no "name" column
                }
            }

            // Fallback: try as row index
            if (int.TryParse(name, out int idx) && idx >= 0)
            {
                _animationSelect.SetSelectedIndex(idx);
                if (_animationIndexSpin != null)
                    _animationIndexSpin.Value = idx;
                RefreshStats();
            }
        }

        private void LoadAnimationData()
        {
            var dlgSettings = new DLGSettings();
            var customFolders = dlgSettings.GetCustom2DAFolders();
            _anim2DASource = null;

            if (_installation != null)
            {
                _anim2DA = _installation.Get2DAWithCustomFolders(OdyInstallation.TwoDADialogAnims, customFolders);
                if (_anim2DA != null)
                    _anim2DASource = $"Installation \"{_installation.Name}\" ({_installation.Path})";
            }
            if (_anim2DA == null)
            {
                var path = dlgSettings.Resolve2DAPath(OdyInstallation.TwoDADialogAnims);
                if (!string.IsNullOrEmpty(path))
                {
                    _anim2DA = TwoDAFileHelper.LoadFromPath(path);
                    if (_anim2DA != null)
                        _anim2DASource = path;
                }
            }

            UpdateSourceDisplay();

            if (_anim2DA == null)
            {
                if (_participantEdit != null)
                    _participantEdit.Text = _animation.Participant ?? "";
                if (_animationIndexSpin != null)
                    _animationIndexSpin.Value = Math.Max(0, _animation.AnimationId);
                return;
            }

            Populate2DAGrid();

            List<string> nameColumn;
            try
            {
                nameColumn = _anim2DA.GetColumn("name");
            }
            catch (KeyNotFoundException)
            {
                nameColumn = new List<string>();
                for (int i = 0; i < _anim2DA.GetHeight(); i++)
                    nameColumn.Add(i.ToString());
            }

            if (_animationSelect != null)
            {
                _animationSelect.SetItems(nameColumn, sortAlphabetically: false, cleanupStrings: false, ignoreBlanks: false);
                _animationSelect.SetSelectedIndex(_animation.AnimationId);
                _animationSelect.SetContext(_anim2DA, _installation, OdyInstallation.TwoDADialogAnims);
            }

            if (_animationIndexSpin != null)
            {
                _animationIndexSpin.Maximum = Math.Max(0, _anim2DA.GetHeight() - 1);
                _animationIndexSpin.Value = Math.Max(0, Math.Min(_animation.AnimationId, (int)_animationIndexSpin.Maximum));
            }

            if (_participantEdit != null)
                _participantEdit.Text = _animation.Participant ?? "";
        }

        private void UpdateSourceDisplay()
        {
            if (_anim2daSourceText != null)
            {
                _anim2daSourceText.Text = _anim2DASource != null
                    ? $"Source: {_anim2DASource}"
                    : "Source: Not loaded — set an installation or 2DA path in File → DLG Settings.";
            }
            if (_anim2daStatsText != null)
            {
                if (_anim2DA != null)
                    _anim2daStatsText.Text = $"{_anim2DA.GetHeight()} rows, {_anim2DA.GetWidth()} columns ({string.Join(", ", _anim2DA.GetHeaders())})";
                else
                    _anim2daStatsText.Text = "";
            }
        }

        private void Populate2DAGrid()
        {
            if (_anim2daGrid == null || _anim2DA == null)
                return;

            _anim2daGrid.Columns.Clear();
            var headers = _anim2DA.GetHeaders();
            int rowCount = _anim2DA.GetHeight();

            _anim2daGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Row",
                Binding = new Avalonia.Data.Binding("[0]"),
                Width = new DataGridLength(50),
                IsReadOnly = true
            });
            for (int c = 0; c < headers.Count; c++)
            {
                int bindIdx = c + 1;
                _anim2daGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = headers[c],
                    Binding = new Avalonia.Data.Binding($"[{bindIdx}]"),
                    IsReadOnly = true
                });
            }

            var items = new ObservableCollection<string[]>();
            for (int r = 0; r < rowCount; r++)
            {
                try
                {
                    var row = _anim2DA.GetRow(r, "dialoganimations.2da");
                    var data = row.GetData();
                    var vals = new string[headers.Count + 1];
                    vals[0] = r.ToString();
                    for (int c = 0; c < headers.Count; c++)
                    {
                        data.TryGetValue(headers[c], out string val);
                        vals[c + 1] = val ?? "";
                    }
                    items.Add(vals);
                }
                catch
                {
                    var vals = new string[headers.Count + 1];
                    vals[0] = r.ToString();
                    items.Add(vals);
                }
            }
            _anim2daGrid.ItemsSource = items;
        }

        private void OnGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_suppressSync || _anim2daGrid == null)
                return;
            if (_anim2daGrid.SelectedItem is string[] row && row.Length > 0 && int.TryParse(row[0], out int rowIndex))
            {
                _suppressSync = true;
                try
                {
                    _animationSelect?.SetSelectedIndex(rowIndex);
                    if (_animationIndexSpin != null)
                        _animationIndexSpin.Value = Math.Max(0, Math.Min(rowIndex, (int)_animationIndexSpin.Maximum));
                }
                finally
                {
                    _suppressSync = false;
                }
                RefreshStats();
            }
        }

        private void RefreshStats()
        {
            if (_statsSummary == null)
                return;

            if (_anim2DA == null)
            {
                _statsSummary.Text = "No dialoganimations.2da loaded. In File → DLG Settings choose an installation and/or set the 2DA directory or dialoganimations.2da path under Manual paths.";
                if (_statsDetailsPanel != null)
                    _statsDetailsPanel.Children.Clear();
                return;
            }

            int rows = _anim2DA.GetHeight();
            int cols = _anim2DA.GetWidth();
            int selectedRow = _animationSelect?.SelectedIndex ?? 0;
            if (selectedRow < 0)
                selectedRow = 0;
            if (rows > 0 && selectedRow >= rows)
                selectedRow = rows - 1;

            _statsSummary.Text = rows == 0
                ? $"2DA has 0 rows. Add rows in dialoganimations.2da or select a valid installation."
                : $"Rows: {rows}, Columns: {cols}. Selected row index: {selectedRow}.";

            if (_statsDetailsPanel != null)
            {
                _statsDetailsPanel.Children.Clear();
                if (rows == 0)
                    return;
                try
                {
                    TwoDARow row = _anim2DA.GetRow(selectedRow, "dialoganimations.2da");
                    var data = row.GetData();
                    foreach (var kv in data.OrderBy(x => x.Key))
                    {
                        _statsDetailsPanel.Children.Add(new TextBlock
                        {
                            Text = $"{kv.Key}: {kv.Value}",
                            TextWrapping = Avalonia.Media.TextWrapping.Wrap
                        });
                    }
                }
                catch (Exception ex)
                {
                    _statsDetailsPanel.Children.Add(new TextBlock
                    {
                        Text = $"Row {selectedRow}: {ex.Message}",
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap
                    });
                }
            }
        }

        private void ApplyToAnimation()
        {
            if (_animation == null)
                return;
            if (_animationSelect != null)
                _animation.AnimationId = _animationSelect.SelectedIndex;
            if (_animationIndexSpin != null)
                _animation.AnimationId = Math.Max(0, (int)(_animationIndexSpin.Value ?? 0));
            if (_participantEdit != null)
                _animation.Participant = _participantEdit.Text ?? "";
        }

        public DLGAnimation GetAnimation()
        {
            ApplyToAnimation();
            return _animation != null
                ? new DLGAnimation { AnimationId = _animation.AnimationId, Participant = _animation.Participant ?? "" }
                : new DLGAnimation();
        }
    }
}
