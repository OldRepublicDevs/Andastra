# 2DA Editor: Why the Main Area Was Not Robust, Intuitive, or Accessible — and Full Expert Feature Completion

## Why the Middle Main Area Was Not Robust, Intuitive, or Accessible

The middle main area was neither robust nor intuitive nor accessible because it did not offer a **pre-existing empty spreadsheet**. Instead, it showed a large blank space with an instructional message telling the user to open a file or use menus to add columns and rows before they could do anything. That meant: (1) **zero immediate affordance** — there was nothing to click or type into, so the core action “edit table” was impossible without extra steps; (2) **menu-first workflow** — the app forced navigation through File → Open or Tools → Add Column and Insert Row before any data entry, which is the opposite of how Excel and Google Sheets work; (3) **weak discoverability** — new users had no visible grid or cells to signal “this is a spreadsheet”; and (4) **inaccessible for quick use** — power users could not click once and start typing. A spreadsheet editor’s main area must present a **visible, clickable, typable grid from the moment the user opens a new document**, with at least one column and one row so that the first interaction can be “click cell, type.” The previous design blocked that and made the middle area feel like a dead canvas instead of the primary workspace.

---

## Full Expert Exhaustive Feature Completion: What Could Be Added

What follows is an exhaustive list of features and improvements that would bring the 2DA Editor to full expert-level, spreadsheet-style parity with applications like Excel and Google Sheets, and beyond, for 2DA-specific workflows.

### 1. Immediate Workspace and Empty-State Behavior

