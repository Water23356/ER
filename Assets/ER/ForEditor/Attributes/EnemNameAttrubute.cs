using System;
#if UNITY_EDITOR

#endif
using System.Reflection;

namespace ER.ForEditor
{

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class EnumNameAttribute : Attribute
    {
        public string DisplayName { get; }

        public EnumNameAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }


    public static class EnumExtensions
    {
        public static string GetEnumName(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field == null) return value.ToString();
            EnumNameAttribute attribute =
                (EnumNameAttribute)Attribute.GetCustomAttribute(field, typeof(EnumNameAttribute));

            return attribute?.DisplayName ?? value.ToString();
        }
    }
}

#if UNITY_EDITOR
/*
[CustomPropertyDrawer(typeof(Enum), true)]
public class EnumNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.Enum)
        {
            //Enum enumValue = GetEnumValue(property);
            //string enumName = enumValue.GetEnumName();

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.Popup(position, label, property.enumValueIndex, GetEnumNames(property));
            EditorGUI.EndProperty();
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    //private Enum GetEnumValue(SerializedProperty property)
    //{
    //    Type enumType = fieldInfo.FieldType;
    //    if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(List<>))
    //    {
    //        enumType = enumType.GetGenericArguments()[0];
    //    }
    //    return (Enum)Enum.ToObject(enumType, property.enumValueIndex);
    //}

    private GUIContent[] GetEnumNames(SerializedProperty property)
    {
        Type enumType = fieldInfo.FieldType;
        if (enumType.IsGenericType && enumType.GetGenericTypeDefinition() == typeof(List<>))
        {
            enumType = enumType.GetGenericArguments()[0];
        }

        Array enumValues = Enum.GetValues(enumType);
        GUIContent[] enumNames = new GUIContent[enumValues.Length];

        for (int i = 0; i < enumValues.Length; i++)
        {
            Enum value = (Enum)enumValues.GetValue(i);
            enumNames[i] = new GUIContent(value.GetEnumName());
        }

        return enumNames;
    }
}*/
#endif