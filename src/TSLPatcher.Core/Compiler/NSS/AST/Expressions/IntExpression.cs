using TSLPatcher.Core.Common.Script;
using TSLPatcher.Core.Formats.NCS;
using System.Collections.Generic;

namespace TSLPatcher.Core.Compiler.NSS.AST.Expressions;

/// <summary>
/// Represents an integer literal expression.
/// </summary>
public class IntExpression : Expression
{
    public int Value { get; set; }

    public IntExpression(int value)
    {
        Value = value;
    }

    public override DynamicDataType Compile(NCS ncs, CodeRoot root, CodeBlock block)
    {
        ncs.Add(NCSInstructionType.CONSTI, new List<object> { Value });
        return new DynamicDataType(DataType.Int);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}

