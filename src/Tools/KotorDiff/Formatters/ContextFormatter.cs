// Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/formatters.py:169-214
// Original: class ContextFormatter(DiffFormatter): ...
using System;
using System.Text;
using KotorDiff.Diff.Objects;

namespace KotorDiff.Formatters
{
    /// <summary>
    /// Context diff formatter (similar to `diff -c`).
    /// 1:1 port of ContextFormatter from vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/formatters.py:169-214
    /// </summary>
    public class ContextFormatter : DiffFormatter
    {
        public ContextFormatter(Action<string> outputFunc = null) : base(outputFunc)
        {
        }

        public override string FormatDiff<T>(DiffResult<T> diffResult)
        {
            if (diffResult.HasError)
            {
                return $"diff: {diffResult.ErrorMessage}";
            }

            if (diffResult.DiffType == DiffType.Identical)
            {
                return "";
            }

            if (diffResult.DiffType == DiffType.Added)
            {
                return $"*** /dev/null\n--- {diffResult.RightIdentifier}";
            }

            if (diffResult.DiffType == DiffType.Removed)
            {
                return $"*** {diffResult.LeftIdentifier}\n--- /dev/null";
            }

            // For modified files
            string header = $"*** {diffResult.LeftIdentifier}\n--- {diffResult.RightIdentifier}";

            // Handle text-like content
            if (diffResult is ResourceDiffResult resourceDiff &&
                (resourceDiff.ResourceType == "txt" || resourceDiff.ResourceType == "nss"))
            {
                try
                {
                    if (resourceDiff.LeftValue != null && resourceDiff.RightValue != null)
                    {
                        string leftText = Encoding.UTF8.GetString(resourceDiff.LeftValue);
                        string rightText = Encoding.UTF8.GetString(resourceDiff.RightValue);

                        var leftLines = leftText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        var rightLines = rightText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                        // Simple context diff implementation
                        var diffLines = GenerateContextDiff(leftLines, rightLines,
                            resourceDiff.LeftIdentifier, resourceDiff.RightIdentifier);

                        return string.Join("\n", diffLines);
                    }
                }
                catch (Exception e)
                {
                    return $"{header}\nError formatting context diff: {e.GetType().Name}: {e.Message}";
                }
            }

            return header + "\nBinary files differ";
        }

        /// <summary>
        /// Generate context diff using Myers diff algorithm (matching Python difflib.context_diff)
        /// Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/formatters.py:199-205
        /// Original: diff_lines = list(difflib.context_diff(...))
        /// </summary>
        private static string[] GenerateContextDiff(string[] leftLines, string[] rightLines, string leftFile, string rightFile)
        {
            // Use Myers diff algorithm to find minimal set of changes (same as UnifiedFormatter)
            var operations = MyersDiff(leftLines, rightLines);

            // Format as context diff with proper hunks and context
            return FormatContextDiff(operations, leftLines, rightLines, leftFile, rightFile);
        }

        /// <summary>
        /// Represents a single diff operation
        /// </summary>
        private class DiffOperation
        {
            public enum OperationType { Equal, Insert, Delete }

            public OperationType Type { get; }
            public int OldIndex { get; }
            public int NewIndex { get; }
            public string Line { get; }

            public DiffOperation(OperationType type, int oldIndex, int newIndex, string line)
            {
                Type = type;
                OldIndex = oldIndex;
                NewIndex = newIndex;
                Line = line;
            }
        }

        /// <summary>
        /// Myers diff algorithm implementation - finds minimal set of changes
        /// Based on the algorithm used in UnifiedFormatter, matching Python difflib behavior
        /// Reference: "An O(ND) Difference Algorithm and Its Variations" by Eugene W. Myers
        /// </summary>
        private static System.Collections.Generic.List<DiffOperation> MyersDiff(string[] oldLines, string[] newLines)
        {
            int n = oldLines.Length;
            int m = newLines.Length;
            int max = n + m;

            var trace = new System.Collections.Generic.List<int[]>();
            var operations = new System.Collections.Generic.List<DiffOperation>();

            // Initialize the trace for backtracking
            for (int d = 0; d <= max; d++)
            {
                trace.Add(new int[2 * d + 1]);
                for (int k = -d; k <= d; k += 2)
                {
                    int x;
                    if (d == 0)
                    {
                        x = 0;
                    }
                    else if (k == -d || (k != d && trace[d - 1][k - 1 + d - 1] < trace[d - 1][k + 1 + d - 1]))
                    {
                        x = trace[d - 1][k + 1 + d - 1];
                    }
                    else
                    {
                        x = trace[d - 1][k - 1 + d - 1] + 1;
                    }

                    int y = x - k;

                    // Move diagonally as far as possible (find equal lines)
                    while (x < n && y < m && oldLines[x] == newLines[y])
                    {
                        x++;
                        y++;
                    }

                    trace[d][k + d] = x;

                    if (x >= n && y >= m)
                    {
                        // Found the end, reconstruct the path
                        return ReconstructPath(trace, d, k, oldLines, newLines);
                    }
                }
            }

            // This should never happen for valid inputs
            throw new System.InvalidOperationException("Diff algorithm failed to find a path");
        }

