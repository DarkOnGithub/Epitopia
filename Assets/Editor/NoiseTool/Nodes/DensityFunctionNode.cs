using System;
using Codice.CM.Common.Serialization;
using XNode;

namespace Editor.NoiseTool.Nodes
{
    public class DensityFunctionNodes : Node
    {
        public Type densityFunction;
        [Output] public float output;
        public override object GetValue(NodePort port)
        {
            output = 10;
            return 0;
        }

        public DensityFunctionNodes()
        {
            
        }
    }
}