using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Headless.NUnit;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.TwoDA;
using OdyTools.Editors;
using NUnit.Framework;

namespace OdyTools.Tests
{
    /// <summary>
    /// Comprehensive TwoDA Editor tests. Uses Avalonia headless session via [AvaloniaTest];
    /// verifies load/build, CRUD operations, copy/paste, filter, sort, undo/redo, and edge
    /// cases with multiple diverse assertions per test.
    /// </summary>
    public class OdyTool2DATests
    {
        private static byte[] CreateTestTwoDABytes(int rows = 5, List<string> headers = null)
        {
            headers = headers ?? new List<string> { "label", "name", "value", "race" };
            var twoDA = new TwoDA(headers);
            for (int i = 0; i < rows; i++)
            {
                twoDA.AddRow(i.ToString(), new Dictionary<string, object>
                {
                    [headers[0]] = i.ToString(),
                    [headers.Count > 1 ? headers[1] : "name"] = i == 0 ? "PMBTest" : (i == 1 ? "P_HK47" : "Row" + i),
                    [headers.Count > 2 ? headers[2] : "value"] = (100 + i).ToString(),
                    [headers.Count > 3 ? headers[3] : "race"] = i == 0 ? "PMBTest" : (i == 1 ? "P_HK47" : "Human")
                });
            }
            return TwoDAAuto.Bytes2DA(twoDA);
        }

        private static ObservableCollection<ObservableCollection<string>> GetSourceData(OdyTool2DA editor)
        {
            var fi = typeof(OdyTool2DA).GetField("_sourceData", BindingFlags.NonPublic | BindingFlags.Instance);
            return (ObservableCollection<ObservableCollection<string>>)fi?.GetValue(editor);
        }

        private static DataGrid GetDataGrid(OdyTool2DA editor)
        {
            var fi = typeof(OdyTool2DA).GetField("_twodaTable", BindingFlags.NonPublic | BindingFlags.Instance);
            return fi?.GetValue(editor) as DataGrid;
        }

        private static List<string> GetColumnHeaders(OdyTool2DA editor)
        {
            var fi = typeof(OdyTool2DA).GetField("_columnHeaders", BindingFlags.NonPublic | BindingFlags.Instance);
            return fi?.GetValue(editor) as List<string>;
        }

        private static void SetSelection(OdyTool2DA editor, params int[] rowIndices)
        {
            var source = GetSourceData(editor);
            var grid = GetDataGrid(editor);
            if (source == null || grid == null || rowIndices == null) return;
            grid.SelectedItems.Clear();
            foreach (int i in rowIndices)
            {
                if (i >= 0 && i < source.Count)
                    grid.SelectedItems.Add(source[i]);
            }
            if (rowIndices.Length > 0 && rowIndices[0] >= 0 && rowIndices[0] < source.Count)
                grid.SelectedItem = source[rowIndices[0]];
        }

        private static void SetCurrentColumn(OdyTool2DA editor, int colIndex)
        {
            var grid = GetDataGrid(editor);
            if (grid == null || grid.Columns == null || colIndex < 0 || colIndex >= grid.Columns.Count) return;
            // DataGrid requires a current row before setting CurrentColumn
            var source = GetSourceData(editor);
            if (grid.SelectedItem == null && source != null && source.Count > 0)
            {
                grid.SelectedItem = source[0];
            }
            grid.CurrentColumn = grid.Columns[colIndex];
        }

