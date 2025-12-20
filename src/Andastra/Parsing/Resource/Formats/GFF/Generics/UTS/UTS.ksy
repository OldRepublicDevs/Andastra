meta:
  id: uts
  title: BioWare UTS (Sound) File Format
  license: MIT
  endian: le
  file-extension: uts
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/uts.py
    reone: vendor/reone/src/libs/resource/parser/gff/uts.cpp
    wiki: vendor/PyKotor/wiki/GFF-File-Format.md
doc: |
  UTS (Sound) files are GFF-based format files that store sound object definitions including
  audio settings, positioning, looping, and volume controls. UTS files use the GFF (Generic File Format)
  binary structure with file type signature "UTS ".
  
  UTS files contain:
  - Root struct with sound configuration fields:
    - Tag (string): Sound object identifier/tag name
    - TemplateResRef (ResRef): Template resource reference
    - LocName (LocalizedString): Localized name of the sound object
    - Active (UInt8): Whether sound is active (0=no, 1=yes)
    - Continuous (UInt8): Whether sound plays continuously (0=no, 1=yes)
    - Looping (UInt8): Whether sound loops (0=no, 1=yes)
    - Positional (UInt8): Whether sound is positional (0=no, 1=yes)
    - RandomPosition (UInt8): Whether to use random position (0=no, 1=yes)
    - Random (UInt8): Whether to play sounds randomly (0=no, 1=yes)
    - Volume (UInt8): Volume level (0-127)
    - VolumeVrtn (UInt8): Volume variance
    - PitchVariation (Single): Pitch variation amount
    - Elevation (Single): Sound elevation in 3D space
    - MinDistance (Single): Minimum distance for positional audio
    - MaxDistance (Single): Maximum distance for positional audio
    - DistanceCutoff (Single): Distance cutoff for positional audio
    - Priority (UInt8): Sound playback priority
    - Hours (UInt32): Time of day restriction (deprecated in some games)
    - Times (UInt8): Number of times to play (deprecated in some games)
    - Interval (UInt32): Interval between playbacks (milliseconds)
    - IntervalVrtn (UInt32): Interval variance (milliseconds)
    - Sound (ResRef): Primary sound resource reference
    - Comment (string): Developer comment
    - RandomRangeX (Single): Random position range on X axis
    - RandomRangeY (Single): Random position range on Y axis
    - PaletteID (UInt8): Palette identifier
    - Sounds (List): Array of sound entry structs for random/playlist sounds
  
  Each sound entry in Sounds list contains:
  - Sound (ResRef): Sound resource reference
  
  References:
  - vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/uts.py
  - vendor/reone/src/libs/resource/parser/gff/uts.cpp
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
          File type signature. Must be "UTS " for sound files.
          Other GFF types: "GFF ", "DLG ", "ARE ", "UTC ", "UTI ", etc.
        valid: "UTS "
      
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
          Common UTS labels include:
          - "Tag" (string field in root struct)
          - "TemplateResRef" (ResRef field in root struct)
          - "LocName" (LocalizedString field in root struct)
          - "Active" (UInt8 field in root struct)
          - "Continuous" (UInt8 field in root struct)
          - "Looping" (UInt8 field in root struct)
          - "Positional" (UInt8 field in root struct)
          - "RandomPosition" (UInt8 field in root struct)
          - "Random" (UInt8 field in root struct)
          - "Volume" (UInt8 field in root struct)
          - "VolumeVrtn" (UInt8 field in root struct)
          - "PitchVariation" (Single field in root struct)
          - "Elevation" (Single field in root struct)
          - "MinDistance" (Single field in root struct)
          - "MaxDistance" (Single field in root struct)
          - "DistanceCutoff" (Single field in root struct)
          - "Priority" (UInt8 field in root struct)
          - "Hours" (UInt32 field in root struct, deprecated)
          - "Times" (UInt8 field in root struct, deprecated)
          - "Interval" (UInt32 field in root struct)
          - "IntervalVrtn" (UInt32 field in root struct)
          - "Sound" (ResRef field in root struct and sound entry structs)
          - "Comment" (string field in root struct)
          - "RandomRangeX" (Single field in root struct)
          - "RandomRangeY" (Single field in root struct)
          - "PaletteID" (UInt8 field in root struct)
          - "Sounds" (List field in root struct)
  
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
          Sound entry structs typically have struct_id = 2.
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
          
          UTS-specific field types:
          - Tag: field_type = 10 (String)
          - TemplateResRef: field_type = 11 (ResRef)
          - LocName: field_type = 12 (LocalizedString)
          - Active, Continuous, Looping, Positional, RandomPosition, Random, Volume, VolumeVrtn, Priority, Times, PaletteID: field_type = 0 (Byte/UInt8)
          - PitchVariation, Elevation, MinDistance, MaxDistance, DistanceCutoff, RandomRangeX, RandomRangeY: field_type = 8 (Single/Float32)
          - Hours, Interval, IntervalVrtn: field_type = 4 (UInt32)
          - Sound: field_type = 11 (ResRef)
          - Comment: field_type = 10 (String)
          - Sounds: field_type = 15 (List)
      
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
          UTS files store strings, ResRefs, and LocalizedStrings in this section.
          ResRef format: 1-byte length (0-16) followed by up to 16 ASCII characters.
          String format: 4-byte length (UInt32) followed by ASCII/UTF-8 characters.
          LocalizedString format: See GFF specification for LocString structure.
  
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
          For UTS files:
          - Sounds LIST: Contains struct indices pointing to sound entry structs
          
          Each LIST field_entry.data_or_offset points to a byte offset in this array.
          The list indices are stored as consecutive uint32 values starting at that offset.
          First uint32 is the count of struct indices, followed by the struct indices themselves.


