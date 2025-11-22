using System;
using TSLPatcher.Core.Common.Script;

namespace TSLPatcher.Core.Compiler.NSS.AST;

/// <summary>
/// Represents a data type that can be either a built-in type or a user-defined struct.
/// </summary>
public class DynamicDataType : IEquatable<DynamicDataType>
{
    public DataType Builtin { get; set; }
    public string? Struct { get; set; }

    public DynamicDataType(DataType builtin, string? structName = null)
    {
        Builtin = builtin;
        Struct = structName;
    }

    public int Size(CodeRoot root)
    {
        if (Builtin == DataType.Struct && Struct != null)
        {
            if (root.StructMap.TryGetValue(Struct, out Struct? structDef))
            {
                return structDef.Size(root);
            }
            throw new CompileError($"Unknown struct type: {Struct}");
        }
        return Builtin.GetSize();
    }

    public bool Equals(DynamicDataType? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (Builtin != other.Builtin) return false;
        if (Builtin == DataType.Struct)
        {
            return Struct == other.Struct;
        }
        return true;
    }

    public override bool Equals(object? obj)
    {
        if (obj is DynamicDataType dt) return Equals(dt);
        if (obj is DataType t) return Builtin == t && Builtin != DataType.Struct;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Builtin, Struct);
    }

    public override string ToString()
    {
        if (Builtin == DataType.Struct && Struct != null)
        {
            return $"struct {Struct}";
        }
        return Builtin.ToScriptString();
    }

    public static bool operator ==(DynamicDataType? left, DynamicDataType? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(DynamicDataType? left, DynamicDataType? right)
    {
        return !Equals(left, right);
    }

    public static bool operator ==(DynamicDataType? left, DataType right)
    {
        return left?.Builtin == right && left?.Builtin != DataType.Struct;
    }

    public static bool operator !=(DynamicDataType? left, DataType right)
    {
        return !(left == right);
    }

    public static implicit operator DynamicDataType(DataType dataType)
    {
        return new DynamicDataType(dataType);
    }
}

