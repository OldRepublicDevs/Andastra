namespace CSharpKOTOR.Formats.NCS.NCSDecomp.ScriptNode
{
    public class AContinueStatement : ScriptNode
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





