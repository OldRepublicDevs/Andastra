using System;
using System.Collections.Generic;
using BioWare.Common;

namespace BioWare.Resource.Formats.NCS
{
    /// <summary>
    /// Patches compiled NCS bytecode in-place to remap ACTION instruction IDs
    /// between K1 and TSL without decompilation or recompilation.
    ///
    /// The NCS binary format is identical between games. The only difference is the
    /// engine action table: IDs 0-767 are shared; IDs 768+ diverge. K1's late
    /// additions (768-771) were relocated to 805-808 in TSL, while TSL replaced
    /// slots 768-804 and added 809-876 with new Obsidian functions.
    /// </summary>
    public static class NCSActionPatcher
    {
        private const int HeaderSize = 13;
        private const byte ActionOpcode = 0x05;

        private static readonly Dictionary<ushort, ushort> K1ToTsl = new Dictionary<ushort, ushort>
        {
            { 768, 805 }, // IsMoviePlaying
            { 769, 806 }, // QueueMovie
            { 770, 807 }, // PlayMovieQueue
            { 771, 808 }, // YavinHackCloseDoor -> YavinHackDoorClose
        };

        private static readonly Dictionary<ushort, ushort> TslToK1 = new Dictionary<ushort, ushort>
        {
            { 805, 768 }, // IsMoviePlaying
            { 806, 769 }, // QueueMovie
            { 807, 770 }, // PlayMovieQueue
            { 808, 771 }, // YavinHackDoorClose -> YavinHackCloseDoor
        };

        private static readonly HashSet<ushort> TslOnlyActions = BuildTslOnlySet();
        private static readonly Dictionary<ushort, byte> K1ExpectedParamCounts = new Dictionary<ushort, byte>
        {
            { 31, 3 }, { 82, 0 }, { 133, 2 }, { 201, 3 }, { 204, 11 },
            { 241, 4 }, { 320, 1 }, { 354, 1 }, { 428, 2 }, { 429, 2 },
            { 671, 2 }, { 712, 3 }, { 733, 1 }, { 745, 2 }, { 767, 0 },
            { 768, 0 }, { 769, 2 }, { 770, 1 }, { 771, 1 },
        };
        private static readonly Dictionary<ushort, byte> TslExpectedParamCounts = new Dictionary<ushort, byte>
        {
            { 31, 4 }, { 82, 1 }, { 133, 3 }, { 201, 4 }, { 204, 15 },
            { 241, 5 }, { 320, 2 }, { 354, 5 }, { 428, 3 }, { 429, 3 },
            { 671, 3 }, { 712, 4 }, { 733, 2 }, { 745, 1 }, { 767, 2 },
            { 805, 0 }, { 806, 2 }, { 807, 1 }, { 808, 1 },
        };

        private static HashSet<ushort> BuildTslOnlySet()
        {
            var set = new HashSet<ushort>();
            for (ushort id = 768; id <= 804; id++)
                set.Add(id);
            for (ushort id = 809; id <= 876; id++)
                set.Add(id);
            return set;
        }

        public struct PatchResult
        {
            public byte[] Data;
            public int ActionsPatched;
            public int ActionsTotal;
            public List<ushort> UnmappableActionIds;
            public List<ParamCountMismatch> ParamCountMismatches;
        }

        public struct ParamCountMismatch
        {
            public ushort ActionId;
            public ushort SourceActionId;
            public byte ScriptParamCount;
            public byte ExpectedTargetParamCount;
        }

        public static byte[] CreateNoOpNcs()
        {
            // Minimal valid NCS: header (13 bytes) + single RETN instruction (2 bytes).
            // Header size field is big-endian and includes the header itself.
            const int totalSize = 15;
            var data = new byte[totalSize];

            data[0] = (byte)'N';
            data[1] = (byte)'C';
            data[2] = (byte)'S';
            data[3] = (byte)' ';
            data[4] = (byte)'V';
            data[5] = (byte)'1';
            data[6] = (byte)'.';
            data[7] = (byte)'0';
            data[8] = 0x42;
            data[9] = 0x00;
            data[10] = 0x00;
            data[11] = 0x00;
            data[12] = 0x0F; // 15 bytes

            // RETN (0x20) with qualifier 0x00 at offset 13.
            data[13] = 0x20;
            data[14] = 0x00;
            return data;
        }

