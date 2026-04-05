from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0
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
  fn = fm.getFunctionAt(toAddr(0x4A1640))
  if fn:
    fn.setName('HandleServerToPlayerPlaceableUpdate_Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A1640: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x505740))
  if fn:
    fn.setName('SendPlayerToServerInput_AbortDriveContro', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x505740: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5067A0))
  if fn:
    fn.setName('SendPlayerToServerInput_GiveItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5067A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x506010))
  if fn:
    fn.setName('SendPlayerToServerInput_TogglePauseRequest', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x506010: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x46C3E0))
  if fn:
    fn.setName('FindClearView', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x46C3E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4777B0))
  if fn:
    fn.setName('AttachmentFromServerObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4777B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4721B0))
  if fn:
    fn.setName('CSWCObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4721B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4776E0))
  if fn:
    fn.setName('GetServerObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4776E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5E02B0))
  if fn:
    fn.setName('ApplyShadowBlob', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E02B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x537DD0))
  if fn:
    fn.setName('AddToRenderList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x537DD0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x41D060))
  if fn:
    fn.setName('LoadBeam', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41D060: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x605810))
  if fn:
    fn.setName('GetNameText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWClass', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWClass', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x605810: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7238C0))
  if fn:
    fn.setName('ComputeMeshNormals', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCollisionMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCollisionMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7238C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x723B40))
  if fn:
    fn.setName('GetMeshOutline', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCollisionMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCollisionMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x723B40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x723EB0))
  if fn:
    fn.setName('LoadMesh', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCollisionMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCollisionMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x723EB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7286D0))
  if fn:
    fn.setName('LocalToWorld', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCollisionMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCollisionMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7286D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x651830))
  if fn:
    fn.setName('GetValueLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGlobalVariableTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGlobalVariableTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x651830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x651DC0))
  if fn:
    fn.setName('GetValueString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGlobalVariableTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGlobalVariableTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x651DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6510D0))
  if fn:
    fn.setName('Hash', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGlobalVariableTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGlobalVariableTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6510D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x650D00))
  if fn:
    fn.setName('ReadCatalogue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGlobalVariableTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGlobalVariableTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x650D00: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6529C0))
  if fn:
    fn.setName('ReadTableWithCatalogue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGlobalVariableTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGlobalVariableTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6529C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x651580))
  if fn:
    fn.setName('SetBit', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGlobalVariableTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGlobalVariableTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x651580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x651C90))
  if fn:
    fn.setName('SetValueLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGlobalVariableTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGlobalVariableTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x651C90: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x912E20))
  if fn:
    fn.setName('CSWGuiBorder', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBorder', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBorder', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x912E20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x913450))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBorder', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBorder', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x913450: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9127A0))
  if fn:
    fn.setName('CSWGuiBorderParams', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBorderParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBorderParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9127A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x912CF0))
  if fn:
    fn.setName('SetBorder', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBorderParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBorderParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x912CF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x912A50))
  if fn:
    fn.setName('SetCornerImage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBorderParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBorderParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x912A50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x912B30))
  if fn:
    fn.setName('SetEdgeImage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBorderParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBorderParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x912B30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x912C10))
  if fn:
    fn.setName('SetFillImage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBorderParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBorderParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x912C10: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5A70A0))
  if fn:
    fn.setName('GiveItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiContainer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiContainer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A70A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x916C30))
  if fn:
    fn.setName('CSWGuiControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiControl', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiControl', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x916C30: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x917090))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiControl', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiControl', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x917090: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x916050))
  if fn:
    fn.setName('AddNewChar', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiEditText', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiEditText', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x916050: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x651960))
  if fn:
    fn.setName('RemoveLastChar', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiEditText', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiEditText', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x651960: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x916250))
  if fn:
    fn.setName('SetText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiEditText', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiEditText', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x916250: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x914AF0))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiImage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiImage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x914AF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5AD000))
  if fn:
    fn.setName('SetTransitionObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAreaTransition', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAreaTransition', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AD000: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5A57E0))
  if fn:
    fn.setName('GetNumItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameItemEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameItemEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5A57E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x590190))
  if fn:
    fn.setName('CSWGuiInGameJournal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x590190: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x91DF70))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91DF70: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x523D50))
  if fn:
    fn.setName('SelectPrevPersonalAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x523D50: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x51FF70))
  if fn:
    fn.setName('ShowActionIcon', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x51FF70: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x911AD0))
  if fn:
    fn.setName('ActivateSoftwareMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x911AD0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5AFD20))
  if fn:
    fn.setName('DrawSafeRegion', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AFD20: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x912020))
  if fn:
    fn.setName('HandleKeyPress', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x912020: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x911EC0))
  if fn:
    fn.setName('HideSoftwareMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x911EC0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x9103D0))
  if fn:
    fn.setName('LoadGuiSounds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9103D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x915270))
  if fn:
    fn.setName('PanelExists', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x915270: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x911F00))
  if fn:
    fn.setName('SetSoftwareMouseCursor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x911F00: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x911EE0))
  if fn:
    fn.setName('ShowSoftwareMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x911EE0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x52CED0))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMessageBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMessageBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x52CED0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x52D0E0))
  if fn:
    fn.setName('SetIcon', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMessageBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMessageBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x52D0E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x90D2C0))
  if fn:
    fn.setName('StartLoadFromLayout', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90D2C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x58D650))
  if fn:
    fn.setName('SetForcedNPC', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPartySelection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPartySelection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x58D650: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x54E5B0))
  if fn:
    fn.setName('CSWGuiPazaakStart', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakStart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakStart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54E5B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5D0D50))
  if fn:
    fn.setName('PopulateGameList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveLoad', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveLoad', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D0D50: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5D6970))
  if fn:
    fn.setName('GetGameDirectory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveLoadEntry', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveLoadEntry', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D6970: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x916720))
  if fn:
    fn.setName('AddModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScene', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScene', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x916720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x916310))
  if fn:
    fn.setName('CSWGuiScene', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScene', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScene', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x916310: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x90DA20))
  if fn:
    fn.setName('SetCurValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSlider', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSlider', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90DA20: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x915850))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiText', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiText', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x915850: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x67A600))
  if fn:
    fn.setName('GetSlot', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67A600: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5EB2C0))
  if fn:
    fn.setName('Control', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGBehaviorTrack', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGBehaviorTrack', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EB2C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EBEB0))
  if fn:
    fn.setName('Control', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGBehaviorTrackAndFire', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGBehaviorTrackAndFire', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EBEB0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x54B6C0))
  if fn:
    fn.setName('ExtendWriteBuffer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54B6C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5626C0))
  if fn:
    fn.setName('WriteBOOL', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5626C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x544160))
  if fn:
    fn.setName('KeepInTunnel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x544160: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x65AD90))
  if fn:
    fn.setName('GetIsLeader', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65AD90: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7046B0))
  if fn:
    fn.setName('GetPlanetSelectable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7046B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x704680))
  if fn:
    fn.setName('SetNPCSelectability', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x704680: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x700CE0))
  if fn:
    fn.setName('SetPlanetAvailable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x700CE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7046E0))
  if fn:
    fn.setName('SetPlanetSelectable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7046E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x703960))
  if fn:
    fn.setName('SetReturnStrref', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x703960: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7048A0))
  if fn:
    fn.setName('SetSoloMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7048A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4F2EB0))
  if fn:
    fn.setName('GetMaxSpeed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPlayerControlCamRelative', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPlayerControlCamRelative', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F2EB0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x401880))
  if fn:
    fn.setName('CSWReentrantServerStats', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWReentrantServerStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWReentrantServerStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x401880: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5623B0))
  if fn:
    fn.setName('SetRoomInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5623B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x715890))
  if fn:
    fn.setName('ComputeHeight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRoomSurfaceMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRoomSurfaceMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x715890: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x714580))
  if fn:
    fn.setName('FindTriangle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRoomSurfaceMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRoomSurfaceMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x714580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x715180))
  if fn:
    fn.setName('GetEdgeVertices', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRoomSurfaceMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRoomSurfaceMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x715180: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7153C0))
  if fn:
    fn.setName('SetRoomAdjacency', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRoomSurfaceMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRoomSurfaceMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7153C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x715290))
  if fn:
    fn.setName('WeldEdgeVertices', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRoomSurfaceMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRoomSurfaceMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x715290: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x50E730))
  if fn:
    fn.setName('GetFeat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRules', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRules', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x50E730: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x717010))
  if fn:
    fn.setName('AIUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x717010: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x717DA0))
  if fn:
    fn.setName('AddObjectToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x717DA0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x722CF0))
  if fn:
    fn.setName('AttachmentFromClientArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x722CF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x722DE0))
  if fn:
    fn.setName('AwardStealthXP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x722DE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x721AF0))
  if fn:
    fn.setName('BudgeCreatures', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x721AF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7054D0))
  if fn:
    fn.setName('ComputeHeight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7054D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x706160))
  if fn:
    fn.setName('ComputeSafeLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x706160: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x781130))
  if fn:
    fn.setName('CountAreaTransitionTriggers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x781130: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x718550))
  if fn:
    fn.setName('DecreaseAILevelPriority', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x718550: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x705570))
  if fn:
    fn.setName('GetRoom', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x705570: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x718750))
  if fn:
    fn.setName('IncreaseAILevelPriority', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x718750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x71AB60))
  if fn:
    fn.setName('LoadRooms', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x71AB60: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x720890))
  if fn:
    fn.setName('PlayVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x720890: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x70D510))
  if fn:
    fn.setName('PlotPathPointPath', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x70D510: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x718240))
  if fn:
    fn.setName('RemoveObjectFromArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x718240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x71F9F0))
  if fn:
    fn.setName('SaveAreaEffects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x71F9F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x71F7B0))
  if fn:
    fn.setName('SaveCreatures', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x71F7B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x71FD20))
  if fn:
    fn.setName('SaveDoors', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x71FD20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x71FF40))
  if fn:
    fn.setName('SaveEncounters', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x71FF40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x71EF80))
  if fn:
    fn.setName('SaveGIT', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x71EF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x71FB00))
  if fn:
    fn.setName('SaveItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x71FB00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x71FC10))
  if fn:
    fn.setName('SavePlaceables', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x71FC10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x720280))
  if fn:
    fn.setName('SaveProperties', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x720280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x720060))
  if fn:
    fn.setName('SaveSounds', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x720060: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x71FE30))
  if fn:
    fn.setName('SaveStores', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x71FE30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x71F8E0))
  if fn:
    fn.setName('SaveWaypoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x71F8E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x721900))
  if fn:
    fn.setName('SetCurrentWeather', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x721900: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x708410))
  if fn:
    fn.setName('ShrinkLineByCreatures', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x708410: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x778100))
  if fn:
    fn.setName('LoadSavedAreaMapData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x778100: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7E69D0))
  if fn:
    fn.setName('AddToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E69D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E6190))
  if fn:
    fn.setName('CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E6190: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7E7D60))
  if fn:
    fn.setName('LoadEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E7D60: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7E7710))
  if fn:
    fn.setName('SetTargetObjID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E7710: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x771C90))
  if fn:
    fn.setName('LoadData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatAttackData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatAttackData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x771C90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7719A0))
  if fn:
    fn.setName('SaveData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatAttackData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatAttackData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7719A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7741F0))
  if fn:
    fn.setName('DecrementPauseTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7741F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x773040))
  if fn:
    fn.setName('EndCombatRound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x773040: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x776EF0))
  if fn:
    fn.setName('LoadCombatRound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x776EF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x771100))
  if fn:
    fn.setName('LoadData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRoundAction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRoundAction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x771100: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x771010))
  if fn:
    fn.setName('SaveData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRoundAction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRoundAction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x771010: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6BF180))
  if fn:
    fn.setName('AIActionDisarmMine', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6BF180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6C9DC0))
  if fn:
    fn.setName('AIActionRandomWalk', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6C9DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6BE140))
  if fn:
    fn.setName('AIActionRecoverMine', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6BE140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6C8920))
  if fn:
    fn.setName('AcquireItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6C8920: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x774310))
  if fn:
    fn.setName('ActivityManager', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x774310: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x696000))
  if fn:
    fn.setName('AddCounterSpellActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x696000: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x680B30))
  if fn:
    fn.setName('AddDropItemActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x680B30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67ED60))
  if fn:
    fn.setName('AddEquipItemActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67ED60: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6A39B0))
  if fn:
    fn.setName('AddPazaakCard', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A39B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x680A70))
  if fn:
    fn.setName('AddPickUpItemActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x680A70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A4470))
  if fn:
    fn.setName('AddPureEvilPowers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A4470: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A4360))
  if fn:
    fn.setName('AddPureGoodPowers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A4360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DB8D0))
  if fn:
    fn.setName('AddToInvitationsIgnored', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DB8D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DEF90))
  if fn:
    fn.setName('AddToInvitationsOffered', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DEF90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6800C0))
  if fn:
    fn.setName('AddUnequipActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6800C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6963E0))
  if fn:
    fn.setName('AddUseTalentOnObjectActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6963E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x692330))
  if fn:
    fn.setName('BroadcastAttackSummaryToParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x692330: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x692830))
  if fn:
    fn.setName('BroadcastBattleCry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x692830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x693760))
  if fn:
    fn.setName('BroadcastCombatStateToParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x693760: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x692F50))
  if fn:
    fn.setName('BroadcastDamageDataToParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x692F50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6920C0))
  if fn:
    fn.setName('BroadcastDeathDataToParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6920C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x692C30))
  if fn:
    fn.setName('BroadcastFloatyData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x692C30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x692420))
  if fn:
    fn.setName('BroadcastMineDetectionData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x692420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x692540))
  if fn:
    fn.setName('BroadcastSpellCast', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x692540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x692AF0))
  if fn:
    fn.setName('BroadcastStealthDetectionData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x692AF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6929A0))
  if fn:
    fn.setName('BroadcastVoiceChat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6929A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x69F650))
  if fn:
    fn.setName('BumpFriends', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69F650: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6C8460))
  if fn:
    fn.setName('CanEquipItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6C8460: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x687250))
  if fn:
    fn.setName('ClearAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x687250: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6DC5D0))
  if fn:
    fn.setName('ComputeModifiedMovementRate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DC5D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x69DC00))
  if fn:
    fn.setName('Disguise', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69DC00: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x68F8F0))
  if fn:
    fn.setName('DoPerceptionUpdateOnCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x68F8F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6D95F0))
  if fn:
    fn.setName('DriveUpdateLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6D95F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x699430))
  if fn:
    fn.setName('EquipMostDamagingRangedWeapon', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x699430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x761BC0))
  if fn:
    fn.setName('EvaluateLock', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x761BC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A4080))
  if fn:
    fn.setName('ForceEquipClothing', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A4080: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6DB920))
  if fn:
    fn.setName('GetCreatureReputation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DB920: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x698DF0))
  if fn:
    fn.setName('GetDetectMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x698DF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x699E10))
  if fn:
    fn.setName('GetGold', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x699E10: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x69F420))
  if fn:
    fn.setName('GetIsCreatureBumpable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69F420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DF230))
  if fn:
    fn.setName('GetIsInInvitationsOffered', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DF230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x69E7A0))
  if fn:
    fn.setName('GetIsInUseRange', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69E7A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x696F20))
  if fn:
    fn.setName('GetItemCount', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x696F20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6973F0))
  if fn:
    fn.setName('GetMaxHitPoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6973F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x694970))
  if fn:
    fn.setName('GetNearestObjects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x694970: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x68EC30))
  if fn:
    fn.setName('GetRunRate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x68EC30: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x69E9F0))
  if fn:
    fn.setName('GetUseRange', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69E9F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6DC370))
  if fn:
    fn.setName('GetVisibleListElement', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DC370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x68E920))
  if fn:
    fn.setName('GetWalkRate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x68E920: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x65A650))
  if fn:
    fn.setName('GetWeaponDischargeRow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65A650: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x684CB0))
  if fn:
    fn.setName('MergeItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x684CB0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x69EFB0))
  if fn:
    fn.setName('PacifyCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69EFB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A2710))
  if fn:
    fn.setName('ProcessPendingCombatActions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A2710: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x686E80))
  if fn:
    fn.setName('RemoveFromArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x686E80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6C8C00))
  if fn:
    fn.setName('RemoveItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6C8C00: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6DEE80))
  if fn:
    fn.setName('ReplyToInvitation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DEE80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x67F280))
  if fn:
    fn.setName('RunEquip', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67F280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x680300))
  if fn:
    fn.setName('RunUnequip', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x680300: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x693090))
  if fn:
    fn.setName('SetCombatState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x693090: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x695EA0))
  if fn:
    fn.setName('SetLockOrientationToObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x695EA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F89F0))
  if fn:
    fn.setName('SetMovementRateFactor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F89F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x697E50))
  if fn:
    fn.setName('SetPortraitId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x697E50: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x684E10))
  if fn:
    fn.setName('SplitItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x684E10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DFDC0))
  if fn:
    fn.setName('SurrenderToEnemies', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DFDC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x69DDF0))
  if fn:
    fn.setName('UnDisguise', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69DDF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6E0170))
  if fn:
    fn.setName('UnsummonMyself', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6E0170: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x698A80))
  if fn:
    fn.setName('UpdateAppearanceDependantInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x698A80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6DBB50))
  if fn:
    fn.setName('UpdateVisibleList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DBB50: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6946C0))
  if fn:
    fn.setName('ValidNearestObjectType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6946C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6D61F0))
  if fn:
    fn.setName('WalkUpdateLocationDistance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6D61F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6D7490))
  if fn:
    fn.setName('WalkUpdateLocationTestDistance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6D7490: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6D2130))
  if fn:
    fn.setName('WalkUpdateLocation_QuickWalk_FollowLeader_FindPath', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6D2130: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6D39E0))
  if fn:
    fn.setName('WalkUpdateLocation_QuickWalk_FollowLeader_Following_Path', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6D39E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6D35A0))
  if fn:
    fn.setName('WalkUpdateLocation_QuickWalk_FollowLeader_Moving_To_Path', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6D35A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6D2890))
  if fn:
    fn.setName('WalkUpdateLocation_QuickWalk_FollowLeader_Offset', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6D2890: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6D5460))
  if fn:
    fn.setName('WalkUpdateLocation_QuickWalk_FollowLeader_WaitingForPath', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6D5460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6D0CB0))
  if fn:
    fn.setName('WalkUpdateLocation_QuickWalk_ForcePushed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6D0CB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x68DDF0))
  if fn:
    fn.setName('CSWSCreaturePartyFollowInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreaturePartyFollowInfo', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreaturePartyFollowInfo', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x68DDF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x68DE70))
  if fn:
    fn.setName('Clear', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreaturePartyFollowInfo', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreaturePartyFollowInfo', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x68DE70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67A9B0))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreaturePartyFollowInfo', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreaturePartyFollowInfo', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67A9B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6E9770))
  if fn:
    fn.setName('GetClassLevel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6E9770: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6EBEE0))
  if fn:
    fn.setName('GetCriticalHitRoll', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6EBEE0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x762640))
  if fn:
    fn.setName('AddToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x762640: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7627D0))
  if fn:
    fn.setName('GetMeshBoundingBox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7627D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x762D80))
  if fn:
    fn.setName('GetMeshOutline', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x762D80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x768450))
  if fn:
    fn.setName('RemoveFromArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x768450: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x768CA0))
  if fn:
    fn.setName('SetOrientation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x768CA0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x741A40))
  if fn:
    fn.setName('OnApplyAreaOfEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x741A40: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x748280))
  if fn:
    fn.setName('OnApplyBlindness', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x748280: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x73EE40))
  if fn:
    fn.setName('OnApplyDeaf', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73EE40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73E870))
  if fn:
    fn.setName('OnApplyDisarm', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73E870: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x745C80))
  if fn:
    fn.setName('OnApplyDispelBestMagic', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x745C80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73DE10))
  if fn:
    fn.setName('OnApplyEntangled', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73DE10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74A0D0))
  if fn:
    fn.setName('OnApplyForceJumpDelayed', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74A0D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x74A720))
  if fn:
    fn.setName('OnApplyForcePush', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x74A720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x743520))
  if fn:
    fn.setName('OnApplyHasteOrSlow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x743520: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x742580))
  if fn:
    fn.setName('OnApplyRegenerate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x742580: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73B940))
  if fn:
    fn.setName('OnApplyResurrection', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73B940: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x73F490))
  if fn:
    fn.setName('OnApplySetState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73F490: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x741560))
  if fn:
    fn.setName('OnApplyVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x741560: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x741DE0))
  if fn:
    fn.setName('OnRemoveAreaOfEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x741DE0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7E9150))
  if fn:
    fn.setName('CSWSEncounter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E9150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ECF00))
  if fn:
    fn.setName('LoadEncounterGeometry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ECF00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7ECD00))
  if fn:
    fn.setName('LoadEncounterSpawnPoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7ECD00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EB640))
  if fn:
    fn.setName('LoadFromTemplate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EB640: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7EE410))
  if fn:
    fn.setName('SpawnIfAppropriate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EE410: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7F12B0))
  if fn:
    fn.setName('CSWSExpression', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSExpression', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSExpression', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F12B0: '+str(e)); errors += 1
print('Batch 5 done: renamed='+str(renamed)+' errors='+str(errors))