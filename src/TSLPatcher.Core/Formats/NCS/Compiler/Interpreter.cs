using System;
using System.Collections.Generic;
using System.Linq;
using TSLPatcher.Core.Common;
using TSLPatcher.Core.Common.Script;
using TSLPatcher.Core.Formats.NCS;

namespace TSLPatcher.Core.Formats.NCS.Compiler;

/// <summary>
/// NCS bytecode interpreter for testing and debugging.
/// 1:1 port from Python Interpreter class in pykotor/resource/formats/ncs/compiler/interpreter.py
/// </summary>
public class Interpreter
{
    private const int DefaultMaxInstructions = 100_000;

    private readonly NCS _ncs;
    private NCSInstruction? _cursor;
    private int _cursorIndex;
    private readonly List<ScriptFunction> _functions;
    private readonly Dictionary<int, int> _instructionIndices;
    private readonly Stack _stack;
    private readonly List<(NCSInstruction, int)> _returns;
    private readonly Dictionary<string, Func<object?[], object?>> _mocks;
    private readonly int _maxInstructions;
    private int _instructionsExecuted;

    public List<StackSnapshot> StackSnapshots { get; }
    public List<ActionSnapshot> ActionSnapshots { get; }

    public Interpreter(NCS ncs, Game game = Game.K1, int? maxInstructions = null)
    {
        _ncs = ncs ?? throw new ArgumentNullException(nameof(ncs));
        _cursor = ncs.Instructions.Count > 0 ? ncs.Instructions[0] : null;
        _cursorIndex = 0;
        _functions = GetFunctionsForGame(game);
        _instructionIndices = ncs.Instructions.Select((inst, idx) => (inst, idx))
            .ToDictionary(x => x.inst.GetHashCode(), x => x.idx);
        _stack = new Stack();
        _returns = new List<(NCSInstruction, int)>();
        _mocks = new Dictionary<string, Func<object?[], object?>>();
        _maxInstructions = maxInstructions ?? DefaultMaxInstructions;
        _instructionsExecuted = 0;
        StackSnapshots = new List<StackSnapshot>();
        ActionSnapshots = new List<ActionSnapshot>();
    }

    /// <summary>
    /// Execute the NCS script instructions.
    /// </summary>
    public void Run()
    {
        while (_cursor != null)
        {
            if (_instructionsExecuted >= _maxInstructions)
            {
                throw new InvalidOperationException(
                    $"Instruction limit exceeded: {_instructionsExecuted} instructions executed " +
                    $"(limit: {_maxInstructions}). Possible infinite loop detected at instruction " +
                    $"index {_cursorIndex} ({_cursor.InsType})");
            }

            _instructionsExecuted++;
            NCSInstruction cursor = _cursor;
            int index = _cursorIndex;

            // Take stack snapshot before executing instruction
            StackSnapshots.Add(new StackSnapshot(cursor, _stack.State()));

            // Execute instruction based on type
            ExecuteInstruction(cursor);

            // Move to next instruction (unless jump occurred)
            if (_cursor == cursor) // No jump occurred
            {
                _cursorIndex++;
                if (_cursorIndex >= _ncs.Instructions.Count)
                {
                    _cursor = null;
                }
                else
                {
                    _cursor = _ncs.Instructions[_cursorIndex];
                }
            }
        }
    }

    private void ExecuteInstruction(NCSInstruction instruction)
    {
        switch (instruction.InsType)
        {
            case NCSInstructionType.NOP:
                // No operation - do nothing
                break;

            case NCSInstructionType.CONSTI:
                // Push integer constant onto stack
                if (instruction.Args.Count > 0 && instruction.Args[0] is int intValue)
                {
                    _stack.Add(DataType.Int, intValue);
                }
                break;

            case NCSInstructionType.CONSTF:
                // Push float constant onto stack
                if (instruction.Args.Count > 0 && instruction.Args[0] is float floatValue)
                {
                    _stack.Add(DataType.Float, floatValue);
                }
                break;

            case NCSInstructionType.CONSTS:
                // Push string constant onto stack
                if (instruction.Args.Count > 0 && instruction.Args[0] is string stringValue)
                {
                    _stack.Add(DataType.String, stringValue);
                }
                break;

            case NCSInstructionType.ACTION:
                // Call a function/action
                ExecuteAction(instruction);
                break;

            case NCSInstructionType.MOVSP:
                // Move stack pointer
                if (instruction.Args.Count > 0 && instruction.Args[0] is int offset)
                {
                    _stack.Move(offset);
                }
                break;

            case NCSInstructionType.JMP:
                // Unconditional jump
                if (instruction.Jump != null)
                {
                    _cursor = instruction.Jump;
                    _cursorIndex = _ncs.GetInstructionIndex(instruction.Jump);
                }
                break;

            case NCSInstructionType.JZ:
                // Jump if zero
                if (_stack.State().Count > 0)
                {
                    StackObject top = _stack.Peek(-4);
                    bool isZero = IsZero(top);
                    if (isZero && instruction.Jump != null)
                    {
                        _cursor = instruction.Jump;
                        _cursorIndex = _ncs.GetInstructionIndex(instruction.Jump);
                    }
                }
                break;

            case NCSInstructionType.JNZ:
                // Jump if not zero
                if (_stack.State().Count > 0)
                {
                    StackObject top = _stack.Peek(-4);
                    bool isZero = IsZero(top);
                    if (!isZero && instruction.Jump != null)
                    {
                        _cursor = instruction.Jump;
                        _cursorIndex = _ncs.GetInstructionIndex(instruction.Jump);
                    }
                }
                break;

            case NCSInstructionType.RETN:
                // Return from function
                if (_returns.Count > 0)
                {
                    (NCSInstruction returnInst, int returnIndex) = _returns[_returns.Count - 1];
                    _returns.RemoveAt(_returns.Count - 1);
                    _cursor = returnInst;
                    _cursorIndex = returnIndex;
                }
                else
                {
                    _cursor = null; // End of execution
                }
                break;

            // TODO: Implement all other instruction types
            // This is a partial implementation - needs to be expanded to handle all NCS instructions
            default:
                throw new NotImplementedException(
                    $"Instruction type {instruction.InsType} is not yet implemented in the interpreter. " +
                    "Full interpreter implementation needs to be ported from Python.");
        }
    }

