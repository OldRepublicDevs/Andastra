meta:
  id: txi
  title: BioWare TXI (Texture Info) File Format
  license: MIT
  endian: le
  file-extension:
    - txi
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/txi/
    reone: vendor/reone/src/libs/graphics/format/txireader.cpp
    xoreos: vendor/xoreos/src/graphics/images/txi.cpp
    wiki: vendor/PyKotor/wiki/TXI-File-Format.md
doc: |
  TXI (Texture Info) files are ASCII text files that provide metadata for TPC textures.
  They specify rendering properties (blending modes, mipmaps, filtering), companion textures
  (bump maps, environment maps), font metrics for bitmap fonts, and animation parameters
  for flipbook textures.
  
  Format Overview:
  - TXI files are plain-text key/value lists
  - Commands are case-insensitive but conventionally lowercase
  - Values can be integers, floats, booleans (0/1), strings, or multi-line coordinate tables
  - A single TXI can be appended to the end of a .tpc file or shipped as a sibling .txi file
  
  References:
  - vendor/PyKotor/wiki/TXI-File-Format.md
  - vendor/reone/src/libs/graphics/format/txireader.cpp
  - vendor/xoreos/src/graphics/images/txi.cpp

seq:
  - id: content
    type: str
    encoding: ASCII
    size-eos: true
    doc: |
      Complete TXI file content as ASCII text.
      Contains line-based command/value pairs that should be parsed by application code.
      
      Common commands include:
      - mipmap <0|1>
      - blending <default|additive|punchthrough>
      - filter <0|1>
      - decal <0|1>
      - cube <0|1>
      - proceduretype <cycle>
      - numx <count>
      - numy <count>
      - fps <float>
      - And many more...
      
      Coordinate blocks (upperleftcoords, lowerrightcoords) use multi-line format:
      upperleftcoords <count>
      <u> <v> <z>
      <u> <v> <z>
      ...
