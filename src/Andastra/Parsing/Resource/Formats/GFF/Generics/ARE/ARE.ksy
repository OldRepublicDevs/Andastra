meta:
  id: are
  title: BioWare ARE (Area) File Format
  license: MIT
  endian: le
  file-extension: are
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/are.py
    reone: vendor/reone/src/libs/resource/parser/gff/are.cpp
    xoreos: vendor/xoreos/src/aurora/arefile.cpp
    wiki: vendor/PyKotor/wiki/GFF-File-Format.md
    bioware_docs: vendor/PyKotor/wiki/Bioware-Aurora-AreaFile.md
doc: |
  ARE (Area) files are GFF-based format files that store static area information including
  lighting, fog, grass, weather, script hooks, and map data. ARE files use the GFF (Generic File Format)
  binary structure with file type signature "ARE ".

  ARE files are used across multiple BioWare game engines:
  - Odyssey Engine (KotOR 1 & 2): Uses ARE files for static area properties
  - Aurora Engine (Neverwinter Nights): Uses ARE files with additional NWN-specific fields
  - Eclipse Engine (Dragon Age, Mass Effect): Uses ARE files for area definitions

  ARE File Structure:
  ==================
  ARE files follow the standard GFF structure with these sections:
  1. GFF Header (56 bytes): Contains file signature, version, and offsets to all other sections
  2. Label Array: Contains field name labels (16-byte null-terminated ASCII strings)
  3. Struct Array: Contains struct entries (12 bytes each) defining the tree structure
  4. Field Array: Contains field entries (12 bytes each) defining data fields
  5. Field Data Section: Contains complex data (strings, ResRefs, LocalizedStrings, vectors, binary)
  6. Field Indices Array: Contains field index arrays for structs with multiple fields
  7. List Indices Array: Contains list structures for LIST type fields

  ARE Root Struct Fields:
  ======================
  The root struct (struct_id = -1) contains the following fields (by label name):

  Core Identity Fields:
  - "Tag" (String): Unique area identifier used for scripting
  - "Name" (LocalizedString): Area name displayed in-game and toolset
  - "Comments" (String): Developer notes/documentation (toolset only)
  - "Creator_ID" (Int32): Toolset creator identifier (deprecated, unused at runtime)
  - "ID" (Int32): Unique area ID (deprecated, unused at runtime)
  - "Version" (Int32): Area version number (increments on each save)
  - "Flags" (UInt32): Area flags (deprecated in KotOR, unused)

  Lighting & Sun Fields:
  - "SunAmbientColor" (UInt32): Ambient light color RGB (BGR format in binary)
  - "SunDiffuseColor" (UInt32): Sun diffuse light color RGB (BGR format in binary)
  - "SunShadows" (Byte): Enable shadow rendering (0 = off, 1 = on)
  - "ShadowOpacity" (Byte/UInt8): Shadow opacity (0-255, or ResRef in some implementations)
  - "DynAmbientColor" (UInt32): Dynamic ambient light RGB (BGR format in binary)

  Fog Settings:
  - "SunFogOn" (Byte): Enable fog rendering (0 = off, 1 = on)
  - "SunFogNear" (Float): Fog start distance in world units
  - "SunFogFar" (Float): Fog end distance in world units
  - "SunFogColor" (UInt32): Fog color RGB (BGR format in binary)

  Moon Lighting (Unused in KotOR):
  - "MoonAmbientColor" (UInt32): Moon ambient light color (unused)
  - "MoonDiffuseColor" (UInt32): Moon diffuse light color (unused)
  - "MoonFogOn" (Byte): Moon fog toggle (unused)
  - "MoonFogNear" (Float): Moon fog start distance (unused)
  - "MoonFogFar" (Float): Moon fog end distance (unused)
  - "MoonFogColor" (UInt32): Moon fog color (unused)
  - "MoonShadows" (Byte): Moon shadows toggle (unused)
  - "IsNight" (Byte): Always night flag (unused)

  Grass Rendering:
  - "Grass_TexName" (ResRef): Grass texture name (TPC/TGA file, max 16 chars)
  - "Grass_Density" (Float): Grass blade density (0.0-1.0, controls coverage)
  - "Grass_QuadSize" (Float): Size of grass patches in world units
  - "Grass_Prob_LL" (Float): Spawn probability lower-left corner (0.0-1.0)
  - "Grass_Prob_LR" (Float): Spawn probability lower-right corner (0.0-1.0)
  - "Grass_Prob_UL" (Float): Spawn probability upper-left corner (0.0-1.0)
  - "Grass_Prob_UR" (Float): Spawn probability upper-right corner (0.0-1.0)
  - "Grass_Ambient" (UInt32): Grass ambient color RGB (BGR format in binary)
  - "Grass_Diffuse" (UInt32): Grass diffuse color RGB (BGR format in binary)
  - "Grass_Emissive" (UInt32): Grass emissive color RGB (KotOR 2 only, BGR format in binary)

  Weather System (KotOR 2 only):
  - "ChanceRain" (Int32): Rain probability (0-100 percent)
  - "ChanceSnow" (Int32): Snow probability (0-100 percent)
  - "ChanceLightning" (Int32): Lightning probability (0-100 percent)

  Dirty/Dust Settings (KotOR 2 only):
  - "DirtyARGBOne" (UInt32): First dust color ARGB (BGR format in binary)
  - "DirtySizeOne" (Float): First dust particle size
  - "DirtyFormulaOne" (Int32): First dust formula type
  - "DirtyFuncOne" (Int32): First dust function
  - "DirtyARGBTwo" (UInt32): Second dust color ARGB (BGR format in binary)
  - "DirtySizeTwo" (Float): Second dust particle size
  - "DirtyFormulaTwo" (Int32): Second dust formula type
  - "DirtyFuncTwo" (Int32): Second dust function
  - "DirtyARGBThree" (UInt32): Third dust color ARGB (BGR format in binary)
  - "DirtySizeThree" (Float): Third dust particle size
  - "DirtyFormulaThre" (Int32): Third dust formula type (note: "Thre" not "Three")
  - "DirtyFuncThree" (Int32): Third dust function

  Environment & Camera:
  - "DefaultEnvMap" (ResRef): Default environment map texture (cubemap for reflections)
  - "CameraStyle" (Int32): Camera behavior type (defines zoom, rotation, collision)
  - "AlphaTest" (Int32/Byte): Alpha testing threshold
  - "WindPower" (Int32): Wind strength (0 = still, 1 = weak, 2 = strong)
  - "LightingScheme" (Int32/Byte): Lighting scheme identifier (index into environment.2da, unused)

  Area Behavior Flags:
  - "Unescapable" (Byte): Cannot use save/travel functions (0 = escapable, 1 = unescapable)
  - "DisableTransit" (Byte): Cannot travel to other modules (0 = transit allowed, 1 = disabled)
  - "StealthXPEnabled" (Byte): Award stealth XP (0 = disabled, 1 = enabled)
  - "StealthXPLoss" (Int32): Stealth detection XP penalty
  - "StealthXPMax" (Int32): Maximum stealth XP per area

  Skill Check Modifiers (Unused):
  - "ModSpotCheck" (Int32): Awareness skill modifier (unused)
  - "ModListenCheck" (Int32): Listen skill modifier (unused)

  Script Hooks:
  - "OnEnter" (ResRef): Script executed when entering area (max 16 chars)
  - "OnExit" (ResRef): Script executed when leaving area (max 16 chars)
  - "OnHeartbeat" (ResRef): Script executed periodically (every 6 seconds, max 16 chars)
  - "OnUserDefined" (ResRef): Script executed on user-defined events (max 16 chars)
  - "OnEnter2" (ResRef): Secondary OnEnter script (if present, max 16 chars)
  - "OnExit2" (ResRef): Secondary OnExit script (if present, max 16 chars)
  - "OnHeartbeat2" (ResRef): Secondary OnHeartbeat script (if present, max 16 chars)
  - "OnUserDefined2" (ResRef): Secondary OnUserDefined script (if present, max 16 chars)

  Minimap/Map Struct:
  - "Map" (Struct): Nested struct containing map coordinate mapping data
    The Map struct contains:
    - "NorthAxis" (Int32): North direction orientation (0 = PositiveY, 1 = NegativeY, 2 = PositiveX, 3 = NegativeX)
    - "MapZoom" (Int32): Map zoom level
    - "MapResX" (Int32): Map texture resolution X dimension (typically 435 pixels)
    - "MapPt1X" (Float): First map point X coordinate (normalized 0.0-1.0, image space)
    - "MapPt1Y" (Float): First map point Y coordinate (normalized 0.0-1.0, image space)
    - "MapPt2X" (Float): Second map point X coordinate (normalized 0.0-1.0, image space)
    - "MapPt2Y" (Float): Second map point Y coordinate (normalized 0.0-1.0, image space)
    - "WorldPt1X" (Float): First world point X coordinate (world units, walkmesh space)
    - "WorldPt1Y" (Float): First world point Y coordinate (world units, walkmesh space)
    - "WorldPt2X" (Float): Second world point X coordinate (world units, walkmesh space)
    - "WorldPt2Y" (Float): Second world point Y coordinate (world units, walkmesh space)

    The Map struct defines a coordinate transformation between the minimap texture (lbl_map<resname>.tpc)
    and the 3D world space walkmesh. This allows the game to:
    1. Display the player's position on the minimap
    2. Render map notes at correct locations
    3. Overlay the minimap texture on the walkmesh in editors

  Lists:
  - "Rooms" (List): List of room definitions for audio zones and minimap regions
    Each room struct contains:
    - "RoomName" (String): Room identifier (referenced by VIS files)
    - "EnvAudio" (Int32): Environment audio index for room acoustics
    - "AmbientScale" (Float): Ambient audio volume scaling factor
    - "DisableWeather" (Byte): Disable weather effects in this room (KotOR 2 only, 0 = weather enabled, 1 = disabled)
    - "ForceRating" (Int32): Force rating modifier for this room (KotOR 2 only)

  - "AreaList" (List): List of area names (typically empty or unused)
  - "MapList" (List): List of map ResRefs (typically empty or unused)

  Aurora/NWN-Specific Fields:
  - "EnvAudio" (Int32): Environment audio ID (Aurora/NWN only, index into soundeax.2da)
  - "DisplayName" (String): Display name (Aurora/NWN only)
  - "MoonFogColor" (UInt32): Moon fog color (Aurora/NWN only, BGR format)
  - "MoonFogAmount" (Byte): Moon fog amount (Aurora/NWN only, 0-15)
  - "MoonAmbientColor" (UInt32): Moon ambient color (Aurora/NWN only, BGR format)
  - "MoonDiffuseColor" (UInt32): Moon diffuse color (Aurora/NWN only, BGR format)

  Deprecated Fields (Not used by game engines):
  - "LoadScreenID" (UInt16): Index into loadscreens.2da (unused)
  - "DayNightCycle" (Byte): Day/night cycle flag (unused)
  - "NoRest" (Byte): Rest restriction flag (unused)
  - "NoHangBack" (Byte): Hang back restriction flag (unused)
  - "PlayerOnly" (Byte): Player-only area flag (unused)
  - "PlayerVsPlayer" (Byte): PvP settings index (unused)

  Color Format (BGR):
  ==================
  Color values in ARE files are stored as 32-bit integers in BGR (Blue-Green-Red) format, not RGB.
  The bytes within a BGR DWORD are arranged as 0BGR, where 0 indicates the first byte is 0x00.
  Since GFF integer data is stored in little-endian byte order, the bytes on disk appear as R G B 0.

  Example: Color RGB(255, 128, 64) = #FF8040 in RGB hex
  - Blue = 64 (0x40)
  - Green = 128 (0x80)
  - Red = 255 (0xFF)
  - BGR DWORD = 0x00FF8040
  - On disk (little-endian): 40 80 FF 00

  References:
  ==========
  - vendor/PyKotor/wiki/GFF-File-Format.md - GFF format documentation
  - vendor/PyKotor/wiki/GFF-ARE.md - ARE-specific field documentation
  - vendor/PyKotor/wiki/Bioware-Aurora-AreaFile.md - Official BioWare ARE format specification
  - vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/are.py - PyKotor ARE implementation
  - vendor/reone/src/libs/resource/parser/gff/are.cpp - Reone ARE parsing implementation
  - vendor/xoreos/src/aurora/arefile.cpp - Xoreos ARE implementation
  - src/Andastra/Parsing/Resource/Formats/GFF/Generics/ARE.cs - Andastra ARE class
  - src/Andastra/Parsing/Resource/Formats/GFF/Generics/AREHelpers.cs - Andastra ARE helper functions

