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
| StrRef find-refs NCS CONSTI cache path | Green — `StrRefReferenceCache` / `IncludeNcsStrRefScan` gating; **2** KotorCLI CLI tests at plan **286** baseline; superseded by **94** NcsConsti + **10** StrRefReferenceHelper rows (plans **324**–**370**) `[REPO]` |
| Script ResRef NCS CONSTS scanner paths | Green — `NcsConstStringScanner` + `(NCS bytecode) offset_<n>` field paths; **3** NcsConstString + **5** FindScriptResRefInNcsBytes tests `[REPO]` (plan **287**, 2026-05-28) |
| Compiled NCS script ResRef installation e2e | Green — `FindScriptReferences_OverrideCompiledNcs_ReturnsNcsBytecodePath`; **1** ReferenceFinder test `[REPO]` (plan **289**, 2026-05-28) |
| KotorCLI find-refs compiled NCS CLI subprocess | Green — `Cli_FindRefs_Script_CompiledNcsInOverride_ExitsZero`; **1** FindRefsCommandCli test `[REPO]` (plan **290**, 2026-05-28) |
| NCS CONSTI StrRef vs 2DA-memory threshold disambiguation | Green — threshold + opcode-context + action-signature + stack-store heuristics; **94** NcsConsti tests `[REPO]` (plans **086**/**095**/**099**/**292**–**294**/**303**/**305**/**307**/**324**–**370**, 2026-06-03) |
| NCS CONSTI control-flow reachability (JZ/JNZ/JMP fork, while-break, subroutine/infinite-loop) | Green — `NcsConstiScanner` forward scan + cache indexing; **94** NcsConsti tests `[REPO]` (plans **324**–**370**, 2026-06-03) |
| KotorCLI find-strref cache-path NCS control-flow gating | Green — in-process + CLI subprocess cache-path tests; **18** FindStrRef + **12** InstallationRefSearch CLI tests `[REPO]` (plans **337**–**338**, 2026-05-24) |
| BioWare find-strref slow vs cache NCS semantics | Green — slow path matches raw CONSTI; cache path excludes dead-path consumers; **3** BioWare FindStrRefReferences CF tests `[REPO]` (plan **339**, 2026-05-24) |
| KotorCLI find-strref slow vs cache documentation | Green — README table + examples under `find-strref` `[REPO]` (plan **340**, 2026-05-24) |
| OdyTools StrRef find-refs cache-path NCS gating | Green — `StrRefReferenceHelper` builds `StrRefReferenceCache` when `IncludeNcsStrRefScan`; **10** StrRefReferenceHelper tests `[REPO]` (plan **346**, 2026-06-03) |
| build-and-test-ladder ref-search filter steps | Green — **Step 3b** NCS CONSTI/StrRef + **Step 3c** ReferenceFinder targeted `dotnet test` filters in [build-and-test-ladder.md](../50-execution/build-and-test-ladder.md) `[REPO]` (plans **348**–**351**, 2026-06-03) |
| 2DA editor / ComboBox2DA row **Find References** | Green — `TwoDAMemoryReferenceHelper` + `OdyTool2DA` context menu (plans 083, 200) `[REPO]` |
| KotorDiff in-app (Tools menu) | Green — shared `KotorDiffApp` host `[REPO]` (2026-05-24) |
| OdyToolLIP keyframe editor UI | Green — list/add/update/delete + `Build` roundtrip; **10** `OdyToolLIPTests` `[REPO]` (plan **377**, 2026-05-24) |
| OdyToolLIP batch WAV→LIP processor | Green — Holocron placeholder shapes; **5** `LipBatchProcessorTests` `[REPO]` (plan **376**, 2026-05-24) |
| OdyToolLIP audio load and preview playback | Green — WAV load, NAudio play/stop; covered in `OdyToolLIPTests` `[REPO]` (plan **378**, PR **#67**) |
| OdyToolLIP playback sync (viseme label + keyframe highlight) | Green — discrete shape/index at playback time; **10** `OdyToolLIPTests` `[REPO]` (plan **379**, PR **#68**) |
| OdyToolLIP 3D head preview (Appearance + mouth overlay) | Green — `LipHeadPreviewHelper` + `ModelRenderer` hint; **15** LIP/batch tests `[REPO]` (plan **380**, PR **#69**) |
| Indoor Map Builder Build/Save/Open | Partial — headless build + WOK AreaModel tests; in-game walkmesh **Unverified** `[OPEN]` |
| Editor roundtrip fidelity vs original Holocron/PyKotor | **Unverified** `[OPEN]` |
| Full AIO launch UX on Linux | **Partial** — compile green; GUI runtime not CI-tested `[OPEN]` |

## Test coverage

- `tests/OdyTools.Tests/` covers selected editor behaviors (DLG, GFF, MDL, FAC, NSS disassembly, reference finder, indoor map, etc.). `[REPO]`
- No browser/automation suite for Avalonia UX in CI. `[REPO]`

## Repo implications

- Format correctness bugs → BioWare parsers + editor code under `OdyTools/`.
- Prefer standalone editor csproj when debugging a single format surface.
