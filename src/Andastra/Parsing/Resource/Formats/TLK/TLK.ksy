meta:
  id: tlk
  title: BioWare TLK (Talk Table) File Format
  license: MIT
  endian: le
  file-extension:
    - tlk
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/tlk/
    reone: vendor/reone/src/libs/resource/format/tlkreader.cpp
    xoreos: vendor/xoreos/src/aurora/talktable.cpp
    wiki: vendor/PyKotor/wiki/TLK-File-Format.md
doc: |
  TLK (Talk Table) files contain all text strings used in the game, both written and spoken.
  They enable easy localization by providing a lookup table from string reference numbers (StrRef)
  to localized text and associated voice-over audio files.

  Binary Format:
  - Header (20 bytes): File type, version, language ID, string count, entries offset
  - String Data Table (40 bytes per entry): Metadata for each string entry
  - String Entries (variable size): Null-terminated text strings

  References:
  - vendor/PyKotor/wiki/TLK-File-Format.md
  - vendor/reone/src/libs/resource/format/tlkreader.cpp:31-84
  - vendor/xoreos/src/aurora/talktable.cpp:42-176
  - vendor/TSLPatcher/lib/site/Bioware/TLK.pm:1-533

seq:
  - id: header
    type: tlk_header
    doc: TLK file header (20 bytes)

  - id: string_data_table
    type: string_data_table
    pos: 20
    doc: Array of string data entries (metadata for each string)

types:
  tlk_header:
    seq:
      - id: file_type
        type: str
        encoding: ASCII
        size: 4
        doc: |
          File type signature. Must be "TLK " (space-padded).
          Validates that this is a TLK file.
        valid: "TLK "

      - id: file_version
        type: str
        encoding: ASCII
        size: 4
        doc: |
          File format version. "V3.0" for KotOR, "V4.0" for Jade Empire.
          KotOR games use V3.0.
        valid: ["V3.0", "V4.0"]

      - id: language_id
        type: u4
        doc: |
          Language identifier:
          - 0 = English
          - 1 = French
          - 2 = German
          - 3 = Italian
          - 4 = Spanish
          - 5 = Polish
          - 128 = Korean
          - 129 = Chinese Traditional
          - 130 = Chinese Simplified
          - 131 = Japanese
          See Language enum for complete list.

      - id: string_count
        type: u4
        doc: |
          Number of string entries in the file.
          Determines the number of entries in string_data_table.

      - id: entries_offset
        type: u4
        doc: |
          Byte offset to string entries array from the beginning of the file.
          Typically 20 + (string_count * 40) = header size + string data table size.
          Points to where the actual text strings begin.

    instances:
      header_size:
        value: 20
        doc: Size of the TLK header in bytes

      expected_entries_offset:
        value: 20 + (_root.string_count * 40)
        doc: |
          Expected offset to string entries (header + string data table).
          Used for validation.

  string_data_table:
    seq:
      - id: entries
        type: string_data_entry
        repeat: expr
        repeat-expr: _root.header.string_count
        doc: Array of string data entries, one per string in the file

  string_data_entry:
    seq:
      - id: flags
        type: u4
        doc: |
          Bit flags indicating what data is present:
          - bit 0 (0x0001): Text present - string has text content
          - bit 1 (0x0002): Sound present - string has associated voice-over audio
          - bit 2 (0x0004): Sound length present - sound length field is valid

          Common flag combinations:
          - 0x0001: Text only (menu options, item descriptions)
          - 0x0003: Text + Sound (voiced dialog lines)
          - 0x0007: Text + Sound + Length (fully voiced with duration)
          - 0x0000: Empty entry (unused StrRef slots)

      - id: sound_resref
        type: str
        encoding: ASCII
        size: 16
        doc: |
          Voice-over audio filename (ResRef), null-terminated ASCII, max 16 chars.
          If the string is shorter than 16 bytes, it is null-padded.
          Empty string (all nulls) indicates no voice-over audio.

      - id: volume_variance
        type: u4
        doc: |
          Volume variance (unused in KotOR, always 0).
          Legacy field from Neverwinter Nights, not used by KotOR engine.

      - id: pitch_variance
        type: u4
        doc: |
          Pitch variance (unused in KotOR, always 0).
          Legacy field from Neverwinter Nights, not used by KotOR engine.

      - id: text_offset
        type: u4
        doc: |
          Offset to string text relative to entries_offset.
          The actual file offset is: header.entries_offset + text_offset.
          First string starts at offset 0, subsequent strings follow sequentially.

      - id: text_length
        type: u4
        doc: |
          Length of string text in bytes (not characters).
          For single-byte encodings (Windows-1252, etc.), byte length equals character count.
          For multi-byte encodings (UTF-8, etc.), byte length may be greater than character count.

      - id: sound_length
        type: f4
        doc: |
          Duration of voice-over audio in seconds (float).
          Only valid if sound_length_present flag (bit 2) is set.
          Used by the engine to determine how long to wait before auto-advancing dialog.

    instances:
      text_present:
        value: (flags & 0x0001) != 0
        doc: Whether text content exists (bit 0 of flags)

      sound_present:
        value: (flags & 0x0002) != 0
        doc: Whether voice-over audio exists (bit 1 of flags)

      sound_length_present:
        value: (flags & 0x0004) != 0
        doc: Whether sound length is valid (bit 2 of flags)

      sound_resref_trimmed:
        value: sound_resref.rstrip("\0")
        doc: Sound ResRef with trailing null bytes removed

      text_file_offset:
        value: _root.header.entries_offset + text_offset
        doc: |
          Absolute file offset to the text string.
          Calculated as entries_offset (from header) + text_offset (from entry).

      text_data:
        pos: text_file_offset
        type: bytes
        size: text_length
        doc: |
          Text string data as raw bytes. The encoding depends on the language_id in the header.
          Common encodings:
          - English/French/German/Italian/Spanish: Windows-1252 (cp1252)
          - Polish: Windows-1250 (cp1250)
          - Korean: EUC-KR (cp949)
          - Chinese Traditional: Big5 (cp950)
          - Chinese Simplified: GB2312 (cp936)
          - Japanese: Shift-JIS (cp932)

          Note: Kaitai Struct reads this as raw bytes. The application layer
          should decode based on the language_id field in the header.

          In practice, strings are stored sequentially starting at entries_offset,
          so text_offset values are relative to entries_offset (0, len1, len1+len2, etc.).

          Strings may be null-terminated, but text_length includes the null terminator.
          Application code should trim null bytes when decoding.

      entry_size:
        value: 40
        doc: Size of each string_data_entry in bytes (flags + sound_resref + volume_variance + pitch_variance + text_offset + text_length + sound_length)

