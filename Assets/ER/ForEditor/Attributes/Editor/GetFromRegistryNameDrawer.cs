using Dev;
using UnityEditor;
using UnityEngine;

namespace ER.ForEditor
{
    [CustomPropertyDrawer(typeof(GetFromRegistryNameAttribute))]
    public class GetFromRegistryNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            /*
            Rect textFieldRect = new Rect(position.x, position.y, position.width - 35, position.height);
            property.stringValue = EditorGUI.TextField(textFieldRect, label, property.stringValue);

            Rect dropArea = new Rect(position.x + position.width - 50, position.y, 50, position.height);
            //GUI.Box(dropArea, "{  }");
            */
            Rect textFieldRect = new Rect(position.x, position.y, position.width - 20, position.height);
            property.stringValue = EditorGUI.TextField(textFieldRect, label, property.stringValue);
            Rect tipArea = new Rect(position.x + position.width - 20, position.y, 20, position.height);
            UnityEngine.GUI.Box(tipArea, "**");

            //Rect dropArea = new Rect(position.x, position.y, position.width, position.height);
            //GUI.Box(dropArea, "");

            HandleDragAndDrop(textFieldRect, property);

            EditorGUI.EndProperty();
        }

        private void HandleDragAndDrop(Rect dropArea, SerializedProperty property)
        {
            Event evt = Event.current;
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        foreach (Object draggedObject in DragAndDrop.objectReferences)
                        {
                            string assetPath = AssetDatabase.GetAssetPath(draggedObject);
                            var registryName = GR.ADToRegName(assetPath);
                            if (!string.IsNullOrEmpty(registryName))
                            {
                                property.stringValue = registryName;
                            }
                        }
                    }
                    Event.current.Use();
                    break;
            }
        }
    }
}