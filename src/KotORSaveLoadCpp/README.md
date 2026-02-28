# KotOR GIT Save/Load — Executable C++

Runnable C++ that implements the **same logic** as the KotOR 1 engine (SaveGIT @ 0x0050ba00, LoadGIT @ 0x0050dd80). This is not raw decompilation: it is compilable, executable code you can build and run in a C/C++ project.

## What’s included

- **`KotORSaveLoad.h`** — Constants (list names, struct IDs), data types (`AreaState`, `AreaMapState`, etc.), and the **GFF writer/reader interface** (`IGffWriter`, `IGffReader`).
- **`KotORSaveLoad.cpp`** — Save/Load implementation in clean C++:
  - **SaveGIT** — Creates GIT GFF, writes weather/transition, then entity lists (Creatures, Items, Doors, Triggers, Encounters, Waypoints, Sounds, Placeables, Stores, AreaEffects), then Properties, Maps, PlaceableCameras (same order as the engine).
  - **LoadGIT** — Opens GIT, reads weather/UseTemplates, loads the same lists (with struct-ID checks), then Properties, Maps, PlaceableCameras.
- **`GffMemoryBackend.h` / `GffMemoryBackend.cpp`** — In-memory implementation of the GFF interface so the code runs without a real GFF file. Use for tests or replace with your own GFF library.
- **`main.cpp`** — Small test: SaveGIT → round-trip in memory → LoadGIT and verify.

## Build and run

```bash
cd src/KotORSaveLoadCpp
cmake -B build -S .
cmake --build build --config Release
./build/Release/kotor_saveload_test.exe   # Windows
# or: ./build/kotor_saveload_test         # Unix)
```

Expected output: `OK: SaveGIT -> LoadGIT round-trip passed.`

## Using in your C++ project

1. Add `KotORSaveLoad.h`, `KotORSaveLoad.cpp` to your project.
2. Implement `IGffWriter` and `IGffReader` (or use `GffMemoryBackend` for in-memory only).
3. Call `KotOR::SaveGIT(...)` and `KotOR::LoadGIT(...)` with your writer/reader and area/entity data.

For **round-trip without writing binary GFF**: use `GffMemoryBackend`; after `SaveGIT` call `LoadGIT` with `OpenGff(nullptr, 0, &root)` so the reader uses the in-memory root.

## Reference

- Full decompilation (Reva) and field names: **`wiki/GFF-GIT-Full-Save-Load-Code.md`**
- Struct IDs and list names: **`wiki/GFF-GIT-Struct-IDs.md`**
