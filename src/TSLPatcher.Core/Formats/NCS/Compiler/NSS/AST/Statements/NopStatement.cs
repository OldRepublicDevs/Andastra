using System.Collections.Generic;
using JetBrains.Annotations;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Formats.NCS.Compiler
{

    /// <summary>
    /// Represents a NOP (no operation) statement.
    /// </summary>
    public class NopStatement : Statement
    {
        public override object Compile(
            NCS ncs,
            CodeRoot root,
            CodeBlock block,
            NCSInstruction returnInstruction,
            [CanBeNull] NCSInstruction breakInstruction,
            [CanBeNull] NCSInstruction continueInstruction)
        {
            ncs.Add(NCSInstructionType.NOP, new List<object>());
            return DynamicDataType.VOID;
        }
    }
}

