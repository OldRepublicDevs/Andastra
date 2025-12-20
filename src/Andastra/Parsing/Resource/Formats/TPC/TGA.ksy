meta:
  id: tga
  title: Truevision TGA (Targa) Image File Format
  license: MIT
  endian: le
  file-extension:
    - tga
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/tpc/tga.py
    wiki: vendor/PyKotor/wiki/TPC-File-Format.md
doc: |
  TGA (Targa) is an uncompressed or RLE-compressed raster image format.
  Supports 8-bit grayscale, 24-bit RGB, and 32-bit RGBA pixel formats.

  Binary Format Structure:
  - Header (18 bytes): ID length, color map type, image type, dimensions, pixel depth
  - Optional ID field (variable length)
  - Optional color map data (variable length)
  - Image data (compressed RLE or uncompressed)
  - Optional footer (26 bytes with signature)

  References:
  - vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/tpc/tga.py

seq:
  - id: header
    type: tga_header
    doc: TGA file header (18 bytes)

  - id: image_id
    type: str
    encoding: ASCII
    size: header.id_length
    if: header.id_length > 0
    doc: Optional image identification field

  - id: color_map_data
    type: color_map_data_section
    if: header.color_map_type == 1
    doc: Optional color map data (palette)

  - id: image_data
    type: image_data_section
    doc: Pixel data (RLE compressed or uncompressed)

  - id: footer
    type: tga_footer
    if: _io.pos < _io.size
    doc: Optional TGA 2.0 footer

types:
  tga_header:
    seq:
      - id: id_length
        type: u1
        doc: |
          Length of image identification field (0-255 bytes).
          If 0, no image identification field follows.

      - id: color_map_type
        type: u1
        doc: |
          Color map type:
          - 0 = No color map
          - 1 = Color map present
        valid: [0, 1]

      - id: image_type
        type: u1
        doc: |
          Image type code:
          - 0 = No image data
          - 1 = Uncompressed color-mapped
          - 2 = Uncompressed true-color
          - 3 = Uncompressed black-and-white
          - 9 = Run-length encoded color-mapped
          - 10 = Run-length encoded true-color
          - 11 = Run-length encoded black-and-white
        valid: [0, 1, 2, 3, 9, 10, 11]

      - id: color_map_origin
        type: u2
        doc: |
          First entry index in color map (0-based).
          Only used if color_map_type == 1.

      - id: color_map_length
        type: u2
        doc: |
          Number of entries in color map.
          Only used if color_map_type == 1.

      - id: color_map_depth
        type: u1
        doc: |
          Number of bits per color map entry (15, 16, 24, or 32).
          Only used if color_map_type == 1.

      - id: x_origin
        type: u2
        doc: X coordinate of lower left corner of image

      - id: y_origin
        type: u2
        doc: Y coordinate of lower left corner of image

      - id: width
        type: u2
        doc: Image width in pixels

      - id: height
        type: u2
        doc: Image height in pixels

      - id: pixel_depth
        type: u1
        doc: |
          Bits per pixel:
          - 8 = Grayscale or indexed color
          - 16 = ARGB 1-5-5-5
          - 24 = RGB
          - 32 = ARGB
        valid: [8, 16, 24, 32]

      - id: image_descriptor
        type: u1
        doc: |
          Image descriptor byte:
          - Bits 0-3: Number of attribute bits per pixel (alpha channel depth)
          - Bit 4: Image origin (0 = bottom-left, 1 = top-left)
          - Bit 5: Interleaving flag (0 = non-interleaved, 1 = two-way interleaved)
          - Bits 6-7: Unused (must be 0)

    instances:
      is_rle:
        value: image_type == 9 || image_type == 10 || image_type == 11
        doc: True if image uses run-length encoding (RLE)

      is_uncompressed:
        value: image_type == 1 || image_type == 2 || image_type == 3
        doc: True if image is uncompressed

      is_true_color:
        value: image_type == 2 || image_type == 10
        doc: True if image is true-color (RGB/RGBA)

      is_grayscale:
        value: image_type == 3 || image_type == 11
        doc: True if image is grayscale

      origin_top_left:
        value: (image_descriptor & 0x20) != 0
        doc: True if image origin is top-left (otherwise bottom-left)

      alpha_depth:
        value: image_descriptor & 0x0F
        doc: Number of alpha channel bits per pixel

      bytes_per_pixel:
        value: pixel_depth / 8
        doc: Bytes per pixel (calculated from pixel_depth)

  color_map_data_section:
    seq:
      - id: entries
        type: color_map_entry
        repeat: expr
        repeat-expr: _root.header.color_map_length
        doc: Array of color map entries

  color_map_entry:
    seq:
      - id: data
        type: u1
        repeat: expr
        repeat-expr: _root.header.color_map_depth / 8
        doc: |
          Color map entry data.
          Format depends on color_map_depth:
          - 15/16 bits: ARGB 1-5-5-5 format
          - 24 bits: BGR
          - 32 bits: BGRA

  image_data_section:
    seq:
      - id: pixel_data
        type: pixel_data_block
        doc: Pixel data (RLE compressed or uncompressed)

  pixel_data_block:
    seq:
      - id: data
        type: u1
        repeat: until
        repeat-until: '_io.pos >= (_io.size - (_root.footer != null ? 26 : 0))'
        doc: |
          Pixel data bytes.
          For uncompressed: raw pixel data (width * height * bytes_per_pixel bytes).
          For RLE: compressed run-length encoded data (variable size).
          Note: RLE decoding should be performed in application code.

  tga_footer:
    seq:
      - id: extension_area_offset
        type: u4
        doc: Offset from beginning of file to extension area (0 if no extension)

      - id: developer_directory_offset
        type: u4
        doc: Offset from beginning of file to developer directory (0 if no directory)

      - id: signature
        size: 16
        type: str
        encoding: ASCII
        doc: TGA signature (should be "TRUEVISION-XFILE")
        valid: "TRUEVISION-XFILE"

      - id: terminator
        size: 2
        type: u1
        repeat: expr
        repeat-expr: 2
        doc: 'Binary terminator (period and null character: ".\\0")'
        valid: [0x2E, 0x00]
