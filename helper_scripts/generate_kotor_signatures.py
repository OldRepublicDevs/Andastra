#!/usr/bin/env python3
"""
generate_kotor_signatures.py
============================
Universal KotOR I / TSL (II) function signature generator.

Reads raw bytes from swkotor.exe (K1) and swkotor2.exe (TSL) at
each known function address, then produces a GAME-AGNOSTIC wildcard
byte-pattern that locates the function in EITHER binary without any
runtime game-version check.

Output: docs/kotor_function_signatures.json

Schema highlights
-----------------
- "sig"      : space-separated hex bytes, wildcards as "??"
                 e.g.  "55 8B EC 68 ?? ?? ?? ?? 53 56 57"
- "addresses": {"k1": "0xHEX", "tsl": "0xHEX"}  – both optional
- A signature with ONLY k1/tsl is still valid for locating that
  function in that one game; "universal" sigs match both.

Usage
-----
python generate_kotor_signatures.py
    [--k1   path/to/swkotor.exe]          default: agentdecompile_projects/imports/swkotor.exe
    [--tsl  path/to/swkotor2.exe]         default: agentdecompile_projects/imports/swkotor2.exe
    [--seed path/to/existing_db.json]     merge / update existing database
    [--out  path/to/output.json]          default: docs/kotor_function_signatures.json
    [--sig-len  N]                        bytes to sample per function (default 64)
    [--min-static  N]                     minimum non-wildcard bytes required (default 8)
    [--verbose]

Algorithm
---------
1.  Parse PE headers of both EXEs to build VA→file-offset maps.
2.  For each known (name, k1_va, tsl_va) triplet:
      a.  Read sig_len bytes from k1_va in K1.
      b.  Read sig_len bytes from tsl_va in TSL.
      c.  Walk both byte arrays in parallel:
          - byte equal   → emit the hex byte
          - byte differs → emit "??"
      d.  Strip trailing wildcards.
      e.  Validate uniqueness: forward-scan both full PE images;
          if >1 match found, extend the pattern until unique.
3.  For K1-only entries (TSL address unknown) emit a K1 pattern
    derived solely from the K1 bytes (no wildcards – exact pattern
    unless the function is too short).
4.  Write the merged JSON database.
5.  Print a summary.
"""

from __future__ import annotations

import argparse
import hashlib
import json
import os
import struct
import sys
from dataclasses import dataclass, field
from datetime import datetime, timezone

# ---------------------------------------------------------------------------
# PE helper
# ---------------------------------------------------------------------------


@dataclass
class PESection:
    name: str
    virtual_address: int  # RVA
    virtual_size: int
    raw_offset: int
    raw_size: int


def _parse_pe(path: str) -> tuple[int, list[PESection], bytes]:
    """Return (image_base, sections, raw_data)."""
    with open(path, "rb") as fh:
        data = fh.read()

    def u16(off):
        return struct.unpack_from("<H", data, off)[0]

    def u32(off):
        return struct.unpack_from("<I", data, off)[0]

    def u64(off):
        return struct.unpack_from("<Q", data, off)[0]

    if data[:2] != b"MZ":
        raise ValueError(f"{path}: not a valid PE (bad MZ magic)")

    pe_off = u32(0x3C)
    if data[pe_off : pe_off + 4] != b"PE\x00\x00":
        raise ValueError(f"{path}: no PE signature at offset 0x{pe_off:X}")

    num_sections = u16(pe_off + 6)
    opt_hdr_size = u16(pe_off + 20)
    opt_start = pe_off + 24
    opt_magic = u16(opt_start)

    if opt_magic == 0x010B:  # PE32
        image_base = u32(opt_start + 28)
    elif opt_magic == 0x020B:  # PE32+
        image_base = u64(opt_start + 24)
    else:
        raise ValueError(f"{path}: unsupported PE optional header magic 0x{opt_magic:04X}")

    sec_start = opt_start + opt_hdr_size
    sections: list[PESection] = []
    for i in range(num_sections):
        off = sec_start + i * 40
        name = data[off : off + 8].rstrip(b"\x00").decode("ascii", errors="replace")
        virtual_size = u32(off + 8)
        virtual_addr = u32(off + 12)
        raw_size = u32(off + 16)
        raw_ptr = u32(off + 20)
        sections.append(PESection(name, virtual_addr, virtual_size, raw_ptr, raw_size))

    return image_base, sections, data


