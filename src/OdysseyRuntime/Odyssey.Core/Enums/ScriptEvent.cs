namespace Odyssey.Core.Enums
{
    /// <summary>
    /// Script event types that can trigger NWScript execution.
    /// </summary>
    public enum ScriptEvent
    {
        // Module events
        OnModuleLoad,
        OnModuleStart,
        OnModuleLeave,
        OnModuleHeartbeat,
        OnClientEnter,
        OnClientLeave,
        OnPlayerDying,
        OnPlayerDeath,
        OnPlayerRespawn,
        OnPlayerLevelUp,
        OnPlayerRest,
        OnAcquireItem,
        OnUnacquireItem,
        OnActivateItem,
        OnUserDefined,
        
        // Area events
        OnEnter,
        OnExit,
        OnAreaHeartbeat,
        
        // Creature events
        OnSpawn,
        OnDeath,
        OnDamaged,
        OnHeartbeat,
        OnPerception,
        OnCombatRoundEnd,
        OnConversation,
        OnDisturbed,
        OnBlocked,
        OnEndRound,
        OnSpellCastAt,
        OnAttacked,
        OnRested,
        
        // Door events
        OnOpen,
        OnClose,
        OnClosed,
        OnFailToOpen,
        OnDoorClick,
        OnLock,
        OnUnlock,
        
        // Placeable events
        OnUsed,
        OnPlaceableClick,
        OnPlaceableDisturbed,
        OnPlaceableLock,
        OnPlaceableUnlock,
        
        // Trigger events
        OnTriggerEnter,
        OnTriggerExit,
        OnTriggerClick,
        OnTriggerHeartbeat,
        
        // Encounter events
        OnEncounterEntered,
        OnEncounterExhausted,
        
        // Store events
        OnStoreOpen,
        OnStoreClose
    }
}

