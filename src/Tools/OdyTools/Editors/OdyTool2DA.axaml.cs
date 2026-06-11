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
using OdyTools.Editors.TwoDACommands;
using OdyTools.Utils;

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
        private MenuItem _ctxFindRowReferences;

        private const int UndoMaxLevels = 30;

        // Command-based undo/redo (replaces snapshot-based legacy stacks)
        private readonly TwoDACommandStack _commandStack = new TwoDACommandStack(100);

        // Legacy snapshot stacks (deprecated, will gradually migrate operations to command-based)
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
        private bool _cellRangeActive;
        private int _rangeAnchorRow = -1;
        private int _rangeAnchorCol = -1;
        private int _rangeEndRow = -1;
        private int _rangeEndCol = -1;
        private static readonly IBrush ColumnHighlightBrush = new SolidColorBrush(Avalonia.Media.Color.Parse("#E3F2FD"));
        private static readonly IBrush RangeHighlightBrush = new SolidColorBrush(Avalonia.Media.Color.Parse("#FFF9C4"));
        private readonly Dictionary<int, ColumnValidationMode> _columnValidationRules = new Dictionary<int, ColumnValidationMode>();

        // Column filter state
        private int _filterColumnIndex = -1;
        private HashSet<string> _filterAllowedValues = new HashSet<string>();
        private List<ObservableCollection<string>> _allRowsBeforeFilter = new List<ObservableCollection<string>>();
        private bool _isColumnFilterActive = false;

        // View options
        private double _zoomLevel = 1.0;
        private bool _textWrappingEnabled = false;
        private HashSet<int> _hiddenColumnIndices = new HashSet<int>();

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

            // Wire command stack events
            _commandStack.CommandExecuted += (s, cmd) => MarkDocumentDirty();
            _commandStack.Undone += (s, e) => UpdateStatusBar();
            _commandStack.Redone += (s, e) => UpdateStatusBar();

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
                _ctxFindRowReferences = this.FindControl<MenuItem>("ctxFindRowReferences");
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

        }

        private void OnWindowPointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (_twodaTable == null || !e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
                return;
            // If click was on a cell or column header, let the grid handle it (don't deselect)
            if (TryFindDataGridCell(e.Source) != null || TryFindDataGridColumnHeader(e.Source) != null)
                return;
            // Click was outside any cell/header (sidebar, empty grid area, etc.) — clear selection
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
            ClearCellRangeSelection();
            UpdateFormulaBarAndStatus();
            UpdateInsertRowVisibility();
        }

        private void OnGridSelectionChanged(object sender, EventArgs e)
        {
            UpdateFormulaBarAndStatus();
            UpdateInsertRowVisibility();
            UpdateFindRowReferencesVisibility();
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

        private void UpdateFindRowReferencesVisibility()
        {
            if (_ctxFindRowReferences == null)
            {
                return;
            }

            int rowIndex = GetPrimarySelectedRowIndex();
            _ctxFindRowReferences.IsEnabled =
                _installation?.Installation != null
                && !string.IsNullOrWhiteSpace(_resname)
                && rowIndex >= 0
                && (_sourceData?.Count ?? 0) > 0;
        }

        private int GetPrimarySelectedRowIndex()
        {
            if (_twodaTable == null || _sourceData == null || _sourceData.Count == 0)
            {
                return -1;
            }

            if (_twodaTable.SelectedItem is ObservableCollection<string> primary)
            {
                int index = _sourceData.IndexOf(primary);
                if (index >= 0)
                {
                    return index;
                }
            }

            if (_twodaTable.SelectedItems != null && _twodaTable.SelectedItems.Count > 0)
            {
                var first = _twodaTable.SelectedItems[0] as ObservableCollection<string>;
                if (first != null)
                {
                    return _sourceData.IndexOf(first);
                }
            }

            return -1;
        }

        private void FindReferencesForSelectedRow()
        {
            if (_installation?.Installation == null || string.IsNullOrWhiteSpace(_resname))
            {
                return;
            }

            int rowIndex = GetPrimarySelectedRowIndex();
            if (rowIndex < 0)
            {
                return;
            }

            TwoDA twoDA = TwoDAAuto.Read2DA(Build().Item1);
            TwoDAMemoryReferenceHelper.FindAndShowTwoDARowReferences(
                this,
                _resname,
                rowIndex,
                twoDA,
                _installation,
                showOptionsDialog: true);
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
                if (colIdx > 0 && colIdx < _twodaTable.Columns.Count)
                {
                    ClearCellRangeSelection();
                    _columnSelectionActive = true;
                    SelectColumnByIndex(colIdx);
                    Avalonia.Threading.Dispatcher.UIThread.Post(ApplyColumnHighlight, Avalonia.Threading.DispatcherPriority.Background);
                }
            }
            else
            {
                var cell = TryFindDataGridCell(e.Source);
                if (cell != null)
                {
                    int colIdx = TryGetColumnIndexFromCell(cell);
                    if (colIdx == 0)
                    {
                        int rowIdx = TryGetRowIndexFromSource(e.Source);
                        if (rowIdx >= 0)
                        {
                            bool ctrl = (e.KeyModifiers & KeyModifiers.Control) != 0;
                            if (ctrl)
                            {
                                ToggleRowSelection(rowIdx);
                            }
                            else
                            {
                                SelectRowByIndex(rowIdx);
                            }
                            e.Handled = true;
                        }
                    }
                    else
                    {
                        int rowIdx = TryGetRowIndexFromSource(e.Source);
                        if (rowIdx >= 0)
                        {
                            bool shift = (e.KeyModifiers & KeyModifiers.Shift) != 0;
                            if (shift && _rangeAnchorRow >= 0 && _rangeAnchorCol >= 0)
                            {
                                SelectCellRange(_rangeAnchorRow, _rangeAnchorCol, rowIdx, colIdx);
                            }
                            else
                            {
                                SetCellRangeAnchor(rowIdx, colIdx);
                                if (_columnSelectionActive)
                                {
                                    _columnSelectionActive = false;
                                    ClearColumnHighlight();
                                }
                                ClearRangeHighlight();
                            }
                        }
                    }
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
            ClearCellRangeSelection();
            SelectAllRows();
            _twodaTable.CurrentColumn = _twodaTable.Columns[columnIndex];
            UpdateFormulaBarAndStatus();
        }

        /// <summary>True when a multi-cell rectangular range is active (Shift+click range).</summary>
        public bool IsCellRangeActive => _cellRangeActive;

        private void GetNormalizedRange(out int minRow, out int maxRow, out int minCol, out int maxCol)
        {
            minRow = Math.Min(_rangeAnchorRow, _rangeEndRow);
            maxRow = Math.Max(_rangeAnchorRow, _rangeEndRow);
            minCol = Math.Min(_rangeAnchorCol, _rangeEndCol);
            maxCol = Math.Max(_rangeAnchorCol, _rangeEndCol);
        }

        private void SetCellRangeAnchor(int rowIndex, int colIndex)
        {
            _rangeAnchorRow = rowIndex;
            _rangeAnchorCol = colIndex;
            _rangeEndRow = rowIndex;
            _rangeEndCol = colIndex;
            _cellRangeActive = false;
        }

        private void ClearCellRangeSelection()
        {
            _cellRangeActive = false;
            _rangeAnchorRow = -1;
            _rangeAnchorCol = -1;
            _rangeEndRow = -1;
            _rangeEndCol = -1;
            ClearRangeHighlight();
        }

        private void ClearRangeHighlight()
        {
            if (_twodaTable == null) return;
            foreach (var cell in _twodaTable.GetVisualDescendants().OfType<DataGridCell>())
                cell.Background = Brushes.Transparent;
        }

        /// <summary>Selects an inclusive rectangular cell range and highlights it.</summary>
        public void SelectCellRange(int row1, int col1, int row2, int col2)
        {
            if (_twodaTable == null || _sourceData.Count == 0) return;
            int maxCol = Math.Max(0, GetEffectiveColumnCount() - 1);
            row1 = Math.Max(0, Math.Min(row1, _sourceData.Count - 1));
            row2 = Math.Max(0, Math.Min(row2, _sourceData.Count - 1));
            col1 = Math.Max(0, Math.Min(col1, maxCol));
            col2 = Math.Max(0, Math.Min(col2, maxCol));

            _columnSelectionActive = false;
            _rangeAnchorRow = row1;
            _rangeAnchorCol = col1;
            _rangeEndRow = row2;
            _rangeEndCol = col2;
            _cellRangeActive = row1 != row2 || col1 != col2;

            GetNormalizedRange(out int minRow, out int maxRow, out int minCol, out int maxColNorm);
            _twodaTable.SelectedItems.Clear();
            for (int r = minRow; r <= maxRow; r++)
                _twodaTable.SelectedItems.Add(_sourceData[r]);
            _twodaTable.SelectedItem = _sourceData[minRow];
            NavigateToCell(minRow, minCol);
            ClearColumnHighlight();
            if (_cellRangeActive)
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(ApplyRangeHighlight, Avalonia.Threading.DispatcherPriority.Background);
            }
            UpdateFormulaBarAndStatus();
        }

        private void ApplyRangeHighlight()
        {
            if (_twodaTable == null || !_cellRangeActive) return;
            GetNormalizedRange(out int minRow, out int maxRow, out int minCol, out int maxCol);
            foreach (var rowControl in _twodaTable.GetVisualDescendants().OfType<DataGridRow>())
            {
                var model = rowControl.DataContext as ObservableCollection<string>;
                if (model == null) continue;
                int rowIdx = _sourceData.IndexOf(model);
                foreach (var cell in rowControl.GetVisualDescendants().OfType<DataGridCell>())
                {
                    int colIdx = TryGetColumnIndexFromCell(cell);
                    if (rowIdx >= minRow && rowIdx <= maxRow && colIdx >= minCol && colIdx <= maxCol)
                        cell.Background = RangeHighlightBrush;
                    else
                        cell.Background = Brushes.Transparent;
                }
            }
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
            UpdateStatusBar();
        }

        private int GetCurrentColumnIndex()
        {
            if (_twodaTable?.CurrentColumn == null) return -1;
            return _twodaTable.Columns.IndexOf(_twodaTable.CurrentColumn);
        }

        private void PushState()
        {
            if (_undoRedoInProgress) return;
            MarkDocumentDirty();
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
            // Prefer command-based undo if available
            if (_commandStack.CanUndo)
            {
                _commandStack.Undo();
                UpdateFormulaBarAndStatus();
                return;
            }

            // Fallback: legacy snapshot undo
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
            // Prefer command-based redo if available
            if (_commandStack.CanRedo)
            {
                _commandStack.Redo();
                UpdateFormulaBarAndStatus();
                return;
            }

            // Fallback: legacy snapshot redo
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

            if (TryHandleSelectionShortcut(e.Key, mod))
            {
                e.Handled = true;
                return;
            }

            if (ctrl)
            {
                if (e.Key == Key.N) { _ = TryNewAsync(); e.Handled = true; }
                else if (e.Key == Key.O) { _ = TryOpenAsync(); e.Handled = true; }
                else if (e.Key == Key.S && shift) { _ = RunSaveAsAsyncCore(null); e.Handled = true; }
                else if (e.Key == Key.S) { Save(); e.Handled = true; }
                else if (e.Key == Key.C) { CopySelection(); e.Handled = true; }
                else if (e.Key == Key.X) { CutSelection(); e.Handled = true; }
                else if (e.Key == Key.V) { PasteSelection(); e.Handled = true; }
                else if (e.Key == Key.A) { SelectAllRows(); e.Handled = true; }
                else if (e.Key == Key.Z) { Undo(); e.Handled = true; }
                else if (e.Key == Key.Y) { Redo(); e.Handled = true; }
                else if (e.Key == Key.F) { ShowFindDialog(); e.Handled = true; }
                else if (e.Key == Key.H) { ShowReplaceDialog(); e.Handled = true; }
                else if (shift && e.Key == Key.G) { ShowGoToColumnDialog(); e.Handled = true; }
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
            ClearCellRangeSelection();
            _columnSelectionActive = true;
            SelectAllRows();
            _twodaTable.CurrentColumn = _twodaTable.Columns[colIdx];
            UpdateFormulaBarAndStatus();
            Avalonia.Threading.Dispatcher.UIThread.Post(ApplyColumnHighlight, Avalonia.Threading.DispatcherPriority.Background);
        }

        /// <summary>Selects the current row only (single-row selection).</summary>
        public void SelectCurrentRow()
        {
            if (_twodaTable == null || _sourceData.Count == 0) return;
            int rowIdx = GetPrimarySelectedRowIndex();
            if (rowIdx < 0) rowIdx = 0;
            SelectRowByIndex(rowIdx);
        }

        /// <summary>Handles Shift+Space (row) and Ctrl+Space (column) selection shortcuts. Skips when editing a cell.</summary>
        public bool TryHandleSelectionShortcut(Key key, KeyModifiers modifiers)
        {
            if (IsGridCellEditing()) return false;
            bool ctrl = (modifiers & KeyModifiers.Control) != 0;
            bool shift = (modifiers & KeyModifiers.Shift) != 0;
            if (shift && !ctrl && key == Key.Space)
            {
                SelectCurrentRow();
                return true;
            }
            if (ctrl && !shift && key == Key.Space)
            {
                SelectCurrentColumn();
                return true;
            }
            return false;
        }

        private bool IsGridCellEditing()
        {
            if (_twodaTable == null) return false;
            var focused = TopLevel.GetTopLevel(this)?.FocusManager?.GetFocusedElement() as Control;
            if (focused == null || !(focused is TextBox)) return false;
            var parent = focused.GetVisualParent();
            while (parent != null)
            {
                if (parent == _twodaTable) return true;
                parent = parent.GetVisualParent();
            }
            return false;
        }

        /// <summary>Selects a single row by index and clears column selection mode.</summary>
        public void SelectRowByIndex(int rowIndex)
        {
            if (_twodaTable == null || _sourceData.Count == 0) return;
            if (rowIndex < 0 || rowIndex >= _sourceData.Count) return;
            _columnSelectionActive = false;
            ClearColumnHighlight();
            ClearCellRangeSelection();
            var row = _sourceData[rowIndex];
            _twodaTable.SelectedItems.Clear();
            _twodaTable.SelectedItems.Add(row);
            _twodaTable.SelectedItem = row;
            _twodaTable.ScrollIntoView(row, _twodaTable.CurrentColumn);
            UpdateFormulaBarAndStatus();
        }

        /// <summary>Ctrl+Click on # column: add or remove row from multi-selection.</summary>
        public void ToggleRowSelection(int rowIndex)
        {
            if (_twodaTable == null || _sourceData.Count == 0) return;
            if (rowIndex < 0 || rowIndex >= _sourceData.Count) return;
            _columnSelectionActive = false;
            ClearColumnHighlight();
            ClearCellRangeSelection();
            var row = _sourceData[rowIndex];
            var nextSelection = _twodaTable.SelectedItems.Cast<object>().ToList();
            if (nextSelection.Contains(row))
                nextSelection.Remove(row);
            else
                nextSelection.Add(row);
            _twodaTable.SelectedItem = row;
            _twodaTable.SelectedItems.Clear();
            foreach (object item in nextSelection)
                _twodaTable.SelectedItems.Add(item);
            _twodaTable.ScrollIntoView(row, _twodaTable.CurrentColumn);
            UpdateFormulaBarAndStatus();
        }

        public void ShowKeyboardShortcutsDialog()
        {
            var dialog = new Dialogs.TwoDAKeyboardShortcutsDialog();
            dialog.ShowDialog(this);
        }

        private void SetupMenuHandlers()
        {
            // actionNew, actionOpen, actionSave, actionRevert, actionExit wired by base Editor (actionSaveAs->RunSaveAsAsync(null))
            void Bind(Action handler, params string[] controlNames)
            {
                foreach (string controlName in controlNames)
                {
                    EditorHelpers.BindMenuOrButtonClick(this, controlName, handler);
                }
            }

            void BindLanguage(string controlName, ToolsetLanguage language)
            {
                EditorHelpers.BindMenuOrButtonClick(this, controlName, () =>
                {
                    Localization.SetLanguage(language);
                    RefreshLocalizedStrings();
                });
            }

            Bind(() => _ = RunSaveAsAsyncCore(false), "actionSaveAs2DA");
            Bind(() => _ = RunSaveAsAsyncCore(true), "actionSaveAsCSV");
            Bind(Undo, "actionUndo");
            Bind(Redo, "actionRedo");
            Bind(CopySelection, "actionCopy", "ctxCopy");
            Bind(CutSelection, "actionCut", "ctxCut");
            Bind(PasteSelection, "actionPaste", "ctxPaste");
            Bind(PasteTransposed, "actionPasteTransposed", "ctxPasteTransposed");
            Bind(ClearCell, "actionClearCell", "ctxClearCell");
            Bind(FindReferencesForSelectedRow, "ctxFindRowReferences");
            Bind(ShowFindDialog, "actionFind");
            Bind(ShowReplaceDialog, "actionReplace");
            Bind(ShowGoToRowDialog, "actionGoToRow", "tbGoToRow");
            Bind(ShowGoToColumnDialog, "actionGoToColumn", "tbGoToColumn");
            Bind(SelectAllRows, "actionSelectAll", "tbSelectAll");
            Bind(SelectCurrentColumn, "actionSelectColumn", "tbSelectColumn");
            Bind(SelectCurrentRow, "actionSelectRow", "tbSelectRow");
            Bind(ShowKeyboardShortcutsDialog, "actionKeyboardShortcuts");
            Bind(ToggleFilter, "actionToggleFilter");
            Bind(ToggleSidebar, "actionToggleSidebar");
            Bind(InsertRow, "actionInsertRow", "tbInsertRowSidebar", "ctxInsertRow");
            Bind(InsertRowAbove, "actionInsertRowAbove", "tbInsertRowAbove", "ctxInsertRowAbove");
            Bind(InsertRowBelow, "actionInsertRowBelow", "tbInsertRowBelow", "ctxInsertRowBelow");
            Bind(InsertMultipleRows, "actionInsertRows", "ctxInsertRows");
            Bind(DuplicateRow, "actionDuplicateRow", "tbDuplicateRow", "ctxDuplicateRow");
            Bind(RemoveSelectedRows, "actionRemoveRows", "tbRemoveRowsSidebar", "ctxRemoveRows");
            Bind(MoveSelectedRowsUp, "actionMoveRowUp", "tbMoveRowUpSidebar", "ctxMoveRowUp");
            Bind(MoveSelectedRowsDown, "actionMoveRowDown", "tbMoveRowDownSidebar", "ctxMoveRowDown");
            Bind(FillDown, "actionFillDown", "tbFillDown", "ctxFillDown");
            Bind(() => _ = AddColumnAsync(), "actionAddColumn");
            Bind(() => _ = RenameColumnAsync(), "actionRenameColumn", "tbRenameColumn", "ctxRenameColumn");
            Bind(RemoveColumn, "actionRemoveColumn", "tbRemoveColumnSidebar");
            Bind(MoveCurrentColumnLeft, "actionMoveColumnLeft", "tbMoveColumnLeftSidebar", "ctxMoveColumnLeft");
            Bind(MoveCurrentColumnRight, "actionMoveColumnRight", "tbMoveColumnRightSidebar", "ctxMoveColumnRight");
            Bind(() => SortRows(ascending: true), "actionSortAsc", "tbSortAscSidebar", "ctxSortAsc");
            Bind(() => SortRows(ascending: false), "actionSortDesc", "tbSortDescSidebar", "ctxSortDesc");
            Bind(RedoRowLabels, "actionRedoRowLabels", "tbRedoRowLabels");

            // Python parity: advanced features
            Bind(ShowMultiLevelSortDialog, "actionMultiLevelSort", "ctxMultiLevelSort");
            Bind(TransposeTable, "actionTransposeTable", "ctxTranspose");
            Bind(RemoveDuplicateRows, "actionRemoveDuplicateRows", "ctxRemoveDuplicates");
            Bind(FillRight, "actionFillRight", "ctxFillRight");
            Bind(DuplicateColumn, "actionDuplicateColumn", "ctxDuplicateColumn");
            Bind(SelectBlankCells, "actionSelectBlankCells");
            Bind(SelectCellsWithContent, "actionSelectCellsWithContent");
            Bind(ShowBulkEditDialog, "actionBulkEdit");
            Bind(ShowColumnStatisticsDialog, "actionColumnStats");
            Bind(ShowSetValidationRuleDialog, "actionSetValidation");
            Bind(ValidateDataAndShowReport, "actionValidateData");
            Bind(AutocompleteCurrentCell, "actionAutocompleteCell");
            Bind(ShowColumnFilterDialog, "actionColumnFilter");
            Bind(ClearColumnFilter, "actionClearColumnFilter");
            Bind(() => SetZoomLevel(1.0), "actionZoom100");
            Bind(() => SetZoomLevel(1.25), "actionZoom125");
            Bind(() => SetZoomLevel(1.5), "actionZoom150");
            Bind(() => SetZoomLevel(2.0), "actionZoom200");
            Bind(ToggleTextWrapping, "actionToggleTextWrap");
            Bind(AutoFitAllColumns, "actionAutoFitColumns");
            Bind(ShowManageColumnsDialog, "actionManageColumns");

            Bind(AddColumnQuick, "addColumnButton", "tbAddColumnSidebar");
            EditorHelpers.BindMenuOrButtonClick(this, "tbClearFilter", () =>
            {
                if (_filterEdit != null)
                {
                    _filterEdit.Text = "";
                    DoFilter("");
                }
            });

            BindLanguage("actionLangEnglish", ToolsetLanguage.English);
            BindLanguage("actionLangFrench", ToolsetLanguage.French);
            BindLanguage("actionLangGerman", ToolsetLanguage.German);
            BindLanguage("actionLangItalian", ToolsetLanguage.Italian);
            BindLanguage("actionLangSpanish", ToolsetLanguage.Spanish);
            BindLanguage("actionLangPolish", ToolsetLanguage.Polish);
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
                SetHeader("menuHelp", "Help");

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
                SetHeader("actionGoToColumn", "Go to Column...");
                SetHeader("actionSelectAll", "Select All");
                SetHeader("actionSelectColumn", "Select Column");
                SetHeader("actionSelectRow", "Select Row");
                SetHeader("actionKeyboardShortcuts", "Keyboard Shortcuts...");
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
                SetSidebarBtn("tbSelectRow", "Select Row", "Select current row");
                SetSidebarBtn("tbFillDown", "Fill Down", "Fill Down");
                SetSidebarBtn("tbGoToRow", "Go to Row…", "Go to Row...");
                SetSidebarBtn("tbGoToColumn", "Go to Column…", "Go to Column...");
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
                // Skip hidden columns
                if (_hiddenColumnIndices.Contains(i)) continue;

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
            // Clear any active filter when loading new data
            if (_isColumnFilterActive)
            {
                _allRowsBeforeFilter.Clear();
                _filterColumnIndex = -1;
                _filterAllowedValues.Clear();
                _isColumnFilterActive = false;
            }

            if (data == null || data.Length == 0)
            {
                // Clear all data for null/empty input (don't call New() which adds a starter row)
                _sourceData.Clear();
                _columnHeaders.Clear();
                _undoStack.Clear();
                _redoStack.Clear();
                _twodaTable?.Columns.Clear();
                ClearCellRangeSelection();
                _columnSelectionActive = false;
                ClearColumnHighlight();
                RebuildGridColumns();
                UpdateStatusBar();
                UpdateFindRowReferencesVisibility();
                return;
            }
            TwoDA twoda = TwoDAAuto.Read2DA(data);

            _sourceData.Clear();
            _columnHeaders.Clear();
            _undoStack.Clear();
            _redoStack.Clear();
            _twodaTable?.Columns.Clear();
            ClearCellRangeSelection();
            _columnSelectionActive = false;
            ClearColumnHighlight();

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
            UpdateFindRowReferencesVisibility();
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
                    if (IsDirty)
                    {
                        baseText += " | " + Localization.Tr("Modified");
                    }
                    int selCount = _twodaTable?.SelectedItems?.Count ?? 0;
                    if (selCount > 1)
                    {
                        baseText += " | " + Localization.Trf("{0} rows selected", selCount);
                    }
                    if (_isColumnFilterActive)
                    {
                        int totalRows = _allRowsBeforeFilter?.Count ?? rows;
                        int hiddenRows = totalRows - rows;
                        if (hiddenRows > 0)
                        {
                            baseText += " | " + Localization.Trf("Hidden: {0}", hiddenRows);
                        }
                    }
                    else if (_filteredData?.View?.Filter != null)
                    {
                        int visibleRows = _filteredData.View.Cast<object>().Count();
                        int hiddenRows = rows - visibleRows;
                        baseText += " | " + Localization.Trf("Visible: {0}", visibleRows);
                        if (hiddenRows > 0)
                        {
                            baseText += " | " + Localization.Trf("Hidden: {0}", hiddenRows);
                        }
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
                    if (_cellRangeActive)
                    {
                        GetNormalizedRange(out int minRow, out int maxRow, out int minCol, out int maxCol);
                        int cellCount = (maxRow - minRow + 1) * (maxCol - minCol + 1);
                        if (cellCount > 1)
                        {
                            baseText += " | " + Localization.Trf("Range: R{0}–R{1}, C{2}–C{3}", minRow, maxRow, minCol, maxCol);
                        }
                    }
                    _statusText.Text = baseText;
                }
                if (_sidebarStatsText != null)
                {
                    int rows = _sourceData?.Count ?? 0;
                    int cols = 1 + (_columnHeaders?.Count ?? 0);
                    string stats = Localization.Trf("{0} rows × {1} columns", rows, cols);
                    if (_isColumnFilterActive)
                    {
                        int total = _allRowsBeforeFilter?.Count ?? rows;
                        int hidden = total - rows;
                        stats = hidden > 0
                            ? Localization.Trf("{0} of {1} rows ({2} hidden)", rows, total, hidden)
                            : Localization.Trf("{0} of {1} rows", rows, total);
                    }
                    else if (_filteredData?.View?.Filter != null)
                    {
                        int visible = _filteredData.View.Cast<object>().Count();
                        int hidden = rows - visible;
                        stats = hidden > 0
                            ? Localization.Trf("{0} of {1} rows ({2} hidden)", visible, rows, hidden)
                            : Localization.Trf("{0} of {1} rows", visible, rows);
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

            // Clear both command-based and legacy undo stacks
            _commandStack.Clear();
            _undoStack.Clear();
            _redoStack.Clear();

            // Pre-existing empty table: one data column and one row so the user can click and type immediately.
            _columnHeaders.Add("Column1");
            _sourceData.Add(new ObservableCollection<string> { "", "" }); // row label + one cell
            ClearCellRangeSelection();
            _columnSelectionActive = false;
            ClearColumnHighlight();
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
        protected override async Task RunOpenAsync()
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

        protected override async Task RunSaveAsAsync() => await RunSaveAsAsyncCore(null);

        private async Task RunSaveAsAsyncCore(bool? preferCsv)
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
            _ = RunSaveAsAsyncCore(null);
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
                if (_cellRangeActive && _sourceData != null && _sourceData.Count > 0)
                {
                    GetNormalizedRange(out int minRow, out int maxRow, out int minCol, out int maxCol);
                    var rangeLines = new List<string>();
                    for (int r = minRow; r <= maxRow; r++)
                    {
                        if (r < 0 || r >= _sourceData.Count) continue;
                        var row = _sourceData[r];
                        var cells = new List<string>();
                        for (int c = minCol; c <= maxCol; c++)
                        {
                            string val = (c >= 0 && c < row.Count) ? (row[c] ?? "") : "";
                            cells.Add(EscapeTsv(val));
                        }
                        rangeLines.Add(string.Join("\t", cells));
                    }
                    var rangeText = string.Join("\n", rangeLines);
                    var rangeClipboard = (this as Window)?.Clipboard;
                    if (rangeClipboard != null)
                    {
                        _ = rangeClipboard.SetTextAsync(rangeText);
                    }
                    return;
                }

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

                int startRow;
                int startCol;
                if (ShouldUseAnchorPaste(lines, out startRow, out startCol))
                {
                    var grid = ParseClipboardGrid(lines);
                    if (grid.Count > 0)
                    {
                        PushState();
                        PasteAnchorOverwrite(startRow, startCol, grid);
                        SetItemDisplayData(Math.Min(startRow + grid.Count - 1, _sourceData.Count - 1));
                        UpdateStatusBar();
                    }
                    return;
                }

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

        private bool TryGetPasteAnchor(out int startRow, out int startCol)
        {
            startRow = 0;
            startCol = 0;
            if (_cellRangeActive && _sourceData.Count > 0)
            {
                GetNormalizedRange(out startRow, out int maxRow, out startCol, out int maxCol);
                return true;
            }
            int colIdx = GetCurrentColumnIndex();
            // Column 0 is the row-label (#) column; focus there implies row selection, not cell anchor.
            if (colIdx <= 0) return false;
            var selectedRow = _twodaTable?.SelectedItem as ObservableCollection<string>;
            if (selectedRow == null) return false;
            int rowIdx = _sourceData.IndexOf(selectedRow);
            if (rowIdx < 0) return false;
            startRow = rowIdx;
            startCol = colIdx;
            return true;
        }

        /// <summary>Anchor overwrite when a cell/range is targeted; full-width row paste keeps insert semantics.</summary>
        private bool ShouldUseAnchorPaste(string[] lines, out int startRow, out int startCol)
        {
            startRow = 0;
            startCol = 0;
            if (_columnHeaders.Count == 0 || _sourceData.Count == 0) return false;
            if (!TryGetPasteAnchor(out startRow, out startCol)) return false;

            var grid = ParseClipboardGrid(lines);
            if (grid.Count == 0) return false;

            if (_cellRangeActive) return true;

            // Row-label column focus implies row-oriented paste (insert), not cell anchor.
            if (startCol == 0) return false;

            int fullColCount = GetEffectiveColumnCount();
            bool allLinesFullWidth = true;
            for (int i = 0; i < grid.Count; i++)
            {
                if (grid[i].Count < fullColCount)
                {
                    allLinesFullWidth = false;
                    break;
                }
            }
            return !allLinesFullWidth;
        }

        private static List<List<string>> ParseClipboardGrid(string[] lines)
        {
            var grid = new List<List<string>>();
            if (lines.Length == 0) return grid;
            bool preferCsv = lines[0].Contains(",") && !lines[0].Contains("\t");
            int maxCols = 0;
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var cells = ParsePasteLine(line, preferCsv);
                grid.Add(cells);
                if (cells.Count > maxCols) maxCols = cells.Count;
            }
            for (int r = 0; r < grid.Count; r++)
            {
                while (grid[r].Count < maxCols) grid[r].Add("");
            }
            return grid;
        }

        private void PasteAnchorOverwrite(int startRow, int startCol, List<List<string>> grid)
        {
            int pasteRows = grid.Count;
            int pasteCols = 0;
            for (int r = 0; r < grid.Count; r++)
            {
                if (grid[r].Count > pasteCols) pasteCols = grid[r].Count;
            }
            if (pasteRows == 0 || pasteCols == 0) return;

            int requiredRows = startRow + pasteRows;
            int requiredCols = startCol + pasteCols;
            int colCount = GetEffectiveColumnCount();
            if (requiredCols > colCount)
            {
                int newDataCols = requiredCols - colCount;
                for (int i = 0; i < newDataCols; i++)
                {
                    string colName = "Col" + (_columnHeaders.Count + 1);
                    while (_columnHeaders.Contains(colName)) colName = colName + "_";
                    _columnHeaders.Add(colName);
                }
                RebuildGridColumns();
                colCount = GetEffectiveColumnCount();
            }
            while (_sourceData.Count < requiredRows)
            {
                var row = new ObservableCollection<string>();
                for (int i = 0; i < colCount; i++)
                {
                    row.Add("");
                }
                _sourceData.Add(row);
            }
            for (int r = 0; r < grid.Count; r++)
            {
                int targetRow = startRow + r;
                if (targetRow < 0 || targetRow >= _sourceData.Count) continue;
                EnsureRowColumnCount(_sourceData[targetRow], colCount);
                for (int c = 0; c < grid[r].Count; c++)
                {
                    int targetCol = startCol + c;
                    if (targetCol < 0 || targetCol >= _sourceData[targetRow].Count) continue;
                    _sourceData[targetRow][targetCol] = grid[r][c] ?? "";
                }
            }
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
            RecalculateRowLabels();
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
            RecalculateRowLabels();
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
                RecalculateRowLabels();
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
            RecalculateRowLabels();
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
            RecalculateRowLabels();
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
            RecalculateRowLabels();
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
            RecalculateRowLabels();
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
            RecalculateRowLabels();
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
            int headerIndex = colIdx - 1;
            if (headerIndex < 0 || headerIndex >= _columnHeaders.Count) headerIndex = 0;
            if (_sourceData.Count == 0) return;

            // Preserve CurrentColumn across the sort
            var savedColumn = _twodaTable?.CurrentColumn;

            var command = new SortCommand(_sourceData, headerIndex, ascending, () =>
            {
                // Restore CurrentColumn so subsequent sorts target the same column
                if (savedColumn != null && _twodaTable != null && _sourceData.Count > 0)
                {
                    try
                    {
                        if (_twodaTable.SelectedItem == null)
                            _twodaTable.SelectedItem = _sourceData[0];
                        _twodaTable.CurrentColumn = savedColumn;
                    }
                    catch { /* Headless or no visual tree */ }
                }
                UpdateStatusBar();
            });

            _commandStack.Execute(command);
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

        /// <summary>Resolves a column name or 0-based data column index to a grid column index, or -1.</summary>
        public int ResolveGoToColumnGridIndex(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return -1;
            string trimmed = input.Trim();
            if (int.TryParse(trimmed, out int dataColIdx))
            {
                int gridIdx = dataColIdx + 1;
                if (gridIdx >= 1 && gridIdx < GetEffectiveColumnCount())
                    return gridIdx;
                return -1;
            }
            for (int i = 0; i < _columnHeaders.Count; i++)
            {
                if (string.Equals(_columnHeaders[i], trimmed, StringComparison.OrdinalIgnoreCase))
                    return i + 1;
            }
            return -1;
        }

        /// <summary>Navigates to the given grid column index on the current row and focuses it.</summary>
        public void GoToColumn(int gridColumnIndex)
        {
            if (_twodaTable == null || _sourceData.Count == 0) return;
            if (gridColumnIndex < 0 || gridColumnIndex >= GetEffectiveColumnCount()) return;
            int rowIdx = GetPrimarySelectedRowIndex();
            if (rowIdx < 0) rowIdx = 0;
            NavigateToCell(rowIdx, gridColumnIndex);
        }

        /// <summary>Navigates to a column by name or 0-based data column index.</summary>
        public void GoToColumnByInput(string input)
        {
            int gridIdx = ResolveGoToColumnGridIndex(input);
            if (gridIdx >= 0)
                GoToColumn(gridIdx);
        }

        private void ShowGoToColumnDialog()
        {
            string columnHint = _columnHeaders.Count > 0
                ? string.Join(", ", _columnHeaders)
                : "0";
            var dialog = new Window
            {
                Title = Localization.Tr("Go to Column"),
                Width = 320,
                Height = 140,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var label = new TextBlock { Text = Localization.Tr("Column name or index (0-based):") };
            var textBox = new TextBox { Watermark = columnHint };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var ok = new Button { Content = Localization.Tr("OK"), Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var cancel = new Button { Content = Localization.Tr("Cancel") };
            panel.Children.Add(label);
            panel.Children.Add(textBox);
            buttons.Children.Add(ok);
            buttons.Children.Add(cancel);
            panel.Children.Add(buttons);
            dialog.Content = panel;
            string goToColumnInput = null;
            ok.Click += (s, e) =>
            {
                string text = textBox.Text?.Trim() ?? "";
                if (!string.IsNullOrEmpty(text) && ResolveGoToColumnGridIndex(text) >= 0)
                {
                    goToColumnInput = text;
                    dialog.Close();
                }
            };
            cancel.Click += (s, e) => dialog.Close();
            _ = dialog.ShowDialog(this as Window);
            if (goToColumnInput != null)
                GoToColumnByInput(goToColumnInput);
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
                var command = new RenameColumnCommand(_columnHeaders, colIdx - 1, currentName, _renameColumnResult, () =>
                {
                    if (_twodaTable != null && colIdx < _twodaTable.Columns.Count)
                    {
                        _twodaTable.Columns[colIdx].Header = _columnHeaders[colIdx - 1];
                    }
                    UpdateStatusBar();
                });

                _commandStack.Execute(command);
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
            RecalculateRowLabels();
            SetItemDisplayData(insertIndex);
            UpdateStatusBar();
        }

        private void RecalculateRowLabels()
        {
            for (int i = 0; i < _sourceData.Count; i++)
            {
                if (_sourceData[i].Count > 0)
                {
                    _sourceData[i][0] = i.ToString();
                }
            }
        }

        public void RedoRowLabels()
        {
            if (_sourceData.Count == 0) return;

            var command = new RedoRowLabelsCommand(_sourceData);
            _commandStack.Execute(command);
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

            var command = new MoveColumnCommand(_sourceData, _columnHeaders, headerIndex, headerIndex - 1, () =>
            {
                RebuildGridColumns();
                if (_twodaTable != null && headerIndex < _twodaTable.Columns.Count)
                {
                    _twodaTable.CurrentColumn = _twodaTable.Columns[headerIndex];
                }
                UpdateStatusBar();
            });

            _commandStack.Execute(command);
        }

        public void MoveCurrentColumnRight()
        {
            int colIdx = GetCurrentColumnIndex();
            int headerIndex = colIdx - 1;
            if (headerIndex < 0 || headerIndex >= _columnHeaders.Count - 1) return;

            var command = new MoveColumnCommand(_sourceData, _columnHeaders, headerIndex, headerIndex + 1, () =>
            {
                RebuildGridColumns();
                if (_twodaTable != null && headerIndex + 2 < _twodaTable.Columns.Count)
                {
                    _twodaTable.CurrentColumn = _twodaTable.Columns[headerIndex + 2];
                }
                UpdateStatusBar();
            });

            _commandStack.Execute(command);
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

        // ===== PYTHON PARITY: ADVANCED FEATURES =====

        /// <summary>Shows multi-level sort dialog (primary + up to 2 then-by columns) matching PyKotor functionality.</summary>
        public async void ShowMultiLevelSortDialog()
        {
            if (_columnHeaders.Count == 0) return;

            var dialog = new Window
            {
                Title = "Sort",
                Width = 400,
                Height = 300,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var panel = new StackPanel { Margin = new Avalonia.Thickness(12), Spacing = 8 };

            // Primary sort
            panel.Children.Add(new TextBlock { Text = "Sort by:", FontWeight = FontWeight.Bold });
            var primaryCol = new ComboBox { ItemsSource = _columnHeaders.ToList(), SelectedIndex = 0 };
            panel.Children.Add(primaryCol);
            var primaryOrder = new ComboBox { ItemsSource = new[] { "Ascending", "Descending" }, SelectedIndex = 0 };
            panel.Children.Add(primaryOrder);

            // Then by 1
            panel.Children.Add(new TextBlock { Text = "Then by:", FontWeight = FontWeight.Bold, Margin = new Avalonia.Thickness(0, 8, 0, 0) });
            var thenBy1Col = new ComboBox { ItemsSource = new List<string> { "— None —" }.Concat(_columnHeaders), SelectedIndex = 0 };
            panel.Children.Add(thenBy1Col);
            var thenBy1Order = new ComboBox { ItemsSource = new[] { "Ascending", "Descending" }, SelectedIndex = 0 };
            panel.Children.Add(thenBy1Order);

            // Then by 2
            panel.Children.Add(new TextBlock { Text = "Then by:", FontWeight = FontWeight.Bold, Margin = new Avalonia.Thickness(0, 8, 0, 0) });
            var thenBy2Col = new ComboBox { ItemsSource = new List<string> { "— None —" }.Concat(_columnHeaders), SelectedIndex = 0 };
            panel.Children.Add(thenBy2Col);
            var thenBy2Order = new ComboBox { ItemsSource = new[] { "Ascending", "Descending" }, SelectedIndex = 0 };
            panel.Children.Add(thenBy2Order);

            // Buttons
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 12, 0, 0), Spacing = 8 };
            var ok = new Button { Content = "OK" };
            var cancel = new Button { Content = "Cancel" };
            buttons.Children.Add(ok);
            buttons.Children.Add(cancel);
            panel.Children.Add(buttons);

            dialog.Content = new ScrollViewer { Content = panel };

            bool result = false;
            ok.Click += (s, e) => { result = true; dialog.Close(); };
            cancel.Click += (s, e) => dialog.Close();

            await dialog.ShowDialog(this as Window);

            if (!result) return;

            // Build sort levels list
            var sortLevels = new List<(int columnIndex, bool ascending)>();

            // Primary is always included
            sortLevels.Add((primaryCol.SelectedIndex, primaryOrder.SelectedIndex == 0));

            // Then by 1
            if (thenBy1Col.SelectedIndex > 0)
            {
                sortLevels.Add((thenBy1Col.SelectedIndex - 1, thenBy1Order.SelectedIndex == 0));
            }

            // Then by 2
            if (thenBy2Col.SelectedIndex > 0)
            {
                sortLevels.Add((thenBy2Col.SelectedIndex - 1, thenBy2Order.SelectedIndex == 0));
            }

            // Execute multi-level sort command
            var command = new MultiLevelSortCommand(_sourceData, sortLevels, () => UpdateStatusBar());
            _commandStack.Execute(command);
            RebuildGridColumns();
            UpdateStatusBar();
        }

        /// <summary>Transposes the entire table (rows become columns, columns become rows).</summary>
        public void TransposeTable()
        {
            if (_sourceData.Count == 0) return;

            var command = new TransposeCommand(_sourceData, _columnHeaders, () =>
            {
                RebuildGridColumns();
                UpdateStatusBar();
            });

            _commandStack.Execute(command);
            RebuildGridColumns();
            UpdateStatusBar();
        }

        /// <summary>Removes duplicate rows, keeping only the first occurrence of each unique row.</summary>
        public void RemoveDuplicateRows()
        {
            if (_sourceData.Count == 0) return;

            var command = new RemoveDuplicateRowsCommand(_sourceData, () => UpdateStatusBar());
            _commandStack.Execute(command);
            UpdateStatusBar();
        }

        /// <summary>Fills right: copies the leftmost selected cell value rightward across the selection.</summary>
        public void FillRight()
        {
            var selected = _twodaTable?.SelectedItems?.Cast<ObservableCollection<string>>().ToList();
            if (selected == null || selected.Count == 0) return;

            int startCol = GetCurrentColumnIndex();
            if (startCol < 0) startCol = 1;

            // For each selected row, fill right from current column to end of selection
            var changes = new List<(int row, int col, string oldValue, string newValue)>();

            foreach (var row in selected)
            {
                int rowIndex = _sourceData.IndexOf(row);
                if (rowIndex < 0 || startCol >= row.Count) continue;

                string fillValue = row[startCol];

                // Fill from startCol+1 to end of row (or a reasonable limit)
                for (int col = startCol + 1; col < row.Count; col++)
                {
                    if (row[col] != fillValue)
                    {
                        changes.Add((rowIndex, col, row[col], fillValue));
                    }
                }
            }

            if (changes.Count > 0)
            {
                var command = new BatchSetCellsCommand(_sourceData, changes, "Fill right");
                _commandStack.Execute(command);
                UpdateStatusBar();
            }
        }

        /// <summary>Duplicates the current column (inserted immediately to the right).</summary>
        public void DuplicateColumn()
        {
            int colIdx = GetCurrentColumnIndex();
            int headerIndex = colIdx - 1;

            if (headerIndex < 0 || headerIndex >= _columnHeaders.Count) return;

            var command = new DuplicateColumnCommand(_sourceData, _columnHeaders, headerIndex, () =>
            {
                RebuildGridColumns();
                UpdateStatusBar();
            });

            _commandStack.Execute(command);
            RebuildGridColumns();
            UpdateStatusBar();
        }

        /// <summary>Selects all blank (empty or whitespace-only) cells in the current column.</summary>
        public void SelectBlankCells()
        {
            int colIdx = GetCurrentColumnIndex();
            if (colIdx < 0 || _sourceData.Count == 0) return;

            _twodaTable.SelectedItems.Clear();

            foreach (var row in _sourceData)
            {
                if (colIdx < row.Count && string.IsNullOrWhiteSpace(row[colIdx]))
                {
                    _twodaTable.SelectedItems.Add(row);
                }
            }

            UpdateFormulaBarAndStatus();
        }

        /// <summary>Selects all cells with content (non-blank) in the current column.</summary>
        public void SelectCellsWithContent()
        {
            int colIdx = GetCurrentColumnIndex();
            if (colIdx < 0 || _sourceData.Count == 0) return;

            _twodaTable.SelectedItems.Clear();

            foreach (var row in _sourceData)
            {
                if (colIdx < row.Count && !string.IsNullOrWhiteSpace(row[colIdx]))
                {
                    _twodaTable.SelectedItems.Add(row);
                }
            }

            UpdateFormulaBarAndStatus();
        }

        public async void ShowBulkEditDialog()
        {
            if (_sourceData.Count == 0) return;
            int colIdx = GetCurrentColumnIndex();
            if (colIdx < 0) colIdx = 1;

            var dialog = new Window
            {
                Title = "Bulk Edit Selected",
                Width = 420,
                Height = 260,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var panel = new StackPanel { Margin = new Avalonia.Thickness(12), Spacing = 8 };
            panel.Children.Add(new TextBlock { Text = "Find:" });
            var findBox = new TextBox { Watermark = "Text to find (optional)" };
            panel.Children.Add(findBox);
            panel.Children.Add(new TextBlock { Text = "Replace with:" });
            var replaceBox = new TextBox { Watermark = "Replacement" };
            panel.Children.Add(replaceBox);
            panel.Children.Add(new TextBlock { Text = "Prefix:" });
            var prefixBox = new TextBox { Watermark = "Optional prefix" };
            panel.Children.Add(prefixBox);
            panel.Children.Add(new TextBlock { Text = "Suffix:" });
            var suffixBox = new TextBox { Watermark = "Optional suffix" };
            panel.Children.Add(suffixBox);

            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var apply = new Button { Content = "Apply" };
            var cancel = new Button { Content = "Cancel" };
            buttons.Children.Add(apply);
            buttons.Children.Add(cancel);
            panel.Children.Add(buttons);
            dialog.Content = panel;

            bool confirmed = false;
            apply.Click += (s, e) => { confirmed = true; dialog.Close(); };
            cancel.Click += (s, e) => dialog.Close();

            await dialog.ShowDialog(this as Window);
            if (!confirmed) return;

            string find = findBox.Text ?? "";
            string replace = replaceBox.Text ?? "";
            string prefix = prefixBox.Text ?? "";
            string suffix = suffixBox.Text ?? "";

            var selectedRows = _twodaTable?.SelectedItems?.Cast<ObservableCollection<string>>().ToList() ?? new List<ObservableCollection<string>>();
            if (selectedRows.Count == 0 && _twodaTable?.SelectedItem is ObservableCollection<string> single)
            {
                selectedRows.Add(single);
            }
            if (selectedRows.Count == 0) return;

            var changes = new List<(int row, int col, string oldValue, string newValue)>();
            foreach (var row in selectedRows)
            {
                int rowIndex = _sourceData.IndexOf(row);
                if (rowIndex < 0 || colIdx >= row.Count) continue;

                string oldValue = row[colIdx] ?? "";
                string newValue = oldValue;
                if (!string.IsNullOrEmpty(find))
                {
                    newValue = ReplaceAllInString(newValue, find, replace, true);
                }
                else if (!string.IsNullOrEmpty(replace))
                {
                    newValue = replace;
                }
                newValue = prefix + newValue + suffix;
                if (newValue != oldValue)
                {
                    changes.Add((rowIndex, colIdx, oldValue, newValue));
                }
            }

            if (changes.Count > 0)
            {
                _commandStack.Execute(new BatchSetCellsCommand(_sourceData, changes, "Bulk edit"));
                UpdateStatusBar();
            }
        }

        public async void ShowColumnStatisticsDialog()
        {
            if (_sourceData.Count == 0) return;
            int colIdx = GetCurrentColumnIndex();
            if (colIdx <= 0 || colIdx > _columnHeaders.Count) return;

            string header = _columnHeaders[colIdx - 1];
            var values = _sourceData.Select(r => colIdx < r.Count ? (r[colIdx] ?? "") : "").ToList();
            int total = values.Count;
            int blank = values.Count(v => string.IsNullOrWhiteSpace(v));
            int nonBlank = total - blank;
            int unique = values.Distinct().Count();
            var numeric = values.Select(v => double.TryParse(v, out var n) ? (double?)n : null).Where(v => v.HasValue).Select(v => v.Value).ToList();

            var dialog = new Window
            {
                Title = $"Column Statistics: {header}",
                Width = 420,
                Height = 320,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var text = $"Total Cells: {total}\nNon-Blank: {nonBlank}\nBlank: {blank}\nUnique: {unique}";
            if (numeric.Count > 0)
            {
                text += $"\nNumeric Values: {numeric.Count}\nSum: {numeric.Sum():0.####}\nAvg: {numeric.Average():0.####}\nMin: {numeric.Min():0.####}\nMax: {numeric.Max():0.####}";
            }

            var panel = new StackPanel { Margin = new Avalonia.Thickness(12), Spacing = 8 };
            panel.Children.Add(new TextBlock { Text = text, TextWrapping = TextWrapping.Wrap });
            var closeBtn = new Button { Content = "Close", HorizontalAlignment = HorizontalAlignment.Left };
            closeBtn.Click += (s, e) => dialog.Close();
            panel.Children.Add(closeBtn);
            dialog.Content = panel;
            await dialog.ShowDialog(this as Window);
        }

        public async void ShowColumnFilterDialog()
        {
            if (_columnHeaders.Count == 0) return;

            var dialog = new Window
            {
                Title = "Filter by Column Values",
                Width = 450,
                Height = 550,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var panel = new StackPanel { Margin = new Avalonia.Thickness(12), Spacing = 8 };

            panel.Children.Add(new TextBlock { Text = "Select Column:", FontWeight = FontWeight.Bold });
            var columnCombo = new ComboBox { ItemsSource = _columnHeaders.ToList(), SelectedIndex = 0 };
            panel.Children.Add(columnCombo);

            var checkboxContainer = new StackPanel { Spacing = 4 };
            var scrollViewer = new ScrollViewer { Content = checkboxContainer, Height = 300, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            panel.Children.Add(scrollViewer);

            var selectButtons = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var selectAllBtn = new Button { Content = "Select All" };
            var selectNoneBtn = new Button { Content = "Select None" };
            selectButtons.Children.Add(selectAllBtn);
            selectButtons.Children.Add(selectNoneBtn);
            panel.Children.Add(selectButtons);

            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 12, 0, 0), Spacing = 8 };
            var applyBtn = new Button { Content = "Apply Filter" };
            var cancelBtn = new Button { Content = "Cancel" };
            buttons.Children.Add(applyBtn);
            buttons.Children.Add(cancelBtn);
            panel.Children.Add(buttons);

            dialog.Content = panel;

            List<CheckBox> currentCheckboxes = new List<CheckBox>();

            Action populateCheckboxes = () =>
            {
                checkboxContainer.Children.Clear();
                currentCheckboxes.Clear();

                int colIndex = columnCombo.SelectedIndex;
                if (colIndex < 0 || colIndex >= _columnHeaders.Count) return;

                var sourceRows = _isColumnFilterActive ? _allRowsBeforeFilter : _sourceData.ToList();
                var uniqueValues = new HashSet<string>();

                foreach (var row in sourceRows)
                {
                    if (colIndex + 1 < row.Count)
                    {
                        string cellValue = row[colIndex + 1] ?? "";
                        uniqueValues.Add(cellValue);
                    }
                }

                var sortedValues = uniqueValues.OrderBy(v => v).ToList();

                foreach (var value in sortedValues)
                {
                    string displayValue = string.IsNullOrEmpty(value) ? "(blank)" : value;
                    var checkbox = new CheckBox { Content = displayValue };

                    if (_isColumnFilterActive && colIndex == _filterColumnIndex)
                    {
                        checkbox.IsChecked = _filterAllowedValues.Contains(value);
                    }
                    else
                    {
                        checkbox.IsChecked = true;
                    }

                    currentCheckboxes.Add(checkbox);
                    checkboxContainer.Children.Add(checkbox);
                }
            };

            columnCombo.SelectionChanged += (s, e) => populateCheckboxes();
            selectAllBtn.Click += (s, e) => { foreach (var cb in currentCheckboxes) cb.IsChecked = true; };
            selectNoneBtn.Click += (s, e) => { foreach (var cb in currentCheckboxes) cb.IsChecked = false; };

            populateCheckboxes();

            bool result = false;
            applyBtn.Click += async (s, e) =>
            {
                if (!currentCheckboxes.Any(cb => cb.IsChecked == true))
                {
                    await ShowInfoDialog("Filter Error", "Please select at least one value to filter by.");
                    return;
                }
                result = true;
                dialog.Close();
            };
            cancelBtn.Click += (s, e) => dialog.Close();

            await dialog.ShowDialog(this as Window);

            if (!result) return;

            var allowedValues = new HashSet<string>();
            foreach (var cb in currentCheckboxes)
            {
                if (cb.IsChecked == true)
                {
                    string displayValue = cb.Content?.ToString() ?? "";
                    string actualValue = displayValue == "(blank)" ? "" : displayValue;
                    allowedValues.Add(actualValue);
                }
            }

            ApplyColumnFilter(columnCombo.SelectedIndex, allowedValues);
        }

        private void ApplyColumnFilter(int columnIndex, HashSet<string> allowedValues)
        {
            if (columnIndex < 0 || columnIndex >= _columnHeaders.Count || allowedValues.Count == 0) return;

            if (!_isColumnFilterActive)
            {
                _allRowsBeforeFilter = _sourceData.Select(row => new ObservableCollection<string>(row)).ToList();
            }

            _filterColumnIndex = columnIndex;
            _filterAllowedValues = allowedValues;
            _isColumnFilterActive = true;

            var filteredRows = _allRowsBeforeFilter.Where(row =>
            {
                if (columnIndex + 1 >= row.Count) return false;
                string cellValue = row[columnIndex + 1] ?? "";
                return allowedValues.Contains(cellValue);
            }).ToList();

            _sourceData.Clear();
            foreach (var row in filteredRows)
            {
                _sourceData.Add(new ObservableCollection<string>(row));
            }

            UpdateStatusBar();
        }

        public void ClearColumnFilter()
        {
            if (!_isColumnFilterActive) return;

            _sourceData.Clear();
            foreach (var row in _allRowsBeforeFilter)
            {
                _sourceData.Add(new ObservableCollection<string>(row));
            }

            _allRowsBeforeFilter.Clear();
            _filterColumnIndex = -1;
            _filterAllowedValues.Clear();
            _isColumnFilterActive = false;

            UpdateStatusBar();
        }

        private void SetZoomLevel(double level)
        {
            _zoomLevel = level;
            if (_twodaTable == null) return;

            // Apply zoom to font size
            _twodaTable.FontSize = 12 * _zoomLevel;

            // Recalculate row height
            var rowHeight = (int)(RowHeightEstimate * _zoomLevel);
            _twodaTable.RowHeight = rowHeight;

            UpdateStatusBar();
        }

        private void ToggleTextWrapping()
        {
            _textWrappingEnabled = !_textWrappingEnabled;
            if (_twodaTable == null) return;

            // Rebuild columns to apply text wrapping
            RebuildGridColumns();

            UpdateStatusBar();
        }

        private void AutoFitAllColumns()
        {
            if (_twodaTable == null || _twodaTable.Columns.Count == 0) return;

            foreach (var column in _twodaTable.Columns)
            {
                if (column is DataGridTextColumn textColumn)
                {
                    // Calculate max content width for this column
                    int colIndex = _twodaTable.Columns.IndexOf(column);
                    int maxWidth = MinColumnWidth;

                    // Check header width
                    string headerText = textColumn.Header?.ToString() ?? "";
                    int headerWidth = (int)(headerText.Length * 8 * _zoomLevel) + 20;
                    maxWidth = Math.Max(maxWidth, headerWidth);

                    // Check cell content widths
                    foreach (var row in _sourceData)
                    {
                        if (colIndex < row.Count)
                        {
                            string cellText = row[colIndex] ?? "";
                            int cellWidth = (int)(cellText.Length * 8 * _zoomLevel) + 20;
                            maxWidth = Math.Max(maxWidth, cellWidth);
                        }
                    }

                    // Cap at reasonable maximum
                    maxWidth = Math.Min(maxWidth, 500);
                    column.Width = new DataGridLength(maxWidth);
                }
            }

            UpdateStatusBar();
        }

        public async void ShowManageColumnsDialog()
        {
            if (_columnHeaders.Count == 0) return;

            var dialog = new Window
            {
                Title = "Manage Columns",
                Width = 400,
                Height = 500,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var panel = new StackPanel { Margin = new Avalonia.Thickness(12), Spacing = 8 };

            panel.Children.Add(new TextBlock
            {
                Text = "Select columns to show:",
                FontWeight = FontWeight.Bold
            });

            var scrollViewer = new ScrollViewer { Height = 350 };
            var checkBoxPanel = new StackPanel { Spacing = 4 };
            scrollViewer.Content = checkBoxPanel;
            panel.Children.Add(scrollViewer);

            // Create checkbox for each column
            var columnCheckBoxes = new List<CheckBox>();
            for (int i = 0; i < _columnHeaders.Count; i++)
            {
                var checkbox = new CheckBox
                {
                    Content = _columnHeaders[i],
                    IsChecked = !_hiddenColumnIndices.Contains(i)
                };
                columnCheckBoxes.Add(checkbox);
                checkBoxPanel.Children.Add(checkbox);
            }

            // Buttons
            var selectButtons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 8,
                Margin = new Avalonia.Thickness(0, 8, 0, 0)
            };
            var showAllBtn = new Button { Content = "Show All" };
            var hideAllBtn = new Button { Content = "Hide All" };
            selectButtons.Children.Add(showAllBtn);
            selectButtons.Children.Add(hideAllBtn);
            panel.Children.Add(selectButtons);

            var buttons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 8,
                Margin = new Avalonia.Thickness(0, 12, 0, 0)
            };
            var okBtn = new Button { Content = "OK" };
            var cancelBtn = new Button { Content = "Cancel" };
            buttons.Children.Add(okBtn);
            buttons.Children.Add(cancelBtn);
            panel.Children.Add(buttons);

            dialog.Content = panel;

            // Button handlers
            showAllBtn.Click += (s, e) =>
            {
                foreach (var cb in columnCheckBoxes)
                    cb.IsChecked = true;
            };

            hideAllBtn.Click += (s, e) =>
            {
                foreach (var cb in columnCheckBoxes)
                    cb.IsChecked = false;
            };

            bool result = false;
            okBtn.Click += async (s, e) =>
            {
                // Check if at least one column is visible
                if (!columnCheckBoxes.Any(cb => cb.IsChecked == true))
                {
                    await ShowInfoDialog("Column Visibility", "At least one column must be visible.");
                    return;
                }
                result = true;
                dialog.Close();
            };
            cancelBtn.Click += (s, e) => dialog.Close();

            await dialog.ShowDialog(this as Window);

            if (!result) return;

            // Update hidden columns
            _hiddenColumnIndices.Clear();
            for (int i = 0; i < columnCheckBoxes.Count; i++)
            {
                if (columnCheckBoxes[i].IsChecked != true)
                {
                    _hiddenColumnIndices.Add(i);
                }
            }

            // Rebuild grid to reflect changes
            RebuildGridColumns();

            UpdateStatusBar();
        }

        public async void ShowSetValidationRuleDialog()
        {
            if (_columnHeaders.Count == 0) return;
            int colIdx = GetCurrentColumnIndex();
            if (colIdx <= 0 || colIdx > _columnHeaders.Count) colIdx = 1;
            int headerIdx = colIdx - 1;

            var dialog = new Window
            {
                Title = "Set Validation Rule",
                Width = 360,
                Height = 180,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            var panel = new StackPanel { Margin = new Avalonia.Thickness(12), Spacing = 8 };
            panel.Children.Add(new TextBlock { Text = $"Column: {_columnHeaders[headerIdx]}" });
            var modeBox = new ComboBox
            {
                ItemsSource = new[] { "None", "Required", "Numeric" },
                SelectedIndex = _columnValidationRules.TryGetValue(headerIdx, out var mode) ? (int)mode : 0
            };
            panel.Children.Add(modeBox);
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            var ok = new Button { Content = "OK" };
            var cancel = new Button { Content = "Cancel" };
            buttons.Children.Add(ok);
            buttons.Children.Add(cancel);
            panel.Children.Add(buttons);
            dialog.Content = panel;

            bool confirmed = false;
            ok.Click += (s, e) => { confirmed = true; dialog.Close(); };
            cancel.Click += (s, e) => dialog.Close();

            await dialog.ShowDialog(this as Window);
            if (!confirmed) return;

            _columnValidationRules[headerIdx] = (ColumnValidationMode)modeBox.SelectedIndex;
            UpdateStatusBar();
        }

        public async void ValidateDataAndShowReport()
        {
            if (_columnValidationRules.Count == 0)
            {
                await ShowInfoDialog("Validation", "No validation rules configured.");
                return;
            }

            var issues = new List<string>();
            foreach (var rule in _columnValidationRules)
            {
                int headerIdx = rule.Key;
                var mode = rule.Value;
                if (mode == ColumnValidationMode.None || headerIdx < 0 || headerIdx >= _columnHeaders.Count) continue;

                int col = headerIdx + 1;
                for (int row = 0; row < _sourceData.Count; row++)
                {
                    string value = col < _sourceData[row].Count ? (_sourceData[row][col] ?? "") : "";
                    bool invalid;
                    if (mode == ColumnValidationMode.Required)
                    {
                        invalid = string.IsNullOrWhiteSpace(value);
                    }
                    else if (mode == ColumnValidationMode.Numeric)
                    {
                        double parsed;
                        invalid = !string.IsNullOrWhiteSpace(value) && !double.TryParse(value, out parsed);
                    }
                    else
                    {
                        invalid = false;
                    }
                    if (invalid)
                    {
                        issues.Add($"Row {row}, Column '{_columnHeaders[headerIdx]}': '{value}'");
                    }
                }
            }

            string message = issues.Count == 0
                ? "Validation passed. No issues found."
                : $"Found {issues.Count} issue(s):\n\n" + DialogHelper.BuildTruncatedList(issues, 30);
            await ShowInfoDialog("Validation Report", message);
        }

        public void AutocompleteCurrentCell()
        {
            int colIdx = GetCurrentColumnIndex();
            if (colIdx <= 0) return;
            if (!(_twodaTable?.SelectedItem is ObservableCollection<string> selectedRow)) return;
            int rowIdx = _sourceData.IndexOf(selectedRow);
            if (rowIdx < 0 || colIdx >= selectedRow.Count) return;

            string current = selectedRow[colIdx] ?? "";
            if (string.IsNullOrWhiteSpace(current)) return;

            var candidates = _sourceData
                .Where((r, i) => i != rowIdx && colIdx < r.Count)
                .Select(r => r[colIdx] ?? "")
                .Where(v => !string.IsNullOrWhiteSpace(v) && v.StartsWith(current, StringComparison.OrdinalIgnoreCase))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(v => v.Length)
                .ToList();

            if (candidates.Count == 0) return;
            string match = candidates[0];
            if (string.Equals(match, current, StringComparison.Ordinal)) return;

            _commandStack.Execute(new SetCellCommand(_sourceData, rowIdx, colIdx, current, match));
            UpdateStatusBar();
        }

        private async Task ShowInfoDialog(string title, string text)
        {
            var dialog = new Window
            {
                Title = title,
                Width = 520,
                Height = 360,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12), Spacing = 8 };
            panel.Children.Add(new TextBlock { Text = text, TextWrapping = TextWrapping.Wrap });
            var closeBtn = new Button { Content = "Close", HorizontalAlignment = HorizontalAlignment.Left };
            closeBtn.Click += (s, e) => dialog.Close();
            panel.Children.Add(closeBtn);
            dialog.Content = new ScrollViewer { Content = panel };
            await dialog.ShowDialog(this as Window);
        }
    }

    public enum ColumnValidationMode
    {
        None = 0,
        Required = 1,
        Numeric = 2
    }

    public enum VerticalHeaderOption
    {
        RowIndex = 0,
        RowLabel = 1,
        CellValue = 2,
        None = 3
    }
}
