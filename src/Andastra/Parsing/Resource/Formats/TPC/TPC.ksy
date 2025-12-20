meta:
  id: tpc
  title: BioWare TPC (Texture Pack Container) File Format
  license: MIT
  endian: le
  file-extension:
    - tpc
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/tpc/
    reone: vendor/reone/src/libs/graphics/format/tpcreader.cpp
    xoreos: vendor/xoreos/src/graphics/images/tpc.cpp
    wiki: vendor/PyKotor/wiki/TPC-File-Format.md
doc: |
  TPC (Texture Pack Container) is KotOR's native texture format. It supports paletteless RGB/RGBA,
  greyscale, and block-compressed DXT1/DXT3/DXT5 data, optional mipmaps, cube maps, and flipbook
  animations controlled by companion TXI files.

  Binary Format Structure:
  - Header (128 bytes): data_size, alpha_test, width, height, pixel_encoding, mipmap_count, reserved
  - Texture Data: Per layer, per mipmap compressed/uncompressed pixel data
  - Optional TXI Footer: ASCII text metadata appended after texture data

  References:
  - vendor/PyKotor/wiki/TPC-File-Format.md
  - vendor/reone/src/libs/graphics/format/tpcreader.cpp
  - vendor/xoreos/src/graphics/images/tpc.cpp

seq:
  - id: header
    type: tpc_header
    doc: TPC file header (128 bytes total)

  - id: texture_data
    type: texture_data_section
    doc: Texture pixel data (compressed or uncompressed) for all layers and mipmaps

  - id: txi_footer
    type: txi_footer_section
    if: _io.pos < _io.size
    doc: Optional ASCII TXI metadata appended after texture data

types:
  tpc_header:
    seq:
      - id: data_size
        type: u4
        doc: |
          Total compressed payload size. If non-zero, texture is compressed (DXT).
          If zero, texture is uncompressed and size is derived from format/width/height.

      - id: alpha_test
        type: f4
        doc: |
          Float threshold used by punch-through rendering.
          Commonly 0.0 or 0.5.

      - id: width
        type: u2
        doc: |
          Texture width in pixels (uint16).
          Must be power-of-two for compressed formats.

      - id: height
        type: u2
        doc: |
          Texture height in pixels (uint16).
          For cube maps, this is 6x the face width.
          Must be power-of-two for compressed formats.

      - id: pixel_encoding
        type: u1
        doc: |
          Pixel encoding flag:
          - 0x01 = Greyscale (8-bit luminance)
          - 0x02 = RGB (24-bit) or DXT1 (if compressed)
          - 0x04 = RGBA (32-bit) or DXT5 (if compressed)
          - 0x0C = BGRA (32-bit, Xbox-specific swizzle)

      - id: mipmap_count
        type: u1
        doc: |
          Number of mip levels per layer (minimum 1).
          Each mip level is half the size of the previous level.

      - id: reserved
        size: 114
        type: u1
        repeat: expr
        repeat-expr: 114
        doc: |
          Reserved/padding bytes (0x72 = 114 bytes).
          KotOR stores platform hints here but all implementations skip them.

    instances:
      is_compressed:
        value: data_size != 0
        doc: True if texture data is compressed (DXT format)

      is_uncompressed:
        value: data_size == 0
        doc: True if texture data is uncompressed (raw pixels)

  texture_data_section:
    seq:
      - id: layers
        type: texture_layer
        repeat: expr
        repeat-expr: |
          (_root.header.is_compressed && _root.header.height != 0 && _root.header.width != 0 && (_root.header.height / _root.header.width) == 6) ?
            6 : 1
        doc: Array of texture layers (1 for regular, 6 for cube maps)

  texture_layer:
    seq:
      - id: mipmaps
        type: texture_mipmap
        repeat: expr
        repeat-expr: _root.header.mipmap_count
        doc: Array of mipmap levels for this layer

  texture_mipmap:
    seq:
      - id: pixel_data
        type: pixel_data_block
        doc: Compressed or uncompressed pixel data for this mipmap level

  pixel_data_block:
    seq:
      - id: data
        type: u1
        repeat: until
        repeat-until: _io.pos >= _io.size || (_root.txi_footer != null && _io.pos >= (_io.size - _root.txi_footer._io.size))
        doc: |
          Pixel data bytes (size calculated based on format and mipmap level).
          For DXT: block-compressed data (8 bytes per 4x4 block for DXT1, 16 for DXT3/DXT5).
          For uncompressed: raw pixel data (1 byte per pixel for greyscale, 3 for RGB, 4 for RGBA/BGRA).
          Note: Actual size should be calculated in application code based on format, width, height, and mip level.

  txi_footer_section:
    seq:
      - id: txi_data
        type: str
        encoding: ASCII
        size-eos: true
        doc: |
          ASCII TXI metadata appended after texture data.
          Contains texture rendering properties, animation parameters, etc.
          Parsed as line-based command/value pairs.
