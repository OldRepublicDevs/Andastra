using System.Collections.Generic;
using Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST;
using Andastra.Parsing.Common.Script;
using Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST;
using Andastra.Parsing.Resource.Formats.NCS;
using Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST;

namespace Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST.Expressions
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

