using System.Collections.Generic;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Compiler.NSS.AST.Statements;

/// <summary>
/// Represents a continue statement in a loop.
/// </summary>
public class ContinueStatement : Statement
{
    public NCSInstruction? JumpTarget { get; set; }

    public override void Compile(NCS ncs, CodeRoot root, CodeBlock block)
    {
        // Create jump instruction - target will be set by enclosing loop
        NCSInstruction jumpInstruction = ncs.Add(NCSInstructionType.JMP, new List<object>());
        
        if (JumpTarget != null)
        {
            jumpInstruction.Jump = JumpTarget;
        }
        // If JumpTarget is null, it will be set later by the enclosing loop context
    }

    public override string ToString()
    {
        return "continue;";
    }
}

