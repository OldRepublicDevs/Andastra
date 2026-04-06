from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0
tid = currentProgram.startTransaction('batch_rename_1')
try:
  fn = fm.getFunctionAt(toAddr(0x608260))
  if fn:
    fn.setName('CExoTimers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoTimers', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoTimers', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x608260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40C0E0))
  if fn:
    fn.setName('RemovePlayer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CObjectTableManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CObjectTableManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C0E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x639CC0))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639CC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63F2F0))
  if fn:
    fn.setName('SetOneShotDelay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63F2F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x401B20))
  if fn:
    fn.setName('GetObjectTableManager', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x401B20: '+str(e)); errors += 1
currentProgram.endTransaction(tid, True)
print('Batch 1 done: renamed='+str(renamed)+' errors='+str(errors))