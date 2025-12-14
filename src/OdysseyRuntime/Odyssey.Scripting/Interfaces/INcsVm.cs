namespace Odyssey.Scripting.Interfaces
{
    /// <summary>
    /// NWScript compiled script virtual machine.
    /// </summary>
    public interface INcsVm
    {
        /// <summary>
        /// Executes a script and returns the result.
        /// </summary>
        int Execute(byte[] ncsBytes, IExecutionContext ctx);
        
        /// <summary>
        /// Executes a script by resource reference.
        /// </summary>
        int ExecuteScript(string resRef, IExecutionContext ctx);
        
        /// <summary>
        /// Aborts the currently executing script.
        /// </summary>
        void Abort();
        
        /// <summary>
        /// Whether a script is currently running.
        /// </summary>
        bool IsRunning { get; }
        
        /// <summary>
        /// Gets the total instructions executed in the current run.
        /// </summary>
        int InstructionsExecuted { get; }
        
        /// <summary>
        /// Maximum instructions per execution (budget).
        /// </summary>
        int MaxInstructions { get; set; }
        
        /// <summary>
        /// Whether to enable instruction tracing.
        /// </summary>
        bool EnableTracing { get; set; }
    }
}