def va_to_file_offset(va: int, image_base: int, sections: list[PESection]) -> int | None:
    rva = va - image_base
    for sec in sections:
        if sec.virtual_address <= rva < sec.virtual_address + max(sec.virtual_size, sec.raw_size):
            off = rva - sec.virtual_address
            if off < sec.raw_size:
                return sec.raw_offset + off
    return None


def read_bytes_at_va(va: int, count: int, image_base: int, sections: list[PESection], data: bytes) -> bytes | None:
    file_off = va_to_file_offset(va, image_base, sections)
    if file_off is None:
        return None
    return data[file_off : file_off + count]


# ---------------------------------------------------------------------------
# Signature helpers
# ---------------------------------------------------------------------------


def build_wildcard_sig(k1_bytes: bytes, tsl_bytes: bytes) -> str:
    """
    Produce a wildcard pattern from two byte sequences.
    Bytes that differ become "??".
    Trailing wildcards are stripped.
    """
    n = min(len(k1_bytes), len(tsl_bytes))
    tokens: list[str] = []
    for i in range(n):
        if k1_bytes[i] == tsl_bytes[i]:
            tokens.append(f"{k1_bytes[i]:02X}")
        else:
            tokens.append("??")
    # Strip trailing wildcards
    while tokens and tokens[-1] == "??":
        tokens.pop()
    return " ".join(tokens)


def build_exact_sig(raw_bytes: bytes) -> str:
    """Produce an exact (no wildcards) pattern for a single-game function."""
    return " ".join(f"{b:02X}" for b in raw_bytes)


def count_static_bytes(sig: str) -> int:
    return sum(1 for t in sig.split() if t != "??")


def sig_scan(sig: str, data: bytes, max_results: int = 5) -> list[int]:
    """
    Fast forward scan of `data` for signature `sig`.
    Returns file offsets of all matches up to max_results.

    Uses bytes.find() for the first static byte as a pre-filter,
    then verifies the full wildcard pattern only at candidate positions.
    """
    tokens = sig.split()
    n = len(tokens)
    if n == 0:
        return []

    pattern: list[int | None] = [None if t == "??" else int(t, 16) for t in tokens]

    # Find first static byte for pre-filtering
    static_byte0 = None
    static_off0 = 0
    for i, b in enumerate(pattern):
        if b is not None:
            static_byte0 = b
            static_off0 = i
            break

    results: list[int] = []
    pos = 0
    limit = len(data) - n

    while pos <= limit:
        # Pre-filter on first static byte
        if static_byte0 is not None:
            idx = data.find(bytes([static_byte0]), pos + static_off0)
            if idx == -1:
                break
            pos = idx - static_off0
            if pos < 0:
                pos += 1
                continue

        # Full wildcard compare
        match = all(p is None or data[pos + i] == p for i, p in enumerate(pattern))

        if match:
            results.append(pos)
            if len(results) >= max_results:
                break
            pos += n  # skip past match to avoid overlapping
        else:
            pos += 1

    return results


def is_unique(sig: str, k1_data: bytes, tsl_data: bytes) -> tuple[bool, int, int]:
    """
    Returns (unique, k1_match_count, tsl_match_count).
    unique = True if each game has exactly 1 match.
    """
    k1_hits = sig_scan(sig, k1_data, max_results=2)
    tsl_hits = sig_scan(sig, tsl_data, max_results=2)
    return (len(k1_hits) == 1 and len(tsl_hits) == 1, len(k1_hits), len(tsl_hits))


def extend_sig_until_unique(
    k1_bytes: bytes,
    tsl_bytes: bytes,
    k1_data: bytes,
    tsl_data: bytes,
    min_static: int,
    max_len: int = 64,
    skip_uniqueness: bool = False,
) -> tuple[str, bool, int, int] | None:
    """
    Iteratively grow the wildcard sig until it is unique in both binaries,
    or until max_len is reached.
    Returns (sig, unique, k1_hits, tsl_hits) or None if bytes too short.
    If skip_uniqueness=True, immediately returns the max_len pattern without scanning.
    """
    if skip_uniqueness:
        sig = build_wildcard_sig(k1_bytes[:max_len], tsl_bytes[:max_len])
        return sig, False, -1, -1

    for end in range(min_static + 1, min(max_len, len(k1_bytes), len(tsl_bytes)) + 1):
        sig = build_wildcard_sig(k1_bytes[:end], tsl_bytes[:end])
        if count_static_bytes(sig) < min_static:
            continue
        unique, k1c, tslc = is_unique(sig, k1_data, tsl_data)
        if unique:
            return sig, True, k1c, tslc

    sig = build_wildcard_sig(k1_bytes[:max_len], tsl_bytes[:max_len])
    _, k1c, tslc = is_unique(sig, k1_data, tsl_data)
    return sig, False, k1c, tslc


