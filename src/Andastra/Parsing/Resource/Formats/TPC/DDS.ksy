meta:
  id: dds
  title: DirectDraw Surface (DDS) File Format
  license: MIT
  endian: le
  file-extension:
    - dds
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/tpc/io_dds.py
    reone: vendor/reone/src/libs/graphics/format/ddsreader.cpp
    xoreos: vendor/xoreos/src/graphics/images/dds.cpp
    wiki: vendor/PyKotor/wiki/DDS-File-Format.md
doc: |
  DirectDraw Surface (DDS) textures appear in two flavors:

  1. Standard DirectX DDS: Header magic 0x44445320 ("DDS "), 124-byte header
  2. BioWare DDS variant: No magic; width/height/bpp/dataSize leading integers

  The format supports DXT1/DXT3/DXT5 compressed textures, uncompressed RGB/RGBA,
  cube maps, and mipmaps.

  References:
  - vendor/PyKotor/wiki/DDS-File-Format.md
  - vendor/reone/src/libs/graphics/format/ddsreader.cpp
  - vendor/xoreos/src/graphics/images/dds.cpp

seq:
  - id: magic
    type: u4
    doc: |
      Magic number. 0x44445320 ("DDS ") for standard DDS.
      For BioWare variant, this is width (uint32) instead.

  - id: header_size
    type: u4
    doc: |
      Header size (124 for standard DDS, or height for BioWare variant).
      Used to determine format variant.
    valid: [124, 1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768]

  - id: header_data
    type: dds_header_fields
    doc: DDS header fields (structure differs based on format variant)

  - id: pixel_data
    type: pixel_data_section
    doc: Compressed or uncompressed pixel data

types:
  dds_header_fields:
    seq:
      - id: raw_fields
        size: 120
        type: u1
        repeat: expr
        repeat-expr: 120
        doc: |
          Raw header fields (120 bytes after size field for standard DDS,
          or height/bytesPerPixel/dataSize/unused for BioWare variant).
          Application should parse based on magic number to determine variant.
    instances:
      is_standard:
        value: _parent.magic == 0x44445320
        doc: True if standard DDS format

  pixel_data_section:
    seq:
      - id: data
        type: u1
        repeat: until
        repeat-until: _io.pos >= _io.size
        doc: |
          Pixel data for all mipmap levels.
          Size depends on format (DXT block size or uncompressed pixel size).
          Note: Actual size should be calculated in application code based on format and dimensions.
