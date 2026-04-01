# Ghidra TSL rename script  (676 entries)
from ghidra.program.model.symbol import SourceType
fm = currentProgram.getFunctionManager()
sm = currentProgram.getSymbolTable()
renamed = 0; errors = 0

try:
    fn = fm.getFunctionAt(toAddr(0x42BC30))
    if fn:
        fn.setName('GetCreaturePathfindInformation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x42BC30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x43AB70))
    if fn:
        fn.setName('AddSimpleObjectModelDirectlyToIgnoreList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x43AB70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x454660))
    if fn:
        fn.setName('SetWeather', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x454660: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x474B00))
    if fn:
        fn.setName('AddFireForgetAnimation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x474B00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x47E830))
    if fn:
        fn.setName('NVThunkSWCItmSWCObject_LoadModel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x47E830: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x48C1E0))
    if fn:
        fn.setName('GetAnimType', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreatureAppearance', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreatureAppearance', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x48C1E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x494DC0))
    if fn:
        fn.setName('GetPortrait', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x494DC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4E8110))
    if fn:
        fn.setName('UpdateCamraStyle', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCameraFreeLook', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCameraFreeLook', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4E8110: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4F3E80))
    if fn:
        fn.setName('HideHeadItem', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAnimBaseHeadWield', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAnimBaseHeadWield', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4F3E80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x50F380))
    if fn:
        fn.setName('LoadFeatGain', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWClass', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWClass', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x50F380: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x513BF0))
    if fn:
        fn.setName('SetFriend', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x513BF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5403B0))
    if fn:
        fn.setName('AddHintToMessageBuffer', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiLoadScreen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiLoadScreen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5403B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x541250))
    if fn:
        fn.setName('Update', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWTrackFollower', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x541250: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x55C300))
    if fn:
        fn.setName('UpdateVolumeVariation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCSoundObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x55C300: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x562100))
    if fn:
        fn.setName('SpawnRoom', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCRoom', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCRoom', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x562100: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x56B430))
    if fn:
        fn.setName('OnAdvanced', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiOptionsGraphics', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiOptionsGraphics', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x56B430: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x586860))
    if fn:
        fn.setName('SetMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTutorialBox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTutorialBox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x586860: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x589DF0))
    if fn:
        fn.setName('HandleInput', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiSkillFlowChart', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiSkillFlowChart', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x589DF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5994B0))
    if fn:
        fn.setName('Draw', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameCharacter', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameCharacter', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5994B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5DB120))
    if fn:
        fn.setName('GetAnimationName', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAnimBaseTrigger', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAnimBaseTrigger', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5DB120: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5EC150))
    if fn:
        fn.setName('Control', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAurBehaviorAttachTranslate', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAurBehaviorAttachTranslate', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5EC150: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5F2E20))
    if fn:
        fn.setName('OnSelectPowersButton', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiLevelUpPanel', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiLevelUpPanel', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5F2E20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5F42F0))
    if fn:
        fn.setName('OnSelectFeatsButton', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiCustomPanel', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiCustomPanel', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5F42F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x603F10))
    if fn:
        fn.setName('OnSelectDisplayButton', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiPortraitCharGen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiPortraitCharGen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x603F10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x604730))
    if fn:
        fn.setName('Draw', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiPortraitCharGen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiPortraitCharGen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x604730: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60CF00))
    if fn:
        fn.setName('Introduce', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoStringList', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoStringList', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60CF00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60DDA0))
    if fn:
        fn.setName('CExoTimersInternal', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoTimersInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoTimersInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60DDA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60F8E0))
    if fn:
        fn.setName('ReadAsync', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoFileInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoFileInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60F8E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x611C00))
    if fn:
        fn.setName('CExoInputClass', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoInputClass', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoInputClass', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x611C00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x617080))
    if fn:
        fn.setName('HandleImeStatusChange', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoImeUI', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoImeUI', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x617080: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x61AF60))
    if fn:
        fn.setName('AddResourceImageFilesAsync', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoResMan', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x61AF60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x62B170))
    if fn:
        fn.setName('Create', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CERFFile', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CERFFile', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x62B170: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6380C0))
    if fn:
        fn.setName('LoadPriorityGroups', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6380C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64C5C0))
    if fn:
        fn.setName('ClearAllQueuedCombatActions', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64C5C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x65AAE0))
    if fn:
        fn.setName('GetWeaponDischargeTime', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x65AAE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x65F950))
    if fn:
        fn.setName('ResolveDamageShields', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x65F950: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x66C760))
    if fn:
        fn.setName('GetDamageTypeString', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSItem', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x66C760: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x693B40))
    if fn:
        fn.setName('GetFeatRemainingUses', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x693B40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6FD5A0))
    if fn:
        fn.setName('GetPartyItems', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPartyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6FD5A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x703650))
    if fn:
        fn.setName('AddGameInProgress', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPartyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x703650: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7036E0))
    if fn:
        fn.setName('RemoveGameInProgress', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPartyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7036E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x722F60))
    if fn:
        fn.setName('EnforceNoPCHeadCloning', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x722F60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x74C8D0))
    if fn:
        fn.setName('IsEffectToIgnoreOnDeath', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEffectListHandler', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x74C8D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x74D050))
    if fn:
        fn.setName('CSWSDialog', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSDialog', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSDialog', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x74D050: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x75DA10))
    if fn:
        fn.setName('CalculateNearestPoint', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSTrigger', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x75DA10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7672C0))
    if fn:
        fn.setName('LoadFromTemplate', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSDoor', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7672C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7C43D0))
    if fn:
        fn.setName('RemoveAttackBonus', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7C43D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F00E0))
    if fn:
        fn.setName('GetString', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSScriptVarTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSScriptVarTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F00E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F6B70))
    if fn:
        fn.setName('LoadFromTemplate', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSSoundObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F6B70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x80B0B0))
    if fn:
        fn.setName('SetWorldTime', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CWorldTimer', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x80B0B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x811600))
    if fn:
        fn.setName('LoadNameTable', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWNameGen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWNameGen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x811600: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x85A640))
    if fn:
        fn.setName('DoGobBuckets', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('Scene', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'Scene', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x85A640: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8904A0))
    if fn:
        fn.setName('Draw', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('PartAnimMesh', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'PartAnimMesh', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8904A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x89A850))
    if fn:
        fn.setName('RenderBlur', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('Gob', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'Gob', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x89A850: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8AEED0))
    if fn:
        fn.setName('updateParticleFrame', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('PartEmitter', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'PartEmitter', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8AEED0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x409000))
    if fn:
        fn.setName('messagepump', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x409000: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40CD90))
    if fn:
        fn.setName('AddObjectToHitcheckIgnoreList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40CD90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40CDB0))
    if fn:
        fn.setName('RemoveObjectFromHitcheckIgnoreList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40CDB0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40D090))
    if fn:
        fn.setName('GetHitCheckMaterialMask', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40D090: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40D8E0))
    if fn:
        fn.setName('SetCanSendDialog', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40D8E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40D960))
    if fn:
        fn.setName('SetDialogModelsSent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40D960: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40E7C0))
    if fn:
        fn.setName('GetDisplayCharacterForKey', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40E7C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x416C70))
    if fn:
        fn.setName('GetWayPoints', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x416C70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x41AC10))
    if fn:
        fn.setName('GetLastInstant', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x41AC10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x42AC40))
    if fn:
        fn.setName('Setup3DScene', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x42AC40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x454600))
    if fn:
        fn.setName('StartSounds', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x454600: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x475510))
    if fn:
        fn.setName('GetFinalFadeAlpha', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x475510: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4776A0))
    if fn:
        fn.setName('ClearAllQueuedCombatActions', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4776A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x48F020))
    if fn:
        fn.setName('ClearSkills', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x48F020: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x496520))
    if fn:
        fn.setName('CSWCMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x496520: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4D4570))
    if fn:
        fn.setName('InitializeShotCamera', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4D4570: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4DC4B0))
    if fn:
        fn.setName('SetPaused', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4DC4B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4DEE40))
    if fn:
        fn.setName('SetAreaTransition', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4DEE40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4E15C0))
    if fn:
        fn.setName('FadeAndStopDialogAmbientTrack', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4E15C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4EADA0))
    if fn:
        fn.setName('GetModelResRef', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWBaseItem', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWBaseItem', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4EADA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4F0350))
    if fn:
        fn.setName('SetEnabled', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPlayerControl', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPlayerControl', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4F0350: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x510DC0))
    if fn:
        fn.setName('IsGrantedFeat', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWClass', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWClass', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x510DC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x520090))
    if fn:
        fn.setName('ShowActionQueue', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiMainInterface', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x520090: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x549AC0))
    if fn:
        fn.setName('FindOptionIndex', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerInfo', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerInfo', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x549AC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x549B80))
    if fn:
        fn.setName('SetModuleName', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerInfo', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerInfo', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x549B80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x54B460))
    if fn:
        fn.setName('ReadVOIDPtr', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x54B460: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x54C140))
    if fn:
        fn.setName('CSWGuiTutorial', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTutorial', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTutorial', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x54C140: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x573A10))
    if fn:
        fn.setName('DetermineSafePosition', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWBehaviorCameraCombat', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWBehaviorCameraCombat', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x573A10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x57B560))
    if fn:
        fn.setName('SetProjectileVelAndAccel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCProjectile', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x57B560: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5873D0))
    if fn:
        fn.setName('CSWGuiSkillFlow', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiSkillFlow', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiSkillFlow', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5873D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x59BF40))
    if fn:
        fn.setName('SetTopVisible', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiListBox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x59BF40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x62FC90))
    if fn:
        fn.setName('BuildNewTable', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoKeyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x62FC90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x638930))
    if fn:
        fn.setName('LoadProviderPreferences', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x638930: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x63CF80))
    if fn:
        fn.setName('Update3DStream', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoStreamingSoundSourceInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoStreamingSoundSourceInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x63CF80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x642870))
    if fn:
        fn.setName('CVirtualMachineScript', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CVirtualMachineScript', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CVirtualMachineScript', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x642870: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x647090))
    if fn:
        fn.setName('Shutdown', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CPathfindInformation', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CPathfindInformation', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x647090: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64BE20))
    if fn:
        fn.setName('GetDebugMode', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64BE20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64C210))
    if fn:
        fn.setName('GetActiveExclusionList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64C210: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x65DF60))
    if fn:
        fn.setName('ResolveDefensiveEffects', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x65DF60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x667090))
    if fn:
        fn.setName('UpdateUsedActiveProperties', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSItem', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x667090: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x66AFC0))
    if fn:
        fn.setName('GetPropertyDescription', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSItem', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x66AFC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x67CF00))
    if fn:
        fn.setName('ComputeSpellRange', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x67CF00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6AD730))
    if fn:
        fn.setName('IsSoundPlayingInDialog', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6AD730: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6DB480))
    if fn:
        fn.setName('CanUnEquipWeapon', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6DB480: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6F6D70))
    if fn:
        fn.setName('GetIsInSpellLikeAbilityList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6F6D70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6F9C90))
    if fn:
        fn.setName('GetSpellLikeAbilityCasterLevel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6F9C90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6FDA30))
    if fn:
        fn.setName('UpdateMembers', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPartyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6FDA30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x706AC0))
    if fn:
        fn.setName('ComputeNonVisibleLocation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x706AC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x707350))
    if fn:
        fn.setName('IntersectingLineSegment', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x707350: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x714000))
    if fn:
        fn.setName('ComputeClippedLineSegment', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWRoomSurfaceMesh', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWRoomSurfaceMesh', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x714000: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7184D0))
    if fn:
        fn.setName('IncrementPlayersInArea', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7184D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7221C0))
    if fn:
        fn.setName('NoNonWalkPolys', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7221C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x74CA90))
    if fn:
        fn.setName('AddUnique', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoArrayList<LARGE_INTEGER>', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoArrayList<LARGE_INTEGER>', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x74CA90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x778730))
    if fn:
        fn.setName('IsWorldPointExplored', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSAreaMap', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSAreaMap', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x778730: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x778B80))
    if fn:
        fn.setName('GetQuestEntryXP', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSJournal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSJournal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x778B80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x778BF0))
    if fn:
        fn.setName('GetState', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSJournal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSJournal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x778BF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7C15D0))
    if fn:
        fn.setName('AddItemToInventory', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSStore', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7C15D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7CD8A0))
    if fn:
        fn.setName('SendServerToPlayerLogin_NeedCharacter', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7CD8A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7CE400))
    if fn:
        fn.setName('SendServerToPlayerCharList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7CE400: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7E3B90))
    if fn:
        fn.setName('SendServerToPlayerAmbientSoundVolumeChange', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7E3B90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7E75E0))
    if fn:
        fn.setName('SetShape', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7E75E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7EF670))
    if fn:
        fn.setName('LoadReputations', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CFactionManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CFactionManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7EF670: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F1220))
    if fn:
        fn.setName('DeleteList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSExpressionList', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSExpressionList', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F1220: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F39B0))
    if fn:
        fn.setName('_CSWLevelStats', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWLevelStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWLevelStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F39B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x80B310))
    if fn:
        fn.setName('ResetTimer', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CWorldTimer', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x80B310: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x80B580))
    if fn:
        fn.setName('UnpauseWorldTimer', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CWorldTimer', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CWorldTimer', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x80B580: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x868320))
    if fn:
        fn.setName('dofile', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x868320: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8D5470))
    if fn:
        fn.setName('NewCAurGUIImage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8D5470: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8F5230))
    if fn:
        fn.setName('ReplaceTexture', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('Material', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'Material', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8F5230: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9147A0))
    if fn:
        fn.setName('Initialize', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiImage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiImage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9147A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x91AB60))
    if fn:
        fn.setName('HitCheckScrollbar', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiScrollBar', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiScrollBar', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x91AB60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x410200))
    if fn:
        fn.setName('CSWCCreaturePartyStatus', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreaturePartyStatus', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreaturePartyStatus', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x410200: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4771C0))
    if fn:
        fn.setName('NewVocalization', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4771C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4D1300))
    if fn:
        fn.setName('SetReplyData', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4D1300: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4D1550))
    if fn:
        fn.setName('GetCameraAnimationName', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4D1550: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4ECC30))
    if fn:
        fn.setName('CSWSpell', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSpell', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSpell', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4ECC30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5068C0))
    if fn:
        fn.setName('CSWCDoor', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCDoor', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCDoor', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5068C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5347D0))
    if fn:
        fn.setName('CSWCPlaceable', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCPlaceable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5347D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5371E0))
    if fn:
        fn.setName('SetAppearance', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCPlaceable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5371E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5380E0))
    if fn:
        fn.setName('SetCursor', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCTrigger', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCTrigger', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5380E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x53ECD0))
    if fn:
        fn.setName('GetTokenPair', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientMsgSysAdmin', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientMsgSysAdmin', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x53ECD0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x55AA20))
    if fn:
        fn.setName('CSWCSoundObject', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCSoundObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x55AA20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x61D550))
    if fn:
        fn.setName('ServiceFromImage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoResMan', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x61D550: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x62C910))
    if fn:
        fn.setName('CExoKeyTable', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoKeyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x62C910: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x633790))
    if fn:
        fn.setName('CExoArrayList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoArrayList<CExoString>', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoArrayList<CExoString>', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x633790: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x638020))
    if fn:
        fn.setName('AddToNonPlayingStreamingLoopingList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x638020: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7E6110))
    if fn:
        fn.setName('Initialise', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSFaction', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7E6110: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F3B00))
    if fn:
        fn.setName('SetSkillRankChange', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWLevelStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWLevelStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F3B00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40EC10))
    if fn:
        fn.setName('StringCopy', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoString', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40EC10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4C1A40))
    if fn:
        fn.setName('HandleServerToPlayerEndGame', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4C1A40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x640F80))
    if fn:
        fn.setName('_CScriptCompilerIncludeFileStackEntry', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CScriptCompilerIncludeFileStackEntry', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CScriptCompilerIncludeFileStackEntry', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x640F80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x73F2A0))
    if fn:
        fn.setName('OnRemoveArcaneSpellFailure', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x73F2A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7B4920))
    if fn:
        fn.setName('Initialize', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7B4920: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x878D00))
    if fn:
        fn.setName('GetSkinSphereMapVP', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x878D00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8D7FC0))
    if fn:
        fn.setName('AurResGetLast', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8D7FC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40D0B0))
    if fn:
        fn.setName('SetVideoMode', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40D0B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40DF30))
    if fn:
        fn.setName('DoVFXRumble', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffect', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffect', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40DF30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x445190))
    if fn:
        fn.setName('AddMoveToModuleMovie', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x445190: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x454D00))
    if fn:
        fn.setName('SetFog', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x454D00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4581E0))
    if fn:
        fn.setName('setstrength', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4581E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x458680))
    if fn:
        fn.setName('revealmap', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x458680: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x472AB0))
    if fn:
        fn.setName('SetLoopingAnimation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x472AB0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x475420))
    if fn:
        fn.setName('RemoveAllFireForgetAnimations', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x475420: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x477070))
    if fn:
        fn.setName('SetGroundNormal', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x477070: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4773C0))
    if fn:
        fn.setName('QueuedCallbackFunction', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4773C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x477520))
    if fn:
        fn.setName('ClientServerSyncHandler_Animation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x477520: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4F4DE0))
    if fn:
        fn.setName('NVThunkHeadWieldAnimBase_ShowHeadItem', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4F4DE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4F4E10))
    if fn:
        fn.setName('NVThunkHeadWieldAnimBase_SetAnimation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4F4E10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5999F0))
    if fn:
        fn.setName('CameraFreeStyleControl', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCameraOnAStick', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCameraOnAStick', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5999F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60CC30))
    if fn:
        fn.setName('FlushScriptLogFile', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoDebug', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoDebug', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60CC30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x63F130))
    if fn:
        fn.setName('HurryDeathSequence', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x63F130: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64A240))
    if fn:
        fn.setName('IsPathClear', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAvoidCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAvoidCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64A240: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64C8D0))
    if fn:
        fn.setName('GetNormalOfPoint', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64C8D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6699D0))
    if fn:
        fn.setName('GetItemCount', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSPlaceable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6699D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x67D2F0))
    if fn:
        fn.setName('AIActionCheckForceFollowObject', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x67D2F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6B5880))
    if fn:
        fn.setName('ClearActivities', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6B5880: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6C9D80))
    if fn:
        fn.setName('AIActionAreaWait', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6C9D80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7CA150))
    if fn:
        fn.setName('WriteGameObjUpdate_WorkRemaining', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7CA150: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F61A0))
    if fn:
        fn.setName('CSWSSoundObject', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSSoundObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F61A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x843670))
    if fn:
        fn.setName('InternalPostProcess', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('MdlNodeEmitter', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'MdlNodeEmitter', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x843670: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x84C420))
    if fn:
        fn.setName('GetCutsceneDummyPosition', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('Gob', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'Gob', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x84C420: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8F3CDB))
    if fn:
        fn.setName('Catch_0047bffd', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8F3CDB: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x907780))
    if fn:
        fn.setName('GetProcessedTextureDataSize', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAuroraProcessedTexture', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAuroraProcessedTexture', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x907780: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x910360))
    if fn:
        fn.setName('HideAllPanels', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x910360: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x91B5A0))
    if fn:
        fn.setName('SetSelectOnLeftClick', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiListBox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x91B5A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x41CEC0))
    if fn:
        fn.setName('SetPortraitId', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x41CEC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x42C320))
    if fn:
        fn.setName('CSWLevelStats', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWLevelStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWLevelStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x42C320: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x456350))
    if fn:
        fn.setName('DeleteCurrentAIAction', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x456350: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x51C2C0))
    if fn:
        fn.setName('_CScriptCompilerIdListEntry', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CScriptCompilerIdListEntry', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CScriptCompilerIdListEntry', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x51C2C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x53A5F0))
    if fn:
        fn.setName('LoadScripts', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWMiniGameObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWMiniGameObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x53A5F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x58A510))
    if fn:
        fn.setName('_CSWGuiUpgrade', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiUpgrade', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x58A510: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x639A50))
    if fn:
        fn.setName('CExoSoundSource', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundSource', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundSource', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x639A50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x730CA0))
    if fn:
        fn.setName('NWPlayerCharacterList_st', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('NWPlayerCharacterList_st', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'NWPlayerCharacterList_st', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x730CA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x75CB00))
    if fn:
        fn.setName('GetPlayerName', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSPlayer', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x75CB00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7ED360))
    if fn:
        fn.setName('ReadEncounterScriptsFromGff', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEncounter', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7ED360: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x846160))
    if fn:
        fn.setName('ParseFieldDispatch', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('MdlNode', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'MdlNode', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x846160: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x914FA0))
    if fn:
        fn.setName('operator_', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTextParams', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTextParams', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x914FA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x92BA03))
    if fn:
        fn.setName('_FindAndUnlinkFrame', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x92BA03: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40B8D0))
    if fn:
        fn.setName('toggleFullScreen', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40B8D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40CB60))
    if fn:
        fn.setName('SetPauseState', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40CB60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40D000))
    if fn:
        fn.setName('StopServices', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40D000: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40D110))
    if fn:
        fn.setName('GetItemDropSound', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40D110: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40D5B0))
    if fn:
        fn.setName('PlayLoadModuleMovies', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40D5B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x41F7E0))
    if fn:
        fn.setName('SetSpecialWalkAnimation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x41F7E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x41F810))
    if fn:
        fn.setName('SetRunLooksAwful', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x41F810: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x456260))
    if fn:
        fn.setName('SetLightningDone', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x456260: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x474890))
    if fn:
        fn.setName('AnimationPlayOutOfPlace', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x474890: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x477390))
    if fn:
        fn.setName('GetFeedbackInfo', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x477390: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x54D4E0))
    if fn:
        fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiMainMenu', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiMainMenu', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x54D4E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x55B710))
    if fn:
        fn.setName('ChangeVolume', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCSoundObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x55B710: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x61B890))
    if fn:
        fn.setName('Exists', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoResMan', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x61B890: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6271C0))
    if fn:
        fn.setName('CEncounterListEntry', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CEncounterListEntry', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CEncounterListEntry', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6271C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x630390))
    if fn:
        fn.setName('GetTableIndex', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoKeyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x630390: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x637A70))
    if fn:
        fn.setName('GetNext', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoLinkedList', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoLinkedList', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x637A70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x63A0E0))
    if fn:
        fn.setName('GetPlayFailed', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoStreamingSoundSource', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoStreamingSoundSource', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x63A0E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x63AAA0))
    if fn:
        fn.setName('StopAllOneShots', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSound', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x63AAA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x63F620))
    if fn:
        fn.setName('DoObstructionWork', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundSourceInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x63F620: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64BF40))
    if fn:
        fn.setName('LoadModule', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64BF40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64BFE0))
    if fn:
        fn.setName('StartServices', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64BFE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64C070))
    if fn:
        fn.setName('InitiateModuleForPlayer', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64C070: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64C230))
    if fn:
        fn.setName('ResolvePlayerByFirstName', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64C230: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64CAD0))
    if fn:
        fn.setName('SetSaveGameName', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64CAD0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x650A80))
    if fn:
        fn.setName('CSWGlobalVariableTable', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGlobalVariableTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGlobalVariableTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x650A80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x67A880))
    if fn:
        fn.setName('PutItemInSlot', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWInventory', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWInventory', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x67A880: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6A3B70))
    if fn:
        fn.setName('RemovePazaakCard', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6A3B70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6AB6F0))
    if fn:
        fn.setName('RemoveSomeEffectsOfDurationType', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6AB6F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6DD5D0))
    if fn:
        fn.setName('AIActionSurrenderToEnemies', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6DD5D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x723E00))
    if fn:
        fn.setName('GetVertex', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCollisionMesh', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCollisionMesh', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x723E00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x771220))
    if fn:
        fn.setName('CSWSSpellScriptData', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSSpellScriptData', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSSpellScriptData', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x771220: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7CA6E0))
    if fn:
        fn.setName('ComputeAppearanceUpdateRequired', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7CA6E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7E6010))
    if fn:
        fn.setName('GetFactionMember', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSFaction', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSFaction', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7E6010: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F79D0))
    if fn:
        fn.setName('CResBWM', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CResBWM', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CResBWM', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F79D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F7AF0))
    if fn:
        fn.setName('IsBinary', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CResBWM', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CResBWM', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F7AF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x809D00))
    if fn:
        fn.setName('ParseToken', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x809D00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8630E0))
    if fn:
        fn.setName('ProcessStaticObject', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8630E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x921FE0))
    if fn:
        fn.setName('inline_unlock_file', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x921FE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9224D0))
    if fn:
        fn.setName('inline_unlock_file', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9224D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x924711))
    if fn:
        fn.setName('inline_unlock_file', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x924711: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x926253))
    if fn:
        fn.setName('inline_unlock_12', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x926253: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x926E81))
    if fn:
        fn.setName('inline_unlock_12', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x926E81: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x92888B))
    if fn:
        fn.setName('inline_unlock_file', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x92888B: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x92A42D))
    if fn:
        fn.setName('__isatty', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x92A42D: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x92DC34))
    if fn:
        fn.setName('inline_unlock_fhandle', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x92DC34: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x92E4E3))
    if fn:
        fn.setName('inline_unlock_fhandle', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x92E4E3: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x92E811))
    if fn:
        fn.setName('inline_unlock_fhandle', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x92E811: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x92EB91))
    if fn:
        fn.setName('inline_unlock_fhandle', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x92EB91: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9324E4))
    if fn:
        fn.setName('inline_conditional_unlock_0', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9324E4: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9327C6))
    if fn:
        fn.setName('inline_unlock_4', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9327C6: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9393E4))
    if fn:
        fn.setName('inline_unlock_fhandle', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9393E4: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x44DD60))
    if fn:
        fn.setName('ComputeLastUpdate_StoreUpdateSpellLikeAbility', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x44DD60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x59F490))
    if fn:
        fn.setName('GetSlotItemIdIndex', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameEquip', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x59F490: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x61C890))
    if fn:
        fn.setName('Malloc', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoResMan', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x61C890: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x63A410))
    if fn:
        fn.setName('SetMusicDelay', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAmbientSound', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x63A410: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6A6760))
    if fn:
        fn.setName('AddActionNodeParameter', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6A6760: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6F2EA0))
    if fn:
        fn.setName('GetExpNeededForLevelUp', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6F2EA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6F9280))
    if fn:
        fn.setName('ComputeNumberKnownSpellsLeft', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6F9280: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6FA2B0))
    if fn:
        fn.setName('CheckInclusionMatch', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6FA2B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7747C0))
    if fn:
        fn.setName('GetCurrentlyFollowingLeader', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7747C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x912DC0))
    if fn:
        fn.setName('Show', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiMainInterfaceChar', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiMainInterfaceChar', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x912DC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60BD10))
    if fn:
        fn.setName('GetAtPos', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoLinkedListInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60BD10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60BDD0))
    if fn:
        fn.setName('GetPrev', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoLinkedListInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60BDD0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60BE90))
    if fn:
        fn.setName('RemoveTail', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoLinkedListInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60BE90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60BFC0))
    if fn:
        fn.setName('Contains', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoLinkedListInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoLinkedListInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60BFC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x668850))
    if fn:
        fn.setName('RemoveItem', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSStore', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x668850: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x66A2B0))
    if fn:
        fn.setName('EventHandler', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSStore', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x66A2B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7C1750))
    if fn:
        fn.setName('_CSWSStore', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSStore', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSStore', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7C1750: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40C950))
    if fn:
        fn.setName('GetModuleCamera', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40C950: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40CEC0))
    if fn:
        fn.setName('CreatureAcquireItem', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40CEC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40DD80))
    if fn:
        fn.setName('operator_new', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40DD80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x41ADE0))
    if fn:
        fn.setName('SetEquippedBy', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCItem', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCItem', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x41ADE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x422D70))
    if fn:
        fn.setName('SwitchMode', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x422D70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x42B180))
    if fn:
        fn.setName('GetClientOptions', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x42B180: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x44E510))
    if fn:
        fn.setName('_CExoString', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoString', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x44E510: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4571A0))
    if fn:
        fn.setName('GetCreatureLight', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4571A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4729F0))
    if fn:
        fn.setName('CExoString', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoString', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4729F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x472AE0))
    if fn:
        fn.setName('GetLoopingAnimation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x472AE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x47DF90))
    if fn:
        fn.setName('GetCreatureByGameObjectID', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x47DF90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x48FC50))
    if fn:
        fn.setName('_free', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x48FC50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4D70B0))
    if fn:
        fn.setName('CloseDialog', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4D70B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4F32F0))
    if fn:
        fn.setName('operator_', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoString', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4F32F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x534110))
    if fn:
        fn.setName('CExoString', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoString', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x534110: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x586770))
    if fn:
        fn.setName('CSWGuiMessageBox', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiMessageBox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiMessageBox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x586770: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x58A600))
    if fn:
        fn.setName('GetInGameGui', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x58A600: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5D6910))
    if fn:
        fn.setName('CResRef', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CResRef', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5D6910: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x627790))
    if fn:
        fn.setName('_eh_vector_destructor_iterator_', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x627790: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x62CA40))
    if fn:
        fn.setName('_CExoKeyTable', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoKeyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x62CA40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x637850))
    if fn:
        fn.setName('RemoveFromPlayingList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x637850: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x639340))
    if fn:
        fn.setName('_pow', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x639340: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x63A9E0))
    if fn:
        fn.setName('PlayOneShotSound', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSound', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSound', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x63A9E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x63EB30))
    if fn:
        fn.setName('ShutDownSource', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundSourceInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x63EB30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x63F3E0))
    if fn:
        fn.setName('SetPitchVariance', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundSourceInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x63F3E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x647C80))
    if fn:
        fn.setName('GetGameObject', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGameObjectArray', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGameObjectArray', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x647C80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64BBC0))
    if fn:
        fn.setName('GetDoorByGameObjectID', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64BBC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64BC20))
    if fn:
        fn.setName('StackPopObject', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CVirtualMachine', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CVirtualMachine', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64BC20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64C800))
    if fn:
        fn.setName('GetLoadFromSaveGame', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64C800: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x667380))
    if fn:
        fn.setName('GetUsedActivePropertyUsesLeft', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSItem', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x667380: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x678640))
    if fn:
        fn.setName('GetPropertyStrings', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSItem', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x678640: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6793C0))
    if fn:
        fn.setName('Format', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoString', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6793C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x68A3D0))
    if fn:
        fn.setName('ComputeAIState', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x68A3D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6A3CF0))
    if fn:
        fn.setName('GetListenCheckDistance', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6A3CF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6A3E00))
    if fn:
        fn.setName('GetItemInSlot', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWInventory', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWInventory', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6A3E00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6A4260))
    if fn:
        fn.setName('GetServerAIMaster', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6A4260: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6A5190))
    if fn:
        fn.setName('GetIsBeam', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CLoopingVisualEffect', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CLoopingVisualEffect', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6A5190: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6AE6A0))
    if fn:
        fn.setName('GetObjectArray', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6AE6A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6B1200))
    if fn:
        fn.setName('ClearAllEffects', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6B1200: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6B6D30))
    if fn:
        fn.setName('UpdateStatus', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6B6D30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6DC020))
    if fn:
        fn.setName('AddToVisibleList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6DC020: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x703DC0))
    if fn:
        fn.setName('GetPlayerList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x703DC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x722210))
    if fn:
        fn.setName('NoNonWalkPolysDetailed', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x722210: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x722590))
    if fn:
        fn.setName('NoNonWalkPolysInDoors', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x722590: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7239E0))
    if fn:
        fn.setName('GetMeshBoundingBox', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCollisionMesh', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCollisionMesh', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7239E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x733C50))
    if fn:
        fn.setName('operator_', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoLocString', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoLocString', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x733C50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x734C20))
    if fn:
        fn.setName('SetCreator', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGameEffect', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x734C20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x734C90))
    if fn:
        fn.setName('CGameEffect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGameEffect', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x734C90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x74CA20))
    if fn:
        fn.setName('Allocate', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoArrayList<>', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoArrayList<>', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x74CA20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x75ED20))
    if fn:
        fn.setName('GetPartyTable', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x75ED20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x75EEF0))
    if fn:
        fn.setName('GetArea', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x75EEF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x76F960))
    if fn:
        fn.setName('ApplyEffect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x76F960: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x770E80))
    if fn:
        fn.setName('ClearData', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCombatRoundAction', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCombatRoundAction', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x770E80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x778850))
    if fn:
        fn.setName('operator_', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('SJournalEntry', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'SJournalEntry', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x778850: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7E3FD0))
    if fn:
        fn.setName('GetGameObject', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSPlayer', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSPlayer', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7E3FD0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x810750))
    if fn:
        fn.setName('IntersectLineSegments2d', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x810750: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x811150))
    if fn:
        fn.setName('IntersectCircle2d', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x811150: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x86AF00))
    if fn:
        fn.setName('AurQueryAntiAliasingMode', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x86AF00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x90FF00))
    if fn:
        fn.setName('SetSize', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x90FF00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x914D30))
    if fn:
        fn.setName('CSWGuiTextParams', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTextParams', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTextParams', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x914D30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x914E70))
    if fn:
        fn.setName('CResRef', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CResRef', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x914E70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9434E0))
    if fn:
        fn.setName('GetProperty0x2c', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9434E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x41E770))
    if fn:
        fn.setName('DoFootstepVisual', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x41E770: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x428C00))
    if fn:
        fn.setName('PerformSetMine', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTutorialBox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTutorialBox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x428C00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x42C070))
    if fn:
        fn.setName('UpdateInventory', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameEquip', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x42C070: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x42C720))
    if fn:
        fn.setName('HandleWMCharMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x42C720: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x439850))
    if fn:
        fn.setName('SetHitcheckIgnoreList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x439850: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x439C70))
    if fn:
        fn.setName('SetTexturePack', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x439C70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x43ABF0))
    if fn:
        fn.setName('StartMenuMusic', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x43ABF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x43ADB0))
    if fn:
        fn.setName('StopMenuMusic', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x43ADB0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x444A00))
    if fn:
        fn.setName('HideLoadScreen', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x444A00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4482C0))
    if fn:
        fn.setName('SetAmbientNightTrack', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAmbientSound', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4482C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x44AA20))
    if fn:
        fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x44AA20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x44BA30))
    if fn:
        fn.setName('PlayerFlourishWeapons', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x44BA30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x44BD70))
    if fn:
        fn.setName('ChangeCharacterToNextLivingPartyMember', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x44BD70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x44EA00))
    if fn:
        fn.setName('UnloadArea', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x44EA00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4544C0))
    if fn:
        fn.setName('ApplyFireAndForgetVisualEffect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4544C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x475C30))
    if fn:
        fn.setName('RefreshVisualEffects', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x475C30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x477110))
    if fn:
        fn.setName('SetPortraitId', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x477110: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x47A530))
    if fn:
        fn.setName('AcceptParty', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiPartySelection', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiPartySelection', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x47A530: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x47E090))
    if fn:
        fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameMap', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameMap', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x47E090: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x48F710))
    if fn:
        fn.setName('HandleServerToPlayerDebugInfo_Creature', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x48F710: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4916B0))
    if fn:
        fn.setName('FeatRequirementsMet', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCLevelUpStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCLevelUpStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4916B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x495600))
    if fn:
        fn.setName('GetPortrait', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPortrait', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPortrait', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x495600: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x495C40))
    if fn:
        fn.setName('CSoundSetList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSoundSetList', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSoundSetList', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x495C40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x49AF50))
    if fn:
        fn.setName('HandleServerToPlayerCreatureUpdate_VisEffec', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x49AF50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4A5A40))
    if fn:
        fn.setName('HandleServerToPlayerModule', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4A5A40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4AA2F0))
    if fn:
        fn.setName('HandleServerToPlayerUpdate_GuiInventory', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4AA2F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4AA4B0))
    if fn:
        fn.setName('HandleServerToPlayerUpdate_GuiContainer', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4AA4B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4BF920))
    if fn:
        fn.setName('HandleServerToPlayerCheat_DoNasty', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4BF920: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4BFF10))
    if fn:
        fn.setName('HandleServerToPlayerJournalMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4BFF10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4CCE20))
    if fn:
        fn.setName('SetGroundTilt', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAnimBase', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4CCE20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4D46F0))
    if fn:
        fn.setName('HandleDialogModels', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4D46F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4DBCE0))
    if fn:
        fn.setName('SetPrecalculatedPosition', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4DBCE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4DF970))
    if fn:
        fn.setName('ShowStatusSummary', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4DF970: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4E9700))
    if fn:
        fn.setName('Control', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPlayerControlCamRelative', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPlayerControlCamRelative', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4E9700: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x504BF0))
    if fn:
        fn.setName('ApplyMotionBlur', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x504BF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x50F540))
    if fn:
        fn.setName('LoadFeatsTable', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWClass', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWClass', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x50F540: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x512F40))
    if fn:
        fn.setName('Initialize', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiMainInterfaceAction', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiMainInterfaceAction', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x512F40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x514E50))
    if fn:
        fn.setName('CSWGuiLevelUpPanel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiLevelUpPanel', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiLevelUpPanel', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x514E50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5210C0))
    if fn:
        fn.setName('SetHealth', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiMainInterfaceChar', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiMainInterfaceChar', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5210C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5365C0))
    if fn:
        fn.setName('GetTargetActions', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCPlaceable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCPlaceable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5365C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x53E000))
    if fn:
        fn.setName('OnModulePicked', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiMainMenu', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiMainMenu', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x53E000: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x546CC0))
    if fn:
        fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x546CC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x549FE0))
    if fn:
        fn.setName('HandleServerToPlayerCCMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x549FE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x554D40))
    if fn:
        fn.setName('Draw', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiPazaakGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiPazaakGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x554D40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x557BA0))
    if fn:
        fn.setName('Update', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAmbientSound', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x557BA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x55AC90))
    if fn:
        fn.setName('_CSWCSoundObject', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCSoundObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x55AC90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x55ADF0))
    if fn:
        fn.setName('LoadFromMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCSoundObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x55ADF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x574BE0))
    if fn:
        fn.setName('UpdateShotCamera', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x574BE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5792E0))
    if fn:
        fn.setName('_CSWCProjectile', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCProjectile', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5792E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5870D0))
    if fn:
        fn.setName('SetNextMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTutorialBox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTutorialBox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5870D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x587590))
    if fn:
        fn.setName('CSWGuiFlowSkillStruct', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiFlowSkillStruct', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiFlowSkillStruct', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x587590: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5915E0))
    if fn:
        fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameJournal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5915E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x597FC0))
    if fn:
        fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameCharacter', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameCharacter', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x597FC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x59B420))
    if fn:
        fn.setName('OnPanelRemoved', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameInventory', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x59B420: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x59F980))
    if fn:
        fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameEquip', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x59F980: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5A0C20))
    if fn:
        fn.setName('ShowDescription', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameEquip', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5A0C20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5A9300))
    if fn:
        fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiStore', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5A9300: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5AD230))
    if fn:
        fn.setName('Draw', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameAreaTransition', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameAreaTransition', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5AD230: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5B5200))
    if fn:
        fn.setName('OnFeatPicked', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiPowersFeatsSkillsDebugMenu', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiPowersFeatsSkillsDebugMenu', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5B5200: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5B7470))
    if fn:
        fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiUpgradeSelection', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiUpgradeSelection', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5B7470: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5B8A10))
    if fn:
        fn.setName('OnControlEntered', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiStore', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5B8A10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5C47F0))
    if fn:
        fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiScriptSelect', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiScriptSelect', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5C47F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5CFA50))
    if fn:
        fn.setName('OnUpgradeSelected', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiUpgrade', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5CFA50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5DE910))
    if fn:
        fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiClassSelection', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiClassSelection', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5DE910: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5EA480))
    if fn:
        fn.setName('UpdatePlayerStats', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameInventory', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameInventory', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5EA480: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5EE100))
    if fn:
        fn.setName('GetClassString', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5EE100: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x605430))
    if fn:
        fn.setName('HandleDoneButton', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiNameChargen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiNameChargen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x605430: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x607550))
    if fn:
        fn.setName('AddString', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoLocString', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoLocString', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x607550: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x607A00))
    if fn:
        fn.setName('HandleServerAdminToServerMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x607A00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x613010))
    if fn:
        fn.setName('PollInput', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoInputInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoInputInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x613010: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x616E60))
    if fn:
        fn.setName('HandleNotification', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoImeUI', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoImeUI', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x616E60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x621350))
    if fn:
        fn.setName('LoadOptions', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientOptions', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientOptions', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x621350: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x62BB40))
    if fn:
        fn.setName('WriteResource', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CERFFile', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CERFFile', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x62BB40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x62BE00))
    if fn:
        fn.setName('Finish', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CERFFile', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CERFFile', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x62BE00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x62E950))
    if fn:
        fn.setName('AddKeyTableContents', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoKeyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x62E950: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6300C0))
    if fn:
        fn.setName('FindKey', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoKeyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6300C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x638D80))
    if fn:
        fn.setName('PauseAllSounds', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x638D80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64D1B0))
    if fn:
        fn.setName('UpdateState', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerAIMaster', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64D1B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64E730))
    if fn:
        fn.setName('WalkUpdateLocation_QuickWalk', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64E730: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64E7A0))
    if fn:
        fn.setName('AIActionPlayAnimation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64E7A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x650700))
    if fn:
        fn.setName('DeleteEventData', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerAIMaster', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x650700: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x659DB0))
    if fn:
        fn.setName('SignalRangedDamage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x659DB0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x66BC50))
    if fn:
        fn.setName('AddProperty', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSItem', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSItem', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x66BC50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x67A8C0))
    if fn:
        fn.setName('SaveModuleIFOStart', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSModule', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x67A8C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6A69C0))
    if fn:
        fn.setName('SaveObjectState', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6A69C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6B6F00))
    if fn:
        fn.setName('AIActionMoveToPoint', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6B6F00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6D5A90))
    if fn:
        fn.setName('AIActionDialogObject', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6D5A90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6E47F0))
    if fn:
        fn.setName('CSWGuiCreateItemMenu', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiCreateItemMenu', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiCreateItemMenu', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6E47F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6FDE90))
    if fn:
        fn.setName('OnApplyDeath', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEffectListHandler', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6FDE90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6FEB00))
    if fn:
        fn.setName('EventHandler', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6FEB00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7031C0))
    if fn:
        fn.setName('SpawnNPC', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPartyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7031C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7034A0))
    if fn:
        fn.setName('KillNPCObject', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPartyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7034A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x72E330))
    if fn:
        fn.setName('SaveModuleFinish', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSModule', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSModule', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x72E330: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7380A0))
    if fn:
        fn.setName('OnApplyLightsaberThrow', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7380A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x73BDF0))
    if fn:
        fn.setName('OnApplySummonCreature', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEffectListHandler', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x73BDF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x741FE0))
    if fn:
        fn.setName('OnApplyForceResistanceIncrease', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEffectListHandler', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x741FE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x744740))
    if fn:
        fn.setName('OnApplySanctuary', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x744740: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7465C0))
    if fn:
        fn.setName('OnRemoveHitPointChangeWhenDying', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7465C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x746CB0))
    if fn:
        fn.setName('OnApplyEffectIcon', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x746CB0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7B1130))
    if fn:
        fn.setName('EventHandler', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSTrigger', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSTrigger', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7B1130: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7B34F0))
    if fn:
        fn.setName('ExecuteLoadModule', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7B34F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7B8D50))
    if fn:
        fn.setName('MovePlayerToArea', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7B8D50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7B9510))
    if fn:
        fn.setName('RunModule', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7B9510: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7BC010))
    if fn:
        fn.setName('UnloadModule', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7BC010: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7BCE70))
    if fn:
        fn.setName('MainLoop', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7BCE70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7C5670))
    if fn:
        fn.setName('RemoveACBonus', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSItemPropertyHandler', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSItemPropertyHandler', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7C5670: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7E23A0))
    if fn:
        fn.setName('SendServerToPlayerJournalFullUpdate', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7E23A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7E8D40))
    if fn:
        fn.setName('JumpToPoint', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7E8D40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F0B60))
    if fn:
        fn.setName('SaveVarTable', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSScriptVarTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSScriptVarTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F0B60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F70B0))
    if fn:
        fn.setName('AddToArea', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSSoundObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F70B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8080A0))
    if fn:
        fn.setName('HandlePlayerToServerMapPinMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8080A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x811680))
    if fn:
        fn.setName('GetRandomName', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWNameGen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWNameGen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x811680: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x811FA0))
    if fn:
        fn.setName('GetRandomName', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWNameGen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWNameGen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x811FA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8586C0))
    if fn:
        fn.setName('Render', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('Gob', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'Gob', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8586C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x863920))
    if fn:
        fn.setName('SetTileLights', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('Scene', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'Scene', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x863920: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x86D990))
    if fn:
        fn.setName('SceneBSPAddPart', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x86D990: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x86E3C0))
    if fn:
        fn.setName('DeleteNonGobParts', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x86E3C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x897390))
    if fn:
        fn.setName('Mat', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('PartTriMesh', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'PartTriMesh', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x897390: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8C4B30))
    if fn:
        fn.setName('GetBoundingRect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAnimBase', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAnimBase', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8C4B30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8F6C10))
    if fn:
        fn.setName('BeginLookAtAnimate', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('Gob', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'Gob', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8F6C10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x900350))
    if fn:
        fn.setName('EventHandler', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSDoor', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSDoor', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x900350: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x918D40))
    if fn:
        fn.setName('Initialize', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiSlider', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiSlider', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x918D40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x91B720))
    if fn:
        fn.setName('SetExtent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiListBox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x91B720: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x86D5F0))
    if fn:
        fn.setName('SceneBSPAddTileData', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x86D5F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x90B220))
    if fn:
        fn.setName('GetAuroraResDataSize', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x90B220: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40DE80))
    if fn:
        fn.setName('AddPanel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40DE80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x41F8E0))
    if fn:
        fn.setName('HideEquippedItems', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x41F8E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4443B0))
    if fn:
        fn.setName('SetActive', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoInput', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoInput', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4443B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4909E0))
    if fn:
        fn.setName('GetArmorClass', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4909E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x491BA0))
    if fn:
        fn.setName('GetBaseAttackBonus', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCLevelUpStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCLevelUpStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x491BA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4E2A60))
    if fn:
        fn.setName('PlayGuiSound', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4E2A60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x520390))
    if fn:
        fn.setName('SetCombatMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiMainInterface', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x520390: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5585C0))
    if fn:
        fn.setName('ResetLastUpdate', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAmbientSound', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5585C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5A4650))
    if fn:
        fn.setName('ShowCantEquipMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameEquip', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5A4650: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5A4790))
    if fn:
        fn.setName('UpdatePortraits', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameEquip', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameEquip', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5A4790: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60B9E0))
    if fn:
        fn.setName('Seek', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoFile', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoFile', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60B9E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x626E10))
    if fn:
        fn.setName('GetTotalSize', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CResGFF', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CResGFF', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x626E10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x630410))
    if fn:
        fn.setName('Hash', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoKeyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x630410: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6393E0))
    if fn:
        fn.setName('Pause', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundSourceInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6393E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x63F010))
    if fn:
        fn.setName('return_zero', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x63F010: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x63F4F0))
    if fn:
        fn.setName('CalculatePitchVarianceFrequency', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundSourceInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundSourceInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x63F4F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64BC00))
    if fn:
        fn.setName('GetWaypointByGameObjectID', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64BC00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x65AD40))
    if fn:
        fn.setName('SetInteger', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGameEffect', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGameEffect', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x65AD40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x668830))
    if fn:
        fn.setName('GetModule', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x668830: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6B11B0))
    if fn:
        fn.setName('ClearDirtyFlag', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6B11B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x912F70))
    if fn:
        fn.setName('GetProperty0x10', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x912F70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x912FF0))
    if fn:
        fn.setName('Initialize', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiBorder', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiBorder', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x912FF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x913150))
    if fn:
        fn.setName('GetInnerExtent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiBorder', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiBorder', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x913150: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4269A0))
    if fn:
        fn.setName('GetDefaultActions', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4269A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x439480))
    if fn:
        fn.setName('UpdateCamera', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x439480: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x448660))
    if fn:
        fn.setName('ProcessInput', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x448660: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4DB560))
    if fn:
        fn.setName('LoadOptions', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4DB560: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4DF280))
    if fn:
        fn.setName('PlayMoviesInExoArrayList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoAppInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoAppInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4DF280: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4F03C0))
    if fn:
        fn.setName('SetPlayer', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPlayerControl', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPlayerControl', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4F03C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x502EC0))
    if fn:
        fn.setName('UpdateFOV', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x502EC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5139D0))
    if fn:
        fn.setName('Initialize', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5139D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5868F0))
    if fn:
        fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTutorialBox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTutorialBox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5868F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x58E470))
    if fn:
        fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiPartySelection', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiPartySelection', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x58E470: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x605300))
    if fn:
        fn.setName('HandleRandomName', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiNameChargen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiNameChargen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x605300: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x62D270))
    if fn:
        fn.setName('AddEncapsulatedContents', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoKeyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoKeyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x62D270: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x632060))
    if fn:
        fn.setName('LoadHeader', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoEncapsulatedFile', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoEncapsulatedFile', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x632060: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6587E0))
    if fn:
        fn.setName('ResolveRangedMiss', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6587E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x67C870))
    if fn:
        fn.setName('SetId', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x67C870: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6C7B80))
    if fn:
        fn.setName('HandlePlayerToServerCheatMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6C7B80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7CAC50))
    if fn:
        fn.setName('CompareVisualEffectLists', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7CAC50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x90FFA0))
    if fn:
        fn.setName('DrawPCBG', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x90FFA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x513D50))
    if fn:
        fn.setName('SetNameLabel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x513D50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x47D960))
    if fn:
        fn.setName('UpdateIndicator', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiMainInterface', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiMainInterface', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x47D960: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4D6BC0))
    if fn:
        fn.setName('CleanUpReplyNode', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4D6BC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x540260))
    if fn:
        fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiStatusSummary', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiStatusSummary', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x540260: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x605040))
    if fn:
        fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiNameChargen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiNameChargen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x605040: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x91B440))
    if fn:
        fn.setName('Initialize', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiListBox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiListBox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x91B440: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x592150))
    if fn:
        fn.setName('MarkNewQuestsObserved', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameJournal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x592150: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5916E0))
    if fn:
        fn.setName('_CSWGuiInGameJournal', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameJournal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5916E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x591AE0))
    if fn:
        fn.setName('_CSWGuiInGameJournal', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameJournal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x591AE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x591FE0))
    if fn:
        fn.setName('OnControlEntered', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameJournal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x591FE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x918130))
    if fn:
        fn.setName('OnPanelRemoved', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameJournal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x918130: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x592200))
    if fn:
        fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameJournal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameJournal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x592200: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40E8A0))
    if fn:
        fn.setName('Allocate', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoArrayList', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoArrayList', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40E8A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x516500))
    if fn:
        fn.setName('GetSWParty', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x516500: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6416A0))
    if fn:
        fn.setName('AddToTopOfStack', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CVirtualMachineStack', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CVirtualMachineStack', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6416A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6612B0))
    if fn:
        fn.setName('__ftol2', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6612B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6E5390))
    if fn:
        fn.setName('GetBaseItem', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWBaseItemArray', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWBaseItemArray', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6E5390: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x41BDD0))
    if fn:
        fn.setName('ResolveSpecialAttackVisuals', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x41BDD0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x41CE40))
    if fn:
        fn.setName('SetPortrait', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x41CE40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x46A260))
    if fn:
        fn.setName('LoadModule', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCModule', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCModule', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x46A260: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x48C590))
    if fn:
        fn.setName('RequestHeadAppearance', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreatureAppearance', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreatureAppearance', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x48C590: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x48E3B0))
    if fn:
        fn.setName('GetBTypeModelAndTextureResRef', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreatureAppearance', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreatureAppearance', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x48E3B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4DE1A0))
    if fn:
        fn.setName('ShowPartySelection', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4DE1A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4FD2F0))
    if fn:
        fn.setName('StartVisualEffect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4FD2F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4FDBE0))
    if fn:
        fn.setName('StopVisualEffect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4FDBE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5228C0))
    if fn:
        fn.setName('OnSlotSelected', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiUpgrade', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5228C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x54F5B0))
    if fn:
        fn.setName('OnPanelAdded', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiPazaakStart', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiPazaakStart', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x54F5B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x557910))
    if fn:
        fn.setName('LoadFromMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAmbientSound', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAmbientSound', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x557910: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x558940))
    if fn:
        fn.setName('CSWGuiInGameCredits', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameCredits', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameCredits', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x558940: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x55B860))
    if fn:
        fn.setName('AIUpdate', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCSoundObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x55B860: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x55CEE0))
    if fn:
        fn.setName('HitCheckBullet', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWMiniGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWMiniGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x55CEE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5B0D70))
    if fn:
        fn.setName('CSWGuiDialogComputer', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiDialogComputer', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiDialogComputer', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5B0D70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5B5080))
    if fn:
        fn.setName('OnPowerPicked', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiPowersFeatsSkillsDebugMenu', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiPowersFeatsSkillsDebugMenu', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5B5080: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5B5480))
    if fn:
        fn.setName('OnSkillPicked', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiPowersFeatsSkillsDebugMenu', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiPowersFeatsSkillsDebugMenu', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5B5480: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5BB650))
    if fn:
        fn.setName('CloseItems', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiUpgrade', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5BB650: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5F0560))
    if fn:
        fn.setName('CSWGuiKeyMapButton', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiKeyMapButton', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiKeyMapButton', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5F0560: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x604B10))
    if fn:
        fn.setName('CSWGuiNameChargen', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiNameChargen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiNameChargen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x604B10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x642C30))
    if fn:
        fn.setName('ExecuteCode', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CVirtualMachineInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CVirtualMachineInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x642C30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6922C0))
    if fn:
        fn.setName('HandlePlayerToServerInputMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6922C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6954C0))
    if fn:
        fn.setName('GetNearestEnemy', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6954C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6AEF80))
    if fn:
        fn.setName('OnApplyDamage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEffectListHandler', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6AEF80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6B0460))
    if fn:
        fn.setName('SpawnBodyBag', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6B0460: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6DA3D0))
    if fn:
        fn.setName('AIActionOrientCamera', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6DA3D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6E50A0))
    if fn:
        fn.setName('CheckItemRaceRestrictions', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6E50A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6FACA0))
    if fn:
        fn.setName('AddControl', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiPanel', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6FACA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x720B70))
    if fn:
        fn.setName('HandleTransparentDoors', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x720B70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x73D5B0))
    if fn:
        fn.setName('OnRemoveTemporaryHitpoints', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEffectListHandler', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x73D5B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x73E1C0))
    if fn:
        fn.setName('OnApplyKnockdown', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEffectListHandler', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x73E1C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x742FC0))
    if fn:
        fn.setName('OnApplyDisease', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEffectListHandler', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEffectListHandler', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x742FC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7AF4D0))
    if fn:
        fn.setName('AddEventAbsoluteTime', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerAIMaster', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerAIMaster', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7AF4D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7CB040))
    if fn:
        fn.setName('TestString', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSExpression', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSExpression', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7CB040: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7DDD60))
    if fn:
        fn.setName('SendServerToPlayerPlayerList_All', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7DDD60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7EB1F0))
    if fn:
        fn.setName('LoadEncounter', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEncounter', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7EB1F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F78B0))
    if fn:
        fn.setName('FadeAndStop', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSSoundObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSSoundObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F78B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x80F6C0))
    if fn:
        fn.setName('ApplyLightsaberThrow', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x80F6C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x88DC30))
    if fn:
        fn.setName('RenderSkinnedVP', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x88DC30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x90CCE0))
    if fn:
        fn.setName('_CSWGuiPanel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiPanel', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiPanel', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x90CCE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9128C0))
    if fn:
        fn.setName('operator_', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiBorderParams', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiBorderParams', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9128C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x914350))
    if fn:
        fn.setName('operator_', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiImageParams', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiImageParams', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x914350: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9176C0))
    if fn:
        fn.setName('DisplayToolTip', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiControl', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiControl', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9176C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x616B40))
    if fn:
        fn.setName('handleCompositionResult_', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoImeUI', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoImeUI', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x616B40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6170E0))
    if fn:
        fn.setName('_CExoImeUI', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoImeUI', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoImeUI', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6170E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x54C6B0))
    if fn:
        fn.setName('SetText', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTextParams', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTextParams', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x54C6B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x595230))
    if fn:
        fn.setName('GetLength', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoString', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoString', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x595230: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5FF290))
    if fn:
        fn.setName('IsClassSkill', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiSkillsCharGen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiSkillsCharGen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5FF290: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x419360))
    if fn:
        fn.setName('RequestAppearance', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x419360: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x475E00))
    if fn:
        fn.setName('ActivateHardCodedVisualWithHighestPriority', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x475E00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4F95D0))
    if fn:
        fn.setName('LoadEffect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffect', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffect', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4F95D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x522390))
    if fn:
        fn.setName('CSWGuiLoadScreen', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiLoadScreen', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiLoadScreen', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x522390: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5B25A0))
    if fn:
        fn.setName('_CSWGuiBarkBubble', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiBarkBubble', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiBarkBubble', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5B25A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x65A5B0))
    if fn:
        fn.setName('GetWeaponImpactTime', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x65A5B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7DD9D0))
    if fn:
        fn.setName('SendServerToPlayerPlayerList_Add', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7DD9D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x503930))
    if fn:
        fn.setName('DetachVisualEffect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x503930: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x510AF0))
    if fn:
        fn.setName('IsSkillClassSkill', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWClass', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWClass', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x510AF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x476F40))
    if fn:
        fn.setName('DetachVisualEffects', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x476F40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x491220))
    if fn:
        fn.setName('IncreaseSkillRank', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCLevelUpStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCLevelUpStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x491220: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x59F240))
    if fn:
        fn.setName('GetUpgradableType', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x59F240: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5F3000))
    if fn:
        fn.setName('OnSelectSkillsButton', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5F3000: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x809C50))
    if fn:
        fn.setName('ParseGetString', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x809C50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x809DB0))
    if fn:
        fn.setName('GetTokenPair', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x809DB0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x848CB0))
    if fn:
        fn.setName('InternalCreateInstance', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x848CB0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x848E10))
    if fn:
        fn.setName('InternalCreateInstance', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x848E10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x407440))
    if fn:
        fn.setName('ClearLogCounters', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x407440: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x40E820))
    if fn:
        fn.setName('ResetCurrentSessionStartTim', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPartyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x40E820: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4275F0))
    if fn:
        fn.setName('IsLeaderUsableItem', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4275F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4C75E0))
    if fn:
        fn.setName('Remove', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoArrayList<ushort>', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoArrayList<ushort>', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4C75E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4CE840))
    if fn:
        fn.setName('AppendToMsgBuffer', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4CE840: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4D4880))
    if fn:
        fn.setName('UnlockDialogParticipantOrientations', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4D4880: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4E74D0))
    if fn:
        fn.setName('SaveCameraSettings', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCameraOnAStick', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCameraOnAStick', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4E74D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4FE1F0))
    if fn:
        fn.setName('ApplyHardCodedVisualEffect', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCVisualEffectOnObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCVisualEffectOnObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4FE1F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x509220))
    if fn:
        fn.setName('_CCombatInformation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CCombatInformation', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CCombatInformation', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x509220: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x512D80))
    if fn:
        fn.setName('GetAutoPaused', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CClientExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CClientExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x512D80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x54D020))
    if fn:
        fn.setName('sprintf', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x54D020: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x57C8E0))
    if fn:
        fn.setName('ClearBehaviorList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCProjectile', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCProjectile', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x57C8E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5ABF20))
    if fn:
        fn.setName('GetItemBuyValue', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiStore', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiStore', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5ABF20: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5B01D0))
    if fn:
        fn.setName('CSWGuiDialogCinematic', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiDialogCinematic', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiDialogCinematic', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5B01D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6081D0))
    if fn:
        fn.setName('GetResourceExtension', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoBase', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoBase', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6081D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60D4A0))
    if fn:
        fn.setName('GetString', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoStringList', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoStringList', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60D4A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x61B030))
    if fn:
        fn.setName('RemoveResourceImageFile', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoResMan', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoResMan', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x61B030: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x637990))
    if fn:
        fn.setName('RemoveFromNonPlayingLoopingList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x637990: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x640350))
    if fn:
        fn.setName('UpdateStatusLoop', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CNetLayer', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CNetLayer', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x640350: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64BBA0))
    if fn:
        fn.setName('GetStoreByGameObjectID', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64BBA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x669A30))
    if fn:
        fn.setName('ClearItems', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CItemRepository', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CItemRepository', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x669A30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6F6DF0))
    if fn:
        fn.setName('GetTalentHasCompatibleCategory', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreatureStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreatureStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6F6DF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7035A0))
    if fn:
        fn.setName('GetNPCID', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPartyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7035A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x70CFB0))
    if fn:
        fn.setName('NoNoneWalkPolysInStaticObject', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x70CFB0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x728C60))
    if fn:
        fn.setName('CResRef', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CResRef', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CResRef', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x728C60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x770D30))
    if fn:
        fn.setName('SetOrientation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSPlaceable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSPlaceable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x770D30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7DE210))
    if fn:
        fn.setName('SendServerToPlayerServerStatus_Status', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7DE210: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7EB810))
    if fn:
        fn.setName('ReadEncounterFromGff', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEncounter', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7EB810: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F3180))
    if fn:
        fn.setName('_CLastUpdateObject', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CLastUpdateObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CLastUpdateObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F3180: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F34A0))
    if fn:
        fn.setName('ClearKnownSpells', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSPlayerLastUpdateObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSPlayerLastUpdateObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F34A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7F3B40))
    if fn:
        fn.setName('AddFeat', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWLevelStats', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWLevelStats', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7F3B40: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x80BC50))
    if fn:
        fn.setName('NormalOf', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x80BC50: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x81B9C0))
    if fn:
        fn.setName('allocate', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoArrayList<Vector>', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoArrayList<Vector>', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x81B9C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x825660))
    if fn:
        fn.setName('DeletePool', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('GLRender', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'GLRender', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x825660: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x843BE0))
    if fn:
        fn.setName('operator_', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x843BE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x84B050))
    if fn:
        fn.setName('FindScene', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x84B050: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8C3B00))
    if fn:
        fn.setName('Control', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAurBehaviorAttach', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAurBehaviorAttach', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8C3B00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8C5260))
    if fn:
        fn.setName('firstword', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8C5260: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8C5370))
    if fn:
        fn.setName('Parse_int', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8C5370: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8F6F60))
    if fn:
        fn.setName('Insert', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoArrayList<Model*>', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoArrayList<Model*>', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8F6F60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9125D0))
    if fn:
        fn.setName('StoreCurrentMousePosition', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiManager', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiManager', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9125D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x916870))
    if fn:
        fn.setName('GetModel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiScene', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiScene', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x916870: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4215B0))
    if fn:
        fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiContainer', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiContainer', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4215B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4247A0))
    if fn:
        fn.setName('IsProjectileParryable', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4247A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x44EC30))
    if fn:
        fn.setName('HandleServerToPlayerPlayerList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x44EC30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4583A0))
    if fn:
        fn.setName('restartminigame', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4583A0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x471D00))
    if fn:
        fn.setName('CSWGuiClassSelection', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiClassSelection', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiClassSelection', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x471D00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x48F5E0))
    if fn:
        fn.setName('AIActionItemCastSpell', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x48F5E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4C9020))
    if fn:
        fn.setName('GetCameraParameters', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWBehaviorCameraDialog', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWBehaviorCameraDialog', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4C9020: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4D0350))
    if fn:
        fn.setName('BuildAnimation', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('LookAtAnimation', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'LookAtAnimation', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4D0350: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4D9AD0))
    if fn:
        fn.setName('ResetDialogAnimations', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CGuiInGame', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CGuiInGame', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4D9AD0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4E38B0))
    if fn:
        fn.setName('LocaleEnumCallback', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4E38B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4F58B0))
    if fn:
        fn.setName('ReAttachWieldedObjects', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAnimBaseWield', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAnimBaseWield', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4F58B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x501E30))
    if fn:
        fn.setName('UpdateDayNightCycle', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCArea', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCArea', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x501E30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5073B0))
    if fn:
        fn.setName('ToggleDoorState', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCDoor', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCDoor', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5073B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5075F0))
    if fn:
        fn.setName('MenuActionBash', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCDoor', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCDoor', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5075F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x513750))
    if fn:
        fn.setName('CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTargetActionMenu', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTargetActionMenu', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x513750: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x532C00))
    if fn:
        fn.setName('GetMemoryUsage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAurTextureBasic', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAurTextureBasic', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x532C00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x540F30))
    if fn:
        fn.setName('LoadScripts', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWTrackFollower', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWTrackFollower', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x540F30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x549DA0))
    if fn:
        fn.setName('SetReplies', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiDialog', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiDialog', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x549DA0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x55B480))
    if fn:
        fn.setName('AddToArea', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCSoundObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCSoundObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x55B480: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x586A00))
    if fn:
        fn.setName('SetTutorialReason', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTutorialBox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTutorialBox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x586A00: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x586EC0))
    if fn:
        fn.setName('PerformBash', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiTutorialBox', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiTutorialBox', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x586EC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5937D0))
    if fn:
        fn.setName('SetCharacter', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiInGameAbilities', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiInGameAbilities', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5937D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5BD230))
    if fn:
        fn.setName('Draw', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiUpgrade', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiUpgrade', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5BD230: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5D0B30))
    if fn:
        fn.setName('HandleInputEvent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWGuiSaveLoad', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWGuiSaveLoad', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5D0B30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x5EB040))
    if fn:
        fn.setName('Fire', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWMGBehaviorFire', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWMGBehaviorFire', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x5EB040: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60DB60))
    if fn:
        fn.setName('ShutDown', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60DB60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x638870))
    if fn:
        fn.setName('AddToStreamingList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoSoundInternal', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoSoundInternal', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x638870: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x64C620))
    if fn:
        fn.setName('GetRoomIndex', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x64C620: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6AC150))
    if fn:
        fn.setName('SetDialogDelay', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6AC150: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x704C70))
    if fn:
        fn.setName('PositionParty', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWPartyTable', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWPartyTable', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x704C70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x73E770))
    if fn:
        fn.setName('AIActionCastSpell', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x73E770: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x774DB0))
    if fn:
        fn.setName('EquipMostEffectiveArmor', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCreature', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCreature', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x774DB0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x798390))
    if fn:
        fn.setName('ExecuteCommandGetLoadFromSaveGame', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWVirtualMachineCommands', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWVirtualMachineCommands', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x798390: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7BC850))
    if fn:
        fn.setName('UnloadModule', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CServerExoApp', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CServerExoApp', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7BC850: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7CA5E0))
    if fn:
        fn.setName('SendServerToPlayerExamineGui_ItemData', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7CA5E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7CE2D0))
    if fn:
        fn.setName('SendServerToPlayerExamineGui_CreatureData', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7CE2D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7E6800))
    if fn:
        fn.setName('EventHandler', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSAreaOfEffectObject', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSAreaOfEffectObject', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7E6800: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x801AF0))
    if fn:
        fn.setName('HandlePlayerToServerInventoryMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x801AF0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x808A80))
    if fn:
        fn.setName('HandlePlayerToServerCharacterDownload', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x808A80: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x85BCD0))
    if fn:
        fn.setName('DoEmitterBucket', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('Scene', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'Scene', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x85BCD0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x866A10))
    if fn:
        fn.setName('__interpretLocale', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x866A10: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x86A8E0))
    if fn:
        fn.setName('AurMakeCurrent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x86A8E0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8887C0))
    if fn:
        fn.setName('RenderLightMappedNoShaders', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8887C0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8FA000))
    if fn:
        fn.setName('AnimateHierarchy', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8FA000: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x912160))
    if fn:
        fn.setName('_internal_initTimeZone', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x912160: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x92BB72))
    if fn:
        fn.setName('_CxxFrameHandlerCore', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x92BB72: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x9836D0))
    if fn:
        fn.setName('Project', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('PartSkin', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'PartSkin', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x9836D0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x7E9770))
    if fn:
        fn.setName('ClearSpawnList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSEncounter', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSEncounter', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x7E9770: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x845E70))
    if fn:
        fn.setName('ControlDispatch', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('MdlNode', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'MdlNode', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x845E70: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4F3A60))
    if fn:
        fn.setName('LoadModel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAnimBaseHeadWield', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAnimBaseHeadWield', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4F3A60: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x4F53B0))
    if fn:
        fn.setName('LoadModel', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWCAnimBaseWield', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWCAnimBaseWield', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x4F53B0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x559C90))
    if fn:
        fn.setName('Control', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWBehaviorAttachSound', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWBehaviorAttachSound', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x559C90: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x749640))
    if fn:
        fn.setName('OnRemoveBonusFeat', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                '', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x749640: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x807980))
    if fn:
        fn.setName('HandlePlayerToServerServerStatusMessage', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSMessage', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSMessage', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x807980: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8E8000))
    if fn:
        fn.setName('_VertexPrimitiveAnim', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('VertexPrimitiveAnim', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'VertexPrimitiveAnim', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8E8000: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x8E85F0))
    if fn:
        fn.setName('PostProcess', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('PartSkin', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'PartSkin', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x8E85F0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x817660))
    if fn:
        fn.setName('Message', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CAurBehaviorAttach', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CAurBehaviorAttach', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x817660: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x86B190))
    if fn:
        fn.setName('HasBumpMap', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('Material', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'Material', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x86B190: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x770EC0))
    if fn:
        fn.setName('CSWSCombatRoundAction', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CSWSCombatRoundAction', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CSWSCombatRoundAction', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x770EC0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x54C040))
    if fn:
        fn.setName('operator__', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CScriptEvent', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CScriptEvent', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x54C040: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x6A4D30))
    if fn:
        fn.setName('_CScriptEvent', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CScriptEvent', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CScriptEvent', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x6A4D30: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60FCE0))
    if fn:
        fn.setName('CExoAliasList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoAliasList', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoAliasList', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60FCE0: '+str(e)); errors += 1

try:
    fn = fm.getFunctionAt(toAddr(0x60FD90))
    if fn:
        fn.setName('_CExoAliasList', SourceType.USER_DEFINED)
        nsobj = sm.getNamespace('CExoAliasList', currentProgram.getGlobalNamespace())
        if nsobj is None:
            nsobj = sm.createNameSpace(currentProgram.getGlobalNamespace(),
                'CExoAliasList', SourceType.USER_DEFINED)
        fn.setParentNamespace(nsobj); renamed += 1
    else: errors += 1
except Exception as e:
    print('ERR 0x60FD90: '+str(e)); errors += 1

print('renamed={} errors={}'.format(renamed, errors))