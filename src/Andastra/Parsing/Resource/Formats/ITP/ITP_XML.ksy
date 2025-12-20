meta:
  id: itp_xml
  title: BioWare ITP XML Format
  license: MIT
  endian: le
  file-extension: itp.xml
  encoding: UTF-8
doc: |
  ITP XML format is a human-readable XML representation of ITP (Palette) binary files.
  ITP files use GFF format (FileType "ITP " in GFF header).
  Uses GFF XML structure with root element <gff3> containing <struct> elements.
  Each field has a label attribute and appropriate type element (byte, uint32, exostring, etc.).

seq:
  - id: xml_content
    type: str
    size-eos: true
    encoding: UTF-8
    doc: XML document content as UTF-8 text

