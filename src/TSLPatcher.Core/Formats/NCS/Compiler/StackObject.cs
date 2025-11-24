using System;
using TSLPatcher.Core.Common.Script;

namespace TSLPatcher.Core.Formats.NCS.Compiler;

/// <summary>
/// Represents a single value on the NCS execution stack.
/// 1:1 port from Python StackObject class in pykotor/resource/formats/ncs/compiler/interpreter.py
/// </summary>
public class StackObject
{
    public DataType DataType { get; set; }
    public object? Value { get; set; }

    public StackObject(DataType dataType, object? value)
    {
        DataType = dataType;
        Value = value;
    }

    public override string ToString()
    {
        return $"{DataType}={Value}";
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is StackObject other)
        {
            return Value?.Equals(other.Value) ?? other.Value == null;
        }
        return Value?.Equals(obj) ?? obj == null;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(DataType, Value?.GetHashCode() ?? 0);
    }
}

