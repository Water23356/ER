using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using UnityEngine;

namespace ER.StateNodeGraph
{
    // 新增的自定义编辑器类
    [CustomEditor(typeof(StateGraph))]
    public class StateMachineEditorInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            StateGraph graph = (StateGraph)target;
            if (GUILayout.Button("打开编辑窗口"))
            {
                StateNodeGraphEditorWindow.OpenWindow(graph);
            }
        }
    }
}