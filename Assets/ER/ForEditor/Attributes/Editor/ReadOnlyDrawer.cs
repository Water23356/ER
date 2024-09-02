using UnityEditor;
using UnityEngine;

namespace ER.ForEditor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            UnityEngine.GUI.enabled = false; // 禁用编辑
            EditorGUI.PropertyField(position, property, label);
            UnityEngine.GUI.enabled = true; // 恢复编辑状态
        }
    }
}