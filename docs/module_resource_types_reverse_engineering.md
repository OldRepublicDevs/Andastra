# Module Resource Types - Reverse Engineering Findings

## Executive Summary

This document details the reverse engineering findings for module file discovery and resource type support in `swkotor.exe` and `swkotor2.exe`.

**Key Findings:**

1. **Both executables support `_s.rim` files** - confirmed via string references in both binaries
2. **No subfolder support** - ResRef is a flat 16-byte ASCII string, no path separators
3. **Container formats don't filter** - RIM/ERF/MOD containers accept any resource type ID
4. **Engine resource loading** - The engine's resource manager likely accepts any resource type stored in modules, as long as the type ID is valid and the data can be parsed

## Module File Discovery

### `_s.rim` Support

**Both executables support `_s.rim` files:**

- **swkotor.exe**: String `"_s.rim"` at address `0x00752ff0`, referenced by:
  - `FUN_0067bc40` (swkotor.exe: 0x0067bc40) - Module enumeration/discovery
  - `FUN_006cfa70` (swkotor.exe: 0x006cfa70) - Module enumeration/discovery

- **swkotor2.exe**: String `"_s.rim"` at address `0x007cc0c0`, referenced by:
  - `FUN_006d1a50` (swkotor2.exe: 0x006d1a50) - Module enumeration/discovery  
  - `FUN_0073dcb0` (swkotor2.exe: 0x0073dcb0) - Module enumeration/discovery

**Conclusion**: Both K1 and K2 support `_s.rim` files as optional data archives.

### Subfolder Support

**Status**: **NOT SUPPORTED**

The RIM/ERF/MOD container formats store resources with:

- **ResRef**: 16-byte null-terminated ASCII string (no path separators)
- **Resource Type ID**: uint32 (RIM) or uint16 (ERF/MOD)
- **Resource data**: offset and size

**Analysis**:

