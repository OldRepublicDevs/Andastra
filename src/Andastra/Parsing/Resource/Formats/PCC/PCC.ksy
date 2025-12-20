meta:
  id: pcc
  title: BioWare PCC (Package) File Format
  license: MIT
  endian: le
  file-extension: pcc
  xref:
    me3explorer: https://me3explorer.fandom.com/wiki/PCC_File_Format
doc: |
  PCC (Package) files are BioWare's proprietary version of Unreal Engine package files.
  They store game resources including textures, meshes, materials, scripts, and more.
  
  PCC files can be either compressed or uncompressed:
  - Uncompressed: Direct structure with header, tables, and data
  - Compressed: Chunks containing compressed blocks using zlib or LZO
  
  The file structure consists of:
  1. File Header - Contains magic number, version, table offsets, and metadata
  2. Name Table - Contains all string names used in the package
  3. Import Table - Contains references to external packages/classes
  4. Export Table - Contains all objects stored in the package
  5. Export Data - Raw binary data for each export
  
  References:
  - https://me3explorer.fandom.com/wiki/PCC_File_Format
  - Unreal Engine package format (BioWare variant)

seq:
  - id: header
    type: file_header
    doc: File header containing format metadata and table offsets.
  
  - id: name_table
    type: name_table
    if: header.name_count > 0
    pos: header.name_table_offset
    doc: Table containing all string names used in the package.
  
  - id: import_table
    type: import_table
    if: header.import_count > 0
    pos: header.import_table_offset
    doc: Table containing references to external packages and classes.
  
  - id: export_table
    type: export_table
    if: header.export_count > 0
    pos: header.export_table_offset
    doc: Table containing all objects exported from this package.

instances:
  is_compressed:
    value: (header.package_flags & 0x2000000) != 0
    doc: True if package uses compressed chunks (bit 25 of package_flags).
  
  compression_type:
    value: header.compression_type
    doc: Compression algorithm used (0=None, 1=Zlib, 2=LZO).

