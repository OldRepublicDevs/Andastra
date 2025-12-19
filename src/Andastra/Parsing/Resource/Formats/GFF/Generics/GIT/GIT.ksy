meta:
  id: git
  title: BioWare GIT (Game Instance Template) File Format
  license: MIT
  endian: le
  file-extension: git
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/git.py
    reone: vendor/reone/src/libs/resource/parser/gff/git.cpp
    xoreos: vendor/xoreos/src/aurora/gitfile.cpp
    wiki: vendor/PyKotor/wiki/GFF-File-Format.md
doc: |
  GIT (Game Instance Template) files are GFF-based format files that store dynamic area information
  including creatures, doors, placeables, triggers, waypoints, stores, encounters, sounds, and cameras.
  GIT files use the GFF (Generic File Format) binary structure with file type signature "GIT ".
  
  GIT files contain:
  - Root struct with:
    - UseTemplates (UInt8): Flag indicating whether templates are used
    - AreaProperties (Struct, struct_id=100): Area audio and environment properties
    - CameraList (List): Array of GITCamera structs (struct_id=14)
    - Creature List (List): Array of GITCreature structs (struct_id=4)
    - Door List (List): Array of GITDoor structs (struct_id=8)
    - Encounter List (List): Array of GITEncounter structs (struct_id=7)
    - Placeable List (List): Array of GITPlaceable structs (struct_id=9)
    - SoundList (List): Array of GITSound structs (struct_id=6)
    - StoreList (List): Array of GITStore structs (struct_id=11)
    - TriggerList (List): Array of GITTrigger structs (struct_id=1)
    - WaypointList (List): Array of GITWaypoint structs (struct_id=5)
    - List (List, deprecated): Deprecated list field (KotOR 1 compatibility)
  
  AreaProperties struct contains:
  - AmbientSndDayVol (Int32): Day ambient sound volume
  - AmbientSndDay (Int32): Day ambient sound ID
  - AmbientSndNitVol (Int32): Night ambient sound volume
  - AmbientSndNight (Int32): Night ambient sound ID
  - EnvAudio (Int32): Environment audio ID
  - MusicDay (Int32): Day music ID
  - MusicNight (Int32): Night music ID
  - MusicBattle (Int32): Battle music ID
  - MusicDelay (Int32): Music delay in milliseconds
  
  GITCamera (struct_id=14) contains:
  - CameraID (Int32): Unique camera identifier
  - FieldOfView (Single): Camera field of view in degrees
  - Height (Single): Camera height
  - MicRange (Single): Microphone range
  - Orientation (Vector4): Camera orientation quaternion
  - Position (Vector3): Camera position in 3D space
  - Pitch (Single): Camera pitch angle
  
  GITCreature (struct_id=4) contains:
  - TemplateResRef (ResRef): Reference to creature template (UTC file)
  - XPosition (Single): X coordinate
  - YPosition (Single): Y coordinate
  - ZPosition (Single): Z coordinate
  - XOrientation (Single): X component of orientation vector
  - YOrientation (Single): Y component of orientation vector
  
  GITDoor (struct_id=8) contains:
  - TemplateResRef (ResRef): Reference to door template (UTD file)
  - Bearing (Single): Door bearing/rotation angle
  - Tag (String): Door tag identifier
  - LinkedTo (String): Linked object tag
  - LinkedToFlags (Byte): Module link flags (0=NoLink, 1=ToDoor, 2=ToWaypoint)
  - LinkedToModule (ResRef): Linked module reference
  - TransitionDestin (LocalizedString): Transition destination name
  - X (Single): X coordinate
  - Y (Single): Y coordinate
  - Z (Single): Z coordinate
  - TweakColor (UInt32, KotOR 2 only): Color tweak value (BGR format)
  - UseTweakColor (Byte, KotOR 2 only): Flag indicating if color tweak is used
  
  GITEncounter (struct_id=7) contains:
  - TemplateResRef (ResRef): Reference to encounter template (UTE file)
  - XPosition (Single): X coordinate
  - YPosition (Single): Y coordinate
  - ZPosition (Single): Z coordinate
  - Geometry (List): List of geometry points (struct_id=1)
  - SpawnPointList (List): List of spawn points (struct_id=2)
  
  GITEncounterGeometry (struct_id=1) contains:
  - X (Single): X coordinate of geometry point
  - Y (Single): Y coordinate of geometry point
  - Z (Single): Z coordinate of geometry point
  
  GITEncounterSpawnPoint (struct_id=2) contains:
  - X (Single): X coordinate of spawn point
  - Y (Single): Y coordinate of spawn point
  - Z (Single): Z coordinate of spawn point
  - Orientation (Single): Spawn point orientation angle
  
  GITPlaceable (struct_id=9) contains:
  - TemplateResRef (ResRef): Reference to placeable template (UTP file)
  - Bearing (Single): Placeable bearing/rotation angle
  - X (Single): X coordinate
  - Y (Single): Y coordinate
  - Z (Single): Z coordinate
  - TweakColor (UInt32, KotOR 2 only): Color tweak value (BGR format)
  - UseTweakColor (Byte, KotOR 2 only): Flag indicating if color tweak is used
  
  GITSound (struct_id=6) contains:
  - TemplateResRef (ResRef): Reference to sound template (UTS file)
  - GeneratedType (UInt32): Generated type identifier
  - XPosition (Single): X coordinate
  - YPosition (Single): Y coordinate
  - ZPosition (Single): Z coordinate
  
  GITStore (struct_id=11) contains:
  - ResRef (ResRef): Reference to store template (UTM file)
  - XPosition (Single): X coordinate
  - YPosition (Single): Y coordinate
  - ZPosition (Single): Z coordinate
  - XOrientation (Single): X component of orientation vector
  - YOrientation (Single): Y component of orientation vector
  
  GITTrigger (struct_id=1) contains:
  - TemplateResRef (ResRef): Reference to trigger template (UTT file)
  - XPosition (Single): X coordinate
  - YPosition (Single): Y coordinate
  - ZPosition (Single): Z coordinate
  - XOrientation (Single): X component of orientation vector
  - YOrientation (Single): Y component of orientation vector
  - ZOrientation (Single): Z component of orientation vector
  - Tag (String): Trigger tag identifier
  - LinkedTo (String): Linked object tag
  - LinkedToFlags (Byte): Module link flags (0=NoLink, 1=ToDoor, 2=ToWaypoint)
  - LinkedToModule (ResRef): Linked module reference
  - TransitionDestin (LocalizedString): Transition destination name
  - Geometry (List): List of geometry points (struct_id=3)
  
  GITTriggerGeometry (struct_id=3) contains:
  - PointX (Single): X coordinate of geometry point
  - PointY (Single): Y coordinate of geometry point
  - PointZ (Single): Z coordinate of geometry point
  
  GITWaypoint (struct_id=5) contains:
  - TemplateResRef (ResRef): Reference to waypoint template (UTW file)
  - LocalizedName (LocalizedString): Waypoint display name
  - Tag (String): Waypoint tag identifier
  - XPosition (Single): X coordinate
  - YPosition (Single): Y coordinate
  - ZPosition (Single): Z coordinate
  - XOrientation (Single): X component of orientation vector
  - YOrientation (Single): Y component of orientation vector
  - MapNoteEnabled (Byte): Flag indicating if map note is enabled
  - HasMapNote (Byte): Flag indicating if map note exists
  - MapNote (LocalizedString): Map note text (only if HasMapNote is true)
  - Appearance (Byte, deprecated): Appearance flag (KotOR 1 compatibility)
  - Description (LocalizedString, deprecated): Description (KotOR 1 compatibility)
  - LinkedTo (String, deprecated): Linked object tag (KotOR 1 compatibility)
  
  References:
  - vendor/PyKotor/wiki/GFF-File-Format.md
  - vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/git.py
  - vendor/reone/src/libs/resource/parser/gff/git.cpp
  - vendor/xoreos/src/aurora/gitfile.cpp

