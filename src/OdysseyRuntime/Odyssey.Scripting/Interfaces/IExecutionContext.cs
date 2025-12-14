using Odyssey.Core.Interfaces;

namespace Odyssey.Scripting.Interfaces
{
    /// <summary>
    /// Execution context for a script run.
    /// </summary>
    public interface IExecutionContext
    {
        /// <summary>
        /// The object running the script (OBJECT_SELF).
        /// </summary>
        IEntity Caller { get; }
        
        /// <summary>
        /// The triggering object (GetEnteringObject, etc.)
        /// </summary>
        IEntity Triggerer { get; }
        
        /// <summary>
        /// The game world.
        /// </summary>
        IWorld World { get; }
        
        /// <summary>
        /// The engine API for ACTION calls.
        /// </summary>
        IEngineApi EngineApi { get; }
        
        /// <summary>
        /// Global script variables.
        /// </summary>
        IScriptGlobals Globals { get; }
        
        /// <summary>
        /// The resource provider for loading scripts.
        /// </summary>
        object ResourceProvider { get; }
        
        /// <summary>
        /// Creates a child context with a new caller.
        /// </summary>
        IExecutionContext WithCaller(IEntity newCaller);
        
        /// <summary>
        /// Creates a child context with a new triggerer.
        /// </summary>
        IExecutionContext WithTriggerer(IEntity newTriggerer);
    }
}