- **Pre-existing empty grid on New:** New documents should always open with at least one data column and one row so the user can click a cell and type immediately, with no need to open a file or use menus. Row label column (#) and one data column (e.g. “Column1”) and one empty row is the minimum. This has been implemented.
- **Empty-state overlay only when truly empty:** When the document has zero rows (e.g. after opening an empty file), show a short overlay message (“No rows — add rows or open a file”) so the grid area never looks like an unexplained void, but the overlay should not appear when there is at least one row.
- **Starter template option:** Optional “New from template” (e.g. empty 10×5 grid, or game-specific 2DA templates like appearance.2da structure) so power users can start from a known shape.

### 2. Grid Interaction and Navigation

- **Click-to-edit and in-cell editing:** Single click selects the cell; double-click or F2 enters in-cell edit mode with cursor in the cell; Escape cancels, Enter commits and moves down. The formula bar should stay in sync and support editing the same value.
- **Arrow-key and Tab navigation:** Arrow keys and Tab/Shift+Tab move selection and commit any pending edit; wrap at edges (optional) and support “Enter moves down” vs “Enter stays in cell” as a preference.
- **Go To Cell (e.g. Ctrl+G):** Dialog or quick-jump to row index and/or column name (e.g. “R5, name”) for large tables.
- **Extended selection with Shift+Click and Ctrl+Click:** Rectangular range selection (e.g. click cell A1, Shift+click cell C5 selects the block); Ctrl+Click adds disjoint cells or rows to selection for bulk operations.
- **Column and row header selection:** Clicking column header selects entire column; clicking row header (#) selects entire row; corner cell could select all. This enables “Select Column” and “Select Row” in a discoverable way.
- **Visible focus and selection styling:** Clear focus rectangle and distinct background for selected cell(s) and selected row/column headers so keyboard and mouse users always see where they are.

### 3. Clipboard and Data Exchange

- **Copy/Paste rectangular ranges:** Copy selection as tab-separated (TSV) so paste into Excel or Google Sheets preserves columns; paste from TSV/clipboard parses tabs and newlines and fills or extends the grid.
- **Paste transposed:** Already present; ensure it works for any rectangular selection.
- **Paste over selection:** When pasting, either replace the selected range or insert and shift (user preference or modifier key).
- **Paste special (optional):** Paste values only, or paste and match column by header name when pasting from another 2DA or CSV.
- **Drag-and-drop reorder rows (and optionally columns):** Reorder rows by dragging the row header; optionally allow column reorder by dragging column headers.

### 4. Formula Bar and Cell Editing

- **Formula bar as primary editor:** Formula bar shows and edits the current cell; Enter commits, Escape cancels; optional “fx” dropdown for a list of supported functions if the editor ever supports expressions.
- **Multi-line and long text:** Support for multi-line cell content (e.g. for 2DA columns that store descriptions) and horizontal scrolling or word wrap in the formula bar for long strings.
- **Clear cell (Delete/Backspace):** Already present; ensure it clears only the current cell or selected range and pushes undo.
- **Undo/redo for all edits:** Undo/redo stacks for cell edits, row/column insert/delete, renames, and paste; clear redo on new edit. Already partially there; extend to every mutating action.

### 5. Rows and Columns

- **Insert row above/below:** Already present; ensure keyboard shortcut (e.g. Ctrl+Plus or context menu) and that new row gets correct width (same number of cells as others).
- **Insert multiple rows:** “Insert N rows above/below” dialog or repeat action.
- **Delete rows:** Delete key or context menu removes selected rows with undo.
- **Add column:** Already present; optional “Add column left of current” vs “right of current” for precise layout.
- **Remove column:** Already present; confirm when column has data if desired.
- **Rename column:** Already present; ensure header is editable in-place (e.g. double-click header) as well as via dialog.
- **Column width:** Resize columns by dragging header edge; double-click to auto-fit to content; persist column widths per session or per file if format allows.
- **Row height:** Optional adjustable row height or “auto height” for multi-line cells.
- **Freeze panes (optional):** Freeze top N rows and/or left M columns for large 2DAs so headers stay visible while scrolling.

### 6. Find, Replace, and Filter

- **Find (Ctrl+F):** Find dialog: search in current column, all columns, or row labels; match case, whole cell; next/previous; highlight all matches.
- **Replace (Ctrl+H):** Replace one, replace all in selection or in sheet; optional regex for advanced users.
- **Row filter:** Already present (filter by text); extend with “filter by column” dropdown (e.g. show only rows where column “race” equals “Human”) and clear filter button.
- **Filter by multiple columns (optional):** AND/OR conditions per column for expert users.
- **Sort:** Already present (A–Z, Z–A by current column); add “Sort by multiple columns” (e.g. sort by column A, then by column B) and “Restore original order” if we keep a hidden row index.

### 7. Data Types and Validation (Optional for 2DA)

- **Column type hint (optional):** Mark column as “integer,” “float,” “string,” or “row label” for validation and paste behavior; 2DA is string-based but game tooling often expects numbers in certain columns.
- **Validation on paste or commit:** Warn or reject non-numeric values in a column marked numeric; optional “strict mode” for game asset hygiene.
- **Default value for new cells:** When inserting rows or columns, fill new cells with a default (e.g. empty, or “0” for numeric columns).

### 8. File and Format

- **Save/Save As 2DA binary and CSV:** Already present; ensure encoding (e.g. UTF-8 for CSV) and line endings are consistent.
- **Export to Excel (XLSX) or ODS (optional):** For users who want to edit in Excel and re-import; export preserves columns and row labels.
- **Import from Excel/CSV:** Paste from clipboard already helps; optional “Import from file” that detects delimiter and first-row-as-header for CSV/TSV.
- **Revert:** Already present; ensure “Revert to last saved” is clear and safe (e.g. confirm if dirty).
- **Recent files:** List of recently opened 2DA/CSV files in File menu for quick re-open.
- **Auto-save or recovery (optional):** Optional auto-save to temp file and “recover unsaved” on next launch.

### 9. Accessibility and Keyboard

- **Full keyboard navigation:** All actions (New, Open, Save, Insert Row/Column, Delete, Find, Replace, Sort, Filter) available via keyboard and announced to screen readers where applicable.
- **High-contrast and focus indicators:** Visible focus ring and selection that meet contrast guidelines; optional high-contrast theme.
- **Screen reader:** Expose table structure (row/column headers, cell coordinates, current cell value) so the grid is navigable by assistive technology.
- **Shortcuts list:** Help → Keyboard shortcuts or a cheat-sheet panel listing all shortcuts (Ctrl+S, Ctrl+C/V/X, Ctrl+F/H, Ctrl+G, Ctrl+Z/Y, Ctrl+A, Ctrl+D, Tab, Enter, Delete, etc.).

### 10. Visual and Status Feedback

- **Status bar:** Already shows dimensions, selection count, current cell, and SUM/AVERAGE/COUNT for numeric selection; optional additions: “Dirty” indicator (unsaved), current filter state, and “N rows hidden by filter.”
- **Name box:** Already shows current cell (e.g. R0, Column1); keep and ensure it updates on selection.
- **Tooltips:** Tooltips on toolbar buttons and column headers (e.g. show full header name if truncated).
- **Progress for long operations:** For very large 2DAs, show progress when loading, saving, or applying sort/filter/replace.

### 11. Multi-Sheet and Large Data (If Applicable)

- **Single-sheet focus:** 2DA is single-table; no need for multiple sheets unless the editor is generalized. For very large 2DAs, virtualization (only render visible rows) keeps scrolling smooth.
- **Virtualized grid:** If row count is large (e.g. thousands), virtualize rows so only visible rows are in the visual tree; same for columns if needed.

### 12. 2DA-Specific and Game Tooling

- **Row label column:** The first column (#) is the row label; ensure it is always present and editable; “Regenerate row labels” already exists for renumbering.
- **Column header semantics:** Some 2DAs have required columns (e.g. appearance.2da); optional “column template” or validation that certain headers exist.
- **ResRef or ID columns:** Optional highlighting or validation for columns that typically store ResRefs or numeric IDs (e.g. green for valid, red for missing).
- **Comments or notes (if format allows):** Some formats allow row/column comments; if the 2DA format or an extended format supports it, expose in UI (e.g. small icon in cell with tooltip).
- **Diff/merge (advanced):** Compare two 2DA files and show differences; merge changes from one into another for modding workflows.

### 13. Performance and Robustness

- **Large file handling:** Lazy load or stream very large 2DAs; avoid loading entire file into memory if it is huge (e.g. millions of cells).
- **Error handling:** Graceful handling of corrupt or partial 2DA/CSV (e.g. show partial data and a warning, or “Revert” to last good state).
- **No redundant UI:** Avoid duplicate Copy/Paste or other actions in the main grid area; keep primary actions in toolbar and context menu so the main area is purely the grid. Already addressed by removing status-bar Copy/Paste.

### 14. Summary Table (Implementation Priority)

| Area | Feature | Priority |
|------|---------|----------|
| Workspace | Pre-existing empty grid (1 col, 1 row) on New | Done |
| Workspace | Empty-state overlay only when 0 rows | Done |
| Grid | In-cell editing (double-click / F2) | High |
| Grid | Shift+Click range, Ctrl+Click multi-select | High |
| Grid | Column/row header click selects entire column/row | High |
| Clipboard | Paste over range, paste special options | Medium |
| Rows/Columns | Insert N rows, column resize persist | Medium |
| Find/Replace | Find in column, replace all, regex option | Medium |
| Accessibility | Full keyboard + shortcuts list + screen reader | High |
| Status | Dirty indicator, “N rows hidden” | Low |
| 2DA-specific | Column templates, ResRef validation | Low |
| Performance | Virtualized grid for large 2DAs | Medium |

Implementing the high-priority items above would make the 2DA Editor robust, intuitive, and accessible for both quick data entry and expert table editing, with the main area always offering a real, clickable, typable spreadsheet from the start.

---

### 15. Expanded Notes on Critical Areas

**Grid interaction and discoverability.** The single most impactful improvement after “pre-existing grid” is making the grid itself the obvious place to interact. In Excel and Google Sheets, users learn within seconds that they click a cell and type. That requires: (1) the grid being visibly present from the start (now true); (2) single-click selection with clear visual feedback; (3) double-click or F2 to edit in-cell so the cursor appears in the cell; (4) Enter committing and moving down so data entry is fast. Without in-cell editing, the formula bar is the only place to type, which is less discoverable for new users. Header clicks (select column/row) should be consistent and visually indicated so power users can select a column and apply Sort or Delete without hunting for menus.

**Clipboard and interoperability.** The 2DA Editor is used in a modding and game-asset pipeline. Users often prepare data in Excel or Google Sheets and paste into the editor, or copy from the editor into a script or another tool. Tab-separated copy and paste is the minimum; ensuring that pasted data maps correctly to columns (first row of pasted block to current cell, then fill right and down) and that copying a block produces the same shape when pasted elsewhere reduces friction. Paste transposed and “paste over selection” (replace vs insert) cover most expert workflows. Optional “paste by header name” would allow merging a column from another 2DA by matching header names.

**Accessibility.** A table editor is information-dense; keyboard-only and screen-reader users must be able to navigate and edit without the mouse. Every toolbar and menu action should have a shortcut; the grid must support full keyboard navigation (Tab, arrows, Enter, Escape) and expose row/column/cell identity to the platform’s accessibility APIs. A dedicated Keyboard Shortcuts dialog or panel (e.g. Help → Shortcuts) lists every shortcut and reduces reliance on memorization. High-contrast focus and selection styling help low-vision users and anyone in bright environments.

**Performance and large files.** 2DA files are usually small (tens to hundreds of rows), but some mods or tools generate very large tables. If the grid is implemented with one UI row per data row, thousands of rows can cause slow layout and scrolling. Virtualization (rendering only visible rows and reusing row controls) keeps interaction smooth. Similarly, loading and saving should avoid blocking the UI for large files (e.g. async load with a progress indicator, or background save). Robust handling of malformed or truncated files (show partial data and a clear error message, or offer to revert) prevents the editor from feeling brittle.

**2DA-specific semantics.** The format has a row label column and named columns; many columns are numeric IDs or ResRefs. The editor can stay format-agnostic (everything as string), but optional features improve the experience: column type hints (integer/float/string) for validation and display, ResRef columns that could be validated against a list or shown with a picker, and “column template” for known 2DA types (e.g. appearance.2da) that warn if required columns are missing or renamed. These are polish rather than requirements for basic robustness.

**Conclusion.** The main area must always present a real spreadsheet: visible grid, at least one row and one column on New, and no gatekeeping message that blocks clicking and typing. On top of that, expert-level completion means: rich selection (ranges, columns, rows), full keyboard and accessibility support, reliable clipboard round-trip with Excel/Sheets, comprehensive find/replace and filter, and optional enhancements for large files and 2DA-specific validation. Prioritizing the “pre-existing empty grid,” in-cell editing, range selection, and accessibility will yield the largest gain in robustness, intuitiveness, and accessibility for the middle main area.
