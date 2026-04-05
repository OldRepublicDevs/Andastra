from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0
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
  fn = fm.getFunctionAt(toAddr(0x61C540))
  if fn:
    fn.setName('GetResRefFromFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61C540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E8AB0))
  if fn:
    fn.setName('Control', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAurBehaviorCameraFollow', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAurBehaviorCameraFollow', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E8AB0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x686B80))
  if fn:
    fn.setName('CExoRandInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoRandInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoRandInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x686B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4446C0))
  if fn:
    fn.setName('TogglePartyFollow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4446C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41B9C0))
  if fn:
    fn.setName('ResolveReactionAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41B9C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x413850))
  if fn:
    fn.setName('BlurWeapon', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x413850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x419400))
  if fn:
    fn.setName('UpdateWeaponClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x419400: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4DE6F0))
  if fn:
    fn.setName('DialogNodeSpanningTextParse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DE6F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54D140))
  if fn:
    fn.setName('CleanUpEntryNode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54D140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x47F250))
  if fn:
    fn.setName('GetName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47F250: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x523BD0))
  if fn:
    fn.setName('SelectNextAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x523BD0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x421560))
  if fn:
    fn.setName('PerformGiveItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x421560: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x41AE50))
  if fn:
    fn.setName('GetAttackingWeapon', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41AE50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41B0C0))
  if fn:
    fn.setName('BuildArturoImage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('ArturoTextureController', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'ArturoTextureController', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41B0C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x420720))
  if fn:
    fn.setName('ForceAppearanceUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x420720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x421410))
  if fn:
    fn.setName('UpdatePopUpBubble', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x421410: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x421480))
  if fn:
    fn.setName('ActionMenuDisableMine', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x421480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4214F0))
  if fn:
    fn.setName('PerformUseItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4214F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4368E0))
  if fn:
    fn.setName('CExoImeUI', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoImeUI', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoImeUI', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4368E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x448B50))
  if fn:
    fn.setName('SetCanSendDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x448B50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44D8E0))
  if fn:
    fn.setName('PlayCharacterSelectSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44D8E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44D940))
  if fn:
    fn.setName('EnableMouseAtEndOfLoadModuleMovie', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44D940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x457720))
  if fn:
    fn.setName('ForceBrightSunAndMoon', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x457720: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x458F70))
  if fn:
    fn.setName('dancedancemalak', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x458F70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x45FFC0))
  if fn:
    fn.setName('DismissInGameGUI', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x45FFC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x462C60))
  if fn:
    fn.setName('SetAutoLevelUpNPCs', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientOptions', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientOptions', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x462C60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x479590))
  if fn:
    fn.setName('GetWorldFormationPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWParty', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWParty', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x479590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x480120))
  if fn:
    fn.setName('GetServerItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x480120: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C1A70))
  if fn:
    fn.setName('HandleServerToPlayerGuiQuickbar', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C1A70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C9A50))
  if fn:
    fn.setName('ApplyShadowBlob', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C9A50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DADE0))
  if fn:
    fn.setName('ShowBarkBubble', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DADE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DC2F0))
  if fn:
    fn.setName('StopGlobalFade', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DC2F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E0CA0))
  if fn:
    fn.setName('GetMiniMapVisible', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E0CA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E1240))
  if fn:
    fn.setName('MarkNoClickEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E1240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F3F60))
  if fn:
    fn.setName('ShowHeadItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseHeadWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseHeadWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F3F60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F41A0))
  if fn:
    fn.setName('SetAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseHeadWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseHeadWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F41A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F4250))
  if fn:
    fn.setName('SetOverlayAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseHeadWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseHeadWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F4250: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x512990))
  if fn:
    fn.setName('CSWGuiOptionsCheckbox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsCheckbox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsCheckbox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x512990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x512EA0))
  if fn:
    fn.setName('SetIcon', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterfaceAction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterfaceAction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x512EA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x517390))
  if fn:
    fn.setName('Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterfaceStatus', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterfaceStatus', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x517390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x522E90))
  if fn:
    fn.setName('DoTargetAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainInterface', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x522E90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x527FB0))
  if fn:
    fn.setName('OnShoulderRight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x527FB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x527FF0))
  if fn:
    fn.setName('OnShoulderLeft', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x527FF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x52D460))
  if fn:
    fn.setName('GetPartyAccessPanelUp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x52D460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x535FD0))
  if fn:
    fn.setName('ActionMenuSecurity', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x535FD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x537F50))
  if fn:
    fn.setName('DisplayTrigger', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCTrigger', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCTrigger', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x537F50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5457B0))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniEnemy', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniEnemy', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5457B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x545A40))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGObstacle', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGObstacle', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x545A40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x546130))
  if fn:
    fn.setName('SortItemsByDate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSortedJournalArrayList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSortedJournalArrayList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x546130: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x546BA0))
  if fn:
    fn.setName('SetJournalPicture', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x546BA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x547EA0))
  if fn:
    fn.setName('Reset', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('KOTOR_AUTOSAVE_PARAMS', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'KOTOR_AUTOSAVE_PARAMS', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x547EA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5567F0))
  if fn:
    fn.setName('fourExoStrings_', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5567F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x559CE0))
  if fn:
    fn.setName('PlaySoundA', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWBehaviorAttachSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWBehaviorAttachSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x559CE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55C4F0))
  if fn:
    fn.setName('CSWCMapPin', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMapPin', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMapPin', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55C4F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x560520))
  if fn:
    fn.setName('Control_ComputeDesiredPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCameraOnAStick', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCameraOnAStick', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x560520: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x567B90))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameGalaxyMap', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameGalaxyMap', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x567B90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x569AA0))
  if fn:
    fn.setName('SetUpOptions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsFeedback', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsFeedback', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x569AA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x56D7A0))
  if fn:
    fn.setName('OnAnisotropyLeft', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsGraphicsAdvanced', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsGraphicsAdvanced', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x56D7A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x571320))
  if fn:
    fn.setName('SetNewOptions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsSoundAdvanced', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsSoundAdvanced', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x571320: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x593970))
  if fn:
    fn.setName('OnPanelRemoved', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAbilities', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAbilities', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x593970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x595370))
  if fn:
    fn.setName('DisplayPowers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAbilities', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAbilities', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x595370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AD870))
  if fn:
    fn.setName('SetQueryText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameSoloModeQuery', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameSoloModeQuery', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AD870: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B1D10))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialogComputer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialogComputer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B1D10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B77B0))
  if fn:
    fn.setName('OnTypeSelected', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgradeSelection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgradeSelection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B77B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EAF10))
  if fn:
    fn.setName('CSWMGBehaviorFire', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGBehaviorFire', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGBehaviorFire', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EAF10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EECA0))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMainCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMainCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EECA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F3180))
  if fn:
    fn.setName('CancelledByChild', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLevelUpPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLevelUpPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F3180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F45E0))
  if fn:
    fn.setName('PortraitCreationCompleted', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCustomPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCustomPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F45E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F7380))
  if fn:
    fn.setName('ShowGranted', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F7380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x623790))
  if fn:
    fn.setName('ReleaseResource', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x623790: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x625BA0))
  if fn:
    fn.setName('WriteFieldCHAR', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x625BA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62C3F0))
  if fn:
    fn.setName('Write', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CERFKey', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CERFKey', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62C3F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63DBF0))
  if fn:
    fn.setName('Demand', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63DBF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x650960))
  if fn:
    fn.setName('SaveBodyBag', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIBodyBagInfo', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIBodyBagInfo', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x650960: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6509E0))
  if fn:
    fn.setName('LoadBodyBag', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIBodyBagInfo', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIBodyBagInfo', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6509E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x650B80))
  if fn:
    fn.setName('_CSWGlobalVariableTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGlobalVariableTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGlobalVariableTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x650B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x656E30))
  if fn:
    fn.setName('HasTargetBeenDetected', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x656E30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65C900))
  if fn:
    fn.setName('SetCombatReactionTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65C900: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x669A60))
  if fn:
    fn.setName('SetParentId', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CItemRepository', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CItemRepository', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x669A60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x67D3A0))
  if fn:
    fn.setName('AddMoveToPointAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x67D3A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x686E10))
  if fn:
    fn.setName('RangeWeaponEquipped', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x686E10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x696DB0))
  if fn:
    fn.setName('RemoveSkillBonus', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x696DB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x698430))
  if fn:
    fn.setName('UpdateAppearanceForEquippedItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x698430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B1660))
  if fn:
    fn.setName('TimeStampCameraAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B1660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6CA520))
  if fn:
    fn.setName('TerminateClientSidePath', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6CA520: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F5300))
  if fn:
    fn.setName('GetWeaponSpecialization', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F5300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F7280))
  if fn:
    fn.setName('GetCreatureTalentRandomPower', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F7280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7231F0))
  if fn:
    fn.setName('SetTransitionPending', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7231F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73C7F0))
  if fn:
    fn.setName('OnApplyACIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73C7F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73D020))
  if fn:
    fn.setName('OnRemoveACDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73D020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73DA20))
  if fn:
    fn.setName('OnApplyDamageImmunityIncrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73DA20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73E160))
  if fn:
    fn.setName('OnRemoveEntangled', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73E160: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7423B0))
  if fn:
    fn.setName('OnRemoveForceResistanceDecrease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7423B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x746470))
  if fn:
    fn.setName('OnApplySkillIncrease', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x746470: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x748E60))
  if fn:
    fn.setName('OnApplyAppear', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x748E60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75CCD0))
  if fn:
    fn.setName('PermittedToDisplayCharacterSheet', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75CCD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x768650))
  if fn:
    fn.setName('RenderMesh', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCollisionMesh', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCollisionMesh', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x768650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x768BF0))
  if fn:
    fn.setName('MoveToNextOpenState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x768BF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x76F230))
  if fn:
    fn.setName('RemoveItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x76F230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7767A0))
  if fn:
    fn.setName('HasCreatureWeapons', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7767A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x79CB60))
  if fn:
    fn.setName('ExecuteCommandGetPartyMemberCount', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x79CB60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C1770))
  if fn:
    fn.setName('GetItemInInventory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C1770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7D4370))
  if fn:
    fn.setName('AddTriggerGeometryToMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7D4370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E4950))
  if fn:
    fn.setName('AttemptRemove', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSFaction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E4950: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F05E0))
  if fn:
    fn.setName('SetLocation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSScriptVarTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSScriptVarTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F05E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F2D40))
  if fn:
    fn.setName('ItemListGetUpdateItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayerLUOInventory', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayerLUOInventory', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F2D40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F3640))
  if fn:
    fn.setName('clear', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<int>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<int>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F3640: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F5480))
  if fn:
    fn.setName('Load', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F5480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F55B0))
  if fn:
    fn.setName('Save', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F55B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x801230))
  if fn:
    fn.setName('HandlePlayerToServerInputCancelGuiTimingEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x801230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x808CC0))
  if fn:
    fn.setName('HandlePlayerToServerPlayModuleCharacterList_Stop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x808CC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x84ADD0))
  if fn:
    fn.setName('GetPart', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Gob', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Gob', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x84ADD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x866F90))
  if fn:
    fn.setName('TypeMatch', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x866F90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8C3320))
  if fn:
    fn.setName('UpdateScenesAttach', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8C3320: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8D7AD0))
  if fn:
    fn.setName('ImageReadS3TCHeader', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8D7AD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90BB40))
  if fn:
    fn.setName('Unload', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAurora', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAurora', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90BB40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9168B0))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScene', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScene', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9168B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x917300))
  if fn:
    fn.setName('HandleMouseOver', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiControl', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiControl', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x917300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91C940))
  if fn:
    fn.setName('SetActiveControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91C940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91DEA0))
  if fn:
    fn.setName('SetActive', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91DEA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x933A08))
  if fn:
    fn.setName('_x87FloatMathInternal_00707685', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x933A08: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x983880))
  if fn:
    fn.setName('staticInitDefaultHitInfo', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x983880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x808B60))
  if fn:
    fn.setName('HandlePlayerToServerPlayModuleCharacterList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x808B60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F5030))
  if fn:
    fn.setName('GetCreatureTalentRandom', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F5030: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x458480))
  if fn:
    fn.setName('bright', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x458480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DC500))
  if fn:
    fn.setName('SetGunModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGGunBank', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGGunBank', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DC500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B19C0))
  if fn:
    fn.setName('SetDialogMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialogComputer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialogComputer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B19C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x571740))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameOptions', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameOptions', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x571740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x572990))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x572990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x568930))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsSoundAdvanced', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsSoundAdvanced', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x568930: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x569D30))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsMain', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsMain', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x569D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5702D0))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsFeedback', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsFeedback', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5702D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5E70D0))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsGraphics', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsGraphics', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E70D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x58FEF0))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiOptionsGraphicsAdvanced', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiOptionsGraphicsAdvanced', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x58FEF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5E5EC0))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameGameplay', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameGameplay', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E5EC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F7970))
  if fn:
    fn.setName('HandleCancelButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersLevelUp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersLevelUp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F7970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x601660))
  if fn:
    fn.setName('HandleCancelButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x601660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FA1F0))
  if fn:
    fn.setName('HandleCancelButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FA1F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FD880))
  if fn:
    fn.setName('HandleCancelButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiAbilitiesCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiAbilitiesCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FD880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FD740))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiAbilitiesCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiAbilitiesCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FD740: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F7340))
  if fn:
    fn.setName('GetCreatureTalentRandomSpecialAbility', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F7340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F8560))
  if fn:
    fn.setName('GetCreatureTalentRandomFeat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F8560: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5AEA80))
  if fn:
    fn.setName('SetDialogMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5AEA80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x915C80))
  if fn:
    fn.setName('GetFontHeight', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiText', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiText', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x915C80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x91DD30))
  if fn:
    fn.setName('AddControls', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiListBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x91DD30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F4560))
  if fn:
    fn.setName('CancelledByChild', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiCustomPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiCustomPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F4560: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48F0D0))
  if fn:
    fn.setName('SetSkillRankBase', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCLevelUpStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCLevelUpStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48F0D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DBE50))
  if fn:
    fn.setName('IsCameraDialog', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DBE50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x52D4F0))
  if fn:
    fn.setName('AddMessageLabel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMessageBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMessageBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x52D4F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x52D610))
  if fn:
    fn.setName('FixMessageLabel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiMessageBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiMessageBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x52D610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x595120))
  if fn:
    fn.setName('UpdateDescriptionListBox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAbilities', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAbilities', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x595120: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x595290))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAbilities', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAbilities', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x595290: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FF190))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameEquip', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FF190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B8B30))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B8B30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5BD130))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgradeItemSelect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgradeItemSelect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5BD130: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5CB280))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgrade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5CB280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59C000))
  if fn:
    fn.setName('OnControlEntered', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiQuestItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiQuestItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59C000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x56B7A0))
  if fn:
    fn.setName('SetDescription', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAutoPause', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAutoPause', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x56B7A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x603D60))
  if fn:
    fn.setName('HandleCancelButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPortraitCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPortraitCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x603D60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x605230))
  if fn:
    fn.setName('HandleCancelButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiNameChargen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiNameChargen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x605230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F5550))
  if fn:
    fn.setName('CancelledByChild', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiQuickPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiQuickPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F5550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x419C50))
  if fn:
    fn.setName('UpdateRelativePerception', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x419C50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4955D0))
  if fn:
    fn.setName('CSWPortrait', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPortrait', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPortrait', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4955D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x50E900))
  if fn:
    fn.setName('GetWeightedDamageAmount', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWRules', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWRules', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x50E900: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x693AC0))
  if fn:
    fn.setName('ComputeArmourClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x693AC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x69E3B0))
  if fn:
    fn.setName('RemoveCombatInvisibilityEffects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x69E3B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x717CA0))
  if fn:
    fn.setName('GetFirstObjectIndiceByX', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x717CA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x419DC0))
  if fn:
    fn.setName('UpdateRelativePerception', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x419DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4206A0))
  if fn:
    fn.setName('GetModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4206A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x448240))
  if fn:
    fn.setName('BarkString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x448240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4774E0))
  if fn:
    fn.setName('BarkBubble', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4774E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x571A00))
  if fn:
    fn.setName('OnEnterSlot', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameOptions', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameOptions', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x571A00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5E5700))
  if fn:
    fn.setName('OnAutoLevelUp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameGameplay', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameGameplay', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5E5700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x68EF60))
  if fn:
    fn.setName('GetForceItemMask', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x68EF60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x722D30))
  if fn:
    fn.setName('ClearObjects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x722D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x77FF30))
  if fn:
    fn.setName('UpdatePositionInObjectsArray', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x77FF30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7C6430))
  if fn:
    fn.setName('RemoveSpecialWalk', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C6430: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9169F0))
  if fn:
    fn.setName('HandleMouseOver', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiNavigable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiNavigable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9169F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40B800))
  if fn:
    fn.setName('ActivateRenderWindow', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40B800: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60F840))
  if fn:
    fn.setName('AsyncReadThreadProc', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60F840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AC50))
  if fn:
    fn.setName('AsyncRimLoadThreadProc', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AC50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86B370))
  if fn:
    fn.setName('AurBumpyShinyAvailable', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86B370: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x884770))
  if fn:
    fn.setName('AurCubeMapAvailable', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x884770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8685C0))
  if fn:
    fn.setName('AurDisableHeadLight', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8685C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86B0B0))
  if fn:
    fn.setName('AurDisableVertexBufferObjects', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86B0B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86B0C0))
  if fn:
    fn.setName('AurDisableVertexBufferObjectsWriteOnly', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86B0C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86B0D0))
  if fn:
    fn.setName('AurEnableVertexBufferObjectsWriteOnly', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86B0D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8D02D0))
  if fn:
    fn.setName('AurGUICloseViewport', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8D02D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8CFD70))
  if fn:
    fn.setName('AurGUISetupViewport', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8CFD70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8CFD00))
  if fn:
    fn.setName('AurGUIStopLayer', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8CFD00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86B970))
  if fn:
    fn.setName('AurGetStencilClearValue', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86B970: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8846F0))
  if fn:
    fn.setName('AurMultiTextureAvailable', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8846F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86B420))
  if fn:
    fn.setName('AurOnePassEnvMapAvailable', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86B420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8D5EB0))
  if fn:
    fn.setName('AurPartTriggerSetHighlight', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8D5EB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8D5ED0))
  if fn:
    fn.setName('AurPartTriggerSetHighlightParams', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8D5ED0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x867E10))
  if fn:
    fn.setName('AurResFree', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x867E10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8682E0))
  if fn:
    fn.setName('AurResFreeDataBytes', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8682E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x867CC0))
  if fn:
    fn.setName('AurResGet', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x867CC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x868260))
  if fn:
    fn.setName('AurResGetDataBytes', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x868260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x867EE0))
  if fn:
    fn.setName('AurResGetNextLine', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x867EE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x81B670))
  if fn:
    fn.setName('AurTextureEnvMapDisabled', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x81B670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x81A630))
  if fn:
    fn.setName('AurTextureForceControllerUpdate', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x81A630: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x829630))
  if fn:
    fn.setName('AurTextureMakeCurrent', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x829630: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8FE280))
  if fn:
    fn.setName('AurVertexProgramMakeCurrent', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8FE280: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x945180))
  if fn:
    fn.setName('BSPFitSphere', SourceType.USER_DEFINED)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x945180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6207E0))
  if fn:
    fn.setName('C2DA', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('C2DA', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'C2DA', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6207E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x620C70))
  if fn:
    fn.setName('GetCExoStringEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('C2DA', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'C2DA', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x620C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x620990))
  if fn:
    fn.setName('GetColumnIndex', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('C2DA', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'C2DA', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x620990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x621DC0))
  if fn:
    fn.setName('GetRowIndex', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('C2DA', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'C2DA', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x621DC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x621E70))
  if fn:
    fn.setName('Load2DArray', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('C2DA', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'C2DA', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x621E70: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4E8AB0))
  if fn:
    fn.setName('Control', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAurBehaviorCameraFollow', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAurBehaviorCameraFollow', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E8AB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x844470))
  if fn:
    fn.setName('AnimateExternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAurObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAurObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x844470: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x44EEB0))
  if fn:
    fn.setName('AddObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientAIList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientAIList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44EEB0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x40C770))
  if fn:
    fn.setName('GetCaptureEventFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C770: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x40C790))
  if fn:
    fn.setName('GetCaptureEvents', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C790: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x40CC50))
  if fn:
    fn.setName('GetClientAIMaster', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CC50: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x40CA10))
  if fn:
    fn.setName('GetCreatureByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CA10: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x40C910))
  if fn:
    fn.setName('GetPlayerCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C910: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x40D380))
  if fn:
    fn.setName('GetSWGuiManager', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D380: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x40C870))
  if fn:
    fn.setName('GetWorldTimer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40C870: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40E440))
  if fn:
    fn.setName('HideMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40E440: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x40CE80))
  if fn:
    fn.setName('IsClientInitialized', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CE80: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x40D1C0))
  if fn:
    fn.setName('RestartMoviesIfNecessary', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D1C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x40CFE0))
  if fn:
    fn.setName('SetEventDescriptions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40CFE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D320))
  if fn:
    fn.setName('SetInputActive', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D320: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40D3C0))
  if fn:
    fn.setName('SetInputClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40D3C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x40E460))
  if fn:
    fn.setName('ShowMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40E460: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4ED450))
  if fn:
    fn.setName('StopRumblePattern', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4ED450: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x431980))
  if fn:
    fn.setName('ExitProgram', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x431980: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x430A60))
  if fn:
    fn.setName('GetActivePauseState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x430A60: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4383D0))
  if fn:
    fn.setName('GetCreatureByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4383D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4394C0))
  if fn:
    fn.setName('GetPlayerCreature', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4394C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4604E0))
  if fn:
    fn.setName('HideMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4604E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44CE30))
  if fn:
    fn.setName('HideMoviePlayerFadePanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44CE30: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x436CF0))
  if fn:
    fn.setName('ReadVideoModeSettings', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x436CF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x43A820))
  if fn:
    fn.setName('RestartMoviesIfNecessary', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x43A820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x432410))
  if fn:
    fn.setName('SetEventDescriptions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x432410: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x45FC60))
  if fn:
    fn.setName('SetInputClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x45FC60: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x460540))
  if fn:
    fn.setName('ShowMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x460540: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4446C0))
  if fn:
    fn.setName('TogglePartyFollow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4446C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x444E40))
  if fn:
    fn.setName('WaitForMovie', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x444E40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x468C60))
  if fn:
    fn.setName('GetGammaSetting', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientOptions', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientOptions', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x468C60: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x5340D0))
  if fn:
    fn.setName('CExoArrayList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5340D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x873110))
  if fn:
    fn.setName('Count', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x873110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x866620))
  if fn:
    fn.setName('Remove', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x866620: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C7640))
  if fn:
    fn.setName('insert', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C7640: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44E700))
  if fn:
    fn.setName('Allocate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44E700: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x44E910))
  if fn:
    fn.setName('Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CGuiStringStruct>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CGuiStringStruct>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44E910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44E7B0))
  if fn:
    fn.setName('Allocate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CGuiStringStruct>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CGuiStringStruct>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44E7B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44E110))
  if fn:
    fn.setName('Insert', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CGuiStringStruct>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CGuiStringStruct>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44E110: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44E030))
  if fn:
    fn.setName('CExoArrayList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CSWGuiControlEvent*>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CSWGuiControlEvent*>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44E030: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8BFD40))
  if fn:
    fn.setName('Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<CSWGuiControlEvent>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<CSWGuiControlEvent>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8BFD40: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x61A5F0))
  if fn:
    fn.setName('contains', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<NWPlayerCharacterList_st*>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<NWPlayerCharacterList_st*>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61A5F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6E02D0))
  if fn:
    fn.setName('deleteAt', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<Vector>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<Vector>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6E02D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x903DF0))
  if fn:
    fn.setName('Insert', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<pointer>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<pointer>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x903DF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64CB50))
  if fn:
    fn.setName('AddUnique', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<unsigned_long>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<unsigned_long>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64CB50: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x495320))
  if fn:
    fn.setName('AddUnique', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<ushort>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<ushort>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x495320: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x60C500))
  if fn:
    fn.setName('GetCurrentTimestamp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoDebug', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoDebug', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60C500: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x60BF80))
  if fn:
    fn.setName('Warning', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoDebug', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoDebug', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60BF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60CB90))
  if fn:
    fn.setName('GetCurrentTimestamp', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoDebugInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoDebugInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60CB90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60C4D0))
  if fn:
    fn.setName('Warning', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoDebugInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoDebugInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60C4D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62AF50))
  if fn:
    fn.setName('OpenAsyncFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoEncapsulatedFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoEncapsulatedFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62AF50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62ADC0))
  if fn:
    fn.setName('OpenFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoEncapsulatedFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoEncapsulatedFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62ADC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60B880))
  if fn:
    fn.setName('CExoFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60B880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60B970))
  if fn:
    fn.setName('FileOpened', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60B970: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x60BB30))
  if fn:
    fn.setName('Write', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60BB30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60B910))
  if fn:
    fn.setName('_CExoFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60B910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60FF40))
  if fn:
    fn.setName('CExoFileInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFileInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFileInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60FF40: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x610030))
  if fn:
    fn.setName('_CExoFileInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoFileInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoFileInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x610030: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x608370))
  if fn:
    fn.setName('CExoIni', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoIni', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoIni', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x608370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x608450))
  if fn:
    fn.setName('ReadIniEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoIni', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoIni', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x608450: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6083F0))
  if fn:
    fn.setName('_CExoIni', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoIni', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoIni', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6083F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60DEF0))
  if fn:
    fn.setName('CExoIniInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoIniInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoIniInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60DEF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x60E100))
  if fn:
    fn.setName('ReadIniEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoIniInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoIniInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60E100: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x60DF60))
  if fn:
    fn.setName('_CExoIniInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoIniInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoIniInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60DF60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x616380))
  if fn:
    fn.setName('CExoInput', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInput', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x616380: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x616810))
  if fn:
    fn.setName('GetMouseMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInput', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x616810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6168B0))
  if fn:
    fn.setName('HideMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInput', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6168B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x616700))
  if fn:
    fn.setName('KeyboardModeOff', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInput', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x616700: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6166E0))
  if fn:
    fn.setName('KeyboardModeOn', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInput', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6166E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x616830))
  if fn:
    fn.setName('RefreshMouseCursor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInput', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x616830: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6167F0))
  if fn:
    fn.setName('SetMouseMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInput', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6167F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6168D0))
  if fn:
    fn.setName('ShowMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInput', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6168D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x610E10))
  if fn:
    fn.setName('CExoInputInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x610E10: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6159E0))
  if fn:
    fn.setName('HideMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6159E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x614CE0))
  if fn:
    fn.setName('KeyboardModeOff', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x614CE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x614CA0))
  if fn:
    fn.setName('KeyboardModeOn', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x614CA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6157D0))
  if fn:
    fn.setName('RefreshMouseCursor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6157D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x615820))
  if fn:
    fn.setName('SetMouseCursor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x615820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x615770))
  if fn:
    fn.setName('SetMouseMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x615770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x615A00))
  if fn:
    fn.setName('ShowMouse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x615A00: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4C21D0))
  if fn:
    fn.setName('AddDirectoryContents', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoKeyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C21D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x62FDA0))
  if fn:
    fn.setName('DestroyTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoKeyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62FDA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62FB20))
  if fn:
    fn.setName('LocateBifFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoKeyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62FB20: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7C02E0))
  if fn:
    fn.setName('AddHead', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLinkedListInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7C02E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60BCA0))
  if fn:
    fn.setName('CExoLinkedListInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLinkedListInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60BCA0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x733B00))
  if fn:
    fn.setName('Remove', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLinkedListInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x733B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x477C30))
  if fn:
    fn.setName('RemoveHead', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLinkedListInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x477C30: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x60D770))
  if fn:
    fn.setName('Assign', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLocStringInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLocStringInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60D770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60D5C0))
  if fn:
    fn.setName('CExoLocStringInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoLocStringInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoLocStringInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60D5C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x921250))
  if fn:
    fn.setName('GetNumberOfMoviesToPlay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoMoviePlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoMoviePlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x921250: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x921230))
  if fn:
    fn.setName('HideFadePanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoMoviePlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoMoviePlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x921230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x445220))
  if fn:
    fn.setName('IsMoviePlaying', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoMoviePlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoMoviePlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x445220: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x921280))
  if fn:
    fn.setName('RestartMoviesAsync', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoMoviePlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoMoviePlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x921280: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x921080))
  if fn:
    fn.setName('RestartMoviesAsync', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoMoviePlayerInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoMoviePlayerInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x921080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x920CB0))
  if fn:
    fn.setName('ShowFadePanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoMoviePlayerInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoMoviePlayerInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x920CB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x686B80))
  if fn:
    fn.setName('CExoRandInternal', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoRandInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoRandInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x686B80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x619B40))
  if fn:
    fn.setName('KeyboardModeOff', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoRawInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoRawInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x619B40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x619B00))
  if fn:
    fn.setName('KeyboardModeOn', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoRawInputInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoRawInputInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x619B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x630F50))
  if fn:
    fn.setName('OpenAsyncFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x630F50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x630DF0))
  if fn:
    fn.setName('OpenFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x630DF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x61B0B0))
  if fn:
    fn.setName('AddKeyTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61B0B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x61F0A0))
  if fn:
    fn.setName('FileCopy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61F0A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x61BCC0))
  if fn:
    fn.setName('GetKeyEntry', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61BCC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61BC20))
  if fn:
    fn.setName('GetKeyEntryFromTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61BC20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61C2D0))
  if fn:
    fn.setName('GetResObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61C2D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61C540))
  if fn:
    fn.setName('GetResRefFromFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61C540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61C6D0))
  if fn:
    fn.setName('GetTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61C6D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61F070))
  if fn:
    fn.setName('NukeDirectory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61F070: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61CA10))
  if fn:
    fn.setName('Release', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61CA10: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x61B050))
  if fn:
    fn.setName('RemoveFixedKeyTableFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61B050: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61CB30))
  if fn:
    fn.setName('RemoveKeyTable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61CB30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61B070))
  if fn:
    fn.setName('RemoveResourceDirectory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61B070: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61CD00))
  if fn:
    fn.setName('ServiceFromDirectory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61CD00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61DF90))
  if fn:
    fn.setName('ServiceFromDirectoryRaw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61DF90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61D910))
  if fn:
    fn.setName('ServiceFromResFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61D910: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x61E990))
  if fn:
    fn.setName('WipeDirectory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61E990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61AE30))
  if fn:
    fn.setName('_CExoResMan', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61AE30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x431EF0))
  if fn:
    fn.setName('OpenFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResourceImageFile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResourceImageFile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x431EF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x639CF0))
  if fn:
    fn.setName('Play', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x639CF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x63DCF0))
  if fn:
    fn.setName('Play', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundSourceInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63DCF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x557FE0))
  if fn:
    fn.setName('SetVolume', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStreamingSoundSource', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x557FE0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x605B70))
  if fn:
    fn.setName('AsFLOAT', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x605B70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x924B48))
  if fn:
    fn.setName('AsINT', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x924B48: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6067A0))
  if fn:
    fn.setName('LowerCase', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6067A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x606C90))
  if fn:
    fn.setName('UpperCase', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x606C90: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x605F00))
  if fn:
    fn.setName('operator_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x605F00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x605EC0))
  if fn:
    fn.setName('operator__', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x605EC0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7EF1E0))
  if fn:
    fn.setName('GetIsNPCFaction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CFactionManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CFactionManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EF1E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EF110))
  if fn:
    fn.setName('GetNPCFactionReputation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CFactionManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CFactionManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EF110: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7EF390))
  if fn:
    fn.setName('LoadFactionsFromSaveGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CFactionManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CFactionManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EF390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EF840))
  if fn:
    fn.setName('LoadReputationsFromSaveGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CFactionManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CFactionManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EF840: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7EF910))
  if fn:
    fn.setName('SaveFactions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CFactionManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CFactionManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EF910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EF9D0))
  if fn:
    fn.setName('SaveReputations', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CFactionManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CFactionManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EF9D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7EEEF0))
  if fn:
    fn.setName('_CFactionManager', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CFactionManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CFactionManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EEEF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x7351D0))
  if fn:
    fn.setName('LoadGameEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7351D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x734960))
  if fn:
    fn.setName('_CGameEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734960: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x469770))
  if fn:
    fn.setName('AddExternalObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameObjectArray', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameObjectArray', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x469770: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4693F0))
  if fn:
    fn.setName('_CGameObjectArray', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGameObjectArray', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGameObjectArray', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4693F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4CE960))
  if fn:
    fn.setName('AppendToDialogBuffer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4CE960: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54D140))
  if fn:
    fn.setName('CleanUpEntryNode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54D140: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E2040))
  if fn:
    fn.setName('CopyQuickSaveGameToFutureGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E2040: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DE6F0))
  if fn:
    fn.setName('DialogNodeSpanningTextParse', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DE6F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4DB260))
  if fn:
    fn.setName('HideSubItemCreateMenu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DB260: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x857940))
  if fn:
    fn.setName('CHitInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CHitInfo', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CHitInfo', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x857940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8FE2A0))
  if fn:
    fn.setName('_CHitInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CHitInfo', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CHitInfo', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8FE2A0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6687E0))
  if fn:
    fn.setName('CItemRepository', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CItemRepository', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CItemRepository', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6687E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x629340))
  if fn:
    fn.setName('CLYT', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CLYT', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CLYT', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x629340: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62A660))
  if fn:
    fn.setName('GetRoomName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CLYT', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CLYT', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62A660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62A6A0))
  if fn:
    fn.setName('GetRoomPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CLYT', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CLYT', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62A6A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x629610))
  if fn:
    fn.setName('LoadLayout', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CLYT', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CLYT', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x629610: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x62A630))
  if fn:
    fn.setName('UnloadLayout', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CLYT', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CLYT', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62A630: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x640120))
  if fn:
    fn.setName('EndServerMode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CNetLayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CNetLayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640120: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6403A0))
  if fn:
    fn.setName('GetPlayerInfo', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CNetLayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CNetLayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6403A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x640860))
  if fn:
    fn.setName('HandleBNCRMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CNetLayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CNetLayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640860: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x640510))
  if fn:
    fn.setName('HandleBNCSMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CNetLayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CNetLayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640510: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x640420))
  if fn:
    fn.setName('NonWindowMessages', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CNetLayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CNetLayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640420: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x63FC00))
  if fn:
    fn.setName('SendMessageToPlayer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CNetLayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CNetLayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63FC00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x640020))
  if fn:
    fn.setName('ShutDown', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CNetLayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CNetLayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640020: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x63F910))
  if fn:
    fn.setName('_CNetLayer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CNetLayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CNetLayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x63F910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40BF90))
  if fn:
    fn.setName('CObjectTableManager', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CObjectTableManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CObjectTableManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40BF90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6470B0))
  if fn:
    fn.setName('Initialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CPathfindInformation', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CPathfindInformation', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6470B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6244D0))
  if fn:
    fn.setName('AddDataField', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6244D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6246E0))
  if fn:
    fn.setName('AddDataLayoutList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6246E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x624190))
  if fn:
    fn.setName('AddField', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x624190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x624090))
  if fn:
    fn.setName('AddStruct', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x624090: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6238D0))
  if fn:
    fn.setName('GetDataField', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6238D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6239E0))
  if fn:
    fn.setName('GetDataLayoutList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6239E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x623A40))
  if fn:
    fn.setName('GetFieldByLabel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x623A40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x624AC0))
  if fn:
    fn.setName('GetStructFromStruct', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x624AC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x623C00))
  if fn:
    fn.setName('InitializeForWriting', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x623C00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x626A10))
  if fn:
    fn.setName('Pack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x626A10: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6257A0))
  if fn:
    fn.setName('ReadFieldVOID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6257A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x625680))
  if fn:
    fn.setName('ReadFieldVector', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x625680: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x626480))
  if fn:
    fn.setName('WriteFieldDWORD64', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x626480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x625DB0))
  if fn:
    fn.setName('WriteFieldQuaternion', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x625DB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6263C0))
  if fn:
    fn.setName('WriteFieldVOID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6263C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x626310))
  if fn:
    fn.setName('WriteFieldVector', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x626310: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6267D0))
  if fn:
    fn.setName('WriteGFFData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResGFF', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6267D0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x622E00))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CRes2DA,2017>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CRes2DA,2017>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x622E00: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6272C0))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResARE,2012>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResARE,2012>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6272C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x62A900))
  if fn:
    fn.setName('SetResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResHelper<CResIFO,2014>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResHelper<CResIFO,2014>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x62A900: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x61A380))
  if fn:
    fn.setName('GetLength', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResRef', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61A380: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61A300))
  if fn:
    fn.setName('GetResRefStr', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResRef', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61A300: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61A360))
  if fn:
    fn.setName('IsValid', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResRef', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61A360: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x61A890))
  if fn:
    fn.setName('operator__', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResRef', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61A890: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61A570))
  if fn:
    fn.setName('operator__', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResRef', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61A570: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x55A6F0))
  if fn:
    fn.setName('AddStaticBoundingBox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55A6F0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x456E60))
  if fn:
    fn.setName('HiliteAreaObjects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x456E60: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x454D70))
  if fn:
    fn.setName('SetFogColor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x454D70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x453160))
  if fn:
    fn.setName('Update', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x453160: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x581F60))
  if fn:
    fn.setName('CSWCAreaOfEffectObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x581F60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x582950))
  if fn:
    fn.setName('LoadEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x582950: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x549DE0))
  if fn:
    fn.setName('SaveData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCMessageData', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCMessageData', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x549DE0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x413850))
  if fn:
    fn.setName('BlurWeapon', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x413850: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x421560))
  if fn:
    fn.setName('PerformGiveItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x421560: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DDC10))
  if fn:
    fn.setName('PerformSkill', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DDC10: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x41B9C0))
  if fn:
    fn.setName('ResolveReactionAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41B9C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x419400))
  if fn:
    fn.setName('UpdateWeaponClass', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x419400: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x6E04E0))
  if fn:
    fn.setName('AddKnownSpell', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureStats_ClassInfo', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureStats_ClassInfo', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6E04E0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x47F250))
  if fn:
    fn.setName('GetName', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCItem', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x47F250: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4C6830))
  if fn:
    fn.setName('HandleServerToPlayerAmbientMusicChangeTrack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C6830: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4C6AF0))
  if fn:
    fn.setName('HandleServerToPlayerAmbientSoundLoopChange', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C6AF0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4BF0B0))
  if fn:
    fn.setName('HandleServerToPlayerAreaOfEffectUpdate_Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4BF0B0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4990C0))
  if fn:
    fn.setName('HandleServerToPlayerCreatureUpdate_Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4990C0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4A0CA0))
  if fn:
    fn.setName('HandleServerToPlayerDoorUpdate_Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A0CA0: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4A4470))
  if fn:
    fn.setName('HandleServerToPlayerItemUpdate_Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4A4470: '+str(e)); errors += 1
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
  fn = fm.getFunctionAt(toAddr(0x4C1E60))
  if fn:
    fn.setName('HandleServerToPlayerMapPinEnabled', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj)
    renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C1E60: '+str(e)); errors += 1
print('Batch 4 done: renamed='+str(renamed)+' errors='+str(errors))