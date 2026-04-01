"""Check if swkotor.exe .text section contains any standard x86 function prologues."""
with open('agentdecompile_projects/imports/swkotor.exe','rb') as f:
    data = f.read()

# .text section: file offset 0x1000, size 0x33C000, VA base 0x401000
text = data[0x1000:0x1000+0x33C000]

hits55 = []
pos = 0
while len(hits55) < 20:
    i = text.find(b'\x55\x8B\xEC', pos)
    if i == -1:
        break
    hits55.append(i)
    pos = i + 1

print(f"'55 8B EC' (push ebp; mov ebp,esp) in .text: {len(hits55)} hits")
if hits55:
    for h in hits55[:5]:
        va = 0x400000 + 0x1000 + h
        ctx = ' '.join(f'{b:02X}' for b in text[h:h+16])
        print(f"  VA=0x{va:08X}  {ctx}")

# Check .bind section: file offset 0x3DB000, size 0x056000
bind = data[0x3DB000:0x3DB000+0x056000]
hits_bind = []
pos = 0
while len(hits_bind) < 20:
    i = bind.find(b'\x55\x8B\xEC', pos)
    if i == -1:
        break
    hits_bind.append(i)
    pos = i + 1
print(f"\n'55 8B EC' in .bind section: {len(hits_bind)} hits")
if hits_bind:
    for h in hits_bind[:5]:
        # .bind files at VA 0x46D000
        va = 0x400000 + 0x46D000 + h
        ctx = ' '.join(f'{b:02X}' for b in bind[h:h+16])
        print(f"  VA=0x{va:08X}  {ctx}")
