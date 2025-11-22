using System.Collections.Generic;
using TSLPatcher.Core.Common.Script;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Compiler.NSS.AST.Statements;

/// <summary>
/// Represents a local variable declaration statement.
/// </summary>
public class DeclarationStatement : Statement
{
    public DynamicDataType DataType { get; set; }
    public Identifier Identifier { get; set; }
    public Expression? Initializer { get; set; }

    public DeclarationStatement(DynamicDataType dataType, Identifier identifier, Expression? initializer = null)
    {
        DataType = dataType ?? throw new System.ArgumentNullException(nameof(dataType));
        Identifier = identifier ?? throw new System.ArgumentNullException(nameof(identifier));
        Initializer = initializer;
    }

    public override void Compile(NCS ncs, CodeRoot root, CodeBlock block)
    {
        // Add variable to local scope
        block.AddLocal(Identifier, DataType, root);
        
        int size = DataType.Size(root);
        
        if (Initializer != null)
        {
            // Compile initializer expression
            DynamicDataType initType = Initializer.Compile(ncs, root, block);
            
            if (initType != DataType)
            {
                throw new CompileError(
                    $"Type mismatch in variable declaration for '{Identifier}'\n" +
                    $"  Declared type: {DataType.Builtin.ToScriptString()}\n" +
                    $"  Initializer type: {initType.Builtin.ToScriptString()}");
            }
            
            block.TempStack += size;
        }
        else
        {
            // Initialize with default value based on type
            switch (DataType.Builtin)
            {
                case Common.Script.DataType.Int:
                    ncs.Add(NCSInstructionType.CONSTI, new List<object> { 0 });
                    break;
                case Common.Script.DataType.Float:
                    ncs.Add(NCSInstructionType.CONSTF, new List<object> { 0.0f });
                    break;
                case Common.Script.DataType.String:
                    ncs.Add(NCSInstructionType.CONSTS, new List<object> { string.Empty });
                    break;
                case Common.Script.DataType.Object:
                    ncs.Add(NCSInstructionType.CONSTO, new List<object> { 0 });
                    break;
                case Common.Script.DataType.Vector:
                    // Initialize vector to [0.0, 0.0, 0.0]
                    ncs.Add(NCSInstructionType.CONSTF, new List<object> { 0.0f });
                    ncs.Add(NCSInstructionType.CONSTF, new List<object> { 0.0f });
                    ncs.Add(NCSInstructionType.CONSTF, new List<object> { 0.0f });
                    break;
                case Common.Script.DataType.Struct:
                    if (DataType.Struct != null && root.StructMap.TryGetValue(DataType.Struct, out Struct? structDef))
                    {
                        structDef.Initialize(ncs, root);
                    }
                    else
                    {
                        throw new CompileError($"Unknown struct type: {DataType.Struct}");
                    }
                    break;
                default:
                    throw new CompileError($"Unsupported variable type: {DataType.Builtin}");
            }
            
            block.TempStack += size;
        }
        
        // Reserve space on stack for this variable
        ncs.Add(NCSInstructionType.MOVSP, new List<object> { size });
    }

    public override string ToString()
    {
        if (Initializer != null)
        {
            return $"{DataType} {Identifier} = {Initializer};";
        }
        return $"{DataType} {Identifier};";
    }
}

