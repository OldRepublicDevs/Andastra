meta:
  id: nfo
  title: BioWare NFO (Save Metadata) File Format
  license: MIT
  endian: le
  file-extension: nfo
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/extract/savedata.py
    reone: vendor/reone/src/libs/resource/parser/gff/nfo.cpp
    xoreos: vendor/xoreos/src/aurora/gff3file.cpp
doc: |
  NFO (Save Metadata) files are GFF-based format files that store save game metadata for Odyssey Engine
  games (KotOR and KotOR 2). NFO files use the GFF (Generic File Format) binary structure with
  file type signature "NFO ".

  NFO files are typically named "savenfo.res" and contain save game information displayed in the
  save game list UI, including:
  - Save name, area name, module name
  - Play time and save timestamp
  - Player character name and portrait
  - Save slot number
  - Story hints and live content flags
  - Cheat usage flag

  NFO files are stored alongside savegame.sav (ERF archive) files in the SAVES directory.
  The savegame.sav file contains the actual game state (global variables, party state, area states).

  NFO Root Struct Fields:
  - AREANAME (String): Current area name for display (e.g., "Endar Spire")
  - LASTMODULE (String): Last module ResRef (e.g., "m01aa_end")
  - SAVEGAMENAME (String): User-entered save name displayed in UI
  - TIMEPLAYED (UInt32): Total play time in seconds
  - TIMESTAMP (UInt64, optional): Windows FILETIME structure for save creation time
  - CHEATUSED (UInt8/Byte): Flag indicating if cheats were used (0 = no, 1 = yes)
  - GAMEPLAYHINT (UInt8/Byte): Gameplay hint flag
  - STORYHINT (UInt8/Byte, legacy): Legacy single-byte story hint field
  - STORYHINT0-9 (UInt8/Byte each): Per-index story hint flags (10 boolean flags)
  - PORTRAIT0-2 (ResRef): Player character portrait resource names (3 portrait slots)
  - LIVECONTENT (UInt8/Byte): Bitmask for live content flags
  - LIVE1-9 (String each): Live content entry strings (up to 9 entries)
  - PCNAME (String): Player character name
  - SAVENUMBER (Int32, optional): Save slot number

  Story Hints:
  - Some saves use legacy STORYHINT (single byte)
  - Newer saves use STORYHINT0-9 (10 separate byte fields)
  - Both may be present in the same file for compatibility

  Live Content:
  - LIVECONTENT bitmask indicates which live content entries are enabled
  - LIVE1-9 strings contain the actual live content entry names/identifiers

  References:
  - vendor/PyKotor/Libraries/PyKotor/src/pykotor/extract/savedata.py
  - Based on swkotor2.exe: SerializeSaveNfo @ 0x004eb750
  - Located via string reference: "savenfo" @ 0x007be1f0

seq:
  - id: gff_header
    type: gff_header
    doc: GFF file header (56 bytes)

  - id: label_array
    type: label_array
    if: gff_header.label_count > 0
    pos: gff_header.label_array_offset
    doc: Array of field name labels (16-byte null-terminated strings)

  - id: struct_array
    type: struct_array
    if: gff_header.struct_count > 0
    pos: gff_header.struct_array_offset
    doc: Array of struct entries (12 bytes each)

  - id: field_array
    type: field_array
    if: gff_header.field_count > 0
    pos: gff_header.field_array_offset
    doc: Array of field entries (12 bytes each)

  - id: field_data
    type: field_data_section
    if: gff_header.field_data_count > 0
    pos: gff_header.field_data_offset
    doc: Field data section for complex types (strings, ResRefs, LocalizedStrings, etc.)

  - id: field_indices
    type: field_indices_array
    if: gff_header.field_indices_count > 0
    pos: gff_header.field_indices_offset
    doc: Field indices array (MultiMap) for structs with multiple fields

  - id: list_indices
    type: list_indices_array
    if: gff_header.list_indices_count > 0
    pos: gff_header.list_indices_offset
    doc: List indices array for LIST type fields (not typically used in NFO)

