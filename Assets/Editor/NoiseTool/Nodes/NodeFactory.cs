using System;
using System.Reflection;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using World.WorldGeneration.DensityFunctions;
using XNodeEditor;

namespace Editor.NoiseTool.Nodes
{
    public static class NodeFactory
    {
        public static void CreateNode(XNode.NodeGraph graph, MemberInfo info, Vector2 at)
        {
            if (info is Type type)
            {
                if (typeof(NoiseFunction).IsAssignableFrom(type))
                {
                    var noiseFunctionInstance = (NoiseFunction)Activator.CreateInstance(type);
                    CreateNoiseNode(graph, noiseFunctionInstance, at);
                }
            }
        }
                
        public static void CreateNoiseNode(XNode.NodeGraph graph, NoiseFunction typeOf, Vector2 at)
        {
            var node = graph.AddNode<NoiseFunctionNode>();
            node.position = at;
            node.noiseFunction = typeOf;
            foreach (var field in typeOf.GetType().GetFields())
            {
                 if(field.GetCustomAttribute(typeof(ReadOnlyAttribute)) != null) node.ReadOnlyFields.Add(field);
                 else node.AddDynamicInput(field.FieldType, fieldName: field.Name);
            }
            node.AddDynamicOutput(typeof(float), fieldName: "output");
            node.Initialize();
        }
    }
}