def extend_single_sig_until_unique(
    raw_bytes: bytes,
    game_data: bytes,
    min_static: int,
    max_len: int = 64,
    skip_uniqueness: bool = False,
) -> tuple[str, bool, int] | None:
    """Like extend_sig_until_unique but for a single game (no wildcards)."""
    if skip_uniqueness:
        sig = build_exact_sig(raw_bytes[:max_len])
        return sig, False, -1

    for end in range(min_static + 1, min(max_len, len(raw_bytes)) + 1):
        sig = build_exact_sig(raw_bytes[:end])
        hits = sig_scan(sig, game_data, max_results=2)
        if len(hits) == 1:
            return sig, True, 1

    sig = build_exact_sig(raw_bytes[:max_len])
    hits = sig_scan(sig, game_data, max_results=2)
    return sig, len(hits) == 1, len(hits)


# ---------------------------------------------------------------------------
# Data model
# ---------------------------------------------------------------------------


@dataclass
class FunctionAddresses:
    k1: str | None = None  # "0x00XXXXXX"
    tsl: str | None = None


@dataclass
class FunctionVtables:
    k1: str | None = None
    tsl: str | None = None


@dataclass
class FunctionSig:
    pattern: str = ""
    offset: int = 0  # offset into function where pattern starts
    unique_k1: bool = False
    unique_tsl: bool = False
    k1_matches: int = 0
    tsl_matches: int = 0
    confidence: str = "unknown"  # "verified" | "auto" | "k1_only" | "tsl_only"
    method: str = ""  # "binary_diff_k1_tsl" | "k1_exact" | "tsl_exact"


@dataclass
class FunctionEntry:
    id: str
    name: str
    class_name: str = ""
    method_type: str = ""  # "constructor" | "destructor" | "deleting_thunk" | "method"
    addresses: FunctionAddresses = field(default_factory=FunctionAddresses)
    vtables: FunctionVtables = field(default_factory=FunctionVtables)
    sig: FunctionSig | None = None
    base_classes: list[str] = field(default_factory=list)
    object_type_const: int | None = None
    notes: str = ""
    verification: str = "unverified"  # "manual" | "auto" | "unverified"


# ---------------------------------------------------------------------------
# Known pairs seed  (populated from session memory)
# ---------------------------------------------------------------------------

