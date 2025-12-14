using System.Collections.Generic;

namespace Odyssey.Core.Interfaces
{
    /// <summary>
    /// Represents a game module (collection of areas and global state).
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// The resource reference name of this module.
        /// </summary>
        string ResRef { get; }
        
        /// <summary>
        /// The display name of the module.
        /// </summary>
        string DisplayName { get; }
        
        /// <summary>
        /// The entry area of the module.
        /// </summary>
        string EntryArea { get; }
        
        /// <summary>
        /// Gets all areas in this module.
        /// </summary>
        IEnumerable<IArea> Areas { get; }
        
        /// <summary>
        /// Gets an area by its resource reference.
        /// </summary>
        IArea GetArea(string resRef);
        
        /// <summary>
        /// Gets the script to run for a module event.
        /// </summary>
        string GetScript(Enums.ScriptEvent eventType);
        
        /// <summary>
        /// The dawn hour (0-23).
        /// </summary>
        int DawnHour { get; }
        
        /// <summary>
        /// The dusk hour (0-23).
        /// </summary>
        int DuskHour { get; }
        
        /// <summary>
        /// Current game time - minutes past midnight.
        /// </summary>
        int MinutesPastMidnight { get; set; }
        
        /// <summary>
        /// Current game calendar day.
        /// </summary>
        int Day { get; set; }
        
        /// <summary>
        /// Current game calendar month.
        /// </summary>
        int Month { get; set; }
        
        /// <summary>
        /// Current game calendar year.
        /// </summary>
        int Year { get; set; }
    }
}

