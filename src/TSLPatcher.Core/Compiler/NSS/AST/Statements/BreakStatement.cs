using System.Collections.Generic;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Compiler.NSS.AST.Statements;

/// <summary>
/// Represents a break statement in a loop or switch.
/// </summary>
public class BreakStatement : Statement
{
    public NCSInstruction? JumpTarget { get; set; }

    public override void Compile(NCS ncs, CodeRoot root, CodeBlock block)
    {
        // Create jump instruction - target will be set by enclosing loop/switch
        NCSInstruction jumpInstruction = ncs.Add(NCSInstructionType.JMP, new List<object>());
        
        if (JumpTarget != null)
        {
            jumpInstruction.Jump = JumpTarget;
        }
        // If JumpTarget is null, it will be set later by the enclosing loop/switch context
    }

    public override string ToString()
    {
        return "break;";
    }
}

