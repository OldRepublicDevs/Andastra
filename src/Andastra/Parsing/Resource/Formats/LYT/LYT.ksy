meta:
  id: lyt
  title: BioWare LYT (Layout) File Format
  license: MIT
  endian: le
  encoding: ASCII
  file-extension: lyt
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/lyt/
    reone: vendor/reone/src/libs/resource/format/lytreader.cpp
    xoreos: vendor/xoreos/src/aurora/lytfile.cpp
    kotor_js: vendor/KotOR.js/src/resource/LYTObject.ts
    kotor_unity: vendor/KotOR-Unity/Assets/Scripts/FileObjects/LYTObject.cs
    kotor_net: vendor/Kotor.NET/Kotor.NET/Formats/KotorLYT/LYT.cs
    wiki: vendor/PyKotor/wiki/LYT-File-Format.md
doc: |
  LYT (Layout) files define how area geometry is assembled from room models and where
  interactive elements (doors, tracks, obstacles) are positioned. The game engine
  uses LYT files to load and position room models (MDL files) and determine
  door placement points for area transitions.
  
  Format Overview:
  - LYT files are ASCII text with a deterministic order
  - Structure: beginlayout, optional sections, then donelayout
  - Every section declares a count and then lists entries on subsequent lines
  - Sections: roomcount, trackcount, obstaclecount, doorhookcount
  
  Syntax:
  ```
  beginlayout
  roomcount <N>
    <room_model> <x> <y> <z>
  trackcount <N>
    <track_model> <x> <y> <z>
  obstaclecount <N>
    <obstacle_model> <x> <y> <z>
  doorhookcount <N>
    <room_name> <door_name> <x> <y> <z> <qx> <qy> <qz> <qw> [optional floats]
  donelayout
  ```
  
  Note: Since LYT is a line-based ASCII text format, this Kaitai Struct definition
  provides the raw text content. Actual parsing of rooms, tracks, obstacles, and
  doorhooks should be done by the application code, splitting by lines and parsing tokens.
  
  All implementations (vendor/reone, vendor/xoreos, vendor/KotOR.js, vendor/Kotor.NET)
  parse identical tokens. KotOR-Unity mirrors the same structure.
  
  References:
  - vendor/PyKotor/wiki/LYT-File-Format.md
  - vendor/reone/src/libs/resource/format/lytreader.cpp:37-77
  - vendor/xoreos/src/aurora/lytfile.cpp
  - Libraries/PyKotor/src/pykotor/resource/formats/lyt/io_lyt.py:17-165

seq:
  - id: raw_content
    type: str
    size-eos: true
    encoding: ASCII
    doc: |
      Raw ASCII text content of the entire LYT file.
      The file format is:
      - Header: "beginlayout" followed by newline (\n or \r\n)
      - Sections (optional, in order):
        * roomcount <N> followed by N room entries (<model> <x> <y> <z>)
        * trackcount <N> followed by N track entries (<model> <x> <y> <z>)
        * obstaclecount <N> followed by N obstacle entries (<model> <x> <y> <z>)
        * doorhookcount <N> followed by N doorhook entries (<room> <door> 0 <x> <y> <z> <qx> <qy> <qz> <qw>)
      - Footer: "donelayout"
      
      Application code should parse raw_content line-by-line to extract structured data.
      See Libraries/PyKotor/src/pykotor/resource/formats/lyt/io_lyt.py for reference implementation.
