using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ER.RPG
{
    public class ProcessGraphEditorWindow : EditorWindow
    {
        private Vector2 defaultNodeSize = new Vector2(200, 100);
        private float cellHeight = 20f;
        private float distanceWaline = 50f;

        private ProcessGraph graph;
        private ProcessGraphNode selectedNode = null;
        private ProcessGraphNode displayNode = null;
        private Vector2 mousePosition;

        private ProcessGraphNode creatingTransition = null;
        private Vector2 creatingPos;

        private Texture2D texUpArrow;
        private Texture2D texDownArrow;

        private FieldInfo[] fields;
        private Vector2 scrollPosition;

        private void OnEnable()
        {
            texUpArrow = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ER/Assets/Textures/uparrow.png");
            texDownArrow = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ER/Assets/Textures/downarrow.png");
        }

        public ProcessGraphNode GetNode(string key)
        {
            return graph.GetNode(key);
        }

        public void RemoveNode(string key)
        {
            graph.RemoveNode(key);
        }

        [MenuItem("Window/ER/流程图")]
        public static void OpenWindow()
        {
            ProcessGraphEditorWindow window = GetWindow<ProcessGraphEditorWindow>();
            window.titleContent = new GUIContent("ER/流程图");
            window.graph = null;
            window.LoadStateMachine();
        }

        public static void OpenWindow(ProcessGraph graph)
        {
            ProcessGraphEditorWindow window = GetWindow<ProcessGraphEditorWindow>();
            window.SaveStateMachine();
            window.titleContent = new GUIContent("ER/流程图");
            window.graph = graph;
            window.LoadFields(graph.infoType);
            window.LoadStateMachine();
        }

        private void LoadFields(Type type)
        {
            fields = type.GetFields();
        }

        private void OnGUI()
        {
            if (graph == null) return;

            Event e = Event.current;
            mousePosition = e.mousePosition;


            DrawTool();
            DrawTransitions();
            DrawNodes();
            DrawInfosBox();

            ProcessNodeEvents(e);
            ProcessEvents(e);

            if (UnityEngine.GUI.changed) Repaint();
        }

        private void DrawTool()
        {
            DrawSaveLoadButtons();
            GUILayout.Label($"节点数量: {graph.nodes.Count}");
        }

        private void DrawNodes()
        {
            foreach (var node in graph)
            {
                int count = node.childs.Count;
                node.nodeSize = new Vector2(defaultNodeSize.x, defaultNodeSize.y + count * cellHeight);
                GUILayout.BeginArea(new Rect(node.position.x, node.position.y, node.nodeSize.x, node.nodeSize.y), UnityEngine.GUI.skin.window);

                DrawNodeHeader(node);
                DrawNodeChildren(node);

                if (GUILayout.Button("创建新的过渡"))
                {
                    creatingTransition = node;
                    creatingPos = new Vector2(node.position.x, node.position.y) + new Vector2(node.nodeSize.x, cellHeight * (node.childs.Count + 2.5f));
                }
                GUILayout.EndArea();
            }
        }

        private void DrawNodeHeader(ProcessGraphNode node)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("节点名称", GUILayout.Width(60));
            node.name = EditorGUILayout.TextField(node.name);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("默认状态", GUILayout.Width(60));
            node.defaultStatus = (Node.Status)EditorGUILayout.EnumPopup(node.defaultStatus);
            GUILayout.EndHorizontal();
        }

        private void DrawNodeChildren(ProcessGraphNode node)
        {
            for (int i = 0; i < node.childs.Count; i++)
            {
                var nextNode = node.childs[i];
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"[{i}] {nextNode}", GUILayout.Width(150));

                if (GUILayout.Button("", GUILayout.Width(15), GUILayout.Height(15)))
                {
                    if (i > 0)
                    {
                        ChangeIndexPos(node, i, i - 1);
                    }
                }
                DrawArrowButton(texUpArrow);

                if (GUILayout.Button("", GUILayout.Width(15), GUILayout.Height(15)))
                {
                    if (i < node.childs.Count - 1)
                    {
                        ChangeIndexPos(node, i, i + 1);
                    }
                }
                DrawArrowButton(texDownArrow);

                GUILayout.EndHorizontal();
            }
        }

        private void DrawArrowButton(Texture2D texture)
        {
            Rect lastRect = GUILayoutUtility.GetLastRect();
            if (texture != null)
            {
                UnityEngine.GUI.DrawTexture(lastRect, texture, ScaleMode.StretchToFill);
            }
        }

        private void ChangeIndexPos(ProcessGraphNode node, int a, int b)
        {
            string tmp = node.childs[a];
            node.childs[a] = node.childs[b];
            node.childs[b] = tmp;
        }

        private void DrawTransitions()
        {
            foreach (var node in graph)
            {
                Vector2 originCenter = node.position;
                for (int i = 0; i < node.childs.Count; i++)
                {
                    bool repeat = node.childs.IndexOf(node.childs[i]) != i;
                    var nextNode = GetNode(node.childs[i]);
                    Vector2 endPos = nextNode.position + new Vector2(0, 12);
                    var startPos = originCenter + new Vector2(node.nodeSize.x, cellHeight * (i + 2.5f));

                    Handles.DrawBezier(
                        startPos,
                        endPos,
                        startPos + Vector2.right * 60,
                        endPos + Vector2.left * 60,
                        repeat ? Color.red : Color.green,
                        null,
                        4f);

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

        private void DrawInfosBox()
        {
            float height = position.height - 80;
            GUILayout.BeginArea(new Rect(5, 60, 300, height), UnityEngine.GUI.skin.window);
            GUILayout.BeginVertical();

            if (displayNode != null)
            {
                DrawNodeHeader(displayNode);

                DrawCustomSeparator(Color.gray, 1);
                EditorGUILayout.LabelField("携带信息类型: "+graph.infoType.FullName);
                DrawCustomSeparator(Color.gray, 1);


                scrollPosition = GUILayout.BeginScrollView(scrollPosition);
                if (graph.infoType != null)
                {
                    if (displayNode.extraInfos == null)
                    {
                        displayNode.extraInfos = Activator.CreateInstance(graph.infoType);
                    }
                    else if(displayNode.extraInfos.GetType()!= graph.infoType)//如果值类型不匹配也需要重新赋值
                    {
                        displayNode.extraInfos = Activator.CreateInstance(graph.infoType);
                    }

                    DrawFields(displayNode.extraInfos);
                }
                GUILayout.EndScrollView();
            }

            GUILayout.EndVertical();
            GUILayout.EndArea();
        }

        private void DrawCustomSeparator(Color color, int thickness)
        {
            GUILayout.Space(10); // 添加一些空间以分隔内容

            // 获取当前 Rect 并设置为指定的高度
            var rect = EditorGUILayout.GetControlRect(false, thickness);

            // 绘制指定颜色和厚度的分隔线
            EditorGUI.DrawRect(rect, color);

            GUILayout.Space(10); // 添加一些空间以分隔内容
        }

        private void DrawFields(object obj)
        {
            if (fields == null) return;
            foreach (var field in fields)
            {
                var fieldName = field.Name;
                object value = field.GetValue(obj);
                object newValue = null;

                if (field.FieldType == typeof(int))
                {
                    newValue = EditorGUILayout.IntField(fieldName, (int)value);
                }
                else if (field.FieldType == typeof(float))
                {
                    newValue = EditorGUILayout.FloatField(fieldName, (float)value);
                }
                else if (field.FieldType == typeof(string))
                {
                    newValue = EditorGUILayout.TextField(fieldName, (string)value);
                }
                else if (field.FieldType == typeof(bool))
                {
                    newValue = EditorGUILayout.Toggle(fieldName, (bool)value);
                }
                else if (field.FieldType.IsEnum)
                {
                    if (field.FieldType.GetCustomAttributes(typeof(System.FlagsAttribute), false).Length > 0)
                    {
                        newValue = EditorGUILayout.EnumFlagsField(fieldName, (System.Enum)value);
                    }
                    else
                    {
                        newValue = EditorGUILayout.EnumPopup(fieldName, (System.Enum)value);
                    }
                }
                else if (field.FieldType == typeof(Color))
                {
                    newValue = EditorGUILayout.ColorField(fieldName, (Color)value);
                }

                if (newValue != null && !newValue.Equals(value))
                {
                    field.SetValue(obj, newValue);
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            if (creatingTransition != null)
            {
                Handles.DrawBezier(
                    creatingPos,
                    mousePosition,
                    creatingPos + Vector2.right * distanceWaline,
                    mousePosition + Vector2.left * distanceWaline,
                    Color.white,
                    null,
                    2f);

                Repaint();
            }

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                var targetNode = GetNodeAtPosition(mousePosition);
                if (creatingTransition != null)
                {
                    if (targetNode != null && targetNode != creatingTransition)
                    {
                        creatingTransition.childs.Add(targetNode.name);
                        targetNode.parent = creatingTransition.name;
                        creatingTransition = null;
                    }
                    else
                    {
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
                if (selectedNode != null)
                {
                    displayNode = selectedNode;
                    UnityEngine.GUI.FocusControl(null);
                }
            }

            if (e.type == EventType.MouseDrag && e.button == 0)
            {
                foreach (var node in graph)
                {
                    node.position += e.delta;
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

            if (e.type == EventType.MouseDown && e.button == 1)
            {
                selectedNode = GetNodeAtPosition(e.mousePosition);
                if (selectedNode != null)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("删除该节点"), false, () => RemoveNode(selectedNode));
                    var nodes = selectedNode.childs.ToArray();
                    foreach (var next in nodes)
                    {
                        menu.AddItem(new GUIContent($"删除对[{GetNode(next).name}]的过渡"), false, () =>
                        {
                            selectedNode.childs.Remove(next);
                        });
                    }

                    menu.ShowAsContext();
                    e.Use();
                }
            }
        }

        private ProcessGraphNode GetNodeAtPosition(Vector2 position)
        {
            foreach (var node in graph)
            {
                Rect nodeRect = new Rect(node.position.x, node.position.y, node.nodeSize.x, node.nodeSize.y);
                if (nodeRect.Contains(position))
                {
                    return node;
                }
            }
            return null;
        }

        private void AddNode(Vector2 position)
        {
            var node = new ProcessGraphNode()
            {
                name = $"新节点{graph.nodes.Count}",
                childs = new List<string>(),
                nodeSize = defaultNodeSize,
                position = position
            };
            graph.nodes.Add(node);
        }

        private void RemoveNode(ProcessGraphNode node)
        {
            foreach (var nd in graph)
            {
                int index = nd.childs.IndexOf(node.name);
                if (index != -1)
                {
                    nd.childs.RemoveAt(index);
                }
            }
            foreach(var next in node.childs)
            {
                var sub = GetNode(next);
                sub.parent = string.Empty;
            }
            RemoveNode(node.name);
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
            if (graph == null) return;

            if (graph == null)
            {
                graph = CreateInstance<ProcessGraph>();
                AssetDatabase.CreateAsset(graph, "Assets/action_graph.asset");
            }

            EditorUtility.SetDirty(graph);
            AssetDatabase.SaveAssets();
            Debug.Log($"已保存");
        }

        private void LoadStateMachine()
        {
            // Implement loading logic here
        }

        private void OnDestroy()
        {
            SaveStateMachine();
        }
    }
}