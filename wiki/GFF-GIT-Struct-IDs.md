# KotOR GIT Module Save/Load: Complete GFF Struct ID Reference

**Target Binary:** `/k1_win_gog_swkotor.exe` (KotOR 1, GoG)  
**Cross-Validated Against:** `/k2_win_gog_legacypc_swkotor2.exe` (KotOR 2, GoG Legacy)  
**Analysis Platform:** Reva (Ghidra MCP), Project: `C:\Users\boden\AndastraGhidraProject.gpr`

**For exhaustive save-the-game and load-the-game logic** (every step needed to implement full serialization), see **§ Exhaustive Save Serialization (Complete Logic)** and **§ Exhaustive Load Serialization (Complete Logic)** in this document. **For a single reference that puts Reva first** — with step-by-step checklists, every function address, Reva `get-functions` / `get-call-graph` commands, and callees-to-expand tables — see **[GFF-Save-Load-Exhaustive.md](GFF-Save-Load-Exhaustive.md)**. **For full 1:1 C/C++ decompiled code** (no omissions) of the save and load paths, see **[GFF-GIT-Full-Save-Load-Code.md](GFF-GIT-Full-Save-Load-Code.md)**.

Key addresses, struct IDs, list names, CreateGFFFile/WriteResource usage, and save/load order were re-verified via Reva `get-functions` (decompile) on the listed binaries. Spot-checks confirmed: SaveDoors `"Door List"` + AddListElement(..., **8**), SaveTriggers `"TriggerList"` + AddListElement(..., **1**), SaveEncounters `"Encounter List"` + **7**, SaveWaypoints `"WaypointList"` + **5**, SaveStores `"StoreList"` + **0xb**; LoadDoors GetList `"Door List"` + GetElementType == **8**; SaveGIT `uVar8 = 0x7e7`, CreateGFFFile `"GIT "` / `"V2.0"`; **AddListElement** at **0x004124e0** (params: gff, struct, list, structId).

---

## Background: GFF Struct ID Mechanics

A GFF file is structured as a header pointing to arrays of structs, fields, labels, and data. GIT files use file type `"GIT "` and version **"V2.0"** (written by `CreateGFFFile` in SaveGIT; see exact order of operations). The relevant in-memory C types (from `manage-structures info` on the project):

**`GFFHeaderInfo`** (56 bytes, `/KotOR Types`):
```
struct GFFHeaderInfo {
    char[4] file_type;        // e.g. "GIT "
    char[4] file_version;     // "V2.0" for GIT
    ulong struct_offset;
    ulong struct_count;
    ulong field_offset;
    ulong field_count;
    ulong label_offset;
    ulong label_count;
    ulong field_data_offset;
    ulong field_data_count;
    ulong field_indices_offset;
    ulong field_indices_count;
    ulong list_indices_offset;
    ulong list_indices_count;
};
```

**`GFFStructData`** / **`CResGFFStruct`** (12 bytes each, `/KotOR Types`):
```
struct GFFStructData {
    ulong id;           // ← THIS IS THE STRUCT ID
    undefined[4] _;     // field data offset or index
    ulong field_count;
};
```
The `id` field in every GFF struct entry in the binary file is the "Struct ID" written by `CResGFF::AddListElement(..., structId)` and validated on read by `CResGFF::GetElementType(...)`.

**`CResGFFField`** (12 bytes, `/KotOR Types/Resources`):
```
struct CResGFFField {
    GFFFieldTypes field_type;
    ulong label_index;
    undefined[4] _;   // data or offset
};
```

**`CResGFF`** (160 bytes, `/KotOR Types/Resources`) — the runtime GFF object:
- `header` (offset 64): `GFFHeaderInfo *`
- `structs` (offset 68): `GFFStructData *`
- `fields` (offset 76): `GFFFieldData *`
- `labels` (offset 84): `char[16] *`
- `field_data` (offset 92): `void *`
- `field_indices_data` (offset 100): `ulong *`
- `list_indices_data` (offset 112): `ulong *`
- `field_type` (offset 141): `char[4]` — the 4-char GFF type tag

The two key functions governing Struct IDs in GIT:
- **`CResGFF::AddListElement`** at `0x004124e0` — takes `(pGFF, pOutStruct, pListStruct, structId)`. Writes a new list element with the given `structId` into the binary GFF struct array.
- **`CResGFF::GetElementType`** at `0x004111c0` — called during load to read back the `structId` from a GFF struct entry. Load functions check the returned value and skip/reject structs with the wrong ID.

---

## How GIT Save/Load Works (Plain Language)

This section explains the flow of saving and loading a GIT in both games, what struct IDs are for, and how the engine uses list names and element types. All of this is backed by the decompiled code referenced elsewhere in the document.

### What the struct ID actually is

In the binary GFF file, every struct is represented by a **struct entry** in the GFF struct array. Each entry has an **id** field (the “struct ID”). When the engine **saves** a list element, it calls `CResGFF::AddListElement(..., structId)`. Inside that function, the engine calls `AddStruct(this, structId)`, which allocates a new struct entry and stores `structId` in that entry’s **id** field. So the struct ID is literally the 32-bit value written into the GFF file for that element.

When the engine **loads**, it does not trust that the list only contains the right kind of object. For each list element it calls `CResGFF::GetElementType(pGFF, &elementStruct)`. That function just reads `this->structs[elementStruct->index].id` and returns it (or -1 if the index is invalid). So the struct ID is read back from the file and used to decide whether to treat the element as a creature, door, item, and so on.

### When the engine loads and saves a GIT

The game **loads** a GIT when an area is loaded: **LoadGIT** is called from **LoadArea** (with a parameter that indicates whether this is part of a saved-game load). So every time the player enters an area, the engine opens that area’s GIT (by the area’s resref), reads the root fields and the 11 lists, and spawns creatures, items, doors, triggers, and so on. The **save** path runs when the player saves the game: **SaveGIT** is called from **SaveModuleInProgress**, which writes each area’s GIT into the save ERF (resource type 0x7e7, name = area resref). So the GIT is the per-area snapshot of dynamic state (objects, weather, script vars, map reveal, cameras) that the engine persists when saving and restores when loading.

### How saving a GIT works (K1 flow; K2 is the same idea)

1. **Create an empty GFF** — The area’s `SaveGIT` creates a new `CResGFF`, then calls `CreateGFFFile` with type `"GIT "` and version `"V2.0"`. That gives a root struct; all lists are fields of this root.

2. **Bucket every object in the area by type** — The code walks the area’s `game_objects` array. For each object it gets the game object and then uses the **vtable** to see what kind of thing it is: `AsSWSCreature`, then `AsSWSItem`, then `AsSWSDoor`, then `AsSWSTrigger`, then `AsSWSEncounter`, then `AsSWSWaypoint`, then `AsSWSSoundObject`, then `AsSWSPlaceable`, then `AsSWSStore`, then `AsSWSAreaOfEffectObjec`. The first non-null cast wins, and the object’s ID is pushed into the corresponding local list (e.g. creatures go into one list, doors into another). Triggers are handled inline (added to `local_a8`). So by the end of this loop, the engine has separate lists of object IDs for creatures, items, doors, triggers, encounters, waypoints, sounds, placeables, stores, and area effects.

3. **Write each list into the GFF** — For each of those categories, the engine calls the corresponding `Save*` function (e.g. `SaveCreatures`, `SaveDoors`). Each save function:
   - Calls **`CResGFF::AddList`** with the **list name** (e.g. `"Creature List"`, `"Door List"`) and the root struct. That creates a new **List** field on the root with that label and an empty list of struct indices.
   - For each object in that category, calls **`CResGFF::AddListElement`** with the **struct ID** for that list (e.g. 4 for creatures, 8 for doors). That appends a new struct to the GFF with that **id** and adds its index to the list.
   - Then writes all the fields for that object (position, orientation, resref, etc.) with `WriteField*` into the struct just added.

So: **list name** selects which top-level list in the GIT the object goes into; **struct ID** is the type tag stored in each element so the loader can check it later. The exact fields written per type differ: for example, **SaveCreatures** writes **ObjectId** then calls **SerializeCreature_K2** (creature-specific data); **SaveItems** writes **ObjectId**, then **SaveItem** (item resref and item state), then **SaveObjectState** (position, orientation, and other shared object state). So each list element typically has at least an object ID plus type-specific and shared state written by the corresponding serialize routine.

4. **After the entity lists** — K1 then calls `SaveProperties`, `SaveMaps`, and `SavePlaceableCameras` on the same root struct (writing area properties, fog-of-war/map data, and the placeable camera list). K2 does the same conceptually but with different function names (see below).

### How loading a GIT works (same in both games)

1. **Open the GFF and get the root struct** — The loader already has the GIT GFF in memory and the root struct (e.g. from `GetTopLevelStruct`).

2. **For each list the game cares about** — The loader calls the corresponding `Load*` function (e.g. `LoadCreatures`, `LoadDoors`). Each load function:
   - Calls **`CResGFF::GetList`** with the **list name** and the root struct. That finds the field with that label and returns a handle to the list (or fails if the list is missing).
   - Gets **`GetListCount`** to know how many elements are in the list.
   - Loops from index 0 to count-1. For each index:
     - Calls **`CResGFF::GetListElement`** to get the struct at that index. That fills an output struct with the **index** of the element in the GFF struct array.
     - Calls **`CResGFF::GetElementType`** on that struct to read the **id** from the GFF.
     - If the returned id **equals** the expected value for that loader (e.g. 4 for creatures, 8 for doors), the code reads all the fields with `ReadField*` and spawns the object (creature, door, etc.) into the area.
     - If the id **does not** match, the element is **skipped** — no error is raised, no object is created. This way corrupted or hand-edited GITs with a wrong id in a list slot do not crash the game; the engine just ignores that element.

So the struct ID is a **safety check** on load: it ensures the loader only interprets data that was written by the matching save path. If you put a struct with id 8 in the Creature List, the creature loader will skip it because it expects id 4.

### How AddList / GetList and AddListElement / GetListElement relate to the GFF layout

