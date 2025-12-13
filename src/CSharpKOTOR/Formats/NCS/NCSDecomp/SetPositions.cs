// Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/utils/SetPositions.java:14-39
// Original: public class SetPositions extends PrunedReversedDepthFirstAdapter
using CSharpKOTOR.Formats.NCS.NCSDecomp;
using CSharpKOTOR.Formats.NCS.NCSDecomp.Analysis;
using CSharpKOTOR.Formats.NCS.NCSDecomp.AST;

namespace CSharpKOTOR.Formats.NCS.NCSDecomp.Utils
{
    public class SetPositions : PrunedReversedDepthFirstAdapter
    {
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/utils/SetPositions.java:15
        // Original: private NodeAnalysisData nodedata;
        private NodeAnalysisData nodedata;
        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/utils/SetPositions.java:16
        // Original: private int currentPos;
        private int currentPos;

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/utils/SetPositions.java:18-21
        // Original: public SetPositions(NodeAnalysisData nodedata) { this.nodedata = nodedata; this.currentPos = 0; }
        public SetPositions(NodeAnalysisData nodedata)
        {
            this.nodedata = nodedata;
            this.currentPos = 0;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/utils/SetPositions.java:23-25
        // Original: public void done() { this.nodedata = null; }
        public virtual void Done()
        {
            this.nodedata = null;
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/utils/SetPositions.java:27-33
        // Original: @Override public void defaultIn(Node node) { int pos = NodeUtils.getCommandPos(node); if (pos > 0) { this.currentPos = pos; } }
        public override void DefaultIn(Node node)
        {
            int pos = NodeUtils.GetCommandPos(node);
            if (pos > 0)
            {
                this.currentPos = pos;
            }
        }

        // Matching DeNCS implementation at vendor/DeNCS/src/main/java/com/kotor/resource/formats/ncs/utils/SetPositions.java:36-38
        // Original: this.nodedata.setPos(node, this.currentPos);
        public override void DefaultOut(Node node)
        {
            // For ASubroutine nodes, try to get position from first command if currentPos is 0
            // Since we're using reversed depth-first traversal, children are visited first,
            // so positions should already be set on command nodes
            if (typeof(ASubroutine).IsInstanceOfType(node) && this.currentPos == 0)
            {
                Node firstCmd = NodeUtils.GetCommandChild(node);
                if (firstCmd != null)
                {
                    // Try to get position that was already set on the first command
                    int existingPos = this.nodedata.TryGetPos(firstCmd);
                    if (existingPos > 0)
                    {
                        this.currentPos = existingPos;
                    }
                    else
                    {
                        // Fallback: try GetCommandPos if position wasn't set yet
                        int cmdPos = NodeUtils.GetCommandPos(firstCmd);
                        if (cmdPos > 0)
                        {
                            this.currentPos = cmdPos;
                        }
                    }
                }
            }
            this.nodedata.SetPos(node, this.currentPos);
        }
    }
}




