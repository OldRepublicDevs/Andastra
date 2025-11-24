using System;
using System.Collections.Generic;
using FluentAssertions;
using TSLPatcher.Core.Common;
using TSLPatcher.Core.Formats.NCS;
using TSLPatcher.Core.Formats.NCS.Compiler;
using TSLPatcher.Core.Formats.NCS.Optimizers;
using Xunit;

namespace TSLPatcher.Tests.Formats
{

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
    [Fact]
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

        string source = @"
            void main()
            {
                int value = 3;
                while (value > 0)
                {
                    if (value > 0)
                    {
                        PrintInteger(value);
                        value -= 1;
                    }
                }
            }
        ";

        NCS ncs = NCSAuto.CompileNss(source, Game.K1);
        ncs.Optimize(new List<INCSOptimizer> { new RemoveNopOptimizer() });
        ncs.Print();

        var interpreter = new Interpreter(ncs, Game.K1);
        interpreter.Run();

        interpreter.ActionSnapshots.Count.Should().Be(3);
        interpreter.ActionSnapshots[0].ArgValues[0].Value.Should().Be(3);
        interpreter.ActionSnapshots[1].ArgValues[0].Value.Should().Be(2);
        interpreter.ActionSnapshots[2].ArgValues[0].Value.Should().Be(1);
    }
}
}

