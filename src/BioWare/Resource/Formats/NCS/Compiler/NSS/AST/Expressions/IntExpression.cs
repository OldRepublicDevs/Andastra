using System.Collections.Generic;
using BioWare.Common.Script;
using BioWare.Resource.Formats.NCS;
using BioWare.Resource.Formats.NCS.Compiler.NSS.AST;

namespace BioWare.Resource.Formats.NCS.Compiler.NSS.AST.Expressions
{

    /// <summary>
    /// Represents an integer literal expression.
    /// </summary>
    public class IntExpression : Expression
    {
        public int Value { get; set; }

        public IntExpression(int value)
        {
            Value = value;
        }

        public override DynamicDataType Compile(NCS ncs, CodeRoot root, CodeBlock block)
        {
            ncs.Add(NCSInstructionType.CONSTI, new List<object> { Value });
            return new DynamicDataType(DataType.Int);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}

