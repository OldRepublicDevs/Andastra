using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using BioWare.Resource.Formats.LIP;
using BioWare.Common;
using OdyTools.Data;
using OdyTools.Dialogs;

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
            var panel = new StackPanel();
            var dock = new DockPanel();
            var menu = BuildMenu();
            dock.Children.Add(menu);
            DockPanel.SetDock(menu, Dock.Top);
            dock.Children.Add(panel);
            _statusText = new Avalonia.Controls.TextBlock { Name = "statusText", Text = "LIP", Margin = new Avalonia.Thickness(4, 2) };
            dock.Children.Add(_statusText);
            DockPanel.SetDock(_statusText, Dock.Bottom);
            SetContentOrInject(dock);
        }

        private void SetupSignals()
        {
            Opened += (s, e) => { UpdateStatusBar(); Focus(); };
            KeyDown += OnWindowKeyDown;
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
        /// Loads LIP into editor state. UI binding for keyframes/duration can be expanded when additional fields are added.
        /// </summary>
        private void LoadLIP(LIP lip)
        {
            _lip = lip ?? _lip;
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
            UpdateStatusBar();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        // Properties for tests
        public LIP Lip => _lip;
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
            }
        }
    }
}
