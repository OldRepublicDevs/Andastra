# Rename (1)
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
except Exception as e: print('ERR:'+str(e)); errors += 1

print('renamed:{} errors:{}'.format(renamed, errors))