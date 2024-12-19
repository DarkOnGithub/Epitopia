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
        private readonly List<(GUIContent content, bool on, GenericMenu.MenuFunction func, MemberInfo info)> _allNodesInfos = new();
        public override void AddContextMenuItems(GenericMenu menu, Type compatibleType = null, NodePort.IO direction = NodePort.IO.Input)
        {
            base.AddContextMenuItems(menu, compatibleType, direction);
            foreach (var infos in _allNodesInfos)
                menu.AddItem(infos.content, infos.on, infos.func); 
            menu.AddItem(new GUIContent($"Reset"), false, () => NodeEditorWindow.current.graph.Clear());
        }
        private void AddNode(MemberInfo info)
        {
            var graph = NodeEditorWindow.current.graph;
            NodeFactory.CreateNode(graph, info, NodeEditorWindow.current.WindowToGridPosition(Event.current.mousePosition));
        }
        public override void OnCreate()
        {
            base.OnCreate();
            foreach (var node in DensityFunctionManager.GetMembersWithDensityFunctionAttribute())
            {
                var declaringType = node.DeclaringType?.ToString().Split(".").Last();
                _allNodesInfos.Add((new GUIContent($"Nodes/{declaringType}/{node.Name}"), false, () => AddNode(node), node));
            }
        }
    }
}