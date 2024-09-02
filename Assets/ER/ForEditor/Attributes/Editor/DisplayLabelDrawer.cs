using System;
using UnityEditor;
using UnityEngine;

namespace ER.ForEditor
{
    [CustomPropertyDrawer(typeof(DisplayLabelAttribute))]
    public class DisplayLabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DisplayLabelAttribute displayLabel = (DisplayLabelAttribute)attribute;
            label.text = displayLabel.label;

            // 保持原有的显示和编辑功能
            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 返回原有属性的高度
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}