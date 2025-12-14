using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.ERF;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resources;
using Odyssey.Core.Save;

namespace Odyssey.Content.Save
{
    /// <summary>
    /// Serializes save data to/from KOTOR save file formats.
    /// </summary>
    /// <remarks>
    /// Save file formats:
    /// - savenfo.res: GFF containing metadata
    /// - savegame.sav: ERF archive containing:
    ///   - GLOBALVARS.res (GFF)
    ///   - PARTYTABLE.res (GFF)
    ///   - [module]_s.rim files
    ///
    /// Based on swkotor2.exe save serialization:
    /// - Save NFO: FUN_004eb750 @ 0x004eb750 (creates GFF with "NFO " signature, "V2.0" version)
    /// - Save archive: FUN_004eb750 @ 0x004eb750 (creates ERF with "MOD V1.0" signature @ 0x007be0d4)
    /// - GLOBALVARS serialization: FUN_005ac670 @ 0x005ac670
    /// - PARTYTABLE serialization: FUN_0057bd70 @ 0x0057bd70 (creates GFF with "PT  " signature)
    /// </remarks>
    public class SaveSerializer : ISaveSerializer
    {
        // GFF field labels for save NFO
        private const string FIELD_SAVE_NAME = "SAVENAME";
        private const string FIELD_MODULE_NAME = "MODULENAME";
        private const string FIELD_SAVE_DATE = "SAVEDATE";
        private const string FIELD_SAVE_TIME = "SAVETIME";
        private const string FIELD_TIME_PLAYED = "TIMEPLAYED";
        private const string FIELD_PLAYER_NAME = "PLAYERNAME";
        private const string FIELD_PORTRAIT = "PORTRAIT";
        private const string FIELD_CHEAT_USED = "CHEATUSED";

        // ERF types
        private const string ERF_TYPE_SAV = "SAV ";

        #region ISaveSerializer Implementation

        // Serialize save metadata to NFO GFF format
        // Based on swkotor2.exe: FUN_004eb750 @ 0x004eb750
        // Located via string reference: "savenfo" @ 0x007be1f0
        // Original implementation: Creates GFF with "NFO " signature and "V2.0" version string
        // Writes fields: SAVEGAMENAME, MODULENAME, SAVEDATE, SAVETIME, TIMEPLAYED, PLAYERNAME, PORTRAIT, CHEATUSED,
        // AREANAME, LASTMODULE, PCNAME, SAVENUMBER, GAMEPLAYHINT, STORYHINT0-9, LIVECONTENT, TIMESTAMP
        public byte[] SerializeSaveNfo(SaveGameData saveData)
        {
            // Use CSharpKOTOR GFF writer
            var gff = new GFF(GFFContent.GFF);
            var root = gff.Root;

            root.SetString(FIELD_SAVE_NAME, saveData.Name ?? "");
            root.SetString(FIELD_MODULE_NAME, saveData.CurrentModule ?? "");
            root.SetString(FIELD_SAVE_DATE, saveData.SaveTime.ToString("yyyy-MM-dd"));
            root.SetString(FIELD_SAVE_TIME, saveData.SaveTime.ToString("HH:mm:ss"));
            root.SetInt32(FIELD_TIME_PLAYED, (int)saveData.PlayTime.TotalSeconds);
            root.SetString(FIELD_PLAYER_NAME, ""); // TODO: Get player name from party state
            root.SetInt32(FIELD_CHEAT_USED, 0); // TODO: Track cheat usage

            return gff.ToBytes();
        }

