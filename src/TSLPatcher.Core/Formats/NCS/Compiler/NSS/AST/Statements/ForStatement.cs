using System.Collections.Generic;
using JetBrains.Annotations;
using TSLPatcher.Core.Common.Script;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Formats.NCS.Compiler
{

    /// <summary>
    /// Represents a for loop statement.
    /// </summary>
    public class ForStatement : Statement
    {
        public Expression Initializer { get; set; }
        public Expression Condition { get; set; }
        public Expression Iterator { get; set; }
        public CodeBlock Body { get; set; }
        public List<BreakStatement> BreakStatements { get; set; }
        public List<ContinueStatement> ContinueStatements { get; set; }

        public ForStatement(Expression initializer, Expression condition, Expression iterator, CodeBlock body)
        {
            Initializer = initializer ?? throw new System.ArgumentNullException(nameof(initializer));
            Condition = condition ?? throw new System.ArgumentNullException(nameof(condition));
            Iterator = iterator ?? throw new System.ArgumentNullException(nameof(iterator));
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
            // Compile initializer
            DynamicDataType initialType = Initializer.Compile(ncs, root, block);

            // Pop initializer result off stack (it's not needed)
            if (initialType.Size(root) > 0)
            {
                ncs.Add(NCSInstructionType.MOVSP, new List<object> { -initialType.Size(root) });
            }

            block.MarkBreakScope();

            // Loop start (condition evaluation point)
            NCSInstruction loopStart = ncs.Add(NCSInstructionType.NOP, new List<object>());
            var updateStart = new NCSInstruction(NCSInstructionType.NOP);
            var loopEnd = new NCSInstruction(NCSInstructionType.NOP);

            // Compile condition
            DynamicDataType conditionType = Condition.Compile(ncs, root, block);

            if (conditionType.Builtin != DataType.Int)
            {
                throw new CompileError(
                    $"for loop condition must be int type, got {conditionType.Builtin.ToScriptString()}\n" +
                    "  Note: Conditions must evaluate to int (0=false, non-zero=true)");
            }

            block.TempStack += 4;

            // If condition is false, jump to end
            NCSInstruction jumpToEnd = ncs.Add(NCSInstructionType.JZ, new List<object>());
            block.TempStack -= 4;

            block.MarkBreakScope();

            // Compile loop body
            Body.Compile(ncs, root, block, returnInstruction, loopEnd, updateStart);

            // Update/iteration point (continue jumps here)
            ncs.Instructions.Add(updateStart);

            // Compile iterator
            DynamicDataType iteratorType = Iterator.Compile(ncs, root, block);

            // Pop iterator result off stack
            if (iteratorType.Size(root) > 0)
            {
                ncs.Add(NCSInstructionType.MOVSP, new List<object> { -iteratorType.Size(root) });
            }

            // Jump back to loop start (condition re-evaluation)
            ncs.Add(NCSInstructionType.JMP, jump: loopStart);

            // Loop end marker
            ncs.Instructions.Add(loopEnd);
            jumpToEnd.Jump = loopEnd;

            return DynamicDataType.VOID;
        }
    }
}