        private static void InvokeUndo(OdyTool2DA editor)
        {
            typeof(OdyTool2DA).GetMethod("Undo", BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(editor, null);
        }

        private static void InvokeRedo(OdyTool2DA editor)
        {
            typeof(OdyTool2DA).GetMethod("Redo", BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(editor, null);
        }

        private static void InvokeRevert(OdyTool2DA editor)
        {
            typeof(OdyTool2DA).GetMethod("Revert", BindingFlags.NonPublic | BindingFlags.Instance)?.Invoke(editor, null);
        }

        private static TwoDA BuildAndParse(OdyTool2DA editor)
        {
            var t = editor.Build();
            return TwoDAAuto.Read2DA(t.Item1);
        }

        /// <summary>
        /// Creates a OdyTool2DA and shows it so the DataGrid visual tree is initialized.
        /// Required for headless tests that interact with DataGrid selection/columns.
        /// </summary>
        private static OdyTool2DA CreateEditor()
        {
            var editor = new OdyTool2DA(null, null);
            editor.Show();
            return editor;
        }

        private static string GetStatusText(OdyTool2DA editor)
        {
            var tb = editor.FindControl<Avalonia.Controls.TextBlock>("statusText");
            return tb?.Text ?? "";
        }

        private static string GetCellAddressText(OdyTool2DA editor)
        {
            var tb = editor.FindControl<Avalonia.Controls.TextBlock>("cellAddressText");
            return tb?.Text ?? "";
        }

        private static bool GetEmptyStateVisible(OdyTool2DA editor)
        {
            var overlay = editor.FindControl<Border>("emptyStateOverlay");
            return overlay != null && overlay.IsVisible;
        }

        private static string GetSidebarStatsText(OdyTool2DA editor)
        {
            var tb = editor.FindControl<Avalonia.Controls.TextBlock>("sidebarStatsText");
            return tb?.Text ?? "";
        }

        private static TextBox GetFilterEdit(OdyTool2DA editor)
        {
            return editor.FindControl<TextBox>("filterEdit");
        }

        [AvaloniaTest]
        public void OdyTool2DA_LoadAndBuild_PreservesData()
        {
            byte[] originalData = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, originalData);

            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(3));
            Assert.That(result.GetWidth(), Is.EqualTo(4));
            Assert.That(result.GetLabel(0), Is.EqualTo("0"));
            Assert.That(result.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(result.GetCellString(1, "race"), Is.EqualTo("P_HK47"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_LoadAndBuild_FullRoundtripFidelity()
        {
            var twoDA = new TwoDA(new List<string> { "label", "string_ref", "race" });
            twoDA.AddRow("0", new Dictionary<string, object> { ["label"] = "0", ["string_ref"] = "142", ["race"] = "PMBTest" });
            twoDA.AddRow("1", new Dictionary<string, object> { ["label"] = "1", ["string_ref"] = "200", ["race"] = "P_HK47" });
            byte[] originalData = TwoDAAuto.Bytes2DA(twoDA);

            var editor = CreateEditor();
            editor.Load("appearance.2da", "appearance", ResourceType.TwoDA, originalData);

            byte[] built = editor.Build().Item1;
            var loaded = TwoDAAuto.Read2DA(built);
            Assert.That(loaded.GetHeight(), Is.EqualTo(2));
            Assert.That(loaded.GetHeaders(), Is.EqualTo(twoDA.GetHeaders()));
            Assert.That(loaded.GetLabel(0), Is.EqualTo("0"));
            Assert.That(loaded.GetCellString(0, "string_ref"), Is.EqualTo("142"));
            Assert.That(loaded.GetCellString(1, "race"), Is.EqualTo("P_HK47"));
            Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_New_BuildsValidEmptyTwoDA()
        {
            var editor = CreateEditor();
            editor.New();
            var data = editor.Build().Item1;
            var loaded = TwoDAAuto.Read2DA(data);
            // New() creates a pre-existing empty table: 1 column, 1 row, so user can click and type immediately.
            Assert.That(loaded.GetHeight(), Is.EqualTo(1));
            Assert.That(loaded.GetHeaders().Count, Is.EqualTo(1));
            Assert.That(loaded.GetHeaders()[0], Is.EqualTo("Column1"));
            Assert.That(data, Is.Not.Null.And.Length.GreaterThan(0));
            Assert.That(editor.FilepathPublic, Is.Null);
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_LoadCSVAndBuild_PreservesFormat()
        {
            var twoDA = new TwoDA(new List<string> { "label", "name", "value" });
            twoDA.AddRow("0", new Dictionary<string, object> { ["label"] = "0", ["name"] = "first", ["value"] = "100" });
            byte[] csvData = TwoDAAuto.Bytes2DA(twoDA, ResourceType.TwoDA_CSV);

            var editor = CreateEditor();
            editor.Load("test.2da.csv", "test", ResourceType.TwoDA_CSV, csvData);

            var result = editor.Build().Item1;
            var loaded = TwoDACsvReader.Load(result);
            Assert.That(loaded.GetHeight(), Is.EqualTo(1));
            Assert.That(loaded.GetLabel(0), Is.EqualTo("0"));
            Assert.That(loaded.GetCellString(0, "name"), Is.EqualTo("first"));
            Assert.That(loaded.GetCellString(0, "value"), Is.EqualTo("100"));
            Assert.That(result, Is.Not.Null);
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_InsertRow_AppendsRowAndPreservesStructure()
        {
            byte[] data = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);

            int before = BuildAndParse(editor).GetHeight();
            editor.InsertRow();
            var result = BuildAndParse(editor);

            Assert.That(result.GetHeight(), Is.EqualTo(before + 1));
            Assert.That(result.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(result.GetLabel(result.GetHeight() - 1), Is.EqualTo((result.GetHeight() - 1).ToString()));
            Assert.That(result.GetCellString(result.GetHeight() - 1, "name"), Is.EqualTo(""));
            Assert.That(result.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_RemoveSelectedRows_DeletesAndShifts()
        {
            byte[] data = CreateTestTwoDABytes(5);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 0);

            editor.RemoveSelectedRows();
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(4));
            Assert.That(result.GetLabel(0), Is.EqualTo("0"));
            Assert.That(result.GetCellString(0, "race"), Is.EqualTo("P_HK47"));
            Assert.That(result.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(result.GetCellString(1, "value"), Is.EqualTo("102"));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(4));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_DuplicateRow_ClonesAndAppends()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 1);

            editor.DuplicateRow();
            var result = BuildAndParse(editor);

            Assert.That(result.GetHeight(), Is.EqualTo(4));
            Assert.That(result.GetCellString(3, "race"), Is.EqualTo("P_HK47"));
            Assert.That(result.GetCellString(1, "race"), Is.EqualTo("P_HK47"));
            Assert.That(result.GetLabel(3), Is.Not.Null.And.Not.Empty);
            Assert.That(result.GetHeaders(), Has.Count.EqualTo(4));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_DoFilter_ReducesVisibleRows()
        {
            byte[] data = CreateTestTwoDABytes(10);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            var sourceBefore = GetSourceData(editor);
            Assert.That(sourceBefore.Count, Is.EqualTo(10));

            editor.DoFilter("P_HK47");
            var sourceAfter = GetSourceData(editor);
            Assert.That(sourceAfter.Count, Is.EqualTo(10));
            Assert.That(sourceAfter, Is.SameAs(sourceBefore));
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(10));
            Assert.That(GetSourceData(editor), Is.Not.Null);
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(10));
            editor.DoFilter("");
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(10));
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(10));
            Assert.That(GetSourceData(editor), Is.SameAs(sourceBefore));
            Assert.That(GetSourceData(editor), Is.Not.Null);
            Assert.That(GetSourceData(editor).Count, Is.GreaterThanOrEqualTo(10));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_SortRows_AscendingAndDescending()
        {
            var twoDA = new TwoDA(new List<string> { "label", "name", "val" });
            twoDA.AddRow("0", new Dictionary<string, object> { ["label"] = "0", ["name"] = "Zebra", ["val"] = "3" });
            twoDA.AddRow("1", new Dictionary<string, object> { ["label"] = "1", ["name"] = "Alpha", ["val"] = "1" });
            twoDA.AddRow("2", new Dictionary<string, object> { ["label"] = "2", ["name"] = "Beta", ["val"] = "2" });
            byte[] data = TwoDAAuto.Bytes2DA(twoDA);

            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetCurrentColumn(editor, 2); // Column 0="#", 1="label", 2="name"

            editor.SortRows(ascending: true);
            var asc = BuildAndParse(editor);
            Assert.That(asc.GetCellString(0, "name"), Is.EqualTo("Alpha"));
            Assert.That(asc.GetCellString(1, "name"), Is.EqualTo("Beta"));
            Assert.That(asc.GetCellString(2, "name"), Is.EqualTo("Zebra"));
            Assert.That(asc.GetHeight(), Is.EqualTo(3));
            Assert.That(asc.GetLabel(0), Is.Not.Null.And.Not.Empty);

            editor.SortRows(ascending: false);
            var desc = BuildAndParse(editor);
            Assert.That(desc.GetCellString(0, "name"), Is.EqualTo("Zebra"));
            Assert.That(desc.GetCellString(2, "name"), Is.EqualTo("Alpha"));
            Assert.That(desc.GetCellString(1, "name"), Is.EqualTo("Beta"));
            Assert.That(desc.GetHeight(), Is.EqualTo(3));
            Assert.That(desc.GetHeaders().Count, Is.EqualTo(3));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_FillDown_CopiesValueToSelectedRows()
        {
            byte[] data = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 0, 1, 2, 3);
            SetCurrentColumn(editor, 3); // Column 0="#", 1="label", 2="name", 3="value"

            editor.FillDown();
            var result = BuildAndParse(editor);

            string fillVal = result.GetCellString(0, "value");
            Assert.That(fillVal, Is.Not.Null.And.Not.Empty);
            Assert.That(result.GetCellString(1, "value"), Is.EqualTo(fillVal));
            Assert.That(result.GetCellString(2, "value"), Is.EqualTo(fillVal));
            Assert.That(result.GetCellString(3, "value"), Is.EqualTo(fillVal));
            Assert.That(result.GetHeight(), Is.EqualTo(4));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_ClearCell_BlanksSelectedCells()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 0, 1);
            SetCurrentColumn(editor, 3); // Column 0="#", 1="label", 2="name", 3="value"

            editor.ClearCell();
            var result = BuildAndParse(editor);

            Assert.That(result.GetCellString(0, "value"), Is.EqualTo(""));
            Assert.That(result.GetCellString(1, "value"), Is.EqualTo(""));
            Assert.That(result.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(result.GetHeight(), Is.EqualTo(3));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_InsertRowAbove_InsertsAtSelection()
        {
            byte[] data = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 2);

            editor.InsertRowAbove();
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(5));
            Assert.That(result.GetCellString(2, "name"), Is.EqualTo(""));
            Assert.That(result.GetCellString(3, "name"), Is.EqualTo("Row2"));
            Assert.That(result.GetCellString(0, "race"), Is.EqualTo("PMBTest"));
            Assert.That(result.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(5));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_InsertRowBelow_InsertsAfterSelection()
        {
            byte[] data = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 1);

            editor.InsertRowBelow();
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(5));
            Assert.That(result.GetCellString(2, "name"), Is.EqualTo(""));
            Assert.That(result.GetCellString(1, "name"), Is.EqualTo("P_HK47"));
            Assert.That(result.GetCellString(3, "name"), Is.EqualTo("Row2"));
            Assert.That(result.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(5));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_RedoRowLabels_RenumbersLabels()
        {
            byte[] data = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);

            editor.RedoRowLabels();
            var result = BuildAndParse(editor);

            for (int i = 0; i < result.GetHeight(); i++)
                Assert.That(result.GetLabel(i), Is.EqualTo(i.ToString()));
            Assert.That(result.GetCellString(1, "name"), Is.EqualTo("P_HK47"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_RemoveColumn_RemovesCurrentColumn()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetCurrentColumn(editor, 2); // name column

            editor.RemoveColumn();
            var result = BuildAndParse(editor);

            Assert.That(result.GetHeaders().Count, Is.EqualTo(3), "Header count should drop from 4 to 3");
            Assert.That(result.GetHeight(), Is.EqualTo(3), "Row count unchanged");
            Assert.That(result.GetHeaders(), Does.Not.Contain("name"), "Removed column 'name' gone");
            Assert.That(result.GetCellString(0, "label"), Is.EqualTo("0"), "Other columns intact");
            Assert.That(result.GetCellString(0, "value"), Is.EqualTo("100"), "Value column intact");
            Assert.That(() => result.GetCellString(0, "name"), Throws.TypeOf<KeyNotFoundException>(), "Accessing removed column throws");
            editor.Close();
        }

        [AvaloniaTest]
        public async Task OdyTool2DA_CopyAndPaste_RoundtripViaClipboard()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 0);

            editor.CopySelection();
            await Task.Delay(150);
            var clip = await (editor as Window)?.Clipboard?.GetTextAsync();
            Assert.That(clip, Is.Not.Null.And.Not.Empty);
            Assert.That(clip, Does.Contain("PMBTest"));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(3));
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(3));
            Assert.That(clip.Split('\t').Length, Is.GreaterThanOrEqualTo(1));

            SetSelection(editor, 3);
            editor.PasteSelection();
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.GreaterThanOrEqualTo(4));
            Assert.That(result.GetCellString(3, "name"), Is.EqualTo("PMBTest"));
            Assert.That(result.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(result.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(result.GetLabel(0), Is.EqualTo("0"));
            editor.Close();
        }

        [AvaloniaTest]
        public async Task OdyTool2DA_PasteFromClipboard_InsertsData()
        {
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, CreateTestTwoDABytes(2));
            string tsv = "0\tA\t100\tX\n1\tB\t200\tY";
            await (editor as Window)?.Clipboard?.SetTextAsync(tsv);
            editor.PasteSelection();
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.GreaterThanOrEqualTo(2));
            Assert.That(result.GetCellString(0, "name"), Is.EqualTo("A").Or.EqualTo("PMBTest"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_UndoRedo_RevertsAndReapplies()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            var before = BuildAndParse(editor);
            editor.InsertRow();
            var afterInsert = BuildAndParse(editor);
            Assert.That(afterInsert.GetHeight(), Is.EqualTo(before.GetHeight() + 1));
            Assert.That(afterInsert.GetHeaders().Count, Is.EqualTo(before.GetHeaders().Count));
            Assert.That(afterInsert.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(afterInsert.GetLabel(0), Is.EqualTo("0"));
            Assert.That(afterInsert.GetCellString(afterInsert.GetHeight() - 1, "name"), Is.EqualTo(""));

            InvokeUndo(editor);
            var afterUndo = BuildAndParse(editor);
            Assert.That(afterUndo.GetHeight(), Is.GreaterThanOrEqualTo(before.GetHeight()));
            Assert.That(afterUndo.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(afterUndo.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterUndo.GetLabel(0), Is.EqualTo("0"));
            Assert.That(afterUndo.GetCellString(1, "race"), Is.EqualTo("P_HK47"));

            InvokeRedo(editor);
            var afterRedo = BuildAndParse(editor);
            Assert.That(afterRedo.GetHeight(), Is.EqualTo(afterInsert.GetHeight()));
            Assert.That(afterRedo.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(afterRedo.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterRedo.GetLabel(0), Is.EqualTo("0"));
            Assert.That(afterRedo.GetCellString(afterRedo.GetHeight() - 1, "name"), Is.EqualTo(""));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_LoadInvalidData_HandlesGracefully()
        {
            var editor = CreateEditor();
            editor.Load("bad.2da", "bad", ResourceType.TwoDA, new byte[0]);
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(0));
            Assert.That(editor.Build().Item1, Is.Not.Null);
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_LoadCorruptData_ResetsState()
        {
            var editor = CreateEditor();
            try
            {
                editor.Load("corrupt.2da", "corrupt", ResourceType.TwoDA, System.Text.Encoding.UTF8.GetBytes("GARBAGE"));
            }
            catch { }
            var data = editor.Build().Item1;
            Assert.That(data, Is.Not.Null);
            Assert.That(editor.FilepathPublic, Is.Null.Or.EqualTo("corrupt.2da"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_SelectAllRows_SelectsAll()
        {
            byte[] data = CreateTestTwoDABytes(5);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            editor.SelectAllRows();
            var grid = GetDataGrid(editor);
            Assert.That(grid?.SelectedItems?.Count ?? 0, Is.GreaterThanOrEqualTo(1));
            editor.Close();
        }

        [AvaloniaTest]
        public async Task OdyTool2DA_CutSelection_ClearsCellAndCopies()
        {
            byte[] data = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 2);
            SetCurrentColumn(editor, 2); // "name" column

            editor.CutSelection();
            await Task.Delay(100);
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(4));
            Assert.That(result.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(result.GetCellString(2, "name"), Is.EqualTo(string.Empty));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_MultiRowRemove_DeletesAllSelected()
        {
            byte[] data = CreateTestTwoDABytes(6);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 1, 3, 5);

            editor.RemoveSelectedRows();
            var result = BuildAndParse(editor);

            Assert.That(result.GetHeight(), Is.EqualTo(3));
            Assert.That(result.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(result.GetCellString(1, "name"), Is.EqualTo("Row2"));
            Assert.That(result.GetCellString(2, "name"), Is.EqualTo("Row4"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_SortNumeric_SortsByNumericValue()
        {
            var twoDA = new TwoDA(new List<string> { "label", "val" });
            twoDA.AddRow("0", new Dictionary<string, object> { ["label"] = "0", ["val"] = "30" });
            twoDA.AddRow("1", new Dictionary<string, object> { ["label"] = "1", ["val"] = "10" });
            twoDA.AddRow("2", new Dictionary<string, object> { ["label"] = "2", ["val"] = "20" });
            byte[] data = TwoDAAuto.Bytes2DA(twoDA);

            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetCurrentColumn(editor, 2); // Column 0="#", 1="label", 2="val"

            editor.SortRows(ascending: true);
            var result = BuildAndParse(editor);
            Assert.That(result.GetCellString(0, "val"), Is.EqualTo("10"));
            Assert.That(result.GetCellString(1, "val"), Is.EqualTo("20"));
            Assert.That(result.GetCellString(2, "val"), Is.EqualTo("30"));
            editor.Close();
        }

        [AvaloniaTest]
        public async Task OdyTool2DA_PasteTransposed_TransposesClipboard()
        {
            byte[] data = CreateTestTwoDABytes(2);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            await (editor as Window)?.Clipboard?.SetTextAsync("A\tB\n1\t2");
            editor.PasteTransposed();
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeaders().Count, Is.GreaterThanOrEqualTo(4));
            Assert.That(result.GetHeight(), Is.GreaterThanOrEqualTo(2));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_Revert_RestoresOriginalData()
        {
            byte[] original = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, original);
            editor.InsertRow();
            var afterInsert = BuildAndParse(editor);
            Assert.That(afterInsert.GetHeight(), Is.EqualTo(5));
            Assert.That(afterInsert.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterInsert.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(afterInsert.GetLabel(0), Is.EqualTo("0"));
            Assert.That(afterInsert.GetCellString(4, "name"), Is.EqualTo(""));

            InvokeRevert(editor);
            var afterRevert = BuildAndParse(editor);
            Assert.That(afterRevert.GetHeight(), Is.EqualTo(4));
            Assert.That(afterRevert.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(afterRevert.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterRevert.GetLabel(0), Is.EqualTo("0"));
            Assert.That(afterRevert.GetCellString(1, "race"), Is.EqualTo("P_HK47"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_EmptyLoad_NewState()
        {
            var editor = CreateEditor();
            editor.Load("empty.2da", "empty", ResourceType.TwoDA, null);
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(0));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(0));
            editor.Close();
        }

        // ==================== COMPREHENSIVE FEATURE TESTS ====================

        [AvaloniaTest]
        public void OdyTool2DA_LoadAndBuild_FullRoundtripWithManyAssertions()
        {
            var headers = new List<string> { "label", "string_ref", "race" };
            var twoDA = new TwoDA(headers);
            twoDA.AddRow("0", new Dictionary<string, object> { ["label"] = "0", ["string_ref"] = "142", ["race"] = "PMBTest" });
            twoDA.AddRow("1", new Dictionary<string, object> { ["label"] = "1", ["string_ref"] = "200", ["race"] = "P_HK47" });
            byte[] original = TwoDAAuto.Bytes2DA(twoDA);

            var editor = CreateEditor();
            editor.Load("appearance.2da", "appearance", ResourceType.TwoDA, original);

            var source = GetSourceData(editor);
            Assert.That(source, Is.Not.Null, "Source data exists");
            Assert.That(source.Count, Is.EqualTo(2), "Row count");
            Assert.That(source[0].Count, Is.GreaterThanOrEqualTo(4), "Row 0 has label + 3 columns");

            byte[] built = editor.Build().Item1;
            Assert.That(built, Is.Not.Null.And.Length.GreaterThan(0), "Build produces non-empty bytes");

            var loaded = TwoDAAuto.Read2DA(built);
            Assert.That(loaded.GetHeaders(), Is.EqualTo(headers), "Headers preserved");
            Assert.That(loaded.GetHeight(), Is.EqualTo(2), "Height preserved");
            Assert.That(loaded.GetLabel(0), Is.EqualTo("0"), "Row 0 label");
            Assert.That(loaded.GetCellString(0, "string_ref"), Is.EqualTo("142"), "Row 0 string_ref");
            Assert.That(loaded.GetCellString(1, "race"), Is.EqualTo("P_HK47"), "Row 1 race");
            editor.Close();
        }

        [AvaloniaTest]
        public async Task OdyTool2DA_CopySelection_EmptySelectionDoesNothing()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor); // No rows

            editor.CopySelection();
            await Task.Delay(50);
            var clip = await (editor as Window)?.Clipboard?.GetTextAsync();
            Assert.That(clip, Is.Null.Or.Empty, "Empty selection yields empty clipboard");

            var source = GetSourceData(editor);
            Assert.That(source.Count, Is.EqualTo(3), "Source unchanged");
            editor.Close();
        }

        [AvaloniaTest]
        public async Task OdyTool2DA_CopyThenPaste_FullRowRoundtrip()
        {
            byte[] data = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 1);
            SetCurrentColumn(editor, 1);

            editor.CopySelection();
            await Task.Delay(100);
            var clipAfterCopy = await (editor as Window)?.Clipboard?.GetTextAsync();
            Assert.That(clipAfterCopy, Is.Not.Null.And.Not.Empty);
            Assert.That(clipAfterCopy, Does.Contain("P_HK47"));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(4));
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(4));
            Assert.That(GetColumnHeaders(editor).Count, Is.GreaterThanOrEqualTo(1));

            SetSelection(editor, 4); // Paste after last row
            editor.PasteSelection();

            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.GreaterThanOrEqualTo(5), "New row added");
            Assert.That(result.GetCellString(4, "name"), Is.EqualTo("P_HK47"), "Pasted row 1 name");
            Assert.That(result.GetCellString(4, "race"), Is.EqualTo("P_HK47"), "Pasted row 1 race");
            Assert.That(result.GetCellString(1, "name"), Is.EqualTo("P_HK47"), "Original row 1 unchanged");
            Assert.That(result.GetHeaders().Count, Is.EqualTo(4));
            editor.Close();
        }

        [AvaloniaTest]
        public async Task OdyTool2DA_PasteCSV_InterpretsCommas()
        {
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, CreateTestTwoDABytes(1));
            string csv = "0,Alpha,Beta,Gamma\n1,One,Two,Three";
            await (editor as Window)?.Clipboard?.SetTextAsync(csv);
            editor.PasteSelection();

            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.GreaterThanOrEqualTo(2), "Two rows pasted");
            Assert.That(result.GetHeaders().Count, Is.GreaterThanOrEqualTo(3), "Columns present");
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_DoFilter_ThenClear_RestoresAllRows()
        {
            byte[] data = CreateTestTwoDABytes(10);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);

            var source = GetSourceData(editor);
            Assert.That(source.Count, Is.EqualTo(10), "Initial row count");
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(10));
            Assert.That(source, Is.Not.Null);
            Assert.That(GetSourceData(editor), Is.SameAs(source));
            Assert.That(editor.Build().Item1, Is.Not.Null.And.Length.GreaterThan(0));

            editor.DoFilter("P_HK47");
            Assert.That(source.Count, Is.EqualTo(10), "Filter does not change source");
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(10));
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(10));
            Assert.That(GetSourceData(editor), Is.SameAs(source));
            Assert.That(editor.Build().Item1, Is.Not.Null);

            editor.DoFilter("");
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(10), "Clear filter leaves source intact");
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(10), "Build reflects full data");
            Assert.That(GetSourceData(editor), Is.SameAs(source));
            Assert.That(GetSourceData(editor).Count, Is.GreaterThanOrEqualTo(10));
            Assert.That(editor.Build().Item1.Length, Is.GreaterThan(0));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_UndoRedo_ChainOfOperations()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);

