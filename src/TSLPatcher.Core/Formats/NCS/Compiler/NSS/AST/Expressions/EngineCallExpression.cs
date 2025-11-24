using System;
using System.Collections.Generic;
using System.Linq;
using TSLPatcher.Core.Common.Script;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Formats.NCS.Compiler
{

    /// <summary>
    /// Represents a call to an engine-defined function (e.g., SendMessageToPC, GetFirstPC).
    /// </summary>
    public class EngineCallExpression : Expression
    {
        public ScriptFunction Function { get; set; }
        public int RoutineId { get; set; }
        public List<Expression> Arguments { get; set; }

        public EngineCallExpression(ScriptFunction function, int routineId, List<Expression> arguments)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
            RoutineId = routineId;
            Arguments = arguments ?? new List<Expression>();
        }

        public override DynamicDataType Compile(NCS ncs, CodeRoot root, CodeBlock block)
        {
            int argCount = Arguments.Count;

            if (argCount > Function.Params.Count)
            {
                throw new CompileError(
                    $"Too many arguments for '{Function.Name}'\n" +
                    $"  Expected: {Function.Params.Count}, Got: {argCount}");
            }

            // Fill in default parameters if needed
            for (int i = argCount; i < Function.Params.Count; i++)
            {
                ScriptParam param = Function.Params[i];
                if (param.Default == null)
                {
                    IEnumerable<string> requiredParams = Function.Params.Where(p => p.Default == null).Select(p => p.Name);
                    throw new CompileError(
                        $"Missing required arguments for '{Function.Name}'\n" +
                        $"  Required parameters: {string.Join(", ", requiredParams)}\n" +
                        $"  Provided: {argCount} argument(s)");
                }

                // [CanBeNull] Try to find constant
                ScriptConstant constant = root.FindConstant(param.Default.ToString() ?? "");
                if (constant != null)
                {
                    Arguments.Add(CreateConstantExpression(constant));
                }
                else
                {
                    // Parse default value based on parameter type
                    Arguments.Add(CreateDefaultExpression(param));
                }
            }

            // Compile arguments in reverse order (right-to-left on stack)
            int thisStack = 0;
            for (int i = Arguments.Count - 1; i >= 0; i--)
            {
                Expression arg = Arguments[i];
                ScriptParam param = Function.Params[i];
                var paramType = new DynamicDataType(param.DataType);

                if (paramType.Builtin == DataType.Action)
                {
                    // Special handling for action parameters (delayed execution)
                    NCSInstruction afterCommand = ncs.Add(NCSInstructionType.NOP, new List<object>());
                    ncs.Add(NCSInstructionType.STORE_STATE, new List<object>
                {
                    -root.ScopeSize(),
                    block.StackOffset + block.TempStack
                });
                    NCSInstruction jumpInst = ncs.Add(NCSInstructionType.JMP, new List<object>());
                    jumpInst.Jump = afterCommand;

                    arg.Compile(ncs, root, block);
                    ncs.Add(NCSInstructionType.RETN, new List<object>());
                }
                else
                {
                    DynamicDataType addedType = arg.Compile(ncs, root, block);
                    block.TempStack += addedType.Size(root);
                    thisStack += addedType.Size(root);

                    if (addedType.Builtin != paramType.Builtin)
                    {
                        throw new CompileError(
                            $"Type mismatch for parameter '{param.Name}' in call to '{Function.Name}'\n" +
                            $"  Expected: {paramType.Builtin.ToScriptString()}\n" +
                            $"  Got: {addedType.Builtin.ToScriptString()}");
                    }
                }
            }

            ncs.Add(NCSInstructionType.ACTION, new List<object> { RoutineId, Arguments.Count });
            block.TempStack -= thisStack;

            return new DynamicDataType(Function.ReturnType);
        }

        private static Expression CreateConstantExpression(ScriptConstant constant)
        {
            switch (constant.DataType)
            {
                case DataType.Int:
                    return new IntExpression((int)constant.Value);
                case DataType.Float:
                    return new FloatExpression(Convert.ToSingle(constant.Value));
                case DataType.String:
                    return new StringExpression((string)constant.Value);
                case DataType.Object:
                    return new ObjectExpression((int)constant.Value);
                default:
                    throw new CompileError($"Unsupported constant type: {constant.DataType}");
            }
        }

        private Expression CreateDefaultExpression(ScriptParam param)
        {
            if (param.Default == null)
            {
                throw new CompileError($"Parameter '{param.Name}' has no default value");
            }

            switch (param.DataType)
            {
                case DataType.Int:
                    return new IntExpression(Convert.ToInt32(param.Default));
                case DataType.Float:
                    return new FloatExpression(Convert.ToSingle(param.Default));
                case DataType.String:
                    return new StringExpression(param.Default.ToString() ?? "");
                case DataType.Object:
                    return new ObjectExpression(Convert.ToInt32(param.Default));
                default:
                    throw new CompileError(
                        $"Unsupported default parameter type '{param.DataType}' for '{param.Name}' in '{Function.Name}'\n" +
                        "  This may indicate a compiler limitation");
            }
        }

        public override string ToString()
        {
            string args = string.Join(", ", Arguments.Select(a => a.ToString()));
            return $"{Function.Name}({args})";
        }
    }
}

