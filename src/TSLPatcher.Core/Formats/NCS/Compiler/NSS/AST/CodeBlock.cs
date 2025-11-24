using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Formats.NCS.Compiler
{

    /// <summary>
    /// Represents a block of code containing statements and local variable declarations.
    /// </summary>
    public class CodeBlock
    {
        public List<Statement> Statements { get; set; }
        public List<ScopedValue> Scope { get; set; }
        public Dictionary<Identifier, (DynamicDataType DataType, int Offset)> LocalVariables { get; set; }
        public int StackOffset { get; set; }
        public int TempStack { get; set; }
        [CanBeNull]
        public CodeBlock Parent { get; set; }
        private bool _breakScope;

        public CodeBlock([CanBeNull] CodeBlock parent = null)
        {
            Statements = new List<Statement>();
            Scope = new List<ScopedValue>();
            LocalVariables = new Dictionary<Identifier, (DynamicDataType, int)>();
            StackOffset = 0;
            TempStack = 0;
            Parent = parent;
            _breakScope = false;
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

        public void Compile(
            NCS ncs,
            CodeRoot root,
            [CanBeNull] CodeBlock block,
            NCSInstruction returnInstruction,
            [CanBeNull] NCSInstruction breakInstruction,
            [CanBeNull] NCSInstruction continueInstruction)
        {
            Parent = block;

            foreach (Statement statement in Statements)
            {
                if (!(statement is ReturnStatement))
                {
                    statement.Compile(ncs, root, this, returnInstruction, breakInstruction, continueInstruction);
                }
                else
                {
                    int scopeSize = FullScopeSize(root);
                    DynamicDataType returnType = (DynamicDataType)statement.Compile(ncs, root, this, returnInstruction, null, null);
                    if (returnType != DynamicDataType.VOID)
                    {
                        ncs.Add(NCSInstructionType.CPDOWNSP, new List<object> { -scopeSize - returnType.Size(root) * 2, returnType.Size(root) });
                        ncs.Add(NCSInstructionType.MOVSP, new List<object> { -returnType.Size(root) });
                    }
                    ncs.Add(NCSInstructionType.MOVSP, new List<object> { -scopeSize });
                    ncs.Add(NCSInstructionType.JMP, jump: returnInstruction);
                    return;
                }
            }
            ncs.Instructions.Add(new NCSInstruction(NCSInstructionType.MOVSP, new List<object> { -ScopeSize(root) }));

            if (TempStack != 0)
            {
                throw new CompileError(
                    $"Internal compiler error: Temporary stack not cleared after block compilation\n" +
                    $"  Temp stack size: {TempStack}\n" +
                    $"  This indicates a bug in one of the expression/statement compile methods");
            }
        }

        public void AddScoped(Identifier identifier, DynamicDataType dataType)
        {
            // Insert at beginning to match Python's list.insert(0, ...)
            Scope.Insert(0, new ScopedValue(identifier, dataType));
        }

        public void MarkBreakScope()
        {
            _breakScope = true;
        }

        public GetScopedResult GetScoped(Identifier identifier, CodeRoot root, int? offset = null)
        {
            int currentOffset = offset ?? -TempStack;
            currentOffset -= TempStack;

            // Python implementation iterates through scope list
            foreach (ScopedValue scoped in Scope)
            {
                currentOffset -= scoped.DataType.Size(root);
                if (scoped.Identifier.Equals(identifier))
                {
                    return new GetScopedResult(false, scoped.DataType, currentOffset);
                }
            }

            if (Parent != null)
            {
                return Parent.GetScoped(identifier, root, currentOffset);
            }

            return root.GetScoped(identifier, root);
        }

        public int ScopeSize(CodeRoot root)
        {
            return Scope.Sum(scoped => scoped.DataType.Size(root));
        }

        public int FullScopeSize(CodeRoot root)
        {
            int size = ScopeSize(root);
            if (Parent != null)
            {
                size += Parent.FullScopeSize(root);
            }
            return size;
        }

        public int BreakScopeSize(CodeRoot root)
        {
            int size = ScopeSize(root);
            if (Parent != null && !Parent._breakScope)
            {
                size += Parent.BreakScopeSize(root);
            }
            return size;
        }
    }
}

