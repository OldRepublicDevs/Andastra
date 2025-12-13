using CSharpKOTOR.Formats.NCS.NCSDecomp.Scriptnode;

namespace CSharpKOTOR.Formats.NCS.NCSDecomp.ScriptNode
{
    public class AContinueStatement : Scriptnode.ScriptNode
    {
        public AContinueStatement()
        {
        }

        public override string ToString()
        {
            return this.tabs + "continue;" + this.newline;
        }
    }
}





