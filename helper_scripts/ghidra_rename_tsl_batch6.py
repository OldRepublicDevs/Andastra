from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0
try:
  fn = fm.getFunctionAt(toAddr(0x7F1520))
  if fn:
    fn.setName('ParseString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSExpression', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSExpression', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F1520: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7E5DC0))
  if fn:
    fn.setName('GetLeader', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E5DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E4E40))
  if fn:
    fn.setName('GetLeastDamagedMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E4E40: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7E5F30))
  if fn:
    fn.setName('GetSingletonParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E5F30: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7E49B0))
  if fn:
    fn.setName('InviteMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E49B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7CA080))
  if fn:
    fn.setName('SaveForcedAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSForcedAction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSForcedAction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CA080: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x671900))
  if fn:
    fn.setName('AddAttackBonusStrings', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x671900: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x66EEE0))
  if fn:
    fn.setName('AddDamageProperties', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x66EEE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x673710))
  if fn:
    fn.setName('AddDefenceProperties', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x673710: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6747F0))
  if fn:
    fn.setName('AddMiscellaneousProperties', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6747F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6646E0))
  if fn:
    fn.setName('AddToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6646E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x673600))
  if fn:
    fn.setName('AddWeaponSizeProperties', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x673600: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x667F40))
  if fn:
    fn.setName('CompareItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x667F40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x667EF0))
  if fn:
    fn.setName('ComputeArmorClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x667EF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x66A400))
  if fn:
    fn.setName('GetDamageFlags', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x66A400: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x667B10))
  if fn:
    fn.setName('GetPropertyByType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x667B10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x667790))
  if fn:
    fn.setName('GetPropertyByTypeExists', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x667790: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x664F70))
  if fn:
    fn.setName('LoadDataFromGff', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x664F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x664E40))
  if fn:
    fn.setName('LoadFromTemplate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x664E40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x666410))
  if fn:
    fn.setName('LoadItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x666410: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x667020))
  if fn:
    fn.setName('RemoveFromArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x667020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x666600))
  if fn:
    fn.setName('SaveItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x666600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x666BC0))
  if fn:
    fn.setName('SaveItemProperties', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x666BC0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7C9A60))
  if fn:
    fn.setName('ApplyBlasterBoltDeflectionDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C9A60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C4DC0))
  if fn:
    fn.setName('ApplyBlasterBoltDeflectionIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C4DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C6C70))
  if fn:
    fn.setName('ApplyBonusFeat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C6C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A1490))
  if fn:
    fn.setName('ApplyChangedSavingThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A1490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C4F00))
  if fn:
    fn.setName('ApplyDamageBonus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C4F00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C95D0))
  if fn:
    fn.setName('ApplyDisguise', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C95D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65C950))
  if fn:
    fn.setName('ApplyFreedomofMovement', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65C950: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C8730))
  if fn:
    fn.setName('ApplyImmunity', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C8730: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C9840))
  if fn:
    fn.setName('ApplyLight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C9840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74ACC0))
  if fn:
    fn.setName('ApplyRegeneration', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74ACC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C9340))
  if fn:
    fn.setName('ApplySkillBonus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C9340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C5940))
  if fn:
    fn.setName('ApplySpecialWalk', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C5940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C65C0))
  if fn:
    fn.setName('ApplyTrueSeeing', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C65C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7370F0))
  if fn:
    fn.setName('RemoveEnhancementBonus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7370F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7F3BF0))
  if fn:
    fn.setName('AddItemAppearanceToMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F3BF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CAE60))
  if fn:
    fn.setName('AssignCreatureLists', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CAE60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CAAA0))
  if fn:
    fn.setName('AssignVisualEffectLists', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CAAA0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7D2900))
  if fn:
    fn.setName('ComputeGameObjectUpdateForObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D2900: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CD560))
  if fn:
    fn.setName('ComputeLastUpdate_AssociateState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CD560: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7D4C80))
  if fn:
    fn.setName('DeleteLastUpdateObjectsForObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D4C80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x805C90))
  if fn:
    fn.setName('HandlePlayerToServerCharListMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x805C90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FFE20))
  if fn:
    fn.setName('HandlePlayerToServerDialogMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FFE20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x804410))
  if fn:
    fn.setName('HandlePlayerToServerGoldMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x804410: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x800D70))
  if fn:
    fn.setName('HandlePlayerToServerGuiContainerMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x800D70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x800D00))
  if fn:
    fn.setName('HandlePlayerToServerGuiInventoryMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x800D00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x801170))
  if fn:
    fn.setName('HandlePlayerToServerInputAbortDriveControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x801170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x801010))
  if fn:
    fn.setName('HandlePlayerToServerInputDriveControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x801010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x801290))
  if fn:
    fn.setName('HandlePlayerToServerInputWalkToWaypoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x801290: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8079D0))
  if fn:
    fn.setName('HandlePlayerToServerLevelUpMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8079D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8053D0))
  if fn:
    fn.setName('HandlePlayerToServerLoginMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8053D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x808670))
  if fn:
    fn.setName('HandlePlayerToServerMapPinDestroyMapPin', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x808670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x808100))
  if fn:
    fn.setName('HandlePlayerToServerMapPinSetMapPinAt', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x808100: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x805DE0))
  if fn:
    fn.setName('HandlePlayerToServerModuleMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x805DE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x808BC0))
  if fn:
    fn.setName('HandlePlayerToServerPlayModuleCharacterList_Start', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x808BC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x808970))
  if fn:
    fn.setName('HandlePlayerToServerPlayerDeath', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x808970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FC590))
  if fn:
    fn.setName('HandlePlayerToServerQuickChatMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FC590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C6700))
  if fn:
    fn.setName('HandlePlayerToServerShutDownServer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C6700: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x64BD00))
  if fn:
    fn.setName('SendServerToPlayerCCMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BD00: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7DD510))
  if fn:
    fn.setName('SendServerToPlayerInventory_EquipCancel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DD510: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7CD990))
  if fn:
    fn.setName('SendServerToPlayerLoadBar_UpdateStallEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CD990: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7CDB80))
  if fn:
    fn.setName('SendServerToPlayerModule_ExportReply', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CDB80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7CDA70))
  if fn:
    fn.setName('SendServerToPlayerModule_SetPauseState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CDA70: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7D1380))
  if fn:
    fn.setName('SendServerToPlayerQuickChatMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D1380: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7D0660))
  if fn:
    fn.setName('SendServerToPlayerSetCustomTokenList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D0660: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7DBB80))
  if fn:
    fn.setName('SendServerToPlayerSoundObject_ChangePosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DBB80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x80A020))
  if fn:
    fn.setName('SendServerToServerAdminMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80A020: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7D2C50))
  if fn:
    fn.setName('SortObjectsForGameObjectUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D2C50: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7D36A0))
  if fn:
    fn.setName('StoreValuesInLastUpdateObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D36A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7D36E0))
  if fn:
    fn.setName('TestObjectUpdateDifferences', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D36E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D4450))
  if fn:
    fn.setName('TestObjectVisible', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D4450: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7DE450))
  if fn:
    fn.setName('UpdateLastUpdateObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DE450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DF290))
  if fn:
    fn.setName('UpdateLastUpdateObjectAppearance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DF290: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7D97C0))
  if fn:
    fn.setName('WriteGameObjUpdate_UpdateObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D97C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x729A10))
  if fn:
    fn.setName('AIUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x729A10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x732120))
  if fn:
    fn.setName('AddModuleResources', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x732120: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6B5DE0))
  if fn:
    fn.setName('AddWorldJournalEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B5DE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x731700))
  if fn:
    fn.setName('AddWorldJournalEntryStrref', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x731700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F7500))
  if fn:
    fn.setName('DeleteWorldJournalEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F7500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x731A80))
  if fn:
    fn.setName('DeleteWorldJournalEntryStrref', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x731A80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x729B90))
  if fn:
    fn.setName('DoUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x729B90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x729BF0))
  if fn:
    fn.setName('EventHandler', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x729BF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x731340))
  if fn:
    fn.setName('FindObjectByTagOrdinal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x731340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x731420))
  if fn:
    fn.setName('FindObjectByTagTypeOrdinal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x731420: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x730950))
  if fn:
    fn.setName('GetPlayerIndexInPlayerList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x730950: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x72A7A0))
  if fn:
    fn.setName('InterAreaDFS', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x72A7A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7306A0))
  if fn:
    fn.setName('LoadLimboCreatures', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7306A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x72CF90))
  if fn:
    fn.setName('PackModuleIntoMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x72CF90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x730B00))
  if fn:
    fn.setName('PackPlayerCharacterListIntoMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x730B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x731230))
  if fn:
    fn.setName('RemoveObjectFromLookupTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x731230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7305A0))
  if fn:
    fn.setName('SaveLimboCreatures', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7305A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x416CF0))
  if fn:
    fn.setName('SaveModuleFAC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x416CF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x72F6B0))
  if fn:
    fn.setName('SaveModuleInProgress', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x72F6B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x72FFA0))
  if fn:
    fn.setName('SavePrimaryPlayerInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x72FFA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7302B0))
  if fn:
    fn.setName('SaveStatic', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7302B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x732020))
  if fn:
    fn.setName('TimeStopSanityCheck', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x732020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x731BB0))
  if fn:
    fn.setName('UpdateTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x731BB0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6BD530))
  if fn:
    fn.setName('AddDoCommandAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6BD530: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6A7CB0))
  if fn:
    fn.setName('AnimationStationary', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A7CB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B15C0))
  if fn:
    fn.setName('AttachmentFromClientObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B15C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6ADDE0))
  if fn:
    fn.setName('BroadcastDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6ADDE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AEBA0))
  if fn:
    fn.setName('BroadcastSafeProjectile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AEBA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x801C10))
  if fn:
    fn.setName('BroadcastSpellData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x801C10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A7D60))
  if fn:
    fn.setName('ClearAllActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A7D60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6ADD20))
  if fn:
    fn.setName('ClearMatchedExpressionStrings', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6ADD20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A9AE0))
  if fn:
    fn.setName('DoDamageImmunity', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A9AE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A8110))
  if fn:
    fn.setName('DoDamageReduction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A8110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A8CB0))
  if fn:
    fn.setName('DoDamageResistance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A8CB0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6BD870))
  if fn:
    fn.setName('GetAIStateReputation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6BD870: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AAAC0))
  if fn:
    fn.setName('GetDamageLevel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AAAC0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6ADAB0))
  if fn:
    fn.setName('GetListenExpressionObj', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6ADAB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AD8B0))
  if fn:
    fn.setName('GetNearestObjectByName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AD8B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A6240))
  if fn:
    fn.setName('GetNodeById', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A6240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6BD5B0))
  if fn:
    fn.setName('GetReputation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6BD5B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6AC200))
  if fn:
    fn.setName('IsDialogDelay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AC200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A6A30))
  if fn:
    fn.setName('LoadActionQueue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A6A30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A73C0))
  if fn:
    fn.setName('LoadEffectList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A73C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A4E30))
  if fn:
    fn.setName('RemoveEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A4E30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AB470))
  if fn:
    fn.setName('RemoveEffectById', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AB470: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x611D10))
  if fn:
    fn.setName('RemoveGroup', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x611D10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AD7E0))
  if fn:
    fn.setName('RemoveObjectFromDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AD7E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6B0C50))
  if fn:
    fn.setName('ReportOverflow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B0C50: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6ACDD0))
  if fn:
    fn.setName('RunDialogOneLiner', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6ACDD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A70F0))
  if fn:
    fn.setName('SaveActionQueue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A70F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A7540))
  if fn:
    fn.setName('SaveEffectList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A7540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AD220))
  if fn:
    fn.setName('SendDialogEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AD220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AD330))
  if fn:
    fn.setName('SendDialogReplies', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AD330: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AC100))
  if fn:
    fn.setName('SetArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AC100: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6A9ED0))
  if fn:
    fn.setName('SetDamageImmunity', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A9ED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B1480))
  if fn:
    fn.setName('SetDeleteNoFade', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B1480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6ADA90))
  if fn:
    fn.setName('SetDialogOwner', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6ADA90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B1340))
  if fn:
    fn.setName('SetFadeOutTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B1340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A6880))
  if fn:
    fn.setName('SetGroupInterruptable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A6880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B13E0))
  if fn:
    fn.setName('SetKeepCorpse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B13E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B0EC0))
  if fn:
    fn.setName('SetLastHostileActor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B0EC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6ADB20))
  if fn:
    fn.setName('SetListenExpression', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6ADB20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AABF0))
  if fn:
    fn.setName('SetOrientation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AABF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A7820))
  if fn:
    fn.setName('SetPortraitId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A7820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AC440))
  if fn:
    fn.setName('StartDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AC440: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AD5E0))
  if fn:
    fn.setName('StopDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AD5E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AD490))
  if fn:
    fn.setName('UpdateDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AD490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AB7B0))
  if fn:
    fn.setName('UpdateEffectList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AB7B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A5D30))
  if fn:
    fn.setName('_CSWSObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A5D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C0300))
  if fn:
    fn.setName('_CSWSObjectActionNode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObjectActionNode', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObjectActionNode', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C0300: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x770DD0))
  if fn:
    fn.setName('OnEndDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x770DD0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7294D0))
  if fn:
    fn.setName('ClearPlayerOnDestroyGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7294D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7F2C00))
  if fn:
    fn.setName('ClearRepository', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayerLUOInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayerLUOInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F2C00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F2CF0))
  if fn:
    fn.setName('ClearSlots', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayerLUOInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayerLUOInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F2CF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734500))
  if fn:
    fn.setName('ComputeHeight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734360))
  if fn:
    fn.setName('ComputeMeshNormals', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734360: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x734050))
  if fn:
    fn.setName('LoadWalkMesh', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734050: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734390))
  if fn:
    fn.setName('SetRoomAdjacency', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734280))
  if fn:
    fn.setName('TransformToWorld', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7342E0))
  if fn:
    fn.setName('WeldRoom', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7342E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7F7690))
  if fn:
    fn.setName('ChangeFixedVariance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F7690: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F75F0))
  if fn:
    fn.setName('ChangePitchVariation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F75F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7F76F0))
  if fn:
    fn.setName('ChangeVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F76F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7F7410))
  if fn:
    fn.setName('Stop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F7410: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x750110))
  if fn:
    fn.setName('LoadData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSpellScriptData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSpellScriptData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x750110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x69E4D0))
  if fn:
    fn.setName('SaveData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSpellScriptData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSpellScriptData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69E4D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7C1080))
  if fn:
    fn.setName('AddToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C1080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C0320))
  if fn:
    fn.setName('CSWSStore', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C0320: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C05E0))
  if fn:
    fn.setName('LoadFromTemplate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C05E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C0780))
  if fn:
    fn.setName('LoadStore', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C0780: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x75D4F0))
  if fn:
    fn.setName('AddToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75D4F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75CF80))
  if fn:
    fn.setName('CSWSTrigger', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75CF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75E200))
  if fn:
    fn.setName('ComputeBoundingBox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75E200: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x75F3B0))
  if fn:
    fn.setName('LoadFromTemplate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75F3B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75F4E0))
  if fn:
    fn.setName('LoadTrigger', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75F4E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x760B90))
  if fn:
    fn.setName('LoadTriggerGeometry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x760B90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x761900))
  if fn:
    fn.setName('RemoveFromArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x761900: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F5920))
  if fn:
    fn.setName('AddToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSWaypoint', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSWaypoint', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F5920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F5750))
  if fn:
    fn.setName('CSWSWaypoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSWaypoint', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSWaypoint', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F5750: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7F5A30))
  if fn:
    fn.setName('LoadWaypoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSWaypoint', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSWaypoint', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F5A30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F5FB0))
  if fn:
    fn.setName('RemoveFromArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSWaypoint', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSWaypoint', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F5FB0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x443100))
  if fn:
    fn.setName('Fetch', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTlkTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTlkTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x443100: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x542360))
  if fn:
    fn.setName('AdjustHitPoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x542360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x541370))
  if fn:
    fn.setName('GetGunBank', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x541370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x53A280))
  if fn:
    fn.setName('GetLastObstacleHit', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x53A280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x542A70))
  if fn:
    fn.setName('GetSoundFrequency', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x542A70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x542A10))
  if fn:
    fn.setName('GetSoundFrequencyIsRandom', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x542A10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5429B0))
  if fn:
    fn.setName('GetSoundVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5429B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x541190))
  if fn:
    fn.setName('LoadSounds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x541190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x542400))
  if fn:
    fn.setName('OnDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x542400: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5420E0))
  if fn:
    fn.setName('OnHitObstacle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5420E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x542560))
  if fn:
    fn.setName('SetHitPoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x542560: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x541FF0))
  if fn:
    fn.setName('SetInvincibility', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x541FF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5423D0))
  if fn:
    fn.setName('SetMaxHitPoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5423D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x542AA0))
  if fn:
    fn.setName('SetSoundFrequency', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x542AA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x542A40))
  if fn:
    fn.setName('SetSoundFrequencyIsRandom', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x542A40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5426C0))
  if fn:
    fn.setName('SetSoundName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5426C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5429E0))
  if fn:
    fn.setName('SetSoundVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5429E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x541740))
  if fn:
    fn.setName('SetSpeed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x541740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x542390))
  if fn:
    fn.setName('SetSphereRadius', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x542390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A72F0))
  if fn:
    fn.setName('CSWUpgradeItemEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWUpgradeItemEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWUpgradeItemEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A72F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x77E710))
  if fn:
    fn.setName('CopyGameDefinedStructure', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77E710: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77ED70))
  if fn:
    fn.setName('CreateGameDefinedStructure', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77ED70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x846350))
  if fn:
    fn.setName('DestroyGameDefinedStructure', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x846350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77EEF0))
  if fn:
    fn.setName('ExecuteCommandActionBarkString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77EEF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A2D30))
  if fn:
    fn.setName('ExecuteCommandActionCloseDoor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A2D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x799310))
  if fn:
    fn.setName('ExecuteCommandActionEquipItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x799310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77F0B0))
  if fn:
    fn.setName('ExecuteCommandActionEquipMostDamagingRanged', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77F0B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77F170))
  if fn:
    fn.setName('ExecuteCommandActionExchangeItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77F170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8064B0))
  if fn:
    fn.setName('ExecuteCommandActionFollowLeader', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8064B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77F260))
  if fn:
    fn.setName('ExecuteCommandActionFollowObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77F260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x762FF0))
  if fn:
    fn.setName('ExecuteCommandActionInteractObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x762FF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77F6D0))
  if fn:
    fn.setName('ExecuteCommandActionJumpToObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77F6D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x449E40))
  if fn:
    fn.setName('ExecuteCommandActionLockActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x449E40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77FB70))
  if fn:
    fn.setName('ExecuteCommandActionMoveAwayFromLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77FB70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A2000))
  if fn:
    fn.setName('ExecuteCommandActionMoveAwayFromObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A2000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x784990))
  if fn:
    fn.setName('ExecuteCommandActionOpenDoor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x784990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x414430))
  if fn:
    fn.setName('ExecuteCommandActionPauseConversation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x414430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A30E0))
  if fn:
    fn.setName('ExecuteCommandActionPickUpItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A30E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A4BB0))
  if fn:
    fn.setName('ExecuteCommandActionPutDownItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A4BB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x780100))
  if fn:
    fn.setName('ExecuteCommandActionRandomWalk', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x780100: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x780240))
  if fn:
    fn.setName('ExecuteCommandActionResumeConversation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x780240: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7802F0))
  if fn:
    fn.setName('ExecuteCommandActionStartConversation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7802F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x781230))
  if fn:
    fn.setName('ExecuteCommandActionUseFeat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x781230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x794130))
  if fn:
    fn.setName('ExecuteCommandActionUseSkill', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x794130: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7813A0))
  if fn:
    fn.setName('ExecuteCommandActionUseTalent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7813A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x79EF30))
  if fn:
    fn.setName('ExecuteCommandAddAvailableNPCByObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79EF30: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x781A10))
  if fn:
    fn.setName('ExecuteCommandAddJournalWorldEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x781A10: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x781D80))
  if fn:
    fn.setName('ExecuteCommandAddMultiClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x781D80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x781F70))
  if fn:
    fn.setName('ExecuteCommandAddPartyMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x781F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x782060))
  if fn:
    fn.setName('ExecuteCommandAddToParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x782060: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x782130))
  if fn:
    fn.setName('ExecuteCommandAdjustAlignment', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x782130: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x782790))
  if fn:
    fn.setName('ExecuteCommandAdjustReputation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x782790: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x782890))
  if fn:
    fn.setName('ExecuteCommandAmbientSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x782890: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x782AB0))
  if fn:
    fn.setName('ExecuteCommandApplyEffectAtLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x782AB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x782D60))
  if fn:
    fn.setName('ExecuteCommandApplyEffectToObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x782D60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x782FD0))
  if fn:
    fn.setName('ExecuteCommandAssignCommand', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x782FD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60C840))
  if fn:
    fn.setName('ExecuteCommandAurPostString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60C840: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x783550))
  if fn:
    fn.setName('ExecuteCommandBeginConversation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x783550: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x784910))
  if fn:
    fn.setName('ExecuteCommandClearAllActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x784910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x446AE0))
  if fn:
    fn.setName('ExecuteCommandClearAllEffects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x446AE0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x784A60))
  if fn:
    fn.setName('ExecuteCommandCreateItemOnObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x784A60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x781580))
  if fn:
    fn.setName('ExecuteCommandCutsceneAttack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x781580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x783260))
  if fn:
    fn.setName('ExecuteCommandCutsceneMove', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x783260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B7AE0))
  if fn:
    fn.setName('ExecuteCommandDelayCommand', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B7AE0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7B4240))
  if fn:
    fn.setName('ExecuteCommandDeleteJournalWorldEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B4240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6134E0))
  if fn:
    fn.setName('ExecuteCommandDeleteJournalWorldEntryStrref', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6134E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x785D40))
  if fn:
    fn.setName('ExecuteCommandDestroyObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x785D40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x787F10))
  if fn:
    fn.setName('ExecuteCommandDisplayFeedBackText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x787F10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AC150))
  if fn:
    fn.setName('ExecuteCommandDistanceConversions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AC150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7862F0))
  if fn:
    fn.setName('ExecuteCommandDoDoorAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7862F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x687B00))
  if fn:
    fn.setName('ExecuteCommandDuplicateHeadAppearance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x687B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78B5A0))
  if fn:
    fn.setName('ExecuteCommandEffectACDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78B5A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78B7B0))
  if fn:
    fn.setName('ExecuteCommandEffectACIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78B7B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x786E00))
  if fn:
    fn.setName('ExecuteCommandEffectAbilityDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x786E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x786C40))
  if fn:
    fn.setName('ExecuteCommandEffectAbilityIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x786C40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x787400))
  if fn:
    fn.setName('ExecuteCommandEffectAreaEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x787400: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x789200))
  if fn:
    fn.setName('ExecuteCommandEffectAttackDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x789200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x787700))
  if fn:
    fn.setName('ExecuteCommandEffectAttackIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x787700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x787AC0))
  if fn:
    fn.setName('ExecuteCommandEffectBeam', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x787AC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78D6B0))
  if fn:
    fn.setName('ExecuteCommandEffectBlasterDeflectionDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78D6B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78D590))
  if fn:
    fn.setName('ExecuteCommandEffectBlasterDeflectionIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78D590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78D9C0))
  if fn:
    fn.setName('ExecuteCommandEffectBodyFuel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78D9C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78D7D0))
  if fn:
    fn.setName('ExecuteCommandEffectChoke', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78D7D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C1EE0))
  if fn:
    fn.setName('ExecuteCommandEffectComplex', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C1EE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78A840))
  if fn:
    fn.setName('ExecuteCommandEffectConcealment', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78A840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7889D0))
  if fn:
    fn.setName('ExecuteCommandEffectDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7889D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7878E0))
  if fn:
    fn.setName('ExecuteCommandEffectDamageDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7878E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78D130))
  if fn:
    fn.setName('ExecuteCommandEffectDamageForcePoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78D130: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x789C80))
  if fn:
    fn.setName('ExecuteCommandEffectDamageImmunityDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x789C80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78A500))
  if fn:
    fn.setName('ExecuteCommandEffectDamageImmunityIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78A500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x788C70))
  if fn:
    fn.setName('ExecuteCommandEffectDamageIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x788C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x789600))
  if fn:
    fn.setName('ExecuteCommandEffectDamageReduction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x789600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78C8C0))
  if fn:
    fn.setName('ExecuteCommandEffectDamageResistance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78C8C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x789930))
  if fn:
    fn.setName('ExecuteCommandEffectDamageShield', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x789930: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x787E10))
  if fn:
    fn.setName('ExecuteCommandEffectDroidStun', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x787E10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78A350))
  if fn:
    fn.setName('ExecuteCommandEffectForceDrain', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78A350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78CE40))
  if fn:
    fn.setName('ExecuteCommandEffectForcePushTargeted', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78CE40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78B9C0))
  if fn:
    fn.setName('ExecuteCommandEffectForcePushed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78B9C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78B2E0))
  if fn:
    fn.setName('ExecuteCommandEffectForceResistanceDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78B2E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7897F0))
  if fn:
    fn.setName('ExecuteCommandEffectForceResistanceIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7897F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78C300))
  if fn:
    fn.setName('ExecuteCommandEffectForceShield', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78C300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x789B10))
  if fn:
    fn.setName('ExecuteCommandEffectHeal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x789B10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78D250))
  if fn:
    fn.setName('ExecuteCommandEffectHealForcePoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78D250: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78A6C0))
  if fn:
    fn.setName('ExecuteCommandEffectHitPointChangeWhenDying', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78A6C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78CAC0))
  if fn:
    fn.setName('ExecuteCommandEffectHorrified', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78CAC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x788850))
  if fn:
    fn.setName('ExecuteCommandEffectImmunity', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x788850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78ACF0))
  if fn:
    fn.setName('ExecuteCommandEffectInvisibility', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78ACF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78DAB0))
  if fn:
    fn.setName('ExecuteCommandEffectLightsaberThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78DAB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78AB80))
  if fn:
    fn.setName('ExecuteCommandEffectLinkEffects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78AB80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78A9F0))
  if fn:
    fn.setName('ExecuteCommandEffectMissChance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78A9F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78C1C0))
  if fn:
    fn.setName('ExecuteCommandEffectModifyAttacks', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78C1C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78B420))
  if fn:
    fn.setName('ExecuteCommandEffectMovementSpeedDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78B420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78A0A0))
  if fn:
    fn.setName('ExecuteCommandEffectMovementSpeedIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78A0A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78AE60))
  if fn:
    fn.setName('ExecuteCommandEffectPoison', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78AE60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78D8D0))
  if fn:
    fn.setName('ExecuteCommandEffectPsychicStatic', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78D8D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78A210))
  if fn:
    fn.setName('ExecuteCommandEffectResurrection', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78A210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x786FC0))
  if fn:
    fn.setName('ExecuteCommandEffectSavingThrowDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x786FC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7871E0))
  if fn:
    fn.setName('ExecuteCommandEffectSavingThrowIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7871E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78C7A0))
  if fn:
    fn.setName('ExecuteCommandEffectSeeInvisible', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78C7A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78BCA0))
  if fn:
    fn.setName('ExecuteCommandEffectSkillDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78BCA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78BAE0))
  if fn:
    fn.setName('ExecuteCommandEffectSkillIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78BAE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78BE60))
  if fn:
    fn.setName('ExecuteCommandEffectSpellImmunity', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78BE60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78BFE0))
  if fn:
    fn.setName('ExecuteCommandEffectSpellLevelAbsorption', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78BFE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x789F30))
  if fn:
    fn.setName('ExecuteCommandEffectTemporaryForcePoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x789F30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78C440))
  if fn:
    fn.setName('ExecuteCommandEffectTemporaryHitpoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78C440: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78CD30))
  if fn:
    fn.setName('ExecuteCommandEffectTrueSeeing', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78CD30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78AFC0))
  if fn:
    fn.setName('ExecuteCommandEffectVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78AFC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78D030))
  if fn:
    fn.setName('ExecuteCommandEffectWhirlWind', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78D030: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78DE70))
  if fn:
    fn.setName('ExecuteCommandEventActivateItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78DE70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6C0890))
  if fn:
    fn.setName('ExecuteCommandEventConversation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6C0890: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78E220))
  if fn:
    fn.setName('ExecuteCommandEventSpellCastAt', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78E220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78E490))
  if fn:
    fn.setName('ExecuteCommandEventUserDefined', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78E490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78E630))
  if fn:
    fn.setName('ExecuteCommandExecuteScript', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78E630: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7A9820))
  if fn:
    fn.setName('ExecuteCommandFindSubString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A9820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AC030))
  if fn:
    fn.setName('ExecuteCommandFloatToInt', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AC030: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78E8F0))
  if fn:
    fn.setName('ExecuteCommandFloatingText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78E8F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x793A80))
  if fn:
    fn.setName('ExecuteCommandGetAC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x793A80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78F0E0))
  if fn:
    fn.setName('ExecuteCommandGetAbilityModifier', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78F0E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78EED0))
  if fn:
    fn.setName('ExecuteCommandGetAbilityScore', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78EED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797400))
  if fn:
    fn.setName('ExecuteCommandGetAppearanceType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797400: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78F500))
  if fn:
    fn.setName('ExecuteCommandGetAreaOfEffectCreator', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78F500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AD000))
  if fn:
    fn.setName('ExecuteCommandGetAreaUnescapable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AD000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78FA00))
  if fn:
    fn.setName('ExecuteCommandGetAttackTarget', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78FA00: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x79AF30))
  if fn:
    fn.setName('ExecuteCommandGetAttemptedAttackTarget', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79AF30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x707770))
  if fn:
    fn.setName('ExecuteCommandGetAttemptedMovementTarget', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x707770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x72D430))
  if fn:
    fn.setName('ExecuteCommandGetAttemptedSpellTarget', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x72D430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78FA80))
  if fn:
    fn.setName('ExecuteCommandGetBaseItemType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78FA80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x797770))
  if fn:
    fn.setName('ExecuteCommandGetBlockingDoor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78FC00))
  if fn:
    fn.setName('ExecuteCommandGetCasterLevel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78FC00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78FEB0))
  if fn:
    fn.setName('ExecuteCommandGetChallengeRating', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78FEB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5933B0))
  if fn:
    fn.setName('ExecuteCommandGetCheatCode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5933B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x790200))
  if fn:
    fn.setName('ExecuteCommandGetCommandable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x790200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7902A0))
  if fn:
    fn.setName('ExecuteCommandGetCreatureHasTalent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7902A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797150))
  if fn:
    fn.setName('ExecuteCommandGetCreatureMovmentType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x799100))
  if fn:
    fn.setName('ExecuteCommandGetCreatureSize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x799100: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x790420))
  if fn:
    fn.setName('ExecuteCommandGetCreatureTalent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x790420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C4A60))
  if fn:
    fn.setName('ExecuteCommandGetCurrentAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C4A60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x793F80))
  if fn:
    fn.setName('ExecuteCommandGetCurrentForcePoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x793F80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x790810))
  if fn:
    fn.setName('ExecuteCommandGetDamageDealtByType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x790810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x46B830))
  if fn:
    fn.setName('ExecuteCommandGetDifficultyModifier', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x46B830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x790A80))
  if fn:
    fn.setName('ExecuteCommandGetDistanceBetween', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x790A80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x790990))
  if fn:
    fn.setName('ExecuteCommandGetDistanceBetween2D', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x790990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x790BA0))
  if fn:
    fn.setName('ExecuteCommandGetDistanceBetweenLocations', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x790BA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x790DB0))
  if fn:
    fn.setName('ExecuteCommandGetDistanceBetweenLocations2D', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x790DB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x791000))
  if fn:
    fn.setName('ExecuteCommandGetDistanceToObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x791000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7911A0))
  if fn:
    fn.setName('ExecuteCommandGetDistanceToObject2D', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7911A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7916E0))
  if fn:
    fn.setName('ExecuteCommandGetEffectCreator', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7916E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A0D10))
  if fn:
    fn.setName('ExecuteCommandGetEffectDurationType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A0D10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x791830))
  if fn:
    fn.setName('ExecuteCommandGetEffectSpellId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x791830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x791790))
  if fn:
    fn.setName('ExecuteCommandGetEffectSubType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x791790: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x791970))
  if fn:
    fn.setName('ExecuteCommandGetEffectType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x791970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x791A10))
  if fn:
    fn.setName('ExecuteCommandGetEmotions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x791A10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B02B0))
  if fn:
    fn.setName('ExecuteCommandGetEncounterData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B02B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74B140))
  if fn:
    fn.setName('ExecuteCommandGetEnteringObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74B140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797ED0))
  if fn:
    fn.setName('ExecuteCommandGetExitingObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797ED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x791E90))
  if fn:
    fn.setName('ExecuteCommandGetFacing', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x791E90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7922A0))
  if fn:
    fn.setName('ExecuteCommandGetFactionAverageGoodEvilAlignment', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7922A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x792550))
  if fn:
    fn.setName('ExecuteCommandGetFactionAverageLevel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x792550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7921B0))
  if fn:
    fn.setName('ExecuteCommandGetFactionAverageReputation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7921B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x791FF0))
  if fn:
    fn.setName('ExecuteCommandGetFactionAverageXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x791FF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x792960))
  if fn:
    fn.setName('ExecuteCommandGetFactionBestAC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x792960: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ABD00))
  if fn:
    fn.setName('ExecuteCommandGetFactionEqual', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ABD00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7920D0))
  if fn:
    fn.setName('ExecuteCommandGetFactionGold', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7920D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x792740))
  if fn:
    fn.setName('ExecuteCommandGetFactionLeader', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x792740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x792C60))
  if fn:
    fn.setName('ExecuteCommandGetFactionLeastDamagedMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x792C60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7927D0))
  if fn:
    fn.setName('ExecuteCommandGetFactionMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7927D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x792380))
  if fn:
    fn.setName('ExecuteCommandGetFactionMostDamagedMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x792380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x792A70))
  if fn:
    fn.setName('ExecuteCommandGetFactionMostFrequentClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x792A70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x792630))
  if fn:
    fn.setName('ExecuteCommandGetFactionStrongestMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x792630: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x792B50))
  if fn:
    fn.setName('ExecuteCommandGetFactionWeakestMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x792B50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x792D70))
  if fn:
    fn.setName('ExecuteCommandGetFactionWorstAC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x792D70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x792E80))
  if fn:
    fn.setName('ExecuteCommandGetFortitudeSavingThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x792E80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7A13C0))
  if fn:
    fn.setName('ExecuteCommandGetGender', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A13C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x793030))
  if fn:
    fn.setName('ExecuteCommandGetGlobalLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x793030: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x793150))
  if fn:
    fn.setName('ExecuteCommandGetGlobalNumber', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x793150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6E5820))
  if fn:
    fn.setName('ExecuteCommandGetGlobalString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6E5820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x796F10))
  if fn:
    fn.setName('ExecuteCommandGetGoingToBeAttackedBy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x796F10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x793430))
  if fn:
    fn.setName('ExecuteCommandGetGold', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x793430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x793D10))
  if fn:
    fn.setName('ExecuteCommandGetGoldPieceValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x793D10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7938B0))
  if fn:
    fn.setName('ExecuteCommandGetHasFeatEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7938B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A0B00))
  if fn:
    fn.setName('ExecuteCommandGetHasInventory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A0B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x793C50))
  if fn:
    fn.setName('ExecuteCommandGetHasSkill', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x793C50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78F270))
  if fn:
    fn.setName('ExecuteCommandGetHasSpellEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78F270: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x717FB0))
  if fn:
    fn.setName('ExecuteCommandGetHitDice', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x717FB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A0300))
  if fn:
    fn.setName('ExecuteCommandGetHitpoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A0300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AC7C0))
  if fn:
    fn.setName('ExecuteCommandGetIdentified', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AC7C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x421990))
  if fn:
    fn.setName('ExecuteCommandGetInPersistentObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x421990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x448BB0))
  if fn:
    fn.setName('ExecuteCommandGetInventoryDisturbItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x448BB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A0E60))
  if fn:
    fn.setName('ExecuteCommandGetInventoryDisturbType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A0E60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x700D10))
  if fn:
    fn.setName('ExecuteCommandGetIsConversationActive', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x700D10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x723410))
  if fn:
    fn.setName('ExecuteCommandGetIsDawn', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x723410: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x70F0D0))
  if fn:
    fn.setName('ExecuteCommandGetIsDay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x70F0D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x460D20))
  if fn:
    fn.setName('ExecuteCommandGetIsDead', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x460D20: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x795000))
  if fn:
    fn.setName('ExecuteCommandGetIsDoorActionPossible', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x795000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x71DAC0))
  if fn:
    fn.setName('ExecuteCommandGetIsDusk', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x71DAC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7918D0))
  if fn:
    fn.setName('ExecuteCommandGetIsEffectValid', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7918D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x795410))
  if fn:
    fn.setName('ExecuteCommandGetIsEncounterCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x795410: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7954A0))
  if fn:
    fn.setName('ExecuteCommandGetIsImmune', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7954A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79E110))
  if fn:
    fn.setName('ExecuteCommandGetIsInCombat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79E110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B17D0))
  if fn:
    fn.setName('ExecuteCommandGetIsInConversation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B17D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7A10F0))
  if fn:
    fn.setName('ExecuteCommandGetIsListening', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A10F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x64B780))
  if fn:
    fn.setName('ExecuteCommandGetIsNight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64B780: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x795720))
  if fn:
    fn.setName('ExecuteCommandGetIsObjectValid', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x795720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79C090))
  if fn:
    fn.setName('ExecuteCommandGetIsOpen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79C090: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x791BF0))
  if fn:
    fn.setName('ExecuteCommandGetIsPC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x791BF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6557B0))
  if fn:
    fn.setName('ExecuteCommandGetIsTalentValid', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6557B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B1990))
  if fn:
    fn.setName('ExecuteCommandGetIsTrapped', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B1990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A14D0))
  if fn:
    fn.setName('ExecuteCommandGetIsWeaponEffective', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A14D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A0100))
  if fn:
    fn.setName('ExecuteCommandGetItemACValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A0100: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x795C80))
  if fn:
    fn.setName('ExecuteCommandGetItemHasItemProperty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x795C80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7D1FB0))
  if fn:
    fn.setName('ExecuteCommandGetItemInSlot', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D1FB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x796630))
  if fn:
    fn.setName('ExecuteCommandGetItemPossessedBy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x796630: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x796930))
  if fn:
    fn.setName('ExecuteCommandGetItemPossessor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x796930: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x796BC0))
  if fn:
    fn.setName('ExecuteCommandGetJournalQuestExperience', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x796BC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x796F90))
  if fn:
    fn.setName('ExecuteCommandGetLastAttackAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x796F90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79CF80))
  if fn:
    fn.setName('ExecuteCommandGetLastAttackMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79CF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7903A0))
  if fn:
    fn.setName('ExecuteCommandGetLastAttackResult', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7903A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797010))
  if fn:
    fn.setName('ExecuteCommandGetLastAttackType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x796E60))
  if fn:
    fn.setName('ExecuteCommandGetLastAttacker', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x796E60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x791CD0))
  if fn:
    fn.setName('ExecuteCommandGetLastClosedBy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x791CD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B0C30))
  if fn:
    fn.setName('ExecuteCommandGetLastCombatFeatUsed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B0C30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x794E20))
  if fn:
    fn.setName('ExecuteCommandGetLastDamager', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x794E20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797340))
  if fn:
    fn.setName('ExecuteCommandGetLastDisarmed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797480))
  if fn:
    fn.setName('ExecuteCommandGetLastDisturbed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A1230))
  if fn:
    fn.setName('ExecuteCommandGetLastForcePowerUsed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A1230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x412180))
  if fn:
    fn.setName('ExecuteCommandGetLastHostileActor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x412180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AD310))
  if fn:
    fn.setName('ExecuteCommandGetLastHostileTarget', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AD310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797510))
  if fn:
    fn.setName('ExecuteCommandGetLastKiller', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797610))
  if fn:
    fn.setName('ExecuteCommandGetLastLocked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7979B0))
  if fn:
    fn.setName('ExecuteCommandGetLastOpenedBy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7979B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x798200))
  if fn:
    fn.setName('ExecuteCommandGetLastPerceived', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x798200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797CE0))
  if fn:
    fn.setName('ExecuteCommandGetLastPerception', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797CE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797C40))
  if fn:
    fn.setName('ExecuteCommandGetLastSpeaker', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797C40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AD3A0))
  if fn:
    fn.setName('ExecuteCommandGetLastTrapDetected', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AD3A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797A40))
  if fn:
    fn.setName('ExecuteCommandGetLastUnlocked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797A40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797B20))
  if fn:
    fn.setName('ExecuteCommandGetLastUsedBy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797B20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78F830))
  if fn:
    fn.setName('ExecuteCommandGetLastWeaponUsed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78F830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x798C90))
  if fn:
    fn.setName('ExecuteCommandGetListenPatternNumber', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x798C90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7983E0))
  if fn:
    fn.setName('ExecuteCommandGetLocalValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7983E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x798520))
  if fn:
    fn.setName('ExecuteCommandGetLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x798520: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x798770))
  if fn:
    fn.setName('ExecuteCommandGetLockInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x798770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x795810))
  if fn:
    fn.setName('ExecuteCommandGetLocked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x795810: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x79DD00))
  if fn:
    fn.setName('ExecuteCommandGetMatchedStringsCount', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79DD00: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x794EA0))
  if fn:
    fn.setName('ExecuteCommandGetMetaMagicFeat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x794EA0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x73A680))
  if fn:
    fn.setName('ExecuteCommandGetMineInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73A680: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x798E20))
  if fn:
    fn.setName('ExecuteCommandGetModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x798E20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x798EA0))
  if fn:
    fn.setName('ExecuteCommandGetModuleItemStuff', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x798EA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x798F70))
  if fn:
    fn.setName('ExecuteCommandGetModuleName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x798F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78F5C0))
  if fn:
    fn.setName('ExecuteCommandGetMovementRate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78F5C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7991A0))
  if fn:
    fn.setName('ExecuteCommandGetName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7991A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6CA9C0))
  if fn:
    fn.setName('ExecuteCommandGetNearestObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6CA9C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79AA50))
  if fn:
    fn.setName('ExecuteCommandGetNumStackedItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79AA50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79AAD0))
  if fn:
    fn.setName('ExecuteCommandGetObjectByTag', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79AAD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79AC00))
  if fn:
    fn.setName('ExecuteCommandGetObjectInArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79AC00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x890A60))
  if fn:
    fn.setName('ExecuteCommandGetObjectInShape', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x890A60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x798690))
  if fn:
    fn.setName('ExecuteCommandGetObjectType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x798690: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79BF60))
  if fn:
    fn.setName('ExecuteCommandGetObjectVisibility', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79BF60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79CC80))
  if fn:
    fn.setName('ExecuteCommandGetPCLevellingUp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79CC80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5BDEC0))
  if fn:
    fn.setName('ExecuteCommandGetPCSpeaker', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5BDEC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55CC70))
  if fn:
    fn.setName('ExecuteCommandGetPartyAIStyle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55CC70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79CAC0))
  if fn:
    fn.setName('ExecuteCommandGetPartyMemberByIndex', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79CAC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x793DF0))
  if fn:
    fn.setName('ExecuteCommandGetPlaceableIllumination', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x793DF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79D510))
  if fn:
    fn.setName('ExecuteCommandGetPlayerRestrictMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79D510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AF950))
  if fn:
    fn.setName('ExecuteCommandGetPlotFlag', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AF950: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79CE90))
  if fn:
    fn.setName('ExecuteCommandGetPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79CE90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AF640))
  if fn:
    fn.setName('ExecuteCommandGetRacialType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AF640: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79D050))
  if fn:
    fn.setName('ExecuteCommandGetReflexAdjustedDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79D050: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79D300))
  if fn:
    fn.setName('ExecuteCommandGetReflexSavingThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79D300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79D450))
  if fn:
    fn.setName('ExecuteCommandGetReputation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79D450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79C1D0))
  if fn:
    fn.setName('ExecuteCommandGetSelectedPlanet', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79C1D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A1190))
  if fn:
    fn.setName('ExecuteCommandGetSoloMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A1190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79D740))
  if fn:
    fn.setName('ExecuteCommandGetSpellCast', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79D740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797FF0))
  if fn:
    fn.setName('ExecuteCommandGetSpellCastItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797FF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x798D10))
  if fn:
    fn.setName('ExecuteCommandGetSpellId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x798D10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79DDA0))
  if fn:
    fn.setName('ExecuteCommandGetSpellSaveDC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79DDA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x798180))
  if fn:
    fn.setName('ExecuteCommandGetSpellTarget', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x798180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79DF00))
  if fn:
    fn.setName('ExecuteCommandGetSpellTargetLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79DF00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79E050))
  if fn:
    fn.setName('ExecuteCommandGetSpellTargetObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79E050: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x79E8F0))
  if fn:
    fn.setName('ExecuteCommandGetStrRefSoundDuration', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79E8F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79E1B0))
  if fn:
    fn.setName('ExecuteCommandGetStringByStrRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79E1B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79E650))
  if fn:
    fn.setName('ExecuteCommandGetStringLeft', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79E650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79E430))
  if fn:
    fn.setName('ExecuteCommandGetStringLength', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79E430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79E520))
  if fn:
    fn.setName('ExecuteCommandGetStringLowerCase', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79E520: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79E2C0))
  if fn:
    fn.setName('ExecuteCommandGetStringRight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79E2C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79E7C0))
  if fn:
    fn.setName('ExecuteCommandGetStringUpperCase', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79E7C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5910C0))
  if fn:
    fn.setName('ExecuteCommandGetSubScreenID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5910C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A0760))
  if fn:
    fn.setName('ExecuteCommandGetSubString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A0760: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79EBB0))
  if fn:
    fn.setName('ExecuteCommandGetTag', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79EBB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DF450))
  if fn:
    fn.setName('ExecuteCommandGetTimeHour', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DF450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DD610))
  if fn:
    fn.setName('ExecuteCommandGetTimeMillisecond', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DD610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x72E4C0))
  if fn:
    fn.setName('ExecuteCommandGetTimeMinute', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x72E4C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6844D0))
  if fn:
    fn.setName('ExecuteCommandGetTimeSecond', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6844D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79EE60))
  if fn:
    fn.setName('ExecuteCommandGetTotalDamageDealt', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79EE60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79FFA0))
  if fn:
    fn.setName('ExecuteCommandGetTransitionTarget', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79FFA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x797250))
  if fn:
    fn.setName('ExecuteCommandGetUserActionsPending', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x797250: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79DA40))
  if fn:
    fn.setName('ExecuteCommandGetUserDefinedEventNumber', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79DA40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A12B0))
  if fn:
    fn.setName('ExecuteCommandGetWasForcePowerSuccessful', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A12B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79FE60))
  if fn:
    fn.setName('ExecuteCommandGetWaypointByTag', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79FE60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x795B90))
  if fn:
    fn.setName('ExecuteCommandGetWeaponRanged', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x795B90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A01B0))
  if fn:
    fn.setName('ExecuteCommandGetWillSavingThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A01B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AB7E0))
  if fn:
    fn.setName('ExecuteCommandGetXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AB7E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A03C0))
  if fn:
    fn.setName('ExecuteCommandGiveGoldToCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A03C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A05D0))
  if fn:
    fn.setName('ExecuteCommandGiveItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A05D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7A04B0))
  if fn:
    fn.setName('ExecuteCommandGiveXPToCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A04B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79EA00))
  if fn:
    fn.setName('ExecuteCommandInsertString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79EA00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B0510))
  if fn:
    fn.setName('ExecuteCommandIntToFloat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B0510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A0930))
  if fn:
    fn.setName('ExecuteCommandIntToHexString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A0930: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A0A10))
  if fn:
    fn.setName('ExecuteCommandIntToString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A0A10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A0C40))
  if fn:
    fn.setName('ExecuteCommandIsAvailableNPC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A0C40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AF9F0))
  if fn:
    fn.setName('ExecuteCommandIsMoviePlaying', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AF9F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A4DC0))
  if fn:
    fn.setName('ExecuteCommandIsNPCPartyMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A4DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7971D0))
  if fn:
    fn.setName('ExecuteCommandIsObjectPartyMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7971D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A15B0))
  if fn:
    fn.setName('ExecuteCommandItemActivated', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A15B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A17B0))
  if fn:
    fn.setName('ExecuteCommandLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A17B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A19D0))
  if fn:
    fn.setName('ExecuteCommandLocationAccess', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A19D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A1E10))
  if fn:
    fn.setName('ExecuteCommandModuleAccess', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A1E10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A21A0))
  if fn:
    fn.setName('ExecuteCommandMoveToObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A21A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A2530))
  if fn:
    fn.setName('ExecuteCommandMoveToPoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A2530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A2950))
  if fn:
    fn.setName('ExecuteCommandMusicBackground', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A2950: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x78FB00))
  if fn:
    fn.setName('ExecuteCommandMusicBackgroundGetBattleTrack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78FB00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x790900))
  if fn:
    fn.setName('ExecuteCommandMusicBackgroundGetDayTrack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x790900: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79A9C0))
  if fn:
    fn.setName('ExecuteCommandMusicBackgroundGetNightTrack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79A9C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A27B0))
  if fn:
    fn.setName('ExecuteCommandMusicBattle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A27B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x428700))
  if fn:
    fn.setName('ExecuteCommandNoClicksFor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x428700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A2C40))
  if fn:
    fn.setName('ExecuteCommandObjectToString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A2C40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A2E20))
  if fn:
    fn.setName('ExecuteCommandOpenStore', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A2E20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A7E40))
  if fn:
    fn.setName('ExecuteCommandPauseGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A7E40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A3150))
  if fn:
    fn.setName('ExecuteCommandPlayAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A3150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A3D70))
  if fn:
    fn.setName('ExecuteCommandPlayMovie', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A3D70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A7780))
  if fn:
    fn.setName('ExecuteCommandPlayPazaak', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A7780: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A3C10))
  if fn:
    fn.setName('ExecuteCommandPlayRoomAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A3C10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7893F0))
  if fn:
    fn.setName('ExecuteCommandPlayRumblePattern', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7893F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A40C0))
  if fn:
    fn.setName('ExecuteCommandPlaySound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A40C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A4240))
  if fn:
    fn.setName('ExecuteCommandPlayVisualAreaEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A4240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A4330))
  if fn:
    fn.setName('ExecuteCommandPopUpGUIPanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A4330: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A4540))
  if fn:
    fn.setName('ExecuteCommandPrintFloat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A4540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A9AC0))
  if fn:
    fn.setName('ExecuteCommandPrintInteger', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A9AC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AC310))
  if fn:
    fn.setName('ExecuteCommandPrintObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AC310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A48B0))
  if fn:
    fn.setName('ExecuteCommandPrintString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A48B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D4DB0))
  if fn:
    fn.setName('ExecuteCommandQueueMovie', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D4DB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A4C20))
  if fn:
    fn.setName('ExecuteCommandRandom', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A4C20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4FAE10))
  if fn:
    fn.setName('ExecuteCommandRandomName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4FAE10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ACB70))
  if fn:
    fn.setName('ExecuteCommandRemoveAvailableNPC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ACB70: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7A5390))
  if fn:
    fn.setName('ExecuteCommandRemoveFromParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A5390: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7A5650))
  if fn:
    fn.setName('ExecuteCommandRemovePartyMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A5650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74E160))
  if fn:
    fn.setName('ExecuteCommandResetDialogState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74E160: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A5740))
  if fn:
    fn.setName('ExecuteCommandResistSpell', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A5740: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7A5DF0))
  if fn:
    fn.setName('ExecuteCommandRollDice', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A5DF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F9FB0))
  if fn:
    fn.setName('ExecuteCommandSWMG_AdjustFollowerHitPoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F9FB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FA060))
  if fn:
    fn.setName('ExecuteCommandSWMG_DefaultImplementation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FA060: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FA650))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetCameraClipping', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FA650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FAAF0))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetFollowerFloatInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FAAF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FA370))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetFollowerIntInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FA370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FC050))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetFollowerPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FC050: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7FB740))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetLastBulletHitPart', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FB740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F9E00))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetLastBulletInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F9E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F9F30))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetLastBulletShooter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F9F30: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7FA4D0))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetLastHPChange', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FA4D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FA180))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetLastObjectHit', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FA180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FB810))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetLateralAccelerationPerSecond', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FB810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FA7D0))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetMiniGameCounts', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FA7D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FA860))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetMiniGameObjects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FA860: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7FA760))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetPlayer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FA760: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FBF80))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetPlayerFloatInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FBF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FB900))
  if fn:
    fn.setName('ExecuteCommandSWMG_GetPlayerVectorInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FB900: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7FA920))
  if fn:
    fn.setName('ExecuteCommandSWMG_IsSomething', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FA920: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7FA6E0))
  if fn:
    fn.setName('ExecuteCommandSWMG_SetCameraClip', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FA6E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FAD60))
  if fn:
    fn.setName('ExecuteCommandSWMG_SetFollowerFloatInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FAD60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FACB0))
  if fn:
    fn.setName('ExecuteCommandSWMG_SetFollowerIntInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FACB0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7FB420))
  if fn:
    fn.setName('ExecuteCommandSWMG_SetGunBankIntInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FB420: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7FC210))
  if fn:
    fn.setName('ExecuteCommandSWMG_SetLateralAccelerationPerSecond', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FC210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FBAA0))
  if fn:
    fn.setName('ExecuteCommandSWMG_SetPlayerFloatInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FBAA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FBB90))
  if fn:
    fn.setName('ExecuteCommandSWMG_SetPlayerVectorInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FBB90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7FBDF0))
  if fn:
    fn.setName('ExecuteCommandSWMG_SetSoundProperty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FBDF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F97C0))
  if fn:
    fn.setName('ExecuteCommandSWMG_SetSpeedBlurEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F97C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7FAA80))
  if fn:
    fn.setName('ExecuteCommandSWMG_StartInvulnerability', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7FAA80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A5F20))
  if fn:
    fn.setName('ExecuteCommandSaveNPCState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A5F20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A5F70))
  if fn:
    fn.setName('ExecuteCommandSavingThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A5F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A60E0))
  if fn:
    fn.setName('ExecuteCommandSendMessageToPC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A60E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7AFA40))
  if fn:
    fn.setName('ExecuteCommandSetAreaTransitionBMP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AFA40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B1240))
  if fn:
    fn.setName('ExecuteCommandSetAreaUnescapable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B1240: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7AB560))
  if fn:
    fn.setName('ExecuteCommandSetCameraFacing', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AB560: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A64F0))
  if fn:
    fn.setName('ExecuteCommandSetCameraMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A64F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A6590))
  if fn:
    fn.setName('ExecuteCommandSetCommandable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A6590: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7ACE40))
  if fn:
    fn.setName('ExecuteCommandSetDialogPlaceableCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ACE40: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7A84A0))
  if fn:
    fn.setName('ExecuteCommandSetEncounterData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A84A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A69B0))
  if fn:
    fn.setName('ExecuteCommandSetFacing', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A69B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AD530))
  if fn:
    fn.setName('ExecuteCommandSetForcePowerUnsuccessful', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AD530: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7A7C20))
  if fn:
    fn.setName('ExecuteCommandSetGalaxyMapPlanetAvailable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A7C20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ACB00))
  if fn:
    fn.setName('ExecuteCommandSetGalaxyMapPlanetSelectable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ACB00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x53A840))
  if fn:
    fn.setName('ExecuteCommandSetGlobalFadeIn', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x53A840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x52B520))
  if fn:
    fn.setName('ExecuteCommandSetGlobalFadeOut', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x52B520: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A6C60))
  if fn:
    fn.setName('ExecuteCommandSetGlobalLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A6C60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A6DA0))
  if fn:
    fn.setName('ExecuteCommandSetGlobalNumber', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A6DA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A6EE0))
  if fn:
    fn.setName('ExecuteCommandSetGlobalString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A6EE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7955E0))
  if fn:
    fn.setName('ExecuteCommandSetGoodEvilValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7955E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A7000))
  if fn:
    fn.setName('ExecuteCommandSetIdentified', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A7000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A7080))
  if fn:
    fn.setName('ExecuteCommandSetIsDestroyable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A7080: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7A7620))
  if fn:
    fn.setName('ExecuteCommandSetLightsaberPowered', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A7620: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D2890))
  if fn:
    fn.setName('ExecuteCommandSetListenString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D2890: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ABBD0))
  if fn:
    fn.setName('ExecuteCommandSetListening', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ABBD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A7890))
  if fn:
    fn.setName('ExecuteCommandSetLocalValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A7890: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7A76E0))
  if fn:
    fn.setName('ExecuteCommandSetLocked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A76E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7ACBE0))
  if fn:
    fn.setName('ExecuteCommandSetNPCSelectability', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ACBE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A4710))
  if fn:
    fn.setName('ExecuteCommandSetPartyAIStyle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A4710: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x78DCE0))
  if fn:
    fn.setName('ExecuteCommandSetPlaceableIllumination', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x78DCE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ABF80))
  if fn:
    fn.setName('ExecuteCommandSetPlotFlag', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ABF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A7DB0))
  if fn:
    fn.setName('ExecuteCommandSetRestrictMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A7DB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x758AD0))
  if fn:
    fn.setName('ExecuteCommandSetReturnStrref', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x758AD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AEBA0))
  if fn:
    fn.setName('ExecuteCommandSetSoloMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AEBA0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7A7F60))
  if fn:
    fn.setName('ExecuteCommandSetSubType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A7F60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A8050))
  if fn:
    fn.setName('ExecuteCommandSetTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A8050: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x438930))
  if fn:
    fn.setName('ExecuteCommandSetTutorialWindowsEnabled', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x438930: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ACAB0))
  if fn:
    fn.setName('ExecuteCommandShowGalaxyMap', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ACAB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A82D0))
  if fn:
    fn.setName('ExecuteCommandShowLevelUpGUI', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A82D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64DD70))
  if fn:
    fn.setName('ExecuteCommandShowPartySelectionGUI', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64DD70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x788E60))
  if fn:
    fn.setName('ExecuteCommandShowTutorialWindow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x788E60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AF840))
  if fn:
    fn.setName('ExecuteCommandShowUpgradeScreen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AF840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A8670))
  if fn:
    fn.setName('ExecuteCommandSignalEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A8670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A89D0))
  if fn:
    fn.setName('ExecuteCommandSoundObjectFadeAndStop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A89D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A8770))
  if fn:
    fn.setName('ExecuteCommandSoundObjectGetFixedVariance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A8770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A87F0))
  if fn:
    fn.setName('ExecuteCommandSoundObjectGetPitchVariance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A87F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A8870))
  if fn:
    fn.setName('ExecuteCommandSoundObjectGetVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A8870: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A8150))
  if fn:
    fn.setName('ExecuteCommandSoundObjectPlay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A8150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A8950))
  if fn:
    fn.setName('ExecuteCommandSoundObjectSetFixedVariance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A8950: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A8BD0))
  if fn:
    fn.setName('ExecuteCommandSoundObjectSetPitchVariation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A8BD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A8A50))
  if fn:
    fn.setName('ExecuteCommandSoundObjectSetPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A8A50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A8AF0))
  if fn:
    fn.setName('ExecuteCommandSoundObjectSetVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A8AF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A88F0))
  if fn:
    fn.setName('ExecuteCommandSoundObjectStop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A88F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x79DBE0))
  if fn:
    fn.setName('ExecuteCommandStopRumblePattern', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79DBE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48D4C0))
  if fn:
    fn.setName('ExecuteCommandStringConversions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48D4C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x564320))
  if fn:
    fn.setName('ExecuteCommandSuppressStatusSummaryEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x564320: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7AAE00))
  if fn:
    fn.setName('ExecuteCommandVersusEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AAE00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7A4770))
  if fn:
    fn.setName('ExecuteCommandWriteTimestampedLogEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7A4770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F9140))
  if fn:
    fn.setName('InitializeSWMGCommands', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F9140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77E950))
  if fn:
    fn.setName('LoadGameDefinedStructure', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77E950: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4ED490))
  if fn:
    fn.setName('RunScriptCallback', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4ED490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77E8C0))
  if fn:
    fn.setName('SaveGameDefinedStructure', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77E8C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6408D0))
  if fn:
    fn.setName('_CScriptCompiler', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptCompiler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptCompiler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6408D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6410D0))
  if fn:
    fn.setName('ShutDown', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptCompilerInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptCompilerInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6410D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x640FE0))
  if fn:
    fn.setName('_CScriptCompilerInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptCompilerInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptCompilerInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640FE0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x77AC40))
  if fn:
    fn.setName('CopyScriptEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptEvent', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptEvent', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77AC40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77AFE0))
  if fn:
    fn.setName('LoadEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptEvent', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptEvent', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77AFE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77ADB0))
  if fn:
    fn.setName('SaveEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptEvent', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptEvent', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77ADB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A4E60))
  if fn:
    fn.setName('CopyScriptLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptLocation', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptLocation', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A4E60: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6A5200))
  if fn:
    fn.setName('CopyScriptTalent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptTalent', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptTalent', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A5200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A5330))
  if fn:
    fn.setName('LoadTalent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptTalent', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptTalent', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A5330: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A5270))
  if fn:
    fn.setName('SaveTalent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptTalent', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptTalent', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A5270: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x650010))
  if fn:
    fn.setName('LoadNode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIEventNode', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIEventNode', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x650010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64FBB0))
  if fn:
    fn.setName('SaveNode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIEventNode', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIEventNode', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64FBB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64CB70))
  if fn:
    fn.setName('RemoveObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64CB70: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x64DA60))
  if fn:
    fn.setName('AddObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIMaster', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64DA60: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x64E8F0))
  if fn:
    fn.setName('LoadEventQueue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIMaster', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64E8F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64DAE0))
  if fn:
    fn.setName('RemoveObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIMaster', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64DAE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64E840))
  if fn:
    fn.setName('SaveEventQueue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIMaster', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64E840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64CF10))
  if fn:
    fn.setName('_CServerAIMaster', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIMaster', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64CF10: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x64C530))
  if fn:
    fn.setName('ClearAllActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C2D0))
  if fn:
    fn.setName('ClientToServerObjectId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C2D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C460))
  if fn:
    fn.setName('ComputeHeight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C7D0))
  if fn:
    fn.setName('ComputeSafeLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C7D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x64BB40))
  if fn:
    fn.setName('GetAreaByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BB40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D3A0))
  if fn:
    fn.setName('GetClientObjectByObjectId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D3A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x64C0B0))
  if fn:
    fn.setName('GetFactionOfObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C0B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64BB00))
  if fn:
    fn.setName('GetItemByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BB00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63AC00))
  if fn:
    fn.setName('GetPauseState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63AC00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64BB80))
  if fn:
    fn.setName('GetPlaceableByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BB80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x64BC40))
  if fn:
    fn.setName('GetSoundObjectByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BC40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C140))
  if fn:
    fn.setName('SetPauseState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C000))
  if fn:
    fn.setName('StopServices', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C330))
  if fn:
    fn.setName('TestDirectLine', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C330: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C7B0))
  if fn:
    fn.setName('TestSafeLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C7B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C0D0))
  if fn:
    fn.setName('TogglePauseState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C0D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64B700))
  if fn:
    fn.setName('_CServerExoApp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64B700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BC600))
  if fn:
    fn.setName('AddToExclusionList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BC600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BDF50))
  if fn:
    fn.setName('AutoSaveOnEnteringModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BDF50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BCF80))
  if fn:
    fn.setName('ClientToServerObjectId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BCF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BD1D0))
  if fn:
    fn.setName('CreatureAcquireItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BD1D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BFB10))
  if fn:
    fn.setName('DoModuleEnterSaveCleanup', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BFB10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BB4B0))
  if fn:
    fn.setName('DumpServerOptionsToLog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BB4B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B4750))
  if fn:
    fn.setName('ExportAllPlayers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B4750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BDC10))
  if fn:
    fn.setName('ForceRunAreaOnExitScripts', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BDC10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BC050))
  if fn:
    fn.setName('GetActiveTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BC050: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BAE30))
  if fn:
    fn.setName('GetAreaByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BAE30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B26B0))
  if fn:
    fn.setName('GetClientObjectByObjectId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B26B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B2750))
  if fn:
    fn.setName('GetClientObjectByPlayerId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B2750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BB570))
  if fn:
    fn.setName('GetFactionOfObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BB570: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BFCB0))
  if fn:
    fn.setName('GetModulePreviouslyVisited', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BFCB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BDD30))
  if fn:
    fn.setName('GetNormalOfPoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BDD30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BAF80))
  if fn:
    fn.setName('GetPlaceableByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BAF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BB700))
  if fn:
    fn.setName('GetPlayerListString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BB700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BB340))
  if fn:
    fn.setName('GetServerInfoFromIniFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BB340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BB1B0))
  if fn:
    fn.setName('GetSoundObjectByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BB1B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B8A10))
  if fn:
    fn.setName('InitiateModuleForPlayer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B8A10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B2F00))
  if fn:
    fn.setName('LoadGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B2F00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B4FD0))
  if fn:
    fn.setName('LoadModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B4FD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B8F70))
  if fn:
    fn.setName('PlayerListChange', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B8F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x612C40))
  if fn:
    fn.setName('RemoveFromExclusionList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x612C40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BBF50))
  if fn:
    fn.setName('ReprocessExclusionListActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BBF50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BC8A0))
  if fn:
    fn.setName('ResolvePlayerByFirstName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BC8A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B4800))
  if fn:
    fn.setName('SendCharacterQuery', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B4800: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BBF20))
  if fn:
    fn.setName('SetPauseState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BBF20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BE0F0))
  if fn:
    fn.setName('ShrinkLineByCreatures', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BE0F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BAAC0))
  if fn:
    fn.setName('Shutdown', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BAAC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B3120))
  if fn:
    fn.setName('StartNewModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B3120: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B96E0))
  if fn:
    fn.setName('StartServices', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B96E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B9EF0))
  if fn:
    fn.setName('StopServices', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B9EF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B39E0))
  if fn:
    fn.setName('StoreCurrentModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B39E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B3660))
  if fn:
    fn.setName('StorePlayerCharacters', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B3660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BD310))
  if fn:
    fn.setName('TempDeathUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BD310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BDA30))
  if fn:
    fn.setName('TestSafeLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BDA30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BBE70))
  if fn:
    fn.setName('TogglePauseState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BBE70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6C9710))
  if fn:
    fn.setName('UpDatePlayerPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6C9710: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BDB00))
  if fn:
    fn.setName('UpdateMapData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BDB00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B2380))
  if fn:
    fn.setName('_CServerExoAppInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B2380: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x438080))
  if fn:
    fn.setName('_CServerInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerInfo', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerInfo', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x438080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D2E00))
  if fn:
    fn.setName('LoadFromGFF', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CStatusSummary', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CStatusSummary', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D2E00: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x628430))
  if fn:
    fn.setName('AddFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTlkTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTlkTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x628430: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x627640))
  if fn:
    fn.setName('_CTlkTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTlkTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTlkTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x627640: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x50B000))
  if fn:
    fn.setName('Load2DArrays_ForceAdjust', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x50B000: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x482800))
  if fn:
    fn.setName('Load2DArrays_Placeables', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CTwoDimArrays', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CTwoDimArrays', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x482800: '+str(e)); errors += 1
print('Batch 6 done: renamed='+str(renamed)+' errors='+str(errors))