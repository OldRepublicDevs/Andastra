# KotOR Save/Load: Exhaustive Serialization Reference (Agentdecompile-Primary)

This document is the **single exhaustive reference** for implementing full save-the-game and load-the-game logic. It lists **every step**, **every function**, and **every callee** in order, with **K1 (and where useful K2) addresses** so you can retrieve complete decompilation from **Agentdecompile** (Ghidra MCP). The goal is to have **enough information to implement exhaustive and complete save and load**.

**What “exhaustive” means here**
- **Save:** Every piece of logic from the moment the game decides to save (e.g. **StoreCurrentModule**) through creating the ERF, writing the IFO (module state, game time, limbo creatures, faction file), writing the current area’s GIT (var tables, weather, all 10 entity lists, Properties, Maps, Cameras), writing the IFO into the ERF as "Module", and finalizing the ERF. That includes every **SaveVarTable**, **WriteField***, **AddList** / **AddListElement**, and every type-specific saver (SaveItem, SaveDoor, SaveObjectState, SaveTrigger, … SerializeCreature_K2, SaveStats, etc.).
- **Load:** Every piece of logic from the moment the game decides to load (e.g. **LoadModuleInProgress**) through creating the area, loading the ARE (header, rooms), loading the GIT (var tables, weather, UseTemplates, all 10 entity lists, Properties, Maps, Cameras), and loading path points. That includes every **LoadVarTable**, **ReadField***, **GetList** / **GetListElement** / **GetElementType**, and every type-specific loader (LoadCreature, LoadItem, LoadDoorExternal, LoadObjectState, LoadTrigger, … ReadStatsFromGff, etc.).

**Agentdecompile is the primary source:** All addresses and call relationships in this doc are intended to be verified or filled in via Agentdecompile (`get-functions`, `get-call-graph`). When a callee is not fully inlined in [GFF-GIT-Full-Save-Load-Code.md](GFF-GIT-Full-Save-Load-Code.md), this doc tells you which parent to run `get-call-graph` on and how to then run `get-functions` on the callee to get the complete logic.

**Related docs:**
- **[GFF-GIT-Full-Save-Load-Code.md](GFF-GIT-Full-Save-Load-Code.md)** — Full C/C++ decompilation already inlined for many top-level and GIT-level functions.
- **[GFF-GIT-Struct-IDs.md](GFF-GIT-Struct-IDs.md)** — Struct IDs, list names, exact order of operations, and K1/K2 address tables.

---

## 1. Agentdecompile as primary source — investigation guide

All logic below is derived from or verifiable via **Agentdecompile** (Ghidra MCP) on the Andastra Ghidra project. Binary: **k1_win_gog_swkotor.exe** (K1). Use **k2_win_gog_legacypc_swkotor2.exe** for K2.

### 1.1 Agentdecompile tools used for this documentation

- **`get-functions`** — Decompile or disassemble a single function.
  - **Parameters:** `identifier` (e.g. `0x0050ba00` or symbol name), `view` = `decompile` or `disassemble` or `info`, `limit` = line count for decompile (use 300–500 for large functions).
  - **Example:** To get full SaveGIT: `get-functions identifier=0x0050ba00 view=decompile limit=320`.
- **`get-call-graph`** — List callers and **callees** of a function.
  - **Parameters:** `functionIdentifier` (address or symbol).
  - **Use:** From any save/load function, run `get-call-graph` to get the list of callees; then run `get-functions identifier=<callee_addr> view=decompile limit=N` for each callee to get exhaustive logic.
- **`list-functions`** — Search by symbol name or string reference (e.g. `"SaveGIT"`, `"Creature List"`).

### 1.2 How to exhaustively expand any save/load path

1. Start from the **entry point** (e.g. `StoreCurrentModule` @ `0x004b2e70` for save, `LoadModuleInProgress` @ `0x004c5720` for load).
2. Run **`get-functions identifier=0x004b2e70 view=decompile limit=80`** to get the function body.
3. Run **`get-call-graph functionIdentifier=0x004b2e70`** to list all **callees**.
4. For each callee (e.g. `SaveModuleStart`, `SaveModuleInProgress`, `SaveModuleFinish`), note its address from the call-graph and run **`get-functions identifier=<addr> view=decompile limit=200`** (increase `limit` if the decompilation is truncated).
5. **Recurse:** For every callee that is part of the save/load logic (not generic runtime like `operator_new`), run `get-call-graph` again and repeat until you have no new save/load-specific callees.
6. **GFF API:** Engine functions like `CResGFF::AddList`, `CResGFF::AddListElement`, `CResGFF::WriteFieldDWORD` are in the Ghidra project; use `list-functions` or known addresses (e.g. **AddListElement** @ `0x004124e0`, **GetElementType** @ `0x004111c0`) to get their signatures if needed.

