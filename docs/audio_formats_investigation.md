# Audio Format Investigation - KotOR Games

## Summary

This document provides comprehensive information about audio file formats, obfuscation, resource types, and loading priority in KotOR games, based on reverse engineering analysis using Ghidra MCP.

---

## Audio Format Resource Types

### Supported Audio Formats

| Format | Resource Type ID | Extension | Handler Address | Module Support | Notes |
|--------|-----------------|-----------|-----------------|----------------|-------|
| **WAV** | 4 (0x4) | `.wav` | swkotor.exe: 0x005d5e90<br>swkotor2.exe: 0x00621ac0 | ✅ YES | Standard WAV format, supports obfuscation headers |
| **BMU** | 8 (0x8) | `.bmu` | ❌ NOT FOUND | ❓ UNKNOWN | MP3 with obfuscated extra header (per codebase comment) |
| **MPG** | 9 (0x9) | `.mpg` | ❌ NOT FOUND | ❓ UNKNOWN | Video format, may contain audio |
| **WMA** | 11 (0xB) | `.wma` | ❌ NOT FOUND | ❓ UNKNOWN | Windows Media Audio |
| **WMV** | 12 (0xC) | `.wmv` | ❌ NOT FOUND | ❓ UNKNOWN | Windows Media Video (may contain audio) |
| **XMV** | 13 (0xD) | `.xmv` | ❌ NOT FOUND | ❓ UNKNOWN | Xbox video format |
| **OGG** | 2078 (0x81e) | `.ogg` | ❌ NOT FOUND | ❌ NO | **NOT SUPPORTED** - No handler found in swkotor.exe or swkotor2.exe |
| **MP3** | 25014 | `.mp3` | ❌ NOT FOUND | ❌ NO | **Toolset-only** - Not a game resource type, only used by development tools |

### Audio Format Details

#### WAV (Resource Type 4)

**Status**: ✅ **FULLY SUPPORTED**

- **Handler**: `swkotor.exe: 0x005d5e90` (`LoadWAVAudio`)
- **Resource Search**: Uses `FUN_004074d0` with type 4, which searches all locations including modules
- **Module Support**: ✅ **YES** - Can be loaded from modules, override, and BIF files
- **Processing**: Uses Miles Sound System (`MSS32.DLL`) via `_AIL_WAV_info_8` for WAV file parsing

**Obfuscation Support**: ✅ **YES - WAV files support obfuscation headers**

WAV files in KotOR can have obfuscation headers that are automatically detected and skipped:

1. **SFX Header** (470 bytes / 0x1DA):
   - Magic bytes: `0xFF 0xF3 0x60 0xC4` at offset 0
   - Used for sound effects
   - Header is skipped, actual WAV data starts at offset 470

2. **VO Header** (20 bytes):
   - Magic bytes: `"RIFF"` (0x52 0x49 0x46 0x46) at offset 0
   - Used for voice-over files
   - If "RIFF" appears again at offset 20, it's a VO header
   - Header is skipped, actual WAV data starts at offset 20

3. **MP3-in-WAV** (58 bytes):
   - Magic bytes: `"RIFF"` at offset 0
   - RIFF size field (bytes 4-8) equals 50 (0x32)
   - Contains MP3 audio wrapped in WAV container
   - Header is skipped, actual MP3 data starts at offset 58

4. **Standard WAV** (0 bytes header):
   - Standard RIFF/WAVE format
   - No obfuscation header
   - Starts with `"RIFF"` magic

**Deobfuscation**: The game automatically detects and skips obfuscation headers. Both obfuscated and deobfuscated WAV files work:
- ✅ **Obfuscated WAV**: Game detects header type and skips it automatically
- ✅ **Deobfuscated WAV**: Standard RIFF/WAVE format works directly

**Reference Implementation**: 
- `src/Andastra/Parsing/Resource/Formats/WAV/WAVObfuscation.cs`
- `vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/wav/wav_obfuscation.py`
- `vendor/reone/src/libs/audio/format/wavreader.cpp:34`

#### BMU (Resource Type 8)

**Status**: ❓ **UNKNOWN - No handler found**

- **Description**: MP3 with obfuscated extra header (per codebase comment)
- **Handler**: ❌ **NOT FOUND** in swkotor.exe or swkotor2.exe
- **Module Support**: ❓ **UNKNOWN** - No handler means no module loading capability
- **Usage**: May be Xbox-specific or unused format

