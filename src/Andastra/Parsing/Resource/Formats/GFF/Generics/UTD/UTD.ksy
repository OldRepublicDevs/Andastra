meta:
  id: utd
  title: BioWare UTD (Door) File Format
  license: MIT
  endian: le
  file-extension: utd
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/utd.py
    reone: vendor/reone/src/libs/resource/parser/gff/utd.cpp
    wiki: vendor/PyKotor/wiki/GFF-UTD.md
doc: |
  UTD (Door) files are GFF-based format files that define door templates for all interactive doors
  in the game world. Doors can be locked, require keys, have hit points, conversations, and various
  gameplay interactions. UTD files use the GFF (Generic File Format) binary structure with
  file type signature "UTD ".
  
  UTD files contain:
  - Root struct with door metadata:
    - TemplateResRef: Door template ResRef (unique identifier, max 16 characters, ResRef type)
    - LocName: Localized door name (LocalizedString/CExoLocString type)
    - Description: Door description (LocalizedString/CExoLocString type, deprecated, not used by game engine)
    - Tag: Door tag identifier (String/CExoString type, used for scripting references)
    - Comment: Developer comment string (String/CExoString type, toolset only, not used by game engine)
    - Conversation: Dialog ResRef (ResRef type, triggers dialog when door is clicked)
    - Faction: Faction identifier (UInt32, faction ID for door ownership)
    - Appearance: Appearance index (UInt32, deprecated, always 0, not used by game engine)
    - GenericType: Door appearance type (UInt8/Byte, index into genericdoors.2da, determines model)
    - AnimationState: Current animation state (UInt8/Byte, always 0 in templates)
    - PaletteID: Toolset palette category (UInt8/Byte, editor organization, doesn't affect gameplay)
    - PortraitId: Portrait icon identifier (UInt16, deprecated, not used by game engine)
  - Lock/unlock properties:
    - Lockable: Can be locked/unlocked (UInt8/Byte, 0 = cannot lock, 1 = can lock)
    - Locked: Currently locked (UInt8/Byte, 0 = unlocked, 1 = locked)
    - KeyRequired: Requires specific KEY item (UInt8/Byte, 0 = no key, 1 = key required)
    - KeyName: Tag of required KEY item (String/CExoString type)
    - AutoRemoveKey: KEY consumed on use (UInt8/Byte, 0 = keep key, 1 = remove key)
    - OpenLockDC: Security skill DC to pick lock (UInt8/Byte, 0-255, difficulty to unlock)
    - CloseLockDC: Security skill DC to lock door (UInt8/Byte, KotOR2 only, deprecated, always 0)
    - OpenLockDiff: Unlock difficulty modifier (UInt8/Byte, KotOR2 only, additional difficulty)
    - OpenLockDiffMod: Additional unlock difficulty modifier (Int8/Char, KotOR2 only, signed modifier)
    - OpenState: Current open state (UInt8/Byte, KotOR2 only, 0=closed, 1=open1, 2=open2)
  - HP and combat stats:
    - HP: Maximum hit points (Int16, 0 = indestructible, >0 = can be destroyed)
    - CurrentHP: Current hit points (Int16, current damage state)
    - Hardness: Damage reduction (UInt8/Byte, reduces each hit's damage)
    - Min1HP: Cannot drop below 1 HP (UInt8/Byte, KotOR2 only, 0 = can be destroyed, 1 = plot door)
    - NotBlastable: Cannot be blasted (UInt8/Byte, KotOR2 only, 0 = can blast, 1 = cannot blast)
    - Fort: Fortitude save (UInt8/Byte, deprecated, always 0, not used)
    - Ref: Reflex save (UInt8/Byte, deprecated, always 0, not used)
    - Will: Will save (UInt8/Byte, deprecated, always 0, not used)
  - Interaction flags:
    - Plot: Plot-critical flag (UInt8/Byte, 0 = normal door, 1 = cannot be destroyed)
    - Static: Static geometry flag (UInt8/Byte, 0 = interactive, 1 = non-interactive)
    - Interruptable: Opening can be interrupted (UInt8/Byte, deprecated, not used by game engine)
  - Script hooks (all ResRef type):
    - OnClick: Fires when door is clicked
    - OnOpen: Fires when door opens
    - OnClosed: Fires after door finishes closing (always empty in files)
    - OnClose: Fires when door closes (not in UTD, used in runtime)
    - OnDamaged: Fires when door takes damage (always empty in files)
    - OnDeath: Fires when door is destroyed
    - OnDisarm: Fires when trap is disarmed (deprecated, always empty)
    - OnFailToOpen: Fires when opening fails (KotOR2 only)
    - OnHeartbeat: Fires periodically
    - OnLock: Fires when door is locked (always empty in files)
    - OnMeleeAttacked: Fires when attacked in melee (always empty in files)
    - OnSpellCastAt: Fires when spell cast at door (always empty in files)
    - OnTrapTriggered: Fires when trap triggers (deprecated, always empty)
    - OnUnlock: Fires when door is unlocked (always empty in files)
    - OnUserDefined: Fires on user-defined events
  - Trap system (deprecated, toolset only, not used by game engine):
    - TrapDetectable: Trap can be detected (UInt8/Byte, deprecated, always 1, not used)
    - TrapDetectDC: Awareness DC to detect trap (UInt8/Byte, deprecated, always 0, not used)
    - TrapDisarmable: Trap can be disarmed (UInt8/Byte, deprecated, always 1, not used)
    - DisarmDC: Security DC to disarm trap (UInt8/Byte, deprecated, mostly 28, not used)
    - TrapFlag: Trap is active (UInt8/Byte, deprecated, always 0, not used)
    - TrapOneShot: Trap triggers only once (UInt8/Byte, deprecated, always 1, not used)
    - TrapType: Trap type index (UInt8/Byte, deprecated, index into traps.2da, not used)
  - Load-bearing doors (KotOR2 only, deprecated, not used by game engine):
    - LoadScreenID: Loading screen identifier (UInt16, deprecated, always 0, not used)
    - LinkedTo: Destination module tag (String/CExoString type, deprecated, not used)
    - LinkedToFlags: Transition behavior flags (UInt8/Byte, deprecated, not used)
    - LinkedToModule: Destination module ResRef (ResRef type, deprecated, not used)
    - TransitionDestin: Destination label (LocalizedString type, deprecated, not used)
  
  Door State Machine:
  Doors maintain runtime state:
  1. Closed: Default state, blocking
  2. Opening: Animation playing, becoming non-blocking
  3. Open: Fully open, non-blocking
  4. Closing: Animation playing, becoming blocking
  5. Locked: Closed and cannot open
  6. Destroyed: Hit points depleted, permanently open
  
  Opening Sequence:
  1. Player clicks door
  2. If conversation set, start dialog
  3. If locked, check for KEY or Security skill
  4. If trapped, check for detection/disarm
  5. Fire OnOpen script
  6. Play opening animation
  7. Transition to "open" state
  
  Locking System:
  - Lockable=0: Door cannot be locked (always opens)
  - Locked=1, KeyRequired=1: Must have specific KEY
  - Locked=1, OpenLockDC>0: Can pick lock with Security skill
  - Locked=1, KeyRequired=0, OpenLockDC=0: Locked via script only
  
  Common Door Types:
  - Standard Doors: Simple open/close, no lock, HP, or trap
  - Locked Doors: Requires KEY or Security skill, quest progression gates
  - Destructible Doors: Have HP and Hardness, can be bashed down
  - Trapped Doors: Trigger trap on opening (deprecated system)
  - Transition Doors: Load new modules/areas (deprecated system)
  - Conversation Doors: Trigger dialog on click, may open after conversation
  
  References:
  - vendor/PyKotor/wiki/GFF-UTD.md
  - vendor/PyKotor/wiki/GFF-File-Format.md
  - vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/utd.py:16-665
  - vendor/reone/src/libs/resource/parser/gff/utd.cpp:28-89
  - vendor/reone/include/reone/resource/parser/gff/utd.h:28-87

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
          File type signature. Must be "UTD " for door template files.
          Other GFF types: "GFF ", "DLG ", "ARE ", "UTC ", "UTI ", "UTM ", "GIT ", etc.
        valid: "UTD "
      
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
          Common UTD field names: "TemplateResRef", "LocName", "Description", "Tag", "Comment",
          "Conversation", "Faction", "Appearance", "GenericType", "AnimationState", "PaletteID",
          "PortraitId", "Lockable", "Locked", "KeyRequired", "KeyName", "AutoRemoveKey",
          "OpenLockDC", "CloseLockDC", "OpenLockDiff", "OpenLockDiffMod", "OpenState",
          "HP", "CurrentHP", "Hardness", "Min1HP", "NotBlastable", "Fort", "Ref", "Will",
          "Plot", "Static", "Interruptable", "OnClick", "OnOpen", "OnClosed", "OnDamaged",
          "OnDeath", "OnDisarm", "OnFailToOpen", "OnHeartbeat", "OnLock", "OnMeleeAttacked",
          "OnSpellCastAt", "OnTrapTriggered", "OnUnlock", "OnUserDefined", "TrapDetectable",
          "TrapDetectDC", "TrapDisarmable", "DisarmDC", "TrapFlag", "TrapOneShot", "TrapType",
          "LoadScreenID", "LinkedTo", "LinkedToFlags", "LinkedToModule", "TransitionDestin".
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
          UTD-specific struct IDs:
          - 0xFFFFFFFF (-1): Root struct containing door metadata
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
          0 = Byte (UInt8) - Used for: Lockable, Locked, KeyRequired, AutoRemoveKey, OpenLockDC,
              CloseLockDC, OpenLockDiff, OpenState, GenericType, AnimationState, PaletteID,
              Hardness, Min1HP, NotBlastable, Plot, Static, Interruptable, Fort, Ref, Will,
              TrapDetectable, TrapDetectDC, TrapDisarmable, DisarmDC, TrapFlag, TrapOneShot,
              TrapType, LinkedToFlags
          1 = Char (Int8)
          2 = UInt16 - Used for: PortraitId, LoadScreenID
          3 = Int16 - Used for: HP, CurrentHP
          4 = UInt32 - Used for: Faction, Appearance
          5 = Int32
          6 = UInt64
          7 = Int64
          8 = Single (Float32)
          9 = Double (Float64)
          10 = CExoString (String) - Used for: Tag, Comment, KeyName, LinkedTo
          11 = ResRef - Used for: TemplateResRef, Conversation, OnClick, OnOpen, OnClosed,
              OnDamaged, OnDeath, OnDisarm, OnFailToOpen, OnHeartbeat, OnLock, OnMeleeAttacked,
              OnSpellCastAt, OnTrapTriggered, OnUnlock, OnUserDefined, LinkedToModule
          12 = CExoLocString (LocalizedString) - Used for: LocName, Description, TransitionDestin
          13 = Void (Binary)
          14 = Struct
          15 = List
          16 = Vector4
          17 = Vector3
      
      - id: label_index
        type: u4
        doc: |
          Index into label_array for field name.
          The label_array[label_index].name_trimmed gives the field name.
      
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
        doc: "True if field stores data inline (simple types: Byte, Char, UInt16, Int16, UInt32, Int32, Float)"
      is_complex_type:
        value: field_type.value >= 6 && field_type.value <= 13 || field_type.value >= 16 && field_type.value <= 17
        doc: "True if field stores data in field_data section (complex types: UInt64, Int64, Double, String, ResRef, LocalizedString, Binary, Vector3, Vector4)"
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
          - UInt64/Int64/Double: 8 bytes (stored in field_data section)
          - String (CExoString): 4-byte length (u4) + string bytes (ASCII, null-terminated)
          - ResRef: 1-byte length (u1, max 16) + string bytes (ASCII, null-padded to 16 bytes total)
          - LocalizedString (CExoLocString): variable (see localized_string_data type)
          - Binary (Void): 4-byte length (u4) + binary bytes
          - Vector3: 12 bytes (3×f4, little-endian: X, Y, Z)
          - Vector4: 16 bytes (4×f4, little-endian: W, X, Y, Z)
  
  # Helper types for parsing field_data section entries (documentation only)
  # Note: These types are conceptual - actual parsing accesses field_data via offsets
  
  resref_data:
    seq:
      - id: length
        type: u1
        doc: |
          Length of ResRef string (0-16). ResRef strings are limited to 16 characters.
          Common UTD ResRefs: door template names (e.g., "door_001", "door_locked"),
          conversation scripts (e.g., "door_conv"), script hooks (e.g., "door_open").
      
      - id: name
        type: str
        size: 16
        encoding: ASCII
        doc: |
          ResRef string (null-padded to 16 bytes).
          The actual string length is given by the length field above.
          Trailing null bytes should be ignored. To extract the actual ResRef name,
          take the first 'length' characters from 'name' (trim trailing nulls).
          Note: This is a documentation type - actual parsing accesses field_data via offsets.
  
  string_data:
    seq:
      - id: length
        type: u4
        doc: Length of string data in bytes (not including null terminator)
      
      - id: data
        type: str
        size: length
        encoding: ASCII
        doc: |
          String data (ASCII encoded, null-terminated).
          Trailing null bytes should be trimmed.
          Common UTD strings: Tag (door tag for scripting), Comment (developer notes),
          KeyName (tag of required KEY item), LinkedTo (destination module tag).
          Note: To extract the actual string value, trim trailing null bytes from 'data'.
    instances:
      data_trimmed:
        value: data.rstrip('\x00')
        doc: "String data with trailing nulls removed"
  
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
          Example: Root struct with 50+ fields stores offset to this array, then 50+ consecutive
          u4 values here are the field indices for TemplateResRef, LocName, Description, Tag,
          Comment, Conversation, Faction, GenericType, Lockable, Locked, KeyRequired, etc.
  
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
          Note: UTD files typically do not use LIST type fields, but the structure
          is available for future extensions.
      
      - id: entries
        type: list_entry
        repeat: until
        repeat-until: _io.pos >= (_root.gff_header.list_indices_offset + _root.gff_header.list_indices_count)
        doc: |
          Array of list entries. In practice, list entries are accessed via offsets
          stored in list-type field entries (field_entry.list_indices_offset_value),
          not as a sequential array. This is a simplified representation for documentation.
          UTD files typically do not use LIST type fields.
  
  list_entry:
    seq:
      - id: count
        type: u4
        doc: |
          Number of struct indices in this list.
          UTD files typically do not use LIST type fields.
      
      - id: struct_indices
        type: u4
        repeat: expr
        repeat-expr: count
        doc: |
          Array of struct indices (indices into struct_array).
          UTD files typically do not use LIST type fields.
  
  # Complex field data types (used when accessing field_data section)
  
  localized_string_data:
    seq:
      - id: total_size
        type: u4
        doc: |
          Total size of this LocalizedString structure in bytes (not including this count).
          Used for skipping over the structure, but can be calculated from the data.
          Format: total_size = 4 (string_ref) + 4 (string_count) + sum(substring sizes)
          Each substring: 4 (string_id) + 4 (string_length) + string_length (string_data)
      
      - id: string_ref
        type: u4
        doc: |
          String reference ID (StrRef) into dialog.tlk file.
          Value 0xFFFFFFFF indicates no string reference (-1).
          If string_ref is valid (not -1), this is the primary string shown to the user.
          Language-specific substrings override this for specific languages/genders.
          For UTD, LocName and Description use this to reference dialog.tlk entries.
      
      - id: string_count
        type: u4
        doc: |
          Number of language-specific string substrings.
          Typically 0 if only using string_ref, or 1-10+ for multi-language support.
      
      - id: substrings
        type: localized_substring
        repeat: expr
        repeat-expr: string_count
        doc: |
          Array of language-specific string substrings.
          Each substring provides text for a specific language and gender combination.
          Used to override or supplement the string_ref text.
    instances:
      string_ref_value:
        value: 'string_ref == 0xFFFFFFFF ? -1 : string_ref'
        doc: "String reference as signed integer (-1 if none)"
  
  localized_substring:
    seq:
      - id: string_id
        type: u4
        doc: |
          String ID encoding language and gender:
          - Bits 0-7: Gender (0 = Male, 1 = Female)
          - Bits 8-15: Language ID (see Language enum)
          - Bits 16-31: Reserved/unused
          Common languages: 0 = English, 1 = French, 2 = German, 3 = Italian, 4 = Spanish,
          5 = Polish, 6 = Korean, 7 = Chinese (Traditional), 8 = Chinese (Simplified), 9 = Japanese
      
      - id: string_length
        type: u4
        doc: Length of string data in bytes (UTF-8 encoded)
      
      - id: string_data
        type: str
        size: string_length
        encoding: UTF-8
        doc: |
          String data (encoding depends on language, but UTF-8 is common).
          Trailing null bytes should be trimmed.
          For UTD LocName and Description, this contains the door name/description
          in the specified language/gender.
    instances:
      language_id:
        value: (string_id >> 8) & 0xFF
        doc: "Language ID (extracted from string_id, bits 8-15)"
      gender_id:
        value: string_id & 0xFF
        doc: "Gender ID (extracted from string_id, bits 0-7: 0 = Male, 1 = Female)"

enums:
  gff_field_type:
    0: uint8
    doc: |
      8-bit unsigned integer (byte).
      Used in UTD for: Lockable, Locked, KeyRequired, AutoRemoveKey, OpenLockDC, CloseLockDC,
      OpenLockDiff, OpenState, GenericType, AnimationState, PaletteID, Hardness, Min1HP,
      NotBlastable, Plot, Static, Interruptable, Fort, Ref, Will, TrapDetectable, TrapDetectDC,
      TrapDisarmable, DisarmDC, TrapFlag, TrapOneShot, TrapType, LinkedToFlags.
    1: int8
    doc: |
      8-bit signed integer (char).
      Used in UTD for: OpenLockDiffMod (KotOR2 only, signed modifier).
    2: uint16
    doc: |
      16-bit unsigned integer (word).
      Used in UTD for: PortraitId, LoadScreenID.
    3: int16
    doc: |
      16-bit signed integer (short).
      Used in UTD for: HP (maximum hit points), CurrentHP (current hit points).
    4: uint32
    doc: |
      32-bit unsigned integer (dword).
      Used in UTD for: Faction (faction identifier), Appearance (deprecated appearance index).
    5: int32
    doc: 32-bit signed integer (int)
    6: uint64
    doc: 64-bit unsigned integer (stored in field_data section)
    7: int64
    doc: 64-bit signed integer (stored in field_data section)
    8: single
    doc: 32-bit floating point (float)
    9: double
    doc: 64-bit floating point (stored in field_data section)
    10: string
    doc: |
      Null-terminated string (CExoString, stored in field_data section).
      Used in UTD for: Tag (door tag identifier), Comment (developer comment),
      KeyName (tag of required KEY item), LinkedTo (destination module tag).
    11: resref
    doc: |
      Resource reference (ResRef, max 16 chars, stored in field_data section).
      Used in UTD for: TemplateResRef (door template name), Conversation (dialog ResRef),
      OnClick, OnOpen, OnClosed, OnDamaged, OnDeath, OnDisarm, OnFailToOpen, OnHeartbeat,
      OnLock, OnMeleeAttacked, OnSpellCastAt, OnTrapTriggered, OnUnlock, OnUserDefined
      (script hook ResRefs), LinkedToModule (destination module ResRef).
    12: localized_string
    doc: |
      Localized string (CExoLocString, stored in field_data section).
      Used in UTD for: LocName (localized door name with multiple language/gender support),
      Description (door description, deprecated, not used by game engine),
      TransitionDestin (destination label, deprecated, not used by game engine).
    13: binary
    doc: Binary data blob (Void, stored in field_data section)
    14: struct
    doc: Nested struct (struct index stored inline)
    15: list
    doc: List of structs (offset to list_indices stored inline). UTD files typically do not use LIST type fields.
    16: vector4
    doc: Quaternion/Orientation (4×float, stored in field_data as Vector4)
    17: vector3
    doc: 3D vector (3×float, stored in field_data)
