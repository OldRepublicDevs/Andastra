meta:
  id: vis
  title: BioWare VIS (Visibility) File Format
  license: MIT
  endian: le
  file-extension: vis
  encoding: ASCII
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/vis/
    reone: vendor/reone/src/libs/resource/format/visreader.cpp
    xoreos: vendor/xoreos/src/aurora/visfile.cpp
    wiki: vendor/PyKotor/wiki/VIS-File-Format.md
doc: |
  VIS (Visibility) files describe which module rooms can be seen from other rooms.
  They drive the engine's occlusion culling so that only geometry visible from the
  player's current room is rendered, reducing draw calls and overdraw.
  
  Format Overview:
  - VIS files are plain ASCII text
  - Each parent room line lists how many child rooms follow
  - Child room lines are indented by two spaces
  - Empty lines are ignored and names are case-insensitive
  - Files usually ship as moduleXXX.vis pairs
  
  File Layout:
  - Parent Lines: "ROOM_NAME CHILD_COUNT"
  - Child Lines: "  ROOM_NAME" (indented with 2 spaces)
  - Version headers (e.g., "room V3.28") are skipped
  
  Example:
  ```
  room012 3
    room013
    room014
    room015
  ```
  
  References:
  - vendor/PyKotor/wiki/VIS-File-Format.md
  - vendor/reone/src/libs/resource/format/visreader.cpp
  - vendor/xoreos/src/aurora/visfile.cpp

seq:
  - id: raw_content
    type: str
    encoding: ASCII
    size-eos: true
    doc: |
      Raw ASCII text content of the entire VIS file.
      The generated C# code parses this text into structured VIS entries
      by splitting on newlines and processing each line according to the
      VIS format specification (parent lines with child counts, indented
      child lines, empty lines, version headers).

types:
  vis_line:
    doc: |
      A single line in a VIS file. Can be:
      - Empty line (ignored during parsing)
      - Version header line (e.g., "room V3.28", skipped during parsing)
      - Parent line (e.g., "room012 3" - room name followed by child count)
      - Child line (e.g., "  room013" - indented with 2 spaces)
    
    seq:
      - id: line_content
        type: str
        encoding: ASCII
        terminator: 0x0A
        consume: false
        doc: |
          Single line of text, terminated by newline (0x0A).
          May include carriage return (0x0D) before newline on Windows.
          The terminator is not consumed so we can detect line boundaries.

