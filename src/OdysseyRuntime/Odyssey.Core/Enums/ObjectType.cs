using System;

namespace Odyssey.Core.Enums
{
    /// <summary>
    /// Types of game objects in the Odyssey engine.
    /// </summary>
    [Flags]
    public enum ObjectType
    {
        Invalid = 0,
        Creature = 1,
        Item = 2,
        Trigger = 4,
        Door = 8,
        AreaOfEffect = 16,
        Waypoint = 32,
        Placeable = 64,
        Store = 128,
        Encounter = 256,
        Sound = 512,
        All = Creature | Item | Trigger | Door | AreaOfEffect | Waypoint | Placeable | Store | Encounter | Sound
    }
}

