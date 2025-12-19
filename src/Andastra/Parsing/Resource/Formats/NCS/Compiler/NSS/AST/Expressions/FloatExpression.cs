using System.Collections.Generic;
using Andastra.Parsing.Common.Script;
using Andastra.Parsing.Formats.NCS;

namespace Andastra.Parsing.Formats.NCS.Compiler
{

    /// <summary>
    /// Represents a floating-point literal expression.
    /// </summary>
    public class FloatExpression : Expression
    {
        public float Value { get; set; }

        public FloatExpression(float value)
        {
            Value = value;
        }

        public override DynamicDataType Compile(NCS ncs, CodeRoot root, CodeBlock block)
        {
            ncs.Add(NCSInstructionType.CONSTF, new List<object> { Value });
            return new DynamicDataType(DataType.Float);
        }

        public override string ToString()
        {
            return Value.ToString("F");
        }
    }
}

