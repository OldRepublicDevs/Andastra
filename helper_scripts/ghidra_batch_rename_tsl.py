# Ghidra TSL rename script  (12 entries)
from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0

try:
    fn = fm.getFunctionAt(toAddr(0x40E4D0))
    if fn:
        fn.setName('FadeAndStopStreamingSoundSource', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40E4D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x42A670))
    if fn:
        fn.setName('GetReticlePoint', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x42A670: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x458AD0))
    if fn:
        fn.setName('givesitharmour', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x458AD0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x475A80))
    if fn:
        fn.setName('StopAllVisualEffects', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x475A80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5072C0))
    if fn:
        fn.setName('SetAnimationWithoutTrigger', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCDoor', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCDoor', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5072C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x54D3C0))
    if fn:
        fn.setName('PlaceAbove', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTutorial', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTutorial', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x54D3C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5EBDC0))
    if fn:
        fn.setName('CSWMGBehaviorTrackAndFire', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWMGBehaviorTrackAndFire', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWMGBehaviorTrackAndFire', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5EBDC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64BF60))
    if fn:
        fn.setName('RunModule', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64BF60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x775D20))
    if fn:
        fn.setName('AddSWSpellAction', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCombatRound', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x775D20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x775F40))
    if fn:
        fn.setName('AddSWItemSpellAction', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCombatRound', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x775F40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x776060))
    if fn:
        fn.setName('AddCutsceneMoveActions', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCombatRound', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCombatRound', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x776060: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x475B50))
    if fn:
        fn.setName('DeleteVisualEffect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x475B50: '+str(e)); errors += 1

print('renamed={} errors={}'.format(renamed, errors))