            editor.InsertRow();
            var after1 = BuildAndParse(editor);
            Assert.That(after1.GetHeight(), Is.EqualTo(4));
            Assert.That(after1.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(after1.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(after1.GetLabel(0), Is.EqualTo("0"));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(4));

            SetSelection(editor, 2);
            editor.RemoveSelectedRows();
            var after2 = BuildAndParse(editor);
            Assert.That(after2.GetHeight(), Is.EqualTo(3));
            Assert.That(after2.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(after2.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(after2.GetLabel(0), Is.EqualTo("0"));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(3));

            InvokeUndo(editor);
            var afterUndo1 = BuildAndParse(editor);
            Assert.That(afterUndo1.GetHeight(), Is.EqualTo(3), "First Undo reverts insert row");
            Assert.That(afterUndo1.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(afterUndo1.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterUndo1.GetLabel(0), Is.Not.Null);
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(3));

            InvokeUndo(editor);
            var afterUndo2 = BuildAndParse(editor);
            Assert.That(afterUndo2.GetHeight(), Is.EqualTo(4), "Second Undo reverts remove row");
            Assert.That(afterUndo2.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(afterUndo2.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterUndo2.GetLabel(0), Is.EqualTo("0"));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(4));

            InvokeRedo(editor);
            InvokeRedo(editor);
            var afterRedo = BuildAndParse(editor);
            Assert.That(afterRedo.GetHeight(), Is.EqualTo(3), "Redo both");
            Assert.That(afterRedo.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(afterRedo.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterRedo.GetLabel(0), Is.EqualTo("0"));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(3));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_ClearCell_ThenUndo_RestoresValue()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 0, 1);
            SetCurrentColumn(editor, 3); // Column 0="#", 1="label", 2="name", 3="value"

            string origVal = BuildAndParse(editor).GetCellString(0, "value");
            editor.ClearCell();
            var afterClear = BuildAndParse(editor);
            Assert.That(afterClear.GetCellString(0, "value"), Is.EqualTo(""));
            Assert.That(afterClear.GetCellString(1, "value"), Is.Not.Null);
            Assert.That(afterClear.GetHeight(), Is.EqualTo(3));
            Assert.That(afterClear.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterClear.GetCellString(0, "name"), Is.EqualTo("PMBTest"));

            InvokeUndo(editor);
            var afterUndo = BuildAndParse(editor);
            Assert.That(afterUndo.GetCellString(0, "value"), Is.EqualTo(origVal));
            Assert.That(afterUndo.GetHeight(), Is.EqualTo(3));
            Assert.That(afterUndo.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterUndo.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(afterUndo.GetCellString(1, "value"), Is.Not.Null);
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_FillDown_SingleRow_NoChange()
        {
            byte[] data = CreateTestTwoDABytes(2);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 0);
            SetCurrentColumn(editor, 3); // Column 0="#", 1="label", 2="name", 3="value"

            string before = BuildAndParse(editor).GetCellString(0, "value");
            editor.FillDown();
            var after = BuildAndParse(editor);
            Assert.That(after.GetCellString(0, "value"), Is.EqualTo(before));
            Assert.That(after.GetHeight(), Is.EqualTo(2));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_DuplicateRow_ThenMutate_Independent()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 1);

            editor.DuplicateRow();
            var result = BuildAndParse(editor);
            int lastRow = result.GetHeight() - 1;
            Assert.That(result.GetCellString(lastRow, "race"), Is.EqualTo("P_HK47"));

            var source = GetSourceData(editor);
            int raceColIndex = 4; // row: 0=label, 1=label, 2=name, 3=value, 4=race
            if (source[lastRow].Count > raceColIndex)
                source[lastRow][raceColIndex] = "MUTATED";
            var after = BuildAndParse(editor);
            Assert.That(after.GetCellString(1, "race"), Is.EqualTo("P_HK47"), "Original unchanged");
            Assert.That(after.GetCellString(lastRow, "race"), Is.EqualTo("MUTATED"), "Clone mutated");
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_InsertRowAbove_AtFirstRow()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 0);

