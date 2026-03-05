using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OdyTools.Editors.TwoDACommands
{
    /// <summary>Command for editing a single cell value.</summary>
    public class SetCellCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly int _row;
        private readonly int _col;
        private readonly string _oldValue;
        private readonly string _newValue;

        public string Description => "Edit cell";

        public SetCellCommand(ObservableCollection<ObservableCollection<string>> data, int row, int col, string oldValue, string newValue)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _row = row;
            _col = col;
            _oldValue = oldValue ?? "";
            _newValue = newValue ?? "";
        }

        public void Execute()
        {
            if (_row >= 0 && _row < _data.Count && _col >= 0 && _col < _data[_row].Count)
            {
                _data[_row][_col] = _newValue;
            }
        }

        public void Undo()
        {
            if (_row >= 0 && _row < _data.Count && _col >= 0 && _col < _data[_row].Count)
            {
                _data[_row][_col] = _oldValue;
            }
        }
    }

    /// <summary>Command for editing multiple cells in a single operation (e.g., paste, fill, replace-all).</summary>
    public class BatchSetCellsCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly List<(int row, int col, string oldValue, string newValue)> _changes;
        private readonly string _description;

        public string Description => _description;

        public BatchSetCellsCommand(
            ObservableCollection<ObservableCollection<string>> data,
            List<(int row, int col, string oldValue, string newValue)> changes,
            string description = "Edit cells")
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _changes = changes ?? throw new ArgumentNullException(nameof(changes));
            _description = description;
        }

        public void Execute()
        {
            foreach (var (row, col, _, newValue) in _changes)
            {
                if (row >= 0 && row < _data.Count && col >= 0 && col < _data[row].Count)
                {
                    _data[row][col] = newValue;
                }
            }
        }

        public void Undo()
        {
            foreach (var (row, col, oldValue, _) in _changes)
            {
                if (row >= 0 && row < _data.Count && col >= 0 && col < _data[row].Count)
                {
                    _data[row][col] = oldValue;
                }
            }
        }
    }

    /// <summary>Command for inserting a new row into the table.</summary>
    public class InsertRowCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly int _index;
        private readonly ObservableCollection<string> _row;
        private readonly Action _onModify;

        public string Description => "Insert row";

        public InsertRowCommand(
            ObservableCollection<ObservableCollection<string>> data,
            int index,
            ObservableCollection<string> row,
            Action onModify = null)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _index = index;
            _row = row ?? throw new ArgumentNullException(nameof(row));
            _onModify = onModify;
        }

        public void Execute()
        {
            if (_index >= 0 && _index <= _data.Count)
            {
                _data.Insert(_index, _row);
                _onModify?.Invoke();
            }
        }

        public void Undo()
        {
            if (_index >= 0 && _index < _data.Count && _data[_index] == _row)
            {
                _data.RemoveAt(_index);
                _onModify?.Invoke();
            }
        }
    }

    /// <summary>Command for removing rows from the table.</summary>
    public class RemoveRowsCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly List<(int index, ObservableCollection<string> row)> _removed;
        private readonly Action _onModify;

        public string Description => "Remove rows";

        public RemoveRowsCommand(
            ObservableCollection<ObservableCollection<string>> data,
            List<int> indices,
            Action onModify = null)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _onModify = onModify;
            _removed = new List<(int, ObservableCollection<string>)>();

            // Store rows in descending order for correct restoration
            var sorted = indices.OrderByDescending(i => i).ToList();
            foreach (var index in sorted)
            {
                if (index >= 0 && index < _data.Count)
                {
                    _removed.Add((index, _data[index]));
                }
            }
            // Reverse so we have ascending order for undo
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

    /// <summary>Command for inserting a new column into the table.</summary>
    public class InsertColumnCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly List<string> _columnHeaders;
        private readonly int _index;
        private readonly string _header;
        private readonly Action _onModify;

        public string Description => "Insert column";

        public InsertColumnCommand(
            ObservableCollection<ObservableCollection<string>> data,
            List<string> columnHeaders,
            int index,
            string header,
            Action onModify = null)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _columnHeaders = columnHeaders ?? throw new ArgumentNullException(nameof(columnHeaders));
            _index = index;
            _header = header ?? "Column";
            _onModify = onModify;
        }

        public void Execute()
        {
            // Insert header (index is 0-based in _columnHeaders)
            if (_index >= 0 && _index <= _columnHeaders.Count)
            {
                _columnHeaders.Insert(_index, _header);
            }

            // Insert empty cells in all rows (index+1 because row[0] is the row label)
            foreach (var row in _data)
            {
                if (_index + 1 <= row.Count)
                {
                    row.Insert(_index + 1, "");
                }
            }

            _onModify?.Invoke();
        }

        public void Undo()
        {
            // Remove header
            if (_index >= 0 && _index < _columnHeaders.Count)
            {
                _columnHeaders.RemoveAt(_index);
            }

            // Remove cells from all rows
            foreach (var row in _data)
            {
                if (_index + 1 < row.Count)
                {
                    row.RemoveAt(_index + 1);
                }
            }

            _onModify?.Invoke();
        }
    }

    /// <summary>Command for removing a column from the table.</summary>
    public class RemoveColumnCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly List<string> _columnHeaders;
        private readonly int _index;
        private readonly string _header;
        private readonly List<string> _cells;
        private readonly Action _onModify;

        public string Description => "Remove column";

        public RemoveColumnCommand(
            ObservableCollection<ObservableCollection<string>> data,
            List<string> columnHeaders,
            int index,
            Action onModify = null)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _columnHeaders = columnHeaders ?? throw new ArgumentNullException(nameof(columnHeaders));
            _index = index;
            _onModify = onModify;

            // Save column data
            _header = _index >= 0 && _index < _columnHeaders.Count ? _columnHeaders[_index] : "";
            _cells = new List<string>();
            foreach (var row in _data)
            {
                if (_index + 1 < row.Count)
                {
                    _cells.Add(row[_index + 1]);
                }
                else
                {
                    _cells.Add("");
                }
            }
        }

        public void Execute()
        {
            // Remove header
            if (_index >= 0 && _index < _columnHeaders.Count)
            {
                _columnHeaders.RemoveAt(_index);
            }

            // Remove cells from all rows
            foreach (var row in _data)
            {
                if (_index + 1 < row.Count)
                {
                    row.RemoveAt(_index + 1);
                }
            }

            _onModify?.Invoke();
        }

        public void Undo()
        {
            // Restore header
            if (_index >= 0 && _index <= _columnHeaders.Count)
            {
                _columnHeaders.Insert(_index, _header);
            }

            // Restore cells in all rows
            for (int i = 0; i < _data.Count; i++)
            {
                if (_index + 1 <= _data[i].Count)
                {
                    string value = i < _cells.Count ? _cells[i] : "";
                    _data[i].Insert(_index + 1, value);
                }
            }

            _onModify?.Invoke();
        }
    }

    /// <summary>Command for renaming a column header.</summary>
    public class RenameColumnCommand : I2DACommand
    {
        private readonly List<string> _columnHeaders;
        private readonly int _index;
        private readonly string _oldName;
        private readonly string _newName;
        private readonly Action _onModify;

        public string Description => "Rename column";

        public RenameColumnCommand(
            List<string> columnHeaders,
            int index,
            string oldName,
            string newName,
            Action onModify = null)
        {
            _columnHeaders = columnHeaders ?? throw new ArgumentNullException(nameof(columnHeaders));
            _index = index;
            _oldName = oldName ?? "";
            _newName = newName ?? "";
            _onModify = onModify;
        }

        public void Execute()
        {
            if (_index >= 0 && _index < _columnHeaders.Count)
            {
                _columnHeaders[_index] = _newName;
                _onModify?.Invoke();
            }
        }

        public void Undo()
        {
            if (_index >= 0 && _index < _columnHeaders.Count)
            {
                _columnHeaders[_index] = _oldName;
                _onModify?.Invoke();
            }
        }
    }

    /// <summary>Command for moving (swapping) a row up or down by one position.</summary>
    public class MoveRowCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly int _fromIndex;
        private readonly int _toIndex;
        private readonly Action _onModify;

        public string Description => _fromIndex < _toIndex ? "Move row down" : "Move row up";

        public MoveRowCommand(
            ObservableCollection<ObservableCollection<string>> data,
            int fromIndex,
            int toIndex,
            Action onModify = null)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _fromIndex = fromIndex;
            _toIndex = toIndex;
            _onModify = onModify;

            if (_fromIndex < 0 || _fromIndex >= _data.Count || _toIndex < 0 || _toIndex >= _data.Count)
                throw new ArgumentOutOfRangeException("Row move indices out of range");
        }

        public void Execute()
        {
            SwapRows(_fromIndex, _toIndex);
        }

        public void Undo()
        {
            SwapRows(_toIndex, _fromIndex);
        }

        private void SwapRows(int idx1, int idx2)
        {
            if (idx1 >= 0 && idx1 < _data.Count && idx2 >= 0 && idx2 < _data.Count)
            {
                var temp = _data[idx1];
                _data[idx1] = _data[idx2];
                _data[idx2] = temp;
                _onModify?.Invoke();
            }
        }
    }

    /// <summary>Command for moving (swapping) a column left or right by one position.</summary>
    public class MoveColumnCommand : I2DACommand
    {
        private readonly ObservableCollection<ObservableCollection<string>> _data;
        private readonly List<string> _columnHeaders;
        private readonly int _fromIndex;
        private readonly int _toIndex;
        private readonly Action _onModify;

        public string Description => _fromIndex < _toIndex ? "Move column right" : "Move column left";

        public MoveColumnCommand(
            ObservableCollection<ObservableCollection<string>> data,
            List<string> columnHeaders,
            int fromIndex,
            int toIndex,
            Action onModify = null)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _columnHeaders = columnHeaders ?? throw new ArgumentNullException(nameof(columnHeaders));
            _fromIndex = fromIndex;
            _toIndex = toIndex;
            _onModify = onModify;

            if (_fromIndex < 0 || _fromIndex >= _columnHeaders.Count || _toIndex < 0 || _toIndex >= _columnHeaders.Count)
                throw new ArgumentOutOfRangeException("Column move indices out of range");
        }

        public void Execute()
        {
            SwapColumns(_fromIndex, _toIndex);
        }

        public void Undo()
        {
            SwapColumns(_toIndex, _fromIndex);
        }

        private void SwapColumns(int idx1, int idx2)
        {
            // Swap headers
            if (idx1 >= 0 && idx1 < _columnHeaders.Count && idx2 >= 0 && idx2 < _columnHeaders.Count)
            {
                var temp = _columnHeaders[idx1];
                _columnHeaders[idx1] = _columnHeaders[idx2];
                _columnHeaders[idx2] = temp;
            }

            // Swap cells in all rows (remember: idx+1 because row[0] is row label)
            foreach (var row in _data)
            {
                if (idx1 + 1 < row.Count && idx2 + 1 < row.Count)
                {
                    var temp = row[idx1 + 1];
                    row[idx1 + 1] = row[idx2 + 1];
                    row[idx2 + 1] = temp;
                }
            }

            _onModify?.Invoke();
        }
    }
}
