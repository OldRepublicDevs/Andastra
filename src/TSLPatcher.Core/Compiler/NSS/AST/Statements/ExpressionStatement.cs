using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Compiler.NSS.AST.Statements;

/// <summary>
/// Represents an expression used as a statement (e.g., function call, assignment).
/// </summary>
public class ExpressionStatement : Statement
{
    public Expression Expression { get; set; }

    public ExpressionStatement(Expression expression)
    {
        Expression = expression ?? throw new System.ArgumentNullException(nameof(expression));
    }

    public override void Compile(NCS ncs, CodeRoot root, CodeBlock block)
    {
        DynamicDataType resultType = Expression.Compile(ncs, root, block);
        
        // Pop the result off the stack if it produced one
        int resultSize = resultType.Size(root);
        if (resultSize > 0)
        {
            ncs.Add(NCSInstructionType.MOVSP, new System.Collections.Generic.List<object> { -resultSize });
            block.TempStack -= resultSize;
        }
    }

    public override string ToString()
    {
        return $"{Expression};";
    }
}