        /// <summary>
        /// Reconstruct the diff operations from the Myers algorithm trace
        /// </summary>
        private static System.Collections.Generic.List<DiffOperation> ReconstructPath(System.Collections.Generic.List<int[]> trace, int d, int k, string[] oldLines, string[] newLines)
        {
            var operations = new System.Collections.Generic.List<DiffOperation>();
            int x = oldLines.Length;
            int y = newLines.Length;

            for (int currentD = d; currentD > 0; currentD--)
            {
                int prevK = k;
                int prevX = trace[currentD - 1][k + currentD - 1];

                if (k == -currentD || (k != currentD && trace[currentD - 1][k - 1 + currentD - 1] < trace[currentD - 1][k + 1 + currentD - 1]))
                {
                    prevK = k + 1;
                }
                else
                {
                    prevK = k - 1;
                }

                int prevY = prevX - prevK;

                // Add operations for the diagonal move (equal lines)
                while (x > prevX && y > prevY)
                {
                    x--;
                    y--;
                    operations.Insert(0, new DiffOperation(DiffOperation.OperationType.Equal, x, y, oldLines[x]));
                }

                if (currentD > 0)
                {
                    if (x > prevX)
                    {
                        // Delete operation
                        x--;
                        operations.Insert(0, new DiffOperation(DiffOperation.OperationType.Delete, x, y, oldLines[x]));
                    }
                    else if (y > prevY)
                    {
                        // Insert operation
                        y--;
                        operations.Insert(0, new DiffOperation(DiffOperation.OperationType.Insert, x, y, newLines[y]));
                    }
                }

                k = prevK;
            }

            // Add remaining equal operations at the beginning
            while (x > 0 && y > 0 && oldLines[x - 1] == newLines[y - 1])
            {
                x--;
                y--;
                operations.Insert(0, new DiffOperation(DiffOperation.OperationType.Equal, x, y, oldLines[x]));
            }

            return operations;
        }

        /// <summary>
        /// Represents a hunk of changes in the context diff
        /// </summary>
        private class ContextHunk
        {
            public int OldStart { get; set; }
            public int OldCount { get; set; }
            public int NewStart { get; set; }
            public int NewCount { get; set; }
            public int StartOpIndex { get; set; }
            public int EndOpIndex { get; set; }
        }

        /// <summary>
        /// Find hunks of changes with context lines (matching Python difflib.context_diff default context=3)
        /// </summary>
        private static System.Collections.Generic.List<ContextHunk> FindContextHunks(System.Collections.Generic.List<DiffOperation> operations, int oldCount, int newCount)
        {
            var hunks = new System.Collections.Generic.List<ContextHunk>();
            const int CONTEXT_LINES = 3; // Matching Python difflib.context_diff default

            int i = 0;
            while (i < operations.Count)
            {
                // Skip equal operations until we find a change
                while (i < operations.Count && operations[i].Type == DiffOperation.OperationType.Equal)
                {
                    i++;
                }

                if (i >= operations.Count)
                {
                    break;
                }

                // Found the start of a change hunk
                var hunk = new ContextHunk();
                hunk.StartOpIndex = System.Math.Max(0, i - CONTEXT_LINES);

                // Find the end of this change hunk
                int changeStart = i;
                while (i < operations.Count && operations[i].Type != DiffOperation.OperationType.Equal)
                {
                    i++;
                }
                int changeEnd = i - 1;

                hunk.EndOpIndex = System.Math.Min(operations.Count - 1, changeEnd + CONTEXT_LINES);

                // Calculate line numbers (1-based for context diff format)
                hunk.OldStart = 1;
                hunk.NewStart = 1;

                for (int j = 0; j < hunk.StartOpIndex; j++)
                {
                    if (operations[j].Type == DiffOperation.OperationType.Equal || operations[j].Type == DiffOperation.OperationType.Delete)
                    {
                        hunk.OldStart++;
                    }
                    if (operations[j].Type == DiffOperation.OperationType.Equal || operations[j].Type == DiffOperation.OperationType.Insert)
                    {
                        hunk.NewStart++;
                    }
                }

                // Count lines in hunk
                hunk.OldCount = 0;
                hunk.NewCount = 0;

                for (int j = hunk.StartOpIndex; j <= hunk.EndOpIndex; j++)
                {
                    if (operations[j].Type == DiffOperation.OperationType.Equal || operations[j].Type == DiffOperation.OperationType.Delete)
                    {
                        hunk.OldCount++;
                    }
                    if (operations[j].Type == DiffOperation.OperationType.Equal || operations[j].Type == DiffOperation.OperationType.Insert)
                    {
                        hunk.NewCount++;
                    }
                }

                hunks.Add(hunk);
            }

            return hunks;
        }