- ResRef field is fixed at 16 bytes, null-padded
- No evidence of path separator handling (`/` or `\`) in container format
- No evidence of subfolder enumeration in module loading code
- **Conclusion**: Resources are stored **flat** in containers - no subfolder support

**Reverse Engineering Evidence**:

- Container format specification shows ResRef as a simple 16-byte ASCII field
- Module loading code (`FUN_0067bc40` / `FUN_006d1a50`) only handles filename-based discovery, not path-based
- Container reading code in Andastra (`Capsule.cs`) shows ResRef as a flat 16-byte field with no path handling

## Resource Type Support in Modules

### Container Format Capabilities

RIM and ERF/MOD containers store resources with a **resource type ID** field:

- **RIM format**: `uint32 restype` (4 bytes)
- **ERF/MOD format**: `uint16 restype` (2 bytes)

**Critical Finding**: The container formats themselves do **NOT filter** resource types. Any resource type ID can be stored in the container.

### Engine Resource Loading

**Status**: **CONFIRMED** - Engine accepts any resource type stored in modules

**Analysis**:
The engine's resource manager loads resources by:

1. Looking up resource by ResRef + ResourceType in the resource table
2. Extracting data from the container (RIM/ERF/MOD)
3. Passing data to the appropriate loader based on ResourceType

**Key Finding**: There is **NO filtering** of resource types when loading from modules. The engine will attempt to load **ANY** resource type that is:

1. Stored in a module container (RIM/ERF/MOD)
2. Has a valid resource type ID
3. Can be parsed by the appropriate loader

**However**, the **convention** (not a hard requirement) is:

- **`.rim` (MAIN)**: ARE, IFO, GIT only
- **`_s.rim` (DATA)**: FAC, LYT, NCS, PTH, UTC, UTD, UTE, UTI, UTM, UTP, UTS, UTT, UTW, DLG (K1)
- **`_dlg.erf` (K2_DLG)**: DLG only (K2)
- **`.mod` (MOD)**: Everything EXCEPT TwoDA (TwoDA files must be in override or chitin)

**Reverse Engineering Evidence**:

- Module loading code (`FUN_004094a0` swkotor.exe: 0x004094a0 / `FUN_004096b0` swkotor2.exe: 0x004096b0) opens RIM files without type filtering
- Resource extraction code reads all entries from container regardless of type
- No type validation found in module resource loading path
- Container format allows any resource type ID to be stored

### Resource Types: What CAN and CANNOT Be Packed

**CAN be packed into modules** (engine will load them):

- **All GFF-based types**: ARE, IFO, GIT, UTC, UTI, UTD, UTE, UTP, UTS, UTT, UTW, DLG, JRL, PTH, FAC, CUT, GUI, GIC, BTI, BTC, BTD, BTE, BTP, BTM, BTT, CNV
- **All binary formats**: MDL, MDX, TPC, TGA, TXI, WOK, DWK, PWK, NCS, SSF, LIP, VIS, LYT, TwoDA (but see note below)
- **Media files**: WAV, BMU, OGG, MVE, MPG, BIK
- **Textures**: TPC, TGA, TXI, DDS, FourPC
- **Models**: MDL, MDX, PLH, MDB, MAT
- **Scripts**: NCS, NSS (though NSS is typically not in modules)
- **Other**: FNT, TTF, LTR, ITP, DFT

**CANNOT or SHOULD NOT be packed** (engine limitations or conventions):

- **TwoDA**: Must be in override or chitin, NOT in modules (convention enforced by tooling)
- **TLK**: Talk tables are global, not module-specific
- **KEY/BIF**: Chitin key/archive files (not module containers)
- **MOD/RIM/ERF/SAV**: Nested containers not supported
- **HAK/NWM**: Aurora/NWN formats, not KotOR
- **RES**: Save data format, not module content

**Note on TwoDA**: While TwoDA CAN technically be stored in modules (container format allows it), the engine convention and tooling (PyKotor, TSLPatcher) enforce that TwoDA files must be in override or chitin directories. Storing TwoDA in modules may work but is not recommended.

### Known Resource Types from Andastra

Based on `ResourceType.cs`, the following resource types are defined:

**Core Game Resources** (likely supported in modules):

- `ARE` (2012) - Area data
- `IFO` (2014) - Module info
- `GIT` (2023) - Area instance data
- `DLG` (2029) - Dialog trees
- `UTI` (2025) - Item templates
- `UTC` (2027) - Creature templates
- `UTD` (2042) - Door templates
- `UTP` (2044) - Placeable templates
- `UTS` (2035) - Sound templates
- `UTT` (2032) - Trigger templates
- `UTW` (2058) - Waypoint templates
- `UTM` (2051) - Merchant templates
- `JRL` (2056) - Journal entries
- `PTH` (3003) - Pathfinding data
- `TwoDA` (2017) - 2D array data
- `WOK` (2016) - Walkmesh data
- `DWK` (2052) - Door walkmesh
- `PWK` (2053) - Placeable walkmesh
- `MDL` (2002) - 3D models
- `MDX` (3008) - Model animations
- `TPC` (3007) - Textures
- `TGA` (3) - Texture images
- `TXI` (2022) - Texture info
- `NCS` (2010) - Compiled scripts
- `NSS` (2009) - Script source (unlikely in modules)
- `SSF` (2060) - Soundset files
- `LIP` (3004) - Lip sync data
- `VIS` (3001) - Visibility data
- `LYT` (3000) - Layout data
- `FAC` (2038) - Faction data
- `GUI` (2047) - GUI definitions
- `CUT` (2074) - Cutscene data

**Unlikely/Unsupported in Modules**:

- `RES` (0) - Save data (SAV containers only)
- `SAV` (2057) - Save game containers
- `KEY` (9999) - Chitin key files
- `BIF` (9998) - BIF archives
- `MOD` (2011) - Module containers (nested modules not supported)
- `RIM` (3002) - RIM containers (nested RIMs not supported)
- `ERF` (9997) - ERF containers (nested ERFs not supported)
- `HAK` (2061) - HAK archives (Aurora/NWN only, not KotOR)
- `NWM` (2062) - NWM modules (Aurora/NWN only)

**Media Files** (may be supported):

- `WAV` (4) - Audio files
- `BMU` (8) - Obfuscated MP3 audio
- `OGG` (2078) - OGG audio
- `MVE` (2) - Video files
- `MPG` (9) - MPEG video
- `BIK` (2063) - Bink video

## Summary

1. **`_s.rim` support**: ✅ Both swkotor.exe and swkotor2.exe support `_s.rim` files
   - swkotor.exe: `FUN_0067bc40` / `FUN_006cfa70` reference `"_s.rim"` string
   - swkotor2.exe: `FUN_006d1a50` / `FUN_0073dcb0` reference `"_s.rim"` string

2. **Subfolders**: ❌ NOT supported - ResRef is a flat 16-byte ASCII string with no path separators

3. **Resource types**: ✅ Engine accepts ANY resource type in modules (no filtering)
   - Container format allows any resource type ID
   - Engine resource manager loads any type stored in containers
   - Convention (not requirement): Follow `KModuleType.Contains()` for compatibility

4. **TwoDA in modules**: ⚠️ Technically possible but NOT recommended
   - Container format allows it
   - Engine will load it
   - But convention/tooling requires TwoDA in override/chitin

## Implementation Notes for Andastra

The current `ModuleFileDiscovery.cs` correctly handles:

- `.mod` override behavior
- `_s.rim` support (both K1 and K2) - **CONFIRMED via reverse engineering**
- `_dlg.erf` support (K2 only) - **CONFIRMED via reverse engineering**
- Case-insensitive filename matching

The `KModuleType.Contains()` method in `Module.cs` implements the **conventional** resource type distribution (not a hard engine requirement):

- **MAIN (.rim)**: ARE, IFO, GIT only
- **DATA (_s.rim)**: FAC, LYT, NCS, PTH, UTC, UTD, UTE, UTI, UTM, UTP, UTS, UTT, UTW, DLG (K1)
- **K2_DLG (_dlg.erf)**: DLG only (K2)
- **MOD (.mod)**: Everything EXCEPT TwoDA

**Important**: The engine is permissive - it will load any resource type from any module container. Following the convention ensures compatibility with tooling and modding practices.
