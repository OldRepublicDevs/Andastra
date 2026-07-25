using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Encoding = System.Text.Encoding;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Tools;
using OdyTools.Common.Widgets;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Themes;
using OdyTools.Utils;
using OdyTools.Widgets;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using IconType = MsBox.Avalonia.Enums.Icon;

namespace OdyTools.Editors
{
    /// <summary>
    /// Full-featured NWScript (NSS) editor: load/save NSS and NCS (decompile), compile (F5),
    /// format document, analyze code, outline, bookmarks, snippets, problems, output, command palette.
    /// </summary>
    public partial class OdyToolNSS : Editor
    {
        private static ResourceType[] GetSupportedTypes() => new[] { ResourceType.NSS, ResourceType.NCS };

        private MonacoEditorHost _monacoHost;
        private CodeEditor _codeEdit;
        private CommandPalette _commandPalette;
        private FindReplaceWidget _findReplaceWidget;
        private BreadcrumbsWidget _breadcrumbs;
        private TextBlock _statusLeft;
        private TextBlock _statusRight;
        private VsCodeWorkbenchShell _workbenchShell;
        private TabControl _bottomTabs;
        private TabControl _sourceTabs;
        private TextBox _disassemblyBox;
        private ListBox _problemsList;
        private TextBox _outputBox;
        private TreeView _outlineView;
        private TreeView _bookmarkTree;
        private ListBox _snippetList;
        private TextBox _snippetSearchEdit;
        private Completer _completer;
        private NWScriptSyntaxHighlighter _highlighter;
        private List<(int Line, bool IsError, string Message)> _problemDiagnostics = new List<(int, bool, string)>();
        private Dictionary<string, string> _functions = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private bool _loadingDocument;
        private bool _clearInitialDirtyOnOpen = true;
        private bool _isTsl;
        private const string IndentString = "    ";

        internal string SourceTextForTest => _codeEdit?.Text ?? "";
        internal string OutputTextForTest => _outputBox?.Text ?? "";
        internal IReadOnlyList<(int Line, bool IsError, string Message)> DiagnosticsForTest => _problemDiagnostics.ToList();

        public OdyToolNSS() : this(null, null) { }
        public OdyToolNSS(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolNSS", "none", GetSupportedTypes(), new[] { ResourceType.NSS }, installation)
        {
            InitializeComponent();
            _isTsl = installation?.Tsl ?? false;
            BuildUI();
            SetupCompleterAndHighlighter();
            SetupKeyHandlers();
            SetupContextMenu();
            SetupCommandPalette();
            SetupSignals();
            AddHelpAction();
            New();
        }

        private void InitializeComponent()
        {
            try { Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this); }
            catch { }
        }

