using System;
using FluentAssertions;
using TSLPatcher.Core.Formats.NCS;
using Xunit;

namespace TSLPatcher.Tests.Formats;

/// <summary>
/// Tests for NCS optimizers.
/// 1:1 port of test_ncs_optimizer.py from tests/resource/formats/test_ncs_optimizer.py
/// 
/// NOTE: These tests require NSS compilation and NCS optimizer functionality.
/// </summary>
public class NCSOptimizerTests
{
    /// <summary>
    /// Python: test_no_op_optimizer
    /// </summary>
    [Fact(Skip = "Requires NSS compilation and optimizer functionality")]
    public void TestNoOpOptimizer()
    {
        // Python test:
        // ncs = self.compile("""
        //     void main()
        //     {
        //         int value = 3;
        //         while (value > 0)
        //         {
        //             if (value > 0)
        //             {
        //                 PrintInteger(value);
        //                 value -= 1;
        //             }
        //         }
        //     }
        // """)
        //
        // ncs.optimize([RemoveNopOptimizer()])
        // ncs.print()
        //
        // interpreter = Interpreter(ncs)
        // interpreter.run()
        //
        // assert len(interpreter.action_snapshots) == 3
        // assert interpreter.action_snapshots[0].arg_values[0] == 3
        // assert interpreter.action_snapshots[1].arg_values[0] == 2
        // assert interpreter.action_snapshots[2].arg_values[0] == 1

        // TODO: Implement when compile, optimize, and interpreter are available
    }
}

