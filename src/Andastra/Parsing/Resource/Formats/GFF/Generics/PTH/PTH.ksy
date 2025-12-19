meta:
  id: pth
  title: BioWare PTH (Pathfinding) File Format
  license: MIT
  endian: le
  file-extension: pth
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/pth.py
    reone: vendor/reone/src/libs/resource/parser/gff/pth.cpp
    xoreos: vendor/xoreos/src/aurora/pthfile.cpp
    wiki: vendor/PyKotor/wiki/GFF-File-Format.md
doc: |
  PTH (Pathfinding) files are GFF-based format files that store pathfinding data including
  waypoints and connections for NPC navigation. PTH files use the GFF (Generic File Format) 
  binary structure with file type signature "PTH ".
  
  PTH files contain:
  - Root struct with two LIST fields:
    - Path_Points: Array of PTHPoint structs (waypoint coordinates and connection metadata)
    - Path_Conections: Array of PTHConnection structs (edges connecting waypoints)
  
  Each PTHPoint struct contains:
  - Conections (UInt32): Number of outgoing connections from this point
  - First_Conection (UInt32): Index into Path_Conections list for first connection
  - X (Single/Float32): X coordinate of the waypoint
  - Y (Single/Float32): Y coordinate of the waypoint
  
  Each PTHConnection struct contains:
  - Destination (UInt32): Index of the target waypoint in Path_Points list
  
  The pathfinding graph is constructed by:
  1. Iterating through Path_Points to get waypoint positions
  2. For each point, using First_Conection and Conections to index into Path_Conections
  3. Reading Destination indices to build the graph edges
  
  References:
  - vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/pth.py:160-209
  - vendor/reone/src/libs/resource/parser/gff/pth.cpp
  - vendor/xoreos/src/aurora/pthfile.cpp
  - vendor/PyKotor/wiki/GFF-File-Format.md

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
          File type signature. Must be "PTH " for pathfinding files.
          Other GFF types: "GFF ", "ARE ", "UTC ", "UTI ", "DLG ", etc.
        valid: "PTH "
      
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
          Common PTH labels include:
          - "Path_Points" (root LIST field for waypoints)
          - "Path_Conections" (root LIST field for connections)
          - "Conections" (UInt32 field in PTHPoint struct)
          - "First_Conection" (UInt32 field in PTHPoint struct)
          - "X" (Single field in PTHPoint struct)
          - "Y" (Single field in PTHPoint struct)
          - "Destination" (UInt32 field in PTHConnection struct)
  
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
          PTHPoint structs typically have struct_id = 2.
          PTHConnection structs typically have struct_id = 3.
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
          
          PTH-specific field types:
          - Path_Points: field_type = 15 (List)
          - Path_Conections: field_type = 15 (List)
          - Conections: field_type = 4 (UInt32)
          - First_Conection: field_type = 4 (UInt32)
          - X: field_type = 8 (Single/Float32)
          - Y: field_type = 8 (Single/Float32)
          - Destination: field_type = 4 (UInt32)
      
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
          PTH files typically have minimal field_data since most fields are simple types
          (UInt32, Single) stored inline in field_entry.data_or_offset.
  
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
  
  # List Indices Array
  list_indices_array:
    seq:
      - id: indices
        type: u4
        repeat: expr
        repeat-expr: _root.gff_header.list_indices_count
        doc: |
          Array of list indices (uint32 values) for LIST type fields.
          For PTH files:
          - Path_Points LIST: Contains struct indices pointing to PTHPoint structs
          - Path_Conections LIST: Contains struct indices pointing to PTHConnection structs
          
          Each LIST field_entry.data_or_offset points to a byte offset in this array.
          The list indices are stored as consecutive uint32 values starting at that offset.

