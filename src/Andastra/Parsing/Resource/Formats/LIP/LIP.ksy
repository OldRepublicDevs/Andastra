meta:
  id: lip
  title: BioWare LIP (LIP Synchronization) File
  license: MIT
  endian: le
  file-extension: lip
  xref:
    pykotor: Libraries/PyKotor/src/pykotor/resource/formats/lip/
    reone: vendor/reone/src/libs/graphics/format/lipreader.cpp
    xoreos: vendor/xoreos/src/graphics/aurora/lipfile.cpp
    kotorjs: vendor/KotOR.js/src/resource/LIPObject.ts
    kotornet: vendor/Kotor.NET/Kotor.NET/Formats/KotorLIP/LIP.cs
doc: |
  LIP (LIP Synchronization) files drive mouth animation for voiced dialogue in BioWare games.
  Each file contains a compact series of keyframes that map timestamps to discrete viseme
  (mouth shape) indices so that the engine can interpolate character lip movement while
  playing the companion WAV audio line.
  
  LIP files are always binary and contain only animation data. They are paired with WAV
  voice-over resources of identical duration; the LIP length field must match the WAV
  playback time for glitch-free animation.
  
  Keyframes are sorted chronologically and store a timestamp (float seconds) plus a
  1-byte viseme index (0-15). The format uses the 16-shape Preston Blair phoneme set.
  
  References:
  - vendor/PyKotor/wiki/LIP-File-Format.md
  - vendor/reone/src/libs/graphics/format/lipreader.cpp:27-42
  - vendor/xoreos/src/graphics/aurora/lipfile.cpp
  - vendor/KotOR.js/src/resource/LIPObject.ts:93-146

seq:
  - id: file_type
    type: str
    encoding: ASCII
    size: 4
    doc: File type signature. Must be "LIP " (space-padded) for LIP files.
  
  - id: file_version
    type: str
    encoding: ASCII
    size: 4
    doc: File format version. Must be "V1.0" for LIP files.
  
  - id: length
    type: f4
    doc: |
      Duration in seconds. Must equal the paired WAV file playback time for
      glitch-free animation. This is the total length of the lip sync animation.
  
  - id: num_keyframes
    type: u4
    doc: |
      Number of keyframes immediately following. Each keyframe contains a timestamp
      and a viseme shape index. Keyframes should be sorted ascending by timestamp.
  
  - id: keyframes
    type: keyframe_entry
    repeat: expr
    repeat-expr: num_keyframes
    doc: |
      Array of keyframe entries. Each entry maps a timestamp to a mouth shape.
      Entries must be stored in chronological order (ascending by timestamp).

types:
  keyframe_entry:
    doc: |
      A single keyframe entry mapping a timestamp to a viseme (mouth shape).
      Keyframes are used by the engine to interpolate between mouth shapes during
      audio playback to create lip sync animation.
    seq:
      - id: timestamp
        type: f4
        doc: |
          Seconds from animation start. Must be >= 0 and <= length.
          Keyframes should be sorted ascending by timestamp.
      
      - id: shape
        type: u1
        enum: lip_shapes
        doc: |
          Viseme index (0-15) indicating which mouth shape to use at this timestamp.
          Uses the 16-shape Preston Blair phoneme set. See lip_shapes enum for details.
    
enums:
  lip_shapes:
    0: neutral
    1: ee
    2: eh
    3: ah
    4: oh
    5: ooh
    6: y
    7: sts
    8: fv
    9: ng
    10: th
    11: mpb
    12: td
    13: sh
    14: l
    15: kg

