using Andastra.Game.Scripting.Abstractions;

namespace Andastra.Game.Games.Common
{
    /// <summary>
    /// Base interface for game profiles across all engines.
    /// </summary>
    public interface IEngineProfile
    {
        string GameType { get; }
        string Name { get; }
        EngineFamily EngineFamily { get; }
        IEngineApi CreateEngineApi();
        IResourceConfig ResourceConfig { get; }
        ITableConfig TableConfig { get; }
        bool SupportsFeature(string feature);
    }
}
