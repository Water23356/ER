using System;
using UnityEditor;
using UnityEngine;

namespace ER.ForEditor
{
    [CustomPropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownDrawer : PropertyDrawer
    {
        private const float inputBoxHeight = 20f;
        private const float dropdownHeight = 20f;
        private const float spacing = 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            DropdownAttribute customStringAttribute = (DropdownAttribute)attribute;
            string[] options = customStringAttribute.GetOptions();
            // 计算下拉框的位置和大小
            Rect dropdownRect = new Rect(position.x, position.y, position.width, dropdownHeight);

            // 获取当前选中的下标，如果没有匹配的，则为 -1
            int selectedIndex = System.Array.IndexOf(options, property.stringValue);

            // 显示下拉框
            selectedIndex = EditorGUI.Popup(dropdownRect, label.text, selectedIndex, options);

            // 如果用户选择了下拉框中的一个选项，则更新字符串属性
            if (selectedIndex >= 0)
            {
                property.stringValue = options[selectedIndex];
            }

            // 计算输入框的位置和大小
            Rect inputBoxRect = new Rect(
                position.x,
                position.y + dropdownHeight + spacing,
                position.width,
                inputBoxHeight
            );

            // 创建一个 GUIStyle 来设置标签颜色
            GUIStyle fadedLabelStyle = new GUIStyle(EditorStyles.label);
            fadedLabelStyle.normal.textColor = Color.gray; // 设置为灰色
            fadedLabelStyle.fontStyle = FontStyle.Bold;

            // 显示输入框标签
            EditorGUI.LabelField(inputBoxRect, "*自定义值*", fadedLabelStyle);

            // 创建输入框，允许用户输入自定义值
            Rect inputFieldRect = new Rect(
                inputBoxRect.x + EditorGUIUtility.labelWidth,
                inputBoxRect.y,
                inputBoxRect.width - EditorGUIUtility.labelWidth,
                inputBoxRect.height
            );
            string customValue = EditorGUI.TextField(
                inputFieldRect,
                GUIContent.none,
                property.stringValue
            );

            // 如果自定义值不等于下拉框选中的值，则更新字符串属性
            if (customValue != property.stringValue)
            {
                property.stringValue = customValue;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 返回总高度，包括下拉框和输入框的高度以及间距
            return dropdownHeight + inputBoxHeight + spacing;
        }
    }
}