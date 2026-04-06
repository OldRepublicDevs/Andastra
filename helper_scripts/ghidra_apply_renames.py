import json
from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renames = json.load(open('C:/GitHub/Andastra/docs/tsl_pending_renames.json'))
chunk_start = int(getScriptArgs()[0]) if getScriptArgs() else 0
chunk_size  = int(getScriptArgs()[1]) if len(getScriptArgs()) > 1 else len(renames)
chunk = renames[chunk_start:chunk_start+chunk_size]
renamed = 0; errors = 0
for r in chunk:
    try:
        fn = fm.getFunctionAt(toAddr(r['addr']))
        if fn:
            fn.setName(r['name'], SourceType.USER_DEFINED)
            ns = r.get('ns','')
            if ns:
                nsobj = sm.getNamespace(ns, currentProgram.getGlobalNamespace())
                if nsobj is None:
                    nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), ns, SourceType.USER_DEFINED)
                fn.setParentNamespace(nsobj)
            renamed += 1
        else:
            errors += 1
    except Exception as e:
        errors += 1
print('renamed={} errors={} chunk={}/{}'.format(renamed, errors, chunk_start, len(renames)))