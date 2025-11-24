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

            // TODO: Implement full instruction execution
            // This is a placeholder - needs full implementation matching Python interpreter
            throw new NotImplementedException("Full interpreter implementation required. This needs to be ported from Python.");
        }
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
        // TODO: Load actual function definitions for K1/TSL
        // This needs to be implemented to load from scriptdefs
        return new List<ScriptFunction>();
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

