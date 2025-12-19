using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Games.Common;
using Andastra.Parsing.Formats.GFF;
using Andastra.Runtime.Core.Save;

namespace Andastra.Runtime.Games.Odyssey
{
    /// <summary>
    /// Odyssey Engine (KotOR/KotOR2) save game serializer implementation.
    /// </summary>
    /// <remarks>
    /// Odyssey Save Serializer Implementation:
    /// - Based on swkotor.exe and swkotor2.exe save systems
    /// - Uses GFF format with "SAV " and "NFO " signatures
    /// - Handles entity serialization, global variables, party data
    ///
    /// Based on reverse engineering of:
    /// - swkotor2.exe: SerializeSaveNfo @ 0x004eb750 for metadata creation
    /// - swkotor2.exe: Global variable save/load functions
    /// - Entity serialization: FUN_004e28c0 save, FUN_005fb0f0 load
    /// - Party management and companion state saving
    ///
    /// Save file structure:
    /// - Save directory with numbered subdirectories
    /// - NFO file: Metadata (name, time, area, screenshot)
    /// - SAV file: Main save data (entities, globals, party)
    /// - RES directory: Screenshots and additional resources
    /// - GFF format for structured data storage
    /// </remarks>
    [PublicAPI]
    public class OdysseySaveSerializer : BaseSaveSerializer
    {
        /// <summary>
        /// Gets the save file format version for Odyssey engine.
        /// </summary>
        /// <remarks>
        /// KotOR uses version 1, KotOR2 uses version 2.
        /// Used for compatibility checking between game versions.
        /// </remarks>
        protected override int SaveVersion => 2; // KotOR 2 version

        /// <summary>
        /// Gets the engine identifier.
        /// </summary>
        /// <remarks>
        /// Identifies this as an Odyssey engine save.
        /// Used for cross-engine compatibility detection.
        /// </remarks>
        protected override string EngineIdentifier => "Odyssey";

        /// <summary>
        /// Serializes save game metadata to NFO format.
        /// </summary>
        /// <remarks>
        /// Based on SerializeSaveNfo @ 0x004eb750 in swkotor2.exe.
        /// Creates GFF with "NFO " signature containing save information.
        /// Includes SAVEGAMENAME, TIMEPLAYED, AREANAME, and metadata.
        ///
        /// NFO structure:
        /// - Signature: "NFO "
        /// - Version: "V2.0" for KotOR 2
        /// - SAVEGAMENAME: Display name
        /// - TIMEPLAYED: Play time in seconds
        /// - AREANAME: Current area resource
        /// - LASTMODIFIED: Timestamp
        /// </remarks>
        public override byte[] SerializeSaveNfo(Andastra.Runtime.Games.Common.SaveGameData saveData)
        {
            // TODO: Implement complete NFO serialization
            // Create GFF structure with NFO signature
            // Add standard metadata fields
            // Include screenshot data if available
            // Write timestamp and version info

            throw new NotImplementedException("Odyssey NFO serialization not yet implemented");
        }

        /// <summary>
        /// Deserializes save game metadata from NFO format.
        /// </summary>
        /// <remarks>
        /// Reads NFO GFF and extracts save metadata.
        /// Validates NFO signature and version compatibility.
        /// Returns structured metadata for save game display.
        /// </remarks>
        public override SaveGameMetadata DeserializeSaveNfo(byte[] nfoData)
        {
            // TODO: Implement NFO deserialization
            // Validate NFO signature
            // Read metadata fields
            // Extract screenshot if present
            // Return structured metadata

            throw new NotImplementedException("Odyssey NFO deserialization not yet implemented");
        }

        /// <summary>
        /// Serializes global game state.
        /// </summary>
        /// <remarks>
        /// Based on global variable serialization in swkotor2.exe.
        /// Saves quest states, player choices, persistent variables.
        /// Uses GFF format with variable categories.
        ///
        /// Global categories:
        /// - QUEST: Quest completion states
        /// - CHOICE: Player dialogue choices
        /// - PERSISTENT: Long-term game state
        /// - MODULE: Per-module variables
        /// </remarks>
        public override byte[] SerializeGlobals(IGameState gameState)
        {
            // TODO: Implement global variable serialization
            // Create GFF with GLOBALS struct
            // Categorize variables by type
            // Handle different data types (int, float, string, location)
            // Include variable metadata

            throw new NotImplementedException("Odyssey global serialization not yet implemented");
        }