### 1.3 Key GFF/ERF API addresses (K1)

| API | K1 address | Notes |
|-----|------------|--------|
| **CResGFF::AddList** | (from get-call-graph of SaveCreatures) | Called with (gff, listHandle, rootStruct, "Creature List") |
| **CResGFF::AddListElement** | **0x004124e0** | (gff, outStruct, listHandle, structId) |
| **CResGFF::GetElementType** | **0x004111c0** | (gff, struct) → struct ID |
| **CResGFF::GetListCount** | **0x00411940** | (gff, listHandle) → count |
| **CResGFF::CreateGFFFile** | (search in SaveModuleStart / SaveGIT) | (gff, struct, "GIT ", "V2.0") |
| **CERFFile::WriteResource** | (search in SaveGIT / SaveModuleIFOFinish) | (erf, resref, type, resource, ...) — type **0x7e7** = GIT, **0x7de** = IFO, **0x7dc** = ARE |

Use **get-call-graph** from SaveGIT (0x0050ba00) or any Save* to find the exact addresses of AddList, WriteField*, AddStructToStruct in your project.

---

## 2. Exhaustive save serialization — complete logic

Every step required to **save the game** in the same order and with the same data as the engine. Each bullet is a function or engine action; **K1 address** is given so you can run **Agentdecompile `get-functions identifier=<addr> view=decompile limit=N`** to get full C/C++.

### 2.1 Module-level save (entry and three phases)

| Step | Function / action | K1 address | Agentdecompile / notes |
|------|-------------------|------------|--------------|
| 1 | **Entry:** `CServerExoAppInternal::StoreCurrentModule` | **0x004b2e70** | Callees: GetModule, IncludeModuleInSave, SaveModuleStart, SaveModuleInProgress, SaveModuleFinish. Full decomp in [GFF-GIT-Full-Save-Load-Code.md](GFF-GIT-Full-Save-Load-Code.md) § StoreCurrentModule. |
| 2 | **SaveModuleStart** — create ERF, IFO GFF, write module-level state | **0x004c8960** | Creates ERF, CreateGFFFile "IFO "/"V2.0", then SerializeIfoGameTime, SaveModuleFAC. |
| 3 | **SaveModuleInProgress** — write current area GIT into ERF | **0x004c3b10** | Calls CSWSArea::SaveGIT(area, erf, ..., party_list). |
| 4 | **SaveModuleFinish** — write IFO to ERF, optional ARE, finalize ERF | **0x004ca680** | SaveStatic (optional), SaveModuleIFOFinish (SavePlayers + WriteResource "Module" 0x7de), CERFFile::Finish. |

### 2.2 SaveModuleStart callees (IFO and FAC)

| Function | K1 address | Purpose |
|----------|------------|---------|
| **SerializeIfoGameTime** | **0x004c7050** | Writes full IFO root: Mod_ID, Mod_Creator_ID, Mod_Version, Mod_Name, Mod_Description, Mod_IsSaveGame, Mod_IsNWMFile, Mod_NWMResName (if NWM), Mod_Hak, game time, entry area/position, expansion list, cutscene list, scripts, Mod_Area_list, Mod_Tokens, **SaveVarTable** (script), **SaveVarTable** (var), SaveEventQueue, SaveLimboCreatures. ~208 lines decomp. |
| **SaveModuleFAC** | **0x004c3960** | Writes **separate file** (GAMEINPROGRESS:REPUTE), type FAC: FactionList, RepList. Not in save ERF. |
| **SaveLimboCreatures** | **0x004c5bb0** | AddList "Creature List", then for each limbo creature AddListElement(..., 4), WriteFieldDWORD ObjectId, SerializeCreature_K2. |

### 2.3 SaveModuleFinish callees

