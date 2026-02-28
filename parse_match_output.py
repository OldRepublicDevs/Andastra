#!/usr/bin/env python3
"""Parse match-function output to extract K1->K2 address mappings.
Match output: sourceProgramPath is first target (K2_legacy). targetResults has
K1 and K2_Aspyr. For K1 result: sourceAddress=K2_legacy, targetAddress=K1.
So we invert: K1_addr -> K2_legacy_addr. For K2_Aspyr: source=K2_legacy, target=K2_aspyr."""
import json
import sys

K1_ADDRS = [
    "0x004b2e70", "0x0050ba00", "0x0050dd80", "0x004c5720", "0x0050e190",
    "0x004c8960", "0x004c3b10", "0x004ca680", "0x004c7050", "0x004c3960",
    "0x004c5bb0", "0x004c5980", "0x004c8b90", "0x004c7870", "0x00507680",
    "0x00507750", "0x00507810", "0x005078d0", "0x00507990", "0x00507a50",
    "0x00507b10", "0x00507bd0", "0x00507ca0", "0x00507d60", "0x00506090",
    "0x005061d0", "0x005062a0", "0x00500610", "0x0059adb0", "0x0059b250",
    "0x004cec50", "0x0055ccd0", "0x005b1b90", "0x004d3ec0", "0x004cca50",
    "0x00588ad0", "0x0058e660", "0x00591350", "0x005c8230", "0x00586a70",
    "0x005c6cd0", "0x00594d80", "0x004cc9d0", "0x004cc7e0", "0x004124e0",
    "0x004111c0", "0x00411940", "0x004d1cf0", "0x00504a70", "0x00504de0",
    "0x0050a0e0", "0x0050a350", "0x00505060", "0x00505360", "0x00505560",
    "0x0050a7b0", "0x005057a0", "0x00505af0", "0x00507490", "0x00505da0",
    "0x00505eb0", "0x00560970", "0x005649f0", "0x0058c5f0", "0x0058da80",
    "0x00595d20", "0x004d1be0", "0x004cecb0", "0x0058e0a0"
]

def main():
    if len(sys.argv) < 2:
        print("Usage: parse_match_output.py <output.txt>")
        sys.exit(1)
    
    with open(sys.argv[1]) as f:
        d = json.load(f)
    
    # Find targetResults for K1 and K2_Aspyr
    tr_k1 = tr_aspyr = None
    for tr in d.get("targetResults", []):
        p = tr.get("programPath", "")
        if "k1_win" in p and "k2" not in p:
            tr_k1 = tr
        elif "aspyr" in p:
            tr_aspyr = tr
    
    # K1 result: sourceAddress=K2_legacy, targetAddress=K1 -> invert for K1->K2_legacy
    k1_to_k2legacy = {}
    if tr_k1:
        for pd in tr_k1.get("propagatedDetails", []):
            k1_to_k2legacy[pd["targetAddress"]] = pd["sourceAddress"]
    
    # K2_Aspyr result: sourceAddress=K2_legacy, targetAddress=K2_aspyr
    k2legacy_to_k2aspyr = {}
    if tr_aspyr:
        for pd in tr_aspyr.get("propagatedDetails", []):
            k2legacy_to_k2aspyr[pd["sourceAddress"]] = pd["targetAddress"]
    
    # Build K1 -> K2 Legacy -> K2 Aspyr
    results = {}
    for addr in K1_ADDRS:
        k2l = k1_to_k2legacy.get(addr)
        k2a = k2legacy_to_k2aspyr.get(k2l) if k2l else None
        results[addr] = {"K2_Legacy": k2l or "-", "K2_Aspyr": k2a or "-"}
    
    print("| K1 Address | K2 Legacy | K2 Aspyr |")
    print("|------------|-----------|----------|")
    for addr in K1_ADDRS:
        r = results[addr]
        print(f"| {addr} | {r['K2_Legacy']} | {r['K2_Aspyr']} |")

if __name__ == "__main__":
    main()
