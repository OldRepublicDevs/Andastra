meta:
  id: nss
  title: BioWare NSS (NWScript Source) File
  license: MIT
  endian: le
  file-extension: nss
  encoding: UTF-8
  xref:
    pykotor: vendor/PyKotor/Libraries/PyKotor/src/pykotor/resource/formats/ncs/
    wiki: vendor/PyKotor/wiki/NSS-File-Format.md
doc: |
  NSS (NWScript Source) files contain human-readable NWScript source code
  that compiles to NCS bytecode. NWScript is the scripting language used
  in KotOR, TSL, and Neverwinter Nights.
  
  NSS files are plain text files (typically Windows-1252 or UTF-8 encoding)
  containing NWScript source code. The nwscript.nss file defines all
  engine-exposed functions and constants available to scripts.
  
  Format:
  - Plain text source code
  - May include BOM (Byte Order Mark) for UTF-8 files
  - Lines are typically terminated with CRLF (\r\n) or LF (\n)
  - Comments: // for single-line, /* */ for multi-line
  - Preprocessor directives: #include, #define, etc.
  
  References:
  - vendor/PyKotor/wiki/NSS-File-Format.md
  - vendor/xoreos-tools/src/nwscript/ (NWScript compiler)
  - vendor/reone/src/libs/script/ (Script execution engine)

seq:
  - id: bom
    type: u2
    if: _io.pos == 0
    doc: |
      Optional UTF-8 BOM (Byte Order Mark) at the start of the file.
      If present, will be 0xFEFF (UTF-8 BOM).
      Most NSS files do not include a BOM.
    valid: [0xFEFF, 0x0000]
  
  - id: source_code
    type: str
    size-eos: true
    encoding: UTF-8
    doc: |
      Complete NWScript source code.
      Contains function definitions, variable declarations, control flow
      statements, and engine function calls.
      
      Common elements:
      - Function definitions: void function_name() { ... }
      - Variable declarations: int variable_name;
      - Control flow: if, while, for, switch
      - Engine function calls: GetFirstObject(), GetObjectByTag(), etc.
      - Constants: OBJECT_SELF, OBJECT_INVALID, etc.
      
      The source code is compiled to NCS bytecode by the NWScript compiler.

types:
  nss_source:
    doc: |
      NWScript source code structure.
      This is primarily a text format, so the main content is the source_code string.
      
      The source can be parsed into:
      - Tokens (keywords, identifiers, operators, literals)
      - Statements (declarations, assignments, control flow)
      - Functions (definitions with parameters and body)
      - Preprocessor directives (#include, #define)
    
    seq:
      - id: content
        type: str
        size-eos: true
        encoding: UTF-8
        doc: Complete source code content.

