using System.Collections.Generic;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Formats.NCS.Compiler
{
    /// <summary>
    /// Represents a global variable declaration.
    /// 1:1 port from pykotor.resource.formats.ncs.compiler.classes.GlobalVariableDeclaration
    /// </summary>
    public class GlobalVariableDeclaration : TopLevelObject
    {
        public Identifier Identifier { get; set; }
        public DynamicDataType DataType { get; set; }

        public GlobalVariableDeclaration(Identifier identifier, DynamicDataType dataType)
        {
            Identifier = identifier;
            DataType = dataType;
        }

        public override void Compile(NCS ncs, CodeRoot root)
        {
            root.AddScoped(Identifier, DataType);
        }
    }

    /// <summary>
    /// Represents a global variable initialization.
    /// 1:1 port from pykotor.resource.formats.ncs.compiler.classes.GlobalVariableInitialization
    /// </summary>
    public class GlobalVariableInitialization : TopLevelObject
    {
        public Identifier Identifier { get; set; }
        public DynamicDataType DataType { get; set; }
        public Expression Expression { get; set; }

        public GlobalVariableInitialization(Identifier identifier, DynamicDataType dataType, Expression expression)
        {
            Identifier = identifier;
            DataType = dataType;
            Expression = expression;
        }

        public override void Compile(NCS ncs, CodeRoot root)
        {
            // Allocate storage for the global variable (this also registers it in the global scope)
            GlobalVariableDeclaration declaration = new GlobalVariableDeclaration(Identifier, DataType);
            declaration.Compile(ncs, root);

            CodeBlock block = new CodeBlock();
            DynamicDataType expressionType = Expression.Compile(ncs, root, block);
            if (expressionType != DataType)
            {
                string msg = $"Type mismatch in initialization of global variable '{Identifier}'\n" +
                             $"  Declared type: {DataType.Builtin}\n" +
                             $"  Initializer type: {expressionType.Builtin}";
                throw new CompileError(msg);
            }

            GetScopedResult scoped = root.GetScoped(Identifier, root);
            // Global storage resides on the stack before base pointer is saved, so use stack-pointer-relative copy.
            int stackIndex = scoped.Offset - scoped.Datatype.Size(root);
            ncs.Instructions.Add(
                new NCSInstruction(NCSInstructionType.CPDOWNSP, new List<object> { stackIndex, scoped.Datatype.Size(root) })
            );
            // Remove the initializer value from the stack
            ncs.Add(NCSInstructionType.MOVSP, new List<object> { -scoped.Datatype.Size(root) });
        }
    }

    /// <summary>
    /// Represents a struct definition.
    /// 1:1 port from pykotor.resource.formats.ncs.compiler.classes.StructDefinition
    /// </summary>
    public class StructDefinition : TopLevelObject
    {
        public Identifier Identifier { get; set; }
        public List<StructMember> Members { get; set; }

        public StructDefinition(Identifier identifier, List<StructMember> members)
        {
            Identifier = identifier;
            Members = members;
        }

        public override void Compile(NCS ncs, CodeRoot root)
        {
            if (Members.Count == 0)
            {
                string msg = $"Struct '{Identifier}' cannot be empty\n" +
                            "  Structs must have at least one member";
                throw new CompileError(msg);
            }
            root.StructMap[Identifier.Label] = new Struct(Identifier, Members);
        }
    }

    /// <summary>
    /// Represents an include script statement.
    /// 1:1 port from pykotor.resource.formats.ncs.compiler.classes.IncludeScript
    /// </summary>
    public class IncludeScript : TopLevelObject
    {
        public StringExpression File { get; set; }
        public Dictionary<string, byte[]> Library { get; set; }

        public IncludeScript(StringExpression file, Dictionary<string, byte[]> library = null)
        {
            File = file;
            Library = library ?? new Dictionary<string, byte[]>();
        }

        public override void Compile(NCS ncs, CodeRoot root)
        {
            // TODO: Implement include script compilation
            // This requires the NssParser which may not be available yet
            // For now, this is a stub to match the Python structure
            throw new System.NotImplementedException("IncludeScript compilation not yet implemented");
        }
    }
}

