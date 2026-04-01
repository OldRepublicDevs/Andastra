"""Verify TSL bytes at known function addresses look like real code."""
from __future__ import annotations

import struct

def parse_pe(path):
    with open(path,'rb') as f:
        data = f.read()
    e_lfanew = struct.unpack_from('<I', data, 0x3C)[0]
    num_sec  = struct.unpack_from('<H', data, e_lfanew+6)[0]
    opt_sz   = struct.unpack_from('<H', data, e_lfanew+20)[0]
    opt_off  = e_lfanew + 24
    image_base = struct.unpack_from('<I', data, opt_off+28)[0]
    sec_off = opt_off + opt_sz
    sections = []
    for i in range(num_sec):
        s = sec_off + i*40
        name    = data[s:s+8].rstrip(b'\x00').decode('ascii','replace')
        vsize   = struct.unpack_from('<I', data, s+8)[0]
        vaddr   = struct.unpack_from('<I', data, s+12)[0]
        raw_siz = struct.unpack_from('<I', data, s+16)[0]
        raw_off = struct.unpack_from('<I', data, s+20)[0]
        sections.append((name, vaddr, vsize, raw_off, raw_siz))
    return image_base, sections, data

def read_va(va, n, ib, sections, data):
    rva = va - ib
    for name, vaddr, vsize, raw_off, raw_siz in sections:
        if vaddr <= rva < vaddr + vsize:
            off = raw_off + (rva - vaddr)
            return data[off:off+n]
    return None

tsl_ib, tsl_sec, tsld = parse_pe('agentdecompile_projects/imports/swkotor2.exe')

checks = [
    ("CGameObject::~CGameObject",              0x0044F630),
    ("CSWCObject::CSWCObject",                 0x007FF960),
    ("CSWCObject::~CSWCObject",                0x007FA760),
    ("CSWCCreature::CSWCCreature",             0x007B4AB0),
    ("CSWCCreature::~CSWCCreature",            0x008C3600),
    ("CSWCModule::CSWCModule",                 0x009B8180),
    ("CSWSArea::CSWSArea",                     0x009BB800),
]

print("TSL byte reads at known function addresses:")
for name, va in checks:
    bs = read_va(va, 16, tsl_ib, tsl_sec, tsld)
    looks_like_prologue = bs and bs[:3] in (b'\x55\x8B\xEC', b'\x53\x56\x57', b'\x55\x89\xE5')
    hex_str = ' '.join(f'{b:02X}' for b in bs) if bs else 'NOT FOUND'
    flag = ' [VALID PROLOG]' if looks_like_prologue else ''
    print(f"  0x{va:08X}  {name}")
    print(f"    {hex_str}{flag}")
