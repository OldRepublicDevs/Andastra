using System.Collections.Generic;
using Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST;
using Andastra.Parsing.Common.Script;
using Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST;
using Andastra.Parsing.Resource.Formats.NCS;
using Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST;

namespace Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST.Expressions
{

    /// <summary>
    /// Represents a bitwise NOT expression (~x).
    /// </summary>
    public class BitwiseNotExpression : Expression
    {
        public Expression Operand { get; set; }

        public BitwiseNotExpression(Expression operand)
        {
            Operand = operand ?? throw new System.ArgumentNullException(nameof(operand));
        }

        public override DynamicDataType Compile(NCS ncs, CodeRoot root, CodeBlock block)
        {
            DynamicDataType operandType = Operand.Compile(ncs, root, block);
            block.TempStack += 4;

            if (operandType.Builtin != DataType.Int)
            {
                throw new CompileError(
                    $"Bitwise NOT (~) requires integer operand, got {operandType.Builtin.ToScriptString()}\n" +
                    "  Note: Bitwise operations only work on int types");
            }

            ncs.Add(NCSInstructionType.COMPI, new List<object>());

            block.TempStack -= 4;
            return operandType;
        }

        public override string ToString()
        {
            return $"~{Operand}";
        }
    }
}

