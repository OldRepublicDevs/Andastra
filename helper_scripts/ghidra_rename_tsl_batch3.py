from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0
try:
  fn = fm.getFunctionAt(toAddr(0x7E84F0))
  if fn:
    fn.setName('UpdateSubAreas', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E84F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7F0E90))
  if fn:
    fn.setName('Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CSWSScriptVar>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CSWSScriptVar>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F0E90: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x640B80))
  if fn:
    fn.setName('GetRunScriptReturnValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachine', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachine', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x477660))
  if fn:
    fn.setName('ClearAllActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x477660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A2FE0))
  if fn:
    fn.setName('OnEndDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A2FE0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x656CB0))
  if fn:
    fn.setName('GetWeaponFinesse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x656CB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65EF50))
  if fn:
    fn.setName('IsLightsaberEquipped', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65EF50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x413720))
  if fn:
    fn.setName('operator__', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Vector', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Vector', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x413720: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x774F70))
  if fn:
    fn.setName('GetAttack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x774F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x771940))
  if fn:
    fn.setName('GetTotalDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatAttackData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatAttackData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x771940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77AB90))
  if fn:
    fn.setName('SetFloat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCMessageData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCMessageData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77AB90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x720D10))
  if fn:
    fn.setName('ClearLineOfSight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x720D10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C0310))
  if fn:
    fn.setName('SJournalEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('SJournalEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'SJournalEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C0310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C0400))
  if fn:
    fn.setName('SJournalEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('SJournalEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'SJournalEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C0400: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x547A90))
  if fn:
    fn.setName('AddAndDispose', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<SJournalEntry>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<SJournalEntry>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x547A90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77A8F0))
  if fn:
    fn.setName('Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CSWSPlayerJournalQuestUpdates>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CSWSPlayerJournalQuestUpdates>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77A8F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D550))
  if fn:
    fn.setName('GetMGOArray', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x539D40))
  if fn:
    fn.setName('GetMiniGameObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGameObjectArray', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGameObjectArray', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x539D40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x539E60))
  if fn:
    fn.setName('GetMiniGameObjectByName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGameObjectArray', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGameObjectArray', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x539E60: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5DC910))
  if fn:
    fn.setName('SetBulletSpeed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGGunBank', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGGunBank', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DC910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DCA00))
  if fn:
    fn.setName('SetBulletLifeSpan', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGGunBank', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGGunBank', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DCA00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DC9D0))
  if fn:
    fn.setName('SetRateOfFire', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGGunBank', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGGunBank', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DC9D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DD130))
  if fn:
    fn.setName('SetHorizontalSpread', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGTargettingGunBank', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGTargettingGunBank', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DD130: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DD0F0))
  if fn:
    fn.setName('SetVerticalSpread', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGTargettingGunBank', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGTargettingGunBank', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DD0F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DD0C0))
  if fn:
    fn.setName('SetSensingRadius', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGTargettingGunBank', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGTargettingGunBank', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DD0C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DD170))
  if fn:
    fn.setName('SetInaccuracy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGTargettingGunBank', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGTargettingGunBank', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DD170: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x943910))
  if fn:
    fn.setName('CResNCS', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResNCS', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResNCS', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x943910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44C050))
  if fn:
    fn.setName('UpdateLoadingScreenHint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44C050: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x919D00))
  if fn:
    fn.setName('SetCurValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiProgressBar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiProgressBar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x919D00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A890))
  if fn:
    fn.setName('Render', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A890: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x608480))
  if fn:
    fn.setName('WriteIniEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoIni', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoIni', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x608480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A480))
  if fn:
    fn.setName('CExoSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63AAF0))
  if fn:
    fn.setName('EnableNonStreamingEnvironmentEffects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63AAF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63AAC0))
  if fn:
    fn.setName('EnableStreamingEnvironmentEffects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63AAC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A770))
  if fn:
    fn.setName('Set2D3DBias', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63AB20))
  if fn:
    fn.setName('GetEAX', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63AB20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A800))
  if fn:
    fn.setName('Get2D3DBias', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A800: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A830))
  if fn:
    fn.setName('GetNumber2DVoices', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A860))
  if fn:
    fn.setName('GetNumber3DVoices', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A860: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4789B0))
  if fn:
    fn.setName('GetPlayerCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWParty', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWParty', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4789B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AF40))
  if fn:
    fn.setName('AddResourceImageFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AF40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AF20))
  if fn:
    fn.setName('AddEncapsulatedResourceFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AF20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x462C00))
  if fn:
    fn.setName('SetFullScreenEnabled', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientOptions', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientOptions', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x462C00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x43A5E0))
  if fn:
    fn.setName('SetDisplayDevMode', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x43A5E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x43A510))
  if fn:
    fn.setName('GetNearestVideoMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAuroraInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAuroraInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x43A510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x43A1C0))
  if fn:
    fn.setName('SetVideoMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x43A1C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D660))
  if fn:
    fn.setName('GetMoveToModuleMovies', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44DCE0))
  if fn:
    fn.setName('CExoArrayList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CExoString>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CExoString>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44DCE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B530))
  if fn:
    fn.setName('PauseWorldTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E11F0))
  if fn:
    fn.setName('SetGlobalUnpauseableState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E11F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4447B0))
  if fn:
    fn.setName('ShowLoadScreen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4447B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x921210))
  if fn:
    fn.setName('ShowFadePanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoMoviePlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoMoviePlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x921210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9211E0))
  if fn:
    fn.setName('PlayMoviesAsync', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoMoviePlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoMoviePlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9211E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44DDA0))
  if fn:
    fn.setName('_CExoArrayList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CExoString>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CExoString>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44DDA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F7920))
  if fn:
    fn.setName('CSWCAnimBaseTW', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseTW', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseTW', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F7920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F67F0))
  if fn:
    fn.setName('CSWCAnimBaseHead', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseHead', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseHead', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F67F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F3850))
  if fn:
    fn.setName('CSWCAnimBaseHeadWield', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseHeadWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseHeadWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F3850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F37D0))
  if fn:
    fn.setName('Set', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWAnimBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWAnimBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F37D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x907620))
  if fn:
    fn.setName('IsResident', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAuroraInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAuroraInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x907620: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x906ED0))
  if fn:
    fn.setName('RequestModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAuroraInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAuroraInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x906ED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40C930))
  if fn:
    fn.setName('GetPlayerCreatureId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C930: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63AA30))
  if fn:
    fn.setName('Play3DOneShotSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63AA30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x42C190))
  if fn:
    fn.setName('SetSize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x42C190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x911660))
  if fn:
    fn.setName('HandleLMouseDown', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x911660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44CB30))
  if fn:
    fn.setName('SetLastTarget', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44CB30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x51E760))
  if fn:
    fn.setName('PopulateMenus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x51E760: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x515FF0))
  if fn:
    fn.setName('IsTargetActionMenuControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x515FF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x911760))
  if fn:
    fn.setName('HandleLMouseUp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x911760: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90DAA0))
  if fn:
    fn.setName('InitControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90DAA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90DA20))
  if fn:
    fn.setName('StopLoadFromLayout', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90DA20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D700))
  if fn:
    fn.setName('ResetDriveAcceleration', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B2E00))
  if fn:
    fn.setName('StopBark', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBarkBubble', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBarkBubble', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B2E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AF660))
  if fn:
    fn.setName('SetMiddle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialogLetterbox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialogLetterbox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AF660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AF3F0))
  if fn:
    fn.setName('SetTop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialogLetterbox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialogLetterbox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AF3F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AF4F0))
  if fn:
    fn.setName('SetBottom', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialogLetterbox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialogLetterbox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AF4F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DA340))
  if fn:
    fn.setName('SetSWGuiStatus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DA340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x472990))
  if fn:
    fn.setName('GetProperty0x68', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x472990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x536580))
  if fn:
    fn.setName('GetServerPlaceable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x536580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90EAF0))
  if fn:
    fn.setName('RemovePanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90EAF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E0E60))
  if fn:
    fn.setName('UpdateCreatedInGameGUI', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E0E60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90EC00))
  if fn:
    fn.setName('SendPanelToBack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90EC00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x527F60))
  if fn:
    fn.setName('SetActiveControlID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x527F60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D7F0))
  if fn:
    fn.setName('GetPausedByCombat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D7F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D810))
  if fn:
    fn.setName('SetPausedByCombat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D6B0))
  if fn:
    fn.setName('IndicateObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D6B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x566340))
  if fn:
    fn.setName('CSWGuiInGameGalaxyMap', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameGalaxyMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameGalaxyMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x566340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x704740))
  if fn:
    fn.setName('SetSelectedPlanet', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x704740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40E480))
  if fn:
    fn.setName('GetCharacterChangeInProgress', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40E480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x595EB0))
  if fn:
    fn.setName('CSWGuiInGameCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameCharacter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameCharacter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x595EB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A8190))
  if fn:
    fn.setName('CSWGuiStore', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A8190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60B840))
  if fn:
    fn.setName('ResolveFileName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoAliasList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoAliasList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60B840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x606610))
  if fn:
    fn.setName('Left', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x606610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x927726))
  if fn:
    fn.setName('_rename', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x927726: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60BB10))
  if fn:
    fn.setName('Write', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60BB10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60B990))
  if fn:
    fn.setName('Flush', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60B990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AEE10))
  if fn:
    fn.setName('GetReplyIndex', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AEE10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D05C0))
  if fn:
    fn.setName('InitializeDialogAnimationData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D05C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D8BD0))
  if fn:
    fn.setName('SetDialogAnimations', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D8BD0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5D8B00))
  if fn:
    fn.setName('Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRK4Acceleration', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRK4Acceleration', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D8B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80D740))
  if fn:
    fn.setName('Yaw', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80D740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40DD20))
  if fn:
    fn.setName('GetCombatMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40DD20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x574940))
  if fn:
    fn.setName('SetTurn', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWBehaviorCameraCombat', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWBehaviorCameraCombat', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x574940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80D900))
  if fn:
    fn.setName('Roll', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80D900: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80D820))
  if fn:
    fn.setName('Pitch', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80D820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x42A8F0))
  if fn:
    fn.setName('zdir', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Quaternion', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Quaternion', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x42A8F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4ECB70))
  if fn:
    fn.setName('CSWItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4ECB70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44EB90))
  if fn:
    fn.setName('RemoveObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientAIList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientAIList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44EB90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44EEE0))
  if fn:
    fn.setName('RemoveObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientAIMaster', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientAIMaster', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44EEE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40E4A0))
  if fn:
    fn.setName('SetCharacterChangeInProgress', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40E4A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40CDF0))
  if fn:
    fn.setName('SetPlayerCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CDF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40C9D0))
  if fn:
    fn.setName('GetPlayerByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C9D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x493880))
  if fn:
    fn.setName('GetMaxHitPoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x493880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40CD30))
  if fn:
    fn.setName('AttachPlayerGameObjectToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CD30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x46A990))
  if fn:
    fn.setName('SetCameraFollow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x46A990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47EAC0))
  if fn:
    fn.setName('GetEquippedBy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47EAC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40CAF0))
  if fn:
    fn.setName('GetSoundObjectByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CAF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55B650))
  if fn:
    fn.setName('Stop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55B650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55B7E0))
  if fn:
    fn.setName('ChangePosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55B7E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40CA50))
  if fn:
    fn.setName('GetTriggerByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CA50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x537EA0))
  if fn:
    fn.setName('RemoveFromRenderList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x537EA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x539350))
  if fn:
    fn.setName('AddFireAndForgetAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x539350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40C9F0))
  if fn:
    fn.setName('GetItemByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C9F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47EC20))
  if fn:
    fn.setName('GetActiveProperty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47EC20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47F4C0))
  if fn:
    fn.setName('SetItemPropertyUsesLeft', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47F4C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47EF40))
  if fn:
    fn.setName('SetStackSize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47EF40: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x41F5A0))
  if fn:
    fn.setName('StripEquippedItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41F5A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x411770))
  if fn:
    fn.setName('PlaySoundSetSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x411770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40C550))
  if fn:
    fn.setName('GetWorldJournal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5474A0))
  if fn:
    fn.setName('DeleteEntryStrRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCWorldJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCWorldJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5474A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5476D0))
  if fn:
    fn.setName('DeleteAllEntries', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCWorldJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCWorldJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5476D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D00B0))
  if fn:
    fn.setName('SetLevelUpMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D00B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x46CC20))
  if fn:
    fn.setName('SetTimeOfDayState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x46CC20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x456060))
  if fn:
    fn.setName('GetMapPin', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x456060: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x476D60))
  if fn:
    fn.setName('SetObjectVisibilityOnSetArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x476D60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x49BD20))
  if fn:
    fn.setName('HandleServerToPlayerCreatureUpdate_Delete', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x49BD20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BF260))
  if fn:
    fn.setName('HandleServerToPlayerAreaOfEffectUpdate_Delete', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BF260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C6790))
  if fn:
    fn.setName('HandleServerToPlayerAmbientMusicSetDelay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C6790: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C69B0))
  if fn:
    fn.setName('HandleServerToPlayerAmbientBattleMusicChange', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C69B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C6BC0))
  if fn:
    fn.setName('HandleServerToPlayerAmbientSoundVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C6BC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A4B00))
  if fn:
    fn.setName('HandleServerToPlayerItemUpdate_Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A4B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A6BA0))
  if fn:
    fn.setName('HandleServerToPlayerTriggerUpdate_Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A6BA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A1530))
  if fn:
    fn.setName('HandleServerToPlayerDoorUpdate_Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A1530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BF2C0))
  if fn:
    fn.setName('HandleServerToPlayerAreaOfEffectUpdate_Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BF2C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4A2260))
  if fn:
    fn.setName('HandleServerToPlayerPlaceableUpdate_Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A2260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61A360))
  if fn:
    fn.setName('SetBullet', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61A360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54BE20))
  if fn:
    fn.setName('WriteVOIDPtr', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54BE20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x646DE0))
  if fn:
    fn.setName('CResSSF', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResSSF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResSSF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x646DE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5505C0))
  if fn:
    fn.setName('HandleWagerExit', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakStart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakStart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5505C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90EE40))
  if fn:
    fn.setName('PopModalPanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90EE40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54E530))
  if fn:
    fn.setName('UpdateWagerText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiWagerPopup', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiWagerPopup', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54E530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x555700))
  if fn:
    fn.setName('SetHandEnabled', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x555700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5E2D40))
  if fn:
    fn.setName('ChooseSidedeck', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CPazaakPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CPazaakPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E2D40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DA580))
  if fn:
    fn.setName('CSWCAnimBasePlaceable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBasePlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBasePlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DA580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E1290))
  if fn:
    fn.setName('GetCanClick', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E1290: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x534CC0))
  if fn:
    fn.setName('AnimationUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x534CC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x506CD0))
  if fn:
    fn.setName('AnimationUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x506CD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x51E680))
  if fn:
    fn.setName('SetActionDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x51E680: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A190))
  if fn:
    fn.setName('GetFilePlaying', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A080))
  if fn:
    fn.setName('IsPlaying', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D3E0))
  if fn:
    fn.setName('GetInputClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D3E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90D840))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90D840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x639F70))
  if fn:
    fn.setName('CExoStreamingSoundSource', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A2F0))
  if fn:
    fn.setName('SetPriorityGroup', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A2F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A110))
  if fn:
    fn.setName('Play', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x538180))
  if fn:
    fn.setName('UpdatePosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x538180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5393C0))
  if fn:
    fn.setName('AnimationUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5393C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9167F0))
  if fn:
    fn.setName('RemoveModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScene', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScene', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9167F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5661C0))
  if fn:
    fn.setName('SetMapNote', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMapHider', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMapHider', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5661C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x563C40))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x563C40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x911080))
  if fn:
    fn.setName('HandleMouseMove', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x911080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x489D30))
  if fn:
    fn.setName('UnloadConjureVisual', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSpellCastVisual', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSpellCastVisual', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x489D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C96B0))
  if fn:
    fn.setName('RefreshHilite', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C96B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61A3C0))
  if fn:
    fn.setName('operator__', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResRef', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61A3C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F5D30))
  if fn:
    fn.setName('ItemIsValid', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F5D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x490150))
  if fn:
    fn.setName('GetSkillRank', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x490150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91AEA0))
  if fn:
    fn.setName('CSWGuiListBox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91AEA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AECF0))
  if fn:
    fn.setName('Reset', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AECF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x597FF0))
  if fn:
    fn.setName('SetStats', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameCharacter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameCharacter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x597FF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48F3D0))
  if fn:
    fn.setName('GetLevel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCLevelUpStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCLevelUpStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48F3D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91DF50))
  if fn:
    fn.setName('GetSelectedControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91DF50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x774AE0))
  if fn:
    fn.setName('RemoveAllActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x774AE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x52D170))
  if fn:
    fn.setName('SetAddRemoveLabel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPartySelection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPartySelection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x52D170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x512A40))
  if fn:
    fn.setName('CSWGuiButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiButton', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiButton', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x512A40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90D110))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90D110: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x91EEA0))
  if fn:
    fn.setName('CSWGuiEditbox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiEditbox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiEditbox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91EEA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90CE80))
  if fn:
    fn.setName('CenterPanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90CE80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x588180))
  if fn:
    fn.setName('ClearChart', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlowChart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlowChart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x588180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x588BA0))
  if fn:
    fn.setName('BuildSortedFeatList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlowChart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlowChart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x588BA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x510EE0))
  if fn:
    fn.setName('IsFeatUseable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWClass', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWClass', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x510EE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B4810))
  if fn:
    fn.setName('CreateMainPowersFeatsSkillsMenu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersFeatsSkillsDebugMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersFeatsSkillsDebugMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B4810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DBBA0))
  if fn:
    fn.setName('HideLoadModuleDebugMenu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DBBA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x733B00))
  if fn:
    fn.setName('AddTail', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLinkedListInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x733B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86BA10))
  if fn:
    fn.setName('AurCheckFrameBufferModificationsAvailable', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86BA10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86B9F0))
  if fn:
    fn.setName('AurCheckSoftShadowsAvailable', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86B9F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x56D550))
  if fn:
    fn.setName('SetAntiAliasText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsGraphicsAdvanced', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsGraphicsAdvanced', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x56D550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x56D880))
  if fn:
    fn.setName('SetAnisotropyText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsGraphicsAdvanced', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsGraphicsAdvanced', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x56D880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x622B40))
  if fn:
    fn.setName('Unload2DArray', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('C2DA', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'C2DA', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x622B40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x56D100))
  if fn:
    fn.setName('SetTexQualText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsGraphicsAdvanced', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsGraphicsAdvanced', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x56D100: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90CF20))
  if fn:
    fn.setName('ReloadToolTipPanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90CF20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9185D0))
  if fn:
    fn.setName('CSWGuiSlider', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSlider', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSlider', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9185D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x506050))
  if fn:
    fn.setName('SendPlayerToServer_LevelUp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x506050: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x599500))
  if fn:
    fn.setName('AttachModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameCharacter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameCharacter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x599500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5E9AC0))
  if fn:
    fn.setName('CopyCreatureStats', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E9AC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6165D0))
  if fn:
    fn.setName('GetEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInput', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6165D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F8160))
  if fn:
    fn.setName('DeterminePower', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F8160: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F81F0))
  if fn:
    fn.setName('BuildButtons', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F81F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F8630))
  if fn:
    fn.setName('RemoveChosenPower', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F8630: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D33B0))
  if fn:
    fn.setName('ShowTutorialWindow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D33B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90E2E0))
  if fn:
    fn.setName('SetVisible', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90E2E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91B2A0))
  if fn:
    fn.setName('ClearItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91B2A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4929E0))
  if fn:
    fn.setName('CalcNumberFeats', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCLevelUpStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCLevelUpStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4929E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F31F0))
  if fn:
    fn.setName('SelectionCompleted', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F31F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F46F0))
  if fn:
    fn.setName('FeatCreationCompleted', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCustomPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCustomPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F46F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FEA40))
  if fn:
    fn.setName('CompletePanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiAbilitiesCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiAbilitiesCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FEA40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x944330))
  if fn:
    fn.setName('CResLIP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResLIP', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResLIP', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x944330: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90C4B0))
  if fn:
    fn.setName('CResPLT', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResPLT', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResPLT', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90C4B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x578DE0))
  if fn:
    fn.setName('CResTGA', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResTGA', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResTGA', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x578DE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90C550))
  if fn:
    fn.setName('CResVIS', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResVIS', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResVIS', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90C550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90C5F0))
  if fn:
    fn.setName('CResTPC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResTPC', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResTPC', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90C5F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F7900))
  if fn:
    fn.setName('CResMDL', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResMDL', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResMDL', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F7900: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8131E0))
  if fn:
    fn.setName('CResDDS', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResDDS', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResDDS', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8131E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63D990))
  if fn:
    fn.setName('CExoSoundSourceInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63D990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63F190))
  if fn:
    fn.setName('SetPriorityGroup', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63F190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61B390))
  if fn:
    fn.setName('CancelRequest', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61B390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91B820))
  if fn:
    fn.setName('SetScrollBarExtent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91B820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91C9F0))
  if fn:
    fn.setName('OrganizeControls', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91C9F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9171C0))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiControl', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiControl', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9171C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60BC00))
  if fn:
    fn.setName('SeekBeginning', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60BC00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x610C70))
  if fn:
    fn.setName('CExoCriticalSection', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoCriticalSection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoCriticalSection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x610C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62C820))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFFFieldIDHash', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFFFieldIDHash', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62C820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54A3F0))
  if fn:
    fn.setName('CSWMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54A3F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6283B0))
  if fn:
    fn.setName('Hash', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFFFieldIDHash', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFFFieldIDHash', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6283B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x646AB0))
  if fn:
    fn.setName('SetCommandImplementer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x646AB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x653380))
  if fn:
    fn.setName('ReadTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGlobalVariableTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGlobalVariableTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x653380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DB4B0))
  if fn:
    fn.setName('AnimationDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DB4B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x64DC40))
  if fn:
    fn.setName('AddEventAbsoluteTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIMaster', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64DC40: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7E78C0))
  if fn:
    fn.setName('SetDuration', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E78C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47A9A0))
  if fn:
    fn.setName('GetFollowRange', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWParty', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWParty', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47A9A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47DC80))
  if fn:
    fn.setName('GetPointBackFromStart', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyFollowDataArray', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyFollowDataArray', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47DC80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x757420))
  if fn:
    fn.setName('HandleReply', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x757420: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x693FF0))
  if fn:
    fn.setName('UseItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x693FF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67DF40))
  if fn:
    fn.setName('AddCastSpellActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67DF40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D360))
  if fn:
    fn.setName('IsPlayerCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x479870))
  if fn:
    fn.setName('RemoveCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWParty', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWParty', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x479870: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EF060))
  if fn:
    fn.setName('SetNPCFactionReputation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CFactionManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CFactionManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EF060: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6E1590))
  if fn:
    fn.setName('GetArmorClassVersus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6E1590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F3820))
  if fn:
    fn.setName('GetBaseFortSavingThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F3820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F3C60))
  if fn:
    fn.setName('GetBaseReflexSavingThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F3C60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F3A40))
  if fn:
    fn.setName('GetBaseWillSavingThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F3A40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41A480))
  if fn:
    fn.setName('GetEquippedItemID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41A480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80AF80))
  if fn:
    fn.setName('ConvertFromCalendarDay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80AF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80AED0))
  if fn:
    fn.setName('ConvertHourMinSecMsToMs', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80AED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B110))
  if fn:
    fn.setName('SetWorldTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F8BB0))
  if fn:
    fn.setName('EffectTypeToGameEffect', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F8BB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40CA30))
  if fn:
    fn.setName('GetAreaByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CA30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x457880))
  if fn:
    fn.setName('AttachmentFromServerArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x457880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61B7C0))
  if fn:
    fn.setName('Free', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61B7C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x733BF0))
  if fn:
    fn.setName('RemoveAt', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CSWSTagNode>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CSWSTagNode>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x733BF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x469B10))
  if fn:
    fn.setName('Delete', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameObjectArray', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameObjectArray', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x469B10: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7F0670))
  if fn:
    fn.setName('_CExoLinkedList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLinkedList<uint>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLinkedList<uint>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F0670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B1730))
  if fn:
    fn.setName('Allocate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CSWSScriptVar>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CSWSScriptVar>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B1730: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x645C50))
  if fn:
    fn.setName('GetRunScriptReturnValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x645C50: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6073E0))
  if fn:
    fn.setName('CExoLocString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLocString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLocString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6073E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75CDF0))
  if fn:
    fn.setName('Allocate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CSWSPlayerJournalQuestUpdates>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CSWSPlayerJournalQuestUpdates>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75CDF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AA20))
  if fn:
    fn.setName('CRes', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CRes', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CRes', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AA20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x634CF0))
  if fn:
    fn.setName('Render', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x634CF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x633CF0))
  if fn:
    fn.setName('CExoSoundInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x633CF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x634140))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x634140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40E920))
  if fn:
    fn.setName('Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CExoString>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CExoString>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40E920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x608330))
  if fn:
    fn.setName('StartGlobalFade', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x608330: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x43AEC0))
  if fn:
    fn.setName('StartLoadMusic', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x43AEC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x920C30))
  if fn:
    fn.setName('PlayMoviesAsync', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoMoviePlayerInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoMoviePlayerInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x920C30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9084A0))
  if fn:
    fn.setName('GetModelInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAuroraInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAuroraInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9084A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x777C00))
  if fn:
    fn.setName('AddBefore', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLinkedListInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x777C00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x637490))
  if fn:
    fn.setName('Play3DOneShotSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x637490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x912380))
  if fn:
    fn.setName('RemoveToolTip', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x912380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9120D0))
  if fn:
    fn.setName('UpdateMouseOverControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9120D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x912460))
  if fn:
    fn.setName('SetTemporarilyDisableToolTip', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x912460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4388E0))
  if fn:
    fn.setName('ResetDriveAcceleration', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4388E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D76C0))
  if fn:
    fn.setName('setToDestroy_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D76C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90ED10))
  if fn:
    fn.setName('GetPosInPanelList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90ED10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90ECB0))
  if fn:
    fn.setName('GetPosInModalStack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90ECB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x903DF0))
  if fn:
    fn.setName('deleteAt', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x903DF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90F5F0))
  if fn:
    fn.setName('DetermineNeedToDraw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90F5F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59D120))
  if fn:
    fn.setName('CSWGuiInGameEquip', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59D120: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59A070))
  if fn:
    fn.setName('CSWGuiInGameInventory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59A070: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44DC90))
  if fn:
    fn.setName('indexOf', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<int>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<int>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44DC90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x448500))
  if fn:
    fn.setName('GetPausedByCombat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x448500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x448540))
  if fn:
    fn.setName('SetPausedByCombat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x448540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x443170))
  if fn:
    fn.setName('IndicateObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x443170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61F040))
  if fn:
    fn.setName('CleanDirectory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61F040: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62AAF0))
  if fn:
    fn.setName('CERFFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CERFFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CERFFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62AAF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40E120))
  if fn:
    fn.setName('GetLoadStep', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40E120: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62ABB0))
  if fn:
    fn.setName('_CERFFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CERFFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CERFFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62ABB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4CEA50))
  if fn:
    fn.setName('AppendToDialogBuffer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4CEA50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D07A0))
  if fn:
    fn.setName('StoreLastDialogAnimationData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D07A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5D8C30))
  if fn:
    fn.setName('Accelerate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRK4Acceleration', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRK4Acceleration', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D8C30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x444C80))
  if fn:
    fn.setName('SetPlayerCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x444C80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x438300))
  if fn:
    fn.setName('GetPlayerByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x438300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x438750))
  if fn:
    fn.setName('AttachPlayerGameObjectToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x438750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E2C60))
  if fn:
    fn.setName('CSWCameraOnAStick', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCameraOnAStick', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCameraOnAStick', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E2C60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x46AE40))
  if fn:
    fn.setName('GetCameraOffset', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x46AE40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x421880))
  if fn:
    fn.setName('ChangeTarget', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCameraOnAStick', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCameraOnAStick', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x421880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E7940))
  if fn:
    fn.setName('SetPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCameraOnAStick', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCameraOnAStick', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E7940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x458FB0))
  if fn:
    fn.setName('lockcamera', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x458FB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4204F0))
  if fn:
    fn.setName('UpdateShadowBlob', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4204F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4386E0))
  if fn:
    fn.setName('GetSoundObjectByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4386E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x639E30))
  if fn:
    fn.setName('SetPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639E30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A390))
  if fn:
    fn.setName('SetPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4384B0))
  if fn:
    fn.setName('GetTriggerByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4384B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x438360))
  if fn:
    fn.setName('GetItemByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x438360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x455E00))
  if fn:
    fn.setName('SetAsDay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x455E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x455F20))
  if fn:
    fn.setName('SetAsNight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x455F20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x455D60))
  if fn:
    fn.setName('SetAsDawn', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x455D60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x455CC0))
  if fn:
    fn.setName('SetAsDusk', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x455CC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x456180))
  if fn:
    fn.setName('UpdateSoundObjectsTimeOfDay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x456180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C72D0))
  if fn:
    fn.setName('return_true_4', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C72D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x476A20))
  if fn:
    fn.setName('SetFadeState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x476A20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4578C0))
  if fn:
    fn.setName('AddCorpse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4578C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x414080))
  if fn:
    fn.setName('SetCombatState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x414080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40CAD0))
  if fn:
    fn.setName('GetAreaOfEffectByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CAD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5847A0))
  if fn:
    fn.setName('StopEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5847A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x537C70))
  if fn:
    fn.setName('UpdateTriggerColor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x537C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40CAB0))
  if fn:
    fn.setName('GetDoorByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CAB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40CA90))
  if fn:
    fn.setName('GetPlaceableByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CA90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x553000))
  if fn:
    fn.setName('RefreshDisplay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x553000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63CA40))
  if fn:
    fn.setName('IsPlaying', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63CA40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63ACE0))
  if fn:
    fn.setName('CExoStreamingSoundSourceInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63ACE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63C970))
  if fn:
    fn.setName('SetVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63C970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x474730))
  if fn:
    fn.setName('AnimationFireAndForget', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x474730: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x474660))
  if fn:
    fn.setName('AnimationLooping', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x474660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x474C20))
  if fn:
    fn.setName('GetTopFireForgetAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x474C20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x475210))
  if fn:
    fn.setName('RemoveTopFireForgetAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x475210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90F4B0))
  if fn:
    fn.setName('HitCheckMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90F4B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x917640))
  if fn:
    fn.setName('GetSelectableParent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiControl', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiControl', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x917640: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9175F0))
  if fn:
    fn.setName('GetIsChild', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiControl', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiControl', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9175F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91E590))
  if fn:
    fn.setName('HitCheckMouseLocal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91E590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91E490))
  if fn:
    fn.setName('HandleMouseMove', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91E490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x916930))
  if fn:
    fn.setName('CSWGuiNavigable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiNavigable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiNavigable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x916930: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x919FE0))
  if fn:
    fn.setName('CSWGuiScrollBar', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScrollBar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScrollBar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x919FE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91F4B0))
  if fn:
    fn.setName('CExoArrayList_CSWGuiExtent_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CSWGuiExtent>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CSWGuiExtent>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91F4B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91DF10))
  if fn:
    fn.setName('GetControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91DF10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x915570))
  if fn:
    fn.setName('CSWGuiText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiText', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiText', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x915570: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90E4A0))
  if fn:
    fn.setName('GetExtentAccountingForPanelOffset', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90E4A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8CFD70))
  if fn:
    fn.setName('AurGUISetupViewport', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8CFD70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8D02D0))
  if fn:
    fn.setName('AurGUICloseViewport', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8D02D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8CFD00))
  if fn:
    fn.setName('AurGUIStopLayer', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8CFD00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x915E00))
  if fn:
    fn.setName('CSWGuiEditText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiEditText', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiEditText', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x915E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D580))
  if fn:
    fn.setName('GetMiniGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x611F70))
  if fn:
    fn.setName('GetEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x611F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F2940))
  if fn:
    fn.setName('ChangeState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F2940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F3A60))
  if fn:
    fn.setName('SetState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCustomPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCustomPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F3A60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90C850))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiExtent', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiExtent', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90C850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90E320))
  if fn:
    fn.setName('GetControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90E320: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9175D0))
  if fn:
    fn.setName('AddChildControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiControl', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiControl', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9175D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x610D90))
  if fn:
    fn.setName('CExoCriticalSectionInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoCriticalSectionInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoCriticalSectionInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x610D90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x605BF0))
  if fn:
    fn.setName('operator__', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x605BF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6074F0))
  if fn:
    fn.setName('operator__', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLocString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLocString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6074F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DDBA0))
  if fn:
    fn.setName('CanUseItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DDBA0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x67D500))
  if fn:
    fn.setName('AddItemCastSpellActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67D500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x693DB0))
  if fn:
    fn.setName('UseSkill', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x693DB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x444BF0))
  if fn:
    fn.setName('IsPlayerCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x444BF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x438440))
  if fn:
    fn.setName('GetAreaByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x438440: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40EA10))
  if fn:
    fn.setName('Allocate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CExoString>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CExoString>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40EA10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x907730))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAuroraModel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAuroraModel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x907730: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61ABB0))
  if fn:
    fn.setName('GetDemands', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CRes', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CRes', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61ABB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x912410))
  if fn:
    fn.setName('ResetToolTipTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x912410: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x616790))
  if fn:
    fn.setName('PauseRumble', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInput', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x616790: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6167B0))
  if fn:
    fn.setName('UnpauseRumble', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInput', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6167B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80DF50))
  if fn:
    fn.setName('RotationArc', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80DF50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63ECA0))
  if fn:
    fn.setName('SetPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63ECA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55C120))
  if fn:
    fn.setName('UpdateTimeOfDay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55C120: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x438670))
  if fn:
    fn.setName('GetAreaOfEffectByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x438670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F9D00))
  if fn:
    fn.setName('SetAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F9D00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x438600))
  if fn:
    fn.setName('GetDoorByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x438600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x438590))
  if fn:
    fn.setName('GetPlaceableByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x438590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B1B0))
  if fn:
    fn.setName('GetTimeDifferenceFromWorldTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B1B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90E350))
  if fn:
    fn.setName('GetLocalMouseCoords', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90E350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9179B0))
  if fn:
    fn.setName('RemoveToolTip', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9179B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91C810))
  if fn:
    fn.setName('SetSelectedControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91C810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x914660))
  if fn:
    fn.setName('CSWGuiImage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiImage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiImage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x914660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91F570))
  if fn:
    fn.setName('Allocate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CSWGuiExtent>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CSWGuiExtent>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91F570: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x915440))
  if fn:
    fn.setName('SetTextObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTextParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTextParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x915440: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x472050))
  if fn:
    fn.setName('GetMiniGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x472050: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6120D0))
  if fn:
    fn.setName('IsEventInClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6120D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F3280))
  if fn:
    fn.setName('ClearPowers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F3280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F3220))
  if fn:
    fn.setName('ClearAbilities', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F3220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F3240))
  if fn:
    fn.setName('ClearSkills', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F3240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F3260))
  if fn:
    fn.setName('ClearFeats', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F3260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6074D0))
  if fn:
    fn.setName('Compare', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLocString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLocString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6074D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AB40))
  if fn:
    fn.setName('Demand', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CRes', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CRes', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AB40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80C0F0))
  if fn:
    fn.setName('Normalize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Vector', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Vector', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80C0F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63E8F0))
  if fn:
    fn.setName('IsPlaying', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63E8F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x639C90))
  if fn:
    fn.setName('Set3D', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639C90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x639DD0))
  if fn:
    fn.setName('SetLooping', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639DD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91A7D0))
  if fn:
    fn.setName('setCurValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScrollBar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScrollBar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91A7D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x914270))
  if fn:
    fn.setName('CSWGuiImageParams', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiImageParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiImageParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x914270: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x914600))
  if fn:
    fn.setName('SetImageObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiImageParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiImageParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x914600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E4350))
  if fn:
    fn.setName('SetSize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<unsigned_long>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<unsigned_long>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E4350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x607CA0))
  if fn:
    fn.setName('Compare', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLocStringInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLocStringInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x607CA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61B580))
  if fn:
    fn.setName('Demand', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61B580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63ED20))
  if fn:
    fn.setName('Set3D', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63ED20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61E740))
  if fn:
    fn.setName('GetFreeDiskSpace', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61E740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61DE00))
  if fn:
    fn.setName('UpdateKeyTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61DE00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61DC60))
  if fn:
    fn.setName('Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61DC60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AC50))
  if fn:
    fn.setName('AsyncRimLoadThreadProc', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AC50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90ED80))
  if fn:
    fn.setName('PushModalPanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90ED80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9117D0))
  if fn:
    fn.setName('HandleRMouseDown', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9117D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x911920))
  if fn:
    fn.setName('HandleZMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x911920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90FCF0))
  if fn:
    fn.setName('Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90FCF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62E5A0))
  if fn:
    fn.setName('AddKey', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoKeyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62E5A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6304A0))
  if fn:
    fn.setName('RebuildTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoKeyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6304A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x496280))
  if fn:
    fn.setName('CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CRes2DA,2017>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CRes2DA,2017>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x496280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91AA30))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScrollBar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScrollBar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91AA30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91D920))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91D920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x919B80))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiProgressBar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiProgressBar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x919B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91A880))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScrollBar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScrollBar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91A880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91E750))
  if fn:
    fn.setName('HandleLMouseDown', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91E750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x861E10))
  if fn:
    fn.setName('LoadVisibility', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Scene', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Scene', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x861E10: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x706F40))
  if fn:
    fn.setName('CountVisibleToPlayers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x706F40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x653640))
  if fn:
    fn.setName('NoCreaturesOnLine', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x653640: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90B300))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResIFO,2014>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResIFO,2014>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90B300: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x775960))
  if fn:
    fn.setName('ResolveMasterState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x775960: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x774920))
  if fn:
    fn.setName('AddAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x774920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7776B0))
  if fn:
    fn.setName('DEBUGDumpCombatRoundToScreen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7776B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x773E50))
  if fn:
    fn.setName('IncrementTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x773E50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x772DE0))
  if fn:
    fn.setName('StartCombatRound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x772DE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73C680))
  if fn:
    fn.setName('OnRemoveSummonCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73C680: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73C730))
  if fn:
    fn.setName('OnApplySavingThrowDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73C730: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73CDA0))
  if fn:
    fn.setName('OnApplyACDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73CDA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73D330))
  if fn:
    fn.setName('OnApplyAttackDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73D330: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73DBD0))
  if fn:
    fn.setName('OnApplyDamageImmunityDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73DBD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x740720))
  if fn:
    fn.setName('OnApplySetStateInternal', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x740720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x741150))
  if fn:
    fn.setName('OnRemoveSetStateInternal', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x741150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7464E0))
  if fn:
    fn.setName('OnApplyMovementSpeedDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7464E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7422A0))
  if fn:
    fn.setName('OnApplyForceResistanceDecrease', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7422A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x743F50))
  if fn:
    fn.setName('OnApplyLink', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x743F50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x743FD0))
  if fn:
    fn.setName('OnApplyHasteInternal', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x743FD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7441A0))
  if fn:
    fn.setName('OnApplySlowInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7441A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x744D20))
  if fn:
    fn.setName('OnApplyLight', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x744D20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7460F0))
  if fn:
    fn.setName('OnApplyTaunt', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7460F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x741470))
  if fn:
    fn.setName('OnApplySkillDecrease', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x741470: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7474A0))
  if fn:
    fn.setName('OnRemoveDisguise', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7474A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7487D0))
  if fn:
    fn.setName('OnApplyDarkness', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7487D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x748DB0))
  if fn:
    fn.setName('OnApplyDisappear', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x748DB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x749540))
  if fn:
    fn.setName('OnRemoveNegativeLevel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x749540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74C480))
  if fn:
    fn.setName('OnApplyPureEvilPowers', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74C480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x744E00))
  if fn:
    fn.setName('OnApplyVision', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x744E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x747330))
  if fn:
    fn.setName('OnApplyDisguise', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x747330: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x69D9D0))
  if fn:
    fn.setName('AutoCloseGUIPanels', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69D9D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A2A10))
  if fn:
    fn.setName('DisplayFloatyDataToSelf', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A2A10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A2B70))
  if fn:
    fn.setName('DumpToLog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A2B70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x696C70))
  if fn:
    fn.setName('CancelRest', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x696C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A19B0))
  if fn:
    fn.setName('UpdateActionQueue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A19B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x69F120))
  if fn:
    fn.setName('PacifyCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69F120: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x69DF60))
  if fn:
    fn.setName('GetInvisible', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69DF60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A42D0))
  if fn:
    fn.setName('UpdatePureGoodEvilPowers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A42D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90B700))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResARE,2012>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResARE,2012>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90B700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x720690))
  if fn:
    fn.setName('UnloadArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x720690: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x716B80))
  if fn:
    fn.setName('_CSWSArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x716B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6D5F50))
  if fn:
    fn.setName('WalkUpdateLocation_QuickWalk_FollowLeader_TeleportForward', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6D5F50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DC500))
  if fn:
    fn.setName('AIActionAppear', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DC500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DC430))
  if fn:
    fn.setName('AIActionDisappear', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DC430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6CD6F0))
  if fn:
    fn.setName('HandleSubAreaStateChanges', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6CD6F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DFFC0))
  if fn:
    fn.setName('GetUserActionsPending', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DFFC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6CD3A0))
  if fn:
    fn.setName('UpdateSubareasOnMoveTo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6CD3A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x652550))
  if fn:
    fn.setName('ReadTableWithoutCatalogue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGlobalVariableTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGlobalVariableTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x652550: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6DF080))
  if fn:
    fn.setName('NVThunkItemGameObject_AIUpdate', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DF080: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x768A60))
  if fn:
    fn.setName('GetLinkedObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x768A60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x765140))
  if fn:
    fn.setName('OpenDoor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x765140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x778C60))
  if fn:
    fn.setName('LoadDialogCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x778C60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x753700))
  if fn:
    fn.setName('SendDialogModels', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x753700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x758E80))
  if fn:
    fn.setName('ReEquipItemsForDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x758E80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x758C90))
  if fn:
    fn.setName('ReEquipHItemForDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x758C90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F5AC0))
  if fn:
    fn.setName('ValidateLevelUp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F5AC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x656DD0))
  if fn:
    fn.setName('CancelAllCombatModes', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x656DD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6583F0))
  if fn:
    fn.setName('ResolveSafeProjectile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6583F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x658870))
  if fn:
    fn.setName('ResolveProjectileMissLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x658870: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x663170))
  if fn:
    fn.setName('SetAttackSummaryFeedbackEffectData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x663170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x657750))
  if fn:
    fn.setName('ResolveRangedSpecialAttack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x657750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65CE10))
  if fn:
    fn.setName('ResolveAttackRoll', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65CE10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65D840))
  if fn:
    fn.setName('ResolveDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65D840: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x90BC80))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResNSS,2009>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResNSS,2009>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90BC80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x627210))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResNCS,2010>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResNCS,2010>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x627210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x636980))
  if fn:
    fn.setName('GetClosestRecentSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x636980: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x638C70))
  if fn:
    fn.setName('StopAllOneShots', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x638C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6415A0))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResWave,4>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResWave,4>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6415A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62B2E0))
  if fn:
    fn.setName('WriteStringTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CERFFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CERFFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62B2E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x907820))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResLYT,3000>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResLYT,3000>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x907820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x612A50))
  if fn:
    fn.setName('SetEventRepeatable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x612A50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44EAD0))
  if fn:
    fn.setName('CoolDownEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44EAD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6126A0))
  if fn:
    fn.setName('RemoveEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6126A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A1780))
  if fn:
    fn.setName('GetString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLocString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLocString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A1780: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60F840))
  if fn:
    fn.setName('AsyncReadThreadProc', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60F840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60D650))
  if fn:
    fn.setName('AddString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLocStringInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLocStringInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60D650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44CB80))
  if fn:
    fn.setName('BusyWaitMenuMusic', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44CB80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x43AE40))
  if fn:
    fn.setName('FadeAndStopStreamingSoundSource', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x43AE40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7756A0))
  if fn:
    fn.setName('GetControlledCharacterDead', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7756A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x430CA0))
  if fn:
    fn.setName('AddToPauseExclusionList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x430CA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x458730))
  if fn:
    fn.setName('giveitem', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x458730: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41CCD0))
  if fn:
    fn.setName('SetNewLoopingCreatureSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41CCD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4207D0))
  if fn:
    fn.setName('ReAttachCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4207D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x426FE0))
  if fn:
    fn.setName('PartyMenuStealth', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x426FE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40F4A0))
  if fn:
    fn.setName('SwitchWeaponEvent', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40F4A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x413E70))
  if fn:
    fn.setName('ComputeWeaponOverlays', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x413E70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x419700))
  if fn:
    fn.setName('UpdateVisibleWeapons', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x419700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x419850))
  if fn:
    fn.setName('SetStealthState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x419850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x419FA0))
  if fn:
    fn.setName('EquipToInventorySlot', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x419FA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x423DF0))
  if fn:
    fn.setName('AnimateFireAndForget', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x423DF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x415D80))
  if fn:
    fn.setName('LoadModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x415D80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x52E060))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMessageBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMessageBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x52E060: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x52EF50))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStatusSummary', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStatusSummary', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x52EF50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5E43C0))
  if fn:
    fn.setName('CSWGuiInGameMessages', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameMessages', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameMessages', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E43C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D4750))
  if fn:
    fn.setName('LoadAmbientTrackForDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D4750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D45E0))
  if fn:
    fn.setName('SetAnimatedCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D45E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DB160))
  if fn:
    fn.setName('HideItemCreateMenu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DB160: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DB310))
  if fn:
    fn.setName('HideExamineBox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DB310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DB3B0))
  if fn:
    fn.setName('ShowContainerGui', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DB3B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DB430))
  if fn:
    fn.setName('HideContainerGui', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DB430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DBC40))
  if fn:
    fn.setName('HidePowersFeatsSkillsDebugMenu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DBC40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DD200))
  if fn:
    fn.setName('HandleDialogSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DD200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DF070))
  if fn:
    fn.setName('HideAreaTransition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DF070: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E02F0))
  if fn:
    fn.setName('EnterControllerPauseState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E02F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E03A0))
  if fn:
    fn.setName('ResetInterfaceForSize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E03A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E0AE0))
  if fn:
    fn.setName('IsOnDialogNonDependantList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E0AE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E1370))
  if fn:
    fn.setName('UpdateDialogLipSync', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E1370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4CE110))
  if fn:
    fn.setName('InitializeGlobal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4CE110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47D9F0))
  if fn:
    fn.setName('AdvancePointOnPath', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyFollowDataArray', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyFollowDataArray', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47D9F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x563290))
  if fn:
    fn.setName('IsPartyTogether', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWParty', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWParty', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x563290: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E3AB0))
  if fn:
    fn.setName('Control', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCameraOnAStick', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCameraOnAStick', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E3AB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x474DA0))
  if fn:
    fn.setName('InsertFireForgetAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x474DA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x472750))
  if fn:
    fn.setName('_CSWCObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x472750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4740F0))
  if fn:
    fn.setName('SetPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4740F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x46B440))
  if fn:
    fn.setName('AcclTurnCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x46B440: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x46BED0))
  if fn:
    fn.setName('GetNextCameraPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x46BED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x591010))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x591010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47FF70))
  if fn:
    fn.setName('ResolveCreaturePoweredAnimations', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47FF70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x493C70))
  if fn:
    fn.setName('SetPortraitId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x493C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6026F0))
  if fn:
    fn.setName('GetClassesString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6026F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C0770))
  if fn:
    fn.setName('HandleServerToPlayerJournalMessage_SetQuestPicture', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C0770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C1AD0))
  if fn:
    fn.setName('HandleServerToPlayerGuiQuickbar_SetButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C1AD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55C280))
  if fn:
    fn.setName('GetTimeDiff', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55C280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55ABF0))
  if fn:
    fn.setName('SetPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55ABF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x545550))
  if fn:
    fn.setName('CreateGunBank', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x545550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x544440))
  if fn:
    fn.setName('CreateGunBank', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniEnemy', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniEnemy', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x544440: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x55CD00))
  if fn:
    fn.setName('Go', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55CD00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55EBF0))
  if fn:
    fn.setName('AddTrack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55EBF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DC370))
  if fn:
    fn.setName('CSWMGGunBank', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGGunBank', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGGunBank', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DC370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x909AA0))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResSSF,2060>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResSSF,2060>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x909AA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x540490))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLoadScreen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLoadScreen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x540490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F5AC0))
  if fn:
    fn.setName('LoadFromLayout', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F5AC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x552E80))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x552E80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x507580))
  if fn:
    fn.setName('MenuActionSecurity', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x507580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x514880))
  if fn:
    fn.setName('DrawStack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterfaceChar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterfaceChar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x514880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x516DE0))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x516DE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x515860))
  if fn:
    fn.setName('SelectNextAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x515860: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x515A10))
  if fn:
    fn.setName('SelectPrevAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x515A10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x523760))
  if fn:
    fn.setName('OnStealthButtonPressed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x523760: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5237D0))
  if fn:
    fn.setName('OnCharacterClicked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5237D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x523B60))
  if fn:
    fn.setName('ClearAllActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x523B60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x592310))
  if fn:
    fn.setName('CSWGuiMainInterfaceChar', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterfaceChar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterfaceChar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x592310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x558350))
  if fn:
    fn.setName('PlayStinger', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x558350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x558680))
  if fn:
    fn.setName('PlayTruncated', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x558680: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x557380))
  if fn:
    fn.setName('PlayMusic', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x557380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F9280))
  if fn:
    fn.setName('_CSWCVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F9280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F9380))
  if fn:
    fn.setName('AIUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F9380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x559A80))
  if fn:
    fn.setName('CSWBehaviorAttachSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWBehaviorAttachSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWBehaviorAttachSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x559A80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x567AB0))
  if fn:
    fn.setName('OnPlanetClicked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameGalaxyMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameGalaxyMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x567AB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x567440))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameGalaxyMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameGalaxyMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x567440: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48E6E0))
  if fn:
    fn.setName('ForceAnimate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureAppearance', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureAppearance', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48E6E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48B9D0))
  if fn:
    fn.setName('CreateSimple', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureAppearance', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureAppearance', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48B9D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F6360))
  if fn:
    fn.setName('UpdateSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F6360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F5970))
  if fn:
    fn.setName('SetAnimationInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F5970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F7BB0))
  if fn:
    fn.setName('LoadModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseTW', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseTW', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F7BB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C9E40))
  if fn:
    fn.setName('Lean', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C9E40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5383C0))
  if fn:
    fn.setName('GetCenterPoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5383C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41DE90))
  if fn:
    fn.setName('RemoveBumpedOutShell', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41DE90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x504FD0))
  if fn:
    fn.setName('AttachVFXModelToTargetModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x504FD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4BD180))
  if fn:
    fn.setName('ApplyEnvironmentMap', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BD180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41FAC0))
  if fn:
    fn.setName('ApplyModelAttachment', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41FAC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5822A0))
  if fn:
    fn.setName('Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5822A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B2EA0))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBarkBubble', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBarkBubble', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B2EA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B33F0))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBarkBubble', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBarkBubble', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B33F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59B250))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59B250: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5C0340))
  if fn:
    fn.setName('CSWGuiStoreItemEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStoreItemEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStoreItemEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5C0340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A4960))
  if fn:
    fn.setName('CSWGuiInGameItemEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameItemEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameItemEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A4960: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A40B0))
  if fn:
    fn.setName('AddItemEntryToList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A40B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AE040))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGamePause', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGamePause', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AE040: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A9350))
  if fn:
    fn.setName('PopulateStore', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A9350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x52CED0))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgradeItemSelect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgradeItemSelect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x52CED0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5BFED0))
  if fn:
    fn.setName('InsertUpgrade', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgrade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5BFED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5BFE20))
  if fn:
    fn.setName('VerifyUpgrade', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgrade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5BFE20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5C0150))
  if fn:
    fn.setName('CancelPanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgrade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5C0150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D4250))
  if fn:
    fn.setName('RemoveGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveLoad', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveLoad', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D4250: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D5140))
  if fn:
    fn.setName('CSWGuiSaveLoadEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveLoadEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveLoadEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D5140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B3580))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiExamine', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiExamine', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B3580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B4720))
  if fn:
    fn.setName('Back', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersFeatsSkillsDebugMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersFeatsSkillsDebugMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B4720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B61B0))
  if fn:
    fn.setName('Back', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLoadModuleDebugMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLoadModuleDebugMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B61B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B67C0))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCreateItemMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCreateItemMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B67C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B6940))
  if fn:
    fn.setName('Back', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCreateItemMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCreateItemMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B6940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B9740))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiQuestItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiQuestItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B9740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x56DE50))
  if fn:
    fn.setName('CSWGuiQuestItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiQuestItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiQuestItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x56DE50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x57FF10))
  if fn:
    fn.setName('SetBehaviorListBounce', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCProjectile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x57FF10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5843D0))
  if fn:
    fn.setName('UpdateInstancePosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5843D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x584A20))
  if fn:
    fn.setName('SetPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x584A20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DEA60))
  if fn:
    fn.setName('OnSelectButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiClassSelection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiClassSelection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DEA60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DE5B0))
  if fn:
    fn.setName('CSWGuiClassSelChar', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiClassSelChar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiClassSelChar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DE5B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B9B10))
  if fn:
    fn.setName('SetCreatureID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScriptSelect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScriptSelect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B9B10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F8A10))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScriptSelect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScriptSelect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F8A10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5E1D10))
  if fn:
    fn.setName('CreateOption', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScriptSelect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScriptSelect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E1D10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EDB00))
  if fn:
    fn.setName('SetVisiblePanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EDB00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EDD60))
  if fn:
    fn.setName('PortraitChanged', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EDD60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F0B20))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiKeyMapButton', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiKeyMapButton', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F0B20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F13D0))
  if fn:
    fn.setName('ResetToDefault', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiKeyMapButton', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiKeyMapButton', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F13D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F00C0))
  if fn:
    fn.setName('OnAcceptClick', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameOptKeyMappings', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameOptKeyMappings', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F00C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EFD90))
  if fn:
    fn.setName('UpdateEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameOptKeyMappings', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameOptKeyMappings', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EFD90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F0700))
  if fn:
    fn.setName('UpdateEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiKeyMapButton', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiKeyMapButton', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F0700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F2F10))
  if fn:
    fn.setName('OnSelectFeatsButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F2F10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F31C0))
  if fn:
    fn.setName('AbilitySelectionCompleted', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F31C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F3360))
  if fn:
    fn.setName('OnCancelPressed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F3360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F4070))
  if fn:
    fn.setName('OnSelectPortraitButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCustomPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCustomPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F4070: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F53E0))
  if fn:
    fn.setName('OnSelectPortraitButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiQuickPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiQuickPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F53E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x602D50))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiQuickOrCustomPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiQuickOrCustomPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x602D50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F6170))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F6170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FB220))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FB220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FA840))
  if fn:
    fn.setName('OnEnterFeat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FA840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FABE0))
  if fn:
    fn.setName('BuildButtons', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FABE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FEAC0))
  if fn:
    fn.setName('OnRecommendButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FEAC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FDCD0))
  if fn:
    fn.setName('OnPlusButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FDCD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5594D0))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiAbilitiesCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiAbilitiesCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5594D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63F6B0))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResLIP,3004>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResLIP,3004>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63F6B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x907D30))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResPLT,6>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResPLT,6>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x907D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x646B40))
  if fn:
    fn.setName('_CResHelper_CResTGA_3_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResTGA,3>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResTGA,3>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x646B40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x812F40))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResVIS,3001>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResVIS,3001>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x812F40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90AAC0))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResTPC,3007>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResTPC,3007>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90AAC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90AEC0))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResTXI,2022>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResTXI,2022>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90AEC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x622D50))
  if fn:
    fn.setName('_CResHelper_CResMDX_3008_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResMDX,3008>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResMDX,3008>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x622D50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62A850))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResMDL,2002>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResMDL,2002>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62A850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90A150))
  if fn:
    fn.setName('_CResHelper_CResDDS_2033_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResDDS,2033>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResDDS,2033>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90A150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x561CE0))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CRes4PC,2059>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CRes4PC,2059>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x561CE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x733860))
  if fn:
    fn.setName('_CResHelper', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResLTR,2036>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResLTR,2036>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x733860: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x919CC0))
  if fn:
    fn.setName('SetMaxValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiProgressBar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiProgressBar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x919CC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x919D60))
  if fn:
    fn.setName('SetStartFromLeft', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiProgressBar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiProgressBar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x919D60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91A770))
  if fn:
    fn.setName('setMaxValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScrollBar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScrollBar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91A770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91A820))
  if fn:
    fn.setName('setVisibleValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScrollBar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScrollBar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91A820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7785C0))
  if fn:
    fn.setName('SetPartyMemberWorldLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7785C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x778640))
  if fn:
    fn.setName('SetPartyMemberWorldOrientation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x778640: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x774FA0))
  if fn:
    fn.setName('ClearAttackData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatAttackData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatAttackData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x774FA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x775AC0))
  if fn:
    fn.setName('CalculateOnHandAttacks', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x775AC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7769A0))
  if fn:
    fn.setName('CalculateOffHandAttacks', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7769A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6F8880))
  if fn:
    fn.setName('SetForceResistancePenalty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F8880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6E10B0))
  if fn:
    fn.setName('GetFullName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6E10B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6F5C10))
  if fn:
    fn.setName('CanLevelUp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F5C10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F5CF0))
  if fn:
    fn.setName('LevelUp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F5CF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6E3610))
  if fn:
    fn.setName('UpdateCombatInformation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6E3610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x771850))
  if fn:
    fn.setName('GetBaseDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatAttackData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatAttackData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x771850: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x774C30))
  if fn:
    fn.setName('GetOffHandAttack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x774C30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6EA340))
  if fn:
    fn.setName('GetDamageRoll', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6EA340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x771810))
  if fn:
    fn.setName('SetBaseDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatAttackData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatAttackData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x771810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65F2A0))
  if fn:
    fn.setName('ResolveOnHitEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65F2A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65EFD0))
  if fn:
    fn.setName('ResolveOnHitVisuals', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65EFD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6983A0))
  if fn:
    fn.setName('GetDamageFlags', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6983A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x771870))
  if fn:
    fn.setName('SetDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatAttackData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatAttackData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x771870: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x53A230))
  if fn:
    fn.setName('GetLastBulletHitShooter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGameObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGameObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x53A230: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x542C40))
  if fn:
    fn.setName('GetLastFollowerHit', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGameObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGameObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x542C40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55E780))
  if fn:
    fn.setName('GetObstacle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55E780: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55ED80))
  if fn:
    fn.setName('GetEnemy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55ED80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5DCA30))
  if fn:
    fn.setName('SetBulletTargetType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGGunBank', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGGunBank', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DCA30: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x639A30))
  if fn:
    fn.setName('GetOffset', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639A30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60BC50))
  if fn:
    fn.setName('SeekEnd', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60BC50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A2C0))
  if fn:
    fn.setName('FadeAndStop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A2C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A290))
  if fn:
    fn.setName('Stop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A290: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x430AA0))
  if fn:
    fn.setName('GetActiveTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x430AA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A020))
  if fn:
    fn.setName('_CExoStreamingSoundSource', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5A6580))
  if fn:
    fn.setName('SetContainer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiContainer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiContainer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A6580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AAC0))
  if fn:
    fn.setName('return_true', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AAC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40C530))
  if fn:
    fn.setName('GetQuestJournal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x552F50))
  if fn:
    fn.setName('StartNewGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x552F50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x554DD0))
  if fn:
    fn.setName('ReconnectSidedeckButtons', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x554DD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40DEC0))
  if fn:
    fn.setName('DelayNextEnemyAutoPause', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40DEC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x42BE70))
  if fn:
    fn.setName('IsStealthCapable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x42BE70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x42BD40))
  if fn:
    fn.setName('SetCombatMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x42BD40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8C35E0))
  if fn:
    fn.setName('CAurBehaviorAttach', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAurBehaviorAttach', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAurBehaviorAttach', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8C35E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A1E0))
  if fn:
    fn.setName('SetLooping', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A1E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A320))
  if fn:
    fn.setName('Set3D', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A320: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A350))
  if fn:
    fn.setName('SetDistance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x639D20))
  if fn:
    fn.setName('SetVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639D20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F8CB0))
  if fn:
    fn.setName('SetAnimationInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseTW', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseTW', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F8CB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5BE7B0))
  if fn:
    fn.setName('OnEnterSlot', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgrade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5BE7B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5853C0))
  if fn:
    fn.setName('SetInstanceRandomPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5853C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5841A0))
  if fn:
    fn.setName('SetAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5841A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x419070))
  if fn:
    fn.setName('SetAppearance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x419070: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80DBE0))
  if fn:
    fn.setName('Pitch', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80DBE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x616A30))
  if fn:
    fn.setName('SetCaptureKeyboardKeys', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInput', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x616A30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F0800))
  if fn:
    fn.setName('UpdateColor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiKeyMapButton', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiKeyMapButton', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F0800: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x603EE0))
  if fn:
    fn.setName('SetParentPanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPortraitCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPortraitCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x603EE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x589CC0))
  if fn:
    fn.setName('SetSkillStatusAll', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlowChart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlowChart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x589CC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x589C10))
  if fn:
    fn.setName('SetSkillStatus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlowChart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlowChart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x589C10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91DBD0))
  if fn:
    fn.setName('AddControls', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91DBD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x589D30))
  if fn:
    fn.setName('SetTopLeft', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlowChart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlowChart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x589D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x589B10))
  if fn:
    fn.setName('SetSelectedSkill', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlowChart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlowChart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x589B10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7783A0))
  if fn:
    fn.setName('GetMapPixelFromWorldCoord', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7783A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x754E50))
  if fn:
    fn.setName('SendDialogEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x754E50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7562C0))
  if fn:
    fn.setName('SendDialogReplies', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7562C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7764C0))
  if fn:
    fn.setName('GetCurrentAttackWeapon', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7764C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x63CCB0))
  if fn:
    fn.setName('Stop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63CCB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63AF20))
  if fn:
    fn.setName('_CExoStreamingSoundSourceInterna', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63AF20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AD1F0))
  if fn:
    fn.setName('GetModelName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AD1F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x606980))
  if fn:
    fn.setName('Right', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x606980: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5E2FA0))
  if fn:
    fn.setName('ClearGameBoard', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPazaak', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPazaak', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E2FA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54D260))
  if fn:
    fn.setName('ShowMultipleHelp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTutorial', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTutorial', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54D260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44AFA0))
  if fn:
    fn.setName('DelayNextEnemyAutoPause', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44AFA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40DD40))
  if fn:
    fn.setName('SetCombatMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40DD40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63ED60))
  if fn:
    fn.setName('SetVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63ED60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C8C90))
  if fn:
    fn.setName('SetAnimationInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C8C90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x587C00))
  if fn:
    fn.setName('SetSkillStatus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlow', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlow', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x587C00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91F4F0))
  if fn:
    fn.setName('Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CSWGuiExtent>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CSWGuiExtent>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91F4F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x587B80))
  if fn:
    fn.setName('SetSkillStatus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlow', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlow', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x587B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x776680))
  if fn:
    fn.setName('GetWeaponAttackType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x776680: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91D110))
  if fn:
    fn.setName('OrganizeUnequal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91D110: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x704780))
  if fn:
    fn.setName('AutoLevelUpNPCs', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x704780: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x639040))
  if fn:
    fn.setName('InterruptPlayingStream', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639040: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x753B30))
  if fn:
    fn.setName('Destroy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoIniInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoIniInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x753B30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E40A0))
  if fn:
    fn.setName('SetCameraForScreenShot', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCameraOnAStick', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCameraOnAStick', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E40A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x592030))
  if fn:
    fn.setName('GetQuestEntryAt', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x592030: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5552D0))
  if fn:
    fn.setName('HandleTutorialClose', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5552D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x556550))
  if fn:
    fn.setName('CSWCAmbientSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x556550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F69E0))
  if fn:
    fn.setName('LoadModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseHead', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseHead', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F69E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5953D0))
  if fn:
    fn.setName('OnAbilitySelectionChanged', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAbilities', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAbilities', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5953D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5E7510))
  if fn:
    fn.setName('SetOrientation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseProjectile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseProjectile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E7510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EF550))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameOptKeyMappings', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameOptKeyMappings', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EF550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EFF00))
  if fn:
    fn.setName('IsKeyUsedAlready', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameOptKeyMappings', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameOptKeyMappings', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EFF00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FB3C0))
  if fn:
    fn.setName('OnFeatSelectionChanged', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FB3C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x604420))
  if fn:
    fn.setName('UpdatePortraitButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPortraitCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPortraitCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x604420: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x60D2D0))
  if fn:
    fn.setName('GetProperty0x4', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60D2D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x546D80))
  if fn:
    fn.setName('GetDoneEntryAt', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x546D80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x546D60))
  if fn:
    fn.setName('GetActiveEntryAt', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x546D60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x588010))
  if fn:
    fn.setName('SkillHitCheckMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlow', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlow', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x588010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x594720))
  if fn:
    fn.setName('OnEnterPower', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAbilities', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAbilities', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x594720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x595080))
  if fn:
    fn.setName('OnEnterFeat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAbilities', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAbilities', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x595080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F8B90))
  if fn:
    fn.setName('OnPowerSelectionChanged', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F8B90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F8C00))
  if fn:
    fn.setName('OnDoubleClick', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F8C00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F7E00))
  if fn:
    fn.setName('OnEnterPower', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F7E00: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4BCF10))
  if fn:
    fn.setName('GetProperty0xc', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BCF10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x606B10))
  if fn:
    fn.setName('SubString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x606B10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61A710))
  if fn:
    fn.setName('operator_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResRef', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61A710: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x606140))
  if fn:
    fn.setName('Find', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x606140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x495270))
  if fn:
    fn.setName('GetClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x495270: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6F7000))
  if fn:
    fn.setName('GetTotalEffectBonus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F7000: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x55F790))
  if fn:
    fn.setName('SetClipDist', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55F790: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x544F80))
  if fn:
    fn.setName('SetMinSpeed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x544F80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x545010))
  if fn:
    fn.setName('SetAPS', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x545010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x544FD0))
  if fn:
    fn.setName('SetMaxSpeed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x544FD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x544640))
  if fn:
    fn.setName('SetOffset', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x544640: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x545040))
  if fn:
    fn.setName('SetOrigin', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x545040: '+str(e)); errors += 1
print('Batch 3 done: renamed='+str(renamed)+' errors='+str(errors))