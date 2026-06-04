using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using BioWare.Resource.Formats.LIP;
using BioWare.Common;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Utils;
using OdyTools.Widgets;

namespace OdyTools.Editors
{
    public partial class OdyToolLIP : Editor
    {
        private const int MinEditorWidth = 480;
        private const int MinEditorHeight = 400;
        private const int UndoMaxLevels = 30;

        private LIP _lip;
        private float _duration;

        private Avalonia.Controls.TextBlock _statusText;
        private bool _uiSyncInProgress;
        private ListBox _keyframeList;
        private NumericUpDown _durationSpin;
        private NumericUpDown _timeSpin;
        private ComboBox _shapeCombo;
        private Button _addKeyframeButton;
        private Button _updateKeyframeButton;
        private Button _deleteKeyframeButton;
        private TextBox _audioPathBox;
        private Button _loadAudioButton;
        private Button _playPreviewButton;
        private Button _stopPreviewButton;
        private string _audioFilePath;
        private NAudioMediaPlayer _previewPlayer;
        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private readonly List<byte[]> _redoStack = new List<byte[]>();
        private bool _undoRedoInProgress;
        private string _findText = "";
        private bool _findMatchCase;
        private int _lastFindIndex = -1;

        public OdyToolLIP() : this(null, null) { }
        public OdyToolLIP(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolLIP", "lip",
                new[] { ResourceType.LIP, ResourceType.LIP_XML, ResourceType.LIP_JSON },
                new[] { ResourceType.LIP, ResourceType.LIP_XML, ResourceType.LIP_JSON },
                installation)
        {
            _lip = null;
            _duration = 0.0f;
            _previewPlayer = new NAudioMediaPlayer();
            InitializeComponent();
            SetupSignals();
            SetupMenuHandlers();
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
            New();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
            SetupUI();
        }

        private Menu BuildMenu()
        {
            var menu = new Menu();
            var fileMenu = new MenuItem { Header = "_File" };
            fileMenu.Items.Add(new MenuItem { Header = "_New", Name = "actionNew" });
            fileMenu.Items.Add(new MenuItem { Header = "_Open", Name = "actionOpen" });
            fileMenu.Items.Add(new MenuItem { Header = "_Save", Name = "actionSave" });
            fileMenu.Items.Add(new MenuItem { Header = "Save _As", Name = "actionSaveAs" });
            fileMenu.Items.Add(new Separator());
            fileMenu.Items.Add(new MenuItem { Header = "_Revert", Name = "actionRevert" });
            fileMenu.Items.Add(new Separator());
            fileMenu.Items.Add(new MenuItem { Header = "E_xit", Name = "actionExit" });
            menu.Items.Add(fileMenu);
            var editMenu = new MenuItem { Header = "_Edit" };
            editMenu.Items.Add(new MenuItem { Header = "_Undo", Name = "actionUndo" });
            editMenu.Items.Add(new MenuItem { Header = "_Redo", Name = "actionRedo" });
            editMenu.Items.Add(new Separator());
            editMenu.Items.Add(new MenuItem { Header = "Find...", Name = "actionFind" });
            editMenu.Items.Add(new MenuItem { Header = "Find _Next", Name = "actionFindNext" });
            menu.Items.Add(editMenu);
            var toolsMenu = new MenuItem { Header = "_Tools" };
            toolsMenu.Items.Add(new MenuItem { Header = "Batch Process WAV to LIP...", Name = "actionBatchProcessWavToLip" });
            menu.Items.Add(toolsMenu);
            return menu;
        }

        private void SetupUI()
        {
            var dock = new DockPanel();
            var menu = BuildMenu();
            dock.Children.Add(menu);
            DockPanel.SetDock(menu, Dock.Top);
            dock.Children.Add(BuildEditorPanel());
            _statusText = new Avalonia.Controls.TextBlock { Name = "statusText", Text = "LIP", Margin = new Avalonia.Thickness(4, 2) };
            dock.Children.Add(_statusText);
            DockPanel.SetDock(_statusText, Dock.Bottom);
            SetContentOrInject(dock);
        }