        // Deserialize save metadata from NFO GFF format
        // Based on swkotor2.exe: FUN_00707290 @ 0x00707290
        // Located via string reference: "savenfo" @ 0x007be1f0
        // Original implementation: Reads GFF with "NFO " signature, extracts SAVEGAMENAME, MODULENAME, SAVEDATE, SAVETIME,
        // TIMEPLAYED, PLAYERNAME, CHEATUSED, REBOOTAUTOSAVE, PCAUTOSAVE, SCREENSHOT, TIMESTAMP, PCNAME, SAVENUMBER,
        // GAMEPLAYHINT, STORYHINT0-9, LIVECONTENT flags
        public SaveGameData DeserializeSaveNfo(byte[] data)
        {
            // Use CSharpKOTOR GFF reader
            GFF gff;
            try
            {
                gff = GFF.FromBytes(data);
            }
            catch (Exception)
            {
                return null;
            }

            if (gff == null || gff.Root == null)
            {
                return null;
            }

            var saveData = new SaveGameData();
            var root = gff.Root;

            saveData.Name = root.GetString(FIELD_SAVE_NAME);
            saveData.CurrentModule = root.GetString(FIELD_MODULE_NAME);

            string dateStr = root.GetString(FIELD_SAVE_DATE);
            string timeStr = root.GetString(FIELD_SAVE_TIME);

            DateTime saveTime;
            if (DateTime.TryParse(dateStr + " " + timeStr, out saveTime))
            {
                saveData.SaveTime = saveTime;
            }
            else
            {
                saveData.SaveTime = DateTime.Now;
            }

            int seconds = root.GetInt32(FIELD_TIME_PLAYED);
            saveData.PlayTime = TimeSpan.FromSeconds(seconds);

            return saveData;
        }

        // Serialize save game archive to ERF format
        // Based on swkotor2.exe: FUN_004eb750 @ 0x004eb750
        // Located via string reference: "MOD V1.0" @ 0x007be0d4
        // Original implementation: Creates ERF archive with "MOD V1.0" signature, adds GLOBALVARS.res (GFF),
        // PARTYTABLE.res (GFF), and module state files ([module]_s.rim) for each visited area
        public byte[] SerializeSaveArchive(SaveGameData saveData)
        {
            // Use CSharpKOTOR ERF writer
            var erf = new ERF(ERFType.MOD, true); // MOD type is used for save files

            // Add GLOBALVARS.res
            byte[] globalVarsData = SerializeGlobalVariables(saveData.GlobalVariables);
            erf.SetData("GLOBALVARS", ResourceType.GFF, globalVarsData);

            // Add PARTYTABLE.res
            byte[] partyTableData = SerializePartyTable(saveData.PartyState);
            erf.SetData("PARTYTABLE", ResourceType.GFF, partyTableData);

            // Add module state files
            if (saveData.AreaStates != null)
            {
                foreach (KeyValuePair<string, AreaState> kvp in saveData.AreaStates)
                {
                    string areaResRef = kvp.Key;
                    AreaState areaState = kvp.Value;

                    string stateFileName = areaResRef + "_s";
                    byte[] stateData = SerializeAreaState(areaState);
                    erf.SetData(stateFileName, ResourceType.RIM, stateData);
                }
            }

            // Write ERF using CSharpKOTOR writer
            var writer = new ERFBinaryWriter(erf);
            return writer.Write();
        }

        // Deserialize save game archive from ERF format
        // Based on swkotor2.exe: FUN_00708990 @ 0x00708990
        // Located via string reference: "LoadSavegame" @ 0x007bdc90
        // Original implementation: Reads ERF archive with "MOD V1.0" signature, extracts GLOBALVARS.res and PARTYTABLE.res,
        // reads module state files ([module]_s.rim) for each area and stores in AreaStates dictionary
        public void DeserializeSaveArchive(byte[] data, SaveGameData saveData)
        {
            // Use CSharpKOTOR ERF reader
            ERF erf;
            try
            {
                var reader = new ERFBinaryReader(data);
                erf = reader.Load();
            }
            catch (Exception)
            {
                return;
            }

            if (erf == null)
            {
                return;
            }

            // Read GLOBALVARS.res
            byte[] globalVarsData = erf.Get("GLOBALVARS", ResourceType.GFF);
            if (globalVarsData != null)
            {
                saveData.GlobalVariables = DeserializeGlobalVariables(globalVarsData);
            }

            // Read PARTYTABLE.res
            byte[] partyTableData = erf.Get("PARTYTABLE", ResourceType.GFF);
            if (partyTableData != null)
            {
                saveData.PartyState = DeserializePartyTable(partyTableData);
            }

            // Read module state files
            if (saveData.AreaStates == null)
            {
                saveData.AreaStates = new Dictionary<string, AreaState>();
            }

            foreach (ERFResource resource in erf)
            {
                string resName = resource.ResRef.ToString();
                if (resName.EndsWith("_s") && resource.ResType == ResourceType.RIM)
                {
                    string areaResRef = resName.Substring(0, resName.Length - 2);
                    AreaState areaState = DeserializeAreaState(resource.Data);
                    if (areaState != null)
                    {
                        saveData.AreaStates[areaResRef] = areaState;
                    }
                }
            }
        }