        private void BuildUI()
        {
            var mainDock = new DockPanel { LastChildFill = true };

            var menu = new Menu();
            var fileMenu = new MenuItem { Header = "_File" };
            fileMenu.Items.Add(CreateMenuItem("_New", () => New()));
            fileMenu.Items.Add(CreateMenuItem("_Open...", () => _ = RunOpenAsync()));
            fileMenu.Items.Add(CreateMenuItem("_Save", () => Save(), Key.S, KeyModifiers.Control));
            fileMenu.Items.Add(CreateMenuItem("Save _As...", () => SaveAs()));
            fileMenu.Items.Add(CreateMenuItem("_Revert", () => Revert()));
            fileMenu.Items.Add(new Separator());
            fileMenu.Items.Add(CreateMenuItem("E_xit", () => Close()));
            menu.Items.Add(fileMenu);

            var editMenu = new MenuItem { Header = "_Edit" };
            editMenu.Items.Add(CreateMenuItem("_Cut", () => _codeEdit?.Cut(), Key.X, KeyModifiers.Control));
            editMenu.Items.Add(CreateMenuItem("C_opy", () => _codeEdit?.Copy(), Key.C, KeyModifiers.Control));
            editMenu.Items.Add(CreateMenuItem("_Paste", () => _codeEdit?.Paste(), Key.V, KeyModifiers.Control));
            editMenu.Items.Add(CreateMenuItem("Select _All", () => _codeEdit?.SelectAll(), Key.A, KeyModifiers.Control));
            editMenu.Items.Add(new Separator());
            editMenu.Items.Add(CreateMenuItem("_Find...", () => ShowFind(), Key.F, KeyModifiers.Control));
            editMenu.Items.Add(CreateMenuItem("_Replace...", () => ShowReplace(), Key.H, KeyModifiers.Control));
            editMenu.Items.Add(CreateMenuItem("Go to _Line...", () => ShowGotoLine(), Key.G, KeyModifiers.Control));
            menu.Items.Add(editMenu);

            var viewMenu = new MenuItem { Header = "_View" };
            viewMenu.Items.Add(CreateMenuItem("_Command Palette", () => _commandPalette?.Show(), Key.P, KeyModifiers.Control | KeyModifiers.Shift));
            viewMenu.Items.Add(CreateMenuItem("Toggle _Sidebar", () => _workbenchShell?.ToggleSidebar(), Key.B, KeyModifiers.Control));
            viewMenu.Items.Add(CreateMenuItem("Toggle _Panel", () => ToggleBottomPanel(), Key.J, KeyModifiers.Control));
            viewMenu.Items.Add(CreateMenuItem("_Keyboard Shortcuts", () => { var d = new KeyboardShortcutsDialog(); d.ShowDialog(this); }));
            menu.Items.Add(viewMenu);

            var nssMenu = new MenuItem { Header = "_NSS" };
            nssMenu.Items.Add(CreateMenuItem("_Compile Script", () => CompileCurrentScript(), Key.F5));
            nssMenu.Items.Add(CreateMenuItem("_Format Document", () => FormatDocument()));
            nssMenu.Items.Add(CreateMenuItem("_Analyze Code", () => AnalyzeCode()));
            menu.Items.Add(nssMenu);

            DockPanel.SetDock(menu, Dock.Top);
            mainDock.Children.Add(menu);

            _workbenchShell = new VsCodeWorkbenchShell();
            _workbenchShell.PanelVisible = false;
            _workbenchShell.SidebarVisible = true;

            var sidebarContent = CreateSidebarContent();
            _workbenchShell.SidebarContent = sidebarContent;

            var editorColumn = new Grid { RowDefinitions = new RowDefinitions("Auto,*,Auto") };
            _breadcrumbs = new BreadcrumbsWidget();
            _breadcrumbs.ItemClicked += (path) => { };
            _breadcrumbs.SetPath(new List<string> { "script", "main" });
            Grid.SetRow(_breadcrumbs, 0);
            editorColumn.Children.Add(_breadcrumbs);

            var sourceEditorGrid = new Grid { RowDefinitions = new RowDefinitions("*,Auto") };

            _monacoHost = new MonacoEditorHost { ShowMinimap = true };
            _codeEdit = _monacoHost.CodeEditor;
            _codeEdit.AcceptsReturn = true;
            _codeEdit.AcceptsTab = true;
            _codeEdit.FontFamily = new FontFamily("Cascadia Code, Consolas, Menlo, Monaco, monospace");
            _codeEdit.FontSize = 12;
            _codeEdit.Background = MonacoColors.EditorBackgroundBrush;
            _codeEdit.Foreground = MonacoColors.EditorForegroundBrush;
            _codeEdit.TextChanged += (s, e) =>
            {
                if (!_loadingDocument)
                {
                    MarkDocumentDirty();
                }

                UpdateStatusBar();
                UpdateBreadcrumbs();
            };
            _codeEdit.KeyUp += (s, e) => UpdateStatusBar();
            Grid.SetRow(_monacoHost, 0);
            sourceEditorGrid.Children.Add(_monacoHost);

            _findReplaceWidget = new FindReplaceWidget();
            _findReplaceWidget.Background = MonacoColors.EditorBackgroundBrush;
            _findReplaceWidget.FindRequested += OnFindRequested;
            _findReplaceWidget.ReplaceRequested += OnReplaceRequested;
            _findReplaceWidget.ReplaceAllRequested += OnReplaceAllRequested;
            _findReplaceWidget.CloseRequested += () => { _findReplaceWidget.IsVisible = false; };
            _findReplaceWidget.FindNextRequested += () => OnFindNext();
            _findReplaceWidget.FindPreviousRequested += () => OnFindPrevious();
            Grid.SetRow(_findReplaceWidget, 1);
            sourceEditorGrid.Children.Add(_findReplaceWidget);

            _disassemblyBox = new TextBox
            {
                IsReadOnly = true,
                AcceptsReturn = true,
                TextWrapping = TextWrapping.NoWrap,
                FontFamily = new FontFamily("Consolas, Cascadia Code, monospace"),
                FontSize = 11,
                Background = MonacoColors.EditorBackgroundBrush,
                Foreground = MonacoColors.EditorForegroundBrush,
            };

            _sourceTabs = new TabControl();
            _sourceTabs.ItemsSource = new List<object>
            {
                new TabItem { Header = "Source", Content = sourceEditorGrid },
                new TabItem
                {
                    Header = "Disassembly",
                    Content = new ScrollViewer
                    {
                        Content = _disassemblyBox,
                        HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                    }
                },
            };

            Grid.SetRow(_sourceTabs, 1);
            editorColumn.Children.Add(_sourceTabs);

            _workbenchShell.EditorContent = editorColumn;

            _bottomTabs = new TabControl();
            var problemsItem = new TabItem { Header = "Problems", Content = CreateProblemsPanel() };
            var outputItem = new TabItem { Header = "Output", Content = CreateOutputPanel() };
            _bottomTabs.Background = MonacoColors.PanelBackgroundBrush;
            _bottomTabs.ItemsSource = new List<object> { problemsItem, outputItem };
            var panelContent = new Border { Background = new SolidColorBrush(MonacoColors.PanelBackground), Child = _bottomTabs };
            _workbenchShell.PanelContent = panelContent;

            var statusBarDock = new DockPanel { LastChildFill = true };
            _statusLeft = new TextBlock { Text = "Ln 1, Col 1", Foreground = MonacoColors.StatusBarForegroundBrush, VerticalAlignment = VerticalAlignment.Center };
            _statusRight = new TextBlock { Foreground = MonacoColors.StatusBarForegroundBrush, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Right };
            statusBarDock.Children.Add(_statusLeft);
            DockPanel.SetDock(_statusRight, Dock.Right);
            statusBarDock.Children.Add(_statusRight);
            _workbenchShell.StatusBarContent = statusBarDock;

            mainDock.Children.Add(_workbenchShell);
            Content = mainDock;
        }