- **AddList(gff, listHandle, rootStruct, "Creature List")** — Creates a new **field** on the root struct with label `"Creature List"` and type **List**. The list starts empty (zero elements). The `listHandle` is filled in so the next calls can use it.
- **AddListElement(gff, elementStruct, listHandle, 4)** — Finds the list field (by the label stored in `listHandle`), grows the list’s index array, calls **AddStruct(gff, 4)** to allocate a new struct with **id = 4**, appends that struct’s index to the list, and returns. The `elementStruct` is updated with the new struct’s index so the caller can then call **WriteField*** on it.

On load:

- **GetList(gff, listHandle, rootStruct, "Creature List")** — Finds the root’s field with label `"Creature List"` and type List, and fills `listHandle` with the struct index and label so the engine can iterate the list.
- **GetListElement(gff, elementStruct, listHandle, index)** — Reads the list’s index array at `index` to get a struct index, and fills `elementStruct` with that index.
- **GetElementType(gff, elementStruct)** — Returns `gff->structs[elementStruct->index].id`, i.e. the struct ID stored when the element was saved.

So: the **list** is a field on the root, and each **element** is a struct whose index is stored in that list. The **struct ID** lives in the struct entry itself and is what the load functions compare against.

### What happens when the loader sees a wrong struct ID

If a list element has a struct ID that does not match what the loader expects (e.g. the creature loader sees 7 instead of 4), the loader does **not** create an object for that element and does **not** report an error. It simply advances to the next element. So one bad or edited element does not break the rest of the list; the engine is tolerant of mismatched or unknown IDs in a list.

### K2 vs K1: same schema, different function names

K2 uses the **same** list names and **same** struct IDs for all 11 GIT lists (Creature List through CameraList). The only difference is that in K2 many of the save/load routines are unnamed in the binary (FUN_004e29a0, FUN_004e0ee0, etc.). For example, after the 10 entity-list serializers, K2 calls `SaveAreaProperties`, then **FUN_004e1320** (writes AreaMap, AreaMapResX, AreaMapResY, AreaMapDataSize, AreaMapData — i.e. **SaveMaps**), then **FUN_004e13f0** (writes the **CameraList** with AddList `"CameraList"` and AddListElement with struct ID **14** — i.e. **SavePlaceableCameras**). On load, after `LoadAreaProperties`, K2 calls **FUN_004e0ee0** (reads AreaMap fields — **LoadMaps**) and **FUN_004e0ff0** (gets list `"CameraList"`, iterates elements, reads CameraID, Position, Orientation, etc. — **LoadPlaceableCameras**). So both games save and load the same GIT content, including CameraList; K2 just uses different (often auto-named) function addresses.

### Root-level GIT fields: what lives on the root struct (before and after the lists)

The GIT root struct is not only the parent of the 11 lists; it also holds **direct fields** that the engine reads or writes before or after the entity lists. From the decompiled **K1 SaveGIT** and **LoadGIT**:

**Written on save, before any list:**
- **Script / variable data** — `CSWSScriptVarTable::SaveVarTable` and `CSWVarTable::SaveVarTable` write the area’s script variable tables into the root struct.
- **Weather and transition state** — The engine writes these as BYTEs on the root: `CurrentWeather`, `WeatherStarted`, `TransPending`, `TransPendNextID`, `TransPendCurrID`. So the current weather, whether weather has started, and transition-pending state are stored directly on the root.

**Written on save, after the entity lists:**
- **SaveProperties** (K1 @ `0x00506090`) — Calls `AddStructToStruct(gff, &outStruct, rootStruct, "AreaProperties", 100)`, which creates a **child struct** of the root with label **"AreaProperties"** and struct ID **100** and returns a handle to that child. All following writes go **into that child struct**: `CSWSAmbientSound::Save` (ambient sound state), then **Unescapable**, **RestrictMode**, **StealthXPMax**, **StealthXPCurrent**, **StealthXPLoss**, **StealthXPEnabled**, **TransPending**, **TransPendNextID**, **TransPendCurrID**, **SunFogColor** (BYTE or DWORD). So area behaviour and ambient sound live entirely **inside the AreaProperties child**, not on the root. **LoadProperties** (K1) reads from this same child via `GetStructFromStruct(..., "AreaProperties")` then the same field names.
- **SaveMaps** (K1 @ `0x005061d0`) — Only runs if the module has an area map with data. It adds a **child struct** to the root with label **"AreaMap"** and struct ID **0x65** (101), then writes **AreaMapResX** (INT), **AreaMapResY** (INT), **AreaMapDataSize** (DWORD, size in bytes), **AreaMapData** (VOID, raw bytes) into that child. Fog-of-war / revealed map data lives only in the AreaMap child.
- **SavePlaceableCameras** — Writes the CameraList.

**Read on load:**
- When **param_1 != 0** (e.g. loading a saved game), the loader first runs `LoadVarTable` for the script var tables and reads **CurrentWeather** and **WeatherStarted**. If the area has a certain flag (e.g. `(this->sw_area).flags & 1`), it forces weather off. So the same root fields used on save are read back when restoring a game.
- **UseTemplates** — The loader always reads a BYTE field **UseTemplates** from the **root** (`CResGFF::ReadFieldBYTE(..., "UseTemplates", ..., 0)`). The last argument **0** is the **default value** used when the field is missing. So if a GIT has no UseTemplates field (e.g. a module GIT from the game data), the engine uses **0** (load from serialized state). That value is then passed as the last parameter to every entity loader. **K1 SaveGIT never writes UseTemplates**—the pre-list block only writes CurrentWeather, WeatherStarted, TransPending, TransPendNextID, TransPendCurrID. So when the game saves a GIT (saved game), the UseTemplates field is absent and on load the default 0 is used (correct for saved games). For **module GITs** (static area layout), tool authors must **write UseTemplates = 1** on the root if they want the engine to spawn entities from template resrefs instead of full state.
- Then the 11 list loaders run, then **LoadProperties**, **LoadMaps**, **LoadPlaceableCameras**.

So the root struct layout is: (1) script/var data and weather/transition BYTEs, (2) the 11 list fields, (3) an optional **AreaProperties** child (struct ID 100) holding area options and ambient sound, (4) an optional **AreaMap** child (struct ID 0x65) when the area has map data, (5) CameraList. **UseTemplates** is read from the root (default 0); write it explicitly for module GITs that use template-based loading.

### UseTemplates: what it controls when loading entities

**UseTemplates** is a single byte read from the GIT root by LoadGIT before calling any entity loader (in K1 it is **never written** by SaveGIT; the loader uses **default 0** when the field is missing). It is passed unchanged to every `Load*` (creatures, items, doors, triggers, encounters, waypoints, sounds, placeables, stores, area effects). In **LoadCreatures** (K1), when that parameter is **0**, the code reads **ObjectId** from the GFF and then **LoadCreature** (full saved state). When it is **non-zero**, the code creates a creature with a temporary ID and reads **TemplateResRef** from the GFF, then calls **LoadFromTemplate** to spawn from the blueprint instead of from the saved bytes. So in practice: **UseTemplates = 0** means “load from the serialized state in the GIT (saved game style)”; **UseTemplates ≠ 0** means “ignore full state and spawn from template resref (module/area template style)”. Tool authors writing GITs for a module (static layout) must write **UseTemplates = 1** on the root so the engine spawns from blueprints; when writing a saved-game GIT, omit it or use 0 and each list element carries full state.

### Exact order of operations (K1)

So that implementers can match the engine’s behaviour exactly, the order is:

**SaveGIT (K1):**
1. Create GFF, CreateGFFFile with type `"GIT "`, root struct.
2. **Before any list:** SaveVarTable (script), SaveVarTable (var table), WriteFieldBYTE CurrentWeather, WeatherStarted, TransPending, TransPendNextID, TransPendCurrID.
3. **Entity lists (in this order):** SaveCreatures, SaveItems, SaveDoors, SaveTriggers, SaveEncounters, SaveWaypoints, SaveSounds, SavePlaceables, SaveStores, SaveAreaEffects.
4. **After entity lists:** SaveProperties, SaveMaps, SavePlaceableCameras.
5. Write the GFF into the ERF (WriteResource with area resref, type 0x7e7).

**LoadGIT (K1):**
1. Open GIT by area resref (`CResGFF::CResGFF(..., GIT, "GIT ", templateResRef)`), GetTopLevelStruct.
2. If **param_1 != 0:** LoadVarTable (script), LoadVarTable (var table), ReadFieldBYTE CurrentWeather, WeatherStarted; if area flags & 1, clear weather.
3. ReadFieldBYTE **UseTemplates** from root (default **0** if missing).
4. **Entity lists (in this order):** LoadCreatures, LoadItems, LoadDoors, LoadTriggers, LoadEncounters, LoadWaypoints, LoadSounds, LoadPlaceables, LoadStores, LoadAreaEffects (each called with UseTemplates as last argument).
5. **After entity lists:** LoadProperties, LoadMaps, LoadPlaceableCameras.
6. Release GFF and return.

K2 uses the same logical order (script/var, weather, UseTemplates, then entity lists, then LoadAreaProperties, LoadMaps, LoadPlaceableCameras). The **entity list call order** in K2 LoadGIT differs: K2 calls Triggers, Encounters, Waypoints before Doors (see “K2 Post–AreaEffects Save/Load” and the K2 LoadGIT decompilation block). Function names differ (SaveAreaProperties / LoadAreaProperties; FUN_* for Maps and CameraList).

---

## Exhaustive Save Serialization (Complete Logic)

All steps below are taken from Reva decompilation of K1 (`/k1_win_gog_swkotor.exe`). This section gives every step required to implement full save-the-game behaviour. GIT is the per-area snapshot written into the save ERF; the module-level flow creates the ERF and then writes the current area's GIT into it.

### Module-level save entry and sequence

1. **Entry point:** `CServerExoAppInternal::StoreCurrentModule` @ **0x004b2e70**. Called from `StallEventSaveGame` (actual save) and `StartNewModule` (e.g. transition).
2. **Preconditions:** `GetModule(this)` non-null. `IncludeModuleInSave(this, GetModuleResourceName(module))` must return non-zero (otherwise no save runs).
3. **Build save filename:** `local_38 = "GAMEINPROGRESS:"`; `local_40 = module->field16_0x5c` (module name/path). If `Find(&local_40, ':', 0) == -1` then `local_40 = local_38 + moduleResName + local_40`, else take `Right` of `local_40` after ':' and `local_40 = local_38 + that`. So the final path is built from alias list / save directory and the chosen name.
4. **Three-phase save:**
   - **SaveModuleStart(module, &local_38, &local_40)** — see below.
   - **SaveModuleInProgress(module)** — writes the **current area's GIT** into the ERF (see below).
   - **SaveModuleFinish(module, &local_38, &local_40)** — writes IFO finish, optional ARE, finalizes ERF.

