using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Editor.NoiseTool.Nodes;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using World.WorldGeneration.DensityFunctions;
using XNode;
using XNodeEditor;
using XNodeEditor.Internal;
using Type = System.Type;

namespace Editor.NoiseTool
{
    
    [NodeGraphEditor.CustomNodeGraphEditor(typeof(NodeGraph))]
    public class GraphEditor : NodeGraphEditor
    {
        private List<(GUIContent content, bool on, GenericMenu.MenuFunction func, MemberInfo info)> _allNodesInfos = new();
        public override void AddContextMenuItems(GenericMenu menu, Type compatibleType = null, NodePort.IO direction = NodePort.IO.Input)
        {
            base.AddContextMenuItems(menu, compatibleType, direction);
            foreach (var infos in _allNodesInfos)
                menu.AddItem(infos.content, infos.on, infos.func); 
        }

        private void AddNode<T>(MemberInfo info)
        {
            var graph = NodeEditorWindow.current.graph;
            Type genericType = info.GetType().MakeGenericType(new Type[] { info.GetType() });
            // Get the 'B' method and invoke it:
            object res = genericType.GetMethod("B").Invoke(new object[] { o });
            graph.AddNode(new DensityFunctionNodes<info>())
        }
        public override void OnCreate()
        {
            base.OnCreate();
            foreach (var node in DensityFunctionManager.GetMembersWithDensityFunctionAttribute())
            {
                var declaringType = node.DeclaringType.ToString().Split(".").Last();
                if (node.IsStruct())
                    _allNodesInfos.Add((new GUIContent($"Nodes/{declaringType}/{node.Name}"), false, () => Debug.Log(node.Name), node));
                
            }

        }
    }
}