        private Control CreateSidebarContent()
        {
            _outlineView = new TreeView { MinHeight = 100 };
            var outlinePanel = new StackPanel { Margin = new Thickness(4) };
            outlinePanel.Children.Add(new TextBlock { Text = "Outline", FontWeight = FontWeight.SemiBold, Margin = new Thickness(0, 0, 0, 4) });
            outlinePanel.Children.Add(_outlineView);
            var scroll = new ScrollViewer { Content = outlinePanel, HorizontalScrollBarVisibility = ScrollBarVisibility.Auto, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            return scroll;
        }

        private StackPanel CreateProblemsPanel()
        {
            _problemsList = new ListBox { SelectionMode = SelectionMode.Single };
            _problemsList.DoubleTapped += (s, e) =>
            {
                if (_problemsList.SelectedItem is ProblemItem pi)
                {
                    GotoLine(pi.Line);
                }
            };
            var sp = new StackPanel();
            sp.Children.Add(new TextBlock { Text = "Diagnostics", FontWeight = FontWeight.Bold, Margin = new Thickness(0, 0, 0, 4) });
            sp.Children.Add(_problemsList);
            return sp;
        }

        private StackPanel CreateOutputPanel()
        {
            _outputBox = new TextBox
            {
                IsReadOnly = true,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                MinHeight = 80,
                FontFamily = new FontFamily("Consolas, monospace"),
                FontSize = 11,
            };
            var sp = new StackPanel();
            sp.Children.Add(_outputBox);
            return sp;
        }

        private void SetupCompleterAndHighlighter()
        {
            _highlighter = new NWScriptSyntaxHighlighter(null, _installation);
            _completer = new Completer();
            _completer.SetWidget(_codeEdit);
            _completer.SetCaseSensitivity(false);
            _completer.CompletionSelected += (text) =>
            {
                if (_codeEdit == null || string.IsNullOrEmpty(text)) return;
                int pos = _codeEdit.SelectionStart;
                int len = _codeEdit.SelectionEnd - pos;
                _codeEdit.Text = _codeEdit.Text.Remove(pos, len).Insert(pos, text);
                _codeEdit.SelectionStart = _codeEdit.SelectionEnd = pos + text.Length;
            };
            LoadGameFunctions();
        }

        private void LoadGameFunctions()
        {
            _functions.Clear();
            try
            {
                string nwscriptPath = ScriptUtils.GetNwscriptPath(_installation?.Path, _isTsl);
                if (!string.IsNullOrEmpty(nwscriptPath) && File.Exists(nwscriptPath))
                {
                    string content = File.ReadAllText(nwscriptPath);
                    var rx = new Regex(@"void\s+(\w+)\s*\(([^)]*)\)", RegexOptions.Multiline);
                    foreach (Match m in rx.Matches(content))
                    {
                        string name = m.Groups[1].Value;
                        string args = m.Groups[2].Value;
                        if (!_functions.ContainsKey(name))
                            _functions[name] = $"void {name}({args})";
                    }
                }
            }
            catch { }
        }

        private void SetupKeyHandlers()
        {
            KeyDown += (s, e) =>
            {
                if (e.Key == Key.F5 && e.KeyModifiers == KeyModifiers.None) { CompileCurrentScript(); e.Handled = true; }
                if (e.Key == Key.G && e.KeyModifiers == KeyModifiers.Control) { ShowGotoLine(); e.Handled = true; }
                if (e.Key == Key.B && e.KeyModifiers == KeyModifiers.Control) { _workbenchShell?.ToggleSidebar(); e.Handled = true; }
                if (e.Key == Key.J && e.KeyModifiers == KeyModifiers.Control) { ToggleBottomPanel(); e.Handled = true; }
                if (e.Key == Key.P && (e.KeyModifiers & KeyModifiers.Shift) != 0 && (e.KeyModifiers & KeyModifiers.Control) != 0) { _commandPalette?.Show(); e.Handled = true; }
            };
            if (_codeEdit != null)
            {
                _codeEdit.KeyDown += (s, e) =>
                {
                    if (e.Key == Key.Space && (e.KeyModifiers & KeyModifiers.Control) != 0) { TriggerSuggest(); e.Handled = true; }
                    if (e.Key == Key.D && e.KeyModifiers == (KeyModifiers.Control | KeyModifiers.Shift)) { _codeEdit.DuplicateLine(); e.Handled = true; }
                    if (e.Key == Key.K && e.KeyModifiers == (KeyModifiers.Control | KeyModifiers.Shift)) { _codeEdit.DeleteLine(); e.Handled = true; }
                };
            }
        }

        private void SetupContextMenu()
        {
            if (_codeEdit == null) return;
            _codeEdit.ContextMenu = new ContextMenu();
            AddContextItem("Cut", () => _codeEdit.Cut(), Key.X, KeyModifiers.Control);
            AddContextItem("Copy", () => _codeEdit.Copy(), Key.C, KeyModifiers.Control);
            AddContextItem("Paste", () => _codeEdit.Paste(), Key.V, KeyModifiers.Control);
            AddContextItem("Select All", () => _codeEdit.SelectAll(), Key.A, KeyModifiers.Control);
            ((ContextMenu)_codeEdit.ContextMenu).Items.Add(new Separator());
            AddContextItem("Find...", () => ShowFind(), Key.F, KeyModifiers.Control);
            AddContextItem("Replace...", () => ShowReplace(), Key.H, KeyModifiers.Control);
            AddContextItem("Go to Line...", () => ShowGotoLine(), Key.G, KeyModifiers.Control);
            ((ContextMenu)_codeEdit.ContextMenu).Items.Add(new Separator());
            AddContextItem("Format Document", () => FormatDocument());
            AddContextItem("Analyze Code", () => AnalyzeCode());
            AddContextItem("Compile Script", () => CompileCurrentScript(), Key.F5);
        }

        private void AddContextItem(string header, Action action, Key? key = null, KeyModifiers? mod = null)
        {
            var item = new MenuItem { Header = header };
            if (key.HasValue) item.HotKey = new KeyGesture(key.Value, mod ?? KeyModifiers.None);
            item.Click += (s, e) => action();
            ((ContextMenu)_codeEdit.ContextMenu).Items.Add(item);
        }

        private void SetupCommandPalette()
        {
            _commandPalette = new CommandPalette(this);
            _commandPalette.RegisterCommand("file.new", "New", () => New(), "File");
            _commandPalette.RegisterCommand("file.open", "Open...", () => _ = RunOpenAsync(), "File");
            _commandPalette.RegisterCommand("file.save", "Save", () => Save(), "File");
            _commandPalette.RegisterCommand("editor.gotoLine", "Go to Line...", () => ShowGotoLine(), "Navigation");
            _commandPalette.RegisterCommand("nss.compile", "Compile Script", () => CompileCurrentScript(), "NSS");
            _commandPalette.RegisterCommand("nss.format", "Format Document", () => FormatDocument(), "NSS");
            _commandPalette.RegisterCommand("nss.analyze", "Analyze Code", () => AnalyzeCode(), "NSS");
            _commandPalette.RegisterCommand("view.toggleOutput", "Toggle Output Panel", () => ToggleBottomPanel(), "View");
        }

        private void SetupSignals()
        {
            Opened += (s, e) =>
            {
                UpdateStatusBar();
                _codeEdit?.Focus();
                ClearInitialDirtyOnOpen();
            };
        }

        private void ClearInitialDirtyOnOpen()
        {
            if (!_clearInitialDirtyOnOpen)
            {
                return;
            }

            ClearDirty();
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                if (_clearInitialDirtyOnOpen)
                {
                    ClearDirty();
                    _clearInitialDirtyOnOpen = false;
                }
            }, Avalonia.Threading.DispatcherPriority.Background);
        }

