using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using BioWare.Extract;
using BioWare.Resource.Formats.SSF;
using BioWare.Common;
using OdyTools.Data;
using OdyTools.Widgets;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace OdyTools.Editors
{
    public partial class OdyToolSSF : Editor
    {
        private const int MinEditorWidth = 720;
        private const int MinEditorHeight = 520;
        private const int UndoMaxLevels = 30;

        private static readonly (string DisplayName, SSFSound Sound, string Tooltip)[] SoundRows = new[]
        {
            ("Battle Cry 1", SSFSound.BATTLE_CRY_1, "Played when the character issues a battle cry (first variant)."),
            ("Battle Cry 2", SSFSound.BATTLE_CRY_2, "Played when the character issues a battle cry (second variant)."),
            ("Battle Cry 3", SSFSound.BATTLE_CRY_3, "Played when the character issues a battle cry (third variant)."),
            ("Battle Cry 4", SSFSound.BATTLE_CRY_4, "Played when the character issues a battle cry (fourth variant)."),
            ("Battle Cry 5", SSFSound.BATTLE_CRY_5, "Played when the character issues a battle cry (fifth variant)."),
            ("Battle Cry 6", SSFSound.BATTLE_CRY_6, "Played when the character issues a battle cry (sixth variant)."),
            ("Select 1", SSFSound.SELECT_1, "Played when the character is selected (first variant)."),
            ("Select 2", SSFSound.SELECT_2, "Played when the character is selected (second variant)."),
            ("Select 3", SSFSound.SELECT_3, "Played when the character is selected (third variant)."),
            ("Attack 1", SSFSound.ATTACK_GRUNT_1, "Played during an attack (first grunt variant)."),
            ("Attack 2", SSFSound.ATTACK_GRUNT_2, "Played during an attack (second grunt variant)."),
            ("Attack 3", SSFSound.ATTACK_GRUNT_3, "Played during an attack (third grunt variant)."),
            ("Pain 1", SSFSound.PAIN_GRUNT_1, "Played when the character takes damage (first variant)."),
            ("Pain 2", SSFSound.PAIN_GRUNT_2, "Played when the character takes damage (second variant)."),
            ("Low HP", SSFSound.LOW_HEALTH, "Played when the character's health drops below a threshold."),
            ("Dead", SSFSound.DEAD, "Played when the character dies."),
            ("Critical", SSFSound.CRITICAL_HIT, "Played when the character scores a critical hit."),
            ("Immune", SSFSound.TARGET_IMMUNE, "Played when the target is immune to the effect."),
            ("Lay Mine", SSFSound.LAY_MINE, "Played when the character lays a mine."),
            ("Disarm Mine", SSFSound.DISARM_MINE, "Played when the character disarms a mine."),
            ("Begin Stealth", SSFSound.BEGIN_STEALTH, "Played when the character enters stealth mode."),
            ("Begin Search", SSFSound.BEGIN_SEARCH, "Played when the character begins searching for traps or locks."),
            ("Begin Unlock", SSFSound.BEGIN_UNLOCK, "Played when the character attempts to unlock something."),
            ("Unlock Success", SSFSound.UNLOCK_SUCCESS, "Played when an unlock attempt succeeds."),
            ("Unlock Failed", SSFSound.UNLOCK_FAILED, "Played when an unlock attempt fails."),
            ("Party Separated", SSFSound.SEPARATED_FROM_PARTY, "Played when the character is separated from the party."),
            ("Rejoin Party", SSFSound.REJOINED_PARTY, "Played when the character rejoins the party."),
            ("Poisoned", SSFSound.POISONED, "Played when the character is poisoned or affected by a similar hazard."),
        };

        // Reva: Engine loads SSF (soundset) via FUN_006789a0 (swkotor.exe); see docs/module_resource_types_reverse_engineering.md. Toolset playback: same resolution as dialogue voice — strref → TLK GetSound (Voiceover ResRef) → installation.Sound with OVERRIDE, VOICE (StreamVoice/StreamWaves), SOUND, CHITIN. Matches OdyToolDLG Play Voice and OdyToolUTS Play.
        /// <summary>Canonical search order for TLK-linked voice/soundset playback (matches OdyToolDLG Play Voice and OdyToolUTS: Override, then StreamVoice/StreamWaves, StreamSounds, CHITIN).</summary>
        private static readonly SearchLocation[] VoiceSearchOrder = new[]
        {
            SearchLocation.OVERRIDE,
            SearchLocation.VOICE,
            SearchLocation.SOUND,
            SearchLocation.CHITIN
        };

        private Dictionary<SSFSound, NumericUpDown> _spinBySound;
        private Dictionary<SSFSound, (TextBox SoundEdit, TextBox TextEdit)> _soundTextPairs;
        private Dictionary<SSFSound, Border> _rowBorderBySound;
        private SSFSound? _selectedSound;
        private TalkTable _talktable;
        private TextBlock _talktableLabel;
        private TextBlock _statusText;
        private TextBlock _soundCountLabel;
        private StackPanel _soundRowsPanel;
        private Button _talktableButton;
        private Button _stopPlayButton;
        private TextBlock _previewCategoryLabel;
        private TextBlock _previewStrrefLabel;
        private TextBox _previewTextBlock;
        private TextBlock _previewResRefLabel;
        private Button _previewPlayButton;
        private TextBlock _previewPlayingLabel;
        private NAudioMediaPlayer _player;
        private string _tempPlayPath;
        private SSFSound? _playingSound;
        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private readonly List<byte[]> _redoStack = new List<byte[]>();
        private bool _undoRedoInProgress;
        private int _findStrref = -1;

        public OdyToolSSF(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolSSF", "soundset", new[] { ResourceType.SSF }, new[] { ResourceType.SSF }, installation)
        {
            _talktable = installation != null ? new TalkTable(Path.Combine(installation.Path, "dialog.tlk")) : null;
            _soundTextPairs = new Dictionary<SSFSound, (TextBox, TextBox)>();
            _spinBySound = new Dictionary<SSFSound, NumericUpDown>();
            _rowBorderBySound = new Dictionary<SSFSound, Border>();
            InitializeComponent();
            SetupUI();
            SetupSignals();
            SetupMenuHandlers();
            UpdateTalktableLabel();
            New();
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
            Closed += OnWindowClosed;
        }

        private void InitializeComponent()
        {
            bool xamlLoaded = false;
            try
            {
                AvaloniaXamlLoader.Load(this);
                xamlLoaded = true;
            }
            catch { }

            if (!xamlLoaded)
            {
                SetupProgrammaticUI();
            }
        }

        private void SetupProgrammaticUI()
        {
            var mainPanel = new ScrollViewer { Content = new StackPanel { Orientation = Orientation.Vertical } };
            var stackPanel = mainPanel.Content as StackPanel;
            if (stackPanel == null) return;

            var talktablePanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 10) };
            _talktableButton = new Button { Content = "Select Talk Table", Width = 150, Height = 30 };
            _talktableButton.Click += async (s, e) => await SelectTalkTable();
            talktablePanel.Children.Add(_talktableButton);
            _talktableLabel = new TextBlock { Text = _talktable != null ? $"Using: {Path.GetFileName(_talktable.Path)}" : "Using installation talktable", VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(10, 0, 0, 0) };
            talktablePanel.Children.Add(_talktableLabel);
            stackPanel.Children.Add(talktablePanel);

            _soundRowsPanel = stackPanel;
            PopulateSoundRows(_soundRowsPanel);

            var dock = new DockPanel();
            var menu = BuildMenu();
            dock.Children.Add(menu);
            DockPanel.SetDock(menu, Dock.Top);
            dock.Children.Add(mainPanel);
            _statusText = new TextBlock { Text = "27 sounds", Margin = new Thickness(4, 2) };
            dock.Children.Add(_statusText);
            DockPanel.SetDock(_statusText, Dock.Bottom);
            Content = dock;
        }

        private void SetupUI()
        {
            _talktableButton = this.FindControl<Button>("talktableButton");
            _talktableLabel = this.FindControl<TextBlock>("talktableLabel");
            _statusText = this.FindControl<TextBlock>("statusText");
            _soundCountLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "soundCountLabel");
            _soundRowsPanel = this.FindControl<StackPanel>("SoundRowsPanel");
            _stopPlayButton = EditorHelpers.FindControlSafe<Button>(this, "stopPlayButton");
            _previewCategoryLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "previewCategoryLabel");
            _previewStrrefLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "previewStrrefLabel");
            _previewTextBlock = EditorHelpers.FindControlSafe<TextBox>(this, "previewTextBlock");
            _previewResRefLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "previewResRefLabel");
            _previewPlayButton = EditorHelpers.FindControlSafe<Button>(this, "previewPlayButton");
            _previewPlayingLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "previewPlayingLabel");

            if (_talktableButton != null)
                _talktableButton.Click += async (s, e) => await SelectTalkTable();
            if (_stopPlayButton != null)
                _stopPlayButton.Click += (s, e) => StopPlayback();
            if (_previewPlayButton != null)
                _previewPlayButton.Click += (s, e) => { if (_selectedSound.HasValue) PlaySoundForEntry(_selectedSound.Value); };
            if (_soundRowsPanel != null)
                PopulateSoundRows(_soundRowsPanel);
            UpdatePreviewPanel();
        }

        private static readonly (string GroupName, SSFSound[] Sounds)[] SoundGroups = new[]
        {
            ("Battle Cries", new[] { SSFSound.BATTLE_CRY_1, SSFSound.BATTLE_CRY_2, SSFSound.BATTLE_CRY_3, SSFSound.BATTLE_CRY_4, SSFSound.BATTLE_CRY_5, SSFSound.BATTLE_CRY_6 }),
            ("Selection", new[] { SSFSound.SELECT_1, SSFSound.SELECT_2, SSFSound.SELECT_3 }),
            ("Combat", new[] { SSFSound.ATTACK_GRUNT_1, SSFSound.ATTACK_GRUNT_2, SSFSound.ATTACK_GRUNT_3, SSFSound.PAIN_GRUNT_1, SSFSound.PAIN_GRUNT_2, SSFSound.LOW_HEALTH, SSFSound.DEAD, SSFSound.CRITICAL_HIT, SSFSound.TARGET_IMMUNE }),
            ("Mines & Skills", new[] { SSFSound.LAY_MINE, SSFSound.DISARM_MINE, SSFSound.BEGIN_STEALTH, SSFSound.BEGIN_SEARCH, SSFSound.BEGIN_UNLOCK, SSFSound.UNLOCK_SUCCESS, SSFSound.UNLOCK_FAILED }),
            ("Party & Status", new[] { SSFSound.SEPARATED_FROM_PARTY, SSFSound.REJOINED_PARTY, SSFSound.POISONED }),
        };

        private void PopulateSoundRows(StackPanel panel)
        {
            if (panel == null) return;
            panel.Children.Clear();
            _spinBySound.Clear();
            _soundTextPairs.Clear();
            _rowBorderBySound.Clear();

            const string strrefTip = "TLK string reference. -1 = no sound. This value indexes into the talk table (dialog.tlk). The linked WAV in that entry is played in-game.";
            const string previewTip = "Dialogue or label text from the talk table for this string ref (read-only).";
            const string soundTip = "Sound ResRef from the talk table (WAV filename without extension) played in-game for this entry (read-only).";
            const string playTip = "Play the WAV linked in the talk table. Searches Override, StreamVoice/StreamWaves, StreamSounds, and CHITIN. Requires a game installation and dialog.tlk with a valid sound resref.";

            var soundToRow = new Dictionary<SSFSound, (string DisplayName, string Tooltip)>();
            foreach (var (displayName, sound, tooltip) in SoundRows)
                soundToRow[sound] = (displayName, tooltip);

            foreach (var (groupName, sounds) in SoundGroups)
            {
                var headerBorder = new Border { Padding = new Thickness(12, 8, 12, 6), BorderThickness = new Thickness(0, 0, 0, 1) };
                headerBorder.Classes.Add("sound-group-header");
                headerBorder.Child = new TextBlock { Text = groupName, FontWeight = Avalonia.Media.FontWeight.SemiBold, FontSize = 13 };
                ToolTip.SetTip(headerBorder, "Sound events in this category are triggered together in-game (e.g. combat, selection).");
                panel.Children.Add(headerBorder);

                foreach (var sound in sounds)
                {
                    if (!soundToRow.TryGetValue(sound, out var rowInfo)) continue;
                    var (displayName, tooltip) = rowInfo;

                    var spin = new NumericUpDown { Minimum = -1, Maximum = int.MaxValue, Width = 72, Margin = new Thickness(0, 0, 8, 0) };
                    var previewEdit = new TextBox { IsReadOnly = true, MinWidth = 140, Margin = new Thickness(0, 0, 8, 0) };
                    var soundEdit = new TextBox { IsReadOnly = true, MinWidth = 100, Margin = new Thickness(0, 0, 8, 0) };
                    var playBtn = new Button { Content = "▶ Play", MinWidth = 44, Tag = sound };
                    playBtn.Classes.Add("play-sound");

                    ToolTip.SetTip(spin, strrefTip);
                    ToolTip.SetTip(previewEdit, previewTip);
                    ToolTip.SetTip(soundEdit, soundTip);
                    ToolTip.SetTip(playBtn, playTip);

                    var label = new TextBlock { Text = displayName, Width = 130, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(0, 0, 8, 0) };
                    ToolTip.SetTip(label, tooltip);

                    _spinBySound[sound] = spin;
                    _soundTextPairs[sound] = (soundEdit, previewEdit);

                    var row = new Border { Padding = new Thickness(12, 8) };
                    row.Classes.Add("sound-row");
                    _rowBorderBySound[sound] = row;
                    var grid = new Grid { ColumnDefinitions = new ColumnDefinitions("130,80,*,110,Auto") };
                    grid.Children.Add(label); Grid.SetColumn(label, 0);
                    grid.Children.Add(spin); Grid.SetColumn(spin, 1);
                    grid.Children.Add(previewEdit); Grid.SetColumn(previewEdit, 2);
                    grid.Children.Add(soundEdit); Grid.SetColumn(soundEdit, 3);
                    grid.Children.Add(playBtn); Grid.SetColumn(playBtn, 4);
                    row.Child = grid;
                    panel.Children.Add(row);

                    spin.ValueChanged += (s, e) => UpdateTextBoxes();
                    spin.LostFocus += (s, e) => OnSpinCommitted();
                    spin.GotFocus += (s, e) => SelectRow(sound);
                    previewEdit.GotFocus += (s, e) => SelectRow(sound);
                    soundEdit.GotFocus += (s, e) => SelectRow(sound);
                    playBtn.Click += (s, e) => PlaySoundForEntry(sound);
                    row.PointerPressed += (s, e) => SelectRow(sound);
                }
            }

            UpdateSelectionVisuals();
        }

        private void SelectRow(SSFSound sound)
        {
            _selectedSound = sound;
            UpdateSelectionVisuals();
            UpdatePreviewPanel();
        }

        private void UpdateSelectionVisuals()
        {
            foreach (var kv in _rowBorderBySound)
            {
                var border = kv.Value;
                if (border == null) continue;
                if (kv.Key == _selectedSound)
                    border.Classes.Add("selected");
                else
                    border.Classes.Remove("selected");
            }
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
            editMenu.Items.Add(new MenuItem { Header = "Find _Strref...", Name = "actionFindStrref" });
            editMenu.Items.Add(new MenuItem { Header = "Find Strref _Next", Name = "actionFindStrrefNext" });
            menu.Items.Add(editMenu);
            return menu;
        }

        private void SetupSignals()
        {
            Opened += (s, e) => { UpdateStatusBar(); if (_spinBySound?.Count > 0) FocusFirstSpin(); }; 
            KeyDown += OnWindowKeyDown;
        }

        private void FocusFirstSpin()
        {
            if (_spinBySound != null && _spinBySound.TryGetValue(SSFSound.BATTLE_CRY_1, out var first))
                first.Focus();
        }

        private void SetupMenuHandlers()
        {
            void Bind(string name, Action handler)
            {
                var item = EditorHelpers.FindControlSafe<MenuItem>(this, name) ?? this.FindControl<MenuItem>(name);
                if (item != null) item.Click += (s, e) => handler();
            }
            // actionNew, actionOpen, actionSave, actionSaveAs, actionRevert, actionExit wired by base Editor
            Bind("actionUndo", () => Undo());
            Bind("actionRedo", () => Redo());
            Bind("actionFindStrref", () => ShowFindStrrefDialog());
            Bind("actionFindStrrefNext", () => FindStrrefNext());
        }

        private void OnSpinCommitted()
        {
            if (_undoRedoInProgress) return;
            PushState();
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
            }
            catch { }
        }

        private void Undo()
        {
            if (_undoStack.Count == 0) return;
            _undoRedoInProgress = true;
            try
            {
                var data = _undoStack[_undoStack.Count - 1];
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
                var data = _redoStack[_redoStack.Count - 1];
                _redoStack.RemoveAt(_redoStack.Count - 1);
                _undoStack.Add(Build().Item1);
                LoadFromBytes(data);
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
            catch (Exception ex) { Console.WriteLine($"Revert failed: {ex}"); }
        }

        protected override async Task RunSaveAsAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            string suggestedName = string.IsNullOrEmpty(_resname) ? "soundset" : _resname;
            var options = new FilePickerSaveOptions { Title = "Save As", SuggestedFileName = suggestedName + ".ssf", FileTypeChoices = new[] { new FilePickerFileType("SSF") { Patterns = new[] { "*.ssf" } } } };
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
                string text = "27 sounds";
                if (_talktable != null) text += " | " + Path.GetFileName(_talktable.Path);
                if (_statusText != null) _statusText.Text = text;
                if (_soundCountLabel != null) _soundCountLabel.Text = "27 sounds";
            }
            catch { }
        }

        private void UpdatePreviewPanel()
        {
            if (!_selectedSound.HasValue)
            {
                if (_previewCategoryLabel != null) _previewCategoryLabel.Text = "Category: —";
                if (_previewStrrefLabel != null) _previewStrrefLabel.Text = "Strref: —";
                if (_previewTextBlock != null) _previewTextBlock.Text = "";
                if (_previewResRefLabel != null) _previewResRefLabel.Text = "Sound ResRef: —";
                if (_previewPlayButton != null) _previewPlayButton.IsEnabled = false;
                return;
            }
            var sound = _selectedSound.Value;
            string displayName = null;
            foreach (var (name, s, _) in SoundRows)
                if (s == sound) { displayName = name; break; }
            if (_previewCategoryLabel != null) _previewCategoryLabel.Text = "Category: " + (displayName ?? sound.ToString());
            if (!_spinBySound.TryGetValue(sound, out var spin) || spin == null)
                return;
            int strref = (int)(spin.Value ?? -1);
            if (_previewStrrefLabel != null) _previewStrrefLabel.Text = "Strref: " + strref;
            string previewText = "";
            string resrefText = "";
            if (_soundTextPairs.TryGetValue(sound, out var pair))
            {
                previewText = pair.TextEdit?.Text ?? "";
                resrefText = pair.SoundEdit?.Text ?? "";
            }
            if (_previewTextBlock != null) _previewTextBlock.Text = previewText;
            if (_previewResRefLabel != null) _previewResRefLabel.Text = string.IsNullOrEmpty(resrefText) ? "Sound ResRef: —" : "Sound ResRef: " + resrefText;
            if (_previewPlayButton != null) _previewPlayButton.IsEnabled = strref >= 0 && _talktable != null && _installation != null;
        }

        private void StopPlayback()
        {
            _player?.Stop();
            SetPlayingState(null);
        }

        private void SetPlayingState(SSFSound? sound)
        {
            _playingSound = sound;
            if (_stopPlayButton != null)
                _stopPlayButton.IsVisible = sound.HasValue;
            if (_previewPlayingLabel != null)
            {
                _previewPlayingLabel.IsVisible = sound.HasValue;
                if (sound.HasValue)
                {
                    string name = null;
                    foreach (var (n, s, _) in SoundRows)
                        if (s == sound) { name = n; break; }
                    _previewPlayingLabel.Text = "Playing: " + (name ?? sound.ToString());
                }
            }
        }

        private void ShowFindStrrefDialog()
        {
            var dialog = new Window { Title = "Find Strref", Width = 320, Height = 120, WindowStartupLocation = WindowStartupLocation.CenterOwner };
            var spin = new NumericUpDown { Minimum = -1, Maximum = int.MaxValue, Value = _findStrref >= 0 ? _findStrref : 0, Margin = new Thickness(8) };
            var findBtn = new Button { Content = "Find Next", Margin = new Thickness(8) };
            var closeBtn = new Button { Content = "Close", Margin = new Thickness(8) };
            var panel = new StackPanel { Margin = new Thickness(10) };
            panel.Children.Add(new TextBlock { Text = "Strref:" });
            panel.Children.Add(spin);
            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal };
            btnPanel.Children.Add(findBtn);
            btnPanel.Children.Add(closeBtn);
            panel.Children.Add(btnPanel);
            dialog.Content = panel;
            findBtn.Click += (s, e) => { _findStrref = (int)(spin.Value ?? 0); FindStrrefNext(); };
            closeBtn.Click += (s, e) => dialog.Close();
            dialog.Opened += (s, e) => spin.Focus();
            dialog.ShowDialog(this);
        }

        private void FindStrrefNext()
        {
            int target = _findStrref;
            foreach (var kv in _spinBySound)
                if (kv.Value != null && (int)(kv.Value.Value ?? -1) == target)
                {
                    kv.Value.Focus();
                    return;
                }
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && (e.KeyModifiers & KeyModifiers.Control) != 0) { Save(); e.Handled = true; return; }
            if (e.Key == Key.Z && (e.KeyModifiers & KeyModifiers.Control) != 0) { Undo(); e.Handled = true; return; }
            if (e.Key == Key.Y && (e.KeyModifiers & KeyModifiers.Control) != 0) { Redo(); e.Handled = true; return; }
            if (e.Key == Key.F && (e.KeyModifiers & KeyModifiers.Control) != 0) { ShowFindStrrefDialog(); e.Handled = true; return; }
            if (e.Key == Key.F3) { FindStrrefNext(); e.Handled = true; }
        }

        /// <summary>Plays the WAV for the given soundset entry: strref → TalkTable.GetSound → ResRef → installation.Sound(VoiceSearchOrder). Uses temp file + NAudioMediaPlayer (file-based API); cleanup on PlaybackStopped and OnClosed.</summary>
        private void PlaySoundForEntry(SSFSound sound)
        {
            if (!_spinBySound.TryGetValue(sound, out var spin) || spin == null) return;
            int strref = (int)(spin.Value ?? -1);
            if (strref < 0)
            {
                ShowPlayMessage("No string ref assigned. Set a TLK string reference (≥ 0) to play a sound.");
                return;
            }
            if (_talktable == null)
            {
                ShowPlayMessage("No talk table loaded. Use 'Select Talk Table' to choose dialog.tlk.");
                return;
            }
            var resref = _talktable.GetSound(strref);
            string resname = resref?.ToString()?.Trim();
            if (string.IsNullOrEmpty(resname))
            {
                ShowPlayMessage("No sound linked in the talk table for this string ref.");
                return;
            }
            if (_installation == null)
            {
                ShowPlayMessage("No game installation loaded. Sound playback requires an installation to resolve WAV files.");
                return;
            }
            byte[] wavBytes = _installation.Sound(resname, VoiceSearchOrder);
            if (wavBytes == null || wavBytes.Length == 0)
            {
                ShowPlayMessage($"Sound '{resname}' not found (searched Override, StreamVoice/StreamWaves, StreamSounds, CHITIN).");
                return;
            }
            try
            {
                if (_player == null)
                {
                    _player = new NAudioMediaPlayer();
                    _player.PlaybackStopped += (s, ev) => { CleanupTempPlayFile(); Dispatcher.UIThread.Post(() => SetPlayingState(null)); };
                }
                _player.Stop();
                CleanupTempPlayFile();
                _tempPlayPath = Path.Combine(Path.GetTempPath(), "AndastraSSF_" + Guid.NewGuid().ToString("N") + ".wav");
                File.WriteAllBytes(_tempPlayPath, wavBytes);
                _player.SetSource(_tempPlayPath);
                _player.Play();
                SetPlayingState(sound);
            }
            catch (Exception ex)
            {
                ShowPlayMessage("Playback failed: " + ex.Message);
            }
        }

        private void ShowPlayMessage(string message)
        {
            Dispatcher.UIThread.Post(() =>
            {
                try
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("OdyToolSSF", message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info);
                    _ = box.ShowWindowDialogAsync(this);
                }
                catch { }
            });
        }

        private void CleanupTempPlayFile()
        {
            try
            {
                if (!string.IsNullOrEmpty(_tempPlayPath) && File.Exists(_tempPlayPath))
                    File.Delete(_tempPlayPath);
            }
            catch { }
            _tempPlayPath = null;
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            _player?.Stop();
            _player?.Dispose();
            _player = null;
            CleanupTempPlayFile();
        }

        private void LoadFromBytes(byte[] data)
        {
            SSF ssf = data == null || data.Length == 0 ? new SSF() : null;
            if (ssf == null)
            {
                try { ssf = SSFAuto.ReadSsf(data); }
                catch { ssf = new SSF(); }
            }
            _undoRedoInProgress = true;
            try
            {
                foreach (var kv in _spinBySound)
                    if (kv.Value != null)
                        kv.Value.Value = ssf.Get(kv.Key) ?? 0;
            }
            finally { _undoRedoInProgress = false; }
            UpdateTextBoxes();
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _undoStack.Clear();
            _redoStack.Clear();
            LoadFromBytes(data);
        }

        public override Tuple<byte[], byte[]> Build()
        {
            var ssf = new SSF();
            foreach (var kv in _spinBySound)
                if (kv.Value != null)
                    ssf.SetData(kv.Key, (int)(kv.Value.Value ?? 0));
            byte[] data = SSFAuto.BytesSsf(ssf);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _undoStack.Clear();
            _redoStack.Clear();
            foreach (var spin in _spinBySound.Values)
                if (spin != null)
                    spin.Value = 0;
            UpdateTextBoxes();
        }

        private void UpdateTextBoxes()
        {
            if (_talktable == null) return;
            var stringrefs = new List<int>();
            foreach (var kv in _spinBySound)
            {
                if (kv.Value == null || !kv.Value.Value.HasValue) continue;
                int strref = (int)kv.Value.Value.Value;
                if (!stringrefs.Contains(strref)) stringrefs.Add(strref);
            }
            var results = _talktable.Batch(stringrefs);
            foreach (var kv in _spinBySound)
            {
                if (kv.Value == null || !kv.Value.Value.HasValue || !_soundTextPairs.TryGetValue(kv.Key, out var pair)) continue;
                int strref = (int)kv.Value.Value.Value;
                if (!results.TryGetValue(strref, out var result)) continue;
                pair.SoundEdit.Text = result.Sound?.ToString() ?? "";
                pair.TextEdit.Text = result.Text ?? "";
            }
            UpdatePreviewPanel();
        }

        private void UpdateTalktableLabel()
        {
            if (_talktableLabel != null)
                _talktableLabel.Text = _talktable != null ? "Using: " + Path.GetFileName(_talktable.Path) : "No talk table";
        }

        private async Task SelectTalkTable()
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;
            var options = new FilePickerOpenOptions { Title = "Select Talk Table (TLK) File", AllowMultiple = false, FileTypeFilter = new List<FilePickerFileType> { new FilePickerFileType("Talk Table Files") { Patterns = new[] { "*.tlk" } }, new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } } } };
            try
            {
                var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
                if (files != null && files.Count > 0)
                {
                    string tlkPath = files[0].Path.LocalPath;
                    if (File.Exists(tlkPath))
                    {
                        _talktable = new TalkTable(tlkPath);
                        UpdateTalktableLabel();
                        UpdateTextBoxes();
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine($"Select talk table failed: {ex.Message}"); }
        }

        public override void SaveAs() => _ = RunSaveAsAsync();
    }
}