        /// <summary>
        /// Patch ACTION instruction IDs in a compiled NCS file for the target game.
        /// Returns the (possibly modified) byte array and patch statistics.
        /// </summary>
        public static PatchResult Patch(byte[] ncsData, BioWareGame sourceGame, BioWareGame targetGame)
        {
            var result = new PatchResult
            {
                Data = ncsData,
                ActionsPatched = 0,
                ActionsTotal = 0,
                UnmappableActionIds = new List<ushort>(),
                ParamCountMismatches = new List<ParamCountMismatch>()
            };

            if (ncsData == null || ncsData.Length < HeaderSize)
                return result;

            // Full header validation: "NCS V1.0" + 0x42
            if (ncsData[0] != (byte)'N' || ncsData[1] != (byte)'C' || ncsData[2] != (byte)'S' || ncsData[3] != (byte)' '
                || ncsData[4] != (byte)'V' || ncsData[5] != (byte)'1' || ncsData[6] != (byte)'.' || ncsData[7] != (byte)'0'
                || ncsData[8] != 0x42)
                return result;

            bool k1ToTsl = sourceGame.IsK1() && targetGame.IsK2();
            bool tslToK1 = sourceGame.IsK2() && targetGame.IsK1();
            if (!k1ToTsl && !tslToK1)
                return result;

            Dictionary<ushort, ushort> remap = k1ToTsl ? K1ToTsl : TslToK1;
            Dictionary<ushort, byte> targetParamCounts = k1ToTsl ? TslExpectedParamCounts : K1ExpectedParamCounts;
            var paramMismatchSet = new HashSet<uint>();

            byte[] patched = (byte[])ncsData.Clone();
            int offset = HeaderSize;

            while (offset + 1 < patched.Length)
            {
                byte opcode = patched[offset];

                if (opcode == ActionOpcode && offset + 4 < patched.Length)
                {
                    result.ActionsTotal++;
                    ushort actionId = (ushort)((patched[offset + 2] << 8) | patched[offset + 3]);
                    byte scriptParamCount = patched[offset + 4];
                    ushort targetActionId = actionId;

                    if (remap.TryGetValue(actionId, out ushort newId))
                    {
                        patched[offset + 2] = (byte)(newId >> 8);
                        patched[offset + 3] = (byte)(newId & 0xFF);
                        result.ActionsPatched++;
                        targetActionId = newId;
                    }
                    else if (tslToK1 && TslOnlyActions.Contains(actionId))
                    {
                        if (!result.UnmappableActionIds.Contains(actionId))
                            result.UnmappableActionIds.Add(actionId);
                    }

                    if (targetParamCounts.TryGetValue(targetActionId, out byte expectedTargetParamCount)
                        && scriptParamCount != expectedTargetParamCount)
                    {
                        uint key = (uint)(targetActionId << 16) | (uint)(scriptParamCount << 8) | expectedTargetParamCount;
                        if (paramMismatchSet.Add(key))
                        {
                            result.ParamCountMismatches.Add(new ParamCountMismatch
                            {
                                ActionId = targetActionId,
                                SourceActionId = actionId,
                                ScriptParamCount = scriptParamCount,
                                ExpectedTargetParamCount = expectedTargetParamCount
                            });
                        }
                    }
                }

                int size = GetInstructionSize(patched, offset);
                if (size <= 0)
                    break;
                offset += size;
            }

            result.Data = patched;
            return result;
        }

        /// <summary>
        /// Compute the byte size of the instruction at the given offset.
        /// Returns 0 if the instruction cannot be decoded (corrupt data).
        /// </summary>
        private static int GetInstructionSize(byte[] data, int offset)
        {
            if (offset >= data.Length)
                return 0;

            byte opcode = data[offset];
            byte qualifier = (offset + 1 < data.Length) ? data[offset + 1] : (byte)0;

            switch (opcode)
            {
                case 0x01: // CPDOWNSP
                case 0x03: // CPTOPSP
                case 0x26: // CPDOWNBP
                case 0x27: // CPTOPBP
                    return 2 + 4 + 2; // opcode+qual + int32 offset + uint16 size

                case 0x02: // RSADDx
                    return 2;

                case 0x04: // CONSTx
                    switch (qualifier)
                    {
                        case 0x03: // Int
                        case 0x06: // Object
                            return 2 + 4;
                        case 0x04: // Float
                            return 2 + 4;
                        case 0x05: // String
                            if (offset + 3 < data.Length)
                            {
                                ushort strLen = (ushort)((data[offset + 2] << 8) | data[offset + 3]);
                                return 2 + 2 + strLen;
                            }
                            return 0;
                        default:
                            return 2 + 4;
                    }

                case 0x05: // ACTION
                    return 2 + 2 + 1; // opcode+qual + uint16 action_id + uint8 param_count

                case 0x06: // LOGANDxx
                case 0x07: // LOGORxx
                case 0x08: // INCORxx
                case 0x09: // EXCORxx
                case 0x0A: // BOOLANDxx
                    return 2;

                case 0x0B: // EQUALxx
                case 0x0C: // NEQUALxx
                    if (qualifier == 0x24) // StructStruct
                        return 2 + 2;
                    return 2;

                case 0x0D: // GEQxx
                case 0x0E: // GTxx
                case 0x0F: // LTxx
                case 0x10: // LEQxx
                case 0x11: // SHLEFTxx
                case 0x12: // SHRIGHTxx
                case 0x13: // USHRIGHTxx
                case 0x14: // ADDxx
                case 0x15: // SUBxx
                case 0x16: // MULxx
                case 0x17: // DIVxx
                case 0x18: // MODxx
                case 0x19: // NEGx
                case 0x1A: // COMPx
                    return 2;

                case 0x1B: // MOVSP
                    return 2 + 4;

                case 0x1D: // JMP
                case 0x1E: // JSR
                case 0x1F: // JZ
                case 0x25: // JNZ
                    return 2 + 4;

                case 0x20: // RETN
                    return 2;

                case 0x21: // DESTRUCT
                    return 2 + 2 + 2 + 2; // opcode+qual + 3x uint16

                case 0x22: // NOTx
                    return 2;

                case 0x23: // DECxSP
                case 0x24: // INCxSP
                case 0x28: // DECxBP
                case 0x29: // INCxBP
                    return 2 + 4;

                case 0x2A: // SAVEBP
                case 0x2B: // RESTOREBP
                    return 2;

                case 0x2C: // STORE_STATE
                    return 2 + 4 + 4;

                case 0x2D: // NOP / NOP2
                    return 2;

                default:
                    return 2; // unknown opcode: assume minimal size to avoid infinite loop
            }
        }
    }
}
