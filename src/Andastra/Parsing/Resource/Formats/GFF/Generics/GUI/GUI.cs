using System.Collections.Generic;
using Andastra.Parsing.Common;
using Andastra.Parsing.Resource;
using JetBrains.Annotations;

namespace Andastra.Parsing.Resource.Generics.GUI
{
    /// <summary>
    /// A class representing a GUI resource in KotOR games.
    /// </summary>
    [PublicAPI]
    public sealed class GUI
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/gui.py:170
        // Original: class GUI:
        public static readonly ResourceType BinaryType = ResourceType.GUI;

        public string Tag { get; set; } = string.Empty;
        public GUIControl Root { get; set; }
        public List<GUIControl> Controls { get; set; } = new List<GUIControl>();

        public GUI()
        {
        }
    }
}

