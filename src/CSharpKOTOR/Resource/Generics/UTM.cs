using System.Collections.Generic;
using CSharpKOTOR.Common;
using CSharpKOTOR.Resources;
using JetBrains.Annotations;

namespace CSharpKOTOR.Resource.Generics
{
    /// <summary>
    /// Stores merchant data.
    ///
    /// UTM (User Template Merchant) files define merchant/store blueprints. Stored as GFF format
    /// with inventory, pricing, and script references.
    /// </summary>
    [PublicAPI]
    public sealed class UTM
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utm.py:16
        // Original: BINARY_TYPE = ResourceType.UTM
        public static readonly ResourceType BinaryType = ResourceType.UTM;

        // Basic UTM properties
        public ResRef ResRef { get; set; } = ResRef.FromBlank();
        public LocalizedString Name { get; set; } = LocalizedString.FromInvalid();
        public string Tag { get; set; } = string.Empty;
        public int MarkUp { get; set; }
        public int MarkDown { get; set; }
        public string Comment { get; set; } = string.Empty;
        public int OnOpen { get; set; }
        public int OnStore { get; set; }
        public ResRef OnOpenScript { get; set; } = ResRef.FromBlank();
        public ResRef OnStoreScript { get; set; } = ResRef.FromBlank();

        // Inventory items
        public List<UTMItem> Items { get; set; } = new List<UTMItem>();

        public UTM()
        {
        }
    }

    /// <summary>
    /// Represents an item in a merchant's inventory.
    /// </summary>
    [PublicAPI]
    public sealed class UTMItem
    {
        public ResRef ResRef { get; set; } = ResRef.FromBlank();
        public int Infinite { get; set; }
        public int Droppable { get; set; }

        public UTMItem()
        {
        }
    }
}
