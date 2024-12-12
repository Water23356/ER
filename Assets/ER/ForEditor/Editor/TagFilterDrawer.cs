using UnityEditor;
using UnityEngine;

namespace ER.ForEditor
{
    [CustomPropertyDrawer(typeof(TagFilter))]
    public class TagFilterDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // 找到子属性
            SerializedProperty modeProperty = property.FindPropertyRelative("mode");
            SerializedProperty tagsProperty = property.FindPropertyRelative("tags");

            // 行高
            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            float buttonWidth = 20f;

            // 计算Rect
            Rect modeRect = new Rect(position.x, position.y, position.width, singleLineHeight);
            Rect listLabelRect = new Rect(position.x, position.y + singleLineHeight, position.width, singleLineHeight);

            // 绘制过滤模式的枚举下拉框
            EditorGUI.PropertyField(modeRect, modeProperty);

            TagFilter.FilterMode currentMode = (TagFilter.FilterMode)modeProperty.enumValueIndex;

            if (currentMode == TagFilter.FilterMode.Whitelist || currentMode == TagFilter.FilterMode.Blacklist)
            {
                // 绘制标签列表标签
                EditorGUI.LabelField(listLabelRect, "标签列表:");

                // 获取所有定义的 Unity 标签
                string[] tagOptions = UnityEditorInternal.InternalEditorUtility.tags;

                // 初始化当前绘制位置
                float yPos = position.y + 2 * singleLineHeight;

                // 绘制每个标签
                for (int i = 0; i < tagsProperty.arraySize; i++)
                {
                    SerializedProperty element = tagsProperty.GetArrayElementAtIndex(i);

                    // 确定枚举下拉位置
                    Rect elementRect = new Rect(position.x + position.width / 2, yPos, position.width / 2 - buttonWidth - 8, singleLineHeight);
                    Rect removeButtonRect = new Rect(position.x + position.width - buttonWidth, yPos, buttonWidth, singleLineHeight);

                    // 显示标签下拉菜单
                    int index = System.Array.IndexOf(tagOptions, element.stringValue);
                    index = EditorGUI.Popup(elementRect, index, tagOptions);

                    if (index >= 0 && index < tagOptions.Length)
                    {
                        element.stringValue = tagOptions[index];
                    }

                    // 删除按钮
                    if (UnityEngine.GUI.Button(removeButtonRect, "-"))
                    {
                        tagsProperty.DeleteArrayElementAtIndex(i);
                    }

                    yPos += singleLineHeight + 2;
                }

                // 添加按钮位置
                Rect addButtonRect = new Rect(position.x + position.width / 2, yPos, position.width / 2, singleLineHeight);

                // 添加新标签按钮
                if (UnityEngine.GUI.Button(addButtonRect, "+"))
                {
                    int newIndex = tagsProperty.arraySize;
                    tagsProperty.InsertArrayElementAtIndex(newIndex);
                    tagsProperty.GetArrayElementAtIndex(newIndex).stringValue = tagOptions.Length > 0 ? tagOptions[0] : "";
                }
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 默认高度为模式字段的高度
            float height = EditorGUIUtility.singleLineHeight;

            // 找到子属性
            SerializedProperty modeProperty = property.FindPropertyRelative("mode");
            SerializedProperty tagsProperty = property.FindPropertyRelative("tags");

            // 检查当前模式
            TagFilter.FilterMode currentMode = (TagFilter.FilterMode)modeProperty.enumValueIndex;

            if (currentMode == TagFilter.FilterMode.Whitelist || currentMode == TagFilter.FilterMode.Blacklist)
            {
                // 如果是白名单或黑名单，增加标签列表高度
                height += (EditorGUIUtility.singleLineHeight + 2) * (1 + tagsProperty.arraySize + 1); // 1 for the label, each tag, and 1 for the add button
            }

            return height;
        }
    }
}