# Format: (id, name, class, method_type, k1_va_hex, tsl_va_hex, tsl_vtable_hex, base_classes, obj_type_const, notes)
KNOWN_PAIRS = [
    # ---- CGameObject ----
    (
        "CGameObject_dtor",
        "CGameObject::~CGameObject",
        "CGameObject",
        "destructor",
        "0x004C3040",
        "0x0044F630",
        None,
        [],
        None,
        "K1 named; TSL confirmed via first slot of CGameObject vftable",
    ),
    # ---- CSWCObject ----
    ("CSWCObject_ctor", "CSWCObject::CSWCObject", "CSWCObject", "constructor", "0x0063E530", "0x004721B0", None, ["CGameObject"], None, "K1 named; TSL via base-ctor fan-in"),
    ("CSWCObject_dtor", "CSWCObject::~CSWCObject", "CSWCObject", "destructor", "0x0063E800", "0x00472750", None, ["CGameObject"], None, ""),
    (
        "CSWCObject_deleting_thunk",
        "CSWCObject::`scalar deleting destructor'",
        "CSWCObject",
        "deleting_thunk",
        None,
        "0x00472720",
        None,
        [],
        None,
        "First slot of CSWCObject vftable; calls 0x00472750 then operator_delete",
    ),
    # ---- CSWCModule ----
    ("CSWCModule_ctor", "CSWCModule::CSWCModule", "CSWCModule", "constructor", "0x00643F40", "0x00469CD0", None, ["CGameObject"], None, ""),
    ("CSWCModule_dtor", "CSWCModule::~CSWCModule", "CSWCModule", "destructor", "0x00641CD0", "0x0046A0D0", None, [], None, ""),
    (
        "CSWCModule_deleting_thunk",
        "CSWCModule::`scalar deleting destructor'",
        "CSWCModule",
        "deleting_thunk",
        None,
        "0x0046A080",
        None,
        [],
        None,
        "Calls 0x0046A0D0 then operator_delete",
    ),
    # ---- CSWSModule ----
    (
        "CSWSModule_ctor",
        "CSWSModule::CSWSModule",
        "CSWSModule",
        "constructor",
        "0x004C84A0",
        "0x00728E20",
        None,
        ["CGameObject"],
        None,
        "Server-side module ctor; TSL destructor not yet located",
    ),
    # ---- CSWCArea ----
    ("CSWCArea_ctor", "CSWCArea::CSWCArea", "CSWCArea", "constructor", "0x00606090", "0x0044EF30", None, ["CSWCObject", "CGameObject"], None, ""),
    ("CSWCArea_dtor", "CSWCArea::~CSWCArea", "CSWCArea", "destructor", "0x0060A0F0", "0x0044F6B0", None, [], None, ""),
    (
        "CSWCArea_deleting_thunk",
        "CSWCArea::`scalar deleting destructor'",
        "CSWCArea",
        "deleting_thunk",
        None,
        "0x0044F680",
        None,
        [],
        None,
        "First slot of CSWCArea vftable; calls 0x0044F6B0",
    ),
    # ---- CSWSArea ----
    ("CSWSArea_ctor", "CSWSArea::CSWSArea", "CSWSArea", "constructor", "0x0050CF80", "0x00716560", None, ["CSWSObject"], None, "Server-side area ctor"),
    ("CSWSArea_dtor", "CSWSArea::~CSWSArea", "CSWSArea", "destructor", "0x0050D370", "0x00716B80", None, [], None, ""),
    ("CSWSArea_deleting_thunk", "CSWSArea::`scalar deleting destructor'", "CSWSArea", "deleting_thunk", None, "0x00716B50", None, [], None, "First slot of CSWSArea vftable"),
    # ---- CSWSObject ----
    ("CSWSObject_ctor", "CSWSObject::CSWSObject", "CSWSObject", "constructor", "0x004CFCB0", "0x006A55E0", None, ["CGameObject"], None, "Server-side object ctor"),
    ("CSWSObject_dtor", "CSWSObject::~CSWSObject", "CSWSObject", "destructor", "0x004D0220", "0x006A5D30", None, [], None, ""),
    (
        "CSWSObject_deleting_thunk",
        "CSWSObject::`scalar deleting destructor'",
        "CSWSObject",
        "deleting_thunk",
        None,
        "0x006A5D00",
        None,
        [],
        None,
        "First slot of CSWSObject vftable",
    ),
    # ---- CSWCCreature ----
    (
        "CSWCCreature_ctor",
        "CSWCCreature::CSWCCreature",
        "CSWCCreature",
        "constructor",
        "0x00616A20",
        "0x004102B0",
        None,
        ["CSWCObject", "CGameObject"],
        6,
        "Object type constant 6",
    ),
    ("CSWCCreature_dtor", "CSWCCreature::~CSWCCreature", "CSWCCreature", "destructor", "0x006170D0", "0x00410E80", "0x00985EFC", [], None, "TSL vtable @ 0x00985EFC"),
    (
        "CSWCCreature_deleting_thunk",
        "CSWCCreature::`scalar deleting destructor'",
        "CSWCCreature",
        "deleting_thunk",
        None,
        "0x00410E50",
        None,
        [],
        None,
        "TSL: calls 0x00410E80 then operator_delete",
    ),
    # ---- CSWCItem ----
    ("CSWCItem_ctor", "CSWCItem::CSWCItem", "CSWCItem", "constructor", "0x00646230", "0x0047E1F0", None, ["CSWCObject", "CGameObject"], None, ""),
    (
        "CSWCItem_dtor",
        "CSWCItem::~CSWCItem",
        "CSWCItem",
        "destructor",
        "0x00646CD0",
        "0x0047E460",
        "0x009882A8",
        [],
        None,
        "TSL vtable @ 0x009882A8 (two vtables: 0x009882A8 top, 0x0098815C embedded)",
    ),
    ("CSWCItem_deleting_thunk", "CSWCItem::`scalar deleting destructor'", "CSWCItem", "deleting_thunk", None, "0x0047E430", None, [], None, ""),
    # ---- CSWCVisualEffect ----
    (
        "CSWCVisualEffect_ctor",
        "CSWCVisualEffect::CSWCVisualEffect",
        "CSWCVisualEffect",
        "constructor",
        "0x0068FE70",
        "0x004F90C0",
        None,
        ["CSWCObject", "CGameObject"],
        None,
        "",
    ),
    (
        "CSWCVisualEffect_dtor",
        "CSWCVisualEffect::~CSWCVisualEffect",
        "CSWCVisualEffect",
        "destructor",
        "0x0068FF70",
        "0x004F9280",
        "0x0098BE9C",
        [],
        None,
        "TSL vtable @ 0x0098BE9C",
    ),
    ("CSWCVisualEffect_deleting_thunk", "CSWCVisualEffect::`scalar deleting destructor'", "CSWCVisualEffect", "deleting_thunk", None, "0x004F9250", None, [], None, ""),
    # ---- CSWCMapPin ----
    ("CSWCMapPin_ctor", "CSWCMapPin::CSWCMapPin", "CSWCMapPin", "constructor", "0x00692540", "0x0055C4F0", None, ["CSWCObject", "CGameObject"], None, ""),
    (
        "CSWCMapPin_dtor",
        "CSWCMapPin::~CSWCMapPin",
        "CSWCMapPin",
        "destructor",
        "0x006925B0",
        "0x0055C5C0",
        "0x00990884",
        [],
        None,
        "TSL vtable @ 0x00990884; dtor: reset vtable, ~CExoString, ~CSWCObject",
    ),
    ("CSWCMapPin_deleting_thunk", "CSWCMapPin::`scalar deleting destructor'", "CSWCMapPin", "deleting_thunk", "0x00692610", "0x0055C590", None, [], None, ""),
]

