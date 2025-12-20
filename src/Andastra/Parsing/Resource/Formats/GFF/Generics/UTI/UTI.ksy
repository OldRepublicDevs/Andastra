meta:
  id: uti
  title: BioWare UTI (Item) File Format
  license: MIT
  endian: le
  file-extension: uti
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/uti.py
    reone: vendor/reone/src/libs/resource/parser/gff/uti.cpp
    xoreos: vendor/xoreos/src/aurora/gfffile.cpp
    wiki: vendor/PyKotor/wiki/GFF-UTI.md
doc: |
  UTI (Item) files are GFF-based format files that store item definitions including
  properties, costs, charges, upgrade information, and visual variations. UTI files use
  the GFF (Generic File Format) binary structure with file type signature "UTI ".
  
  UTI files contain:
  - Root struct with item metadata (TemplateResRef, Tag, LocalizedName, Description, etc.)
  - Base item configuration (BaseItem, Cost, AddCost, Plot, Charges, StackSize)
  - Visual variations (ModelVariation, BodyVariation, TextureVar)
  - Item properties list (PropertiesList) containing enchantments and bonuses
  - KotOR 2 specific fields (UpgradeLevel, WeaponColor, WeaponWhoosh, ArmorRulesType)
  - Quest and special item flags (Plot, Stolen, Identified, Cursed)
  - Palette and editor metadata (PaletteID, Comment)
  
  Each field in the root struct contains:
  - Basic item properties (TemplateResRef, Tag, LocalizedName, Description)
  - Base item configuration (BaseItem index into baseitems.2da, Cost, AddCost)
  - Usage properties (Charges, StackSize, Plot flag)
  - Visual properties (ModelVariation, BodyVariation, TextureVar)
  - Item properties list (List of UTIProperty structs)
  
  PropertiesList contains structs with:
  - PropertyName (UInt16): Index into itempropdef.2da
  - Subtype (UInt16): Property subtype/category
  - CostTable (UInt8): Cost table index
  - CostValue (UInt16): Cost value
  - Param1 (UInt8): First parameter
  - Param1Value (UInt8): First parameter value
  - ChanceAppear (UInt8): Percentage chance to appear (random loot, default 100)
  - UpgradeType (UInt8, optional): Upgrade type restriction (KotOR2)
  
  References:
  - vendor/PyKotor/wiki/GFF-UTI.md
  - vendor/PyKotor/wiki/GFF-File-Format.md
  - vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/generics/uti.py
  - vendor/reone/src/libs/resource/parser/gff/uti.cpp
  - vendor/xoreos/src/aurora/gfffile.cpp

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
          File type signature. Must be "UTI " for item files.
          Other GFF types: "GFF ", "ARE ", "DLG ", "UTC ", "UTD ", etc.
        valid: "UTI "
      
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
      - id: indices
        type: u4
        repeat: expr
        repeat-expr: _root.gff_header.list_indices_count
        doc: Array of list indices (uint32 values) for LIST type fields
  
  # UTI-specific field documentation (for reference when parsing root struct)
  # These are not separate types but are fields that appear in the root struct:
  #
  # Core Identity fields:
  # - TemplateResRef (ResRef, type 11): Template identifier for this item
  # - Tag (CExoString, type 10): Unique tag for script references
  # - LocalizedName (CExoLocString, type 12): Item name (localized)
  # - Description (CExoLocString, type 12): Generic description
  # - DescIdentified (CExoLocString, type 12): Description when identified
  # - Comment (CExoString, type 10): Developer comment/notes
  #
  # Base Item Configuration:
  # - BaseItem (Int32, type 5): Index into baseitems.2da (defines item type)
  # - Cost (UInt32, type 4): Base value in credits
  # - AddCost (UInt32, type 4): Additional cost from properties
  # - Plot (Byte, type 0): Plot-critical item (cannot be sold/destroyed)
  # - Charges (Byte, type 0): Number of uses remaining
  # - StackSize (UInt16, type 2): Current stack quantity
  # - ModelVariation (Byte, type 0): Model variation index (1-99)
  # - BodyVariation (Byte, type 0): Body variation for armor (1-9)
  # - TextureVar (Byte, type 0): Texture variation for armor (1-9)
  #
  # Item Properties:
  # - PropertiesList (List, type 15): Item properties and enchantments
  #   Each entry in the list is a struct containing:
  #   - PropertyName (UInt16, type 2): Index into itempropdef.2da
  #   - Subtype (UInt16, type 2): Property subtype/category
  #   - CostTable (Byte, type 0): Cost table index
  #   - CostValue (UInt16, type 2): Cost value
  #   - Param1 (Byte, type 0): First parameter
  #   - Param1Value (Byte, type 0): First parameter value
  #   - ChanceAppear (Byte, type 0): Percentage chance to appear (random loot, default 100)
  #   - UpgradeType (Byte, type 0, optional): Upgrade type restriction (KotOR2)
  #
  # KotOR 2 specific fields:
  # - UpgradeLevel (Byte, type 0): Current upgrade tier (0-2)
  # - WeaponColor (Byte, type 0): Lightsaber blade color (0-10)
  # - WeaponWhoosh (Byte, type 0): Swing sound type
  # - ArmorRulesType (Byte, type 0): Armor restriction category
  #
  # Quest & Special Items:
  # - Stolen (Byte, type 0): Marked as stolen
  # - Identified (Byte, type 0): Player has identified the item
  #
  # Palette & Editor:
  # - PaletteID (Byte, type 0): Toolset palette category

