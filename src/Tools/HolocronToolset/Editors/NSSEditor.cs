using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Andastra.Parsing.Resource;
using HolocronToolset.Common;
using HolocronToolset.Common.Widgets;
using HolocronToolset.Data;
using HolocronToolset.Utils;
using HolocronToolset.Widgets;
using System.Text.Json;

namespace HolocronToolset.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:115
    // Original: class NSSEditor(Editor):
    public class NSSEditor : Editor
    {
        private CodeEditor _codeEdit;
        private TerminalWidget _terminalWidget;
        private bool _isDecompiled;
        private NoScrollEventFilter _noScrollFilter;
        private FindReplaceWidget _findReplaceWidget;
        private TreeView _bookmarkTree;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:119-199
        // Original: def __init__(self, parent: QWidget | None = None, installation: HTInstallation | None = None):
        public NSSEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Script Editor", "script",
                new[] { ResourceType.NSS, ResourceType.NCS },
                new[] { ResourceType.NSS, ResourceType.NCS },
                installation)
        {
            _installation = installation;
            _isDecompiled = false;

            InitializeComponent();
            SetupUI();
            SetupTerminal();
            SetupSignals();
            SetupFindReplaceWidget();
            SetupBookmarks();
            AddHelpAction();

            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:145-148
            // Original: Setup scrollbar event filter to prevent scrollbar interaction with controls
            // Setup scrollbar event filter to prevent scrollbar interaction with controls
            _noScrollFilter = new NoScrollEventFilter();
            _noScrollFilter.SetupFilter(this);

            // Set Content after AddHelpAction (which may wrap it in a DockPanel)
            if (Content == null && _codeEdit != null)
            {
                Content = _codeEdit;
            }

            New();
        }

        private void InitializeComponent()
        {
            bool xamlLoaded = false;
            try
            {
                AvaloniaXamlLoader.Load(this);
                xamlLoaded = true;

                // Try to find code editor from XAML
                _codeEdit = this.FindControl<CodeEditor>("codeEdit");
            }
            catch
            {
                // XAML not available - will use programmatic UI
            }

            if (!xamlLoaded)
            {
                SetupUI();
            }
        }

        private void SetupUI()
        {
            // Create code editor if not found from XAML
            if (_codeEdit == null)
            {
                _codeEdit = new CodeEditor();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py
        // Original: Terminal widget setup for NSS editor
        // Based on industry-standard IDE patterns: integrated terminal for script execution and debugging
        private void SetupTerminal()
        {
            // Initialize terminal widget for script execution and debugging
            // Terminal allows users to run scripts, view compilation output, and execute commands
            _terminalWidget = new TerminalWidget();

            // Terminal widget is available but not automatically visible
            // Users can toggle terminal visibility via View menu or keyboard shortcut
            // This follows common IDE patterns (VS Code, Visual Studio, etc.)
        }

        // Public property to access terminal widget for testing and external access
        // Based on Avalonia control access patterns and testability best practices
        public TerminalWidget TerminalWidget => _terminalWidget;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:149
        // Original: def _setup_signals(self):
        private void SetupSignals()
        {
            // TODO: STUB - Signals setup - will be implemented as needed
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:3060-3117
        // Original: def _setup_find_replace_widget(self):
        private void SetupFindReplaceWidget()
        {
            // Create find/replace widget
            _findReplaceWidget = new FindReplaceWidget();

            // Connect signals/events
            _findReplaceWidget.FindRequested += OnFindRequested;
            _findReplaceWidget.FindNextRequested += OnFindNextRequested;
            _findReplaceWidget.FindPreviousRequested += OnFindPreviousRequested;
            _findReplaceWidget.ReplaceRequested += OnReplaceRequested;
            _findReplaceWidget.ReplaceAllRequested += OnReplaceAllRequested;
            _findReplaceWidget.CloseRequested += OnFindReplaceClose;

            // Initially hidden
            _findReplaceWidget.IsVisible = false;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:3199-3210
        // Original: def _on_find_requested(self, text: str | None = "", case_sensitive: bool = False, whole_words: bool = False, regex: bool = False):
        private void OnFindRequested(string text, bool caseSensitive, bool wholeWords, bool regex)
        {
            // TODO: STUB - Implement find functionality in CodeEditor
            // This will be implemented when CodeEditor find methods are added
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py
        // Original: Handle find next request
        private void OnFindNextRequested()
        {
            // TODO: STUB - Implement find next functionality
            // This will be implemented when CodeEditor find methods are added
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py
        // Original: Handle find previous request
        private void OnFindPreviousRequested()
        {
            // TODO: STUB - Implement find previous functionality
            // This will be implemented when CodeEditor find methods are added
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:3212-3225
        // Original: def _on_replace_requested(self, find_text: str, replace_text: str, case_sensitive: bool = False, whole_words: bool = False, regex: bool = False):
        private void OnReplaceRequested(string findText, string replaceText, bool caseSensitive, bool wholeWords, bool regex)
        {
            // TODO: STUB - Implement replace functionality in CodeEditor
            // This will be implemented when CodeEditor find/replace methods are added
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:3227-3240
        // Original: def _on_replace_all_requested(self, find_text: str, replace_text: str, case_sensitive: bool = False, whole_words: bool = False, regex: bool = False):
        private void OnReplaceAllRequested(string findText, string replaceText, bool caseSensitive, bool wholeWords, bool regex)
        {
            // TODO: STUB - Implement replace all functionality in CodeEditor
            // This will be implemented when CodeEditor find/replace methods are added
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py
        // Original: Handle find/replace close request
        private void OnFindReplaceClose()
        {
            if (_findReplaceWidget != null)
            {
                _findReplaceWidget.IsVisible = false;
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:2121-2162
        // Original: def load(self, filepath: os.PathLike | str, resref: str, restype: ResourceType, data: bytes | bytearray):
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _isDecompiled = false;
            
            // Reload bookmarks when file is loaded
            LoadBookmarks();

            if (data == null || data.Length == 0)
            {
                if (_codeEdit != null)
                {
                    _codeEdit.SetPlainText("");
                }
                return;
            }

            if (restype == ResourceType.NSS)
            {
                // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:2136-2145
                // Original: Try multiple encodings to properly decode the text
                string text = null;
                try
                {
                    text = Encoding.UTF8.GetString(data);
                }
                catch (DecoderFallbackException)
                {
                    try
                    {
                        text = Encoding.GetEncoding("windows-1252").GetString(data);
                    }
                    catch (DecoderFallbackException)
                    {
                        text = Encoding.GetEncoding("latin-1").GetString(data);
                    }
                }

                if (_codeEdit != null)
                {
                    _codeEdit.SetPlainText(text);
                }
            }
            else if (restype == ResourceType.NCS)
            {
                // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:2146-2156
                // Original: elif restype is ResourceType.NCS:
                bool errorOccurred = false;
                try
                {
                    // Matching PyKotor implementation: self._handle_user_ncs(data, resref)
                    // In full implementation, this would show a dialog asking to decompile or download
                    // TODO: SIMPLIFIED - For now, we'll attempt decompilation directly
                    if (_installation != null)
                    {
                        // Attempt decompilation using DeNCS (matching Python _decompile_ncs_dencs)
                        string source = DecompileNcsDencs(data);
                        if (_codeEdit != null)
                        {
                            _codeEdit.SetPlainText(source);
                        }
                        _isDecompiled = true;
                    }
                    else
                    {
                        errorOccurred = true;
                    }
                }
                catch (Exception ex)
                {
                    // Matching PyKotor implementation: self._handle_exc_debug_mode("Decompilation/Download Failed", e)
                    errorOccurred = true;
                    System.Console.WriteLine($"Decompilation/Download Failed: {ex.Message}");
                }

                if (errorOccurred)
                {
                    New();
                }
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:2196-2246
        // Original: def _decompile_ncs_dencs(self, ncs_data: bytes) -> str:
        private string DecompileNcsDencs(byte[] ncsData)
        {
            // Use ScriptDecompiler to decompile NCS
            if (_installation != null)
            {
                try
                {
                    string decompiled = ScriptDecompiler.HtDecompileScript(ncsData, _installation.Path, _installation.Tsl);
                    if (!string.IsNullOrEmpty(decompiled))
                    {
                        return decompiled;
                    }
                }
                catch (Exception ex)
                {
                    // Decompilation failed - in full implementation would show error dialog
                    // TODO: SIMPLIFIED - For now, return empty string
                    System.Console.WriteLine($"Decompilation failed: {ex.Message}");
                }
            }

            // If decompilation fails, raise ValueError (matching Python behavior)
            throw new InvalidOperationException("Decompilation failed: decompile_ncs returned None");
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:2269-2291
        // Original: def build(self) -> tuple[bytes | bytearray, bytes]:
        public override Tuple<byte[], byte[]> Build()
        {
            if (_codeEdit == null)
            {
                return Tuple.Create(new byte[0], new byte[0]);
            }

            string text = _codeEdit.ToPlainText();

            if (_restype == ResourceType.NCS)
            {
                // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:2281-2291
                // Original: Compile script if restype is NCS
                if (_installation != null)
                {
                    byte[] compiled = ScriptCompiler.HtCompileScript(text, _installation.Path, _installation.Tsl);
                    if (compiled != null && compiled.Length > 0)
                    {
                        return Tuple.Create(compiled, new byte[0]);
                    }
                    // User cancelled compilation
                    return Tuple.Create(new byte[0], new byte[0]);
                }
            }

            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:2270-2279
            // Original: Encode with proper error handling
            byte[] data;
            try
            {
                data = Encoding.UTF8.GetBytes(text);
            }
            catch (EncoderFallbackException)
            {
                try
                {
                    data = Encoding.GetEncoding("windows-1252").GetBytes(text);
                }
                catch (EncoderFallbackException)
                {
                    data = Encoding.GetEncoding("latin-1").GetBytes(text);
                }
            }

            return Tuple.Create(data, new byte[0]);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:2293-2296
        // Original: def new(self):
        public override void New()
        {
            base.New();
            if (_codeEdit != null)
            {
                _codeEdit.SetPlainText("\n\nvoid main()\n{\n    \n}\n");
            }
        }

        public override void SaveAs()
        {
            Save();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:227-232
        // Original: def _setup_bookmarks(self):
        private void SetupBookmarks()
        {
            _bookmarkTree = new TreeView();
            
            // Load bookmarks when editor is initialized
            LoadBookmarks();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:234-253
        // Original: def add_bookmark(self):
        public void AddBookmark()
        {
            if (_codeEdit == null || _bookmarkTree == null)
            {
                return;
            }

            // Get current line number (1-indexed)
            // For TextBox, we need to calculate line number from cursor position
            int lineNumber = GetCurrentLineNumber();
            string defaultDescription = $"Bookmark at line {lineNumber}";

            var item = new TreeViewItem
            {
                Header = $"{lineNumber} - {defaultDescription}",
                Tag = new BookmarkData { LineNumber = lineNumber, Description = defaultDescription }
            };

            if (_bookmarkTree.Items == null)
            {
                _bookmarkTree.Items = new List<TreeViewItem>();
            }

            var itemsList = _bookmarkTree.Items as List<TreeViewItem> ?? new List<TreeViewItem>();
            itemsList.Add(item);
            _bookmarkTree.Items = itemsList;

            SaveBookmarks();
            UpdateBookmarkVisualization();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:255-265
        // Original: def delete_bookmark(self):
        public void DeleteBookmark()
        {
            if (_bookmarkTree == null)
            {
                return;
            }

            var selectedItems = _bookmarkTree.SelectedItems?.Cast<TreeViewItem>().ToList() ?? new List<TreeViewItem>();
            if (selectedItems.Count == 0 && _bookmarkTree.SelectedItem is TreeViewItem selectedItem)
            {
                selectedItems.Add(selectedItem);
            }

            if (selectedItems.Count == 0)
            {
                return;
            }

            var itemsList = _bookmarkTree.Items as List<TreeViewItem> ?? new List<TreeViewItem>();
            foreach (var item in selectedItems)
            {
                if (item != null)
                {
                    itemsList.Remove(item);
                }
            }
            _bookmarkTree.Items = itemsList;

            SaveBookmarks();
            UpdateBookmarkVisualization();
        }

        // Helper method to get current line number from TextBox cursor position
        private int GetCurrentLineNumber()
        {
            if (_codeEdit == null || string.IsNullOrEmpty(_codeEdit.Text))
            {
                return 1;
            }

            int cursorPosition = _codeEdit.SelectionStart;
            string text = _codeEdit.Text;
            
            // Count newlines before cursor position (1-indexed)
            int lineNumber = 1;
            for (int i = 0; i < cursorPosition && i < text.Length; i++)
            {
                if (text[i] == '\n')
                {
                    lineNumber++;
                }
            }

            return lineNumber;
        }

        // Helper method to set cursor to a specific line number (for testing)
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:271-280
        // Original: def _goto_line(self, line_number: int):
        public void GotoLine(int lineNumber)
        {
            if (_codeEdit == null || string.IsNullOrEmpty(_codeEdit.Text))
            {
                return;
            }

            string text = _codeEdit.Text;
            int position = 0;
            int currentLine = 1;

            // Find the position of the specified line (1-indexed)
            for (int i = 0; i < text.Length && currentLine < lineNumber; i++)
            {
                if (text[i] == '\n')
                {
                    currentLine++;
                    position = i + 1;
                }
            }

            // Set cursor position
            _codeEdit.SelectionStart = position;
            _codeEdit.SelectionEnd = position;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:282-303
        // Original: def _save_bookmarks(self):
        private void SaveBookmarks()
        {
            if (_bookmarkTree == null)
            {
                return;
            }

            var bookmarks = new List<BookmarkData>();
            var itemsList = _bookmarkTree.Items as IEnumerable<TreeViewItem> ?? new List<TreeViewItem>();
            
            foreach (var item in itemsList)
            {
                if (item?.Tag is BookmarkData bookmarkData)
                {
                    bookmarks.Add(bookmarkData);
                }
            }

            // Save to application settings (using a simple JSON approach)
            // In a full implementation, this would use Avalonia's settings system
            string fileKey = !string.IsNullOrEmpty(_resname) 
                ? $"nss_editor/bookmarks/{_resname}" 
                : "nss_editor/bookmarks/untitled";
            
            try
            {
                string json = JsonSerializer.Serialize(bookmarks);
                // Store in a simple dictionary for now (would use proper settings in production)
                // This is a simplified implementation for testing
            }
            catch
            {
                // Ignore save errors in test environment
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:305-326
        // Original: def load_bookmarks(self):
        private void LoadBookmarks()
        {
            if (_bookmarkTree == null)
            {
                return;
            }

            string fileKey = !string.IsNullOrEmpty(_resname) 
                ? $"nss_editor/bookmarks/{_resname}" 
                : "nss_editor/bookmarks/untitled";

            // Load from application settings (simplified for testing)
            // In a full implementation, this would use Avalonia's settings system
            try
            {
                // For testing, we'll start with empty bookmarks
                // In production, this would load from persistent storage
                _bookmarkTree.Items = new List<TreeViewItem>();
            }
            catch
            {
                // Ignore load errors in test environment
                _bookmarkTree.Items = new List<TreeViewItem>();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:541-550
        // Original: def _update_bookmark_visualization(self):
        private void UpdateBookmarkVisualization()
        {
            // In a full implementation, this would update visual indicators in the code editor
            // For now, this is a placeholder that matches the Python interface
        }

        // Public property to access bookmark tree for testing
        public TreeView BookmarkTree => _bookmarkTree;

        // Helper class to store bookmark data
        private class BookmarkData
        {
            public int LineNumber { get; set; }
            public string Description { get; set; }
        }
    }
}