seq:
  - id: gff_header
    type: gff_header
    doc: GFF file header (56 bytes)
  
  - id: label_array
    type: label_array
    if: gff_header.label_count > 0
    pos: gff_header.label_offset
    doc: Array of field name labels (16-byte null-terminated strings)
  
  - id: struct_array
    type: struct_array
    if: gff_header.struct_count > 0
    pos: gff_header.struct_offset
    doc: Array of struct entries (12 bytes each)
  
  - id: field_array
    type: field_array
    if: gff_header.field_count > 0
    pos: gff_header.field_offset
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
          File type signature. Must be "GIT " for Game Instance Template files.
          Other GFF types: "GFF ", "ARE ", "UTC ", "UTI ", "DLG ", etc.
        valid: "GIT "
      
      - id: file_version
        type: str
        encoding: ASCII
        size: 4
        doc: |
          File format version. Typically "V3.2" for KotOR.
          Other versions: "V3.3", "V4.0", "V4.1" for other BioWare games.
        valid: ["V3.2", "V3.3", "V4.0", "V4.1"]
      
      - id: struct_offset
        type: u4
        doc: Byte offset to struct array from the beginning of the file
      
      - id: struct_count
        type: u4
        doc: Number of structs in the struct array
      
      - id: field_offset
        type: u4
        doc: Byte offset to field array from the beginning of the file
      
      - id: field_count
        type: u4
        doc: Number of fields in the field array
      
      - id: label_offset
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
    instances:
      name_trimmed:
        value: name.rstrip('\x00')
        doc: Label name with trailing nulls removed
  
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
          GIT-specific struct IDs:
          - 1: GITTrigger, GITTriggerGeometry
          - 2: GITEncounterSpawnPoint
          - 3: GITTriggerGeometry
          - 4: GITCreature
          - 5: GITWaypoint
          - 6: GITSound
          - 7: GITEncounter
          - 8: GITDoor
          - 9: GITPlaceable
          - 11: GITStore
          - 14: GITCamera
          - 100: AreaProperties
      
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
          16 = Vector4
          17 = Vector3
      
      - id: label_index
        type: u4
        doc: Index into label_array for field name
      
      - id: data_or_offset
        type: u4
        doc: |
          For simple types (Byte, Char, UInt16, Int16, UInt32, Int32, UInt64, Int64, Single, Double):
            Inline data value (stored directly in this field)
          For complex types (String, ResRef, LocalizedString, Binary, Vector3, Vector4):
            Byte offset into field_data section (relative to field_data_offset)
          For Struct type:
            Struct index into struct_array
          For List type:
            Byte offset into list_indices array (relative to list_indices_offset)
    instances:
      is_simple_type:
        value: field_type.value >= 0 && field_type.value <= 5 || field_type.value == 8
        doc: True if field stores data inline (simple types)
      is_complex_type:
        value: field_type.value >= 6 && field_type.value <= 13 || field_type.value >= 16 && field_type.value <= 17
        doc: True if field stores data in field_data section
      is_struct_type:
        value: field_type.value == 14
        doc: True if field is a nested struct
      is_list_type:
        value: field_type.value == 15
        doc: True if field is a list of structs
      field_data_offset_value:
        value: _root.gff_header.field_data_offset + data_or_offset
        if: is_complex_type
        doc: Absolute file offset to field data for complex types
      struct_index_value:
        value: data_or_offset
        if: is_struct_type
        doc: Struct index for struct type fields
      list_indices_offset_value:
        value: _root.gff_header.list_indices_offset + data_or_offset
        if: is_list_type
        doc: Absolute file offset to list indices for list type fields
  
  # Field Data Section
  field_data_section:
    seq:
      - id: data
        type: str
        size: _root.gff_header.field_data_count
        doc: |
          Raw field data bytes for complex types. Individual field data entries are accessed via
          field_entry.field_data_offset_value offsets. The structure of each entry depends on the field_type:
          - UInt64/Int64/Double: 8 bytes
          - String: 4-byte length (u4) + string bytes (ASCII)
          - ResRef: 1-byte length (u1, max 16) + string bytes (ASCII, null-padded)
          - LocalizedString: variable (see localized_string_data type)
          - Binary: 4-byte length (u4) + binary bytes
          - Vector3: 12 bytes (3×f4, little-endian)
          - Vector4: 16 bytes (4×f4, little-endian)
  
  # Field Indices Array (MultiMap)
  field_indices_array:
    seq:
      - id: indices
        type: u4
        repeat: expr
        repeat-expr: _root.gff_header.field_indices_count
        doc: |
          Array of field indices. When a struct has multiple fields, it stores an offset
          into this array, and the next N consecutive u4 values (where N = struct.field_count)
          are the field indices for that struct.
  
  # List Indices Array
  list_indices_array:
    seq:
      - id: raw_data
        type: str
        size: _root.gff_header.list_indices_count
        doc: |
          Raw list indices data. List entries are accessed via offsets stored in
          list-type field entries. Each entry starts with a count (u4), followed
          by that many struct indices (u4 each).
      
      - id: entries
        type: list_entry
        repeat: until
        repeat-until: _io.pos >= (_root.gff_header.list_indices_offset + _root.gff_header.list_indices_count)
        doc: |
          Array of list entries. In practice, list entries are accessed via offsets
          stored in list-type field entries (field_entry.list_indices_offset_value),
          not as a sequential array. This is a simplified representation for documentation.
  
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
  
  # Complex field data types (used when accessing field_data section)
  
  localized_string_data:
    seq:
      - id: total_size
        type: u4
        doc: |
          Total size of this LocalizedString structure in bytes (not including this count).
          Used for skipping over the structure, but can be calculated from the data.
      
      - id: string_ref
        type: u4
        doc: |
          String reference ID (StrRef) into dialog.tlk file.
          Value 0xFFFFFFFF indicates no string reference (-1).
      
      - id: string_count
        type: u4
        doc: Number of language-specific string substrings
      
      - id: substrings
        type: localized_substring
        repeat: expr
        repeat-expr: string_count
        doc: Array of language-specific string substrings
    instances:
      string_ref_value:
        value: string_ref == 0xFFFFFFFF ? -1 : string_ref
        doc: String reference as signed integer (-1 if none)
  
  localized_substring:
    seq:
      - id: string_id
        type: u4
        doc: |
          String ID encoding language and gender:
          - Bits 0-7: Gender (0 = Male, 1 = Female)
          - Bits 8-15: Language ID (see Language enum)
          - Bits 16-31: Reserved/unused
      
      - id: string_length
        type: u4
        doc: Length of string data in bytes
      
      - id: string_data
        type: str
        size: string_length
        encoding: UTF-8
        doc: |
          String data (encoding depends on language, but UTF-8 is common).
          Trailing null bytes should be trimmed.
    instances:
      language_id:
        value: (string_id >> 8) & 0xFF
        doc: Language ID (extracted from string_id)
      gender_id:
        value: string_id & 0xFF
        doc: Gender ID (0 = Male, 1 = Female)

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

