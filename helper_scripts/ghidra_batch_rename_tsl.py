# Ghidra TSL rename script  (136 entries)
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
    fn = fm.getFunctionAt(toAddr(0x774710))
    if fn:
        fn.setName('CheckActionLengthAtTime', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCombatRound', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x774710: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40D5D0))
    if fn:
        fn.setName('PlayMovie', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40D5D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x418F80))
    if fn:
        fn.setName('SetCreatureType', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x418F80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x41F840))
    if fn:
        fn.setName('ClearVisibilityList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x41F840: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4218A0))
    if fn:
        fn.setName('GetPauseReadyAnimation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4218A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x421970))
    if fn:
        fn.setName('IsInjured', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x421970: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x47D850))
    if fn:
        fn.setName('FindFurthestPartyMemeber_Current', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPartyFollowDataArray', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPartyFollowDataArray', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x47D850: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x490D90))
    if fn:
        fn.setName('AddSkillPointsPerLevel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCLevelUpStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCLevelUpStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x490D90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4C92D0))
    if fn:
        fn.setName('Hilite', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAnimBase', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4C92D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4C9350))
    if fn:
        fn.setName('UnHilite', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAnimBase', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4C9350: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4DBDA0))
    if fn:
        fn.setName('SetFade', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiDialogLetterbox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiDialogLetterbox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4DBDA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4ECBE0))
    if fn:
        fn.setName('_CSWItem', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWItem', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWItem', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4ECBE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4FAD60))
    if fn:
        fn.setName('_CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4FAD60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5042D0))
    if fn:
        fn.setName('AttachLight', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5042D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x510F60))
    if fn:
        fn.setName('GetRecommendedFeat', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWClass', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWClass', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x510F60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x511020))
    if fn:
        fn.setName('GetRecommendedSkill', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWClass', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWClass', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x511020: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x535D20))
    if fn:
        fn.setName('UnloadLight', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCPlaceable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x535D20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x55C670))
    if fn:
        fn.setName('AddObstacle', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWMiniGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWMiniGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x55C670: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x55EFF0))
    if fn:
        fn.setName('SetCamera', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWMiniGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWMiniGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x55EFF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x565C20))
    if fn:
        fn.setName('ClearMapNotes', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiMapHider', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiMapHider', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x565C20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x56AB00))
    if fn:
        fn.setName('_CSWGuiSlider', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiSlider', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiSlider', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x56AB00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5AE5D0))
    if fn:
        fn.setName('_CSWGuiDialog', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiDialog', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiDialog', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5AE5D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5E32E0))
    if fn:
        fn.setName('UsePlayerSidedeckCard', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPazaak', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPazaak', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5E32E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5F4E60))
    if fn:
        fn.setName('SetState', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiQuickPanel', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiQuickPanel', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5F4E60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x607640))
    if fn:
        fn.setName('ClearLocString', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoLocString', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoLocString', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x607640: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x636FF0))
    if fn:
        fn.setName('PlaySourceOn2DVoice', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x636FF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x63A910))
    if fn:
        fn.setName('GetListenerPosition', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSound', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x63A910: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x63CE70))
    if fn:
        fn.setName('Resume', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoStreamingSoundSourceInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoStreamingSoundSourceInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x63CE70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64C110))
    if fn:
        fn.setName('IsInjured', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64C110: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6C8800))
    if fn:
        fn.setName('CheckProficiencies', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6C8800: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7111A0))
    if fn:
        fn.setName('CSWWalkMeshHeader', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWWalkMeshHeader', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWWalkMeshHeader', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7111A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7E3980))
    if fn:
        fn.setName('SendServerToPlayerAmbientMusicChangeTrack', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7E3980: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7E3B20))
    if fn:
        fn.setName('SendServerToPlayerAmbientSoundLoopChange', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7E3B20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7EE1A0))
    if fn:
        fn.setName('UpdateActivateAreaList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEncounter', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7EE1A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x80BA90))
    if fn:
        fn.setName('cosineLUT', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x80BA90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x80C200))
    if fn:
        fn.setName('planelineintersection', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x80C200: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x80E4D0))
    if fn:
        fn.setName('polyhit', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x80E4D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x80F240))
    if fn:
        fn.setName('BoxIntersect', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x80F240: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x815520))
    if fn:
        fn.setName('SetIsEnvironmentMapped', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAurTexture', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAurTexture', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x815520: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8255C0))
    if fn:
        fn.setName('RequestPool', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('GLRender', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'GLRender', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8255C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x845590))
    if fn:
        fn.setName('_MdlNode', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('MdlNode', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'MdlNode', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x845590: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x853010))
    if fn:
        fn.setName('findallmaterials', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x853010: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x867970))
    if fn:
        fn.setName('internal_AurResGet', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x867970: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x868590))
    if fn:
        fn.setName('AurEnableHeadLight', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x868590: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x86AC20))
    if fn:
        fn.setName('AurQueryMaximumAnisotropy', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x86AC20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8B7440))
    if fn:
        fn.setName('LightningEmitter', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('LightningEmitter', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'LightningEmitter', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8B7440: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8D5370))
    if fn:
        fn.setName('FlushBuffer', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAurGUIImage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAurGUIImage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8D5370: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8D9AC0))
    if fn:
        fn.setName('Height2Channel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('TextureController', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'TextureController', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8D9AC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8DB2C0))
    if fn:
        fn.setName('FloatInterpolate', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8DB2C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8FDDE0))
    if fn:
        fn.setName('ResetVertexPrograms', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('VertexProgramManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'VertexProgramManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8FDDE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x90C8F0))
    if fn:
        fn.setName('Intersection', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiExtent', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiExtent', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x90C8F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9118A0))
    if fn:
        fn.setName('CaptureMouse', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9118A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9118E0))
    if fn:
        fn.setName('ReleaseMouse', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9118E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x912310))
    if fn:
        fn.setName('DisplayToolTip', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x912310: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x912600))
    if fn:
        fn.setName('RestoreOldMousePosition', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x912600: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9126B0))
    if fn:
        fn.setName('MoveMouseToPosition', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9126B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x914880))
    if fn:
        fn.setName('GetImageExtent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiImage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiImage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x914880: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x915550))
    if fn:
        fn.setName('SetColor', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTextParams', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTextParams', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x915550: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x915710))
    if fn:
        fn.setName('Initialize', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiText', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiText', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x915710: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x414210))
    if fn:
        fn.setName('CalculateSpellRangedMissTarge', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x414210: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x428C80))
    if fn:
        fn.setName('PartyMenuUseHostileItem', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x428C80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4290A0))
    if fn:
        fn.setName('RotateCreatureToOrientation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4290A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x42BA20))
    if fn:
        fn.setName('PlayRollingFootstepSound', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x42BA20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x477C50))
    if fn:
        fn.setName('SetCombatMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPartyMemberData', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPartyMemberData', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x477C50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4BFE80))
    if fn:
        fn.setName('HandleServerToPlayerPopUpGUIPanel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4BFE80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4E8840))
    if fn:
        fn.setName('SetTarget', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCameraDeath', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCameraDeath', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4E8840: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x540B20))
    if fn:
        fn.setName('_CSWTrackFollower', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWTrackFollower', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x540B20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x550DF0))
    if fn:
        fn.setName('HandleQuitDialog', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiPazaakStart', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiPazaakStart', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x550DF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x555660))
    if fn:
        fn.setName('HandleHandcardAction', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiPazaakGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiPazaakGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x555660: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x55B6B0))
    if fn:
        fn.setName('FadeAndStop', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCSoundObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x55B6B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x55C240))
    if fn:
        fn.setName('CSWCProjectile', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCProjectile', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x55C240: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x56CF00))
    if fn:
        fn.setName('SetNewOptions', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiOptionsGraphicsAdvanced', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiOptionsGraphicsAdvanced', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x56CF00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5D7EF0))
    if fn:
        fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiSaveNamePanel', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiSaveNamePanel', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5D7EF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x615EC0))
    if fn:
        fn.setName('_mbStringOpsInternal_00709462', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x615EC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7EA490))
    if fn:
        fn.setName('TallyEnemyRadiusPoints', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEncounter', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7EA490: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x88A4A0))
    if fn:
        fn.setName('RenderEMBM', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x88A4A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8D7750))
    if fn:
        fn.setName('Write', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoFileInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoFileInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8D7750: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9069E0))
    if fn:
        fn.setName('__displayRuntimeError', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9069E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x919F50))
    if fn:
        fn.setName('DrawMap', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiMainInterface', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x919F50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x92475D))
    if fn:
        fn.setName('_acos', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x92475D: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7B6860))
    if fn:
        fn.setName('SetEstimatedSaveSize', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7B6860: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6DD200))
    if fn:
        fn.setName('AIActionCounterSpell', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6DD200: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4282C0))
    if fn:
        fn.setName('CreateUsableItemEntry', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4282C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x428490))
    if fn:
        fn.setName('GetUsableItems', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x428490: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5247C0))
    if fn:
        fn.setName('CreateEntry', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5247C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x536450))
    if fn:
        fn.setName('OnControlInvAButton', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x536450: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5B78D0))
    if fn:
        fn.setName('CSWGuiScriptSelect', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5B78D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4DE610))
    if fn:
        fn.setName('ShowSoloModeQuery', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4DE610: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4CE630))
    if fn:
        fn.setName('_CSWGuiButton', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4CE630: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x542C90))
    if fn:
        fn.setName('GetPosition', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x542C90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x857A30))
    if fn:
        fn.setName('ClearOutput', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x857A30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7E44A0))
    if fn:
        fn.setName('remove', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7E44A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x873000))
    if fn:
        fn.setName('operator_', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x873000: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x80E0C0))
    if fn:
        fn.setName('_PartSkin', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('PartSkin', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'PartSkin', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x80E0C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x80EC30))
    if fn:
        fn.setName('freeProjectTBones', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('PartSkin', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'PartSkin', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x80EC30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x848300))
    if fn:
        fn.setName('freeProjectQBones', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('PartSkin', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'PartSkin', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x848300: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x86B4A0))
    if fn:
        fn.setName('freeTBones', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('PartSkin', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'PartSkin', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x86B4A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x873F10))
    if fn:
        fn.setName('freeQBones', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('PartSkin', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'PartSkin', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x873F10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5400B0))
    if fn:
        fn.setName('_CSWGuiLoadScreen', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiLoadScreen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiLoadScreen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5400B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5DE9D0))
    if fn:
        fn.setName('OnEnterButton', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiClassSelection', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiClassSelection', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5DE9D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4EA9D0))
    if fn:
        fn.setName('_CAurBehaviorAttach', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAurBehaviorAttach', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAurBehaviorAttach', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4EA9D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8557B0))
    if fn:
        fn.setName('_CAurBehaviorAttach', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAurBehaviorAttach', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAurBehaviorAttach', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8557B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8557F0))
    if fn:
        fn.setName('_CAurBehaviorAttach', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAurBehaviorAttach', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAurBehaviorAttach', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8557F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8C46A0))
    if fn:
        fn.setName('GetTargetWorldPosition', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAurBehaviorCameraFollow', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAurBehaviorCameraFollow', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8C46A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8C52E0))
    if fn:
        fn.setName('_CAurBehaviorCameraFollow', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAurBehaviorCameraFollow', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAurBehaviorCameraFollow', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8C52E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x80BAD0))
    if fn:
        fn.setName('ArturoTextureController', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('ArturoTextureController', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'ArturoTextureController', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x80BAD0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8DC0B0))
    if fn:
        fn.setName('_ArturoTextureController', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('ArturoTextureController', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'ArturoTextureController', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8DC0B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8F4340))
    if fn:
        fn.setName('_Part', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('Part', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'Part', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8F4340: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x456CE0))
    if fn:
        fn.setName('RemovePlaceableObjectLight', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x456CE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x48F190))
    if fn:
        fn.setName('UpdateAbilityModifiedSkillRanks', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x48F190: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4FC8D0))
    if fn:
        fn.setName('UnloadVisualEffect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4FC8D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60D880))
    if fn:
        fn.setName('ClearLocString', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoLocStringInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoLocStringInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60D880: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x81B990))
    if fn:
        fn.setName('AddUnique', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoArrayList', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoArrayList', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x81B990: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8D5540))
    if fn:
        fn.setName('NewCAurGUIString', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8D5540: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x91F410))
    if fn:
        fn.setName('SetFocus', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiEditbox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiEditbox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x91F410: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4244D0))
    if fn:
        fn.setName('CanSee', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4244D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5C63C0))
    if fn:
        fn.setName('SellItemCallback', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiStore', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5C63C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5D7FE0))
    if fn:
        fn.setName('OnPanelRemoved', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiSaveNamePanel', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiSaveNamePanel', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5D7FE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5F4E20))
    if fn:
        fn.setName('ConfirmCancel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiQuickPanel', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiQuickPanel', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5F4E20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6C83D0))
    if fn:
        fn.setName('CanEquipMiscellaneous', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6C83D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x713550))
    if fn:
        fn.setName('CheckAABBNode', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWRoomSurfaceMesh', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWRoomSurfaceMesh', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x713550: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8441B0))
    if fn:
        fn.setName('allocate', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoArrayList<CExoArrayList<CAurTriangleBin*>>', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoArrayList<CExoArrayList<CAurTriangleBin*>>', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8441B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x86E830))
    if fn:
        fn.setName('HitPartOnBSPNode', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x86E830: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x91EA00))
    if fn:
        fn.setName('HandleLMouseUp', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiListBox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x91EA00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8533E0))
    if fn:
        fn.setName('ReplaceTextureSubtree', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8533E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8D6940))
    if fn:
        fn.setName('Read', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoFileInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoFileInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8D6940: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8D6A10))
    if fn:
        fn.setName('Read', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoFileInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoFileInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8D6A10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8D6A70))
    if fn:
        fn.setName('ReadAsyncComplete', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoFileInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoFileInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8D6A70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9164F0))
    if fn:
        fn.setName('_CSWGui3DSceneView', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGui3DSceneView', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGui3DSceneView', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9164F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4FE0D0))
    if fn:
        fn.setName('KillVisualEffect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4FE0D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5DE630))
    if fn:
        fn.setName('_CSWGuiInGameGalaxyMap', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameGalaxyMap', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameGalaxyMap', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5DE630: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x610220))
    if fn:
        fn.setName('InitializeSource', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoStreamingSoundSourceInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoStreamingSoundSourceInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x610220: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x819E40))
    if fn:
        fn.setName('AurTextureSetAttributes', SourceType.USER_DEFINED)
        renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x819E40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x501AD0))
    if fn:
        fn.setName('RemoveHardCodedVisualEffect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x501AD0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x53BFF0))
    if fn:
        fn.setName('_CSWGuiMainCharGen', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiMainCharGen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiMainCharGen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x53BFF0: '+str(e)); errors += 1

print('renamed={} errors={}'.format(renamed, errors))