| Function | K1 address | Purpose |
|----------|------------|---------|
| **SaveStatic** | **0x004c5980** | Gets ARE list from module (IFO 0x7de), for each ARE resref writes resource into ERF (type **0x7dc**). |
| **SaveModuleIFOFinish** | **0x004c8b90** | SavePlayers(gff, struct, ...); WriteResource(erf, "Module", **0x7de**, gff); release gff/struct. |
| **SavePlayers** | **0x004c7870** | AddList "Mod_PlayerList", struct ID 0xbead; per player: Mod_CommntyName, Mod_IsPrimaryPlr, Mod_FirstName, Mod_LastName, ObjectId, **SerializeCreature_K2**. |

### 2.4 Per-area GIT save: SaveGIT and its callees

| Function | K1 address | Purpose |
|----------|------------|---------|
| **CSWSArea::SaveGIT** | **0x0050ba00** | Create GFF "GIT "/"V2.0"; **bucket area objects** (GetObjectArray, GetGameObject, AsSWSCreature / AsSWSItem / AsSWSDoor / … / AsSWSAreaOfEffectObjec; PCs → party list, rest → per-type lists); **SaveVarTable** x2; WriteFieldBYTE CurrentWeather, WeatherStarted, TransPending, TransPendNextID, TransPendCurrID; then 10 entity lists (below); then SaveProperties, SaveMaps, SavePlaceableCameras; WriteResource(erf, area_resref, **0x7e7**, gff). Full list order in [GFF-GIT-Struct-IDs.md](GFF-GIT-Struct-IDs.md) § SaveGIT. Decompile in Agentdecompile to see exact bucketing loop and cast order. |
| **CSWSScriptVarTable::SaveVarTable** | (get-call-graph 0x0050ba00) | Writes script var table into GIT root. |
| **CSWVarTable::SaveVarTable** | (get-call-graph 0x0050ba00) | Writes var table into GIT root. |

**Entity list savers (exact order):**

| Order | Function | K1 address | List name | Struct ID |
|-------|----------|------------|-----------|-----------|
| 1 | **SaveCreatures** | **0x00507680** | "Creature List" | 4 |
| 2 | **SaveItems** | **0x00507750** | "List" | 0 |
| 3 | **SaveDoors** | **0x00507810** | "Door List" | 8 |
| 4 | **SaveTriggers** | **0x005078d0** | "TriggerList" | 1 |
| 5 | **SaveEncounters** | **0x00507990** | "Encounter List" | 7 |
| 6 | **SaveWaypoints** | **0x00507a50** | "WaypointList" | 5 |
| 7 | **SaveSounds** | **0x00507b10** | "SoundList" | 6 |
| 8 | **SavePlaceables** | **0x00507bd0** | "Placeable List" | 9 |
| 9 | **SaveStores** | **0x00507ca0** | "StoreList" | 11 |
| 10 | **SaveAreaEffects** | **0x00507d60** | "AreaEffectList" | 13 |

**After entity lists:**

| Function | K1 address | Purpose |
|----------|------------|---------|
| **SaveProperties** | **0x00506090** | AddStructToStruct(root, "AreaProperties", 100); CSWSAmbientSound::Save; WriteField Unescapable, RestrictMode, StealthXPMax, StealthXPCurrent, StealthXPLoss, StealthXPEnabled, TransPending, TransPendNextID, TransPendCurrID, SunFogColor. |
| **SaveMaps** | **0x005061d0** | If area has map: AddStructToStruct(root, "AreaMap", 0x65); WriteFieldINT AreaMapResX, AreaMapResY; WriteFieldDWORD AreaMapDataSize; WriteFieldVOID AreaMapData. |
| **SavePlaceableCameras** | **0x005062a0** | AddList "CameraList"; for each camera AddListElement(..., 14); WriteFieldINT CameraID; WriteFieldVector Position; WriteFieldQuaternion Orientation; WriteFieldFLOAT Pitch, Height, FieldOfView, MicRange. |

### 2.5 Save path — callees to expand in Agentdecompile (per-entity and shared)

These are called from the Save* functions above but not fully inlined in the main doc. Use **get-call-graph** from the parent to get the **exact K1 address** in your binary, then **get-functions identifier=<addr> view=decompile limit=150** (or higher) for each.