#### OGG (Resource Type 2078 / 0x81e)

**Status**: ❌ **NOT SUPPORTED**

- **Handler**: ❌ **NOT FOUND** in swkotor.exe or swkotor2.exe
- **Module Support**: ❌ **NO** - No handler exists
- **Conclusion**: OGG is **NOT a supported audio format** in KotOR games. The resource type ID exists in the type registry, but no actual handler processes OGG files.

#### MP3 (Resource Type 25014)

**Status**: ❌ **NOT A GAME RESOURCE TYPE**

- **Handler**: ❌ **NOT FOUND**
- **Usage**: **Toolset-only** - Used by development tools, not loaded by the game
- **Note**: MP3 audio can appear in WAV files via "MP3-in-WAV" format (see WAV section above)

---

## Audio Loading Priority

### WAV Audio Loading

**Function**: `FUN_005d5e90` (swkotor.exe: 0x005d5e90) - WAV resource loader

**Loading Process**:
1. Searches resource system using `FUN_004074d0` with type 4 (WAV)
2. Creates WAV audio object (60 bytes = 0x3c allocation)
3. If `loadImmediately != 0`: Calls `FUN_00408620` to load data immediately
4. WAV data is processed by Miles Sound System library

**Priority Chain**: **NO AUTOMATIC PRIORITY CHAIN FOUND**

Unlike textures (TGA → TPC), **audio formats do NOT have an automatic priority fallback system**. The game loads the specific format requested:

- If code requests WAV (type 4), it only searches for WAV files
- If code requests BMU (type 8), it would search for BMU (but no handler exists)
- **No automatic fallback** from WAV → OGG → MP3

**Module Support**: ✅ **YES** - WAV files can be placed in modules and will be found through the standard resource search mechanism.

### Audio File Priority (Same Name, Different Formats)

**Question**: If two files with the same name exist but different formats (e.g., `sound.wav` and `sound.ogg`), which takes priority?

**Answer**: **NO AUTOMATIC PRIORITY - Format is explicit**

The game does **NOT** automatically check multiple audio formats. The loading code explicitly requests a specific resource type:

- Code requesting WAV will **only** find `.wav` files (type 4)
- Code requesting OGG will **only** find `.ogg` files (type 2078) - but no handler exists
- **No fallback chain** like textures have

**Conclusion**: Audio format selection is **explicit** - the game code must request the specific format. There is no automatic format fallback system for audio.

---

## WAV Obfuscation Details

### Are All WAV Files Obfuscated?

**Answer**: ❌ **NO - Not all WAV files are obfuscated**

WAV files can exist in multiple states:

1. **Obfuscated SFX** (470-byte header) - Common for sound effects
2. **Obfuscated VO** (20-byte header) - Common for voice-over files
3. **MP3-in-WAV** (58-byte header) - MP3 audio wrapped in WAV
4. **Standard WAV** (no header) - Standard RIFF/WAVE format

**Detection**: The game automatically detects the format by checking:
- First 4 bytes for magic numbers (`0xFF 0xF3 0x60 0xC4` for SFX, `"RIFF"` for others)
- RIFF size field for MP3-in-WAV detection
- Presence of "RIFF" at offset 20 for VO header detection

**Support**: The game supports **both obfuscated and deobfuscated** WAV files:
- ✅ **Obfuscated**: Automatically detected and header skipped
- ✅ **Deobfuscated**: Standard WAV format works directly

### Obfuscation Header Sizes

| Type | Header Size | Magic Bytes | Usage |
|------|-------------|-------------|-------|
| SFX | 470 bytes (0x1DA) | `0xFF 0xF3 0x60 0xC4` | Sound effects |
| VO | 20 bytes (0x14) | `"RIFF"` at 0, `"RIFF"` at 20 | Voice-over files |
| MP3-in-WAV | 58 bytes (0x3A) | `"RIFF"` + size=50 | MP3 wrapped in WAV |
| Standard | 0 bytes | `"RIFF"` | Standard WAV format |

### What Can You Modify in WAV Files?

**Modifiable Elements**:
1. **Audio Data**: Change the actual audio samples (after deobfuscation)
2. **Obfuscation Headers**: Add/remove/modify obfuscation headers
3. **Format**: Convert between obfuscated and deobfuscated formats