### SaveModuleStart (K1 @ 0x004c8960)

- Set `module->is_save_game = 1`.
- **Resolve path:** `CExoAliasList::ResolveFileName(&local_14, param_2, 0xbc1)` (param_2 = filename string). `DeleteFileA(CStr(&local_14))` to remove existing file.
- **Create ERF:** `operator_new(0xd0)` → `CERFFile::CERFFile`; store in **module->field76_0x1e8** (the save ERF handle).
- **Party list for GIT:** `operator_new(0xc)` for a CExoArrayList; store in **module->field79_0x1f4** (passed to SaveGIT as the list of PC object IDs to save in the area).
- **ERF on disk:** `CERFFile::Create(erf, param_2)`, `SetVersion(erf, "MOD V1.0")`, `WriteHeader(erf)`, `WriteStringTable(erf)`, `module->table_count_ = 3`, `SetNumEntries(erf, 3)`.
- **IFO GFF:** New CResGFF → **module->field78_0x1f0**; new CResStruct → **module->field77_0x1ec**. `CreateGFFFile(gff, struct, "IFO ", "V2.0")`. On success: (1) **SerializeIfoGameTime(module, gff, struct)** (K1 @ 0x004c7050) — writes into the IFO root: **Mod_ID** (VOID, 0x20 bytes), **Mod_Creator_ID** (INT), **Mod_Version** (DWORD), **Mod_Name** (CExoLocString), **Mod_Description** (CExoLocString), **Mod_IsSaveGame** (BYTE), **Mod_IsNWMFile** (BYTE), optionally **Mod_NWMResName** (CExoString) when is_nwm_file, **Mod_Hak** (CExoString), plus game time, current area ref, and other module fields (full list: Reva decompile SerializeIfoGameTime, 208 lines); (2) **SaveModuleFAC()** — see next. On failure: destroy ERF, gff, struct and return. **SaveModuleFAC** (K1 @ 0x004c3960) writes faction/repute to a **separate file** (path `"GAMEINPROGRESS:REPUTE"`, type FAC) via `CResGFF::WriteGFFFile` — it does **not** write into the save ERF; it creates a standalone FAC GFF with "FactionList" and "RepList" (CFactionManager::SaveFactions, SaveReputations).

### SaveModuleInProgress (K1 @ 0x004c3b10)

- If **module->field76_0x1e8 == 0** (no ERF), return 0.
- **Current area only:** `this_00 = CServerExoApp::GetAreaByGameObjectID(AppManager->server, module->area_id)`. So only the area the player is currently in is saved.
- **Single GIT write:** `CSWSArea::SaveGIT(this_00, (CERFFile *)module->field76_0x1e8, &local_1c, (CExoArrayList *)module->field79_0x1f4)`. The second parameter (CExoString "tmpgit") is not used for the resource name inside SaveGIT; the resource name is the area's resref (see SaveGIT below). The fourth parameter is the party list (PCs are collected into this and not serialized into Creature List; they are handled elsewhere).

### SaveGIT (K1 @ 0x0050ba00) — complete per-area GIT write

**Signature:** `void CSWSArea::SaveGIT(CSWSArea *this, CERFFile *param_1, CExoString *param_2, CExoArrayList *param_3)`.  
**Parameters:** `param_1` = ERF to write to; `param_2` = unused for resource name (resource name = area resref); `param_3` = list of PC (player character) object IDs — creatures that are PCs are added to this list and not put into the Creature List.

**Steps in order:**

1. **Allocate and clear 10 local CExoArrayLists** for object IDs: creatures (local_90), items (local_9c), doors (doors_list), triggers (local_a8), encounters (local_50), waypoints (local_30), sounds (local_44), placeables (local_5c), stores (local_6c), area effects (local_78). `game_object_array = CServerExoApp::GetObjectArray(AppManager->server)`.
2. **Bucket every object in the area:** Loop `i = 0` to `this->game_object_count - 1`. For each `game_object = GetGameObject(game_object_array, this->game_objects[i])`. If get fails, skip. Otherwise cast in this order (first non-null wins): `AsSWSCreature` → if PC (`creature_stats->is_pc != 0`) add to **param_3** (party), else add to **local_90**; `AsSWSItem` → local_9c; `AsSWSDoor` → doors_list; `AsSWSTrigger` → local_a8; `AsSWSEncounter` → local_50; `AsSWSWaypoint` → local_30; `AsSWSSoundObject` → local_44; `AsSWSPlaceable` → local_5c; `AsSWSStore` → local_6c; `AsSWSAreaOfEffectObjec` → local_78. Unrecognized types are skipped.
3. **Create GFF:** `operator_new(0xa0)` → `CResGFF::CResGFF`; `struct = operator_new(4)` (CResStruct); `CreateGFFFile(gff, struct, "GIT ", "V2.0")` → root struct.
4. **Root fields (before any list):** `CSWSScriptVarTable::SaveVarTable(&this->field43_0x1e4, gff, struct)`; `CSWVarTable::SaveVarTable(&this->script_var_table, gff, struct)`; `WriteFieldBYTE(gff, struct, this->current_weather, "CurrentWeather")`; `WriteFieldBYTE(..., "WeatherStarted")`; `WriteFieldBYTE(..., "TransPending")`; `WriteFieldBYTE(..., this->next_transition_pending_id, "TransPendNextID")`; `WriteFieldBYTE(..., this->trans_pend_curr_id, "TransPendCurrID")`.
5. **Entity lists (exact order):** SaveCreatures(this, gff, struct, &local_90); SaveItems(this, gff, struct, &local_9c); SaveDoors(this, gff, struct, &doors_list); SaveTriggers(this, gff, struct, &local_a8); SaveEncounters(this, gff, struct, &local_50); SaveWaypoints(this, gff, struct, &local_30); SaveSounds(this, gff, struct, &local_44); SavePlaceables(this, gff, struct, &local_5c); SaveStores(this, gff, struct, &local_6c); SaveAreaEffects(this, gff, struct, &local_78).
6. **Post-entity:** SaveProperties(this, gff, struct); SaveMaps(this, gff, struct); SavePlaceableCameras(this, gff, struct).
7. **Write to ERF:** `CResRef::CopyToString(&this->res_helper.resref, &CStack_38)`; `CERFFile::WriteResource(param_1, CStr(&CStack_38), 0x7e7, &gff->resource, 1, (void *)0xffffffff)`. So **resource name = area resref**, **resource type = 0x7e7** (GIT in save ERF), one copy, then release GFF and free struct and all local array lists.

### SaveModuleFinish (K1 @ 0x004ca680)

- If **module->field76_0x1e8 == 0**, return 0.
- If **module->is_nwm_file == 0**: `SaveStatic(module, erf, "ARE ", 0x7dc, 1)` (writes static ARE resources into the save ERF).
- **SaveModuleIFOFinish** (K1 @ 0x004c8b90): `SavePlayers(this, gff, struct, param_1, param_6)` — writes player/party data into the IFO GFF; then `CERFFile::WriteResource(erf, "Module", 0x7de, &gff->resource, 1, (void *)0xffffffff)`. So the IFO is written to the ERF with **resource name "Module"**, **resource type 0x7de**. Then release GFF and free struct.
- `CERFFile::Finish(erf)` — finalizes ERF (directory, etc.).
- Destruct and free ERF, party list (field79_0x1f4), clear field76_0x1e8 and field79_0x1f4. Return 1.

**Summary for implementers:** To save the game you must create an ERF (Create, SetVersion "MOD V1.0", WriteHeader, WriteStringTable, SetNumEntries(3)), write IFO (CreateGFFFile "IFO "/"V2.0", SerializeIfoGameTime, SaveModuleFAC), then for the **current area only** run the full SaveGIT sequence above and write that GIT into the ERF with resource name = area resref and type **0x7e7**, then call SaveStatic (if not NWM), SaveModuleIFOFinish (which adds **SavePlayers** into the IFO then writes the IFO to the ERF as resource name **"Module"**, type **0x7de**), and CERFFile::Finish. **Resources written into the save ERF (K1):** one **GIT** per saved area (name = area resref, type 0x7e7); one **IFO** (name "Module", type 0x7de); optionally **ARE** resources (SaveStatic, type 0x7dc) when not NWM. SaveModuleFAC writes faction/repute to a separate file (e.g. GAMEINPROGRESS:REPUTE), not into the ERF.

---

## Exhaustive Load Serialization (Complete Logic)

All steps below are from Reva decompilation of K1. This section gives every step required to implement full load-the-game behaviour. The GIT is loaded when an area is loaded; the module-level flow creates the area and then LoadArea (and thus LoadGIT) runs for that area, with GIT supplied by the resource manager from the save ERF when loading a saved game.

### Module-level load and area creation

1. **Entry point:** `CSWSModule::LoadModuleInProgress` @ **0x004c5720** is called from the main loop (e.g. `MainLoop` @ 0x004bb142) with a progress parameter. The module has already been prepared (e.g. save ERF or module ERF set as the resource source; **module->is_save_game** and **module->area_id** and **module->area_name** are set).
2. **Create area object:** If `is_save_game == 0`: `CSWSArea::CSWSArea(local_10, &module->area_name, 0, 0x7f000000)`. If `is_save_game != 0`: `CSWSArea::CSWSArea(local_10, &module->area_name, 0, module->area_id)`. So for a saved game the area is created with the saved area ID.
3. **Load the area:** `CSWSArea::LoadArea(this_00, module->is_save_game)`. The second parameter is the "saved game" flag passed through to LoadGIT (param_1). If LoadArea returns 0, load fails (return 4); otherwise update progress and return 0 (success). So **LoadArea(area, is_save_game)** is the single call that loads ARE + GIT for that area.

