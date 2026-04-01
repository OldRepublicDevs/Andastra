"""Count all 55 8B EC hits in TSL .text and sample some to verify addresses."""

from __future__ import annotations

with open("agentdecompile_projects/imports/swkotor2.exe", "rb") as f:
    data = f.read()

# TSL .text: VA=0x1000 (VA start=0x401000), FileOff=0x400, RawSz=0x584800
text_file_start = 0x400
text_raw_size = 0x584800
text_vaddr_rva = 0x1000  # section RVA
image_base = 0x400000
text_va_start = image_base + text_vaddr_rva  # 0x401000

text = data[text_file_start : text_file_start + text_raw_size]

# Count all 55 8B EC occurrences
count = 0
pos = 0
sample_locs = []
while True:
    i = text.find(b"\x55\x8b\xec", pos)
    if i == -1:
        break
    count += 1
    if count <= 5 or (count % 1000 == 0):
        va = text_va_start + i
        sample_locs.append((i, va, data[text_file_start + i : text_file_start + i + 16]))
    pos = i + 1

print(f"Total 55 8B EC in TSL .text: {count}")
print("\nSamples:")
for fi, va, bs in sample_locs[:10]:
    print(f"  VA=0x{va:08X}  {' '.join(f'{b:02X}' for b in bs)}")

# Now check if known-function VAs are within range of 55 8B EC
known_vas = [
    ("CGameObject::~CGameObject", 0x0044F630),
    ("CSWCObject::CSWCObject", 0x007FF960),
    ("CSWCCreature::CSWCCreature", 0x007B4AB0),
]
print("\nSearching for known VAs within ±256 bytes of any 55 8B EC...")
# Collect all prologues
all_prologues = []
pos = 0
while True:
    i = text.find(b"\x55\x8b\xec", pos)
    if i == -1:
        break
    all_prologues.append(text_va_start + i)
    pos = i + 1

for fname, tgt in known_vas:
    nearest = min(all_prologues, key=lambda x: abs(x - tgt), default=None)
    dist = abs(nearest - tgt) if nearest else 9999
    print(f"  {fname}")
    print(f"    target=0x{tgt:08X}  nearest_prologue=0x{nearest:08X}  dist={dist}")
    if dist < 256 and nearest:
        off = nearest - text_va_start
        print(f"    bytes at nearest: {' '.join(f'{b:02X}' for b in text[off : off + 16])}")
