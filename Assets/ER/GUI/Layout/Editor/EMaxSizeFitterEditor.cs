using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace ER.GUI
{
    [CustomEditor(typeof(EMaxSizeFitter))]
    public class EMaxSizeFitterEditor : Editor
    {
        private SerializedObject so;
        private SerializedProperty maxWidth;
        private SerializedProperty maxHeight;
        private RectTransform rectTransform;

        private bool applyMaxWidth;
        private bool applyMaxHeight;

        private void OnEnable()
        {
            so = new SerializedObject(target);
            maxWidth = so.FindProperty("m_maxWidth");
            maxHeight = so.FindProperty("m_maxHeight");
            rectTransform = target.GetComponent<RectTransform>();

            applyMaxWidth = maxWidth.floatValue > 0f;
            applyMaxHeight = maxHeight.floatValue > 0f;

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            so.Update();//更新显示

            EditorGUILayout.BeginHorizontal();
            bool tmp;
            tmp = EditorGUILayout.Toggle("最大宽度", applyMaxWidth);
            if (applyMaxWidth != tmp)
            {
                if (tmp)
                {
                    maxWidth.floatValue = rectTransform.rect.width;
                }
                else
                {
                    maxWidth.floatValue = -1;
                }
            }
            applyMaxWidth = tmp;
            if (applyMaxWidth)
            {
                EditorGUILayout.PropertyField(maxWidth);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            tmp = EditorGUILayout.Toggle("最大高度", applyMaxHeight);
            if (applyMaxHeight != tmp)
            {
                if (tmp)
                {
                    maxHeight.floatValue = rectTransform.rect.width;
                }
                else
                {
                    maxHeight.floatValue = -1;
                }
            }
            applyMaxHeight = tmp;
            if (applyMaxHeight)
            {
                EditorGUILayout.PropertyField(maxHeight);
            }
            else
            {
                maxHeight.floatValue = -1;
            }
            EditorGUILayout.EndHorizontal();

            so.ApplyModifiedProperties();//保存修改
        }
    }
}