using System.Collections.Generic;
using Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST;
using Andastra.Parsing.Common.Script;
using Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST;
using Andastra.Parsing.Resource.Formats.NCS;
using Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST;

namespace Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST.Expressions
{

    /// <summary>
    /// Represents a string literal expression.
    /// </summary>
    public class StringExpression : Expression
    {
        public string Value { get; set; }

        public StringExpression(string value)
        {
            Value = value ?? string.Empty;
        }

        public override DynamicDataType Compile(NCS ncs, CodeRoot root, CodeBlock block)
        {
            ncs.Add(NCSInstructionType.CONSTS, new List<object> { Value });
            return new DynamicDataType(DataType.String);
        }

        public override string ToString()
        {
            return $"\"{Value}\"";
        }
    }
}

