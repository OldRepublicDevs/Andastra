meta:
  id: ifo
  title: BioWare IFO (Module Info) File Format
  license: MIT
  endian: le
  file-extension: ifo
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/ifo.py
    reone: vendor/reone/src/libs/resource/parser/gff/ifo.cpp
    xoreos: vendor/xoreos/src/aurora/ifofile.cpp
    wiki: vendor/PyKotor/wiki/GFF-IFO.md
    bioware: vendor/PyKotor/wiki/Bioware-Aurora-IFO.md
doc: |
  IFO (Module Info) files are GFF-based format files that store module information including
  entry points, script hooks, area lists, and module metadata. IFO files use the GFF (Generic File Format)
  binary structure with file type signature "IFO ".
  
  IFO files contain:
  - Root struct with module metadata (Mod_ID, Mod_Name, Mod_Tag, Mod_Version, etc.)
  - Entry configuration (Mod_Entry_Area, Mod_Entry_X/Y/Z, Mod_Entry_Dir_X/Y)
  - Area list (Mod_Area_list): List of areas in the module
  - Script hooks (Mod_OnModLoad, Mod_OnModStart, Mod_OnHeartbeat, etc.)
  - Time settings (Mod_DawnHour, Mod_DuskHour, Mod_StartMonth/Day/Hour/Year)
  - Expansion pack requirements (Expansion_Pack, Mod_MinGameVer)
  - HAK file lists (Mod_Hak, Mod_HakList)
  - Module description and version information
  
  Each area in Mod_Area_list contains:
  - Area_Name (ResRef): Area ResRef (ARE file)
  - ObjectId (DWord, savegame only): ObjectID of the area
  
  References:
  - vendor/PyKotor/wiki/GFF-IFO.md
  - vendor/PyKotor/wiki/Bioware-Aurora-IFO.md
  - vendor/PyKotor/wiki/GFF-File-Format.md
  - vendor/reone/src/libs/resource/parser/gff/ifo.cpp
  - vendor/xoreos/src/aurora/ifofile.cpp

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
          File type signature. Must be "IFO " for module info files.
          Other GFF types: "GFF ", "ARE ", "UTC ", "UTI ", "DLG ", etc.
        valid: "IFO "
      
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
        doc: Array of 16-byte null-terminated field name labels
  
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
          Other structs have programmer-defined IDs:
          - StructID 6: Area list entry (Mod_Area_list)
          - StructID 7: Token entry (Mod_Tokens)
          - StructID 8: HAK list entry (Mod_HakList)
          - StructID 9: Cached script entry (Mod_CacheNSSList)
          - StructID 0: Variable table entry (Mod_VarTable)
          - StructID 43981: Event queue entry
          - StructID 48813: Player list entry
          - StructID 13634816: TURD (Temporary User Resource Data) entry
          - StructID 47787: Personal reputation entry
          - StructID 43962: Reputation list entry
      
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
        enum: gff_field_type
        doc: |
          Field data type (see gff_field_type enum):
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
        doc: Raw field data bytes for complex types
  
  # Field Indices Array (MultiMap)
  field_indices_array:
    seq:
      - id: indices
        type: u4
        repeat: expr
        repeat-expr: _root.gff_header.field_indices_count
        doc: Array of field indices (uint32 values) for structs with multiple fields
  
  # List Indices Array
  list_indices_array:
    seq:
      - id: entries
        type: list_entry
        repeat: until
        repeat-until: _io.pos >= (_root.gff_header.list_indices_offset + _root.gff_header.list_indices_count * 4)
        doc: |
          Array of list entries. Each entry starts with a count, followed by that many struct indices.
          Entries are accessed via field_entry.list_indices_offset_value offsets.
  
  list_entry:
    seq:
      - id: count
        type: u4
        doc: Number of struct indices in this list
      - id: struct_indices
        type: u4
        repeat: expr
        repeat-expr: count
        doc: Array of struct indices (indices into struct_array)

enums:
  gff_field_type:
    0: uint8
    doc: 8-bit unsigned integer (byte)
    1: int8
    doc: 8-bit signed integer (char)
    2: uint16
    doc: 16-bit unsigned integer (word)
    3: int16
    doc: 16-bit signed integer (short)
    4: uint32
    doc: 32-bit unsigned integer (dword)
    5: int32
    doc: 32-bit signed integer (int)
    6: uint64
    doc: 64-bit unsigned integer (stored in field_data)
    7: int64
    doc: 64-bit signed integer (stored in field_data)
    8: single
    doc: 32-bit floating point (float)
    9: double
    doc: 64-bit floating point (stored in field_data)
    10: string
    doc: Null-terminated string (CExoString, stored in field_data)
    11: resref
    doc: Resource reference (ResRef, max 16 chars, stored in field_data)
    12: localized_string
    doc: Localized string (CExoLocString, stored in field_data)
    13: binary
    doc: Binary data blob (Void, stored in field_data)
    14: struct
    doc: Nested struct (struct index stored inline)
    15: list
    doc: List of structs (offset to list_indices stored inline)
    16: vector4
    doc: Quaternion/Orientation (4×float, stored in field_data as Vector4)
    17: vector3
    doc: 3D vector (3×float, stored in field_data)