seq:
  - id: gff_header
    type: gff_header
    doc: |
      GFF file header (56 bytes total, 14 fields * 4 bytes each).
      Contains file signature, version, and offsets to all other sections.

  - id: label_array
    type: label_array
    if: gff_header.label_count > 0
    pos: gff_header.label_array_offset
    doc: |
      Array of field name labels (16-byte null-terminated ASCII strings).
      Each label identifies a field in the ARE root struct or nested structs.
      Common labels: "Tag", "Name", "Map", "Rooms", "SunAmbientColor", etc.

  - id: struct_array
    type: struct_array
    pos: gff_header.struct_array_offset
    doc: |
      Array of struct entries (12 bytes each: 4 bytes struct_id + 4 bytes data_or_offset + 4 bytes field_count).
      ARE files typically contain:
      - Root struct (struct_id = -1) containing all area properties
      - Map struct (struct_id varies) containing map coordinate data
      - Room structs (struct_id varies) for each room in the Rooms list

  - id: field_array
    type: field_array
    pos: gff_header.field_array_offset
    doc: |
      Array of field entries (12 bytes each: 4 bytes field_type + 4 bytes label_index + 4 bytes data_or_offset).
      Each field entry references a label by index and contains either inline data or an offset to complex data.

  - id: field_data
    type: field_data_section
    if: gff_header.field_data_count > 0
    pos: gff_header.field_data_offset
    doc: |
      Field data section for complex types stored as byte sequences.
      Contains:
      - Strings (CExoString): 4-byte length + string bytes (ASCII)
      - ResRefs: 1-byte length + string bytes (ASCII, max 16 chars, null-padded)
      - LocalizedStrings (CExoLocString): Variable-length structure with string reference and language-specific strings
      - Vector3: 12 bytes (3×float, X/Y/Z coordinates)
      - Vector4: 16 bytes (4×float, quaternion/orientation)
      - Binary: 4-byte length + binary bytes

  - id: field_indices
    type: field_indices_array
    if: gff_header.field_indices_count > 0
    pos: gff_header.field_indices_offset
    doc: |
      Field indices array (MultiMap) for structs with multiple fields.
      When a struct has more than 1 field, it stores an offset into this array,
      and the next N consecutive u4 values (where N = struct.field_count) are the field indices.

  - id: list_indices
    type: list_indices_array
    if: gff_header.list_indices_count > 0
    pos: gff_header.list_indices_offset
    doc: |
      List indices array for LIST type fields.
      Each list entry starts with a count (u4), followed by that many struct indices (u4 each).
      ARE files use lists for:
      - "Rooms": List of room structs (audio zones, weather regions)
      - "AreaList": List of area names (typically empty)
      - "MapList": List of map ResRefs (typically empty)