| Callee | Called from | Get address via | Notes |
|--------|-------------|-----------------|--------|
| **CSWSItem::SaveItem** | SaveItems, SerializeCreature_K2 (Equip_ItemList, ItemList) | get-call-graph **0x00507750**, **0x00500610** | Item resref and state. Vendor: SaveItem @ **0x0055ccd0** (GFF-UTI). |
| **CSWSDoor::SaveDoor** | SaveDoors | get-call-graph **0x00507810** | Door-specific: resref, linked module/area, trap state, etc. |
| **CSWSObject::SaveObjectState** | SaveItems, SaveDoors, SaveTriggers, SaveEncounters, SaveWaypoints, SaveSounds, SavePlaceables, SaveStores, SaveAreaEffects, SerializeCreature_K2 | get-call-graph any of **0x00507750**, **0x00507810**, … | Shared: position, orientation, and other object state. |
| **CSWSTrigger::SaveTrigger** | SaveTriggers | get-call-graph **0x005078d0** | Trigger-specific fields. |
| **CSWSEncounter::SaveEncounter** | SaveEncounters | get-call-graph **0x00507990** | Encounter-specific. |
| **CSWSWaypoint::SaveWaypoint** | SaveWaypoints | get-call-graph **0x00507a50** | Waypoint-specific. |
| **CSWSSoundObject::Save** | SaveSounds | get-call-graph **0x00507b10** | Vendor: Save @ **0x005c86d0** (GFF-UTS). |
| **CSWSPlaceable::SavePlaceable** | SavePlaceables | get-call-graph **0x00507bd0** | Placeable-specific. |
| **CSWSStore::SaveStore** | SaveStores | get-call-graph **0x00507ca0** | Store-specific. |
| **CSWSAreaOfEffectObject::SaveEffect** | SaveAreaEffects | get-call-graph **0x00507d60** | Area-effect-specific. |
| **CSWSCreature::SerializeCreature_K2** | SaveCreatures, SaveLimboCreatures, SavePlayers | **0x00500610** | Full creature: SaveStats, DetectMode, StealthMode, scripts, Equip_ItemList, ItemList, PerceptionList, CombatRoundData, AreaId, position/orientation, FollowInfo, SaveObjectState. |
| **CSWSCreatureStats::SaveStats** | SerializeCreature_K2 | get-call-graph **0x00500610** | Creature stats GFF. |
| **CSWSCombatRound::SaveCombatRound** | SerializeCreature_K2 | get-call-graph **0x00500610** | Combat round state. |
| **CSWSCreaturePartyFollowInfo::Save** | SerializeCreature_K2 | get-call-graph **0x00500610** | Follow info struct. |
| **CSWSObject::SaveListenData** | SerializeCreature_K2 | get-call-graph **0x00500610** | Listen data. |
| **CSWSAmbientSound::Save** | SaveProperties | get-call-graph **0x00506090** | Ambient sound state in AreaProperties child. |

### 2.6 Save serialization — ordered step list (copy-paste checklist)

1. **StoreCurrentModule** (0x004b2e70): GetModule, IncludeModuleInSave; build path; **SaveModuleStart**; **SaveModuleInProgress**; **SaveModuleFinish**.
2. **SaveModuleStart** (0x004c8960): DeleteFile existing; create ERF; Create "MOD V1.0"; WriteHeader, WriteStringTable; create IFO GFF "IFO "/"V2.0"; **SerializeIfoGameTime**; **SaveModuleFAC**.
3. **SerializeIfoGameTime** (0x004c7050): Write all IFO root fields (Mod_*, game time, lists, **SaveVarTable** x2, SaveEventQueue, **SaveLimboCreatures**).
4. **SaveModuleInProgress** (0x004c3b10): GetAreaByGameObjectID(area_id); **CSWSArea::SaveGIT**(area, erf, ..., party_list).
5. **SaveGIT** (0x0050ba00): Bucket objects into 10 lists + party; CreateGFF "GIT "/"V2.0"; **SaveVarTable** (script), **SaveVarTable** (var); WriteFieldBYTE CurrentWeather, WeatherStarted, TransPending, TransPendNextID, TransPendCurrID; **SaveCreatures** … **SaveAreaEffects** (10 calls); **SaveProperties**; **SaveMaps**; **SavePlaceableCameras**; WriteResource(erf, area_resref, 0x7e7, gff).
6. **SaveModuleFinish** (0x004ca680): **SaveStatic** (optional); **SaveModuleIFOFinish** → **SavePlayers**; WriteResource(erf, "Module", 0x7de, IFO); CERFFile::Finish.

---

## 3. Exhaustive load serialization — complete logic

Every step required to **load the game** in the same order and with the same data as the engine.

### 3.1 Module-level load

