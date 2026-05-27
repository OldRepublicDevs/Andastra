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
- `config` - Configuration management (stub)
- `init` - Project initialization (stub)
- `list` - List targets (stub)
- `unpack` - Unpack archives (stub)
- `convert` - Convert JSON to GFF (stub)
- `compile` - Compile NSS scripts (stub)
- `pack` - Pack sources into modules (stub)
- `install` - Install to KOTOR directory (stub)
- `launch` - Launch game (stub)

### Archive Commands
- `extract` - Extract from archives (stub)
- `list-archive` - List archive contents (stub)
- `create-archive` - Create archives (stub)
- `search-archive` - Search archives (stub)
- `key-pack` - Create KEY files (stub)

### Format Conversion Commands
- `gff2json`, `json2gff` - GFF ↔ JSON (wired)
- `gff2xml`, `xml2gff` - GFF ↔ XML (stub)
- `tlk2xml`, `xml2tlk` - TLK ↔ XML (stub)
- `ssf2xml`, `xml2ssf` - SSF ↔ XML (stub)
- `2da2csv`, `csv22da` - 2DA ↔ CSV (wired)

### Script Tools
- `decompile` - Decompile NCS to NSS (stub)
- `disassemble` - Disassemble NCS bytecode to text (via BioWare Scripts.DisassembleNcs)
- `assemble` - Compile NSS to NCS (via BioWare NCSAuto.CompileNss)

### Resource Tools
- `texture-convert` - Convert textures (stub)
- `sound-convert` - Convert sounds (stub)
- `model-convert` - Convert models (stub)

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
- `diff`, `grep`, `stats`, `validate`, `merge`, `cat` - Utility commands (`cat`/`diff`/`grep`/`merge` wired; `stats`/`validate` partial)

### Validation
- `check-txi`, `check-2da` - Validation commands (`check-txi`/`check-2da` wired)

## Known Issues

1. **System.CommandLine API Usage**: Some commands may still need API cleanup (use `.Options.Add()` instead of deprecated patterns).
2. **Project Reference**: Path to Andastra.Parsing needs verification for remaining stub commands.
3. **Implementation**: Core build/archive pipeline commands (`init`, `unpack`, `pack`, `install`, etc.) remain stubs.

## Next Steps

1. Implement core build commands (init, unpack, convert pipeline, compile, pack, install)
2. Wire remaining format conversion stubs (`gff2xml`, TLK/SSF XML pairs)
3. Expand automated test coverage for newly wired commands
4. Integrate with Andastra.Parsing library where BioWare coverage is incomplete

## References

Original Python implementation: `vendor/PyKotor/Tools/KotorCLI/`