**Effects in Game**:
- Audio content changes will be audible in-game
- Obfuscation headers are automatically handled - both formats work
- Must maintain valid WAV/RIFF format or game may crash/not play audio
- Header modifications are transparent to the game (auto-detected)

---

## Audio Loading Functions

### WAV Loader

**Function**: `FUN_005d5e90` (swkotor.exe: 0x005d5e90)
```c
void __thiscall LoadWAVAudio(
    void* this,
    int* resourceName,
    int loadImmediately
)
```

**Process**:
1. Validates resource name
2. Searches resource cache via `FUN_004074d0` with type 4
3. If not cached, creates WAV object (0x3c bytes) via `FUN_005df1b0`
4. Registers in resource cache via `FUN_00407680`
5. If `loadImmediately != 0`, loads data via `FUN_00408620`
6. WAV data processed by Miles Sound System (`_AIL_WAV_info_8`)

### Audio Playback

**Function**: `FUN_005db4d0` (swkotor.exe: 0x005db4d0) - Audio playback/streaming
- Handles audio playback via Miles Sound System
- Processes WAV data (with automatic obfuscation detection)
- Manages 3D positional audio and streaming

---

## Module Support Summary

| Format | Module Support | Override Support | BIF Support |
|--------|----------------|------------------|-------------|
| **WAV** | ✅ YES | ✅ YES | ✅ YES |
| **BMU** | ❓ UNKNOWN | ❓ UNKNOWN | ❓ UNKNOWN |
| **OGG** | ❌ NO | ❌ NO | ❌ NO |
| **MP3** | ❌ NO | ❌ NO | ❌ NO |

**Conclusion**: Only **WAV** format is fully supported and can be loaded from modules. Other audio formats either have no handler (OGG, MP3) or unknown status (BMU).

---

## Priority Chain Summary

### Audio Format Priority

**NO AUTOMATIC PRIORITY CHAIN EXISTS**

Unlike textures (TGA → TPC), audio formats do not have an automatic fallback system. The game explicitly requests specific formats:

- **WAV requests** → Only searches for WAV (type 4)
- **OGG requests** → Would search for OGG (type 2078) but no handler exists
- **MP3 requests** → Would search for MP3 but no handler exists

**No automatic fallback**: WAV → OGG → MP3 does NOT exist.

### Resource Location Priority (Applies to All Formats)

When loading audio (or any resource), the game searches in this order:

1. **Override Directory** - Highest priority
2. **Module Containers** (.mod files)
3. **Module RIM Files** (.rim, _s.rim, etc.)
4. **Chitin Archives** (BIF files) - Lowest priority

This priority applies to **all resource types**, including audio.

---

## Recommendations

### For Modders

1. **Use WAV format** - Only fully supported audio format
2. **Obfuscation is optional** - Both obfuscated and deobfuscated WAV files work
3. **Module placement works** - WAV files can be placed in modules
4. **Avoid OGG/MP3** - Not supported by the game engine

### For Developers

1. **WAV Support**: Implement WAV loader with obfuscation detection
2. **OGG/MP3**: Not needed for KotOR compatibility - formats are not supported
3. **BMU**: Investigate further if Xbox compatibility is needed
4. **Obfuscation**: Support both obfuscated and deobfuscated formats

---

## References

- **Ghidra Analysis**: `swkotor.exe` and `swkotor2.exe` reverse engineering
- **Resource Type Definitions**: `src/Andastra/Parsing/Resource/ResourceType.cs`
- **WAV Obfuscation**: `src/Andastra/Parsing/Resource/Formats/WAV/WAVObfuscation.cs`
- **PyKotor Reference**: `vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/wav/wav_obfuscation.py`
- **Reone Reference**: `vendor/reone/src/libs/audio/format/wavreader.cpp`
- **Module Resource Types**: `docs/module_resource_types_reverse_engineering.md`

---

## Conclusion

- **WAV**: Fully supported, supports obfuscation (SFX: 470 bytes, VO: 20 bytes, MP3-in-WAV: 58 bytes), can be in modules
- **OGG**: NOT supported - no handler exists
- **MP3**: NOT a game resource type - toolset-only
- **BMU**: Unknown - no handler found
- **Priority**: NO automatic audio format fallback chain exists
- **Obfuscation**: Both obfuscated and deobfuscated WAV files work - game auto-detects headers