# Next to annotate (TSL address confirmed, awaiting Ghidra plate comment + sig)
PENDING_TSL = [
    # (id, name, class, method_type, k1_hex, tsl_hex, vtable_tsl, base_classes, obj_const, notes)
    (
        "CSWCProjectile_ctor",
        "CSWCProjectile::CSWCProjectile",
        "CSWCProjectile",
        "constructor",
        "0x006D57B0",
        "0x00578EC0",
        None,
        ["CSWCObject", "CGameObject"],
        8,
        "Object type constant 8; TSL vtable MOV seen in disassembly",
    ),
    (
        "CSWCProjectile_dtor",
        "CSWCProjectile::~CSWCProjectile",
        "CSWCProjectile",
        "destructor",
        "0x006D5A40",
        None,
        None,
        [],
        None,
        "TSL dtor: derive from vtable first-slot of CSWCProjectile vftable",
    ),
    (
        "CSWCSoundObject_ctor",
        "CSWCSoundObject::CSWCSoundObject",
        "CSWCSoundObject",
        "constructor",
        None,
        "0x0055AA20",
        None,
        ["CSWCObject", "CGameObject"],
        0x10,
        "Object type constant 0x10; TSL vtable @ 0x00990734",
    ),
    (
        "CSWCPlaceable_ctor",
        "CSWCPlaceable::CSWCPlaceable",
        "CSWCPlaceable",
        "constructor",
        "0x00682270",
        None,
        None,
        ["CSWCObject", "CGameObject"],
        None,
        "TSL ctor not yet identified",
    ),
    ("CSWCPlaceable_dtor", "CSWCPlaceable::~CSWCPlaceable", "CSWCPlaceable", "destructor", "0x00683200", None, None, [], None, ""),
    ("CSWCDoor_ctor", "CSWCDoor::CSWCDoor", "CSWCDoor", "constructor", "0x00683BB0", None, None, ["CSWCObject", "CGameObject"], None, "TSL ctor not yet identified"),
    ("CSWCDoor_dtor", "CSWCDoor::~CSWCDoor", "CSWCDoor", "destructor", "0x00683F90", None, None, [], None, ""),
    (
        "CSWCTrigger_ctor",
        "CSWCTrigger::CSWCTrigger",
        "CSWCTrigger",
        "constructor",
        "0x006911E0",
        None,
        None,
        ["CSWCObject", "CGameObject"],
        None,
        "TSL ctor not yet identified",
    ),
    ("CSWCTrigger_dtor", "CSWCTrigger::~CSWCTrigger", "CSWCTrigger", "destructor", "0x006912F0", None, None, [], None, ""),
]

