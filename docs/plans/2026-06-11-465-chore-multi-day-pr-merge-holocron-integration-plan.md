---
title: "chore: multi-day open PR merge and Holocron integration"
type: chore
status: active
date: 2026-06-11
origin: user request — investigate open PRs, merge stack, continue PyKotor/Holocron port
branch: master
---

# chore: multi-day open PR merge and Holocron integration (plan 465)

## Executive summary (2026-06-11)

Plan **465** Days **1–12** landed on `master`:

| Day | PR | Outcome |
|-----|-----|---------|
| **1** | [#135](https://github.com/th3w1zard1/Andastra/pull/135) | Stack-simulation arc landed — **165** NcsConsti tests |
| **2** | [#136](https://github.com/th3w1zard1/Andastra/pull/136) | Five-hop mixed CONST relay — **167** NcsConsti tests |
| **3** | [#137](https://github.com/th3w1zard1/Andastra/pull/137) | Field-value UT editor wiring — **10** FieldValueReferenceHelper tests |
| **4** | [#138](https://github.com/th3w1zard1/Andastra/pull/138) | Tracker closure + verification (plan **468**); **#76** closed |
| **5** | [#140](https://github.com/th3w1zard1/Andastra/pull/140) | 2DA row selection, dirty status, shortcuts (plan **469**); **97** `OdyTool2DA_*` tests |
| **6** | [#141](https://github.com/th3w1zard1/Andastra/pull/141) | 2DA Shift/Ctrl+Space + Go To Column (plan **470**); **100** `OdyTool2DA_*` tests |
| **7** | [#142](https://github.com/th3w1zard1/Andastra/pull/142) | 2DA Shift+Click range selection (plan **471**); **103** `OdyTool2DA_*` tests |
| **8** | [#143](https://github.com/th3w1zard1/Andastra/pull/143) | 2DA paste over selection / anchor paste (plan **472**); **106** `OdyTool2DA_*` tests |
| **9** | [#144](https://github.com/th3w1zard1/Andastra/pull/144) | 2DA Ctrl+Click row multi-select (plan **473**); **109** `OdyTool2DA_*` tests |
| **10** | [#145](https://github.com/th3w1zard1/Andastra/pull/145) | 2DA Fill Down within range (plan **474**); **112** `OdyTool2DA_*` tests |
| **11** | [#146](https://github.com/th3w1zard1/Andastra/pull/146) | 2DA in-cell edit tests (plan **475**); **115** `OdyTool2DA_*` tests |
| **12** | [#147](https://github.com/th3w1zard1/Andastra/pull/147) | 2DA header column/row select tests (plan **476**); **118** `OdyTool2DA_*` tests |

Holocron plan **063** core units (U1–U7) remain **complete**. KotorDiff installation reference search (plans **001**/**002**) is **already on `master`** — no `TODO: STUB` in `ReferenceAnalyzers.cs`.

**Open PRs:** none from the pre-integration backlog.

---

## Day 1 — Land stack simulation on `master` (2026-06-11) — complete

| Step | Action | Done |
|------|--------|------|
| D1.1 | Branch `work/day1-stack-simulation-land` from `origin/master` | ✅ |
| D1.2 | Merge stack-simulation arc (plans **420**–**464**) onto `master` | ✅ |
| D1.3 | Resolve conflicts; keep master's field-value/KotorDiff/LIP tests | ✅ |
| D1.4 | Verify **≥163** NcsConsti tests pass | ✅ **165** |
| D1.5 | PR **#135** → `master` | ✅ @ `6a449a97b` |
| D1.6 | Close superseded **#104–#122**, **#89–#91** | ✅ |

---

## Day 2 — Relay arc + stack base cleanup — complete

| Step | Action | Done |
|------|--------|------|
| D2.1 | Six-hop relay (**#87**/**#88**) on `master` | ✅ |
| D2.2 | Five-hop mixed relay (plan **411**, **#136**) | ✅ **167** NcsConsti |
| D2.3 | Close **#76** or rebase | ✅ via **#138** |
| D2.4 | Tracker sync v21 (plan **466**) | ✅ |

---

## Day 3 — Field-value arc — complete via plan **467**

| Step | Action | Done |
|------|--------|------|
| D3.1 | Cherry-pick field-value wiring onto current `master` | ✅ |
| D3.2 | UTC + UT* editors; **10** tests | ✅ |
| D3.3 | Step 3d ladder + `pr-merge-readiness.md` | ✅ |
| D3.4 | Close **#78**, **#81**–**#85** | ✅ via **#137** |

---

## Day 4 — Verification + tracker closure (plan **468**)

| Step | Action | Done |
|------|--------|------|
| D4.1 | Confirm KotorDiff `CollectInstallationStrRefResources` / `CollectInstallationGffResources` on `master` | ✅ |
| D4.2 | Confirm `BuildStrrefMappingsFromTlkMod` wired in batch path | ✅ |
| D4.3 | Run KotorDiff + FieldValue + NcsConsti test filters | ✅ |
| D4.4 | Close **#76**; update this plan + KB tracker | ✅ via **#138** |

---

## Day 5 — 2DA spreadsheet UX slice (plan **469**)

| Step | Action | Done |
|------|--------|------|
| D5.1 | Select Row (menu, sidebar, `#` column click) | ✅ |
| D5.2 | Status bar `Modified` when dirty | ✅ |
| D5.3 | Status bar hidden row count when filter active | ✅ |
| D5.4 | Help → Keyboard Shortcuts dialog | ✅ |
| D5.5 | OdyTool2DATests for row select, dirty status, shortcuts smoke | ✅ |
| D5.6 | PR **#140** → `master` | ✅ @ `4ceb97899` |

---

## Day 6 — 2DA selection shortcuts + go to column (plan **470**)

| Step | Action | Done |
|------|--------|------|
| D6.1 | Shift+Space → Select Row; Ctrl+Space → Select Column | ✅ |
| D6.2 | Go To Column menu/sidebar + dialog (name or index) | ✅ |
| D6.3 | Keyboard shortcuts dialog updates | ✅ |
| D6.4 | OdyTool2DATests for shortcuts + go to column | ✅ |
| D6.5 | PR **#141** → `master` | ✅ @ `1abe9337e` |

---

## Day 7 — 2DA Shift+Click rectangular range selection (plan **471**)

| Step | Action | Done |
|------|--------|------|
| D7.1 | Range anchor + Shift+Click in `OnGridPointerPressed` | ✅ |
| D7.2 | `ApplyRangeHighlight()` / `ClearRangeHighlight()` visual feedback | ✅ |
| D7.3 | `CopySelection()` TSV block when range active | ✅ |
| D7.4 | Status bar range coords when >1 cell | ✅ |
| D7.5 | OdyTool2DATests: rectangle, block copy, column clear | ✅ |
| D7.6 | PR **#142** → `master` | ✅ @ `b8131328b` |

---

## Day 8 — 2DA paste over selection / anchor paste (plan **472**)

| Step | Action | Done |
|------|--------|------|
| D8.1 | `TryGetPasteAnchor` — current cell or range min corner | ✅ |
| D8.2 | `PasteAnchorOverwrite` — overwrite cells, expand rows/cols at bounds | ✅ |
| D8.3 | Insert fallback when no current column | ✅ |
| D8.4 | OdyTool2DATests: anchor overwrite, range anchor, insert regression | ✅ |
| D8.5 | PR **#143** → `master` | ✅ @ `7bb42c493` |

---

## Day 9 — 2DA Ctrl+Click row multi-select (plan **473**)

| Step | Action | Done |
|------|--------|------|
| D9.1 | Ctrl+Click `#` column toggles row in `SelectedItems` | ✅ |
| D9.2 | Clear column selection and cell range on toggle | ✅ |
| D9.3 | Normal `#` click keeps single-row select | ✅ |
| D9.4 | Keyboard shortcuts dialog update | ✅ |
| D9.5 | OdyTool2DATests: toggle, clear modes, regression | ✅ |
| D9.6 | PR **#144** → `master` | ✅ @ `15321b9e5` |

---



## Day 10 — 2DA Fill Down within active cell range (plan **474**)

| Step | Action | Done |
|------|--------|------|
| D10.1 | When `_cellRangeActive`, copy each column's top-row cell down through the range | ✅ |
| D10.2 | Legacy Fill Down unchanged for row/column selection without active range | ✅ |
| D10.3 | Single-row range is a no-op | ✅ |
| D10.4 | OdyTool2DATests: multi-column fill, single-column fill, single-cell no-op | ✅ |
| D10.5 | PR **#145** → `master` | ✅ @ `c6e7f6083` |

---

## Day 11 — 2DA in-cell editing tests (plan **475**)

| Step | Action | Done |
|------|--------|------|
| D11.1 | `BeginCellEdit()` / `IsCellEditing()` public test hooks | ✅ |
| D11.2 | F2 handler delegates to `BeginCellEdit()` | ✅ |
| D11.3 | Selection shortcuts skip during cell edit | ✅ |
| D11.4 | OdyTool2DATests: BeginCellEdit, F2, shortcut guard | ✅ |
| D11.5 | PR **#146** → `master` | ✅ @ `d35191412` |

---

## Day 12 — 2DA column/row header selection tests (plan **476**)

| Step | Action | Done |
|------|--------|------|
| D12.1 | `SelectColumnByIndex()` public for column header path | ✅ |
| D12.2 | Column select clears cell range; row select clears column mode | ✅ |
| D12.3 | OdyTool2DATests: column all-rows, range clear, row clears column | ✅ |
| D12.4 | PR **#147** → `master` | ✅ @ `a8759fb25` |

## Day 5+ — Holocron continuation

Per plan **063** deferred items:

1. ~~Field-value find-refs~~ — **done** (plan **467**)
2. ~~KotorDiff installation ref search~~ — **done** (plans **001**/**002** on `master`)
3. **2DA spreadsheet UX** — `docs/twoda_editor_ux_and_feature_completion.md`
4. **Module Designer 3D / Lip Syncer / PLT** — separate plans; out of scope for week 1

---

## Integration PRs landed

| PR | Merge | Scope |
|----|-------|-------|
| [#135](https://github.com/th3w1zard1/Andastra/pull/135) | `6a449a97b` | Stack-simulation arc |
| [#136](https://github.com/th3w1zard1/Andastra/pull/136) | `b4421c112` | Five-hop mixed relay |
| [#137](https://github.com/th3w1zard1/Andastra/pull/137) | `374118902` | Field-value UT wiring |
| [#140](https://github.com/th3w1zard1/Andastra/pull/140) | `4ceb97899` | 2DA row selection, dirty status, shortcuts (plan **469**) |
| [#141](https://github.com/th3w1zard1/Andastra/pull/141) | `1abe9337e` | 2DA Shift/Ctrl+Space + Go To Column (plan **470**) |
| [#142](https://github.com/th3w1zard1/Andastra/pull/142) | `b8131328b` | 2DA Shift+Click range selection (plan **471**) |
| [#143](https://github.com/th3w1zard1/Andastra/pull/143) | `7bb42c493` | 2DA paste over selection / anchor paste (plan **472**) |
| [#144](https://github.com/th3w1zard1/Andastra/pull/144) | `15321b9e5` | 2DA Ctrl+Click row multi-select (plan **473**) |
| [#145](https://github.com/th3w1zard1/Andastra/pull/145) | `c6e7f6083` | 2DA Fill Down within active cell range (plan **474**) |
| [#146](https://github.com/th3w1zard1/Andastra/pull/146) | `d35191412` | 2DA in-cell editing test coverage (plan **475**) |
| [#147](https://github.com/th3w1zard1/Andastra/pull/147) | `a8759fb25` | 2DA header column/row selection tests (plan **476**) |
