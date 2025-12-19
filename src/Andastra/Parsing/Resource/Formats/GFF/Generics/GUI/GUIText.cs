using Andastra.Parsing;
using JetBrains.Annotations;
using Andastra.Parsing.Common;

namespace Andastra.Parsing.Resource.Generics.GUI
{
    /// <summary>
    /// Represents text properties for GUI controls.
    /// </summary>
    [PublicAPI]
    public class GUIText
    {
        public int Alignment { get; set; }
        public Color Color { get; set; }
        public ResRef Font { get; set; } = ResRef.FromBlank();
        public int? Pulsing { get; set; }
        public int StrRef { get; set; } = -1;
        public string Text { get; set; }

        public GUIText()
        {
        }
    }
}
