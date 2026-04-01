"""Find known class functions in extracted K1/TSL byte dumps."""
import json

k1 = json.load(open('docs/k1_function_bytes.json'))
tsl = json.load(open('docs/tsl_function_bytes.json'))

# Build lookup by namespace::name
k1_by_class = {}
for f in k1['functions']:
    if f['ns']:
        key = f['ns'] + '::' + f['n']
        k1_by_class[key] = f

tsl_by_class = {}
for f in tsl['functions']:
    if f['ns']:
        key = f['ns'] + '::' + f['n']
        tsl_by_class[key] = f

# Check known classes
classes = [
    'CGameObject', 'CSWCObject', 'CSWCModule', 'CSWSModule',
    'CSWCArea', 'CSWSArea', 'CSWSObject', 'CSWCCreature',
    'CSWCItem', 'CSWCVisualEffect', 'CSWCMapPin',
    'CSWCProjectile', 'CSWCSoundObject', 'CSWCPlaceable',
    'CSWCDoor', 'CSWCTrigger', 'CAppManager',
]

print("=== K1 FUNCTIONS BY CLASS ===")
for cls in classes:
    funcs = [(k, v) for k, v in k1_by_class.items() if k.startswith(cls + '::')]
    if funcs:
        print(f"\n{cls} ({len(funcs)} funcs):")
        for k, v in sorted(funcs, key=lambda x: x[1]['a']):
            print(f"  {v['a']}  {v['n']:<45} [{v['b'][:50]}...]")

print("\n\n=== TSL FUNCTIONS BY CLASS ===")
for cls in classes:
    funcs = [(k, v) for k, v in tsl_by_class.items() if k.startswith(cls + '::')]
    if funcs:
        print(f"\n{cls} ({len(funcs)} funcs):")
        for k, v in sorted(funcs, key=lambda x: x[1]['a']):
            print(f"  {v['a']}  {v['n']:<45} [{v['b'][:50]}...]")