| Step | Function / action | K1 address | Agentdecompile / notes |
|------|-------------------|------------|--------------|
| 1 | **Entry:** `CSWSModule::LoadModuleInProgress` | **0x004c5720** | Create area (with saved area_id if is_save_game); **LoadArea**(area, is_save_game); on success update progress. Full decomp in [GFF-GIT-Full-Save-Load-Code.md](GFF-GIT-Full-Save-Load-Code.md) § LoadModuleInProgress. |
| 2 | **CSWSArea::LoadArea** | **0x0050e190** | Demand ARE resource; GetTopLevelStruct (ARE root); **LoadAreaHeader**; **LoadRoomInfo**; **LoadGIT**(this, param_2); **LoadPathPoints**; Release; AddObjectToLookupTable. Callees LoadAreaHeader, LoadRoomInfo, LoadPathPoints: get addresses via **get-call-graph functionIdentifier=0x0050e190**. |
| 3 | **CSWSArea::LoadGIT** | **0x0050dd80** | Exists(GIT, area_resref); CResGFF(..., GIT, "GIT ", area_resref); GetTopLevelStruct; if param_1: **LoadVarTable** x2, ReadFieldBYTE CurrentWeather, WeatherStarted (and clear if area flags&1); ReadFieldBYTE UseTemplates (default 0); **LoadCreatures** … **LoadAreaEffects** (10); **LoadProperties**; **LoadMaps**; **LoadPlaceableCameras**; release GFF. |

### 3.2 LoadGIT callees — entity list loaders (exact order)

| Order | Function | K1 address | List name | Struct ID check |
|-------|----------|------------|-----------|------------------|
| 1 | **LoadCreatures** | **0x00504a70** | "Creature List" | 4 |
| 2 | **LoadItems** | **0x00504de0** | "List" | 0 |
| 3 | **LoadDoors** | **0x0050a0e0** | "Door List" | 8 |
| 4 | **LoadTriggers** | **0x0050a350** | "TriggerList" | 1 |
| 5 | **LoadEncounters** | **0x00505060** | "Encounter List" | 7 |
| 6 | **LoadWaypoints** | **0x00505360** | "WaypointList" | 5 |
| 7 | **LoadSounds** | **0x00505560** | "SoundList" | 6 |
| 8 | **LoadPlaceables** | **0x0050a7b0** | "Placeable List" | 9 |
| 9 | **LoadStores** | **0x005057a0** | "StoreList" | 11 |
| 10 | **LoadAreaEffects** | **0x00505af0** | "AreaEffectList" | 13 |

Then: **LoadProperties** (0x00507490), **LoadMaps** (0x00505da0), **LoadPlaceableCameras** (0x00505eb0).

### 3.3 Load path — callees to expand in Agentdecompile

| Callee | Called from | Get address via | Notes |
|--------|-------------|-----------------|--------|
| **CSWSScriptVarTable::LoadVarTable** | LoadGIT (when param_1 != 0) | get-call-graph **0x0050dd80** | Script var table from GIT root. |
| **CSWVarTable::LoadVarTable** | LoadGIT (when param_1 != 0) | get-call-graph **0x0050dd80** | Var table from GIT root. |
| **LoadCreature** / **LoadFromTemplate** | LoadCreatures | get-call-graph **0x00504a70** | Per-creature load; UseTemplates selects path. |
| **CSWSCreatureStats::ReadStatsFromGff** / **LoadCreatureData** | LoadCreature path | Vendor: **ReadStatsFromGff** @ **0x005afce0**, **LoadCreatureData** @ **0x00560e60** | Creature stats from GFF. |
| **LoadItem** (from GIT list element) | LoadItems | get-call-graph **0x00504de0** | Item state from list element. |
| **CSWSItem::LoadDataFromGff** | LoadItem / engine | Vendor: **0x0055fcd0** | Item fields (GFF-UTI). |
| **CSWSDoor::LoadDoorExternal** | LoadDoors | get-call-graph **0x0050a0e0** | Door-specific load from GIT element. |
| **CSWSDoor::LoadDoor** (template) | LoadFromTemplate path | Vendor: **0x0058a1f0** (GFF-UTD) | Door template fields. |
| **CSWSObject::LoadObjectState** | LoadDoors, LoadWaypoints, … (when saved-game flag) | get-call-graph **0x0050a0e0**, **0x00505360**, … | Shared object state. |
| **LoadTrigger** | LoadTriggers | get-call-graph **0x0050a350** | Trigger-specific. |
| **LoadEncounter** | LoadEncounters | get-call-graph **0x00505060** | Encounter-specific. Vendor: **CSWSEncounter::ReadEncounterFromGff** K1 **0x00592430**. |
| **CSWSWaypoint::LoadWaypoint** | LoadWaypoints | Vendor: **0x005c7f30** (GFF-UTW) | Waypoint fields. |
| **CSWSSoundObject::Load** | LoadSounds | Vendor: **0x005c9040** (GFF-UTS) | Sound object fields. |
| **CSWSPlaceable::LoadPlaceable** | LoadPlaceables | Vendor: **0x00585670** (GFF-UTP) | Placeable fields. |
| **CSWSStore::LoadStore** / **LoadFromTemplate** | LoadStores | Vendor: **LoadStore** @ **0x005c7180**, **LoadFromTemplate** @ **0x005c7760** (GFF-UTM) | Store load. |
| **LoadAreaEffect** | LoadAreaEffects | get-call-graph **0x00505af0** | Area-effect-specific. |
| **CSWSAmbientSound::Load** | LoadProperties | get-call-graph **0x00507490** | Ambient sound from AreaProperties child. |
| **CSWSAreaMap::LoadSavedAreaMapData** | LoadMaps | get-call-graph **0x00505da0** | Apply map blob (DWORD count = size/4). |

