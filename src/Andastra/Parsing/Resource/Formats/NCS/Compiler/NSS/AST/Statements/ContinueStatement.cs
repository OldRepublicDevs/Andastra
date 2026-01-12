using System.Collections.Generic;
using Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST;
using Andastra.Parsing.Resource.Formats.NCS;
using Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST;
using JetBrains.Annotations;
using Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST;

namespace Andastra.Parsing.Resource.Formats.NCS.Compiler.NSS.AST.Statements
{

    /// <summary>
    /// Represents a continue statement in a loop.
    /// </summary>
    public class ContinueStatement : Statement
    {
        [CanBeNull]
        public NCSInstruction JumpTarget { get; set; }

        public override object Compile(
            NCS ncs,
            CodeRoot root,
            CodeBlock block,
            NCSInstruction returnInstruction,
            [CanBeNull] NCSInstruction breakInstruction,
            [CanBeNull] NCSInstruction continueInstruction)
        {
            if (continueInstruction == null)
            {
                throw new CompileError("continue statement not inside loop");
            }

            ncs.Add(NCSInstructionType.MOVSP, new List<object> { -block.BreakScopeSize(root) });
            ncs.Add(NCSInstructionType.JMP, jump: continueInstruction);
            return DynamicDataType.VOID;
        }
    }
}

