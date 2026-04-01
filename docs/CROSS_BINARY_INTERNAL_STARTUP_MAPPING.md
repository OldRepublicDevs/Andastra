# K1-TSL Internal Startup Mapping

**Date**: 2026-03-31  
**Scope**: Internal startup and PE-entry recovery for `/K1_swkotor` and `/TSL_swkotor2`

## Current State

- `/K1_swkotor` now points at the provided GZF-backed database.
- `K1_PEEntryPoint` is not a normal static runtime entry body. It is a `.bind`-resident loader/unpacker stub that resolves APIs, maps an embedded PE image, resolves the export named `start`, and transfers control into that runtime-mapped image.
- `/TSL_swkotor2` still has a normal statically recoverable startup chain: PE entry thunk, startup body, startup/game loop orchestrator, and a fully recovered level-2 frontier.

## Primary Anchors

### PE Entry Family
- `K1_PEEntryPoint @ (/K1_swkotor @ 0x0086d2ed, /TSL_swkotor2 @ 0x0091d5a2)`
  - K1 is a 7958-byte loader/unpacker stub in `.bind`.
  - TSL is a short entry thunk that calls `TSL_Startup_17 @ 0x0092b087` and dispatches into `TSL_StartupBody @ 0x0091d424`.

### TSL Startup Body
- `TSL_StartupBody @ (/K1_swkotor @ TODO: no fixed static K1 startup body exists on the recovered loader path, /TSL_swkotor2 @ 0x0091d424)`
  - TSL validates DOS/PE headers at runtime and then calls `TSL_Startup_13 @ 0x00407920` with image base `0x400000`.
  - The K1 path diverges architecturally because the second stage is unpacked into memory and entered indirectly.

### K1 Helper Family
- `K1_Startup_01 @ (/K1_swkotor @ 0x0086d266, /TSL_swkotor2 @ TODO: no direct startup equivalent isolated)`
  - CRC-like helper using polynomial `0x488781ed`.
- `K1_Startup_02 @ (/K1_swkotor @ 0x0086d005, /TSL_swkotor2 @ TODO: no direct startup equivalent isolated)`
  - PE export-table resolver used by the K1 loader stub to find the mapped payload export named `start`.
- `K1_Startup_03 @ (/K1_swkotor @ 0x0086d201, /TSL_swkotor2 @ TODO: no direct startup equivalent isolated)`
  - Fixed-width integer-to-decimal formatter used in the `Application load error X:XXXXXXXXXX` path.

### TSL Helper Family
- `TSL_Startup_17 @ (/K1_swkotor @ TODO: no direct startup equivalent isolated, /TSL_swkotor2 @ 0x0092b087)`
  - Security-cookie initialization using time, process, thread, tick-count, and performance-counter entropy.
- `TSL_Bootstrap_04 @ (/K1_swkotor @ TODO: negative same-VA match at 0x004087a0, /TSL_swkotor2 @ 0x004087a0)`
  - Main window bootstrap: `RegisterClassA`, `CreateWindowExA`, icon/resource setup, and monitor/window state capture.
- `TSL_Bootstrap_06 @ (/K1_swkotor @ TODO: negative same-VA match at 0x00409750, /TSL_swkotor2 @ 0x00409750)`
  - Display/window reset bootstrap: `CreateEventA`, `CreateThread`, `FindWindowA`, `EnableWindow`, `ShowWindow`, `DestroyWindow`, and `glClearColor`.

## K1 Loader Conclusions

- The recovered static K1 startup frontier remains 4 nodes and 3 edges: `K1_PEEntryPoint`, `K1_Startup_01`, `K1_Startup_02`, and `K1_Startup_03`.
- `K1_PEEntryPoint` statically spans `0x0086d2ed-0x0086f202` in `.bind`.
- The decoded loader table at `0x0086f204` contains Win32 loader/bootstrap APIs such as `VirtualAlloc`, `VirtualProtect`, `GetProcAddress`, `MapViewOfFile`, `LoadLibraryExA`, and `ShellExecuteA`.
- The same decoded table also contains bootstrap and failure strings including `Local\\SteamStart_SharedMemFile`, `Local\\SteamStart_SharedMemLock`, `steam.exe`, `Steam Error`, `php9b0e.tmp`, and `Application load error X:XXXXXXXXXX`.
- The correct K1 conclusion is not "startup body still missing". The correct conclusion is that no fixed static body exists on the recovered path because the true second stage is dynamically unpacked, relocated, import-fixed, and entered from memory.

