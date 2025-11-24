using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace TSLPatcher.Core.Formats.NCS
{

    /// <summary>
    /// Represents a single NCS bytecode instruction.
    /// 
    /// Each instruction consists of a bytecode opcode, a qualifier specifying operand types,
    /// optional arguments (offsets, constants, etc.), and an optional jump target for
    /// control flow instructions.
    /// 
    /// References:
    ///     vendor/reone/include/reone/script/program.h - Instruction struct
    ///     vendor/reone/src/libs/script/format/ncsreader.cpp:48-190 (instruction reading)
    ///     vendor/xoreos/src/aurora/nwscript/ncsfile.h:131-177 (InstructionType enum)
    ///     vendor/xoreos/src/aurora/nwscript/ncsfile.cpp:194-1649 (instruction execution)
    ///     vendor/Kotor.NET/Kotor.NET/Formats/KotorNCS/NCS.cs:19-711 - NCSInstruction classes
    /// </summary>
    public class NCSInstruction : IEquatable<NCSInstruction>
    {
        public NCSInstructionType InsType { get; set; }
        public List<object> Args { get; set; }
        [CanBeNull] public NCSInstruction Jump { get; set; }
        public int Offset { get; set; }

        public NCSInstruction(
            NCSInstructionType insType = NCSInstructionType.NOP,
            [CanBeNull] List<object> args = null,
            [CanBeNull] NCSInstruction jump = null)
        {
            InsType = insType;
            Args = args ?? new List<object>();
            Jump = jump;
            Offset = -1;
        }

        public bool IsJumpInstruction()
        {
            return InsType == NCSInstructionType.JMP
                || InsType == NCSInstructionType.JSR
                || InsType == NCSInstructionType.JZ
                || InsType == NCSInstructionType.JNZ;
        }

        public bool IsStackOperation()
        {
            return InsType == NCSInstructionType.CPDOWNSP
                || InsType == NCSInstructionType.CPTOPSP
                || InsType == NCSInstructionType.CPDOWNBP
                || InsType == NCSInstructionType.CPTOPBP
                || InsType == NCSInstructionType.MOVSP
                || InsType == NCSInstructionType.RSADDI
                || InsType == NCSInstructionType.RSADDF
                || InsType == NCSInstructionType.RSADDS
                || InsType == NCSInstructionType.RSADDO
                || InsType == NCSInstructionType.RSADDEFF
                || InsType == NCSInstructionType.RSADDEVT
                || InsType == NCSInstructionType.RSADDLOC
                || InsType == NCSInstructionType.RSADDTAL;
        }

        public bool IsConstant()
        {
            return InsType == NCSInstructionType.CONSTI
                || InsType == NCSInstructionType.CONSTF
                || InsType == NCSInstructionType.CONSTS
                || InsType == NCSInstructionType.CONSTO;
        }

        public bool IsArithmetic()
        {
            return InsType == NCSInstructionType.ADDII
                || InsType == NCSInstructionType.ADDIF
                || InsType == NCSInstructionType.ADDFI
                || InsType == NCSInstructionType.ADDFF
                || InsType == NCSInstructionType.ADDSS
                || InsType == NCSInstructionType.ADDVV
                || InsType == NCSInstructionType.SUBII
                || InsType == NCSInstructionType.SUBIF
                || InsType == NCSInstructionType.SUBFI
                || InsType == NCSInstructionType.SUBFF
                || InsType == NCSInstructionType.SUBVV
                || InsType == NCSInstructionType.MULII
                || InsType == NCSInstructionType.MULIF
                || InsType == NCSInstructionType.MULFI
                || InsType == NCSInstructionType.MULFF
                || InsType == NCSInstructionType.MULVF
                || InsType == NCSInstructionType.MULFV
                || InsType == NCSInstructionType.DIVII
                || InsType == NCSInstructionType.DIVIF
                || InsType == NCSInstructionType.DIVFI
                || InsType == NCSInstructionType.DIVFF
                || InsType == NCSInstructionType.DIVVF
                || InsType == NCSInstructionType.DIVFV
                || InsType == NCSInstructionType.MODII
                || InsType == NCSInstructionType.NEGI
                || InsType == NCSInstructionType.NEGF;
        }

        public bool IsComparison()
        {
            return InsType == NCSInstructionType.EQUALII
                || InsType == NCSInstructionType.EQUALFF
                || InsType == NCSInstructionType.EQUALSS
                || InsType == NCSInstructionType.EQUALOO
                || InsType == NCSInstructionType.EQUALTT
                || InsType == NCSInstructionType.NEQUALII
                || InsType == NCSInstructionType.NEQUALFF
                || InsType == NCSInstructionType.NEQUALSS
                || InsType == NCSInstructionType.NEQUALOO
                || InsType == NCSInstructionType.NEQUALTT
                || InsType == NCSInstructionType.GTII
                || InsType == NCSInstructionType.GTFF
                || InsType == NCSInstructionType.GEQII
                || InsType == NCSInstructionType.GEQFF
                || InsType == NCSInstructionType.LTII
                || InsType == NCSInstructionType.LTFF
                || InsType == NCSInstructionType.LEQII
                || InsType == NCSInstructionType.LEQFF;
        }

        public bool IsLogical()
        {
            return InsType == NCSInstructionType.LOGANDII
                || InsType == NCSInstructionType.LOGORII
                || InsType == NCSInstructionType.NOTI;
        }

        public bool IsBitwise()
        {
            return InsType == NCSInstructionType.BOOLANDII
                || InsType == NCSInstructionType.INCORII
                || InsType == NCSInstructionType.EXCORII
                || InsType == NCSInstructionType.COMPI
                || InsType == NCSInstructionType.SHLEFTII
                || InsType == NCSInstructionType.SHRIGHTII
                || InsType == NCSInstructionType.USHRIGHTII;
        }

        public bool IsControlFlow()
        {
            return InsType == NCSInstructionType.JMP
                || InsType == NCSInstructionType.JSR
                || InsType == NCSInstructionType.JZ
                || InsType == NCSInstructionType.JNZ
                || InsType == NCSInstructionType.RETN;
        }

        public bool IsFunctionCall()
        {
            return InsType == NCSInstructionType.ACTION;
        }

        public bool Equals([CanBeNull] NCSInstruction other)
        {
            if (other is null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return InsType == other.InsType
                && Args.SequenceEqual(other.Args)
                && ReferenceEquals(Jump, other.Jump);
        }

        public override bool Equals([CanBeNull] object obj)
        {
            if (obj is NCSInstruction other)
            {
                return Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(InsType, Args.Count, Jump?.GetHashCode() ?? 0);
        }

        public override string ToString()
        {
            string argsStr = Args.Count > 0 ? $" args=[{string.Join(", ", Args)}]" : "";
            string jumpStr = Jump != null ? $" jump=<{Jump.GetHashCode()}>" : "";
            return $"{InsType}{argsStr}{jumpStr}";
        }
    }
}

