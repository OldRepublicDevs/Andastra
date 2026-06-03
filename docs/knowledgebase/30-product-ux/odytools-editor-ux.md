# OdyTools Editor UX

Content authoring and inspection workflows for OdyTools and standalone editors.

## Surfaces

| Surface | Entry | Notes |
|---------|-------|-------|
| **OdyTools AIO** | `dotnet build src/Tools/OdyTools/OdyTools.csproj --framework net9.0` | Holocron-style combined editor `[REPO]` |
| **Standalone editors** | Per-csproj under `src/Tools/OdyTools/` (GFF, DLG, NSS, etc.) | Prefer individual csproj over AIO for narrow tasks `[REPO]` |
| **ConvertKotorGame** | `dotnet build src/Tools/ConvertKotorGame/ConvertKotorGame.csproj --framework net9.0` | K1↔TSL portability wizard `[REPO]` |

## Expected workflow (typical mod author)

1. Open or create a game resource (GFF, DLG, 2DA, etc.) via the relevant editor. `[SYNTH]`
2. Edit with format-aware UI; save back to module/installation layout. `[SYNTH]`
3. Optional: compile NSS via NSSComp; diff installs via KotorDiff. `[REPO]` ([run-tools-reference.md](../50-execution/run-tools-reference.md))

## Verification status

| Claim | Status |
|-------|--------|
| OdyTools + standalones compile on net9.0 | Green `[REPO]` (2026-05-24) |
| OdyToolFAC faction/reputation editor | Green — headless roundtrip tests `[REPO]` (2026-05-24) |
| OdyToolNSS NCS disassembly tab | Green — `DisassembleNcsBytes` + tab UI tests `[REPO]` (2026-05-24) |
| Reference finder (script ResRef, UTC/UTD/UTP/UTT/UTE/UTM/ARE/IFO context menus) | Green — override fixture tests `[REPO]` (2026-05-24) |
| Reference search options dialog (override/modules/chitin, module-glob, StrRef NCS scan) | Green — `ReferenceSearchOptionsDialogTests` `[REPO]` (2026-05-24) |
| StrRef find-refs NCS CONSTI cache path | Green — `StrRefReferenceCache` / `IncludeNcsStrRefScan` gating; **10** NcsConsti + **2** CLI tests `[REPO]` (plan **286**, 2026-05-28) |
| Script ResRef NCS CONSTS scanner paths | Green — `NcsConstStringScanner` + `(NCS bytecode) offset_<n>` field paths; **3** NcsConstString + **5** FindScriptResRefInNcsBytes tests `[REPO]` (plan **287**, 2026-05-28) |
| Compiled NCS script ResRef installation e2e | Green — `FindScriptReferences_OverrideCompiledNcs_ReturnsNcsBytecodePath`; **1** ReferenceFinder test `[REPO]` (plan **289**, 2026-05-28) |
| KotorCLI find-refs compiled NCS CLI subprocess | Green — `Cli_FindRefs_Script_CompiledNcsInOverride_ExitsZero`; **1** FindRefsCommandCli test `[REPO]` (plan **290**, 2026-05-28) |
| NCS CONSTI StrRef vs 2DA-memory threshold disambiguation | Green — threshold + opcode-context + action-signature + stack-store heuristics; **74** NcsConsti tests `[REPO]` (plans **086**/**095**/**099**/**292**–**294**/**303**/**305**/**307**/**324**–**335**, 2026-05-28) |
| NCS CONSTI control-flow reachability (JZ/JNZ/JMP fork, while-break, subroutine/infinite-loop) | Green — `NcsConstiScanner` forward scan + cache indexing; **74** NcsConsti tests `[REPO]` (plans **324**–**335**, 2026-05-24) |
| KotorCLI find-strref cache-path NCS control-flow gating | Green — in-process + CLI subprocess cache-path tests; **18** FindStrRef + **12** InstallationRefSearch CLI tests `[REPO]` (plans **337**–**338**, 2026-05-24) |
| BioWare find-strref slow vs cache NCS semantics | Green — slow path matches raw CONSTI; cache path excludes dead-path consumers; **3** BioWare FindStrRefReferences CF tests `[REPO]` (plan **339**, 2026-05-24) |
| KotorCLI find-strref slow vs cache documentation | Green — README table + examples under `find-strref` `[REPO]` (plan **340**, 2026-05-24) |
| OdyTools StrRef find-refs cache-path NCS gating | Green — `StrRefReferenceHelper` builds `StrRefReferenceCache` when `IncludeNcsStrRefScan`; **10** StrRefReferenceHelper tests `[REPO]` (plan **346**, 2026-05-24) |
| 2DA editor / ComboBox2DA row **Find References** | Green — `TwoDAMemoryReferenceHelper` + `OdyTool2DA` context menu (plans 083, 200) `[REPO]` |
| KotorDiff in-app (Tools menu) | Green — shared `KotorDiffApp` host `[REPO]` (2026-05-24) |
| Indoor Map Builder Build/Save/Open | Partial — headless build + WOK AreaModel tests; in-game walkmesh **Unverified** `[OPEN]` |
| Editor roundtrip fidelity vs original Holocron/PyKotor | **Unverified** `[OPEN]` |
| Full AIO launch UX on Linux | **Partial** — compile green; GUI runtime not CI-tested `[OPEN]` |

## Test coverage

- `tests/OdyTools.Tests/` covers selected editor behaviors (DLG, GFF, MDL, FAC, NSS disassembly, reference finder, indoor map, etc.). `[REPO]`
- No browser/automation suite for Avalonia UX in CI. `[REPO]`

## Repo implications

- Format correctness bugs → BioWare parsers + editor code under `OdyTools/`.
- Prefer standalone editor csproj when debugging a single format surface.