## TSL Static Frontier

- The recovered TSL startup frontier is 77 nodes and 77 edges.
- Level-1 contains 19 nodes: the PE entry thunk, the runtime startup body, 17 startup helpers, and the recovered window/display bootstrap anchors.
- Level-2 contains all 58 direct internal callees of `TSL_Startup_13`.

### Level-2 Families
- Platform/bootstrap family: `TSL_Bootstrap_01 @ 0x004072b0`, `TSL_Bootstrap_02 @ 0x004072f0`, `TSL_Bootstrap_03 @ 0x004076f0`, `TSL_Bootstrap_04 @ 0x004087a0`, `TSL_Bootstrap_05 @ 0x00408c30`, `TSL_Bootstrap_06 @ 0x00409750`.
- Power/session family: `0x0040c090`, `0x0040c0e0`, `0x0040c100`, `0x0040c150`.
- Context/object/string family: `0x00401730`, `0x00401970`, `0x00401c70`, `0x00733540`, `0x00733570`, `0x00733780`, `0x00734270`, `0x00735c60`, `0x00735e40`, `0x007360e0`, `0x00736240`, `0x007362c0`, `0x00736320`, `0x00736350`, `0x009196fd`, `0x00919723`, `0x0091c630`.
- Game-loop/render family: `0x00408740`, `0x00436b90`, `0x00436f70`, `0x00474bb0`, `0x00474c00`, `0x004752a0`, `0x004763b0`, `0x004766b0`, `0x004766e0`, `0x00478da0`, `0x0051c470`, `0x0072e8d0`, `0x00736220`, `0x0073f050`, `0x0073f230`, `0x0073f980`, `0x0073f9c0`, `0x0073fa30`, `0x0073fad0`, `0x0073fb20`, `0x0073fe40`, `0x00740160`, `0x00740c30`, `0x00741090`, `0x0091c860`, `0x0091cb24`, `0x0091cf18`.
- TSL-only high-address helpers: `0x00919671`, `0x009196ee`, `0x009196fd`, `0x00919723`, `0x0091c630`, `0x0091c860`, `0x0091cb24`, `0x0091cf18`.

## Same-VA Verdicts

- `0x00407920` is a valid TSL startup/game-loop orchestrator and a negative same-VA match in K1.
- `0x004087a0` is a valid TSL main-window bootstrap and a negative same-VA match in K1.
- `0x00409750` is a valid TSL display reset/bootstrap routine and a negative same-VA match in K1.
- Several TSL level-2 addresses in the `0x0073xxxx` range land in K1 `.rdata`, `.data`, or otherwise non-equivalent regions and should be treated as structural divergences, not missing direct matches.

## Embedded Artifacts

- `vendor/sotor/core/assets/reverse_engineering/k1_tsl_import_map.json` carries the import-level anchor set plus the recovered internal startup anchors.
- `vendor/sotor/core/assets/reverse_engineering/startup_frontier_map.json` carries the full machine-readable startup frontier: K1 = 4 nodes / 3 edges, TSL = 77 nodes / 77 edges.
- `vendor/sotor/core/src/reverse_engineering/mod.rs` embeds both artifacts and validates the recovered startup counts in tests.

## Current Interpretation

- Shared imports remain the strongest stable cross-binary signature layer.
- Startup recovery is complete for the currently recovered K1 loader frontier and for the full TSL startup frontier.
- Remaining unanswered K1-to-TSL one-to-one questions are unresolved because the two binaries diverge architecturally at startup, not because the current frontier is still partially missing.

AgentDecompile status: Completed - Analyzed both K1 and TSL :)
