namespace TSLPatcher.Core.Formats.NCS;

/// <summary>
/// Base interface for NCS bytecode optimizers.
/// </summary>
public interface INCSOptimizer
{
    int InstructionsCleared { get; }
    void Optimize(NCS ncs);
    void Reset();
}

/// <summary>
/// Base class for NCS optimizers with common functionality.
/// </summary>
public abstract class NCSOptimizerBase : INCSOptimizer
{
    public int InstructionsCleared { get; protected set; }

    public abstract void Optimize(NCS ncs);

    public virtual void Reset()
    {
        InstructionsCleared = 0;
    }
}

