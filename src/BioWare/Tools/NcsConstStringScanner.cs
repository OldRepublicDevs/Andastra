using System;
using System.Collections.Generic;
using BioWare.Common;

namespace BioWare.Tools
{
    /// <summary>
    /// Walks NCS V1.0 bytecode and extracts CONSTS (opcode 0x04, qualifier 0x05) string operands.
    /// </summary>
    public static class NcsConstStringScanner
    {
        public struct ConstsInstruction
        {
            public int StringByteOffset;
            public string Value;
        }

        public static List<ConstsInstruction> ExtractConstsInstructions(byte[] ncsData)
        {
            var instructions = new List<ConstsInstruction>();
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

                        if (opcode == 0x04 && qualifier == 0x05)
                        {
                            ushort strLen = reader.ReadUInt16(bigEndian: true);
                            int stringOffset = reader.Position;
                            string constValue = reader.ReadString(strLen, "ascii");
                            instructions.Add(new ConstsInstruction
                            {
                                StringByteOffset = stringOffset,
                                Value = constValue ?? string.Empty
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

        public static List<int> ExtractConstsOffsetsForValue(
            byte[] ncsData,
            string targetValue,
            StringComparison comparison = StringComparison.Ordinal)
        {
            var offsets = new List<int>();
            if (string.IsNullOrEmpty(targetValue))
            {
                return offsets;
            }

            foreach (ConstsInstruction instruction in ExtractConstsInstructions(ncsData))
            {
                if (string.Equals(instruction.Value, targetValue, comparison))
                {
                    offsets.Add(instruction.StringByteOffset);
                }
            }

            return offsets;
        }

        private static void SkipInstructionPayload(RawBinaryReader reader, byte opcode, byte qualifier)
        {
            if (opcode == 0x04)
            {
                if (qualifier == 0x03 || qualifier == 0x06)
                {
                    reader.Skip(4);
                }
                else if (qualifier == 0x04)
                {
                    reader.Skip(4);
                }
                else if (qualifier == 0x05)
                {
                    ushort strLen = reader.ReadUInt16(bigEndian: true);
                    reader.Skip(strLen);
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