### LoadArea (K1 @ 0x0050e190)

**Signature:** `undefined4 CSWSArea::LoadArea(CSWSArea *this, int param_2)`. **param_2** = "is saved game" (non-zero when loading from a save).

1. **Demand area resource:** `CRes::Demand(&this->res_helper.gff->resource)`. The area's GFF (ARE) is already associated with the area (e.g. from module's area list or from save metadata). If Demand fails, return 0.
2. **Root struct:** `CResGFF::GetTopLevelStruct(this->res_helper.gff, &local_4)` — this is the **ARE** root, not the GIT.
3. **ARE content:** `LoadAreaHeader(this, &local_4)`; `LoadRoomInfo(this, &local_4)`; **LoadGIT(this, param_2)**; `LoadPathPoints(this, &local_4)`.
4. **Release:** `CRes::Release(&this->res_helper.gff->resource)`.
5. **Bookkeeping:** `CSWSModule::AddObjectToLookupTable(module, &this->tag, this->game_object.id)`; set `this->field57_0x223` from BSP dimensions. Return 1.

So **LoadGIT** is called with the same **param_2** (is_save_game). The GIT is loaded by **area resref** from the resource manager; when loading a saved game the save ERF is in the path, so the GIT with that resref is the one from the save file.

### LoadGIT (K1 @ 0x0050dd80) — complete per-area GIT read

**Signature:** `int CSWSArea::LoadGIT(CSWSArea *this, int param_1)`. **param_1** = non-zero when loading a saved game (restore vars/weather and use full state for entities).

**Steps in order:**

