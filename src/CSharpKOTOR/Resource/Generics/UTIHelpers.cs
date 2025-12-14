using System;
using System.Collections.Generic;
using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resources;
using static CSharpKOTOR.Common.GameExtensions;
using GFFAuto = CSharpKOTOR.Formats.GFF.GFFAuto;

namespace CSharpKOTOR.Resource.Generics
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/uti.py
    // Original: construct_uti and dismantle_uti functions
    public static class UTIHelpers
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/uti.py:203-280
        // Original: def construct_uti(gff: GFF) -> UTI:
        public static UTI ConstructUti(GFF gff)
        {
            var uti = new UTI();
            var root = gff.Root;

            // Extract basic fields
            uti.ResRef = root.Acquire<ResRef>("TemplateResRef", ResRef.FromBlank());
            uti.BaseItem = root.Acquire<int>("BaseItem", 0);
            uti.Name = root.Acquire<LocalizedString>("LocalizedName", LocalizedString.FromInvalid());
            uti.Description = root.Acquire<LocalizedString>("DescIdentified", LocalizedString.FromInvalid());
            uti.DescriptionUnidentified = root.Acquire<LocalizedString>("Description", LocalizedString.FromInvalid());
            uti.Tag = root.Acquire<string>("Tag", "");
            uti.Charges = root.Acquire<int>("Charges", 0);
            uti.Cost = root.Acquire<int>("Cost", 0);
            uti.StackSize = root.Acquire<int>("StackSize", 0);
            uti.Plot = root.Acquire<int>("Plot", 0);
            uti.AddCost = root.Acquire<int>("AddCost", 0);
            uti.PaletteId = root.Acquire<int>("PaletteID", 0);
            uti.Comment = root.Acquire<string>("Comment", "");
            uti.ModelVariation = root.Acquire<int>("ModelVariation", 0);
            uti.BodyVariation = root.Acquire<int>("BodyVariation", 0);
            uti.TextureVariation = root.Acquire<int>("TextureVar", 0);
            uti.UpgradeLevel = root.Acquire<int>("UpgradeLevel", 0);
            uti.Stolen = root.Acquire<int>("Stolen", 0);
            uti.Identified = root.Acquire<int>("Identified", 0);

            // Extract properties list
            var propertiesList = root.Acquire<GFFList>("PropertiesList", new GFFList());
            uti.Properties.Clear();
            foreach (var propStruct in propertiesList)
            {
                var prop = new UTIProperty();
                prop.CostTable = propStruct.Acquire<int>("CostTable", 0);
                prop.CostValue = propStruct.Acquire<int>("CostValue", 0);
                prop.Param1 = propStruct.Acquire<int>("Param1", 0);
                prop.Param1Value = propStruct.Acquire<int>("Param1Value", 0);
                prop.PropertyName = propStruct.Acquire<int>("PropertyName", 0);
                prop.Subtype = propStruct.Acquire<int>("Subtype", 0);
                prop.ChanceAppear = propStruct.Acquire<int>("ChanceAppear", 100);
                if (propStruct.Exists("UpgradeType"))
                {
                    prop.UpgradeType = propStruct.Acquire<int>("UpgradeType", 0);
                }
                uti.Properties.Add(prop);
            }

            return uti;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/uti.py:283-400
        // Original: def dismantle_uti(uti: UTI, game: Game = Game.K2, *, use_deprecated: bool = True) -> GFF:
        public static GFF DismantleUti(UTI uti, Game game = Game.K2, bool useDeprecated = true)
        {
            var gff = new GFF(GFFContent.UTI);
            var root = gff.Root;

            // Set basic fields
            root.SetResRef("TemplateResRef", uti.ResRef);
            root.SetInt32("BaseItem", uti.BaseItem);
            root.SetLocString("LocalizedName", uti.Name);
            root.SetLocString("Description", uti.DescriptionUnidentified);
            root.SetLocString("DescIdentified", uti.Description);
            root.SetString("Tag", uti.Tag);
            root.SetUInt8("Charges", (byte)uti.Charges);
            root.SetUInt32("Cost", (uint)uti.Cost);
            root.SetUInt16("StackSize", (ushort)uti.StackSize);
            root.SetUInt8("Plot", (byte)uti.Plot);
            root.SetUInt32("AddCost", (uint)uti.AddCost);
            root.SetUInt8("PaletteID", (byte)uti.PaletteId);
            root.SetString("Comment", uti.Comment);
            root.SetUInt8("ModelVariation", (byte)uti.ModelVariation);
            root.SetUInt8("BodyVariation", (byte)uti.BodyVariation);
            root.SetUInt8("TextureVar", (byte)uti.TextureVariation);

            // KotOR 2 only fields
            if (game.IsK2())
            {
                root.SetUInt8("UpgradeLevel", (byte)uti.UpgradeLevel);
            }

            if (useDeprecated)
            {
                root.SetUInt8("Stolen", (byte)uti.Stolen);
                root.SetUInt8("Identified", (byte)uti.Identified);
            }

            // Set properties list
            var propertiesList = new GFFList();
            root.SetList("PropertiesList", propertiesList);
            if (uti.Properties != null)
            {
                foreach (var prop in uti.Properties)
                {
                    var propStruct = propertiesList.Add(0);
                    propStruct.SetUInt8("CostTable", (byte)prop.CostTable);
                    propStruct.SetUInt16("CostValue", (ushort)prop.CostValue);
                    propStruct.SetUInt8("Param1", (byte)prop.Param1);
                    propStruct.SetUInt8("Param1Value", (byte)prop.Param1Value);
                    propStruct.SetUInt16("PropertyName", (ushort)prop.PropertyName);
                    propStruct.SetUInt16("Subtype", (ushort)prop.Subtype);
                    propStruct.SetUInt8("ChanceAppear", (byte)prop.ChanceAppear);
                    if (prop.UpgradeType.HasValue)
                    {
                        propStruct.SetUInt8("UpgradeType", (byte)prop.UpgradeType.Value);
                    }
                }
            }

            return gff;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/uti.py:371-390
        // Original: def read_uti(source: SOURCE_TYPES, offset: int = 0, size: int | None = None) -> UTI:
        public static UTI ReadUti(byte[] data, int offset = 0, int size = -1)
        {
            byte[] dataToRead = data;
            if (size > 0 && offset + size <= data.Length)
            {
                dataToRead = new byte[size];
                System.Array.Copy(data, offset, dataToRead, 0, size);
            }
            GFF gff = GFF.FromBytes(dataToRead);
            return ConstructUti(gff);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/uti.py:392-407
        // Original: def bytes_uti(uti: UTI, game: Game = Game.K2, file_format: ResourceType = ResourceType.GFF) -> bytes:
        public static byte[] BytesUti(UTI uti, Game game = Game.K2, ResourceType fileFormat = null)
        {
            if (fileFormat == null)
            {
                fileFormat = ResourceType.UTI;
            }
            GFF gff = DismantleUti(uti, game);
            return GFFAuto.BytesGff(gff, fileFormat);
        }
    }
}
