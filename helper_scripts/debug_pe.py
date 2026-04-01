"""One-shot PE-header debug: prints section table and verifies byte reads at known addresses."""

from __future__ import annotations

import struct


def dump_pe(path: str) -> tuple[int, list[tuple[str, int, int, int, int]], bytes]:
    with open(path, "rb") as f:
        data = f.read()

    e_lfanew = struct.unpack_from("<I", data, 0x3C)[0]
    sig = data[e_lfanew : e_lfanew + 4]
    print(f"e_lfanew=0x{e_lfanew:X}  PE sig={sig}")

    machine = struct.unpack_from("<H", data, e_lfanew + 4)[0]
    num_sec = struct.unpack_from("<H", data, e_lfanew + 6)[0]
    opt_sz = struct.unpack_from("<H", data, e_lfanew + 20)[0]
    print(f"Machine=0x{machine:04X}  NumSections={num_sec}  OptHdrSz={opt_sz} (0x{opt_sz:X})")

    opt_off = e_lfanew + 24
    magic = struct.unpack_from("<H", data, opt_off)[0]
    image_base = struct.unpack_from("<I", data, opt_off + 28)[0]
    print(f"OptHdr magic=0x{magic:04X}  ImageBase=0x{image_base:08X}")

    sec_off = opt_off + opt_sz
    print(f"Section table at file offset 0x{sec_off:X}")
    sections = []
    for i in range(num_sec):
        s = sec_off + i * 40
        name = data[s : s + 8].rstrip(b"\x00").decode("ascii", "replace")
        vsize = struct.unpack_from("<I", data, s + 8)[0]
        vaddr = struct.unpack_from("<I", data, s + 12)[0]
        raw_siz = struct.unpack_from("<I", data, s + 16)[0]
        raw_off = struct.unpack_from("<I", data, s + 20)[0]
        sections.append((name, vaddr, vsize, raw_off, raw_siz))
        print(f"  {name:<10} VA=0x{vaddr:08X} Vsize=0x{vsize:06X} FileOff=0x{raw_off:08X} RawSz=0x{raw_siz:06X}")

    return image_base, sections, data


def read_va(va: int, n: int, image_base: int, sections: list[tuple[str, int, int, int, int]], data: bytes) -> tuple[str | None, int | None, bytes | None]:
    rva = va - image_base
    for name, vaddr, vsize, raw_off, raw_siz in sections:
        if vaddr <= rva < vaddr + vsize:
            off = raw_off + (rva - vaddr)
            bs = data[off : off + n]
            return name, off, bs
    return None, None, None


print("=== K1 swkotor.exe ===")
k1_ib, k1_sec, k1d = dump_pe("agentdecompile_projects/imports/swkotor.exe")

print("\n=== TSL swkotor2.exe ===")
tsl_ib, tsl_sec, tsld = dump_pe("agentdecompile_projects/imports/swkotor2.exe")

# Spot-check known addresses
checks = [
    ("CGameObject::~CGameObject", 0x004C3040, 0x0044F630),
    ("CSWCCreature::CSWCCreature", 0x0056CF00, 0x007B4AB0),
    ("CSWCObject::CSWCObject", 0x00572A30, 0x007FF960),
]
print("\n=== Byte reads at known addresses ===")
for name, k1_va, tsl_va in checks:
    sec, off, bs = read_va(k1_va, 16, k1_ib, k1_sec, k1d)
    print(f"{name}")
    print(f"  K1  0x{k1_va:08X} -> section={sec} fileoff=0x{(off or 0):08X}")
    print(f"       bytes: {' '.join(f'{b:02X}' for b in bs) if bs else 'NOT FOUND'}")
    sec, off, bs = read_va(tsl_va, 16, tsl_ib, tsl_sec, tsld)
    print(f"  TSL 0x{tsl_va:08X} -> section={sec} fileoff=0x{(off or 0):08X}")
    print(f"       bytes: {' '.join(f'{b:02X}' for b in bs) if bs else 'NOT FOUND'}")
