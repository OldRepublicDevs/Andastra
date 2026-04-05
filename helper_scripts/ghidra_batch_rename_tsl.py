# Ghidra TSL rename script  (43 entries)
from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0

try:
    fn = fm.getFunctionAt(toAddr(0x45CEA0))
    if fn:
        fn.setName('FUN_0061FBA0', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x45CEA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x53D6A0))
    if fn:
        fn.setName('PopulateModules', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiMainMenu', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiMainMenu', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x53D6A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x572CC0))
    if fn:
        fn.setName('CSWBehaviorCameraCombat', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWBehaviorCameraCombat', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWBehaviorCameraCombat', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x572CC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4A76C0))
    if fn:
        fn.setName('HandleServerToPlayerUpdate_Appearance', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4A76C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5E5A70))
    if fn:
        fn.setName('OnKeyMap', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameGameplay', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameGameplay', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5E5A70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5E5B30))
    if fn:
        fn.setName('OnMouse', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameGameplay', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameGameplay', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5E5B30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8D3660))
    if fn:
        fn.setName('CAurGUIImageInternal', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAurGUIImageInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAurGUIImageInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8D3660: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4DB200))
    if fn:
        fn.setName('ShowSubItemCreateMenu', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4DB200: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x41A990))
    if fn:
        fn.setName('SetLastInstant', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x41A990: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x41CE10))
    if fn:
        fn.setName('GetArmourClass', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x41CE10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x54D2E0))
    if fn:
        fn.setName('PlaceRightOf', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTutorial', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTutorial', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x54D2E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x605DC0))
    if fn:
        fn.setName('operator__', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoString', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x605DC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6E9620))
    if fn:
        fn.setName('ResolveSpecialAttackDamageBonus', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6E9620: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5413B0))
    if fn:
        fn.setName('CopyToString', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CResRef', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5413B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5D9920))
    if fn:
        fn.setName('CSWRK4SplitAcceleration', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWRK4SplitAcceleration', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWRK4SplitAcceleration', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5D9920: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5E2CD0))
    if fn:
        fn.setName('GetTotal', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CPazaakPlayer', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CPazaakPlayer', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5E2CD0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60D090))
    if fn:
        fn.setName('_CExoStringList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoStringList', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoStringList', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60D090: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x709770))
    if fn:
        fn.setName('void_return', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x709770: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7B6B70))
    if fn:
        fn.setName('SendServerToPlayerSaveLoad_Status', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7B6B70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x86AEE0))
    if fn:
        fn.setName('AurSetAntiAliasingMode', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x86AEE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8C5220))
    if fn:
        fn.setName('_sscanf', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8C5220: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x503030))
    if fn:
        fn.setName('LoadHardCodedVisualEffect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x503030: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x51BC70))
    if fn:
        fn.setName('CSWGuiSaveLoad', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiSaveLoad', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiSaveLoad', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x51BC70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x54AAF0))
    if fn:
        fn.setName('HandleServerToPlayerUpdate_CharacterSheet', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x54AAF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5B69A0))
    if fn:
        fn.setName('ItemSubTypeMenu', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiCreateItemMenu', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiCreateItemMenu', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5B69A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6E9340))
    if fn:
        fn.setName('GetAttackModifierVersus', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6E9340: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x723770))
    if fn:
        fn.setName('_CSWCollisionMesh', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCollisionMesh', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCollisionMesh', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x723770: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x567CF0))
    if fn:
        fn.setName('CSWGuiButtonToggle', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x567CF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x86B5B0))
    if fn:
        fn.setName('AurVertexProgramsAvailable', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x86B5B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x468D00))
    if fn:
        fn.setName('GetType', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWBehaviorCameraCombat', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWBehaviorCameraCombat', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x468D00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x503600))
    if fn:
        fn.setName('GetHardCodedEffectClass', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x503600: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4A67F0))
    if fn:
        fn.setName('HandleServerToPlayerCheatMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4A67F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4F05F0))
    if fn:
        fn.setName('CSWPlayerControlCamRelative', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPlayerControlCamRelative', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPlayerControlCamRelative', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4F05F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x61BDD0))
    if fn:
        fn.setName('GetResOfTypeFromTable', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoResMan', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x61BDD0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x827460))
    if fn:
        fn.setName('SetInterleavedBuffer', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('GLRender', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'GLRender', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x827460: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6301E0))
    if fn:
        fn.setName('GetKeysOfType', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6301E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x81B7A0))
    if fn:
        fn.setName('Parse_Vector_', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x81B7A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60D040))
    if fn:
        fn.setName('CExoStringList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoStringList', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoStringList', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60D040: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x61C280))
    if fn:
        fn.setName('GetResOfType', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoResMan', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x61C280: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8CEFC0))
    if fn:
        fn.setName('Parse_int_', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8CEFC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x61BF10))
    if fn:
        fn.setName('GetResOfType', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoResMan', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x61BF10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60D2F0))
    if fn:
        fn.setName('Merge', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoStringList', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoStringList', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60D2F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8680D0))
    if fn:
        fn.setName('AurResGetData', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8680D0: '+str(e)); errors += 1

print('renamed={} errors={}'.format(renamed, errors))