        private Control BuildEditorPanel()
        {
            var root = new Grid
            {
                Margin = new Avalonia.Thickness(8),
                RowDefinitions = new RowDefinitions("Auto,Auto,*,Auto"),
                ColumnDefinitions = new ColumnDefinitions("*"),
            };

            var audioRow = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto,Auto,Auto"),
                Margin = new Avalonia.Thickness(0, 0, 0, 8),
            };
            audioRow.Children.Add(new TextBlock
            {
                Text = "Audio:",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(0, 0, 8, 0),
            });
            _audioPathBox = new TextBox { IsReadOnly = true, Watermark = "No audio loaded" };
            Grid.SetColumn(_audioPathBox, 1);
            audioRow.Children.Add(_audioPathBox);
            _loadAudioButton = new Button { Content = "Load...", Margin = new Avalonia.Thickness(8, 0, 0, 0) };
            _loadAudioButton.Click += async (s, e) => await LoadAudioFromPickerAsync();
            Grid.SetColumn(_loadAudioButton, 2);
            audioRow.Children.Add(_loadAudioButton);
            _playPreviewButton = new Button { Content = "Play", Margin = new Avalonia.Thickness(4, 0, 0, 0) };
            _playPreviewButton.Click += (s, e) => PlayPreview();
            Grid.SetColumn(_playPreviewButton, 3);
            audioRow.Children.Add(_playPreviewButton);
            _stopPreviewButton = new Button { Content = "Stop", Margin = new Avalonia.Thickness(4, 0, 0, 0) };
            _stopPreviewButton.Click += (s, e) => StopPreview();
            Grid.SetColumn(_stopPreviewButton, 4);
            audioRow.Children.Add(_stopPreviewButton);
            Grid.SetRow(audioRow, 0);
            root.Children.Add(audioRow);