        private MenuItem CreateMenuItem(string header, Action action, Key? key = null, KeyModifiers mod = KeyModifiers.None)
        {
            var item = new MenuItem { Header = header };
            if (key.HasValue) item.HotKey = new KeyGesture(key.Value, mod);
            item.Click += (s, e) => action();
            return item;
        }

        private void ToggleBottomPanel()
        {
            if (_workbenchShell != null)
            {
                _workbenchShell.TogglePanel();
                UpdateStatusBar();
            }
        }

        private void UpdateStatusBar()
        {
            if (_statusLeft == null || _codeEdit == null) return;
            GetLineColumn(out int line, out int col);
            int total = GetTotalLineCount();
            string sel = _codeEdit.SelectionStart != _codeEdit.SelectionEnd ? $" ({_codeEdit.SelectionEnd - _codeEdit.SelectionStart} selected)" : "";
            _statusLeft.Text = $"Ln {line}, Col {col}{sel}  |  {total} lines";
            string sig = GetSignatureHelpAtCursor();
            string meta = "UTF-8 | " + (_codeEdit.Text.Contains("\r\n") ? "CRLF" : "LF") + " | " + (_isTsl ? "TSL" : "K1") + " | NSS";
            if (!string.IsNullOrEmpty(sig)) meta += " | " + sig;
            _statusRight.Text = meta;
        }

