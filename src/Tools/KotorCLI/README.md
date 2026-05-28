# KotorCLI for .NET

A comprehensive build tool for KOTOR projects with cli-compatible syntax, ported from PyKotor's KotorCLI to C#/.NET.

## Status

**Partial implementation, many commands wired.** The Holocron port phase on this branch delivers installation reference search, archive extract/list/create/search, standalone format converters, script decompile/disassemble/assemble, resource converters, utilities, and validation checks through BioWare. The **kotorcli.cfg build pipeline** (`config`, `init`, `list`, `unpack`, `convert`, `compile`, `pack`, `install`) is implemented for typical mod workflows; **`launch`** remains a fail-fast stub. See command tables below for **wired** / **partial** / **stub** labels.

Legend: **wired** = functional CLI backed by BioWare or `Conversions`; **partial** = works with known gaps; **stub** = not implemented or fail-fast only.

## Project Structure

- `Program.cs` - Main entry point with root command setup
- `Commands/` - All command implementations
- `Configuration/` - TOML configuration file parser (KotorCLIConfig)
- `Logging/` - Logger implementations (Standard, Verbose, Debug, Quiet)

## Commands

### Core Build Commands
- `config` - Read/write `kotorcli.cfg` (**wired**)
- `init` - Scaffold a new kotorcli package (**wired**)
- `list` - List targets and optional source files (**wired**)
- `unpack` - Unpack module/ERF/RIM into project sources (**wired**; `--removeDeleted` removes stale files under rule roots)
- `convert` - Convert JSON sources to GFF for configured targets (**wired**)
- `compile` - Compile NSS for configured targets (**wired**)
- `pack` - Pack sources into module archives (**wired**)
- `install` - Convert, compile, pack, and copy to game install (**wired**)
- `launch` - Full pipeline plus game launch (**stub** — fail-fast; use `--dry-run` to resolve paths only)

### Archive Commands
- `extract` - Extract KEY/BIF, RIM, ERF/MOD/SAV/HAK (**wired**)
- `list-archive` - List archive contents (**wired**)
- `create-archive` - Create ERF/MOD/SAV/HAK or RIM from a directory (**wired**)
- `search-archive` - Search resource names (and optional content) in archives (**wired**)
- `key-pack` - Build KEY from a BIF directory (**wired**)

### Archive test coverage (plans 140–186)

Archive CLI commands have **substantial integration test coverage** in `tests/KotorCLI.Tests/` (**273** tests total on net9.0):

| Area | Coverage |
|------|----------|
| `list-archive` / `search-archive` | RIM, MOD, ERF — verbose, filter, case-sensitive, content search, error paths |
| BIF + sibling KEY | Named resources, filter/no-match, case-sensitive search |
| Standalone KEY | List/search, verbose, filter/no-match |
| BIF without KEY | Blank ResRef / extension-only names (`.utc`), filter and search paths |
| `extract` | Baseline (no filter) and `--filter` on RIM, MOD, ERF, BIF+KEY, KEY; default output dir; BIF-stem subdirs for KEY |
| `create-archive` | RIM/MOD/ERF baselines, `--filter`, empty input directories |
| `key-pack` | BIF directory to KEY (happy path, filter, missing directory) |

Intentionally out of scope for archive closure: full `launch` pipeline and game-runtime behavior.

### Format Conversion Commands
- `gff2json`, `json2gff` - GFF ↔ JSON (**wired**)
- `gff2xml`, `xml2gff` - GFF ↔ XML (**wired**)
- `tlk2xml`, `xml2tlk` - TLK ↔ XML (**wired**)
- `ssf2xml`, `xml2ssf` - SSF ↔ XML (**wired**)
- `2da2csv`, `csv22da` - 2DA ↔ CSV (**wired**)

### Script Tools
- `decompile` - Decompile NCS to NSS (**wired** via BioWare)
- `disassemble` - Disassemble NCS bytecode to text (**wired** via `Scripts.DisassembleNcs`)
- `assemble` - Compile NSS to NCS (**wired** via `NCSAuto.CompileNss`)

### Resource Tools
- `texture-convert` - TPC ↔ TGA (**wired**)
- `sound-convert` - WAV normalization (**wired**)
- `model-convert` - MDL/MDX conversion (**wired**)

### Reference search

Installation-wide reference finders ported from Holocron/PyKotor (BioWare `ReferenceFinder` and `ReferenceCacheHelpers`). All commands exit `0` when matches are found and `1` when none match or arguments are invalid.

