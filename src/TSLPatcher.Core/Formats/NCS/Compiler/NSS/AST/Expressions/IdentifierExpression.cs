using System;
using System.Collections.Generic;
using TSLPatcher.Core.Common.Script;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Formats.NCS.Compiler
{

    /// <summary>
    /// Represents a variable identifier expression.
    /// </summary>
    public class IdentifierExpression : Expression
    {
        public Identifier Identifier { get; set; }

        public IdentifierExpression(Identifier identifier)
        {
            Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        }

        public override DynamicDataType Compile(NCS ncs, CodeRoot root, CodeBlock block)
        {
            // [CanBeNull] Try to find as a constant first
            ScriptConstant constant = root.FindConstant(Identifier.Label);
            if (constant != null)
            {
                // Emit constant value
                switch (constant.DataType)
                {
                    case DataType.Int:
                        ncs.Add(NCSInstructionType.CONSTI, new List<object> { constant.Value });
                        block.TempStack += 4;
                        return new DynamicDataType(DataType.Int);
                    case DataType.Float:
                        ncs.Add(NCSInstructionType.CONSTF, new List<object> { constant.Value });
                        block.TempStack += 4;
                        return new DynamicDataType(DataType.Float);
                    case DataType.String:
                        ncs.Add(NCSInstructionType.CONSTS, new List<object> { constant.Value });
                        block.TempStack += 4;
                        return new DynamicDataType(DataType.String);
                    default:
                        throw new CompileError($"Unsupported constant type: {constant.DataType}");
                }
            }

            // Otherwise, it's a variable - look up in scope
            GetScopedResult scoped = block.GetScoped(Identifier, root);
            bool isGlobal = scoped.IsGlobal;
            DynamicDataType dataType = scoped.Datatype;
            int offset = scoped.Offset;
            NCSInstructionType instructionType = isGlobal ? NCSInstructionType.CPTOPBP : NCSInstructionType.CPTOPSP;

            ncs.Add(instructionType, new List<object> { offset, dataType.Size(root) });
            block.TempStack += dataType.Size(root);

            return dataType;
        }

        public bool IsConstant(CodeRoot root)
        {
            return root.FindConstant(Identifier.Label) != null;
        }

        public override string ToString()
        {
            return Identifier.Label;
        }
    }
}