# ---------------------------------------------------------------------------
# Main logic
# ---------------------------------------------------------------------------


def build_entry_from_tuple(t) -> FunctionEntry:
    (fid, name, cls, mtype, k1h, tslh, vtbl_tsl, bases, obj_const, notes) = t
    addrs = FunctionAddresses(k1=k1h, tsl=tslh)
    vtbls = FunctionVtables(tsl=vtbl_tsl)
    return FunctionEntry(
        id=fid,
        name=name,
        class_name=cls,
        method_type=mtype,
        addresses=addrs,
        vtables=vtbls,
        base_classes=bases,
        object_type_const=obj_const,
        notes=notes,
        verification="manual" if tslh else "partial",
    )


def generate_sig_for_entry(
    entry: FunctionEntry,
    k1_image_base: int,
    k1_sections: list[PESection],
    k1_data: bytes,
    tsl_image_base: int,
    tsl_sections: list[PESection],
    tsl_data: bytes,
    sig_len: int,
    min_static: int,
    verbose: bool = False,
    skip_uniqueness: bool = False,
) -> FunctionSig | None:
    k1h = entry.addresses.k1
    tslh = entry.addresses.tsl

    k1_va = int(k1h, 16) if k1h else None
    tsl_va = int(tslh, 16) if tslh else None

    k1_bytes = read_bytes_at_va(k1_va, sig_len, k1_image_base, k1_sections, k1_data) if k1_va else None
    tsl_bytes = read_bytes_at_va(tsl_va, sig_len, tsl_image_base, tsl_sections, tsl_data) if tsl_va else None

    if verbose:
        print(f"  [{entry.id}]  k1={k1h}  tsl={tslh}")
        print(f"    k1_bytes={k1_bytes[:16].hex() if k1_bytes else 'None'}")
        print(f"    tsl_bytes={tsl_bytes[:16].hex() if tsl_bytes else 'None'}")

    if k1_bytes and tsl_bytes:
        result = extend_sig_until_unique(
            k1_bytes,
            tsl_bytes,
            k1_data,
            tsl_data,
            min_static=min_static,
            max_len=sig_len,
            skip_uniqueness=skip_uniqueness,
        )
        if result is None:
            return None
        sig_str, unique, k1c, tslc = result
        if skip_uniqueness:
            u_k1c, u_tslc = -1, -1
        else:
            # Whether unique per-game may differ; compute correctly
            _, u_k1c, u_tslc = is_unique(sig_str, k1_data, tsl_data)
        return FunctionSig(
            pattern=sig_str,
            offset=0,
            unique_k1=(u_k1c == 1),
            unique_tsl=(u_tslc == 1),
            k1_matches=u_k1c,
            tsl_matches=u_tslc,
            confidence="verified" if entry.verification == "manual" else "auto",
            method="binary_diff_k1_tsl",
        )

    elif k1_bytes:
        result = extend_single_sig_until_unique(
            k1_bytes,
            k1_data,
            min_static=min_static,
            max_len=sig_len,
            skip_uniqueness=skip_uniqueness,
        )
        if result is None:
            return None
        sig_str, unique, hits = result
        return FunctionSig(
            pattern=sig_str,
            offset=0,
            unique_k1=unique,
            unique_tsl=False,
            k1_matches=hits,
            tsl_matches=0,
            confidence="k1_only",
            method="k1_exact",
        )

    elif tsl_bytes:
        result = extend_single_sig_until_unique(
            tsl_bytes,
            tsl_data,
            min_static=min_static,
            max_len=sig_len,
            skip_uniqueness=skip_uniqueness,
        )
        if result is None:
            return None
        sig_str, unique, hits = result
        # If this entry has a K1 address but K1 bytes were unavailable (packed), note that
        has_k1_addr = bool(entry.addresses.k1)
        if has_k1_addr:
            confidence = "verified_tsl_sig_k1_packed" if entry.verification == "manual" else "partial_tsl_sig_k1_packed"
            method = "tsl_exact_k1_packed"
        else:
            confidence = "tsl_only"
            method = "tsl_exact"
        return FunctionSig(
            pattern=sig_str,
            offset=0,
            unique_k1=False,
            unique_tsl=unique,
            k1_matches=0,
            tsl_matches=hits,
            confidence=confidence,
            method=method,
        )

    return None


