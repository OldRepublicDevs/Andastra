using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using Andastra.Runtime.Core.Save;
using Andastra.Runtime.Engines.Eclipse.Save;

namespace Andastra.Runtime.Engines.Eclipse.MassEffect.Save
{
    /// <summary>
    /// Save serializer for Mass Effect 1 (.pcsave save files).
    /// </summary>
    /// <remarks>
    /// Mass Effect 1 Save Format:
    /// - Based on MassEffect.exe: intABioWorldInfoexecBioSaveGame @ 0x11800ca0
    /// - Located via string references: "BioSaveGame" @ 0x11800ca0, extensive save system (20+ functions)
    /// - Save file format: Binary format with signature "MES1" (Mass Effect Save 1)
    /// - Version: 1 (int32)
    /// - Structure: Signature (4 bytes) -> Version (4 bytes) -> Metadata -> Game State
    /// - Inheritance: Base class EclipseSaveSerializer (Runtime.Engines.Eclipse.Save) - abstract save serializer, MassEffect override - .pcsave format
    /// - Original implementation: UnrealScript message-based save system, binary serialization
    /// - Note: Mass Effect uses .pcsave file extension
    /// </remarks>
    public class MassEffectSaveSerializer : EclipseSaveSerializer
    {
        private const string SaveSignature = "MES1";
        private const int SaveVersion = 1;

        /// <summary>
        /// Serializes save metadata to NFO format (Mass Effect 1-specific).
        /// </summary>
        public override byte[] SerializeSaveNfo(SaveGameData saveData)
        {
            if (saveData == null)
            {
                throw new ArgumentNullException(nameof(saveData));
            }

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                // Write signature
                writer.Write(System.Text.Encoding.UTF8.GetBytes(SaveSignature));

                // Write version
                writer.Write(SaveVersion);

                // Write common metadata
                WriteCommonMetadata(writer, saveData);

                // TODO: Add Mass Effect 1-specific metadata fields
                // Based on MassEffect.exe: intABioWorldInfoexecBioSaveGame structure
                // Fields may include: Character name, class, level, squad members, etc.

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Deserializes save metadata from NFO format (Mass Effect 1-specific).
        /// </summary>
        public override SaveGameData DeserializeSaveNfo(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Save data cannot be null or empty", nameof(data));
            }

            var saveData = new SaveGameData();

            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                // Validate signature
                ValidateSignature(reader, SaveSignature);

                // Validate version
                ValidateVersion(reader, SaveVersion, "Mass Effect 1");

                // Read common metadata
                ReadCommonMetadata(reader, saveData);

                // TODO: Read Mass Effect 1-specific metadata fields
            }

            return saveData;
        }

