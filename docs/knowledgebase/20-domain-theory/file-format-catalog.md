# File Format Catalog

Index of BioWare parsers with wiki spec pointers and test coverage.

## Parser Locations

Primary implementation: `src/BioWare/Resource/Formats/` `[REPO]`

| Format | Parser folder | Wiki spec (root) | Tests |
|--------|---------------|------------------|-------|
| GFF | `GFF/` | `wiki/GFF-*.md`, `wiki/Bioware-Aurora-*.md` | BioWare.Tests GFF roundtrips |
| 2DA | `TwoDA/` | `wiki/Bioware-Aurora-2DA.md` | BioWare.Tests |
| TLK | `TLK/` | `wiki/Bioware-Aurora-TalkTable.md` | BioWare.Tests |
| BWM/WOK | `BWM/` | `wiki/BWM-File-Format.md` | BioWare.Tests |
| LYT | `LYT/` | `wiki/LYT-File-Format.md` | BioWare.Tests |
| MDL/MDX | `MDL/` | `wiki/MDL-*.md` | OdyTools.Tests MDL |
| NCS | `NCS/` | `wiki/NCS-File-Format.md` | Andastra.Tests NCS roundtrip |
| ERF/RIM/MOD | `ERF/`, `RIM/` | Aurora capsule docs in wiki | BioWare.Tests modules |
| SSF | `SSF/` | — | BioWare.Tests |
| SAV | `SAV/` | `wiki/GFF-Save-Load-*.md` | BioWare.Tests saves |
| TPC/TEX | `TPC/`, `TEX/` | — | OdyTools.Tests TPC |
| LIP/LTR/LYT | respective folders | wiki mirrors | partial |

Exact test file names under `tests/BioWare.Tests/Formats/`. `[REPO]`

## Wiki Authority

For **byte layout** and struct field offsets, prefer `wiki/` (58 root markdown files). `[REPO]`

KB and BioWare code are authoritative for **Andastra parser behavior** — when wiki and parser disagree, file `[OPEN]` and add roundtrip test.

## Kaitai / Codegen

Some formats use Kaitai Struct (e.g. TPC investigation in `docs/kaitai_struct_*.md`). `[REPO]`

## Adding a New Format

1. Implement parser under `src/BioWare/Resource/Formats/`
2. Add or extend wiki spec if format is public modding surface
3. Add roundtrip test in `tests/BioWare.Tests/`
4. Link from this catalog

## Repo Implications

- Do not copy vendor PyKotor wiki wholesale — root `wiki/` is the maintained set for this repo.
- Format PRs without tests are high-risk for mod compatibility.
- Investigation JSON dumps in `docs/k1_*.json`, `docs/tsl_*.json` are RE artifacts, not parser specs.