            var durationRow = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto"),
                Margin = new Avalonia.Thickness(0, 0, 0, 8),
            };
            durationRow.Children.Add(new TextBlock
            {
                Text = "Duration (s):",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(0, 0, 8, 0),
            });
            _durationSpin = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 999.999m,
                Increment = 0.001m,
                FormatString = "F3",
                Width = 120,
            };
            _durationSpin.ValueChanged += OnDurationSpinChanged;
            Grid.SetColumn(_durationSpin, 1);
            durationRow.Children.Add(_durationSpin);
            Grid.SetRow(durationRow, 1);
            root.Children.Add(durationRow);

            _keyframeList = new ListBox { MinHeight = 180 };
            _keyframeList.SelectionChanged += OnKeyframeSelectionChanged;
            Grid.SetRow(_keyframeList, 2);
            root.Children.Add(_keyframeList);

            var editRow = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("Auto,Auto,*,Auto,Auto,Auto"),
                Margin = new Avalonia.Thickness(0, 8, 0, 0),
            };
            editRow.Children.Add(new TextBlock
            {
                Text = "Time (s):",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(0, 0, 8, 0),
            });
            _timeSpin = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 999.999m,
                Increment = 0.001m,
                FormatString = "F3",
                Width = 100,
            };
            Grid.SetColumn(_timeSpin, 1);
            editRow.Children.Add(_timeSpin);

            editRow.Children.Add(new TextBlock
            {
                Text = "Shape:",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(12, 0, 8, 0),
            });
            Grid.SetColumn(editRow.Children[editRow.Children.Count - 1], 2);

            _shapeCombo = new ComboBox { MinWidth = 100, VerticalAlignment = VerticalAlignment.Center };
            foreach (LIPShape shape in Enum.GetValues(typeof(LIPShape)))
            {
                _shapeCombo.Items.Add(shape);
            }

            if (_shapeCombo.Items.Count > 0)
            {
                _shapeCombo.SelectedIndex = 0;
            }

            Grid.SetColumn(_shapeCombo, 3);
            editRow.Children.Add(_shapeCombo);

            _addKeyframeButton = new Button { Content = "Add", Margin = new Avalonia.Thickness(8, 0, 0, 0) };
            _addKeyframeButton.Click += (s, e) => OnAddKeyframeClick();
            Grid.SetColumn(_addKeyframeButton, 4);
            editRow.Children.Add(_addKeyframeButton);

            _updateKeyframeButton = new Button { Content = "Update", Margin = new Avalonia.Thickness(4, 0, 0, 0) };
            _updateKeyframeButton.Click += (s, e) => OnUpdateKeyframeClick();
            Grid.SetColumn(_updateKeyframeButton, 5);
            editRow.Children.Add(_updateKeyframeButton);

            var buttonRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Spacing = 6,
                Margin = new Avalonia.Thickness(0, 8, 0, 0),
            };
            _deleteKeyframeButton = new Button { Content = "Delete Selected" };
            _deleteKeyframeButton.Click += (s, e) => OnDeleteKeyframeClick();
            buttonRow.Children.Add(_deleteKeyframeButton);

            var editPanel = new StackPanel();
            editPanel.Children.Add(editRow);
            editPanel.Children.Add(buttonRow);
            Grid.SetRow(editPanel, 3);
            root.Children.Add(editPanel);

            return root;
        }

        /// <summary>
        /// Loads a WAV file for preview playback and sets duration from audio length (Holocron load_audio).
        /// </summary>
        public void LoadAudioFile(string wavPath)
        {
            if (string.IsNullOrWhiteSpace(wavPath) || !File.Exists(wavPath))
            {
                throw new FileNotFoundException("Audio file not found.", wavPath);
            }

            _previewPlayer?.Stop();

            float duration = LipBatchProcessor.GetWavDurationSeconds(wavPath);
            _audioFilePath = wavPath;
            if (_audioPathBox != null)
            {
                _audioPathBox.Text = wavPath;
            }

            _undoStack.Clear();
            _redoStack.Clear();
            _duration = duration;
            if (_lip != null)
            {
                _lip.Length = duration;
            }

            if (_previewPlayer != null)
            {
                _previewPlayer.SetSource(wavPath);
            }

            RefreshKeyframeList();
        }

        private async Task LoadAudioFromPickerAsync()
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel?.StorageProvider == null)
            {
                return;
            }

            try
            {
                var options = new FilePickerOpenOptions
                {
                    Title = "Select Audio File",
                    AllowMultiple = false,
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType("Audio Files") { Patterns = new[] { "*.wav" } },
                    },
                };

                var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
                if (files == null || files.Count == 0)
                {
                    return;
                }

                string path = files[0].Path.LocalPath;
                LoadAudioFile(path);
            }
            catch (Exception ex)
            {
                await DialogHelper.ShowWindowAsync(this, "Error", ex.Message, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            }
        }

        private async void PlayPreview()
        {
            if (string.IsNullOrWhiteSpace(_audioFilePath))
            {
                await DialogHelper.ShowWindowAsync(
                    this,
                    "Error",
                    "Please load an audio file first",
                    MsBox.Avalonia.Enums.ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Warning);
                return;
            }

            if (_previewPlayer != null)
            {
                _previewPlayer.Play();
            }
        }

        private void StopPreview()
        {
            _previewPlayer?.Stop();
        }

        private static string FormatKeyframeLabel(LIPKeyFrame frame)
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:F3}s — {1}", frame.Time, frame.Shape);
        }

        private void RefreshKeyframeList()
        {
            if (_keyframeList == null)
            {
                return;
            }

            _uiSyncInProgress = true;
            try
            {
                int selectedIndex = _keyframeList.SelectedIndex;
                _keyframeList.Items.Clear();
                if (_lip != null && _lip.Frames != null)
                {
                    foreach (LIPKeyFrame frame in _lip.Frames)
                    {
                        _keyframeList.Items.Add(FormatKeyframeLabel(frame));
                    }
                }

                if (selectedIndex >= 0 && selectedIndex < _keyframeList.Items.Count)
                {
                    _keyframeList.SelectedIndex = selectedIndex;
                }

                if (_durationSpin != null)
                {
                    _durationSpin.Value = (decimal)_duration;
                }

                if (_timeSpin != null)
                {
                    _timeSpin.Maximum = _duration > 0f ? (decimal)_duration : 999.999m;
                }

                UpdateStatusBar();
            }
            finally
            {
                _uiSyncInProgress = false;
            }
        }

        private void OnDurationSpinChanged(object sender, NumericUpDownValueChangedEventArgs e)
        {
            if (_uiSyncInProgress || _durationSpin == null)
            {
                return;
            }

            float newDuration = (float)(_durationSpin.Value ?? 0m);
            if (Math.Abs(_duration - newDuration) < 1e-6f)
            {
                return;
            }

            Duration = newDuration;
            RefreshKeyframeList();
        }

        private void OnKeyframeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_uiSyncInProgress || _keyframeList == null || _lip == null)
            {
                return;
            }

            int index = _keyframeList.SelectedIndex;
            if (index < 0 || index >= _lip.Frames.Count)
            {
                return;
            }

            LIPKeyFrame frame = _lip.Frames[index];
            _uiSyncInProgress = true;
            try
            {
                if (_timeSpin != null)
                {
                    _timeSpin.Value = (decimal)frame.Time;
                }

                if (_shapeCombo != null)
                {
                    _shapeCombo.SelectedItem = frame.Shape;
                }
            }
            finally
            {
                _uiSyncInProgress = false;
            }
        }

        private LIPShape GetSelectedShape()
        {
            if (_shapeCombo?.SelectedItem is LIPShape shape)
            {
                return shape;
            }

            return LIPShape.MPB;
        }

        private float GetTimeSpinValue()
        {
            return _timeSpin != null ? (float)(_timeSpin.Value ?? 0m) : 0f;
        }

        private void OnAddKeyframeClick()
        {
            AddKeyframe(GetTimeSpinValue(), GetSelectedShape());
            RefreshKeyframeList();
        }

        private void OnUpdateKeyframeClick()
        {
            if (_keyframeList == null || _lip == null)
            {
                return;
            }

            int index = _keyframeList.SelectedIndex;
            if (index < 0 || index >= _lip.Frames.Count)
            {
                return;
            }

            UpdateKeyframe(index, GetTimeSpinValue(), GetSelectedShape());
            RefreshKeyframeList();
            if (index < _keyframeList.Items.Count)
            {
                _keyframeList.SelectedIndex = index;
            }
        }

        private void OnDeleteKeyframeClick()
        {
            if (_keyframeList == null || _lip == null)
            {
                return;
            }

            int index = _keyframeList.SelectedIndex;
            if (index < 0 || index >= _lip.Frames.Count)
            {
                return;
            }

            DeleteKeyframe(index);
            RefreshKeyframeList();
        }

        private void SetupSignals()
        {
            Opened += (s, e) => { UpdateStatusBar(); Focus(); };
            KeyDown += OnWindowKeyDown;
            Closed += (s, e) =>
            {
                if (_previewPlayer != null)
                {
                    _previewPlayer.Dispose();
                    _previewPlayer = null;
                }
            };
        }

        private void SetupMenuHandlers()
        {
            // actionNew, actionOpen, actionSave, actionSaveAs, actionRevert, actionExit wired by base Editor
            EditorHelpers.BindMenuClicks(this, new (string menuItemName, Action handler)[]
            {
                ("actionUndo", Undo),
                ("actionRedo", Redo),
                ("actionFind", ShowFindDialog),
                ("actionFindNext", FindNextMatch),
                ("actionBatchProcessWavToLip", ShowBatchProcessorDialog),
            });
        }

        private async void ShowBatchProcessorDialog()
        {
            var dialog = new LipBatchProcessorDialog(this);
            await dialog.ShowDialog(this);
        }

        private void PushState()
        {
            if (_undoRedoInProgress) return;
            try
            {
                var (data, _) = Build();
                if (data == null || data.Length == 0) return;
                _undoStack.Add(data);
                if (_undoStack.Count > UndoMaxLevels) _undoStack.RemoveAt(0);
                _redoStack.Clear();
                MarkDocumentDirty();
            }
            catch { }
        }

        private void Undo()
        {
            if (_undoStack.Count == 0) return;
            _undoRedoInProgress = true;
            try
            {
                byte[] data = _undoStack[_undoStack.Count - 1];
                _undoStack.RemoveAt(_undoStack.Count - 1);
                _redoStack.Add(Build().Item1);
                LoadFromBytes(data);
                UpdateStatusBar();
            }
            finally { _undoRedoInProgress = false; }
        }

        private void Redo()
        {
            if (_redoStack.Count == 0) return;
            _undoRedoInProgress = true;
            try
            {
                byte[] data = _redoStack[_redoStack.Count - 1];
                _redoStack.RemoveAt(_redoStack.Count - 1);
                _undoStack.Add(Build().Item1);
                LoadFromBytes(data);
                UpdateStatusBar();
            }
            finally { _undoRedoInProgress = false; }
        }

        private void LoadFromBytes(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                _lip = new LIP();
                _duration = 0.0f;
            }
            else
            {
                try
                {
                    _lip = LIPAuto.ReadLip(data);
                    _duration = _lip != null ? _lip.Length : 0.0f;
                    if (_lip == null) _lip = new LIP();
                }
                catch
                {
                    _lip = new LIP();
                    _duration = 0.0f;
                }
            }
            _lastFindIndex = -1;
            _undoRedoInProgress = true;
            try
            {
                LoadLIP(_lip);
                UpdateStatusBar();
            }
            finally { _undoRedoInProgress = false; }
        }

        public override void Revert()
        {
            if (_revert == null || _revert.Length == 0) return;
            try
            {
                _undoStack.Clear();
                _redoStack.Clear();
                LoadFromBytes(_revert);
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Revert failed: {ex}");
            }
        }

        protected override async Task RunSaveAsAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            string suggestedName = string.IsNullOrEmpty(_resname) ? "lip" : _resname;
            var options = new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggestedName + ".lip",
                FileTypeChoices = new[] { new FilePickerFileType("LIP") { Patterns = new[] { "*.lip" } } }
            };
            var file = await storageProvider.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path.LocalPath;
            if (string.IsNullOrWhiteSpace(path)) return;
            _filepath = path;
            RefreshWindowTitle();
            Save();
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            try
            {
                int frames = _lip?.Frames?.Count ?? 0;
                string text = _lip == null ? "LIP" : $"{frames} keyframe(s), duration {_duration:F2}s";
                var c = _statusText ?? EditorHelpers.FindControlSafe<Avalonia.Controls.TextBlock>(this, "statusText");
                if (c != null) c.Text = text;
            }
            catch { }
        }

        private void ShowFindDialog()
        {
            var dialog = new Window
            {
                Title = "Find",
                Width = 400,
                Height = 180,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var findBox = new TextBox { Watermark = "Find what:", Text = _findText, Margin = new Avalonia.Thickness(8) };
            var matchCase = new CheckBox { Content = "Match case", IsChecked = _findMatchCase, Margin = new Avalonia.Thickness(8) };
            var findNext = new Button { Content = "Find Next", Margin = new Avalonia.Thickness(8) };
            var closeBtn = new Button { Content = "Close", Margin = new Avalonia.Thickness(8) };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(10) };
            panel.Children.Add(findBox);
            panel.Children.Add(matchCase);
            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal };
            btnPanel.Children.Add(findNext);
            btnPanel.Children.Add(closeBtn);
            panel.Children.Add(btnPanel);
            dialog.Content = panel;
            findNext.Click += (s, e) => { _findText = findBox.Text ?? ""; _findMatchCase = matchCase.IsChecked == true; FindNextMatch(); };
            closeBtn.Click += (s, e) => dialog.Close();
            dialog.Opened += (s, e) => findBox.Focus();
            dialog.ShowDialog(this);
        }

        private List<string> GetSearchableStrings()
        {
            var list = new List<string>();
            if (_lip == null) return list;
            list.Add(_duration.ToString(System.Globalization.CultureInfo.InvariantCulture));
            list.Add(_lip.Length.ToString(System.Globalization.CultureInfo.InvariantCulture));
            if (_lip.Frames != null)
                foreach (var frame in _lip.Frames)
                {
                    list.Add(frame.Time.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    list.Add(frame.Shape.ToString());
                }
            return list;
        }

        private void FindNextMatch()
        {
            if (string.IsNullOrEmpty(_findText)) return;
            var strings = GetSearchableStrings();
            if (strings.Count == 0) return;
            string t = _findMatchCase ? _findText : _findText.ToLowerInvariant();
            bool Match(string value) => value != null && (_findMatchCase ? value : value.ToLowerInvariant()).Contains(t);
            int start = _lastFindIndex + 1;
            for (int i = 0; i < strings.Count; i++)
            {
                int idx = (start + i) % strings.Count;
                if (Match(strings[idx])) { _lastFindIndex = idx; UpdateStatusBar(); return; }
            }
            _lastFindIndex = -1;
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && (e.KeyModifiers & KeyModifiers.Control) != 0) { Save(); e.Handled = true; return; }
            if (e.Key == Key.Z && (e.KeyModifiers & KeyModifiers.Control) != 0) { Undo(); e.Handled = true; return; }
            if (e.Key == Key.Y && (e.KeyModifiers & KeyModifiers.Control) != 0) { Redo(); e.Handled = true; return; }
            if (e.Key == Key.F && (e.KeyModifiers & KeyModifiers.Control) != 0) { ShowFindDialog(); e.Handled = true; return; }
            if (e.Key == Key.F3) { FindNextMatch(); e.Handled = true; }
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _undoStack.Clear();
            _redoStack.Clear();
            try { LoadFromBytes(data); }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to load LIP: {ex}");
                New();
            }
        }

        /// <summary>
        /// Loads LIP into editor state and refreshes keyframe UI.
        /// </summary>
        private void LoadLIP(LIP lip)
        {
            _lip = lip ?? _lip;
            if (_lip != null)
            {
                _duration = _lip.Length;
            }

            RefreshKeyframeList();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            if (_lip == null)
            {
                _lip = new LIP();
            }
            // Ensure LIP length matches duration (matching Python behavior)
            _lip.Length = _duration;
            ResourceType lipType = _restype ?? ResourceType.LIP;
            byte[] data = LIPAuto.BytesLip(_lip, lipType);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _undoStack.Clear();
            _redoStack.Clear();
            _lip = new LIP();
            _duration = 0.0f;
            _lastFindIndex = -1;
            RefreshKeyframeList();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        // Properties for tests
        public LIP Lip => _lip;
        public string AudioFilePath => _audioFilePath;
        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;
        public float Duration
        {
            get => _duration;
            set
            {
                if (Math.Abs(_duration - value) < 1e-6f) return;
                PushState();
                _duration = value;
                if (_lip != null)
                    _lip.Length = value;
            }
        }

        // Helper method for tests to add keyframes
        public void AddKeyframe(float time, LIPShape shape)
        {
            PushState();
            if (_lip == null)
            {
                _lip = new LIP();
                _lip.Length = _duration;
            }
            _lip.Add(time, shape);
            RefreshKeyframeList();
            // Note: In Python, lip.length is set to duration when creating, not based on max keyframe time
            // The duration property is separate from the max keyframe time
        }

        // Helper method for tests to update keyframes
        public void UpdateKeyframe(int index, float time, LIPShape shape)
        {
            if (_lip == null || _lip.Frames.Count == 0)
                return;
            if (index >= 0 && index < _lip.Frames.Count)
            {
                PushState();
                _lip.Remove(index);
                _lip.Add(time, shape);
                RefreshKeyframeList();
            }
        }

        // Helper method for tests to delete keyframes
        public void DeleteKeyframe(int index)
        {
            if (_lip == null || _lip.Frames.Count == 0)
                return;
            if (index >= 0 && index < _lip.Frames.Count)
            {
                PushState();
                _lip.Remove(index);
                RefreshKeyframeList();
            }
        }
    }
}
