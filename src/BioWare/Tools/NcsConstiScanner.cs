using System;
using System.Collections.Generic;
using BioWare.Common;

namespace BioWare.Tools
{
    /// <summary>
    /// Walks NCS V1.0 bytecode and extracts CONSTI (opcode 0x04, qualifier 0x03) operands.
    /// </summary>
    public static class NcsConstiScanner
    {
        /// <summary>
        /// CONSTI values below this are usually 2DA row indices, enums, or loop counters — not TLK StrRefs.
        /// Cache scans use this threshold; explicit StrRef queries still match any CONSTI via slow path.
        /// </summary>
        public const int StrRefCandidateMinimum = 100;

        public struct ConstiInstruction
        {
            public int ValueByteOffset;
            public int Value;
        }

        /// <summary>
        /// Heuristic classification of how a CONSTI operand is used in the next instruction.
        /// </summary>
        public enum ConstiUsageContext
        {
            Unknown = 0,
            StrRefConsumer = 1,
            GenericInteger = 2
        }

        /// <summary>
        /// NWScript ACTION indices whose first stack argument is a TLK StrRef (K1/TSL shared table).
        /// </summary>
        private static readonly int[] StrRefConsumerActionIds =
        {
            239, // GetStringByStrRef
            240, // ActionSpeakStringByStrRef
            671, // BarkString
            700  // ActionBarkString
        };

        public static List<ConstiInstruction> ExtractConstiInstructions(byte[] ncsData)
        {
            var instructions = new List<ConstiInstruction>();
            if (ncsData == null || ncsData.Length < 13)
            {
                return instructions;
            }

            try
            {
                using (RawBinaryReader reader = BinaryReader.FromBytes(ncsData))
                {
                    string signature = reader.ReadString(4);
                    if (signature != "NCS ")
                    {
                        return instructions;
                    }

                    string version = reader.ReadString(4);
                    if (version != "V1.0")
                    {
                        return instructions;
                    }

                    byte magicByte = reader.ReadUInt8();
                    if (magicByte != 0x42)
                    {
                        return instructions;
                    }

                    uint totalSize = reader.ReadUInt32(bigEndian: true);

                    while (reader.Position < totalSize && reader.Remaining > 0)
                    {
                        byte opcode = reader.ReadUInt8();
                        byte qualifier = reader.ReadUInt8();

                        if (opcode == 0x04 && qualifier == 0x03)
                        {
                            int valueOffset = reader.Position;
                            int constValue = reader.ReadInt32(bigEndian: true);
                            instructions.Add(new ConstiInstruction
                            {
                                ValueByteOffset = valueOffset,
                                Value = constValue
                            });
                        }
                        else
                        {
                            SkipInstructionPayload(reader, opcode, qualifier);
                        }
                    }
                }
            }
            catch
            {
                // Return partial results on malformed bytecode.
            }

            return instructions;
        }

        public static List<int> ExtractConstiOffsetsForValue(byte[] ncsData, int targetValue)
        {
            var offsets = new List<int>();
            foreach (ConstiInstruction instruction in ExtractConstiInstructions(ncsData))
            {
                if (instruction.Value == targetValue)
                {
                    offsets.Add(instruction.ValueByteOffset);
                }
            }

            return offsets;
        }

        public static bool IsPlausibleStrRefCandidate(int value)
        {
            return IsPlausibleStrRefCandidate(value, StrRefCandidateMinimum);
        }

        public static bool IsPlausibleStrRefCandidate(int value, int minimum)
        {
            return value >= minimum;
        }

        public static ConstiUsageContext GetConstiUsageContext(byte[] ncsData, ConstiInstruction instruction)
        {
            if (ncsData == null)
            {
                return ConstiUsageContext.Unknown;
            }

            int nextOffset = instruction.ValueByteOffset + 4;
            if (nextOffset + 2 > ncsData.Length)
            {
                return ConstiUsageContext.Unknown;
            }

            byte opcode = ncsData[nextOffset];
            byte qualifier = ncsData[nextOffset + 1];

            if (IsGenericIntegerConsumerOpcode(opcode, qualifier))
            {
                return ConstiUsageContext.GenericInteger;
            }

            int actionId;
            if (TryFindStrRefConsumerActionAfterConstiRun(ncsData, instruction, out actionId))
            {
                return ConstiUsageContext.StrRefConsumer;
            }

            return ConstiUsageContext.Unknown;
        }