types:
  file_header:
    seq:
      - id: magic
        type: u4
        doc: Magic number identifying PCC format. Must be 0x9E2A83C1.
        valid: 0x9E2A83C1
      
      - id: version
        type: u4
        doc: |
          File format version.
          Encoded as: (major << 16) | (minor << 8) | patch
          Example: 0xC202AC = 194/684 (major=194, minor=684)
      
      - id: licensee_version
        type: u4
        doc: Licensee-specific version field (typically 0x67C).
      
      - id: header_size
        type: s4
        doc: Header size field (typically -5 = 0xFFFFFFFB).
      
      - id: package_name
        type: str
        size: 10
        encoding: UTF-16LE
        doc: Package name (typically "None" = 0x4E006F006E006500).
      
      - id: package_flags
        type: u4
        doc: |
          Package flags bitfield.
          Bit 25 (0x2000000): Compressed package
          Bit 20 (0x100000): ME3Explorer edited format flag
          Other bits: Various package attributes
      
      - id: package_type
        type: u4
        doc: |
          Package type indicator.
          0 = Normal Package
          1 = Patch Package (only for use in Patch_001.sfar)
      
      - id: name_count
        type: u4
        doc: Number of entries in the name table.
      
      - id: name_table_offset
        type: u4
        doc: Byte offset to the name table from the beginning of the file.
      
      - id: export_count
        type: u4
        doc: Number of entries in the export table.
      
      - id: export_table_offset
        type: u4
        doc: Byte offset to the export table from the beginning of the file.
      
      - id: import_count
        type: u4
        doc: Number of entries in the import table.
      
      - id: import_table_offset
        type: u4
        doc: Byte offset to the import table from the beginning of the file.
      
      - id: depend_offset
        type: u4
        doc: Offset to dependency table (typically 0x664).
      
      - id: depend_count
        type: u4
        doc: Number of dependencies (typically 0x67C).
      
      - id: guid_part1
        type: u4
        doc: First 32 bits of package GUID.
      
      - id: guid_part2
        type: u4
        doc: Second 32 bits of package GUID.
      
      - id: guid_part3
        type: u4
        doc: Third 32 bits of package GUID.
      
      - id: guid_part4
        type: u4
        doc: Fourth 32 bits of package GUID.
      
      - id: generations
        type: u4
        doc: Number of generation entries.
      
      - id: export_count_dup
        type: u4
        doc: Duplicate export count (should match export_count).
      
      - id: name_count_dup
        type: u4
        doc: Duplicate name count (should match name_count).
      
      - id: unknown1
        type: u4
        doc: Unknown field (typically 0x0).
      
      - id: engine_version
        type: u4
        doc: Engine version (typically 0x18EF = 6383).
      
      - id: cooker_version
        type: u4
        doc: Cooker version (typically 0x3006B = 196715).
      
      - id: compression_flags
        type: u4
        doc: Compression flags (typically 0x15330000).
      
      - id: package_source
        type: u4
        doc: Package source identifier (typically 0x8AA0000).
      
      - id: compression_type
        type: u4
        doc: |
          Compression algorithm type.
          0 = None
          1 = Zlib
          2 = LZO
          Unused if package is not compressed.
      
      - id: chunk_count
        type: u4
        doc: |
          Number of compressed chunks (0 for uncompressed, 1 for compressed).
          If > 0, file uses compressed structure with chunks.
  
  name_table:
    seq:
      - id: entries
        type: name_entry
        repeat: expr
        repeat-expr: _root.header.name_count
        doc: Array of name entries.
  
  name_entry:
    seq:
      - id: length
        type: s4
        doc: |
          Length of the name string in characters (signed).
          Negative value indicates the number of WCHAR characters.
          Positive value is also valid but less common.
      
      - id: name
        type: str
        size: (length < 0 ? -length : length) * 2
        encoding: UTF-16LE
        doc: |
          Name string encoded as UTF-16LE (WCHAR).
          Size is absolute value of length * 2 bytes per character.
  
  import_table:
    seq:
      - id: entries
        type: import_entry
        repeat: expr
        repeat-expr: _root.header.import_count
        doc: Array of import entries.
  
  import_entry:
    seq:
      - id: package_name_index
        type: s8
        doc: |
          Index into name table for package name.
          Negative value indicates import from external package.
          Positive value indicates import from this package.
      
      - id: class_name_index
        type: s4
        doc: Index into name table for class name.
      
      - id: link
        type: s8
        doc: |
          Link to import/export table entry.
          Used to resolve the actual object reference.
      
      - id: import_name_index
        type: s8
        doc: Index into name table for the imported object name.
  
  export_table:
    seq:
      - id: entries
        type: export_entry
        repeat: expr
        repeat-expr: _root.header.export_count
        doc: Array of export entries.
  
  export_entry:
    seq:
      - id: class_index
        type: s4
        doc: |
          Object index for the class.
          Negative = import table index
          Positive = export table index
          Zero = no class
      
      - id: super_class_index
        type: s4
        doc: |
          Object index for the super class.
          Negative = import table index
          Positive = export table index
          Zero = no super class
      
      - id: link
        type: s4
        doc: Link to other objects (internal reference).
      
      - id: object_name_index
        type: s4
        doc: Index into name table for the object name.
      
      - id: object_name_number
        type: s4
        doc: Object name number (for duplicate names).
      
      - id: archetype_index
        type: s4
        doc: |
          Object index for the archetype.
          Negative = import table index
          Positive = export table index
          Zero = no archetype
      
      - id: object_flags
        type: u8
        doc: Object flags bitfield (64-bit).
      
      - id: data_size
        type: u4
        doc: Size of the export data in bytes.
      
      - id: data_offset
        type: u4
        doc: Byte offset to the export data from the beginning of the file.
      
      - id: unknown1
        type: u4
        doc: Unknown field.
      
      - id: component_count
        type: s4
        doc: Number of component entries (can be negative).
      
      - id: unknown2
        type: u4
        doc: Unknown field.
      
      - id: guid
        type: guid
        doc: GUID for this export object.
      
      - id: components
        type: s4
        repeat: expr
        repeat-expr: (component_count > 0 ? component_count : 0)
        doc: |
          Array of component indices.
          Only present if component_count > 0.
  
  guid:
    seq:
      - id: part1
        type: u4
        doc: First 32 bits of GUID.
      
      - id: part2
        type: u4
        doc: Second 32 bits of GUID.
      
      - id: part3
        type: u4
        doc: Third 32 bits of GUID.
      
      - id: part4
        type: u4
        doc: Fourth 32 bits of GUID.