types:
  # GFF Header (56 bytes)
  gff_header:
    seq:
      - id: file_type
        type: str
        encoding: ASCII
        size: 4
        doc: |
          File type signature. Must be "NFO " for save metadata files.
        valid: "NFO "

      - id: file_version
        type: str
        encoding: ASCII
        size: 4
        doc: |
          File format version. Typically "V3.2" for KotOR.
          Other versions: "V3.3", "V4.0", "V4.1" for other BioWare games.
        valid: ["V3.2", "V3.3", "V4.0", "V4.1"]

      - id: struct_array_offset
        type: u4
        doc: Byte offset to struct array from the beginning of the file

      - id: struct_count
        type: u4
        doc: Number of structs in the struct array

      - id: field_array_offset
        type: u4
        doc: Byte offset to field array from the beginning of the file

      - id: field_count
        type: u4
        doc: Number of fields in the field array

      - id: label_array_offset
        type: u4
        doc: Byte offset to label array from the beginning of the file

      - id: label_count
        type: u4
        doc: Number of labels in the label array

      - id: field_data_offset
        type: u4
        doc: Byte offset to field data section from the beginning of the file

      - id: field_data_count
        type: u4
        doc: Size of field data section in bytes

      - id: field_indices_offset
        type: u4
        doc: Byte offset to field indices array from the beginning of the file

      - id: field_indices_count
        type: u4
        doc: Number of field indices (uint32 values) in the field indices array

      - id: list_indices_offset
        type: u4
        doc: Byte offset to list indices array from the beginning of the file

      - id: list_indices_count
        type: u4
        doc: Number of list indices (uint32 values) in the list indices array

  # Label Array
  label_array:
    seq:
      - id: labels
        type: label_entry
        repeat: expr
        repeat-expr: _root.gff_header.label_count
        doc: Array of label entries (16 bytes each)

  label_entry:
    seq:
      - id: name
        type: str
        encoding: ASCII
        size: 16
        doc: |
          Field name label (null-padded to 16 bytes, null-terminated).
          The actual label length is determined by the first null byte.
          Common NFO field names: "AREANAME", "LASTMODULE", "SAVEGAMENAME", "TIMEPLAYED",
          "TIMESTAMP", "CHEATUSED", "GAMEPLAYHINT", "STORYHINT", "STORYHINT0" through "STORYHINT9",
          "PORTRAIT0", "PORTRAIT1", "PORTRAIT2", "LIVECONTENT", "LIVE1" through "LIVE9",
          "PCNAME", "SAVENUMBER".
    instances:
      name_trimmed:
        value: name.rstrip('\x00')
        doc: "Label name with trailing nulls removed"

  # Struct Array
  struct_array:
    seq:
      - id: entries
        type: struct_entry
        repeat: expr
        repeat-expr: _root.gff_header.struct_count
        doc: Array of struct entries (12 bytes each)

  struct_entry:
    seq:
      - id: struct_id
        type: s4
        doc: |
          Structure type identifier.
          Root struct always has struct_id = 0xFFFFFFFF (-1).
          NFO files typically only have the root struct (no nested structs).

      - id: data_or_offset
        type: u4
        doc: |
          If field_count = 1: Direct field index into field_array.
          If field_count > 1: Byte offset into field_indices array.
          If field_count = 0: Unused (empty struct).

      - id: field_count
        type: u4
        doc: Number of fields in this struct (0, 1, or >1)
    instances:
      has_single_field:
        value: field_count == 1
        doc: True if struct has exactly one field (direct field index in data_or_offset)
      has_multiple_fields:
        value: field_count > 1
        doc: True if struct has multiple fields (offset to field indices in data_or_offset)
      single_field_index:
        value: data_or_offset
        if: has_single_field
        doc: Direct field index when struct has exactly one field
      field_indices_offset:
        value: data_or_offset
        if: has_multiple_fields
        doc: Byte offset into field_indices_array when struct has multiple fields

  # Field Array
  field_array:
    seq:
      - id: entries
        type: field_entry
        repeat: expr
        repeat-expr: _root.gff_header.field_count
        doc: Array of field entries (12 bytes each)

  field_entry:
    seq:
      - id: field_type
        type: u4
        doc: |
          GFF field data type (enum value):
          - 0: UInt8/Byte
          - 1: Int8/Char
          - 2: UInt16/Word
          - 3: Int16/Short
          - 4: UInt32/DWord
          - 5: Int32/Int
          - 6: UInt64/DWord64
          - 7: Int64/Int64
          - 8: Float/Single
          - 9: Double
          - 10: String/CExoString
          - 11: ResRef
          - 12: LocalizedString/CExoLocString
          - 13: Binary/Void
          - 14: Struct (nested struct)
          - 15: List (list of structs)
          - 16: Vector4/Orientation (quaternion)
          - 17: Vector3/Vector

      - id: label_index
        type: u4
        doc: Index into label_array for field name

      - id: data_or_offset
        type: u4
        doc: |
          For simple types: Direct data value or offset into field_data section.
          For complex types: Byte offset into field_data section.
          For Struct type: Struct index into struct_array.
          For List type: Byte offset into list_indices_array.
    instances:
      label_name:
        value: _root.label_array.labels[label_index].name_trimmed
        doc: Field name from label array

  # Field Data Section
  field_data_section:
    seq:
      - id: data
        type: u1
        repeat: expr
        repeat-expr: _root.gff_header.field_data_count
        doc: Raw field data bytes (strings, ResRefs, binary data, etc.)

  # Field Indices Array
  field_indices_array:
    seq:
      - id: indices
        type: u4
        repeat: expr
        repeat-expr: _root.gff_header.field_indices_count
        doc: Array of field indices (uint32 values) for structs with multiple fields

  # List Indices Array (typically empty for NFO)
  list_indices_array:
    seq:
      - id: entries
        type: list_entry
        repeat: until
        repeat-until: _io.pos >= (_root.gff_header.list_indices_offset + _root.gff_header.list_indices_count)
        doc: List entry structures (count + struct indices)

  list_entry:
    seq:
      - id: count
        type: u4
        doc: Number of struct indices in this list entry

      - id: struct_indices
        type: u4
        repeat: expr
        repeat-expr: count
        doc: Array of struct indices (uint32) pointing to struct_array entries