        /// <summary>
        /// Deserializes global game state.
        /// </summary>
        /// <remarks>
        /// Restores global variables from save data.
        /// Updates quest states and player choice consequences.
        /// Validates variable integrity and types.
        /// </remarks>
        public override void DeserializeGlobals(byte[] globalsData, IGameState gameState)
        {
            // TODO: Implement global variable deserialization
            // Parse GFF GLOBALS struct
            // Restore variables by category
            // Validate data types and ranges
            // Update game state accordingly
        }

        /// <summary>
        /// Serializes party information.
        /// </summary>
        /// <remarks>
        /// Odyssey party serialization includes companions and their states.
        /// Saves companion approval, equipment, position, quest involvement.
        /// Includes party formation and leadership information.
        ///
        /// Based on swkotor2.exe: FUN_0057bd70 @ 0x0057bd70
        /// Located via string reference: "PARTYTABLE" @ 0x007c1910
        /// Original implementation creates GFF with "PT  " signature and "V2.0" version.
        ///
        /// Party data includes:
        /// - Companion entities and their states
        /// - Approval ratings and relationship flags (K2 influence system)
        /// - Equipment and inventory
        /// - Position in party formation
        /// - Active quest involvement
        /// - Game state (gold, XP, components, chemicals, swoop times)
        /// - UI state (Pazaak cards, tutorial windows, messages, galaxy map)
        /// - AI and follow states
        /// </remarks>
        public override byte[] SerializeParty(IPartyState partyState)
        {
            // Convert IPartyState to PartyState if possible
            // The runtime typically passes PartyState instances which contain all the rich data
            PartyState state = partyState as PartyState;
            if (state == null)
            {
                // Fallback: Create minimal PartyState from IPartyState interface
                state = new PartyState();
                if (partyState != null)
                {
                    // Extract basic party information from interface
                    if (partyState.Leader != null)
                    {
                        state.LeaderResRef = partyState.Leader.Tag ?? "";
                        if (state.PlayerCharacter == null)
                        {
                            state.PlayerCharacter = new CreatureState();
                        }
                        state.PlayerCharacter.Tag = partyState.Leader.Tag ?? "";
                    }

                    // Extract party members
                    if (partyState.Members != null)
                    {
                        foreach (IEntity member in partyState.Members)
                        {
                            if (member != null && !string.IsNullOrEmpty(member.Tag))
                            {
                                if (!state.AvailableMembers.ContainsKey(member.Tag))
                                {
                                    state.AvailableMembers[member.Tag] = new PartyMemberState
                                    {
                                        TemplateResRef = member.Tag,
                                        IsAvailable = true,
                                        IsSelectable = true
                                    };
                                }
                                if (!state.SelectedParty.Contains(member.Tag))
                                {
                                    state.SelectedParty.Add(member.Tag);
                                }
                            }
                        }
                    }
                }
            }

            // Use Andastra.Parsing GFF writer
            // Original creates GFF with "PT  " signature and "V2.0" version
            // Based on swkotor2.exe: FUN_0057bd70 @ 0x0057bd70 creates GFF with "PT  " signature
            // Located via string reference: "PARTYTABLE" @ 0x007c1910
            // Note: Andastra.Parsing GFFBinaryWriter always writes "V3.2" version, but signature is correct
            var gff = new GFF(GFFContent.PT);
            var root = gff.Root;

            // PT_PCNAME - Player character name
            string pcName = "";
            if (state.PlayerCharacter != null)
            {
                pcName = state.PlayerCharacter.Tag ?? "";
            }
            root.SetString("PT_PCNAME", pcName);

            // PT_GOLD - Gold/credits
            root.SetInt32("PT_GOLD", state.Gold);

            // PT_ITEM_COMPONENT - Item component count
            root.SetInt32("PT_ITEM_COMPONENT", state.ItemComponent);

            // PT_ITEM_CHEMICAL - Item chemical count
            root.SetInt32("PT_ITEM_CHEMICAL", state.ItemChemical);

            // PT_SWOOP1-3 - Swoop race times
            root.SetInt32("PT_SWOOP1", state.Swoop1);
            root.SetInt32("PT_SWOOP2", state.Swoop2);
            root.SetInt32("PT_SWOOP3", state.Swoop3);

            // PT_XP_POOL - Experience point pool (float)
            root.SetSingle("PT_XP_POOL", state.ExperiencePoints);

            // PT_PLAYEDSECONDS - Total seconds played
            root.SetInt32("PT_PLAYEDSECONDS", (int)state.PlayTime.TotalSeconds);

            // PT_CONTROLLED_NPC - Currently controlled NPC ID (float, -1 if none)
            root.SetSingle("PT_CONTROLLED_NPC", state.ControlledNPC >= 0 ? (float)state.ControlledNPC : -1.0f);

            // PT_SOLOMODE - Solo mode flag (byte)
            root.SetUInt8("PT_SOLOMODE", state.SoloMode ? (byte)1 : (byte)0);

            // PT_CHEAT_USED - Cheat used flag (byte)
            root.SetUInt8("PT_CHEAT_USED", state.CheatUsed ? (byte)1 : (byte)0);

            // PT_NUM_MEMBERS - Number of party members (byte)
            int numMembers = state.SelectedParty != null ? state.SelectedParty.Count : 0;
            root.SetUInt8("PT_NUM_MEMBERS", (byte)numMembers);

            // PT_MEMBERS - List of party members
            var membersList = new GFFList();
            if (state.SelectedParty != null)
            {
                foreach (string memberResRef in state.SelectedParty)
                {
                    GFFStruct entry = membersList.Add();
                    // PT_MEMBER_ID - Member ID (float)
                    entry.SetSingle("PT_MEMBER_ID", GetMemberId(memberResRef));
                    // PT_IS_LEADER - Whether this member is the leader (byte)
                    bool isLeader = state.LeaderResRef == memberResRef;
                    entry.SetUInt8("PT_IS_LEADER", isLeader ? (byte)1 : (byte)0);
                }
            }
            root.SetList("PT_MEMBERS", membersList);

            // PT_NUM_PUPPETS - Number of puppets (byte)
            int numPuppets = state.Puppets != null ? state.Puppets.Count : 0;
            root.SetUInt8("PT_NUM_PUPPETS", (byte)numPuppets);

            // PT_PUPPETS - List of puppets
            var puppetsList = new GFFList();
            if (state.Puppets != null)
            {
                foreach (uint puppetId in state.Puppets)
                {
                    GFFStruct entry = puppetsList.Add();
                    entry.SetSingle("PT_PUPPET_ID", (float)puppetId);
                }
            }
            root.SetList("PT_PUPPETS", puppetsList);

            // PT_AVAIL_PUPS - Available puppets list (3 entries)
            var availPupsList = new GFFList();
            for (int i = 0; i < 3; i++)
            {
                GFFStruct entry = availPupsList.Add();
                bool available = state.AvailablePuppets != null && i < state.AvailablePuppets.Count && state.AvailablePuppets[i];
                entry.SetUInt8("PT_PUP_AVAIL", available ? (byte)1 : (byte)0);
                bool selectable = state.SelectablePuppets != null && i < state.SelectablePuppets.Count && state.SelectablePuppets[i];
                entry.SetUInt8("PT_PUP_SELECT", selectable ? (byte)1 : (byte)0);
            }
            root.SetList("PT_AVAIL_PUPS", availPupsList);

            // PT_AVAIL_NPCS - Available NPCs list (12 entries)
            var availNpcsList = new GFFList();
            List<PartyMemberState> memberList = state.AvailableMembers != null ? new List<PartyMemberState>(state.AvailableMembers.Values) : new List<PartyMemberState>();
            for (int i = 0; i < 12; i++)
            {
                GFFStruct entry = availNpcsList.Add();
                bool available = i < memberList.Count && memberList[i] != null;
                entry.SetUInt8("PT_NPC_AVAIL", available ? (byte)1 : (byte)0);
                bool selectable = available && memberList[i].IsSelectable;
                entry.SetUInt8("PT_NPC_SELECT", selectable ? (byte)1 : (byte)0);
            }
            root.SetList("PT_AVAIL_NPCS", availNpcsList);

            // PT_INFLUENCE - Influence values list (12 entries, K2 only)
            var influenceList = new GFFList();
            for (int i = 0; i < 12; i++)
            {
                GFFStruct entry = influenceList.Add();
                float influence = 0.0f;
                if (state.Influence != null && i < state.Influence.Count)
                {
                    influence = (float)state.Influence[i];
                }
                entry.SetSingle("PT_NPC_INFLUENCE", influence);
            }
            root.SetList("PT_INFLUENCE", influenceList);

            // PT_AISTATE - AI state (float)
            root.SetSingle("PT_AISTATE", (float)state.AIState);

            // PT_FOLLOWSTATE - Follow state (float)
            root.SetSingle("PT_FOLLOWSTATE", (float)state.FollowState);

            // GlxyMap - Galaxy map data
            var glxyMapStruct = new GFFStruct();
            glxyMapStruct.SetInt32("GlxyMapNumPnts", 16); // Always 16 points
            glxyMapStruct.SetInt32("GlxyMapPlntMsk", state.GalaxyMapPlanetMask);
            glxyMapStruct.SetSingle("GlxyMapSelPnt", (float)state.GalaxyMapSelectedPoint);
            root.SetStruct("GlxyMap", glxyMapStruct);

            // PT_PAZAAKCARDS - Pazaak cards list (23 entries)
            var pazaakCardsList = new GFFList();
            for (int i = 0; i < 23; i++)
            {
                GFFStruct entry = pazaakCardsList.Add();
                int count = 0;
                if (state.PazaakCards != null && i < state.PazaakCards.Count)
                {
                    count = state.PazaakCards[i];
                }
                entry.SetSingle("PT_PAZAAKCOUNT", (float)count);
            }
            root.SetList("PT_PAZAAKCARDS", pazaakCardsList);

            // PT_PAZSIDELIST - Pazaak side list (10 entries)
            var pazaakSideList = new GFFList();
            for (int i = 0; i < 10; i++)
            {
                GFFStruct entry = pazaakSideList.Add();
                int card = 0;
                if (state.PazaakSideList != null && i < state.PazaakSideList.Count)
                {
                    card = state.PazaakSideList[i];
                }
                entry.SetSingle("PT_PAZSIDECARD", (float)card);
            }
            root.SetList("PT_PAZSIDELIST", pazaakSideList);

            // PT_TUT_WND_SHOWN - Tutorial windows shown (array of 33 bytes)
            if (state.TutorialWindowsShown != null)
            {
                byte[] tutArray = new byte[33];
                for (int i = 0; i < 33 && i < state.TutorialWindowsShown.Count; i++)
                {
                    tutArray[i] = state.TutorialWindowsShown[i] ? (byte)1 : (byte)0;
                }
                root.SetBinary("PT_TUT_WND_SHOWN", tutArray);
            }

            // PT_LAST_GUI_PNL - Last GUI panel (float)
            root.SetSingle("PT_LAST_GUI_PNL", (float)state.LastGUIPanel);

            // PT_FB_MSG_LIST - Feedback message list
            var fbMsgList = new GFFList();
            if (state.FeedbackMessages != null)
            {
                foreach (var msg in state.FeedbackMessages)
                {
                    GFFStruct entry = fbMsgList.Add();
                    entry.SetString("PT_FB_MSG_MSG", msg.Message ?? "");
                    entry.SetInt32("PT_FB_MSG_TYPE", msg.Type);
                    entry.SetUInt8("PT_FB_MSG_COLOR", msg.Color);
                }
            }
            root.SetList("PT_FB_MSG_LIST", fbMsgList);

            // PT_DLG_MSG_LIST - Dialogue message list
            var dlgMsgList = new GFFList();
            if (state.DialogueMessages != null)
            {
                foreach (var msg in state.DialogueMessages)
                {
                    GFFStruct entry = dlgMsgList.Add();
                    entry.SetString("PT_DLG_MSG_SPKR", msg.Speaker ?? "");
                    entry.SetString("PT_DLG_MSG_MSG", msg.Message ?? "");
                }
            }
            root.SetList("PT_DLG_MSG_LIST", dlgMsgList);

            // PT_COM_MSG_LIST - Combat message list
            var comMsgList = new GFFList();
            if (state.CombatMessages != null)
            {
                foreach (var msg in state.CombatMessages)
                {
                    GFFStruct entry = comMsgList.Add();
                    entry.SetString("PT_COM_MSG_MSG", msg.Message ?? "");
                    entry.SetInt32("PT_COM_MSG_TYPE", msg.Type);
                    entry.SetUInt8("PT_COM_MSG_COOR", msg.Color);
                }
            }
            root.SetList("PT_COM_MSG_LIST", comMsgList);

            // PT_COST_MULT_LIST - Cost multiplier list
            var costMultList = new GFFList();
            if (state.CostMultipliers != null)
            {
                foreach (var mult in state.CostMultipliers)
                {
                    GFFStruct entry = costMultList.Add();
                    entry.SetSingle("PT_COST_MULT_VALUE", mult);
                }
            }
            root.SetList("PT_COST_MULT_LIST", costMultList);

            // PT_DISABLEMAP - Disable map flag (float)
            root.SetSingle("PT_DISABLEMAP", state.DisableMap ? 1.0f : 0.0f);

            // PT_DISABLEREGEN - Disable regen flag (float)
            root.SetSingle("PT_DISABLEREGEN", state.DisableRegen ? 1.0f : 0.0f);

            // Serialize GFF to bytes
            return gff.ToBytes();
        }