        #endregion

        #region Global Variables

        // Serialize global variables to GFF format
        // Based on swkotor2.exe: FUN_005ac670 @ 0x005ac670
        // Located via string reference: "GLOBALVARS" @ 0x007c27bc
        // Original implementation: Creates GFF file, stores booleans, numbers, and strings as separate lists
        // Each list entry contains "Name" (string) and "Value" (int32 for bools/ints, string for strings)
        private byte[] SerializeGlobalVariables(GlobalVariableState state)
        {
            if (state == null)
            {
                state = new GlobalVariableState();
            }

            // Use CSharpKOTOR GFF writer
            var gff = new GFF(GFFContent.GFF);
            var root = gff.Root;

            // Store booleans as a list
            var boolList = root.Acquire<GFFList>("Booleans", new GFFList());
            foreach (KeyValuePair<string, bool> kvp in state.Booleans)
            {
                GFFStruct entry = boolList.Add();
                entry.SetString("Name", kvp.Key);
                entry.SetInt32("Value", kvp.Value ? 1 : 0);
            }

            // Store numbers as a list
            var numList = root.Acquire<GFFList>("Numbers", new GFFList());
            foreach (KeyValuePair<string, int> kvp in state.Numbers)
            {
                GFFStruct entry = numList.Add();
                entry.SetString("Name", kvp.Key);
                entry.SetInt32("Value", kvp.Value);
            }

            // Store strings as a list
            var strList = root.Acquire<GFFList>("Strings", new GFFList());
            foreach (KeyValuePair<string, string> kvp in state.Strings)
            {
                GFFStruct entry = strList.Add();
                entry.SetString("Name", kvp.Key);
                entry.SetString("Value", kvp.Value ?? "");
            }

            return gff.ToBytes();
        }

        // Deserialize global variables from GFF format
        // Based on swkotor2.exe: FUN_005ac740 @ 0x005ac740
        // Located via string reference: "GLOBALVARS" @ 0x007c27bc
        // Original implementation: Reads GFF file, extracts "Booleans", "Numbers", and "Strings" lists,
        // restores each variable by name and value from list entries
        private GlobalVariableState DeserializeGlobalVariables(byte[] data)
        {
            var state = new GlobalVariableState();

            if (data == null || data.Length < 12)
            {
                return state;
            }

            // Use CSharpKOTOR GFF reader
            try
            {
                GFF gff = GFF.FromBytes(data);
                if (gff == null || gff.Root == null)
                {
                    return state;
                }

                var root = gff.Root;

                // Read booleans
                GFFList boolList = root.GetList("Booleans");
                if (boolList != null)
                {
                    foreach (GFFStruct entry in boolList)
                    {
                        string name = entry.GetString("Name");
                        int value = entry.GetInt32("Value");
                        state.Booleans[name] = value != 0;
                    }
                }

                // Read numbers
                GFFList numList = root.GetList("Numbers");
                if (numList != null)
                {
                    foreach (GFFStruct entry in numList)
                    {
                        string name = entry.GetString("Name");
                        int value = entry.GetInt32("Value");
                        state.Numbers[name] = value;
                    }
                }

                // Read strings
                GFFList strList = root.GetList("Strings");
                if (strList != null)
                {
                    foreach (GFFStruct entry in strList)
                    {
                        string name = entry.GetString("Name");
                        string value = entry.GetString("Value");
                        state.Strings[name] = value;
                    }
                }
            }
            catch (Exception)
            {
                // Return empty state on error
            }

            return state;
        }

        #endregion

        #region Party Table

