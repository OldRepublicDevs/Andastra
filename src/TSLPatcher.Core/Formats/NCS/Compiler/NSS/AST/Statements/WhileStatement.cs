using System.Collections.Generic;
using JetBrains.Annotations;
using TSLPatcher.Core.Common.Script;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Formats.NCS.Compiler
{

    /// <summary>
    /// Represents a while loop statement.
    /// </summary>
    public class WhileStatement : Statement
    {
        public Expression Condition { get; set; }
        public CodeBlock Body { get; set; }
        public List<BreakStatement> BreakStatements { get; set; }
        public List<ContinueStatement> ContinueStatements { get; set; }

        public WhileStatement(Expression condition, CodeBlock body)
        {
            Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            Body = body ?? throw new System.ArgumentNullException(nameof(body));
            BreakStatements = new List<BreakStatement>();
            ContinueStatements = new List<ContinueStatement>();
        }

        public override object Compile(
            NCS ncs,
            CodeRoot root,
            CodeBlock block,
            NCSInstruction returnInstruction,
            [CanBeNull] NCSInstruction breakInstruction,
            [CanBeNull] NCSInstruction continueInstruction)
        {
            // Loop start (condition evaluation point)
            NCSInstruction loopStart = ncs.Add(NCSInstructionType.NOP, new List<object>());

            // Compile condition
            DynamicDataType conditionType = Condition.Compile(ncs, root, block);

            if (conditionType.Builtin != DataType.Int)
            {
                throw new CompileError(
                    $"While condition must be int type, got {conditionType.Builtin.ToScriptString()}\n" +
                    "  Note: In NWScript, conditions must evaluate to int (0=false, non-zero=true)");
            }

            block.TempStack += 4;

            block.MarkBreakScope();

            // If condition is false, jump to end
            var loopEnd = new NCSInstruction(NCSInstructionType.NOP);
            NCSInstruction jumpToEnd = ncs.Add(NCSInstructionType.JZ, new List<object>());
            block.TempStack -= 4;

            // Compile loop body
            Body.Compile(ncs, root, block, returnInstruction, loopEnd, loopStart);

            // Jump back to loop start
            ncs.Add(NCSInstructionType.JMP, jump: loopStart);

            // Loop end marker
            ncs.Instructions.Add(loopEnd);
            jumpToEnd.Jump = loopEnd;

            return DynamicDataType.VOID;
        }
    }
}

