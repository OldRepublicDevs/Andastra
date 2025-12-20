meta:
  id: rim
  title: BioWare RIM (Resource Information Manager) Format
  license: MIT
  endian: le
  file-extension: rim
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/rim/
    reone: vendor/reone/src/libs/resource/format/rimreader.cpp
    xoreos: vendor/xoreos/src/aurora/rimfile.cpp
    wiki: vendor/PyKotor/wiki/RIM-File-Format.md
doc: |
  RIM (Resource Information Manager) files are self-contained archives used for module templates.
  RIM files are similar to ERF files but are read-only from the game's perspective. The game
  loads RIM files as templates for modules and exports them to ERF format for runtime mutation.
  RIM files store all resources inline with metadata, making them self-contained archives.
  
  Format Variants:
  - Standard RIM: Basic module template files
  - Extension RIM: Files ending in 'x' (e.g., module001x.rim) that extend other RIMs
  
  Binary Format:
  - Header (20 bytes): File type, version, resource count, offset to resource table
  - Extended Header (100 bytes): Reserved padding (total header = 120 bytes)
  - Resource Entry Table (32 bytes per entry): ResRef, type, ID, offset, size
  - Resource Data (variable size): Raw binary data for each resource
  
  References:
  - vendor/PyKotor/wiki/RIM-File-Format.md
  - vendor/reone/src/libs/resource/format/rimreader.cpp:24-100
  - vendor/xoreos/src/aurora/rimfile.cpp:40-160
  - vendor/Kotor.NET/Kotor.NET/Formats/KotorRIM/RIMBinaryStructure.cs:11-121
  - vendor/KotOR_IO/KotOR_IO/File Formats/RIM.cs:20-260

seq:
  - id: header
    type: rim_header
    doc: RIM file header (20 bytes)
  
  - id: extended_header
    type: rim_extended_header
    doc: Extended header padding (100 bytes, total header = 120 bytes)
  
  - id: resource_entry_table
    type: resource_entry_table
    if: header.resource_count > 0
    doc: Array of resource entries mapping ResRefs to resource data

types:
  rim_header:
    seq:
      - id: file_type
        type: str
        encoding: ASCII
        size: 4
        doc: |
          File type signature. Must be "RIM " (0x52 0x49 0x4D 0x20).
          This identifies the file as a RIM archive.
      
      - id: file_version
        type: str
        encoding: ASCII
        size: 4
        doc: |
          File format version. Always "V1.0" for KotOR RIM files.
          Other versions may exist in Neverwinter Nights but are not supported in KotOR.
      
      - id: reserved
        type: u4
        doc: |
          Reserved field (typically 0x00000000).
          Unknown purpose, but always set to 0 in practice.
      
      - id: resource_count
        type: u4
        doc: |
          Number of resources in the archive. This determines:
          - Number of entries in resource_entry_table
          - Number of resources in resource_data_section
      
      - id: offset_to_resource_table
        type: u4
        doc: |
          Byte offset to the resource entry table from the beginning of the file.
          Typically 120 (right after header + extended header) if resources are present.
          Points to the start of the resource_entry_table.
    
    instances:
      has_resources:
        value: resource_count > 0
        doc: Whether the RIM file contains any resources
  
  rim_extended_header:
    seq:
      - id: reserved_padding
        size: 100
        type: str
        encoding: ASCII
        doc: |
          Reserved padding bytes (typically all zeros).
          Total header size is 120 bytes:
          header (20) + extended_header (100) = 120 bytes
          
          In extension RIMs (files ending in 'x'), byte 0x14 (offset 20 in extended header)
          may contain an IsExtension flag, but this is not consistently used.
    
  
  resource_entry_table:
    seq:
      - id: entries
        type: resource_entry
        repeat: expr
        repeat-expr: _root.header.resource_count
        doc: Array of resource entries, one per resource in the archive
  
  resource_entry:
    seq:
      - id: resref
        type: str
        encoding: ASCII
        size: 16
        doc: |
          Resource filename (ResRef), null-padded to 16 bytes.
          Maximum 16 characters. If exactly 16 characters, no null terminator exists.
          Resource names can be mixed case, though most are lowercase in practice.
          The game engine typically lowercases ResRefs when loading.
      
      - id: resource_type
        type: u4
        doc: |
          Resource type identifier (see ResourceType enum).
          Examples: 0x000B (TPC/texture), 0x000A (MOD/module), 0x0000 (RES/unknown)
      
      - id: resource_id
        type: u4
        doc: |
          Resource ID (index, usually sequential).
          Typically matches the index of this entry in the resource_entry_table.
          Used for internal reference, but not critical for parsing.
      
      - id: offset_to_data
        type: u4
        doc: |
          Byte offset to resource data from the beginning of the file.
          Points to the actual binary data for this resource in resource_data_section.
      
      - id: resource_size
        type: u4
        doc: |
          Size of resource data in bytes.
          Uncompressed size of the resource.
    
    instances:
      data:
        pos: offset_to_data
        size: resource_size
        type: str
        encoding: ASCII
        doc: Raw binary data for this resource (read at specified offset)
  