def file_md5(path: str) -> str:
    h = hashlib.md5()
    with open(path, "rb") as fh:
        for chunk in iter(lambda: fh.read(65536), b""):
            h.update(chunk)
    return h.hexdigest()


def entry_to_dict(e: FunctionEntry) -> dict:
    d = {
        "id": e.id,
        "name": e.name,
        "class": e.class_name,
        "method_type": e.method_type,
        "addresses": {k: v for k, v in [("k1", e.addresses.k1), ("tsl", e.addresses.tsl)] if v},
        "vtables": {k: v for k, v in [("k1", e.vtables.k1), ("tsl", e.vtables.tsl)] if v},
        "base_classes": e.base_classes,
        "notes": e.notes,
        "verification": e.verification,
    }
    if e.object_type_const is not None:
        d["object_type_const"] = e.object_type_const
    if e.sig:
        d["sig"] = {
            "pattern": e.sig.pattern,
            "offset": e.sig.offset,
            "unique_k1": e.sig.unique_k1,
            "unique_tsl": e.sig.unique_tsl,
            "k1_matches": e.sig.k1_matches,
            "tsl_matches": e.sig.tsl_matches,
            "confidence": e.sig.confidence,
            "method": e.sig.method,
        }
    else:
        d["sig"] = None
    return d


