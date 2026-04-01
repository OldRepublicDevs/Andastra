# AUTO-GENERATED Ghidra batch-rename script for TSL FUN_xxx functions.
# Run via mcp_agdec-mcp_execute-script with program_path=/TSL_GOG_swkotor2
from ghidra.app.util import NamespaceUtils
from ghidra.program.model.symbol import SourceType

RENAMES = [
    (0x00409000, "", "messagepump"),
    (0x0040cd90, "CClientExoApp", "AddObjectToHitcheckIgnoreList"),
    (0x0040cdb0, "CClientExoApp", "RemoveObjectFromHitcheckIgnoreList"),
    (0x0040d090, "CClientExoApp", "GetHitCheckMaterialMask"),
    (0x0040d8e0, "CClientExoApp", "SetCanSendDialog"),
    (0x0040d960, "CClientExoApp", "SetDialogModelsSent"),
    (0x0040e7c0, "CClientExoApp", "GetDisplayCharacterForKey"),
    (0x00416c70, "CSWCCreature", "GetWayPoints"),
    (0x0041ac10, "CSWCCreature", "GetLastInstant"),
    (0x0042ac40, "CSWCCreature", "Setup3DScene"),
    (0x0042bc30, "CServerExoApp", "GetCreaturePathfindInformation"),
    (0x0043ab70, "CClientExoAppInternal", "AddSimpleObjectModelDirectlyToIgnoreList"),
    (0x00454600, "CSWCArea", "StartSounds"),
    (0x00454660, "CSWCArea", "SetWeather"),
    (0x00474b00, "CSWCObject", "AddFireForgetAnimation"),
    (0x00475510, "CSWCObject", "GetFinalFadeAlpha"),
    (0x004776a0, "CSWCObject", "ClearAllQueuedCombatActions"),
    (0x0047e830, "", "NVThunkSWCItmSWCObject_LoadModel"),
    (0x0048c1e0, "CSWCCreatureAppearance", "GetAnimType"),
    (0x0048f020, "CSWCCreatureStats", "ClearSkills"),
    (0x00494dc0, "CSWCCreatureStats", "GetPortrait"),
    (0x00496520, "CSWCMessage", "CSWCMessage"),
    (0x004d4570, "CGuiInGame", "InitializeShotCamera"),
    (0x004dc4b0, "CGuiInGame", "SetPaused"),
    (0x004dee40, "CGuiInGame", "SetAreaTransition"),
    (0x004e15c0, "CGuiInGame", "FadeAndStopDialogAmbientTrack"),
    (0x004e8110, "CSWCameraFreeLook", "UpdateCamraStyle"),
    (0x004eada0, "CSWBaseItem", "GetModelResRef"),
    (0x004f0350, "CSWPlayerControl", "SetEnabled"),
    (0x004f3e80, "CSWCAnimBaseHeadWield", "HideHeadItem"),
    (0x0050f380, "CSWClass", "LoadFeatGain"),
    (0x00510dc0, "CSWClass", "IsGrantedFeat"),
    (0x00513bf0, "CSWGuiTargetActionMenu", "SetFriend"),
    (0x00520090, "CSWGuiMainInterface", "ShowActionQueue"),
    (0x005403b0, "CSWGuiLoadScreen", "AddHintToMessageBuffer"),
    (0x00541250, "CSWTrackFollower", "Update"),
    (0x00549ac0, "CServerInfo", "FindOptionIndex"),
    (0x00549b80, "CServerInfo", "SetModuleName"),
    (0x0054b460, "CSWMessage", "ReadVOIDPtr"),
    (0x0054c140, "CSWGuiTutorial", "CSWGuiTutorial"),
    (0x0055c300, "CSWCSoundObject", "UpdateVolumeVariation"),
    (0x00562100, "CSWCRoom", "SpawnRoom"),
    (0x0056b430, "CSWGuiOptionsGraphics", "OnAdvanced"),
    (0x00573a10, "CSWBehaviorCameraCombat", "DetermineSafePosition"),
    (0x0057b560, "CSWCProjectile", "SetProjectileVelAndAccel"),
    (0x00586860, "CSWGuiTutorialBox", "SetMessage"),
    (0x005873d0, "CSWGuiSkillFlow", "CSWGuiSkillFlow"),
    (0x00589df0, "CSWGuiSkillFlowChart", "HandleInput"),
    (0x005994b0, "CSWGuiInGameCharacter", "Draw"),
    (0x0059bf40, "CSWGuiListBox", "SetTopVisible"),
    (0x005db120, "CSWCAnimBaseTrigger", "GetAnimationName"),
    (0x005ec150, "CAurBehaviorAttachTranslate", "Control"),
    (0x005f2e20, "CSWGuiLevelUpPanel", "OnSelectPowersButton"),
    (0x005f42f0, "CSWGuiCustomPanel", "OnSelectFeatsButton"),
    (0x00603f10, "CSWGuiPortraitCharGen", "OnSelectDisplayButton"),
    (0x00604730, "CSWGuiPortraitCharGen", "Draw"),
    (0x0060cf00, "CExoStringList", "Introduce"),
    (0x0060dda0, "CExoTimersInternal", "CExoTimersInternal"),
    (0x0060f8e0, "CExoFileInternal", "ReadAsync"),
    (0x00611c00, "CExoInputClass", "CExoInputClass"),
    (0x00617080, "CExoImeUI", "HandleImeStatusChange"),
    (0x0061af60, "CExoResMan", "AddResourceImageFilesAsync"),
    (0x0062b170, "CERFFile", "Create"),
    (0x0062fc90, "CExoKeyTable", "BuildNewTable"),
    (0x006380c0, "CExoSoundInternal", "LoadPriorityGroups"),
    (0x00638930, "CExoSoundInternal", "LoadProviderPreferences"),
    (0x0063cf80, "CExoStreamingSoundSourceInternal", "Update3DStream"),
    (0x00642870, "CVirtualMachineScript", "CVirtualMachineScript"),
    (0x00647090, "CPathfindInformation", "Shutdown"),
    (0x0064be20, "CServerExoApp", "GetDebugMode"),
    (0x0064c210, "CServerExoApp", "GetActiveExclusionList"),
    (0x0064c5c0, "CServerExoApp", "ClearAllQueuedCombatActions"),
    (0x0065aae0, "CSWSCreature", "GetWeaponDischargeTime"),
    (0x0065df60, "CSWSCreature", "ResolveDefensiveEffects"),
    (0x0065f950, "CSWSCreature", "ResolveDamageShields"),
    (0x00667090, "CSWSItem", "UpdateUsedActiveProperties"),
    (0x0066afc0, "CSWSItem", "GetPropertyDescription"),
    (0x0066c760, "CSWSItem", "GetDamageTypeString"),
    (0x0067cf00, "CSWSCreature", "ComputeSpellRange"),
    (0x00693b40, "CSWSCreatureStats", "GetFeatRemainingUses"),
    (0x006ad730, "CClientExoApp", "IsSoundPlayingInDialog"),
    (0x006db480, "CSWSCreature", "CanUnEquipWeapon"),
    (0x006f6d70, "CSWSCreatureStats", "GetIsInSpellLikeAbilityList"),
    (0x006f9c90, "CSWSCreatureStats", "GetSpellLikeAbilityCasterLevel"),
    (0x006fd5a0, "CSWPartyTable", "GetPartyItems"),
    (0x006fda30, "CSWPartyTable", "UpdateMembers"),
    (0x00703650, "CSWPartyTable", "AddGameInProgress"),
    (0x007036e0, "CSWPartyTable", "RemoveGameInProgress"),
    (0x00706ac0, "CSWSArea", "ComputeNonVisibleLocation"),
    (0x00707350, "CSWSArea", "IntersectingLineSegment"),
    (0x00714000, "CSWRoomSurfaceMesh", "ComputeClippedLineSegment"),
    (0x007184d0, "CSWSArea", "IncrementPlayersInArea"),
    (0x007221c0, "CSWSArea", "NoNonWalkPolys"),
    (0x00722f60, "CSWSArea", "EnforceNoPCHeadCloning"),
    (0x0074c8d0, "CSWSEffectListHandler", "IsEffectToIgnoreOnDeath"),
    (0x0074ca90, "CExoArrayList<LARGE_INTEGER>", "AddUnique"),
    (0x0074d050, "CSWSDialog", "CSWSDialog"),
    (0x0075da10, "CSWSTrigger", "CalculateNearestPoint"),
    (0x007672c0, "CSWSDoor", "LoadFromTemplate"),
    (0x00778730, "CSWSAreaMap", "IsWorldPointExplored"),
    (0x00778b80, "CSWSJournal", "GetQuestEntryXP"),
    (0x00778bf0, "CSWSJournal", "GetState"),
    (0x007c15d0, "CSWSStore", "AddItemToInventory"),
    (0x007c43d0, "CSWSItemPropertyHandler", "RemoveAttackBonus"),
    (0x007cd8a0, "CSWSMessage", "SendServerToPlayerLogin_NeedCharacter"),
    (0x007ce400, "CSWSMessage", "SendServerToPlayerCharList"),
    (0x007e3b90, "CSWSMessage", "SendServerToPlayerAmbientSoundVolumeChange"),
    (0x007e75e0, "CSWSAreaOfEffectObject", "SetShape"),
    (0x007ef670, "CFactionManager", "LoadReputations"),
    (0x007f00e0, "CSWSScriptVarTable", "GetString"),
    (0x007f1220, "CSWSExpressionList", "DeleteList"),
    (0x007f39b0, "CSWLevelStats", "~CSWLevelStats"),
    (0x007f6b70, "CSWSSoundObject", "LoadFromTemplate"),
    (0x0080b0b0, "CWorldTimer", "SetWorldTime"),
    (0x0080b310, "CWorldTimer", "ResetTimer"),
    (0x0080b580, "CWorldTimer", "UnpauseWorldTimer"),
    (0x00811600, "CSWNameGen", "LoadNameTable"),
    (0x0085a640, "Scene", "DoGobBuckets"),
    (0x00868320, "", "dofile"),
    (0x008904a0, "PartAnimMesh", "Draw"),
    (0x0089a850, "Gob", "RenderBlur"),
    (0x008aeed0, "PartEmitter", "updateParticleFrame"),
    (0x008d5470, "", "NewCAurGUIImage"),
    (0x008f5230, "Material", "ReplaceTexture"),
    (0x009147a0, "CSWGuiImage", "Initialize"),
    (0x0091ab60, "CSWGuiScrollBar", "HitCheckScrollbar"),
]

skipped = 0
renamed = 0
for entry in RENAMES:
    addr_int = entry[0]
    ns_name  = entry[1]
    fn_name  = entry[2]
    addr = toAddr(addr_int)
    fn = getFunctionAt(addr)
    if fn is None:
        skipped += 1
        continue
    cur = fn.getName()
    if not cur.startswith("FUN_"):
        skipped += 1
        continue
    try:
        if ns_name:
            ns = NamespaceUtils.createNamespaceHierarchy(
                ns_name, None, currentProgram, SourceType.USER_DEFINED)
            fn.setParentNamespace(ns)
        fn.setName(fn_name, SourceType.USER_DEFINED)
        renamed += 1
        print("OK  %08x  %s::%s" % (addr_int, ns_name, fn_name))
    except Exception as ex:
        skipped += 1
        print("ERR %08x  %s::%s  (%s)" % (addr_int, ns_name, fn_name, ex))

print("Done: renamed=%d  skipped=%d" % (renamed, skipped))
