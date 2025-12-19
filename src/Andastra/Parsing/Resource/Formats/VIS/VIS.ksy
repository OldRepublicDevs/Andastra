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
    kotor_js: vendor/KotOR.js/src/resource/VISObject.ts
    kotor_unity: vendor/KotOR-Unity/Assets/Scripts/FileObjects/VISObject.cs
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
  - Empty lines are ignored
  
  Example:
  ```
  room012 3
    room013
    room014
    room015
  ```
  
  Parsing Rules:
  1. Lines are processed sequentially
  2. Empty lines (whitespace only) are ignored
  3. Version header lines (second token starts with "V") are skipped
  4. Parent lines contain room name and child count (e.g., "room012 3")
  5. Child lines are indented with 2 spaces and contain room name
  6. After a parent line, exactly CHILD_COUNT child lines must follow
  7. Room names are case-insensitive (stored lowercase)
  
  Runtime Behavior:
  - When the player stands in room A, the engine renders any room listed under A
  - VIS files only control visibility; collision and pathfinding rely on walkmeshes
  - Editing VIS entries is a common optimization for performance
  - Missing VIS files cause all rooms to render (performance impact)
  
  Performance Impact:
  - Without VIS: Engine renders all rooms, even those behind walls/doors
  - With VIS: Only visible rooms are submitted to the renderer (10-50x fewer draw calls)
  - Overly restrictive VIS: Causes pop-in where rooms suddenly appear
  - Too permissive VIS: Wastes GPU resources rendering unseen geometry
  
  Common Issues:
  - Missing Room: Room not in any VIS list → never renders → appears invisible
  - One-way Visibility: Room A lists B, but B doesn't list A → asymmetric rendering
  - Performance Problems: All rooms list each other → defeats purpose of VIS optimization
  - Doorway Artifacts: Door rooms not mutually visible → walls clip during door animations
  
  References:
  - vendor/PyKotor/wiki/VIS-File-Format.md
  - vendor/reone/src/libs/resource/format/visreader.cpp
  - vendor/xoreos/src/aurora/visfile.cpp
  - Libraries/PyKotor/src/pykotor/resource/formats/vis/io_vis.py:14-87
  - Libraries/PyKotor/src/pykotor/resource/formats/vis/vis_data.py:52-294

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
      
      Application code should:
      1. Split raw_content by newlines (\r\n, \n, or \r)
      2. Skip empty lines (whitespace only)
      3. Skip version header lines (second token starts with "V")
      4. Parse parent lines: "ROOM_NAME CHILD_COUNT"
      5. Parse child lines: "  ROOM_NAME" (indented with 2 spaces)
      6. Validate that CHILD_COUNT child lines follow each parent line
      7. Store room names in lowercase for case-insensitive comparison
      
      See VISAsciiReader.cs for reference implementation matching
      Libraries/PyKotor/src/pykotor/resource/formats/vis/io_vis.py:14-73.