### 3.4 Load serialization — ordered step list (copy-paste checklist)

1. **LoadModuleInProgress** (0x004c5720): Create area (area_id from module if is_save_game); **LoadArea**(area, is_save_game).
2. **LoadArea** (0x0050e190): Demand ARE; GetTopLevelStruct(ARE); LoadAreaHeader; LoadRoomInfo; **LoadGIT**(this, param_2); LoadPathPoints; Release; AddObjectToLookupTable.
3. **LoadGIT** (0x0050dd80): Exists(GIT, area_resref); open GFF "GIT "; GetTopLevelStruct; if param_1: **LoadVarTable** x2, ReadFieldBYTE CurrentWeather, WeatherStarted (clear if flags&1); ReadFieldBYTE UseTemplates (default 0); **LoadCreatures** … **LoadAreaEffects** (10); **LoadProperties**; **LoadMaps**; **LoadPlaceableCameras**; release GFF.

**Per-entity loader pattern:** GetList(root, "<ListName>"); count = GetListCount(list); for (i = 0; i < count; i++) { GetListElement(list, i); if (GetElementType(elem) == expected_struct_id) { read ObjectId, type-specific fields, position/orientation; if (saved_game) LoadObjectState; create object; AddToArea; } }

---

## 4. GFF field reference (save/load)

### 4.1 GIT root (before lists)

| Field | Type | Save | Load |
|-------|------|------|------|
| (script var table) | (SaveVarTable) | SaveVarTable (script) | LoadVarTable (script) when param_1 |
| (var table) | (SaveVarTable) | SaveVarTable (var) | LoadVarTable (var) when param_1 |
| **CurrentWeather** | BYTE | ✓ | ✓ (if param_1) |
| **WeatherStarted** | BYTE | ✓ | ✓ (if param_1) |
| **TransPending** | BYTE | ✓ | — |
| **TransPendNextID** | BYTE | ✓ | — |
| **TransPendCurrID** | BYTE | ✓ | — |
| **UseTemplates** | BYTE | — | ✓ (default 0) |

### 4.2 AreaProperties child (struct ID 100)

| Field | Type |
|-------|------|
| (ambient sound) | CSWSAmbientSound::Save/Load |
| **Unescapable** | BYTE |
| **RestrictMode** | BYTE |
| **StealthXPMax** | DWORD |
| **StealthXPCurrent** | DWORD |
| **StealthXPLoss** | DWORD |
| **StealthXPEnabled** | BYTE |
| **TransPending** | BYTE |
| **TransPendNextID** | BYTE |
| **TransPendCurrID** | BYTE |
| **SunFogColor** | DWORD |

### 4.3 AreaMap child (struct ID 0x65)

| Field | Type |
|-------|------|
| **AreaMapResX** | INT |
| **AreaMapResY** | INT |
| **AreaMapDataSize** | DWORD (bytes) |
| **AreaMapData** | VOID (blob) |

### 4.4 Camera list element (struct ID 14)

| Field | Type |
|-------|------|
| **CameraID** | INT |
| **Position** | Vector (X,Y,Z) |
| **Orientation** | Quaternion |
| **Pitch** | FLOAT |
| **Height** | FLOAT |
| **FieldOfView** | FLOAT |
| **MicRange** | FLOAT |

