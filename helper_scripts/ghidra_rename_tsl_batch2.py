from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0
try:
  fn = fm.getFunctionAt(toAddr(0x6F9280))
  if fn:
    fn.setName('ComputeNumberKnownSpellsLeft', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F9280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FA2B0))
  if fn:
    fn.setName('CheckInclusionMatch', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FA2B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7747C0))
  if fn:
    fn.setName('GetCurrentlyFollowingLeader', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7747C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x912DC0))
  if fn:
    fn.setName('Show', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterfaceChar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterfaceChar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x912DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60BD10))
  if fn:
    fn.setName('GetAtPos', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLinkedListInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60BD10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60BDD0))
  if fn:
    fn.setName('GetPrev', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLinkedListInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60BDD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60BE90))
  if fn:
    fn.setName('RemoveTail', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLinkedListInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60BE90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60BFC0))
  if fn:
    fn.setName('Contains', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLinkedListInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60BFC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x668850))
  if fn:
    fn.setName('RemoveItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x668850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x66A2B0))
  if fn:
    fn.setName('EventHandler', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x66A2B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C1750))
  if fn:
    fn.setName('_CSWSStore', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C1750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40C950))
  if fn:
    fn.setName('GetModuleCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C950: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40CEC0))
  if fn:
    fn.setName('CreatureAcquireItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CEC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40DD80))
  if fn:
    fn.setName('operator_new', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40DD80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41ADE0))
  if fn:
    fn.setName('SetEquippedBy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41ADE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x422D70))
  if fn:
    fn.setName('SwitchMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x422D70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x42B180))
  if fn:
    fn.setName('GetClientOptions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x42B180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44E510))
  if fn:
    fn.setName('_CExoString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44E510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4571A0))
  if fn:
    fn.setName('GetCreatureLight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4571A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4729F0))
  if fn:
    fn.setName('CExoString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4729F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x472AE0))
  if fn:
    fn.setName('GetLoopingAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x472AE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47DF90))
  if fn:
    fn.setName('GetCreatureByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47DF90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48FC50))
  if fn:
    fn.setName('_free', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48FC50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D70B0))
  if fn:
    fn.setName('CloseDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D70B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F32F0))
  if fn:
    fn.setName('operator_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F32F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x534110))
  if fn:
    fn.setName('CExoString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x534110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x586770))
  if fn:
    fn.setName('CSWGuiMessageBox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMessageBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMessageBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x586770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x58A600))
  if fn:
    fn.setName('GetInGameGui', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x58A600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D6910))
  if fn:
    fn.setName('CResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResRef', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D6910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x627790))
  if fn:
    fn.setName('_eh_vector_destructor_iterator_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x627790: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62CA40))
  if fn:
    fn.setName('_CExoKeyTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoKeyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62CA40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x637850))
  if fn:
    fn.setName('RemoveFromPlayingList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x637850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x639340))
  if fn:
    fn.setName('_pow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A9E0))
  if fn:
    fn.setName('PlayOneShotSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A9E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63EB30))
  if fn:
    fn.setName('ShutDownSource', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63EB30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63F3E0))
  if fn:
    fn.setName('SetPitchVariance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63F3E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x647C80))
  if fn:
    fn.setName('GetGameObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameObjectArray', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameObjectArray', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x647C80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64BBC0))
  if fn:
    fn.setName('GetDoorByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BBC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64BC20))
  if fn:
    fn.setName('StackPopObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachine', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachine', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BC20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C800))
  if fn:
    fn.setName('GetLoadFromSaveGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C800: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x667380))
  if fn:
    fn.setName('GetUsedActivePropertyUsesLeft', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x667380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x678640))
  if fn:
    fn.setName('GetPropertyStrings', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x678640: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6793C0))
  if fn:
    fn.setName('Format', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6793C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x68A3D0))
  if fn:
    fn.setName('ComputeAIState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x68A3D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A3CF0))
  if fn:
    fn.setName('GetListenCheckDistance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A3CF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A3E00))
  if fn:
    fn.setName('GetItemInSlot', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A3E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A4260))
  if fn:
    fn.setName('GetServerAIMaster', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A4260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A5190))
  if fn:
    fn.setName('GetIsBeam', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CLoopingVisualEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CLoopingVisualEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A5190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AE6A0))
  if fn:
    fn.setName('GetObjectArray', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AE6A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B1200))
  if fn:
    fn.setName('ClearAllEffects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B1200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B6D30))
  if fn:
    fn.setName('UpdateStatus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B6D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DC020))
  if fn:
    fn.setName('AddToVisibleList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DC020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x703DC0))
  if fn:
    fn.setName('GetPlayerList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x703DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x722210))
  if fn:
    fn.setName('NoNonWalkPolysDetailed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x722210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x722590))
  if fn:
    fn.setName('NoNonWalkPolysInDoors', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x722590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7239E0))
  if fn:
    fn.setName('GetMeshBoundingBox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCollisionMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCollisionMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7239E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x733C50))
  if fn:
    fn.setName('operator_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLocString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLocString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x733C50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734C20))
  if fn:
    fn.setName('SetCreator', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734C20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734C90))
  if fn:
    fn.setName('CGameEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734C90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74CA20))
  if fn:
    fn.setName('Allocate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74CA20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75ED20))
  if fn:
    fn.setName('GetPartyTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75ED20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75EEF0))
  if fn:
    fn.setName('GetArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75EEF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x76F960))
  if fn:
    fn.setName('ApplyEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x76F960: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x770E80))
  if fn:
    fn.setName('ClearData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRoundAction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRoundAction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x770E80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x778850))
  if fn:
    fn.setName('operator_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('SJournalEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'SJournalEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x778850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E3FD0))
  if fn:
    fn.setName('GetGameObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E3FD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x810750))
  if fn:
    fn.setName('IntersectLineSegments2d', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x810750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x811150))
  if fn:
    fn.setName('IntersectCircle2d', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x811150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86AF00))
  if fn:
    fn.setName('AurQueryAntiAliasingMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86AF00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90FF00))
  if fn:
    fn.setName('SetSize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90FF00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x914D30))
  if fn:
    fn.setName('CSWGuiTextParams', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTextParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTextParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x914D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x914E70))
  if fn:
    fn.setName('CResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResRef', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x914E70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9434E0))
  if fn:
    fn.setName('GetProperty0x2c', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9434E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41E770))
  if fn:
    fn.setName('DoFootstepVisual', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41E770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x428C00))
  if fn:
    fn.setName('PerformSetMine', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTutorialBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTutorialBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x428C00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x42C070))
  if fn:
    fn.setName('UpdateInventory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x42C070: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x42C720))
  if fn:
    fn.setName('HandleWMCharMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x42C720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x439850))
  if fn:
    fn.setName('SetHitcheckIgnoreList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x439850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x439C70))
  if fn:
    fn.setName('SetTexturePack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x439C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x43ABF0))
  if fn:
    fn.setName('StartMenuMusic', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x43ABF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x43ADB0))
  if fn:
    fn.setName('StopMenuMusic', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x43ADB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x444A00))
  if fn:
    fn.setName('HideLoadScreen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x444A00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4482C0))
  if fn:
    fn.setName('SetAmbientNightTrack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4482C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44AA20))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44AA20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44BA30))
  if fn:
    fn.setName('PlayerFlourishWeapons', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44BA30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44BD70))
  if fn:
    fn.setName('ChangeCharacterToNextLivingPartyMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44BD70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44EA00))
  if fn:
    fn.setName('UnloadArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44EA00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4544C0))
  if fn:
    fn.setName('ApplyFireAndForgetVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4544C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x475C30))
  if fn:
    fn.setName('RefreshVisualEffects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x475C30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x477110))
  if fn:
    fn.setName('SetPortraitId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x477110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47A530))
  if fn:
    fn.setName('AcceptParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPartySelection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPartySelection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47A530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47E090))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47E090: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48F710))
  if fn:
    fn.setName('HandleServerToPlayerDebugInfo_Creature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48F710: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4916B0))
  if fn:
    fn.setName('FeatRequirementsMet', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCLevelUpStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCLevelUpStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4916B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x495600))
  if fn:
    fn.setName('GetPortrait', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPortrait', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPortrait', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x495600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x495C40))
  if fn:
    fn.setName('CSoundSetList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSoundSetList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSoundSetList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x495C40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x49AF50))
  if fn:
    fn.setName('HandleServerToPlayerCreatureUpdate_VisEffec', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x49AF50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A5A40))
  if fn:
    fn.setName('HandleServerToPlayerModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A5A40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4AA2F0))
  if fn:
    fn.setName('HandleServerToPlayerUpdate_GuiInventory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4AA2F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4AA4B0))
  if fn:
    fn.setName('HandleServerToPlayerUpdate_GuiContainer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4AA4B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BF920))
  if fn:
    fn.setName('HandleServerToPlayerCheat_DoNasty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BF920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BFF10))
  if fn:
    fn.setName('HandleServerToPlayerJournalMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BFF10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4CCE20))
  if fn:
    fn.setName('SetGroundTilt', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4CCE20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D46F0))
  if fn:
    fn.setName('HandleDialogModels', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D46F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DBCE0))
  if fn:
    fn.setName('SetPrecalculatedPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DBCE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DF970))
  if fn:
    fn.setName('ShowStatusSummary', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DF970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E9700))
  if fn:
    fn.setName('Control', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPlayerControlCamRelative', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPlayerControlCamRelative', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E9700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x504BF0))
  if fn:
    fn.setName('ApplyMotionBlur', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x504BF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x50F540))
  if fn:
    fn.setName('LoadFeatsTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWClass', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWClass', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x50F540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x512F40))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterfaceAction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterfaceAction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x512F40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x514E50))
  if fn:
    fn.setName('CSWGuiLevelUpPanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x514E50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5210C0))
  if fn:
    fn.setName('SetHealth', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterfaceChar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterfaceChar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5210C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5365C0))
  if fn:
    fn.setName('GetTargetActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5365C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x53E000))
  if fn:
    fn.setName('OnModulePicked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x53E000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x546CC0))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x546CC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x549FE0))
  if fn:
    fn.setName('HandleServerToPlayerCCMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x549FE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x554D40))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x554D40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x557BA0))
  if fn:
    fn.setName('Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x557BA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55AC90))
  if fn:
    fn.setName('_CSWCSoundObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55AC90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55ADF0))
  if fn:
    fn.setName('LoadFromMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55ADF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x574BE0))
  if fn:
    fn.setName('UpdateShotCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x574BE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5792E0))
  if fn:
    fn.setName('_CSWCProjectile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCProjectile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5792E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5870D0))
  if fn:
    fn.setName('SetNextMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTutorialBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTutorialBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5870D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x587590))
  if fn:
    fn.setName('CSWGuiFlowSkillStruct', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFlowSkillStruct', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFlowSkillStruct', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x587590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5915E0))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5915E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x597FC0))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameCharacter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameCharacter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x597FC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59B420))
  if fn:
    fn.setName('OnPanelRemoved', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59B420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59F980))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59F980: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A0C20))
  if fn:
    fn.setName('ShowDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A0C20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A9300))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A9300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AD230))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAreaTransition', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAreaTransition', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AD230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B5200))
  if fn:
    fn.setName('OnFeatPicked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersFeatsSkillsDebugMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersFeatsSkillsDebugMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B5200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B7470))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgradeSelection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgradeSelection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B7470: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B8A10))
  if fn:
    fn.setName('OnControlEntered', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B8A10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5C47F0))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScriptSelect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScriptSelect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5C47F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5CFA50))
  if fn:
    fn.setName('OnUpgradeSelected', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgrade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5CFA50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DE910))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiClassSelection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiClassSelection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DE910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EA480))
  if fn:
    fn.setName('UpdatePlayerStats', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EA480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EE100))
  if fn:
    fn.setName('GetClassString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EE100: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x605430))
  if fn:
    fn.setName('HandleDoneButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiNameChargen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiNameChargen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x605430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x607550))
  if fn:
    fn.setName('AddString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLocString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLocString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x607550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x607A00))
  if fn:
    fn.setName('HandleServerAdminToServerMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x607A00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x613010))
  if fn:
    fn.setName('PollInput', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x613010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x616E60))
  if fn:
    fn.setName('HandleNotification', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoImeUI', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoImeUI', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x616E60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x621350))
  if fn:
    fn.setName('LoadOptions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientOptions', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientOptions', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x621350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62BB40))
  if fn:
    fn.setName('WriteResource', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CERFFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CERFFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62BB40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62BE00))
  if fn:
    fn.setName('Finish', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CERFFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CERFFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62BE00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62E950))
  if fn:
    fn.setName('AddKeyTableContents', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoKeyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62E950: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6300C0))
  if fn:
    fn.setName('FindKey', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoKeyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6300C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x638D80))
  if fn:
    fn.setName('PauseAllSounds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x638D80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64D1B0))
  if fn:
    fn.setName('UpdateState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIMaster', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64D1B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64E730))
  if fn:
    fn.setName('WalkUpdateLocation_QuickWalk', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64E730: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64E7A0))
  if fn:
    fn.setName('AIActionPlayAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64E7A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x650700))
  if fn:
    fn.setName('DeleteEventData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIMaster', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x650700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x659DB0))
  if fn:
    fn.setName('SignalRangedDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x659DB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x66BC50))
  if fn:
    fn.setName('AddProperty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x66BC50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67A8C0))
  if fn:
    fn.setName('SaveModuleIFOStart', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67A8C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A69C0))
  if fn:
    fn.setName('SaveObjectState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A69C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B6F00))
  if fn:
    fn.setName('AIActionMoveToPoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B6F00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6D5A90))
  if fn:
    fn.setName('AIActionDialogObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6D5A90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6E47F0))
  if fn:
    fn.setName('CSWGuiCreateItemMenu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCreateItemMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCreateItemMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6E47F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FDE90))
  if fn:
    fn.setName('OnApplyDeath', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FDE90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FEB00))
  if fn:
    fn.setName('EventHandler', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FEB00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7031C0))
  if fn:
    fn.setName('SpawnNPC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7031C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7034A0))
  if fn:
    fn.setName('KillNPCObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7034A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x72E330))
  if fn:
    fn.setName('SaveModuleFinish', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x72E330: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7380A0))
  if fn:
    fn.setName('OnApplyLightsaberThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7380A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73BDF0))
  if fn:
    fn.setName('OnApplySummonCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73BDF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x741FE0))
  if fn:
    fn.setName('OnApplyForceResistanceIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x741FE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x744740))
  if fn:
    fn.setName('OnApplySanctuary', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x744740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7465C0))
  if fn:
    fn.setName('OnRemoveHitPointChangeWhenDying', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7465C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x746CB0))
  if fn:
    fn.setName('OnApplyEffectIcon', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x746CB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B1130))
  if fn:
    fn.setName('EventHandler', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B1130: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B34F0))
  if fn:
    fn.setName('ExecuteLoadModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B34F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B8D50))
  if fn:
    fn.setName('MovePlayerToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B8D50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B9510))
  if fn:
    fn.setName('RunModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B9510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BC010))
  if fn:
    fn.setName('UnloadModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BC010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BCE70))
  if fn:
    fn.setName('MainLoop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BCE70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C5670))
  if fn:
    fn.setName('RemoveACBonus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C5670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E23A0))
  if fn:
    fn.setName('SendServerToPlayerJournalFullUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E23A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E8D40))
  if fn:
    fn.setName('JumpToPoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E8D40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F0B60))
  if fn:
    fn.setName('SaveVarTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSScriptVarTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSScriptVarTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F0B60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F70B0))
  if fn:
    fn.setName('AddToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F70B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8080A0))
  if fn:
    fn.setName('HandlePlayerToServerMapPinMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8080A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x811680))
  if fn:
    fn.setName('GetRandomName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWNameGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWNameGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x811680: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x811FA0))
  if fn:
    fn.setName('GetRandomName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWNameGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWNameGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x811FA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8586C0))
  if fn:
    fn.setName('Render', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Gob', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Gob', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8586C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x863920))
  if fn:
    fn.setName('SetTileLights', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Scene', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Scene', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x863920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86D990))
  if fn:
    fn.setName('SceneBSPAddPart', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86D990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86E3C0))
  if fn:
    fn.setName('DeleteNonGobParts', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86E3C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x897390))
  if fn:
    fn.setName('Mat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('PartTriMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'PartTriMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x897390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8C4B30))
  if fn:
    fn.setName('GetBoundingRect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8C4B30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8F6C10))
  if fn:
    fn.setName('BeginLookAtAnimate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Gob', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Gob', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8F6C10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x900350))
  if fn:
    fn.setName('EventHandler', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x900350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x918D40))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSlider', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSlider', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x918D40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91B720))
  if fn:
    fn.setName('SetExtent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91B720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86D5F0))
  if fn:
    fn.setName('SceneBSPAddTileData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86D5F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90B220))
  if fn:
    fn.setName('GetAuroraResDataSize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90B220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40DE80))
  if fn:
    fn.setName('AddPanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40DE80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41F8E0))
  if fn:
    fn.setName('HideEquippedItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41F8E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4443B0))
  if fn:
    fn.setName('SetActive', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInput', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4443B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4909E0))
  if fn:
    fn.setName('GetArmorClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4909E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x491BA0))
  if fn:
    fn.setName('GetBaseAttackBonus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCLevelUpStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCLevelUpStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x491BA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E2A60))
  if fn:
    fn.setName('PlayGuiSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E2A60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x520390))
  if fn:
    fn.setName('SetCombatMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x520390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5585C0))
  if fn:
    fn.setName('ResetLastUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5585C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A4650))
  if fn:
    fn.setName('ShowCantEquipMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A4650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A4790))
  if fn:
    fn.setName('UpdatePortraits', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A4790: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60B9E0))
  if fn:
    fn.setName('Seek', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60B9E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x626E10))
  if fn:
    fn.setName('GetTotalSize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x626E10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x630410))
  if fn:
    fn.setName('Hash', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoKeyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x630410: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6393E0))
  if fn:
    fn.setName('Pause', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6393E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63F010))
  if fn:
    fn.setName('return_zero', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63F010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63F4F0))
  if fn:
    fn.setName('CalculatePitchVarianceFrequency', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63F4F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64BC00))
  if fn:
    fn.setName('GetWaypointByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BC00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65AD40))
  if fn:
    fn.setName('SetInteger', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65AD40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x668830))
  if fn:
    fn.setName('GetModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x668830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B11B0))
  if fn:
    fn.setName('ClearDirtyFlag', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B11B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x912F70))
  if fn:
    fn.setName('GetProperty0x10', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x912F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x912FF0))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBorder', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBorder', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x912FF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x913150))
  if fn:
    fn.setName('GetInnerExtent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBorder', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBorder', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x913150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4269A0))
  if fn:
    fn.setName('GetDefaultActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4269A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x439480))
  if fn:
    fn.setName('UpdateCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x439480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x448660))
  if fn:
    fn.setName('ProcessInput', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x448660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DB560))
  if fn:
    fn.setName('LoadOptions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DB560: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DF280))
  if fn:
    fn.setName('PlayMoviesInExoArrayList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DF280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F03C0))
  if fn:
    fn.setName('SetPlayer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPlayerControl', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPlayerControl', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F03C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x502EC0))
  if fn:
    fn.setName('UpdateFOV', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x502EC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5139D0))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5139D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5868F0))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTutorialBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTutorialBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5868F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x58E470))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPartySelection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPartySelection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x58E470: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x605300))
  if fn:
    fn.setName('HandleRandomName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiNameChargen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiNameChargen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x605300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62D270))
  if fn:
    fn.setName('AddEncapsulatedContents', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoKeyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62D270: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x632060))
  if fn:
    fn.setName('LoadHeader', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoEncapsulatedFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoEncapsulatedFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x632060: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6587E0))
  if fn:
    fn.setName('ResolveRangedMiss', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6587E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67C870))
  if fn:
    fn.setName('SetId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67C870: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6C7B80))
  if fn:
    fn.setName('HandlePlayerToServerCheatMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6C7B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CAC50))
  if fn:
    fn.setName('CompareVisualEffectLists', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CAC50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90FFA0))
  if fn:
    fn.setName('DrawPCBG', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90FFA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x513D50))
  if fn:
    fn.setName('SetNameLabel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x513D50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47D960))
  if fn:
    fn.setName('UpdateIndicator', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47D960: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D6BC0))
  if fn:
    fn.setName('CleanUpReplyNode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D6BC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x540260))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStatusSummary', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStatusSummary', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x540260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x605040))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiNameChargen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiNameChargen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x605040: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91B440))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91B440: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x592150))
  if fn:
    fn.setName('MarkNewQuestsObserved', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x592150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5916E0))
  if fn:
    fn.setName('_CSWGuiInGameJournal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5916E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x591AE0))
  if fn:
    fn.setName('_CSWGuiInGameJournal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x591AE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x591FE0))
  if fn:
    fn.setName('OnControlEntered', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x591FE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x918130))
  if fn:
    fn.setName('OnPanelRemoved', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x918130: '+str(e)); errors += 1
print('Batch 2 done: renamed='+str(renamed)+' errors='+str(errors))