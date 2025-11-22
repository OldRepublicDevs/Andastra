using System.Collections.Generic;
using TSLPatcher.Core.Common.Script;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Compiler.NSS.AST.Statements;

/// <summary>
/// Represents an if/else statement.
/// </summary>
public class IfStatement : Statement
{
    public Expression Condition { get; set; }
    public CodeBlock ThenBlock { get; set; }
    public CodeBlock? ElseBlock { get; set; }

    public IfStatement(Expression condition, CodeBlock thenBlock, CodeBlock? elseBlock = null)
    {
        Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
        ThenBlock = thenBlock ?? throw new System.ArgumentNullException(nameof(thenBlock));
        ElseBlock = elseBlock;
    }

    public override void Compile(NCS ncs, CodeRoot root, CodeBlock block)
    {
        // Compile condition
        DynamicDataType conditionType = Condition.Compile(ncs, root, block);
        
        if (conditionType.Builtin != DataType.Int)
        {
            throw new CompileError(
                $"Condition must be int type, got {conditionType.Builtin.ToScriptString()}\n" +
                "  Note: In NWScript, conditions must evaluate to int (0=false, non-zero=true)");
        }
        
        block.TempStack += 4;

        if (ElseBlock != null)
        {
            // If condition is false (zero), jump to else block
            NCSInstruction jumpToElse = ncs.Add(NCSInstructionType.JZ, new List<object>());
            block.TempStack -= 4;

            // Compile then block
            ThenBlock.Compile(ncs, root);

            // Jump over else block after then completes
            NCSInstruction jumpToEnd = ncs.Add(NCSInstructionType.JMP, new List<object>());

            // Else block starts here
            NCSInstruction elseStart = ncs.Add(NCSInstructionType.NOP, new List<object>());
            jumpToElse.Jump = elseStart;

            // Compile else block
            ElseBlock.Compile(ncs, root);

            // End marker
            NCSInstruction endMarker = ncs.Add(NCSInstructionType.NOP, new List<object>());
            jumpToEnd.Jump = endMarker;
        }
        else
        {
            // If condition is false (zero), jump past then block
            NCSInstruction jumpToEnd = ncs.Add(NCSInstructionType.JZ, new List<object>());
            block.TempStack -= 4;

            // Compile then block
            ThenBlock.Compile(ncs, root);

            // End marker
            NCSInstruction endMarker = ncs.Add(NCSInstructionType.NOP, new List<object>());
            jumpToEnd.Jump = endMarker;
        }
    }

    public override string ToString()
    {
        if (ElseBlock != null)
        {
            return $"if ({Condition}) {{ ... }} else {{ ... }}";
        }
        return $"if ({Condition}) {{ ... }}";
    }
}