### 4.5 Entity list elements — common and type-specific

- **ObjectId** (DWORD) — written by every Save*; read by every Load*.
- **Position:** Doors use **X**, **Y**, **Z**; Creatures/Waypoints use **XPosition**, **YPosition**, **ZPosition**.
- **Orientation:** Doors use **Bearing** (FLOAT); Creatures use **XOrientation**, **YOrientation**, **ZOrientation**.
- Type-specific fields: see Agentdecompile decomp of the corresponding Save* / Load* (e.g. SaveDoor/LoadDoorExternal, SaveWaypoint/LoadWaypoint).

---

## 5. Agentdecompile commands — quick reference for exhaustive decomp

Run these in Agentdecompile (Ghidra MCP) with binary **k1_win_gog_swkotor.exe** to get full decompilation for every listed function. Use **get-call-graph** first if you need callee addresses.

**Save path (top → bottom) — copy-paste all to get full save decompilation:**
```
get-functions identifier=0x004b2e70 view=decompile limit=80
get-functions identifier=0x004c8960 view=decompile limit=120
get-functions identifier=0x004c7050 view=decompile limit=220
get-functions identifier=0x004c3960 view=decompile limit=80
get-functions identifier=0x004c5bb0 view=decompile limit=60
get-functions identifier=0x004c3b10 view=decompile limit=60
get-functions identifier=0x0050ba00 view=decompile limit=320
get-functions identifier=0x00507680 view=decompile limit=80
get-functions identifier=0x00507750 view=decompile limit=80
get-functions identifier=0x00507810 view=decompile limit=80
get-functions identifier=0x005078d0 view=decompile limit=80
get-functions identifier=0x00507990 view=decompile limit=80
get-functions identifier=0x00507a50 view=decompile limit=80
get-functions identifier=0x00507b10 view=decompile limit=80
get-functions identifier=0x00507bd0 view=decompile limit=80
get-functions identifier=0x00507ca0 view=decompile limit=80
get-functions identifier=0x00507d60 view=decompile limit=80
get-functions identifier=0x00506090 view=decompile limit=60
get-functions identifier=0x005061d0 view=decompile limit=60
get-functions identifier=0x005062a0 view=decompile limit=60
get-functions identifier=0x00500610 view=decompile limit=250
get-functions identifier=0x004ca680 view=decompile limit=100
get-functions identifier=0x004c8b90 view=decompile limit=60
get-functions identifier=0x004c7870 view=decompile limit=250
get-functions identifier=0x004c5980 view=decompile limit=80
```

**Load path — copy-paste all to get full load decompilation:**
```
get-functions identifier=0x004c5720 view=decompile limit=80
get-functions identifier=0x0050e190 view=decompile limit=60
get-functions identifier=0x0050dd80 view=decompile limit=80
get-functions identifier=0x00504a70 view=decompile limit=150
get-functions identifier=0x00504de0 view=decompile limit=150
get-functions identifier=0x0050a0e0 view=decompile limit=150
get-functions identifier=0x0050a350 view=decompile limit=150
get-functions identifier=0x00505060 view=decompile limit=150
get-functions identifier=0x00505360 view=decompile limit=150
get-functions identifier=0x00505560 view=decompile limit=150
get-functions identifier=0x0050a7b0 view=decompile limit=150
get-functions identifier=0x005057a0 view=decompile limit=150
get-functions identifier=0x00505af0 view=decompile limit=150
get-functions identifier=0x00507490 view=decompile limit=80
get-functions identifier=0x00505da0 view=decompile limit=80
get-functions identifier=0x00505eb0 view=decompile limit=80
```

**Callees (expand as needed):**
```
get-call-graph functionIdentifier=0x0050ba00   # SaveGIT callees
get-call-graph functionIdentifier=0x00500610   # SerializeCreature_K2 callees
get-call-graph functionIdentifier=0x0050dd80  # LoadGIT callees
get-call-graph functionIdentifier=0x00504a70  # LoadCreatures callees
```

This document, together with **GFF-GIT-Full-Save-Load-Code.md** (for already-inlined decompilation) and **GFF-GIT-Struct-IDs.md** (for struct IDs and order), provides **exhaustive and complete** information to implement full save and load with **Agentdecompile as the primary source** for any missing or deeper logic.