        /// <summary>
        /// Helper to get member ID from ResRef.
        /// </summary>
        /// <remarks>
        /// Member IDs: -1 = Player, 0-8 = NPC slots (K1), 0-11 = NPC slots (K2)
        /// Based on nwscript.nss constants: NPC_PLAYER = -1, NPC_BASTILA = 0, etc.
        /// </remarks>
        private float GetMemberId(string resRef)
        {
            if (string.IsNullOrEmpty(resRef))
            {
                return -1.0f; // Default to player
            }

            // Player character is typically identified by specific ResRefs or -1
            // Common player ResRefs: "player", "pc", etc.
            string resRefLower = resRef.ToLowerInvariant();
            if (resRefLower == "player" || resRefLower == "pc" || resRefLower.StartsWith("pc_"))
            {
                return -1.0f; // NPC_PLAYER
            }

            // For NPCs, we would need to map ResRefs to NPC slot indices
            // This is typically done via partytable.2da or script constants
            // For now, we'll use a simple hash-based approach for non-player members
            // In a full implementation, this would reference partytable.2da or script definitions
            // TODO: SIMPLIFIED - Full implementation would map ResRefs to NPC indices via partytable.2da
            // For now, use a simple mapping: return index based on first character hash
            // This is a placeholder - real implementation should use proper NPC index mapping
            int hash = Math.Abs(resRefLower.GetHashCode());
            return (float)(hash % 12); // Map to 0-11 range for K2 NPC slots
        }