| Command | Purpose | BioWare API |
|---------|---------|-------------|
| `find-refs` | Script, tag, template, or conversation ResRef | `ReferenceFinder` |
| `find-strref` | TLK StrRef in 2DA, SSF, GFF, and NCS (CONSTI) | `ReferenceCacheHelpers.FindStrRefReferences` |
| `find-2da-ref` | GFF fields indexing a 2DA row (optional full row sweep) | `ReferenceCacheHelpers.Find2DAMemoryReferences` / `CollectTwoDARowReferences` with `--full-row` |
| `find-field-value` | GFF string/ResRef field values | `ReferenceFinder.FindFieldValueReferences` |

Shared flags (where supported):

- `--install-dir` / `--installation` — KOTOR install path (or `KOTOR_PATH` / `K1_PATH`)
- `--override-only`, `--no-override`, `--no-chitin`, `--no-modules` — limit which installation areas are scanned (`find-refs`, `find-strref`, `find-2da-ref`, `find-field-value`)
- `--module-glob <pattern>` — repeatable module filename glob (`*` / `?`, case-insensitive); when set, only matching `.mod`/`.rim`/`.erf` capsules under `modules/` are scanned

**find-refs** — script/tag/template/conversation search:

```bash
dotnet run --project src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0 -- \
  find-refs k_test_hb --install-dir /path/to/kotor --type script --override-only
```

Flags: `--type script|tag|template|conversation`, `--override-only`, `--no-override`, `--no-chitin`, `--no-modules`, `--module-glob`, `--case-sensitive`, `--partial`, `--json`, `--count-only`.

**find-strref** — TLK string reference search (2DA columns, SSF sound slots, GFF localized strings, NCS CONSTI literals):

```bash
dotnet run --project src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0 -- \
  find-strref 12345 --install-dir /path/to/kotor --override-only
```

Flags: `--override-only`, `--no-override`, `--no-chitin`, `--no-modules`, `--module-glob`, `--no-ncs`, `--ncs-strref-min <n>`, `--cache-file <path>`, `--rebuild-cache`, `--json`, `--count-only`.

NCS hits report bytecode offsets (`(NCS bytecode) offset_<n>` in field-path display).

**find-2da-ref** — 2DA row index referenced from GFF (e.g. `Appearance_Type` → `appearance.2da`):

```bash
dotnet run --project src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0 -- \
  find-2da-ref appearance 17 --install-dir /path/to/kotor --override-only
```

Flags: `--override-only`, `--no-override`, `--no-chitin`, `--no-modules`, `--module-glob`, `--cache-file <path>`, `--rebuild-cache`, `--full-row`, `--json`, `--count-only`.

With `--full-row`, also searches for the row label as a GFF field value and positive StrRef values in row columns (loads the 2DA from the installation when available).

The `twoda` argument accepts a resname or filename (`appearance` or `appearance.2da`).

**find-field-value** — GFF field value search:

```bash
dotnet run --project src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0 -- \
  find-field-value player --install-dir /path/to/kotor --partial --override-only
```

Flags: `--override-only`, `--no-override`, `--no-chitin`, `--no-modules`, `--module-glob`, `--partial`, `--case-sensitive`, `--json`, `--count-only`.

Example limiting module scan to one area prefix:

```bash
dotnet run --project src/Tools/KotorCLI/KotorCLI.csproj --framework net9.0 -- \
  find-refs k_def_buff --install-dir /path/to/kotor --type script --no-override --no-chitin --module-glob tar_m01*
```

### Utilities
- `cat`, `diff`, `grep`, `merge` - File/archive utilities (**wired**)
- `stats` - Per-format file statistics (**partial** — not all resource types analyzed)
- `validate` - Structural validation (**partial**)

### Validation
- `check-txi`, `check-2da` - TXI and 2DA validators (**wired**)

## Known Issues

1. **`launch` is a stub** — documents the full pipeline but exits unless `--dry-run` is used.
2. **Remaining test gaps (non-archive)** — build-pipeline commands (`init`, `config`, `convert`, `compile`, `pack`, `install`, `unpack`, `list`), format convert integration, reference finders, and `launch` path resolution (`ResolveGameBinary`) have coverage; archive create/list/search/extract/key-pack is **substantially closed** (see Archive test coverage above). `stats`/`validate` integration tests cover GFF, 2DA, ERF, BIF, TLK, and NCS (plans 189–195). Remaining gaps: `launch` workflow beyond dry-run and any newly wired commands without tests. **273** tests in `KotorCLI.Tests` on net9.0.
3. **System.CommandLine** — prefer `.Options.Add()` / current `Cli.Opt` helpers when touching command definitions.

## Next Steps

1. Implement `launch` (or document a supported external launcher workflow).
2. Expand coverage for partial utilities (`stats`, `validate`) and any newly wired commands.
3. Keep this README inventory in sync when adding or stubbing commands.

## References

Original Python implementation: `vendor/PyKotor/Tools/KotorCLI/`

