using Andastra.Game.Games.Common;

namespace Andastra.Game.Scripting.Abstractions
{
    /// <summary>
    /// Base interface for all BioWare engine implementations.
    /// Runtime-specific types (resource provider, world, game session) are exposed as object to avoid Runtime dependency.
    /// </summary>
    public interface IEngine
    {
        EngineFamily EngineFamily { get; }
        IEngineProfile Profile { get; }
        object ResourceProvider { get; }
        object World { get; }
        IEngineApi EngineApi { get; }
        object CreateGameSession();
        void Initialize(string installationPath);
        void Shutdown();
    }
}
