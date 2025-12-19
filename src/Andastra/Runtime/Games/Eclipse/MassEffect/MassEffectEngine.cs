using Andastra.Runtime.Engines.Common;
using Andastra.Parsing.Common;

namespace Andastra.Runtime.Engines.Eclipse.MassEffect
{
    /// <summary>
    /// Mass Effect engine implementation (MassEffect.exe).
    /// </summary>
    /// <remarks>
    /// Mass Effect Engine:
    /// - Based on MassEffect.exe: Eclipse/Unreal Engine architecture
    /// - Save/Load: intABioWorldInfoexecBioSaveGame @ 0x11800ca0
    ///   intABioWorldInfoexecSaveGamesExist @ 0x117fef90
    ///   intABioWorldInfoexecOnSaveGameNotFound @ 0x117fefd8
    ///   intABioWorldInfoexecSetDisplayRealSaveGameNames @ 0x117ff320
    ///   intABioWorldInfoexecGetDisplayRealSaveGameNames @ 0x117ff380
    ///   intUBioSFHandler_PCSaveGameexecSaveComplete @ 0x11811870
    ///   intUBioSFHandler_SaveGameexecSaveComplete @ 0x11812920
    ///   intUBioSFHandler_SaveGameexecSaveGameConfirm @ 0x11812978
    ///   intUBioSaveGameexecApplyGameOptions @ 0x11813c38
    ///   intUBioSaveGameexecSetGameOptions @ 0x11813c80
    ///   intUBioSaveGameexecNativeReset @ 0x11813cc4
    ///   intUBioSaveGameexecGetTimePlayed @ 0x11813d08
    ///   intUBioSaveGameexecGetStorageDevice @ 0x11813d50
    ///   intUBioSaveGameexecSaveCharacter @ 0x11813d98
    ///   intUBioSaveGameexecClearWorldSaveObject @ 0x11813de0
    ///   intUBioSaveGameexecEmptySavedMaps @ 0x11813e30
    ///   intUBioSaveGameexecShowSavingMessageBox @ 0x11813e78
    ///   intUBioSaveGameexecIsAutoSaveComplete @ 0x11813ec8
    ///   intUBioSaveGameexecAutoSaveDelegate @ 0x11813f18
    ///   intUBioSaveGameexecTryAutoSaving @ 0x11813f60
    /// - Dialogue: intUBioConversationexecStartConversation @ 0x117fb620, intUBioConversationexecEndConversation @ 0x117fb5d0
    ///   intUBioConversationexecGetReplyText @ 0x117fb1a0, intUBioConversationexecGetEntryText @ 0x117fb1e8
    ///   intUBioConversationexecGetSpeaker @ 0x117fb230, intUBioConversationexecSelectReply @ 0x117fb530
    ///   intUBioConversationexecUpdateConversation @ 0x117fb578, intUBioConversationexecIsAmbient @ 0x117fb3b8
    ///   intABioWorldInfoexecStartConversation @ 0x117ffa78, intABioWorldInfoexecEndCurrentConversation @ 0x117ffa20
    ///   intABioWorldInfoexecInterruptConversation @ 0x117ff970, intUMassEffectGuiManagerexecIsInConversation @ 0x11813280
    /// - Combat: intUBioActorBehaviorexecEnterCombatStasis @ 0x117ed418, intUBioActorBehaviorexecExitCombatStasis @ 0x117ed3c0
    ///   intABioPlayerSquadexecIsInCombat @ 0x11809418, intABioPlayerSquadexecProbeOnCombatBegin @ 0x118093c0
    ///   intABioPlayerSquadexecProbeOnCombatEnd @ 0x11809370, intUBioGamerProfileexecGetCombatDifficulty @ 0x117e7fe8
    ///   intUBioGamerProfileexecSetCombatDifficulty @ 0x117e8040, intUBioProbeCombatexecStart @ 0x11813920
    ///   intUBioProbeCombatexecStop @ 0x118138e8, intUBioProbeCombatexecReset @ 0x118138b0
    /// - Module/Package: intABioSPGameexecPreloadPackage @ 0x117fede8, Engine.StartupPackages @ 0x11849d54
    ///   Package @ 0x11849d84, intUBioMorphFaceFrontEndexecPreload2DAPackage @ 0x1180ecc0
    /// </remarks>
    public class MassEffectEngine : EclipseEngine
    {
        public MassEffectEngine(IEngineProfile profile)
            : base(profile, Game.ME)
        {
        }

        protected override IEngineGame CreateGameSessionInternal()
        {
            return new MassEffectGameSession(this);
        }
    }
}