        private string GetSignatureHelpAtCursor()
        {
            if (_codeEdit == null || _functions.Count == 0) return "";
            string text = _codeEdit.Text;
            int pos = Math.Min(_codeEdit.SelectionStart, _codeEdit.SelectionEnd);
            if (pos <= 0 || pos > text.Length) return "";
            int paren = -1;
            for (int i = pos - 1; i >= 0; i--)
            {
                if (text[i] == ')') { int d = 1; while (i > 0 && d != 0) { i--; if (text[i] == ')') d++; else if (text[i] == '(') d--; } continue; }
                if (text[i] == '(') { paren = i; break; }
            }
            if (paren < 0) return "";
            int start = paren;
            while (start > 0 && (char.IsLetterOrDigit(text[start - 1]) || text[start - 1] == '_')) start--;
            string name = text.Substring(start, paren - start);
            return _functions.TryGetValue(name, out string sig) ? sig : "";
        }

        private void GetLineColumn(out int line, out int column)
        {
            line = 1; column = 1;
            if (_codeEdit == null || string.IsNullOrEmpty(_codeEdit.Text)) return;
            int pos = Math.Min(_codeEdit.SelectionStart, _codeEdit.SelectionEnd);
            string t = _codeEdit.Text;
            for (int i = 0; i < pos && i < t.Length; i++)
            {
                if (t[i] == '\n') { line++; column = 1; }
                else column++;
            }
        }

        private int GetTotalLineCount()
        {
            if (_codeEdit == null || string.IsNullOrEmpty(_codeEdit.Text)) return 1;
            return 1 + _codeEdit.Text.Count(c => c == '\n');
        }

        private async void ShowGotoLine()
        {
            GetLineColumn(out int line, out _);
            var dlg = new GoToLineDialog(line, GetTotalLineCount());
            await dlg.ShowDialog(this);
            int? target = dlg.GetLineNumber();
            if (target.HasValue) GotoLine(target.Value);
        }

