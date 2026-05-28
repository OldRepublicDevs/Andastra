---
title: "feat: Holocron port phase J — KotorCLI find-refs command"
type: feat
status: complete
date: 2026-05-24
origin: docs/plans/2026-05-24-078-feat-holocron-phase-i-conversation-refs-plan.md
branch: feat/holocron-port-phase-b
---

# feat: Holocron port phase J (plan 079)

## Summary

Expose BioWare `ReferenceFinder` installation-wide search as `kotorcli find-refs` for script, tag, template ResRef, and conversation ResRef needles — headless parity with OdyTools **Find References** for CI and mod pipelines.

## Problem frame

Plan 063 deferred KotorCLI utility wiring beyond per-file `grep`/`diff`. OdyTools editors now call `ReferenceFinder` for tag, template, script, and conversation fields; automation agents still lack a CLI entry point.

## Requirements

- R1. `find-refs <needle>` with required `--installation` and `--type` (`script` | `tag` | `template` | `conversation`).
- R2. Delegates to `ReferenceFinder.FindScriptReferences`, `FindTagReferences`, `FindTemplateResRefReferences`, or `FindConversationResRefReferences`.
- R3. Scope flags: `--no-chitin`, `--no-modules`, `--no-override` (default: search all three).
- R4. Match flags: `--case-sensitive`, `--partial` forwarded to `ReferenceSearchOptions`.
- R5. Prints one line per hit using `ReferenceSearchResult.DisplayLabel`; exit 0 when hits exist, 1 when none or on error.
- R6. `tests/KotorCLI.Tests/FindRefsCommandTests.cs` with override UTC fixture (tag + conversation).

## Scope boundaries

- No DLG-internal graph search, NCS bytecode reference cache, or GUI `FileResultsDialog` changes.
- No new BioWare search kinds beyond existing `ReferenceFinder` APIs.

## Deferred

- JSON output mode, `--count-only`, module filter globs.

## Implementation units

| Unit | Files | Test scenarios |
|------|-------|----------------|
| U1 CLI command | `src/Tools/KotorCLI/Commands/FindRefsCommand.cs`, `src/Tools/KotorCLI/Program.cs` | Missing install/type/needle exits 1; invalid type exits 1 |
| U2 Tests | `tests/KotorCLI.Tests/FindRefsCommandTests.cs` | Tag hit in Override UTC; conversation hit; no-match exits 1 |

## Verification

- `dotnet build src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0`
- `dotnet test tests/KotorCLI.Tests/KotorCLI.Tests.csproj --framework net9.0 --filter FindRefs`

## Patterns

- Installation construction: `ValidationCommands.ExecuteCheckTxi`
- Override fixture: `tests/OdyTools.Tests/ReferenceFinderTests.cs` (`FindTagReferences_OverrideUtc_ReturnsFieldPath`)
