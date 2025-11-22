using System.Collections.Generic;
using TSLPatcher.Core.Common.Script;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Compiler.NSS.AST.Statements;

/// <summary>
/// Represents a while loop statement.
/// </summary>
public class WhileStatement : Statement
{
    public Expression Condition { get; set; }
    public CodeBlock Body { get; set; }
    public List<BreakStatement> BreakStatements { get; set; }
    public List<ContinueStatement> ContinueStatements { get; set; }

    public WhileStatement(Expression condition, CodeBlock body)
    {
        Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
        Body = body ?? throw new System.ArgumentNullException(nameof(body));
        BreakStatements = new List<BreakStatement>();
        ContinueStatements = new List<ContinueStatement>();
    }

    public override void Compile(NCS ncs, CodeRoot root, CodeBlock block)
    {
        // Loop start (condition evaluation point)
        NCSInstruction loopStart = ncs.Add(NCSInstructionType.NOP, new List<object>());

        // Compile condition
        DynamicDataType conditionType = Condition.Compile(ncs, root, block);
        
        if (conditionType.Builtin != DataType.Int)
        {
            throw new CompileError(
                $"While condition must be int type, got {conditionType.Builtin.ToScriptString()}\n" +
                "  Note: In NWScript, conditions must evaluate to int (0=false, non-zero=true)");
        }
        
        block.TempStack += 4;

        // If condition is false, jump to end
        NCSInstruction jumpToEnd = ncs.Add(NCSInstructionType.JZ, new List<object>());
        block.TempStack -= 4;

        // Compile loop body
        Body.Compile(ncs, root);

        // Jump back to loop start
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

        // Patch all continue statements to jump to loop start (condition re-evaluation)
        foreach (ContinueStatement continueStmt in ContinueStatements)
        {
            continueStmt.JumpTarget = loopStart;
        }
    }

    public override string ToString()
    {
        return $"while ({Condition}) {{ ... }}";
    }
}

