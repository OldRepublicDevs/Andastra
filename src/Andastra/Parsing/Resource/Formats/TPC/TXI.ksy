meta:
  id: txi
  title: BioWare TXI (Texture Info) File Format
  license: MIT
  endian: le
  file-extension:
    - txi
  encoding: ASCII
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/txi/
    reone: vendor/reone/src/libs/graphics/format/txireader.cpp
    xoreos: vendor/xoreos/src/graphics/images/txi.cpp
    wiki: vendor/PyKotor/wiki/TXI-File-Format.md
doc: |
  TXI (Texture Info) files are compact ASCII descriptors that attach metadata to TPC textures.
  They control mipmap usage, filtering, flipbook animation, environment mapping, font atlases,
  and platform-specific downsampling.

  Format Overview:
  - TXI files are plain-text key/value lists
  - Each command modifies a field in the TPC runtime metadata
  - Commands are case-insensitive but conventionally lowercase
  - Values can be integers, floats, booleans (0/1), ResRefs, or multi-line coordinate tables

  References:
  - vendor/PyKotor/wiki/TXI-File-Format.md
  - vendor/reone/src/libs/graphics/format/txireader.cpp
  - vendor/xoreos/src/graphics/images/txi.cpp

seq:
  - id: txi_content
    type: str
    encoding: ASCII
    size-eos: true
    doc: |
      ASCII text content containing TXI commands.
      Parsed as line-based command/value pairs.
      Commands include: mipmap, blending, proceduretype, numx, numy, fps, etc.
