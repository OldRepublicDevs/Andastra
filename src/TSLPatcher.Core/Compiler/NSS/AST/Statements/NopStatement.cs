using System.Collections.Generic;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Compiler.NSS.AST.Statements;

/// <summary>
/// Represents a NOP (no operation) statement.
/// </summary>
public class NopStatement : Statement
{
    public override void Compile(NCS ncs, CodeRoot root, CodeBlock block)
    {
        ncs.Add(NCSInstructionType.NOP, new List<object>());
    }

    public override string ToString()
    {
        return "nop;";
    }
}

