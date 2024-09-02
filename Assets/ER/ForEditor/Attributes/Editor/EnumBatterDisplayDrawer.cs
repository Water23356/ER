
using System;
using UnityEditor;
using UnityEngine;

namespace ER.ForEditor
{
    [CustomPropertyDrawer(typeof(EnumBatterDisplayAttribute))]
    public class EnumBatterDisplayDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Enum)
            {
                EditorGUI.BeginProperty(position, label, property);
                property.enumValueIndex = EditorGUI.Popup(position, label, property.enumValueIndex, GetEnumNames(property));
                EditorGUI.EndProperty();
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use LargeEnum with Enum.");
            }
        }


        private GUIContent[] GetEnumNames(SerializedProperty property)
        {
            Enum enumValue = (Enum)Enum.ToObject(fieldInfo.FieldType, property.enumValueIndex);
            Array enumValues = Enum.GetValues(enumValue.GetType());
            GUIContent[] enumNames = new GUIContent[enumValues.Length];

            for (int i = 0; i < enumValues.Length; i++)
            {
                Enum value = (Enum)enumValues.GetValue(i);
                enumNames[i] = new GUIContent(value.GetEnumName());
            }

            return enumNames;
        }
    }
}