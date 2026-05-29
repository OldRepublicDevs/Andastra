using System;
using System.Collections.Generic;
using BioWare.Common;
using BioWare.Common.Script;

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
            GenericInteger = 2,
            StackStored = 3
        }

        /// <summary>
        /// NWScript ACTION indices with int StrRef parameters, mapped to parameter indices (from ScriptDefs).
        /// </summary>
        private static readonly Dictionary<int, int[]> StrRefParamIndicesByActionId = BuildStrRefParamIndicesByActionId();

        private static Dictionary<int, int[]> BuildStrRefParamIndicesByActionId()
        {
            var map = new Dictionary<int, int[]>();
            List<ScriptFunction> functions = ScriptDefs.KOTOR_FUNCTIONS;
            for (int actionId = 0; actionId < functions.Count; actionId++)
            {
                ScriptFunction function = functions[actionId];
                var indices = new List<int>();
                for (int paramIndex = 0; paramIndex < function.Params.Count; paramIndex++)
                {
                    ScriptParam param = function.Params[paramIndex];
                    if (param.DataType == DataType.Int && IsStrRefParameterName(param.Name))
                    {
                        indices.Add(paramIndex);
                    }
                }

                if (indices.Count > 0)
                {
                    map[actionId] = indices.ToArray();
                }
            }

            return map;
        }

        private static bool IsStrRefParameterName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            return name.IndexOf("strref", StringComparison.OrdinalIgnoreCase) >= 0;
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

            if (IsStackStoreOpcode(opcode))
            {
                return ConstiUsageContext.StackStored;
            }

            int actionId;
            List<ActionStackSlot> stackSlots;
            if (TryGetActionArgumentRun(ncsData, instruction, out actionId, out stackSlots)
                && IsConstiAtStrRefParameterSlot(actionId, instruction.ValueByteOffset, stackSlots))
            {
                return ConstiUsageContext.StrRefConsumer;
            }

            return ConstiUsageContext.Unknown;
        }

        private struct ActionStackSlot
        {
            public bool IsIntConst;
            public int ValueByteOffset;
        }

        private static bool TryGetActionArgumentRun(
            byte[] ncsData,
            ConstiInstruction fromInstruction,
            out int actionId,
            out List<ActionStackSlot> stackSlots)
        {
            actionId = -1;
            stackSlots = new List<ActionStackSlot>();

            int constiOpcodeOffset = fromInstruction.ValueByteOffset - 2;
            if (constiOpcodeOffset < 0)
            {
                return false;
            }

            int runStart = constiOpcodeOffset;
            while (runStart > 13)
            {
                int previousStart = runStart - 6;
                if (previousStart < 13 || !IsConstantPushAt(ncsData, previousStart))
                {
                    break;
                }

                int previousSize = GetConstantPushInstructionSizeAt(ncsData, previousStart);
                if (previousSize <= 0 || previousStart + previousSize != runStart)
                {
                    break;
                }

                runStart = previousStart;
            }

            int scanOffset = runStart;
            while (scanOffset + 2 <= ncsData.Length)
            {
                byte opcode = ncsData[scanOffset];
                byte qualifier = ncsData[scanOffset + 1];

                if (opcode == 0x04 && qualifier == 0x03)
                {
                    stackSlots.Add(new ActionStackSlot
                    {
                        IsIntConst = true,
                        ValueByteOffset = scanOffset + 2
                    });
                    scanOffset += 6;
                    continue;
                }

                if (opcode == 0x04 && (qualifier == 0x04 || qualifier == 0x06))
                {
                    stackSlots.Add(new ActionStackSlot { IsIntConst = false, ValueByteOffset = -1 });
                    scanOffset += GetConstantPushInstructionSizeAt(ncsData, scanOffset);
                    continue;
                }

                if (opcode == 0x04 && qualifier == 0x05)
                {
                    stackSlots.Add(new ActionStackSlot { IsIntConst = false, ValueByteOffset = -1 });
                    scanOffset += GetConstantPushInstructionSizeAt(ncsData, scanOffset);
                    continue;
                }

                if (IsStackSpillOrLoadOpcode(opcode))
                {
                    return false;
                }

                if (opcode == 0x05 && scanOffset + 4 <= ncsData.Length)
                {
                    actionId = (ncsData[scanOffset + 2] << 8) | ncsData[scanOffset + 3];
                    return StrRefParamIndicesByActionId.ContainsKey(actionId);
                }

                break;
            }

            return false;
        }

        private static bool IsConstantPushAt(byte[] ncsData, int opcodeOffset)
        {
            return GetConstantPushInstructionSizeAt(ncsData, opcodeOffset) > 0;
        }

        private static int GetConstantPushInstructionSizeAt(byte[] ncsData, int opcodeOffset)
        {
            if (opcodeOffset + 2 > ncsData.Length)
            {
                return 0;
            }

            byte opcode = ncsData[opcodeOffset];
            byte qualifier = ncsData[opcodeOffset + 1];
            if (opcode != 0x04)
            {
                return 0;
            }

            if (qualifier == 0x03 || qualifier == 0x04 || qualifier == 0x06)
            {
                return 6;
            }

            if (qualifier == 0x05)
            {
                if (opcodeOffset + 4 > ncsData.Length)
                {
                    return 0;
                }

                ushort strLen = (ushort)((ncsData[opcodeOffset + 2] << 8) | ncsData[opcodeOffset + 3]);
                return 4 + strLen;
            }

            return 0;
        }

        private static bool IsConstiAtStrRefParameterSlot(int actionId, int valueByteOffset, List<ActionStackSlot> stackSlots)
        {
            int[] strRefParamIndices;
            if (!StrRefParamIndicesByActionId.TryGetValue(actionId, out strRefParamIndices)
                || strRefParamIndices.Length == 0
                || stackSlots.Count == 0)
            {
                return false;
            }

            int slotIndex = -1;
            for (int i = 0; i < stackSlots.Count; i++)
            {
                if (stackSlots[i].IsIntConst && stackSlots[i].ValueByteOffset == valueByteOffset)
                {
                    slotIndex = i;
                    break;
                }
            }

            if (slotIndex < 0)
            {
                return false;
            }

            int paramIndex = slotIndex;
            for (int i = 0; i < strRefParamIndices.Length; i++)
            {
                if (strRefParamIndices[i] == paramIndex)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool ShouldIndexAsStrRefCandidate(byte[] ncsData, ConstiInstruction instruction, int minimum)
        {
            if (instruction.Value < 0)
            {
                return false;
            }

            ConstiUsageContext context = GetConstiUsageContext(ncsData, instruction);
            if (context == ConstiUsageContext.GenericInteger || context == ConstiUsageContext.StackStored)
            {
                return false;
            }

            if (context == ConstiUsageContext.StrRefConsumer)
            {
                return true;
            }

            return IsPlausibleStrRefCandidate(instruction.Value, minimum);
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

        private static bool IsStackStoreOpcode(byte opcode)
        {
            return opcode == 0x01 || opcode == 0x26;
        }

        private static bool IsStackSpillOrLoadOpcode(byte opcode)
        {
            return opcode == 0x01 || opcode == 0x03 || opcode == 0x26 || opcode == 0x27;
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