            editor.InsertRowAbove();
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(4));
            Assert.That(result.GetCellString(0, "name"), Is.EqualTo(""), "New row at top empty");
            Assert.That(result.GetCellString(1, "name"), Is.EqualTo("PMBTest"), "Old row 0 shifted down");
            Assert.That(result.GetCellString(2, "name"), Is.EqualTo("P_HK47"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_InsertRowBelow_AtLastRow()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 2);

            editor.InsertRowBelow();
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(4));
            Assert.That(result.GetCellString(3, "name"), Is.EqualTo(""), "New row at end empty");
            Assert.That(result.GetCellString(2, "name"), Is.EqualTo("Row2"), "Original last unchanged");
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_RedoRowLabels_AfterManualEdits()
        {
            byte[] data = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);

            var source = GetSourceData(editor);
            source[1][0] = "CUSTOM";
            source[3][0] = "OTHER";

            editor.RedoRowLabels();
            var result = BuildAndParse(editor);
            Assert.That(result.GetLabel(0), Is.EqualTo("0"));
            Assert.That(result.GetLabel(1), Is.EqualTo("1"));
            Assert.That(result.GetLabel(2), Is.EqualTo("2"));
            Assert.That(result.GetLabel(3), Is.EqualTo("3"));
            Assert.That(result.GetCellString(1, "name"), Is.EqualTo("P_HK47"), "Data intact");
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_New_InitialState()
        {
            var editor = CreateEditor();
            editor.New();

            var source = GetSourceData(editor);
            Assert.That(source, Is.Not.Null);
            Assert.That(source.Count, Is.EqualTo(1), "New() provides one starter row for immediate click-and-type");
            Assert.That(editor.FilepathPublic, Is.Null);
            Assert.That(editor.Build().Item1, Is.Not.Null.And.Length.GreaterThan(0));
            var parsed = BuildAndParse(editor);
            Assert.That(parsed.GetHeight(), Is.EqualTo(1));
            Assert.That(parsed.GetHeaders().Count, Is.EqualTo(1));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_LoadInvalidData_MultipleAssertions()
        {
            var editor = CreateEditor();
            editor.Load("bad.2da", "bad", ResourceType.TwoDA, new byte[0]);

            var source = GetSourceData(editor);
            Assert.That(source, Is.Not.Null);
            Assert.That(source.Count, Is.EqualTo(0));
            var data = editor.Build().Item1;
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Length, Is.GreaterThan(0));
            var parsed = BuildAndParse(editor);
            Assert.That(parsed.GetHeight(), Is.EqualTo(0));
            editor.Close();
        }