types:
  # GFF Header (56 bytes total: 14 fields * 4 bytes each)
  gff_header:
    seq:
      - id: file_type
        type: str
        encoding: ASCII
        size: 4
        doc: |
          File type signature (FourCC). Must be "ARE " for area files (space-padded).
          Other GFF types: "GFF ", "DLG ", "UTC ", "UTI ", "GIT ", etc.
        valid: "ARE "

      - id: file_version
        type: str
        encoding: ASCII
        size: 4
        doc: |
          File format version string. Must be one of the valid GFF versions.
          - "V3.2": KotOR 1 & 2, Neverwinter Nights
          - "V3.3": Later BioWare games
          - "V4.0": Dragon Age: Origins, Dragon Age 2
          - "V4.1": Mass Effect series
        valid: ["V3.2", "V3.3", "V4.0", "V4.1"]

      - id: struct_array_offset
        type: u4
        doc: |
          Byte offset to struct array from the beginning of the file.
          Typically 56 (immediately after header) or larger if label array comes first.

      - id: struct_count
        type: u4
        doc: |
          Number of structs in the struct array.
          ARE files typically have 1 root struct + 1 Map struct + N room structs (if Rooms list exists).
          Minimum: 1 (just root struct), but ARE files usually have at least 2 (root + Map).

      - id: field_array_offset
        type: u4
        doc: Byte offset to field array from the beginning of the file

      - id: field_count
        type: u4
        doc: |
          Number of fields in the field array.
          ARE root struct typically has 30-50+ fields depending on game version and optional fields.

      - id: label_array_offset
        type: u4
        doc: |
          Byte offset to label array from the beginning of the file.
          Label array typically comes before struct/field arrays for efficient parsing.

      - id: label_count
        type: u4
        doc: |
          Number of labels in the label array.
          Each unique field name used in the ARE file has one label entry.
          ARE files typically have 30-50+ unique field names.

      - id: field_data_offset
        type: u4
        doc: |
          Byte offset to field data section from the beginning of the file.
          Field data section contains all complex types (strings, ResRefs, LocalizedStrings, vectors, binary).

      - id: field_data_count
        type: u4
        doc: |
          Size of field data section in bytes.
          Contains all string, ResRef, LocalizedString, Vector3, Vector4, and binary data.

      - id: field_indices_offset
        type: u4
        doc: |
          Byte offset to field indices array from the beginning of the file.
          Used when structs have multiple fields (field_count > 1).

      - id: field_indices_count
        type: u4
        doc: |
          Number of field indices (uint32 values) in the field indices array.
          Total count across all structs with multiple fields.
          ARE root struct typically has 30-50+ fields, so this is usually equal to or greater than field_count.

      - id: list_indices_offset
        type: u4
        doc: |
          Byte offset to list indices array from the beginning of the file.
          Used for LIST type fields like "Rooms", "AreaList", "MapList".

      - id: list_indices_count
        type: u4
        doc: |
          Number of list indices (uint32 values) in the list indices array.
          Includes count fields + struct indices for all LIST entries.
          ARE files typically have 1-3 lists (Rooms, AreaList, MapList).

  # Label Array
  label_array:
    seq:
      - id: labels
        type: str
        encoding: ASCII
        size: 16
        repeat: expr
        repeat-expr: _root.gff_header.label_count
        doc: |
          Array of 16-byte null-terminated field name labels.
          Each label is padded to 16 bytes with null bytes.
          The actual label length is determined by the first null byte.

          Common ARE field labels include:
          - Core: "Tag", "Name", "Comments", "Version"
          - Lighting: "SunAmbientColor", "SunDiffuseColor", "DynAmbientColor", "SunFogColor"
          - Fog: "SunFogOn", "SunFogNear", "SunFogFar"
          - Grass: "Grass_TexName", "Grass_Density", "Grass_QuadSize", "Grass_Prob_LL", etc.
          - Weather: "ChanceRain", "ChanceSnow", "ChanceLightning" (KotOR 2 only)
          - Scripts: "OnEnter", "OnExit", "OnHeartbeat", "OnUserDefined"
          - Map: "Map" (struct), "NorthAxis", "MapZoom", "MapResX", "MapPt1X", etc.
          - Rooms: "Rooms" (list)
          - Flags: "Unescapable", "DisableTransit", "StealthXPEnabled"

  # Struct Array
  struct_array:
    seq:
      - id: entries
        type: struct_entry
        repeat: expr
        repeat-expr: _root.gff_header.struct_count
        doc: |
          Array of struct entries defining the tree structure.
          Each struct entry is 12 bytes and defines one node in the GFF tree.
          ARE files use a tree structure:
          - Root struct (struct_id = -1) at index 0
          - Map struct (nested under root) with arbitrary struct_id
          - Room structs (in Rooms list) with arbitrary struct_id values

  struct_entry:
    seq:
      - id: struct_id
        type: s4
        doc: |
          Structure type identifier (signed 32-bit integer).
          Root struct always has struct_id = 0xFFFFFFFF (-1).
          Other structs have programmer-defined IDs.
          ARE files typically have:
          - Root struct: struct_id = -1 (always)
          - Map struct: struct_id = arbitrary (usually 0 or small positive number)
          - Room structs: struct_id = arbitrary (usually 0 or small positive number)

      - id: data_or_offset
        type: u4
        doc: |
          Field index or offset (unsigned 32-bit integer).
          Interpretation depends on field_count:
          - If field_count = 0: Unused (empty struct, value typically -1 or 0)
          - If field_count = 1: Direct field index into field_array (single field)
          - If field_count > 1: Byte offset into field_indices_array (multiple fields)

          ARE root struct typically has field_count > 1, so this is an offset into field_indices_array.
          Map struct and Room structs may have field_count = 1 or > 1 depending on number of fields.

      - id: field_count
        type: u4
        doc: |
          Number of fields in this struct (unsigned 32-bit integer).
          - 0: No fields (empty struct)
          - 1: Single field (data_or_offset contains direct field index)
          - >1: Multiple fields (data_or_offset contains byte offset into field_indices_array)

          ARE root struct typically has 30-50+ fields.
          Map struct typically has 10-11 fields (NorthAxis, MapZoom, MapResX, MapPt1X/Y, MapPt2X/Y, WorldPt1X/Y, WorldPt2X/Y).
          Room structs typically have 3-5 fields (RoomName, EnvAudio, AmbientScale, DisableWeather, ForceRating).

  # Field Array
  field_array:
    seq:
      - id: entries
        type: field_entry
        repeat: expr
        repeat-expr: _root.gff_header.field_count
        doc: |
          Array of field entries defining data fields.
          Each field entry is 12 bytes and defines one field in a struct.
          Fields are referenced by structs through the field_indices_array.

  field_entry:
    seq:
      - id: field_type
        type: u4
        enum: gff_field_type
        doc: |
          Field data type (unsigned 32-bit integer, see gff_field_type enum).
          Determines how data_or_offset is interpreted and how data is stored.

          ARE files use the following field types:
          - Byte (0/UInt8): Boolean flags (Unescapable, DisableTransit, StealthXPEnabled, SunFogOn, SunShadows)
          - UInt16 (2): LoadScreenID (if present)
          - UInt32 (4): Color values (SunAmbientColor, Grass_Ambient, etc. in BGR format)
          - Int32 (5): Numeric values (AlphaTest, CameraStyle, WindPower, StealthXPLoss/Max, DirtyFormulaOne/Two/Thre, NorthAxis, MapZoom, MapResX, EnvAudio, ForceRating)
          - Single/Float (8): Float values (fog distances, grass properties, map coordinates: SunFogNear/Far, Grass_Density, MapPt1X/Y, WorldPt1X/Y, AmbientScale)
          - String (10/CExoString): Text values (Tag, Comments, RoomName, DisplayName)
          - ResRef (11): Resource references (DefaultEnvMap, Grass_TexName, script hooks OnEnter/OnExit/OnHeartbeat/OnUserDefined)
          - LocalizedString (12/CExoLocString): Localized text (Name)
          - Struct (14): Nested struct (Map struct)
          - List (15): List of structs (Rooms, AreaList, MapList)

      - id: label_index
        type: u4
        doc: |
          Index into label_array for field name (unsigned 32-bit integer).
          Used to look up the field's label name (e.g., "Tag", "Name", "Map", "Rooms").
          Must be in range [0, label_count).

      - id: data_or_offset
        type: u4
        doc: |
          Inline data or offset (unsigned 32-bit integer).
          Interpretation depends on field_type:

          Simple types (stored inline, 1-4 bytes sign/zero-extended to 4 bytes):
          - Byte (0): Value stored directly (0 or 1 for booleans)
          - UInt16 (2): Value stored directly (LoadScreenID)
          - UInt32 (4): Value stored directly (color values in BGR format)
          - Int32 (5): Value stored directly (numeric values)
          - Single/Float (8): Value stored directly (IEEE 754 single precision)

          Complex types (offset into field_data section, relative to field_data_offset):
          - String (10): Byte offset to string data (4-byte length + string bytes)
          - ResRef (11): Byte offset to ResRef data (1-byte length + string bytes, max 16 chars)
          - LocalizedString (12): Byte offset to LocalizedString data (variable-length structure)
          - Vector3 (17): Byte offset to Vector3 data (12 bytes: 3×float)
          - Vector4 (16): Byte offset to Vector4 data (16 bytes: 4×float)
          - Binary (13): Byte offset to binary data (4-byte length + binary bytes)

          Reference types:
          - Struct (14): Struct index into struct_array (Map struct reference)
          - List (15): Byte offset into list_indices_array (relative to list_indices_offset)

  # Field Data Section
  field_data_section:
    seq:
      - id: data
        type: str
        size: _root.gff_header.field_data_count
        doc: |
          Raw field data bytes for complex types.
          Individual field data entries are accessed via offsets stored in field_entry.data_or_offset.
          The structure of each entry depends on the field_type:

          String (CExoString, type 10):
          - 4 bytes: Length (u4, number of bytes)
          - Length bytes: String data (ASCII encoding, no null terminator)

          ResRef (type 11):
          - 1 byte: Length (u1, number of characters, max 16)
          - Length bytes: String data (ASCII encoding, null-padded to 16 bytes in label storage, but only Length bytes here)

          LocalizedString (CExoLocString, type 12):
          - 4 bytes: Total size (u4, size of rest of structure)
          - 4 bytes: String reference (u4, StrRef into dialog.tlk, 0xFFFFFFFF = -1 if none)
          - 4 bytes: String count (u4, number of language-specific strings)
          - For each string:
            - 4 bytes: String ID (u4, encodes language and gender: bits 0-7 = gender, bits 8-15 = language)
            - 4 bytes: String length (u4, number of bytes)
            - Length bytes: String data (UTF-8 encoding, typically)

          Vector3 (type 17):
          - 4 bytes: X coordinate (f4, IEEE 754 single precision, little-endian)
          - 4 bytes: Y coordinate (f4, IEEE 754 single precision, little-endian)
          - 4 bytes: Z coordinate (f4, IEEE 754 single precision, little-endian)
          - Total: 12 bytes

          Vector4 (type 16):
          - 4 bytes: X component (f4, IEEE 754 single precision, little-endian)
          - 4 bytes: Y component (f4, IEEE 754 single precision, little-endian)
          - 4 bytes: Z component (f4, IEEE 754 single precision, little-endian)
          - 4 bytes: W component (f4, IEEE 754 single precision, little-endian)
          - Total: 16 bytes

          Binary (Void, type 13):
          - 4 bytes: Length (u4, number of bytes)
          - Length bytes: Binary data

  # Field Indices Array (MultiMap)
  field_indices_array:
    seq:
      - id: indices
        type: u4
        repeat: expr
        repeat-expr: _root.gff_header.field_indices_count
        doc: |
          Array of field indices (uint32 values) for structs with multiple fields.
          When a struct has field_count > 1, it stores an offset into this array.
          The next N consecutive u4 values (where N = struct.field_count) are the field indices
          that belong to that struct. Each field index is an index into field_array.

          Example: If root struct has field_count = 35 and data_or_offset = 0,
          then indices[0] through indices[34] are the field indices for the root struct.

  # List Indices Array
  list_indices_array:
    seq:
      - id: indices
        type: u4
        repeat: expr
        repeat-expr: _root.gff_header.list_indices_count
        doc: |
          Array of list indices (uint32 values) for LIST type fields.
          List entries are accessed via offsets stored in list-type field entries.
          Each entry starts with a count (u4), followed by that many struct indices (u4 each).

          Example: If "Rooms" field has data_or_offset = 0 and contains 3 rooms:
          - indices[0] = 3 (count)
          - indices[1] = struct index of first room
          - indices[2] = struct index of second room
          - indices[3] = struct index of third room

          Note: This is a raw data block. In practice, list entries are accessed via
          offsets stored in list-type field entries, not as a sequential array.
          Use list_entry type to parse individual entries at specific offsets.

  # Helper type for parsing individual list entries (not used in main seq, but documented for reference)
  list_entry:
    seq:
      - id: count
        type: u4
        doc: |
          Number of struct indices in this list (unsigned 32-bit integer).
          For ARE "Rooms" list, this is the number of room structs.
      - id: struct_indices
        type: u4
        repeat: expr
        repeat-expr: count
        doc: |
          Array of struct indices (unsigned 32-bit integers).
          Each struct index references a struct in struct_array.
          For ARE "Rooms" list, each struct index references a room struct.

