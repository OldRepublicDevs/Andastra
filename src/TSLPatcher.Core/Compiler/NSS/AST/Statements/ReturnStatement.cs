using System.Collections.Generic;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Compiler.NSS.AST.Statements;

/// <summary>
/// Represents a return statement.
/// </summary>
public class ReturnStatement : Statement
{
    public Expression? Expression { get; set; }

    public ReturnStatement(Expression? expression = null)
    {
        Expression = expression;
    }

    public override void Compile(NCS ncs, CodeRoot root, CodeBlock block)
    {
        if (Expression != null)
        {
            DynamicDataType returnType = Expression.Compile(ncs, root, block);
            block.TempStack += returnType.Size(root);
            // Value is already on stack for return
        }

        ncs.Add(NCSInstructionType.RETN, new List<object>());
    }

    public override string ToString()
    {
        return Expression != null ? $"return {Expression};" : "return;";
    }
}

