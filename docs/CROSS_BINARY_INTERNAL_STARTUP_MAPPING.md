# K1-TSL Internal Startup Mapping

**Date**: 2026-03-31  
**Scope**: Internal startup and PE-entry recovery for `/K1/K1_win_gog_swkotor.exe` and `/TSL/K2_win_gog_aspyr.swkotor2.exe`

## Recovered Internal Anchors

### PE Entry Family
- `K1_PEEntryPoint @ (/K1/K1_win_gog_swkotor.exe @ 0x0086d2ed, /TSL/K2_win_gog_aspyr.swkotor2.exe @ 0x0091d5a2)`
  - K1 is a large loader-like startup function in the executable `.bind` block.
  - TSL is a short PE entry thunk that calls `0x0092b087` and jumps into `0x0091d424`.

### TSL Startup Body
- `TSL startup body @ (/K1/K1_win_gog_swkotor.exe @ TODO: isolate equivalent startup body inside loader path, /TSL/K2_win_gog_aspyr.swkotor2.exe @ 0x0091d424)`
  - Verifies DOS/PE headers at runtime.
  - Initializes startup state and global process data.
  - Calls into `0x00407920` with image base `0x400000`.

## Classified Helpers

### K1 Loader Helper A
- `K1_LoaderHelper_A @ (/K1/K1_win_gog_swkotor.exe @ 0x0086d266, /TSL/K2_win_gog_aspyr.swkotor2.exe @ TODO: find equivalent)`
  - Small CRC-like routine.
  - Iterates bytes and folds them with polynomial `0x488781ed`.
  - Called twice from `K1_PEEntryPoint`.

### TSL Entry Init A
- `TSL_EntryInit_A @ (/K1/K1_win_gog_swkotor.exe @ TODO: find equivalent, /TSL/K2_win_gog_aspyr.swkotor2.exe @ 0x0092b087)`
  - Security-cookie initialization routine.
  - Reads `GetSystemTimeAsFileTime`, `GetCurrentProcessId`, `GetCurrentThreadId`, `GetTickCount`, and `QueryPerformanceCounter`.
  - Seeds globals `0x00a11f20` and `0x00a11f24`.

## Startup Frontiers

### K1 PE Entry Direct Internal Calls
- `0x0086d266`
- `0x0086d005`
- `0x0086d201`

### TSL PE Entry Direct Internal Calls
- `0x00924dcc`
- `0x009270e0`
- `0x0091d3fb`
- `0x00924b53`
- `0x0092a3b0`
- `0x00925fde`
- `0x0091cbfa`
- `0x0092af50`
- `0x0092ae95`
- `0x0092ac1d`
- `0x0091ccb9`
- `0x0092abbe`
- `0x00407920`
- `0x0091ce6a`
- `0x0091ce96`
- `0x00924e11`
- `0x0092b087`

### TSL Startup Loader Direct Internal Calls (sample)
- `0x00402510`
- `0x0091c630`
- `0x004763b0`
- `0x00919723`
- `0x00735c60`
- `0x00733570`
- `0x007360e0`
- `0x00733780`
- `0x00401730`
- `0x004087a0`
- `0x004072b0`
- `0x004072f0`
- `0x004076f0`
- `0x0040c090`
- `0x0040c100`
- `0x00736240`
- `0x00733540`
- `0x00736320`
- `0x0091cf18`
- `0x00734270`
- `0x00736350`
- `0x00408c30`
- `0x0073fad0`
- `0x007362c0`
- `0x0073f980`
- `0x0073fe40`
- `0x00478da0`
- `0x00740c30`
- `0x0073f050`
- `0x0073f230`
- `0x0091c860`
- `0x0091cb24`
- `0x00409750`
- `0x00740160`
- `0x00408740`
- `0x004766b0`
- `0x004766e0`
- `0x00736220`
- `0x0073f9c0`
- `0x0051c470`

## Current Interpretation

- The imported-symbol map remains the most reliable cross-binary mapping layer.
- Internal startup recovery is now working when driven from the PE header `AddressOfEntryPoint` via explicit disassembly and function creation.
- `0x00407920` is a valid startup loader in TSL, but the same VA in K1 is not a clean equivalent and currently decompiles as overlapped/bad instruction data.
- The next productive step is to continue recursively recovering functions from the K1 and TSL startup frontiers and compare their call topology rather than assuming identical addresses.

AgentDecompile status: Partially completed - Missing TSL address for some K1 startup helpers, TODO find it :(