using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
namespace ER.TurnBox
{
    [CustomPropertyDrawer(typeof(FiniteValueInt))]
    public class FiniteValueDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);//开始绘制该属性

            // 绘制标签
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            Rect rect1 = new Rect(position.x, position.y, position.width, position.height / 3);

            Rect rect2 = new Rect(position.x, position.y + position.height / 3 + 2, position.width, position.height / 3);
            Rect rect2_0 = new Rect(rect2.x - 40, rect2.y, 40, rect2.height);
            Rect rect2_1 = new Rect(rect2.x - 60, rect2.y, 20, rect2.height);

            Rect rect3 = new Rect(position.x, position.y + position.height * 2 / 3 + 4, position.width, position.height / 3);
            Rect rect3_0 = new Rect(rect3.x - 40, rect3.y, 40, rect3.height);
            Rect rect3_1 = new Rect(rect3.x - 60, rect3.y, 20, rect3.height);

            var value = property.FindPropertyRelative("m_current");
            var max = property.FindPropertyRelative("m_max");
            var max_limit = property.FindPropertyRelative("m_max_limit");
            var min = property.FindPropertyRelative("m_min");
            var min_limit = property.FindPropertyRelative("m_min_limit");

            value.intValue = EditorGUI.IntField(rect1, value.intValue);

            EditorGUI.LabelField(rect2_0, "上限");
            max_limit.boolValue = EditorGUI.Toggle(rect2_1, max_limit.boolValue);
            //if (max_limit.boolValue)
            max.intValue = EditorGUI.IntField(rect2, max.intValue);

            EditorGUI.LabelField(rect3_0, "下限");
            min_limit.boolValue = EditorGUI.Toggle(rect3_1, min_limit.boolValue);
            //if (min_limit.boolValue)
            min.intValue = EditorGUI.IntField(rect3, min.intValue);

            EditorGUI.EndProperty();//结束绘制
        }

        //用于调整绘制区域的高度
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 两行高度
            // EditorGUIUtility.singleLineHeight 为标准的单行高度
            return EditorGUIUtility.singleLineHeight * 3 + 6; // 3行高度加上间隔
        }
    }

    [CustomPropertyDrawer(typeof(FiniteValueFloat))]
    public class FiniteValueFloatDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);//开始绘制该属性

            // 绘制标签
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            Rect rect1 = new Rect(position.x, position.y, position.width, position.height / 3);

            Rect rect2 = new Rect(position.x, position.y + position.height / 3 + 2, position.width, position.height / 3);
            Rect rect2_0 = new Rect(rect2.x - 40, rect2.y, 40, rect2.height);
            Rect rect2_1 = new Rect(rect2.x - 60, rect2.y, 20, rect2.height);

            Rect rect3 = new Rect(position.x, position.y + position.height * 2 / 3 + 4, position.width, position.height / 3);
            Rect rect3_0 = new Rect(rect3.x - 40, rect3.y, 40, rect3.height);
            Rect rect3_1 = new Rect(rect3.x - 60, rect3.y, 20, rect3.height);

            var value = property.FindPropertyRelative("m_current");
            var max = property.FindPropertyRelative("m_max");
            var max_limit = property.FindPropertyRelative("m_max_limit");
            var min = property.FindPropertyRelative("m_min");
            var min_limit = property.FindPropertyRelative("m_min_limit");

            value.floatValue = EditorGUI.FloatField(rect1, value.floatValue);

            EditorGUI.LabelField(rect2_0, "上限");
            max_limit.boolValue = EditorGUI.Toggle(rect2_1, max_limit.boolValue);
            //if (max_limit.boolValue)
            max.floatValue = EditorGUI.FloatField(rect2, max.floatValue);

            EditorGUI.LabelField(rect3_0, "下限");
            min_limit.boolValue = EditorGUI.Toggle(rect3_1, min_limit.boolValue);
            //if (min_limit.boolValue)
            min.floatValue = EditorGUI.FloatField(rect3, min.floatValue);

            EditorGUI.EndProperty();//结束绘制
        }

        //用于调整绘制区域的高度
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        { 
            // EditorGUIUtility.singleLineHeight 为标准的单行高度
            return EditorGUIUtility.singleLineHeight * 3 + 12; // 3行高度加上间隔
        }
    }
}
#endif