        // Serialize party table to GFF format
        // Based on swkotor2.exe: FUN_0057bd70 @ 0x0057bd70
        // Located via string reference: "PARTYTABLE" @ 0x007c1910
        // Original implementation: Creates GFF with "PT  " signature and "V2.0" version string
        // Writes fields: PT_PCNAME, PT_GOLD, PT_ITEM_COMPONENT, PT_ITEM_CHEMICAL, PT_SWOOP1-3, PT_XP_POOL,
        // PT_PLAYEDSECONDS, PT_CONTROLLED_NPC, PT_SOLOMODE, PT_CHEAT_USED, PT_NUM_MEMBERS, PT_MEMBERS (list),
        // PT_NUM_PUPPETS, PT_PUPPETS (list), PT_AVAIL_PUPS (list), PT_AVAIL_NPCS (list), PT_INFLUENCE (list),
        // PT_AISTATE, PT_FOLLOWSTATE, GlxyMap data, PT_PAZAAKCARDS, PT_PAZSIDELIST, PT_TUT_WND_SHOWN, PT_LAST_GUI_PNL,
        // PT_FB_MSG_LIST, PT_DLG_MSG_LIST, PT_COM_MSG_LIST, PT_COST_MULT_LIST, PT_DISABLEMAP, PT_DISABLEREGEN
        private byte[] SerializePartyTable(PartyState state)
        {
            if (state == null)
            {
                state = new PartyState();
            }

            // Use CSharpKOTOR GFF writer
            var gff = new GFF(GFFContent.GFF);
            var root = gff.Root;

            root.SetInt32("Gold", state.Gold);
            root.SetInt32("ExperiencePoints", state.ExperiencePoints);

            // Store selected party as a list
            var selectedList = root.Acquire<GFFList>("SelectedParty", new GFFList());
            foreach (string member in state.SelectedParty)
            {
                GFFStruct entry = selectedList.Add();
                entry.SetString("ResRef", member);
            }

            // Store available members as a list
            var availableList = root.Acquire<GFFList>("AvailableMembers", new GFFList());
            foreach (KeyValuePair<string, PartyMemberState> kvp in state.AvailableMembers)
            {
                GFFStruct entry = availableList.Add();
                entry.SetString("ResRef", kvp.Key);
                entry.SetInt32("IsAvailable", kvp.Value.IsAvailable ? 1 : 0);
                entry.SetInt32("IsSelectable", kvp.Value.IsSelectable ? 1 : 0);
            }

            return gff.ToBytes();
        }

        // Deserialize party table from GFF format
        // Based on swkotor2.exe: FUN_0057dcd0 @ 0x0057dcd0
        // Located via string reference: "PARTYTABLE" @ 0x007c1910
        // Original implementation: Reads GFF with "PT  " signature, extracts all party-related fields including
        // party members, puppets, available NPCs, influence values, gold, XP pool, solo mode, cheat flags,
        // galaxy map state, Pazaak cards, tutorial windows, message lists, cost multipliers, and various game state flags
        private PartyState DeserializePartyTable(byte[] data)
        {
            var state = new PartyState();

            if (data == null || data.Length < 12)
            {
                return state;
            }

            // Use CSharpKOTOR GFF reader
            try
            {
                GFF gff = GFF.FromBytes(data);
                if (gff == null || gff.Root == null)
                {
                    return state;
                }

                var root = gff.Root;

                state.Gold = root.GetInt32("Gold");
                state.ExperiencePoints = root.GetInt32("ExperiencePoints");

                // Read selected party
                GFFList selectedList = root.GetList("SelectedParty");
                if (selectedList != null)
                {
                    foreach (GFFStruct entry in selectedList)
                    {
                        string resRef = entry.GetString("ResRef");
                        state.SelectedParty.Add(resRef);
                    }
                }

                // Read available members
                GFFList availableList = root.GetList("AvailableMembers");
                if (availableList != null)
                {
                    foreach (GFFStruct entry in availableList)
                    {
                        string resRef = entry.GetString("ResRef");
                        bool isAvailable = entry.GetInt32("IsAvailable") != 0;
                        bool isSelectable = entry.GetInt32("IsSelectable") != 0;

                        var memberState = new PartyMemberState
                        {
                            TemplateResRef = resRef,
                            IsAvailable = isAvailable,
                            IsSelectable = isSelectable
                        };
                        state.AvailableMembers[resRef] = memberState;
                    }
                }
            }
            catch (Exception)
            {
                // Return empty state on error
            }

            return state;
        }

        #endregion

        #region Area State

