using System.Collections.Generic;

namespace Andastra.Game.Scripting.Abstractions
{
    /// <summary>
    /// Engine function dispatch interface for NWScript ACTION calls.
    /// Execution context is passed as object to avoid Runtime dependency; implementers cast to IExecutionContext.
    /// </summary>
    public interface IEngineApi
    {
        Variable CallEngineFunction(int routineId, IReadOnlyList<Variable> args, object executionContext);
        string GetFunctionName(int routineId);
        int GetArgumentCount(int routineId);
        bool IsImplemented(int routineId);
    }
}
