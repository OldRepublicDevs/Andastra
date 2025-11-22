using System;
using System.Collections.Generic;
using System.Linq;

namespace TSLPatcher.Core.Formats.NCS;

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
    public NCSInstruction? Jump { get; set; }
    public int Offset { get; set; }

    public NCSInstruction(
        NCSInstructionType insType = NCSInstructionType.NOP,
        List<object>? args = null,
        NCSInstruction? jump = null)
    {
        InsType = insType;
        Args = args ?? new List<object>();
        Jump = jump;
        Offset = -1;
    }

    public bool IsJumpInstruction()
    {
        return InsType is NCSInstructionType.JMP
            or NCSInstructionType.JSR
            or NCSInstructionType.JZ
            or NCSInstructionType.JNZ;
    }

    public bool IsStackOperation()
    {
        return InsType is NCSInstructionType.CPDOWNSP
            or NCSInstructionType.CPTOPSP
            or NCSInstructionType.CPDOWNBP
            or NCSInstructionType.CPTOPBP
            or NCSInstructionType.MOVSP
            or NCSInstructionType.RSADDI
            or NCSInstructionType.RSADDF
            or NCSInstructionType.RSADDS
            or NCSInstructionType.RSADDO
            or NCSInstructionType.RSADDEFF
            or NCSInstructionType.RSADDEVT
            or NCSInstructionType.RSADDLOC
            or NCSInstructionType.RSADDTAL;
    }

    public bool IsConstant()
    {
        return InsType is NCSInstructionType.CONSTI
            or NCSInstructionType.CONSTF
            or NCSInstructionType.CONSTS
            or NCSInstructionType.CONSTO;
    }

    public bool IsArithmetic()
    {
        return InsType is NCSInstructionType.ADDII or NCSInstructionType.ADDIF or NCSInstructionType.ADDFI
            or NCSInstructionType.ADDFF or NCSInstructionType.ADDSS or NCSInstructionType.ADDVV
            or NCSInstructionType.SUBII or NCSInstructionType.SUBIF or NCSInstructionType.SUBFI
            or NCSInstructionType.SUBFF or NCSInstructionType.SUBVV
            or NCSInstructionType.MULII or NCSInstructionType.MULIF or NCSInstructionType.MULFI
            or NCSInstructionType.MULFF or NCSInstructionType.MULVF or NCSInstructionType.MULFV
            or NCSInstructionType.DIVII or NCSInstructionType.DIVIF or NCSInstructionType.DIVFI
            or NCSInstructionType.DIVFF or NCSInstructionType.DIVVF or NCSInstructionType.DIVFV
            or NCSInstructionType.MODII
            or NCSInstructionType.NEGI or NCSInstructionType.NEGF;
    }

    public bool IsComparison()
    {
        return InsType is NCSInstructionType.EQUALII or NCSInstructionType.EQUALFF or NCSInstructionType.EQUALSS
            or NCSInstructionType.EQUALOO or NCSInstructionType.EQUALTT
            or NCSInstructionType.NEQUALII or NCSInstructionType.NEQUALFF or NCSInstructionType.NEQUALSS
            or NCSInstructionType.NEQUALOO or NCSInstructionType.NEQUALTT
            or NCSInstructionType.GTII or NCSInstructionType.GTFF
            or NCSInstructionType.GEQII or NCSInstructionType.GEQFF
            or NCSInstructionType.LTII or NCSInstructionType.LTFF
            or NCSInstructionType.LEQII or NCSInstructionType.LEQFF;
    }

    public bool IsLogical()
    {
        return InsType is NCSInstructionType.LOGANDII
            or NCSInstructionType.LOGORII
            or NCSInstructionType.NOTI;
    }

    public bool IsBitwise()
    {
        return InsType is NCSInstructionType.BOOLANDII
            or NCSInstructionType.INCORII
            or NCSInstructionType.EXCORII
            or NCSInstructionType.COMPI
            or NCSInstructionType.SHLEFTII
            or NCSInstructionType.SHRIGHTII
            or NCSInstructionType.USHRIGHTII;
    }

    public bool IsControlFlow()
    {
        return InsType is NCSInstructionType.JMP
            or NCSInstructionType.JSR
            or NCSInstructionType.JZ
            or NCSInstructionType.JNZ
            or NCSInstructionType.RETN;
    }

    public bool IsFunctionCall()
    {
        return InsType == NCSInstructionType.ACTION;
    }

    public bool Equals(NCSInstruction? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return InsType == other.InsType
            && Args.SequenceEqual(other.Args)
            && ReferenceEquals(Jump, other.Jump);
    }

    public override bool Equals(object? obj)
    {
        return obj is NCSInstruction other && Equals(other);
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

