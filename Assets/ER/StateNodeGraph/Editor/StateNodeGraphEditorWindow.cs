using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ER.StateNodeGraph
{
    public class StateNodeGraphEditorWindow : EditorWindow
    {
        public List<StateKeyPair> nodes = new();
        private Vector2 defaultNodeSize = new Vector2(200, 100);
        private float cell_height = 20f;
        private float distance_waline = 50f;

        private StateGraph graph;

        private StateNode selectedNode = null;
        private Vector2 mousePosition;

        private StateNode creatingTransition = null;
        private Vector2 creatingPos;

        public StateNode GetNode(string key)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].key == key)
                {
                    return nodes[i].value;
                }
            }
            return null;
        }

        public void RemoveNode(string key)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].key == key)
                {
                    nodes.RemoveAt(i);
                    return;
                }
            }
        }

        [MenuItem("Window/敌人行为状态图编辑器")]
        public static void OpenWindow()
        {
            StateNodeGraphEditorWindow window = GetWindow<StateNodeGraphEditorWindow>();
            window.titleContent = new GUIContent("敌人行为状态图编辑器");
            window.graph = null;
            window.LoadStateMachine();
        }

        public static void OpenWindow(StateGraph graph)
        {
            StateNodeGraphEditorWindow window = GetWindow<StateNodeGraphEditorWindow>();
            window.SaveStateMachine();
            window.titleContent = new GUIContent("敌人行为状态图编辑器");
            window.graph = graph;
            window.nodes = graph.nodes;
            window.LoadStateMachine();
        }

        private void OnGUI()
        {
            Event e = Event.current;

            mousePosition = e.mousePosition;
            DrawTool();
            DrawTransitions();
            DrawNodes();

            ProcessNodeEvents(e);
            ProcessEvents(e);

            if (UnityEngine.GUI.changed) Repaint();
        }

        private void DrawTool()
        {
            DrawSaveLoadButtons();
            GUILayout.BeginHorizontal();
            GUILayout.Label($"节点数量: {nodes.Count}");
            GUILayout.EndHorizontal();
        }

        private void DrawNodes()
        {
            foreach (var pair in nodes)
            {
                var node = pair.value;
                // 调整节点区域大小，例如宽度为200，高度为100
                int count = node.nexts.Count;
                node.nodeSize = new Vector2(defaultNodeSize.x, defaultNodeSize.y + count * cell_height);
                GUILayout.BeginArea(new Rect(node.position.x, node.position.y, node.nodeSize.x, node.nodeSize.y), EditorStyles.helpBox);
                node.name = EditorGUILayout.TextField(node.name);
                node.index = EditorGUILayout.IntField("关联行为索引", node.index);

                for (int i = 0; i < node.nexts.Count; i++)
                {
                    var nextNode = node.nexts[i];
                    var nextWeight = 1f;
                    if (i < node.weights.Count)
                    {
                        nextWeight = node.weights[i];
                        node.weights[i] = EditorGUILayout.FloatField($"[{GetNode(node.nexts[i]).name} 权重", node.weights[i]);
                    }
                }

                if (GUILayout.Button("创建新的过渡"))
                {
                    creatingTransition = node;
                    creatingPos = new Vector2(node.position.x, node.position.y) + new Vector2(node.nodeSize.x, cell_height * (node.nexts.Count + 2.5f));
                }
                GUILayout.EndArea();
            }
        }

        private void DrawTransitions()
        {
            foreach (var pair in nodes)
            {
                var node = pair.value;
                Vector2 originCenter = node.position;// + new Vector2(node.nodeSize.x, node.nodeSize.y) / 2; // 默认起始位置
                for (int i = 0; i < node.nexts.Count; i++)
                {
                    bool repeat = false;
                    int index = node.nexts.IndexOf(node.nexts[i]);
                    repeat = index != i;//检测是否重复

                    var nextNode = GetNode(node.nexts[i]);
                    Vector2 endPos = nextNode.position + new Vector2(0, 12); // 终点位置
                    var startPos = originCenter + new Vector2(node.nodeSize.x, cell_height * (i + 2.5f));

                    var direction = (endPos - startPos).normalized;
                    var distance = (endPos - startPos).magnitude;

                    Handles.DrawBezier(
                        startPos,
                        endPos,
                        startPos + Vector2.right * 100,
                        endPos + Vector2.left * 100,
                        repeat ? Color.red : Color.green,
                        null,
                        4f);

                    //DrawArrow((startPos + endPos) / 2, direction, 10f);
                    DrawArrow(startPos + Vector2.right * 10, Vector2.right, 10f);
                    DrawArrow(endPos, Vector2.right, 10f);
                }
            }
        }

        private void DrawArrow(Vector2 position, Vector2 direction, float size)
        {
            Vector3[] arrowVertices = new Vector3[3];
            Vector3 right = new Vector3(direction.y, -direction.x, 0) * size * 0.5f;
            Vector3 left = new Vector3(-direction.y, direction.x, 0) * size * 0.5f;

            arrowVertices[0] = position;
            arrowVertices[1] = position - (Vector2)direction * size + (Vector2)right;
            arrowVertices[2] = position - (Vector2)direction * size + (Vector2)left;

            Handles.DrawAAConvexPolygon(arrowVertices);
        }

        /// <summary>
        /// 按下事件
        /// </summary>
        /// <param name="e"></param>
        private void ProcessEvents(Event e)
        {
            //正在创建新的过渡
            if (creatingTransition != null)
            {
                Handles.DrawBezier(
                    creatingPos,//+ new Vector2(100, 50),
                    mousePosition,
                    creatingPos + Vector2.right * distance_waline,//+ new Vector2(150, 50),
                    mousePosition + Vector2.left * distance_waline,
                    Color.white,
                    null,
                    2f);

                Repaint();
                //Debug.Log($"有创建源");
            }

            if (e.type == EventType.MouseDown && e.button == 0)//左键点击
            {
                //确认点击节点
                var targetNode = GetNodeAtPosition(mousePosition);
                if (creatingTransition != null)//如果处于创建新的过渡状态, 则创建新过渡
                {
                    if (targetNode != null && targetNode != creatingTransition)
                    {
                        creatingTransition.nexts.Add(targetNode.key);
                        if (creatingTransition.weights.Count < creatingTransition.nexts.Count)
                        {
                            creatingTransition.weights.Add(1f);
                        }
                        else
                        {
                            creatingTransition.weights[creatingTransition.nexts.Count - 1] = 1f;
                        }

                        creatingTransition = null; // 完成过渡创建
                    }
                    else
                    {
                        // 如果未找到目标节点或目标节点与起点节点相同，则取消创建
                        creatingTransition = null;
                    }
                }
            }

            if (e.type == EventType.MouseDown && e.button == 1)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("创建新的行为节点"), false, () => AddNode(mousePosition));
                menu.ShowAsContext();
            }

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                selectedNode = GetNodeAtPosition(e.mousePosition);
                if (selectedNode != null && creatingTransition == null)
                {
                    e.Use();
                }
                else
                {
                    dragging = true;
                    e.Use();
                }
            }

            if (e.type == EventType.MouseDrag && e.button == 0)
            {
                foreach (var node in nodes)
                {
                    node.value.position += e.delta;
                }
                e.Use();
            }

            if (dragging && e.type == EventType.MouseUp && e.button == 0)
            {
                dragging = false;
                e.Use();
            }
        }

        private bool dragging = false;

        /// <summary>
        /// 按下节点事件
        /// </summary>
        /// <param name="e"></param>
        private void ProcessNodeEvents(Event e)
        {
            if (selectedNode != null)
            {
                switch (e.type)
                {
                    case EventType.MouseDrag:
                        selectedNode.position += e.delta;
                        e.Use();
                        break;

                    case EventType.MouseUp:
                        selectedNode = null;
                        break;
                }
            }

            if (e.type == EventType.MouseDown && e.button == 1)//右键
            {
                selectedNode = GetNodeAtPosition(e.mousePosition);
                if (selectedNode != null)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("删除该节点"), false, () => RemoveNode(selectedNode));
                    var nodes = selectedNode.nexts.ToArray();
                    foreach (var next in nodes)
                    {
                        menu.AddItem(new GUIContent($"删除对[{GetNode(next).name}]的过渡"), false, () =>
                        {
                            selectedNode.nexts.Remove(next);
                        });
                    }

                    menu.ShowAsContext();
                    e.Use();
                }
            }
        }

        /// <summary>
        /// 获取指定位置上的节点
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private StateNode GetNodeAtPosition(Vector2 position)
        {
            foreach (var p in nodes)
            {
                var node = p.value;
                Rect nodeRect = new Rect(node.position.x, node.position.y, node.nodeSize.x, node.nodeSize.y); // 更新为新的节点区域大小
                if (nodeRect.Contains(position))
                {
                    return node;
                }
            }
            return null;
        }

        private void AddNode(Vector2 position)
        {
            var node = new StateNode()
            {
                name = $"新节点{nodes.Count}",
                index = nodes.Count,
                nexts = new List<string>(),
                nodeSize = defaultNodeSize,
                position = position,
                weights = new List<float>()
            };
            node.key = $"node_{node.GetHashCode()}";
            nodes.Add(new StateKeyPair
            {
                key = node.key,
                value = node,
            });
        }

        private void RemoveNode(StateNode node)
        {
            // 删除与该节点相关的过渡
            foreach (var nd in nodes)
            {
                int index = nd.value.nexts.IndexOf(node.key);
                if (index != -1)
                {
                    nd.value.nexts.RemoveAt(index);
                    if (index >= 0 && index < nd.value.weights.Count)
                    {
                        nd.value.weights.RemoveAt(index);
                    }
                }
            }

            RemoveNode(node.key);
        }

        private void DrawSaveLoadButtons()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("保存状态图"))
            {
                Debug.Log("写入状态图");
                SaveStateMachine();
            }

            if (GUILayout.Button("加载状态图"))
            {
                LoadStateMachine();
            }

            GUILayout.EndHorizontal();
        }

        private void SaveStateMachine()
        {
            if (graph == null && (nodes == null || nodes.Count == 0)) return;

            if (graph == null)
            {
                graph = CreateInstance<StateGraph>();
                AssetDatabase.CreateAsset(graph, "Assets/action_graph.asset");
            }

            graph.nodes = nodes;
            EditorUtility.SetDirty(graph);
            AssetDatabase.SaveAssets();
            Debug.Log($"已保存");
        }

        private void LoadStateMachine()
        {
            //stateMachine = AssetDatabase.LoadAssetAtPath<StateMachine>("Assets/StateMachine.asset");
            if (graph != null)
            {
                nodes = graph.nodes;
            }
        }

        private void OnDestroy()
        {
            SaveStateMachine();
        }
    }
}