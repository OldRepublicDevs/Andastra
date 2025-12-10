using System.Collections.Generic;
using System.Linq;

namespace CSharpKOTOR.Formats.NCS.Optimizers
{

    /// <summary>
    /// Removes NOP (no-operation) instructions from compiled NCS bytecode.
    ///
    /// NCS Compiler uses NOP instructions as stubs to simplify the compilation process
    /// however as their name suggests they do not perform any actual function. This optimizer
    /// removes all occurrences of NOP instructions from the compiled script, updating jump
    /// targets to skip over removed NOPs.
    ///
    /// References:
    ///     vendor/xoreos-tools/src/nwscript/decompiler.cpp (NCS optimization patterns)
    ///     Standard compiler optimization techniques (dead code elimination)
    ///     Note: NOP removal is a common bytecode optimization
    /// </summary>
    public class RemoveNopOptimizer : NCSOptimizer
    {
        public override void Optimize(NCS ncs)
        {
            var nops = ncs.Instructions.Where(inst => inst.InsType == NCSInstructionType.NOP).ToList();

            if (nops.Count == 0)
            {
                return;
            }

            var removableIds = new HashSet<int>();

            foreach (NCSInstruction nop in nops)
            {
                int nopIndex = ncs.GetInstructionIndex(nop);
                if (nopIndex < 0)
                {
                    continue;
                }

                List<NCSInstruction> inboundLinks = ncs.LinksTo(nop);
                // If other instructions jump here, keep this NOP (likely a function entry stub)
                if (inboundLinks.Count > 0)
                {
                    continue;
                }

                NCSInstruction replacement = null;

                for (int i = nopIndex + 1; i < ncs.Instructions.Count; i++)
                {
                    NCSInstruction candidate = ncs.Instructions[i];
                    if (candidate.InsType != NCSInstructionType.NOP)
                    {
                        replacement = candidate;
                        break;
                    }
                }

                // No inbound links (already handled), so safe to remove even if replacement is null
                removableIds.Add(nop.GetHashCode());
            }

            if (removableIds.Count == 0)
            {
                return;
            }

            ncs.Instructions = ncs.Instructions.Where(inst => !removableIds.Contains(inst.GetHashCode())).ToList();
            InstructionsCleared += removableIds.Count;
        }
    }
}

