using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.TwoDA;
using OdyTools.Common;
using OdyTools.Data;

namespace OdyTools.Editors
{
    public partial class OdyTool2DA : Editor
    {
        private const int DefaultColumnWidth = 120;
        private const int RowLabelColumnWidth = 52;
        private const int MinColumnWidth = 48;
        private const int MaxInitialWidth = 1400;
        private const int MaxInitialHeight = 900;
        private const int RowHeightEstimate = 26;
        private const int HeaderHeightEstimate = 28;

        private ObservableCollection<ObservableCollection<string>> _sourceData;
        private CollectionViewSource _filteredData;
        private DataGrid _twodaTable;
        private TextBox _filterEdit;
        private TextBox _formulaBarEdit;
        private TextBlock _cellAddressText;
        private Panel _filterBox;
        private VerticalHeaderOption _verticalHeaderOption;
        private string _verticalHeaderColumn;
        private List<string> _columnHeaders;
        private bool _sizeToContentPending;
        private TextBlock _statusText;
        private TextBlock _sidebarStatsText;
        private Border _emptyStateOverlay;
        private Border _sidebarHost;
        private Border _filterSection;
        private bool _isSidebarVisible = true;
        private Button _tbInsertRowSidebar;
        private Button _tbInsertRowAbove;
        private Button _tbInsertRowBelow;
        private MenuItem _actionInsertRow;
        private MenuItem _actionInsertRowAbove;
        private MenuItem _actionInsertRowBelow;
        private MenuItem _ctxInsertRow;
        private MenuItem _ctxInsertRowAbove;
        private MenuItem _ctxInsertRowBelow;

        private const int UndoMaxLevels = 30;
        private readonly List<(List<List<string>> rows, List<string> headers)> _undoStack = new List<(List<List<string>>, List<string>)>();
        private readonly List<(List<List<string>> rows, List<string> headers)> _redoStack = new List<(List<List<string>>, List<string>)>();
        private bool _undoRedoInProgress;

        private string _findText = "";
        private string _replaceText = "";
        private bool _findMatchCase;
        private int _lastFindRow = -1;
        private int _lastFindCol = -1;
        private int _rowDragStartIndex = -1;
        private bool _rowDragArmed;
        private bool _rowDragActive;
        private Avalonia.Point _rowDragStartPoint;
        private bool _columnSelectionActive;
        private static readonly IBrush ColumnHighlightBrush = new SolidColorBrush(Avalonia.Media.Color.Parse("#E3F2FD"));

        /// <summary>
        /// Parameterless constructor required by Avalonia XAML runtime loader (AVLN3001).
        /// </summary>
        public OdyTool2DA() : this(null, null) { }

        public OdyTool2DA(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyTool2DA", "none",
                new[] { ResourceType.TwoDA, ResourceType.TwoDA_CSV, ResourceType.TwoDA_JSON },
                new[] { ResourceType.TwoDA, ResourceType.TwoDA_CSV, ResourceType.TwoDA_JSON },
                installation)
        {
            _sourceData = new ObservableCollection<ObservableCollection<string>>();
            _filteredData = new CollectionViewSource { Source = _sourceData };
            _verticalHeaderOption = VerticalHeaderOption.None;
            _verticalHeaderColumn = "";
            _columnHeaders = new List<string>();

            InitializeComponent();
            SetupUI();
            SetupSignals();
            SetupMenuHandlers();
            RefreshLocalizedStrings();
            New();
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
                // XAML not available (e.g. headless tests)
            }

            if (!xamlLoaded)
            {
                SetupProgrammaticUI();
            }
        }

