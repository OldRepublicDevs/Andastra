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
                int storeOffset;
                int storeSize;
                if (TryReadStackCopyOperands(ncsData, nextOffset, out storeOffset, out storeSize)
                    && TryFindStrRefConsumerViaStackReload(ncsData, instruction, nextOffset, storeOffset, storeSize))
                {
                    return ConstiUsageContext.StrRefConsumer;
                }

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

        private static bool TryReadStackCopyOperands(byte[] ncsData, int opcodeOffset, out int stackOffset, out int copySize)
        {
            stackOffset = 0;
            copySize = 0;
            if (opcodeOffset + 8 > ncsData.Length)
            {
                return false;
            }

            byte opcode = ncsData[opcodeOffset];
            if (!IsStackStoreOpcode(opcode) && opcode != 0x03 && opcode != 0x27)
            {
                return false;
            }

            stackOffset = (ncsData[opcodeOffset + 2] << 24)
                | (ncsData[opcodeOffset + 3] << 16)
                | (ncsData[opcodeOffset + 4] << 8)
                | ncsData[opcodeOffset + 5];
            copySize = (ncsData[opcodeOffset + 6] << 8) | ncsData[opcodeOffset + 7];
            return true;
        }

        private const int VariableStrRefForwardScanLimitBytes = 128;
        private const int RelayStoreDiscoveryLimitBytes = 32;

        private static bool TryFindNextStackStoreAfterLoad(
            byte[] ncsData,
            int afterLoadOpcodeOffset,
            int scanLimit,
            out int storeOpcodeOffset,
            out int storeOffset,
            out int storeSize)
        {
            storeOpcodeOffset = 0;
            storeOffset = 0;
            storeSize = 0;

            int loadInstructionSize = GetInstructionSizeAt(ncsData, afterLoadOpcodeOffset);
            if (loadInstructionSize <= 0)
            {
                return false;
            }

            int offset = afterLoadOpcodeOffset + loadInstructionSize;
            int limit = Math.Min(scanLimit, afterLoadOpcodeOffset + RelayStoreDiscoveryLimitBytes);
            while (offset + 8 <= limit)
            {
                byte opcode = ncsData[offset];
                if (opcode == 0x01 || opcode == 0x26)
                {
                    if (TryReadStackCopyOperands(ncsData, offset, out storeOffset, out storeSize)
                        && storeSize == 4)
                    {
                        storeOpcodeOffset = offset;
                        return true;
                    }

                    return false;
                }

                int instructionSize = GetInstructionSizeAt(ncsData, offset);
                if (instructionSize <= 0)
                {
                    break;
                }

                offset += instructionSize;
            }

            return false;
        }

        private static bool TryFindNextCpdownspAfterLoad(
            byte[] ncsData,
            int afterLoadOpcodeOffset,
            int scanLimit,
            out int storeOpcodeOffset,
            out int storeOffset,
            out int storeSize)
        {
            return TryFindNextStackStoreAfterLoad(
                ncsData,
                afterLoadOpcodeOffset,
                scanLimit,
                out storeOpcodeOffset,
                out storeOffset,
                out storeSize);
        }

        private static int TryResolveJumpTarget(byte[] ncsData, int opcodeOffset)
        {
            if (opcodeOffset + 6 > ncsData.Length)
            {
                return -1;
            }

            int relative = (ncsData[opcodeOffset + 2] << 24)
                | (ncsData[opcodeOffset + 3] << 16)
                | (ncsData[opcodeOffset + 4] << 8)
                | ncsData[opcodeOffset + 5];
            return relative + opcodeOffset;
        }

        private static bool TryFindStrRefConsumerViaStackReload(
            byte[] ncsData,
            ConstiInstruction storedConsti,
            int storeOpcodeOffset,
            int storeOffset,
            int storeSize)
        {
            if (storeSize != 4)
            {
                return false;
            }

            int scanLimit = Math.Min(ncsData.Length, storeOpcodeOffset + 8 + VariableStrRefForwardScanLimitBytes);
            byte storeOpcode = ncsData[storeOpcodeOffset];
            if (TryFindStrRefConsumerViaStackReloadFromScan(
                    ncsData,
                    storedConsti,
                    storeOffset,
                    storeSize,
                    storeOpcodeOffset + 8,
                    scanLimit,
                    0))
            {
                return true;
            }

            if (storeOpcode == 0x26)
            {
                return TryFindStrRefConsumerViaBpReload(ncsData, storedConsti, storeOffset, storeSize);
            }

            return false;
        }

        private static int FindPreviousInstructionStart(byte[] ncsData, int opcodeOffset)
        {
            if (opcodeOffset <= 13)
            {
                return -1;
            }

            for (int candidate = opcodeOffset - 2; candidate >= 13; candidate--)
            {
                int size = GetInstructionSizeAt(ncsData, candidate);
                if (size > 0 && candidate + size == opcodeOffset)
                {
                    return candidate;
                }
            }

            return -1;
        }

        private static bool TryReadConstIntImmediatelyBeforeInstruction(byte[] ncsData, int instructionOffset, out int constValue)
        {
            constValue = 0;
            if (instructionOffset <= 13)
            {
                return false;
            }

            int offset = instructionOffset;
            for (int step = 0; step < 6 && offset > 13; step++)
            {
                int foundStart = FindPreviousInstructionStart(ncsData, offset);
                if (foundStart < 0)
                {
                    return false;
                }

                byte opcode = ncsData[foundStart];
                byte qualifier = ncsData[foundStart + 1];
                if (opcode == 0x04 && qualifier == 0x03 && foundStart + 6 <= instructionOffset)
                {
                    constValue = (ncsData[foundStart + 2] << 24)
                        | (ncsData[foundStart + 3] << 16)
                        | (ncsData[foundStart + 4] << 8)
                        | ncsData[foundStart + 5];
                    return true;
                }

                if (opcode == 0x1B || opcode == 0x23 || opcode == 0x24 || opcode == 0x2D || opcode == 0x02)
                {
                    offset = foundStart;
                    continue;
                }

                return false;
            }

            return false;
        }

        private static bool TryReadConstIntFromLocalLoad(byte[] ncsData, int cptopspOffset, out int constValue)
        {
            constValue = 0;
            int loadOffset;
            int loadSize;
            if (!TryReadStackCopyOperands(ncsData, cptopspOffset, out loadOffset, out loadSize) || loadSize != 4)
            {
                return false;
            }

            int offset = cptopspOffset;
            int stackPointerDelta = 0;
            for (int step = 0; step < 12 && offset > 13; step++)
            {
                int foundStart = FindPreviousInstructionStart(ncsData, offset);
                if (foundStart < 0)
                {
                    return false;
                }

                byte opcode = ncsData[foundStart];
                if (opcode == 0x01)
                {
                    int storeOffset;
                    int storeSize;
                    if (TryReadStackCopyOperands(ncsData, foundStart, out storeOffset, out storeSize)
                        && storeSize == loadSize
                        && (storeOffset == loadOffset || storeOffset + stackPointerDelta == loadOffset)
                        && TryReadConstIntImmediatelyBeforeInstruction(ncsData, foundStart, out constValue))
                    {
                        return true;
                    }
                }

                if (opcode == 0x1B || opcode == 0x23 || opcode == 0x24)
                {
                    if (foundStart + 6 <= ncsData.Length)
                    {
                        int movOffset = (ncsData[foundStart + 2] << 24)
                            | (ncsData[foundStart + 3] << 16)
                            | (ncsData[foundStart + 4] << 8)
                            | ncsData[foundStart + 5];
                        stackPointerDelta -= movOffset;
                    }
                }

                offset = foundStart;
            }

            return false;
        }

        private static bool TryResolveJumpConditionConstInt(byte[] ncsData, int jumpOpcodeOffset, out int constValue)
        {
            if (TryReadConstIntImmediatelyBeforeInstruction(ncsData, jumpOpcodeOffset, out constValue))
            {
                return true;
            }

            int previousStart = FindPreviousInstructionStart(ncsData, jumpOpcodeOffset);
            if (previousStart >= 0 && ncsData[previousStart] == 0x03)
            {
                return TryReadConstIntFromLocalLoad(ncsData, previousStart, out constValue);
            }

            return false;
        }

        private static bool ShouldForkConditionalJumpTarget(byte[] ncsData, int jumpOpcodeOffset, byte opcode)
        {
            int constValue;
            if (!TryResolveJumpConditionConstInt(ncsData, jumpOpcodeOffset, out constValue))
            {
                return false;
            }

            if (opcode == 0x1F)
            {
                return constValue == 0;
            }

            if (opcode == 0x25)
            {
                return constValue != 0;
            }

            return false;
        }

        private static bool IsReturnCleanupMovspAfterJump(byte[] ncsData, int jumpOpcodeOffset)
        {
            int jumpSize = GetInstructionSizeAt(ncsData, jumpOpcodeOffset);
            if (jumpSize <= 0)
            {
                return false;
            }

            int nextStart = jumpOpcodeOffset + jumpSize;
            if (nextStart + 6 > ncsData.Length || ncsData[nextStart] != 0x1B)
            {
                return false;
            }

            int movOffset = (ncsData[nextStart + 2] << 24)
                | (ncsData[nextStart + 3] << 16)
                | (ncsData[nextStart + 4] << 8)
                | ncsData[nextStart + 5];
            return movOffset < 0;
        }

        private static bool IsForwardLoopBreakJump(byte[] ncsData, int jumpTarget, int loopHead, int scanLimit)
        {
            int offset = jumpTarget;
            for (int i = 0; i < 24 && offset + 2 <= scanLimit; i++)
            {
                byte op = ncsData[offset];
                if (op == 0x20)
                {
                    return false;
                }

                if (op == 0x1D)
                {
                    int target = TryResolveJumpTarget(ncsData, offset);
                    if (target > 0 && target <= loopHead + 4)
                    {
                        return false;
                    }
                }

                int step = GetInstructionSizeAt(ncsData, offset);
                if (step <= 0)
                {
                    break;
                }

                offset += step;
            }

            return true;
        }

        private static bool HasForwardLoopExitJump(byte[] ncsData, int loopHead, int backEdgeOffset, int scanLimit)
        {
            int backEdgeSize = GetInstructionSizeAt(ncsData, backEdgeOffset);
            if (backEdgeSize <= 0)
            {
                return false;
            }

            int minExitTarget = backEdgeOffset + backEdgeSize;
            int offset = loopHead;
            while (offset < backEdgeOffset && offset + 2 <= scanLimit)
            {
                byte op = ncsData[offset];
                if (op == 0x1D)
                {
                    int target = TryResolveJumpTarget(ncsData, offset);
                    if (target >= minExitTarget
                        && target < scanLimit
                        && IsForwardLoopBreakJump(ncsData, target, loopHead, scanLimit))
                    {
                        return true;
                    }
                }

                int step = GetInstructionSizeAt(ncsData, offset);
                if (step <= 0)
                {
                    break;
                }

                offset += step;
            }

            return false;
        }

        private static bool ShouldContinueLinearAfterConditionalJump(byte[] ncsData, int jumpOpcodeOffset, byte opcode)
        {
            int constValue;
            if (!TryResolveJumpConditionConstInt(ncsData, jumpOpcodeOffset, out constValue))
            {
                return false;
            }

            if (opcode == 0x1F && constValue != 0)
            {
                return !IsReturnCleanupMovspAfterJump(ncsData, jumpOpcodeOffset);
            }

            if (opcode == 0x25 && constValue == 0)
            {
                return !IsReturnCleanupMovspAfterJump(ncsData, jumpOpcodeOffset);
            }

            return false;
        }

        private static bool TryFindStrRefConsumerViaStackReloadFromScan(
            byte[] ncsData,
            ConstiInstruction storedConsti,
            int storeOffset,
            int storeSize,
            int scanOffset,
            int scanLimit,
            int stackPointerDelta)
        {
            while (scanOffset + 8 <= scanLimit)
            {
                byte opcode = ncsData[scanOffset];
                if (opcode == 0x03 || opcode == 0x27)
                {
                    int loadOffset;
                    int loadSize;
                    if (TryReadStackCopyOperands(ncsData, scanOffset, out loadOffset, out loadSize)
                        && loadSize == storeSize
                        && (loadOffset == storeOffset || loadOffset + stackPointerDelta == storeOffset))
                    {
                        int actionId;
                        List<ActionStackSlot> stackSlots;
                        int argRunStart = FindActionArgumentRunStart(ncsData, scanOffset);
                        if (TryGetActionArgumentRunFrom(
                                ncsData,
                                argRunStart,
                                storedConsti.ValueByteOffset,
                                out actionId,
                                out stackSlots)
                            && IsConstiAtStrRefParameterSlot(actionId, storedConsti.ValueByteOffset, stackSlots))
                        {
                            return true;
                        }

                        if (opcode == 0x03 || opcode == 0x27)
                        {
                            int relayStoreOpcodeOffset;
                            int relayStoreOffset;
                            int relayStoreSize;
                            if (TryFindNextStackStoreAfterLoad(
                                    ncsData,
                                    scanOffset,
                                    scanLimit,
                                    out relayStoreOpcodeOffset,
                                    out relayStoreOffset,
                                    out relayStoreSize)
                                && TryFindStrRefConsumerViaStackReload(
                                    ncsData,
                                    storedConsti,
                                    relayStoreOpcodeOffset,
                                    relayStoreOffset,
                                    relayStoreSize))
                            {
                                return true;
                            }
                        }
                    }
                }

                if (opcode == 0x1D)
                {
                    int jumpTarget = TryResolveJumpTarget(ncsData, scanOffset);
                    // Forward JMP forks only — backward edges are loop back-edges and recurse indefinitely.
                    if (jumpTarget > scanOffset
                        && jumpTarget < scanLimit
                        && TryFindStrRefConsumerViaStackReloadFromScan(
                            ncsData,
                            storedConsti,
                            storeOffset,
                            storeSize,
                            jumpTarget,
                            scanLimit,
                            stackPointerDelta))
                    {
                        return true;
                    }

                    if (jumpTarget > 0
                        && jumpTarget <= scanOffset
                        && !HasForwardLoopExitJump(ncsData, jumpTarget, scanOffset, scanLimit))
                    {
                        break;
                    }
                }
                else if (opcode == 0x1F || opcode == 0x25)
                {
                    if (ShouldForkConditionalJumpTarget(ncsData, scanOffset, opcode))
                    {
                        int jumpTarget = TryResolveJumpTarget(ncsData, scanOffset);
                        if (jumpTarget > 0
                            && jumpTarget < scanLimit
                            && TryFindStrRefConsumerViaStackReloadFromScan(
                                ncsData,
                                storedConsti,
                                storeOffset,
                                storeSize,
                                jumpTarget,
                                scanLimit,
                                stackPointerDelta))
                        {
                            return true;
                        }
                    }

                    if (!ShouldContinueLinearAfterConditionalJump(ncsData, scanOffset, opcode))
                    {
                        break;
                    }
                }

                int instructionSize = GetInstructionSizeAt(ncsData, scanOffset);
                if (instructionSize <= 0)
                {
                    break;
                }

                if (opcode == 0x1B || opcode == 0x23 || opcode == 0x24)
                {
                    if (scanOffset + 6 <= ncsData.Length)
                    {
                        int movOffset = (ncsData[scanOffset + 2] << 24)
                            | (ncsData[scanOffset + 3] << 16)
                            | (ncsData[scanOffset + 4] << 8)
                            | ncsData[scanOffset + 5];
                        stackPointerDelta += movOffset;
                    }
                }

                scanOffset += instructionSize;
            }

            return false;
        }

        private static bool TryFindStrRefConsumerViaBpReload(
            byte[] ncsData,
            ConstiInstruction storedConsti,
            int storeOffset,
            int storeSize)
        {
            if (storeSize != 4 || ncsData == null || ncsData.Length < 21)
            {
                return false;
            }

            int scanLimit = ncsData.Length - 8;
            int scanOffset = 13;
            while (scanOffset + 8 <= scanLimit)
            {
                byte opcode = ncsData[scanOffset];
                if (opcode == 0x27)
                {
                    int loadOffset;
                    int loadSize;
                    if (TryReadStackCopyOperands(ncsData, scanOffset, out loadOffset, out loadSize)
                        && loadSize == storeSize
                        && loadOffset == storeOffset)
                    {
                        int actionId;
                        List<ActionStackSlot> stackSlots;
                        int argRunStart = FindActionArgumentRunStart(ncsData, scanOffset);
                        if (TryGetActionArgumentRunFrom(
                                ncsData,
                                argRunStart,
                                storedConsti.ValueByteOffset,
                                out actionId,
                                out stackSlots)
                            && IsConstiAtStrRefParameterSlot(actionId, storedConsti.ValueByteOffset, stackSlots))
                        {
                            return true;
                        }

                        int relayStoreOpcodeOffset;
                        int relayStoreOffset;
                        int relayStoreSize;
                        if (TryFindNextStackStoreAfterLoad(
                                ncsData,
                                scanOffset,
                                ncsData.Length,
                                out relayStoreOpcodeOffset,
                                out relayStoreOffset,
                                out relayStoreSize)
                            && TryFindStrRefConsumerViaStackReload(
                                ncsData,
                                storedConsti,
                                relayStoreOpcodeOffset,
                                relayStoreOffset,
                                relayStoreSize))
                        {
                            return true;
                        }
                    }
                }

                int instructionSize = GetInstructionStepSizeAt(ncsData, scanOffset);
                scanOffset += instructionSize;
            }

            return false;
        }

        private static int FindActionArgumentRunStart(byte[] ncsData, int loadOpcodeOffset)
        {
            int runStart = loadOpcodeOffset;
            while (runStart > 13)
            {
                int previousStart = runStart - 2;
                bool found = false;
                while (previousStart >= 13)
                {
                    int size = GetInstructionSizeAt(ncsData, previousStart);
                    if (size > 0 && previousStart + size == runStart)
                    {
                        found = true;
                        break;
                    }

                    previousStart--;
                }

                if (!found)
                {
                    break;
                }

                byte previousOpcode = ncsData[previousStart];
                if (IsStackStoreOpcode(previousOpcode) || previousOpcode == 0x1B || previousOpcode == 0x23 || previousOpcode == 0x24)
                {
                    break;
                }

                if (previousOpcode == 0x04 || previousOpcode == 0x03 || previousOpcode == 0x27)
                {
                    runStart = previousStart;
                    continue;
                }

                break;
            }

            return runStart;
        }

        private static bool TryGetActionArgumentRunFrom(
            byte[] ncsData,
            int runStart,
            int linkedConstiValueByteOffset,
            out int actionId,
            out List<ActionStackSlot> stackSlots)
        {
            actionId = -1;
            stackSlots = new List<ActionStackSlot>();

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

                if (opcode == 0x03 || opcode == 0x27)
                {
                    stackSlots.Add(new ActionStackSlot
                    {
                        IsIntConst = true,
                        ValueByteOffset = linkedConstiValueByteOffset
                    });
                    int loadSize;
                    int loadOffset;
                    if (!TryReadStackCopyOperands(ncsData, scanOffset, out loadOffset, out loadSize))
                    {
                        return false;
                    }

                    scanOffset += 8;
                    continue;
                }

                if (opcode == 0x02)
                {
                    int rsaddSize = GetInstructionSizeAt(ncsData, scanOffset);
                    if (rsaddSize <= 0)
                    {
                        return false;
                    }

                    scanOffset += rsaddSize;
                    continue;
                }

                if (opcode == 0x1B || opcode == 0x23 || opcode == 0x24)
                {
                    int movSize = GetInstructionSizeAt(ncsData, scanOffset);
                    if (movSize <= 0)
                    {
                        return false;
                    }

                    scanOffset += movSize;
                    continue;
                }

                if (IsStackSpillOrLoadOpcode(opcode) && opcode != 0x03 && opcode != 0x27)
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

        private static int GetInstructionSizeAt(byte[] ncsData, int opcodeOffset)
        {
            if (opcodeOffset + 2 > ncsData.Length)
            {
                return 0;
            }

            byte opcode = ncsData[opcodeOffset];
            byte qualifier = ncsData[opcodeOffset + 1];

            int constantSize = GetConstantPushInstructionSizeAt(ncsData, opcodeOffset);
            if (constantSize > 0)
            {
                return constantSize;
            }

            if (opcode == 0x01 || opcode == 0x03 || opcode == 0x26 || opcode == 0x27)
            {
                return 8;
            }

            if (opcode == 0x2C)
            {
                return 10;
            }

            if (opcode == 0x02)
            {
                return 2;
            }

            if (opcode == 0x1B || opcode == 0x1D || opcode == 0x1E || opcode == 0x1F ||
                opcode == 0x23 || opcode == 0x24 || opcode == 0x25 || opcode == 0x28 || opcode == 0x29)
            {
                return 6;
            }

            if (opcode == 0x20 || opcode == 0x22 || opcode == 0x2A || opcode == 0x2B)
            {
                return 2;
            }

            if (opcode == 0x2D)
            {
                return 2;
            }

            if (opcode == 0x05)
            {
                return 5;
            }

            if (opcode == 0x21)
            {
                return 8;
            }

            if ((opcode == 0x0B || opcode == 0x0C) && qualifier == 0x24)
            {
                return 4;
            }

            if (opcode >= 0x06 && opcode <= 0x18)
            {
                return 2;
            }

            if (opcode >= 0x0B && opcode <= 0x10)
            {
                return 2;
            }

            return 0;
        }

        private static int GetInstructionStepSizeAt(byte[] ncsData, int opcodeOffset)
        {
            int instructionSize = GetInstructionSizeAt(ncsData, opcodeOffset);
            if (instructionSize > 0)
            {
                return instructionSize;
            }

            // Match NCSActionPatcher: unknown opcode — minimal 2-byte step for full-file walks only.
            return 2;
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