    private void ExecuteAction(NCSInstruction instruction)
    {
        // ACTION instruction: Args[0] = action ID, Args[1] = parameter count
        if (instruction.Args.Count < 2)
        {
            throw new InvalidOperationException("ACTION instruction requires at least 2 arguments");
        }

        int actionId = Convert.ToInt32(instruction.Args[0]);
        int paramCount = Convert.ToInt32(instruction.Args[1]);

        // Pop parameters from stack (parameters are pushed in reverse order)
        var argValues = new List<StackObject>();
        for (int i = paramCount - 1; i >= 0; i--)
        {
            int offset = -(paramCount - i) * 4;
            StackObject arg = _stack.Peek(offset);
            argValues.Add(arg);
        }

        // Try to find function by matching parameter count and types
        // Note: Action ID mapping to function names is complex and requires function table lookup
        // For now, we'll use a simplified approach
        ScriptFunction? function = null;
        if (actionId >= 0 && actionId < _functions.Count)
        {
            // Try index-based lookup (action IDs often correspond to function table indices)
            function = _functions[actionId];
        }
        else
        {
            // Try to find by parameter count match (heuristic)
            function = _functions.FirstOrDefault(f => f.Params.Count == paramCount);
        }

        string functionName = function?.Name ?? $"Action_{actionId}";

        // Check for mock
        object? returnValue = null;
        if (_mocks.TryGetValue(functionName, out Func<object?[], object?>? mock))
        {
            object?[] mockArgs = argValues.Select(a => a.Value).ToArray();
            returnValue = mock(mockArgs);
        }
        else if (function != null)
        {
            // Function exists but no mock - for void functions, just record the call
            // For functions with return values, we'd need to execute them (not implemented yet)
            returnValue = null;
        }

        // Record action snapshot
        ActionSnapshots.Add(new ActionSnapshot(functionName, argValues, returnValue));

        // Remove parameters from stack
        _stack.Move(-paramCount * 4);

        // Push return value if any
        if (returnValue != null && function != null && function.ReturnType != DataType.Void)
        {
            _stack.Add(function.ReturnType, returnValue);
        }
    }

    private static bool IsZero(StackObject obj)
    {
        return obj.Value switch
        {
            int i => i == 0,
            float f => Math.Abs(f) < float.Epsilon,
            string s => string.IsNullOrEmpty(s),
            null => true,
            _ => false
        };
    }

    /// <summary>
    /// Set a mock function for testing.
    /// </summary>
    public void SetMock(string functionName, Func<object?[], object?> mock)
    {
        ScriptFunction? function = _functions.FirstOrDefault(f => f.Name == functionName);
        if (function == null)
        {
            throw new ArgumentException($"Function '{functionName}' does not exist.");
        }

        // TODO: Validate parameter count matches
        _mocks[functionName] = mock;
    }

    /// <summary>
    /// Remove a mock function.
    /// </summary>
    public void RemoveMock(string functionName)
    {
        _mocks.Remove(functionName);
    }

    private static List<ScriptFunction> GetFunctionsForGame(Game game)
    {
        return game switch
        {
            Game.K1 => ScriptDefs.KOTOR_FUNCTIONS,
            Game.K2 => ScriptDefs.TSL_FUNCTIONS,
            _ => new List<ScriptFunction>()
        };
    }
}

/// <summary>
/// Represents a snapshot of the stack at a particular instruction.
/// 1:1 port from Python StackSnapshot NamedTuple
/// </summary>
public class StackSnapshot
{
    public NCSInstruction Instruction { get; }
    public List<StackObject> Stack { get; }

    public StackSnapshot(NCSInstruction instruction, List<StackObject> stack)
    {
        Instruction = instruction;
        Stack = stack;
    }
}

/// <summary>
/// Represents a snapshot of an action call.
/// 1:1 port from Python ActionSnapshot NamedTuple
/// </summary>
public class ActionSnapshot
{
    public string FunctionName { get; }
    public List<StackObject> ArgValues { get; }
    public object? ReturnValue { get; }

    public ActionSnapshot(string functionName, List<StackObject> argValues, object? returnValue)
    {
        FunctionName = functionName;
        ArgValues = argValues;
        ReturnValue = returnValue;
    }
}