enums:
  gff_field_type:
    0: uint8
    doc: |
      8-bit unsigned integer (Byte).
      Used in ARE files for: Unescapable, DisableTransit, StealthXPEnabled, SunFogOn, SunShadows,
      ShadowOpacity (in some implementations), DisableWeather (room structs, KotOR 2 only).
      Values: 0 = false/off, 1 = true/on (for boolean fields).
    1: int8
    doc: 8-bit signed integer (Char). Rarely used in ARE files.
    2: uint16
    doc: |
      16-bit unsigned integer (Word).
      Used in ARE files for: LoadScreenID (index into loadscreens.2da, if present).
    3: int16
    doc: 16-bit signed integer (Short). Rarely used in ARE files.
    4: uint32
    doc: |
      32-bit unsigned integer (DWord).
      Used in ARE files for: Color values in BGR format (SunAmbientColor, SunDiffuseColor, DynAmbientColor,
      SunFogColor, Grass_Ambient, Grass_Diffuse, Grass_Emissive, DirtyARGBOne/Two/Three).
      Note: Colors are stored in BGR format, not RGB. See main documentation for details.
    5: int32
    doc: |
      32-bit signed integer (Int).
      Used in ARE files for: AlphaTest, CameraStyle, WindPower, StealthXPLoss, StealthXPMax,
      DirtyFormulaOne, DirtyFormulaTwo, DirtyFormulaThre, DirtyFuncOne, DirtyFuncTwo, DirtyFuncThree,
      NorthAxis (Map struct), MapZoom (Map struct), MapResX (Map struct),
      ChanceRain, ChanceSnow, ChanceLightning (KotOR 2 only),
      EnvAudio (room structs, Aurora/NWN), ForceRating (room structs, KotOR 2 only),
      Version, Creator_ID, ID, Flags, ModSpotCheck, ModListenCheck (deprecated fields).
    6: uint64
    doc: 64-bit unsigned integer (QWord). Not used in ARE files.
    7: int64
    doc: 64-bit signed integer (Long). Not used in ARE files.
    8: single
    doc: |
      32-bit floating point (Float, IEEE 754 single precision).
      Used in ARE files for: SunFogNear, SunFogFar, MoonFogNear, MoonFogFar (if present),
      Grass_Density, Grass_QuadSize, Grass_Prob_LL, Grass_Prob_LR, Grass_Prob_UL, Grass_Prob_UR,
      DirtySizeOne, DirtySizeTwo, DirtySizeThree (KotOR 2 only),
      MapPt1X, MapPt1Y, MapPt2X, MapPt2Y (Map struct, normalized 0.0-1.0),
      WorldPt1X, WorldPt1Y, WorldPt2X, WorldPt2Y (Map struct, world coordinates),
      AmbientScale (room structs).
    9: double
    doc: 64-bit floating point (Double, IEEE 754 double precision). Not used in ARE files.
    10: string
    doc: |
      Null-terminated string (CExoString).
      Used in ARE files for: Tag (area identifier), Comments (developer notes),
      RoomName (room structs), DisplayName (Aurora/NWN only).
      Format: 4-byte length (u4) + length bytes (ASCII encoding, no null terminator).
    11: resref
    doc: |
      Resource reference (ResRef, max 16 characters).
      Used in ARE files for: DefaultEnvMap (environment map texture),
      Grass_TexName (grass texture), script hooks (OnEnter, OnExit, OnHeartbeat, OnUserDefined,
      OnEnter2, OnExit2, OnHeartbeat2, OnUserDefined2).
      Format: 1-byte length (u1, max 16) + length bytes (ASCII encoding).
    12: localized_string
    doc: |
      Localized string (CExoLocString).
      Used in ARE files for: Name (area name, displayed in-game and toolset).
      Format: Variable-length structure with string reference (StrRef) and language-specific strings.
      See field_data_section documentation for detailed structure.
    13: binary
    doc: |
      Binary data blob (Void).
      Rarely used in ARE files. Format: 4-byte length (u4) + length bytes.
    14: struct
    doc: |
      Nested struct.
      Used in ARE files for: Map (nested struct containing map coordinate data).
      Format: data_or_offset contains struct index into struct_array.
    15: list
    doc: |
      List of structs.
      Used in ARE files for: Rooms (list of room structs for audio zones and weather regions),
      AreaList (list of area names, typically empty), MapList (list of map ResRefs, typically empty).
      Format: data_or_offset contains byte offset into list_indices_array.
      Each list entry starts with count (u4) followed by struct indices (u4 each).
    16: vector4
    doc: |
      Quaternion/Orientation (4×float, Vector4).
      Not commonly used in ARE files. Format: 16 bytes (4×f4, little-endian).
    17: vector3
    doc: |
      3D vector (3×float, Vector3).
      Not commonly used in ARE files directly, but Vector2 is used conceptually for map coordinates
      (stored as two separate Float fields: MapPt1X/Y, WorldPt1X/Y).
      Format: 12 bytes (3×f4, little-endian) if present.