        private void GotoLine(int lineOneBased)
        {
            if (_codeEdit == null || lineOneBased < 1) return;
            string t = _codeEdit.Text;
            int index = 0;
            for (int L = 1; L < lineOneBased && index < t.Length; L++)
            {
                int next = t.IndexOf('\n', index);
                index = next < 0 ? t.Length : next + 1;
            }
            _codeEdit.SelectionStart = _codeEdit.SelectionEnd = Math.Min(index, t.Length);
            _codeEdit.Focus();
            UpdateStatusBar();
        }

        private void ShowFind()
        {
            string sel = _codeEdit?.SelectedText;
            _findReplaceWidget?.ShowFind(string.IsNullOrEmpty(sel) ? null : sel);
        }

        private void ShowReplace()
        {
            string sel = _codeEdit?.SelectedText;
            _findReplaceWidget?.ShowReplace(string.IsNullOrEmpty(sel) ? null : sel);
        }

        private void OnFindRequested(string findText, bool caseSensitive, bool wholeWords, bool regex)
        {
            if (_codeEdit == null || string.IsNullOrEmpty(findText)) return;
            bool found = _codeEdit.FindNext(findText, caseSensitive, wholeWords, regex, backward: false);
            if (!found) LogToOutput($"Find: no more matches for \"{findText}\"");
        }

        private void OnFindPrevious()
        {
            string findText = _findReplaceWidget?.GetFindText();
            if (_codeEdit == null || string.IsNullOrEmpty(findText)) return;
            bool found = _codeEdit.FindPrevious(findText, _findReplaceWidget.GetCaseSensitive(), _findReplaceWidget.GetWholeWords(), _findReplaceWidget.GetRegex());
            if (!found) LogToOutput($"Find: no more matches for \"{findText}\"");
        }

        private void OnFindNext()
        {
            string findText = _findReplaceWidget?.GetFindText();
            if (_codeEdit == null || string.IsNullOrEmpty(findText)) return;
            OnFindRequested(findText, _findReplaceWidget.GetCaseSensitive(), _findReplaceWidget.GetWholeWords(), _findReplaceWidget.GetRegex());
        }

        private void OnReplaceRequested(string findText, string replaceText, bool caseSensitive, bool wholeWords, bool regex)
        {
            if (_codeEdit == null || string.IsNullOrEmpty(findText)) return;
            int pos = _codeEdit.SelectionStart;
            int len = _codeEdit.SelectionEnd - pos;
            if (pos >= 0 && len > 0 && _codeEdit.Text.Substring(pos, len) == findText)
            {
                _codeEdit.Text = _codeEdit.Text.Remove(pos, len).Insert(pos, replaceText ?? "");
                _codeEdit.SelectionStart = _codeEdit.SelectionEnd = pos + (replaceText?.Length ?? 0);
                MarkDocumentDirty();
            }
            OnFindNext();
        }

        private void OnReplaceAllRequested(string findText, string replaceText, bool caseSensitive, bool wholeWords, bool regex)
        {
            if (_codeEdit == null || string.IsNullOrEmpty(findText)) return;
            int count = _codeEdit.ReplaceAllOccurrences(findText, replaceText ?? "", caseSensitive, wholeWords, regex);
            MarkDocumentDirty();
            LogToOutput($"Replace all: {count} occurrence(s) replaced.");
        }

        private void UpdateBreadcrumbs()
        {
            if (_breadcrumbs == null || _codeEdit == null) return;
            GetLineColumn(out int line, out _);
            string func = GetCurrentFunctionContext();
            var path = new List<string> { "script" };
            if (!string.IsNullOrEmpty(func)) path.Add(func);
            path.Add($"Ln {line}");
            _breadcrumbs.SetPath(path);
        }

        private string GetCurrentFunctionContext()
        {
            if (_codeEdit == null || string.IsNullOrEmpty(_codeEdit.Text)) return "";
            int pos = Math.Min(_codeEdit.SelectionStart, _codeEdit.SelectionEnd);
            if (pos <= 0) return "";
            string t = _codeEdit.Text;
            int lastFunc = -1;
            for (int i = 0; i < pos && i < t.Length; i++)
            {
                if (i + 4 < t.Length && t.Substring(i, 4) == "void" && (i == 0 || !char.IsLetterOrDigit(t[i - 1])))
                {
                    int j = i + 4;
                    while (j < t.Length && char.IsWhiteSpace(t[j])) j++;
                    int start = j;
                    while (j < t.Length && (char.IsLetterOrDigit(t[j]) || t[j] == '_')) j++;
                    if (j > start) lastFunc = start;
                }
            }
            if (lastFunc < 0) return "";
            int end = lastFunc;
            while (end < t.Length && (char.IsLetterOrDigit(t[end]) || t[end] == '_')) end++;
            return t.Substring(lastFunc, end - lastFunc);
        }

