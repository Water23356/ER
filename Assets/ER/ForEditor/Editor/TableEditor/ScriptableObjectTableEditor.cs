using ER.Resource;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace ER.ForEditor
{
    public class ScriptableObjectEditorWindow : EditorWindow
    {
        private string directoryPath = "Assets/ScriptableObjects";
        private List<BaseAssetConfigure> assets = new List<BaseAssetConfigure>();
        private Vector2 scrollPosition;
        private FieldInfo[] fieldTypes;
        private bool[] visibles;
        private List<float> columnWidths = new List<float>();

        private float registryNameWidth = 100f;

        //private bool isDragging = false;
        private string newAssetName = "";

        private int rightClickedIndex = -1;

        private int currentPage = 0;
        private int itemsPerPage = 30;

        private bool isRenaming = false;
        private string newName = "";

        private string searchQuery = "";
        private List<int> selectedIndices = new List<int>();

        private Dictionary<Type, FieldInfo[]> cachedFieldTypes = new Dictionary<Type, FieldInfo[]>();

        [MenuItem("Window/ScriptableObject Editor")]
        public static void ShowWindow()
        {
            GetWindow<ScriptableObjectEditorWindow>("ScriptableObject Editor");
        }

        public static void ShowWindow(string path)
        {
            var window = GetWindow<ScriptableObjectEditorWindow>("ScriptableObject Editor");
            window.directoryPath = path;
            window.LoadAssets();
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("ScriptableObject Editor", EditorStyles.boldLabel);

            directoryPath = EditorGUILayout.TextField("Directory Path", directoryPath);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load Assets"))
            {
                LoadAssets();
            }
            if (GUILayout.Button("Save Assets"))
            {
                SaveAssets();
            }
            EditorGUILayout.EndHorizontal();

            searchQuery = EditorGUILayout.TextField("Search", searchQuery);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            if (assets.Count > 0 && fieldTypes != null && fieldTypes.Length != 0)
            {
                DrawColumnWidthControls();
                DrawTableHeader();
                DrawTableRows();
            }

            GUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"Total Assets: {assets.Count}");
            GUILayout.Label($"Page: {currentPage + 1}/{Mathf.CeilToInt(assets.Count / itemsPerPage)}");
            itemsPerPage = Mathf.Max(5, EditorGUILayout.IntField("Items Per Page", itemsPerPage));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Previous Page"))
            {
                if (currentPage > 0)
                {
                    currentPage--;
                }
            }
            if (GUILayout.Button("Next Page"))
            {
                if ((currentPage + 1) * itemsPerPage < assets.Count)
                {
                    currentPage++;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            newAssetName = EditorGUILayout.TextField("New Asset Name", newAssetName);
            if (GUILayout.Button("Create New Asset"))
            {
                CreateNewAsset();
            }
            EditorGUILayout.EndHorizontal();

            if (Event.current.type == EventType.MouseUp)
            {
                //isDragging = false;
                Repaint();
            }

            if (Event.current.type == EventType.ContextClick)
            {
                rightClickedIndex = -1;
                for (int i = 0; i < assets.Count; i++)
                {
                    Rect registryNameRect = GUILayoutUtility.GetLastRect();
                    if (registryNameRect.Contains(Event.current.mousePosition))
                    {
                        rightClickedIndex = i;
                        break;
                    }
                }

                if (rightClickedIndex != -1)
                {
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Rename"), false, RenameAsset);
                    menu.AddItem(new GUIContent("Delete"), false, DeleteAsset);
                    menu.ShowAsContext();
                    Event.current.Use();
                }
            }
        }

        private void LoadAssets()
        {
            assets.Clear();
            string[] guids = AssetDatabase.FindAssets("t:BaseAssetConfigure", new[] { directoryPath });
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                BaseAssetConfigure asset = AssetDatabase.LoadAssetAtPath<BaseAssetConfigure>(path);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }

            if (assets.Count > 0)
            {
                var assetType = assets[0].GetType();
                if (!cachedFieldTypes.TryGetValue(assetType, out fieldTypes))
                {
                    var fields = assetType.GetFields(BindingFlags.Public | BindingFlags.Instance);
                    fieldTypes = FilterFieldInfo(fields);
                    cachedFieldTypes[assetType] = fieldTypes;
                }

                columnWidths.Clear();
                foreach (var field in fieldTypes)
                {
                    if (field.Name != "registryName")
                    {
                        columnWidths.Add(100f);
                    }
                }
                visibles = new bool[fieldTypes.Length];
                for (int i = 0; i < visibles.Length; i++)
                {
                    visibles[i] = true;
                }
            }
        }

        private void SaveAssets()
        {
            foreach (var asset in assets)
            {
                EditorUtility.SetDirty(asset);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void DrawColumnWidthControls()
        {
            EditorGUILayout.BeginHorizontal();
            float newRegistryNameWidth = EditorGUILayout.Slider(registryNameWidth, 50, 300);
            if (newRegistryNameWidth != registryNameWidth)
            {
                //isDragging = true;
                registryNameWidth = newRegistryNameWidth;
            }

            for (int i = 0; i < fieldTypes.Length; i++)
            {
                if (fieldTypes[i].Name != "registryName")
                {
                    float newWidth = EditorGUILayout.Slider(columnWidths[i], 50, 300);
                    if (newWidth != columnWidths[i])
                    {
                        //isDragging = true;
                        columnWidths[i] = newWidth;
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DrawTableHeader()
        {
            EditorGUILayout.BeginHorizontal();
            visibles[0] = GUILayout.Toggle(visibles[0], "", GUILayout.Width(20));
            GUILayout.Label("registryName", EditorStyles.boldLabel, GUILayout.Width(registryNameWidth - 20));

            for (int i = 0; i < fieldTypes.Length; i++)
            {
                if (fieldTypes[i].Name != "registryName")
                {
                    visibles[i] = GUILayout.Toggle(visibles[i], "", GUILayout.Width(20));
                    GUILayout.Label(fieldTypes[i].Name, EditorStyles.boldLabel, GUILayout.Width(columnWidths[i] - 20));
                }
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
        }

        private void DrawTableRows()
        {
            int startIndex = currentPage * itemsPerPage;
            int endIndex = Mathf.Min(startIndex + itemsPerPage, assets.Count);

            for (int i = startIndex; i < endIndex; i++)
            {
                var asset = assets[i];
                SerializedObject serializedObject = new SerializedObject(asset);
                EditorGUILayout.BeginHorizontal();

                Rect registryNameRect = EditorGUILayout.GetControlRect(GUILayout.Width(2));
                registryNameRect.width = registryNameWidth;
                if (visibles[0])
                {
                    if (isRenaming && rightClickedIndex == i)
                    {
                        newName = EditorGUILayout.TextField(newName, GUILayout.Width(registryNameWidth - 40));
                        if (GUILayout.Button("OK", GUILayout.Width(40)))
                        {
                            RenameAssetConfirmed(i);
                        }
                    }
                    else
                    {
                        GUILayout.Label(asset.registryName, EditorStyles.label, GUILayout.Width(registryNameWidth - 2));
                    }
                }
                else
                {
                    GUILayout.Label("-", EditorStyles.label, GUILayout.Width(registryNameWidth - 2));
                }

                if (Event.current.type == EventType.ContextClick && registryNameRect.Contains(Event.current.mousePosition))
                {
                    rightClickedIndex = i;
                    GenericMenu menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Rename"), false, RenameAsset);
                    menu.AddItem(new GUIContent("Delete"), false, DeleteAsset);
                    menu.ShowAsContext();
                    Event.current.Use();
                }
                if (Event.current.type == EventType.MouseDown && registryNameRect.Contains(Event.current.mousePosition) && Event.current.clickCount == 2)
                {
                    Selection.activeObject = asset;
                    EditorGUIUtility.PingObject(asset);
                    Event.current.Use();
                }

                DrawFields(asset, serializedObject);

                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndHorizontal();

                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            }
        }

        private void DrawFields(BaseAssetConfigure asset, SerializedObject serializedObject)
        {
            for (int j = 0; j < fieldTypes.Length; j++)
            {
                if (fieldTypes[j].Name != "registryName")
                {
                    if (visibles[j])
                    {
                        object value = fieldTypes[j].GetValue(asset);
                        object newValue = null;

                        if (fieldTypes[j].FieldType == typeof(int))
                        {
                            newValue = EditorGUILayout.IntField((int)value, GUILayout.Width(columnWidths[j]));
                        }
                        else if (fieldTypes[j].FieldType == typeof(float))
                        {
                            newValue = EditorGUILayout.FloatField((float)value, GUILayout.Width(columnWidths[j]));
                        }
                        else if (fieldTypes[j].FieldType == typeof(string))
                        {
                            newValue = EditorGUILayout.TextField((string)value, GUILayout.Width(columnWidths[j]));
                        }
                        else if (fieldTypes[j].FieldType == typeof(bool))
                        {
                            newValue = EditorGUILayout.Toggle((bool)value, GUILayout.Width(columnWidths[j]));
                        }
                        else if (fieldTypes[j].FieldType.IsEnum)
                        {
                            if (fieldTypes[j].FieldType.GetCustomAttributes(typeof(System.FlagsAttribute), false).Length > 0)
                            {
                                newValue = EditorGUILayout.EnumFlagsField((System.Enum)value, GUILayout.Width(columnWidths[j]));
                            }
                            else
                            {
                                newValue = EditorGUILayout.EnumPopup((System.Enum)value, GUILayout.Width(columnWidths[j]));
                            }
                        }
                        else if (IsVisibleObjectType(fieldTypes[j].FieldType))
                        {
                            SerializedProperty property = serializedObject.FindProperty(fieldTypes[j].Name);
                            EditorGUILayout.ObjectField(property, new GUIContent(""), GUILayout.Width(columnWidths[j]));
                        }
                        else if (fieldTypes[j].FieldType == typeof(Color))
                        {
                            newValue = EditorGUILayout.ColorField((Color)value, GUILayout.Width(columnWidths[j]));
                        }

                        if (newValue != null && !newValue.Equals(value))
                        {
                            fieldTypes[j].SetValue(asset, newValue);
                            EditorUtility.SetDirty(asset);
                        }
                    }
                    else
                    {
                        GUILayout.Label("-", EditorStyles.label, GUILayout.Width(columnWidths[j] - 50));
                    }
                }
            }
        }

        private void CreateNewAsset()
        {
            if (assets.Count > 0)
            {
                var assetType = assets[0].GetType();
                var newAsset = ScriptableObject.CreateInstance(assetType);
                string assetName = string.IsNullOrWhiteSpace(newAssetName) ? "New" + assetType.Name : newAssetName;
                string path = AssetDatabase.GenerateUniqueAssetPath(directoryPath + "/" + assetName + ".asset");

                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                AssetDatabase.CreateAsset(newAsset, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                LoadAssets();
            }
        }

        private void DeleteAsset()
        {
            if (rightClickedIndex != -1)
            {
                var asset = assets[rightClickedIndex];
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                LoadAssets();
            }
        }

        private void RenameAsset()
        {
            if (rightClickedIndex != -1)
            {
                isRenaming = true;
                newName = assets[rightClickedIndex].registryName;
            }
        }

        private void RenameAssetConfirmed(int index)
        {
            if (rightClickedIndex != -1 && !string.IsNullOrEmpty(newName))
            {
                var asset = assets[rightClickedIndex];
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.RenameAsset(path, newName);
                newName = string.Empty;
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                LoadAssets();
                isRenaming = false;
            }
        }

        private bool IsVisibleObjectType(Type type)
        {
            if (type.IsSubclassOf(typeof(UnityEngine.Object)))
            {
                return true;
            }
            if (
                type == typeof(GameObject) ||
               type == typeof(Texture2D) ||
               type == typeof(Material) ||
               type == typeof(AudioClip) ||
               type == typeof(Sprite) ||
               type == typeof(Animation) ||
               type == typeof(Mesh))
            {
                return true;
            }
            return false;
        }

        private bool IsVisibleType(Type type)
        {
            if (type == typeof(int) ||
                type == typeof(float) ||
                type.IsEnum ||
                type == typeof(bool) ||
                type == typeof(string))
            {
                return true;
            }
            if (
                type == typeof(GameObject) ||
                type == typeof(Texture2D) ||
                type == typeof(Material) ||
                type == typeof(AudioClip) ||
                type == typeof(Sprite) ||
                type == typeof(Animation) ||
                type == typeof(Mesh) ||
                type == typeof(Color))
            {
                return true;
            }
            if (type.IsSubclassOf(typeof(ScriptableObject)))
            {
                return true;
            }
            return false;
        }

        private FieldInfo[] FilterFieldInfo(FieldInfo[] infos)
        {
            List<FieldInfo> handled = new List<FieldInfo>();
            foreach (var field in infos)
            {
                if (IsVisibleType(field.FieldType))
                    handled.Add(field);
            }
            return handled.ToArray();
        }

        private void SearchAssets()
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                assets = assets.FindAll(asset => asset.registryName.Contains(searchQuery));
            }
        }

        private void DeleteSelectedAssets()
        {
            foreach (var index in selectedIndices)
            {
                var asset = assets[index];
                string path = AssetDatabase.GetAssetPath(asset);
                AssetDatabase.DeleteAsset(path);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            LoadAssets();
            selectedIndices.Clear();
        }
    }
}
#endif