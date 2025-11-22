using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Compiler.NSS.AST;

/// <summary>
/// Base class for all NSS statements.
/// 
/// Statements perform actions and control flow, but do not produce values.
/// </summary>
public abstract class Statement
{
    /// <summary>
    /// Compile this statement to NCS bytecode.
    /// </summary>
    /// <param name="ncs">NCS object to emit instructions to</param>
    /// <param name="root">Root compilation context</param>
    /// <param name="block">Current code block context</param>
    public abstract void Compile(NCS ncs, CodeRoot root, CodeBlock block);
}

