using BioWare.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using BioWare.Resource.Formats.GFF.Generics;
using OdyTools.Data;
using OdyTools.Widgets;

namespace OdyTools.Editors
{
    public partial class OdyToolUTW : Editor
    {
        private const int MinEditorWidth = 400;
        private const int MinEditorHeight = 340;
        private const int UndoMaxLevels = 30;

        private UTW _utw;

        private TextBlock _statusText;
        private TextBlock _waypointSummary;
        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private readonly List<byte[]> _redoStack = new List<byte[]>();
        private bool _undoRedoInProgress;
        private string _findText = "";
        private bool _findMatchCase;

        // UI Controls - Basic
        private LocalizedStringEdit _nameEdit;
        private TextBox _tagEdit;
        private Button _tagGenerateButton;
        private TextBox _resrefEdit;
        private Button _resrefGenerateButton;

        // UI Controls - Advanced
        private CheckBox _isNoteCheckbox;
        private CheckBox _noteEnabledCheckbox;
        private LocalizedStringEdit _noteEdit;

        // UI Controls - Comments
        private TextBox _commentsEdit;

        public OdyToolUTW(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolUTW", "waypoint",
                new[] { ResourceType.UTW },
                new[] { ResourceType.UTW },
                installation)
        {
            _utw = new UTW();
            InitializeComponent();
            SetupSignals();
            SetupMenuHandlers();
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
            Opened += (s, e) => { UpdateStatusBar(); _tagEdit?.Focus(); };
            AddHelpAction("GFF-UTW.md");  // Adds or wires Help > Documentation when wiki exists
            KeyDown += OnWindowKeyDown;
            if (installation != null)
            {
                SetupInstallation(installation);
            }
            New();
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
            return menu;
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
            else
            {
                // Try to find controls from XAML
                _nameEdit = EditorHelpers.FindControlSafe<LocalizedStringEdit>(this, "nameEdit");
                _tagEdit = EditorHelpers.FindControlSafe<TextBox>(this, "tagEdit");
                _tagGenerateButton = EditorHelpers.FindControlSafe<Button>(this, "tagGenerateButton");
                _resrefEdit = EditorHelpers.FindControlSafe<TextBox>(this, "resrefEdit");
                _resrefGenerateButton = EditorHelpers.FindControlSafe<Button>(this, "resrefGenerateButton");
                _isNoteCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "isNoteCheckbox");
                _noteEnabledCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "noteEnabledCheckbox");
                _noteEdit = EditorHelpers.FindControlSafe<LocalizedStringEdit>(this, "noteEdit");
                _commentsEdit = EditorHelpers.FindControlSafe<TextBox>(this, "commentsEdit");
                _statusText = EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");
                _waypointSummary = EditorHelpers.FindControlSafe<TextBlock>(this, "waypointSummary");

