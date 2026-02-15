using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using BioWare.Common;
using BioWare.Resource;
using OdyTools.Data;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace OdyTools.Editors
{
    /// <summary>
    /// Full-featured text editor matching Windows 11 Notepad capabilities:
    /// File (New, Open, Save, Save As, Revert, Exit), Edit (Undo, Redo, Cut, Copy, Paste,
    /// Delete, Find, Replace, Go To Line, Select All, Time/Date), Format (Word Wrap, Font),
    /// View (Zoom, Status Bar).
    /// </summary>
    public partial class OdyToolTXT : Editor
    {
        private const int MinEditorWidth = 500;
        private const int MinEditorHeight = 350;
        private const int UndoMaxLevels = 50;
        private const double DefaultFontSize = 14;
        private const double MinZoom = 8;
        private const double MaxZoom = 72;

        private TextBox _textEdit;
        private TextBlock _statusLnCol;
        private TextBlock _statusChars;
        private TextBlock _statusLines;
        private TextBlock _statusEncoding;
        private TextBlock _zoomLabel;
        private Border _statusBar;
        private MenuItem _actionWordWrap;
        private MenuItem _actionStatusBar;

        private bool _wordWrap;
        private double _currentFontSize = DefaultFontSize;
        private string _fontFamily = "Consolas,Courier New,Monospace";
        private bool _statusBarVisible = true;
        private string _encodingName = "UTF-8";
        private bool _useUtf8Bom;

        private readonly List<string> _undoStack = new List<string>();
        private readonly List<string> _redoStack = new List<string>();
        private bool _undoRedoInProgress;

        private string _findText = "";
        private string _replaceText = "";
        private bool _findMatchCase;
        private bool _findWholeWord;
        private bool _findDown = true;
        private int _lastFindStart;

        public OdyToolTXT(Window parent = null, OdyInstallation installation = null)
            : base(parent, "Text Editor (KotOR)", "none", GetSupportedTypes(), GetSupportedTypes(), installation)
        {
            InitializeComponent();
            SetupUI();
            SetupSignals();
            SetupMenuHandlers();
            SetupToolbar();
            SetupContextMenu();
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
            Width = 800;
            Height = 600;
            _wordWrap = false;
            New();
        }

        private void InitializeComponent()
        {
            try
            {
                AvaloniaXamlLoader.Load(this);
                _textEdit = this.FindControl<TextBox>("TextEdit") ?? EditorHelpers.FindControlSafe<TextBox>(this, "TextEdit");
            }
            catch
            {
                _textEdit = null;
            }

            if (_textEdit == null)
                SetupProgrammaticUI();
        }

        private void SetupUI()
        {
            _statusLnCol = this.FindControl<TextBlock>("statusLnCol");
            _statusChars = this.FindControl<TextBlock>("statusChars");
            _statusLines = this.FindControl<TextBlock>("statusLines");
            _statusEncoding = this.FindControl<TextBlock>("statusEncoding");
            _zoomLabel = this.FindControl<TextBlock>("zoomLabel");
            _statusBar = this.FindControl<Border>("statusBar");
            _actionWordWrap = this.FindControl<MenuItem>("actionWordWrap");
            _actionStatusBar = this.FindControl<MenuItem>("actionStatusBar");
        }

        private void SetupSignals()
        {
            KeyDown += OnWindowKeyDown;
            Opened += (s, e) =>
            {
                UpdateStatusBar();
                _textEdit?.Focus();
            };
            if (_textEdit != null)
            {
                _textEdit.LostFocus += (s, e) => CommitEdits();
                _textEdit.TextChanged += (s, e) => Dispatcher.UIThread.Post(UpdateStatusBar);
                _textEdit.KeyUp += (s, e) => UpdateStatusBar();
            }
        }

        private void SetupMenuHandlers()
        {
            void Bind(string name, Action handler)
            {
                try
                {
                    var item = this.FindControl<MenuItem>(name);
                    if (item != null) item.Click += (s, e) => handler();
                }
                catch { }
            }

            Bind("actionNew", () => New());
            Bind("actionOpen", () => _ = RunOpenAsync());
            Bind("actionSave", () => Save());
            Bind("actionSaveAs", () => _ = RunSaveAsAsync());
            Bind("actionRevert", () => Revert());
            Bind("actionExit", () => Close());

            Bind("actionUndo", () => Undo());
            Bind("actionRedo", () => Redo());

            Bind("actionCut", () => Cut());
            Bind("actionCopy", () => Copy());
            Bind("actionPaste", () => Paste());
            Bind("actionDelete", () => Delete());

            Bind("actionFind", () => ShowFindDialog());
            Bind("actionReplace", () => ShowReplaceDialog());
            Bind("actionFindNext", () => FindNextMatch());
            Bind("actionFindPrevious", () => FindPreviousMatch());

            Bind("actionGoToLine", () => ShowGoToLineDialog());
            Bind("actionSelectAll", () => SelectAll());
            Bind("actionTimeDate", () => InsertTimeDate());

            Bind("actionWordWrap", () => ToggleWordWrap());
            Bind("actionFont", () => ShowFontDialog());

            Bind("actionZoomIn", () => ZoomIn());
            Bind("actionZoomOut", () => ZoomOut());
            Bind("actionZoomReset", () => ZoomReset());
            Bind("actionStatusBar", () => ToggleStatusBar());

            Bind("ctxUndo", () => Undo());
            Bind("ctxRedo", () => Redo());
            Bind("ctxCut", () => Cut());
            Bind("ctxCopy", () => Copy());
            Bind("ctxPaste", () => Paste());
            Bind("ctxDelete", () => Delete());
            Bind("ctxFind", () => ShowFindDialog());
            Bind("ctxReplace", () => ShowReplaceDialog());
            Bind("ctxFindNext", () => FindNextMatch());
            Bind("ctxSelectAll", () => SelectAll());
            Bind("ctxTimeDate", () => InsertTimeDate());
        }

        private void SetupToolbar()
        {
            void Bind(string name, Action handler)
            {
                var btn = this.FindControl<Button>(name);
                if (btn != null) btn.Click += (s, e) => handler();
            }
            Bind("tbNew", () => New());
            Bind("tbOpen", () => _ = RunOpenAsync());
            Bind("tbSave", () => Save());
            Bind("tbCut", () => Cut());
            Bind("tbCopy", () => Copy());
            Bind("tbPaste", () => Paste());
            Bind("tbUndo", () => Undo());
            Bind("tbRedo", () => Redo());
            Bind("tbFind", () => ShowFindDialog());
            Bind("tbReplace", () => ShowReplaceDialog());
        }

        private void SetupContextMenu()
        {
            // Already bound in SetupMenuHandlers
        }

        private void CommitEdits()
        {
            if (_textEdit == null) return;
            string current = _textEdit.Text ?? "";
            if (_undoStack.Count > 0 && _undoStack[_undoStack.Count - 1] == current) return;
            PushState();
        }

        private void PushState()
        {
            if (_undoRedoInProgress || _textEdit == null) return;
            _redoStack.Clear();
            _undoStack.Add(_textEdit.Text ?? "");
            if (_undoStack.Count > UndoMaxLevels) _undoStack.RemoveAt(0);
        }

        private void ApplyState(string text)
        {
            if (_textEdit == null) return;
            _textEdit.Text = text ?? "";
            UpdateStatusBar();
        }

        public void Undo()
        {
            if (_undoStack.Count == 0) return;
            _undoRedoInProgress = true;
            try
            {
                _redoStack.Add(_textEdit?.Text ?? "");
                string text = _undoStack[_undoStack.Count - 1];
                _undoStack.RemoveAt(_undoStack.Count - 1);
                ApplyState(text);
            }
            finally { _undoRedoInProgress = false; }
        }

        public void Redo()
        {
            if (_redoStack.Count == 0) return;
            _undoRedoInProgress = true;
            try
            {
                _undoStack.Add(_textEdit?.Text ?? "");
                string text = _redoStack[_redoStack.Count - 1];
                _redoStack.RemoveAt(_redoStack.Count - 1);
                ApplyState(text);
            }
            finally { _undoRedoInProgress = false; }
        }

        private void Cut()
        {
            _textEdit?.Cut();
            UpdateStatusBar();
        }

        private void Copy()
        {
            _textEdit?.Copy();
        }

        private void Paste()
        {
            _textEdit?.Paste();
            UpdateStatusBar();
        }

        private void Delete()
        {
            if (_textEdit == null) return;
            int start = Math.Min(_textEdit.SelectionStart, _textEdit.SelectionEnd);
            int end = Math.Max(_textEdit.SelectionStart, _textEdit.SelectionEnd);
            if (start == end) return;
            PushState();
            string t = _textEdit.Text ?? "";
            _textEdit.Text = t.Remove(start, end - start);
            _textEdit.SelectionStart = _textEdit.SelectionEnd = start;
            UpdateStatusBar();
        }

        private void InsertTimeDate()
        {
            if (_textEdit == null) return;
            PushState();
            string stamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.CurrentCulture);
            int pos = _textEdit.SelectionStart;
            string t = _textEdit.Text ?? "";
            _textEdit.Text = t.Insert(pos, stamp);
            _textEdit.SelectionStart = _textEdit.SelectionEnd = pos + stamp.Length;
            UpdateStatusBar();
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            var mod = e.KeyModifiers;
            bool ctrl = (mod & KeyModifiers.Control) != 0;

            if (ctrl)
            {
                if (e.Key == Key.N) { New(); e.Handled = true; }
                else if (e.Key == Key.O) { _ = RunOpenAsync(); e.Handled = true; }
                else if (e.Key == Key.S) { Save(); e.Handled = true; }
                else if (e.Key == Key.Z) { Undo(); e.Handled = true; }
                else if (e.Key == Key.Y) { Redo(); e.Handled = true; }
                else if (e.Key == Key.F) { ShowFindDialog(); e.Handled = true; }
                else if (e.Key == Key.H) { ShowReplaceDialog(); e.Handled = true; }
                else if (e.Key == Key.A) { SelectAll(); e.Handled = true; }
                else if (e.Key == Key.G) { ShowGoToLineDialog(); e.Handled = true; }
                else if (e.Key == Key.Add || e.Key == Key.OemPlus) { ZoomIn(); e.Handled = true; }
                else if (e.Key == Key.Subtract || e.Key == Key.OemMinus) { ZoomOut(); e.Handled = true; }
                else if (e.Key == Key.D0) { ZoomReset(); e.Handled = true; }
            }
            else
            {
                if (e.Key == Key.F3)
                {
                    if ((mod & KeyModifiers.Shift) != 0)
                        FindPreviousMatch();
                    else
                        FindNextMatch();
                    e.Handled = true;
                }
                else if (e.Key == Key.F5) { InsertTimeDate(); e.Handled = true; }
            }
        }

        private (int line, int col) GetLineAndColumn()
        {
            if (_textEdit == null) return (1, 1);
            string text = _textEdit.Text ?? "";
            int pos = Math.Min(_textEdit.SelectionStart, _textEdit.SelectionEnd);
            if (pos <= 0) return (1, 1);
            int line = 1;
            int col = 1;
            for (int i = 0; i < pos && i < text.Length; i++)
            {
                if (text[i] == '\n') { line++; col = 1; }
                else col++;
            }
            return (line, col);
        }

        private void UpdateStatusBar()
        {
            try
            {
                string text = _textEdit?.Text ?? "";
                int chars = text.Length;
                int lines = string.IsNullOrEmpty(text) ? 1 : text.Split('\n').Length;

                if (_statusLnCol != null)
                {
                    var (line, col) = GetLineAndColumn();
                    _statusLnCol.Text = $"Ln {line}, Col {col}";
                }
                if (_statusChars != null)
                    _statusChars.Text = $"{chars} characters";
                if (_statusLines != null)
                    _statusLines.Text = $"{lines} line{(lines == 1 ? "" : "s")}";
                if (_statusEncoding != null)
                    _statusEncoding.Text = _encodingName;

                int pct = (int)Math.Round(100.0 * _currentFontSize / DefaultFontSize);
                if (_zoomLabel != null)
                    _zoomLabel.Text = $"{pct}%";

                if (_actionWordWrap != null)
                    _actionWordWrap.IsChecked = _wordWrap;
                if (_actionStatusBar != null)
                    _actionStatusBar.IsChecked = _statusBarVisible;
            }
            catch { }
        }

        private void SelectAll()
        {
            if (_textEdit == null) return;
            _textEdit.SelectionStart = 0;
            _textEdit.SelectionEnd = (_textEdit.Text ?? "").Length;
            UpdateStatusBar();
        }

        private async Task RunOpenAsync()
        {
            var storageProvider = StorageProvider;
            if (storageProvider == null) return;
            var options = new FilePickerOpenOptions
            {
                Title = "Open",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Text files") { Patterns = new[] { "*.txt", "*.log", "*.nss", "*.xml", "*.json", "*.ini", "*.cfg" } },
                    new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
                }
            };
            var files = await storageProvider.OpenFilePickerAsync(options);
            if (files == null || files.Count == 0) return;
            string path = files[0].Path.LocalPath;
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return;
            try
            {
                byte[] data = File.ReadAllBytes(path);
                string resname = Path.GetFileNameWithoutExtension(path);
                string ext = Path.GetExtension(path).TrimStart('.');
                ResourceType restype = ResourceType.FromExtension(ext) ?? ResourceType.TXT;
                Load(path, resname, restype, data);
            }
            catch (Exception ex)
            {
                await MessageBoxManager.GetMessageBoxStandard("Open failed", $"Could not open file:\n{ex.Message}", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowWindowDialogAsync(this);
            }
        }

        private void Revert()
        {
            if (_revert == null || _revert.Length == 0) return;
            try
            {
                Load(Filepath ?? "", _resname ?? "", _restype ?? ResourceType.TXT, _revert);
                _undoStack.Clear();
                _redoStack.Clear();
                UpdateStatusBar();
            }
            catch (Exception ex) { Console.WriteLine($"Revert failed: {ex}"); }
        }

        private async Task RunSaveAsAsync()
        {
            var storageProvider = StorageProvider;
            if (storageProvider == null) return;
            string suggestedName = string.IsNullOrEmpty(_resname) ? "file" : _resname;
            var options = new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggestedName + ".txt",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("Text files") { Patterns = new[] { "*.txt", "*.log", "*.nss", "*.xml", "*.json", "*.ini", "*.cfg" } },
                    new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
                }
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

        private void SetupProgrammaticUI()
        {
            _textEdit = new TextBox
            {
                AcceptsReturn = true,
                AcceptsTab = true,
                TextWrapping = TextWrapping.NoWrap,
                FontFamily = _fontFamily,
                FontSize = _currentFontSize
            };
            Content = _textEdit;
        }

        private static ResourceType[] GetSupportedTypes()
        {
            return typeof(ResourceType).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)
                .Where(f => f.FieldType == typeof(ResourceType))
                .Select(f => (ResourceType)f.GetValue(null))
                .Where(rt => rt != null && rt.Contents == "plaintext")
                .ToArray();
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _undoStack.Clear();
            _redoStack.Clear();
            string text = DecodeBytesWithFallbacks(data, out _encodingName, out _useUtf8Bom);
            text = text.Replace("\r\n", "\n").Replace("\r", "\n");
            _textEdit.Text = text;
            UpdateStatusBar();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            string text = _textEdit?.Text ?? string.Empty;
            text = text.Replace("\r\n", "\n").Replace("\r", "\n");
            text = text.Replace("\n", Environment.NewLine);

            Encoding enc;
            if (_encodingName.Contains("UTF-8") || _encodingName.Contains("utf-8"))
            {
                enc = _useUtf8Bom ? new UTF8Encoding(true) : new UTF8Encoding(false);
            }
            else if (_encodingName.Contains("1252") || _encodingName.Contains("Windows"))
            {
                enc = Encoding.GetEncoding("windows-1252");
            }
            else
            {
                enc = Encoding.UTF8;
            }

            return Tuple.Create(enc.GetBytes(text), new byte[0]);
        }

        public override void New()
        {
            base.New();
            _undoStack.Clear();
            _redoStack.Clear();
            _textEdit.Text = "";
            _encodingName = "UTF-8";
            _useUtf8Bom = false;
            UpdateStatusBar();
        }

        public void ToggleWordWrap()
        {
            _wordWrap = !_wordWrap;
            _textEdit.TextWrapping = _wordWrap ? TextWrapping.Wrap : TextWrapping.NoWrap;
            UpdateStatusBar();
        }

        private void ShowFontDialog()
        {
            var dialog = new Window
            {
                Title = "Font",
                Width = 360,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(16), Spacing = 12 };
            var fontRow = new StackPanel { Orientation = Orientation.Horizontal };
            var fontLabel = new TextBlock { Text = "Font:", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Width = 80 };
            var fontBox = new TextBox { Text = _fontFamily, Watermark = "e.g. Consolas, Courier New", MinWidth = 180 };
            fontRow.Children.Add(fontLabel);
            fontRow.Children.Add(fontBox);
            var sizeRow = new StackPanel { Orientation = Orientation.Horizontal };
            var sizeLabel = new TextBlock { Text = "Size:", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Width = 80 };
            var sizeBox = new NumericUpDown { Value = (decimal)_currentFontSize, Minimum = (decimal)MinZoom, Maximum = (decimal)MaxZoom, Increment = 1, FormatString = "0", MinWidth = 80 };
            sizeRow.Children.Add(sizeLabel);
            sizeRow.Children.Add(sizeBox);
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 12, 0, 0) };
            var okBtn = new Button { Content = "OK", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var cancelBtn = new Button { Content = "Cancel" };
            buttons.Children.Add(okBtn);
            buttons.Children.Add(cancelBtn);
            panel.Children.Add(fontRow);
            panel.Children.Add(sizeRow);
            panel.Children.Add(buttons);
            dialog.Content = panel;

            okBtn.Click += (s, e) =>
            {
                _fontFamily = fontBox.Text?.Trim() ?? _fontFamily;
                if (string.IsNullOrEmpty(_fontFamily)) _fontFamily = "Consolas,Courier New,Monospace";
                _currentFontSize = Math.Max(MinZoom, Math.Min(MaxZoom, (double)(sizeBox.Value ?? (decimal)DefaultFontSize)));
                ApplyFont();
                dialog.Close();
            };
            cancelBtn.Click += (s, e) => dialog.Close();
            _ = dialog.ShowDialog(this);
        }

        private void ApplyFont()
        {
            if (_textEdit == null) return;
            _textEdit.FontFamily = _fontFamily;
            _textEdit.FontSize = _currentFontSize;
            UpdateStatusBar();
        }

        private void ZoomIn()
        {
            _currentFontSize = Math.Min(MaxZoom, _currentFontSize + 1);
            ApplyFont();
        }

        private void ZoomOut()
        {
            _currentFontSize = Math.Max(MinZoom, _currentFontSize - 1);
            ApplyFont();
        }

        private void ZoomReset()
        {
            _currentFontSize = DefaultFontSize;
            ApplyFont();
        }

        private void ToggleStatusBar()
        {
            _statusBarVisible = !_statusBarVisible;
            if (_statusBar != null)
                _statusBar.IsVisible = _statusBarVisible;
            UpdateStatusBar();
        }

        private void ShowGoToLineDialog()
        {
            string text = _textEdit?.Text ?? "";
            int maxLine = string.IsNullOrEmpty(text) ? 1 : text.Split('\n').Length;
            var (line, _) = GetLineAndColumn();

            var dialog = new Window
            {
                Title = "Go To Line",
                Width = 300,
                Height = 120,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(16), Spacing = 8 };
            var label = new TextBlock { Text = $"Line number (1 - {maxLine}):" };
            var lineBox = new NumericUpDown { Value = (decimal)line, Minimum = 1, Maximum = Math.Max(1, maxLine), Increment = 1, FormatString = "0" };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var goBtn = new Button { Content = "Go To", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var cancelBtn = new Button { Content = "Cancel" };
            buttons.Children.Add(goBtn);
            buttons.Children.Add(cancelBtn);
            panel.Children.Add(label);
            panel.Children.Add(lineBox);
            panel.Children.Add(buttons);
            dialog.Content = panel;

            goBtn.Click += (s, e) =>
            {
                int targetLine = (int)(lineBox.Value ?? 1);
                string[] lines = (_textEdit?.Text ?? "").Split('\n');
                int idx = 0;
                for (int i = 0; i < targetLine - 1 && i < lines.Length; i++)
                    idx += lines[i].Length + 1;
                _textEdit.SelectionStart = _textEdit.SelectionEnd = Math.Min(idx, (_textEdit.Text ?? "").Length);
                _textEdit.Focus();
                dialog.Close();
            };
            cancelBtn.Click += (s, e) => dialog.Close();
            _ = dialog.ShowDialog(this);
        }

        private void ShowFindDialog()
        {
            var dialog = new Window
            {
                Title = "Find",
                Width = 420,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(16), Spacing = 10 };
            var findLabel = new TextBlock { Text = "Find what:" };
            var findBox = new TextBox { Text = _findText, Watermark = "Search text" };
            var optsPanel = new StackPanel { Orientation = Orientation.Horizontal };
            var matchCaseCb = new CheckBox { Content = "Match case", IsChecked = _findMatchCase, Margin = new Avalonia.Thickness(0, 0, 16, 0) };
            var matchWordCb = new CheckBox { Content = "Match whole word", IsChecked = _findWholeWord, Margin = new Avalonia.Thickness(0, 0, 16, 0) };
            var directionPanel = new StackPanel { Orientation = Orientation.Horizontal };
            var downRb = new RadioButton { Content = "Down", IsChecked = _findDown };
            var upRb = new RadioButton { Content = "Up", IsChecked = !_findDown };
            directionPanel.Children.Add(downRb);
            directionPanel.Children.Add(upRb);
            optsPanel.Children.Add(matchCaseCb);
            optsPanel.Children.Add(matchWordCb);
            optsPanel.Children.Add(new TextBlock { Text = "Direction:", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Margin = new Avalonia.Thickness(16, 0, 8, 0) });
            optsPanel.Children.Add(directionPanel);
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 12, 0, 0) };
            var findNextBtn = new Button { Content = "Find Next", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var closeBtn = new Button { Content = "Close" };
            buttons.Children.Add(findNextBtn);
            buttons.Children.Add(closeBtn);
            panel.Children.Add(findLabel);
            panel.Children.Add(findBox);
            panel.Children.Add(optsPanel);
            panel.Children.Add(buttons);
            dialog.Content = panel;

            findNextBtn.Click += (s, e) =>
            {
                _findText = findBox.Text ?? "";
                _findMatchCase = matchCaseCb.IsChecked == true;
                _findWholeWord = matchWordCb.IsChecked == true;
                _findDown = downRb.IsChecked == true;
                bool found = _findDown ? FindNextMatch() : FindPreviousMatch();
                if (found) dialog.Close();
            };
            closeBtn.Click += (s, e) => dialog.Close();
            findBox.Focus();
            _ = dialog.ShowDialog(this);
        }

        private void ShowReplaceDialog()
        {
            var dialog = new Window
            {
                Title = "Replace",
                Width = 420,
                Height = 260,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(16), Spacing = 10 };
            var findLabel = new TextBlock { Text = "Find what:" };
            var findBox = new TextBox { Text = _findText, Watermark = "Search text" };
            var replaceLabel = new TextBlock { Text = "Replace with:" };
            var replaceBox = new TextBox { Text = _replaceText, Watermark = "Replacement" };
            var matchCaseCb = new CheckBox { Content = "Match case", IsChecked = _findMatchCase };
            var matchWordCb = new CheckBox { Content = "Match whole word", IsChecked = _findWholeWord };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 12, 0, 0) };
            var findNextBtn = new Button { Content = "Find Next", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var replaceOneBtn = new Button { Content = "Replace", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var replaceAllBtn = new Button { Content = "Replace All", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var closeBtn = new Button { Content = "Close" };
            buttons.Children.Add(findNextBtn);
            buttons.Children.Add(replaceOneBtn);
            buttons.Children.Add(replaceAllBtn);
            buttons.Children.Add(closeBtn);
            panel.Children.Add(findLabel);
            panel.Children.Add(findBox);
            panel.Children.Add(replaceLabel);
            panel.Children.Add(replaceBox);
            panel.Children.Add(matchCaseCb);
            panel.Children.Add(matchWordCb);
            panel.Children.Add(buttons);
            dialog.Content = panel;

            findNextBtn.Click += (s, e) =>
            {
                _findText = findBox.Text ?? "";
                _replaceText = replaceBox.Text ?? "";
                _findMatchCase = matchCaseCb.IsChecked == true;
                _findWholeWord = matchWordCb.IsChecked == true;
                FindNextMatch();
            };
            replaceOneBtn.Click += (s, e) =>
            {
                _findText = findBox.Text ?? "";
                _replaceText = replaceBox.Text ?? "";
                _findMatchCase = matchCaseCb.IsChecked == true;
                _findWholeWord = matchWordCb.IsChecked == true;
                ReplaceOne();
            };
            replaceAllBtn.Click += (s, e) =>
            {
                _findText = findBox.Text ?? "";
                _replaceText = replaceBox.Text ?? "";
                _findMatchCase = matchCaseCb.IsChecked == true;
                _findWholeWord = matchWordCb.IsChecked == true;
                if (string.IsNullOrEmpty(_findText)) { dialog.Close(); return; }
                PushState();
                ReplaceAll();
                dialog.Close();
            };
            closeBtn.Click += (s, e) => dialog.Close();
            findBox.Focus();
            _ = dialog.ShowDialog(this);
        }

        private bool MatchesWholeWord(string text, int idx, int len)
        {
            if (!_findWholeWord) return true;
            char before = idx > 0 ? text[idx - 1] : ' ';
            char after = idx + len < text.Length ? text[idx + len] : ' ';
            return !char.IsLetterOrDigit(before) && !char.IsLetterOrDigit(after);
        }

        private bool FindNextMatch()
        {
            if (_textEdit == null || string.IsNullOrEmpty(_findText)) return false;
            string text = _textEdit.Text ?? "";
            var comp = _findMatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int start = _lastFindStart;
            int idx = text.IndexOf(_findText, start, comp);
            if (idx < 0) { _lastFindStart = 0; idx = text.IndexOf(_findText, 0, comp); }
            while (idx >= 0 && !MatchesWholeWord(text, idx, _findText.Length))
            {
                idx = text.IndexOf(_findText, idx + 1, comp);
                if (idx < 0) { _lastFindStart = 0; idx = text.IndexOf(_findText, 0, comp); }
            }
            if (idx < 0) return false;
            _lastFindStart = idx + _findText.Length;
            _textEdit.SelectionStart = idx;
            _textEdit.SelectionEnd = idx + _findText.Length;
            _textEdit.Focus();
            UpdateStatusBar();
            return true;
        }

        private bool FindPreviousMatch()
        {
            if (_textEdit == null || string.IsNullOrEmpty(_findText)) return false;
            string text = _textEdit.Text ?? "";
            var comp = _findMatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int start = Math.Min(_textEdit.SelectionStart, _textEdit.SelectionEnd) - 1;
            if (start < 0) start = text.Length - 1;
            int idx = -1;
            for (int i = start; i >= 0; i--)
            {
                if (string.Compare(text, i, _findText, 0, _findText.Length, comp) == 0 && MatchesWholeWord(text, i, _findText.Length))
                { idx = i; break; }
            }
            if (idx < 0)
                for (int i = text.Length - 1; i >= 0; i--)
                {
                    if (string.Compare(text, i, _findText, 0, _findText.Length, comp) == 0 && MatchesWholeWord(text, i, _findText.Length))
                    { idx = i; break; }
                }
            if (idx < 0) return false;
            _lastFindStart = idx;
            _textEdit.SelectionStart = idx;
            _textEdit.SelectionEnd = idx + _findText.Length;
            _textEdit.Focus();
            UpdateStatusBar();
            return true;
        }

        private void ReplaceOne()
        {
            if (_textEdit == null || string.IsNullOrEmpty(_findText)) return;
            string text = _textEdit.Text ?? "";
            int start = _textEdit.SelectionStart;
            var comp = _findMatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int idx = text.IndexOf(_findText, start, comp);
            if (idx < 0) { _lastFindStart = 0; idx = text.IndexOf(_findText, 0, comp); }
            while (idx >= 0 && !MatchesWholeWord(text, idx, _findText.Length))
            {
                idx = text.IndexOf(_findText, idx + 1, comp);
                if (idx < 0) { _lastFindStart = 0; idx = text.IndexOf(_findText, 0, comp); }
            }
            if (idx < 0) return;
            PushState();
            _textEdit.Text = text.Remove(idx, _findText.Length).Insert(idx, _replaceText ?? "");
            _textEdit.SelectionStart = idx;
            _textEdit.SelectionEnd = idx + (_replaceText ?? "").Length;
            _lastFindStart = idx + (_replaceText ?? "").Length;
            UpdateStatusBar();
        }

        private void ReplaceAll()
        {
            if (_textEdit == null || string.IsNullOrEmpty(_findText)) return;
            string text = _textEdit.Text ?? "";
            string result = ReplaceAllInString(text, _findText, _replaceText ?? "", _findMatchCase, _findWholeWord);
            if (result != text) _textEdit.Text = result;
            _lastFindStart = 0;
            UpdateStatusBar();
        }

        private static string ReplaceAllInString(string text, string find, string replace, bool matchCase, bool wholeWord)
        {
            if (string.IsNullOrEmpty(find)) return text;
            var comparison = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int pos = 0;
            var sb = new StringBuilder();
            while (pos < text.Length)
            {
                int idx = text.IndexOf(find, pos, comparison);
                if (idx < 0) break;
                if (wholeWord)
                {
                    char before = idx > 0 ? text[idx - 1] : ' ';
                    char after = idx + find.Length < text.Length ? text[idx + find.Length] : ' ';
                    if (char.IsLetterOrDigit(before) || char.IsLetterOrDigit(after))
                    {
                        pos = idx + 1;
                        continue;
                    }
                }
                sb.Append(text, pos, idx - pos);
                sb.Append(replace);
                pos = idx + find.Length;
            }
            if (pos == 0) return text;
            sb.Append(text, pos, text.Length - pos);
            return sb.ToString();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        private string DecodeBytesWithFallbacks(byte[] data, out string encodingName, out bool useBom)
        {
            encodingName = "UTF-8";
            useBom = false;
            if (data == null || data.Length == 0) return string.Empty;

            if (data.Length >= 3 && data[0] == 0xEF && data[1] == 0xBB && data[2] == 0xBF)
            {
                encodingName = "UTF-8 with BOM";
                useBom = true;
                return Encoding.UTF8.GetString(data, 3, data.Length - 3);
            }
            try
            {
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                try
                {
                    encodingName = "Windows-1252";
                    return Encoding.GetEncoding("windows-1252").GetString(data);
                }
                catch
                {
                    encodingName = "Latin-1";
                    return Encoding.GetEncoding("latin-1").GetString(data);
                }
            }
        }

        private string DecodeBytesWithFallbacks(byte[] data)
        {
            return DecodeBytesWithFallbacks(data, out _, out _);
        }
    }
}
