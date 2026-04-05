from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0
try:
  fn = fm.getFunctionAt(toAddr(0x7A8C70))
  if fn:
    fn.setName('ExecuteCommandSpawnAvailableNPC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A8C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A8D60))
  if fn:
    fn.setName('ExecuteCommandSpeakOneLinerConversation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A8D60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A8EB0))
  if fn:
    fn.setName('ExecuteCommandSpeakString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A8EB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A9110))
  if fn:
    fn.setName('ExecuteCommandActionSpeakStringByStrRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A9110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A9940))
  if fn:
    fn.setName('ExecuteCommandSurrenderToEnemies', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A9940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A9A50))
  if fn:
    fn.setName('ExecuteCommandSwitchPlayerCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A9A50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AA040))
  if fn:
    fn.setName('ExecuteCommandTakeGoldFromCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AA040: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AA460))
  if fn:
    fn.setName('ExecuteCommandTalent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AA460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AA620))
  if fn:
    fn.setName('ExecuteCommandTalentAccess', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AA620: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AA790))
  if fn:
    fn.setName('ExecuteCommandTimeConversions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AA790: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AA860))
  if fn:
    fn.setName('ExecuteCommandTestString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AA860: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AA9C0))
  if fn:
    fn.setName('ExecuteCommandActionUnequipItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AA9C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AAB50))
  if fn:
    fn.setName('ExecuteCommandVectorConversions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AAB50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AACD0))
  if fn:
    fn.setName('ExecuteCommandVectorMagnitude', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AACD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AAD40))
  if fn:
    fn.setName('ExecuteCommandVectorNormalize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AAD40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A63C0))
  if fn:
    fn.setName('ExecuteCommandActionWait', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A63C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AB660))
  if fn:
    fn.setName('ExecuteCommandGetIsLinkImmune', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AB660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79BD70))
  if fn:
    fn.setName('ExecuteCommandGetNPCAIStyle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79BD70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AB8A0))
  if fn:
    fn.setName('ExecuteCommandSetPartyLeader', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AB8A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A79F0))
  if fn:
    fn.setName('ExecuteCommandSetNPCAIStyle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A79F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x792490))
  if fn:
    fn.setName('ExecuteCommandGetStandardFaction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x792490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ABDF0))
  if fn:
    fn.setName('ExecuteCommandGivePlotXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ABDF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79CDE0))
  if fn:
    fn.setName('ExecuteCommandGetMinOneHP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79CDE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A7D10))
  if fn:
    fn.setName('ExecuteCommandSetMinOneHP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A7D10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AC240))
  if fn:
    fn.setName('ExecuteCommandGetAttacker', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AC240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x783180))
  if fn:
    fn.setName('ExecuteCommandSetFormation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x783180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A1450))
  if fn:
    fn.setName('ExecuteCommandGetIsDebilitated', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A1450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AC460))
  if fn:
    fn.setName('ExecuteCommandChangeByFaction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AC460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AC6E0))
  if fn:
    fn.setName('ExecuteCommandCancelCombat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AC6E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7940B0))
  if fn:
    fn.setName('ExecuteCommandGetItemStackSize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7940B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AC840))
  if fn:
    fn.setName('ExecuteCommandSetItemStackSize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AC840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ACC50))
  if fn:
    fn.setName('ExecuteCommandGetGalaxyMapPlanetAvailable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ACC50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B0910))
  if fn:
    fn.setName('ExecuteCommandGetGalaxyMapPlanetSelectable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B0910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ACD10))
  if fn:
    fn.setName('ExecuteCommandSetItemNonEquippable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ACD10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78F7C0))
  if fn:
    fn.setName('ExecuteCommandGetMaxStealthXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78F7C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AD080))
  if fn:
    fn.setName('ExecuteCommandSetMaxStealthXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AD080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ACF10))
  if fn:
    fn.setName('ExecuteCommandGetCurrentStealthXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ACF10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ACF90))
  if fn:
    fn.setName('ExecuteCommandSetCurrentStealthXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ACF90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AD0F0))
  if fn:
    fn.setName('ExecuteCommandAwardStealthXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AD0F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AD430))
  if fn:
    fn.setName('ExecuteCommandGetStealthXPEnabled', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AD430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AD4B0))
  if fn:
    fn.setName('ExecuteCommandSetStealthXPEnabled', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AD4B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A0FD0))
  if fn:
    fn.setName('ExecuteCommandGetSubRace', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A0FD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7933B0))
  if fn:
    fn.setName('ExecuteCommandGetBlockingCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7933B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AFC50))
  if fn:
    fn.setName('ExecuteCommandGetFoundEnemyCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AFC50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AD170))
  if fn:
    fn.setName('ExecuteCommandGetStealthXPDecrement', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AD170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AD1F0))
  if fn:
    fn.setName('ExecuteCommandSetStealthXPDecrement', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AD1F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AD6C0))
  if fn:
    fn.setName('ExecuteCommandSetLockOrientationInDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AD6C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AD750))
  if fn:
    fn.setName('ExecuteCommandSetLockHeadFollowInDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AD750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ADDF0))
  if fn:
    fn.setName('ExecuteCommandRevealMap', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ADDF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A6250))
  if fn:
    fn.setName('ExecuteCommandStartCreditSequence', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A6250: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AE1F0))
  if fn:
    fn.setName('ExecuteCommandSetEffectIcon', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AE1F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AE4D0))
  if fn:
    fn.setName('ExecuteCommandFaceObjectAwayFromObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AE4D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AE650))
  if fn:
    fn.setName('ExecuteCommandSetAreaFogColor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AE650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AE730))
  if fn:
    fn.setName('ExecuteCommandChangeItemCost', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AE730: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AE890))
  if fn:
    fn.setName('ExecuteCommandGetIsLiveContentAvailable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AE890: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79BEE0))
  if fn:
    fn.setName('ExecuteCommandGetIsPoisoned', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79BEE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AEC30))
  if fn:
    fn.setName('ExecuteCommandSetMaxHitPoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AEC30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AEEF0))
  if fn:
    fn.setName('ExecuteCommandCreateItemOnFloor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AEEF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AF280))
  if fn:
    fn.setName('ExecuteCommandSetAvailableNPCId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AF280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x781740))
  if fn:
    fn.setName('ExecuteCommandAddJournalQuestEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x781740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x781C40))
  if fn:
    fn.setName('ExecuteCommandAddJournalWorldEntryStrref', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x781C40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x783310))
  if fn:
    fn.setName('ExecuteCommandBarkString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x783310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x785A90))
  if fn:
    fn.setName('ExecuteCommandDeleteJournalWorldAllEntries', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x785A90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7866B0))
  if fn:
    fn.setName('ExecuteCommandDoTouchAttack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7866B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x791350))
  if fn:
    fn.setName('ExecuteCommandGetEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x791350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x795E10))
  if fn:
    fn.setName('ExecuteCommandGetItemInInventory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x795E10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x796A30))
  if fn:
    fn.setName('ExecuteCommandGetJournalEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x796A30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79A710))
  if fn:
    fn.setName('ExecuteCommandGetNearestMine', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79A710: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A5170))
  if fn:
    fn.setName('ExecuteCommandRemoveEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A5170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A5500))
  if fn:
    fn.setName('ExecuteCommandRemoveJournalQuestEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A5500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A6730))
  if fn:
    fn.setName('ExecuteCommandSetCustomToken', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A6730: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A7280))
  if fn:
    fn.setName('ExecuteCommandSetJournalQuestEntryPicture', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A7280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A7AE0))
  if fn:
    fn.setName('ExecuteCommandSetMapPinEnabled', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A7AE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x798B40))
  if fn:
    fn.setName('ExecuteCommandGetMatchedString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x798B40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5093F0))
  if fn:
    fn.setName('SaveData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CCombatInformation', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CCombatInformation', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5093F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x509820))
  if fn:
    fn.setName('LoadData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CCombatInformation', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CCombatInformation', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x509820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x663BB0))
  if fn:
    fn.setName('SetPossessor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x663BB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x671EE0))
  if fn:
    fn.setName('AddCriticalThreatProperties', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x671EE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BE170))
  if fn:
    fn.setName('AddOnHitProperties', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BE170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6688C0))
  if fn:
    fn.setName('AddItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CItemRepository', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CItemRepository', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6688C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x669E30))
  if fn:
    fn.setName('CopyItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x669E30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x770BF0))
  if fn:
    fn.setName('CloseItemForAllPlayers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x770BF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x66A650))
  if fn:
    fn.setName('AcquireItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x66A650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x66A8D0))
  if fn:
    fn.setName('DecrementUses', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x66A8D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E2040))
  if fn:
    fn.setName('AddAttackModifierProperties', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E2040: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x663910))
  if fn:
    fn.setName('_CSWSItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x663910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x666220))
  if fn:
    fn.setName('ReadContainerItemsFromGff', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x666220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63BA40))
  if fn:
    fn.setName('GetKeyedPropertyString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63BA40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75AF90))
  if fn:
    fn.setName('GetCharacterInfoFromIFO', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75AF90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75B2D0))
  if fn:
    fn.setName('LoadCreatureData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75B2D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75A650))
  if fn:
    fn.setName('LoadLocalCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75A650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75A880))
  if fn:
    fn.setName('LoadCharacterFromIFO', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75A880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75B140))
  if fn:
    fn.setName('LoadServerCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75B140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90E540))
  if fn:
    fn.setName('CSWSPlayer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90E540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x68DF20))
  if fn:
    fn.setName('LoadJournal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x68DF20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7048D0))
  if fn:
    fn.setName('UnstealthParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7048D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FD6F0))
  if fn:
    fn.setName('SaveJournal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FD6F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FDAA0))
  if fn:
    fn.setName('SaveMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FDAA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FE280))
  if fn:
    fn.setName('UpdateInventory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FE280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x703A70))
  if fn:
    fn.setName('DistributeExperience', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x703A70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FED90))
  if fn:
    fn.setName('CreateParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FED90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D92C0))
  if fn:
    fn.setName('StoreValuesInLastUpdatePartyObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D92C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DD830))
  if fn:
    fn.setName('SendServerToPlayerModule_Info', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DD830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D1770))
  if fn:
    fn.setName('SendServerToPlayerParty_List', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D1770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DE1A0))
  if fn:
    fn.setName('SendServerToPlayerPlayerList_Delete', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DE1A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DD600))
  if fn:
    fn.setName('SendServerToPlayerCheatNasty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DD600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E19A0))
  if fn:
    fn.setName('SendServerToPlayerGameObjUpdateVisEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E19A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E1BF0))
  if fn:
    fn.setName('SendServerToPlayerUpdateActiveItemProperties', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E1BF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E1CC0))
  if fn:
    fn.setName('SendServerToPlayerVoiceChat_Play', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E1CC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DD510))
  if fn:
    fn.setName('SendServerToPlayerGuiContainerObject_Open', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DD510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E1E80))
  if fn:
    fn.setName('SendServerPlayerItemUpdate_DestroyItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E1E80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D1B80))
  if fn:
    fn.setName('SendServerToPlayerJournalRemoveQuest', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D1B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E20B0))
  if fn:
    fn.setName('ExecuteCommandAddJournalWorldEntry', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E20B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E21B0))
  if fn:
    fn.setName('SendServerToPlayerJournalAddWorldStrref', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E21B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E3EB0))
  if fn:
    fn.setName('SendServerToPlayerJournalDeleteWorld', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E3EB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CD930))
  if fn:
    fn.setName('SendServerToPlayerJournalDeleteWorldStrref', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CD930: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E2330))
  if fn:
    fn.setName('SendServerToPlayerJournalDeleteWorldAll', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E2330: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E2BB0))
  if fn:
    fn.setName('SendServerToPlayerModuleUpdate_Time', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E2BB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E2D20))
  if fn:
    fn.setName('SendServerToPlayerMapPinAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E2D20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DF3E0))
  if fn:
    fn.setName('SendServerToPlayerDebugInfo_Item', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DF3E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E32B0))
  if fn:
    fn.setName('SendServerToPlayerDebugInfo_Placeable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E32B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E34A0))
  if fn:
    fn.setName('SendServerToPlayerDebugInfo_Area', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E34A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E22C0))
  if fn:
    fn.setName('SendServerToPlayerAmbientMusicSetDelay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E22C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CDA00))
  if fn:
    fn.setName('SendServerToPlayerAmbientBattleMusicChange', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CDA00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E3C00))
  if fn:
    fn.setName('SendServerToPlayerCharacterDownloadReply', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E3C00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DD5A0))
  if fn:
    fn.setName('SendServerToPlayerInventory_LearnScroll', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DD5A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CDC00))
  if fn:
    fn.setName('SendServerToPlayerShutDownServer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CDC00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DD4B0))
  if fn:
    fn.setName('SendServerToPlayerGUICharacterSheet_NotPermitted', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DD4B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D1E40))
  if fn:
    fn.setName('SendServerToPlayerItemScriptAdd', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D1E40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CB240))
  if fn:
    fn.setName('ComputeUpdateRequired', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CB240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CD670))
  if fn:
    fn.setName('SendServerToPlayerCamera_ChangeLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CD670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CD8D0))
  if fn:
    fn.setName('SendServerToPlayerCamera_SetMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CD8D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CD7D0))
  if fn:
    fn.setName('SendServerToPlayerLogin_CharacterQuery', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CD7D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CD760))
  if fn:
    fn.setName('SendServerToPlayerModule_SaveGameStatus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CD760: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E3F40))
  if fn:
    fn.setName('SendServerToPlayerLoadBar_StartStallEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E3F40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D0660))
  if fn:
    fn.setName('SendServerToPlayerLoadBar_EndStallEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D0660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D16C0))
  if fn:
    fn.setName('SendServerToPlayerArea_VisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D16C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D1550))
  if fn:
    fn.setName('SendServerToPlayerAIActionPlaySound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D1550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E3E40))
  if fn:
    fn.setName('SendServerToPlayerSoundObject_Play', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E3E40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D1650))
  if fn:
    fn.setName('SendServerToPlayerSoundObject_Stop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D1650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D1A30))
  if fn:
    fn.setName('SendServerToPlayerChat_Party', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D1A30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E2CB0))
  if fn:
    fn.setName('SendServerToPlayerQuickChat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E2CB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D17E0))
  if fn:
    fn.setName('SendServerToPlayerChat_Tell', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D17E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E2020))
  if fn:
    fn.setName('SendServerToPlayerChat_ServerTell', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E2020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D1C10))
  if fn:
    fn.setName('SendServerToPlayerChat_Shout', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D1C10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E31A0))
  if fn:
    fn.setName('SendServerToPlayerChat_Whisper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E31A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D8930))
  if fn:
    fn.setName('SendServerToPlayerSafeProjectileProjectile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D8930: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D8A30))
  if fn:
    fn.setName('SendServerToPlayerSafeProjectileSpell', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D8A30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D8BD0))
  if fn:
    fn.setName('TestPartyObjectUpdateDifferences', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D8BD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DC320))
  if fn:
    fn.setName('SendServerToPlayerExamineGui_PlaceableData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DC320: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DC850))
  if fn:
    fn.setName('SendServerToPlayerExamineGui_DoorData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DC850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DD450))
  if fn:
    fn.setName('SendServerToPlayerInventory_Drop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DD450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E18D0))
  if fn:
    fn.setName('SendServerToPlayerInventory_DropCancel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E18D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D15E0))
  if fn:
    fn.setName('SendServerToPlayerInventory_Pickup', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D15E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DD6E0))
  if fn:
    fn.setName('SendServerToPlayerInventory_PickupCancel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DD6E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E4270))
  if fn:
    fn.setName('SendServerToPlayerInventory_UnequipCancel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E4270: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E1DC0))
  if fn:
    fn.setName('SendServerToAllPlayersCreatureUpdate_StripEquippedItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E1DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E2E00))
  if fn:
    fn.setName('SendServerToPlayerDebugInfo_Creature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E2E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D1380))
  if fn:
    fn.setName('SendServerToPlayerChatMultiLangMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D1380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D07B0))
  if fn:
    fn.setName('SendServerToPlayerChatStrRefMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D07B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D7D00))
  if fn:
    fn.setName('StoreValuesInLastPlayerUpdateObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D7D00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x50FE20))
  if fn:
    fn.setName('GetLocStringServer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x50FE20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44CE60))
  if fn:
    fn.setName('SendServerToServerAdminServerStatus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44CE60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80AA70))
  if fn:
    fn.setName('SendServerToServerAdminPlayerList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80AA70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B5A80))
  if fn:
    fn.setName('AIActionSpeak', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B5A80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B66C0))
  if fn:
    fn.setName('AIActionGiveItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B66C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B6D70))
  if fn:
    fn.setName('AIActionTakeItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B6D70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6BD190))
  if fn:
    fn.setName('AddGiveItemActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6BD190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6BD340))
  if fn:
    fn.setName('AddTakeItemActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6BD340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B3670))
  if fn:
    fn.setName('AIActionBarkString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B3670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B18A0))
  if fn:
    fn.setName('RunActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B18A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x710FB0))
  if fn:
    fn.setName('SaveBinaryMesh', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRoomSurfaceMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRoomSurfaceMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x710FB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65A650))
  if fn:
    fn.setName('CSWRoomSurfaceMesh', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRoomSurfaceMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRoomSurfaceMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65A650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7137E0))
  if fn:
    fn.setName('ClippedLineSegmentWalkable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRoomSurfaceMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRoomSurfaceMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7137E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x76CD30))
  if fn:
    fn.setName('PostProcess', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x76CD30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x76D320))
  if fn:
    fn.setName('RemoveFromArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x76D320: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x76F2A0))
  if fn:
    fn.setName('AcquireItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x76F2A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x770400))
  if fn:
    fn.setName('AddCastSpellActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x770400: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7695A0))
  if fn:
    fn.setName('_CSWSPlaceable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7695A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x769940))
  if fn:
    fn.setName('LoadPlaceable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x769940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x76BC90))
  if fn:
    fn.setName('SavePlaceable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x76BC90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x76FB40))
  if fn:
    fn.setName('CloseInventory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x76FB40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x769130))
  if fn:
    fn.setName('CSWSPlaceable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x769130: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7697A0))
  if fn:
    fn.setName('LoadFromTemplate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7697A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x76D3B0))
  if fn:
    fn.setName('EventHandler', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x76D3B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x76F530))
  if fn:
    fn.setName('AcquireItemsFromObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x76F530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7673F0))
  if fn:
    fn.setName('SaveDoor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7673F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x762440))
  if fn:
    fn.setName('_CSWSDoor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x762440: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75D7D0))
  if fn:
    fn.setName('GetTargetArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75D7D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7619E0))
  if fn:
    fn.setName('GetCanFireMineOnObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7619E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75D5F0))
  if fn:
    fn.setName('AIUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75D5F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ED770))
  if fn:
    fn.setName('SaveEncounter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ED770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EE450))
  if fn:
    fn.setName('SpawnCreatureFromSpawnList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EE450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E9E80))
  if fn:
    fn.setName('RemoveFromArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E9E80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E9520))
  if fn:
    fn.setName('_CSWSEncounter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E9520: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E9DF0))
  if fn:
    fn.setName('AddToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E9DF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E97E0))
  if fn:
    fn.setName('AIUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E97E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EB020))
  if fn:
    fn.setName('RemoveFromActiveCreatureCount', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EB020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E7920))
  if fn:
    fn.setName('LoadAreaEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E7920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E8090))
  if fn:
    fn.setName('SaveEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E8090: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E9000))
  if fn:
    fn.setName('SetCreator', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E9000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E8DE0))
  if fn:
    fn.setName('RemoveFromSubAreas', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E8DE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E6530))
  if fn:
    fn.setName('AIUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E6530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E6E70))
  if fn:
    fn.setName('RemoveFromArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E6E70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E63C0))
  if fn:
    fn.setName('_CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E63C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x724810))
  if fn:
    fn.setName('NoNonWalkPolys', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCollisionMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCollisionMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x724810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x724460))
  if fn:
    fn.setName('SaveBinaryMesh', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCollisionMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCollisionMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x724460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EFE10))
  if fn:
    fn.setName('MatchIndex', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSScriptVarTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSScriptVarTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EFE10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F01C0))
  if fn:
    fn.setName('GetLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSScriptVarTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSScriptVarTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F01C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F0290))
  if fn:
    fn.setName('SetInt', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSScriptVarTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSScriptVarTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F0290: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74CF50))
  if fn:
    fn.setName('SaveVarTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVarTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVarTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74CF50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74F290))
  if fn:
    fn.setName('CSWSDialogBase', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialogBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialogBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74F290: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74D6C0))
  if fn:
    fn.setName('ClearDialogOwnerInObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74D6C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x750CA0))
  if fn:
    fn.setName('CheckScript', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x750CA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x750D80))
  if fn:
    fn.setName('RunScript', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x750D80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x752090))
  if fn:
    fn.setName('GetStartEntryOneLiner', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x752090: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x752610))
  if fn:
    fn.setName('AddJournalEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x752610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74F4F0))
  if fn:
    fn.setName('LoadDialogBase', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74F4F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7539A0))
  if fn:
    fn.setName('GetDialogModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7539A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7582E0))
  if fn:
    fn.setName('ClearAllPlayerHostileActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7582E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x758450))
  if fn:
    fn.setName('RunEndConversationScript', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x758450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x758B60))
  if fn:
    fn.setName('UnequipItemsForDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x758B60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x758D60))
  if fn:
    fn.setName('UnequipHItemForDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x758D60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x751AF0))
  if fn:
    fn.setName('BarkOneLine', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x751AF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x752910))
  if fn:
    fn.setName('SendDialogEntryNode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x752910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x750E20))
  if fn:
    fn.setName('GetStartEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x750E20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FC7B0))
  if fn:
    fn.setName('GetHasPowerFromItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FC7B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FCB30))
  if fn:
    fn.setName('GetItemDetailsForPower', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FCB30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65E7B0))
  if fn:
    fn.setName('GetMeleeAttackBonus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65E7B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F2A80))
  if fn:
    fn.setName('GetRangedDamageBonus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F2A80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F7820))
  if fn:
    fn.setName('GetCreatureTalentRandomPowerFromItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F7820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F9420))
  if fn:
    fn.setName('GetCreatureTalentRandomKnownPower', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F9420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FB270))
  if fn:
    fn.setName('AutoLevelup', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FB270: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48C120))
  if fn:
    fn.setName('IsCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48C120: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x658D20))
  if fn:
    fn.setName('ResolveProjectileMissImpact', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x658D20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65AB90))
  if fn:
    fn.setName('GetWeaponDischargeHand', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65AB90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x789030))
  if fn:
    fn.setName('CreateLinkedEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x789030: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6614D0))
  if fn:
    fn.setName('CreateLinkedEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6614D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7382D0))
  if fn:
    fn.setName('SignalMeleeDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7382D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65ECB0))
  if fn:
    fn.setName('ResolveMoveToForceJump', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65ECB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65D300))
  if fn:
    fn.setName('ResolveSneakAttack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65D300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65DA80))
  if fn:
    fn.setName('ResolvePostMeleeDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65DA80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x661E10))
  if fn:
    fn.setName('ResolvePostRangedDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x661E10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x660930))
  if fn:
    fn.setName('SavingThrowRoll', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x660930: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x662200))
  if fn:
    fn.setName('ApplyOnHitAbilityDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x662200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x662690))
  if fn:
    fn.setName('ApplyOnHitSlayRacialGroup', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x662690: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x662900))
  if fn:
    fn.setName('ApplyOnHitInstantDeath', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x662900: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x662B80))
  if fn:
    fn.setName('GetFirstAttacker', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x662B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65B5A0))
  if fn:
    fn.setName('ResolveMeleeSpecialAttack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65B5A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65AD90))
  if fn:
    fn.setName('ResolveMeleeAttack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65AD90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x656FA0))
  if fn:
    fn.setName('ResolveAttack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x656FA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E4E40))
  if fn:
    fn.setName('GetWeakestMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E4E40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E4FA0))
  if fn:
    fn.setName('GetStrongestMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E4FA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E4610))
  if fn:
    fn.setName('CSWSFaction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E4610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E4A50))
  if fn:
    fn.setName('RemoveMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E4A50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x480C10))
  if fn:
    fn.setName('Load2DArrays_Appearance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x480C10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x481510))
  if fn:
    fn.setName('Load2DArrays_AppearanceSounds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x481510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x482620))
  if fn:
    fn.setName('Load2DArrays_Gender', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x482620: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x481740))
  if fn:
    fn.setName('Load2DArrays_SurfaceMaterial', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x481740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x481920))
  if fn:
    fn.setName('Load2DArrays_VisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x481920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x481D80))
  if fn:
    fn.setName('Load2DArrays_PersistentVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x481D80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x483240))
  if fn:
    fn.setName('Load2DArrays_CreatureSpeed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x483240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x482710))
  if fn:
    fn.setName('Load2DArrays_DoorTypes', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x482710: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x481830))
  if fn:
    fn.setName('Load2DArrays_GenericDoors', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x481830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x482800))
  if fn:
    fn.setName('Load2DArrays_Placeables', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x482800: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x482970))
  if fn:
    fn.setName('Load2DArrays_PlaceableSounds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x482970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x484BB0))
  if fn:
    fn.setName('Load2DArrays_FootstepSounds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x484BB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x483370))
  if fn:
    fn.setName('Load2DArrays_WeaponSounds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x483370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4824F0))
  if fn:
    fn.setName('Load2DArrays_Regeneration', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4824F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x484610))
  if fn:
    fn.setName('Load2DArrays_Animations', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x484610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x485170))
  if fn:
    fn.setName('Load2DArrays_DialogAnimations', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x485170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x489180))
  if fn:
    fn.setName('LoadIPRPParamTables', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x489180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4805B0))
  if fn:
    fn.setName('Load2DArrays', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4805B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77A210))
  if fn:
    fn.setName('SetDate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77A210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x779E50))
  if fn:
    fn.setName('SetTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x779E50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C0490))
  if fn:
    fn.setName('_CSWSStore', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C0490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C0E00))
  if fn:
    fn.setName('SaveStore', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C0E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C1450))
  if fn:
    fn.setName('SellItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C1450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C1230))
  if fn:
    fn.setName('AcquireItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C1230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x562410))
  if fn:
    fn.setName('SetRoomEnvAudioProps', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x562410: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F5E30))
  if fn:
    fn.setName('SaveWaypoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSWaypoint', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSWaypoint', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F5E30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F6010))
  if fn:
    fn.setName('LoadFromTemplate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSWaypoint', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSWaypoint', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F6010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F5850))
  if fn:
    fn.setName('_CSWSWaypoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSWaypoint', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSWaypoint', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F5850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F6CA0))
  if fn:
    fn.setName('Save', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F6CA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F7770))
  if fn:
    fn.setName('ChangePosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F7770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F63C0))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F63C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x505EA0))
  if fn:
    fn.setName('PackIntoMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x505EA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F5380))
  if fn:
    fn.setName('GetPlayersInArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F5380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F99E0))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetLastEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F99E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F98E0))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetLastEventModelName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F98E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F9B00))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetObjectByName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F9B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F9C00))
  if fn:
    fn.setName('ExecuteCommandSWMG_PlayAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F9C00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FA250))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetObjectName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FA250: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FA540))
  if fn:
    fn.setName('ExecuteCommandSWMG_RemoveAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FA540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FAE10))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetGunBankStringInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FAE10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FAF80))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetGunBankIntInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FAF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FB080))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetGunBankFloatInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FB080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FB170))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetTargettingGunBankFloatInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FB170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A3ED0))
  if fn:
    fn.setName('ExecuteCommandSWMG_SetGunBankStringInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A3ED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FB500))
  if fn:
    fn.setName('ExecuteCommandSWMG_SetGunBankFloatInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FB500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FB600))
  if fn:
    fn.setName('ExecuteCommandSWMG_SetTargettingGunBankFloatInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FB600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FBD00))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetSoundProperty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FBD00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F8920))
  if fn:
    fn.setName('LoadMeshText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPlaceableSurfaceMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPlaceableSurfaceMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F8920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F7C10))
  if fn:
    fn.setName('LoadMeshText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWDoorSurfaceMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWDoorSurfaceMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F7C10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F8810))
  if fn:
    fn.setName('GetResourceForBinaryWalkMesh', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWDoorSurfaceMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWDoorSurfaceMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F8810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F8490))
  if fn:
    fn.setName('SaveBinaryMesh', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWDoorSurfaceMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWDoorSurfaceMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F8490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x648370))
  if fn:
    fn.setName('ComputeHexPoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAvoidCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAvoidCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x648370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64A2F0))
  if fn:
    fn.setName('SideClearOfObstructions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAvoidCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAvoidCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64A2F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64A560))
  if fn:
    fn.setName('PlotPathAroundCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAvoidCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAvoidCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64A560: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6467D0))
  if fn:
    fn.setName('LoadScriptSituation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachine', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachine', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6467D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90A200))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResNCS,2010>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResNCS,2010>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90A200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x641FC0))
  if fn:
    fn.setName('SaveStack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineStack', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineStack', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x641FC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6421C0))
  if fn:
    fn.setName('LoadStack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineStack', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineStack', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6421C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x907DE0))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResWave,4>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResWave,4>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x907DE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x636C70))
  if fn:
    fn.setName('PlaySourceOn3DVoice', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x636C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x635FD0))
  if fn:
    fn.setName('OcclusionRender', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x635FD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63CD80))
  if fn:
    fn.setName('Pause', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63CD80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63B060))
  if fn:
    fn.setName('Play', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63B060: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x432170))
  if fn:
    fn.setName('Read', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CERFFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CERFFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x432170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6331C0))
  if fn:
    fn.setName('ReadHeaderVariance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CERFFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CERFFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6331C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62BE80))
  if fn:
    fn.setName('ExportFilesFromERF', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CERFFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CERFFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62BE80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B3C30))
  if fn:
    fn.setName('WriteResource', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CERFFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CERFFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B3C30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x909B50))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResLYT,3000>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResLYT,3000>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x909B50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x616C60))
  if fn:
    fn.setName('handleCompositionString_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoImeUI', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoImeUI', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x616C60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x616F00))
  if fn:
    fn.setName('UpdateCandidateList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoImeUI', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoImeUI', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x616F00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6133C0))
  if fn:
    fn.setName('WriteEventToFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6133C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60FA70))
  if fn:
    fn.setName('Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoAliasList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoAliasList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60FA70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6084B0))
  if fn:
    fn.setName('GetGameVersionInfo', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6084B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x609E80))
  if fn:
    fn.setName('GetDirectorySize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoBaseInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoBaseInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x609E80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B4FD0))
  if fn:
    fn.setName('LoadAliases', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoBaseInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoBaseInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B4FD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x609660))
  if fn:
    fn.setName('GetDirectoryList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoBaseInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoBaseInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x609660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60E760))
  if fn:
    fn.setName('WriteIniEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoIniInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoIniInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60E760: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A6940))
  if fn:
    fn.setName('ResolveFileName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoAliasListInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoAliasListInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A6940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6100A0))
  if fn:
    fn.setName('GetSize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFileInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFileInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6100A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x449120))
  if fn:
    fn.setName('SetLoadScreenByModuleName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x449120: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4495B0))
  if fn:
    fn.setName('SetLoadBarProgress', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4495B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44A460))
  if fn:
    fn.setName('AddFloatyText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44A460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44B610))
  if fn:
    fn.setName('LookUpAndPerformRumbleWithCutOff', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44B610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44CC30))
  if fn:
    fn.setName('BusyWaitMuteSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44CC30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x42C8A0))
  if fn:
    fn.setName('SetPrimaryPCPortraitId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x42C8A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x436970))
  if fn:
    fn.setName('LoadItemDropSoundList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x436970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x443420))
  if fn:
    fn.setName('InitializeClientOptions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x443420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4437C0))
  if fn:
    fn.setName('InitializeSoundOptions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4437C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x449670))
  if fn:
    fn.setName('StartPazaakGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x449670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x449830))
  if fn:
    fn.setName('EndPazaakGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x449830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44B050))
  if fn:
    fn.setName('AddDownloadedResources', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44B050: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BDDB0))
  if fn:
    fn.setName('GetNextLoadScreenHintSTRREF', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BDDB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44CA90))
  if fn:
    fn.setName('SetMouseMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44CA90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x436B80))
  if fn:
    fn.setName('ReadAndSetVideoMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x436B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x430D60))
  if fn:
    fn.setName('SetPauseState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x430D60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x445390))
  if fn:
    fn.setName('ShowObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x445390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44D040))
  if fn:
    fn.setName('AddMovieToExoArrayList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44D040: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x43A6C0))
  if fn:
    fn.setName('ShutDownToMainMenu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x43A6C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x444CC0))
  if fn:
    fn.setName('PlayLoadModuleMovies', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x444CC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4575D0))
  if fn:
    fn.setName('GetRoom', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4575D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x456940))
  if fn:
    fn.setName('AddPlaceableObjectStatic', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x456940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x457EA0))
  if fn:
    fn.setName('CheatSetSkill', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x457EA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4582A0))
  if fn:
    fn.setName('addexp', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4582A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4585B0))
  if fn:
    fn.setName('adddarkside', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4585B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x458B20))
  if fn:
    fn.setName('warp', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x458B20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x458CA0))
  if fn:
    fn.setName('whereami', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x458CA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x488DC0))
  if fn:
    fn.setName('HitGroundEvent', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x488DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x412FC0))
  if fn:
    fn.setName('ResetCameraToFollow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x412FC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x413620))
  if fn:
    fn.setName('SetDesiredOrientationForLock', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x413620: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4138F0))
  if fn:
    fn.setName('HandleConjuringCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4138F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x416810))
  if fn:
    fn.setName('RequestModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x416810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x418B00))
  if fn:
    fn.setName('GetSaveCreatureFilename', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x418B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6556C0))
  if fn:
    fn.setName('IsCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6556C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x671110))
  if fn:
    fn.setName('SetName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x671110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41F430))
  if fn:
    fn.setName('DoFootstepAudio', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41F430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x420A90))
  if fn:
    fn.setName('EvaluateNewWayPoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x420A90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x428EF0))
  if fn:
    fn.setName('ActionInitiateDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x428EF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x698A80))
  if fn:
    fn.setName('LoadRollingFootstepSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x698A80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x427150))
  if fn:
    fn.setName('LipSync', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x427150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4102B0))
  if fn:
    fn.setName('CSWCCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4102B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x424900))
  if fn:
    fn.setName('EnableFeatForMenu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x424900: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x42A2F0))
  if fn:
    fn.setName('GetTargetActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x42A2F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x468430))
  if fn:
    fn.setName('SetMovieShown', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientOptions', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientOptions', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x468430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x468660))
  if fn:
    fn.setName('GetMovieShown', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientOptions', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientOptions', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x468660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x460170))
  if fn:
    fn.setName('PerformLButtonDownAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x460170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4608D0))
  if fn:
    fn.setName('HandleMouseClickInWorld', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4608D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x460290))
  if fn:
    fn.setName('PerformLButtonUpAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x460290: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x45DED0))
  if fn:
    fn.setName('caseD_1', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('switchD_00621238', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'switchD_00621238', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x45DED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x45DAA0))
  if fn:
    fn.setName('PlayBackInputEvents', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x45DAA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AC6E0))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AC6E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AC940))
  if fn:
    fn.setName('SetMsg', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AC940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AC420))
  if fn:
    fn.setName('ResetFade', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AC420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AC230))
  if fn:
    fn.setName('CSWGuiFade', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AC230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x590190))
  if fn:
    fn.setName('ShowDialogMessages', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameMessages', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameMessages', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x590190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54E5B0))
  if fn:
    fn.setName('CSWGuiStatusSummary', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStatusSummary', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStatusSummary', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54E5B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x529340))
  if fn:
    fn.setName('CSWGuiInGameMenu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x529340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D01A0))
  if fn:
    fn.setName('ShowDialogEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D01A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D6890))
  if fn:
    fn.setName('SetComputerCameraShot', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D6890: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D9F40))
  if fn:
    fn.setName('UnloadStuntModelsForDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D9F40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DA4F0))
  if fn:
    fn.setName('ShowSWInGameGui', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DA4F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DA760))
  if fn:
    fn.setName('HideSWInGameGui', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DA760: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DAAC0))
  if fn:
    fn.setName('NextSWInGameGui', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DAAC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DA930))
  if fn:
    fn.setName('PrevSWInGameGui', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DA930: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DAC50))
  if fn:
    fn.setName('SwitchToSWInGameGui', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DAC50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DAE30))
  if fn:
    fn.setName('ShowGalaxyMapGui', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DAE30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DB020))
  if fn:
    fn.setName('HideGalaxyMapGui', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DB020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DBEC0))
  if fn:
    fn.setName('ShowLevelUpGUI', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DBEC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DD0A0))
  if fn:
    fn.setName('HandleDialogReplyChosen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DD0A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DD440))
  if fn:
    fn.setName('ShowStoreGui', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DD440: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DD630))
  if fn:
    fn.setName('HideStoreGui', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DD630: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DDC10))
  if fn:
    fn.setName('ShowSoloModeQuery', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DDC10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DE2E0))
  if fn:
    fn.setName('HideSoloMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DE2E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DE4A0))
  if fn:
    fn.setName('ShowUpgradeScreen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DE4A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DE3C0))
  if fn:
    fn.setName('HideUpgradeScreen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DE3C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DEAE0))
  if fn:
    fn.setName('SetShotFromScript', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DEAE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4431C0))
  if fn:
    fn.setName('SetGlobalDialogState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4431C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DF360))
  if fn:
    fn.setName('GetDialogCameraPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DF360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E0210))
  if fn:
    fn.setName('ShowTutorialWindow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E0210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DEE80))
  if fn:
    fn.setName('UpdateAreaTransitionGUI', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DEE80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E09F0))
  if fn:
    fn.setName('AddToDialogNonDependantList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E09F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E1C30))
  if fn:
    fn.setName('UnpackQuickSaveGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E1C30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D8450))
  if fn:
    fn.setName('HandleDialogSelection', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D8450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DD7F0))
  if fn:
    fn.setName('UpdateDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DD7F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D6D00))
  if fn:
    fn.setName('ShowDialogReplies', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D6D00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DCCC0))
  if fn:
    fn.setName('HandleDialogReplies', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DCCC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x478A20))
  if fn:
    fn.setName('SetLeader', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWParty', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWParty', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x478A20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47C8F0))
  if fn:
    fn.setName('SmoothFollowPath', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyFollowDataArray', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyFollowDataArray', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47C8F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x478360))
  if fn:
    fn.setName('AddCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWParty', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWParty', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x478360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B37A0))
  if fn:
    fn.setName('TestAndResetPath', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWParty', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWParty', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B37A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47AAD0))
  if fn:
    fn.setName('Reset', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWParty', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWParty', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47AAD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47AF80))
  if fn:
    fn.setName('ResetAllFollowData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWParty', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWParty', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47AF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E8350))
  if fn:
    fn.setName('Control', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCameraFreeLook', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCameraFreeLook', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E8350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x585020))
  if fn:
    fn.setName('SetEnvMap', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x585020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x475550))
  if fn:
    fn.setName('SetEnvMap', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x475550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4767B0))
  if fn:
    fn.setName('SetDesiredFadeState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4767B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4773E0))
  if fn:
    fn.setName('QueuedCallback', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4773E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x476430))
  if fn:
    fn.setName('FadeObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x476430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x46B0C0))
  if fn:
    fn.setName('TurnCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x46B0C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x46D900))
  if fn:
    fn.setName('LoadAnimatedCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x46D900: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x46DA10))
  if fn:
    fn.setName('SetAnimatedCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x46DA10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x46DFF0))
  if fn:
    fn.setName('SetFreeLookCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x46DFF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x728E20))
  if fn:
    fn.setName('CSWCModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x728E20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x591BD0))
  if fn:
    fn.setName('PopulateItemListBox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x591BD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D6970))
  if fn:
    fn.setName('PlayAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D6970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47E1F0))
  if fn:
    fn.setName('CSWCItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47E1F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47E5F0))
  if fn:
    fn.setName('NVThunkSWCItmSWCObject_AIUpdate', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47E5F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47EC60))
  if fn:
    fn.setName('PlayDropSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47EC60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47F0A0))
  if fn:
    fn.setName('SpawnVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47F0A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47F970))
  if fn:
    fn.setName('PowerItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47F970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x493D20))
  if fn:
    fn.setName('CheckCustomPortrait', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x493D20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x491C30))
  if fn:
    fn.setName('AddGrantedFeats', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCLevelUpStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCLevelUpStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x491C30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x496590))
  if fn:
    fn.setName('CreateItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x496590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x497320))
  if fn:
    fn.setName('HandleServerToPlayerSetCustomToken', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x497320: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x49BAD0))
  if fn:
    fn.setName('HandleServerToPlayerCreatureUpdate_ControlObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x49BAD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A1450))
  if fn:
    fn.setName('HandleServerToPlayerDoorUpdate_Delete', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A1450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A2160))
  if fn:
    fn.setName('HandleServerToPlayerPlaceableUpdate_Delete', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A2160: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A2360))
  if fn:
    fn.setName('HandleServerToPlayerUpdateVisualEffects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A2360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A3550))
  if fn:
    fn.setName('HandleServerToPlayerGuiInventory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A3550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A3E60))
  if fn:
    fn.setName('HandleServerToPlayerActiveItemProperties', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A3E60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A43D0))
  if fn:
    fn.setName('HandleServerToPlayerItemAppearance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A43D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A49B0))
  if fn:
    fn.setName('HandleServerToPlayerItemUpdate_Delete', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A49B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A4BD0))
  if fn:
    fn.setName('HandleServerToPlayerLogin', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A4BD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A5C20))
  if fn:
    fn.setName('HandleServerToPlayerSoundObject_Play', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A5C20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A5C90))
  if fn:
    fn.setName('HandleServerToPlayerSoundObject_Stop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A5C90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A5D00))
  if fn:
    fn.setName('HandleServerToPlayerSoundObject_ChangePosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A5D00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A2950))
  if fn:
    fn.setName('HandleServerToPlayerParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A2950: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A7370))
  if fn:
    fn.setName('HandleServerToPlayerTriggerUpdate_Delete', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A7370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4AA1D0))
  if fn:
    fn.setName('HandleServerToPlayerUpdate_GuiQuickbarItemUseCount', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4AA1D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4AA610))
  if fn:
    fn.setName('HandleServerToPlayerUpdate_GuiRepository', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4AA610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BD010))
  if fn:
    fn.setName('GetSingleCustomTokenStringPart', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BD010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BDEA0))
  if fn:
    fn.setName('HandleServerToPlayerSafeProjectileSpell', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BDEA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BEE80))
  if fn:
    fn.setName('HandleServerToPlayerCombatRound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BEE80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BEF20))
  if fn:
    fn.setName('HandleServerToPlayerActiveItemPropertyUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BEF20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A5DC0))
  if fn:
    fn.setName('HandleServerToPlayerGuiContainerObjectMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A5DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BF410))
  if fn:
    fn.setName('HandleServerToPlayerGenericObjectUpdate_Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BF410: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BF710))
  if fn:
    fn.setName('HandleServerToPlayerCreatureUpdate_StripEquippedItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BF710: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BF770))
  if fn:
    fn.setName('HandleServerToPlayerItemUpdate_DestroyItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BF770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BF840))
  if fn:
    fn.setName('HandleServerToPlayerVoiceChatMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BF840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C0540))
  if fn:
    fn.setName('HandleServerToPlayerJournalMessage_RemoveQuest', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C0540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C1EF0))
  if fn:
    fn.setName('HandleServerToPlayerJournalMessage_AddWorld', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C1EF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C0920))
  if fn:
    fn.setName('HandleServerToPlayerJournalMessage_AddWorldStrref', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C0920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C6F90))
  if fn:
    fn.setName('HandleServerToPlayerJournalMessage_DeleteWorld', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C6F90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C0A30))
  if fn:
    fn.setName('HandleServerToPlayerJournalMessage_DeleteWorldStrref', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C0A30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C0AA0))
  if fn:
    fn.setName('HandleServerToPlayerJournalMessage_DeleteWorldAll', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C0AA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BFFE0))
  if fn:
    fn.setName('HandleServerToPlayerJournalMessage_FullQuestUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BFFE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C10C0))
  if fn:
    fn.setName('HandleServerToPlayerJournalMessage_Updated', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C10C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C1200))
  if fn:
    fn.setName('HandleServerToPlayerLevelUpMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C1200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C6910))
  if fn:
    fn.setName('HandleServerToPlayerModule_EndStartNewModul', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C6910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C1310))
  if fn:
    fn.setName('HandleServerToPlayerModuleUpdate_Time', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C1310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C1950))
  if fn:
    fn.setName('HandleServerToPlayerSaveGameStatus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C1950: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C1E60))
  if fn:
    fn.setName('HandleServerToPlayerMapPinEnabled', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C1E60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C5820))
  if fn:
    fn.setName('HandleServerToPlayerDebugInfo_Placeable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C5820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C6A50))
  if fn:
    fn.setName('HandleServerToPlayerAmbientMusicPlay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C6A50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C6AF0))
  if fn:
    fn.setName('HandleServerToPlayerAmbientBattleMusicPlay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C6AF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C6830))
  if fn:
    fn.setName('HandleServerToPlayerAmbientSoundLoopPlay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C6830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C6C80))
  if fn:
    fn.setName('HandleServerToPlayerCharacterDownload', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C6C80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C7020))
  if fn:
    fn.setName('HandlePlayerToServerShutDownServer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C7020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C7170))
  if fn:
    fn.setName('HandleServerToPlayerGuiTimingEvent_Info', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C7170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BF300))
  if fn:
    fn.setName('HandleServerToPlayerGUICharacterSheet', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BF300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C7380))
  if fn:
    fn.setName('HandleServerToPlayerItemScript_Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C7380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A5BD0))
  if fn:
    fn.setName('HandleServerToPlayerSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A5BD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A7710))
  if fn:
    fn.setName('HandleServerToPlayerUpdate_Delete', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A7710: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C0B00))
  if fn:
    fn.setName('HandleServerToPlayerJournalMessage_AddQuest', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C0B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C1480))
  if fn:
    fn.setName('HandleServerToPlayerExportReply', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C1480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C6650))
  if fn:
    fn.setName('HandleServerToPlayerAmbient', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C6650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C7110))
  if fn:
    fn.setName('HandleServerToPlayerGuiTimingEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C7110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x49BFA0))
  if fn:
    fn.setName('HandleServerToPlayerCreatureUpdate_Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x49BFA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A77F0))
  if fn:
    fn.setName('HandleServerToPlayerUpdate_PlayerInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A77F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4AA0F0))
  if fn:
    fn.setName('HandleServerToPlayerUpdate_GuiElements', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4AA0F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4AA8B0))
  if fn:
    fn.setName('HandleServerToPlayerUpdate_Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4AA8B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C20F0))
  if fn:
    fn.setName('HandleServerToPlayerDebugInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C20F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x578EC0))
  if fn:
    fn.setName('Play', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x578EC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55BE40))
  if fn:
    fn.setName('GetNextSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55BE40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x53A180))
  if fn:
    fn.setName('Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGameObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGameObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x53A180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x53A2D0))
  if fn:
    fn.setName('OnHitBullet', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGameObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGameObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x53A2D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x545C00))
  if fn:
    fn.setName('OnHitFollower', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGameObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGameObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x545C00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x53A510))
  if fn:
    fn.setName('OnCreate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGameObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGameObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x53A510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x541A50))
  if fn:
    fn.setName('OnDeath', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x541A50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x543470))
  if fn:
    fn.setName('LoadGun', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x543470: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5451E0))
  if fn:
    fn.setName('LoadGun', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniEnemy', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniEnemy', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5451E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x540530))
  if fn:
    fn.setName('OnAnimKeyEvent', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x540530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x540D30))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x540D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x543090))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x543090: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x560DC0))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x560DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x53A450))
  if fn:
    fn.setName('OnFire', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGGunBank', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGGunBank', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x53A450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DCC20))
  if fn:
    fn.setName('AddGun', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGGunBank', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGGunBank', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DCC20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DD1A0))
  if fn:
    fn.setName('AddGun', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGTargettingGunBank', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGTargettingGunBank', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DD1A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5116A0))
  if fn:
    fn.setName('GetTokenValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTlkTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTlkTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5116A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5464A0))
  if fn:
    fn.setName('Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5464A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x546E40))
  if fn:
    fn.setName('JournalUpdated', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x546E40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5470E0))
  if fn:
    fn.setName('AddEntryStrRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCWorldJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCWorldJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5470E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x505D10))
  if fn:
    fn.setName('SendPlayerToServerModule_SaveGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x505D10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x505D90))
  if fn:
    fn.setName('SendPlayerToServerModule_LoadGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x505D90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x505280))
  if fn:
    fn.setName('SendPlayerToServerGuiContainer_Close', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x505280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x505210))
  if fn:
    fn.setName('SendPlayerToServerGuiInventory_Statu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x505210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x505600))
  if fn:
    fn.setName('SendPlayerToServerInput_Attack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x505600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x505300))
  if fn:
    fn.setName('SendPlayerToServerInput_UseSkill', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x505300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5053D0))
  if fn:
    fn.setName('SendPlayerToServerInput_UseItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5053D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5055A0))
  if fn:
    fn.setName('SendPlayerToServerInput_UseObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5055A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x505760))
  if fn:
    fn.setName('SendPlayerToServerInput_ChangeDoorState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x505760: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x505B80))
  if fn:
    fn.setName('SendPlayerToServerLogin_LoginLocalCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x505B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x505860))
  if fn:
    fn.setName('SendPlayerToServerLogin_LoginLocalCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x505860: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x646BF0))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResSSF,2060>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResSSF,2060>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x646BF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x495810))
  if fn:
    fn.setName('SetPortraitId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPortrait', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPortrait', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x495810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5402D0))
  if fn:
    fn.setName('SetHint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLoadScreen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLoadScreen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5402D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x53B140))
  if fn:
    fn.setName('CSWGuiMainMenu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x53B140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5CE1A0))
  if fn:
    fn.setName('SetCard', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakCard', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakCard', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5CE1A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D0D50))
  if fn:
    fn.setName('ShowHelp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTutorial', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTutorial', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D0D50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54E320))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiWagerPopup', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiWagerPopup', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54E320: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x553E00))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x553E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54D690))
  if fn:
    fn.setName('CSWGuiWagerPopup', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiWagerPopup', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiWagerPopup', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54D690: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5512F0))
  if fn:
    fn.setName('CSWGuiPazaakGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5512F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x550C50))
  if fn:
    fn.setName('HandleStartDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakStart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakStart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x550C50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5560C0))
  if fn:
    fn.setName('HandleAcceptButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakStart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakStart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5560C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x535230))
  if fn:
    fn.setName('LoadModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x535230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x535560))
  if fn:
    fn.setName('UsePlaceable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x535560: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x534B20))
  if fn:
    fn.setName('AIUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x534B20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5082D0))
  if fn:
    fn.setName('UpdateAreaTransitionDisplay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5082D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x506B20))
  if fn:
    fn.setName('AIUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x506B20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x516220))
  if fn:
    fn.setName('UpdateToolTipText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCharButton', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCharButton', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x516220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x514000))
  if fn:
    fn.setName('UpdateNameLabel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x514000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x51DAA0))
  if fn:
    fn.setName('Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x51DAA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x51E410))
  if fn:
    fn.setName('UpdateActionDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x51E410: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x51F830))
  if fn:
    fn.setName('GetActionIcon', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x51F830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x520610))
  if fn:
    fn.setName('UpdatePortraits', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x520610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x522D00))
  if fn:
    fn.setName('OnMenuButtonPressed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x522D00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x515BD0))
  if fn:
    fn.setName('DoTargetAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x515BD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5166E0))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterfaceChar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterfaceChar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5166E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x51F400))
  if fn:
    fn.setName('UpdateActionQueue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x51F400: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x522ED0))
  if fn:
    fn.setName('DoPersonalAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x522ED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x51CB70))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x51CB70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x518840))
  if fn:
    fn.setName('CSWGuiMainInterface', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x518840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x557280))
  if fn:
    fn.setName('SetAmbientDayVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x557280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x557840))
  if fn:
    fn.setName('SetAmbientNightVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x557840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x557DA0))
  if fn:
    fn.setName('UpdateTimeOfDay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x557DA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x557FE0))
  if fn:
    fn.setName('ReadInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x557FE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x557690))
  if fn:
    fn.setName('SetMusicDayTrack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x557690: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5575B0))
  if fn:
    fn.setName('SetMusicNightTrack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5575B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x557050))
  if fn:
    fn.setName('PlayBattleMusic', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x557050: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x556F50))
  if fn:
    fn.setName('SetBattleMusicTrack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x556F50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x556E50))
  if fn:
    fn.setName('SetAmbientDayTrack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x556E50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x558EA0))
  if fn:
    fn.setName('_CSWGuiInGameCredits', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameCredits', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameCredits', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x558EA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5591A0))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameCredits', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameCredits', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5591A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F90C0))
  if fn:
    fn.setName('CSWCVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F90C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x537660))
  if fn:
    fn.setName('CSWCTrigger', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x537660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5394E0))
  if fn:
    fn.setName('UpdateAreaTransitionDisplay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5394E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x537900))
  if fn:
    fn.setName('AIUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x537900: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x562720))
  if fn:
    fn.setName('ReadCExoLocStringClient', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x562720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x564050))
  if fn:
    fn.setName('SetMapNote', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x564050: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x564460))
  if fn:
    fn.setName('OnXButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x564460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5659F0))
  if fn:
    fn.setName('InitializeMapNotes', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMapHider', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMapHider', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5659F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x565F20))
  if fn:
    fn.setName('GetNextMapNote', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMapHider', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMapHider', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x565F20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x565C80))
  if fn:
    fn.setName('GetPrevMapNote', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMapHider', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMapHider', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x565C80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x567820))
  if fn:
    fn.setName('DisplayPlanet', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameGalaxyMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameGalaxyMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x567820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5641D0))
  if fn:
    fn.setName('OnMapNoteClicked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5641D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x564590))
  if fn:
    fn.setName('CSWGuiMapHider', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMapHider', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMapHider', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x564590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x564C30))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMapHider', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMapHider', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x564C30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5628C0))
  if fn:
    fn.setName('CSWGuiInGameMap', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5628C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x566F80))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameGalaxyMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameGalaxyMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x566F80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4895B0))
  if fn:
    fn.setName('LoadConjureVisual', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSpellCastVisual', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSpellCastVisual', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4895B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41BC70))
  if fn:
    fn.setName('GetGenderChar', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureAppearance', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureAppearance', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41BC70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48CC90))
  if fn:
    fn.setName('CreateBTypeBody', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureAppearance', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureAppearance', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48CC90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F4F20))
  if fn:
    fn.setName('CSWCAnimBaseWield', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F4F20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F60E0))
  if fn:
    fn.setName('ShowWieldedItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F60E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F5D80))
  if fn:
    fn.setName('HideWieldedItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F5D80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F5400))
  if fn:
    fn.setName('Wield', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F5400: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C76B0))
  if fn:
    fn.setName('CSWCAnimBase', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C76B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C7AB0))
  if fn:
    fn.setName('LoadModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C7AB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C8190))
  if fn:
    fn.setName('SetOverlayAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C8190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DA6C0))
  if fn:
    fn.setName('GetAnimationName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DA6C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4CE960))
  if fn:
    fn.setName('ApplyBumpedOutShell', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4CE960: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AD000))
  if fn:
    fn.setName('ApplyTextureReplace', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AD000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x459080))
  if fn:
    fn.setName('RemoveLightSource', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x459080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4FC9B0))
  if fn:
    fn.setName('ShowVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4FC9B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B1B10))
  if fn:
    fn.setName('UpdateSkills', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialogComputer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialogComputer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B1B10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AE430))
  if fn:
    fn.setName('CSWGuiDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AE430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B2060))
  if fn:
    fn.setName('CSWGuiDialogComputerCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialogComputerCamera', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialogComputerCamera', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B2060: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B26B0))
  if fn:
    fn.setName('SetBark', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBarkBubble', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBarkBubble', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B26B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x58FC40))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameOptions', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameOptions', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x58FC40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x602490))
  if fn:
    fn.setName('OnQuit', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameOptions', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameOptions', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x602490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x567D70))
  if fn:
    fn.setName('CSWGuiInGameOptions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameOptions', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameOptions', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x567D70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x595B20))
  if fn:
    fn.setName('SetSkill', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameSkillEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameSkillEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x595B20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x594DA0))
  if fn:
    fn.setName('OnEnterSkill', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAbilities', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAbilities', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x594DA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F73D0))
  if fn:
    fn.setName('UpdateView', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAbilities', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAbilities', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F73D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5E0CA0))
  if fn:
    fn.setName('CSWGuiInGameAbilities', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAbilities', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAbilities', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E0CA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5995D0))
  if fn:
    fn.setName('ShowLevelUpGUI', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameCharacter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameCharacter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5995D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x599720))
  if fn:
    fn.setName('StartAutoLevelup', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameCharacter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameCharacter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x599720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59C2D0))
  if fn:
    fn.setName('OnControlSelected', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59C2D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59C490))
  if fn:
    fn.setName('CantEquip', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59C490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59C520))
  if fn:
    fn.setName('NotUseable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59C520: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59C5B0))
  if fn:
    fn.setName('NotUseableOnHawk', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59C5B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59C640))
  if fn:
    fn.setName('FullHealth', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59C640: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D41C0))
  if fn:
    fn.setName('SquadFullHealth', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D41C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59CC40))
  if fn:
    fn.setName('CheckFilter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59CC40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x563E00))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x563E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59B4F0))
  if fn:
    fn.setName('CreateItemEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59B4F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A4220))
  if fn:
    fn.setName('EquipItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A4220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A4420))
  if fn:
    fn.setName('EquipItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A4420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A44E0))
  if fn:
    fn.setName('UnequipItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A44E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A47E0))
  if fn:
    fn.setName('CopyItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A47E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A48F0))
  if fn:
    fn.setName('ItemExists', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A48F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A4DF0))
  if fn:
    fn.setName('SetItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameItemEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameItemEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A4DF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x568AA0))
  if fn:
    fn.setName('CSWGuiContainer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiContainer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiContainer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x568AA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A3120))
  if fn:
    fn.setName('OnItemSelected', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A3120: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59FCC0))
  if fn:
    fn.setName('OnSelectSlot', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59FCC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B38E0))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCreateItemSubMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCreateItemSubMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B38E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B4390))
  if fn:
    fn.setName('CreateItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCreateItemSubMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCreateItemSubMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B4390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B3610))
  if fn:
    fn.setName('CSWGuiCreateItemSubMenu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCreateItemSubMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCreateItemSubMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B3610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x58B980))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPartySelection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPartySelection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x58B980: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x550430))
  if fn:
    fn.setName('OnToggled', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPartySelection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPartySelection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x550430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AD940))
  if fn:
    fn.setName('CSWGuiInGamePause', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGamePause', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGamePause', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AD940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AB3D0))
  if fn:
    fn.setName('BuyItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AB3D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AB580))
  if fn:
    fn.setName('BuyItemCallback', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AB580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AB620))
  if fn:
    fn.setName('SellItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AB620: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AA850))
  if fn:
    fn.setName('OnControlStoreAButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AA850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A9F30))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A9F30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AD630))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameSoloModeQuery', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameSoloModeQuery', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AD630: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B9820))
  if fn:
    fn.setName('OnUpgradableSelected', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgradeItemSelect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgradeItemSelect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B9820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5BD680))
  if fn:
    fn.setName('Update3DView', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgrade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5BD680: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5BDCA0))
  if fn:
    fn.setName('UpdateItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgrade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5BDCA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B93F0))
  if fn:
    fn.setName('BuildUpgradeList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgradeItemSelect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgradeItemSelect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B93F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B8C40))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgradeItemSelect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgradeItemSelect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B8C40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5BA1D0))
  if fn:
    fn.setName('CSWGuiUpgrade', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgrade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5BA1D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B84B0))
  if fn:
    fn.setName('CSWGuiUpgradeItemSelect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgradeItemSelect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgradeItemSelect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B84B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B6A10))
  if fn:
    fn.setName('CSWGuiUpgradeSelection', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgradeSelection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgradeSelection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B6A10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D3F70))
  if fn:
    fn.setName('WriteGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveLoad', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveLoad', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D3F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D43A0))
  if fn:
    fn.setName('ShowGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveLoad', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveLoad', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D43A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D58B0))
  if fn:
    fn.setName('LoadData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveLoadEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveLoadEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D58B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D6A10))
  if fn:
    fn.setName('SetXboxTitle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveLoadEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveLoadEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D6A10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D2950))
  if fn:
    fn.setName('LoadFromGFF', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('KOTOR_AUTOSAVE_PARAMS', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'KOTOR_AUTOSAVE_PARAMS', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D2950: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F59A0))
  if fn:
    fn.setName('DeleteGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveLoad', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveLoad', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F59A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D6E10))
  if fn:
    fn.setName('UnpackGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveLoad', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveLoad', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D6E10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D7A80))
  if fn:
    fn.setName('CSWGuiSaveNamePanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveNamePanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveNamePanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D7A80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D7230))
  if fn:
    fn.setName('HandleSaveButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveLoad', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveLoad', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D7230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5BE6C0))
  if fn:
    fn.setName('VerifyLoadGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveLoad', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveLoad', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5BE6C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5889D0))
  if fn:
    fn.setName('AddPowerSet', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlowChart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlowChart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5889D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x589970))
  if fn:
    fn.setName('AddFeatSet', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlowChart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlowChart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x589970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x588540))
  if fn:
    fn.setName('CreatePowerChart', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlowChart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlowChart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x588540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x588DC0))
  if fn:
    fn.setName('CreateFeatChart', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlowChart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlowChart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x588DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5890A0))
  if fn:
    fn.setName('CreateFeatChart', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlowChart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlowChart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5890A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x58A090))
  if fn:
    fn.setName('CSWGuiSkillInfoBox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillInfoBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillInfoBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x58A090: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B34C0))
  if fn:
    fn.setName('CSWGuiExamine', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiExamine', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiExamine', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B34C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B5110))
  if fn:
    fn.setName('AddPrerequisiteSpells', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersFeatsSkillsDebugMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersFeatsSkillsDebugMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B5110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B52C0))
  if fn:
    fn.setName('AddPrerequisiteFeats', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersFeatsSkillsDebugMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersFeatsSkillsDebugMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B52C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B4450))
  if fn:
    fn.setName('CSWGuiPowersFeatsSkillsDebugMenu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersFeatsSkillsDebugMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersFeatsSkillsDebugMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B4450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B6200))
  if fn:
    fn.setName('OnModulePicked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLoadModuleDebugMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLoadModuleDebugMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B6200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x573800))
  if fn:
    fn.setName('PutAtDesiredLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWBehaviorCameraCombat', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWBehaviorCameraCombat', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x573800: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x581460))
  if fn:
    fn.setName('SetProjectileSpellId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCProjectile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x581460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x581CE0))
  if fn:
    fn.setName('SetDesiredTargetObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCProjectile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x581CE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x57F670))
  if fn:
    fn.setName('SetBehaviorListSpiral', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCProjectile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x57F670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x57C930))
  if fn:
    fn.setName('SetProjectileType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCProjectile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x57C930: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x581530))
  if fn:
    fn.setName('UpdateCurrentBehavior', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCProjectile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x581530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x57A210))
  if fn:
    fn.setName('MoveProjectileToPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCProjectile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x57A210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5793A0))
  if fn:
    fn.setName('AIUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCProjectile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5793A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4756D0))
  if fn:
    fn.setName('SetEnvMap', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4756D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5851E0))
  if fn:
    fn.setName('SetEnvMap', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5851E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x56E900))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsGraphics', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsGraphics', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x56E900: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x56DAA0))
  if fn:
    fn.setName('SetUpOptions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsGraphicsAdvanced', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsGraphicsAdvanced', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x56DAA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x570F90))
  if fn:
    fn.setName('OnResolutionChosen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsResolution', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsResolution', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x570F90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5727B0))
  if fn:
    fn.setName('OnRevMouseButtons', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsMouse', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsMouse', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5727B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x56CBA0))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsGraphicsAdvanced', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsGraphicsAdvanced', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x56CBA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DD670))
  if fn:
    fn.setName('CSWGuiOptionsResolution', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsResolution', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsResolution', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DD670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x56BA80))
  if fn:
    fn.setName('CSWGuiOptionsGraphicsAdvanced', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsGraphicsAdvanced', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsGraphicsAdvanced', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x56BA80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5705C0))
  if fn:
    fn.setName('CSWGuiOptionsGraphics', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsGraphics', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsGraphics', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5705C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5CFD00))
  if fn:
    fn.setName('CSWGuiOptionsSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5CFD00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5E3F90))
  if fn:
    fn.setName('LoadInitialSideDeck', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPazaak', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPazaak', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E3F90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5E90B0))
  if fn:
    fn.setName('AbilitiesChanged', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E90B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EAB50))
  if fn:
    fn.setName('BeginPlayGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EAB50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EADA0))
  if fn:
    fn.setName('CancelLevelUp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EADA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5E75D0))
  if fn:
    fn.setName('CSWGuiLevelUpCharGen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E75D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EC270))
  if fn:
    fn.setName('CSWGuiMainCharGen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EC270: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F09E0))
  if fn:
    fn.setName('GetActionId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiKeyMapButton', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiKeyMapButton', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F09E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F15B0))
  if fn:
    fn.setName('PostChangeToEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiKeyMapButton', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiKeyMapButton', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F15B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x56B660))
  if fn:
    fn.setName('OnCancelPressed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCustomPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCustomPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x56B660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F3480))
  if fn:
    fn.setName('CSWGuiCustomPanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCustomPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCustomPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F3480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F79E0))
  if fn:
    fn.setName('OnCancelPressed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiQuickPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiQuickPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F79E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F4850))
  if fn:
    fn.setName('CSWGuiQuickPanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiQuickPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiQuickPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F4850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DFB60))
  if fn:
    fn.setName('CSWGuiQuickOrCustomPanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiQuickOrCustomPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiQuickOrCustomPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DFB60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FA720))
  if fn:
    fn.setName('OnAcceptButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FA720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F8760))
  if fn:
    fn.setName('BuildAvailableList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F8760: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F8480))
  if fn:
    fn.setName('AddChosenPower', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F8480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F7B50))
  if fn:
    fn.setName('OnRecommendButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F7B50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F7CC0))
  if fn:
    fn.setName('OnPowerPicked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F7CC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F6370))
  if fn:
    fn.setName('CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F6370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D1BF0))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D1BF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F9D30))
  if fn:
    fn.setName('InitiateFeats', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F9D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x531650))
  if fn:
    fn.setName('OnFeatPicked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x531650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F8C90))
  if fn:
    fn.setName('CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F8C90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FA2C0))
  if fn:
    fn.setName('OnAcceptButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FA2C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x58FB40))
  if fn:
    fn.setName('OnAcceptButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x58FB40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F3450))
  if fn:
    fn.setName('OnEnterPointsButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiAbilitiesCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiAbilitiesCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F3450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FE960))
  if fn:
    fn.setName('OnAcceptButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiAbilitiesCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiAbilitiesCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FE960: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FF360))
  if fn:
    fn.setName('CSWGuiAbilitiesCharGen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiAbilitiesCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiAbilitiesCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FF360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x602EF0))
  if fn:
    fn.setName('CSWGuiPortraitCharGen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPortraitCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPortraitCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x602EF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9274B0))
  if fn:
    fn.setName('floor', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9274B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90AB70))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResLIP,3004>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResLIP,3004>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90AB70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x906220))
  if fn:
    fn.setName('GetResource', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAuroraInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAuroraInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x906220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90AF70))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResPLT,6>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResPLT,6>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90AF70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x908190))
  if fn:
    fn.setName('GetModelResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAuroraInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAuroraInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x908190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x496330))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResTGA,3>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResTGA,3>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x496330: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90B3B0))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResVIS,3001>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResVIS,3001>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90B3B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90B7B0))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResTPC,3007>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResTPC,3007>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90B7B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90BD30))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResTXI,2022>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResTXI,2022>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90BD30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x639850))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResMDX,3008>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResMDX,3008>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9078D0))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResMDL,2002>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResMDL,2002>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9078D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x812FF0))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResDDS,2033>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResDDS,2033>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x812FF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x733910))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CRes4PC,2059>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CRes4PC,2059>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x733910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x561D90))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResLTR,2036>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResLTR,2036>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x561D90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x928B98))
  if fn:
    fn.setName('_stat', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x928B98: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40C7B0))
  if fn:
    fn.setName('GetCaptureEventPlayBack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C7B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x408DE0))
  if fn:
    fn.setName('DoConsoleHist', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x408DE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40C830))
  if fn:
    fn.setName('IsClientImeUI', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40C850))
  if fn:
    fn.setName('GetImeUI', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x616AE0))
  if fn:
    fn.setName('handleIme_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoImeUI', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoImeUI', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x616AE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x610D50))
  if fn:
    fn.setName('EnterCriticalSection', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoCriticalSection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoCriticalSection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x610D50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x610D70))
  if fn:
    fn.setName('LeaveCriticalSection', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoCriticalSection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoCriticalSection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x610D70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x639B30))
  if fn:
    fn.setName('CExoSoundSource', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639B30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x639C60))
  if fn:
    fn.setName('SetPriorityGroup', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639C60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x924490))
  if fn:
    fn.setName('CSWGuiLabel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLabel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLabel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x924490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61A490))
  if fn:
    fn.setName('operator__', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResRef', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61A490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61A420))
  if fn:
    fn.setName('operator__', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResRef', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61A420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AB20))
  if fn:
    fn.setName('CancelRequest', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CRes', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CRes', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AB20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61C970))
  if fn:
    fn.setName('ReleaseResObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61C970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61C4C0))
  if fn:
    fn.setName('SetResObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61C4C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AC30))
  if fn:
    fn.setName('Request', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CRes', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CRes', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AC30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x914460))
  if fn:
    fn.setName('SetImage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiImageParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiImageParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x914460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9145B0))
  if fn:
    fn.setName('SetDrawStyle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiImageParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiImageParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9145B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x914560))
  if fn:
    fn.setName('SetAlignment', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiImageParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiImageParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x914560: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x917040))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiControl', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiControl', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x917040: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91B8F0))
  if fn:
    fn.setName('SetScrollBarOnLeft', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91B8F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91C970))
  if fn:
    fn.setName('SetPadding', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91C970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x916B80))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiNavigable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiNavigable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x916B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91E120))
  if fn:
    fn.setName('LoadProtoItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91E120: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6274B0))
  if fn:
    fn.setName('CTlkFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTlkFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTlkFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6274B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x627540))
  if fn:
    fn.setName('ReadHeader', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTlkFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTlkFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x627540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8F4CB0))
  if fn:
    fn.setName('ParseField', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Material', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Material', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8F4CB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x867A20))
  if fn:
    fn.setName('AurResGet', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x867A20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6400A0))
  if fn:
    fn.setName('StartServerMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CNetLayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CNetLayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6400A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x549B40))
  if fn:
    fn.setName('SetDifficultyLevel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerInfo', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerInfo', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x549B40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61F240))
  if fn:
    fn.setName('RemoveFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61F240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x701AD0))
  if fn:
    fn.setName('SaveToGFF', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CStatusSummary', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CStatusSummary', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x701AD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75C8A0))
  if fn:
    fn.setName('SetGameObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75C8A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75A620))
  if fn:
    fn.setName('SetAreaTransitionBMP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75A620: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CDD40))
  if fn:
    fn.setName('SendServerToPlayerArea_ClientArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CDD40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734550))
  if fn:
    fn.setName('GetNormalOfPoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EEE10))
  if fn:
    fn.setName('CFactionManager', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CFactionManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CFactionManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EEE10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80AC50))
  if fn:
    fn.setName('CWorldTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80AC50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x469300))
  if fn:
    fn.setName('CGameObjectArray', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameObjectArray', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameObjectArray', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x469300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61ACC0))
  if fn:
    fn.setName('CExoResMan', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61ACC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63F840))
  if fn:
    fn.setName('CNetLayer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CNetLayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CNetLayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63F840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x548240))
  if fn:
    fn.setName('CServerInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerInfo', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerInfo', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x548240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FC520))
  if fn:
    fn.setName('CSWSMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FC520: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64CCF0))
  if fn:
    fn.setName('CServerAIMaster', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIMaster', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64CCF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64D190))
  if fn:
    fn.setName('SetExoAppInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientAIMaster', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientAIMaster', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64D190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AFF0))
  if fn:
    fn.setName('AddResourceDirectory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AFF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AFD0))
  if fn:
    fn.setName('AddFixedKeyTableFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AFD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61E890))
  if fn:
    fn.setName('CreateDirectory2', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61E890: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x627590))
  if fn:
    fn.setName('CTlkTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTlkTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTlkTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x627590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x627F70))
  if fn:
    fn.setName('OpenFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTlkTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTlkTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x627F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x640920))
  if fn:
    fn.setName('CVirtualMachine', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachine', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachine', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x640BA0))
  if fn:
    fn.setName('SetCommandImplementer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachine', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachine', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640BA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60C4A0))
  if fn:
    fn.setName('OpenLogFiles', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoDebug', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoDebug', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60C4A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BB310))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CNetLayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CNetLayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BB310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80ABE0))
  if fn:
    fn.setName('CSWSRules', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSRules', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSRules', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80ABE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D8A0))
  if fn:
    fn.setName('GetGameOver', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D8A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D500))
  if fn:
    fn.setName('GetAreaNotReadyToDisplay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D070))
  if fn:
    fn.setName('GetCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWParty', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWParty', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D070: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4789D0))
  if fn:
    fn.setName('GetServerCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4789D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x429780))
  if fn:
    fn.setName('GetDeadTemp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x429780: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BDA10))
  if fn:
    fn.setName('RunDeathSequence', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BDA10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x718400))
  if fn:
    fn.setName('GetFirstObjectInArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x718400: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A3420))
  if fn:
    fn.setName('IsPartyVisible', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A3420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x718460))
  if fn:
    fn.setName('GetNextObjectInArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x718460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62C800))
  if fn:
    fn.setName('GetNext', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLinkedListInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62C800: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x701D10))
  if fn:
    fn.setName('LoadTableInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x701D10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6535D0))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGlobalVariableTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGlobalVariableTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6535D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F3E20))
  if fn:
    fn.setName('WriteCExoLocStringServer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F3E20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54BD10))
  if fn:
    fn.setName('WriteCResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54BD10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x633C20))
  if fn:
    fn.setName('CResIFO', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResIFO', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResIFO', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x633C20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x72A590))
  if fn:
    fn.setName('GenerateInterAreaDFSSuccessors', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x72A590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x731080))
  if fn:
    fn.setName('FindTagPositionInTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x731080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B440))
  if fn:
    fn.setName('SubtractWorldTimes', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B440: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C160))
  if fn:
    fn.setName('GetActivePauseState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C160: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x731EE0))
  if fn:
    fn.setName('AddObjectToLimbo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x731EE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x730D40))
  if fn:
    fn.setName('_NWPlayerCharacterList_st', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('NWPlayerCharacterList_st', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'NWPlayerCharacterList_st', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x730D40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x477C30))
  if fn:
    fn.setName('GetCanSendDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x477C30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AD780))
  if fn:
    fn.setName('IsAnimationPlayingInDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AD780: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x46DE60))
  if fn:
    fn.setName('IsCameraAnimated', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x46DE60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AD7B0))
  if fn:
    fn.setName('IsFadingInDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AD7B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AD750))
  if fn:
    fn.setName('StopSoundPlayingInDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AD750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B3D0))
  if fn:
    fn.setName('AddWorldTimes', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B3D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B4C0))
  if fn:
    fn.setName('CompareWorldTimes', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B4C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64BD70))
  if fn:
    fn.setName('GetActiveTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BD70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734B20))
  if fn:
    fn.setName('GetInteger', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734B20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64DB90))
  if fn:
    fn.setName('AddEventDeltaTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIMaster', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64DB90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734B60))
  if fn:
    fn.setName('GetFloat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734B60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A05A0))
  if fn:
    fn.setName('ApplyPoisonDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A05A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734B80))
  if fn:
    fn.setName('SetFloat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x69FD70))
  if fn:
    fn.setName('ApplyDiseasePayload', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69FD70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734C60))
  if fn:
    fn.setName('GetExpiryTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734C60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x624CE0))
  if fn:
    fn.setName('ReadFieldSHORT', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x624CE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F89F0))
  if fn:
    fn.setName('GetEffectImmunity', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F89F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x549C20))
  if fn:
    fn.setName('CSWCCMessageData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCMessageData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCMessageData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x549C20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x549D60))
  if fn:
    fn.setName('SetObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCMessageData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCMessageData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x549D60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54A2C0))
  if fn:
    fn.setName('CopyTo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCMessageData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCMessageData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54A2C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x69D880))
  if fn:
    fn.setName('SendFeedbackMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69D880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734780))
  if fn:
    fn.setName('CGameEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734780: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734BE0))
  if fn:
    fn.setName('CopyStringFromList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734BE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x605C70))
  if fn:
    fn.setName('operator__', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x605C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734BA0))
  if fn:
    fn.setName('GetObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734BA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C1B0))
  if fn:
    fn.setName('AddToExclusionList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C1B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B750))
  if fn:
    fn.setName('GetCalendarDayFromSeconds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B7E0))
  if fn:
    fn.setName('GetTimeOfDayFromSeconds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B7E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734C40))
  if fn:
    fn.setName('SetExpiryTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734C40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x774380))
  if fn:
    fn.setName('SetPauseTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x774380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x774310))
  if fn:
    fn.setName('SetRoundPaused', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x774310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6962C0))
  if fn:
    fn.setName('AddDisappearActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6962C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x696230))
  if fn:
    fn.setName('AddAppearActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x696230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x76CE50))
  if fn:
    fn.setName('AddToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x76CE50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6E97F0))
  if fn:
    fn.setName('GetClassNegativeLevels', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6E97F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F2F30))
  if fn:
    fn.setName('SetClassNegativeLevels', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F2F30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E4990))
  if fn:
    fn.setName('GetMemberList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E4990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x698810))
  if fn:
    fn.setName('UpdatePersonalSpace', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x698810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6693B0))
  if fn:
    fn.setName('FindItemWithBaseItemType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CItemRepository', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CItemRepository', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6693B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67EB50))
  if fn:
    fn.setName('ReplaceEquipItemActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67EB50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x776210))
  if fn:
    fn.setName('AddEquipAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x776210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67FEF0))
  if fn:
    fn.setName('ReplaceUnequipItemActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67FEF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x776360))
  if fn:
    fn.setName('AddUnequipAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x776360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6685F0))
  if fn:
    fn.setName('MergeItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6685F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6686F0))
  if fn:
    fn.setName('SplitItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6686F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DC130))
  if fn:
    fn.setName('ClearVisibleList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DC130: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77A9B0))
  if fn:
    fn.setName('CScriptEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptEvent', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptEvent', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77A9B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77AB20))
  if fn:
    fn.setName('SetInteger', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCMessageData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCMessageData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77AB20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DC210))
  if fn:
    fn.setName('RemoveFromVisibleList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DC210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67D0E0))
  if fn:
    fn.setName('AddMoveToPointActionToFront', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67D0E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x69F2C0))
  if fn:
    fn.setName('ClearHostileActionsVersus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69F2C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A4580))
  if fn:
    fn.setName('RemovePureGoodPowers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A4580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A4620))
  if fn:
    fn.setName('RemovePureEvilPowers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A4620: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6694E0))
  if fn:
    fn.setName('FindItemWithTag', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CItemRepository', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CItemRepository', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6694E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6695B0))
  if fn:
    fn.setName('GetItemInRepository', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CItemRepository', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CItemRepository', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6695B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6696B0))
  if fn:
    fn.setName('RemoveItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CItemRepository', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CItemRepository', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6696B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x633B40))
  if fn:
    fn.setName('CResARE', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResARE', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResARE', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x633B40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x721010))
  if fn:
    fn.setName('ApplyEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x721010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47B350))
  if fn:
    fn.setName('RecaulateFollowPoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWParty', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWParty', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47B350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47B610))
  if fn:
    fn.setName('GetPathData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyFollowDataArray', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyFollowDataArray', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47B610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80BA70))
  if fn:
    fn.setName('DegToRad', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80BA70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DFD40))
  if fn:
    fn.setName('GetAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DFD40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7059F0))
  if fn:
    fn.setName('TestSafeLocationPoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7059F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DF1B0))
  if fn:
    fn.setName('RemoveFromInvitationsOffered', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DF1B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A29C0))
  if fn:
    fn.setName('SetInvitedToParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A29C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54AF10))
  if fn:
    fn.setName('ReadWORD', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54AF10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AD3F0))
  if fn:
    fn.setName('ReplyDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AD3F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7599F0))
  if fn:
    fn.setName('SetOpen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayerInventoryGUI', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayerInventoryGUI', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7599F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x669C90))
  if fn:
    fn.setName('CloseInventory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x669C90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6C28A0))
  if fn:
    fn.setName('SetCombatMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6C28A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x775CA0))
  if fn:
    fn.setName('ClearAllSpecialAttacks', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x775CA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67C8C0))
  if fn:
    fn.setName('ActionManager', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67C8C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67D240))
  if fn:
    fn.setName('AddDriveAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67D240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E4720))
  if fn:
    fn.setName('CSWSFaction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E4720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EF590))
  if fn:
    fn.setName('ResolveFactionName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CFactionManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CFactionManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EF590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EF210))
  if fn:
    fn.setName('LoadFactions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CFactionManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CFactionManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EF210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x735800))
  if fn:
    fn.setName('SaveGameEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x735800: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A4FC0))
  if fn:
    fn.setName('SaveLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptLocation', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptLocation', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A4FC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A4EB0))
  if fn:
    fn.setName('LoadLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptLocation', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptLocation', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A4EB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A51B0))
  if fn:
    fn.setName('CScriptTalent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptTalent', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptTalent', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A51B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6965C0))
  if fn:
    fn.setName('AddUseTalentAtLocationActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6965C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FE8F0))
  if fn:
    fn.setName('AddMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FE8F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x421750))
  if fn:
    fn.setName('SetInParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x421750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DBA10))
  if fn:
    fn.setName('AdjustReputation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DBA10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F51C0))
  if fn:
    fn.setName('SetAmbientDayVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F51C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F52A0))
  if fn:
    fn.setName('SetAmbientNightVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F52A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6810B0))
  if fn:
    fn.setName('AddCutsceneMoveActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6810B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AE5A0))
  if fn:
    fn.setName('AddCloseDoorAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AE5A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A5080))
  if fn:
    fn.setName('GetIsBeam', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CLoopingVisualEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CLoopingVisualEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A5080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734A90))
  if fn:
    fn.setName('SetNumIntegers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734A90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734F20))
  if fn:
    fn.setName('SetLinked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734F20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734F50))
  if fn:
    fn.setName('UpdateLinked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734F50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F1E10))
  if fn:
    fn.setName('GetSTRStat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F1E10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F2180))
  if fn:
    fn.setName('GetDEXStat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F2180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F1EC0))
  if fn:
    fn.setName('GetCONStat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F1EC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F1F70))
  if fn:
    fn.setName('GetINTStat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F1F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F2020))
  if fn:
    fn.setName('GetWISStat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F2020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F20D0))
  if fn:
    fn.setName('GetCHAStat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F20D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F2230))
  if fn:
    fn.setName('GetDEXMod', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F2230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F3E80))
  if fn:
    fn.setName('GetArmorClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F3E80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F4CC0))
  if fn:
    fn.setName('GetCreatureHasTalent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F4CC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AA030))
  if fn:
    fn.setName('GetLastDamageAmountByFlags', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AA030: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x735C20))
  if fn:
    fn.setName('GetEffectTypeScriptConstant', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x735C20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E57E0))
  if fn:
    fn.setName('GetAverageGoodEvilAlignment', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E57E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E54A0))
  if fn:
    fn.setName('GetAverageLevel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E54A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E5510))
  if fn:
    fn.setName('GetAverageReputation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E5510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E55C0))
  if fn:
    fn.setName('GetAverageXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E55C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E5100))
  if fn:
    fn.setName('GetBestAC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E5100: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E5740))
  if fn:
    fn.setName('GetGold', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E5740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E5BF0))
  if fn:
    fn.setName('GetMostDamagedMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E5BF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E5880))
  if fn:
    fn.setName('GetMostFrequentClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E5880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E5A80))
  if fn:
    fn.setName('GetWorstAC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E5A80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F6AA0))
  if fn:
    fn.setName('GetFortSavingThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F6AA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B0BB0))
  if fn:
    fn.setName('GetHasFeatEffectApplied', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B0BB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AE3A0))
  if fn:
    fn.setName('GetCanUseSkill', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AE3A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x697060))
  if fn:
    fn.setName('HasSpellEffectApplied', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x697060: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x68EF10))
  if fn:
    fn.setName('GetIsWeaponEffective', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x68EF10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4EAFD0))
  if fn:
    fn.setName('GetPassiveProperty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4EAFD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x667A90))
  if fn:
    fn.setName('GetActiveProperty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x667A90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74CC50))
  if fn:
    fn.setName('GetLocalNumber', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVarTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVarTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74CC50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74CD50))
  if fn:
    fn.setName('GetLocalBoolean', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVarTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVarTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74CD50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AE2E0))
  if fn:
    fn.setName('GetScriptLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AE2E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F6BC0))
  if fn:
    fn.setName('GetReflexSavingThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F6BC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A2CA0))
  if fn:
    fn.setName('CalculateSpellSaveDC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A2CA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x72A670))
  if fn:
    fn.setName('GetWaypoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x72A670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F6B30))
  if fn:
    fn.setName('GetWillSavingThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F6B30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x699EF0))
  if fn:
    fn.setName('AddGold', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x699EF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x605810))
  if fn:
    fn.setName('CExoString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x605810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7004D0))
  if fn:
    fn.setName('GetNPCSelectability', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7004D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x700CB0))
  if fn:
    fn.setName('GetIsNPCAvailable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x700CB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x700490))
  if fn:
    fn.setName('GetIsMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x700490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AE620))
  if fn:
    fn.setName('AddOpenDoorAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AE620: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x704710))
  if fn:
    fn.setName('RemoveNPC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x704710: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FE670))
  if fn:
    fn.setName('RemoveMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FE670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AA080))
  if fn:
    fn.setName('DoSpellLevelAbsorption', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AA080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AA7C0))
  if fn:
    fn.setName('DoSpellImmunity', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AA7C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F7190))
  if fn:
    fn.setName('GetForceResistance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F7190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x50E640))
  if fn:
    fn.setName('RollDice', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRules', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRules', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x50E640: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x549CB0))
  if fn:
    fn.setName('_CSWCCMessageData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCMessageData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCMessageData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x549CB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x66A530))
  if fn:
    fn.setName('SetIdentified', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x66A530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x429A90))
  if fn:
    fn.setName('SetOverrideLightsaberState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x429A90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74CCA0))
  if fn:
    fn.setName('SetLocalNumber', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVarTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVarTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74CCA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74CD80))
  if fn:
    fn.setName('SetLocalBoolean', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVarTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVarTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74CD80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x723250))
  if fn:
    fn.setName('SetPlayerRestrictMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x723250: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B8C0))
  if fn:
    fn.setName('AdvanceToTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B8C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x703E60))
  if fn:
    fn.setName('SwitchPlayerCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x703E60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x699FD0))
  if fn:
    fn.setName('RemoveGold', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x699FD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F20C0))
  if fn:
    fn.setName('_CSWSExpression', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSExpression', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSExpression', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F20C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F8E20))
  if fn:
    fn.setName('GetEffectLinkImmunity', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F8E20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x703850))
  if fn:
    fn.setName('GivePlotXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x703850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x662D40))
  if fn:
    fn.setName('GetNextAttacker', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x662D40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x681000))
  if fn:
    fn.setName('SetFormation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x681000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x656D70))
  if fn:
    fn.setName('IsDebilitated', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x656D70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C6A0))
  if fn:
    fn.setName('GetFactionManager', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C6A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x69F230))
  if fn:
    fn.setName('CancelCombat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69F230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7046B0))
  if fn:
    fn.setName('GetPlanetAvailable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7046B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x722F20))
  if fn:
    fn.setName('SetMaxStealthXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x722F20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x722EE0))
  if fn:
    fn.setName('SetCurrentStealthXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x722EE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DFCD0))
  if fn:
    fn.setName('SetLockObjectOrientationInDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DFCD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DFE80))
  if fn:
    fn.setName('SetLockHeadFollowInDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DFE80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x778160))
  if fn:
    fn.setName('SetWorldPointExplored', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x778160: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x778820))
  if fn:
    fn.setName('SetEntireMapExplored', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x778820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x723140))
  if fn:
    fn.setName('SetFogColor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x723140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x722C70))
  if fn:
    fn.setName('GetClientArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x722C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60B820))
  if fn:
    fn.setName('GetAliasPath', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoAliasList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoAliasList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60B820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B12F0))
  if fn:
    fn.setName('SetCurrentHitPoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B12F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7005F0))
  if fn:
    fn.setName('SetNPCObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7005F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A4CC0))
  if fn:
    fn.setName('CExoArrayList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CExoString*>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CExoString*>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A4CC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40DDB0))
  if fn:
    fn.setName('GetGUIString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40DDB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x667AD0))
  if fn:
    fn.setName('GetExistsHigherValuedProperty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x667AD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6790E0))
  if fn:
    fn.setName('GetPropertyDamageRange', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6790E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AB90))
  if fn:
    fn.setName('Dump', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AB90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FEC70))
  if fn:
    fn.setName('GetFilename', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FEC70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64BD40))
  if fn:
    fn.setName('GetServerMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BD40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54BA10))
  if fn:
    fn.setName('WriteWORD', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54BA10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75C290))
  if fn:
    fn.setName('PackCreatureIntoMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75C290: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54BC10))
  if fn:
    fn.setName('WriteVector', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54BC10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64BF80))
  if fn:
    fn.setName('GetPlayerListString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55A8F0))
  if fn:
    fn.setName('RemoveStaticBoundingBox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55A8F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EE410))
  if fn:
    fn.setName('SpawnIfAppropriate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EE410: '+str(e)); errors += 1
print('Batch 2 done: renamed='+str(renamed)+' errors='+str(errors))