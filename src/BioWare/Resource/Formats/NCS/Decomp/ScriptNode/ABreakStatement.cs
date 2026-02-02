// Matching NCSDecomp implementation at vendor/NCSDecomp/src/main/java/com/kotor/resource/formats/ncs/ScriptNode/ABreakStatement.java:7-12
// Original: public class ABreakStatement extends ScriptNode
using BioWare.Resource.Formats.NCS.Decomp.ScriptNode;

namespace BioWare.Resource.Formats.NCS.Decomp.ScriptNode
{
    public class ABreakStatement : ScriptNode
    {
        public ABreakStatement()
        {
        }

        // Matching NCSDecomp implementation at vendor/NCSDecomp/src/main/java/com/kotor/resource/formats/ncs/ScriptNode/ABreakStatement.java:8-11
        // Original: @Override public String toString() { return this.tabs + "break;" + this.newline; }
        public override string ToString()
        {
            return this.tabs + "break;" + this.newline;
        }
    }
}