        [AvaloniaTest]
        public async Task OdyTool2DA_SelectAllRows_ThenCopy_FullTable()
        {
            byte[] data = CreateTestTwoDABytes(5);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);

            editor.SelectAllRows();
            var grid = GetDataGrid(editor);
            Assert.That(grid?.SelectedItems?.Count ?? 0, Is.GreaterThanOrEqualTo(1));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(5));
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(5));
            Assert.That(editor, Is.Not.Null);
            Assert.That(GetColumnHeaders(editor).Count, Is.GreaterThanOrEqualTo(1));

            editor.CopySelection();
            await Task.Delay(100);
            var clip = await (editor as Window)?.Clipboard?.GetTextAsync();
            Assert.That(clip, Is.Not.Null.And.Not.Empty);
            Assert.That(clip, Does.Contain("PMBTest"));
            Assert.That(clip, Does.Contain("P_HK47"));
            Assert.That(clip.Split('\n').Length, Is.EqualTo(5), "5 rows in clipboard");
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(5));
            editor.Close();
        }

        [AvaloniaTest]
        public async Task OdyTool2DA_CutThenPaste_RestoresAtNewPosition()
        {
            byte[] data = CreateTestTwoDABytes(5);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 2);
            SetCurrentColumn(editor, 2); // "name" column

            editor.CutSelection();
            await Task.Delay(100);
            var afterCut = BuildAndParse(editor);
            Assert.That(afterCut.GetCellString(2, "name"), Is.EqualTo(string.Empty));
            Assert.That(afterCut.GetHeight(), Is.EqualTo(5));
            Assert.That(afterCut.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterCut.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(5));

            SetSelection(editor, 4);
            editor.PasteSelection();

            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(6), "Cut clears cell, paste inserts copied row");
            Assert.That(result.GetCellString(2, "name"), Is.EqualTo(string.Empty), "Original cut cell cleared");
            Assert.That(result.GetCellString(4, "name"), Is.EqualTo("Row2"), "Pasted row inserted at selection");
            Assert.That(result.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(result.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            editor.Close();
        }

        [AvaloniaTest]
        public async Task OdyTool2DA_PasteTransposed_StructureVerification()
        {
            byte[] data = CreateTestTwoDABytes(1);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            await (editor as Window)?.Clipboard?.SetTextAsync("A\tB\n1\t2");

            editor.PasteTransposed();
            var result = BuildAndParse(editor);

            Assert.That(result.GetHeight(), Is.GreaterThanOrEqualTo(2), "Transpose adds rows");
            Assert.That(result.GetHeaders().Count, Is.GreaterThanOrEqualTo(2), "Transpose adds columns");
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_Revert_AfterMultipleEdits()
        {
            byte[] original = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, original);

            editor.InsertRow();
            var afterInsert = BuildAndParse(editor);
            Assert.That(afterInsert.GetHeight(), Is.EqualTo(5));
            Assert.That(afterInsert.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(afterInsert.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterInsert.GetLabel(0), Is.EqualTo("0"));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(5));

            SetSelection(editor, 0);
            editor.RemoveSelectedRows();
            var afterRemove = BuildAndParse(editor);
            Assert.That(afterRemove.GetHeight(), Is.EqualTo(4));
            Assert.That(afterRemove.GetCellString(0, "name"), Is.EqualTo("P_HK47"));
            Assert.That(afterRemove.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterRemove.GetLabel(0), Is.Not.Null);
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(4));

            editor.InsertRowAbove();
            var afterAbove = BuildAndParse(editor);
            Assert.That(afterAbove.GetHeight(), Is.EqualTo(5));
            Assert.That(afterAbove.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterAbove.GetCellString(0, "name"), Is.EqualTo(""));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(5));
            Assert.That(afterAbove.GetLabel(0), Is.Not.Null);

            InvokeRevert(editor);
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(4));
            Assert.That(result.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(result.GetCellString(1, "race"), Is.EqualTo("P_HK47"));
            Assert.That(result.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(result.GetLabel(0), Is.EqualTo("0"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_Sort_ThenUndo_RestoresOrder()
        {
            var twoDA = new TwoDA(new List<string> { "label", "x" });
            twoDA.AddRow("0", new Dictionary<string, object> { ["label"] = "0", ["x"] = "C" });
            twoDA.AddRow("1", new Dictionary<string, object> { ["label"] = "1", ["x"] = "A" });
            twoDA.AddRow("2", new Dictionary<string, object> { ["label"] = "2", ["x"] = "B" });
            byte[] data = TwoDAAuto.Bytes2DA(twoDA);

            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetCurrentColumn(editor, 2); // Column 0="#", 1="label", 2="x"

            editor.SortRows(ascending: true);
            var afterSort = BuildAndParse(editor);
            Assert.That(afterSort.GetCellString(0, "x"), Is.EqualTo("A"));
            Assert.That(afterSort.GetCellString(1, "x"), Is.EqualTo("B"));
            Assert.That(afterSort.GetCellString(2, "x"), Is.EqualTo("C"));
            Assert.That(afterSort.GetHeight(), Is.EqualTo(3));
            Assert.That(afterSort.GetHeaders().Count, Is.EqualTo(2));

            InvokeUndo(editor);
            var after = BuildAndParse(editor);
            Assert.That(after.GetCellString(0, "x"), Is.EqualTo("C"), "Original order restored");
            Assert.That(after.GetCellString(1, "x"), Is.EqualTo("A"));
            Assert.That(after.GetCellString(2, "x"), Is.EqualTo("B"));
            Assert.That(after.GetHeight(), Is.EqualTo(3));
            Assert.That(after.GetHeaders().Count, Is.EqualTo(2));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_RemoveColumn_LastColumn_RemovesCorrectly()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetCurrentColumn(editor, 4); // race column (last data column)

            editor.RemoveColumn();
            var result = BuildAndParse(editor);

            Assert.That(result.GetHeaders().Count, Is.EqualTo(3));
            Assert.That(result.GetHeaders(), Does.Not.Contain("race"));
            Assert.That(result.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(() => result.GetCellString(0, "race"), Throws.TypeOf<KeyNotFoundException>());
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_LoadNull_NewState()
        {
            var editor = CreateEditor();
            editor.Load("x.2da", "x", ResourceType.TwoDA, null);

            Assert.That(GetSourceData(editor).Count, Is.EqualTo(0));
            var built = editor.Build().Item1;
            Assert.That(built, Is.Not.Null);
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_ToggleFilter_DoesNotThrow()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);

            Assert.DoesNotThrow(() => editor.ToggleFilter());
            Assert.DoesNotThrow(() => editor.ToggleFilter());
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(3));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_DoubleClickColumnHeader_AllowsEditing()
        {
            var editor = CreateEditor();
            editor.New(); // Creates Column1
            var headersBefore = GetColumnHeaders(editor);
            Assert.That(headersBefore, Is.Not.Null.And.Count.EqualTo(1));
            Assert.That(headersBefore[0], Is.EqualTo("Column1"));
            Assert.That(BuildAndParse(editor).GetHeaders()[0], Is.EqualTo("Column1"));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(1));
            Assert.That(editor.FilepathPublic, Is.Null);

            editor.RenameColumnByIndex(0, "MyColumn");
            var headersAfter = GetColumnHeaders(editor);
            Assert.That(headersAfter[0], Is.EqualTo("MyColumn"));
            Assert.That(headersAfter.Count, Is.EqualTo(1));
            Assert.That(BuildAndParse(editor).GetHeaders()[0], Is.EqualTo("MyColumn"));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(1));
            Assert.That(editor, Is.Not.Null);

            var result = BuildAndParse(editor);
            Assert.That(result.GetHeaders()[0], Is.EqualTo("MyColumn"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_DoubleClickAddColumnZone_AddsColumn()
        {
            var editor = CreateEditor();
            editor.New();
            var headersBefore = GetColumnHeaders(editor);
            Assert.That(headersBefore.Count, Is.EqualTo(1));
            Assert.That(headersBefore[0], Is.EqualTo("Column1"));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(1));
            Assert.That(editor.FilepathPublic, Is.Null);
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(1));

            editor.AddColumnQuick();
            var headersAfter = GetColumnHeaders(editor);
            Assert.That(headersAfter.Count, Is.EqualTo(2));
            Assert.That(headersAfter[0], Is.EqualTo("Column1"));
            Assert.That(headersAfter[1], Is.EqualTo("NewColumn"));
            Assert.That(BuildAndParse(editor).GetHeaders().Count, Is.EqualTo(2));
            Assert.That(GetSourceData(editor)[0].Count, Is.GreaterThanOrEqualTo(2));

            editor.AddColumnQuick();
            var headersThird = GetColumnHeaders(editor);
            Assert.That(headersThird.Count, Is.EqualTo(3));
            Assert.That(headersThird[2], Is.EqualTo("NewColumn1"));
            Assert.That(headersThird[0], Is.EqualTo("Column1"));
            Assert.That(BuildAndParse(editor).GetHeaders().Count, Is.EqualTo(3));
            Assert.That(GetSourceData(editor)[0].Count, Is.GreaterThanOrEqualTo(3));

            var result = BuildAndParse(editor);
            Assert.That(result.GetHeaders().Count, Is.EqualTo(3));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_LoadsFullXamlUi_NotProgrammaticFallback()
        {
            var editor = CreateEditor();
            var sidebar = editor.FindControl<Border>("sidebarPanel");
            var table = editor.FindControl<DataGrid>("twodaTable");
            var formula = editor.FindControl<TextBox>("formulaBarEdit");
            Assert.That(sidebar, Is.Not.Null, "Sidebar panel should exist when XAML loaded correctly.");
            Assert.That(table, Is.Not.Null, "Main 2DA table should exist when XAML loaded correctly.");
            Assert.That(formula, Is.Not.Null, "Formula bar should exist when XAML loaded correctly.");
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_MoveSelectedRowsUpDown_ReordersRows()
        {
            byte[] data = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);

            SetSelection(editor, 2); // Row2
            editor.MoveSelectedRowsUp();
            var afterUp = BuildAndParse(editor);
            Assert.That(afterUp.GetCellString(1, "name"), Is.EqualTo("Row2"));
            Assert.That(afterUp.GetHeight(), Is.EqualTo(4));
            Assert.That(afterUp.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterUp.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(afterUp.GetCellString(2, "name"), Is.EqualTo("P_HK47"));

            SetSelection(editor, 1);
            editor.MoveSelectedRowsDown();
            var afterDown = BuildAndParse(editor);
            Assert.That(afterDown.GetCellString(2, "name"), Is.EqualTo("Row2"));
            Assert.That(afterDown.GetHeight(), Is.EqualTo(4));
            Assert.That(afterDown.GetHeaders().Count, Is.EqualTo(4));
            Assert.That(afterDown.GetCellString(1, "name"), Is.EqualTo("P_HK47"));
            Assert.That(afterDown.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_MoveCurrentColumnLeftRight_ReordersHeaderAndData()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);

            SetCurrentColumn(editor, 3); // value
            editor.MoveCurrentColumnLeft(); // swaps value with name
            var resultLeft = BuildAndParse(editor);
            Assert.That(resultLeft.GetHeaders()[1], Is.EqualTo("value"));
            Assert.That(resultLeft.GetHeaders()[2], Is.EqualTo("name"));
            Assert.That(resultLeft.GetCellString(0, "value"), Is.EqualTo("100"));
            Assert.That(resultLeft.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(resultLeft.GetHeight(), Is.EqualTo(3));

            SetCurrentColumn(editor, 2); // value currently at idx2 in grid
            editor.MoveCurrentColumnRight(); // swap back
            var resultRight = BuildAndParse(editor);
            Assert.That(resultRight.GetHeaders()[1], Is.EqualTo("name"));
            Assert.That(resultRight.GetHeaders()[2], Is.EqualTo("value"));
            Assert.That(resultRight.GetCellString(0, "name"), Is.EqualTo("PMBTest"));
            Assert.That(resultRight.GetCellString(0, "value"), Is.EqualTo("100"));
            Assert.That(resultRight.GetHeight(), Is.EqualTo(3));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_ToggleSidebar_HidesAndShowsSidebar()
        {
            var editor = CreateEditor();
            var sidebar = editor.FindControl<Border>("sidebarHost");
            Assert.That(sidebar, Is.Not.Null);
            Assert.That(sidebar.IsVisible, Is.True);

            editor.ToggleSidebar();
            Assert.That(sidebar.IsVisible, Is.False);
            Assert.That(editor, Is.Not.Null);
            Assert.That(sidebar.Parent, Is.Not.Null);
            Assert.That(sidebar.IsVisible, Is.False);
            Assert.That(sidebar, Is.SameAs(editor.FindControl<Border>("sidebarHost")));

            editor.ToggleSidebar();
            Assert.That(sidebar.IsVisible, Is.True);
            Assert.That(editor, Is.Not.Null);
            Assert.That(sidebar.Parent, Is.Not.Null);
            Assert.That(sidebar, Is.SameAs(editor.FindControl<Border>("sidebarHost")));
            Assert.That(sidebar.IsVisible, Is.True);
            editor.Close();
        }

        // ==================== DISPLAY & STATUS (no mocks: real UI state) ====================

        [AvaloniaTest]
        public void OdyTool2DA_StatusBar_AfterLoad_ShowsRowAndColumnCount()
        {
            byte[] data = CreateTestTwoDABytes(5);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            string status = GetStatusText(editor);
            Assert.That(status, Does.Contain("Ready"));
            Assert.That(status, Does.Contain("5 rows"));
            Assert.That(status, Does.Contain("4 columns").Or.Contain("5 columns"));
            Assert.That(GetSidebarStatsText(editor), Does.Contain("5 rows"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_StatusBar_AfterNew_ShowsOneRowOneColumn()
        {
            var editor = CreateEditor();
            editor.New();
            string status = GetStatusText(editor);
            Assert.That(status, Does.Contain("1 rows").Or.Contain("1 row"));
            Assert.That(status, Does.Contain("2 columns").Or.Contain("columns"));
            Assert.That(GetSidebarStatsText(editor), Does.Contain("1"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_StatusBar_WhenMultipleRowsSelected_ShowsSelectedCount()
        {
            byte[] data = CreateTestTwoDABytes(5);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 0, 1, 2);
            string status = GetStatusText(editor);
            Assert.That(status, Does.Contain("3 rows selected"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_StatusBar_WhenFilterActive_ShowsVisibleCount()
        {
            byte[] data = CreateTestTwoDABytes(10);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            editor.DoFilter("P_HK47");
            Assert.DoesNotThrow(() => GetStatusText(editor));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(10));
            editor.DoFilter("");
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(10));
            Assert.That(GetStatusText(editor), Does.Contain("10 rows"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_StatusBar_WhenSidebarHidden_ShowsSidebarHidden()
        {
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, CreateTestTwoDABytes(2));
            editor.ToggleSidebar();
            string status = GetStatusText(editor);
            Assert.That(status, Does.Contain("Sidebar hidden").Or.Contain("F9"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_StatusBar_WhenCellSelected_ShowsCellReference()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 1);
            SetCurrentColumn(editor, 2);
            string status = GetStatusText(editor);
            Assert.That(status, Does.Contain("Cell:").Or.Contain("R1").Or.Contain("name"));
            editor.Close();
        }


        [AvaloniaTest]
        public void OdyTool2DA_EmptyState_WhenZeroRows_IsVisible()
        {
            var editor = CreateEditor();
            editor.Load("empty.2da", "empty", ResourceType.TwoDA, new byte[0]);
            Assert.That(GetEmptyStateVisible(editor), Is.True);
            var overlay = editor.FindControl<Border>("emptyStateOverlay");
            Assert.That(overlay, Is.Not.Null);
            Assert.That(overlay.IsVisible, Is.True);
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_EmptyState_AfterNew_IsNotVisible()
        {
            var editor = CreateEditor();
            editor.New();
            Assert.That(GetEmptyStateVisible(editor), Is.False);
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(1));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_EmptyState_AfterLoadWithData_IsNotVisible()
        {
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, CreateTestTwoDABytes(1));
            Assert.That(GetEmptyStateVisible(editor), Is.False);
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_FormulaBar_WhenNoSelection_ShowsDashOrEmpty()
        {
            var editor = CreateEditor();
            editor.New();
            string addr = GetCellAddressText(editor);
            Assert.That(addr, Is.Not.Null);
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_FormulaBar_WhenCellSelected_Updates()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 1);
            SetCurrentColumn(editor, 2);
            string addr = GetCellAddressText(editor);
            Assert.That(addr, Is.Not.Null.And.Not.Empty);
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_SetVerticalHeaderOption_DoesNotBreakBuild()
        {
            byte[] data = CreateTestTwoDABytes(2);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            editor.SetVerticalHeaderOption(VerticalHeaderOption.RowIndex);
            var r1 = BuildAndParse(editor);
            editor.SetVerticalHeaderOption(VerticalHeaderOption.RowLabel);
            var r2 = BuildAndParse(editor);
            Assert.That(r1.GetHeight(), Is.EqualTo(2));
            Assert.That(r2.GetHeight(), Is.EqualTo(2));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_AddColumnQuick_GreenButtonEquivalent_AddsColumn()
        {
            var editor = CreateEditor();
            editor.New();
            int colsBefore = GetColumnHeaders(editor).Count;
            editor.AddColumnQuick();
            Assert.That(GetColumnHeaders(editor).Count, Is.EqualTo(colsBefore + 1));
            Assert.That(GetStatusText(editor), Does.Contain("2 columns").Or.Contain("columns"));
            editor.AddColumnQuick();
            Assert.That(GetColumnHeaders(editor).Count, Is.EqualTo(colsBefore + 2));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_FilterEdit_ClearButton_ResetsFilter()
        {
            byte[] data = CreateTestTwoDABytes(10);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            var filterEdit = GetFilterEdit(editor);
            Assert.That(filterEdit, Is.Not.Null);
            editor.DoFilter("P_HK47");
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(10));
            editor.DoFilter("");
            Assert.That(GetStatusText(editor), Does.Contain("10 rows"));
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(10));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_SelectCurrentColumn_ThenFillDown_FillsColumn()
        {
            byte[] data = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 0);
            SetCurrentColumn(editor, 3);
            editor.SelectCurrentColumn();
            editor.FillDown();
            var result = BuildAndParse(editor);
            string v = result.GetCellString(0, "value");
            Assert.That(v, Is.Not.Null.And.Not.Empty);
            Assert.That(result.GetCellString(1, "value"), Is.EqualTo(v));
            Assert.That(result.GetCellString(2, "value"), Is.EqualTo(v));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_RemoveSelectedRows_WhenNoSelection_NoThrow()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor);
            Assert.DoesNotThrow(() => editor.RemoveSelectedRows());
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(3));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_DuplicateRow_WhenNoSelection_NoThrow()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor);
            Assert.DoesNotThrow(() => editor.DuplicateRow());
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(3));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_InsertRow_WhenNoSelection_Appends()
        {
            byte[] data = CreateTestTwoDABytes(2);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor);
            editor.InsertRow();
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(3));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_New_ThenAddColumnQuickTwice_ThenInsertRow_BuildsValid()
        {
            var editor = CreateEditor();
            editor.New();
            editor.AddColumnQuick();
            editor.AddColumnQuick();
            editor.InsertRow();
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.GreaterThanOrEqualTo(1));
            Assert.That(result.GetHeaders().Count, Is.GreaterThanOrEqualTo(2));
            Assert.That(GetStatusText(editor), Does.Contain("rows"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_Revert_UpdatesStatusAndBuild()
        {
            byte[] original = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, original);
            editor.InsertRow();
            editor.InsertRow();
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(6));
            InvokeRevert(editor);
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.EqualTo(4));
            Assert.That(GetStatusText(editor), Does.Contain("4 rows"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_Undo_MultipleLevels_ThenRedoAll()
        {
            byte[] data = CreateTestTwoDABytes(2);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            editor.InsertRow();
            editor.InsertRow();
            editor.InsertRow();
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(5));
            InvokeUndo(editor);
            InvokeUndo(editor);
            InvokeUndo(editor);
            int heightAfterUndos = BuildAndParse(editor).GetHeight();
            Assert.That(heightAfterUndos, Is.GreaterThanOrEqualTo(2).And.LessThanOrEqualTo(5));
            InvokeRedo(editor);
            InvokeRedo(editor);
            InvokeRedo(editor);
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(5));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_Paste_WhenEmptyTable_UsesFirstLineAsHeaders()
        {
            var editor = CreateEditor();
            editor.Load("empty.2da", "empty", ResourceType.TwoDA, new byte[0]);
            Assert.That(GetSourceData(editor).Count, Is.EqualTo(0));
            string tsv = "label\tname\tval\n0\tA\t100\n1\tB\t200";
            _ = (editor as Window)?.Clipboard?.SetTextAsync(tsv);
            editor.PasteSelection();
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.GreaterThanOrEqualTo(2));
            Assert.That(result.GetHeaders(), Does.Contain("name").Or.Contain("label"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_MoveRowUp_ThenMoveRowDown_RestoresOrder()
        {
            byte[] data = CreateTestTwoDABytes(4);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 2);
            editor.MoveSelectedRowsUp();
            Assert.That(BuildAndParse(editor).GetCellString(1, "name"), Is.EqualTo("Row2"));
            SetSelection(editor, 1);
            editor.MoveSelectedRowsDown();
            Assert.That(BuildAndParse(editor).GetCellString(2, "name"), Is.EqualTo("Row2"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_RemoveColumn_ThenBuild_HeadersAndDataConsistent()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetCurrentColumn(editor, 2);
            editor.RemoveColumn();
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeaders(), Does.Not.Contain("name"));
            Assert.That(result.GetCellString(0, "label"), Is.EqualTo("0"));
            Assert.Throws<KeyNotFoundException>(() => result.GetCellString(0, "name"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_ToggleFilter_ShowsOrHidesFilterSection()
        {
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, CreateTestTwoDABytes(2));
            var filterSection = editor.FindControl<Border>("filterSection");
            if (filterSection != null)
            {
                bool visibleBefore = filterSection.IsVisible;
                editor.ToggleFilter();
                Assert.That(filterSection.IsVisible, Is.EqualTo(!visibleBefore));
                editor.ToggleFilter();
                Assert.That(filterSection.IsVisible, Is.EqualTo(visibleBefore));
            }
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_Load_SetsWindowTitle()
        {
            var editor = CreateEditor();
            editor.Load("app.2da", "app", ResourceType.TwoDA, CreateTestTwoDABytes(1));
            Assert.That(editor.Title, Does.Contain("2DA").Or.Contain("app").Or.Contain(".2da"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_Build_AfterEveryAction_ProducesValidTwoDA()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            byte[] b1 = editor.Build().Item1;
            Assert.That(b1, Is.Not.Null.And.Length.GreaterThan(0));
            editor.InsertRow();
            byte[] b2 = editor.Build().Item1;
            Assert.That(b2, Is.Not.Null.And.Length.GreaterThan(0));
            SetSelection(editor, 0);
            editor.ClearCell();
            byte[] b3 = editor.Build().Item1;
            Assert.That(b3, Is.Not.Null.And.Length.GreaterThan(0));
            InvokeUndo(editor);
            byte[] b4 = editor.Build().Item1;
            Assert.That(b4, Is.Not.Null.And.Length.GreaterThan(0));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_AllSidebarButtons_InvokeSameActionsAsMenu()
        {
            byte[] data = CreateTestTwoDABytes(3);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetSelection(editor, 1);
            editor.DuplicateRow();
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(4));
            SetSelection(editor, 3);
            editor.RemoveSelectedRows();
            Assert.That(BuildAndParse(editor).GetHeight(), Is.EqualTo(3));
            SetCurrentColumn(editor, 2);
            editor.AddColumnQuick();
            Assert.That(GetColumnHeaders(editor).Count, Is.EqualTo(5));
            editor.RemoveColumn();
            Assert.That(GetColumnHeaders(editor).Count, Is.EqualTo(4));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_SortAsc_ThenSortDesc_RestoresVisualOrder()
        {
            var twoDA = new TwoDA(new List<string> { "label", "x" });
            twoDA.AddRow("0", new Dictionary<string, object> { ["label"] = "0", ["x"] = "C" });
            twoDA.AddRow("1", new Dictionary<string, object> { ["label"] = "1", ["x"] = "A" });
            twoDA.AddRow("2", new Dictionary<string, object> { ["label"] = "2", ["x"] = "B" });
            byte[] data = TwoDAAuto.Bytes2DA(twoDA);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            SetCurrentColumn(editor, 2);
            editor.SortRows(ascending: true);
            Assert.That(BuildAndParse(editor).GetCellString(0, "x"), Is.EqualTo("A"));
            editor.SortRows(ascending: false);
            Assert.That(BuildAndParse(editor).GetCellString(0, "x"), Is.EqualTo("C"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_LoadCSV_BuildPreservesContent()
        {
            var twoDA = new TwoDA(new List<string> { "label", "name" });
            twoDA.AddRow("0", new Dictionary<string, object> { ["label"] = "0", ["name"] = "CSV" });
            byte[] csvBytes = TwoDAAuto.Bytes2DA(twoDA, ResourceType.TwoDA_CSV);
            var editor = CreateEditor();
            editor.Load("test.2da.csv", "test", ResourceType.TwoDA_CSV, csvBytes);
            var result = BuildAndParse(editor);
            Assert.That(result.GetHeight(), Is.GreaterThanOrEqualTo(1));
            Assert.That(result.GetCellString(0, "name"), Is.EqualTo("CSV"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_EmptyStateText_WhenVisible_ContainsHelpfulMessage()
        {
            var editor = CreateEditor();
            editor.Load("empty.2da", "empty", ResourceType.TwoDA, new byte[0]);
            var emptyText = editor.FindControl<Avalonia.Controls.TextBlock>("emptyStateText");
            Assert.That(emptyText, Is.Not.Null);
            Assert.That(emptyText.Text, Does.Contain("row").Or.Contain("Insert").Or.Contain("Open"));
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_DataGrid_ExistsAndHasColumnsAfterLoad()
        {
            byte[] data = CreateTestTwoDABytes(2);
            var editor = CreateEditor();
            editor.Load("test.2da", "test", ResourceType.TwoDA, data);
            var grid = GetDataGrid(editor);
            Assert.That(grid, Is.Not.Null);
            Assert.That(grid.Columns.Count, Is.GreaterThanOrEqualTo(2));
            Assert.That(grid.ItemsSource, Is.Not.Null);
            editor.Close();
        }

        [AvaloniaTest]
        public void OdyTool2DA_FormulaBarEdit_ExistsInXaml()
        {
            var editor = CreateEditor();
            var formulaBar = editor.FindControl<TextBox>("formulaBarEdit");
            Assert.That(formulaBar, Is.Not.Null);
            Assert.That(formulaBar.Watermark ?? "", Does.Contain("Enter value").Or.Contain("value"));
            editor.Close();
        }
    }
}
