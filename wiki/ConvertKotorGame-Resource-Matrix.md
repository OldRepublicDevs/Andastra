# ConvertKotorGame – Resource Conversion Matrix

**Scope: K1 (KotOR1) and TSL (KotOR2) only.** Formats from Aurora (Neverwinter Nights), Eclipse (Dragon Age), and other BioWare engines are excluded. This document lists (1) all resources the tool **converts** today, (2) K1/TSL resource types with a **conversion-needed** decision and code reference, and (3) NCS script conversion analysis.

---

## Part 1: Resources we are converting

These are the only types for which the converter runs game-specific logic and emits target-game bytes.

| Extension | How converted | Reference (reader / writer or helper) |
|-----------|----------------|--------------------------------------|
| **ncs** | **Binary action-ID patch** (no decompilation). Scan NCS bytecode for ACTION instructions (opcode `0x05`) and remap the 2-byte action ID using a precomputed source→target lookup table. See [NCS Script Conversion](#ncs-script-conversion-deep-dive) below. | [NCSBinaryReader.cs](src/BioWare/Resource/Formats/NCS/NCSBinaryReader.cs) (format parsing), [NCSBinaryWriter.cs L135–138](src/BioWare/Resource/Formats/NCS/NCSBinaryWriter.cs) (ACTION = uint16 action_id + uint8 param_count), [ScriptDefs.cs L3326](src/BioWare/Common/Script/ScriptDefs.cs) (`KOTOR_FUNCTIONS`, 772 entries, IDs 0–771), [ScriptDefs.cs L8737](src/BioWare/Common/Script/ScriptDefs.cs) (`TSL_FUNCTIONS`, 877 entries, IDs 0–876). |
| **mdl** / **mdx** | Treated as a pair; read once, written with `MDLBinaryWriter(..., targetGame)` (different function pointers / layout). | [MDLAuto.ReadMdl](src/BioWare/Resource/Formats/MDL/MDLAuto.cs), [MDLBinaryWriter.cs L699–707](src/BioWare/Resource/Formats/MDL/MDLBinaryWriter.cs) (ctor takes `BioWareGame`). [MDLBinaryWriter.cs L788–825, L966–977](src/BioWare/Resource/Formats/MDL/MDLBinaryWriter.cs): `_game.IsK1()` / `_game.IsK2()` select trimesh and anim geometry function pointers. |
| **are** | Read → DismantleAre(are, targetGame) → BytesGff. | [AREHelpers.cs L312, L374, L449, L542](src/BioWare/Resource/Formats/GFF/Generics/ARE/AREHelpers.cs): `DismantleAre(are, game)`, `if (game.IsK2())` for Grass_Emissive, Dirty*, ChanceRain/Snow/Lightning, room DisableWeather/ForceRating. [AREHelpers.BytesAre](src/BioWare/Resource/Formats/GFF/Generics/ARE/AREHelpers.cs) calls DismantleAre with game. |
| **dlg** | Read → BytesDlg(dlg, targetGame). | [DLGHelper.cs L285, L351, L501, L573, L666](src/BioWare/Resource/Formats/GFF/Generics/DLG/DLGHelper.cs): `DismantleDlg(dlg, game)`, `if (game.IsK2())` for root (AlienRaceOwner, PostProcOwner, etc.), node (ActionParam*, Script2, NodeID, etc.), link (Active2, Logic, Param*). [DLGHelper.BytesDlg](src/BioWare/Resource/Formats/GFF/Generics/DLG/DLGHelper.cs) passes game. |
| **git** | Read GFF → ConstructGit → DismantleGit(git, targetGame) → BytesGff. | [GITHelpers.cs L362, L434, L500, L623](src/BioWare/Resource/Formats/GFF/Generics/GITHelpers.cs): `DismantleGit(git, game)`, `if (game.IsK2())` for door TweakColor/UseTweakColor and placeable TweakColor/UseTweakColor. [GITHelpers.BytesGit](src/BioWare/Resource/Formats/GFF/Generics/GITHelpers.cs) passes game. |
| **utc** | Read → DismantleUtc(utc, targetGame) → BytesGff. | [UTCHelpers.cs L268, L430, L472](src/BioWare/Resource/Formats/GFF/Generics/UTC/UTCHelpers.cs): `DismantleUtc(utc, game)`, `if (game.IsK2())` for BlindSpot, MultiplierSet, IgnoreCrePath, Hologram, WillNotRender. [UTCHelpers.BytesUtc](src/BioWare/Resource/Formats/GFF/Generics/UTC/UTCHelpers.cs) passes game. |
| **utd** | Read → BytesUtd(utd, targetGame). | [UTDHelpers.cs L156, L189–197, L247](src/BioWare/Resource/Formats/GFF/Generics/UTDHelpers.cs): `DismantleUtd(utd, game)`, `if (game.IsK2() || ...)` for OpenLockDiff/Mod, `if (game.IsK2())` for OpenState, NotBlastable. [UTDHelpers.BytesUtd](src/BioWare/Resource/Formats/GFF/Generics/UTDHelpers.cs) passes game. |
| **ute** | Read GFF → ConstructUte → DismantleUte(ute, targetGame) → BytesGff. | [UTEHelpers.cs L92, L130](src/BioWare/Resource/Formats/GFF/Generics/UTEHelpers.cs): `DismantleUte(ute, game)`, `if (game.IsK2())` for creature GuaranteedCount. |
| **uti** | Read → BytesUti(uti, targetGame). | [UTIHelpers.cs L101, L125, L177](src/BioWare/Resource/Formats/GFF/Generics/UTI/UTIHelpers.cs): `DismantleUti(uti, game)`, `if (game.IsK2())` for UpgradeLevel. [UTIHelpers.BytesUti](src/BioWare/Resource/Formats/GFF/Generics/UTI/UTIHelpers.cs) passes game. |
| **utp** | Read GFF → ConstructUtp → DismantleUtp(utp, targetGame) → BytesGff. | [UTPHelpers.cs L139, L207](src/BioWare/Resource/Formats/GFF/Generics/UTPHelpers.cs): `DismantleUtp(utp, game)`, `if (game.IsK2())` for NotBlastable, OpenLockDiff/DiffMod, OnFailToOpen. |

**Summary:** We convert **ncs**, **mdl/mdx** (pair at archive layer), and GFF types **are, dlg, git, utc, utd, ute, uti, utp**. All other types are copied as-is (with module/save alias and NFO remap for K1→TSL when applicable). Blocked conversions are reported; fallback stubs are written when possible (e.g. safe no-op NCS for unmappable TSL→K1 scripts); reports are written to `conversion_blocked_report.txt` and `conversion_blocked_report.json` in the output directory.

---

## Part 2: K1/TSL resource types – conversion needed or not

Only formats that appear in K1 or TSL installations. Whether conversion logic is required, with reader/writer references.

### Binary / non-GFF formats (K1/TSL; no game parameter in writer)

| Extension | Conversion needed? | Reader | Writer | Why not / N/A |
|-----------|--------------------|--------|--------|----------------|
| **2da** | No | [TwoDABinaryReader](src/BioWare/Resource/Formats/TwoDA/TwoDABinaryReader.cs), [TwoDAAuto.Read2DA](src/BioWare/Resource/Formats/TwoDA/TwoDAAuto.cs) | [TwoDABinaryWriter](src/BioWare/Resource/Formats/TwoDA/TwoDABinaryWriter.cs), [TwoDAAuto.Bytes2DA](src/BioWare/Resource/Formats/TwoDA/TwoDAAuto.cs) | Writer has no `BioWareGame`; format is shared. |
| **tlk** | No | [TLKBinaryReader](src/BioWare/Resource/Formats/TLK/TLKBinaryReader.cs), [TLKAuto.ReadTlk](src/BioWare/Resource/Formats/TLK/TLKAuto.cs) | [TLKBinaryWriter](src/BioWare/Resource/Formats/TLK/TLKBinaryWriter.cs), [TLKAuto.BytesTlk](src/BioWare/Resource/Formats/TLK/TLKAuto.cs) | Writer has no game parameter; layout is shared. |
| **ssf** | No | [SSFBinaryReader](src/BioWare/Resource/Formats/SSF/SSFBinaryReader.cs) | [SSFBinaryWriter](src/BioWare/Resource/Formats/SSF/SSFBinaryWriter.cs) | No game-specific logic in codebase. |
| **lip** | No | [LIPBinaryReader](src/BioWare/Resource/Formats/LIP/LIPBinaryReader.cs), LIPAuto | [LIPBinaryWriter](src/BioWare/Resource/Formats/LIP/LIPBinaryWriter.cs) | No game parameter in writer. |
| **lyt** | No | [LYTAsciiReader](src/BioWare/Resource/Formats/LYT/LYTAsciiReader.cs) | [LYTAsciiWriter](src/BioWare/Resource/Formats/LYT/LYTAsciiWriter.cs) | No game-specific logic. |
| **vis** | No | [VISAsciiReader](src/BioWare/Resource/Formats/VIS/VISAsciiReader.cs) | [VISAsciiWriter](src/BioWare/Resource/Formats/VIS/VISAsciiWriter.cs) | No game-specific logic. |
| **txi** | No | [TXIBinaryReader](src/BioWare/Resource/Formats/TXI/TXIBinaryReader.cs) | [TXIBinaryWriter](src/BioWare/Resource/Formats/TXI/TXIBinaryWriter.cs) | No game parameter. |
| **ltr** | No | [LTRBinaryReader](src/BioWare/Resource/Formats/LTR/LTRBinaryReader.cs) | [LTRBinaryWriter](src/BioWare/Resource/Formats/LTR/LTRBinaryWriter.cs) | No game-specific logic. |
| **wav** | No | [WAVBinaryReader](src/BioWare/Resource/Formats/WAV/WAVBinaryReader.cs) | [WAVBinaryWriter](src/BioWare/Resource/Formats/WAV/WAVBinaryWriter.cs), [WAVStandardWriter](src/BioWare/Resource/Formats/WAV/WAVStandardWriter.cs) | No game parameter. |
| **wok**, **dwk**, **pwk**, **bwm** | No | [BWMBinaryReader](src/BioWare/Resource/Formats/BWM/BWMBinaryReader.cs), BWMAuto.ReadBwm | [BWMBinaryWriter](src/BioWare/Resource/Formats/BWM/BWMBinaryWriter.cs), BWMAuto.BytesBwm | No `BioWareGame` in reader/writer; format is shared. |
| **tga**, **dds**, **tpc**, **bmp**, **jpg**, **png**, **ico** | No | Various TPC/TGA/DDS readers | TPCBinaryWriter, TPCTGAWriter, TPCDDSWriter, etc. | No game parameter; copy as-is. |
| **nss** | No | N/A (plaintext) | N/A | Source is portable; copy as-is. |
| **itp** | No | (binary palette) | (binary) | No game-specific writer in codebase; copy as-is. |

### GFF-based formats (K1/TSL) – with game parameter but no conditional write (copy-as-is)

| Extension | Conversion needed? | Helper / Writer | Why not |
|-----------|--------------------|-----------------|--------|
| **ifo** | No | [IFOHelpers.DismantleIfo(ifo, game)](src/BioWare/Resource/Formats/GFF/Generics/IFOHelpers.cs) L293 | No `if (game.IsK2())` in IFOHelpers; same layout for K1/TSL. |
| **utm** | No | [UTMHelpers.DismantleUtm(utm, game)](src/BioWare/Resource/Formats/GFF/Generics/UTM/UTMHelpers.cs) L88, BytesUtm | No IsK2/IsK1 in UTMHelpers; layout shared. |
| **pth** | No | [PTHHelpers.DismantlePth(pth, game)](src/BioWare/Resource/Formats/GFF/Generics/PTHHelpers.cs) L64, PTHAuto.BytesPth | No IsK2/IsK1 in PTHHelpers; layout shared. |
| **utt** | No | [UTTHelpers.DismantleUtt(utt, game)](src/BioWare/Resource/Formats/GFF/Generics/UTTHelpers.cs) L82, UTTAuto.BytesUtt | No IsK2/IsK1 in UTTHelpers. |
| **uts** | No | [UTSHelpers.DismantleUts(uts, game)](src/BioWare/Resource/Formats/GFF/Generics/UTSHelpers.cs) L93 | No IsK2/IsK1 in UTSHelpers. |
| **utw** | No | [UTWHelpers.DismantleUtw(utw, game)](src/BioWare/Resource/Formats/GFF/Generics/UTWHelpers.cs) L51, UTWAuto.BytesUtw | No IsK2/IsK1 in UTWHelpers. |
| **jrl** | No | [JRLHelpers.DismantleJrl(jrl, game)](src/BioWare/Resource/Formats/GFF/Generics/JRLHelpers.cs) L69 | No IsK2/IsK1 in JRLHelpers. |
| **fac** | No | [FACHelpers.BytesFac](src/BioWare/Resource/Formats/GFF/Generics/FACHelpers.cs) | No game parameter; no conditional. |

### GFF-based formats (K1/TSL) – no game-specific writer (copy-as-is)

| Extension | Conversion needed? | Notes |
|-----------|--------------------|--------|
| **gff**, **res** | No | Generic [GFFBinaryWriter](src/BioWare/Resource/Formats/GFF/GFFBinaryWriter.cs) has no game parameter; no per-type K1/TSL branches. |
| **bic**, **btc**, **btd**, **bte**, **bti**, **btm**, **btp**, **btt** | No | No Dismantle* / Bytes* with IsK2(); copy as-is. |
| **cut**, **gui**, **qdb**, **qst**, **gic** | No | No game-conditional writer in codebase; copy as-is. |

### Containers (K1/TSL: mod, sav, rim, bif, erf)

| Extension | Role | Reference |
|-----------|------|-----------|
| **erf**, **mod**, **sav** | Module/save archives. [ERFAuto](src/BioWare/Resource/Formats/ERF/ERFAuto.cs). Each contained resource is passed to ResourceConverter. | No game-specific container layout; content conversion is per-type. |
| **rim** | [RIMAuto](src/BioWare/Resource/Formats/RIM/RIMAuto.cs), [RIMBinaryReader](src/BioWare/Resource/Formats/RIM/RIMBinaryReader.cs) / [RIMBinaryWriter](src/BioWare/Resource/Formats/RIM/RIMBinaryWriter.cs). Same as above. | Same. |
| **bif**, **bzf** | [BIFBinaryReader](src/BioWare/Resource/Formats/BIF/BIFBinaryReader.cs) / [BIFBinaryWriter](src/BioWare/Resource/Formats/BIF/BIFBinaryWriter.cs). ResRef from [KEY](src/BioWare/Resource/Formats/KEY/KEYAuto.cs) where needed; each resource converted per type. | Same. |
| **key** | Read for BIF resref resolution only; not rewritten as a “converted” resource. | [KEYAuto](src/BioWare/Resource/Formats/KEY/KEYAuto.cs), [KEYBinaryReader](src/BioWare/Resource/Formats/KEY/KEYBinaryReader.cs) / [KEYBinaryWriter](src/BioWare/Resource/Formats/KEY/KEYBinaryWriter.cs). |

---

## Summary table: conversion needed vs implemented

| Type | In both games | Conversion needed (evidence) | Converted today |
|------|----------------|------------------------------|-----------------|
| ncs | Yes | Yes – action table diverges at ID 768+ (see [deep dive](#ncs-script-conversion-deep-dive)) | Yes (binary action-ID patch, no decompile) |
| mdl/mdx | Yes | Yes – MDLBinaryWriter(game) | Yes (pair at archive layer) |
| are | Yes | Yes – AREHelpers DismantleAre IsK2() | Yes |
| dlg | Yes | Yes – DLGHelper IsK2() | Yes |
| git | Yes | Yes – GITHelpers IsK2() | Yes |
| utc | Yes | Yes – UTCHelpers IsK2() | Yes |
| utd | Yes | Yes – UTDHelpers IsK2() | Yes |
| ute | Yes | Yes – UTEHelpers IsK2() | Yes |
| uti | Yes | Yes – UTIHelpers IsK2() | Yes |
| utp | Yes | Yes – UTPHelpers IsK2() | Yes |
| ifo, utm, pth, utt, uts, utw, jrl, fac | Yes | No – no IsK2/IsK1 in helper body | No (copy-as-is) |
| 2da, tlk, ssf, lip, lyt, vis, txi, ltr, wav, wok, bwm, … | Yes | No – no game parameter in binary writer | No (copy-as-is) |
| gff, res, bic, btc, btd, bte, bti, btm, btp, btt, cut, gui, qdb, qst | Yes | No – no game-conditional writer in codebase | No (copy-as-is) |

All references are relative to the repository root. gam (Aurora) and cnv (Eclipse) are not K1/TSL formats and are excluded from this matrix. (e.g. `src/BioWare/...`).

---

## NCS Script Conversion Deep Dive

### Why NOT decompile → recompile

The decompile/recompile path (`NCSAuto.DecompileNcs` → text → `NCSAuto.CompileNss`) is fragile and heavyweight:
- Decompilation is lossy (variable names, control flow may not round-trip)
- Recompilation requires a complete include-file tree (script library) for the target game
- Some scripts may fail to decompile or recompile cleanly
- It is orders of magnitude slower than a binary patch

### The NCS binary format is game-agnostic

The NCS file format is **identical** between K1 and TSL:
- Header: `"NCS "` + `"V1.0"` + magic byte `0x42` + uint32 file size (13 bytes total)
- Instructions: sequential stream of `opcode (1 byte) + qualifier (1 byte) + args`
- All 35 opcodes are shared (`0x01`–`0x42`); no game-specific opcodes exist
- Jump offsets, stack operations, constants, strings — all identical

**Evidence:**
- [NCSBinaryReader.cs](src/BioWare/Resource/Formats/NCS/NCSBinaryReader.cs): no `BioWareGame` parameter; reads any NCS file regardless of game
- [NCSBinaryWriter.cs](src/BioWare/Resource/Formats/NCS/NCSBinaryWriter.cs): no `BioWareGame` parameter; writes format-agnostic bytes
- [NWScriptOPCodes.ts](vendor/KotOR.js/src/nwscript/NWScriptOPCodes.ts): identical opcode table for both games

### The ONLY difference: engine action IDs

When a script calls an engine function (e.g. `GetFirstPC()`, `ActionStartConversation()`), the compiled bytecode contains an **ACTION instruction**:

```
Byte 0:   0x05          (ACTION opcode)
Byte 1:   qualifier     (ignored by VM)
Bytes 2–3: action_id    (uint16, big-endian) — index into the game's function table
Byte 4:   param_count   (uint8)             — how many args the VM pops from the stack
```

The `action_id` is an index into the engine's action table. K1 and TSL have **different action tables** at the upper end.

### Action table mapping (from KotOR.js NWScriptDefK1/K2 and ScriptDefs.cs)

| ID Range | K1 | TSL | Status |
|----------|-----|-----|--------|
| **0–767** | 768 shared functions (Random, GetFirstPC, ActionStartConversation, ...) | Same 768 functions at **same IDs** | **Binary-compatible. No patching needed.** |
| **768** | `IsMoviePlaying` | `GetScriptParameter` (OEI) | **Divergent** |
| **769** | `QueueMovie` | `SetFadeUntilScript` (OEI) | **Divergent** |
| **770** | `PlayMovieQueue` | `EffectForceBody` (OEI) | **Divergent** |
| **771** | `YavinHackCloseDoor` | `GetItemComponent` (OEI) | **Divergent** |
| **772–876** | *(does not exist)* | 105 TSL-only functions (GetItemComponentPieceValue, EffectFury, GetCombatActionsPending, RebuildPartyTable, ...) | **TSL-only** |

K1's four divergent functions (768–771) were late additions (Xbox DLC / Yavin patch). TSL was branched from an earlier K1 codebase; Obsidian replaced those slots with their own functions and extended the table.

**Crucially, TSL also has the K1 functions at relocated IDs:**

| Function | K1 ID | TSL ID | Notes |
|----------|-------|--------|-------|
| `IsMoviePlaying` | 768 | **805** | TSL comment: "PC CODE MERGER — dummy func so we can compile" |
| `QueueMovie` | 769 | **806** | — |
| `PlayMovieQueue` | 770 | **807** | — |
| `YavinHackCloseDoor` | 771 | **808** | TSL name: `YavinHackDoorClose` |

### Binary-patch algorithm

For each `.ncs` file:

1. **Validate header** (13 bytes: `"NCS V1.0"` + `0x42` + uint32 size)
2. **Walk instructions** starting at offset 13:
   - Read opcode (1 byte) + qualifier (1 byte)
   - If opcode == `0x05` (ACTION): read action_id (uint16 BE at +2) and param_count (uint8 at +4)
   - Otherwise: skip the instruction's argument bytes based on opcode type
3. **For each ACTION instruction**, check the action_id against a remap table:
   - If the action_id is in the shared range (0–767): **no change**
   - If the action_id needs remapping: **overwrite the 2 bytes at offset+2** with the target game's action_id
   - If the action_id has no target equivalent (TSL→K1 only): **record blocked conversion**, replace the script with a safe no-op NCS, and add an entry to the blocked report
4. **File size is unchanged** — only 2 bytes per affected ACTION are rewritten in-place

### K1 → TSL remap table

Only 4 IDs need remapping. All K1 functions have TSL equivalents.

| K1 action_id | K1 function | TSL action_id | TSL function | Param count match? |
|------|-------------|------|-------------|-------------------|
| 768 | `IsMoviePlaying` | 805 | `IsMoviePlaying` | Yes (0 params) |
| 769 | `QueueMovie` | 806 | `QueueMovie` | Yes (STRING, INT) |
| 770 | `PlayMovieQueue` | 807 | `PlayMovieQueue` | Yes (INT) |
| 771 | `YavinHackCloseDoor` | 808 | `YavinHackDoorClose` | Yes (OBJECT) |

**K1→TSL is 100% lossless.** Every K1 action ID has a valid TSL mapping.

### TSL → K1 remap table

| TSL action_id | TSL function | K1 action_id | K1 function | Status |
|------|-------------|------|-------------|--------|
| 805 | `IsMoviePlaying` | 768 | `IsMoviePlaying` | Remap |
| 806 | `QueueMovie` | 769 | `QueueMovie` | Remap |
| 807 | `PlayMovieQueue` | 770 | `PlayMovieQueue` | Remap |
| 808 | `YavinHackDoorClose` | 771 | `YavinHackCloseDoor` | Remap |
| 768–804 | TSL-only (GetScriptParameter, SetFadeUntilScript, EffectForceBody, ...) | — | — | **No K1 equivalent** |
| 809–876 | TSL-only (EffectFury, EffectBlind, GetCombatActionsPending, ...) | — | — | **No K1 equivalent** |

For unmappable TSL action IDs (768–804, 809–876), conversion is **not semantically possible** by simple ID remap:
1. **There is no K1 engine function table entry** for those IDs.
2. **Substituting a different function ID would change behavior** (not an equivalent conversion).
3. **Leaving IDs unchanged can cause runtime failure** in K1 (out-of-range action lookup / undefined behavior).

**Converter policy:**
- **K1→TSL**: Remap is lossless; all K1 action IDs have TSL equivalents.
- **TSL→K1**: Unmappable calls (768–804, 809–876) are replaced with a safe no-op NCS; the resource is recorded in the blocked report. Output remains runnable; the affected script becomes a no-op.

### Parameter count safety for shared functions with signature differences

`ActionStartConversation` (ID 204) has different parameter counts:
- K1: 11 params (OBJECT, STRING, INT, INT, INT, STRING, STRING, STRING, STRING, STRING, STRING)
- TSL: 15 params (same 11 + INT bUseLeader, INT nBarkX, INT nBarkY, INT bDontClearAllActions)

The param count is encoded in the ACTION instruction's bytecode, not the function table. So:
- **K1→TSL**: K1 bytecode says `ACTION 204 11`. TSL pops 11 from stack, uses defaults for the extra 4. **Safe.** The converter treats "fewer args than TSL expects" as safe.
- **TSL→K1**: TSL bytecode says `ACTION 204 15`. K1 pops 15 from stack, but K1's implementation only reads 11. The NWScript VM parameter accessor reads what it needs; extras are discarded. **Safe** (the VM is responsible for popping, not the engine function).
- **Other mismatches**: Logged as warnings; conversion proceeds.

### What percentage of scripts need patching?

In a vanilla K1 installation, the divergent range (768–771) covers:
- `IsMoviePlaying` / `QueueMovie` / `PlayMovieQueue` — movie queuing, used in a handful of cutscene scripts
- `YavinHackCloseDoor` — used only in Yavin Station DLC scripts

Most vanilla K1 scripts use only action IDs 0–767 and require zero modification.

For TSL→K1, the TSL-only range (768–876) covers force powers, crafting, party management, and other TSL-specific features. Scripts for TSL-specific gameplay will inherently have no K1 content to run against.

### Implementation: NCSActionPatcher

[NCSActionPatcher.cs](src/BioWare/Resource/Formats/NCS/NCSActionPatcher.cs) operates on raw bytes — no NCS model, no decompilation. It:

1. **Validates header** (`"NCS V1.0"` + `0x42`)
2. **Walks instructions** using `GetInstructionSize()` for every NCS opcode (CPDOWNSP, CPTOPSP, CONSTx, ACTION, JMP, DESTRUCT, STORE_STATE, etc.)
3. **Patches ACTION** at opcode `0x05`: overwrites the 2-byte action_id at offset+2 with the remapped ID
4. **Tracks unmappable** TSL-only action IDs (768–804, 809–876) for TSL→K1
5. **Tracks param-count mismatches** for shared actions with different signatures (e.g. ActionStartConversation 11 vs 15)
6. **Returns** `PatchResult` with `Data`, `ActionsPatched`, `ActionsTotal`, `UnmappableActionIds`, `ParamCountMismatches`

The remap table is 4 entries for K1→TSL and 4 for TSL→K1. Unmappable IDs cause `ResourceConverter` to throw `ConversionBlockedException` with `FallbackData` (minimal RETN-only NCS); the service catches it, writes the fallback, and records the entry in `conversion_blocked_report.txt` and `conversion_blocked_report.json`. Output remains runnable. Param-count mismatches are logged as warnings; K1→TSL "fewer args than target" is treated as safe.
