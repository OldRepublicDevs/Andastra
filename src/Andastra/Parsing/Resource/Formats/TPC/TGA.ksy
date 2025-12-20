meta:
  id: tga
  title: Truevision TGA (Targa) File Format
  license: MIT
  endian: le
  file-extension:
    - tga
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/tpc/tga.py
    wiki: vendor/PyKotor/wiki/TPC-File-Format.md
doc: |
  Truevision TGA (Targa) is an uncompressed image format supporting RGB, RGBA, and grayscale.
  Used by KotOR for texture assets. Supports both raw and RLE-compressed pixel data.
  
  Binary Format Structure:
  - Header (18 bytes): ID length, color map type, image type, dimensions, pixel depth, descriptor
  - Image ID: Optional image identification field
  - Color Map Data: Optional color palette (not used in KotOR)
  - Image Data: Raw or RLE-compressed pixel data
  
  References:
  - vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/tpc/tga.py

seq:
  - id: header
    type: tga_header
    doc: TGA file header (18 bytes)
  
  - id: image_id
    type: image_id_field
    if: header.id_length > 0
    doc: Optional image identification field
  
  - id: color_map_data
    type: color_map_data_field
    if: header.color_map_type != 0
    doc: Optional color map data (not used in KotOR)
  
  - id: image_data
    type: image_data_field
    doc: Raw or RLE-compressed image pixel data

types:
  tga_header:
    seq:
      - id: id_length
        type: u1
        doc: |
          Length of image identification field (0-255 bytes).
          If 0, no image ID field is present.
      
      - id: color_map_type
        type: u1
        doc: |
          Color map type:
          - 0 = No color map
          - 1 = Color map present (not supported in KotOR)
        valid: [0, 1]
      
      - id: image_type
        type: u1
        doc: |
          Image type code:
          - 2 = Uncompressed true-color image
          - 3 = Uncompressed black-and-white image (grayscale)
          - 10 = Run-length encoded true-color image
          - 11 = Run-length encoded black-and-white image
        valid: [2, 3, 10, 11]
      
      - id: color_map_origin
        type: u2
        doc: |
          First color map entry index (unused if color_map_type == 0).
          Little-endian uint16.
      
      - id: color_map_length
        type: u2
        doc: |
          Number of color map entries (unused if color_map_type == 0).
          Little-endian uint16.
      
      - id: color_map_depth
        type: u1
        doc: |
          Bits per color map entry (unused if color_map_type == 0).
          Typically 15, 16, 24, or 32.
      
      - id: x_origin
        type: s2
        doc: |
          X coordinate of lower-left corner of image.
          Usually 0. Signed 16-bit integer, little-endian.
      
      - id: y_origin
        type: s2
        doc: |
          Y coordinate of lower-left corner of image.
          Usually 0. Signed 16-bit integer, little-endian.
      
      - id: width
        type: u2
        doc: |
          Image width in pixels.
          Little-endian uint16.
      
      - id: height
        type: u2
        doc: |
          Image height in pixels.
          Little-endian uint16.
      
      - id: pixel_depth
        type: u1
        doc: |
          Bits per pixel:
          - 8 = Grayscale
          - 24 = RGB (8 bits per channel)
          - 32 = RGBA (8 bits per channel)
        valid: [8, 24, 32]
      
      - id: descriptor
        type: u1
        doc: |
          Image descriptor byte:
          - Bits 0-3: Number of alpha channel bits (0-15)
          - Bit 4: Screen origin (0 = bottom-left, 1 = top-left)
          - Bit 5: Interleaving flag (0 = non-interleaved, 1 = interleaved)
          - Bits 6-7: Reserved
    instances:
      has_image_id:
        value: id_length > 0
        doc: True if image ID field is present
      
      has_color_map:
        value: color_map_type != 0
        doc: True if color map is present
      
      is_compressed:
        value: image_type == 10 || image_type == 11
        doc: True if image data is RLE-compressed
      
      is_grayscale:
        value: image_type == 3 || image_type == 11 || pixel_depth == 8
        doc: True if image is grayscale
      
      is_color:
        value: image_type == 2 || image_type == 10 || pixel_depth >= 24
        doc: True if image is color (RGB/RGBA)
      
      alpha_bits:
        value: descriptor & 0x0F
        doc: Number of alpha channel bits
      
      origin_top_left:
        value: (descriptor & 0x20) != 0
        doc: True if image origin is top-left, false if bottom-left
      
      bytes_per_pixel:
        value: pixel_depth / 8
        doc: Number of bytes per pixel

  image_id_field:
    seq:
      - id: data
        type: u1
        repeat: expr
        repeat-expr: _root.header.id_length
        doc: Image identification field data

  color_map_data_field:
    seq:
      - id: entries
        type: u1
        repeat: expr
        repeat-expr: _root.header.color_map_length * (_root.header.color_map_depth / 8)
        doc: Color map entries (not used in KotOR)

  image_data_field:
    seq:
      - id: pixel_data
        type: u1
        repeat: until
        repeat-until: _io.pos >= _io.size
        doc: |
          Raw or RLE-compressed pixel data.
          For uncompressed: width * height * bytes_per_pixel bytes of raw pixel data.
          For RLE-compressed: variable-length run-length encoded data.
          Note: RLE decoding should be performed in application code.
          Pixel order depends on origin_top_left flag (flip vertically if bottom-left origin).
