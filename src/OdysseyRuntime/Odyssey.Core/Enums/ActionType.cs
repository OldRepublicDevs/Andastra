namespace Odyssey.Core.Enums
{
    /// <summary>
    /// Types of actions that can be queued on entities.
    /// </summary>
    public enum ActionType
    {
        Invalid = 0,
        MoveToPoint = 1,
        MoveToObject = 2,
        AttackObject = 3,
        CastSpellAtObject = 4,
        CastSpellAtLocation = 5,
        OpenDoor = 6,
        CloseDoor = 7,
        UseObject = 8,
        Wait = 9,
        PlayAnimation = 10,
        SpeakString = 11,
        JumpToLocation = 12,
        JumpToObject = 13,
        DoCommand = 14,
        EquipItem = 15,
        UnequipItem = 16,
        GiveItem = 17,
        TakeItem = 18,
        StartConversation = 19,
        PauseConversation = 20,
        ResumeConversation = 21,
        FollowLeader = 22,
        FollowOwner = 23,
        RandomWalk = 24,
        InteractObject = 25
    }
}