                // If any critical controls are missing, fall back to programmatic UI
                if (_nameEdit == null || _tagEdit == null || _resrefEdit == null ||
                    _isNoteCheckbox == null || _noteEnabledCheckbox == null ||
                    _commentsEdit == null)
                {
                    SetupProgrammaticUI();
                }
                else
                {
                    AttachCommitHandlers();
                }
            }
        }

        private void SetupSignals()
        {
            if (_tagGenerateButton != null)
            {
                _tagGenerateButton.Click += (s, e) => GenerateTag();
            }
            if (_resrefGenerateButton != null)
            {
                _resrefGenerateButton.Click += (s, e) => GenerateResref();
            }

            // Add checkbox change handlers to properly bind to UTW properties
            // This eliminates the need for workarounds in headless testing
            if (_isNoteCheckbox != null)
            {
                _isNoteCheckbox.PropertyChanged += (s, e) =>
                {
                    if (e.Property == CheckBox.IsCheckedProperty)
                    {
                        _utw.HasMapNote = _isNoteCheckbox.IsChecked == true;
                    }
                };
            }
            if (_noteEnabledCheckbox != null)
            {
                _noteEnabledCheckbox.PropertyChanged += (s, e) =>
                {
                    if (e.Property == CheckBox.IsCheckedProperty)
                    {
                        _utw.MapNoteEnabled = _noteEnabledCheckbox.IsChecked == true;
                    }
                };
            }
        }

        private void SetupInstallation(OdyInstallation installation)
        {
            _nameEdit?.SetInstallation(installation);
            _noteEdit?.SetInstallation(installation);
        }

        private void SetupProgrammaticUI()
        {
            var scrollViewer = new ScrollViewer();
            var mainPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Tab Control
            var tabControl = new TabControl();

            // Basic Tab
            var basicTab = new TabItem { Header = "Basic" };
            var basicPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Name
            var nameLabel = new TextBlock { Text = "Name:" };
            _nameEdit = new LocalizedStringEdit();
            _nameEdit?.SetInstallation(_installation);
            basicPanel.Children.Add(nameLabel);
            basicPanel.Children.Add(_nameEdit);

            // Tag
            var tagLabel = new TextBlock { Text = "Tag:" };
            var tagPanel = new StackPanel { Orientation = Orientation.Horizontal };
            _tagEdit = new TextBox();
            _tagGenerateButton = new Button { Content = "⟳", MinWidth = 32, MinHeight = 32 };
            _tagGenerateButton.Click += (s, e) => GenerateTag();
            tagPanel.Children.Add(_tagEdit);
            tagPanel.Children.Add(_tagGenerateButton);
            basicPanel.Children.Add(tagLabel);
            basicPanel.Children.Add(tagPanel);

            // ResRef
            var resrefLabel = new TextBlock { Text = "ResRef:" };
            var resrefPanel = new StackPanel { Orientation = Orientation.Horizontal };
            _resrefEdit = new TextBox { MaxLength = 16 };
            _resrefGenerateButton = new Button { Content = "⟳", MinWidth = 32, MinHeight = 32 };
            _resrefGenerateButton.Click += (s, e) => GenerateResref();
            resrefPanel.Children.Add(_resrefEdit);
            resrefPanel.Children.Add(_resrefGenerateButton);
            basicPanel.Children.Add(resrefLabel);
            basicPanel.Children.Add(resrefPanel);

            basicTab.Content = basicPanel;
            tabControl.Items.Add(basicTab);

            // Advanced Tab
            var advancedTab = new TabItem { Header = "Advanced" };
            var advancedPanel = new StackPanel { Orientation = Orientation.Vertical };

            _isNoteCheckbox = new CheckBox { Content = "Is a Map Note" };
            _noteEnabledCheckbox = new CheckBox { Content = "Map Note is Enabled" };

            // Map Note
            var noteLabel = new TextBlock { Text = "Map Note:" };
            _noteEdit = new LocalizedStringEdit();

            advancedPanel.Children.Add(_isNoteCheckbox);
            advancedPanel.Children.Add(_noteEnabledCheckbox);
            advancedPanel.Children.Add(noteLabel);
            advancedPanel.Children.Add(_noteEdit);

            advancedTab.Content = advancedPanel;
            tabControl.Items.Add(advancedTab);

            // Comments Tab
            var commentsTab = new TabItem { Header = "Comments" };
            _commentsEdit = new TextBox
            {
                AcceptsReturn = true,
                AcceptsTab = true,
                TextWrapping = Avalonia.Media.TextWrapping.Wrap
            };
            commentsTab.Content = _commentsEdit;
            tabControl.Items.Add(commentsTab);

            mainPanel.Children.Add(tabControl);
            scrollViewer.Content = mainPanel;
            var dock = new DockPanel();
            dock.Children.Add(BuildMenu());
            DockPanel.SetDock(dock.Children[0], Dock.Top);
            dock.Children.Add(scrollViewer);
            _statusText = new TextBlock { Name = "statusText", Text = "Waypoint", Margin = new Avalonia.Thickness(4, 2) };
            dock.Children.Add(_statusText);
            DockPanel.SetDock(_statusText, Dock.Bottom);
            Content = dock;
            AttachCommitHandlers();
        }

        private void SetupMenuHandlers()
        {
            void Bind(string name, Action handler)
            {
                try
                {
                    var item = EditorHelpers.FindControlSafe<MenuItem>(this, name) ?? this.FindControl<MenuItem>(name);
                    if (item != null) item.Click += (s, e) => handler();
                }
                catch { }
            }
            // actionNew, actionOpen, actionSave, actionSaveAs, actionRevert, actionExit wired by base Editor
            Bind("actionUndo", () => Undo());
            Bind("actionRedo", () => Redo());
            Bind("actionFind", () => ShowFindDialog());
            Bind("actionFindNext", () => FindNextMatch());
        }

        private void AttachCommitHandlers()
        {
            void OnCommit(object s, EventArgs e) { if (!_undoRedoInProgress) PushState(); }
            if (_nameEdit != null) _nameEdit.LostFocus += OnCommit;
            if (_tagEdit != null) _tagEdit.LostFocus += OnCommit;
            if (_resrefEdit != null) _resrefEdit.LostFocus += OnCommit;
            if (_noteEdit != null) _noteEdit.LostFocus += OnCommit;
            if (_commentsEdit != null) _commentsEdit.LostFocus += OnCommit;
            if (_isNoteCheckbox != null) _isNoteCheckbox.LostFocus += OnCommit;
            if (_noteEnabledCheckbox != null) _noteEnabledCheckbox.LostFocus += OnCommit;
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
                _utw = new UTW();
                LoadUTW(_utw);
            }
            else
            {
                try
                {
                    var utw = UTWAuto.ReadUtw(data);
                    LoadUTW(utw);
                }
                catch
                {
                    _utw = new UTW();
                    LoadUTW(_utw);
                }
            }
            _undoRedoInProgress = true;
            try { UpdateStatusBar(); }
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
            string suggestedName = string.IsNullOrEmpty(_resname) ? "waypoint" : _resname;
            var options = new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggestedName + ".utw",
                FileTypeChoices = new[] { new FilePickerFileType("UTW") { Patterns = new[] { "*.utw" } } }
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
                string text = _utw == null ? "Waypoint" : (_utw.Tag ?? "Waypoint");
                if (!string.IsNullOrEmpty(_utw?.ResRef?.ToString())) text += " | " + _utw.ResRef;
                if (_statusText != null) _statusText.Text = text;
                if (_waypointSummary != null) _waypointSummary.Text = text;
                // Fallback when programmatic UI is used and _statusText was not captured
                if (_statusText == null)
                {
                    var c = EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");
                    if (c != null) c.Text = text;
                }
            }
            catch { }
        }

        private void ShowFindDialog()
        {
            var dialog = new Window
            {
                Title = "Find in Waypoint",
                Width = 400,
                MinWidth = 360,
                Height = 200,
                MinHeight = 160,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var findLabel = new TextBlock { Text = "Find what:", Margin = new Avalonia.Thickness(16, 16, 16, 6) };
            var findBox = new TextBox { Text = _findText, Watermark = "Tag, ResRef, or Comments...", Margin = new Avalonia.Thickness(16, 0, 16, 12), MinHeight = 28 };
            var matchCase = new CheckBox { Content = "Match case", IsChecked = _findMatchCase, Margin = new Avalonia.Thickness(16, 4, 16, 16) };
            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right, Margin = new Avalonia.Thickness(16, 8, 16, 16), Spacing = 10 };
            var findNext = new Button { Content = "Find Next", MinWidth = 90 };
            var closeBtn = new Button { Content = "Close", MinWidth = 90 };
            btnPanel.Children.Add(findNext);
            btnPanel.Children.Add(closeBtn);
            var panel = new StackPanel();
            panel.Children.Add(findLabel);
            panel.Children.Add(findBox);
            panel.Children.Add(matchCase);
            panel.Children.Add(btnPanel);
            dialog.Content = panel;
            findNext.Click += (s, e) => { _findText = findBox.Text ?? ""; _findMatchCase = matchCase.IsChecked == true; FindNextMatch(); };
            closeBtn.Click += (s, e) => dialog.Close();
            dialog.Opened += (s, e) => findBox.Focus();
            dialog.ShowDialog(this);
        }

        private void FindNextMatch()
        {
            if (string.IsNullOrEmpty(_findText)) return;
            string t = _findMatchCase ? _findText : _findText.ToLowerInvariant();
            bool Match(string value) => value != null && (_findMatchCase ? value : value.ToLowerInvariant()).Contains(t);
            if (Match(_tagEdit?.Text) && _tagEdit != null) { _tagEdit.Focus(); return; }
            if (Match(_resrefEdit?.Text) && _resrefEdit != null) { _resrefEdit.Focus(); return; }
            if (Match(_commentsEdit?.Text) && _commentsEdit != null) { _commentsEdit.Focus(); return; }
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
                System.Console.WriteLine($"Failed to load UTW: {ex}");
                New();
            }
        }

        private void LoadUTW(UTW utw)
        {
            _utw = utw;

            // Basic
            if (_nameEdit != null)
            {
                _nameEdit.SetLocString(utw.Name);
            }
            if (_tagEdit != null)
            {
                _tagEdit.Text = utw.Tag ?? "";
            }
            if (_resrefEdit != null)
            {
                _resrefEdit.Text = utw.ResRef?.ToString() ?? "";
            }

            // Advanced
            if (_isNoteCheckbox != null)
            {
                _isNoteCheckbox.IsChecked = utw.HasMapNote;
            }
            if (_noteEnabledCheckbox != null)
            {
                _noteEnabledCheckbox.IsChecked = utw.MapNoteEnabled;
            }
            if (_noteEdit != null)
            {
                _noteEdit.SetLocString(utw.MapNote);
            }

            // Comments
            if (_commentsEdit != null)
            {
                _commentsEdit.Text = utw.Comment ?? "";
            }
        }

        public override Tuple<byte[], byte[]> Build()
        {
            // Matching Python: utw: UTW = deepcopy(self._utw)
            var utw = CopyUtw(_utw);

            // Matching Python: utw.name = self.ui.nameEdit.locstring()
            if (_nameEdit != null)
            {
                utw.Name = _nameEdit.GetLocString();
            }

            // Matching Python: utw.tag = self.ui.tagEdit.text()
            if (_tagEdit != null)
            {
                utw.Tag = _tagEdit.Text ?? "";
            }

            // Matching Python: utw.resref = ResRef(self.ui.resrefEdit.text())
            if (_resrefEdit != null)
            {
                utw.ResRef = new ResRef(_resrefEdit.Text ?? "");
            }

            // Matching Python: utw.has_map_note = self.ui.isNoteCheckbox.isChecked()
            if (_isNoteCheckbox != null)
            {
                utw.HasMapNote = _isNoteCheckbox.IsChecked == true;
            }

            // Matching Python: utw.map_note_enabled = self.ui.noteEnabledCheckbox.isChecked()
            if (_noteEnabledCheckbox != null)
            {
                utw.MapNoteEnabled = _noteEnabledCheckbox.IsChecked == true;
            }

            // Matching Python: utw.map_note = self.ui.noteEdit.locstring / LocalizedString(self.ui.noteEdit.text())
            if (_noteEdit != null)
            {
                utw.MapNote = _noteEdit.GetLocString();
            }

            // Matching Python: utw.comment = self.ui.commentsEdit.toPlainText()
            if (_commentsEdit != null)
            {
                utw.Comment = _commentsEdit.Text ?? "";
            }

            // Matching Python: gff: GFF = dismantle_utw(utw); write_gff(gff, data)
            byte[] data = UTWAuto.BytesUtw(utw);
            return Tuple.Create(data, new byte[0]);
        }

        // Matching Python: deepcopy(self._utw)
        private static UTW CopyUtw(UTW source)
        {
            var copy = new UTW();
            copy.Name = CopyLocalizedString(source.Name);
            copy.Tag = source.Tag;
            copy.ResRef = source.ResRef;
            copy.HasMapNote = source.HasMapNote;
            copy.MapNoteEnabled = source.MapNoteEnabled;
            copy.MapNote = CopyLocalizedString(source.MapNote);
            copy.AppearanceId = source.AppearanceId;
            copy.PaletteId = source.PaletteId;
            copy.Comment = source.Comment;
            copy.LinkedTo = source.LinkedTo;
            copy.Description = CopyLocalizedString(source.Description);
            return copy;
        }

        private static LocalizedString CopyLocalizedString(LocalizedString source)
        {
            if (source == null)
            {
                return LocalizedString.FromInvalid();
            }
            var copy = new LocalizedString(source.StringRef);
            foreach (var (language, gender, text) in source)
            {
                copy.SetData(language, gender, text);
            }
            return copy;
        }

        public override void New()
        {
            base.New();
            _undoStack.Clear();
            _redoStack.Clear();
            LoadUTW(new UTW());
            UpdateStatusBar();
        }

        // Note: Name change is handled by LocalizedStringEdit's edit button


        private void GenerateTag()
        {
            if (_resrefEdit != null && string.IsNullOrEmpty(_resrefEdit.Text))
            {
                GenerateResref();
            }
            if (_tagEdit != null && _resrefEdit != null)
            {
                _tagEdit.Text = _resrefEdit.Text;
            }
        }

        private void GenerateResref()
        {
            if (_resrefEdit != null)
            {
                if (!string.IsNullOrEmpty(_resname))
                {
                    _resrefEdit.Text = _resname;
                }
                else
                {
                    _resrefEdit.Text = "m00xx_way_000";
                }
            }
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        // Public properties for testing - matching Python's self.ui structure
        public LocalizedStringEdit NameEdit => _nameEdit;
        public TextBox TagEdit => _tagEdit;
        public Button TagGenerateButton => _tagGenerateButton;
        public TextBox ResrefEdit => _resrefEdit;
        public Button ResrefGenerateButton => _resrefGenerateButton;
        public CheckBox IsNoteCheckbox => _isNoteCheckbox;
        public CheckBox NoteEnabledCheckbox => _noteEnabledCheckbox;
        public LocalizedStringEdit NoteEdit => _noteEdit;
        public TextBox CommentsEdit => _commentsEdit;
        public UTW Utw => _utw;
    }
}
