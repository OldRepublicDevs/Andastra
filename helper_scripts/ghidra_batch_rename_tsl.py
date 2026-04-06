# Ghidra TSL rename script  (0 entries)
from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0

print('renamed={} errors={}'.format(renamed, errors))