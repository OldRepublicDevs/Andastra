using System.Collections.Generic;

namespace Andastra.Game.Games.Common
{
    /// <summary>
    /// Game-specific 2DA table configuration.
    /// </summary>
    public interface ITableConfig
    {
        IReadOnlyList<string> RequiredTables { get; }
        IReadOnlyDictionary<string, string> AppearanceColumns { get; }
        IReadOnlyDictionary<string, string> BaseItemsColumns { get; }
    }
}
