using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resources;

namespace CSharpKOTOR.Resource.Generics
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/ifo.py
    // Original: construct_ifo and dismantle_ifo functions
    public static class IFOHelpers
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/ifo.py:127-182
        // Original: def construct_ifo(gff: GFF) -> IFO:
        public static IFO ConstructIfo(GFF gff)
        {
            var ifo = new IFO();
            var root = gff.Root;

            // Extract basic fields
            ifo.ModId = root.Acquire<byte[]>("Mod_ID", new byte[16]);
            ifo.Name = root.Acquire<LocalizedString>("Mod_Name", LocalizedString.FromInvalid());
            ifo.Tag = root.Acquire<string>("Tag", "");
            ifo.EntryArea = root.Acquire<ResRef>("Mod_Entry_Area", ResRef.FromBlank());
            ifo.EntryX = root.Acquire<float>("Mod_Entry_X", 0.0f);
            ifo.EntryY = root.Acquire<float>("Mod_Entry_Y", 0.0f);
            ifo.EntryZ = root.Acquire<float>("Mod_Entry_Z", 0.0f);
            ifo.EntryDirectionX = root.Acquire<float>("Mod_Entry_Dir_X", 0.0f);
            ifo.EntryDirectionY = root.Acquire<float>("Mod_Entry_Dir_Y", 0.0f);
            ifo.EntryDirectionZ = root.Acquire<float>("Mod_Entry_Dir_Z", 0.0f);

            // Extract script hooks
            ifo.OnClientEnter = root.Acquire<ResRef>("Mod_OnClientEnter", ResRef.FromBlank());
            ifo.OnClientLeave = root.Acquire<ResRef>("Mod_OnClientLeave", ResRef.FromBlank());
            ifo.OnHeartbeat = root.Acquire<ResRef>("Mod_OnHeartbeat", ResRef.FromBlank());
            ifo.OnUserDefined = root.Acquire<ResRef>("Mod_OnUserDefined", ResRef.FromBlank());
            ifo.OnActivateItem = root.Acquire<ResRef>("Mod_OnActivateItem", ResRef.FromBlank());
            ifo.OnAcquireItem = root.Acquire<ResRef>("Mod_OnAcquireItem", ResRef.FromBlank());
            ifo.OnUnacquireItem = root.Acquire<ResRef>("Mod_OnUnacquireItem", ResRef.FromBlank());
            ifo.OnPlayerDeath = root.Acquire<ResRef>("Mod_OnPlayerDeath", ResRef.FromBlank());
            ifo.OnPlayerDying = root.Acquire<ResRef>("Mod_OnPlayerDying", ResRef.FromBlank());
            ifo.OnPlayerRespawn = root.Acquire<ResRef>("Mod_OnPlayerRespawn", ResRef.FromBlank());
            ifo.OnPlayerRest = root.Acquire<ResRef>("Mod_OnPlayerRest", ResRef.FromBlank());
            ifo.OnPlayerLevelUp = root.Acquire<ResRef>("Mod_OnPlayerLevelUp", ResRef.FromBlank());
            ifo.OnPlayerCancelCutscene = root.Acquire<ResRef>("Mod_OnPlayerCancelCutscene", ResRef.FromBlank());

            // Extract area list
            var areaList = root.Acquire<GFFList>("Mod_Area_list", new GFFList());
            foreach (var areaStruct in areaList)
            {
                var areaName = areaStruct.Acquire<ResRef>("Area_Name", ResRef.FromBlank());
                ifo.AreaList.Add(areaName);
            }

            // Extract other fields
            ifo.ExpansionPack = root.Acquire<int>("Mod_Expansion_Pack", 0);
            ifo.Description = root.Acquire<LocalizedString>("Mod_Description", LocalizedString.FromInvalid());
            ifo.ModVersion = root.Acquire<int>("Mod_Version", 0);
            ifo.VaultId = root.Acquire<int>("Mod_Vault_ID", 0);

            return ifo;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/ifo.py:183-277
        // Original: def dismantle_ifo(ifo: IFO, game: Game = Game.K2) -> GFF:
        public static GFF DismantleIfo(IFO ifo, Game game = Game.K2)
        {
            var gff = new GFF(GFFContent.IFO);
            var root = gff.Root;

            // Set basic fields
            root.SetBinary("Mod_ID", ifo.ModId);
            root.SetLocString("Mod_Name", ifo.Name);
            root.SetString("Tag", ifo.Tag);
            root.SetResRef("Mod_Entry_Area", ifo.EntryArea);
            root.SetSingle("Mod_Entry_X", ifo.EntryX);
            root.SetSingle("Mod_Entry_Y", ifo.EntryY);
            root.SetSingle("Mod_Entry_Z", ifo.EntryZ);
            root.SetSingle("Mod_Entry_Dir_X", ifo.EntryDirectionX);
            root.SetSingle("Mod_Entry_Dir_Y", ifo.EntryDirectionY);
            root.SetSingle("Mod_Entry_Dir_Z", ifo.EntryDirectionZ);

            // Set script hooks
            root.SetResRef("Mod_OnClientEnter", ifo.OnClientEnter);
            root.SetResRef("Mod_OnClientLeave", ifo.OnClientLeave);
            root.SetResRef("Mod_OnHeartbeat", ifo.OnHeartbeat);
            root.SetResRef("Mod_OnUserDefined", ifo.OnUserDefined);
            root.SetResRef("Mod_OnActivateItem", ifo.OnActivateItem);
            root.SetResRef("Mod_OnAcquireItem", ifo.OnAcquireItem);
            root.SetResRef("Mod_OnUnacquireItem", ifo.OnUnacquireItem);
            root.SetResRef("Mod_OnPlayerDeath", ifo.OnPlayerDeath);
            root.SetResRef("Mod_OnPlayerDying", ifo.OnPlayerDying);
            root.SetResRef("Mod_OnPlayerRespawn", ifo.OnPlayerRespawn);
            root.SetResRef("Mod_OnPlayerRest", ifo.OnPlayerRest);
            root.SetResRef("Mod_OnPlayerLevelUp", ifo.OnPlayerLevelUp);
            root.SetResRef("Mod_OnPlayerCancelCutscene", ifo.OnPlayerCancelCutscene);

            // Set area list
            var areaList = new GFFList();
            root.SetList("Mod_Area_list", areaList);
            foreach (var areaName in ifo.AreaList)
            {
                var areaStruct = areaList.Add(0);
                areaStruct.SetResRef("Area_Name", areaName);
            }

            // Set other fields
            root.SetInt32("Mod_Expansion_Pack", ifo.ExpansionPack);
            root.SetLocString("Mod_Description", ifo.Description);
            root.SetInt32("Mod_Version", ifo.ModVersion);
            root.SetInt32("Mod_Vault_ID", ifo.VaultId);

            return gff;
        }
    }
}
