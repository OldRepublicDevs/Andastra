using Odyssey.Core.Interfaces;
using Odyssey.Scripting.Interfaces;

namespace Odyssey.Scripting.VM
{
    /// <summary>
    /// Execution context for a script run.
    /// </summary>
    public class ExecutionContext : IExecutionContext
    {
        public ExecutionContext(IEntity caller, IWorld world, IEngineApi engineApi, IScriptGlobals globals)
        {
            Caller = caller;
            World = world;
            EngineApi = engineApi;
            Globals = globals;
        }
        
        public IEntity Caller { get; }
        public IEntity Triggerer { get; private set; }
        public IWorld World { get; }
        public IEngineApi EngineApi { get; }
        public IScriptGlobals Globals { get; }
        public object ResourceProvider { get; set; }
        
        public IExecutionContext WithCaller(IEntity newCaller)
        {
            var ctx = new ExecutionContext(newCaller, World, EngineApi, Globals);
            ctx.Triggerer = Triggerer;
            ctx.ResourceProvider = ResourceProvider;
            return ctx;
        }
        
        public IExecutionContext WithTriggerer(IEntity newTriggerer)
        {
            var ctx = new ExecutionContext(Caller, World, EngineApi, Globals);
            ctx.Triggerer = newTriggerer;
            ctx.ResourceProvider = ResourceProvider;
            return ctx;
        }
        
        /// <summary>
        /// Sets the triggerer for this context.
        /// </summary>
        public void SetTriggerer(IEntity triggerer)
        {
            Triggerer = triggerer;
        }
    }
}

