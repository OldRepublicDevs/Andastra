using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Compiler.NSS.AST.Statements;

/// <summary>
/// Represents an empty statement (just a semicolon).
/// </summary>
public class EmptyStatement : Statement
{
    public override void Compile(NCS ncs, CodeRoot root, CodeBlock block)
    {
        // No code generation needed for empty statement
    }

    public override string ToString()
    {
        return ";";
    }
}