        /// <summary>
        /// Deserializes party information.
        /// </summary>
        /// <remarks>
        /// Recreates party from save data.
        /// Restores companion states, relationships, equipment.
        /// Reestablishes party formation and leadership.
        /// </remarks>
        public override void DeserializeParty(byte[] partyData, IPartyState partyState)
        {
            // TODO: Implement party deserialization
            // Parse PARTY struct
            // Recreate companion entities
            // Restore relationships and states
            // Reestablish party structure
        }

        /// <summary>
        /// Serializes area state.
        /// </summary>
        /// <remarks>
        /// Odyssey area serialization saves dynamic changes.
        /// Includes placed objects, modified containers, area effects.
        /// Saves transition states and dynamic object modifications.
        ///
        /// Area state includes:
        /// - Placed creatures and objects
        /// - Modified container contents
        /// - Active area effects
        /// - Door and transition states
        /// - Dynamic lighting changes
        /// </remarks>
        public override byte[] SerializeArea(IArea area)
        {
            // TODO: Implement area serialization
            // Create AREA struct for the specific area
            // Serialize dynamic objects
            // Save modified container states
            // Include area effect states

            throw new NotImplementedException("Odyssey area serialization not yet implemented");
        }

        /// <summary>
        /// Deserializes area state.
        /// </summary>
        /// <remarks>
        /// Restores dynamic area changes from save data.
        /// Recreates placed objects and restores modified states.
        /// Applies area effects and transition states.
        /// </remarks>
        public override void DeserializeArea(byte[] areaData, IArea area)
        {
            // TODO: Implement area deserialization
            // Parse AREA struct
            // Recreate dynamic objects
            // Restore modified states
            // Apply area effects
        }

