"""Check TSL binary for readable code."""
with open('agentdecompile_projects/imports/swkotor2.exe','rb') as f:
    data = f.read()

# TSL .text: raw file offset=0x400, size=0x584800, VA range 0x401000..
text_off = 0x400
text_sz  = 0x584800
text = data[text_off:text_off+text_sz]

hits: list[int] = []
pos = 0
while len(hits) < 10:
    i = text.find(b'\x55\x8B\xEC', pos)
    if i == -1:
        break
    hits.append(i)
    pos = i + 1

print(f"TSL '55 8B EC' in .text: {len(hits)} hits")
for h in hits[:5]:
    va = 0x400000 + 0x1000 + h
    ctx = ' '.join(f'{b:02X}' for b in text[h:h+16])
    print(f"  VA=0x{va:08X}  {ctx}")

# Also check the very first 32 bytes of TSL .text:
print()
print("TSL .text first 32 bytes (file off 0x400):")
print(' '.join(f'{b:02X}' for b in data[0x400:0x420]))
