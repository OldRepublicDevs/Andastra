using System.Text;

namespace CSharpKOTOR.Formats.NCS.NCSDecomp.ScriptNode
{
    public class ADoLoop : AControlLoop
    {
        public ADoLoop() : this(0, 0)
        {
        }

        public ADoLoop(int start, int end) : base(start, end)
        {
        }

        public override string ToString()
        {
            var buff = new StringBuilder();
            var condition = GetCondition();
            buff.Append(this.tabs);
            buff.Append("do {" + this.newline);
            foreach (var child in GetChildren())
            {
                buff.Append(child.ToString());
            }
            buff.Append(this.tabs + "} while (" + (condition != null ? condition.ToString() : "") + ");" + this.newline);
            return buff.ToString();
        }
    }
}





