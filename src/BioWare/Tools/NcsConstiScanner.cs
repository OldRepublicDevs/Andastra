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

        private static void SkipInstructionPayload(RawBinaryReader reader, byte opcode, byte qualifier)
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