        private byte[] SerializeAreaState(AreaState state)
        {
            if (state == null)
            {
                return new byte[0];
            }

            using (var ms = new MemoryStream())
            using (var writer = new System.IO.BinaryWriter(ms))
            {
                // GFF header
                writer.Write(Encoding.ASCII.GetBytes("GFF "));
                writer.Write(Encoding.ASCII.GetBytes("V3.2"));
                writer.Write((uint)0); // Type

                WriteGffString(writer, "AREARES", state.AreaResRef ?? "");
                writer.Write(state.Visited ? (byte)1 : (byte)0);

                // Write entity states
                SerializeEntityStates(writer, "CREATURES", state.CreatureStates);
                SerializeEntityStates(writer, "DOORS", state.DoorStates);
                SerializeEntityStates(writer, "PLACEABLES", state.PlaceableStates);
                SerializeEntityStates(writer, "TRIGGERS", state.TriggerStates);

                // Write destroyed entity IDs
                writer.Write((uint)state.DestroyedEntityIds.Count);
                foreach (uint id in state.DestroyedEntityIds)
                {
                    writer.Write(id);
                }

                return ms.ToArray();
            }
        }

        private AreaState DeserializeAreaState(byte[] data)
        {
            var state = new AreaState();

            if (data == null || data.Length < 12)
            {
                return state;
            }

            // TODO: Proper GFF parsing with CSharpKOTOR
            return state;
        }

        private void SerializeEntityStates(System.IO.BinaryWriter writer, string label, List<EntityState> states)
        {
            writer.Write((uint)states.Count);
            foreach (EntityState entityState in states)
            {
                SerializeEntityState(writer, entityState);
            }
        }

        private void SerializeEntityState(System.IO.BinaryWriter writer, EntityState state)
        {
            WriteGffString(writer, "TAG", state.Tag ?? "");
            writer.Write(state.ObjectId);
            writer.Write((int)state.ObjectType);

            // Position
            writer.Write(state.Position.X);
            writer.Write(state.Position.Y);
            writer.Write(state.Position.Z);

            // Facing
            writer.Write(state.Facing);

            // HP
            writer.Write(state.CurrentHP);
            writer.Write(state.MaxHP);

            // Flags
            writer.Write(state.IsDestroyed ? (byte)1 : (byte)0);
            writer.Write(state.IsPlot ? (byte)1 : (byte)0);
            writer.Write(state.IsOpen ? (byte)1 : (byte)0);
            writer.Write(state.IsLocked ? (byte)1 : (byte)0);
            writer.Write(state.AnimationState);
        }

        #endregion

        #region GFF Helpers

        private void WriteGffString(System.IO.BinaryWriter writer, string label, string value)
        {
            // Write label length and label
            byte[] labelBytes = Encoding.ASCII.GetBytes(label);
            writer.Write((byte)labelBytes.Length);
            writer.Write(labelBytes);

            // Write value length and value
            byte[] valueBytes = Encoding.UTF8.GetBytes(value ?? "");
            writer.Write((ushort)valueBytes.Length);
            writer.Write(valueBytes);
        }

        private void WriteGffInt(System.IO.BinaryWriter writer, string label, int value)
        {
            byte[] labelBytes = Encoding.ASCII.GetBytes(label);
            writer.Write((byte)labelBytes.Length);
            writer.Write(labelBytes);
            writer.Write(value);
        }

        private string ReadGffString(System.IO.BinaryReader reader, string expectedLabel)
        {
            try
            {
                byte labelLength = reader.ReadByte();
                byte[] labelBytes = reader.ReadBytes(labelLength);
                string label = Encoding.ASCII.GetString(labelBytes);

                ushort valueLength = reader.ReadUInt16();
                byte[] valueBytes = reader.ReadBytes(valueLength);
                return Encoding.UTF8.GetString(valueBytes);
            }
            catch
            {
                return "";
            }
        }

        private int ReadGffInt(System.IO.BinaryReader reader, string expectedLabel)
        {
            try
            {
                byte labelLength = reader.ReadByte();
                byte[] labelBytes = reader.ReadBytes(labelLength);
                return reader.ReadInt32();
            }
            catch
            {
                return 0;
            }
        }

        #endregion
    }
}
