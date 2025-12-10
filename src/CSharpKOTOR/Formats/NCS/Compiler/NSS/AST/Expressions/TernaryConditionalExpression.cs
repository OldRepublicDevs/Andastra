using System;
using System.Collections.Generic;
using CSharpKOTOR.Common.Script;
using CSharpKOTOR.Formats.NCS;

namespace CSharpKOTOR.Formats.NCS.Compiler
{
    /// <summary>
    /// Represents a ternary conditional expression (condition ? trueExpr : falseExpr).
    /// </summary>
    public class TernaryConditionalExpression : Expression
    {
        public Expression Condition { get; set; }
        public Expression TrueExpression { get; set; }
        public Expression FalseExpression { get; set; }

        public TernaryConditionalExpression(
            Expression condition,
            Expression trueExpr,
            Expression falseExpr)
        {
            Condition = condition ?? throw new ArgumentNullException(nameof(condition));
            TrueExpression = trueExpr ?? throw new ArgumentNullException(nameof(trueExpr));
            FalseExpression = falseExpr ?? throw new ArgumentNullException(nameof(falseExpr));
        }

        public override DynamicDataType Compile(NCS ncs, CodeRoot root, CodeBlock block)
        {
            DynamicDataType conditionType = Condition.Compile(ncs, root, block);

            if (conditionType.Builtin != DataType.Int)
            {
                throw new CompileError(
                    $"Ternary condition must be int type, got {conditionType.Builtin.ToScriptString()}\n" +
                    "  Note: In NWScript, conditions must evaluate to int (0=false, non-zero=true)");
            }

            block.TempStack += 4;

            NCSInstruction jumpToFalse = ncs.Add(NCSInstructionType.JZ, new List<object>());
            block.TempStack -= 4;

            DynamicDataType trueType = TrueExpression.Compile(ncs, root, block);

            NCSInstruction jumpToEnd = ncs.Add(NCSInstructionType.JMP, new List<object>());

            NCSInstruction falseStart = ncs.Add(NCSInstructionType.NOP, new List<object>());
            jumpToFalse.Jump = falseStart;

            DynamicDataType falseType = FalseExpression.Compile(ncs, root, block);

            NCSInstruction endMarker = ncs.Add(NCSInstructionType.NOP, new List<object>());
            jumpToEnd.Jump = endMarker;

            if (trueType.Builtin != falseType.Builtin)
            {
                throw new CompileError(
                    $"Ternary expression branches must have the same type (got {trueType.Builtin.ToScriptString()} and {falseType.Builtin.ToScriptString()})");
            }

            return trueType;
        }

        public override string ToString()
        {
            return $"({Condition} ? {TrueExpression} : {FalseExpression})";
        }
    }
}