        private void SetupProgrammaticUI()
        {
            var scroll = new ScrollViewer
            {
                HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
                VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto
            };
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            var filterExpander = new Expander { Header = "Filter", IsExpanded = false };
            _filterEdit = new TextBox { Watermark = "Filter...", Margin = new Avalonia.Thickness(4) };
            filterExpander.Content = _filterEdit;
            _filterBox = null;
            Grid.SetRow(filterExpander, 0);
            grid.Children.Add(filterExpander);

            _twodaTable = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserReorderColumns = true,
                CanUserResizeColumns = true,
                SelectionMode = DataGridSelectionMode.Extended,
                GridLinesVisibility = DataGridGridLinesVisibility.All,
                HeadersVisibility = DataGridHeadersVisibility.All,
                IsReadOnly = false
            };
            Grid.SetRow(_twodaTable, 1);
            grid.Children.Add(_twodaTable);
            scroll.Content = grid;
            Content = scroll;
        }

        private void SetupUI()
        {
            if (_twodaTable != null && _filterEdit != null)
            {
                if (_twodaTable.ItemsSource == null)
                {
                    _twodaTable.ItemsSource = _filteredData.View;
                }
                try
                {
                    var expander = this.FindControl<Expander>("filterBox");
                    _filterBox = expander?.Content as Panel;
                    _sidebarStatsText = this.FindControl<TextBlock>("sidebarStatsText");
                    _sidebarHost = this.FindControl<Border>("sidebarHost");
                    _filterSection = this.FindControl<Border>("filterSection");
                }
                catch { /* No name scope available (headless/programmatic UI) */ }
                return;
            }

            try
            {
                _twodaTable = this.FindControl<DataGrid>("twodaTable");
                _filterEdit = this.FindControl<TextBox>("filterEdit");
                var filterBoxExpander = this.FindControl<Expander>("filterBox");
                if (filterBoxExpander != null)
                    _filterBox = filterBoxExpander.Content as Panel;
                _sidebarStatsText = this.FindControl<TextBlock>("sidebarStatsText");
                _sidebarHost = this.FindControl<Border>("sidebarHost");
                _filterSection = this.FindControl<Border>("filterSection");
                _tbInsertRowSidebar = this.FindControl<Button>("tbInsertRowSidebar");
                _tbInsertRowAbove = this.FindControl<Button>("tbInsertRowAbove");
                _tbInsertRowBelow = this.FindControl<Button>("tbInsertRowBelow");
                _actionInsertRow = this.FindControl<MenuItem>("actionInsertRow");
                _actionInsertRowAbove = this.FindControl<MenuItem>("actionInsertRowAbove");
                _actionInsertRowBelow = this.FindControl<MenuItem>("actionInsertRowBelow");
                _ctxInsertRow = this.FindControl<MenuItem>("ctxInsertRow");
                _ctxInsertRowAbove = this.FindControl<MenuItem>("ctxInsertRowAbove");
                _ctxInsertRowBelow = this.FindControl<MenuItem>("ctxInsertRowBelow");
            }
            catch { }

            if (_twodaTable == null)
            {
                _twodaTable = new DataGrid
                {
                    AutoGenerateColumns = false,
                    CanUserReorderColumns = true,
                    CanUserResizeColumns = true,
                    SelectionMode = DataGridSelectionMode.Extended
                };
            }
            if (_filterEdit == null) _filterEdit = new TextBox { Watermark = "Filter..." };
            if (_filterBox == null) _filterBox = new StackPanel { Orientation = Orientation.Horizontal };

            if (_twodaTable != null && _twodaTable.ItemsSource == null)
            {
                _twodaTable.ItemsSource = _filteredData.View;
            }
        }

        private void SetupSignals()
        {
            if (_filterEdit != null)
            {
                _filterEdit.TextChanged += (s, e) => DoFilter(_filterEdit?.Text ?? "");
            }

            Opened += (s, e) =>
            {
                if (_sizeToContentPending)
                {
                    _sizeToContentPending = false;
                    SizeWindowToContent();
                }
                UpdateStatusBar();
                UpdateInsertRowVisibility();
                _twodaTable?.Focus();
            };

            // Keyboard shortcuts (work when grid or window has focus)
            KeyDown += OnWindowKeyDown;

            // Click outside grid/cell: clear selection
            PointerPressed += OnWindowPointerPressed;

            if (_twodaTable != null)
            {
                _twodaTable.SelectionChanged += OnGridSelectionChanged;
                _twodaTable.CurrentCellChanged += (s, e) => UpdateFormulaBarAndStatus();
                _twodaTable.BeginningEdit += (s, e) => PushState();
                _twodaTable.DoubleTapped += OnGridDoubleTapped;
                _twodaTable.PointerPressed += OnGridPointerPressed;
                _twodaTable.PointerMoved += OnGridPointerMoved;
                _twodaTable.PointerReleased += OnGridPointerReleased;
            }

            try
            {
                _formulaBarEdit = this.FindControl<TextBox>("formulaBarEdit");
                _cellAddressText = this.FindControl<Avalonia.Controls.TextBlock>("cellAddressText");
                if (_formulaBarEdit != null)
                {
                    _formulaBarEdit.KeyDown += (s, e) =>
                    {
                        if (e.Key == Key.Enter)
                        {
                            CommitFormulaBar();
                            e.Handled = true;
                        }
                    };
                    _formulaBarEdit.LostFocus += (s, e) => CommitFormulaBar();
                }
            }
            catch { }
        }

        private void OnWindowPointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (_twodaTable == null || !e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                return;
            // If click was on a cell or column header, let the grid handle it (don't deselect)
            if (TryFindDataGridCell(e.Source) != null || TryFindDataGridColumnHeader(e.Source) != null)
                return;
            // Click was outside any cell/header (sidebar, formula bar, empty grid area, etc.) — clear selection
            ClearGridSelection();
        }

        private void ClearGridSelection()
        {
            if (_twodaTable == null) return;
            _twodaTable.SelectedItems.Clear();
            _twodaTable.SelectedItem = null;
            if (_columnSelectionActive)
            {
                _columnSelectionActive = false;
                ClearColumnHighlight();
            }
            UpdateFormulaBarAndStatus();
            UpdateInsertRowVisibility();
        }

        private void OnGridSelectionChanged(object sender, EventArgs e)
        {
            UpdateFormulaBarAndStatus();
            UpdateInsertRowVisibility();
        }

        /// <summary>Show "Insert Row" only when no row/cell is selected; show "Insert Row Above/Below" only when a cell is selected.</summary>
        private void UpdateInsertRowVisibility()
        {
            bool hasSelection = _twodaTable != null &&
                _twodaTable.SelectedItem != null &&
                (_twodaTable.SelectedItems?.Count ?? 0) > 0;
            bool noSelection = !hasSelection;

            if (_tbInsertRowSidebar != null)
                _tbInsertRowSidebar.IsVisible = noSelection;
            if (_actionInsertRow != null)
                _actionInsertRow.IsVisible = noSelection;
            if (_ctxInsertRow != null)
                _ctxInsertRow.IsVisible = noSelection;

            if (_tbInsertRowAbove != null)
                _tbInsertRowAbove.IsVisible = hasSelection;
            if (_tbInsertRowBelow != null)
                _tbInsertRowBelow.IsVisible = hasSelection;
            if (_actionInsertRowAbove != null)
                _actionInsertRowAbove.IsVisible = hasSelection;
            if (_actionInsertRowBelow != null)
                _actionInsertRowBelow.IsVisible = hasSelection;
            if (_ctxInsertRowAbove != null)
                _ctxInsertRowAbove.IsVisible = hasSelection;
            if (_ctxInsertRowBelow != null)
                _ctxInsertRowBelow.IsVisible = hasSelection;
        }

        private void OnGridDoubleTapped(object sender, TappedEventArgs e)
        {
            var control = e.Source as Control;
            while (control != null)
            {
                if (control is DataGridColumnHeader)
                {
                    // Double-click column header: trigger inline rename via dialog
                    e.Handled = true;
                    _ = RenameColumnAsync();
                    return;
                }
                if (control is DataGridRow || control is DataGridCell)
                    return; // Let DataGrid handle cell editing
                control = control.GetVisualParent() as Control;
            }
            InsertRow();
            e.Handled = true;
        }

        private void OnGridPointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (_twodaTable == null || !e.GetCurrentPoint(_twodaTable).Properties.IsLeftButtonPressed)
            {
                return;
            }
            // Single click on column header: select whole column and highlight all cells in it
            var header = TryFindDataGridColumnHeader(e.Source);
            if (header != null)
            {
                int colIdx = TryGetColumnIndexFromHeader(header);
                if (colIdx >= 0 && colIdx < _twodaTable.Columns.Count)
                {
                    _columnSelectionActive = true;
                    SelectColumnByIndex(colIdx);
                    Avalonia.Threading.Dispatcher.UIThread.Post(ApplyColumnHighlight, Avalonia.Threading.DispatcherPriority.Background);
                }
            }
            else
            {
                // Click on a cell: leave column selection mode and clear column highlight
                var cell = TryFindDataGridCell(e.Source);
                if (cell != null && _columnSelectionActive)
                {
                    _columnSelectionActive = false;
                    ClearColumnHighlight();
                }
            }

            _rowDragStartIndex = TryGetRowIndexFromSource(e.Source);
            if (_rowDragStartIndex < 0)
            {
                _rowDragArmed = false;
                _rowDragActive = false;
                return;
            }
            _rowDragStartPoint = e.GetPosition(_twodaTable);
            _rowDragArmed = true;
            _rowDragActive = false;
        }

        private static DataGridColumnHeader TryFindDataGridColumnHeader(object source)
        {
            var c = source as Control;
            while (c != null)
            {
                if (c is DataGridColumnHeader h) return h;
                c = c.GetVisualParent() as Control;
            }
            return null;
        }

        private static DataGridCell TryFindDataGridCell(object source)
        {
            var c = source as Control;
            while (c != null)
            {
                if (c is DataGridCell cell) return cell;
                c = c.GetVisualParent() as Control;
            }
            return null;
        }

        private static int TryGetColumnIndexFromHeader(DataGridColumnHeader header)
        {
            try
            {
                var prop = header.GetType().GetProperty("ColumnIndex", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (prop != null && prop.PropertyType == typeof(int))
                    return (int)prop.GetValue(header);
            }
            catch { }
            return -1;
        }

        private static int TryGetColumnIndexFromCell(DataGridCell cell)
        {
            try
            {
                var prop = cell.GetType().GetProperty("ColumnIndex", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (prop != null && prop.PropertyType == typeof(int))
                    return (int)prop.GetValue(cell);
            }
            catch { }
            return -1;
        }

        private void SelectColumnByIndex(int columnIndex)
        {
            if (_twodaTable == null || _sourceData.Count == 0) return;
            if (columnIndex < 0 || columnIndex >= _twodaTable.Columns.Count) return;
            SelectAllRows();
            _twodaTable.CurrentColumn = _twodaTable.Columns[columnIndex];
            UpdateFormulaBarAndStatus();
        }

        private void ApplyColumnHighlight()
        {
            if (_twodaTable == null || !_columnSelectionActive) return;
            int targetCol = GetCurrentColumnIndex();
            if (targetCol < 0) return;
            foreach (var row in _twodaTable.GetVisualDescendants().OfType<DataGridRow>())
            {
                foreach (var cell in row.GetVisualDescendants().OfType<DataGridCell>())
                {
                    if (TryGetColumnIndexFromCell(cell) == targetCol)
                        cell.Background = ColumnHighlightBrush;
                }
            }
        }

        private void ClearColumnHighlight()
        {
            if (_twodaTable == null) return;
            foreach (var cell in _twodaTable.GetVisualDescendants().OfType<DataGridCell>())
                cell.Background = Brushes.Transparent;
        }

        private void OnGridPointerMoved(object sender, PointerEventArgs e)
        {
            if (!_rowDragArmed || _twodaTable == null) return;
            var p = e.GetPosition(_twodaTable);
            if (Math.Abs(p.X - _rowDragStartPoint.X) > 6 || Math.Abs(p.Y - _rowDragStartPoint.Y) > 6)
            {
                _rowDragActive = true;
            }
        }

        private void OnGridPointerReleased(object sender, PointerReleasedEventArgs e)
        {
            if (!_rowDragArmed)
            {
                return;
            }

            int from = _rowDragStartIndex;
            int to = TryGetRowIndexFromSource(e.Source);
            _rowDragArmed = false;
            _rowDragStartIndex = -1;

            if (!_rowDragActive)
            {
                _rowDragActive = false;
                return;
            }
            _rowDragActive = false;

            if (from < 0 || to < 0 || from == to || from >= _sourceData.Count || to >= _sourceData.Count)
            {
                return;
            }
            MoveRowInternal(from, to);
        }

        private int TryGetRowIndexFromSource(object source)
        {
            var control = source as Control;
            while (control != null)
            {
                if (control is DataGridRow row)
                {
                    var model = row.DataContext as ObservableCollection<string>;
                    if (model != null)
                    {
                        return _sourceData.IndexOf(model);
                    }
                    break;
                }
                control = control.GetVisualParent() as Control;
            }
            return -1;
        }

        private void UpdateFormulaBarAndStatus()
        {
            UpdateFormulaBar();
            UpdateStatusBar();
        }

        private int GetCurrentColumnIndex()
        {
            if (_twodaTable?.CurrentColumn == null) return -1;
            return _twodaTable.Columns.IndexOf(_twodaTable.CurrentColumn);
        }

        private void UpdateFormulaBar()
        {
            try
            {
                if (_cellAddressText != null)
                {
                    int rowIdx = -1, colIdx = GetCurrentColumnIndex();
                    string colName = "";
                    if (_twodaTable?.SelectedItem is ObservableCollection<string> row)
                    {
                        rowIdx = _sourceData.IndexOf(row);
                        if (colIdx >= 1 && colIdx - 1 < _columnHeaders.Count)
                            colName = _columnHeaders[colIdx - 1];
                    }
                    _cellAddressText.Text = rowIdx >= 0 && colIdx >= 0
                        ? $"R{rowIdx}, {(string.IsNullOrEmpty(colName) ? "col" + colIdx : colName)}"
                        : "—";
                }
                if (_formulaBarEdit != null && _twodaTable?.SelectedItem is ObservableCollection<string> selRow)
                {
                    int colIdx = GetCurrentColumnIndex();
                    if (colIdx >= 0 && colIdx < selRow.Count)
                    {
                        _formulaBarEdit.Text = selRow[colIdx] ?? "";
                    }
                }
            }
            catch { }
        }

        private void CommitFormulaBar()
        {
            if (_formulaBarEdit == null) return;
            var row = _twodaTable?.SelectedItem as ObservableCollection<string>;
            if (row == null) return;
            int colIdx = GetCurrentColumnIndex();
            if (colIdx < 0 || colIdx >= row.Count) return;
            string newVal = _formulaBarEdit?.Text ?? "";
            if ((row[colIdx] ?? "") == newVal) return;
            PushState();
            row[colIdx] = newVal;
            UpdateStatusBar();
        }

        private void PushState()
        {
            if (_undoRedoInProgress) return;
            MarkDirty();
            while (_redoStack.Count > 0) _redoStack.RemoveAt(_redoStack.Count - 1);
            var rows = _sourceData.Select(r => r.ToList()).ToList();
            var headers = new List<string>(_columnHeaders);
            _undoStack.Add((rows, headers));
            if (_undoStack.Count > UndoMaxLevels)
            {
                _undoStack.RemoveAt(0);
            }
        }

        private void Undo()
        {
            if (_undoStack.Count == 0) return;
            _undoRedoInProgress = true;
            try
            {
                var (rows, headers) = _undoStack[_undoStack.Count - 1];
                _undoStack.RemoveAt(_undoStack.Count - 1);
                _redoStack.Add((_sourceData.Select(r => r.ToList()).ToList(), new List<string>(_columnHeaders)));
                ApplyState(rows, headers);
            }
            finally
            {
                _undoRedoInProgress = false;
            }
        }

        private void Redo()
        {
            if (_redoStack.Count == 0) return;
            _undoRedoInProgress = true;
            try
            {
                var (rows, headers) = _redoStack[_redoStack.Count - 1];
                _redoStack.RemoveAt(_redoStack.Count - 1);
                _undoStack.Add((_sourceData.Select(r => r.ToList()).ToList(), new List<string>(_columnHeaders)));
                ApplyState(rows, headers);
            }
            finally
            {
                _undoRedoInProgress = false;
            }
        }

        private void ApplyState(List<List<string>> rows, List<string> headers)
        {
            _sourceData.Clear();
            foreach (var r in rows)
            {
                _sourceData.Add(new ObservableCollection<string>(r));
            }
            _columnHeaders.Clear();
            _columnHeaders.AddRange(headers);
            RebuildGridColumns();
            UpdateFormulaBarAndStatus();
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            var mod = e.KeyModifiers;
            bool ctrl = (mod & KeyModifiers.Control) != 0;
            bool shift = (mod & KeyModifiers.Shift) != 0;
            bool alt = (mod & KeyModifiers.Alt) != 0;

            if (ctrl)
            {
                if (e.Key == Key.N) { _ = TryNewAsync(); e.Handled = true; }
                else if (e.Key == Key.O) { _ = TryOpenAsync(); e.Handled = true; }
                else if (e.Key == Key.S && shift) { _ = RunSaveAsAsync(null); e.Handled = true; }
                else if (e.Key == Key.S) { Save(); e.Handled = true; }
                else if (e.Key == Key.C) { CopySelection(); e.Handled = true; }
                else if (e.Key == Key.X) { CutSelection(); e.Handled = true; }
                else if (e.Key == Key.V) { PasteSelection(); e.Handled = true; }
                else if (e.Key == Key.A) { SelectAllRows(); e.Handled = true; }
                else if (e.Key == Key.Z) { Undo(); e.Handled = true; }
                else if (e.Key == Key.Y) { Redo(); e.Handled = true; }
                else if (e.Key == Key.F) { ShowFindDialog(); e.Handled = true; }
                else if (e.Key == Key.H) { ShowReplaceDialog(); e.Handled = true; }
                else if (e.Key == Key.G) { ShowGoToRowDialog(); e.Handled = true; }
                else if (e.Key == Key.D) { FillDown(); e.Handled = true; }
                else if (e.Key == Key.L) { _filterEdit?.Focus(); e.Handled = true; }
                else if (e.Key == Key.B) { ToggleSidebar(); e.Handled = true; }
                else if (shift && e.Key == Key.Left) { MoveCurrentColumnLeft(); e.Handled = true; }
                else if (shift && e.Key == Key.Right) { MoveCurrentColumnRight(); e.Handled = true; }
                // Ctrl+Home: jump to first data cell (row 0, col 1)
                else if (e.Key == Key.Home) { NavigateToCell(0, 1); e.Handled = true; }
                // Ctrl+End: jump to last data cell
                else if (e.Key == Key.End)
                {
                    int lastRow = Math.Max(0, _sourceData.Count - 1);
                    int lastCol = Math.Max(0, GetEffectiveColumnCount() - 1);
                    NavigateToCell(lastRow, lastCol);
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.F9) { ToggleSidebar(); e.Handled = true; }
            else if (e.Key == Key.F2) { _twodaTable?.BeginEdit(); e.Handled = true; }
            else if (e.Key == Key.F3) { FindNextMatch(); e.Handled = true; }
            else if (e.Key == Key.Escape)
            {
                // Escape: clear filter if focused, otherwise return focus to grid
                if (_filterEdit != null && _filterEdit.IsFocused)
                {
                    _filterEdit.Text = "";
                    DoFilter("");
                }
                _twodaTable?.Focus();
                e.Handled = true;
            }
            else if (alt && e.Key == Key.Up) { MoveSelectedRowsUp(); e.Handled = true; }
            else if (alt && e.Key == Key.Down) { MoveSelectedRowsDown(); e.Handled = true; }
            else if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                if (shift && e.Key == Key.Delete)
                    RemoveSelectedRows();
                else
                    ClearCell();
                e.Handled = true;
            }
            // Navigation: Tab, Enter, Home, End, PgUp, PgDn, and arrows (when Shift is NOT held)
            // When Shift+Arrow is pressed, let DataGrid handle native selection extension
            else if (e.Key == Key.Tab || e.Key == Key.Enter ||
                     e.Key == Key.Home || e.Key == Key.End ||
                     e.Key == Key.PageUp || e.Key == Key.PageDown ||
                     (!shift && (e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right)))
            {
                var grid = _twodaTable;
                if (grid != null && _sourceData?.Count > 0 && grid.SelectedItem is ObservableCollection<string> collection)
                {
                    int rowIdx = _sourceData.IndexOf(collection);
                    int colCount = GetEffectiveColumnCount();
                    int colIdx = GetCurrentColumnIndex();
                    if (colIdx < 0) colIdx = 0;

                    if (e.Key == Key.Tab)
                    {
                        bool shiftMod = (e.KeyModifiers & KeyModifiers.Shift) != 0;
                        if (shiftMod) { colIdx--; if (colIdx < 0) { colIdx = colCount - 1; rowIdx--; } }
                        else { colIdx++; if (colIdx >= colCount) { colIdx = 0; rowIdx++; } }
                    }
                    else if (e.Key == Key.Enter)
                    {
                        rowIdx++; colIdx = GetCurrentColumnIndex(); if (colIdx < 0) colIdx = 0;
                        if (rowIdx >= _sourceData.Count) rowIdx = 0;
                    }
                    else if (e.Key == Key.Home) { colIdx = 1; } // First data column
                    else if (e.Key == Key.End) { colIdx = colCount - 1; } // Last data column
                    else if (e.Key == Key.PageUp) { rowIdx = Math.Max(0, rowIdx - 20); }
                    else if (e.Key == Key.PageDown) { rowIdx = Math.Min(_sourceData.Count - 1, rowIdx + 20); }
                    else if (e.Key == Key.Up) { rowIdx--; if (rowIdx < 0) rowIdx = 0; }
                    else if (e.Key == Key.Down) { rowIdx++; if (rowIdx >= _sourceData.Count) rowIdx = _sourceData.Count - 1; }
                    else if (e.Key == Key.Left) { colIdx--; if (colIdx < 0) colIdx = 0; }
                    else if (e.Key == Key.Right) { colIdx++; if (colIdx >= colCount) colIdx = colCount - 1; }

                    NavigateToCell(rowIdx, colIdx);
                    e.Handled = true;
                }
            }
        }

        /// <summary>Navigates to a specific cell by row and column index, updating selection, scroll, and status.</summary>
        private void NavigateToCell(int rowIdx, int colIdx)
        {
            if (_twodaTable == null || _sourceData == null || _sourceData.Count == 0) return;
            rowIdx = Math.Max(0, Math.Min(rowIdx, _sourceData.Count - 1));
            int colCount = GetEffectiveColumnCount();
            colIdx = Math.Max(0, Math.Min(colIdx, colCount - 1));
            _twodaTable.SelectedItem = _sourceData[rowIdx];
            if (colIdx < _twodaTable.Columns.Count)
            {
                _twodaTable.ScrollIntoView(_sourceData[rowIdx], _twodaTable.Columns[colIdx]);
                if (_twodaTable.CurrentColumn != _twodaTable.Columns[colIdx])
                    _twodaTable.CurrentColumn = _twodaTable.Columns[colIdx];
            }
            else
            {
                _twodaTable.ScrollIntoView(_sourceData[rowIdx], null);
            }
            UpdateFormulaBarAndStatus();
        }

        /// <summary>Selects all rows in the grid (Ctrl+A).</summary>
        public void SelectAllRows()
        {
            if (_twodaTable == null || _sourceData.Count == 0) return;
            _twodaTable.SelectedItems.Clear();
            foreach (var row in _sourceData)
            {
                _twodaTable.SelectedItems.Add(row);
            }
        }

        /// <summary>Selects all rows and focuses the current column for column-wide operations; highlights the whole column.</summary>
        public void SelectCurrentColumn()
        {
            if (_twodaTable == null || _sourceData.Count == 0) return;
            int colIdx = GetCurrentColumnIndex();
            if (colIdx < 0) colIdx = 1;
            int maxDataColIndex = Math.Max(0, _columnHeaders.Count);
            if (_twodaTable.Columns.Count == 0) return;
            if (colIdx > maxDataColIndex) colIdx = maxDataColIndex;
            _columnSelectionActive = true;
            SelectAllRows();
            _twodaTable.CurrentColumn = _twodaTable.Columns[colIdx];
            UpdateFormulaBarAndStatus();
            Avalonia.Threading.Dispatcher.UIThread.Post(ApplyColumnHighlight, Avalonia.Threading.DispatcherPriority.Background);
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
                try
                {
                    var button = this.FindControl<Button>(name);
                    if (button != null) button.Click += (s, e) => handler();
                }
                catch { }
            }

            Bind("actionNew", async () => { if (await ConfirmDiscardUnsavedChangesAsync()) New(); });
            Bind("actionOpen", async () => { if (await ConfirmDiscardUnsavedChangesAsync()) await RunOpenAsync(); });
            Bind("actionSave", () => Save());
            Bind("actionSaveAs", () => _ = RunSaveAsAsync(null));
            Bind("actionSaveAs2DA", () => _ = RunSaveAsAsync(false));
            Bind("actionSaveAsCSV", () => _ = RunSaveAsAsync(true));
            Bind("actionRevert", async () => { if (await ConfirmDiscardUnsavedChangesAsync()) Revert(); });
            Bind("actionExit", () => Close());
            Bind("actionUndo", () => Undo());
            Bind("actionRedo", () => Redo());
            Bind("actionCopy", () => CopySelection());
            Bind("actionCut", () => CutSelection());
            Bind("actionPaste", () => PasteSelection());
            Bind("actionPasteTransposed", () => PasteTransposed());
            Bind("actionClearCell", () => ClearCell());
            Bind("actionFind", () => ShowFindDialog());
            Bind("actionReplace", () => ShowReplaceDialog());
            Bind("actionGoToRow", () => ShowGoToRowDialog());
            Bind("actionSelectAll", () => SelectAllRows());
            Bind("actionSelectColumn", () => SelectCurrentColumn());
            Bind("actionToggleFilter", () => ToggleFilter());
            Bind("actionToggleSidebar", () => ToggleSidebar());
            Bind("actionInsertRow", () => InsertRow());
            Bind("actionInsertRowAbove", () => InsertRowAbove());
            Bind("actionInsertRowBelow", () => InsertRowBelow());
            Bind("actionInsertRows", () => InsertMultipleRows());
            Bind("actionDuplicateRow", () => DuplicateRow());
            Bind("actionRemoveRows", () => RemoveSelectedRows());
            Bind("actionMoveRowUp", () => MoveSelectedRowsUp());
            Bind("actionMoveRowDown", () => MoveSelectedRowsDown());
            Bind("actionFillDown", () => FillDown());
            Bind("actionAddColumn", () => _ = AddColumnAsync());
            Bind("actionRenameColumn", () => _ = RenameColumnAsync());
            Bind("actionRemoveColumn", () => RemoveColumn());
            Bind("actionMoveColumnLeft", () => MoveCurrentColumnLeft());
            Bind("actionMoveColumnRight", () => MoveCurrentColumnRight());
            Bind("actionSortAsc", () => SortRows(ascending: true));
            Bind("actionSortDesc", () => SortRows(ascending: false));
            Bind("actionRedoRowLabels", () => RedoRowLabels());
            Bind("addColumnButton", () => AddColumnQuick());
            Bind("tbInsertRowSidebar", () => InsertRow());
            Bind("tbInsertRowAbove", () => InsertRowAbove());
            Bind("tbInsertRowBelow", () => InsertRowBelow());
            Bind("tbDuplicateRow", () => DuplicateRow());
            Bind("tbRemoveRowsSidebar", () => RemoveSelectedRows());
            Bind("tbMoveRowUpSidebar", () => MoveSelectedRowsUp());
            Bind("tbMoveRowDownSidebar", () => MoveSelectedRowsDown());
            Bind("tbSelectColumn", () => SelectCurrentColumn());
            Bind("tbAddColumnSidebar", () => AddColumnQuick());
            Bind("tbRemoveColumnSidebar", () => RemoveColumn());
            Bind("tbMoveColumnLeftSidebar", () => MoveCurrentColumnLeft());
            Bind("tbMoveColumnRightSidebar", () => MoveCurrentColumnRight());
            Bind("tbSortAscSidebar", () => SortRows(ascending: true));
            Bind("tbSortDescSidebar", () => SortRows(ascending: false));
            Bind("tbClearFilter", () =>
            {
                if (_filterEdit != null)
                {
                    _filterEdit.Text = "";
                    DoFilter("");
                }
            });
            Bind("tbSelectAll", () => SelectAllRows());
            Bind("tbFillDown", () => FillDown());
            Bind("tbGoToRow", () => ShowGoToRowDialog());
            Bind("tbRenameColumn", () => _ = RenameColumnAsync());
            Bind("tbRedoRowLabels", () => RedoRowLabels());

            Bind("ctxCut", () => CutSelection());
            Bind("ctxCopy", () => CopySelection());
            Bind("ctxPaste", () => PasteSelection());
            Bind("ctxPasteTransposed", () => PasteTransposed());
            Bind("ctxClearCell", () => ClearCell());
            Bind("ctxInsertRow", () => InsertRow());
            Bind("ctxInsertRowAbove", () => InsertRowAbove());
            Bind("ctxInsertRowBelow", () => InsertRowBelow());
            Bind("ctxInsertRows", () => InsertMultipleRows());
            Bind("ctxDuplicateRow", () => DuplicateRow());
            Bind("ctxRemoveRows", () => RemoveSelectedRows());
            Bind("ctxMoveRowUp", () => MoveSelectedRowsUp());
            Bind("ctxMoveRowDown", () => MoveSelectedRowsDown());
            Bind("ctxFillDown", () => FillDown());
            Bind("ctxRenameColumn", () => _ = RenameColumnAsync());
            Bind("ctxMoveColumnLeft", () => MoveCurrentColumnLeft());
            Bind("ctxMoveColumnRight", () => MoveCurrentColumnRight());
            Bind("ctxSortAsc", () => SortRows(ascending: true));
            Bind("ctxSortDesc", () => SortRows(ascending: false));

            Bind("actionLangEnglish", () => { Localization.SetLanguage(ToolsetLanguage.English); RefreshLocalizedStrings(); });
            Bind("actionLangFrench", () => { Localization.SetLanguage(ToolsetLanguage.French); RefreshLocalizedStrings(); });
            Bind("actionLangGerman", () => { Localization.SetLanguage(ToolsetLanguage.German); RefreshLocalizedStrings(); });
            Bind("actionLangItalian", () => { Localization.SetLanguage(ToolsetLanguage.Italian); RefreshLocalizedStrings(); });
            Bind("actionLangSpanish", () => { Localization.SetLanguage(ToolsetLanguage.Spanish); RefreshLocalizedStrings(); });
            Bind("actionLangPolish", () => { Localization.SetLanguage(ToolsetLanguage.Polish); RefreshLocalizedStrings(); });
        }

        private void RefreshLocalizedStrings()
        {
            try
            {
                _editorTitle = Localization.Tr("OdyTool2DA");
                RefreshWindowTitle();

                void SetHeader(string name, string key) { var c = EditorHelpers.FindControlSafe<MenuItem>(this, name); if (c != null) c.Header = "_" + Localization.Tr(key); }
                // Top-level menu items by name so language switch always updates them (not by current header text)
                SetHeader("menuFile", "File");
                SetHeader("menuEdit", "Edit");
                SetHeader("menuTools", "Tools");
                SetHeader("menuView", "View");
                SetHeader("menuLanguage", "Language");

                SetHeader("actionNew", "New");
                SetHeader("actionOpen", "Open");
                SetHeader("actionSave", "Save");
                SetHeader("actionSaveAs", "Save As");
                SetHeader("actionSaveAs2DA", "Save As _2DA (binary)...");
                SetHeader("actionSaveAsCSV", "Save As _CSV...");
                SetHeader("actionRevert", "Revert");
                SetHeader("actionExit", "Exit");
                SetHeader("actionUndo", "Undo");
                SetHeader("actionRedo", "Redo");
                SetHeader("actionCut", "Cut");
                SetHeader("actionCopy", "Copy");
                SetHeader("actionPaste", "Paste");
                SetHeader("actionPasteTransposed", "Paste Transposed");
                SetHeader("actionClearCell", "Clear Cell");
                SetHeader("actionFind", "Find");
                SetHeader("actionReplace", "Replace");
                SetHeader("actionGoToRow", "Go to Row...");
                SetHeader("actionSelectAll", "Select All");
                SetHeader("actionSelectColumn", "Select Column");
                SetHeader("actionToggleFilter", "Toggle Filter");
                SetHeader("actionToggleSidebar", "Toggle Sidebar");
                SetHeader("actionInsertRow", "Insert Row");
                SetHeader("actionInsertRowAbove", "Insert Row Above");
                SetHeader("actionInsertRowBelow", "Insert Row Below");
                SetHeader("actionInsertRows", "Insert Multiple Rows...");
                SetHeader("actionDuplicateRow", "Duplicate Row");
                SetHeader("actionRemoveRows", "Remove Rows");
                SetHeader("actionMoveRowUp", "Move Row Up");
                SetHeader("actionMoveRowDown", "Move Row Down");
                SetHeader("actionFillDown", "Fill Down");
                SetHeader("actionAddColumn", "Add Column...");
                SetHeader("actionRenameColumn", "Rename Column...");
                SetHeader("actionRemoveColumn", "Remove Column");
                SetHeader("actionMoveColumnLeft", "Move Column Left");
                SetHeader("actionMoveColumnRight", "Move Column Right");
                SetHeader("actionSortAsc", "Sort A-Z");
                SetHeader("actionSortDesc", "Sort Z-A");
                SetHeader("actionRedoRowLabels", "Redo Row Labels");
                SetHeader("actionRowHeaderMenu", "Set Row Header");
                SetHeader("actionRowHeaderIndex", "Row index");
                SetHeader("actionRowHeaderLabel", "Row label");

                SetHeader("ctxCut", "Cut");
                SetHeader("ctxCopy", "Copy");
                SetHeader("ctxPaste", "Paste");
                SetHeader("ctxPasteTransposed", "Paste Transposed");
                SetHeader("ctxClearCell", "Clear Cell");
                SetHeader("ctxInsertRow", "Insert Row");
                SetHeader("ctxInsertRowAbove", "Insert Row Above");
                SetHeader("ctxInsertRowBelow", "Insert Row Below");
                SetHeader("ctxInsertRows", "Insert Multiple Rows...");
                SetHeader("ctxDuplicateRow", "Duplicate Row");
                SetHeader("ctxRemoveRows", "Remove Rows");
                SetHeader("ctxMoveRowUp", "Move Row Up");
                SetHeader("ctxMoveRowDown", "Move Row Down");
                SetHeader("ctxFillDown", "Fill Down");
                SetHeader("ctxRenameColumn", "Rename Column...");
                SetHeader("ctxMoveColumnLeft", "Move Column Left");
                SetHeader("ctxMoveColumnRight", "Move Column Right");
                SetHeader("ctxSortAsc", "Sort A-Z");
                SetHeader("ctxSortDesc", "Sort Z-A");

                if (_filterEdit != null) _filterEdit.Watermark = Localization.Tr("Search rows...");
                var clearBtn = EditorHelpers.FindControlSafe<Button>(this, "tbClearFilter");
                if (clearBtn != null) { clearBtn.Content = Localization.Tr("Clear"); ToolTip.SetTip(clearBtn, Localization.Tr("Clear search")); }

                var filterSectionTxt = EditorHelpers.FindControlSafe<TextBlock>(this, "filterSectionLabel");
                if (filterSectionTxt != null) filterSectionTxt.Text = Localization.Tr("Search");

                ApplySidebarLocalization();
                UpdateStatusBar();
                if (_emptyStateOverlay != null)
                {
                    var emptyTxt = _emptyStateOverlay.GetVisualDescendants().OfType<TextBlock>().FirstOrDefault();
                    if (emptyTxt != null) emptyTxt.Text = Localization.Tr("No rows — Add rows (Tools → Insert Row) or open a file (File → Open).");
                }
                var addColBtn = EditorHelpers.FindControlSafe<Button>(this, "addColumnButton");
                if (addColBtn != null) ToolTip.SetTip(addColBtn, Localization.Tr("Add column (Ctrl+Z to undo)"));

                if (_formulaBarEdit != null) _formulaBarEdit.Watermark = Localization.Tr("Enter value (Enter to apply)");
            }
            catch { }
        }

        private void ApplySidebarLocalization()
        {
            try
            {
                void SetSidebarBtn(string name, string key, string tooltip)
                {
                    var b = EditorHelpers.FindControlSafe<Button>(this, name);
                    if (b != null) { b.Content = Localization.Tr(key); if (tooltip != null) ToolTip.SetTip(b, Localization.Tr(tooltip)); }
                }

                SetSidebarBtn("tbSelectAll", "Select All", "Select All");
                SetSidebarBtn("tbSelectColumn", "Select Column", "Select current column");
                SetSidebarBtn("tbFillDown", "Fill Down", "Fill Down");
                SetSidebarBtn("tbGoToRow", "Go to Row…", "Go to Row...");
                SetSidebarBtn("tbInsertRowSidebar", "Insert Row", "Insert row");
                SetSidebarBtn("tbInsertRowAbove", "Insert Above", "Insert row above selection");
                SetSidebarBtn("tbInsertRowBelow", "Insert Below", "Insert row below selection");
                SetSidebarBtn("tbDuplicateRow", "Duplicate Row", "Duplicate selected row(s)");
                SetSidebarBtn("tbRemoveRowsSidebar", "Delete Rows", "Remove Rows");
                SetSidebarBtn("tbMoveRowUpSidebar", "Move Row Up", "Move Row Up");
                SetSidebarBtn("tbMoveRowDownSidebar", "Move Row Down", "Move Row Down");
                SetSidebarBtn("tbAddColumnSidebar", "Add Column", "Add column");
                SetSidebarBtn("tbRenameColumn", "Rename Column", "Rename current column");
                SetSidebarBtn("tbRemoveColumnSidebar", "Remove Column", "Remove current column");
                SetSidebarBtn("tbMoveColumnLeftSidebar", "Move Column Left", "Move Column Left");
                SetSidebarBtn("tbMoveColumnRightSidebar", "Move Column Right", "Move Column Right");
                SetSidebarBtn("tbRedoRowLabels", "Regen Row Labels", "Redo Row Labels");
                SetSidebarBtn("tbSortAscSidebar", "A → Z", "Sort ascending");
                SetSidebarBtn("tbSortDescSidebar", "Z → A", "Sort descending");

                // Update sidebar section labels and keyboard help by name so language switch always finds them (not by current text)
                const string keyboardHelpKey = "F2 edit cell · F3 find next · F9 sidebar\nHome/End row · Ctrl+Home/End jump\nPgUp/PgDn scroll · Alt+↑↓ move row\nShift+Arrow extend select · Esc refocus\nCtrl+L search";
                void SetSidebarLabel(string name, string key)
                {
                    var tb = EditorHelpers.FindControlSafe<TextBlock>(this, name);
                    if (tb != null) tb.Text = Localization.Tr(key);
                }
                SetSidebarLabel("sidebarLabelSelection", "SELECTION");
                SetSidebarLabel("sidebarLabelRows", "ROWS");
                SetSidebarLabel("sidebarLabelColumns", "COLUMNS");
                SetSidebarLabel("sidebarLabelSort", "SORT");
                SetSidebarLabel("sidebarLabelKeyboard", "KEYBOARD");
                var keyboardHelp = EditorHelpers.FindControlSafe<TextBlock>(this, "sidebarKeyboardHelp");
                if (keyboardHelp != null) keyboardHelp.Text = Localization.Tr(keyboardHelpKey);
            }
            catch { }
        }

        private int GetEffectiveColumnCount()
        {
            return 1 + _columnHeaders.Count;
        }

        private void RebuildGridColumns()
        {
            if (_twodaTable == null) return;
            _twodaTable.Columns.Clear();
            // Column 0: row label
            _twodaTable.Columns.Add(new DataGridTextColumn
            {
                Header = "#",
                Binding = new Binding("[0]"),
                IsReadOnly = false,
                MinWidth = 40,
                Width = new DataGridLength(RowLabelColumnWidth, DataGridLengthUnitType.Pixel)
            });
            for (int i = 0; i < _columnHeaders.Count; i++)
            {
                int index = i + 1;
                _twodaTable.Columns.Add(new DataGridTextColumn
                {
                    Header = _columnHeaders[i],
                    Binding = new Binding($"[{index}]"),
                    IsReadOnly = false,
                    MinWidth = MinColumnWidth,
                    Width = new DataGridLength(DefaultColumnWidth, DataGridLengthUnitType.Pixel)
                });
            }
        }

        /// <summary>Adds a new column with a default name without showing a dialog.</summary>
        public void AddColumnQuick()
        {
            string baseName = Localization.Tr("NewColumn");
            string name = baseName;
            int n = 1;
            while (_columnHeaders.Contains(name)) name = baseName + n++;
            PushState();
            _columnHeaders.Add(name);
            if (_twodaTable != null)
            {
                int idx = _columnHeaders.Count;
                _twodaTable.Columns.Add(new DataGridTextColumn
                {
                    Header = name,
                    Binding = new Binding($"[{idx}]"),
                    IsReadOnly = false,
                    MinWidth = MinColumnWidth,
                    Width = new DataGridLength(DefaultColumnWidth, DataGridLengthUnitType.Pixel)
                });
            }
            foreach (var row in _sourceData)
            {
                while (row.Count < _columnHeaders.Count + 1) row.Add("");
            }
            UpdateStatusBar();
        }

        /// <summary>Renames a column by index without showing a dialog. Used by double-click header edit and tests.</summary>
        public void RenameColumnByIndex(int colIndex, string newName)
        {
            if (colIndex < 0 || colIndex >= _columnHeaders.Count) return;
            string name = (newName ?? "").Trim();
            if (string.IsNullOrEmpty(name)) return;
            string current = _columnHeaders[colIndex];
            while (_columnHeaders.Contains(name) && name != current) name = name + "_";
            if (name == current) return;
            PushState();
            _columnHeaders[colIndex] = name;
            if (_twodaTable != null && colIndex + 1 < _twodaTable.Columns.Count)
            {
                _twodaTable.Columns[colIndex + 1].Header = name;
            }
            UpdateStatusBar();
        }

        private void SizeWindowToContent()
        {
            try
            {
                int cols = 1 + _columnHeaders.Count;
                int rows = _sourceData?.Count ?? 0;
                double w = Math.Min(MaxInitialWidth, 24 + RowLabelColumnWidth + _columnHeaders.Count * DefaultColumnWidth);
                double h = Math.Min(MaxInitialHeight, 80 + HeaderHeightEstimate + rows * RowHeightEstimate);
                // Always apply on startup so window fits content; enforce minimums
                Width = Math.Max(320, w);
                Height = Math.Max(200, h);
                MinWidth = 320;
                MinHeight = 200;
            }
            catch { }
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            try
            {
                LoadMain(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load file: {ex}");
                New();
            }
            _sizeToContentPending = true;
        }

        private void LoadMain(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                // Clear all data for null/empty input (don't call New() which adds a starter row)
                _sourceData.Clear();
                _columnHeaders.Clear();
                _undoStack.Clear();
                _redoStack.Clear();
                _twodaTable?.Columns.Clear();
                RebuildGridColumns();
                UpdateStatusBar();
                return;
            }
            TwoDA twoda = TwoDAAuto.Read2DA(data);

            _sourceData.Clear();
            _columnHeaders.Clear();
            _undoStack.Clear();
            _redoStack.Clear();
            _twodaTable?.Columns.Clear();

            _columnHeaders.AddRange(twoda.GetHeaders());
            RebuildGridColumns();

            for (int i = 0; i < twoda.GetHeight(); i++)
            {
                var row = new ObservableCollection<string> { twoda.GetLabel(i) ?? "" };
                foreach (var header in twoda.GetHeaders())
                {
                    row.Add(twoda.GetCellString(i, header) ?? "");
                }
                _sourceData.Add(row);
            }

            ResetVerticalHeaders();
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            try
            {
                if (_statusText == null)
                {
                    _statusText = this.FindControl<Avalonia.Controls.TextBlock>("statusText");
                }
                if (_statusText != null)
                {
                    int rows = _sourceData?.Count ?? 0;
                    int cols = 1 + (_columnHeaders?.Count ?? 0);
                    string baseText = Localization.Trf("Ready | {0} rows × {1} columns", rows, cols);
                    int selCount = _twodaTable?.SelectedItems?.Count ?? 0;
                    if (selCount > 1)
                    {
                        baseText += " | " + Localization.Trf("{0} rows selected", selCount);
                    }
                    if (_filteredData?.View?.Filter != null)
                    {
                        int visibleRows = _filteredData.View.Cast<object>().Count();
                        baseText += " | " + Localization.Trf("Visible: {0}", visibleRows);
                    }
                    if (!_isSidebarVisible)
                    {
                        baseText += " | " + Localization.Tr("Sidebar hidden (F9)");
                    }
                    int rowIdx = -1, colIdx = GetCurrentColumnIndex();
                    ObservableCollection<string> selRow = _twodaTable?.SelectedItem as ObservableCollection<string>;
                    if (selRow != null && colIdx >= 0)
                    {
                        rowIdx = _sourceData.IndexOf(selRow);
                        if (rowIdx >= 0)
                        {
                            string colName = (colIdx >= 1 && colIdx - 1 < _columnHeaders.Count) ? _columnHeaders[colIdx - 1] : ("col" + colIdx);
                            baseText += " | " + Localization.Trf("Cell: R{0}, {1}", rowIdx, colName);
                        }
                    }
                    _statusText.Text = baseText;
                }
                if (_sidebarStatsText != null)
                {
                    int rows = _sourceData?.Count ?? 0;
                    int cols = 1 + (_columnHeaders?.Count ?? 0);
                    string stats = Localization.Trf("{0} rows × {1} columns", rows, cols);
                    if (_filteredData?.View?.Filter != null)
                    {
                        int visible = _filteredData.View.Cast<object>().Count();
                        stats = Localization.Trf("{0} of {1} rows", visible, rows);
                    }
                    _sidebarStatsText.Text = stats;
                }

                // Show/hide empty-state overlay (no redundant Copy/Paste; grid is the focus)
                if (_emptyStateOverlay == null)
                {
                    _emptyStateOverlay = this.FindControl<Border>("emptyStateOverlay");
                }
                if (_emptyStateOverlay != null)
                {
                    int rows = _sourceData?.Count ?? 0;
                    _emptyStateOverlay.IsVisible = rows == 0;
                }
            }
            catch { }
        }

        public override Tuple<byte[], byte[]> Build()
        {
            var twoda = new TwoDA();
            var descriptors = GetBuildColumnDescriptors();
            foreach (var descriptor in descriptors)
            {
                twoda.AddColumn(descriptor.header);
            }

            foreach (var row in _sourceData)
            {
                if (row.Count == 0) continue;
                int rowIndex = twoda.AddRow();
                twoda.SetLabel(rowIndex, row[0] ?? "");
                foreach (var descriptor in descriptors)
                {
                    string value = descriptor.sourceIndex < row.Count ? (row[descriptor.sourceIndex] ?? "") : "";
                    twoda.SetCellString(rowIndex, descriptor.header, value);
                }
            }

            ResourceType format = _restype ?? ResourceType.TwoDA;
            byte[] data = TwoDAAuto.Bytes2DA(twoda, format);
            return Tuple.Create(data, new byte[0]);
        }

        private List<(string header, int sourceIndex)> GetBuildColumnDescriptors()
        {
            var descriptors = new List<(string header, int sourceIndex)>();
            if (_twodaTable == null || _twodaTable.Columns == null || _twodaTable.Columns.Count == 0)
            {
                for (int i = 0; i < _columnHeaders.Count; i++)
                {
                    descriptors.Add((_columnHeaders[i], i + 1));
                }
                return descriptors;
            }

            var dataColumns = _twodaTable.Columns
                .OfType<DataGridTextColumn>()
                .Select(col => new
                {
                    column = col,
                    sourceIndex = TryParseBindingIndex(col.Binding as Binding),
                    displayIndex = col.DisplayIndex
                })
                .Where(x => x.sourceIndex > 0 && x.sourceIndex <= _columnHeaders.Count)
                .OrderBy(x => x.displayIndex)
                .ToList();

            foreach (var entry in dataColumns)
            {
                descriptors.Add((_columnHeaders[entry.sourceIndex - 1], entry.sourceIndex));
            }

            if (descriptors.Count == 0 && _sourceData.Count > 0 && _sourceData[0].Count > 1)
            {
                for (int i = 1; i < _sourceData[0].Count; i++)
                {
                    string name = i - 1 < _columnHeaders.Count ? _columnHeaders[i - 1] : $"Column{i}";
                    descriptors.Add((name, i));
                }
            }

            return descriptors;
        }

        private static int TryParseBindingIndex(Binding binding)
        {
            if (binding == null || string.IsNullOrEmpty(binding.Path))
            {
                return -1;
            }
            string path = binding.Path.Trim();
            if (path.Length < 3 || path[0] != '[' || path[path.Length - 1] != ']')
            {
                return -1;
            }
            if (int.TryParse(path.Substring(1, path.Length - 2), out int idx))
            {
                return idx;
            }
            return -1;
        }

        public override void New()
        {
            base.New();
            _restype = ResourceType.TwoDA;
            _sourceData.Clear();
            _columnHeaders.Clear();
            _undoStack.Clear();
            _redoStack.Clear();
            // Pre-existing empty table: one data column and one row so the user can click and type immediately.
            _columnHeaders.Add("Column1");
            _sourceData.Add(new ObservableCollection<string> { "", "" }); // row label + one cell
            RebuildGridColumns();
            if (_twodaTable != null && _sourceData.Count > 0)
            {
                var row = _sourceData[0];
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    if (_twodaTable == null || _sourceData.Count == 0) return;
                    try
                    {
                        _filteredData.View?.Refresh();
                        _twodaTable.SelectedItems.Clear();
                        _twodaTable.SelectedItems.Add(row);
                        _twodaTable.SelectedItem = row;
                        if (_twodaTable.Columns.Count > 1)
                        {
                            _twodaTable.CurrentColumn = _twodaTable.Columns[1];
                            _twodaTable.ScrollIntoView(row, _twodaTable.Columns[1]);
                        }
                    }
                    catch (ArgumentException)
                    {
                        // ItemsSource not ready yet (standalone during init). Skip pre-selection.
                    }
                });
            }
            UpdateStatusBar();
        }

        public override void Revert()
        {
            if (_revert == null) return;
            try
            {
                LoadMain(_revert);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Revert failed: {ex}");
            }
        }

        private async Task TryNewAsync()
        {
            if (await ConfirmDiscardUnsavedChangesAsync()) New();
        }

        private async Task TryOpenAsync()
        {
            if (await ConfirmDiscardUnsavedChangesAsync()) await RunOpenAsync();
        }

        /// <summary>Opens a 2DA file from disk (File → Open). Used by standalone and when opening from toolset.</summary>
        private async Task RunOpenAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            var options = new FilePickerOpenOptions
            {
                Title = "Open 2DA",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("2DA (binary)") { Patterns = new[] { "*.2da" } },
                    new FilePickerFileType("2DA CSV") { Patterns = new[] { "*.2da.csv", "*.csv" } },
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
                if (path.EndsWith(".2da.csv", StringComparison.OrdinalIgnoreCase))
                    ext = "2da.csv";
                ResourceType restype = ResourceType.FromExtension(ext) ?? ResourceType.TwoDA;
                Load(path, resname, restype, data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Open failed: {ex}");
            }
        }

        private async Task RunSaveAsAsync(bool? preferCsv)
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;

            string suggestedName = string.IsNullOrEmpty(_resname) ? "table" : _resname;
            string ext = preferCsv == true ? "2da.csv" : "2da";
            var options = new FilePickerSaveOptions
            {
                Title = "Save 2DA As",
                SuggestedFileName = suggestedName + "." + ext,
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("2DA (binary)") { Patterns = new[] { "*.2da" } },
                    new FilePickerFileType("2DA CSV") { Patterns = new[] { "*.2da.csv", "*.csv" } }
                }
            };

            var file = await storageProvider.SaveFilePickerAsync(options);
            if (file == null) return;

            string path = file.Path.LocalPath;
            if (string.IsNullOrWhiteSpace(path)) return;

            if (path.EndsWith(".csv", StringComparison.OrdinalIgnoreCase) || path.EndsWith(".2da.csv", StringComparison.OrdinalIgnoreCase))
            {
                _restype = ResourceType.TwoDA_CSV;
            }
            else
            {
                _restype = ResourceType.TwoDA;
                if (!path.EndsWith(".2da", StringComparison.OrdinalIgnoreCase))
                {
                    path = path.TrimEnd('.') + ".2da";
                }
            }

            _filepath = path;
            RefreshWindowTitle();
            Save();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync(null);
        }

        public void DoFilter(string text)
        {
            if (_filteredData.View == null) return;
            if (string.IsNullOrEmpty(text))
            {
                _filteredData.View.Filter = null;
            }
            else
            {
                string filterText = text.ToLowerInvariant();
                _filteredData.View.Filter = item =>
                {
                    if (item is ObservableCollection<string> row)
                    {
                        return row.Any(cell => cell?.ToLowerInvariant().Contains(filterText) ?? false);
                    }
                    return false;
                };
            }
            _filteredData.View.Refresh();
        }

        public void ToggleFilter()
        {
            if (_filterSection != null)
            {
                _filterSection.IsVisible = !_filterSection.IsVisible;
                if (_filterSection.IsVisible && _filterEdit != null)
                    _filterEdit.Focus();
            }
            else if (_filterEdit != null)
                _filterEdit.Focus();
        }

        public void ToggleSidebar()
        {
            if (_sidebarHost == null)
            {
                _sidebarHost = this.FindControl<Border>("sidebarHost");
                if (_sidebarHost == null) return;
            }
            _isSidebarVisible = !_isSidebarVisible;
            _sidebarHost.IsVisible = _isSidebarVisible;
            if (!_isSidebarVisible)
            {
                _twodaTable?.Focus();
            }
            UpdateStatusBar();
        }

        /// <summary>Cut selected cell(s): copy selection then clear current column values.</summary>
        public void CutSelection()
        {
            CopySelection();
            ClearCell();
        }

        public void CopySelection()
        {
            try
            {
                var selected = _twodaTable?.SelectedItems?.Cast<ObservableCollection<string>>().ToList();
                if (selected == null || selected.Count == 0) return;

                int colCount = GetEffectiveColumnCount();
                var lines = new List<string>();
                foreach (var row in selected)
                {
                    var cells = new List<string>();
                    for (int i = 0; i < colCount && i < row.Count; i++)
                    {
                        cells.Add(EscapeTsv(row[i] ?? ""));
                    }
                    lines.Add(string.Join("\t", cells));
                }
                var text = string.Join("\n", lines);
                var clipboard = (this as Window)?.Clipboard;
                if (clipboard != null)
                {
                    _ = clipboard.SetTextAsync(text);
                }
            }
            catch { }
        }

        private static string EscapeTsv(string cell)
        {
            if (cell.Contains("\t") || cell.Contains("\r") || cell.Contains("\n"))
            {
                return "\"" + cell.Replace("\"", "\"\"") + "\"";
            }
            return cell;
        }

        public void PasteSelection()
        {
            try
            {
                var clipboard = (this as Window)?.Clipboard;
                if (clipboard == null) return;

                var textTask = clipboard.GetTextAsync();
                textTask.Wait(2000);
                string text = textTask.Result;
                if (string.IsNullOrEmpty(text)) return;

                var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                if (lines.Length == 0) return;

                int insertIndex = _sourceData.Count;
                var selectedIndices = _twodaTable?.SelectedItems != null
                    ? _twodaTable.SelectedItems.Cast<ObservableCollection<string>>()
                        .Select(r => _sourceData.IndexOf(r)).Where(i => i >= 0).ToList()
                    : new List<int>();
                if (selectedIndices.Count > 0)
                {
                    insertIndex = selectedIndices.Min();
                }

                PushState();
                // If table has no data columns and pasted content has multiple columns, use first line as headers
                bool useFirstLineAsHeaders = _columnHeaders.Count == 0 && lines.Length > 0;
                bool lineIsCsv(string line) => line.Contains(",") && !line.Contains("\t");
                bool preferCsv = lines.Length > 0 && lineIsCsv(lines[0]);
                var firstLineCells = useFirstLineAsHeaders ? ParsePasteLine(lines[0], preferCsv) : null;
                if (useFirstLineAsHeaders && firstLineCells != null && firstLineCells.Count > 1)
                {
                    _columnHeaders.Clear();
                    // First cell is row label column; remaining cells are data column headers
                    for (int i = 1; i < firstLineCells.Count; i++)
                    {
                        string name = (firstLineCells[i] ?? "").Trim();
                        if (string.IsNullOrEmpty(name)) name = "Column" + i;
                        while (_columnHeaders.Contains(name)) name = name + "_";
                        _columnHeaders.Add(name);
                    }
                    RebuildGridColumns();
                    foreach (var row in _sourceData)
                    {
                        while (row.Count < _columnHeaders.Count + 1) row.Add("");
                    }
                    // Paste remaining lines as data (skip the header line)
                    for (int lineIndex = 1; lineIndex < lines.Length; lineIndex++)
                    {
                        if (string.IsNullOrEmpty(lines[lineIndex])) continue;
                        var cells = ParsePasteLine(lines[lineIndex], preferCsv);
                        var row = new ObservableCollection<string>();
                        row.Add(cells.Count > 0 ? (cells[0] ?? "") : "");
                        for (int i = 0; i < _columnHeaders.Count; i++)
                        {
                            row.Add(i + 1 < cells.Count ? (cells[i + 1] ?? "") : "");
                        }
                        if (insertIndex <= _sourceData.Count)
                            _sourceData.Insert(insertIndex, row);
                        else
                            _sourceData.Add(row);
                        insertIndex++;
                    }
                }
                else
                {
                    int colCount = GetEffectiveColumnCount();
                    bool pasteAsCsv = lines.Length > 0 && lines[0].Contains(",") && !lines[0].Contains("\t");
                    foreach (var line in lines)
                    {
                        if (string.IsNullOrEmpty(line)) continue;
                        var cells = ParsePasteLine(line, pasteAsCsv);
                        var row = new ObservableCollection<string>();
                        for (int i = 0; i < colCount; i++)
                        {
                            row.Add(i < cells.Count ? cells[i] : "");
                        }
                        EnsureRowColumnCount(row, colCount);
                        if (insertIndex <= _sourceData.Count)
                        {
                            _sourceData.Insert(insertIndex, row);
                        }
                        else
                        {
                            _sourceData.Add(row);
                        }
                        insertIndex++;
                    }
                }

                if (insertIndex > 0)
                {
                    SetItemDisplayData(Math.Min(insertIndex - 1, _sourceData.Count - 1));
                }
                UpdateStatusBar();
            }
            catch { }
        }

        public void PasteTransposed()
        {
            try
            {
                var clipboard = (this as Window)?.Clipboard;
                if (clipboard == null) return;
                var textTask = clipboard.GetTextAsync();
                textTask.Wait(2000);
                string text = textTask.Result;
                if (string.IsNullOrEmpty(text)) return;

                var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                bool preferCsv = lines.Length > 0 && lines[0].Contains(",") && !lines[0].Contains("\t");
                var grid = new List<List<string>>();
                int maxCols = 0;
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    var cells = ParsePasteLine(line, preferCsv);
                    grid.Add(cells);
                    if (cells.Count > maxCols) maxCols = cells.Count;
                }
                if (grid.Count == 0 || maxCols == 0) return;

                for (int r = 0; r < grid.Count; r++)
                {
                    while (grid[r].Count < maxCols) grid[r].Add("");
                }

                int insertRow = _sourceData.Count;
                var selectedIndices = _twodaTable?.SelectedItems != null
                    ? _twodaTable.SelectedItems.Cast<ObservableCollection<string>>()
                        .Select(r => _sourceData.IndexOf(r)).Where(i => i >= 0).ToList()
                    : new List<int>();
                if (selectedIndices.Count > 0)
                    insertRow = selectedIndices.Min();

                PushState();
                int transposedRowCount = maxCols;
                int transposedColCount = grid.Count;
                int existingCols = 1 + _columnHeaders.Count;
                int newColsNeeded = transposedColCount;
                for (int c = 0; c < newColsNeeded; c++)
                {
                    string colName = "Col" + (existingCols + c);
                    while (_columnHeaders.Contains(colName)) colName = colName + "_";
                    _columnHeaders.Add(colName);
                }
                RebuildGridColumns();
                for (int r = 0; r < _sourceData.Count; r++)
                {
                    while (_sourceData[r].Count < _columnHeaders.Count + 1) _sourceData[r].Add("");
                }
                for (int tr = 0; tr < transposedRowCount; tr++)
                {
                    var row = new ObservableCollection<string>();
                    row.Add("");
                    for (int tc = 0; tc < transposedColCount; tc++)
                    {
                        string cell = (tc < grid.Count && tr < grid[tc].Count) ? (grid[tc][tr] ?? "") : "";
                        row.Add(cell);
                    }
                    while (row.Count < _columnHeaders.Count + 1) row.Add("");
                    if (insertRow + tr <= _sourceData.Count)
                        _sourceData.Insert(insertRow + tr, row);
                    else
                        _sourceData.Add(row);
                }
                SetItemDisplayData(Math.Min(insertRow, _sourceData.Count - 1));
                UpdateStatusBar();
            }
            catch { }
        }

        private void EnsureRowColumnCount(ObservableCollection<string> row, int colCount)
        {
            while (row.Count < colCount) row.Add("");
        }

        private static List<string> ParsePasteLine(string line, bool preferCsv = false)
        {
            bool useCsv = preferCsv || (line.Contains(",") && !line.Contains("\t"));
            return useCsv ? ParseCsvLine(line) : ParseTsvLine(line);
        }

        private static List<string> ParseCsvLine(string line)
        {
            var result = new List<string>();
            var current = new System.Text.StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (inQuotes)
                {
                    if (c == '"' && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else if (c == '"')
                    {
                        inQuotes = false;
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
                else
                {
                    if (c == '"') inQuotes = true;
                    else if (c == ',')
                    {
                        result.Add(current.ToString());
                        current.Clear();
                    }
                    else current.Append(c);
                }
            }
            result.Add(current.ToString());
            return result;
        }

        private static List<string> ParseTsvLine(string line)
        {
            var result = new List<string>();
            var current = new System.Text.StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (inQuotes)
                {
                    if (c == '"' && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        current.Append('"');
                        i++;
                    }
                    else if (c == '"')
                    {
                        inQuotes = false;
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
                else
                {
                    if (c == '"') inQuotes = true;
                    else if (c == '\t')
                    {
                        result.Add(current.ToString());
                        current.Clear();
                    }
                    else current.Append(c);
                }
            }
            result.Add(current.ToString());
            return result;
        }

        public void InsertRow()
        {
            PushState();
            int colCount = GetEffectiveColumnCount();
            var newRow = new ObservableCollection<string>();
            for (int i = 0; i < colCount; i++)
            {
                newRow.Add("");
            }
            _sourceData.Add(newRow);
            RedoRowLabels();
            SetItemDisplayData(_sourceData.Count - 1);
            UpdateStatusBar();
        }

        public void InsertMultipleRows()
        {
            var dialog = new Window
            {
                Title = Localization.Tr("Insert Multiple Rows"),
                Width = 320,
                Height = 140,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var label = new TextBlock { Text = Localization.Tr("How many rows to insert?") };
            var textBox = new TextBox { Text = "1" };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var ok = new Button { Content = Localization.Tr("OK"), Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var cancel = new Button { Content = Localization.Tr("Cancel") };
            panel.Children.Add(label);
            panel.Children.Add(textBox);
            buttons.Children.Add(ok);
            buttons.Children.Add(cancel);
            panel.Children.Add(buttons);
            dialog.Content = panel;

            _insertRowsResult = 0;
            ok.Click += (s, e) =>
            {
                if (int.TryParse(textBox.Text?.Trim(), out int count) && count > 0)
                {
                    _insertRowsResult = count;
                }
                dialog.Close();
            };
            cancel.Click += (s, e) => dialog.Close();
            _ = dialog.ShowDialog(this);

            if (_insertRowsResult <= 0) return;
            PushState();
            int insertIndex = _sourceData.Count;
            var selectedIndices = _twodaTable?.SelectedItems != null
                ? _twodaTable.SelectedItems.Cast<ObservableCollection<string>>()
                    .Select(r => _sourceData.IndexOf(r)).Where(i => i >= 0).ToList()
                : new List<int>();
            if (selectedIndices.Count > 0)
            {
                insertIndex = selectedIndices.Max() + 1;
            }

            int colCount = GetEffectiveColumnCount();
            for (int i = 0; i < _insertRowsResult; i++)
            {
                var newRow = new ObservableCollection<string>();
                for (int c = 0; c < colCount; c++) newRow.Add("");
                if (insertIndex + i <= _sourceData.Count)
                    _sourceData.Insert(insertIndex + i, newRow);
                else
                    _sourceData.Add(newRow);
                SetItemDisplayData(insertIndex + i);
            }
            RedoRowLabels();
            SetItemDisplayData(Math.Min(insertIndex, _sourceData.Count - 1));
            UpdateStatusBar();
        }

        public void DuplicateRow()
        {
            if (_twodaTable?.SelectedItem is ObservableCollection<string> selectedRow)
            {
                PushState();
                var newRow = new ObservableCollection<string>(selectedRow);
                _sourceData.Add(newRow);
                RedoRowLabels();
                SetItemDisplayData(_sourceData.Count - 1);
                UpdateStatusBar();
            }
        }

        /// <summary>Insert a new row below the last selected row.</summary>
        public void InsertRowBelow()
        {
            int insertIndex = _sourceData.Count;
            var selectedIndices = _twodaTable?.SelectedItems != null
                ? _twodaTable.SelectedItems.Cast<ObservableCollection<string>>()
                    .Select(r => _sourceData.IndexOf(r)).Where(i => i >= 0).ToList()
                : new List<int>();
            if (selectedIndices.Count > 0)
            {
                insertIndex = selectedIndices.Max() + 1;
            }
            PushState();
            int colCount = GetEffectiveColumnCount();
            var newRow = new ObservableCollection<string>();
            for (int i = 0; i < colCount; i++) newRow.Add("");
            if (insertIndex >= _sourceData.Count)
            {
                _sourceData.Add(newRow);
            }
            else
            {
                _sourceData.Insert(insertIndex, newRow);
            }
            RedoRowLabels();
            SetItemDisplayData(insertIndex);
            UpdateStatusBar();
        }

        private void SetItemDisplayData(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= _sourceData.Count) return;
            if (_sourceData[rowIndex].Count > 0 && string.IsNullOrEmpty(_sourceData[rowIndex][0]))
            {
                _sourceData[rowIndex][0] = rowIndex.ToString();
            }
            ResetVerticalHeaders();
        }

        public void RemoveSelectedRows()
        {
            var selected = _twodaTable?.SelectedItems?.Cast<ObservableCollection<string>>().ToList();
            if (selected == null) return;
            if (selected.Count == 0) return;
            PushState();
            foreach (var item in selected)
            {
                _sourceData.Remove(item);
            }
            RedoRowLabels();
            UpdateStatusBar();
        }

        public void MoveSelectedRowsUp()
        {
            var selected = _twodaTable?.SelectedItems?.Cast<ObservableCollection<string>>()
                .Select(r => _sourceData.IndexOf(r))
                .Where(i => i > 0)
                .Distinct()
                .OrderBy(i => i)
                .ToList();
            if (selected == null || selected.Count == 0) return;
            PushState();
            foreach (int idx in selected)
            {
                var row = _sourceData[idx];
                _sourceData.RemoveAt(idx);
                _sourceData.Insert(idx - 1, row);
            }
            RestoreSelectedRowsByIndices(selected.Select(i => i - 1).ToList());
            RedoRowLabels();
            UpdateStatusBar();
        }

        public void MoveSelectedRowsDown()
        {
            var selected = _twodaTable?.SelectedItems?.Cast<ObservableCollection<string>>()
                .Select(r => _sourceData.IndexOf(r))
                .Where(i => i >= 0 && i < _sourceData.Count - 1)
                .Distinct()
                .OrderByDescending(i => i)
                .ToList();
            if (selected == null || selected.Count == 0) return;
            PushState();
            foreach (int idx in selected)
            {
                var row = _sourceData[idx];
                _sourceData.RemoveAt(idx);
                _sourceData.Insert(idx + 1, row);
            }
            RestoreSelectedRowsByIndices(selected.Select(i => i + 1).ToList());
            RedoRowLabels();
            UpdateStatusBar();
        }

        private void MoveRowInternal(int from, int to)
        {
            if (from < 0 || to < 0 || from >= _sourceData.Count || to >= _sourceData.Count || from == to)
            {
                return;
            }
            PushState();
            var row = _sourceData[from];
            _sourceData.RemoveAt(from);
            if (to > from) to--;
            _sourceData.Insert(to, row);
            RedoRowLabels();
            RestoreSelectedRowsByIndices(new List<int> { to });
            UpdateStatusBar();
        }

        private void RestoreSelectedRowsByIndices(List<int> indices)
        {
            if (_twodaTable == null || indices == null) return;
            _twodaTable.SelectedItems.Clear();
            foreach (int i in indices.Where(i => i >= 0 && i < _sourceData.Count).Distinct())
            {
                _twodaTable.SelectedItems.Add(_sourceData[i]);
            }
            int first = indices.Where(i => i >= 0 && i < _sourceData.Count).DefaultIfEmpty(-1).Min();
            if (first >= 0)
            {
                _twodaTable.SelectedItem = _sourceData[first];
                _twodaTable.ScrollIntoView(_sourceData[first], _twodaTable.CurrentColumn);
            }
        }

        private void ShowFindDialog()
        {
            var dialog = new Window
            {
                Title = Localization.Tr("Find"),
                Width = 360,
                Height = 140,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var findLabel = new TextBlock { Text = Localization.Tr("Find what:") };
            var findBox = new TextBox { Text = _findText, Watermark = Localization.Tr("Search text") };
            var matchCase = new CheckBox { Content = Localization.Tr("Match case"), IsChecked = _findMatchCase };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var findNext = new Button { Content = Localization.Tr("Find Next"), Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var closeBtn = new Button { Content = Localization.Tr("Close") };
            panel.Children.Add(findLabel);
            panel.Children.Add(findBox);
            panel.Children.Add(matchCase);
            buttons.Children.Add(findNext);
            buttons.Children.Add(closeBtn);
            panel.Children.Add(buttons);
            dialog.Content = panel;
            findNext.Click += (s, e) =>
            {
                _findText = findBox.Text ?? "";
                _findMatchCase = matchCase.IsChecked == true;
                if (FindNextMatch())
                {
                    dialog.Close();
                }
            };
            closeBtn.Click += (s, e) => dialog.Close();
            findBox.Focus();
            _ = dialog.ShowDialog(this as Window);
        }

        private void ShowReplaceDialog()
        {
            var dialog = new Window
            {
                Title = Localization.Tr("Replace"),
                Width = 360,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var findLabel = new TextBlock { Text = Localization.Tr("Find what:") };
            var findBox = new TextBox { Text = _findText, Watermark = Localization.Tr("Search text") };
            var replaceLabel = new TextBlock { Text = Localization.Tr("Replace with:") };
            var replaceBox = new TextBox { Text = _replaceText, Watermark = Localization.Tr("Replacement") };
            var matchCase = new CheckBox { Content = Localization.Tr("Match case"), IsChecked = _findMatchCase };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var findNextBtn = new Button { Content = Localization.Tr("Find Next"), Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var replaceBtn = new Button { Content = Localization.Tr("Replace"), Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var replaceAllBtn = new Button { Content = Localization.Tr("Replace All"), Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var closeBtn = new Button { Content = Localization.Tr("Close") };
            panel.Children.Add(findLabel);
            panel.Children.Add(findBox);
            panel.Children.Add(replaceLabel);
            panel.Children.Add(replaceBox);
            panel.Children.Add(matchCase);
            buttons.Children.Add(findNextBtn);
            buttons.Children.Add(replaceBtn);
            buttons.Children.Add(replaceAllBtn);
            buttons.Children.Add(closeBtn);
            panel.Children.Add(buttons);
            dialog.Content = panel;
            findNextBtn.Click += (s, e) =>
            {
                _findText = findBox.Text ?? "";
                _replaceText = replaceBox.Text ?? "";
                _findMatchCase = matchCase.IsChecked == true;
                FindNextMatch();
            };
            replaceBtn.Click += (s, e) =>
            {
                _findText = findBox.Text ?? "";
                _replaceText = replaceBox.Text ?? "";
                _findMatchCase = matchCase.IsChecked == true;
                ReplaceOne();
            };
            replaceAllBtn.Click += (s, e) =>
            {
                _findText = findBox.Text ?? "";
                _replaceText = replaceBox.Text ?? "";
                _findMatchCase = matchCase.IsChecked == true;
                ReplaceAll();
                dialog.Close();
            };
            closeBtn.Click += (s, e) => dialog.Close();
            findBox.Focus();
            _ = dialog.ShowDialog(this as Window);
        }

        private StringComparison FindComparison => _findMatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        private bool FindNextMatch()
        {
            if (string.IsNullOrEmpty(_findText)) return false;
            int startRow = _lastFindRow;
            int startCol = _lastFindCol + 1;
            for (int r = 0; r < _sourceData.Count; r++)
            {
                var row = _sourceData[r];
                int colStart = (r == startRow) ? startCol : 0;
                for (int c = colStart; c < row.Count; c++)
                {
                    string cell = row[c] ?? "";
                    if (cell.IndexOf(_findText, FindComparison) >= 0)
                    {
                        _lastFindRow = r;
                        _lastFindCol = c;
                        SelectAndScrollToCell(r, c);
                        return true;
                    }
                }
            }
            _lastFindRow = -1;
            _lastFindCol = -1;
            return false;
        }

        private void SelectAndScrollToCell(int rowIndex, int colIndex)
        {
            if (rowIndex < 0 || rowIndex >= _sourceData.Count) return;
            var row = _sourceData[rowIndex];
            _twodaTable.SelectedItems.Clear();
            _twodaTable.SelectedItems.Add(row);
            _twodaTable.CurrentColumn = colIndex < _twodaTable.Columns.Count ? _twodaTable.Columns[colIndex] : null;
            _twodaTable.ScrollIntoView(row, _twodaTable.CurrentColumn);
            UpdateFormulaBarAndStatus();
        }

        private void ReplaceOne()
        {
            if (string.IsNullOrEmpty(_findText)) return;
            int r = _lastFindRow;
            int c = _lastFindCol;
            if (r < 0 || r >= _sourceData.Count || c < 0) return;
            var row = _sourceData[r];
            if (c >= row.Count) return;
            string cell = row[c] ?? "";
            int idx = cell.IndexOf(_findText, FindComparison);
            if (idx < 0) { FindNextMatch(); return; }
            PushState();
            row[c] = cell.Remove(idx, _findText.Length).Insert(idx, _replaceText ?? "");
            _lastFindCol = c;
            UpdateFormulaBarAndStatus();
        }

        private void ReplaceAll()
        {
            if (string.IsNullOrEmpty(_findText)) return;
            PushState();
            for (int r = 0; r < _sourceData.Count; r++)
            {
                var row = _sourceData[r];
                for (int c = 0; c < row.Count; c++)
                {
                    string cell = row[c] ?? "";
                    string result = ReplaceAllInString(cell, _findText, _replaceText ?? "", _findMatchCase);
                    if (result != cell)
                    {
                        row[c] = result;
                    }
                }
            }
            _lastFindRow = -1;
            _lastFindCol = -1;
            UpdateStatusBar();
        }

        private static string ReplaceAllInString(string text, string find, string replace, bool matchCase)
        {
            if (string.IsNullOrEmpty(find)) return text;
            var comparison = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int pos = 0;
            var sb = new System.Text.StringBuilder();
            while (pos < text.Length)
            {
                int idx = text.IndexOf(find, pos, comparison);
                if (idx < 0) break;
                sb.Append(text, pos, idx - pos);
                sb.Append(replace);
                pos = idx + find.Length;
            }
            if (pos == 0) return text;
            sb.Append(text, pos, text.Length - pos);
            return sb.ToString();
        }

        public void SortRows(bool ascending)
        {
            int colIdx = GetCurrentColumnIndex();
            if (colIdx < 0) colIdx = 1;
            if (_sourceData.Count == 0) return;
            PushState();

            // Preserve CurrentColumn across the Clear/Add cycle which can reset DataGrid state
            var savedColumn = _twodaTable?.CurrentColumn;

            var list = _sourceData.ToList();
            list.Sort((a, b) =>
            {
                string va = colIdx < a.Count ? (a[colIdx] ?? "").Trim() : "";
                string vb = colIdx < b.Count ? (b[colIdx] ?? "").Trim() : "";
                int cmpStr = string.Compare(va, vb, StringComparison.OrdinalIgnoreCase);
                return ascending ? cmpStr : -cmpStr;
            });
            _sourceData.Clear();
            foreach (var row in list) _sourceData.Add(row);

            // Restore CurrentColumn so subsequent sorts target the same column
            if (savedColumn != null && _twodaTable != null && _sourceData.Count > 0)
            {
                try
                {
                    // DataGrid requires a SelectedItem before setting CurrentColumn
                    if (_twodaTable.SelectedItem == null)
                        _twodaTable.SelectedItem = _sourceData[0];
                    _twodaTable.CurrentColumn = savedColumn;
                }
                catch { /* Headless or no visual tree */ }
            }

            UpdateStatusBar();
        }

        public void FillDown()
        {
            var selected = _twodaTable?.SelectedItems?.Cast<ObservableCollection<string>>().ToList();
            if (selected == null || selected.Count == 0) return;
            int colIdx = GetCurrentColumnIndex();
            if (colIdx < 0) return;
            string value = selected[0].Count > colIdx ? (selected[0][colIdx] ?? "") : "";
            PushState();
            foreach (var row in selected)
            {
                while (row.Count <= colIdx) row.Add("");
                row[colIdx] = value;
            }
            UpdateStatusBar();
        }

        public void ClearCell()
        {
            int colIdx = GetCurrentColumnIndex();
            if (colIdx < 0)
            {
                // Prefer clearing the first data column when no current column is active.
                colIdx = _twodaTable?.Columns?.Count > 1 ? 1 : 0;
            }
            var selectedRows = _twodaTable?.SelectedItems?.Cast<ObservableCollection<string>>().ToList() ?? new List<ObservableCollection<string>>();
            var focusedRow = _twodaTable?.SelectedItem as ObservableCollection<string>;
            if (focusedRow != null && selectedRows.Count == 0)
                selectedRows.Add(focusedRow);
            if (selectedRows.Count == 0) return;
            PushState();
            foreach (var row in selectedRows)
            {
                if (row != null && colIdx < row.Count)
                    row[colIdx] = "";
            }
            if (_formulaBarEdit != null) _formulaBarEdit.Text = "";
            UpdateFormulaBarAndStatus();
        }

        private int _goToRowResult = -1;
        private int _insertRowsResult = 1;

        private void ShowGoToRowDialog()
        {
            var dialog = new Window
            {
                Title = Localization.Tr("Go to Row"),
                Width = 280,
                Height = 120,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var label = new TextBlock { Text = Localization.Tr("Row index (0-based):") };
            var textBox = new TextBox { Watermark = "0" };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var ok = new Button { Content = Localization.Tr("OK"), Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var cancel = new Button { Content = Localization.Tr("Cancel") };
            panel.Children.Add(label);
            panel.Children.Add(textBox);
            buttons.Children.Add(ok);
            buttons.Children.Add(cancel);
            panel.Children.Add(buttons);
            dialog.Content = panel;
            _goToRowResult = -1;
            ok.Click += (s, e) =>
            {
                if (int.TryParse(textBox.Text?.Trim(), out int n) && n >= 0)
                {
                    _goToRowResult = n;
                    dialog.Close();
                }
            };
            cancel.Click += (s, e) => dialog.Close();
            _ = dialog.ShowDialog(this as Window);
            if (_goToRowResult >= 0 && _goToRowResult < _sourceData.Count)
            {
                var targetRow = _sourceData[_goToRowResult];
                _twodaTable.SelectedItems.Clear();
                _twodaTable.SelectedItems.Add(targetRow);
                _twodaTable.ScrollIntoView(targetRow, null);
                UpdateFormulaBarAndStatus();
            }
        }

        private string _renameColumnResult = null;

        public async Task RenameColumnAsync()
        {
            int colIdx = GetCurrentColumnIndex();
            if (colIdx < 1 || colIdx - 1 >= _columnHeaders.Count) return;
            string currentName = _columnHeaders[colIdx - 1];
            var dialog = new Window
            {
                Title = Localization.Tr("Rename Column"),
                Width = 320,
                Height = 120,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var label = new TextBlock { Text = Localization.Tr("Column name:") };
            var textBox = new TextBox { Text = currentName };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var ok = new Button { Content = Localization.Tr("OK"), Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var cancel = new Button { Content = Localization.Tr("Cancel") };
            panel.Children.Add(label);
            panel.Children.Add(textBox);
            buttons.Children.Add(ok);
            buttons.Children.Add(cancel);
            panel.Children.Add(buttons);
            dialog.Content = panel;
            _renameColumnResult = null;
            ok.Click += (s, e) =>
            {
                string name = textBox.Text?.Trim() ?? "";
                if (!string.IsNullOrEmpty(name))
                {
                    while (_columnHeaders.Contains(name) && name != currentName) name = name + "_";
                    _renameColumnResult = name;
                }
                dialog.Close();
            };
            cancel.Click += (s, e) => dialog.Close();
            await dialog.ShowDialog(this as Window);
            if (_renameColumnResult != null)
            {
                PushState();
                _columnHeaders[colIdx - 1] = _renameColumnResult;
                if (_twodaTable != null && colIdx < _twodaTable.Columns.Count)
                {
                    _twodaTable.Columns[colIdx].Header = _renameColumnResult;
                }
                UpdateStatusBar();
            }
        }

        public void InsertRowAbove()
        {
            int insertIndex = _sourceData.Count;
            var selectedIndices = _twodaTable?.SelectedItems != null
                ? _twodaTable.SelectedItems.Cast<ObservableCollection<string>>()
                    .Select(r => _sourceData.IndexOf(r)).Where(i => i >= 0).ToList()
                : new List<int>();
            if (selectedIndices.Count > 0)
            {
                insertIndex = selectedIndices.Min();
            }
            PushState();
            int colCount = GetEffectiveColumnCount();
            var newRow = new ObservableCollection<string>();
            for (int i = 0; i < colCount; i++) newRow.Add("");
            _sourceData.Insert(insertIndex, newRow);
            RedoRowLabels();
            SetItemDisplayData(insertIndex);
            UpdateStatusBar();
        }

        public void RedoRowLabels()
        {
            if (_sourceData.Count == 0) return;
            PushState();
            for (int i = 0; i < _sourceData.Count; i++)
            {
                if (_sourceData[i].Count > 0)
                {
                    _sourceData[i][0] = i.ToString();
                }
            }
            UpdateStatusBar();
        }

        private string _addColumnResult;

        public async Task AddColumnAsync()
        {
            var dialog = new Window
            {
                Title = Localization.Tr("Add Column"),
                Width = 320,
                Height = 120,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var label = new TextBlock { Text = Localization.Tr("Column name:") };
            var textBox = new TextBox { Watermark = Localization.Tr("NewColumn") };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var ok = new Button { Content = Localization.Tr("OK"), Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var cancel = new Button { Content = Localization.Tr("Cancel") };
            panel.Children.Add(label);
            panel.Children.Add(textBox);
            buttons.Children.Add(ok);
            buttons.Children.Add(cancel);
            panel.Children.Add(buttons);
            dialog.Content = panel;

            _addColumnResult = null;
            ok.Click += (s, e) =>
            {
                string name = textBox.Text?.Trim() ?? "";
                if (string.IsNullOrEmpty(name)) name = Localization.Tr("NewColumn");
                while (_columnHeaders.Contains(name)) name = name + "_1";
                _addColumnResult = name;
                dialog.Close();
            };
            cancel.Click += (s, e) => dialog.Close();
            await dialog.ShowDialog(this as Window);
            string columnName = _addColumnResult;
            if (string.IsNullOrEmpty(columnName)) return;

            PushState();
            _columnHeaders.Add(columnName);
            if (_twodaTable != null)
            {
                int idx = _columnHeaders.Count;
                _twodaTable.Columns.Add(new DataGridTextColumn
                {
                    Header = columnName,
                    Binding = new Binding($"[{idx}]"),
                    IsReadOnly = false,
                    MinWidth = MinColumnWidth,
                    Width = new DataGridLength(DefaultColumnWidth, DataGridLengthUnitType.Pixel)
                });
            }
            foreach (var row in _sourceData)
            {
                while (row.Count < _columnHeaders.Count + 1) row.Add("");
            }
            UpdateStatusBar();
        }

        public void RemoveColumn()
        {
            if (_columnHeaders.Count == 0) return;
            PushState();
            int removeAt = _columnHeaders.Count - 1;
            if (_twodaTable?.CurrentColumn != null)
            {
                int colIndex = _twodaTable.Columns.IndexOf(_twodaTable.CurrentColumn);
                if (colIndex > 0 && colIndex <= _columnHeaders.Count)
                {
                    removeAt = colIndex - 1;
                }
            }

            _columnHeaders.RemoveAt(removeAt);
            foreach (var row in _sourceData)
            {
                if (row.Count > removeAt + 1)
                {
                    row.RemoveAt(removeAt + 1);
                }
            }
            RebuildGridColumns();
            UpdateStatusBar();
        }

        public void MoveCurrentColumnLeft()
        {
            int colIdx = GetCurrentColumnIndex();
            int headerIndex = colIdx - 1;
            if (headerIndex <= 0 || headerIndex >= _columnHeaders.Count) return;
            PushState();
            SwapDataColumns(headerIndex, headerIndex - 1);
            RebuildGridColumns();
            if (_twodaTable != null && headerIndex < _twodaTable.Columns.Count)
            {
                _twodaTable.CurrentColumn = _twodaTable.Columns[headerIndex];
            }
            UpdateStatusBar();
        }

        public void MoveCurrentColumnRight()
        {
            int colIdx = GetCurrentColumnIndex();
            int headerIndex = colIdx - 1;
            if (headerIndex < 0 || headerIndex >= _columnHeaders.Count - 1) return;
            PushState();
            SwapDataColumns(headerIndex, headerIndex + 1);
            RebuildGridColumns();
            if (_twodaTable != null && headerIndex + 2 < _twodaTable.Columns.Count)
            {
                _twodaTable.CurrentColumn = _twodaTable.Columns[headerIndex + 2];
            }
            UpdateStatusBar();
        }

        private void SwapDataColumns(int leftHeaderIndex, int rightHeaderIndex)
        {
            if (leftHeaderIndex < 0 || rightHeaderIndex < 0 || leftHeaderIndex >= _columnHeaders.Count || rightHeaderIndex >= _columnHeaders.Count)
            {
                return;
            }
            string tmp = _columnHeaders[leftHeaderIndex];
            _columnHeaders[leftHeaderIndex] = _columnHeaders[rightHeaderIndex];
            _columnHeaders[rightHeaderIndex] = tmp;

            int leftRowIndex = leftHeaderIndex + 1;
            int rightRowIndex = rightHeaderIndex + 1;
            foreach (var row in _sourceData)
            {
                while (row.Count <= Math.Max(leftRowIndex, rightRowIndex))
                {
                    row.Add("");
                }
                string cell = row[leftRowIndex];
                row[leftRowIndex] = row[rightRowIndex];
                row[rightRowIndex] = cell;
            }
        }

        public void SetVerticalHeaderOption(VerticalHeaderOption option, string column = null)
        {
            _verticalHeaderOption = option;
            _verticalHeaderColumn = column ?? "";
            ResetVerticalHeaders();
        }

        private void ResetVerticalHeaders()
        {
            // Row header display can be extended here if needed.
        }
    }

    public enum VerticalHeaderOption
    {
        RowIndex = 0,
        RowLabel = 1,
        CellValue = 2,
        None = 3
    }
}