        /// <summary>
        /// Serializes full save archive (Mass Effect 1-specific).
        /// </summary>
        public override byte[] SerializeSaveArchive(SaveGameData saveData)
        {
            if (saveData == null)
            {
                throw new ArgumentNullException(nameof(saveData));
            }

            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream))
            {
                // Write signature
                writer.Write(System.Text.Encoding.UTF8.GetBytes(SaveSignature));

                // Write version
                writer.Write(SaveVersion);

                // Write common metadata
                WriteCommonMetadata(writer, saveData);

                // Serialize full game state
                // Based on MassEffect.exe: intABioWorldInfoexecBioSaveGame serialization
                // Includes: Squad state, inventory, missions, world state, etc.
                SerializeFullGameState(writer, saveData);

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Deserializes full save archive (Mass Effect 1-specific).
        /// </summary>
        public override void DeserializeSaveArchive(byte[] data, SaveGameData saveData)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Save data cannot be null or empty", nameof(data));
            }

            if (saveData == null)
            {
                throw new ArgumentNullException(nameof(saveData));
            }

            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream))
            {
                // Validate signature
                ValidateSignature(reader, SaveSignature);

                // Validate version
                ValidateVersion(reader, SaveVersion, "Mass Effect 1");

                // Read common metadata
                ReadCommonMetadata(reader, saveData);

                // Deserialize full game state
                DeserializeFullGameState(reader, saveData);
            }
        }

        #region Full Game State Serialization

        /// <summary>
        /// Serializes the complete game state including all areas, entities, globals, party, and journal.
        /// Based on MassEffect.exe: intABioWorldInfoexecBioSaveGame serialization @ 0x11800ca0
        /// </summary>
        private void SerializeFullGameState(BinaryWriter writer, SaveGameData saveData)
        {
            // Entry position and facing
            writer.Write(saveData.EntryPosition.X);
            writer.Write(saveData.EntryPosition.Y);
            writer.Write(saveData.EntryPosition.Z);
            writer.Write(saveData.EntryFacing);

            // Current area name
            WriteString(writer, saveData.CurrentAreaName ?? "");

            // Game time
            if (saveData.GameTime != null)
            {
                writer.Write(saveData.GameTime.Year);
                writer.Write(saveData.GameTime.Month);
                writer.Write(saveData.GameTime.Day);
                writer.Write(saveData.GameTime.Hour);
                writer.Write(saveData.GameTime.Minute);
            }
            else
            {
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
                writer.Write(0);
            }

            // Game version
            WriteString(writer, saveData.GameVersion ?? "");

            // Save type
            writer.Write((int)saveData.SaveType);

            // Save number
            writer.Write(saveData.SaveNumber);

            // Cheat used flag
            writer.Write(saveData.CheatUsed ? 1 : 0);

            // Gameplay hint flag
            writer.Write(saveData.GameplayHint ? 1 : 0);

            // Story hints (10 flags)
            int storyHintsCount = saveData.StoryHints != null ? saveData.StoryHints.Count : 0;
            writer.Write(Math.Min(storyHintsCount, 10));
            for (int i = 0; i < 10; i++)
            {
                bool hintValue = saveData.StoryHints != null && i < saveData.StoryHints.Count && saveData.StoryHints[i];
                writer.Write(hintValue ? 1 : 0);
            }

            // Live content flags
            int liveContentCount = saveData.LiveContent != null ? saveData.LiveContent.Count : 0;
            writer.Write(liveContentCount);
            if (saveData.LiveContent != null)
            {
                foreach (bool flag in saveData.LiveContent)
                {
                    writer.Write(flag ? 1 : 0);
                }
            }

            // Player name
            WriteString(writer, saveData.PlayerName ?? "");

            // Screenshot data
            if (saveData.Screenshot != null && saveData.Screenshot.Length > 0)
            {
                writer.Write(saveData.Screenshot.Length);
                writer.Write(saveData.Screenshot);
            }
            else
            {
                writer.Write(0);
            }

            // Serialize global variables
            SerializeGlobalVariables(writer, saveData.GlobalVariables);

            // Serialize party state
            SerializePartyState(writer, saveData.PartyState);

            // Serialize area states
            SerializeAreaStates(writer, saveData.AreaStates);

            // Serialize journal entries
            SerializeJournalEntries(writer, saveData.JournalEntries);
        }

        /// <summary>
        /// Deserializes the complete game state from save data.
        /// Based on MassEffect.exe: intABioWorldInfoexecBioSaveGame deserialization @ 0x11800ca0
        /// </summary>
        private void DeserializeFullGameState(BinaryReader reader, SaveGameData saveData)
        {
            // Entry position and facing
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            float z = reader.ReadSingle();
            saveData.EntryPosition = new Vector3(x, y, z);
            saveData.EntryFacing = reader.ReadSingle();

            // Current area name
            saveData.CurrentAreaName = ReadString(reader);

            // Game time
            int year = reader.ReadInt32();
            int month = reader.ReadInt32();
            int day = reader.ReadInt32();
            int hour = reader.ReadInt32();
            int minute = reader.ReadInt32();
            if (saveData.GameTime == null)
            {
                saveData.GameTime = new GameTime();
            }
            saveData.GameTime.Year = year;
            saveData.GameTime.Month = month;
            saveData.GameTime.Day = day;
            saveData.GameTime.Hour = hour;
            saveData.GameTime.Minute = minute;

            // Game version
            saveData.GameVersion = ReadString(reader);

            // Save type
            saveData.SaveType = (SaveType)reader.ReadInt32();

            // Save number
            saveData.SaveNumber = reader.ReadInt32();

            // Cheat used flag
            saveData.CheatUsed = reader.ReadInt32() != 0;

            // Gameplay hint flag
            saveData.GameplayHint = reader.ReadInt32() != 0;

            // Story hints (10 flags)
            int storyHintsCount = reader.ReadInt32();
            if (saveData.StoryHints == null)
            {
                saveData.StoryHints = new List<bool>();
            }
            saveData.StoryHints.Clear();
            for (int i = 0; i < 10; i++)
            {
                bool hintValue = reader.ReadInt32() != 0;
                saveData.StoryHints.Add(hintValue);
            }

            // Live content flags
            int liveContentCount = reader.ReadInt32();
            if (saveData.LiveContent == null)
            {
                saveData.LiveContent = new List<bool>();
            }
            saveData.LiveContent.Clear();
            for (int i = 0; i < liveContentCount; i++)
            {
                bool flag = reader.ReadInt32() != 0;
                saveData.LiveContent.Add(flag);
            }

            // Player name
            saveData.PlayerName = ReadString(reader);

            // Screenshot data
            int screenshotLength = reader.ReadInt32();
            if (screenshotLength > 0)
            {
                saveData.Screenshot = reader.ReadBytes(screenshotLength);
            }
            else
            {
                saveData.Screenshot = null;
            }

            // Deserialize global variables
            saveData.GlobalVariables = DeserializeGlobalVariables(reader);

            // Deserialize party state
            saveData.PartyState = DeserializePartyState(reader);

            // Deserialize area states
            saveData.AreaStates = DeserializeAreaStates(reader);

            // Deserialize journal entries
            saveData.JournalEntries = DeserializeJournalEntries(reader);
        }

        /// <summary>
        /// Serializes global variable state (Mass Effect-specific: includes Locations, uses int32 for booleans).
        /// Based on MassEffect.exe: Global variable serialization functions
        /// </summary>
        /// <remarks>
        /// Mass Effect uses int32 for boolean serialization (unlike Dragon Age which uses bool).
        /// Mass Effect also includes Location globals which are not in the base class implementation.
        /// </remarks>
        private new void SerializeGlobalVariables(BinaryWriter writer, GlobalVariableState globals)
        {
            if (globals == null)
            {
                // Write empty state
                writer.Write(0); // Boolean count
                writer.Write(0); // Number count
                writer.Write(0); // String count
                writer.Write(0); // Location count
                return;
            }

            // Serialize boolean globals
            writer.Write(globals.Booleans != null ? globals.Booleans.Count : 0);
            if (globals.Booleans != null)
            {
                foreach (var kvp in globals.Booleans)
                {
                    WriteString(writer, kvp.Key);
                    writer.Write(kvp.Value ? 1 : 0); // Mass Effect uses int32 for booleans
                }
            }

            // Serialize numeric globals
            writer.Write(globals.Numbers != null ? globals.Numbers.Count : 0);
            if (globals.Numbers != null)
            {
                foreach (var kvp in globals.Numbers)
                {
                    WriteString(writer, kvp.Key);
                    writer.Write(kvp.Value);
                }
            }

            // Serialize string globals
            writer.Write(globals.Strings != null ? globals.Strings.Count : 0);
            if (globals.Strings != null)
            {
                foreach (var kvp in globals.Strings)
                {
                    WriteString(writer, kvp.Key);
                    WriteString(writer, kvp.Value);
                }
            }

            // Serialize location globals
            writer.Write(globals.Locations != null ? globals.Locations.Count : 0);
            if (globals.Locations != null)
            {
                foreach (var kvp in globals.Locations)
                {
                    WriteString(writer, kvp.Key);
                    SerializeSavedLocation(writer, kvp.Value);
                }
            }
        }

        /// <summary>
        /// Deserializes global variable state (Mass Effect-specific: includes Locations, uses int32 for booleans).
        /// </summary>
        /// <remarks>
        /// Mass Effect uses int32 for boolean deserialization (unlike Dragon Age which uses bool).
        /// Mass Effect also includes Location globals which are not in the base class implementation.
        /// </remarks>
        private new GlobalVariableState DeserializeGlobalVariables(BinaryReader reader)
        {
            var globals = new GlobalVariableState();

            // Deserialize boolean globals
            int boolCount = reader.ReadInt32();
            for (int i = 0; i < boolCount; i++)
            {
                string name = ReadString(reader);
                bool value = reader.ReadInt32() != 0; // Mass Effect uses int32 for booleans
                globals.Booleans[name] = value;
            }

            // Deserialize numeric globals
            int numberCount = reader.ReadInt32();
            for (int i = 0; i < numberCount; i++)
            {
                string name = ReadString(reader);
                int value = reader.ReadInt32();
                globals.Numbers[name] = value;
            }

            // Deserialize string globals
            int stringCount = reader.ReadInt32();
            for (int i = 0; i < stringCount; i++)
            {
                string name = ReadString(reader);
                string value = ReadString(reader);
                globals.Strings[name] = value;
            }

            // Deserialize location globals
            int locationCount = reader.ReadInt32();
            for (int i = 0; i < locationCount; i++)
            {
                string name = ReadString(reader);
                SavedLocation location = DeserializeSavedLocation(reader);
                globals.Locations[name] = location;
            }

            return globals;
        }

        /// <summary>
        /// Serializes party/squad state.
        /// Based on MassEffect.exe: Squad state serialization
        /// </summary>
        private void SerializePartyState(BinaryWriter writer, PartyState partyState)
        {
            if (partyState == null)
            {
                // Write empty state
                writer.Write(0); // Has player character
                writer.Write(0); // Available members count
                writer.Write(0); // Selected party count
                writer.Write(0); // Gold
                writer.Write(0); // Experience points
                return;
            }

            // Serialize player character
            writer.Write(partyState.PlayerCharacter != null ? 1 : 0);
            if (partyState.PlayerCharacter != null)
            {
                SerializeCreatureState(writer, partyState.PlayerCharacter);
            }

            // Serialize available party members
            int availableCount = partyState.AvailableMembers != null ? partyState.AvailableMembers.Count : 0;
            writer.Write(availableCount);
            if (partyState.AvailableMembers != null)
            {
                foreach (var kvp in partyState.AvailableMembers)
                {
                    WriteString(writer, kvp.Key); // Template ResRef
                    writer.Write(kvp.Value.IsAvailable ? 1 : 0);
                    writer.Write(kvp.Value.IsSelectable ? 1 : 0);
                    if (kvp.Value.State != null)
                    {
                        writer.Write(1);
                        SerializeCreatureState(writer, kvp.Value.State);
                    }
                    else
                    {
                        writer.Write(0);
                    }
                }
            }

            // Serialize selected party
            int selectedCount = partyState.SelectedParty != null ? partyState.SelectedParty.Count : 0;
            writer.Write(selectedCount);
            if (partyState.SelectedParty != null)
            {
                foreach (string memberResRef in partyState.SelectedParty)
                {
                    WriteString(writer, memberResRef);
                }
            }

            // Serialize party resources
            writer.Write(partyState.Gold);
            writer.Write(partyState.ExperiencePoints);

            // Serialize influence values (if any)
            int influenceCount = partyState.Influence != null ? partyState.Influence.Count : 0;
            writer.Write(influenceCount);
            if (partyState.Influence != null)
            {
                foreach (int influence in partyState.Influence)
                {
                    writer.Write(influence);
                }
            }

            // Serialize other party state
            writer.Write(partyState.ItemComponent);
            writer.Write(partyState.ItemChemical);
            writer.Write(partyState.Swoop1);
            writer.Write(partyState.Swoop2);
            writer.Write(partyState.Swoop3);
            writer.Write((int)partyState.PlayTime.TotalSeconds);
            writer.Write(partyState.ControlledNPC);
            writer.Write(partyState.SoloMode ? 1 : 0);
            writer.Write(partyState.CheatUsed ? 1 : 0);
            WriteString(writer, partyState.LeaderResRef ?? "");

            // Serialize puppets
            int puppetCount = partyState.Puppets != null ? partyState.Puppets.Count : 0;
            writer.Write(puppetCount);
            if (partyState.Puppets != null)
            {
                foreach (uint puppetId in partyState.Puppets)
                {
                    writer.Write(puppetId);
                }
            }

            // Serialize available puppets
            int availablePuppetCount = partyState.AvailablePuppets != null ? partyState.AvailablePuppets.Count : 0;
            writer.Write(availablePuppetCount);
            if (partyState.AvailablePuppets != null)
            {
                foreach (bool available in partyState.AvailablePuppets)
                {
                    writer.Write(available ? 1 : 0);
                }
            }

            // Serialize selectable puppets
            int selectablePuppetCount = partyState.SelectablePuppets != null ? partyState.SelectablePuppets.Count : 0;
            writer.Write(selectablePuppetCount);
            if (partyState.SelectablePuppets != null)
            {
                foreach (bool selectable in partyState.SelectablePuppets)
                {
                    writer.Write(selectable ? 1 : 0);
                }
            }

            writer.Write(partyState.AIState);
            writer.Write(partyState.FollowState);
            writer.Write(partyState.GalaxyMapPlanetMask);
            writer.Write(partyState.GalaxyMapSelectedPoint);

            // Serialize Pazaak cards
            int pazaakCardCount = partyState.PazaakCards != null ? partyState.PazaakCards.Count : 0;
            writer.Write(pazaakCardCount);
            if (partyState.PazaakCards != null)
            {
                foreach (int card in partyState.PazaakCards)
                {
                    writer.Write(card);
                }
            }

            // Serialize Pazaak side list
            int pazaakSideCount = partyState.PazaakSideList != null ? partyState.PazaakSideList.Count : 0;
            writer.Write(pazaakSideCount);
            if (partyState.PazaakSideList != null)
            {
                foreach (int side in partyState.PazaakSideList)
                {
                    writer.Write(side);
                }
            }

            // Serialize tutorial windows shown
            int tutorialCount = partyState.TutorialWindowsShown != null ? partyState.TutorialWindowsShown.Count : 0;
            writer.Write(tutorialCount);
            if (partyState.TutorialWindowsShown != null)
            {
                foreach (bool shown in partyState.TutorialWindowsShown)
                {
                    writer.Write(shown ? 1 : 0);
                }
            }

            writer.Write(partyState.LastGUIPanel);
            writer.Write(partyState.DisableMap ? 1 : 0);
            writer.Write(partyState.DisableRegen ? 1 : 0);

            // Serialize feedback messages
            int feedbackCount = partyState.FeedbackMessages != null ? partyState.FeedbackMessages.Count : 0;
            writer.Write(feedbackCount);
            if (partyState.FeedbackMessages != null)
            {
                foreach (var msg in partyState.FeedbackMessages)
                {
                    WriteString(writer, msg.Message ?? "");
                    writer.Write(msg.Type);
                    writer.Write(msg.Color);
                }
            }

            // Serialize dialogue messages
            int dialogueCount = partyState.DialogueMessages != null ? partyState.DialogueMessages.Count : 0;
            writer.Write(dialogueCount);
            if (partyState.DialogueMessages != null)
            {
                foreach (var msg in partyState.DialogueMessages)
                {
                    WriteString(writer, msg.Speaker ?? "");
                    WriteString(writer, msg.Message ?? "");
                }
            }

            // Serialize combat messages
            int combatCount = partyState.CombatMessages != null ? partyState.CombatMessages.Count : 0;
            writer.Write(combatCount);
            if (partyState.CombatMessages != null)
            {
                foreach (var msg in partyState.CombatMessages)
                {
                    WriteString(writer, msg.Message ?? "");
                    writer.Write(msg.Type);
                    writer.Write(msg.Color);
                }
            }

            // Serialize cost multipliers
            int costMultiplierCount = partyState.CostMultipliers != null ? partyState.CostMultipliers.Count : 0;
            writer.Write(costMultiplierCount);
            if (partyState.CostMultipliers != null)
            {
                foreach (float multiplier in partyState.CostMultipliers)
                {
                    writer.Write(multiplier);
                }
            }
        }

        /// <summary>
        /// Deserializes party/squad state.
        /// </summary>
        private PartyState DeserializePartyState(BinaryReader reader)
        {
            var partyState = new PartyState();

            // Deserialize player character
            bool hasPlayer = reader.ReadInt32() != 0;
            if (hasPlayer)
            {
                partyState.PlayerCharacter = DeserializeCreatureState(reader);
            }

            // Deserialize available party members
            int availableCount = reader.ReadInt32();
            for (int i = 0; i < availableCount; i++)
            {
                string templateResRef = ReadString(reader);
                bool isAvailable = reader.ReadInt32() != 0;
                bool isSelectable = reader.ReadInt32() != 0;
                bool hasState = reader.ReadInt32() != 0;
                CreatureState state = null;
                if (hasState)
                {
                    state = DeserializeCreatureState(reader);
                }

                var memberState = new PartyMemberState
                {
                    TemplateResRef = templateResRef,
                    IsAvailable = isAvailable,
                    IsSelectable = isSelectable,
                    State = state
                };
                partyState.AvailableMembers[templateResRef] = memberState;
            }

            // Deserialize selected party
            int selectedCount = reader.ReadInt32();
            for (int i = 0; i < selectedCount; i++)
            {
                string memberResRef = ReadString(reader);
                partyState.SelectedParty.Add(memberResRef);
            }

            // Deserialize party resources
            partyState.Gold = reader.ReadInt32();
            partyState.ExperiencePoints = reader.ReadInt32();

            // Deserialize influence values
            int influenceCount = reader.ReadInt32();
            for (int i = 0; i < influenceCount; i++)
            {
                partyState.Influence.Add(reader.ReadInt32());
            }

            // Deserialize other party state
            partyState.ItemComponent = reader.ReadInt32();
            partyState.ItemChemical = reader.ReadInt32();
            partyState.Swoop1 = reader.ReadInt32();
            partyState.Swoop2 = reader.ReadInt32();
            partyState.Swoop3 = reader.ReadInt32();
            partyState.PlayTime = TimeSpan.FromSeconds(reader.ReadInt32());
            partyState.ControlledNPC = reader.ReadInt32();
            partyState.SoloMode = reader.ReadInt32() != 0;
            partyState.CheatUsed = reader.ReadInt32() != 0;
            partyState.LeaderResRef = ReadString(reader);

            // Deserialize puppets
            int puppetCount = reader.ReadInt32();
            for (int i = 0; i < puppetCount; i++)
            {
                partyState.Puppets.Add(reader.ReadUInt32());
            }

            // Deserialize available puppets
            int availablePuppetCount = reader.ReadInt32();
            for (int i = 0; i < availablePuppetCount; i++)
            {
                partyState.AvailablePuppets.Add(reader.ReadInt32() != 0);
            }

            // Deserialize selectable puppets
            int selectablePuppetCount = reader.ReadInt32();
            for (int i = 0; i < selectablePuppetCount; i++)
            {
                partyState.SelectablePuppets.Add(reader.ReadInt32() != 0);
            }

            partyState.AIState = reader.ReadInt32();
            partyState.FollowState = reader.ReadInt32();
            partyState.GalaxyMapPlanetMask = reader.ReadInt32();
            partyState.GalaxyMapSelectedPoint = reader.ReadInt32();

            // Deserialize Pazaak cards
            int pazaakCardCount = reader.ReadInt32();
            for (int i = 0; i < pazaakCardCount; i++)
            {
                partyState.PazaakCards.Add(reader.ReadInt32());
            }

            // Deserialize Pazaak side list
            int pazaakSideCount = reader.ReadInt32();
            for (int i = 0; i < pazaakSideCount; i++)
            {
                partyState.PazaakSideList.Add(reader.ReadInt32());
            }

            // Deserialize tutorial windows shown
            int tutorialCount = reader.ReadInt32();
            for (int i = 0; i < tutorialCount; i++)
            {
                partyState.TutorialWindowsShown.Add(reader.ReadInt32() != 0);
            }

            partyState.LastGUIPanel = reader.ReadInt32();
            partyState.DisableMap = reader.ReadInt32() != 0;
            partyState.DisableRegen = reader.ReadInt32() != 0;

            // Deserialize feedback messages
            int feedbackCount = reader.ReadInt32();
            for (int i = 0; i < feedbackCount; i++)
            {
                partyState.FeedbackMessages.Add(new FeedbackMessage
                {
                    Message = ReadString(reader),
                    Type = reader.ReadInt32(),
                    Color = reader.ReadByte()
                });
            }

            // Deserialize dialogue messages
            int dialogueCount = reader.ReadInt32();
            for (int i = 0; i < dialogueCount; i++)
            {
                partyState.DialogueMessages.Add(new DialogueMessage
                {
                    Speaker = ReadString(reader),
                    Message = ReadString(reader)
                });
            }

            // Deserialize combat messages
            int combatCount = reader.ReadInt32();
            for (int i = 0; i < combatCount; i++)
            {
                partyState.CombatMessages.Add(new CombatMessage
                {
                    Message = ReadString(reader),
                    Type = reader.ReadInt32(),
                    Color = reader.ReadByte()
                });
            }

            // Deserialize cost multipliers
            int costMultiplierCount = reader.ReadInt32();
            for (int i = 0; i < costMultiplierCount; i++)
            {
                partyState.CostMultipliers.Add(reader.ReadSingle());
            }

            return partyState;
        }

        /// <summary>
        /// Serializes creature state (player character or party member).
        /// </summary>
        private void SerializeCreatureState(BinaryWriter writer, CreatureState creature)
        {
            if (creature == null)
            {
                writer.Write(0); // Has creature
                return;
            }

            writer.Write(1); // Has creature

            // Serialize base entity state
            SerializeEntityState(writer, creature);

            // Serialize creature-specific fields
            writer.Write(creature.Level);
            writer.Write(creature.XP);
            writer.Write(creature.CurrentFP);
            writer.Write(creature.MaxFP);
            writer.Write(creature.Alignment);

            // Serialize equipment
            SerializeEquipmentState(writer, creature.Equipment);

            // Serialize inventory
            int inventoryCount = creature.Inventory != null ? creature.Inventory.Count : 0;
            writer.Write(inventoryCount);
            if (creature.Inventory != null)
            {
                foreach (ItemState item in creature.Inventory)
                {
                    SerializeItemState(writer, item);
                }
            }

            // Serialize known powers
            int powersCount = creature.KnownPowers != null ? creature.KnownPowers.Count : 0;
            writer.Write(powersCount);
            if (creature.KnownPowers != null)
            {
                foreach (string power in creature.KnownPowers)
                {
                    WriteString(writer, power);
                }
            }

            // Serialize known feats
            int featsCount = creature.KnownFeats != null ? creature.KnownFeats.Count : 0;
            writer.Write(featsCount);
            if (creature.KnownFeats != null)
            {
                foreach (string feat in creature.KnownFeats)
                {
                    WriteString(writer, feat);
                }
            }

            // Serialize class levels
            int classLevelsCount = creature.ClassLevels != null ? creature.ClassLevels.Count : 0;
            writer.Write(classLevelsCount);
            if (creature.ClassLevels != null)
            {
                foreach (ClassLevel classLevel in creature.ClassLevels)
                {
                    writer.Write(classLevel.ClassId);
                    writer.Write(classLevel.Level);
                    int powersGainedCount = classLevel.PowersGained != null ? classLevel.PowersGained.Count : 0;
                    writer.Write(powersGainedCount);
                    if (classLevel.PowersGained != null)
                    {
                        foreach (string power in classLevel.PowersGained)
                        {
                            WriteString(writer, power);
                        }
                    }
                }
            }

            // Serialize skills
            int skillsCount = creature.Skills != null ? creature.Skills.Count : 0;
            writer.Write(skillsCount);
            if (creature.Skills != null)
            {
                foreach (var kvp in creature.Skills)
                {
                    WriteString(writer, kvp.Key);
                    writer.Write(kvp.Value);
                }
            }

            // Serialize attributes
            if (creature.Attributes != null)
            {
                writer.Write(1);
                writer.Write(creature.Attributes.Strength);
                writer.Write(creature.Attributes.Dexterity);
                writer.Write(creature.Attributes.Constitution);
                writer.Write(creature.Attributes.Intelligence);
                writer.Write(creature.Attributes.Wisdom);
                writer.Write(creature.Attributes.Charisma);
            }
            else
            {
                writer.Write(0);
            }
        }

        /// <summary>
        /// Deserializes creature state.
        /// </summary>
        private CreatureState DeserializeCreatureState(BinaryReader reader)
        {
            bool hasCreature = reader.ReadInt32() != 0;
            if (!hasCreature)
            {
                return null;
            }

            var creature = new CreatureState();

            // Deserialize base entity state
            DeserializeEntityState(reader, creature);

            // Deserialize creature-specific fields
            creature.Level = reader.ReadInt32();
            creature.XP = reader.ReadInt32();
            creature.CurrentFP = reader.ReadInt32();
            creature.MaxFP = reader.ReadInt32();
            creature.Alignment = reader.ReadInt32();

            // Deserialize equipment
            creature.Equipment = DeserializeEquipmentState(reader);

            // Deserialize inventory
            int inventoryCount = reader.ReadInt32();
            for (int i = 0; i < inventoryCount; i++)
            {
                creature.Inventory.Add(DeserializeItemState(reader));
            }

            // Deserialize known powers
            int powersCount = reader.ReadInt32();
            for (int i = 0; i < powersCount; i++)
            {
                creature.KnownPowers.Add(ReadString(reader));
            }

            // Deserialize known feats
            int featsCount = reader.ReadInt32();
            for (int i = 0; i < featsCount; i++)
            {
                creature.KnownFeats.Add(ReadString(reader));
            }

            // Deserialize class levels
            int classLevelsCount = reader.ReadInt32();
            for (int i = 0; i < classLevelsCount; i++)
            {
                var classLevel = new ClassLevel
                {
                    ClassId = reader.ReadInt32(),
                    Level = reader.ReadInt32()
                };
                int powersGainedCount = reader.ReadInt32();
                for (int j = 0; j < powersGainedCount; j++)
                {
                    classLevel.PowersGained.Add(ReadString(reader));
                }
                creature.ClassLevels.Add(classLevel);
            }

            // Deserialize skills
            int skillsCount = reader.ReadInt32();
            for (int i = 0; i < skillsCount; i++)
            {
                string skillName = ReadString(reader);
                int skillValue = reader.ReadInt32();
                creature.Skills[skillName] = skillValue;
            }

            // Deserialize attributes
            bool hasAttributes = reader.ReadInt32() != 0;
            if (hasAttributes)
            {
                creature.Attributes = new AttributeSet
                {
                    Strength = reader.ReadInt32(),
                    Dexterity = reader.ReadInt32(),
                    Constitution = reader.ReadInt32(),
                    Intelligence = reader.ReadInt32(),
                    Wisdom = reader.ReadInt32(),
                    Charisma = reader.ReadInt32()
                };
            }

            return creature;
        }

        /// <summary>
        /// Serializes entity state (base class for all entities).
        /// </summary>
        private void SerializeEntityState(BinaryWriter writer, EntityState entity)
        {
            if (entity == null)
            {
                writer.Write(0); // Has entity
                return;
            }

            writer.Write(1); // Has entity

            WriteString(writer, entity.Tag ?? "");
            writer.Write(entity.ObjectId);
            writer.Write((int)entity.ObjectType);
            WriteString(writer, entity.TemplateResRef ?? "");
            writer.Write(entity.Position.X);
            writer.Write(entity.Position.Y);
            writer.Write(entity.Position.Z);
            writer.Write(entity.Facing);
            writer.Write(entity.CurrentHP);
            writer.Write(entity.MaxHP);
            writer.Write(entity.IsDestroyed ? 1 : 0);
            writer.Write(entity.IsPlot ? 1 : 0);
            writer.Write(entity.IsOpen ? 1 : 0);
            writer.Write(entity.IsLocked ? 1 : 0);
            writer.Write(entity.AnimationState);

            // Serialize local variables
            SerializeLocalVariableSet(writer, entity.LocalVariables);

            // Serialize active effects
            int effectsCount = entity.ActiveEffects != null ? entity.ActiveEffects.Count : 0;
            writer.Write(effectsCount);
            if (entity.ActiveEffects != null)
            {
                foreach (SavedEffect effect in entity.ActiveEffects)
                {
                    SerializeSavedEffect(writer, effect);
                }
            }
        }

        /// <summary>
        /// Deserializes entity state.
        /// </summary>
        private void DeserializeEntityState(BinaryReader reader, EntityState entity)
        {
            bool hasEntity = reader.ReadInt32() != 0;
            if (!hasEntity)
            {
                return;
            }

            entity.Tag = ReadString(reader);
            entity.ObjectId = reader.ReadUInt32();
            entity.ObjectType = (ObjectType)reader.ReadInt32();
            entity.TemplateResRef = ReadString(reader);
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            float z = reader.ReadSingle();
            entity.Position = new Vector3(x, y, z);
            entity.Facing = reader.ReadSingle();
            entity.CurrentHP = reader.ReadInt32();
            entity.MaxHP = reader.ReadInt32();
            entity.IsDestroyed = reader.ReadInt32() != 0;
            entity.IsPlot = reader.ReadInt32() != 0;
            entity.IsOpen = reader.ReadInt32() != 0;
            entity.IsLocked = reader.ReadInt32() != 0;
            entity.AnimationState = reader.ReadInt32();

            // Deserialize local variables
            entity.LocalVariables = DeserializeLocalVariableSet(reader);

            // Deserialize active effects
            int effectsCount = reader.ReadInt32();
            for (int i = 0; i < effectsCount; i++)
            {
                entity.ActiveEffects.Add(DeserializeSavedEffect(reader));
            }
        }

        /// <summary>
        /// Serializes equipment state.
        /// </summary>
        private void SerializeEquipmentState(BinaryWriter writer, EquipmentState equipment)
        {
            if (equipment == null)
            {
                writer.Write(0); // Has equipment
                return;
            }

            writer.Write(1); // Has equipment
            SerializeItemState(writer, equipment.Head);
            SerializeItemState(writer, equipment.Armor);
            SerializeItemState(writer, equipment.Gloves);
            SerializeItemState(writer, equipment.RightHand);
            SerializeItemState(writer, equipment.LeftHand);
            SerializeItemState(writer, equipment.Belt);
            SerializeItemState(writer, equipment.Implant);
            SerializeItemState(writer, equipment.RightArm);
            SerializeItemState(writer, equipment.LeftArm);
        }

        /// <summary>
        /// Deserializes equipment state.
        /// </summary>
        private EquipmentState DeserializeEquipmentState(BinaryReader reader)
        {
            bool hasEquipment = reader.ReadInt32() != 0;
            if (!hasEquipment)
            {
                return new EquipmentState();
            }

            return new EquipmentState
            {
                Head = DeserializeItemState(reader),
                Armor = DeserializeItemState(reader),
                Gloves = DeserializeItemState(reader),
                RightHand = DeserializeItemState(reader),
                LeftHand = DeserializeItemState(reader),
                Belt = DeserializeItemState(reader),
                Implant = DeserializeItemState(reader),
                RightArm = DeserializeItemState(reader),
                LeftArm = DeserializeItemState(reader)
            };
        }

        /// <summary>
        /// Serializes item state.
        /// </summary>
        private void SerializeItemState(BinaryWriter writer, ItemState item)
        {
            if (item == null)
            {
                writer.Write(0); // Has item
                return;
            }

            writer.Write(1); // Has item
            WriteString(writer, item.TemplateResRef ?? "");
            writer.Write(item.StackSize);
            writer.Write(item.Charges);
            writer.Write(item.Identified ? 1 : 0);

            // Serialize upgrades
            int upgradesCount = item.Upgrades != null ? item.Upgrades.Count : 0;
            writer.Write(upgradesCount);
            if (item.Upgrades != null)
            {
                foreach (ItemUpgrade upgrade in item.Upgrades)
                {
                    writer.Write(upgrade.UpgradeSlot);
                    WriteString(writer, upgrade.UpgradeResRef ?? "");
                }
            }
        }

        /// <summary>
        /// Deserializes item state.
        /// </summary>
        private ItemState DeserializeItemState(BinaryReader reader)
        {
            bool hasItem = reader.ReadInt32() != 0;
            if (!hasItem)
            {
                return null;
            }

            var item = new ItemState
            {
                TemplateResRef = ReadString(reader),
                StackSize = reader.ReadInt32(),
                Charges = reader.ReadInt32(),
                Identified = reader.ReadInt32() != 0
            };

            // Deserialize upgrades
            int upgradesCount = reader.ReadInt32();
            for (int i = 0; i < upgradesCount; i++)
            {
                item.Upgrades.Add(new ItemUpgrade
                {
                    UpgradeSlot = reader.ReadInt32(),
                    UpgradeResRef = ReadString(reader)
                });
            }

            return item;
        }

        /// <summary>
        /// Serializes local variable set.
        /// </summary>
        private void SerializeLocalVariableSet(BinaryWriter writer, LocalVariableSet localVars)
        {
            if (localVars == null)
            {
                writer.Write(0); // Has local vars
                return;
            }

            writer.Write(1); // Has local vars

            // Serialize integer variables
            writer.Write(localVars.Ints != null ? localVars.Ints.Count : 0);
            if (localVars.Ints != null)
            {
                foreach (var kvp in localVars.Ints)
                {
                    WriteString(writer, kvp.Key);
                    writer.Write(kvp.Value);
                }
            }

            // Serialize float variables
            writer.Write(localVars.Floats != null ? localVars.Floats.Count : 0);
            if (localVars.Floats != null)
            {
                foreach (var kvp in localVars.Floats)
                {
                    WriteString(writer, kvp.Key);
                    writer.Write(kvp.Value);
                }
            }

            // Serialize string variables
            writer.Write(localVars.Strings != null ? localVars.Strings.Count : 0);
            if (localVars.Strings != null)
            {
                foreach (var kvp in localVars.Strings)
                {
                    WriteString(writer, kvp.Key);
                    WriteString(writer, kvp.Value);
                }
            }

            // Serialize object reference variables
            writer.Write(localVars.Objects != null ? localVars.Objects.Count : 0);
            if (localVars.Objects != null)
            {
                foreach (var kvp in localVars.Objects)
                {
                    WriteString(writer, kvp.Key);
                    writer.Write(kvp.Value);
                }
            }

            // Serialize location variables
            writer.Write(localVars.Locations != null ? localVars.Locations.Count : 0);
            if (localVars.Locations != null)
            {
                foreach (var kvp in localVars.Locations)
                {
                    WriteString(writer, kvp.Key);
                    SerializeSavedLocation(writer, kvp.Value);
                }
            }
        }

        /// <summary>
        /// Deserializes local variable set.
        /// </summary>
        private LocalVariableSet DeserializeLocalVariableSet(BinaryReader reader)
        {
            bool hasLocalVars = reader.ReadInt32() != 0;
            if (!hasLocalVars)
            {
                return new LocalVariableSet();
            }

            var localVars = new LocalVariableSet();

            // Deserialize integer variables
            int intCount = reader.ReadInt32();
            for (int i = 0; i < intCount; i++)
            {
                string name = ReadString(reader);
                int value = reader.ReadInt32();
                localVars.Ints[name] = value;
            }

            // Deserialize float variables
            int floatCount = reader.ReadInt32();
            for (int i = 0; i < floatCount; i++)
            {
                string name = ReadString(reader);
                float value = reader.ReadSingle();
                localVars.Floats[name] = value;
            }

            // Deserialize string variables
            int stringCount = reader.ReadInt32();
            for (int i = 0; i < stringCount; i++)
            {
                string name = ReadString(reader);
                string value = ReadString(reader);
                localVars.Strings[name] = value;
            }

            // Deserialize object reference variables
            int objectCount = reader.ReadInt32();
            for (int i = 0; i < objectCount; i++)
            {
                string name = ReadString(reader);
                uint value = reader.ReadUInt32();
                localVars.Objects[name] = value;
            }

            // Deserialize location variables
            int locationCount = reader.ReadInt32();
            for (int i = 0; i < locationCount; i++)
            {
                string name = ReadString(reader);
                SavedLocation location = DeserializeSavedLocation(reader);
                localVars.Locations[name] = location;
            }

            return localVars;
        }

        /// <summary>
        /// Serializes saved effect.
        /// </summary>
        private void SerializeSavedEffect(BinaryWriter writer, SavedEffect effect)
        {
            if (effect == null)
            {
                writer.Write(0); // Has effect
                return;
            }

            writer.Write(1); // Has effect
            writer.Write(effect.EffectType);
            writer.Write(effect.SubType);
            writer.Write(effect.DurationType);
            writer.Write(effect.RemainingDuration);
            writer.Write(effect.CreatorId);
            writer.Write(effect.SpellId);

            // Serialize parameters
            int intParamCount = effect.IntParams != null ? effect.IntParams.Count : 0;
            writer.Write(intParamCount);
            if (effect.IntParams != null)
            {
                foreach (int param in effect.IntParams)
                {
                    writer.Write(param);
                }
            }

            int floatParamCount = effect.FloatParams != null ? effect.FloatParams.Count : 0;
            writer.Write(floatParamCount);
            if (effect.FloatParams != null)
            {
                foreach (float param in effect.FloatParams)
                {
                    writer.Write(param);
                }
            }

            int stringParamCount = effect.StringParams != null ? effect.StringParams.Count : 0;
            writer.Write(stringParamCount);
            if (effect.StringParams != null)
            {
                foreach (string param in effect.StringParams)
                {
                    WriteString(writer, param);
                }
            }

            int objectParamCount = effect.ObjectParams != null ? effect.ObjectParams.Count : 0;
            writer.Write(objectParamCount);
            if (effect.ObjectParams != null)
            {
                foreach (uint param in effect.ObjectParams)
                {
                    writer.Write(param);
                }
            }
        }

        /// <summary>
        /// Deserializes saved effect.
        /// </summary>
        private SavedEffect DeserializeSavedEffect(BinaryReader reader)
        {
            bool hasEffect = reader.ReadInt32() != 0;
            if (!hasEffect)
            {
                return null;
            }

            var effect = new SavedEffect
            {
                EffectType = reader.ReadInt32(),
                SubType = reader.ReadInt32(),
                DurationType = reader.ReadInt32(),
                RemainingDuration = reader.ReadSingle(),
                CreatorId = reader.ReadUInt32(),
                SpellId = reader.ReadInt32()
            };

            // Deserialize parameters
            int intParamCount = reader.ReadInt32();
            for (int i = 0; i < intParamCount; i++)
            {
                effect.IntParams.Add(reader.ReadInt32());
            }

            int floatParamCount = reader.ReadInt32();
            for (int i = 0; i < floatParamCount; i++)
            {
                effect.FloatParams.Add(reader.ReadSingle());
            }

            int stringParamCount = reader.ReadInt32();
            for (int i = 0; i < stringParamCount; i++)
            {
                effect.StringParams.Add(ReadString(reader));
            }

            int objectParamCount = reader.ReadInt32();
            for (int i = 0; i < objectParamCount; i++)
            {
                effect.ObjectParams.Add(reader.ReadUInt32());
            }

            return effect;
        }

        /// <summary>
        /// Serializes saved location.
        /// </summary>
        private void SerializeSavedLocation(BinaryWriter writer, SavedLocation location)
        {
            if (location == null)
            {
                writer.Write(0); // Has location
                return;
            }

            writer.Write(1); // Has location
            WriteString(writer, location.AreaResRef ?? "");
            writer.Write(location.Position.X);
            writer.Write(location.Position.Y);
            writer.Write(location.Position.Z);
            writer.Write(location.Facing);
        }

        /// <summary>
        /// Deserializes saved location.
        /// </summary>
        private SavedLocation DeserializeSavedLocation(BinaryReader reader)
        {
            bool hasLocation = reader.ReadInt32() != 0;
            if (!hasLocation)
            {
                return null;
            }

            var location = new SavedLocation
            {
                AreaResRef = ReadString(reader)
            };
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();
            float z = reader.ReadSingle();
            location.Position = new Vector3(x, y, z);
            location.Facing = reader.ReadSingle();

            return location;
        }

        /// <summary>
        /// Serializes area states dictionary.
        /// </summary>
        private void SerializeAreaStates(BinaryWriter writer, Dictionary<string, AreaState> areaStates)
        {
            int areaCount = areaStates != null ? areaStates.Count : 0;
            writer.Write(areaCount);

            if (areaStates != null)
            {
                foreach (var kvp in areaStates)
                {
                    WriteString(writer, kvp.Key); // Area ResRef
                    SerializeAreaState(writer, kvp.Value);
                }
            }
        }

        /// <summary>
        /// Deserializes area states dictionary.
        /// </summary>
        private Dictionary<string, AreaState> DeserializeAreaStates(BinaryReader reader)
        {
            var areaStates = new Dictionary<string, AreaState>();

            int areaCount = reader.ReadInt32();
            for (int i = 0; i < areaCount; i++)
            {
                string areaResRef = ReadString(reader);
                AreaState areaState = DeserializeAreaState(reader);
                areaStates[areaResRef] = areaState;
            }

            return areaStates;
        }

        /// <summary>
        /// Serializes a single area state.
        /// </summary>
        private void SerializeAreaState(BinaryWriter writer, AreaState areaState)
        {
            if (areaState == null)
            {
                writer.Write(0); // Has area state
                return;
            }

            writer.Write(1); // Has area state
            WriteString(writer, areaState.AreaResRef ?? "");
            writer.Write(areaState.Visited ? 1 : 0);

            // Serialize entity state lists
            SerializeEntityStateList(writer, areaState.CreatureStates);
            SerializeEntityStateList(writer, areaState.DoorStates);
            SerializeEntityStateList(writer, areaState.PlaceableStates);
            SerializeEntityStateList(writer, areaState.TriggerStates);
            SerializeEntityStateList(writer, areaState.StoreStates);
            SerializeEntityStateList(writer, areaState.SoundStates);
            SerializeEntityStateList(writer, areaState.WaypointStates);
            SerializeEntityStateList(writer, areaState.EncounterStates);
            SerializeEntityStateList(writer, areaState.CameraStates);

            // Serialize destroyed entity IDs
            int destroyedCount = areaState.DestroyedEntityIds != null ? areaState.DestroyedEntityIds.Count : 0;
            writer.Write(destroyedCount);
            if (areaState.DestroyedEntityIds != null)
            {
                foreach (uint entityId in areaState.DestroyedEntityIds)
                {
                    writer.Write(entityId);
                }
            }

            // Serialize spawned entities
            int spawnedCount = areaState.SpawnedEntities != null ? areaState.SpawnedEntities.Count : 0;
            writer.Write(spawnedCount);
            if (areaState.SpawnedEntities != null)
            {
                foreach (SpawnedEntityState spawned in areaState.SpawnedEntities)
                {
                    SerializeEntityState(writer, spawned);
                    WriteString(writer, spawned.BlueprintResRef ?? "");
                    WriteString(writer, spawned.SpawnedBy ?? "");
                }
            }

            // Serialize local variables
            int localVarCount = areaState.LocalVariables != null ? areaState.LocalVariables.Count : 0;
            writer.Write(localVarCount);
            if (areaState.LocalVariables != null)
            {
                foreach (var kvp in areaState.LocalVariables)
                {
                    WriteString(writer, kvp.Key);
                    SerializeObjectValue(writer, kvp.Value);
                }
            }
        }

        /// <summary>
        /// Deserializes a single area state.
        /// </summary>
        private AreaState DeserializeAreaState(BinaryReader reader)
        {
            bool hasAreaState = reader.ReadInt32() != 0;
            if (!hasAreaState)
            {
                return null;
            }

            var areaState = new AreaState
            {
                AreaResRef = ReadString(reader),
                Visited = reader.ReadInt32() != 0
            };

            // Deserialize entity state lists
            areaState.CreatureStates = DeserializeEntityStateList(reader);
            areaState.DoorStates = DeserializeEntityStateList(reader);
            areaState.PlaceableStates = DeserializeEntityStateList(reader);
            areaState.TriggerStates = DeserializeEntityStateList(reader);
            areaState.StoreStates = DeserializeEntityStateList(reader);
            areaState.SoundStates = DeserializeEntityStateList(reader);
            areaState.WaypointStates = DeserializeEntityStateList(reader);
            areaState.EncounterStates = DeserializeEntityStateList(reader);
            areaState.CameraStates = DeserializeEntityStateList(reader);

            // Deserialize destroyed entity IDs
            int destroyedCount = reader.ReadInt32();
            for (int i = 0; i < destroyedCount; i++)
            {
                areaState.DestroyedEntityIds.Add(reader.ReadUInt32());
            }

            // Deserialize spawned entities
            int spawnedCount = reader.ReadInt32();
            for (int i = 0; i < spawnedCount; i++)
            {
                var spawned = new SpawnedEntityState();
                DeserializeEntityState(reader, spawned);
                spawned.BlueprintResRef = ReadString(reader);
                spawned.SpawnedBy = ReadString(reader);
                areaState.SpawnedEntities.Add(spawned);
            }

            // Deserialize local variables
            int localVarCount = reader.ReadInt32();
            for (int i = 0; i < localVarCount; i++)
            {
                string key = ReadString(reader);
                object value = DeserializeObjectValue(reader);
                areaState.LocalVariables[key] = value;
            }

            return areaState;
        }

        /// <summary>
        /// Serializes a list of entity states.
        /// </summary>
        private void SerializeEntityStateList(BinaryWriter writer, List<EntityState> entityStates)
        {
            int count = entityStates != null ? entityStates.Count : 0;
            writer.Write(count);
            if (entityStates != null)
            {
                foreach (EntityState entity in entityStates)
                {
                    SerializeEntityState(writer, entity);
                }
            }
        }

        /// <summary>
        /// Deserializes a list of entity states.
        /// </summary>
        private List<EntityState> DeserializeEntityStateList(BinaryReader reader)
        {
            var entityStates = new List<EntityState>();

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var entity = new EntityState();
                DeserializeEntityState(reader, entity);
                entityStates.Add(entity);
            }

            return entityStates;
        }

        /// <summary>
        /// Serializes an object value (for local variables dictionary).
        /// Supports int, float, string, bool, uint types.
        /// </summary>
        private void SerializeObjectValue(BinaryWriter writer, object value)
        {
            if (value == null)
            {
                writer.Write((byte)0); // Type: null
                return;
            }

            Type valueType = value.GetType();
            if (valueType == typeof(int))
            {
                writer.Write((byte)1); // Type: int
                writer.Write((int)value);
            }
            else if (valueType == typeof(float))
            {
                writer.Write((byte)2); // Type: float
                writer.Write((float)value);
            }
            else if (valueType == typeof(string))
            {
                writer.Write((byte)3); // Type: string
                WriteString(writer, (string)value);
            }
            else if (valueType == typeof(bool))
            {
                writer.Write((byte)4); // Type: bool
                writer.Write((bool)value ? 1 : 0);
            }
            else if (valueType == typeof(uint))
            {
                writer.Write((byte)5); // Type: uint
                writer.Write((uint)value);
            }
            else
            {
                // Unknown type, serialize as string
                writer.Write((byte)3);
                WriteString(writer, value.ToString());
            }
        }

        /// <summary>
        /// Deserializes an object value.
        /// </summary>
        private object DeserializeObjectValue(BinaryReader reader)
        {
            byte type = reader.ReadByte();
            switch (type)
            {
                case 0: // null
                    return null;
                case 1: // int
                    return reader.ReadInt32();
                case 2: // float
                    return reader.ReadSingle();
                case 3: // string
                    return ReadString(reader);
                case 4: // bool
                    return reader.ReadInt32() != 0;
                case 5: // uint
                    return reader.ReadUInt32();
                default:
                    throw new InvalidDataException($"Unknown object value type: {type}");
            }
        }

        /// <summary>
        /// Serializes journal entries (Mass Effect-specific: includes DateAdded timestamp).
        /// </summary>
        /// <remarks>
        /// Mass Effect includes DateAdded timestamp in journal entries, unlike the base class implementation.
        /// </remarks>
        private new void SerializeJournalEntries(BinaryWriter writer, List<JournalEntry> journalEntries)
        {
            int count = journalEntries != null ? journalEntries.Count : 0;
            writer.Write(count);
            if (journalEntries != null)
            {
                foreach (JournalEntry entry in journalEntries)
                {
                    WriteString(writer, entry.QuestTag ?? "");
                    writer.Write(entry.State);
                    writer.Write(entry.DateAdded.ToFileTime());
                }
            }
        }

        /// <summary>
        /// Deserializes journal entries (Mass Effect-specific: includes DateAdded timestamp).
        /// </summary>
        private new List<JournalEntry> DeserializeJournalEntries(BinaryReader reader)
        {
            var journalEntries = new List<JournalEntry>();

            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                journalEntries.Add(new JournalEntry
                {
                    QuestTag = ReadString(reader),
                    State = reader.ReadInt32(),
                    DateAdded = DateTime.FromFileTime(reader.ReadInt64())
                });
            }

            return journalEntries;
        }

        #endregion
    }
}

