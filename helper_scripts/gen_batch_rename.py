"""Regenerate ghidra_batch_rename_tsl.py from cascade + current CG."""
import json
import re

cas = json.load(open('docs/k1_tsl_cascade_matches.json'))
cg = json.load(open('docs/tsl_call_graph.json'))
tsl_name = {f['a']: f['n'] for f in cg['functions']}

pending = []
for m in cas['matches']:
    ta = int(m['tsl_addr'], 16)
    n = tsl_name.get(ta, '???')
    method = m.get('method', '')
    if n.startswith('FUN_') and not method.startswith('FUN_') and method:
        pending.append((ta, method, m.get('class', '')))

print('Useful pending renames:', len(pending))
for ta, meth, cls in pending[:5]:
    print(f'  0x{ta:08X} -> {cls}::{meth}')

lines = [
    f'# Ghidra TSL rename script  ({len(pending)} entries)',
    'from ghidra.program.model.symbol import SourceType',
    'fm = currentProgram.getFunctionManager()',
    'sm = currentProgram.getSymbolTable()',
    'renamed = 0; errors = 0',
    '',
]
for ta, method, ns in pending:
    clean = re.sub(r'[^A-Za-z0-9_]', '_', method)
    if ns and ns.strip():
        lines += [
            'try:',
            f'    fn = fm.getFunctionAt(toAddr(0x{ta:X}))',
            '    if fn:',
            f"        fn.setName('{clean}', SourceType.USER_DEFINED)",
            f"        nsobj = sm.getNamespace('{ns}', currentProgram.getGlobalNamespace())",
            '        if nsobj is None:',
            f"            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '{ns}', SourceType.USER_DEFINED)",
            '        fn.setParentNamespace(nsobj); renamed += 1',
            '    else: errors += 1',
            'except Exception as e:',
            f"    print('ERR 0x{ta:X}: '+str(e)); errors += 1",
            '',
        ]
    else:
        lines += [
            'try:',
            f'    fn = fm.getFunctionAt(toAddr(0x{ta:X}))',
            '    if fn:',
            f"        fn.setName('{clean}', SourceType.USER_DEFINED)",
            '        renamed += 1',
            '    else: errors += 1',
            'except Exception as e:',
            f"    print('ERR 0x{ta:X}: '+str(e)); errors += 1",
            '',
        ]
lines += ["print('renamed={} errors={}'.format(renamed, errors))"]
with open('helper_scripts/ghidra_batch_rename_tsl.py', 'w', encoding='utf-8') as fh:
    fh.write('\n'.join(lines))
print('Written:', len(pending), 'entries to ghidra_batch_rename_tsl.py')
