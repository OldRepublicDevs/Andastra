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
    valid: "LIP "
  
  - id: file_version
    type: str
    encoding: ASCII
    size: 4
    doc: File format version. Must be "V1.0" for LIP files.
    valid: "V1.0"
  
  - id: length
    type: f4
    doc: |
      Duration in seconds. Must equal the paired WAV file playback time for
      glitch-free animation. This is the total length of the lip sync animation.
  
  - id: entry_count
    type: u4
    doc: |
      Number of keyframes immediately following. Each keyframe contains a timestamp
      and a viseme shape index. Keyframes should be sorted ascending by timestamp.
  
  - id: keyframes
    type: keyframe_entry
    repeat: expr
    repeat-expr: entry_count
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
      doc: Rest/closed mouth position. Used for pauses and neutral state.
    1: ee
      doc: Teeth apart, wide smile (long "ee" sound). Used for "see", "be", "me" sounds.
    2: eh
      doc: Relaxed mouth ("eh" sound). Used for "get", "bet", "head" sounds.
    3: ah
      doc: Mouth open ("ah/aa" sound). Used for "father", "cat", "but" sounds.
    4: oh
      doc: Rounded lips ("oh" sound). Used for "go", "bought", "low" sounds.
    5: ooh
      doc: Pursed lips ("oo", "w" sound). Used for "too", "wow", "who" sounds.
    6: y
      doc: Slight smile ("y" sound). Used for "yes", "you", "year" sounds.
    7: sts
      doc: Teeth touching ("s", "z", "ts" sounds). Used for "stop", "see", "zoo" sounds.
    8: fv
      doc: Lower lip touches upper teeth ("f", "v" sounds). Used for "five", "fee", "view" sounds.
    9: ng
      doc: Tongue raised ("n", "ng" sounds). Used for "ring", "sing", "no" sounds.
    10: th
      doc: Tongue between teeth ("th" sound). Used for "thin", "thee", "this" sounds.
    11: mpb
      doc: Lips closed ("m", "p", "b" sounds). Used for "bump", "map", "be" sounds.
    12: td
      doc: Tongue up ("t", "d" sounds). Used for "top", "dee", "time" sounds.
    13: sh
      doc: Rounded relaxed ("sh", "ch", "j" sounds). Used for "measure", "cheese", "joy" sounds.
    14: l
      doc: Tongue forward ("l", "r" sounds). Used for "lip", "red", "light" sounds.
    15: kg
      doc: Back of tongue raised ("k", "g", "h" sounds). Used for "kick", "green", "he" sounds.

