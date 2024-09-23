using UnityEditor;
using UnityEngine;

namespace Dev
{
    [CustomPropertyDrawer(typeof(RegistryName))]
    public class RegistryNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // 使用默认的标签绘制
            EditorGUI.PrefixLabel(position, label);

            // 缩进
            Rect contentPosition = EditorGUI.PrefixLabel(position, label);

            // 增加缩进
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // 获取属性
            SerializedProperty headProperty = property.FindPropertyRelative("head");
            SerializedProperty moduleProperty = property.FindPropertyRelative("module");
            SerializedProperty pathProperty = property.FindPropertyRelative("path");

            // 绘制属性
            var rectLabel = new Rect(contentPosition.x, contentPosition.y, 50, EditorGUIUtility.singleLineHeight);
            var contentLabel = new Rect(contentPosition.x+50, contentPosition.y, contentPosition.width-50, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(rectLabel, "头部");
            EditorGUI.PropertyField(contentLabel, headProperty,new GUIContent());

            rectLabel.y += EditorGUIUtility.singleLineHeight;
            contentLabel.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(rectLabel, "模组");
            EditorGUI.PropertyField(contentLabel, moduleProperty, new GUIContent());


            rectLabel.y += EditorGUIUtility.singleLineHeight;
            contentLabel.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(rectLabel, "路径");
            EditorGUI.PropertyField(contentLabel, pathProperty, new GUIContent());

            // 恢复缩进
            EditorGUI.indentLevel = indent;

            // 自动修正空字符串
            if (string.IsNullOrEmpty(headProperty.stringValue))
            {
                headProperty.stringValue = "unknown";
            }

            if (string.IsNullOrEmpty(moduleProperty.stringValue))
            {
                moduleProperty.stringValue = "origin";
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 计算高度
            return EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 3;
        }
    }
}