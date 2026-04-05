from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0
try:
  fn = fm.getFunctionAt(toAddr(0x592200))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameJournal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x592200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40E8A0))
  if fn:
    fn.setName('Allocate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40E8A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x516500))
  if fn:
    fn.setName('GetSWParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x516500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6416A0))
  if fn:
    fn.setName('AddToTopOfStack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineStack', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineStack', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6416A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6612B0))
  if fn:
    fn.setName('__ftol2', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6612B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6E5390))
  if fn:
    fn.setName('GetBaseItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWBaseItemArray', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWBaseItemArray', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6E5390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41BDD0))
  if fn:
    fn.setName('ResolveSpecialAttackVisuals', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41BDD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x41CE40))
  if fn:
    fn.setName('SetPortrait', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x41CE40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x46A260))
  if fn:
    fn.setName('LoadModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCModule', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x46A260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48C590))
  if fn:
    fn.setName('RequestHeadAppearance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureAppearance', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureAppearance', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48C590: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48E3B0))
  if fn:
    fn.setName('GetBTypeModelAndTextureResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreatureAppearance', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreatureAppearance', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48E3B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4DE1A0))
  if fn:
    fn.setName('ShowPartySelection', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4DE1A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4FD2F0))
  if fn:
    fn.setName('StartVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4FD2F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4FDBE0))
  if fn:
    fn.setName('StopVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4FDBE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5228C0))
  if fn:
    fn.setName('OnSlotSelected', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgrade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5228C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54F5B0))
  if fn:
    fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPazaakStart', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPazaakStart', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54F5B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x557910))
  if fn:
    fn.setName('LoadFromMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAmbientSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x557910: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x558940))
  if fn:
    fn.setName('CSWGuiInGameCredits', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameCredits', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameCredits', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x558940: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55B860))
  if fn:
    fn.setName('AIUpdate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55B860: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55CEE0))
  if fn:
    fn.setName('HitCheckBullet', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMiniGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMiniGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55CEE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B0D70))
  if fn:
    fn.setName('CSWGuiDialogComputer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialogComputer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialogComputer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B0D70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B5080))
  if fn:
    fn.setName('OnPowerPicked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersFeatsSkillsDebugMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersFeatsSkillsDebugMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B5080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B5480))
  if fn:
    fn.setName('OnSkillPicked', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPowersFeatsSkillsDebugMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPowersFeatsSkillsDebugMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B5480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5BB650))
  if fn:
    fn.setName('CloseItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgrade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5BB650: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F0560))
  if fn:
    fn.setName('CSWGuiKeyMapButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiKeyMapButton', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiKeyMapButton', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F0560: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x604B10))
  if fn:
    fn.setName('CSWGuiNameChargen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiNameChargen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiNameChargen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x604B10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x642C30))
  if fn:
    fn.setName('ExecuteCode', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CVirtualMachineInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x642C30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6922C0))
  if fn:
    fn.setName('HandlePlayerToServerInputMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6922C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6954C0))
  if fn:
    fn.setName('GetNearestEnemy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6954C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AEF80))
  if fn:
    fn.setName('OnApplyDamage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AEF80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6B0460))
  if fn:
    fn.setName('SpawnBodyBag', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6B0460: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6DA3D0))
  if fn:
    fn.setName('AIActionOrientCamera', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6DA3D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6E50A0))
  if fn:
    fn.setName('CheckItemRaceRestrictions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6E50A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FACA0))
  if fn:
    fn.setName('AddControl', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FACA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x720B70))
  if fn:
    fn.setName('HandleTransparentDoors', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x720B70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73D5B0))
  if fn:
    fn.setName('OnRemoveTemporaryHitpoints', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73D5B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73E1C0))
  if fn:
    fn.setName('OnApplyKnockdown', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73E1C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x742FC0))
  if fn:
    fn.setName('OnApplyDisease', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEffectListHandler', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x742FC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7AF4D0))
  if fn:
    fn.setName('AddEventAbsoluteTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerAIMaster', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7AF4D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CB040))
  if fn:
    fn.setName('TestString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSExpression', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSExpression', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CB040: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DDD60))
  if fn:
    fn.setName('SendServerToPlayerPlayerList_All', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DDD60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EB1F0))
  if fn:
    fn.setName('LoadEncounter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EB1F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F78B0))
  if fn:
    fn.setName('FadeAndStop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F78B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80F6C0))
  if fn:
    fn.setName('ApplyLightsaberThrow', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80F6C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x88DC30))
  if fn:
    fn.setName('RenderSkinnedVP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x88DC30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x90CCE0))
  if fn:
    fn.setName('_CSWGuiPanel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiPanel', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x90CCE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9128C0))
  if fn:
    fn.setName('operator_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBorderParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBorderParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9128C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x914350))
  if fn:
    fn.setName('operator_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiImageParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiImageParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x914350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9176C0))
  if fn:
    fn.setName('DisplayToolTip', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiControl', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiControl', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9176C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x616B40))
  if fn:
    fn.setName('handleCompositionResult_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoImeUI', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoImeUI', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x616B40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6170E0))
  if fn:
    fn.setName('_CExoImeUI', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoImeUI', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoImeUI', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6170E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54C6B0))
  if fn:
    fn.setName('SetText', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTextParams', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTextParams', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54C6B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x595230))
  if fn:
    fn.setName('GetLength', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoString', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x595230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FF290))
  if fn:
    fn.setName('IsClassSkill', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSkillsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSkillsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FF290: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x419360))
  if fn:
    fn.setName('RequestAppearance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x419360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x475E00))
  if fn:
    fn.setName('ActivateHardCodedVisualWithHighestPriority', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x475E00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F95D0))
  if fn:
    fn.setName('LoadEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffect', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffect', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F95D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x522390))
  if fn:
    fn.setName('CSWGuiLoadScreen', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiLoadScreen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiLoadScreen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x522390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B25A0))
  if fn:
    fn.setName('_CSWGuiBarkBubble', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiBarkBubble', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiBarkBubble', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B25A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x65A5B0))
  if fn:
    fn.setName('GetWeaponImpactTime', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x65A5B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DD9D0))
  if fn:
    fn.setName('SendServerToPlayerPlayerList_Add', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DD9D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x503930))
  if fn:
    fn.setName('DetachVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x503930: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x510AF0))
  if fn:
    fn.setName('IsSkillClassSkill', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWClass', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWClass', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x510AF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x476F40))
  if fn:
    fn.setName('DetachVisualEffects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x476F40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x491220))
  if fn:
    fn.setName('IncreaseSkillRank', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCLevelUpStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCLevelUpStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x491220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59F240))
  if fn:
    fn.setName('GetUpgradableType', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59F240: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F3000))
  if fn:
    fn.setName('OnSelectSkillsButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F3000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x809C50))
  if fn:
    fn.setName('ParseGetString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x809C50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x809DB0))
  if fn:
    fn.setName('GetTokenPair', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x809DB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x848CB0))
  if fn:
    fn.setName('InternalCreateInstance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x848CB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x848E10))
  if fn:
    fn.setName('InternalCreateInstance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x848E10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x407440))
  if fn:
    fn.setName('ClearLogCounters', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x407440: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x40E820))
  if fn:
    fn.setName('ResetCurrentSessionStartTim', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x40E820: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4275F0))
  if fn:
    fn.setName('IsLeaderUsableItem', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4275F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C75E0))
  if fn:
    fn.setName('Remove', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<ushort>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<ushort>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C75E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4CE840))
  if fn:
    fn.setName('AppendToMsgBuffer', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4CE840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D4880))
  if fn:
    fn.setName('UnlockDialogParticipantOrientations', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D4880: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E74D0))
  if fn:
    fn.setName('SaveCameraSettings', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCameraOnAStick', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCameraOnAStick', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E74D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4FE1F0))
  if fn:
    fn.setName('ApplyHardCodedVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4FE1F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x509220))
  if fn:
    fn.setName('_CCombatInformation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CCombatInformation', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CCombatInformation', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x509220: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x512D80))
  if fn:
    fn.setName('GetAutoPaused', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x512D80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54D020))
  if fn:
    fn.setName('sprintf', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54D020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x57C8E0))
  if fn:
    fn.setName('ClearBehaviorList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCProjectile', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x57C8E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5ABF20))
  if fn:
    fn.setName('GetItemBuyValue', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiStore', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5ABF20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B01D0))
  if fn:
    fn.setName('CSWGuiDialogCinematic', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialogCinematic', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialogCinematic', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B01D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6081D0))
  if fn:
    fn.setName('GetResourceExtension', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoBase', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoBase', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6081D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60D4A0))
  if fn:
    fn.setName('GetString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoStringList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoStringList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60D4A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x61B030))
  if fn:
    fn.setName('RemoveResourceImageFile', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoResMan', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x61B030: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x637990))
  if fn:
    fn.setName('RemoveFromNonPlayingLoopingList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x637990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x640350))
  if fn:
    fn.setName('UpdateStatusLoop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CNetLayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CNetLayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x640350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64BBA0))
  if fn:
    fn.setName('GetStoreByGameObjectID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64BBA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x669A30))
  if fn:
    fn.setName('ClearItems', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CItemRepository', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CItemRepository', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x669A30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6F6DF0))
  if fn:
    fn.setName('GetTalentHasCompatibleCategory', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreatureStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6F6DF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7035A0))
  if fn:
    fn.setName('GetNPCID', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7035A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x70CFB0))
  if fn:
    fn.setName('NoNoneWalkPolysInStaticObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x70CFB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x728C60))
  if fn:
    fn.setName('CResRef', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CResRef', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x728C60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x770D30))
  if fn:
    fn.setName('SetOrientation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlaceable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x770D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7DE210))
  if fn:
    fn.setName('SendServerToPlayerServerStatus_Status', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7DE210: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EB810))
  if fn:
    fn.setName('ReadEncounterFromGff', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EB810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F3180))
  if fn:
    fn.setName('_CLastUpdateObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CLastUpdateObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CLastUpdateObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F3180: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F34A0))
  if fn:
    fn.setName('ClearKnownSpells', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayerLastUpdateObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayerLastUpdateObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F34A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7F3B40))
  if fn:
    fn.setName('AddFeat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWLevelStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWLevelStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7F3B40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x80BC50))
  if fn:
    fn.setName('NormalOf', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x80BC50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x81B9C0))
  if fn:
    fn.setName('allocate', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<Vector>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<Vector>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x81B9C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x825660))
  if fn:
    fn.setName('DeletePool', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('GLRender', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'GLRender', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x825660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x843BE0))
  if fn:
    fn.setName('operator_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x843BE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x84B050))
  if fn:
    fn.setName('FindScene', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x84B050: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8C3B00))
  if fn:
    fn.setName('Control', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAurBehaviorAttach', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAurBehaviorAttach', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8C3B00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8C5260))
  if fn:
    fn.setName('firstword', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8C5260: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8C5370))
  if fn:
    fn.setName('Parse_int', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8C5370: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8F6F60))
  if fn:
    fn.setName('Insert', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<Model*>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<Model*>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8F6F60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9125D0))
  if fn:
    fn.setName('StoreCurrentMousePosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiManager', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9125D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x916870))
  if fn:
    fn.setName('GetModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiScene', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiScene', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x916870: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4215B0))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiContainer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiContainer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4215B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4247A0))
  if fn:
    fn.setName('IsProjectileParryable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4247A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44EC30))
  if fn:
    fn.setName('HandleServerToPlayerPlayerList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44EC30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4583A0))
  if fn:
    fn.setName('restartminigame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4583A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x471D00))
  if fn:
    fn.setName('CSWGuiClassSelection', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiClassSelection', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiClassSelection', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x471D00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48F5E0))
  if fn:
    fn.setName('AIActionItemCastSpell', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48F5E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4C9020))
  if fn:
    fn.setName('GetCameraParameters', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWBehaviorCameraDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWBehaviorCameraDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4C9020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D0350))
  if fn:
    fn.setName('BuildAnimation', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('LookAtAnimation', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'LookAtAnimation', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D0350: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4D9AD0))
  if fn:
    fn.setName('ResetDialogAnimations', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CGuiInGame', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4D9AD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E38B0))
  if fn:
    fn.setName('LocaleEnumCallback', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E38B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F58B0))
  if fn:
    fn.setName('ReAttachWieldedObjects', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F58B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x501E30))
  if fn:
    fn.setName('UpdateDayNightCycle', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x501E30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5073B0))
  if fn:
    fn.setName('ToggleDoorState', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5073B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5075F0))
  if fn:
    fn.setName('MenuActionBash', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCDoor', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCDoor', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5075F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x513750))
  if fn:
    fn.setName('CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x513750: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x532C00))
  if fn:
    fn.setName('GetMemoryUsage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAurTextureBasic', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAurTextureBasic', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x532C00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x540F30))
  if fn:
    fn.setName('LoadScripts', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWTrackFollower', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x540F30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x549DA0))
  if fn:
    fn.setName('SetReplies', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x549DA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x55B480))
  if fn:
    fn.setName('AddToArea', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCSoundObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x55B480: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x586A00))
  if fn:
    fn.setName('SetTutorialReason', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTutorialBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTutorialBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x586A00: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x586EC0))
  if fn:
    fn.setName('PerformBash', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiTutorialBox', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiTutorialBox', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x586EC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5937D0))
  if fn:
    fn.setName('SetCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiInGameAbilities', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiInGameAbilities', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5937D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5BD230))
  if fn:
    fn.setName('Draw', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiUpgrade', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5BD230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D0B30))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiSaveLoad', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiSaveLoad', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D0B30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EB040))
  if fn:
    fn.setName('Fire', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGBehaviorFire', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGBehaviorFire', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EB040: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60DB60))
  if fn:
    fn.setName('ShutDown', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60DB60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x638870))
  if fn:
    fn.setName('AddToStreamingList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoSoundInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x638870: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x64C620))
  if fn:
    fn.setName('GetRoomIndex', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x64C620: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6AC150))
  if fn:
    fn.setName('SetDialogDelay', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6AC150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x704C70))
  if fn:
    fn.setName('PositionParty', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWPartyTable', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x704C70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x73E770))
  if fn:
    fn.setName('AIActionCastSpell', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x73E770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x774DB0))
  if fn:
    fn.setName('EquipMostEffectiveArmor', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCreature', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x774DB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x798390))
  if fn:
    fn.setName('ExecuteCommandGetLoadFromSaveGame', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x798390: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7BC850))
  if fn:
    fn.setName('UnloadModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CServerExoApp', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7BC850: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CA5E0))
  if fn:
    fn.setName('SendServerToPlayerExamineGui_ItemData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CA5E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7CE2D0))
  if fn:
    fn.setName('SendServerToPlayerExamineGui_CreatureData', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7CE2D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E6800))
  if fn:
    fn.setName('EventHandler', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E6800: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x801AF0))
  if fn:
    fn.setName('HandlePlayerToServerInventoryMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x801AF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x808A80))
  if fn:
    fn.setName('HandlePlayerToServerCharacterDownload', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x808A80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x85BCD0))
  if fn:
    fn.setName('DoEmitterBucket', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Scene', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Scene', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x85BCD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x866A10))
  if fn:
    fn.setName('__interpretLocale', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x866A10: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86A8E0))
  if fn:
    fn.setName('AurMakeCurrent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86A8E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8887C0))
  if fn:
    fn.setName('RenderLightMappedNoShaders', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8887C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8FA000))
  if fn:
    fn.setName('AnimateHierarchy', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8FA000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x912160))
  if fn:
    fn.setName('_internal_initTimeZone', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x912160: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x92BB72))
  if fn:
    fn.setName('_CxxFrameHandlerCore', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x92BB72: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9836D0))
  if fn:
    fn.setName('Project', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('PartSkin', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'PartSkin', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9836D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E9770))
  if fn:
    fn.setName('ClearSpawnList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSEncounter', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E9770: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x845E70))
  if fn:
    fn.setName('ControlDispatch', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('MdlNode', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'MdlNode', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x845E70: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F3A60))
  if fn:
    fn.setName('LoadModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseHeadWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseHeadWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F3A60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4F53B0))
  if fn:
    fn.setName('LoadModel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCAnimBaseWield', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCAnimBaseWield', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4F53B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x559C90))
  if fn:
    fn.setName('Control', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWBehaviorAttachSound', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWBehaviorAttachSound', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x559C90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x749640))
  if fn:
    fn.setName('OnRemoveBonusFeat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x749640: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x807980))
  if fn:
    fn.setName('HandlePlayerToServerServerStatusMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSMessage', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x807980: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8E8000))
  if fn:
    fn.setName('_VertexPrimitiveAnim', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('VertexPrimitiveAnim', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'VertexPrimitiveAnim', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8E8000: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8E85F0))
  if fn:
    fn.setName('PostProcess', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('PartSkin', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'PartSkin', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8E85F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x817660))
  if fn:
    fn.setName('Message', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAurBehaviorAttach', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAurBehaviorAttach', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x817660: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x86B190))
  if fn:
    fn.setName('HasBumpMap', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('Material', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'Material', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x86B190: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x770EC0))
  if fn:
    fn.setName('CSWSCombatRoundAction', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSCombatRoundAction', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSCombatRoundAction', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x770EC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x54C040))
  if fn:
    fn.setName('operator__', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptEvent', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptEvent', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x54C040: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A4D30))
  if fn:
    fn.setName('_CScriptEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CScriptEvent', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CScriptEvent', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A4D30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60FCE0))
  if fn:
    fn.setName('CExoAliasList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoAliasList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoAliasList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60FCE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60FD90))
  if fn:
    fn.setName('_CExoAliasList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoAliasList', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoAliasList', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60FD90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x72F830))
  if fn:
    fn.setName('SavePlayers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x72F830: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x59BAA0))
  if fn:
    fn.setName('PopulateItemListBox', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x59BAA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x698570))
  if fn:
    fn.setName('TransferGold', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x698570: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x444550))
  if fn:
    fn.setName('ChangeCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x444550: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x556BA0))
  if fn:
    fn.setName('PlayAmbientSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x556BA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x599200))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x599200: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7EFAC0))
  if fn:
    fn.setName('CreateDefaultFactions', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7EFAC0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8063A0))
  if fn:
    fn.setName('HandlePlayerToServerAreaMessage', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8063A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x431A30))
  if fn:
    fn.setName('UnloadModule', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x431A30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x44B360))
  if fn:
    fn.setName('CrownFutureLeader', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x44B360: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5F77B0))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5F77B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6372E0))
  if fn:
    fn.setName('PlayOneShotSound', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6372E0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x424B60))
  if fn:
    fn.setName('GetEquipmentForcePowers', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x424B60: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x60DFD0))
  if fn:
    fn.setName('Uninitialize', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x60DFD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4758F0))
  if fn:
    fn.setName('AddVisualEffect', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4758F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5446A0))
  if fn:
    fn.setName('Control', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5446A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FA090))
  if fn:
    fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FA090: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x430BD0))
  if fn:
    fn.setName('ClearPauseExclusionList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CClientExoAppInternal', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x430BD0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4ED270))
  if fn:
    fn.setName('GetSubRadialSpell', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSpell', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSpell', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4ED270: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x58A840))
  if fn:
    fn.setName('Insert', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CExoArrayList<short>', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CExoArrayList<short>', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x58A840: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x75B5D0))
  if fn:
    fn.setName('SaveServerCharacter', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSPlayer', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x75B5D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4FA9F0))
  if fn:
    fn.setName('CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4FA9F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5324A0))
  if fn:
    fn.setName('StopLoadFromLayout', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5324A0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x70ABA0))
  if fn:
    fn.setName('TestDirectLine', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x70ABA0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6A3C20))
  if fn:
    fn.setName('GetSpotCheckDistance', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6A3C20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x472080))
  if fn:
    fn.setName('operator_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x472080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x605D50))
  if fn:
    fn.setName('operator_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x605D50: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8FDF30))
  if fn:
    fn.setName('Enable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8FDF30: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x48F080))
  if fn:
    fn.setName('CExoString', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x48F080: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4CE500))
  if fn:
    fn.setName('_CSWGuiLabel', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4CE500: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6FCEF0))
  if fn:
    fn.setName('operator_', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6FCEF0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7E7810))
  if fn:
    fn.setName('GetPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7E7810: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FAE20))
  if fn:
    fn.setName('AddChosenFeat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FAE20: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x6CD530))
  if fn:
    fn.setName('UpdateSubareasOnJumpPosition', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x6CD530: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8B77D0))
  if fn:
    fn.setName('InitializeBranch', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8B77D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x9103B0))
  if fn:
    fn.setName('GetSafeMargin', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x9103B0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x721670))
  if fn:
    fn.setName('PositionWalkable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x721670: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x819BE0))
  if fn:
    fn.setName('AurTextureGetReference', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x819BE0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x8C2F40))
  if fn:
    fn.setName('AddDependency', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAurBehavior', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAurBehavior', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x8C2F40: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5EB170))
  if fn:
    fn.setName('CSWMGBehaviorTrack', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGBehaviorTrack', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGBehaviorTrack', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5EB170: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x855F80))
  if fn:
    fn.setName('SetShot', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWBehaviorCameraDialog', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWBehaviorCameraDialog', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x855F80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7341F0))
  if fn:
    fn.setName('ClippedLineSegmentWalkable', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7341F0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x734230))
  if fn:
    fn.setName('NoNonWalkPolysOnRoom', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x734230: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x7342C0))
  if fn:
    fn.setName('GetAdjacentRoom', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSRoom', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSRoom', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x7342C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x88E150))
  if fn:
    fn.setName('RenderEnvironmentMappedVP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x88E150: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x88E2D0))
  if fn:
    fn.setName('RenderEMLMVP', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x88E2D0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FA540))
  if fn:
    fn.setName('OnRecommendButton', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), '', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FA540: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5DD640))
  if fn:
    fn.setName('CSWMGTargettingParameters', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWMGTargettingParameters', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWMGTargettingParameters', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5DD640: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FB0C0))
  if fn:
    fn.setName('BuildAvailableList', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FB0C0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x707520))
  if fn:
    fn.setName('InSubAreas', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWSArea', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x707520: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4E7D80))
  if fn:
    fn.setName('CSWCameraFreeLook', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCameraFreeLook', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCameraFreeLook', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4E7D80: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5B0020))
  if fn:
    fn.setName('CSWGuiDialogTop', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiDialogTop', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiDialogTop', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5B0020: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5D8990))
  if fn:
    fn.setName('CAurBehavior', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CAurBehavior', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CAurBehavior', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5D8990: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x5FAFB0))
  if fn:
    fn.setName('RemoveChosenFeat', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWGuiFeatsCharGen', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWGuiFeatsCharGen', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x5FAFB0: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x491A90))
  if fn:
    fn.setName('HasFeatPrereq', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCLevelUpStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCLevelUpStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x491A90: '+str(e)); errors += 1
try:
  fn = fm.getFunctionAt(toAddr(0x4937C0))
  if fn:
    fn.setName('FeatAcquired', SourceType.USER_DEFINED)
    nsobj = sm.getNamespace('CSWCLevelUpStats', currentProgram.getGlobalNamespace())
    if nsobj is None: nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(), 'CSWCLevelUpStats', SourceType.USER_DEFINED)
    fn.setParentNamespace(nsobj); renamed += 1
  else: errors += 1
except Exception as e: print('ERR 0x4937C0: '+str(e)); errors += 1
print('Batch 3 done: renamed='+str(renamed)+' errors='+str(errors))