using CSharpKOTOR.Formats.NCS.NCSDecomp.Scriptnode;

namespace CSharpKOTOR.Formats.NCS.NCSDecomp.ScriptNode
{
    public class ABreakStatement : Scriptnode.ScriptNode
    {
        public ABreakStatement()
        {
        }

        public override string ToString()
        {
            return this.tabs + "break;" + this.newline;
        }
    }
}





