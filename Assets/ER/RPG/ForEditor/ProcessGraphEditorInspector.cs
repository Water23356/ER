using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ER.RPG
{
    // 新增的自定义编辑器类
    [CustomEditor(typeof(ProcessGraph))]
    public class ProcessGraphEditorInspector : Editor
    {
        private List<Type> graphNodeTypes;
        private string[] typeNames;
        private int selectedIndex;

        private void OnEnable()
        {
            // 获取所有带有 GraphNodeInfoAttribute 的类型
            graphNodeTypes = FindAllGraphNodeInfoTypes();

            // 将类型的名称存入字符串数组，供下拉框显示
            typeNames = graphNodeTypes.Select(t => t.Name).ToArray();

            // 确定当前选中的类型（如果有的话）
            ProcessGraph graph = (ProcessGraph)target;
            if (graph.infoType != null)
            {
                selectedIndex = Array.IndexOf(graphNodeTypes.ToArray(), graph.infoType);
            }
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
            ProcessGraph graph = (ProcessGraph)target;

            // 如果没有类型，提示用户
            if (typeNames.Length == 0)
            {
                EditorGUILayout.LabelField("No types found with [GraphNodeInfoAttribute].");
            }

            // 下拉框选择
            selectedIndex = EditorGUILayout.Popup("携带信息类型", selectedIndex, typeNames);

            // 如果用户选择了新的类型，更新 infoType
            if (selectedIndex >= 0 && selectedIndex < graphNodeTypes.Count)
            {
                graph.infoType = graphNodeTypes[selectedIndex];
                EditorUtility.SetDirty(target); // 标记对象已更改，确保保存
            }

            // 其他绘制逻辑（如显示选中类型的额外信息）
            if (graph.infoType != null)
            {
                GraphNodeInfoAttribute attribute = (GraphNodeInfoAttribute)Attribute.GetCustomAttribute(graph.infoType, typeof(GraphNodeInfoAttribute));
                EditorGUILayout.LabelField("已选择: " + graph.infoType.FullName);
                EditorGUILayout.LabelField("描述: " + attribute.Description);
            }

            if (GUILayout.Button("打开编辑窗口"))
            {
                ProcessGraphEditorWindow.OpenWindow(graph);
            }
        }

        private List<Type> FindAllGraphNodeInfoTypes()
        {
            // 获取当前程序集中的所有类型
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();

            // 查找带有 [GraphNodeInfoAttribute] 特性的类型
            var filteredTypes = allTypes
                .Where(t => t.IsClass && t.GetCustomAttributes(typeof(GraphNodeInfoAttribute), false).Length > 0)
                .ToList();

            return filteredTypes;
        }
    }
}