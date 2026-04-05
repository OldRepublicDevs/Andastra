from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0
try:
  fn = fm.getFunctionAt(toAddr(0x42BC30))
  if fn:
    fn.setName('GetCreaturePathfindInformation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x42BC30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x43AB70))
  if fn:
    fn.setName('AddSimpleObjectModelDirectlyToIgnoreList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x43AB70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x454660))
  if fn:
    fn.setName('SetWeather', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x454660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x474B00))
  if fn:
    fn.setName('AddFireForgetAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x474B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47E830))
  if fn:
    fn.setName('NVThunkSWCItmSWCObject_LoadModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47E830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48C1E0))
  if fn:
    fn.setName('GetAnimType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureAppearance', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureAppearance', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48C1E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x494DC0))
  if fn:
    fn.setName('GetPortrait', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x494DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E8110))
  if fn:
    fn.setName('UpdateCamraStyle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCameraFreeLook', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCameraFreeLook', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E8110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F3E80))
  if fn:
    fn.setName('HideHeadItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseHeadWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseHeadWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F3E80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x50F380))
  if fn:
    fn.setName('LoadFeatGain', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWClass', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWClass', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x50F380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x513BF0))
  if fn:
    fn.setName('SetFriend', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x513BF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5403B0))
  if fn:
    fn.setName('AddHintToMessageBuffer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLoadScreen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLoadScreen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5403B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x541250))
  if fn:
    fn.setName('Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x541250: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55C300))
  if fn:
    fn.setName('UpdateVolumeVariation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55C300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x562100))
  if fn:
    fn.setName('SpawnRoom', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x562100: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x56B430))
  if fn:
    fn.setName('OnAdvanced', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsGraphics', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsGraphics', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x56B430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x586860))
  if fn:
    fn.setName('SetMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTutorialBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTutorialBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x586860: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x589DF0))
  if fn:
    fn.setName('HandleInput', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlowChart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlowChart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x589DF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5994B0))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameCharacter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameCharacter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5994B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DB120))
  if fn:
    fn.setName('GetAnimationName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DB120: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EC150))
  if fn:
    fn.setName('Control', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAurBehaviorAttachTranslate', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAurBehaviorAttachTranslate', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EC150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F2E20))
  if fn:
    fn.setName('OnSelectPowersButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F2E20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F42F0))
  if fn:
    fn.setName('OnSelectFeatsButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCustomPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCustomPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F42F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x603F10))
  if fn:
    fn.setName('OnSelectDisplayButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPortraitCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPortraitCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x603F10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x604730))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPortraitCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPortraitCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x604730: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60CF00))
  if fn:
    fn.setName('Introduce', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStringList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStringList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60CF00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60DDA0))
  if fn:
    fn.setName('CExoTimersInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoTimersInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoTimersInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60DDA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60F8E0))
  if fn:
    fn.setName('ReadAsync', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFileInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFileInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60F8E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x611C00))
  if fn:
    fn.setName('CExoInputClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputClass', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputClass', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x611C00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x617080))
  if fn:
    fn.setName('HandleImeStatusChange', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoImeUI', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoImeUI', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x617080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AF60))
  if fn:
    fn.setName('AddResourceImageFilesAsync', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AF60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62B170))
  if fn:
    fn.setName('Create', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CERFFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CERFFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62B170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6380C0))
  if fn:
    fn.setName('LoadPriorityGroups', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6380C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C5C0))
  if fn:
    fn.setName('ClearAllQueuedCombatActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C5C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65AAE0))
  if fn:
    fn.setName('GetWeaponDischargeTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65AAE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65F950))
  if fn:
    fn.setName('ResolveDamageShields', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65F950: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x66C760))
  if fn:
    fn.setName('GetDamageTypeString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x66C760: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x693B40))
  if fn:
    fn.setName('GetFeatRemainingUses', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x693B40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FD5A0))
  if fn:
    fn.setName('GetPartyItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FD5A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x703650))
  if fn:
    fn.setName('AddGameInProgress', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x703650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7036E0))
  if fn:
    fn.setName('RemoveGameInProgress', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7036E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x722F60))
  if fn:
    fn.setName('EnforceNoPCHeadCloning', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x722F60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74C8D0))
  if fn:
    fn.setName('IsEffectToIgnoreOnDeath', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74C8D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74D050))
  if fn:
    fn.setName('CSWSDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74D050: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75DA10))
  if fn:
    fn.setName('CalculateNearestPoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75DA10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7672C0))
  if fn:
    fn.setName('LoadFromTemplate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7672C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C43D0))
  if fn:
    fn.setName('RemoveAttackBonus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C43D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F00E0))
  if fn:
    fn.setName('GetString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSScriptVarTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSScriptVarTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F00E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F6B70))
  if fn:
    fn.setName('LoadFromTemplate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F6B70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B0B0))
  if fn:
    fn.setName('SetWorldTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B0B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x811600))
  if fn:
    fn.setName('LoadNameTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWNameGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWNameGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x811600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x85A640))
  if fn:
    fn.setName('DoGobBuckets', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Scene', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Scene', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x85A640: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8904A0))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('PartAnimMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'PartAnimMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8904A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x89A850))
  if fn:
    fn.setName('RenderBlur', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Gob', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Gob', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x89A850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8AEED0))
  if fn:
    fn.setName('updateParticleFrame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('PartEmitter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'PartEmitter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8AEED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x409000))
  if fn:
    fn.setName('messagepump', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x409000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40CD90))
  if fn:
    fn.setName('AddObjectToHitcheckIgnoreList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CD90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40CDB0))
  if fn:
    fn.setName('RemoveObjectFromHitcheckIgnoreList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CDB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D090))
  if fn:
    fn.setName('GetHitCheckMaterialMask', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D090: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D8E0))
  if fn:
    fn.setName('SetCanSendDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D8E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D960))
  if fn:
    fn.setName('SetDialogModelsSent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D960: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40E7C0))
  if fn:
    fn.setName('GetDisplayCharacterForKey', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40E7C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x416C70))
  if fn:
    fn.setName('GetWayPoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x416C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41AC10))
  if fn:
    fn.setName('GetLastInstant', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41AC10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x42AC40))
  if fn:
    fn.setName('Setup3DScene', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x42AC40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x454600))
  if fn:
    fn.setName('StartSounds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x454600: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x475510))
  if fn:
    fn.setName('GetFinalFadeAlpha', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x475510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4776A0))
  if fn:
    fn.setName('ClearAllQueuedCombatActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4776A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48F020))
  if fn:
    fn.setName('ClearSkills', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48F020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x496520))
  if fn:
    fn.setName('CSWCMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x496520: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D4570))
  if fn:
    fn.setName('InitializeShotCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D4570: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DC4B0))
  if fn:
    fn.setName('SetPaused', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DC4B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DEE40))
  if fn:
    fn.setName('SetAreaTransition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DEE40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E15C0))
  if fn:
    fn.setName('FadeAndStopDialogAmbientTrack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E15C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4EADA0))
  if fn:
    fn.setName('GetModelResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWBaseItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWBaseItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4EADA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F0350))
  if fn:
    fn.setName('SetEnabled', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPlayerControl', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPlayerControl', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F0350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x510DC0))
  if fn:
    fn.setName('IsGrantedFeat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWClass', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWClass', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x510DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x520090))
  if fn:
    fn.setName('ShowActionQueue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x520090: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x549AC0))
  if fn:
    fn.setName('FindOptionIndex', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerInfo', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerInfo', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x549AC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x549B80))
  if fn:
    fn.setName('SetModuleName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerInfo', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerInfo', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x549B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54B460))
  if fn:
    fn.setName('ReadVOIDPtr', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54B460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54C140))
  if fn:
    fn.setName('CSWGuiTutorial', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTutorial', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTutorial', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54C140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x573A10))
  if fn:
    fn.setName('DetermineSafePosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWBehaviorCameraCombat', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWBehaviorCameraCombat', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x573A10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x57B560))
  if fn:
    fn.setName('SetProjectileVelAndAccel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCProjectile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x57B560: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5873D0))
  if fn:
    fn.setName('CSWGuiSkillFlow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillFlow', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillFlow', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5873D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59BF40))
  if fn:
    fn.setName('SetTopVisible', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59BF40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62FC90))
  if fn:
    fn.setName('BuildNewTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoKeyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62FC90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x638930))
  if fn:
    fn.setName('LoadProviderPreferences', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x638930: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63CF80))
  if fn:
    fn.setName('Update3DStream', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63CF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x642870))
  if fn:
    fn.setName('CVirtualMachineScript', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineScript', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineScript', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x642870: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x647090))
  if fn:
    fn.setName('Shutdown', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CPathfindInformation', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CPathfindInformation', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x647090: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64BE20))
  if fn:
    fn.setName('GetDebugMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BE20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C210))
  if fn:
    fn.setName('GetActiveExclusionList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65DF60))
  if fn:
    fn.setName('ResolveDefensiveEffects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65DF60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x667090))
  if fn:
    fn.setName('UpdateUsedActiveProperties', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x667090: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x66AFC0))
  if fn:
    fn.setName('GetPropertyDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x66AFC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67CF00))
  if fn:
    fn.setName('ComputeSpellRange', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67CF00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AD730))
  if fn:
    fn.setName('IsSoundPlayingInDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AD730: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DB480))
  if fn:
    fn.setName('CanUnEquipWeapon', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DB480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F6D70))
  if fn:
    fn.setName('GetIsInSpellLikeAbilityList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F6D70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F9C90))
  if fn:
    fn.setName('GetSpellLikeAbilityCasterLevel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F9C90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FDA30))
  if fn:
    fn.setName('UpdateMembers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FDA30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x706AC0))
  if fn:
    fn.setName('ComputeNonVisibleLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x706AC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x707350))
  if fn:
    fn.setName('IntersectingLineSegment', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x707350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x714000))
  if fn:
    fn.setName('ComputeClippedLineSegment', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRoomSurfaceMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRoomSurfaceMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x714000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7184D0))
  if fn:
    fn.setName('IncrementPlayersInArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7184D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7221C0))
  if fn:
    fn.setName('NoNonWalkPolys', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7221C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74CA90))
  if fn:
    fn.setName('AddUnique', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<LARGE_INTEGER>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<LARGE_INTEGER>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74CA90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x778730))
  if fn:
    fn.setName('IsWorldPointExplored', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x778730: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x778B80))
  if fn:
    fn.setName('GetQuestEntryXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x778B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x778BF0))
  if fn:
    fn.setName('GetState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x778BF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C15D0))
  if fn:
    fn.setName('AddItemToInventory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C15D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CD8A0))
  if fn:
    fn.setName('SendServerToPlayerLogin_NeedCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CD8A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CE400))
  if fn:
    fn.setName('SendServerToPlayerCharList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CE400: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E3B90))
  if fn:
    fn.setName('SendServerToPlayerAmbientSoundVolumeChange', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E3B90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E75E0))
  if fn:
    fn.setName('SetShape', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E75E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EF670))
  if fn:
    fn.setName('LoadReputations', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CFactionManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CFactionManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EF670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F1220))
  if fn:
    fn.setName('DeleteList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSExpressionList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSExpressionList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F1220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F39B0))
  if fn:
    fn.setName('_CSWLevelStats', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWLevelStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWLevelStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F39B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B310))
  if fn:
    fn.setName('ResetTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80B580))
  if fn:
    fn.setName('UnpauseWorldTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CWorldTimer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80B580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x868320))
  if fn:
    fn.setName('dofile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x868320: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8D5470))
  if fn:
    fn.setName('NewCAurGUIImage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8D5470: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8F5230))
  if fn:
    fn.setName('ReplaceTexture', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Material', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Material', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8F5230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9147A0))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiImage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiImage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9147A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91AB60))
  if fn:
    fn.setName('HitCheckScrollbar', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScrollBar', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScrollBar', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91AB60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x410200))
  if fn:
    fn.setName('CSWCCreaturePartyStatus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreaturePartyStatus', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreaturePartyStatus', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x410200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4771C0))
  if fn:
    fn.setName('NewVocalization', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4771C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D1300))
  if fn:
    fn.setName('SetReplyData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D1300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D1550))
  if fn:
    fn.setName('GetCameraAnimationName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D1550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4ECC30))
  if fn:
    fn.setName('CSWSpell', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSpell', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSpell', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4ECC30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5068C0))
  if fn:
    fn.setName('CSWCDoor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5068C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5347D0))
  if fn:
    fn.setName('CSWCPlaceable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5347D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5371E0))
  if fn:
    fn.setName('SetAppearance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5371E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5380E0))
  if fn:
    fn.setName('SetCursor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5380E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x53ECD0))
  if fn:
    fn.setName('GetTokenPair', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientMsgSysAdmin', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientMsgSysAdmin', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x53ECD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55AA20))
  if fn:
    fn.setName('CSWCSoundObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55AA20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61D550))
  if fn:
    fn.setName('ServiceFromImage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61D550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62C910))
  if fn:
    fn.setName('CExoKeyTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoKeyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62C910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x633790))
  if fn:
    fn.setName('CExoArrayList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CExoString>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CExoString>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x633790: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x638020))
  if fn:
    fn.setName('AddToNonPlayingStreamingLoopingList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x638020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E6110))
  if fn:
    fn.setName('Initialise', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E6110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F3B00))
  if fn:
    fn.setName('SetSkillRankChange', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWLevelStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWLevelStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F3B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40EC10))
  if fn:
    fn.setName('StringCopy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40EC10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C1A40))
  if fn:
    fn.setName('HandleServerToPlayerEndGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C1A40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x640F80))
  if fn:
    fn.setName('_CScriptCompilerIncludeFileStackEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptCompilerIncludeFileStackEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptCompilerIncludeFileStackEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640F80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73F2A0))
  if fn:
    fn.setName('OnRemoveArcaneSpellFailure', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73F2A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7B4920))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7B4920: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x878D00))
  if fn:
    fn.setName('GetSkinSphereMapVP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x878D00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8D7FC0))
  if fn:
    fn.setName('AurResGetLast', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8D7FC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D0B0))
  if fn:
    fn.setName('SetVideoMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D0B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40DF30))
  if fn:
    fn.setName('DoVFXRumble', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40DF30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x445190))
  if fn:
    fn.setName('AddMoveToModuleMovie', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x445190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x454D00))
  if fn:
    fn.setName('SetFog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x454D00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4581E0))
  if fn:
    fn.setName('setstrength', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4581E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x458680))
  if fn:
    fn.setName('revealmap', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x458680: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x472AB0))
  if fn:
    fn.setName('SetLoopingAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x472AB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x475420))
  if fn:
    fn.setName('RemoveAllFireForgetAnimations', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x475420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x477070))
  if fn:
    fn.setName('SetGroundNormal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x477070: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4773C0))
  if fn:
    fn.setName('QueuedCallbackFunction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4773C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x477520))
  if fn:
    fn.setName('ClientServerSyncHandler_Animation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x477520: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F4DE0))
  if fn:
    fn.setName('NVThunkHeadWieldAnimBase_ShowHeadItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F4DE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F4E10))
  if fn:
    fn.setName('NVThunkHeadWieldAnimBase_SetAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F4E10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5999F0))
  if fn:
    fn.setName('CameraFreeStyleControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCameraOnAStick', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCameraOnAStick', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5999F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60CC30))
  if fn:
    fn.setName('FlushScriptLogFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoDebug', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoDebug', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60CC30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63F130))
  if fn:
    fn.setName('HurryDeathSequence', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63F130: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64A240))
  if fn:
    fn.setName('IsPathClear', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAvoidCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAvoidCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64A240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C8D0))
  if fn:
    fn.setName('GetNormalOfPoint', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C8D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6699D0))
  if fn:
    fn.setName('GetItemCount', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6699D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67D2F0))
  if fn:
    fn.setName('AIActionCheckForceFollowObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67D2F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B5880))
  if fn:
    fn.setName('ClearActivities', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B5880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6C9D80))
  if fn:
    fn.setName('AIActionAreaWait', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6C9D80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CA150))
  if fn:
    fn.setName('WriteGameObjUpdate_WorkRemaining', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CA150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F61A0))
  if fn:
    fn.setName('CSWSSoundObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F61A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x843670))
  if fn:
    fn.setName('InternalPostProcess', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('MdlNodeEmitter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'MdlNodeEmitter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x843670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x84C420))
  if fn:
    fn.setName('GetCutsceneDummyPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Gob', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Gob', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x84C420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8F3CDB))
  if fn:
    fn.setName('Catch_0047bffd', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8F3CDB: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x907780))
  if fn:
    fn.setName('GetProcessedTextureDataSize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAuroraProcessedTexture', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAuroraProcessedTexture', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x907780: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x910360))
  if fn:
    fn.setName('HideAllPanels', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x910360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91B5A0))
  if fn:
    fn.setName('SetSelectOnLeftClick', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91B5A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41CEC0))
  if fn:
    fn.setName('SetPortraitId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41CEC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x42C320))
  if fn:
    fn.setName('CSWLevelStats', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWLevelStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWLevelStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x42C320: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x456350))
  if fn:
    fn.setName('DeleteCurrentAIAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x456350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x51C2C0))
  if fn:
    fn.setName('_CScriptCompilerIdListEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptCompilerIdListEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptCompilerIdListEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x51C2C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x53A5F0))
  if fn:
    fn.setName('LoadScripts', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGameObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGameObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x53A5F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x58A510))
  if fn:
    fn.setName('_CSWGuiUpgrade', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgrade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x58A510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x639A50))
  if fn:
    fn.setName('CExoSoundSource', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639A50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x730CA0))
  if fn:
    fn.setName('NWPlayerCharacterList_st', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('NWPlayerCharacterList_st', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'NWPlayerCharacterList_st', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x730CA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75CB00))
  if fn:
    fn.setName('GetPlayerName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75CB00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ED360))
  if fn:
    fn.setName('ReadEncounterScriptsFromGff', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ED360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x846160))
  if fn:
    fn.setName('ParseFieldDispatch', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('MdlNode', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'MdlNode', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x846160: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x914FA0))
  if fn:
    fn.setName('operator_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTextParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTextParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x914FA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92BA03))
  if fn:
    fn.setName('_FindAndUnlinkFrame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92BA03: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40B8D0))
  if fn:
    fn.setName('toggleFullScreen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40B8D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40CB60))
  if fn:
    fn.setName('SetPauseState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CB60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D000))
  if fn:
    fn.setName('StopServices', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D110))
  if fn:
    fn.setName('GetItemDropSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D5B0))
  if fn:
    fn.setName('PlayLoadModuleMovies', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D5B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41F7E0))
  if fn:
    fn.setName('SetSpecialWalkAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41F7E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41F810))
  if fn:
    fn.setName('SetRunLooksAwful', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41F810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x456260))
  if fn:
    fn.setName('SetLightningDone', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x456260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x474890))
  if fn:
    fn.setName('AnimationPlayOutOfPlace', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x474890: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x477390))
  if fn:
    fn.setName('GetFeedbackInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x477390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54D4E0))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54D4E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55B710))
  if fn:
    fn.setName('ChangeVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55B710: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61B890))
  if fn:
    fn.setName('Exists', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61B890: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6271C0))
  if fn:
    fn.setName('CEncounterListEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CEncounterListEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CEncounterListEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6271C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x630390))
  if fn:
    fn.setName('GetTableIndex', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoKeyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x630390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x637A70))
  if fn:
    fn.setName('GetNext', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLinkedList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLinkedList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x637A70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A0E0))
  if fn:
    fn.setName('GetPlayFailed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A0E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63AAA0))
  if fn:
    fn.setName('StopAllOneShots', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63AAA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63F620))
  if fn:
    fn.setName('DoObstructionWork', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63F620: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64BF40))
  if fn:
    fn.setName('LoadModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BF40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64BFE0))
  if fn:
    fn.setName('StartServices', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BFE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C070))
  if fn:
    fn.setName('InitiateModuleForPlayer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C070: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C230))
  if fn:
    fn.setName('ResolvePlayerByFirstName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64CAD0))
  if fn:
    fn.setName('SetSaveGameName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64CAD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x650A80))
  if fn:
    fn.setName('CSWGlobalVariableTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGlobalVariableTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGlobalVariableTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x650A80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67A880))
  if fn:
    fn.setName('PutItemInSlot', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67A880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A3B70))
  if fn:
    fn.setName('RemovePazaakCard', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A3B70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AB6F0))
  if fn:
    fn.setName('RemoveSomeEffectsOfDurationType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AB6F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DD5D0))
  if fn:
    fn.setName('AIActionSurrenderToEnemies', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DD5D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x723E00))
  if fn:
    fn.setName('GetVertex', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCollisionMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCollisionMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x723E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x771220))
  if fn:
    fn.setName('CSWSSpellScriptData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSpellScriptData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSpellScriptData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x771220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CA6E0))
  if fn:
    fn.setName('ComputeAppearanceUpdateRequired', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CA6E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E6010))
  if fn:
    fn.setName('GetFactionMember', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E6010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F79D0))
  if fn:
    fn.setName('CResBWM', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResBWM', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResBWM', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F79D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F7AF0))
  if fn:
    fn.setName('IsBinary', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResBWM', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResBWM', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F7AF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x809D00))
  if fn:
    fn.setName('ParseToken', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x809D00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8630E0))
  if fn:
    fn.setName('ProcessStaticObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8630E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x921FE0))
  if fn:
    fn.setName('inline_unlock_file', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x921FE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9224D0))
  if fn:
    fn.setName('inline_unlock_file', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9224D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x924711))
  if fn:
    fn.setName('inline_unlock_file', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x924711: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x926253))
  if fn:
    fn.setName('inline_unlock_12', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x926253: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x926E81))
  if fn:
    fn.setName('inline_unlock_12', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x926E81: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92888B))
  if fn:
    fn.setName('inline_unlock_file', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92888B: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92A42D))
  if fn:
    fn.setName('__isatty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92A42D: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92DC34))
  if fn:
    fn.setName('inline_unlock_fhandle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92DC34: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92E4E3))
  if fn:
    fn.setName('inline_unlock_fhandle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92E4E3: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92E811))
  if fn:
    fn.setName('inline_unlock_fhandle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92E811: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92EB91))
  if fn:
    fn.setName('inline_unlock_fhandle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92EB91: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9324E4))
  if fn:
    fn.setName('inline_conditional_unlock_0', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9324E4: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9327C6))
  if fn:
    fn.setName('inline_unlock_4', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9327C6: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9393E4))
  if fn:
    fn.setName('inline_unlock_fhandle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9393E4: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44DD60))
  if fn:
    fn.setName('ComputeLastUpdate_StoreUpdateSpellLikeAbility', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44DD60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59F490))
  if fn:
    fn.setName('GetSlotItemIdIndex', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59F490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61C890))
  if fn:
    fn.setName('Malloc', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61C890: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63A410))
  if fn:
    fn.setName('SetMusicDelay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63A410: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A6760))
  if fn:
    fn.setName('AddActionNodeParameter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A6760: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F2EA0))
  if fn:
    fn.setName('GetExpNeededForLevelUp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F2EA0: '+str(e)); errors += 1
print('Batch 1 done: renamed='+str(renamed)+' errors='+str(errors))