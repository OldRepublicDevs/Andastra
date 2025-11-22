using System.Collections.Generic;
using TSLPatcher.Core.Common.Script;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Compiler.NSS.AST.Statements;

/// <summary>
/// Represents a for loop statement.
/// </summary>
public class ForStatement : Statement
{
    public Expression Initializer { get; set; }
    public Expression Condition { get; set; }
    public Expression Iterator { get; set; }
    public CodeBlock Body { get; set; }
    public List<BreakStatement> BreakStatements { get; set; }
    public List<ContinueStatement> ContinueStatements { get; set; }

    public ForStatement(Expression initializer, Expression condition, Expression iterator, CodeBlock body)
    {
        Initializer = initializer ?? throw new System.ArgumentNullException(nameof(initializer));
        Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
        Iterator = iterator ?? throw new System.ArgumentNullException(nameof(iterator));
        Body = body ?? throw new System.ArgumentNullException(nameof(body));
        BreakStatements = new List<BreakStatement>();
        ContinueStatements = new List<ContinueStatement>();
    }

    public override void Compile(NCS ncs, CodeRoot root, CodeBlock block)
    {
        // Compile initializer
        DynamicDataType initialType = Initializer.Compile(ncs, root, block);
        
        // Pop initializer result off stack (it's not needed)
        if (initialType.Size(root) > 0)
        {
            ncs.Add(NCSInstructionType.MOVSP, new List<object> { -initialType.Size(root) });
        }

        // Loop start (condition evaluation point)
        NCSInstruction loopStart = ncs.Add(NCSInstructionType.NOP, new List<object>());

        // Compile condition
        DynamicDataType conditionType = Condition.Compile(ncs, root, block);
        
        if (conditionType.Builtin != DataType.Int)
        {
            throw new CompileError(
                $"for loop condition must be int type, got {conditionType.Builtin.ToScriptString()}\n" +
                "  Note: Conditions must evaluate to int (0=false, non-zero=true)");
        }
        
        block.TempStack += 4;

        // If condition is false, jump to end
        NCSInstruction jumpToEnd = ncs.Add(NCSInstructionType.JZ, new List<object>());
        block.TempStack -= 4;

        // Compile loop body
        Body.Compile(ncs, root);

        // Update/iteration point (continue jumps here)
        NCSInstruction updateStart = ncs.Add(NCSInstructionType.NOP, new List<object>());

        // Compile iterator
        DynamicDataType iteratorType = Iterator.Compile(ncs, root, block);
        
        // Pop iterator result off stack
        if (iteratorType.Size(root) > 0)
        {
            ncs.Add(NCSInstructionType.MOVSP, new List<object> { -iteratorType.Size(root) });
        }

        // Jump back to loop start (condition re-evaluation)
        NCSInstruction jumpToStart = ncs.Add(NCSInstructionType.JMP, new List<object>());
        jumpToStart.Jump = loopStart;

        // Loop end marker
        NCSInstruction loopEnd = ncs.Add(NCSInstructionType.NOP, new List<object>());
        jumpToEnd.Jump = loopEnd;

        // Patch all break statements to jump to loop end
        foreach (BreakStatement breakStmt in BreakStatements)
        {
            breakStmt.JumpTarget = loopEnd;
        }

        // Patch all continue statements to jump to update/iteration
        foreach (ContinueStatement continueStmt in ContinueStatements)
        {
            continueStmt.JumpTarget = updateStart;
        }
    }

    public override string ToString()
    {
        return $"for ({Initializer}; {Condition}; {Iterator}) {{ ... }}";
    }
}