1. **Resolve GIT resource:** `templateResRef = &this->res_helper.resref` (the area's resref). `iVar3 = CExoResMan::Exists(ExoResMan, templateResRef, GIT, 0)`. If Exists returns 0, skip to step 8 (return 0). So the GIT must exist (e.g. in the save ERF or module) under the **area resref**.
2. **Open GFF:** `local_10 = operator_new(0xa0)`; `this_00 = CResGFF::CResGFF(local_10, GIT, "GIT ", templateResRef)`. So the engine loads the resource by type GIT, type tag "GIT ", and resref = area resref. For a saved game this is the GIT written by SaveGIT (resource type 0x7e7, name = area resref).
3. **Validate:** If `this_00->field31_0x94 == 0` (load failed), release and return 0.
4. **Root struct:** `CResGFF::GetTopLevelStruct(this_00, &local_18)`.
5. **Conditional saved-game state (param_1 != 0):** `CSWSScriptVarTable::LoadVarTable(&this->field43_0x1e4, this_00, &local_18)`; `CSWVarTable::LoadVarTable(&this->script_var_table, this_00, &local_18)`; `bVar2 = ReadFieldBYTE(this_00, &local_18, "CurrentWeather", &local_14, 0)` → `this->current_weather`; `bVar2 = ReadFieldBYTE(..., "WeatherStarted", ..., 0)` → `this->weather_started`; if `(this->sw_area).flags & 1` then set `current_weather = 0xff`, `weather_started = 0`.
6. **UseTemplates:** `bVar2 = ReadFieldBYTE(this_00, &local_18, "UseTemplates", &local_14, 0)`; `uVar4 = (uint)bVar2`. Default 0 if field missing.
7. **Entity lists (exact order, each with root and UseTemplates):** LoadCreatures(this, this_00, &local_18, param_1, uVar4); LoadItems(..., param_1, uVar4); LoadDoors(..., param_1, uVar4); LoadTriggers(..., param_1, uVar4); LoadEncounters(..., param_1, uVar4); LoadWaypoints(..., param_1, uVar4); LoadSounds(..., param_1, uVar4); LoadPlaceables(..., param_1, uVar4); LoadStores(..., param_1, uVar4); LoadAreaEffects(..., param_1, uVar4). Then LoadProperties(this, this_00, &local_18); LoadMaps(this, this_00, &local_18); LoadPlaceableCameras(this, this_00, &local_18).
8. **Release GFF:** `(**(code **)(this_00->resource).vtable)(1)`. Return 1. If step 1 or 3 failed, ExceptionList restored and return 0.

**Per-entity loader pattern:** Each Load* calls GetList(gff, &listHandle, root, "<ListName>"). If GetList fails, return. count = GetListCount(listHandle). For index 0..count-1: GetListElement(gff, &elementStruct, listHandle, index); type = GetElementType(gff, &elementStruct); if type == expected struct ID then read fields (ObjectId, type-specific, position/orientation, LoadObjectState if param_1), create object, add to area; else skip element. So **missing or empty lists** are valid (count 0); **wrong struct ID** in an element causes that element to be skipped.

**Summary for implementers:** To load the game you must open the save ERF (or set it as the resource source). The save ERF must contain the **IFO** as resource **"Module"** (type 0x7de) and the current area's **GIT** (resource name = area resref, type 0x7e7). Create the area with the saved area_id and area_name, then call LoadArea(area, 1). LoadArea demands the ARE (from the ERF or module), loads ARE header/rooms, then LoadGIT(area, 1). LoadGIT requests the GIT by **area resref** from the resource manager (which returns the GIT from the save ERF), gets the root struct, restores vars/weather if param_1 != 0, reads UseTemplates (default 0), runs the 10 entity loaders in order with (area, gff, root, param_1, UseTemplates), then LoadProperties, LoadMaps, LoadPlaceableCameras. K2 uses the same logic with different function names and a different entity-list call order (see K2 LoadGIT decompilation block).

---

### LoadProperties and LoadMaps (K1): exact fields read from child structs

**LoadProperties** (K1 @ `0x00507490`) receives the root struct. It calls `GetStructFromStruct(gff, &outStruct, root, "AreaProperties")`. If that fails (no AreaProperties child), it returns 0 and does nothing. If it succeeds, `outStruct` is the AreaProperties child; the function then reads from **that child** (not the root): **Unescapable**, **RestrictMode**, **StealthXPMax**, **StealthXPCurrent**, **StealthXPLoss**, **StealthXPEnabled**, **TransPending**, **TransPendNextID**, **TransPendCurrID**, **SunFogColor** (BYTE or DWORD as in SaveProperties), then **CSWSAmbientSound::Load** for ambient sound state. So the save and load paths match: all of these fields live in the AreaProperties child struct. **K2 LoadAreaProperties** (@ `0x004e26d0`) does the same: GetStructFromStruct `"AreaProperties"`, then the same field names and an ambient-sound load.

**LoadMaps** (K1 @ `0x00505da0`) receives the root struct. It calls `GetStructFromStruct(gff, &outStruct, root, "AreaMap")`. If that fails, it returns 0. Otherwise it reads from the **AreaMap child**: **AreaMapResX** (INT), **AreaMapResY** (INT), **AreaMapDataSize** (DWORD, size in bytes), **AreaMapData** (VOID, raw bytes). It allocates a buffer, reads the blob into it, then calls **CSWSAreaMap::LoadSavedAreaMapData(buffer, size>>2, resX, resY)** so the map data is applied (the count passed is size in bytes shifted right by 2, i.e. DWORD count). The map data is stored and loaded from the AreaMap child only. **K2** does the same in **FUN_004e0ee0** (GetStructFromStruct `"AreaMap"`, then AreaMapResX, AreaMapResY, AreaMapDataSize, AreaMapData; same field names and logic).

### How GetListCount works (for load loops)

When a loader has obtained a list via **GetList**, it needs the number of elements. **CResGFF::GetListCount** (K1 @ `0x00411940`) takes the list handle and returns that count. In the GFF layout, a list field’s data points to a block in **list_indices_data**: the first 4 bytes of that block are the **element count**, and the following 4 bytes per element are the struct indices. So GetListCount simply reads and returns that first DWORD. If the list is missing or invalid, it returns 0, so the loader’s loop runs zero times and no elements are loaded. Similarly, **GetList** returns 0 (failure) if the root struct has no field with that label or the field is not of type List; in that case the loader never enters the loop and simply returns. So a GIT that omits a list (e.g. no “Door List” field) is valid: the door loader will find no list and load zero doors. Empty lists (list present but count 0) are also valid and result in no objects of that type being spawned. The load loop is always: `count = GetListCount(list); for (index = 0; index < count; index++) { GetListElement(..., index); GetElementType(...); if (type == expected) { ... } }`.

### Example: Door List element fields (K1) — save/load pattern and shared position

To make the abstract “each Save* writes type-specific fields” concrete, here is what the engine does for **Door List** (struct ID 8). The same idea applies to other entity lists: **ObjectId** plus type-specific serialization plus shared state (position/orientation).

**SaveDoors** (K1 @ `0x00507810`): For each door, after **AddListElement(..., 8)** it writes **ObjectId** (DWORD), then **CSWSDoor::SaveDoor** (door-specific data: resref, linked module/area, trap state, etc.), then **CSWSObject::SaveObjectState** (shared object state). So each door list element struct contains at least ObjectId plus whatever SaveDoor and SaveObjectState write.

**LoadDoors** (K1 @ `0x0050a0e0`): After **GetElementType(...) == 8**, it reads **ObjectId**, creates a CSWSDoor, then **CSWSDoor::LoadDoorExternal** (door-specific load). Then it reads **Bearing** (FLOAT — used to build orientation), sets orientation, and if the “saved game” parameter is non-zero calls **LoadObjectState**. Finally it reads **X**, **Y**, **Z** (FLOATs) and calls **AddToArea**. So the door element is expected to have at least: **ObjectId**, **Bearing**, **X**, **Y**, **Z**, plus whatever LoadDoorExternal and LoadObjectState read. Many entity types use **X**, **Y**, **Z** for position and either **XOrientation**/**YOrientation**/**ZOrientation** or **Bearing** (doors) or a quaternion for facing; the exact names are type-specific.

This pattern—ObjectId, then type-specific load/save, then shared position and optionally LoadObjectState/SaveObjectState—is repeated across creatures, items, doors, triggers, and so on. Tool authors building a GIT should provide the same root-level list names and struct IDs and, per list type, the field names that the corresponding Save* and Load* expect (often documented in GFF specs or reverse-engineered from the binaries as here).

### Example: WaypointList element fields (K1) — position field names differ from doors

**WaypointList** (struct ID 5) follows the same overall pattern but uses **different position field names** than doors. **SaveWaypoints** (K1 @ `0x00507a50`): AddList `"WaypointList"`, then for each waypoint AddListElement(..., 5), WriteFieldDWORD ObjectId, **CSWSWaypoint::SaveWaypoint**, **SaveObjectState**. **LoadWaypoints** (K1 @ `0x00505360`): After GetElementType == 5, read **ObjectId**, create waypoint, **LoadWaypoint** (type-specific), then if the saved-game flag is non-zero **LoadObjectState**; then read **XPosition**, **YPosition**, **ZPosition** (FLOATs) and **AddToArea**. So waypoints use **XPosition** / **YPosition** / **ZPosition**, whereas doors use **X** / **Y** / **Z**. Creatures use **XPosition**, **YPosition**, **ZPosition** and **XOrientation**, **YOrientation**, **ZOrientation**. So the exact label names for position and orientation are **type-specific**; see the quirk “Position and orientation field names vary by type” below.

### GFF list label length and ordering

**AddList** (K1) copies the list label with `_strncpy(list->label, labelText, 0x10)`, so list names are **truncated to 16 characters** (including the null terminator). The labels in the Primary Table (e.g. `"Creature List"`, `"TriggerList"`) all fit within that limit. The order in which **AddList** is called during SaveGIT is fixed (Creature List, List, Door List, …). The GFF format does not require list fields to appear in any particular order on the root struct; the engine finds each list by **label**, not by position. So when reading a GIT, the loader does not depend on the physical order of list fields in the file—only on the presence of a field with the correct label and type List.

---

## Four Methods of Validation

All Struct IDs below were confirmed by **four independent methods**:

1. **Decompiled C code** (`get-functions view=decompile`) — explicit integer literal passed as last argument to `AddListElement(...)` or compared against `GetElementType(...)` return.
2. **Raw x86 assembly** (`get-functions view=disassemble`) — `PUSH imm8` instruction (`6A XX`) immediately before the `CALL 0x004124e0` (`AddListElement`) site.
3. **Raw memory bytes** (`inspect-memory mode=read`) — actual hex bytes at the `PUSH` instruction address, e.g. `6A 04` = PUSH 4.
4. **K2 cross-validation** (`match-function` + `get-functions decompile` on K2) — matching K2 function decompilations confirm identical `structId` arguments and identical list name strings.

---

## Primary GIT List Struct IDs — Complete Table

| GFF List Field Name | Struct ID (decimal) | Struct ID (hex) | Save Function (K1) | Load Function (K1) | Load Validates? |
|---|---|---|---|---|---|
| `"Creature List"` | **4** | `0x04` | `SaveCreatures` @ `0x00507680` | `LoadCreatures` @ `0x00504a70` | YES — `iVar1 == 4` |
| `"List"` | **0** | `0x00` | `SaveItems` @ `0x00507750` | `LoadItems` @ `0x00504de0` | YES — `iVar2 == 0` |
| `"Door List"` | **8** | `0x08` | `SaveDoors` @ `0x00507810` | `LoadDoors` @ `0x0050a0e0` | YES — `iVar2 == 8` |
| `"TriggerList"` | **1** | `0x01` | `SaveTriggers` @ `0x005078d0` | `LoadTriggers` @ `0x0050a350` | YES — `iVar2 == 1` |
| `"Encounter List"` | **7** | `0x07` | `SaveEncounters` @ `0x00507990` | `LoadEncounters` @ `0x00505060` | YES — `iVar2 == 7` |
| `"WaypointList"` | **5** | `0x05` | `SaveWaypoints` @ `0x00507a50` | `LoadWaypoints` @ `0x00505360` | YES — `iVar2 == 5` |
| `"SoundList"` | **6** | `0x06` | `SaveSounds` @ `0x00507b10` | `LoadSounds` @ `0x00505560` | YES — `iVar3 == 6` |
| `"Placeable List"` | **9** | `0x09` | `SavePlaceables` @ `0x00507bd0` | `LoadPlaceables` @ `0x0050a7b0` | YES — `iVar2 == 9` |
| `"StoreList"` | **11** | `0x0B` | `SaveStores` @ `0x00507ca0` | `LoadStores` @ `0x005057a0` | YES — `iVar2 == 0xb` |
| `"AreaEffectList"` | **13** | `0x0D` | `SaveAreaEffects` @ `0x00507d60` | `LoadAreaEffects` @ `0x00505af0` | YES — `iVar2 == 0xd` |
| `"CameraList"` | **14** | `0x0E` | `SavePlaceableCameras` @ `0x005062a0` | `LoadPlaceableCameras` @ `0x00505eb0` | **NO** — no `GetElementType` check |

---

## K1 vs K2 Function Address Reference

All addresses below come from Reva `get-functions` (decompile) and `get-call-graph` (callees) on the respective binaries.

| Role | K1 (swkotor.exe) | K2 (swkotor2.exe) |
|------|-------------------|-------------------|
| **SaveGIT** | `CSWSArea::SaveGIT` @ **0x0050ba00** | `SaveGIT` @ **0x004e7040** |
| **LoadGIT** | `CSWSArea::LoadGIT` @ **0x0050dd80** | `LoadGIT` @ **0x004e9440** |
| Save Creatures | `SaveCreatures` @ **0x00507680** | `SerializeCreatureList_K2` @ **0x004e28c0** |
| Save Items | `SaveItems` @ **0x00507750** | `FUN_004e29a0` @ **0x004e29a0** |
| Save Doors | `SaveDoors` @ **0x00507810** | `FUN_004e2a60` @ **0x004e2a60** |
| Save Triggers | `SaveTriggers` @ **0x005078d0** | `FUN_004e2b20` @ **0x004e2b20** |
| Save Encounters | `SaveEncounters` @ **0x00507990** | `FUN_004e2be0` @ **0x004e2be0** |
| Save Waypoints | `SaveWaypoints` @ **0x00507a50** | `FUN_004e2ca0` @ **0x004e2ca0** |
| Save Sounds | `SaveSounds` @ **0x00507b10** | `FUN_004e2d60` @ **0x004e2d60** |
| Save Placeables | `SavePlaceables` @ **0x00507bd0** | `FUN_004e2e20` @ **0x004e2e20** |
| Save Stores | `SaveStores` @ **0x00507ca0** | `FUN_004e2ef0` @ **0x004e2ef0** |
| Save AreaEffects | `SaveAreaEffects` @ **0x00507d60** | `FUN_004e2fb0` @ **0x004e2fb0** |
| Save PlaceableCameras | `SavePlaceableCameras` @ **0x005062a0** | `FUN_004e13f0` @ **0x004e13f0** (saves "CameraList", struct ID 14) |
| Load Creatures | `LoadCreatures` @ **0x00504a70** | `FUN_004dfbb0` @ **0x004dfbb0** |
| Load Items | `LoadItems` @ **0x00504de0** | `FUN_004dff20` @ **0x004dff20** |
| Load Doors | `LoadDoors` @ **0x0050a0e0** | `FUN_004e04a0` @ **0x004e04a0** |
| Load Triggers | `LoadTriggers` @ **0x0050a350** | `FUN_004e56b0` @ **0x004e56b0** |
| Load Encounters | `LoadEncounters` @ **0x00505060** | `FUN_004e5920` @ **0x004e5920** |
| Load Waypoints | `LoadWaypoints` @ **0x00505360** | `FUN_004e01a0` @ **0x004e01a0** |
| Load Sounds | `LoadSounds` @ **0x00505560** | `FUN_004e06a0` @ **0x004e06a0** |
| Load Placeables | `LoadPlaceables` @ **0x0050a7b0** | `FUN_004e5d80` @ **0x004e5d80** |
| Load Stores | `LoadStores` @ **0x005057a0** | `FUN_004e08e0` @ **0x004e08e0** |
| Load AreaEffects | `LoadAreaEffects` @ **0x00505af0** | `FUN_004e0c30` @ **0x004e0c30** |
| Load PlaceableCameras | `LoadPlaceableCameras` @ **0x00505eb0** | `FUN_004e0ff0` @ **0x004e0ff0** (loads "CameraList"; no GetElementType check) |
| Save Properties | `SaveProperties` @ **0x00506090** | `SaveAreaProperties` @ **0x004e11d0** |
| Load Properties | `LoadProperties` @ **0x00507490** | `LoadAreaProperties` @ **0x004e26d0** |
| Save Maps | `SaveMaps` @ **0x005061d0** | `FUN_004e1320` @ **0x004e1320** (SaveMaps: AreaMap child, 0x65) |
| Load Maps | `LoadMaps` @ **0x00505da0** | `FUN_004e0ee0` @ **0x004e0ee0** (LoadMaps: AreaMap child) |

K2 `LoadGIT` callees: the 10 entity loaders, then `LoadAreaProperties`, then `FUN_004e0ee0` (LoadMaps — reads AreaMap, AreaMapResX/Y, AreaMapData), then `FUN_004e0ff0` (loads CameraList). K2 `SaveGIT` after AreaEffects calls `SaveAreaProperties`, `FUN_004e1320` (SaveMaps — AreaMap fields), `FUN_004e13f0` (SavePlaceableCameras: AddList "CameraList", AddListElement with struct ID 14). So both games save and load CameraList; K2 uses unnamed functions.

---

## K2 Post–AreaEffects Save/Load: Maps and CameraList (Reva Evidence)

K2 does not use the same symbol names as K1 for the last two steps of SaveGIT and LoadGIT. Reva decompilation identifies them as follows.

**After the 10 entity lists, K2 SaveGIT** (`0x004e7040`) calls:
- `SaveAreaProperties(this, this_00, _Memory)` — area properties (same schema as K1: AreaProperties child, struct ID 100).
- **FUN_004e1320** — **SaveMaps**: calls `FUN_004136d0(param_1, &outStruct, param_2, "AreaMap", 0x65)` (AddStructToStruct) to create the **AreaMap child** with struct ID **0x65**, then writes **AreaMapResX**, **AreaMapResY**, **AreaMapDataSize**, **AreaMapData** into that child (same as K1 SaveMaps).
- **FUN_004e13f0** — Calls `FUN_00413570(..., "CameraList")` (AddList), then for each placeable camera `FUN_00413600(..., 0xe)` (AddListElement with struct ID **14**) and writes `CameraID`, `Position`, `Orientation`, `Pitch`, `Height`, `FieldOfView`, `MicRange`. So **FUN_004e13f0 = SavePlaceableCameras**.

**After the 10 entity loaders and LoadAreaProperties, K2 LoadGIT** (`0x004e9440`) calls:
- **FUN_004e0ee0** — **LoadMaps**: gets the **child struct** by label `"AreaMap"` via GetStructFromStruct (K2: `FUN_00412b30`), then reads **AreaMapResX**, **AreaMapResY**, **AreaMapDataSize**, **AreaMapData** from that child and applies them to the module’s map state. Same schema as K1 LoadMaps.
- **FUN_004e0ff0** — **LoadPlaceableCameras**: GetList `"CameraList"`, GetListCount, then loop with GetListElement and read `CameraID`, Position, Orientation, Pitch, Height, FieldOfView, MicRange. Does **not** call GetElementType (same as K1).

**K2 LoadGIT entity order** (Reva decompilation): the 10 list loaders are called in this order — Creatures (FUN_004dfbb0), Items (FUN_004dff20), **Triggers** (FUN_004e56b0), **Encounters** (FUN_004e5920), **Waypoints** (FUN_004e01a0), **Doors** (FUN_004e04a0), Sounds (FUN_004e06a0), Placeables (FUN_004e5d80), Stores (FUN_004e08e0), AreaEffects (FUN_004e0c30). So K2 calls **Triggers, Encounters, Waypoints** before **Doors**, whereas K1 calls **Doors** before Triggers. The GFF layout is unchanged (lists are found by label); only the call order differs. **FUN_004e04a0** (LoadDoors) is called with **four** arguments `(this, gff, root, param_1)` — no UseTemplates argument is passed to the door loader in K2, unlike the other nine entity loaders which receive UseTemplates as the fifth parameter.

---

## Proof: CameraList is in both K1 and K2 (K2 uses unnamed functions)

The following is proven via Reva decompilation. Earlier documentation claimed CameraList was K1-only; that was incorrect. K2 **does** save and load CameraList; it uses different (unnamed) functions.

### 1. K2 SaveGIT does call a camera-save function

Full serialization block of **K2 SaveGIT** (`0x004e7040`), as returned by `get-functions` decompile (Reva). After the 10 entity serializers and SaveAreaProperties, lines 251–253:

```c
  SaveAreaProperties(this,this_00,_Memory);
  FUN_004e1320(this_00,_Memory);
  FUN_004e13f0(this_00,_Memory);
```

Decompiling **FUN_004e13f0** shows it calls `FUN_00413570(..., "CameraList")` (AddList) and `FUN_00413600(..., 0xe)` (AddListElement with struct ID 14), then writes CameraID, Position, Orientation, Pitch, Height, FieldOfView, MicRange. So **K2 does save CameraList** via FUN_004e13f0. **FUN_004e1320** (SaveMaps) uses `FUN_004136d0(..., "AreaMap", 0x65)` (AddStructToStruct) to create the AreaMap child, then writes AreaMapResX, AreaMapResY, AreaMapDataSize, AreaMapData into it — same layout as K1.

### 2. K2 LoadGIT loads CameraList via FUN_004e0ff0

Decompiling **FUN_004e0ff0** shows it calls GetList `"CameraList"`, gets the list count, then loops with GetListElement and reads CameraID, Position, Orientation, Pitch, Height, FieldOfView, MicRange. So **K2 does load CameraList** via FUN_004e0ff0. It does not call GetElementType (same as K1’s LoadPlaceableCameras). **FUN_004e0ee0** (LoadMaps) uses GetStructFromStruct to get the child struct `"AreaMap"`, then reads the same four fields as K1.

### 3. Why the earlier “K1-only” conclusion was wrong

The earlier proof relied on (a) K2 SaveGIT “not calling any camera-save function” — but FUN_004e13f0 is that function; it is just not named in the binary; (b) search-constants for K1’s string address in K2 returning 0 — K2 has its own data segment, so the string `"CameraList"` lives at a different address in K2 and is still used by FUN_004e13f0 and FUN_004e0ff0. So **CameraList is supported in both games**; the GFF layout (list name "CameraList", struct ID 14) is identical.

---

## Full SaveGIT / LoadGIT Decompiled Call Sequences

These blocks are taken verbatim from Reva `get-functions` decompile (with sufficient `limit`) for the given addresses.

### K1 CSWSArea::SaveGIT (0x0050ba00) — list serialization block (lines 244–256)

```c
  SaveCreatures(this,this_00,struct,&local_90);
  SaveItems(this,this_00,struct,&local_9c);
  SaveDoors(this,this_00,struct,&doors_list);
  SaveTriggers(this,this_00,struct,&local_a8);
  SaveEncounters(this,this_00,struct,&local_50);
  SaveWaypoints(this,this_00,struct,&local_30);
  SaveSounds(this,this_00,struct,&local_44);
  SavePlaceables(this,this_00,struct,&local_5c);
  SaveStores(this,this_00,struct,&local_6c);
  SaveAreaEffects(this,this_00,struct,&local_78);
  SaveProperties(this,this_00,struct);
  SaveMaps(this,this_00,struct);
  SavePlaceableCameras(this,this_00,struct);
```

### K1 CSWSArea::LoadGIT (0x0050dd80) — list loading block (lines 52–64)

```c
      LoadCreatures(this,this_00,&local_18,param_1,uVar4);
      LoadItems(this,this_00,&local_18,param_1,uVar4);
      LoadDoors(this,this_00,&local_18,param_1,uVar4);
      LoadTriggers(this,this_00,&local_18,param_1,uVar4);
      LoadEncounters(this,this_00,&local_18,param_1,uVar4);
      LoadWaypoints(this,this_00,&local_18,param_1,uVar4);
      LoadSounds(this,this_00,&local_18,param_1,uVar4);
      LoadPlaceables(this,this_00,&local_18,param_1,uVar4);
      LoadStores(this,this_00,&local_18,param_1,uVar4);
      LoadAreaEffects(this,this_00,&local_18,param_1,uVar4);
      LoadProperties(this,this_00,&local_18);
      LoadMaps(this,this_00,&local_18);
      LoadPlaceableCameras(this,this_00,&local_18);
```

### K2 SaveGIT (0x004e7040) — list serialization block (lines 241–253)

```c
  SerializeCreatureList_K2(this_00,_Memory,&local_90);
  FUN_004e29a0(this_00,_Memory,&local_9c);
  FUN_004e2a60(this_00,_Memory,&local_84);
  FUN_004e2b20(this_00,_Memory,&local_a8);
  FUN_004e2be0(this_00,_Memory,local_5c + 3);
  FUN_004e2ca0(this_00,_Memory,local_30);
  FUN_004e2d60(this_00,_Memory,local_5c + 6);
  FUN_004e2e20(this_00,_Memory,local_5c);
  FUN_004e2ef0(this_00,_Memory,local_78 + 3);
  FUN_004e2fb0(this_00,_Memory,local_78);
  SaveAreaProperties(this,this_00,_Memory);
  FUN_004e1320(this_00,_Memory);
  FUN_004e13f0(this_00,_Memory);
```

### K2 LoadGIT (0x004e9440) — list loading block (lines 48–60)

```c
      FUN_004dfbb0(this,(undefined2 *)this_00,&local_18,(int)param_1,uVar4);
      FUN_004dff20(this,this_00,&local_18,param_1,uVar4);
      FUN_004e56b0(this,this_00,&local_18,param_1,uVar4);
      FUN_004e5920(this_00,&local_18,(int)param_1,uVar4);
      FUN_004e01a0(this,this_00,&local_18,param_1,uVar4);
      FUN_004e04a0(this,this_00,&local_18,param_1);
      FUN_004e06a0(this,this_00,&local_18,param_1,uVar4);
      FUN_004e5d80(this,this_00,&local_18,param_1,uVar4);
      FUN_004e08e0(this,this_00,&local_18,param_1,uVar4);
      FUN_004e0c30(this,this_00,&local_18,param_1);
      LoadAreaProperties(this,this_00,&local_18);
      FUN_004e0ee0(this_00,&local_18);
      FUN_004e0ff0(this_00,&local_18);
```

---

## Evidence Records Per Entry

### "Creature List" → Struct ID 4

**String address:** `0x007458dc`  
**search-constants for string address** returned:
- `SaveCreatures` (0x00507680): `PUSH 0x7458dc` at `0x005076ad`
- `LoadCreatures` (0x00504a70): `PUSH 0x7458dc` at `0x00504a93`

**K1 full decompilation** (`SaveCreatures` @ `0x00507680`):

```c
void CSWSArea::SaveCreatures(CSWSArea *this,CResGFF *param_1,CResStruct *param_2,CExoArrayList *param_3)
{
  ...
  local_1c = CServerExoApp::GetObjectArray(AppManager->server);
  CServerExoApp::GetPartyTable(AppManager->server);
  CResGFF::AddList(param_1,&local_14,param_2,"Creature List");
  iVar2 = 0;
  if (0 < param_3->size) {
    do {
      bVar1 = CGameObjectArray::GetGameObject(local_1c,param_3->data[iVar2],&local_18);
      if ((bVar1 == bool_false) &&
         (this_00 = (*local_18->vtable->AsSWSCreature)(), this_00->field430_0xa88 == 0)) {
        CResGFF::AddListElement(param_1,&CStack_20,&local_14,4);
        CResGFF::WriteFieldDWORD(param_1,&CStack_20,(this_00->object).game_object.id,"ObjectId");
        CSWSCreature::SerializeCreature_K2(this_00,param_1,(int)&CStack_20);
      }
      iVar2 = iVar2 + 1;
    } while (iVar2 < param_3->size);
  }
  return;
}
```

So the list label is `"Creature List"` and the struct ID passed to `AddListElement` is **4**.

**K1 Disassemble** (`SaveCreatures`): `0x00507702: PUSH 0x4` → `0x00507708: CALL 0x004124e0`  
**K1 inspect-memory** `0x00507702`: hex `6A 04` ← raw byte proof

**K1 full decompilation** (`LoadCreatures` @ `0x00504a70`) — **GetElementType and check for 4**:

```c
  iVar1 = CResGFF::GetList(param_1,&local_20,param_2,"Creature List");
  if (iVar1 != 0) {
    local_70 = 0;
    iVar1 = CResGFF::GetListCount(this_00,&local_20);
    if (iVar1 != 0) {
      do {
        iVar1 = CResGFF::GetListElement(this_00,(CResStruct *)&param_2,&local_20,local_70);
        if ((iVar1 != 0) &&
           (iVar1 = CResGFF::GetElementType(this_00,(CResStruct *)&param_2), iVar1 == 4)) {
          // ... load creature, add to area, set orientation ...
        }
        ...
        local_70 = local_70 + 1;
      } while (local_70 < ...);
    }
  }
```

So `LoadCreatures` calls `GetElementType` and only processes the element when the return value **equals 4**; otherwise the element is skipped.

**K2 match** (`match-function` from `0x00507680` → `SerializeCreatureList_K2` @ `0x004e28c0`, 91% similarity):
```c
FUN_00413570(param_1, local_14, param_2, "Creature List");  // list name
FUN_00413600(param_1, &uStack_20, local_14, 4);             // structId = 4
```

---

### "List" → Struct ID 0 (Items)

**String address:** `0x007474a4`  
**inspect-memory** `0x007474a4` (20 bytes): `4C 69 73 74 00` = `"List\0"`  
**search-constants** for `0x007474a4` returned exactly:
- `SaveItems` (0x00507750): `PUSH 0x7474a4` at `0x0050776b`
- `LoadItems` (0x00504de0): `PUSH 0x7474a4` at `0x00504e02`

**K1 Disassemble** (`SaveItems`): `0x005077b6: PUSH 0x0` → `0x005077c6: CALL 0x004124e0`  
**K1 inspect-memory** `0x005077b6`: hex `6A 00` ← raw byte proof

**K1 Decompile** (`LoadItems`): `CResGFF::GetElementType(...)` → `if (iVar2 == 0)` accepts; else skips

**K2 match** (`SaveItems` @ `0x00507750` → `FUN_004e29a0` @ K2, 100% similarity):
```c
FUN_00413570(param_1, local_14, param_2, "List");   // list name
FUN_00413600(param_1, &uStack_20, local_14, 0);     // structId = 0
```

> **Note:** The GFF field name is simply `"List"`, not `"ItemList"`. The string `"ItemList"` at `0x00747210` is an entirely separate string used by container/placeable/store item sub-lists, not the GIT top-level item list.

---

### "Door List" → Struct ID 8

**String address:** `0x00747680`  
**search-constants** for `0x00747680`:
- `SaveDoors` (0x00507810): `PUSH 0x747680` at `0x0050782b`
- `LoadDoors` (0x0050a0e0): `PUSH 0x747680` at `0x0050a103`

**K1 Disassemble** (`SaveDoors`): `0x00507876: PUSH 0x8` → `CALL 0x004124e0`  
**K1 inspect-memory** `0x00507876`: hex `6A 08`

**K2 match** (`FUN_004e2a60` @ K2):
```c
FUN_00413570(param_1, local_14, param_2, "Door List");  // list name
FUN_00413600(param_1, &uStack_20, local_14, 8);         // structId = 8
```

---

### "TriggerList" → Struct ID 1

**String address:** `0x0074768c`  
**search-constants** for `0x0074768c`:
- `SaveTriggers` (0x005078d0): `PUSH 0x74768c` at `0x005078eb`
- `LoadTriggers` (0x0050a350): `PUSH 0x74768c` at `0x0050a37b`

**K1 Disassemble** (`SaveTriggers`): `0x00507936: PUSH 0x1` → `CALL 0x004124e0`  
**K1 inspect-memory** `0x00507936`: hex `6A 01`

**K2 match** (`FUN_004e2b20` @ K2):
```c
FUN_00413570(param_1, local_14, param_2, "TriggerList");
FUN_00413600(param_1, &uStack_20, local_14, 1);          // structId = 1
```

---

### "Encounter List" → Struct ID 7

**String address:** `0x007474c8`  
**search-constants** for `0x007474c8`:
- `SaveEncounters` (0x00507990): `PUSH 0x7474c8` at `0x005079ab`
- `LoadEncounters` (0x00505060): `PUSH 0x7474c8` at `0x00505082`

**K1 Disassemble** (`SaveEncounters`): `0x005079f6: PUSH 0x7` → `CALL 0x004124e0`  
**K1 inspect-memory** `0x005079f6`: hex `6A 07`

**K2 match** (`FUN_004e2be0` @ K2):
```c
FUN_00413570(param_1, local_14, param_2, "Encounter List");
FUN_00413600(param_1, &uStack_20, local_14, 7);             // structId = 7
```

---

### "WaypointList" → Struct ID 5

**String address:** `0x007474d8`  
**search-constants** for `0x007474d8`:
- `SaveWaypoints` (0x00507a50): `PUSH 0x7474d8` at `0x00507a6b`
- `LoadWaypoints` (0x00505360): `PUSH 0x7474d8` at `0x00505383`

**K1 Disassemble** (`SaveWaypoints`): `0x00507ab6: PUSH 0x5` → `CALL 0x004124e0`  
**K1 inspect-memory** `0x00507ab6`: hex `6A 05`

**K2 match** (`FUN_004e2ca0` @ K2):
```c
FUN_00413570(param_1, local_14, param_2, "WaypointList");
FUN_00413600(param_1, &uStack_20, local_14, 5);           // structId = 5
```

---

### "SoundList" → Struct ID 6

**String address:** `0x007474f8`  
**search-constants** for `0x007474f8`:
- `SaveSounds` (0x00507b10): `PUSH 0x7474f8` at `0x00507b2b`
- `LoadSounds` (0x00505560): `PUSH 0x7474f8` at `0x00505582`

**K1 Disassemble** (`SaveSounds`): `0x00507b76: PUSH 0x6` → `CALL 0x004124e0`  
**K1 inspect-memory** `0x00507b76`: hex `6A 06`

**K2 match** (`FUN_004e2d60` @ K2):
```c
FUN_00413570(param_1, local_14, param_2, "SoundList");
FUN_00413600(param_1, &uStack_20, local_14, 6);       // structId = 6
```

---

### "Placeable List" → Struct ID 9

**String address:** `0x00747698`  
**search-constants** for `0x00747698`:
- `SavePlaceables` (0x00507bd0): `PUSH 0x747698` at `0x00507beb`
- `LoadPlaceables` (0x0050a7b0): `PUSH 0x747698` at `0x0050a7d6`

**K1 Disassemble** (`SavePlaceables`): `0x00507c42: PUSH 0x9` → `CALL 0x004124e0`  
**K1 inspect-memory** `0x00507c42`: hex `6A 09`

**K2 match** (`FUN_004e2e20` @ K2):
```c
FUN_00413570(param_1, local_14, param_2, "Placeable List");
FUN_00413600(param_1, &uStack_20, local_14, 9);             // structId = 9
```

---

### "StoreList" → Struct ID 11 (0x0B)

**String address:** `0x00747510`  
**search-constants** for `0x00747510`:
- `SaveStores` (0x00507ca0): `PUSH 0x747510` at `0x00507cbb`
- `LoadStores` (0x005057a0): `PUSH 0x747510` at `0x005057c2`

**K1 Disassemble** (`SaveStores`): `0x00507d06: PUSH 0xb` → `CALL 0x004124e0`  
**K1 inspect-memory** `0x00507d06`: hex `6A 0B`

**K2 match** (`FUN_004e2ef0` @ K2):
```c
FUN_00413570(param_1, local_14, param_2, "StoreList");
FUN_00413600(param_1, &uStack_20, local_14, 0xb);     // structId = 11
```

---

### "AreaEffectList" → Struct ID 13 (0x0D)

**String address:** `0x0074751c`  
**search-constants** for `0x0074751c`:
- `SaveAreaEffects` (0x00507d60): `PUSH 0x74751c` at `0x00507d7b`
- `LoadAreaEffects` (0x00505af0): `PUSH 0x74751c` at `0x00505b12`

**K1 Disassemble** (`SaveAreaEffects`): `0x00507dc6: PUSH 0xd` → `CALL 0x004124e0`  
**K1 inspect-memory** `0x00507dc6`: hex `6A 0D`

**K2 match** (`FUN_004e2fb0` @ K2):
```c
FUN_00413570(param_1, local_14, param_2, "AreaEffectList");
FUN_00413600(param_1, &uStack_20, local_14, 0xd);          // structId = 13
```

---

### "CameraList" → Struct ID 14 (0x0E)

**String address:** `0x007475b4`  
**search-constants** for `0x007475b4`:
- `SavePlaceableCameras` (0x005062a0): `PUSH 0x7475b4` at `0x005062bb`
- `LoadPlaceableCameras` (0x00505eb0): `PUSH 0x7475b4` at `0x00505ebd`

**K1 Disassemble** (`SavePlaceableCameras`): `0x005062f2: PUSH 0xe` → `CALL 0x004124e0`  
**K1 inspect-memory** `0x005062f2`: hex `6A 0E`

**K1 full decompilation** (`LoadPlaceableCameras` @ `0x00505eb0`) — **no `GetElementType` call**; loop uses `GetListElement` then reads fields directly:

```c
  CResGFF::GetList(param_1,&local_14,param_2,"CameraList");
  iVar1 = CResGFF::GetListCount(param_1,&local_14);
  if (iVar1 < 0x33) {
    ...
    do {
      CResGFF::GetListElement(param_1,(CResStruct *)&param_2,&local_14,index);
      iVar3 = CResGFF::ReadFieldINT(param_1,(CResStruct *)&param_2,"CameraID",&local_5c,-1);
      CResGFF::ReadFieldVector(...);
      CResGFF::ReadFieldQuaternion(...);
      ...
      CGuiInGame::SetPlaceableCamera(pCVar2,index_00,iVar3,pVVar5,pQVar6,pitch,height,fov,micRange);
      index = index + 1;
    } while ((int)index < iVar1);
  }
```

There is no `CResGFF::GetElementType` anywhere in this function (Reva decompilation, 76 lines). So load does **not** validate struct ID for camera list elements.

> **K2:** K2 uses `FUN_004e13f0` (save) and `FUN_004e0ff0` (load) for CameraList; see section **Proof: CameraList is in both K1 and K2** and **K2 Post–AreaEffects Save/Load** above.

---

## Nested Struct IDs Within Complex Objects

### Trigger Geometry ("Geometry" within each TriggerList element)

**GFF List Field Name:** `"Geometry"` (string at `0x007474bc`)  
**Struct ID:** **3**  
**Save function:** `SaveTrigger` @ `0x0058e660`

K1 Decompile:
```c
CResGFF::AddListElement(param_1, &uStack_14, local_14, 3);
//                                                       ^ structId = 3 for geometry points
```

K1 Load validation: `LoadTriggerGeometry` @ `0x0058d060` — calls `GetElementType`, checks `iVar6 == 3`; skips geometry point if wrong.

---

### Encounter Sub-Lists (within each EncounterList element)

Save function: `SaveEncounter` @ `0x00591350`

| Sub-list Name | Struct ID | Notes |
|---|---|---|
| `"Geometry"` | **dynamic** (loop index `i`) | 0-based counter variable `uVar2`; each point gets its index as ID |
| `"CreatureList"` | **dynamic** (loop index `i`) | 0-based counter `param_1`; each entry gets index as ID |
| `"SpawnPointList"` | **dynamic** (loop index `i`) | 0-based counter `param_1`; each entry gets index as ID |
| `"AreaList"` | **3** (fixed) | `CResGFF::AddListElement(..., 3)` — hardcoded constant |
| `"SpawnList"` | **dynamic** (loop index `i`) | 0-based counter `uVar2`; each entry gets index as ID |

Load behavior:
- `LoadEncounterGeometry` @ `0x00590580` — does NOT call `GetElementType`; reads X/Y/Z directly
- `LoadEncounterSpawnPoints` @ `0x00590410` — does NOT call `GetElementType`; reads X/Y/Z/Orientation directly

---

## Load-Time Struct ID Validation Summary

The `Load*` functions call `CResGFF::GetElementType(...)`, read the stored `structId` from the GFF binary, and compare it to the expected value. Elements that fail the check are **silently skipped** — they are not loaded, and no error is raised.

| Load Function | Address | Expected Struct ID | Validates? |
|---|---|---|---|
| `LoadCreatures` | `0x00504a70` | 4 | YES |
| `LoadItems` | `0x00504de0` | 0 | YES |
| `LoadEncounters` | `0x00505060` | 7 | YES |
| `LoadWaypoints` | `0x00505360` | 5 | YES |
| `LoadSounds` | `0x00505560` | 6 | YES |
| `LoadStores` | `0x005057a0` | 11 (0xB) | YES |
| `LoadAreaEffects` | `0x00505af0` | 13 (0xD) | YES |
| `LoadPlaceableCameras` | `0x00505eb0` | (none) | **NO** |
| `LoadDoors` | `0x0050a0e0` | 8 | YES |
| `LoadTriggers` | `0x0050a350` | 1 | YES |
| `LoadPlaceables` | `0x0050a7b0` | 9 | YES |
| `LoadTriggerGeometry` | `0x0058d060` | 3 | YES |
| `LoadEncounterGeometry` | `0x00590580` | (none) | **NO** |
| `LoadEncounterSpawnPoints` | `0x00590410` | (none) | **NO** |

---

## K2 Cross-Validation Summary

All 11 main GIT list struct IDs (including CameraList) were confirmed in KotOR 2 (`/k2_win_gog_legacypc_swkotor2.exe`) via `match-function` + `get-functions decompile`:

| List Name | K1 ID | K2 ID | K2 Function | K2 Address | Similarity |
|---|---|---|---|---|---|
| "Creature List" | 4 | **4** | `SerializeCreatureList_K2` | `0x004e28c0` | 91% |
| "List" | 0 | **0** | `FUN_004e29a0` | `0x004e29a0` | 100% |
| "Door List" | 8 | **8** | `FUN_004e2a60` | `0x004e2a60` | 100% |
| "TriggerList" | 1 | **1** | `FUN_004e2b20` | `0x004e2b20` | 100% |
| "Encounter List" | 7 | **7** | `FUN_004e2be0` | `0x004e2be0` | 100% |
| "WaypointList" | 5 | **5** | `FUN_004e2ca0` | `0x004e2ca0` | 100% |
| "SoundList" | 6 | **6** | `FUN_004e2d60` | `0x004e2d60` | 100% |
| "Placeable List" | 9 | **9** | `FUN_004e2e20` | `0x004e2e20` | 100% |
| "StoreList" | 11 | **11** | `FUN_004e2ef0` | `0x004e2ef0` | 100% |
| "AreaEffectList" | 13 | **13** | `FUN_004e2fb0` | `0x004e2fb0` | 100% |
| "CameraList" | 14 | **14** | Save: `FUN_004e13f0`, Load: `FUN_004e0ff0` | `0x004e13f0` / `0x004e0ff0` | same layout |

Every single struct ID is identical between K1 and K2. The GFF format schema for GIT files is fully consistent across both games. CameraList is saved and loaded in both; K2 uses unnamed functions (FUN_004e13f0 / FUN_004e0ff0).

---

## Practical Summary for Tool Authors

When **writing** a GIT (e.g. from an editor or converter):

- Use the exact **list field names** from the Primary Table (e.g. `"Creature List"`, `"List"`, `"Door List"`, …, `"CameraList"`). Names are case-sensitive and must match the engine.
- For each list, give every element the **struct ID** from the table (e.g. 4 for creatures, 0 for items, 8 for doors, 14 for cameras). The engine stores this in the GFF struct’s **id** field.
- Use the same layout for **nested** data (e.g. trigger Geometry with struct ID 3, encounter AreaList with struct ID 3; encounter sub-lists that use dynamic IDs by index).

When **reading** a GIT:

- **Missing or empty lists are valid.** If a list field is absent, GetList fails and that loader loads zero objects. If a list exists but has zero elements, GetListCount returns 0 and the loop runs zero times. You do not need to emit every list or every element.
- The engine finds lists by **label** (GetList with the same names). It then iterates list elements and, for all lists **except** CameraList (and some encounter sub-lists), calls **GetElementType** and only processes elements whose id matches the expected value. So if you are implementing a loader, you can either mirror this (skip elements with wrong ID) or, for CameraList, read every element without checking the ID.
- **GetElementType** returns **-1** if the struct index is out of range or the GFF is invalid; loaders that check for a specific ID will skip such elements as well (since -1 does not equal 4, 0, 8, etc.).

Both games use the **same** list names and struct IDs. A GIT produced by one game or by a tool can be loaded by the other as long as the layout matches this document.

---

## Notable Quirks / Caveats

1. **Items list is named `"List"`, not `"ItemList"`** — the string `"ItemList"` at `0x00747210` is used by container item sub-lists (`SaveContainerItems`, `LoadPlaceable`, `SaveStore`, etc.), not the top-level GIT item list. The GIT item list uses the bare string `"List"`.

2. **CameraList has no load-time struct ID validation** — In both K1 and K2, the loader that reads CameraList (K1: `LoadPlaceableCameras`, K2: `FUN_004e0ff0`) iterates the list without ever calling `GetElementType`. If you write the wrong struct ID into a camera entry, the engine will still load it. This is confirmed by full decompile inspection. **K1 LoadPlaceableCameras** only processes the list if `GetListCount` is less than **0x33 (51)**; if the count is 51 or more, it skips loading cameras entirely (decompilation: `if (iVar1 < 0x33) { ... }`).

3. **Encounter sub-lists use dynamic struct IDs** (loop index as ID) for `"Geometry"`, `"CreatureList"`, `"SpawnPointList"`, and `"SpawnList"` — but `"AreaList"` uses a fixed ID of **3**. This means each geometry/creature/spawn point entry's `structId` is its 0-based array index, not a constant. The load functions for these sub-lists do NOT call `GetElementType` at all — they do not validate struct IDs on read.

4. **Struct IDs are NOT sequential by game-design significance** — Triggers=1, Items=0, Creatures=4, etc. The values appear to have been assigned during engine development with no particular ordering logic.

5. **No struct IDs 2, 3 (at top level), 10, 12 are used in GIT** — struct ID 3 exists only as the nested Trigger Geometry struct ID and as the Encounter `AreaList` entry ID.

6. **Position and orientation field names vary by type** — **Position:** doors use **X**, **Y**, **Z** (FLOAT); waypoints and creatures use **XPosition**, **YPosition**, **ZPosition** (FLOAT). **Orientation:** doors use **Bearing** (one FLOAT); creatures and many others use **XOrientation**, **YOrientation**, **ZOrientation** (three FLOATs). CameraList uses **Position** (vector) and **Orientation** (quaternion). So when writing or parsing a list element, use the field set expected by that type’s Save* / Load* (see the Door List and WaypointList examples above).

7. **List labels are at most 16 characters** — AddList uses `_strncpy(..., 0x10)`, so any label longer than 16 characters (including null) is truncated. All standard GIT list names in this document fit within that limit.

---

## Reva Tools Used

All analysis exclusively used Reva MCP tools on the Andastra Ghidra project:
- **`open`** — open the Ghidra project (`AndastraGhidraProject.gpr`).
- **`list-functions`** — function search (mode `search`, `by_identifiers`) and discovery; **`get-functions`** (view: `decompile`, `disassemble`, `info`) — code extraction and single-function decompilation with `identifier` and `limit`.
- **`get-call-graph`** — caller/callee enumeration (requires `functionIdentifier`).
- **`inspect-memory`** (read mode) — raw hex byte verification at addresses.
- **`manage-strings`** — string listing/search (modes: `list`, `regex`, `count`, `similarity`) for GFF field name discovery.
- **`manage-structures`** (info + list actions) — GFF type layout verification.
- **`match-function`** (dryRun=true) — K1→K2 cross-binary function matching.

Struct IDs and list names were verified by decompiling the corresponding Save* and Load* functions and inspecting the literal arguments to `AddListElement`, `GetElementType`, `AddList`, and `GetList`.
