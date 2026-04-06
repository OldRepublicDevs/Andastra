# Ghidra TSL rename script  (4 entries)
from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0

try:
    fn = fm.getFunctionAt(toAddr(0x45CEA0))
    if fn:
        fn.setName('FUN_0061FBA0', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x45CEA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x502080))
    if fn:
        fn.setName('RemoveFOV', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x502080: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x501630))
    if fn:
        fn.setName('ApplyFOV', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x501630: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60BA20))
    if fn:
        fn.setName('_CExoFile', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60BA20: '+str(e)); errors += 1

print('renamed={} errors={}'.format(renamed, errors))