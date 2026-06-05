using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using BioWare.Resource.Formats.LIP;
using BioWare.Resource.Formats.TwoDA;
using BioWare.Common;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Utils;
using OdyTools.Widgets;
using OdyTools.Widgets.Edit;

namespace OdyTools.Editors
{
    public partial class OdyToolLIP : Editor
    {
        private const int MinEditorWidth = 820;
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
        private TextBlock _previewLabel;
        private DispatcherTimer _previewTimer;
        private bool _playbackSyncInProgress;
        private ModelRenderer _headPreviewRenderer;
        private ComboBox2DA _appearanceSelect;
        private TextBlock _headPreviewNotice;
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
            _previewPlayer.PlaybackStopped += OnPreviewPlaybackStopped;
            _previewTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16),
            };
            _previewTimer.Tick += OnPreviewTimerTick;
            InitializeComponent();
            SetupSignals();
            SetupMenuHandlers();
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
            New();
            SetupHeadPreview();
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
                ColumnDefinitions = new ColumnDefinitions("320,*"),
            };

            var headPanel = BuildHeadPreviewPanel();
            Grid.SetColumn(headPanel, 0);
            root.Children.Add(headPanel);

            var editorPanel = BuildLipEditorPanel();
            Grid.SetColumn(editorPanel, 1);
            root.Children.Add(editorPanel);

            return root;
        }

        private Control BuildHeadPreviewPanel()
        {
            var panel = new StackPanel
            {
                Margin = new Avalonia.Thickness(0, 0, 8, 0),
                Spacing = 6,
            };

            panel.Children.Add(new TextBlock
            {
                Text = "Head preview",
                FontWeight = Avalonia.Media.FontWeight.SemiBold,
            });

            _appearanceSelect = new ComboBox2DA { MinWidth = 200 };
            _appearanceSelect.SelectionChanged += (s, e) => RefreshHeadPreview();
            panel.Children.Add(_appearanceSelect);

            _headPreviewRenderer = new ModelRenderer
            {
                MinHeight = 220,
                Height = 260,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };
            if (_installation != null)
            {
                _headPreviewRenderer.Installation = _installation;
            }

            panel.Children.Add(_headPreviewRenderer);

            _headPreviewNotice = new TextBlock
            {
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                IsVisible = false,
            };
            panel.Children.Add(_headPreviewNotice);

            return panel;
        }

        private Control BuildLipEditorPanel()
        {
            var root = new Grid
            {
                RowDefinitions = new RowDefinitions("Auto,Auto,Auto,*,Auto"),
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

            var previewRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Avalonia.Thickness(0, 0, 0, 8),
            };
            previewRow.Children.Add(new TextBlock
            {
                Text = "Preview:",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Avalonia.Thickness(0, 0, 8, 0),
            });
            _previewLabel = new TextBlock
            {
                Text = "None",
                VerticalAlignment = VerticalAlignment.Center,
            };
            previewRow.Children.Add(_previewLabel);
            Grid.SetRow(previewRow, 1);
            root.Children.Add(previewRow);

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
            Grid.SetRow(durationRow, 2);
            root.Children.Add(durationRow);

            _keyframeList = new ListBox { MinHeight = 180 };
            _keyframeList.SelectionChanged += OnKeyframeSelectionChanged;
            Grid.SetRow(_keyframeList, 3);
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
            Grid.SetRow(editPanel, 4);
            root.Children.Add(editPanel);

            return root;
        }

        private void SetupHeadPreview()
        {
            if (_headPreviewNotice == null)
            {
                return;
            }

            if (_installation == null)
            {
                _headPreviewNotice.Text = LipHeadPreviewHelper.NoInstallationMessage;
                _headPreviewNotice.IsVisible = true;
                if (_appearanceSelect != null)
                {
                    _appearanceSelect.IsEnabled = false;
                }
                _headPreviewRenderer?.ClearModel();
                return;
            }

            _headPreviewNotice.IsVisible = false;
            if (_appearanceSelect != null)
            {
                _appearanceSelect.IsEnabled = true;
            }
            if (_headPreviewRenderer != null)
            {
                _headPreviewRenderer.Installation = _installation;
            }

            PopulateAppearanceCombo();
            RefreshHeadPreview();
        }

        private void PopulateAppearanceCombo()
        {
            if (_appearanceSelect == null || _installation == null)
            {
                return;
            }

            _installation.HtBatchCache2DA(new List<string> { OdyInstallation.TwoDAAppearances });
            TwoDA appearances = _installation.HtGetCache2DA(OdyInstallation.TwoDAAppearances);
            _appearanceSelect.Items.Clear();
            if (appearances == null)
            {
                return;
            }

            _appearanceSelect.SetContext(appearances, _installation, OdyInstallation.TwoDAAppearances);
            List<string> labels = appearances.GetColumn("label");
            _appearanceSelect.SetItems(labels, sortAlphabetically: true);
            if (_appearanceSelect.Items.Count > 0)
            {
                _appearanceSelect.SetSelectedIndex(0);
            }
        }

        private void RefreshHeadPreview()
        {
            if (_headPreviewRenderer == null)
            {
                return;
            }

            if (_installation == null)
            {
                _headPreviewRenderer.ClearModel();
                return;
            }

            int appearanceId = _appearanceSelect != null ? _appearanceSelect.SelectedIndex : 0;
            byte[] mdlData;
            byte[] mdxData;
            if (!LipHeadPreviewHelper.TryLoadHeadModel(_installation, appearanceId, out mdlData, out mdxData, out _))
            {
                _headPreviewRenderer.ClearModel();
                return;
            }

            _headPreviewRenderer.SetModel(mdlData, mdxData);
        }

        private void UpdateHeadPlaybackHint(LIPShape? shape)
        {
            if (_headPreviewRenderer == null)
            {
                return;
            }

            _headPreviewRenderer.SetPlaybackHint(shape.HasValue ? shape.Value.ToString() : string.Empty);
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

            StopPreview();

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

            StartPreviewTimer();
        }

        private void StopPreview()
        {
            _previewPlayer?.Stop();
            StopPreviewTimer();
            ResetPreviewDisplay();
        }

        private void StartPreviewTimer()
        {
            if (_previewTimer != null && !_previewTimer.IsEnabled)
            {
                _previewTimer.Start();
            }
        }

        private void StopPreviewTimer()
        {
            if (_previewTimer != null && _previewTimer.IsEnabled)
            {
                _previewTimer.Stop();
            }
        }

        private void OnPreviewPlaybackStopped(object sender, EventArgs e)
        {
            StopPreviewTimer();
            ResetPreviewDisplay();
        }

        private void OnPreviewTimerTick(object sender, EventArgs e)
        {
            UpdatePlaybackSync();
        }

        private void UpdatePlaybackSync()
        {
            if (_previewPlayer == null || _lip == null)
            {
                return;
            }

            float currentTime = (float)_previewPlayer.Position.TotalSeconds;
            LIPShape? shape = GetShapeAtPlaybackTime(_lip, currentTime);
            if (_previewLabel != null)
            {
                _previewLabel.Text = shape.HasValue ? shape.Value.ToString() : "None";
            }

            UpdateHeadPlaybackHint(shape);

            int keyframeIndex = GetKeyframeIndexAtTime(_lip, currentTime);
            if (_keyframeList == null)
            {
                return;
            }

            if (keyframeIndex >= 0 && keyframeIndex < _keyframeList.Items.Count)
            {
                if (_keyframeList.SelectedIndex != keyframeIndex)
                {
                    _playbackSyncInProgress = true;
                    try
                    {
                        _keyframeList.SelectedIndex = keyframeIndex;
                    }
                    finally
                    {
                        _playbackSyncInProgress = false;
                    }
                }
            }
            else if (_keyframeList.SelectedIndex >= 0)
            {
                _playbackSyncInProgress = true;
                try
                {
                    _keyframeList.SelectedIndex = -1;
                }
                finally
                {
                    _playbackSyncInProgress = false;
                }
            }
        }

        private void ResetPreviewDisplay()
        {
            if (_previewLabel != null)
            {
                _previewLabel.Text = "None";
            }

            UpdateHeadPlaybackHint(null);
        }

        /// <summary>
        /// Holocron parity: last keyframe at or before playback time.
        /// </summary>
        public static int GetKeyframeIndexAtTime(LIP lip, float time)
        {
            if (lip == null || lip.Frames == null || lip.Frames.Count == 0)
            {
                return -1;
            }

            int bestIndex = -1;
            float bestTime = float.MinValue;
            for (int i = 0; i < lip.Frames.Count; i++)
            {
                LIPKeyFrame frame = lip.Frames[i];
                if (frame.Time <= time && frame.Time >= bestTime)
                {
                    bestTime = frame.Time;
                    bestIndex = i;
                }
            }

            return bestIndex;
        }

        /// <summary>
        /// Holocron parity: discrete shape at playback time (not engine interpolation).
        /// </summary>
        public static LIPShape? GetShapeAtPlaybackTime(LIP lip, float time)
        {
            int index = GetKeyframeIndexAtTime(lip, time);
            if (index < 0)
            {
                return null;
            }

            return lip.Frames[index].Shape;
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
            if (_uiSyncInProgress || _playbackSyncInProgress || _keyframeList == null || _lip == null)
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
                StopPreviewTimer();
                if (_previewPlayer != null)
                {
                    _previewPlayer.PlaybackStopped -= OnPreviewPlaybackStopped;
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
            if (e.Key == Key.F3) { FindNextMatch(); e.Handled = true; return; }
            if (e.Key == Key.Space) { PlayPreview(); e.Handled = true; return; }
            if (e.Key == Key.Escape) { StopPreview(); e.Handled = true; }
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