        /// <summary>
        /// Format diff operations as context diff output with proper hunks
        /// Matching Python difflib.context_diff format:
        /// - Header: *** oldFile / --- newFile
        /// - Hunk separator: ***************
        /// - Hunk header: *** oldStart,oldCount **** / --- newStart,newCount ----
        /// - Lines: '  ' (context), '- ' (deleted), '! ' (changed), '+ ' (added)
        /// </summary>
        private static string[] FormatContextDiff(System.Collections.Generic.List<DiffOperation> operations, string[] oldLines, string[] newLines, string fromFile, string toFile)
        {
            var result = new System.Collections.Generic.List<string>();

            if (operations.Count == 0)
            {
                return result.ToArray();
            }

            // Find hunks (groups of changes with context)
            var hunks = FindContextHunks(operations, oldLines.Length, newLines.Length);

            if (hunks.Count == 0)
            {
                return result.ToArray();
            }

            // Add diff headers (matching Python difflib.context_diff format)
            result.Add($"*** {fromFile}");
            result.Add($"--- {toFile}");

            // Format each hunk
            for (int hunkIndex = 0; hunkIndex < hunks.Count; hunkIndex++)
            {
                var hunk = hunks[hunkIndex];

                // Add hunk separator (except before first hunk)
                if (hunkIndex > 0)
                {
                    result.Add("***************");
                }

                // Add hunk headers (matching Python difflib.context_diff format)
                result.Add($"*** {hunk.OldStart},{hunk.OldCount} ****");
                result.AddRange(FormatContextHunkOld(hunk, operations, oldLines));
                result.Add($"--- {hunk.NewStart},{hunk.NewCount} ----");
                result.AddRange(FormatContextHunkNew(hunk, operations, newLines));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Format the old file portion of a context diff hunk
        /// Uses '  ' for context, '- ' for deleted, '! ' for changed lines
        /// </summary>
        private static System.Collections.Generic.List<string> FormatContextHunkOld(ContextHunk hunk, System.Collections.Generic.List<DiffOperation> operations, string[] oldLines)
        {
            var lines = new System.Collections.Generic.List<string>();

            for (int i = hunk.StartOpIndex; i <= hunk.EndOpIndex; i++)
            {
                var op = operations[i];
                string line = op.Line.TrimEnd('\r', '\n');

                switch (op.Type)
                {
                    case DiffOperation.OperationType.Equal:
                        // Context line
                        lines.Add($"  {line}");
                        break;
                    case DiffOperation.OperationType.Delete:
                        // Deleted line
                        lines.Add($"- {line}");
                        break;
                    case DiffOperation.OperationType.Insert:
                        // For context diff, insertions in new file are shown separately
                        // But we need to mark the corresponding old line as changed if there's an insertion
                        // Check if next operation is an insert (indicating a change)
                        if (i + 1 < operations.Count && operations[i + 1].Type == DiffOperation.OperationType.Insert)
                        {
                            // This is a change (delete + insert), mark as changed
                            lines.Add($"! {line}");
                        }
                        // Otherwise, insertion doesn't appear in old file section
                        break;
                }
            }

            return lines;
        }

        /// <summary>
        /// Format the new file portion of a context diff hunk
        /// Uses '  ' for context, '+ ' for added, '! ' for changed lines
        /// </summary>
        private static System.Collections.Generic.List<string> FormatContextHunkNew(ContextHunk hunk, System.Collections.Generic.List<DiffOperation> operations, string[] newLines)
        {
            var lines = new System.Collections.Generic.List<string>();

            for (int i = hunk.StartOpIndex; i <= hunk.EndOpIndex; i++)
            {
                var op = operations[i];
                string line = op.Line.TrimEnd('\r', '\n');

                switch (op.Type)
                {
                    case DiffOperation.OperationType.Equal:
                        // Context line
                        lines.Add($"  {line}");
                        break;
                    case DiffOperation.OperationType.Delete:
                        // For context diff, deletions in old file are shown separately
                        // But we need to mark the corresponding new line as changed if there's a deletion
                        // Check if previous operation was a delete (indicating a change)
                        if (i > 0 && operations[i - 1].Type == DiffOperation.OperationType.Delete)
                        {
                            // This is a change (delete + insert), mark as changed
                            lines.Add($"! {line}");
                        }
                        // Otherwise, deletion doesn't appear in new file section
                        break;
                    case DiffOperation.OperationType.Insert:
                        // Added line
                        lines.Add($"+ {line}");
                        break;
                }
            }

            return lines;
        }
    }
}

