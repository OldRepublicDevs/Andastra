meta:
  id: utw
  title: BioWare UTW (Waypoint Template) File Format
  license: MIT
  endian: le
  file-extension: utw
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/utw.py
    reone: vendor/reone/src/libs/resource/parser/gff/utw.cpp
    wiki: vendor/PyKotor/wiki/GFF-UTW.md
doc: |
  UTW (User Template Waypoint) files are GFF-based format files that define waypoint templates.
  Waypoints are invisible markers used for spawn points, navigation targets, map notes, and
  reference points for scripts. UTW files use the GFF (Generic File Format) binary structure
  with file type signature "UTW ".

  UTW files contain:
  - Root struct with waypoint metadata:
    - TemplateResRef: Template identifier for this waypoint (ResRef)
    - Tag: Unique tag for script/linking references (String)
    - LocalizedName: Waypoint name displayed in game (LocalizedString)
    - Description: Description text (LocalizedString, typically unused)
    - Comment: Developer comment/notes (String, not used by game engine)
  - Map Note Functionality:
    - HasMapNote: Whether waypoint has a map note (Byte, boolean)
    - MapNoteEnabled: Map note is initially visible (Byte, boolean)
    - MapNote: Text displayed on map (LocalizedString)
  - Linking & Appearance:
    - LinkedTo: Tag of linked object (String, typically unused)
    - Appearance: Appearance type (Byte, 1=Waypoint)
    - PaletteID: Toolset palette category (Byte)

  Waypoints are used for:
  - Spawn Points: CreateObject uses waypoint location
  - Patrols: AI walks between waypoints
  - Teleport: JumpToLocation targets waypoints
  - Transitions: Doors/Triggers link to waypoint tags
  - Map Notes: Display quest objectives and locations on map

  References:
  - vendor/PyKotor/wiki/GFF-UTW.md
  - vendor/PyKotor/wiki/GFF-File-Format.md
  - vendor/PyKotor/wiki/Bioware-Aurora-Waypoint.md
  - vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/utw.py
  - vendor/reone/src/libs/resource/parser/gff/utw.cpp

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
    pos: gff_header.struct_array_offset
    doc: Array of struct entries (12 bytes each)

  - id: field_array
    type: field_array
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
    doc: List indices array for LIST type fields

types:
  # GFF Header (56 bytes)
  gff_header:
    seq:
      - id: file_type
        type: str
        encoding: ASCII
        size: 4
        doc: |
          File type signature. Must be "UTW " for waypoint template files.
          Other GFF types: "GFF ", "DLG ", "ARE ", "UTC ", "UTI ", "UTM ", etc.
        valid: "UTW "

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
        type: str
        encoding: ASCII
        size: 16
        repeat: expr
        repeat-expr: _root.gff_header.label_count
        doc: |
          Array of 16-byte null-terminated field name labels.
          Common UTW labels include:
          - "TemplateResRef" (ResRef field in root struct)
          - "Tag" (String field in root struct)
          - "LocalizedName" (LocalizedString field in root struct)
          - "Description" (LocalizedString field in root struct)
          - "Comment" (String field in root struct)
          - "HasMapNote" (Byte field in root struct)
          - "MapNoteEnabled" (Byte field in root struct)
          - "MapNote" (LocalizedString field in root struct)
          - "LinkedTo" (String field in root struct)
          - "Appearance" (Byte field in root struct)
          - "PaletteID" (Byte field in root struct)

  # Struct Array
  struct_array:
    seq:
      - id: entries
        type: struct_entry
        repeat: expr
        repeat-expr: _root.gff_header.struct_count
        doc: Array of struct entries

  struct_entry:
    seq:
      - id: struct_id
        type: s4
        doc: |
          Structure type identifier.
          Root struct always has struct_id = 0xFFFFFFFF (-1).
          Other structs have programmer-defined IDs.

      - id: data_or_offset
        type: u4
        doc: |
          If field_count = 1: Direct field index into field_array.
          If field_count > 1: Byte offset into field_indices array.
          If field_count = 0: Unused (empty struct).

      - id: field_count
        type: u4
        doc: Number of fields in this struct (0, 1, or >1)

  # Field Array
  field_array:
    seq:
      - id: entries
        type: field_entry
        repeat: expr
        repeat-expr: _root.gff_header.field_count
        doc: Array of field entries

  field_entry:
    seq:
      - id: field_type
        type: u4
        doc: |
          Field data type (see GFFFieldType enum):
          0 = Byte (UInt8)
          1 = Char (Int8)
          2 = UInt16
          3 = Int16
          4 = UInt32
          5 = Int32
          6 = UInt64
          7 = Int64
          8 = Single (Float32)
          9 = Double (Float64)
          10 = CExoString (String)
          11 = ResRef
          12 = CExoLocString (LocalizedString)
          13 = Void (Binary)
          14 = Struct
          15 = List
          16 = Vector3
          17 = Vector4

          UTW-specific field types:
          - TemplateResRef: field_type = 11 (ResRef)
          - Tag: field_type = 10 (String)
          - LocalizedName: field_type = 12 (LocalizedString)
          - Description: field_type = 12 (LocalizedString)
          - Comment: field_type = 10 (String)
          - HasMapNote: field_type = 0 (Byte)
          - MapNoteEnabled: field_type = 0 (Byte)
          - MapNote: field_type = 12 (LocalizedString)
          - LinkedTo: field_type = 10 (String)
          - Appearance: field_type = 0 (Byte)
          - PaletteID: field_type = 0 (Byte)

      - id: label_index
        type: u4
        doc: Index into label_array for field name

      - id: data_or_offset
        type: u4
        doc: |
          For simple types (Byte, Char, UInt16, Int16, UInt32, Int32, UInt64, Int64, Single, Double):
            Inline data value (stored directly in this field)
          For complex types (String, ResRef, LocalizedString, Binary, Vector3, Vector4):
            Byte offset into field_data section
          For Struct type:
            Struct index into struct_array
          For List type:
            Byte offset into list_indices array

  # Field Data Section
  field_data_section:
    seq:
      - id: data
        type: str
        size: _root.gff_header.field_data_count
        doc: |
          Raw field data bytes for complex types.
          UTW files store:
          - ResRef data: 1-byte length + up to 16 bytes of ASCII string
          - String data: 4-byte length + string bytes (ASCII/UTF-8)
          - LocalizedString data: Variable size (see GFF.ksy localized_string_data type)
            Contains string reference ID, language count, and language-specific substrings

  # Field Indices Array (MultiMap)
  field_indices_array:
    seq:
      - id: indices
        type: u4
        repeat: expr
        repeat-expr: _root.gff_header.field_indices_count
        doc: |
          Array of field indices (uint32 values) for structs with multiple fields.
          Used when a struct has more than one field - the struct_entry.data_or_offset
          points to an offset in this array, which contains the field indices.
          UTW root struct typically has multiple fields, so this array is used.

  # List Indices Array
  list_indices_array:
    seq:
      - id: indices
        type: u4
        repeat: expr
        repeat-expr: _root.gff_header.list_indices_count
        doc: |
          Array of list indices (uint32 values) for LIST type fields.
          UTW files typically do not contain LIST fields in the root struct,
          but this array exists for compatibility with GFF format.
