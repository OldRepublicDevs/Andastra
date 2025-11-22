using TSLPatcher.Core.Common.Script;
using TSLPatcher.Core.Formats.NCS;
using System.Collections.Generic;

namespace TSLPatcher.Core.Compiler.NSS.AST.Expressions;

/// <summary>
/// Represents an object literal expression (OBJECT_SELF, OBJECT_INVALID, or object ID).
/// </summary>
public class ObjectExpression : Expression
{
    public int Value { get; set; }

    public ObjectExpression(int value)
    {
        Value = value;
    }

    public override DynamicDataType Compile(NCS ncs, CodeRoot root, CodeBlock block)
    {
        ncs.Add(NCSInstructionType.CONSTO, new List<object> { Value });
        return new DynamicDataType(DataType.Object);
    }

    public override string ToString()
    {
        return Value switch
        {
            0 => "OBJECT_SELF",
            1 => "OBJECT_INVALID",
            _ => $"Object({Value})"
        };
    }
}

