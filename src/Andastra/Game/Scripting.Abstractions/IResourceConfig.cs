using System.Collections.Generic;

namespace Andastra.Game.Games.Common
{
    /// <summary>
    /// Game-specific resource configuration.
    /// </summary>
    public interface IResourceConfig
    {
        string ChitinKeyFile { get; }
        IReadOnlyList<string> TexturePackFiles { get; }
        string DialogTlkFile { get; }
        string ModulesDirectory { get; }
        string OverrideDirectory { get; }
        string SavesDirectory { get; }
    }
}
