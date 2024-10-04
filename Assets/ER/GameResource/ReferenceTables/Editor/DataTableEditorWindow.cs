namespace ER.ResourceManager
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    public class DataTableEditorWindow : EditorWindow
    {
        private RefDataTable dataTable;
        private Vector2 scrollPosition;
        private List<Object> previousAssetReferences;
        private float[] columnWidths = new float[] { 250f, 250f };
        private bool[] locked = new bool[] { false, false };

        private int currentPage = 0;
        private int itemsPerPage = 15;

        [MenuItem("Window/DataTable Editor")]
        public static void ShowWindow()
        {
            GetWindow<DataTableEditorWindow>("路径表编辑器");
        }

        public static void OpenWindow(RefDataTable table)
        {
            var win = GetWindow<DataTableEditorWindow>("路径表编辑器");
            win.dataTable = table;
            win.Show();
        }

        private void OnEnable()
        {
            if (dataTable == null)
            {
                dataTable = ScriptableObject.CreateInstance<RefDataTable>();
            }

            // 初始化 previousAssetReferences 列表
            previousAssetReferences = new List<Object>();
            foreach (var row in dataTable.rows)
            {
                previousAssetReferences.Add(row.assetReference);
            }
        }

        private void OnGUI()
        {
            Event e = Event.current;
            if (dataTable == null)
            {
                EditorGUILayout.LabelField("DataTable is null. Please create a new DataTable asset.");
                return;
            }

            DrawColumnWidthControls();
            DrawHead();

            DrawTable();
            DrawPage();
            DrawButton();
            ProcessEvents(e);
        }

        private void DrawPage()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"总条目: {dataTable.rows.Count}");
            GUILayout.Label($"页码: {currentPage + 1}/{Mathf.CeilToInt(dataTable.rows.Count / itemsPerPage) + 1}");
            itemsPerPage = Mathf.Max(5, EditorGUILayout.IntField("每页数量", itemsPerPage));

            if (GUILayout.Button("<"))
            {
                if (currentPage > 0)
                {
                    currentPage--;
                }
            }
            if (GUILayout.Button(">"))
            {
                if ((currentPage + 1) * itemsPerPage < dataTable.rows.Count)
                {
                    currentPage++;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void ProcessEvents(Event e)
        {
            if (e.type == EventType.MouseDown)
            {
                UnityEngine.GUI.FocusControl(null);
                e.Use();
            }
        }

        private void DrawColumnWidthControls()
        {
            EditorGUILayout.BeginHorizontal();
            columnWidths[0] = EditorGUILayout.Slider(columnWidths[0], 50, 300);
            columnWidths[1] = EditorGUILayout.Slider(columnWidths[1], 50, 300);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawHead()
        {
            // 显示表头
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            EditorGUILayout.LabelField("注册名", EditorStyles.boldLabel, GUILayout.Width(columnWidths[0] - 62));
            if (GUILayout.Button(locked[0] ? "已上锁" : "已解锁", GUILayout.Width(60), GUILayout.Height(18)))
            {
                locked[0] = !locked[0];
            }
            EditorGUILayout.LabelField("加载路径", EditorStyles.boldLabel, GUILayout.Width(columnWidths[1] - 62));
            if (GUILayout.Button(locked[1] ? "已上锁" : "已解锁", GUILayout.Width(60), GUILayout.Height(18)))
            {
                locked[1] = !locked[1];
            }
            EditorGUILayout.LabelField("资产引用", EditorStyles.boldLabel, GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
        }

        private Object GetOrCreatePreviousAssetReferences(int index)
        {
            while (index >= previousAssetReferences.Count)
            {
                previousAssetReferences.Add(null);
            }
            return previousAssetReferences[index];
        }

        private void DrawTable()
        {
            if (dataTable.rows.Count == 0) return;
            int startIndex = Mathf.Clamp(currentPage * itemsPerPage, 0, dataTable.rows.Count - 1);
            int endIndex = Mathf.Min(startIndex + itemsPerPage, dataTable.rows.Count);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            EditorGUILayout.BeginVertical();
            // 显示数据行
            for (int i = startIndex; i < endIndex; i++)
            {
                EditorGUILayout.BeginHorizontal();

                dataTable.rows[i].regName = EditorGUILayout.TextField(dataTable.rows[i].regName, GUILayout.Width(columnWidths[0]));
                dataTable.rows[i].loadPath = EditorGUILayout.TextField(dataTable.rows[i].loadPath, GUILayout.Width(columnWidths[1]));
                Object newAssetReference = EditorGUILayout.ObjectField(dataTable.rows[i].assetReference, typeof(Object), false, GUILayout.Width(100));

                // 检测资产引用的变化
                if (newAssetReference != GetOrCreatePreviousAssetReferences(i))
                {
                    Debug.Log($"$资产更新 !:{newAssetReference != null}");
                    dataTable.rows[i].assetReference = newAssetReference;
                    Refresh(dataTable.rows[i]);

                    // 更新 previousAssetReferences 列表
                    previousAssetReferences[i] = newAssetReference;
                }

                if (GUILayout.Button("X", GUILayout.Width(18), GUILayout.Height(18)))
                {
                    dataTable.rows.RemoveAt(i);
                    previousAssetReferences.RemoveAt(i);

                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndScrollView();
                    return;

                }

                EditorGUILayout.EndHorizontal();
            }


            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(dataTable);
            }
        }

        private void DrawButton()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加"))
            {
                dataTable.rows.Add(new RefTableRow());
                previousAssetReferences.Add(null);
            }
            if (GUILayout.Button("刷新"))
            {
                Refresh();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("保存"))
            {
                SaveAssets();
            }
            EditorGUILayout.EndVertical();
        }

        private void Refresh()
        {
            foreach (var row in dataTable.rows)
            {
                Refresh(row);
            }
        }

        private void Refresh(RefTableRow row, bool ignoreLock = false)
        {
            var refernce = row.assetReference;
            if (refernce != null)
            {
                string addressablePath = GetAddressablePath(refernce);
                if (ignoreLock || !locked[1])
                    row.loadPath = addressablePath;
                if (ignoreLock || !locked[0])
                    row.regName = GR.KeyToRegName(addressablePath);
            }
            else
            {
                if (ignoreLock || !locked[1])
                    row.loadPath = string.Empty;
            }
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private string GetAddressablePath(Object origin)
        {
            string assetPath = AssetDatabase.GetAssetPath(origin);
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            
            AddressableAssetEntry entry = settings?.FindAssetEntry(guid);
            if (entry == null)
            {
                return assetPath;
            }
            string address = entry.address;
            entry.address = GR.ADToKey(address);
            EditorUtility.SetDirty(origin);

            return entry.address;
        }

        public void AddRefData(List<Object> objects)
        {
            ClearEmpty();
            foreach (var obj in objects)
            {
                Debug.Log($"添加索引行: {obj}");
                var row = new RefTableRow();
                dataTable.rows.Add(row);
                row.assetReference = obj;
                previousAssetReferences.Add(obj);
                Refresh(row, true);
            }
        }

        //移除无效键值对
        private void ClearEmpty()
        {
            for (int i = 0; i < dataTable.rows.Count; i++)
            {
                if (string.IsNullOrEmpty(dataTable.rows[i].regName))
                {
                    dataTable.rows.RemoveAt(i);
                    if (i < previousAssetReferences.Count)
                        previousAssetReferences.RemoveAt(i);
                    i--;
                }
            }
        }

        private void SaveAssets()
        {
            ClearEmpty();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void OnDestroy()
        {
            SaveAssets();
        }
    }
}