namespace CSharpKOTOR.Formats.NCS.NCSDecomp.ScriptNode
{
    public class ABreakStatement : ScriptNode
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