        private void TriggerSuggest()
        {
            var list = _functions.Keys.Concat(new[] { "int", "void", "float", "string", "object", "vector" }).OrderBy(x => x).ToList();
            _completer.SetModel(list);
            _completer.SetCompletionPrefix(GetWordBeforeCaret());
            var (line, col) = (0, 0);
            GetLineColumn(out line, out col);
            double y = line * 16.0;
            _completer.Complete(new Rect(0, y, 200, 20));
        }

        private string GetWordBeforeCaret()
        {
            if (_codeEdit == null) return "";
            int pos = _codeEdit.SelectionStart;
            string t = _codeEdit.Text;
            if (pos <= 0 || pos > t.Length) return "";
            int start = pos - 1;
            while (start >= 0 && (char.IsLetterOrDigit(t[start]) || t[start] == '_')) start--;
            start++;
            return t.Substring(start, pos - start);
        }

        private void CompileCurrentScript()
        {
            if (_codeEdit == null) return;
            string source = _codeEdit.Text ?? "";
            string path = _installation?.Path ?? "";
            LogToOutput("Compiling...");
            byte[] result = ScriptCompiler.HtCompileScript(source, path, _isTsl, LogToOutput);
            if (result != null && result.Length > 0)
            {
                LogToOutput("Compile succeeded.");
                RefreshDisassembly(result);
                DialogHelper.ShowWindow(this, "Compile", "Compilation succeeded.", IconType.Info);
            }
            else
            {
                LogToOutput("Compile failed (see messages above).");
                DialogHelper.ShowWindow(this, "Compile", "Compilation failed. Check Output panel.", IconType.Error);
            }
        }

        private void FormatDocument()
        {
            if (_codeEdit == null) return;
            string formatted = NssFormatHelper.FormatDocument(_codeEdit.Text ?? "", IndentString);
            _codeEdit.Text = formatted;
            MarkDocumentDirty();
            LogToOutput("Document formatted.");
        }

        private void AnalyzeCode()
        {
            _problemDiagnostics.Clear();
            if (_codeEdit == null || string.IsNullOrEmpty(_codeEdit.Text)) { RefreshProblemsList(); return; }
            string text = _codeEdit.Text;
            int openB = 0, openP = 0;
            bool inStr = false, inComment = false;
            int line = 1;
            for (int i = 0; i < text.Length; i++)
            {
                if (inComment) { if (text[i] == '\n') { inComment = false; line++; } continue; }
                if (i + 1 < text.Length && text[i] == '/' && text[i + 1] == '/') { inComment = true; i++; continue; }
                if (text[i] == '"') { inStr = !inStr; continue; }
                if (inStr) continue;
                if (text[i] == '\n') line++;
                if (text[i] == '{') openB++;
                if (text[i] == '}') { openB--; if (openB < 0) _problemDiagnostics.Add((line, true, "Unmatched '}'")); }
                if (text[i] == '(') openP++;
                if (text[i] == ')') { openP--; if (openP < 0) _problemDiagnostics.Add((line, true, "Unmatched ')'")); }
            }
            if (openB > 0) _problemDiagnostics.Add((line, true, $"Unmatched '{{' ({openB})"));
            if (openP > 0) _problemDiagnostics.Add((line, true, $"Unmatched '(' ({openP})"));
            RefreshProblemsList();
            LogToOutput($"Analysis: {_problemDiagnostics.Count} diagnostic(s).");
        }

        internal void SetSourceTextForTest(string source)
        {
            if (_codeEdit == null)
            {
                return;
            }

            string next = source ?? "";
            bool changed = next != (_codeEdit.Text ?? "");
            _codeEdit.Text = next;
            if (changed && !_loadingDocument)
            {
                MarkDocumentDirty();
            }
            UpdateStatusBar();
            UpdateBreadcrumbs();
        }

        internal void FormatDocumentForTest()
        {
            FormatDocument();
        }

        internal void AnalyzeCodeForTest()
        {
            AnalyzeCode();
        }