        /// <summary>
        /// Serializes entity collection.
        /// </summary>
        /// <remarks>
        /// Based on FUN_004e28c0 @ 0x004e28c0 in swkotor2.exe.
        /// Saves creature stats, inventory, position, scripts.
        /// Uses GFF format with entity ObjectId as key.
        ///
        /// Entity data includes:
        /// - ObjectId, Tag, ObjectType
        /// - Position and orientation
        /// - Stats (HP, FP, attributes)
        /// - Equipment and inventory
        /// - Active scripts and effects
        /// - AI state and waypoints
        /// </remarks>
        public override byte[] SerializeEntities(IEnumerable<IEntity> entities)
        {
            // TODO: Implement entity serialization
            // Create GFF with Creature List struct
            // Serialize each entity with ObjectId
            // Include all components and state
            // Handle entity references

            throw new NotImplementedException("Odyssey entity serialization not yet implemented");
        }

        /// <summary>
        /// Deserializes entity collection.
        /// </summary>
        /// <remarks>
        /// Based on FUN_005fb0f0 @ 0x005fb0f0 in swkotor2.exe.
        /// Recreates entities from save data.
        /// Restores all components and state information.
        /// Handles entity interdependencies.
        /// </remarks>
        public override IEnumerable<IEntity> DeserializeEntities(byte[] entitiesData)
        {
            // TODO: Implement entity deserialization
            // Parse Creature List struct
            // Create entities with correct types
            // Restore all components and state
            // Resolve entity references

            yield break;
        }