        private static bool TryFindStrRefConsumerActionAfterConstiRun(
            byte[] ncsData,
            ConstiInstruction instruction,
            out int actionId)
        {
            actionId = -1;
            int constiOpcodeOffset = instruction.ValueByteOffset - 2;
            if (constiOpcodeOffset < 0)
            {
                return false;
            }

            if (HasConstiInstructionImmediatelyBefore(ncsData, constiOpcodeOffset))
            {
                return false;
            }

            int scanOffset = instruction.ValueByteOffset + 4;
            int maxScanEnd = Math.Min(ncsData.Length, scanOffset + 16);
            while (scanOffset + 2 <= maxScanEnd)
            {
                byte opcode = ncsData[scanOffset];
                byte qualifier = ncsData[scanOffset + 1];

                if (opcode == 0x05)
                {
                    if (scanOffset + 4 > ncsData.Length)
                    {
                        return false;
                    }

                    actionId = (ncsData[scanOffset + 2] << 8) | ncsData[scanOffset + 3];
                    return IsStrRefConsumerAction(actionId);
                }

                if (opcode == 0x04 && qualifier == 0x03)
                {
                    scanOffset += 6;
                    continue;
                }

                break;
            }

            return false;
        }

        private static bool HasConstiInstructionImmediatelyBefore(byte[] ncsData, int constiOpcodeOffset)
        {
            int previousOffset = constiOpcodeOffset - 6;
            if (previousOffset < 13)
            {
                return false;
            }

            return ncsData[previousOffset] == 0x04 && ncsData[previousOffset + 1] == 0x03;
        }

        public static bool ShouldIndexAsStrRefCandidate(byte[] ncsData, ConstiInstruction instruction, int minimum)
        {
            if (instruction.Value < 0)
            {
                return false;
            }

            ConstiUsageContext context = GetConstiUsageContext(ncsData, instruction);
            if (context == ConstiUsageContext.GenericInteger)
            {
                return false;
            }

            if (context == ConstiUsageContext.StrRefConsumer)
            {
                return true;
            }

            return IsPlausibleStrRefCandidate(instruction.Value, minimum);
        }

        private static bool IsStrRefConsumerAction(int actionId)
        {
            for (int i = 0; i < StrRefConsumerActionIds.Length; i++)
            {
                if (StrRefConsumerActionIds[i] == actionId)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsGenericIntegerConsumerOpcode(byte opcode, byte qualifier)
        {
            // Comparison ops (LT/GT/EQUAL/etc.) — qualifiers vary by operand types (0x03 int-int, 0x20 mixed, etc.).
            if (opcode >= 0x0B && opcode <= 0x10)
            {
                return true;
            }

            // Integer arithmetic / bitwise immediately consuming the CONSTI stack value.
            if (opcode >= 0x06 && opcode <= 0x18 && (qualifier == 0x03 || qualifier == 0x20 || qualifier == 0x23))
            {
                return true;
            }

            return false;
        }

        internal static void SkipInstructionPayload(RawBinaryReader reader, byte opcode, byte qualifier)
        {
            if (opcode == 0x04)
            {
                if (qualifier == 0x04)
                {
                    reader.Skip(4);
                }
                else if (qualifier == 0x05)
                {
                    ushort strLen = reader.ReadUInt16(bigEndian: true);
                    reader.Skip(strLen);
                }
                else if (qualifier == 0x06)
                {
                    reader.Skip(4);
                }
            }
            else if (opcode == 0x01 || opcode == 0x03 || opcode == 0x26 || opcode == 0x27)
            {
                reader.Skip(6);
            }
            else if (opcode == 0x2C)
            {
                reader.Skip(8);
            }
            else if (opcode == 0x1B || opcode == 0x1D || opcode == 0x1E || opcode == 0x1F ||
                     opcode == 0x23 || opcode == 0x24 || opcode == 0x25 || opcode == 0x28 || opcode == 0x29)
            {
                reader.Skip(4);
            }
            else if (opcode == 0x05)
            {
                reader.Skip(3);
            }
            else if (opcode == 0x21)
            {
                reader.Skip(6);
            }
            else if ((opcode == 0x0B || opcode == 0x0C) && qualifier == 0x24)
            {
                reader.Skip(2);
            }
        }
    }
}