        private void RefreshProblemsList()
        {
            if (_problemsList == null) return;
            _problemsList.ItemsSource = _problemDiagnostics.Select(d => new ProblemItem { Line = d.Line, IsError = d.IsError, Message = d.Message }).ToList();
        }

        private void LogToOutput(string message)
        {
            if (_outputBox == null) return;
            _outputBox.Text = (_outputBox.Text ?? "") + message + Environment.NewLine;
        }

        private void RefreshDisassembly(byte[] ncsBytes)
        {
            if (_disassemblyBox == null)
            {
                return;
            }

            _disassemblyBox.Text = Scripts.DisassembleNcsBytes(ncsBytes);
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            string text = "";
            if (restype == ResourceType.NCS)
            {
                try
                {
                    string installPath = _installation?.Path ?? "";
                    text = ScriptDecompiler.HtDecompileScript(data, installPath, _installation?.Tsl ?? false);
                }
                catch (Exception ex)
                {
                    text = "// Decompile failed: " + ex.Message;
                    LogToOutput("Decompilation failed: " + ex.Message);
                }

                RefreshDisassembly(data);
            }
            else
            {
                text = data != null ? Encoding.UTF8.GetString(data) : "";
                RefreshDisassembly(null);
            }
            _loadingDocument = true;
            try
            {
                if (_codeEdit != null) _codeEdit.Text = text;
                _isTsl = _installation?.Tsl ?? false;
                _highlighter?.UpdateRules(_isTsl);
                LoadGameFunctions();
                UpdateStatusBar();
            }
            finally
            {
                _loadingDocument = false;
            }
        }

        public override void New()
        {
            base.New();
            _loadingDocument = true;
            try
            {
                if (_codeEdit != null) _codeEdit.Text = "";
                _problemDiagnostics.Clear();
                RefreshProblemsList();
                RefreshDisassembly(null);
                UpdateStatusBar();
            }
            finally
            {
                _loadingDocument = false;
            }
            ClearDirty();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            string text = _codeEdit?.Text ?? "";
            return Tuple.Create(Encoding.UTF8.GetBytes(text), (byte[])null);
        }

        public override void SaveAs()
        {
            _ = DoSaveAsAsync();
        }

        private async System.Threading.Tasks.Task DoSaveAsAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            var filters = new List<FilePickerFileType> { new FilePickerFileType("NSS Script") { Patterns = new[] { "*.nss" } } };
            var file = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions { FileTypeChoices = filters, SuggestedFileName = (_resname ?? "script") + ".nss" });
            if (file == null) return;
            try
            {
                string path = file.Path.LocalPath;
                File.WriteAllText(path, _codeEdit?.Text ?? "", Encoding.UTF8);
                _filepath = path;
                _resname = Path.GetFileNameWithoutExtension(path);
                _restype = ResourceType.NSS;
                _revert = Encoding.UTF8.GetBytes(_codeEdit?.Text ?? "");
                ClearDirty();
                RefreshWindowTitle();
            }
            catch (Exception ex)
            {
                await DialogHelper.ShowWindowAsync(this, "Save As", "Error: " + ex.Message, ButtonEnum.Ok, IconType.Error);
            }
        }

        protected override async System.Threading.Tasks.Task RunOpenAsync()
        {
            try
            {
                var topLevel = GetTopLevel(this);
                var storageProvider = topLevel?.StorageProvider ?? (this as Window)?.StorageProvider;
                if (storageProvider == null) return;
                var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    FileTypeFilter = new[] { new FilePickerFileType("NSS/NCS") { Patterns = new[] { "*.nss", "*.ncs" } } },
                    AllowMultiple = false
                });
                if (files == null || files.Count == 0) return;
                string path = files[0].Path.LocalPath;
                byte[] data = File.ReadAllBytes(path);
                string resname = Path.GetFileNameWithoutExtension(path);
                ResourceType restype = Path.GetExtension(path).Equals(".ncs", StringComparison.OrdinalIgnoreCase) ? ResourceType.NCS : ResourceType.NSS;
                Load(path, resname, restype, data);
            }
            catch (Exception ex)
            {
                LogToOutput("Open failed: " + ex.Message);
            }
        }

        private class ProblemItem
        {
            public int Line { get; set; }
            public bool IsError { get; set; }
            public string Message { get; set; }
            public override string ToString() => $"Line {Line}: {(IsError ? "Error" : "Warning")} - {Message}";
        }
    }
}