        /// <summary>
        /// Creates a save game directory structure.
        /// </summary>
        /// <remarks>
        /// Creates numbered save directories following KotOR conventions.
        /// Creates NFO, SAV, and supporting files.
        /// Includes screenshot and metadata files.
        ///
        /// Directory structure:
        /// - Save game root directory
        /// - Numbered save subdirectories (save.0, save.1, etc.)
        /// - savenfo.res: Metadata file
        /// - SAVEgame.sav: Main save data
        /// - Screen.tga: Screenshot
        /// </remarks>
        public override void CreateSaveDirectory(string saveName, Andastra.Runtime.Games.Common.SaveGameData saveData)
        {
            // TODO: Implement save directory creation
            // Create numbered save directory
            // Write NFO file
            // Write main SAV file
            // Save screenshot if available
            // Create supporting files
        }

        /// <summary>
        /// Validates save game compatibility.
        /// </summary>
        /// <remarks>
        /// Checks KotOR save compatibility.
        /// Validates NFO signature and version.
        /// Checks for required save files.
        /// Returns compatibility status with details.
        /// </remarks>
        public override SaveCompatibility CheckCompatibility(string savePath)
        {
            // TODO: Implement compatibility checking
            // Check NFO file existence and validity
            // Validate save version compatibility
            // Check for required data files
            // Return detailed compatibility info

            return SaveCompatibility.Compatible;
        }
    }
}
