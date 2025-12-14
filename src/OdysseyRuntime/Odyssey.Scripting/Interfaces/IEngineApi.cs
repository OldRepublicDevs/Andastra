using System.Collections.Generic;

namespace Odyssey.Scripting.Interfaces
{
    /// <summary>
    /// Engine function dispatch interface for NWScript ACTION calls.
    /// </summary>
    public interface IEngineApi
    {
        /// <summary>
        /// Calls an engine function by routine ID.
        /// </summary>
        Variable CallEngineFunction(int routineId, IReadOnlyList<Variable> args, IExecutionContext ctx);
        
        /// <summary>
        /// Gets the name of an engine function by routine ID.
        /// </summary>
        string GetFunctionName(int routineId);
        
        /// <summary>
        /// Gets the expected argument count for a function.
        /// </summary>
        int GetArgumentCount(int routineId);
        
        /// <summary>
        /// Whether the function is implemented.
        /// </summary>
        bool IsImplemented(int routineId);
    }
}

