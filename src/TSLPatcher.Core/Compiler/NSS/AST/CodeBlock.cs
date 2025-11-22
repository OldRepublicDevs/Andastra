using System.Collections.Generic;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Compiler.NSS.AST;

/// <summary>
/// Represents a block of code containing statements and local variable declarations.
/// </summary>
public class CodeBlock
{
    public List<Statement> Statements { get; set; }
    public Dictionary<Identifier, (DynamicDataType DataType, int Offset)> LocalVariables { get; set; }
    public int StackOffset { get; set; }
    public int TempStack { get; set; }
    public CodeBlock? Parent { get; set; }

    public CodeBlock(CodeBlock? parent = null)
    {
        Statements = new List<Statement>();
        LocalVariables = new Dictionary<Identifier, (DynamicDataType, int)>();
        StackOffset = 0;
        TempStack = 0;
        Parent = parent;
    }

    public void AddLocal(Identifier identifier, DynamicDataType dataType, CodeRoot root)
    {
        int size = dataType.Size(root);
        LocalVariables[identifier] = (dataType, StackOffset);
        StackOffset += size;
    }

    public bool TryGetLocal(Identifier identifier, out (DynamicDataType DataType, int Offset) result)
    {
        if (LocalVariables.TryGetValue(identifier, out result))
        {
            return true;
        }
        
        if (Parent != null)
        {
            return Parent.TryGetLocal(identifier, out result);
        }

        return false;
    }

    public void Compile(NCS ncs, CodeRoot root)
    {
        foreach (Statement statement in Statements)
        {
            statement.Compile(ncs, root, this);
        }
    }
}

