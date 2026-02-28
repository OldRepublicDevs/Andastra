# Reva match-function runbook for GFF address equivalents

When the **Reva** (agentdecompile) MCP server is connected, use it to fill K2 Legacy equivalents for addresses still marked "—" in `GFF-Save-Load-Complete-Reference.md`.

## 1. Open project

```json
{ "path": "C:/Users/boden/AndastraGhidraProject.gpr" }
```
**Tool:** `open`

## 2. match-function calls (start small, then increase)

Use **K1** as source, **K2 legacypc** as target. Start with **2 function identifiers** per call; if that’s stable, try 4, then 8.

**Important:** With the improved tool, you may be able to send many identifiers in one call; if the backend disconnects, reduce the batch size.

### Batch 1 (2 identifiers) – module save/load

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": ["0x004c7870", "0x0050e190"]
}
```
- `0x004c7870` = SavePlayers  
- `0x0050e190` = LoadArea  

### Batch 2 (2 identifiers) – GIT entity save

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": ["0x00507810", "0x005078d0"]
}
```
- Save Doors, Save Triggers  

### Batch 3 (2 identifiers)

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": ["0x00507990", "0x00507a50"]
}
```
- Save Encounters, Save Waypoints  

### Batch 4 (2 identifiers)

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": ["0x00507b10", "0x00507ca0"]
}
```
- Save Sounds, Save Stores  

### Batch 5 (2 identifiers)

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": ["0x00507d60", "0x004cec50"]
}
```
- Save AreaEffects, CSWSObject::SaveObjectState  

### Batch 6 (4 identifiers) – per-entity callees

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": ["0x004d1cf0", "0x00588ad0", "0x0058e660", "0x00591350"]
}
```
- LoadObjectState, SaveDoor, SaveTrigger, SaveEncounter  

### Batch 7 (4 identifiers)

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": ["0x005c8230", "0x00586a70", "0x005c6cd0", "0x00594d80"]
}
```
- SaveWaypoint, SavePlaceable, SaveStore, SaveEffect  

### Batch 8 (4 identifiers)

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": ["0x004cc9d0", "0x004cc7e0", "0x0059adb0", "0x0059b250"]
}
```
- SaveEffectList, SaveActionQueue, SaveVarTable script/var  

### Batch 9 (4 identifiers)

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": ["0x004124e0", "0x004111c0", "0x00411940", "0x004cca50"]
}
```
- AddListElement, GetElementType, GetListCount, SaveListenData  

### Batch 10 (4 identifiers) – load + misc

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": ["0x00504de0", "0x0050a0e0", "0x0050a7b0", "0x005057a0"]
}
```
- Load Items, Load Doors, Load Placeables, Load Stores  

### Batch 11 (4 identifiers)

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": ["0x00505af0", "0x00560970", "0x005649f0", "0x0058c5f0"]
}
```
- Load AreaEffects, LoadCreature, LoadFromTemplate, LoadDoorExternal  

### Batch 12 (4 identifiers)

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": ["0x0058da80", "0x00595d20", "0x004d1be0", "0x004cecb0"]
}
```
- LoadTrigger, LoadAreaEffect, LoadEffectList, LoadActionQueue  

### Batch 13 (2 identifiers)

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": ["0x0058e0a0", "0x005b1b90"]
}
```
- CSWSAmbientSound::Load, CSWSCreatureStats::SaveStats  

### Optional: one-shot (if improved tool allows)

If the improved `match-function` accepts many identifiers without disconnecting, you can send all missing K1 addresses in one call:

```json
{
  "sourceProgram": "/k1_win_gog_swkotor.exe",
  "targetProgram": "/k2_win_gog_legacypc_swkotor2.exe",
  "functionIdentifiers": [
    "0x004c7870", "0x0050e190", "0x00507810", "0x005078d0", "0x00507990", "0x00507a50",
    "0x00507b10", "0x00507ca0", "0x00507d60", "0x004cec50", "0x004d1cf0", "0x00588ad0",
    "0x0058e660", "0x00591350", "0x005c8230", "0x00586a70", "0x005c6cd0", "0x00594d80",
    "0x004cc9d0", "0x004cc7e0", "0x0059adb0", "0x0059b250", "0x004124e0", "0x004111c0",
    "0x00411940", "0x004cca50", "0x00504de0", "0x0050a0e0", "0x0050a7b0", "0x005057a0",
    "0x00505af0", "0x00560970", "0x005649f0", "0x0058c5f0", "0x0058da80", "0x00595d20",
    "0x004d1be0", "0x004cecb0", "0x0058e0a0", "0x005b1b90", "0x004d3ec0"
  ]
}
```

## 3. Parse output and update the doc

- Save each `match-function` JSON output to a file (or concatenate if you used multiple batches).
- Run: `python parse_match_output.py <output.txt>`
- Use the printed table to replace "—" in `GFF-Save-Load-Complete-Reference.md` (section "Address equivalents (all programs)").

**Note:** If the tool returns matches with **sourceProgramPath** = K2 legacypc and **targetResults** containing K1, then in the K1 target result: `sourceAddress` = K2 Legacy, `targetAddress` = K1. So K1 → K2 Legacy is: for each K1 address, take the `sourceAddress` of the row where `targetAddress` equals that K1 address.
