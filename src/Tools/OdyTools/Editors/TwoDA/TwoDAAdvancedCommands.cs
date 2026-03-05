using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OdyTools.Editors.TwoDACommands
{
    /// <summary>Command for sorting rows by a single column.</summary>
    public class SortCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly int _columnIndex;
        private readonly bool _ascending;
        private readonly List<ObservableCollection<string>> _originalOrder;
        private readonly Action _onModify;

        public string Description => "Sort column";

        public SortCommand(
            ObservableCollection<ObservableCollection<string>> data,
            int columnIndex,
            bool ascending,
            Action onModify = null)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _columnIndex = columnIndex;
            _ascending = ascending;
            _onModify = onModify;

            // Save original order
            _originalOrder = new List<ObservableCollection<string>>(_data);
        }

        public void Execute()
        {
            var sorted = _data.OrderBy(row =>
            {
                if (_columnIndex + 1 < row.Count)
                    return row[_columnIndex + 1]?.ToLowerInvariant() ?? "";
                return "";
            }).ToList();

            if (!_ascending)
                sorted.Reverse();

            _data.Clear();
            foreach (var row in sorted)
            {
                _data.Add(row);
            }

            _onModify?.Invoke();
        }

        public void Undo()
        {
            _data.Clear();
            foreach (var row in _originalOrder)
            {
                _data.Add(row);
            }

            _onModify?.Invoke();
        }
    }

    /// <summary>Command for multi-level sorting (primary + then-by columns).</summary>
    public class MultiLevelSortCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly List<(int columnIndex, bool ascending)> _sortLevels;
        private readonly List<ObservableCollection<string>> _originalOrder;
        private readonly Action _onModify;

        public string Description => "Sort (multi-level)";

        public MultiLevelSortCommand(
            ObservableCollection<ObservableCollection<string>> data,
            List<(int columnIndex, bool ascending)> sortLevels,
            Action onModify = null)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _sortLevels = sortLevels ?? throw new ArgumentNullException(nameof(sortLevels));
            _onModify = onModify;

            // Save original order
            _originalOrder = new List<ObservableCollection<string>>(_data);
        }

        public void Execute()
        {
            var sorted = _data.ToList();

            // Apply sorts in reverse order (like Python stable sort with reversed levels)
            for (int i = _sortLevels.Count - 1; i >= 0; i--)
            {
                var (columnIndex, ascending) = _sortLevels[i];
                sorted = sorted.OrderBy(row =>
                {
                    if (columnIndex + 1 < row.Count)
                        return row[columnIndex + 1]?.ToLowerInvariant() ?? "";
                    return "";
                }).ToList();

                if (!ascending)
                    sorted.Reverse();
            }

            _data.Clear();
            foreach (var row in sorted)
            {
                _data.Add(row);
            }

            _onModify?.Invoke();
        }

        public void Undo()
        {
            _data.Clear();
            foreach (var row in _originalOrder)
            {
                _data.Add(row);
            }

            _onModify?.Invoke();
        }
    }

    /// <summary>Command for duplicating a column.</summary>
    public class DuplicateColumnCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly List<string> _columnHeaders;
        private readonly int _sourceIndex;
        private readonly Action _onModify;

        public string Description => "Duplicate column";

        public DuplicateColumnCommand(
            ObservableCollection<ObservableCollection<string>> data,
            List<string> columnHeaders,
            int sourceIndex,
            Action onModify = null)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _columnHeaders = columnHeaders ?? throw new ArgumentNullException(nameof(columnHeaders));
            _sourceIndex = sourceIndex;
            _onModify = onModify;
        }

        public void Execute()
        {
            // Duplicate header (insert after source)
            if (_sourceIndex >= 0 && _sourceIndex < _columnHeaders.Count)
            {
                string newHeader = _columnHeaders[_sourceIndex];
                _columnHeaders.Insert(_sourceIndex + 1, newHeader);
            }

            // Duplicate cells in all rows
            foreach (var row in _data)
            {
                if (_sourceIndex + 1 < row.Count)
                {
                    string value = row[_sourceIndex + 1];
                    row.Insert(_sourceIndex + 2, value);
                }
            }

            _onModify?.Invoke();
        }

        public void Undo()
        {
            // Remove duplicated header
            if (_sourceIndex + 1 < _columnHeaders.Count)
            {
                _columnHeaders.RemoveAt(_sourceIndex + 1);
            }

            // Remove duplicated cells from all rows
            foreach (var row in _data)
            {
                if (_sourceIndex + 2 < row.Count)
                {
                    row.RemoveAt(_sourceIndex + 2);
                }
            }

            _onModify?.Invoke();
        }
    }

    /// <summary>Command for removing duplicate rows (keeps first occurrence).</summary>
    public class RemoveDuplicateRowsCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly List<(int index, ObservableCollection<string> row)> _removed;
        private readonly Action _onModify;

        public string Description => "Remove duplicate rows";

        public RemoveDuplicateRowsCommand(
            ObservableCollection<ObservableCollection<string>> data,
            Action onModify = null)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _onModify = onModify;
            _removed = new List<(int, ObservableCollection<string>)>();

            // Find duplicates
            var seen = new HashSet<string>();
            var toRemove = new List<int>();

            for (int i = 0; i < _data.Count; i++)
            {
                // Create key from all cells in the row
                string key = string.Join("\t", _data[i]);
                if (seen.Contains(key))
                {
                    toRemove.Add(i);
                }
                else
                {
                    seen.Add(key);
                }
            }

            // Save removed rows (in descending order)
            foreach (var index in toRemove.OrderByDescending(x => x))
            {
                _removed.Add((index, _data[index]));
            }

            // Reverse to get ascending order for undo
            _removed.Reverse();
        }

        public void Execute()
        {
            // Remove in descending order to preserve indices
            foreach (var (index, _) in _removed.OrderByDescending(x => x.index))
            {
                if (index >= 0 && index < _data.Count)
                {
                    _data.RemoveAt(index);
                }
            }

            _onModify?.Invoke();
        }

        public void Undo()
        {
            // Restore in ascending order
            foreach (var (index, row) in _removed)
            {
                if (index >= 0 && index <= _data.Count)
                {
                    _data.Insert(index, row);
                }
            }

            _onModify?.Invoke();
        }
    }

    /// <summary>Command for transposing the entire table (rows become columns, columns become rows).</summary>
    public class TransposeCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly List<string> _columnHeaders;
        private readonly List<ObservableCollection<string>> _savedRows;
        private readonly List<string> _savedHeaders;
        private readonly Action _onModify;

        public string Description => "Transpose table";

        public TransposeCommand(
            ObservableCollection<ObservableCollection<string>> data,
            List<string> columnHeaders,
            Action onModify = null)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _columnHeaders = columnHeaders ?? throw new ArgumentNullException(nameof(columnHeaders));
            _onModify = onModify;

            // Save current state
            _savedRows = new List<ObservableCollection<string>>();
            foreach (var row in _data)
            {
                _savedRows.Add(new ObservableCollection<string>(row));
            }
            _savedHeaders = new List<string>(_columnHeaders);
        }

        public void Execute()
        {
            if (_data.Count == 0) return;

            int rowCount = _data.Count;
            int colCount = _columnHeaders.Count;

            // Create transposed data
            var transposed = new List<ObservableCollection<string>>();
            var newHeaders = new List<string>();

            // New headers come from old row labels
            for (int r = 0; r < rowCount; r++)
            {
                if (_data[r].Count > 0)
                    newHeaders.Add(_data[r][0]);
                else
                    newHeaders.Add($"Row{r}");
            }

            // New rows come from old columns
            for (int c = 0; c < colCount; c++)
            {
                var newRow = new ObservableCollection<string>();
                // Row label is the old column header
                newRow.Add(_columnHeaders[c]);

                // Cells come from old column c
                for (int r = 0; r < rowCount; r++)
                {
                    if (c + 1 < _data[r].Count)
                        newRow.Add(_data[r][c + 1]);
                    else
                        newRow.Add("");
                }

                transposed.Add(newRow);
            }

            // Apply transposed data
            _data.Clear();
            foreach (var row in transposed)
            {
                _data.Add(row);
            }

            _columnHeaders.Clear();
            _columnHeaders.AddRange(newHeaders);

            _onModify?.Invoke();
        }

        public void Undo()
        {
            _data.Clear();
            foreach (var row in _savedRows)
            {
                _data.Add(row);
            }

            _columnHeaders.Clear();
            _columnHeaders.AddRange(_savedHeaders);

            _onModify?.Invoke();
        }
    }

    /// <summary>Command for the Fill Right operation (copy leftmost selected cell rightward).</summary>
    public class FillRightCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly List<(int row, int col, string oldValue)> _changes;

        public string Description => "Fill right";

        public FillRightCommand(
            ObservableCollection<ObservableCollection<string>> data,
            int row,
            int startCol,
            int endCol)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _changes = new List<(int, int, string)>();

            if (row < 0 || row >= _data.Count || startCol < 0 || endCol < startCol)
                return;

            string fillValue = startCol + 1 < _data[row].Count ? _data[row][startCol + 1] : "";

            for (int col = startCol + 1; col <= endCol; col++)
            {
                if (col + 1 < _data[row].Count)
                {
                    _changes.Add((row, col + 1, _data[row][col + 1]));
                }
            }
        }

        public void Execute()
        {
            if (_changes.Count == 0) return;

            string fillValue = _data[_changes[0].row][_changes[0].col - 1];
            foreach (var (row, col, _) in _changes)
            {
                if (row >= 0 && row < _data.Count && col >= 0 && col < _data[row].Count)
                {
                    _data[row][col] = fillValue;
                }
            }
        }

        public void Undo()
        {
            foreach (var (row, col, oldValue) in _changes)
            {
                if (row >= 0 && row < _data.Count && col >= 0 && col < _data[row].Count)
                {
                    _data[row][col] = oldValue;
                }
            }
        }
    }

    /// <summary>Command for regenerating row labels (0, 1, 2, ...).</summary>
    public class RedoRowLabelsCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly List<string> _oldLabels;

        public string Description => "Redo row labels";

        public RedoRowLabelsCommand(ObservableCollection<ObservableCollection<string>> data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _oldLabels = new List<string>();

            foreach (var row in _data)
            {
                _oldLabels.Add(row.Count > 0 ? row[0] : "");
            }
        }

        public void Execute()
        {
            for (int i = 0; i < _data.Count; i++)
            {
                if (_data[i].Count > 0)
                {
                    _data[i][0] = i.ToString();
                }
            }
        }

        public void Undo()
        {
            for (int i = 0; i < _data.Count && i < _oldLabels.Count; i++)
            {
                if (_data[i].Count > 0)
                {
                    _data[i][0] = _oldLabels[i];
                }
            }
        }
    }
}