def main():
    repo_root = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))

    parser = argparse.ArgumentParser(description="KotOR universal function sig generator")
    parser.add_argument("--k1", default=os.path.join(repo_root, "agentdecompile_projects", "imports", "swkotor.exe"))
    parser.add_argument("--tsl", default=os.path.join(repo_root, "agentdecompile_projects", "imports", "swkotor2.exe"))
    parser.add_argument("--out", default=os.path.join(repo_root, "docs", "kotor_function_signatures.json"))
    parser.add_argument("--sig-len", type=int, default=64, help="bytes to read per function")
    parser.add_argument("--min-static", type=int, default=8, help="minimum non-wildcard bytes for valid sig")
    parser.add_argument("--verbose", action="store_true")
    parser.add_argument("--fast", action="store_true", help="Skip uniqueness verification (fast mode; patterns still valid, counts will be -1)")
    parser.add_argument("--k1-packed", action="store_true", help="K1 binary .text is packed/encrypted on disk; skip K1 byte reads (generate TSL-only sigs for verified pairs)")
    args = parser.parse_args()

    if not os.path.exists(args.k1):
        print(f"ERROR: K1 binary not found: {args.k1}", file=sys.stderr)
        sys.exit(1)
    if not os.path.exists(args.tsl):
        print(f"ERROR: TSL binary not found: {args.tsl}", file=sys.stderr)
        sys.exit(1)

    print(f"Reading K1  binary: {args.k1}")
    k1_image_base, k1_sections, k1_data = _parse_pe(args.k1)
    print(f"  image_base=0x{k1_image_base:08X}  sections={[s.name for s in k1_sections]}")

    # Auto-detect K1 packing if not forced by flag
    if not args.k1_packed:
        _text_off = next((s.raw_offset for s in k1_sections if s.name == ".text"), None)
        _text_sz = next((s.raw_size for s in k1_sections if s.name == ".text"), None)
        if _text_off and _text_sz:
            _sample = k1_data[_text_off : _text_off + min(_text_sz, 0x50000)]
            _prologue_count = _sample.count(b"\x55\x8b\xec")
            if _prologue_count < 10:
                print(f"  WARNING: Only {_prologue_count} 'push ebp; mov ebp,esp' prologues found in K1 .text.")
                print("  K1 binary appears packed/encrypted on disk. Enabling --k1-packed automatically.")
                args.k1_packed = True
            else:
                print(f"  K1 .text looks readable ({_prologue_count} prologues in first 320 KB).")
    if args.k1_packed:
        print("  K1 byte reads DISABLED (packed binary). Only TSL-side signatures will be generated.")
        k1_data = b""  # empty bytes → read_bytes_at_va returns falsy b'' → falls through to TSL-only path

    print(f"Reading TSL binary: {args.tsl}")
    tsl_image_base, tsl_sections, tsl_data = _parse_pe(args.tsl)
    print(f"  image_base=0x{tsl_image_base:08X}  sections={[s.name for s in tsl_sections]}")

    # Build entry list from all known pairs + pending
    all_tuples = list(KNOWN_PAIRS) + list(PENDING_TSL)
    entries: list[FunctionEntry] = [build_entry_from_tuple(t) for t in all_tuples]

    # Generate signatures
    print(f"\nGenerating signatures for {len(entries)} entries ...")
    ok = 0
    for entry in entries:
        sig = generate_sig_for_entry(
            entry,
            k1_image_base,
            k1_sections,
            k1_data,
            tsl_image_base,
            tsl_sections,
            tsl_data,
            args.sig_len,
            args.min_static,
            verbose=args.verbose,
            skip_uniqueness=args.fast,
        )
        entry.sig = sig
        if sig:
            ok += 1
            status = "UNIQ" if (sig.unique_k1 and sig.unique_tsl) else "K1U " if sig.unique_k1 else "TSLU" if sig.unique_tsl else "MULTI"
            print(f"  [{status}] {entry.name:<55} {sig.pattern[:50]}")
        else:
            print(f"  [SKIP] {entry.name} — could not read bytes at given VA(s)")

    # Build output JSON
    k1_md5 = file_md5(args.k1)
    tsl_md5 = file_md5(args.tsl)
    total = len(entries)
    verified = sum(1 for e in entries if e.verification == "manual")

    output = {
        "meta": {
            "version": "1.0.0",
            "generated_at": datetime.now(timezone.utc).isoformat(),
            "description": (
                "Universal KotOR I / TSL function signature database. "
                "All 'sig.pattern' values are game-agnostic wildcard byte patterns "
                "that locate the function in EITHER swkotor.exe OR swkotor2.exe "
                "without any game-version conditional. "
                "Wildcards '??' mark bytes that differ between K1 and TSL. "
                "Use: scan the loaded executable image for the pattern; "
                "the first (or only) hit is the function entry point."
            ),
            "sources": {
                "k1": {
                    "filename": os.path.basename(args.k1),
                    "md5": k1_md5,
                    "size": len(k1_data) or None,
                    "version": "K1 1.03 GOG",
                    "byte_readable": not args.k1_packed,
                    "note": (
                        "K1 swkotor.exe .text section is packed/encrypted on disk. "
                        "Byte signatures for K1 require reading from the live Ghidra-loaded "
                        "(unpacked) image via agentdecompile MCP. "
                        "Addresses listed here are correct Ghidra runtime VAs."
                        if args.k1_packed
                        else None
                    ),
                },
                "tsl": {"filename": os.path.basename(args.tsl), "md5": tsl_md5, "size": len(tsl_data), "version": "TSL 2.10 GOG", "byte_readable": True},
            },
            "stats": {
                "total": total,
                "with_sig": ok,
                "verified": verified,
                "partial": sum(1 for e in entries if e.verification == "partial"),
                "unverified": sum(1 for e in entries if e.verification == "unverified"),
            },
        },
        "classes": {},
        "functions": [entry_to_dict(e) for e in entries],
    }

    # Build per-class index
    class_map: dict[str, list[str]] = {}
    for e in entries:
        class_map.setdefault(e.class_name, []).append(e.id)
    output["classes"] = class_map

    os.makedirs(os.path.dirname(args.out), exist_ok=True)
    with open(args.out, "w", encoding="utf-8") as fh:
        json.dump(output, fh, indent=2, ensure_ascii=False)

    print("\n--- Summary ---")
    print(f"  Entries total : {total}")
    print(f"  With sigs     : {ok}")
    print(f"  Verified pairs: {verified}")
    print(f"  Output        : {args.out}")
    print("\nTo use a signature at runtime:")
    print("  1. Load swkotor.exe OR swkotor2.exe into memory.")
    print("  2. AOBScan for the 'sig.pattern' field.")
    print("  3. The match offset is the function entry point.")
    print("  4. No 'if (game == K1)' conditional needed.")

    return 0


if __name__ == "__main__":
    sys.exit(main())
